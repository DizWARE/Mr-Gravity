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
    public partial class Textures : Form
    {
        #region Member Variables

        ImportForm import = new ImportForm();

        #endregion

        /*
         * Textures Constructor
         *
         * This function creates the images folder if it doesn't already exist, and 
         * initializes the components
         *
         */
        public Textures()
        {
            if (import.imageLocation.IndexOf("Images") == -1)
            {
                System.IO.Directory.CreateDirectory(import.imageLocation + "Images\\");
            }

            InitializeComponent();
        }

        /*
         * importButton_Click
         *
         * The function sets the first form, so that it may return to it once the import
         * form is exited.
         * 
         * It also causes the import form to show.
         *
         * object send: Not sure what this is, it was auto populated by forms.
         * …
         * EventArgs e: I believe this is for error checking, but again it was auto populated.
         *
         * Return Value: Void.
         */
        private void importButton_Click(object sender, EventArgs e)
        {
            import.FirstForm = this;
            import.Show();
        }

        /*
         * selectImageButton_Click
         *
         * This function brings up a windows load dialog box that allows the user
         * to select the image they want to import. It restricts the search to .png files
         * only. It also loads the name of the image into the name folder, and a preview of
         * the selected image.
         *
         * object send: Not sure what this is, it was auto populated by forms.
         * …
         * EventArgs e: I believe this is for error checking, but again it was auto populated.
         *
         * Return Value: Void.
         */
        private void selectImageButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog selectTextureDialog = new OpenFileDialog();
            /* TODO */
            // Get the initial directory set to the images directory
//            selectTextureDialog.InitialDirectory = @"c:\";
//            selectTextureDialog.InitialDirectory = Application.StartupPath;          
            /* Restrict the search for only .png files*/
            selectTextureDialog.Filter = "PNG Files|*.png";
            /* Add a title, so the user knows to only look for .png files */
            selectTextureDialog.Title = "Select a .PNG Image File";
            selectTextureDialog.ShowDialog();
            if (selectTextureDialog.FileName != "")
            {
                /* Set the text to be the location of the file */
                int dash = selectTextureDialog.FileName.LastIndexOf("\\");
                int period = selectTextureDialog.FileName.LastIndexOf('.');
                textureNameBox.Text = selectTextureDialog.FileName.Substring(dash+1, (period - dash) - 1);
                /* Preview the image the user selected */
                textureBox.Load(selectTextureDialog.FileName);
            }

        }

        /*
         * doneButton_Click
         *
         * This function is pretty self explanatory... it basically just closed the form.
         *
         * object send: Not sure what this is, it was auto populated by forms.
         * …
         * EventArgs e: I believe this is for error checking, but again it was auto populated.
         *
         * Return Value: Void.
         */
        private void doneButton_Click(object sender, EventArgs e)
        {
            Textures.ActiveForm.Close();
        }
    }
}
