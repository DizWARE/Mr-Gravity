namespace GravityLevelEditor
{
    partial class TempGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TempGUI));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.mi_file = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_new = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_open = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_save = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_line2 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_play = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_quit = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_undo = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_redo = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_line = new System.Windows.Forms.ToolStripSeparator();
            this.mi_cut = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_paste = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_view = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_zoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_zoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.pnl_Level = new System.Windows.Forms.Panel();
            this.sc_Properties = new System.Windows.Forms.SplitContainer();
            this.sc_PropertiesHorizontal = new System.Windows.Forms.SplitContainer();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.lbl_levelName = new System.Windows.Forms.Label();
            this.tb_cols = new System.Windows.Forms.TextBox();
            this.tb_rows = new System.Windows.Forms.TextBox();
            this.lbl_levelProperties = new System.Windows.Forms.Label();
            this.lbl_cols = new System.Windows.Forms.Label();
            this.lbl_rows = new System.Windows.Forms.Label();
            this.b_modifyLevel = new System.Windows.Forms.Button();
            this.menu.SuspendLayout();
            this.pnl_Level.SuspendLayout();
            this.sc_Properties.Panel2.SuspendLayout();
            this.sc_Properties.SuspendLayout();
            this.sc_PropertiesHorizontal.Panel2.SuspendLayout();
            this.sc_PropertiesHorizontal.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_file,
            this.mi_edit,
            this.mi_view});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1020, 24);
            this.menu.TabIndex = 1;
            this.menu.Text = "Menu";
            // 
            // mi_file
            // 
            this.mi_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_new,
            this.mi_open,
            this.mi_save,
            this.ts_line2,
            this.mi_play,
            this.mi_quit});
            this.mi_file.Name = "mi_file";
            this.mi_file.Size = new System.Drawing.Size(37, 20);
            this.mi_file.Text = "&File";
            // 
            // mi_new
            // 
            this.mi_new.Name = "mi_new";
            this.mi_new.Size = new System.Drawing.Size(103, 22);
            this.mi_new.Text = "&New";
            this.mi_new.Click += new System.EventHandler(this.New);
            // 
            // mi_open
            // 
            this.mi_open.Name = "mi_open";
            this.mi_open.Size = new System.Drawing.Size(103, 22);
            this.mi_open.Text = "&Open";
            this.mi_open.Click += new System.EventHandler(this.Open);
            // 
            // mi_save
            // 
            this.mi_save.Name = "mi_save";
            this.mi_save.Size = new System.Drawing.Size(103, 22);
            this.mi_save.Text = "&Save";
            this.mi_save.Click += new System.EventHandler(this.Save);
            // 
            // ts_line2
            // 
            this.ts_line2.Name = "ts_line2";
            this.ts_line2.Size = new System.Drawing.Size(100, 6);
            // 
            // mi_play
            // 
            this.mi_play.Name = "mi_play";
            this.mi_play.Size = new System.Drawing.Size(103, 22);
            this.mi_play.Text = "&Play!";
            this.mi_play.Click += new System.EventHandler(this.Play);
            // 
            // mi_quit
            // 
            this.mi_quit.Name = "mi_quit";
            this.mi_quit.Size = new System.Drawing.Size(103, 22);
            this.mi_quit.Text = "&Quit";
            this.mi_quit.Click += new System.EventHandler(this.Quit);
            // 
            // mi_edit
            // 
            this.mi_edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_undo,
            this.mi_redo,
            this.ts_line,
            this.mi_cut,
            this.mi_copy,
            this.mi_paste});
            this.mi_edit.Name = "mi_edit";
            this.mi_edit.Size = new System.Drawing.Size(39, 20);
            this.mi_edit.Text = "&Edit";
            // 
            // mi_undo
            // 
            this.mi_undo.Name = "mi_undo";
            this.mi_undo.Size = new System.Drawing.Size(103, 22);
            this.mi_undo.Text = "&Undo";
            this.mi_undo.Click += new System.EventHandler(this.Undo);
            // 
            // mi_redo
            // 
            this.mi_redo.Name = "mi_redo";
            this.mi_redo.Size = new System.Drawing.Size(103, 22);
            this.mi_redo.Text = "&Redo";
            this.mi_redo.Click += new System.EventHandler(this.Redo);
            // 
            // ts_line
            // 
            this.ts_line.Name = "ts_line";
            this.ts_line.Size = new System.Drawing.Size(100, 6);
            // 
            // mi_cut
            // 
            this.mi_cut.Name = "mi_cut";
            this.mi_cut.Size = new System.Drawing.Size(103, 22);
            this.mi_cut.Text = "&Cut";
            this.mi_cut.Click += new System.EventHandler(this.Cut);
            // 
            // mi_copy
            // 
            this.mi_copy.Name = "mi_copy";
            this.mi_copy.Size = new System.Drawing.Size(103, 22);
            this.mi_copy.Text = "C&opy";
            this.mi_copy.Click += new System.EventHandler(this.Copy);
            // 
            // mi_paste
            // 
            this.mi_paste.Name = "mi_paste";
            this.mi_paste.Size = new System.Drawing.Size(103, 22);
            this.mi_paste.Text = "&Paste";
            this.mi_paste.Click += new System.EventHandler(this.Paste);
            // 
            // mi_view
            // 
            this.mi_view.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_zoomIn,
            this.mi_zoomOut});
            this.mi_view.Name = "mi_view";
            this.mi_view.Size = new System.Drawing.Size(44, 20);
            this.mi_view.Text = "&View";
            // 
            // mi_zoomIn
            // 
            this.mi_zoomIn.Name = "mi_zoomIn";
            this.mi_zoomIn.Size = new System.Drawing.Size(129, 22);
            this.mi_zoomIn.Text = "Zoom &In";
            this.mi_zoomIn.Click += new System.EventHandler(this.ZoomIn);
            // 
            // mi_zoomOut
            // 
            this.mi_zoomOut.Name = "mi_zoomOut";
            this.mi_zoomOut.Size = new System.Drawing.Size(129, 22);
            this.mi_zoomOut.Text = "Zoom &Out";
            this.mi_zoomOut.Click += new System.EventHandler(this.ZoomOut);
            // 
            // pnl_Level
            // 
            this.pnl_Level.AutoScroll = true;
            this.pnl_Level.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnl_Level.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Level.Controls.Add(this.sc_Properties);
            this.pnl_Level.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Level.Location = new System.Drawing.Point(0, 24);
            this.pnl_Level.Name = "pnl_Level";
            this.pnl_Level.Size = new System.Drawing.Size(1020, 638);
            this.pnl_Level.TabIndex = 2;
            // 
            // sc_Properties
            // 
            this.sc_Properties.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sc_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_Properties.Location = new System.Drawing.Point(0, 0);
            this.sc_Properties.Name = "sc_Properties";
            // 
            // sc_Properties.Panel1
            // 
            this.sc_Properties.Panel1.AutoScroll = true;
            this.sc_Properties.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.GridPaint);
            this.sc_Properties.Panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.UpdatePaint);
            this.sc_Properties.Panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridSpaceClick);
            // 
            // sc_Properties.Panel2
            // 
            this.sc_Properties.Panel2.Controls.Add(this.sc_PropertiesHorizontal);
            this.sc_Properties.Size = new System.Drawing.Size(1018, 636);
            this.sc_Properties.SplitterDistance = 829;
            this.sc_Properties.TabIndex = 0;
            // 
            // sc_PropertiesHorizontal
            // 
            this.sc_PropertiesHorizontal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sc_PropertiesHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_PropertiesHorizontal.Location = new System.Drawing.Point(0, 0);
            this.sc_PropertiesHorizontal.Name = "sc_PropertiesHorizontal";
            this.sc_PropertiesHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sc_PropertiesHorizontal.Panel1
            // 
            this.sc_PropertiesHorizontal.Panel1.AutoScroll = true;
            this.sc_PropertiesHorizontal.Panel1.BackColor = System.Drawing.SystemColors.Control;
            // 
            // sc_PropertiesHorizontal.Panel2
            // 
            this.sc_PropertiesHorizontal.Panel2.AutoScroll = true;
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.tb_name);
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.lbl_levelName);
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.tb_cols);
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.tb_rows);
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.lbl_levelProperties);
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.lbl_cols);
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.lbl_rows);
            this.sc_PropertiesHorizontal.Panel2.Controls.Add(this.b_modifyLevel);
            this.sc_PropertiesHorizontal.Size = new System.Drawing.Size(181, 632);
            this.sc_PropertiesHorizontal.SplitterDistance = 298;
            this.sc_PropertiesHorizontal.TabIndex = 0;
            // 
            // tb_name
            // 
            this.tb_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tb_name.Location = new System.Drawing.Point(38, 57);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(113, 20);
            this.tb_name.TabIndex = 10;
            this.tb_name.Text = "New Level";
            // 
            // lbl_levelName
            // 
            this.lbl_levelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbl_levelName.AutoSize = true;
            this.lbl_levelName.Location = new System.Drawing.Point(35, 41);
            this.lbl_levelName.Name = "lbl_levelName";
            this.lbl_levelName.Size = new System.Drawing.Size(35, 13);
            this.lbl_levelName.TabIndex = 9;
            this.lbl_levelName.Text = "Name";
            // 
            // tb_cols
            // 
            this.tb_cols.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tb_cols.Location = new System.Drawing.Point(105, 107);
            this.tb_cols.MaxLength = 5;
            this.tb_cols.Name = "tb_cols";
            this.tb_cols.Size = new System.Drawing.Size(46, 20);
            this.tb_cols.TabIndex = 8;
            this.tb_cols.Text = "10";
            this.tb_cols.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tb_cols.TextChanged += new System.EventHandler(this.ValidateSizeTextbox);
            // 
            // tb_rows
            // 
            this.tb_rows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tb_rows.Location = new System.Drawing.Point(38, 107);
            this.tb_rows.MaxLength = 5;
            this.tb_rows.Name = "tb_rows";
            this.tb_rows.Size = new System.Drawing.Size(46, 20);
            this.tb_rows.TabIndex = 7;
            this.tb_rows.Text = "10";
            this.tb_rows.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tb_rows.TextChanged += new System.EventHandler(this.ValidateSizeTextbox);
            // 
            // lbl_levelProperties
            // 
            this.lbl_levelProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbl_levelProperties.AutoSize = true;
            this.lbl_levelProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_levelProperties.Location = new System.Drawing.Point(24, 9);
            this.lbl_levelProperties.Name = "lbl_levelProperties";
            this.lbl_levelProperties.Size = new System.Drawing.Size(138, 20);
            this.lbl_levelProperties.TabIndex = 6;
            this.lbl_levelProperties.Text = "Level Properties";
            // 
            // lbl_cols
            // 
            this.lbl_cols.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbl_cols.AutoSize = true;
            this.lbl_cols.Location = new System.Drawing.Point(102, 91);
            this.lbl_cols.Name = "lbl_cols";
            this.lbl_cols.Size = new System.Drawing.Size(47, 13);
            this.lbl_cols.TabIndex = 5;
            this.lbl_cols.Text = "Columns";
            // 
            // lbl_rows
            // 
            this.lbl_rows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lbl_rows.AutoSize = true;
            this.lbl_rows.Location = new System.Drawing.Point(35, 91);
            this.lbl_rows.Name = "lbl_rows";
            this.lbl_rows.Size = new System.Drawing.Size(34, 13);
            this.lbl_rows.TabIndex = 4;
            this.lbl_rows.Text = "Rows";
            // 
            // b_modifyLevel
            // 
            this.b_modifyLevel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.b_modifyLevel.Location = new System.Drawing.Point(55, 143);
            this.b_modifyLevel.Name = "b_modifyLevel";
            this.b_modifyLevel.Size = new System.Drawing.Size(75, 23);
            this.b_modifyLevel.TabIndex = 0;
            this.b_modifyLevel.Text = "Apply!";
            this.b_modifyLevel.UseVisualStyleBackColor = true;
            this.b_modifyLevel.Click += new System.EventHandler(this.ApplyChanges);
            // 
            // TempGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 662);
            this.Controls.Add(this.pnl_Level);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TempGUI";
            this.Text = "TempGUI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Quit);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.pnl_Level.ResumeLayout(false);
            this.sc_Properties.Panel2.ResumeLayout(false);
            this.sc_Properties.ResumeLayout(false);
            this.sc_PropertiesHorizontal.Panel2.ResumeLayout(false);
            this.sc_PropertiesHorizontal.Panel2.PerformLayout();
            this.sc_PropertiesHorizontal.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem mi_file;
        private System.Windows.Forms.ToolStripMenuItem mi_new;
        private System.Windows.Forms.ToolStripMenuItem mi_open;
        private System.Windows.Forms.ToolStripMenuItem mi_save;
        private System.Windows.Forms.ToolStripSeparator ts_line2;
        private System.Windows.Forms.ToolStripMenuItem mi_play;
        private System.Windows.Forms.ToolStripMenuItem mi_quit;
        private System.Windows.Forms.ToolStripMenuItem mi_edit;
        private System.Windows.Forms.ToolStripMenuItem mi_undo;
        private System.Windows.Forms.ToolStripMenuItem mi_redo;
        private System.Windows.Forms.ToolStripSeparator ts_line;
        private System.Windows.Forms.ToolStripMenuItem mi_cut;
        private System.Windows.Forms.ToolStripMenuItem mi_copy;
        private System.Windows.Forms.ToolStripMenuItem mi_paste;
        private System.Windows.Forms.ToolStripMenuItem mi_view;
        private System.Windows.Forms.ToolStripMenuItem mi_zoomIn;
        private System.Windows.Forms.ToolStripMenuItem mi_zoomOut;
        private System.Windows.Forms.Panel pnl_Level;
        private System.Windows.Forms.SplitContainer sc_Properties;
        private System.Windows.Forms.SplitContainer sc_PropertiesHorizontal;
        private System.Windows.Forms.Button b_modifyLevel;
        private System.Windows.Forms.Label lbl_cols;
        private System.Windows.Forms.Label lbl_rows;
        private System.Windows.Forms.Label lbl_levelProperties;
        private System.Windows.Forms.TextBox tb_rows;
        private System.Windows.Forms.TextBox tb_cols;
        private System.Windows.Forms.Label lbl_levelName;
        private System.Windows.Forms.TextBox tb_name;
    }
}