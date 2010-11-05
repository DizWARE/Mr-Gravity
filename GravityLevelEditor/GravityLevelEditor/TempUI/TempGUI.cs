using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using GravityLevelEditor.GuiTools;

namespace GravityLevelEditor
{
    public partial class TempGUI : Form
    {
        EditorData mData;

        private static GuiTools.SingleSelect TOOL_SELECT = new GuiTools.SingleSelect();
        private static GuiTools.MultiSelect TOOL_MULTISELECT = new GuiTools.MultiSelect();
        private static GuiTools.AddEntity TOOL_ADD = new GuiTools.AddEntity();
        private static GuiTools.RemoveEntity TOOL_REMOVE = new GuiTools.RemoveEntity();

        ITool mCurrentTool = TOOL_SELECT;

        /*
         * TempGUI
         * 
         * Creates a Windows form that functions as a level editor.
         */
        public TempGUI()
        {
            InitializeComponent();
            mData = new EditorData(new ArrayList(), null, 
                new Level("New Level", new Point(10, 10), Color.Red,
                     Image.FromFile("..\\..\\..\\..\\GravityLevelEditor\\GravityLevelEditor\\Content\\defaultBG.png")));

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor,
            true);

            this.DoubleBuffered = true;
            
            time_updater.Start();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
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
            mData.Level.Name = tb_name.Text;

            mData.Level.Resize(int.Parse(tb_rows.Text), 
                          int.Parse(tb_cols.Text));

            Point pixelSize = GridSpace.GetPixelCoord(mData.Level.Size);
            sc_Properties.Panel1.AutoScrollMinSize = new Size(pixelSize);
            sc_Properties.Panel1.Refresh();
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
            Pen pen = new Pen(Brushes.Gray);
            Point p1, p2;
            Point offset = new Point(p.DisplayRectangle.X, p.DisplayRectangle.Y);

            mData.Level.Draw(g, offset);


            for (int i = 0; i <= mData.Level.Size.X; i++)
            {
                p1 = GridSpace.GetPixelCoord(new Point(i, 0));
                p2 = GridSpace.GetPixelCoord(new Point(i, mData.Level.Size.Y));
                g.DrawLine(pen, new Point(p1.X + offset.X, p1.Y + offset.Y),
                    new Point(p2.X + offset.X, p2.Y + offset.Y));
            }

            for (int i = 0; i <= mData.Level.Size.Y; i++)
            {
                p1 = GridSpace.GetPixelCoord(new Point(0, i));
                p2 = GridSpace.GetPixelCoord(new Point(mData.Level.Size.X, i));
                g.DrawLine(pen, new Point(p1.X + offset.X, p1.Y + offset.Y),
                    new Point(p2.X + offset.X, p2.Y + offset.Y));
            }
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
                    mData.Level.Save();

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
            mData.Level.Save();
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
                mData.Level.Save();

            if (result != DialogResult.Cancel)
            {
                mData.Level = new Level("New Level", new Point(10, 10), Color.Red,
                         Image.FromFile("..\\..\\..\\..\\GravityLevelEditor\\GravityLevelEditor\\Content\\defaultBG.png"));
                mData.SelectedEntities.Clear();
                tb_cols.Text = tb_rows.Text = "10";
                tb_name.Text = "New Level";
                sc_Properties.Panel1.Invalidate();
                Point pixelSize = GridSpace.GetPixelCoord(mData.Level.Size);
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
                mData.Level.Save();

            if (result != DialogResult.Cancel)
            {
                //TODO: Add pointer to Xml load code in Level.cs
                mData.SelectedEntities.Clear();
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
            mData.Level.Save();

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
            mData.Level.Undo();
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
            mData.Level.Redo();
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
            mData.Level.Cut(mData.SelectedEntities);
            mData.SelectedEntities.Clear();
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
            mData.Level.Copy(mData.SelectedEntities);
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
            mData.Level.Paste();
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

        private Point MousePosToGrid(Panel p, MouseEventArgs e)
        {
            return GridSpace.GetGridCoord(new Point(-p.DisplayRectangle.X + e.X,
                             -p.DisplayRectangle.Y + e.Y));
        }

        /*
         * GridMouseDown
         * 
         * Invoked when user presses the left or right mouse buttons on the grid. Calls its
         * respective method in the current tool
         * 
         * Standard Mouse Event parameters
         */
        private void GridMouseDown(object sender, MouseEventArgs e)
        {
            Panel p = (Panel)sender;
            if (e.Button == MouseButtons.Left)
                mCurrentTool.LeftMouseDown(ref mData, MousePosToGrid(p, e));
            else if(e.Button == MouseButtons.Right)
                mCurrentTool.RightMouseDown(ref mData, MousePosToGrid(p, e));
        }

        /*
         * GridMouseUp
         * 
         * Invoked when user releases the left or right mouse buttons on the grid. Calls its
         * respective method in the current tool
         * 
         * Standard Mouse Event parameters
         */
        private void GridMouseUp(object sender, MouseEventArgs e)
        {
            Panel p = (Panel)sender;
            if (e.Button == MouseButtons.Left)
                mCurrentTool.LeftMouseUp(ref mData, MousePosToGrid(p, e));
            else if (e.Button == MouseButtons.Right)
                mCurrentTool.RightMouseUp(ref mData, MousePosToGrid(p, e));
        }

        /*
         * GridMouseDown
         * 
         * Invoked when user moves the mouse on the grid. Calls its
         * respective method in the current tool
         * 
         * Standard Mouse Event parameters
         */
        private void GridMouseMove(object sender, MouseEventArgs e)
        {
            Panel p = (Panel)sender;
            mCurrentTool.MouseMove(ref mData, MousePosToGrid(p, e));
        }

        /*
         * SelectTool
         * 
         * Switches the current tool to select
         * 
         * Normal Event parameters
         */
        private void SelectTool(object sender, EventArgs e)
        {
            mCurrentTool = TOOL_SELECT;
        }


        /*
         * MulitSelectTool
         * 
         * Switches the current tool to multiselect
         * 
         * Normal Event parameters
         */
        private void MultiSelectTool(object sender, EventArgs e)
        {
            mCurrentTool = TOOL_MULTISELECT;
        }


        /*
         * AddTool
         * 
         * Switches the current tool to add entity
         * 
         * Normal Event parameters
         */
        private void AddTool(object sender, EventArgs e)
        {
            mCurrentTool = TOOL_ADD;
        }


        /*
         * RemoveTool
         * 
         * Switches the current tool to remove entity
         * 
         * Normal Event parameters
         */
        private void RemoveTool(object sender, EventArgs e)
        {
            mCurrentTool = TOOL_REMOVE;
        }

        private void UpdateGraphics(object sender, EventArgs e)
        {
            sc_Properties.Panel1.Refresh();
        }
    }
}
