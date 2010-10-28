using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GravityLevelEditor
{
    public partial class ImportForm : Form
    {
        public MainForm FirstForm { get; set; }
        MainForm main = new MainForm();

        public ImportForm()
        {
            InitializeComponent();
            folderBox.DataSource = main.folders;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            imageList1 = new ImageList();
            OpenFileDialog importFileDialog = new OpenFileDialog();
            importFileDialog.Filter = "PNG Files|*.png";
            importFileDialog.Title = "Select a .PNG Image File";
            importFileDialog.ShowDialog();
            if (importFileDialog.FileName != "")
            {
                imageLocBox.Text = importFileDialog.FileName;
                previewBox.Load(importFileDialog.FileName);
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (main.imageLocation.IndexOf("Images") == -1)
            {
                System.IO.Directory.CreateDirectory(main.imageLocation + "Images\\");
                main.imageLocation = "..\\..\\..\\..\\WindowsGame1\\Content\\Images\\";
            }
            previewBox.Image.Save(main.imageLocation +
                   nameBox.Text + ".png");
        
        }
    }
}