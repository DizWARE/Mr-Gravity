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
            this.BrowseButton = new System.Windows.Forms.Button();
            this.imageLocBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.folderLabel = new System.Windows.Forms.Label();
            this.folderBox = new System.Windows.Forms.ComboBox();
            this.successfulLabel = new System.Windows.Forms.Label();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.loadAndExitButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(285, 8);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(82, 26);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // imageLocBox
            // 
            this.imageLocBox.Location = new System.Drawing.Point(12, 12);
            this.imageLocBox.Name = "imageLocBox";
            this.imageLocBox.Size = new System.Drawing.Size(226, 20);
            this.imageLocBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter the location of the image";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(25, 79);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(69, 72);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(237, 20);
            this.nameBox.TabIndex = 6;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(45, 349);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(115, 38);
            this.loadButton.TabIndex = 7;
            this.loadButton.Text = "Upload";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // folderLabel
            // 
            this.folderLabel.AutoSize = true;
            this.folderLabel.Location = new System.Drawing.Point(25, 120);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new System.Drawing.Size(39, 13);
            this.folderLabel.TabIndex = 9;
            this.folderLabel.Text = "Folder:";
            // 
            // folderBox
            // 
            this.folderBox.FormattingEnabled = true;
            this.folderBox.Location = new System.Drawing.Point(70, 112);
            this.folderBox.Name = "folderBox";
            this.folderBox.Size = new System.Drawing.Size(236, 21);
            this.folderBox.TabIndex = 11;
            // 
            // successfulLabel
            // 
            this.successfulLabel.AutoSize = true;
            this.successfulLabel.Location = new System.Drawing.Point(73, 333);
            this.successfulLabel.Name = "successfulLabel";
            this.successfulLabel.Size = new System.Drawing.Size(59, 13);
            this.successfulLabel.TabIndex = 13;
            this.successfulLabel.Text = "Successful";
            // 
            // previewBox
            // 
            this.previewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewBox.Location = new System.Drawing.Point(69, 151);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(212, 165);
            this.previewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.previewBox.TabIndex = 14;
            this.previewBox.TabStop = false;
            // 
            // loadAndExitButton
            // 
            this.loadAndExitButton.Location = new System.Drawing.Point(252, 349);
            this.loadAndExitButton.Name = "loadAndExitButton";
            this.loadAndExitButton.Size = new System.Drawing.Size(115, 38);
            this.loadAndExitButton.TabIndex = 15;
            this.loadAndExitButton.Text = "Upload and Exit";
            this.loadAndExitButton.UseVisualStyleBackColor = true;
            this.loadAndExitButton.Click += new System.EventHandler(this.loadAndExitButton_Click);
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 462);
            this.Controls.Add(this.loadAndExitButton);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.successfulLabel);
            this.Controls.Add(this.folderBox);
            this.Controls.Add(this.folderLabel);
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
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Label folderLabel;
        private System.Windows.Forms.ComboBox folderBox;
        private System.Windows.Forms.Label successfulLabel;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.Button loadAndExitButton;
    }
}