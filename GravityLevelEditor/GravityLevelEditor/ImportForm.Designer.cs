namespace GravityLevelEditor
{
    partial class ImportForm
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
            this.components = new System.ComponentModel.Container();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.imageLocBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.folderLabel = new System.Windows.Forms.Label();
            this.folderBox = new System.Windows.Forms.ComboBox();
            this.imageViewer = new System.Windows.Forms.ListView();
            this.successfulLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(713, 15);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(82, 26);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // imageLocBox
            // 
            this.imageLocBox.Location = new System.Drawing.Point(481, 19);
            this.imageLocBox.Name = "imageLocBox";
            this.imageLocBox.Size = new System.Drawing.Size(226, 20);
            this.imageLocBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(478, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter the location of the image";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(494, 86);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(538, 79);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(237, 20);
            this.nameBox.TabIndex = 6;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(593, 356);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(115, 38);
            this.loadButton.TabIndex = 7;
            this.loadButton.Text = "Load Image";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // previewBox
            // 
            this.previewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewBox.Location = new System.Drawing.Point(538, 163);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(200, 165);
            this.previewBox.TabIndex = 8;
            this.previewBox.TabStop = false;
            // 
            // folderLabel
            // 
            this.folderLabel.AutoSize = true;
            this.folderLabel.Location = new System.Drawing.Point(494, 127);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new System.Drawing.Size(39, 13);
            this.folderLabel.TabIndex = 9;
            this.folderLabel.Text = "Folder:";
            // 
            // folderBox
            // 
            this.folderBox.FormattingEnabled = true;
            this.folderBox.Location = new System.Drawing.Point(539, 119);
            this.folderBox.Name = "folderBox";
            this.folderBox.Size = new System.Drawing.Size(236, 21);
            this.folderBox.TabIndex = 11;
            // 
            // imageViewer
            // 
            this.imageViewer.Location = new System.Drawing.Point(26, 21);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(415, 309);
            this.imageViewer.TabIndex = 12;
            this.imageViewer.UseCompatibleStateImageBehavior = false;
            // 
            // successfulLabel
            // 
            this.successfulLabel.AutoSize = true;
            this.successfulLabel.Location = new System.Drawing.Point(619, 340);
            this.successfulLabel.Name = "successfulLabel";
            this.successfulLabel.Size = new System.Drawing.Size(59, 13);
            this.successfulLabel.TabIndex = 13;
            this.successfulLabel.Text = "Successful";
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 462);
            this.Controls.Add(this.successfulLabel);
            this.Controls.Add(this.imageViewer);
            this.Controls.Add(this.folderBox);
            this.Controls.Add(this.folderLabel);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imageLocBox);
            this.Controls.Add(this.BrowseButton);
            this.Name = "ImportForm";
            this.Text = "Import Image";
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox imageLocBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.Label folderLabel;
        private System.Windows.Forms.ComboBox folderBox;
        private System.Windows.Forms.ListView imageViewer;
        private System.Windows.Forms.Label successfulLabel;
    }
}