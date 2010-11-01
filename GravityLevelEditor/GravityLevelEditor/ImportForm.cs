﻿using System;
using System.Collections.Generic;
using System.Collections;
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
        #region Member Variables

        public MainForm FirstForm { get; set; }

        private string imageLocation = "..\\..\\..\\..\\WindowsGame1\\Content\\";

        private string invalidFileMessage = "Please select a valid PNG file.";
        private string fileExistsMessage = "File already exists.";

        private ArrayList folders = new ArrayList();

        #endregion

        /*
         * ImportForm Constructor
         *
         * This function sets up the arraylist for the folders, and 
         * initializes the components
         *
         */
        public ImportForm()
        {
            InitializeComponent();

            /* Adds default items to the folder list */
            folders.Add("Tiles");
            folders.Add("Background");
            folders.Add("Character");
            folderBox.DataSource = folders;

            /* Hide the successful label - only want this to show later */
            successfulLabel.Hide();
        }

        /*
         * BrowseButton_Click
         *
         * This function is called when the browse button is clicked. It will
         * ask the user to select the file they wish to import. Once selected
         * it will fill in the location of the file.
         *
         * object send: Not sure what this is, it was auto populated by forms.
         * …
         * EventArgs e: Event arguments sent from clicking BrowseButton.
         *
         * Return Value: Void.
         */
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            /* Hide the successful label */
            successfulLabel.Hide();
            
            /* Open the dialog box where the user can browse for the file */
            OpenFileDialog importFileDialog = new OpenFileDialog();
            /* Restrict the search for only .png files*/
            importFileDialog.Filter = "PNG Files|*.png";
            /* Add a title, so the user knows to only look for .png files */
            importFileDialog.Title = "Select a .PNG Image File";
            importFileDialog.ShowDialog();
            if (importFileDialog.FileName != "")
            {
                /* Set the text to be the location of the file */
                imageLocBox.Text = importFileDialog.FileName;
                /* Preview the image the user selected */
                Image previewImage = Image.FromFile(importFileDialog.FileName);
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                previewBox.Image = previewImage.GetThumbnailImage(200, 165, myCallback, IntPtr.Zero);
            }
        }
        /*
         * ThumbnailCallback
         * 
         * Required to get a thumbnail of the preview image.
         * 
         * Return Value: always false.
         */
        public bool ThumbnailCallback()
        {
            return false;
        }

        /*
         * LoadButton_Click
         *
         * This function will be called when the load button is clicked (obviously).
         * It will first do error checking:
         *      - If the user has not selected a file
         *      - If the file already exists in that folder
         *
         * The function will also create a new folder if the folder has not already
         * been created.
         * 
         * The funtion also saves the desired file into the folder the user selects.
         * If no folder is selected, it defaults to the image folder (inside content).
         *
         * Once everything is saved, shows the successful label and return everything
         * to its default state.
         * 
         * object sender: Not sure what this is, it was auto populated by forms.
         * …
         * EventArgs e: Event arguments sent from clicking BrowseButton.
         *
         * Return Value: Void.
         */
        private void LoadButton_Click(object sender, EventArgs e)
        {
            /* If the user has not selected an image */
            if (previewBox.Image == null)
            {
                MessageBox.Show(invalidFileMessage);
                return;
            }

            /* If the file already exists in the designated folder */
            if (System.IO.File.Exists(imageLocation + folderBox.Text + "\\" +
                nameBox.Text + ".png"))
            {
                MessageBox.Show(fileExistsMessage);
                imageLocBox.Text = "";
                previewBox.Image = null;
                nameBox.Text = null;
                return;
            }

            /* If the images folder does not exist in the content folder yet */
            if (imageLocation.IndexOf("Images") == -1)
            {
                System.IO.Directory.CreateDirectory(imageLocation + "Images\\");
                imageLocation = "..\\..\\..\\..\\WindowsGame1\\Content\\Images\\";
            }

            /* If the folder selected in the combo box does not exist yet */
            if (imageLocation.IndexOf(folderBox.Text) == -1)
            {
                System.IO.Directory.CreateDirectory(imageLocation + folderBox.Text + "\\");
                folders.Add(folderBox.Text);

                /* TODO */
                /* Make the combo box refresh if the user creates a new folder */
            }
            /* Save the file at the desired location */
            previewBox.Image.Save(imageLocation + folderBox.Text + "\\" +
                   nameBox.Text + ".png");
            successfulLabel.Show();
            /* Reset everything */
            imageLocBox.Text = "";
            previewBox.Image = null;
            nameBox.Text = null;
        }

        /* TODO */
        /* Make the folders and files show as thumbnail views in the listview */
        
        /* Auto populate the folderbox with the folders that already exist in the users
         * directory 
         */ 
    }
}