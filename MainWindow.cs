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
			
		}

		private void BurnFilesGrid_MultiSelectChanged(object sender, EventArgs e)
		{
			
		}

		#endregion
	}
}
