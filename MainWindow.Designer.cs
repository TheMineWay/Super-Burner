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
			BurnableUnitSelectorLabel = new Label();
			OpticalDrivesComboBox = new ComboBox();
			ReloadDrivesListBtn = new Button();
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
			MainMenuStrip.Size = new Size(779, 33);
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
			BurnDirFSWatcher.Created += BurnDirFSWatcher_Changed;
			BurnDirFSWatcher.Deleted += BurnDirFSWatcher_Changed;
			BurnDirFSWatcher.Renamed += BurnDirFSWatcher_Renamed;
			// 
			// BurnFilesGrid
			// 
			BurnFilesGrid.AllowUserToAddRows = false;
			BurnFilesGrid.AllowUserToDeleteRows = false;
			BurnFilesGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			BurnFilesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			BurnFilesGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			BurnFilesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			BurnFilesGrid.Location = new Point(11, 37);
			BurnFilesGrid.Name = "BurnFilesGrid";
			BurnFilesGrid.ReadOnly = true;
			BurnFilesGrid.RowHeadersWidth = 62;
			BurnFilesGrid.Size = new Size(520, 457);
			BurnFilesGrid.TabIndex = 1;
			BurnFilesGrid.SelectionChanged += BurnFilesGrid_MultiSelectChanged;
			// 
			// DeleteSelectedFilesBtn
			// 
			DeleteSelectedFilesBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			DeleteSelectedFilesBtn.Enabled = false;
			DeleteSelectedFilesBtn.Location = new Point(11, 498);
			DeleteSelectedFilesBtn.Name = "DeleteSelectedFilesBtn";
			DeleteSelectedFilesBtn.Size = new Size(217, 33);
			DeleteSelectedFilesBtn.TabIndex = 2;
			DeleteSelectedFilesBtn.Text = "Delete selected files";
			DeleteSelectedFilesBtn.UseVisualStyleBackColor = true;
			DeleteSelectedFilesBtn.Click += DeleteSelectedFilesBtn_Click;
			// 
			// BurnableUnitSelectorLabel
			// 
			BurnableUnitSelectorLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			BurnableUnitSelectorLabel.AutoSize = true;
			BurnableUnitSelectorLabel.Location = new Point(539, 37);
			BurnableUnitSelectorLabel.Name = "BurnableUnitSelectorLabel";
			BurnableUnitSelectorLabel.Size = new Size(116, 25);
			BurnableUnitSelectorLabel.TabIndex = 4;
			BurnableUnitSelectorLabel.Text = "Burnable unit";
			BurnableUnitSelectorLabel.Click += BurnableUnitSelectorLabel_Click;
			// 
			// OpticalDrivesComboBox
			// 
			OpticalDrivesComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			OpticalDrivesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			OpticalDrivesComboBox.Enabled = false;
			OpticalDrivesComboBox.FormattingEnabled = true;
			OpticalDrivesComboBox.Location = new Point(539, 70);
			OpticalDrivesComboBox.Name = "OpticalDrivesComboBox";
			OpticalDrivesComboBox.Size = new Size(228, 33);
			OpticalDrivesComboBox.TabIndex = 5;
			// 
			// ReloadDrivesListBtn
			// 
			ReloadDrivesListBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			ReloadDrivesListBtn.Font = new Font("Segoe UI", 8F);
			ReloadDrivesListBtn.Location = new Point(677, 37);
			ReloadDrivesListBtn.Name = "ReloadDrivesListBtn";
			ReloadDrivesListBtn.Size = new Size(88, 27);
			ReloadDrivesListBtn.TabIndex = 6;
			ReloadDrivesListBtn.Text = "Reload";
			ReloadDrivesListBtn.UseVisualStyleBackColor = true;
			ReloadDrivesListBtn.Click += ReloadDrivesListBtn_Click;
			// 
			// MainWindow
			// 
			AccessibleName = "Super Burner";
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(779, 543);
			Controls.Add(ReloadDrivesListBtn);
			Controls.Add(BurnableUnitSelectorLabel);
			Controls.Add(DeleteSelectedFilesBtn);
			Controls.Add(OpticalDrivesComboBox);
			Controls.Add(BurnFilesGrid);
			Controls.Add(MainMenuStrip);
			MaximizeBox = false;
			MaximumSize = new Size(1001, 1199);
			MinimumSize = new Size(601, 399);
			Name = "MainWindow";
			Text = "Super Burner";
			Load += MainWindow_Load;
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
		private Label BurnableUnitSelectorLabel;
		private ComboBox OpticalDrivesComboBox;
		private Button ReloadDrivesListBtn;
	}
}