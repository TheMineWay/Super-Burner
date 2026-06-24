using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace Super_Burner
{
	public partial class MainWindow : Form
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
			out ulong lpFreeBytesAvailable,
			out ulong lpTotalNumberOfBytes,
			out ulong lpTotalNumberOfFreeBytes);
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
			UpdateOpticalDrivesList();
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

		public void UpdateOpticalDrivesList()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action(UpdateOpticalDrivesList));
				return;
			}

			try
			{
				var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.CDRom);
				OpticalDrivesComboBox.Items.Clear();
				foreach (var d in drives)
				{
					string name = d.Name.TrimEnd('\\');
					string display = name;
					string diag = "";
					try
					{
						bool isReady = false;
						long sizeBytes = 0;
						try { isReady = d.IsReady; } catch { isReady = false; }
						if (isReady)
						{
							try { sizeBytes = d.TotalSize; } catch { sizeBytes = 0; }
							string sizeStr = "";
							if (sizeBytes > 0)
							{
								double mb = sizeBytes / (1024.0 * 1024.0);
								if (mb >= 1024)
									sizeStr = $", {Math.Round(mb / 1024.0, 2)} GB";
								else
									sizeStr = $", {Math.Ceiling(mb)} MB";
							}
							if (!string.IsNullOrEmpty(d.VolumeLabel))
								display = $"{name} ({d.VolumeLabel}{sizeStr})";
							else
								display = $"{name} ({sizeStr.TrimStart(',', ' ')})";
						}
						// If the drive is not ready or no size available, try WMI fallback to detect media/size
						if (!isReady || sizeBytes == 0)
						{
							try
							{
								string deviceId = name; // e.g. "D:"
														// Query logical disk for size/volume
								using (var searcher = new ManagementObjectSearcher($"SELECT Size, VolumeName FROM Win32_LogicalDisk WHERE DeviceID = '{deviceId}'"))
								{
									foreach (ManagementObject mo in searcher.Get())
									{
										var sizeObj = mo["Size"];
										var volObj = mo["VolumeName"];
										long wmiSize = 0;
										if (sizeObj != null && long.TryParse(sizeObj.ToString(), out wmiSize))
										{
											double mb = wmiSize / (1024.0 * 1024.0);
											string sizeStr = mb >= 1024 ? $", {Math.Round(mb / 1024.0, 2)} GB" : $", {Math.Ceiling(mb)} MB";
											string vol = volObj != null ? volObj.ToString() : string.Empty;
											if (!string.IsNullOrEmpty(vol)) display = $"{name} ({vol}{sizeStr})";
											else display = $"{name} ({sizeStr.TrimStart(',', ' ')})";
											break;
										}
									}
								}
								// If still no size, try GetDiskFreeSpaceEx (may work even if DriveInfo.IsReady is false)
								if (display == name)
								{
									try
									{
										string path = name + "\\"; // e.g. "D:\\"
										if (GetDiskFreeSpaceEx(path, out ulong freeAvail, out ulong totalBytes, out ulong totalFree))
										{
											if (totalBytes > 0)
											{
												double mb = totalBytes / (1024.0 * 1024.0);
												string sizeStr = mb >= 1024 ? $", {Math.Round(mb / 1024.0, 2)} GB" : $", {Math.Ceiling(mb)} MB";
												display = $"{name} (Media present{sizeStr})";
											}
										}
									}
									catch { }

									// If still unchanged, check CDROM drive for MediaLoaded
									if (display == name)
									{
										using (var searcher2 = new ManagementObjectSearcher($"SELECT Drive, MediaLoaded FROM Win32_CDROMDrive WHERE Drive = '{deviceId}'"))
										{
											foreach (ManagementObject mo2 in searcher2.Get())
											{
												var mediaLoadedObj = mo2["MediaLoaded"];
												bool mediaLoaded = false;
												if (mediaLoadedObj != null && bool.TryParse(mediaLoadedObj.ToString(), out mediaLoaded) && mediaLoaded)
												{
													display = $"{name} (Media present)";
													break;
												}
											}
										}
									}
								}
							}
							catch { }
						}
					}
					catch (Exception ex)
					{
						// Keep display but show minimal diagnostic
						diag = $"[Err:{ex.GetType().Name}]";
					}
					OpticalDrivesComboBox.Items.Add($"{display} {diag}");
				}
				bool has = OpticalDrivesComboBox.Items.Count > 0;
				OpticalDrivesComboBox.Enabled = has;
				if (has)
				{
					if (OpticalDrivesComboBox.SelectedIndex < 0)
						OpticalDrivesComboBox.SelectedIndex = 0;
				}
				else
				{
					OpticalDrivesComboBox.SelectedIndex = -1;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Unit error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_DEVICECHANGE = 0x0219;
			const int DBT_DEVICEARRIVAL = 0x8000;
			const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

			if (m.Msg == WM_DEVICECHANGE)
			{
				int wParam = m.WParam.ToInt32();
				if (wParam == DBT_DEVICEARRIVAL || wParam == DBT_DEVICEREMOVECOMPLETE)
				{
					UpdateOpticalDrivesList();
				}
			}

			base.WndProc(ref m);
		}

		#endregion

		#region UI

		private void BurnableUnitSelectorLabel_Click(object sender, EventArgs e)
		{
			OpticalDrivesComboBox.Focus();
		}

		#endregion

		private void MainWindow_Load(object sender, EventArgs e)
		{

		}

		private void ReloadDrivesListBtn_Click(object sender, EventArgs e)
		{
			UpdateOpticalDrivesList();
		}
	}
}
