using System.Diagnostics;

namespace Super_Burner
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			BurnerUnitDetector.onUnitConnected += onBurnerUnitConnected;
			BurnerUnitDetector.onUnitDisconnected += onBurnerUnitDisconnected;
			InitializeComponent();
		}

		~MainWindow() {
			BurnerUnitDetector.onUnitConnected -= onBurnerUnitConnected;
			BurnerUnitDetector.onUnitDisconnected -= onBurnerUnitDisconnected;
		}

		private void cleanFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult response = MessageBox.Show("All program info except for burn files will be deleted. ¿Do you want to continue?", "Confirm operation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (response == DialogResult.Yes) {
				FileEssentials.ResetFiles();
				MessageBox.Show("Essential files have been cleared", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void openBurnDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string dir = Constants.BURN_DIR;
			Process.Start("explorer.exe", dir);
		}

		private void onBurnerUnitConnected(string? driveName) {
			MessageBox.Show($"Connected {driveName ?? "No name"}");
		}

		private void onBurnerUnitDisconnected(string? driveName) {
			MessageBox.Show($"Disconnected {driveName ?? "No name"}");
		}
	}
}
