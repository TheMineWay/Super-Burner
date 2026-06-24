namespace Super_Burner
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void cleanFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileEssentials.ResetFiles();
			MessageBox.Show("Essential files have been cleared", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
