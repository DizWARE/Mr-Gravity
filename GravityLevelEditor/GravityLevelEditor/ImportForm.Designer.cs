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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportForm));
            this.BrowseButton = new System.Windows.Forms.Button();
            this.imageLocBox = new System.Windows.Forms.TextBox();
            this.lbl_file = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.folderLabel = new System.Windows.Forms.Label();
            this.folderBox = new System.Windows.Forms.ComboBox();
            this.successfulLabel = new System.Windows.Forms.Label();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.lbl_import = new System.Windows.Forms.Label();
            this.lbl_select = new System.Windows.Forms.Label();
            this.lb_images = new System.Windows.Forms.ListBox();
            this.pb_selectPreview = new System.Windows.Forms.PictureBox();
            this.cb_folder = new System.Windows.Forms.ComboBox();
            this.b_selectImage = new System.Windows.Forms.Button();
            this.lbl_folder = new System.Windows.Forms.Label();
            this.lbl_selectImage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_selectPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(690, 98);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(82, 26);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // imageLocBox
            // 
            this.imageLocBox.Location = new System.Drawing.Point(450, 98);
            this.imageLocBox.Name = "imageLocBox";
            this.imageLocBox.Size = new System.Drawing.Size(226, 20);
            this.imageLocBox.TabIndex = 1;
            // 
            // lbl_file
            // 
            this.lbl_file.AutoSize = true;
            this.lbl_file.Location = new System.Drawing.Point(382, 98);
            this.lbl_file.Name = "lbl_file";
            this.lbl_file.Size = new System.Drawing.Size(65, 13);
            this.lbl_file.TabIndex = 2;
            this.lbl_file.Text = "Choose File:";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(382, 144);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(450, 141);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(237, 20);
            this.nameBox.TabIndex = 6;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(512, 256);
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
            this.folderLabel.Location = new System.Drawing.Point(382, 186);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new System.Drawing.Size(39, 13);
            this.folderLabel.TabIndex = 9;
            this.folderLabel.Text = "Folder:";
            // 
            // folderBox
            // 
            this.folderBox.FormattingEnabled = true;
            this.folderBox.Location = new System.Drawing.Point(451, 183);
            this.folderBox.Name = "folderBox";
            this.folderBox.Size = new System.Drawing.Size(236, 21);
            this.folderBox.TabIndex = 11;
            // 
            // successfulLabel
            // 
            this.successfulLabel.AutoSize = true;
            this.successfulLabel.Location = new System.Drawing.Point(540, 240);
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
            this.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewBox.Location = new System.Drawing.Point(690, 28);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(64, 64);
            this.previewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.previewBox.TabIndex = 14;
            this.previewBox.TabStop = false;
            // 
            // lbl_import
            // 
            this.lbl_import.AutoSize = true;
            this.lbl_import.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_import.Location = new System.Drawing.Point(381, 28);
            this.lbl_import.Name = "lbl_import";
            this.lbl_import.Size = new System.Drawing.Size(133, 20);
            this.lbl_import.TabIndex = 16;
            this.lbl_import.Text = "Import Image File";
            // 
            // lbl_select
            // 
            this.lbl_select.AutoSize = true;
            this.lbl_select.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_select.Location = new System.Drawing.Point(12, 28);
            this.lbl_select.Name = "lbl_select";
            this.lbl_select.Size = new System.Drawing.Size(190, 20);
            this.lbl_select.TabIndex = 17;
            this.lbl_select.Text = "Select Loaded Image File";
            // 
            // lb_images
            // 
            this.lb_images.FormattingEnabled = true;
            this.lb_images.Location = new System.Drawing.Point(90, 141);
            this.lb_images.Name = "lb_images";
            this.lb_images.Size = new System.Drawing.Size(186, 95);
            this.lb_images.TabIndex = 18;
            this.lb_images.SelectedIndexChanged += new System.EventHandler(this.SelectImage);
            // 
            // pb_selectPreview
            // 
            this.pb_selectPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_selectPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_selectPreview.Location = new System.Drawing.Point(262, 28);
            this.pb_selectPreview.Name = "pb_selectPreview";
            this.pb_selectPreview.Size = new System.Drawing.Size(64, 64);
            this.pb_selectPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_selectPreview.TabIndex = 19;
            this.pb_selectPreview.TabStop = false;
            // 
            // cb_folder
            // 
            this.cb_folder.FormattingEnabled = true;
            this.cb_folder.Location = new System.Drawing.Point(90, 97);
            this.cb_folder.Name = "cb_folder";
            this.cb_folder.Size = new System.Drawing.Size(236, 21);
            this.cb_folder.TabIndex = 20;
            this.cb_folder.SelectedIndexChanged += new System.EventHandler(this.SelectDirectory);
            // 
            // b_selectImage
            // 
            this.b_selectImage.Location = new System.Drawing.Point(98, 256);
            this.b_selectImage.Name = "b_selectImage";
            this.b_selectImage.Size = new System.Drawing.Size(115, 38);
            this.b_selectImage.TabIndex = 21;
            this.b_selectImage.Text = "Select Image";
            this.b_selectImage.UseVisualStyleBackColor = true;
            this.b_selectImage.Click += new System.EventHandler(this.OK);
            // 
            // lbl_folder
            // 
            this.lbl_folder.AutoSize = true;
            this.lbl_folder.Location = new System.Drawing.Point(13, 100);
            this.lbl_folder.Name = "lbl_folder";
            this.lbl_folder.Size = new System.Drawing.Size(39, 13);
            this.lbl_folder.TabIndex = 22;
            this.lbl_folder.Text = "Folder:";
            // 
            // lbl_selectImage
            // 
            this.lbl_selectImage.AutoSize = true;
            this.lbl_selectImage.Location = new System.Drawing.Point(13, 141);
            this.lbl_selectImage.Name = "lbl_selectImage";
            this.lbl_selectImage.Size = new System.Drawing.Size(72, 13);
            this.lbl_selectImage.TabIndex = 23;
            this.lbl_selectImage.Text = "Select Image:";
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 320);
            this.Controls.Add(this.lbl_selectImage);
            this.Controls.Add(this.lbl_folder);
            this.Controls.Add(this.b_selectImage);
            this.Controls.Add(this.cb_folder);
            this.Controls.Add(this.pb_selectPreview);
            this.Controls.Add(this.lb_images);
            this.Controls.Add(this.lbl_select);
            this.Controls.Add(this.lbl_import);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.successfulLabel);
            this.Controls.Add(this.folderBox);
            this.Controls.Add(this.folderLabel);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.lbl_file);
            this.Controls.Add(this.imageLocBox);
            this.Controls.Add(this.BrowseButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportForm";
            this.Text = "Import Image";
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_selectPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox imageLocBox;
        private System.Windows.Forms.Label lbl_file;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Label folderLabel;
        private System.Windows.Forms.ComboBox folderBox;
        private System.Windows.Forms.Label successfulLabel;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.Label lbl_import;
        private System.Windows.Forms.Label lbl_select;
        private System.Windows.Forms.ListBox lb_images;
        private System.Windows.Forms.PictureBox pb_selectPreview;
        private System.Windows.Forms.ComboBox cb_folder;
        private System.Windows.Forms.Button b_selectImage;
        private System.Windows.Forms.Label lbl_folder;
        private System.Windows.Forms.Label lbl_selectImage;
    }
}