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
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
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
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.buttonContainer1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.tsc_MainContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sc_Properties = new System.Windows.Forms.SplitContainer();
            this.sc_HorizontalProperties = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.b_ChangePicture = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.l_LevelProperties = new System.Windows.Forms.Label();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.pb_Entity = new System.Windows.Forms.PictureBox();
            this.b_select = new System.Windows.Forms.ToolStripButton();
            this.b_multiselect = new System.Windows.Forms.ToolStripButton();
            this.b_addentity = new System.Windows.Forms.ToolStripButton();
            this.b_removeentity = new System.Windows.Forms.ToolStripButton();
            this.b_paint = new System.Windows.Forms.ToolStripButton();
            this.b_depaint = new System.Windows.Forms.ToolStripButton();
            this.menu.SuspendLayout();
            this.buttonContainer1.SuspendLayout();
            this.tsc_MainContainer.BottomToolStripPanel.SuspendLayout();
            this.tsc_MainContainer.ContentPanel.SuspendLayout();
            this.tsc_MainContainer.TopToolStripPanel.SuspendLayout();
            this.tsc_MainContainer.SuspendLayout();
            this.sc_Properties.Panel2.SuspendLayout();
            this.sc_Properties.SuspendLayout();
            this.sc_HorizontalProperties.Panel1.SuspendLayout();
            this.sc_HorizontalProperties.Panel2.SuspendLayout();
            this.sc_HorizontalProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Entity)).BeginInit();
            this.SuspendLayout();
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menu.Dock = System.Windows.Forms.DockStyle.None;
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_file,
            this.mi_edit,
            this.mi_view});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1184, 29);
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
            this.mi_file.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.mi_file.Name = "mi_file";
            this.mi_file.Size = new System.Drawing.Size(46, 25);
            this.mi_file.Text = "&File";
            // 
            // mi_new
            // 
            this.mi_new.Name = "mi_new";
            this.mi_new.Size = new System.Drawing.Size(134, 26);
            this.mi_new.Text = "&New";
            // 
            // mi_open
            // 
            this.mi_open.Name = "mi_open";
            this.mi_open.Size = new System.Drawing.Size(134, 26);
            this.mi_open.Text = "&Open";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(131, 6);
            // 
            // mi_save
            // 
            this.mi_save.Name = "mi_save";
            this.mi_save.Size = new System.Drawing.Size(134, 26);
            this.mi_save.Text = "&Save";
            // 
            // mi_saveAs
            // 
            this.mi_saveAs.Name = "mi_saveAs";
            this.mi_saveAs.Size = new System.Drawing.Size(134, 26);
            this.mi_saveAs.Text = "Save &As";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(131, 6);
            // 
            // mi_play
            // 
            this.mi_play.Name = "mi_play";
            this.mi_play.Size = new System.Drawing.Size(134, 26);
            this.mi_play.Text = "&Play!";
            // 
            // mi_quit
            // 
            this.mi_quit.Name = "mi_quit";
            this.mi_quit.Size = new System.Drawing.Size(134, 26);
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
            this.mi_edit.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.mi_edit.Name = "mi_edit";
            this.mi_edit.Size = new System.Drawing.Size(48, 25);
            this.mi_edit.Text = "&Edit";
            // 
            // mi_undo
            // 
            this.mi_undo.Name = "mi_undo";
            this.mi_undo.Size = new System.Drawing.Size(118, 26);
            this.mi_undo.Text = "&Undo";
            // 
            // mi_redo
            // 
            this.mi_redo.Name = "mi_redo";
            this.mi_redo.Size = new System.Drawing.Size(118, 26);
            this.mi_redo.Text = "&Redo";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(115, 6);
            // 
            // mi_cut
            // 
            this.mi_cut.Name = "mi_cut";
            this.mi_cut.Size = new System.Drawing.Size(118, 26);
            this.mi_cut.Text = "&Cut";
            // 
            // mi_copy
            // 
            this.mi_copy.Name = "mi_copy";
            this.mi_copy.Size = new System.Drawing.Size(118, 26);
            this.mi_copy.Text = "C&opy";
            // 
            // mi_paste
            // 
            this.mi_paste.Name = "mi_paste";
            this.mi_paste.Size = new System.Drawing.Size(118, 26);
            this.mi_paste.Text = "&Paste";
            // 
            // mi_view
            // 
            this.mi_view.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_zoomIn,
            this.mi_zoomOut});
            this.mi_view.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.mi_view.Name = "mi_view";
            this.mi_view.Size = new System.Drawing.Size(56, 25);
            this.mi_view.Text = "&View";
            // 
            // mi_zoomIn
            // 
            this.mi_zoomIn.Name = "mi_zoomIn";
            this.mi_zoomIn.Size = new System.Drawing.Size(151, 26);
            this.mi_zoomIn.Text = "Zoom &In";
            // 
            // mi_zoomOut
            // 
            this.mi_zoomOut.Name = "mi_zoomOut";
            this.mi_zoomOut.Size = new System.Drawing.Size(151, 26);
            this.mi_zoomOut.Text = "Zoom &Out";
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // buttonContainer1
            // 
            this.buttonContainer1.AutoSize = false;
            this.buttonContainer1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.buttonContainer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonContainer1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.buttonContainer1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.buttonContainer1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.b_select,
            this.b_multiselect,
            this.b_addentity,
            this.b_removeentity,
            this.b_paint,
            this.b_depaint,
            this.toolStripSeparator2});
            this.buttonContainer1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.buttonContainer1.Location = new System.Drawing.Point(0, 0);
            this.buttonContainer1.Name = "buttonContainer1";
            this.buttonContainer1.Size = new System.Drawing.Size(56, 611);
            this.buttonContainer1.TabIndex = 0;
            this.buttonContainer1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.buttonContainer1_ItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(54, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(54, 6);
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.Size = new System.Drawing.Size(1160, 638);
            // 
            // tsc_MainContainer
            // 
            // 
            // tsc_MainContainer.BottomToolStripPanel
            // 
            this.tsc_MainContainer.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // tsc_MainContainer.ContentPanel
            // 
            this.tsc_MainContainer.ContentPanel.AutoScroll = true;
            this.tsc_MainContainer.ContentPanel.Controls.Add(this.sc_Properties);
            this.tsc_MainContainer.ContentPanel.Controls.Add(this.buttonContainer1);
            this.tsc_MainContainer.ContentPanel.Size = new System.Drawing.Size(1184, 611);
            this.tsc_MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsc_MainContainer.Location = new System.Drawing.Point(0, 0);
            this.tsc_MainContainer.Name = "tsc_MainContainer";
            this.tsc_MainContainer.Size = new System.Drawing.Size(1184, 662);
            this.tsc_MainContainer.TabIndex = 1;
            this.tsc_MainContainer.Text = "toolStripContainer1";
            // 
            // tsc_MainContainer.TopToolStripPanel
            // 
            this.tsc_MainContainer.TopToolStripPanel.Controls.Add(this.menu);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // sc_Properties
            // 
            this.sc_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_Properties.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.sc_Properties.IsSplitterFixed = true;
            this.sc_Properties.Location = new System.Drawing.Point(56, 0);
            this.sc_Properties.Name = "sc_Properties";
            // 
            // sc_Properties.Panel1
            // 
            this.sc_Properties.Panel1.AutoScroll = true;
            this.sc_Properties.Panel1MinSize = 100;
            // 
            // sc_Properties.Panel2
            // 
            this.sc_Properties.Panel2.AutoScroll = true;
            this.sc_Properties.Panel2.Controls.Add(this.sc_HorizontalProperties);
            this.sc_Properties.Panel2MinSize = 100;
            this.sc_Properties.Size = new System.Drawing.Size(1128, 611);
            this.sc_Properties.SplitterDistance = 906;
            this.sc_Properties.TabIndex = 1;
            // 
            // sc_HorizontalProperties
            // 
            this.sc_HorizontalProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.sc_HorizontalProperties.Location = new System.Drawing.Point(3, 0);
            this.sc_HorizontalProperties.Name = "sc_HorizontalProperties";
            this.sc_HorizontalProperties.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sc_HorizontalProperties.Panel1
            // 
            this.sc_HorizontalProperties.Panel1.Controls.Add(this.label1);
            this.sc_HorizontalProperties.Panel1.Controls.Add(this.b_ChangePicture);
            this.sc_HorizontalProperties.Panel1.Controls.Add(this.pb_Entity);
            this.sc_HorizontalProperties.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // sc_HorizontalProperties.Panel2
            // 
            this.sc_HorizontalProperties.Panel2.Controls.Add(this.l_LevelProperties);
            this.sc_HorizontalProperties.Panel2.Controls.Add(this.propertyGrid2);
            this.sc_HorizontalProperties.Size = new System.Drawing.Size(215, 611);
            this.sc_HorizontalProperties.SplitterDistance = 339;
            this.sc_HorizontalProperties.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Entity Properties";
            // 
            // b_ChangePicture
            // 
            this.b_ChangePicture.Location = new System.Drawing.Point(143, 129);
            this.b_ChangePicture.Name = "b_ChangePicture";
            this.b_ChangePicture.Size = new System.Drawing.Size(60, 27);
            this.b_ChangePicture.TabIndex = 1;
            this.b_ChangePicture.Text = "Browse...";
            this.b_ChangePicture.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(3, 162);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(209, 171);
            this.propertyGrid1.TabIndex = 0;
            // 
            // l_LevelProperties
            // 
            this.l_LevelProperties.AutoSize = true;
            this.l_LevelProperties.Font = new System.Drawing.Font("Palatino Linotype", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_LevelProperties.Location = new System.Drawing.Point(23, 7);
            this.l_LevelProperties.Name = "l_LevelProperties";
            this.l_LevelProperties.Size = new System.Drawing.Size(175, 29);
            this.l_LevelProperties.TabIndex = 1;
            this.l_LevelProperties.Text = "Level Properties";
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(3, 36);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(209, 229);
            this.propertyGrid2.TabIndex = 0;
            // 
            // pb_Entity
            // 
            this.pb_Entity.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pb_Entity.Location = new System.Drawing.Point(17, 36);
            this.pb_Entity.Name = "pb_Entity";
            this.pb_Entity.Size = new System.Drawing.Size(120, 120);
            this.pb_Entity.TabIndex = 0;
            this.pb_Entity.TabStop = false;
            // 
            // b_select
            // 
            this.b_select.AutoSize = false;
            this.b_select.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_select.Image = global::GravityLevelEditor.Properties.Resources.select;
            this.b_select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_select.Name = "b_select";
            this.b_select.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.b_select.Size = new System.Drawing.Size(48, 48);
            this.b_select.Text = "Select";
            // 
            // b_multiselect
            // 
            this.b_multiselect.AutoSize = false;
            this.b_multiselect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_multiselect.Image = global::GravityLevelEditor.Properties.Resources.multiselect;
            this.b_multiselect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_multiselect.Name = "b_multiselect";
            this.b_multiselect.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.b_multiselect.Size = new System.Drawing.Size(48, 48);
            this.b_multiselect.Text = "Multi-Select";
            // 
            // b_addentity
            // 
            this.b_addentity.AutoSize = false;
            this.b_addentity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_addentity.Image = global::GravityLevelEditor.Properties.Resources.addentity;
            this.b_addentity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_addentity.Name = "b_addentity";
            this.b_addentity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.b_addentity.Size = new System.Drawing.Size(48, 48);
            this.b_addentity.Text = "Add Entity";
            // 
            // b_removeentity
            // 
            this.b_removeentity.AutoSize = false;
            this.b_removeentity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_removeentity.Image = global::GravityLevelEditor.Properties.Resources.removeentity;
            this.b_removeentity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_removeentity.Name = "b_removeentity";
            this.b_removeentity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.b_removeentity.Size = new System.Drawing.Size(48, 48);
            this.b_removeentity.Text = "Remove Entity";
            // 
            // b_paint
            // 
            this.b_paint.AutoSize = false;
            this.b_paint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_paint.Image = global::GravityLevelEditor.Properties.Resources.paint;
            this.b_paint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_paint.Name = "b_paint";
            this.b_paint.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.b_paint.Size = new System.Drawing.Size(48, 48);
            this.b_paint.Text = "Paint Entity";
            // 
            // b_depaint
            // 
            this.b_depaint.AutoSize = false;
            this.b_depaint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.b_depaint.Image = global::GravityLevelEditor.Properties.Resources.depaint;
            this.b_depaint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.b_depaint.Name = "b_depaint";
            this.b_depaint.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.b_depaint.Size = new System.Drawing.Size(48, 48);
            this.b_depaint.Text = "Unpaint Entity";
            this.b_depaint.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 662);
            this.Controls.Add(this.tsc_MainContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Name = "MainForm";
            this.Text = "Gravity Level Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.buttonContainer1.ResumeLayout(false);
            this.buttonContainer1.PerformLayout();
            this.tsc_MainContainer.BottomToolStripPanel.ResumeLayout(false);
            this.tsc_MainContainer.BottomToolStripPanel.PerformLayout();
            this.tsc_MainContainer.ContentPanel.ResumeLayout(false);
            this.tsc_MainContainer.TopToolStripPanel.ResumeLayout(false);
            this.tsc_MainContainer.TopToolStripPanel.PerformLayout();
            this.tsc_MainContainer.ResumeLayout(false);
            this.tsc_MainContainer.PerformLayout();
            this.sc_Properties.Panel2.ResumeLayout(false);
            this.sc_Properties.ResumeLayout(false);
            this.sc_HorizontalProperties.Panel1.ResumeLayout(false);
            this.sc_HorizontalProperties.Panel1.PerformLayout();
            this.sc_HorizontalProperties.Panel2.ResumeLayout(false);
            this.sc_HorizontalProperties.Panel2.PerformLayout();
            this.sc_HorizontalProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Entity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem mi_file;
        private System.Windows.Forms.ToolStripMenuItem mi_new;
        private System.Windows.Forms.ToolStripMenuItem mi_open;
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
        private System.Windows.Forms.ToolStripMenuItem mi_view;
        private System.Windows.Forms.ToolStripMenuItem mi_zoomIn;
        private System.Windows.Forms.ToolStripMenuItem mi_zoomOut;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStrip buttonContainer1;
        private System.Windows.Forms.ToolStripButton b_select;
        private System.Windows.Forms.ToolStripButton b_multiselect;
        private System.Windows.Forms.ToolStripButton b_addentity;
        private System.Windows.Forms.ToolStripButton b_removeentity;
        private System.Windows.Forms.ToolStripButton b_paint;
        private System.Windows.Forms.ToolStripButton b_depaint;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripContainer tsc_MainContainer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.SplitContainer sc_Properties;
        private System.Windows.Forms.SplitContainer sc_HorizontalProperties;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.Button b_ChangePicture;
        private System.Windows.Forms.PictureBox pb_Entity;
        private System.Windows.Forms.Label l_LevelProperties;
        private System.Windows.Forms.Label label1;

    }
}

