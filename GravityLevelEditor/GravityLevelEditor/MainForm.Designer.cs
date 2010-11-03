namespace GravityLevelEditor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.mi_file = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_new = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_open = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.mi_save = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_saveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.mi_play = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_quit = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_undo = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_redo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_cut = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_paste = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_view = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_zoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_zoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.b_select = new System.Windows.Forms.ToolStripButton();
            this.b_multiselect = new System.Windows.Forms.ToolStripButton();
            this.b_addentity = new System.Windows.Forms.ToolStripButton();
            this.b_removeentity = new System.Windows.Forms.ToolStripButton();
            this.b_paint = new System.Windows.Forms.ToolStripButton();
            this.b_depaint = new System.Windows.Forms.ToolStripButton();
            this.menu.SuspendLayout();
            this.toolStripContainer1.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Dock = System.Windows.Forms.DockStyle.None;
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_file,
            this.mi_edit,
            this.mi_view});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1184, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "Menu";
            this.menu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menu_ItemClicked);
            // 
            // mi_file
            // 
            this.mi_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_new,
            this.mi_open,
            this.closeToolStripMenuItem,
            this.mi_save,
            this.mi_saveAs,
            this.playToolStripMenuItem,
            this.mi_play,
            this.mi_quit});
            this.mi_file.Name = "mi_file";
            this.mi_file.Size = new System.Drawing.Size(37, 20);
            this.mi_file.Text = "&File";
            // 
            // mi_new
            // 
            this.mi_new.Name = "mi_new";
            this.mi_new.Size = new System.Drawing.Size(114, 22);
            this.mi_new.Text = "&New";
            // 
            // mi_open
            // 
            this.mi_open.Name = "mi_open";
            this.mi_open.Size = new System.Drawing.Size(114, 22);
            this.mi_open.Text = "&Open";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(111, 6);
            // 
            // mi_save
            // 
            this.mi_save.Name = "mi_save";
            this.mi_save.Size = new System.Drawing.Size(114, 22);
            this.mi_save.Text = "&Save";
            // 
            // mi_saveAs
            // 
            this.mi_saveAs.Name = "mi_saveAs";
            this.mi_saveAs.Size = new System.Drawing.Size(114, 22);
            this.mi_saveAs.Text = "Save &As";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(111, 6);
            // 
            // mi_play
            // 
            this.mi_play.Name = "mi_play";
            this.mi_play.Size = new System.Drawing.Size(114, 22);
            this.mi_play.Text = "&Play!";
            // 
            // mi_quit
            // 
            this.mi_quit.Name = "mi_quit";
            this.mi_quit.Size = new System.Drawing.Size(114, 22);
            this.mi_quit.Text = "&Quit";
            // 
            // mi_edit
            // 
            this.mi_edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_undo,
            this.mi_redo,
            this.toolStripMenuItem1,
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
            // 
            // mi_redo
            // 
            this.mi_redo.Name = "mi_redo";
            this.mi_redo.Size = new System.Drawing.Size(103, 22);
            this.mi_redo.Text = "&Redo";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
            // 
            // mi_cut
            // 
            this.mi_cut.Name = "mi_cut";
            this.mi_cut.Size = new System.Drawing.Size(103, 22);
            this.mi_cut.Text = "&Cut";
            // 
            // mi_copy
            // 
            this.mi_copy.Name = "mi_copy";
            this.mi_copy.Size = new System.Drawing.Size(103, 22);
            this.mi_copy.Text = "C&opy";
            // 
            // mi_paste
            // 
            this.mi_paste.Name = "mi_paste";
            this.mi_paste.Size = new System.Drawing.Size(103, 22);
            this.mi_paste.Text = "&Paste";
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
            // 
            // mi_zoomOut
            // 
            this.mi_zoomOut.Name = "mi_zoomOut";
            this.mi_zoomOut.Size = new System.Drawing.Size(129, 22);
            this.mi_zoomOut.Text = "Zoom &Out";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1160, 638);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1184, 662);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menu);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.b_select,
            this.b_multiselect,
            this.b_addentity,
            this.b_removeentity,
            this.b_paint,
            this.b_depaint});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(24, 149);
            this.toolStrip1.TabIndex = 0;
            // 
            // b_select
            // 
            this.b_select.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_select.Image = ((System.Drawing.Image)(resources.GetObject("b_select.Image")));
            this.b_select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_select.Name = "b_select";
            this.b_select.Size = new System.Drawing.Size(22, 20);
            this.b_select.Text = "Select";
            // 
            // b_multiselect
            // 
            this.b_multiselect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_multiselect.Image = ((System.Drawing.Image)(resources.GetObject("b_multiselect.Image")));
            this.b_multiselect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_multiselect.Name = "b_multiselect";
            this.b_multiselect.Size = new System.Drawing.Size(29, 20);
            this.b_multiselect.Text = "Multi-Select";
            // 
            // b_addentity
            // 
            this.b_addentity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_addentity.Image = ((System.Drawing.Image)(resources.GetObject("b_addentity.Image")));
            this.b_addentity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_addentity.Name = "b_addentity";
            this.b_addentity.Size = new System.Drawing.Size(29, 20);
            this.b_addentity.Text = "Add Entity";
            // 
            // b_removeentity
            // 
            this.b_removeentity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_removeentity.Image = ((System.Drawing.Image)(resources.GetObject("b_removeentity.Image")));
            this.b_removeentity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_removeentity.Name = "b_removeentity";
            this.b_removeentity.Size = new System.Drawing.Size(29, 20);
            this.b_removeentity.Text = "Remove Entity";
            // 
            // b_paint
            // 
            this.b_paint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_paint.Image = ((System.Drawing.Image)(resources.GetObject("b_paint.Image")));
            this.b_paint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_paint.Name = "b_paint";
            this.b_paint.Size = new System.Drawing.Size(29, 20);
            this.b_paint.Text = "Paint Entity";
            // 
            // b_depaint
            // 
            this.b_depaint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_depaint.Image = ((System.Drawing.Image)(resources.GetObject("b_depaint.Image")));
            this.b_depaint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_depaint.Name = "b_depaint";
            this.b_depaint.Size = new System.Drawing.Size(29, 20);
            this.b_depaint.Text = "Unpaint Entity";
            this.b_depaint.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 662);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Name = "MainForm";
            this.Text = "Gravity Level Editor";
<<<<<<< HEAD
            this.Load += new System.EventHandler(this.MainForm_Load);
=======
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
>>>>>>> upstream/master
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.toolStripContainer1.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem mi_file;
        private System.Windows.Forms.ToolStripMenuItem mi_new;
        private System.Windows.Forms.ToolStripMenuItem mi_open;
        private System.Windows.Forms.ToolStripMenuItem mi_view;
        private System.Windows.Forms.ToolStripSeparator closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mi_save;
        private System.Windows.Forms.ToolStripMenuItem mi_saveAs;
        private System.Windows.Forms.ToolStripSeparator playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mi_play;
        private System.Windows.Forms.ToolStripMenuItem mi_quit;
        private System.Windows.Forms.ToolStripMenuItem mi_edit;
        private System.Windows.Forms.ToolStripMenuItem mi_undo;
        private System.Windows.Forms.ToolStripMenuItem mi_redo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mi_cut;
        private System.Windows.Forms.ToolStripMenuItem mi_copy;
        private System.Windows.Forms.ToolStripMenuItem mi_paste;
        private System.Windows.Forms.ToolStripMenuItem mi_zoomIn;
        private System.Windows.Forms.ToolStripMenuItem mi_zoomOut;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton b_select;
        private System.Windows.Forms.ToolStripButton b_multiselect;
        private System.Windows.Forms.ToolStripButton b_addentity;
        private System.Windows.Forms.ToolStripButton b_removeentity;
        private System.Windows.Forms.ToolStripButton b_paint;
        private System.Windows.Forms.ToolStripButton b_depaint;
    }
}

