using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace GravityLevelEditor
{
    public partial class TempGUI : Form
    {
        Level mLevel;
        ArrayList mSelectedEntites;

        /*
         * TempGUI
         * 
         * Creates a Windows form that functions as a level editor.
         */
        public TempGUI()
        {
            InitializeComponent();
            mLevel = new Level("New Level", new Point(10, 10), Color.Red,
                     Image.FromFile("..\\..\\..\\..\\GravityLevelEditor\\GravityLevelEditor\\Content\\defaultBG.png"));
            mSelectedEntites = new ArrayList();
               
        }

        /*
         * ApplyChanges
         * 
         * Change the level properties to what is reflected in
         * the 'level properties' control boxes.
         * 
         * object sender: the apply button.
         * 
         * EventArgs e: arguments of clicking the apply button.
         */
        private void ApplyChanges(object sender, EventArgs e)
        {
            mLevel.Name = tb_name.Text;

            mLevel.Resize(int.Parse(tb_rows.Text), 
                          int.Parse(tb_cols.Text));

            Point pixelSize = GridSpace.GetPixelCoord(mLevel.Size);
            sc_Properties.Panel1.AutoScrollMinSize = new Size(pixelSize);
        }

        /*
         * ValidateSizeTextbox
         * 
         * Check to make sure only numbers are being input on each
         * of the level size textboxes.
         * 
         * object sender: the textbox we are checking.
         * 
         * EventArgs e: arguments of this event.
         */
        private void ValidateSizeTextbox(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string final = "";
            foreach (char c in tb.Text)
                if (char.IsDigit(c))
                    final += c;
            tb.Text = final;
        }

        /*
         * GridSpaceClick
         * 
         * Figure out which grid location we have clicked on.
         * 
         * object sender: the level panel.
         * 
         * MouseEventArgs e: status of the mouse.
         */
        private void GridSpaceClick(object sender, MouseEventArgs e)
        {
            Panel p = (Panel)sender;

            Point mouseLocation = new Point(-p.DisplayRectangle.X + e.X,
                                            -p.DisplayRectangle.Y + e.Y);
            MessageBox.Show(GridSpace.GetGridCoord(mouseLocation).ToString());
        }

        /*
         * GridPaint
         * 
         * Draw method for the level panel. Draws the background image and
         * each entity we have placed on the level. Also draws grid lines
         * for ease of seeing our tiles.
         * 
         * object sender: the level panel.
         * 
         * PaintEventArgs e: paint event arguments.
         */
        private void GridPaint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            Graphics g = e.Graphics;
            g.ResetClip();
            Pen pen = new Pen(Brushes.Gray);

            mLevel.Draw(g);

            for (int i = 1; i <= mLevel.Size.X; i++)
                g.DrawLine(pen, GridSpace.GetPixelCoord(new Point(i, 0)),
                    GridSpace.GetPixelCoord(new Point(i, mLevel.Size.Y)));

            for (int i = 1; i <= mLevel.Size.Y; i++)
                g.DrawLine(pen, GridSpace.GetPixelCoord(new Point(0, i)),
                    GridSpace.GetPixelCoord(new Point(mLevel.Size.X,i)));
        }

        /*
         * UpdatePaint
         * 
         * Invalidate the level panel whenever we scroll it, forcing it
         * to draw again.
         * 
         * object sender: the level panel.
         * 
         * ScrollEventArgs e: scroll event arguments.
         */
        private void UpdatePaint(object sender, ScrollEventArgs e)
        {
            sc_Properties.Panel1.Invalidate();
        }

        /*
         * Quit
         * 
         * Close the level editor, but ask the user if he would like
         * to first save the level, and do so if yes.
         * 
         * object sender: the quit level menu option.
         * 
         * EventArgs e: menu event arguments.
         */
        private void Quit(object sender, EventArgs e)
        {
            DialogResult result;
            try
            {
                FormClosingEventArgs f = (FormClosingEventArgs)e;
                result = MessageBox.Show("Do you want to save?", "Quit", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                    mLevel.Save();

                if (result == DialogResult.Cancel)
                    f.Cancel = true;
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /*
         * Save
         * 
         * Save the level under the Levels folder using the
         * level name as the filename.
         * 
         * object sender: the save level menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Save(object sender, EventArgs e)
        {
            mLevel.Save();
        }

        /*
         * New
         * 
         * Create a new level, but first ask the user if he
         * would like to save the current working level and
         * do so if yes.
         * 
         * object sender: the new level menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void New(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to save?", "New", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                mLevel.Save();

            if (result != DialogResult.Cancel)
            {
                mLevel = new Level("New Level", new Point(10, 10), Color.Red,
                         Image.FromFile("..\\..\\..\\..\\GravityLevelEditor\\GravityLevelEditor\\Content\\defaultBG.png"));
                mSelectedEntites.Clear();
                tb_cols.Text = tb_rows.Text = "10";
                tb_name.Text = "New Level";
                sc_Properties.Panel1.Invalidate();
                Point pixelSize = GridSpace.GetPixelCoord(mLevel.Size);
                sc_Properties.Panel1.AutoScrollMinSize = new Size(pixelSize);
            }
        }

        /*
         * Open
         * 
         * Open a previously created level but fist ask the user
         * if he would like to save the level and do so if yes.
         * 
         * object sender: the open level menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Open(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to save?", "Open", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                mLevel.Save();

            if (result != DialogResult.Cancel)
            {
                //TODO: Add pointer to Xml load code in Level.cs
                mSelectedEntites.Clear();
            }
        }

        /*
         * Play
         * 
         * Test play the current working level, but first force a save
         * just in case.
         * 
         * object sender: the play level menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Play(object sender, EventArgs e)
        {
            mLevel.Save();

            //TODO: Add code to launch game with sample level as a parameter
        }

        /*
         * Undo
         * 
         * Tell the level to undo the last IOperation.
         * 
         * object sender: the undo menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Undo(object sender, EventArgs e)
        {
            mLevel.Undo();
        }

        /*
         * Redo
         * 
         * Tell the level to redo the last IOperation.
         * 
         * object sender: the redo menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Redo(object sender, EventArgs e)
        {
            mLevel.Redo();
        }

        /*
         * Cut
         * 
         * Tell the level to cut the currently selected entities.
         * 
         * object sender: the cut menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Cut(object sender, EventArgs e)
        {
            mLevel.Cut(mSelectedEntites);
        }

        /*
         * Copy
         * 
         * Tell the level to copy the currently selected entities.
         * 
         * object sender: the copy menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Copy(object sender, EventArgs e)
        {
            mLevel.Copy(mSelectedEntites);
        }

        /*
         * Paste
         * 
         * Tell the level to paste what is on the clipboard.
         * 
         * object sender: the paste menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void Paste(object sender, EventArgs e)
        {
            mLevel.Paste();
        }

        /*
         * ZoomIn
         * 
         * Change the viewscope of the working level to be bigger.
         * 
         * object sender: the zoom in menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void ZoomIn(object sender, EventArgs e)
        {
            //TODO: Add zoom in code
        }

        /*
         * ZoomOut
         * 
         * Change the viewscope of the working level to be smaller.
         * 
         * object sender: the zoom out menu option.
         * 
         * EventArgs e: menu event arguments. 
         */
        private void ZoomOut(object sender, EventArgs e)
        {
            //TODO: Add zoom out code
        }
    }
}
