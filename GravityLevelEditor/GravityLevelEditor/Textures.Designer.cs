namespace GravityLevelEditor
{
    partial class Textures
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
            this.importButton = new System.Windows.Forms.Button();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.textureNameBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.textureBox = new System.Windows.Forms.PictureBox();
            this.selectImageButton = new System.Windows.Forms.Button();
            this.doneButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.textureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(566, 12);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(106, 31);
            this.importButton.TabIndex = 0;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Location = new System.Drawing.Point(480, 85);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(54, 13);
            this.propertiesLabel.TabIndex = 1;
            this.propertiesLabel.Text = "Properties";
            // 
            // textureNameBox
            // 
            this.textureNameBox.Location = new System.Drawing.Point(521, 114);
            this.textureNameBox.Name = "textureNameBox";
            this.textureNameBox.Size = new System.Drawing.Size(137, 20);
            this.textureNameBox.TabIndex = 2;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(480, 117);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Name";
            // 
            // textureBox
            // 
            this.textureBox.Location = new System.Drawing.Point(12, 85);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(430, 361);
            this.textureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.textureBox.TabIndex = 4;
            this.textureBox.TabStop = false;
            // 
            // selectImageButton
            // 
            this.selectImageButton.Location = new System.Drawing.Point(12, 12);
            this.selectImageButton.Name = "selectImageButton";
            this.selectImageButton.Size = new System.Drawing.Size(110, 31);
            this.selectImageButton.TabIndex = 5;
            this.selectImageButton.Text = "Select Image";
            this.selectImageButton.UseVisualStyleBackColor = true;
            this.selectImageButton.Click += new System.EventHandler(this.selectImageButton_Click);
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(521, 409);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(98, 37);
            this.doneButton.TabIndex = 6;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // Textures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.selectImageButton);
            this.Controls.Add(this.textureBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.textureNameBox);
            this.Controls.Add(this.propertiesLabel);
            this.Controls.Add(this.importButton);
            this.Name = "Textures";
            this.Text = "Textures";
            ((System.ComponentModel.ISupportInitialize)(this.textureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.TextBox textureNameBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.PictureBox textureBox;
        private System.Windows.Forms.Button selectImageButton;
        private System.Windows.Forms.Button doneButton;
    }
}