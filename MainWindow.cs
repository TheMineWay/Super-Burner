using System.Diagnostics;

namespace Super_Burner
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
			BurnDirFSWatcher.Path = Constants.BURN_DIR;

			// Setup table
			BurnFilesGrid.Columns.Add("name", "Name");
			BurnFilesGrid.Columns.Add("size", "Size");
			BurnFilesGrid.Columns.Add("extension", "Extension");

			// Initializate data
			UpdateBurnFilesList();
		}

		private void cleanFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult response = MessageBox.Show("All program info except for burn files will be deleted. ¿Do you want to continue?", "Confirm operation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (response == DialogResult.Yes)
			{
				FileEssentials.ResetFiles();
				MessageBox.Show("Essential files have been cleared", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void openBurnDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string dir = Constants.BURN_DIR;
			Process.Start("explorer.exe", dir);
		}

		#region Files

		private void BurnDirFSWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			UpdateBurnFilesList();
		}

		public void UpdateBurnFilesList()
		{
			var burnableFiles = BurnFilesUtils.ListBurnableFiles();
			BurnFilesGrid.Rows.Clear();

			foreach (var file in burnableFiles)
			{
				BurnFilesGrid.Rows.Add(file.name, $"{(file.size < 1 ? "<" : "")}{Math.Ceiling(file.size)}MB", file.extension);
			}
		}

		private void DeleteSelectedFilesBtn_Click(object sender, EventArgs e)
		{
			// Gather selected rows (support both full-row and cell selection modes)
			var selectedRowIndexes = BurnFilesGrid.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Index).ToList();
			if (selectedRowIndexes.Count == 0)
			{
				selectedRowIndexes = BurnFilesGrid.SelectedCells.Cast<DataGridViewCell>().Select(c => c.RowIndex).Distinct().ToList();
			}

			if (selectedRowIndexes.Count == 0)
			{
				MessageBox.Show("No files selected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var confirm = MessageBox.Show($"Delete {selectedRowIndexes.Count} selected file(s)? This action cannot be undone.", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (confirm != DialogResult.Yes)
				return;

			int deleted = 0;
			int failed = 0;
			foreach (int rowIndex in selectedRowIndexes.OrderByDescending(i => i))
			{
				if (rowIndex < 0 || rowIndex >= BurnFilesGrid.Rows.Count)
					continue;

				var row = BurnFilesGrid.Rows[rowIndex];
				string fileName = row.Cells[0].Value?.ToString() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(fileName))
					continue;

				string path = Path.Combine(Constants.BURN_DIR, fileName);
				try
				{
					if (File.Exists(path))
					{
						File.Delete(path);
						deleted++;
					}
				}
				catch (Exception ex)
				{
					failed++;
					Debug.WriteLine($"Failed deleting {path}: {ex}");
				}
			}

			UpdateBurnFilesList();
			MessageBox.Show($"Deleted: {deleted}. Failed: {failed}.", "Delete finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void BurnFilesGrid_MultiSelectChanged(object sender, EventArgs e)
		{
			bool hasSelection = BurnFilesGrid.SelectedRows.Count > 0 || BurnFilesGrid.SelectedCells.Count > 0;
			DeleteSelectedFilesBtn.Enabled = hasSelection;
		}

		private void BurnDirFSWatcher_Renamed(object sender, RenamedEventArgs e)
		{
			UpdateBurnFilesList();
		}

		#endregion

		#region UI

		private void BurnableUnitSelectorLabel_Click(object sender, EventArgs e)
		{
			BurnUnitSelector.Focus();
		}

		#endregion
	}
}
