namespace Super_Burner
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			MainMenuStrip = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			openBurnDirectoryToolStripMenuItem = new ToolStripMenuItem();
			cleanFilesToolStripMenuItem = new ToolStripMenuItem();
			BurnDirFSWatcher = new FileSystemWatcher();
			BurnFilesGrid = new DataGridView();
			DeleteSelectedFilesBtn = new Button();
			MainMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)BurnDirFSWatcher).BeginInit();
			((System.ComponentModel.ISupportInitialize)BurnFilesGrid).BeginInit();
			SuspendLayout();
			// 
			// MainMenuStrip
			// 
			MainMenuStrip.ImageScalingSize = new Size(24, 24);
			MainMenuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
			MainMenuStrip.Location = new Point(0, 0);
			MainMenuStrip.Name = "MainMenuStrip";
			MainMenuStrip.Size = new Size(1178, 33);
			MainMenuStrip.TabIndex = 0;
			MainMenuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openBurnDirectoryToolStripMenuItem, cleanFilesToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(54, 29);
			fileToolStripMenuItem.Text = "File";
			// 
			// openBurnDirectoryToolStripMenuItem
			// 
			openBurnDirectoryToolStripMenuItem.Name = "openBurnDirectoryToolStripMenuItem";
			openBurnDirectoryToolStripMenuItem.Size = new Size(275, 34);
			openBurnDirectoryToolStripMenuItem.Text = "Open burn directory";
			openBurnDirectoryToolStripMenuItem.Click += openBurnDirectoryToolStripMenuItem_Click;
			// 
			// cleanFilesToolStripMenuItem
			// 
			cleanFilesToolStripMenuItem.Name = "cleanFilesToolStripMenuItem";
			cleanFilesToolStripMenuItem.Size = new Size(275, 34);
			cleanFilesToolStripMenuItem.Text = "Clean files";
			cleanFilesToolStripMenuItem.Click += cleanFilesToolStripMenuItem_Click;
			// 
			// BurnDirFSWatcher
			// 
			BurnDirFSWatcher.EnableRaisingEvents = true;
			BurnDirFSWatcher.SynchronizingObject = this;
			BurnDirFSWatcher.Changed += BurnDirFSWatcher_Changed;
			// 
			// BurnFilesGrid
			// 
			BurnFilesGrid.AllowUserToAddRows = false;
			BurnFilesGrid.AllowUserToDeleteRows = false;
			BurnFilesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			BurnFilesGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			BurnFilesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			BurnFilesGrid.Location = new Point(12, 36);
			BurnFilesGrid.Name = "BurnFilesGrid";
			BurnFilesGrid.ReadOnly = true;
			BurnFilesGrid.RowHeadersWidth = 62;
			BurnFilesGrid.Size = new Size(888, 556);
			BurnFilesGrid.TabIndex = 1;
			BurnFilesGrid.SelectionChanged += BurnFilesGrid_MultiSelectChanged;
			// 
			// DeleteSelectedFilesBtn
			// 
			DeleteSelectedFilesBtn.Enabled = false;
			DeleteSelectedFilesBtn.Location = new Point(12, 598);
			DeleteSelectedFilesBtn.Name = "DeleteSelectedFilesBtn";
			DeleteSelectedFilesBtn.Size = new Size(217, 34);
			DeleteSelectedFilesBtn.TabIndex = 2;
			DeleteSelectedFilesBtn.Text = "Delete selected files";
			DeleteSelectedFilesBtn.UseVisualStyleBackColor = true;
			DeleteSelectedFilesBtn.Click += DeleteSelectedFilesBtn_Click;
			// 
			// MainWindow
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1178, 644);
			Controls.Add(DeleteSelectedFilesBtn);
			Controls.Add(BurnFilesGrid);
			Controls.Add(MainMenuStrip);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "MainWindow";
			Text = "Super Burner";
			MainMenuStrip.ResumeLayout(false);
			MainMenuStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)BurnDirFSWatcher).EndInit();
			((System.ComponentModel.ISupportInitialize)BurnFilesGrid).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private MenuStrip MainMenuStrip;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem cleanFilesToolStripMenuItem;
		private ToolStripMenuItem openBurnDirectoryToolStripMenuItem;
		private FileSystemWatcher BurnDirFSWatcher;
		private DataGridView BurnFilesGrid;
		private Button DeleteSelectedFilesBtn;
	}
}