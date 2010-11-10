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
using GravityLevelEditor.EntityCreationForm;

namespace GravityLevelEditor
{
    public partial class MainForm : Form
    {
        EditorData mData;

        /**Editor tools**/
        private static GuiTools.SingleSelect TOOL_SELECT = new GuiTools.SingleSelect();
        private static GuiTools.MultiSelect TOOL_MULTISELECT = new GuiTools.MultiSelect();
        private static GuiTools.AddEntity TOOL_ADD = new GuiTools.AddEntity();
        private static GuiTools.RemoveEntity TOOL_REMOVE = new GuiTools.RemoveEntity();
        private static GuiTools.PaintEntity TOOL_PAINT = new GuiTools.PaintEntity();
        private static GuiTools.DepaintEntity TOOL_DEPAINT = new GuiTools.DepaintEntity();

        ITool mCurrentTool = TOOL_SELECT;
        bool didScroll = true;

        /*
         * TempGUI
         * 
         * Creates a Windows form that functions as a level editor.
         */
        public MainForm()
        {
            InitializeComponent();
            mData = new EditorData(new ArrayList(), null, 
                new Level("New Level", new Point(10, 10), Color.Red,
                     Image.FromFile("..\\..\\..\\..\\GravityLevelEditor\\GravityLevelEditor\\Content\\defaultBG.png")));

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

            Point pixelSize = GridSpace.GetDrawingCoord(mData.Level.Size);
            if(pb_bg.Image != null)
                mData.Level.Background = pb_bg.Image;

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

            Pen pen2 = new Pen(Color.FromArgb(55, Color.Blue));
            
            foreach(Entity selectedEntity in mData.SelectedEntities)
                g.FillRectangle(pen2.Brush,GridSpace.GetDrawingRegion(selectedEntity.Location,offset));

            if (TOOL_MULTISELECT.Selecting)
            {

                Pen pen3 = new Pen(Color.FromArgb(100, Color.Black));
                pen3.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                pen3.Width = 4;

                Point initial = TOOL_MULTISELECT.Initial;
                Point current = TOOL_MULTISELECT.Previous;

                Point topLeft = GridSpace.GetDrawingCoord(new Point(Math.Min(initial.X, current.X), Math.Min(initial.Y, current.Y)));
                Point bottomRight = GridSpace.GetDrawingCoord(new Point(Math.Max(initial.X, current.X) + 1, Math.Max(initial.Y, current.Y) + 1));

                g.DrawRectangle(pen3,
                    new Rectangle(topLeft, new Size(Point.Subtract(bottomRight, new Size(topLeft)))));
            }

            for (int i = 0; i <= mData.Level.Size.X; i++)
            {
                p1 = GridSpace.GetDrawingCoord(new Point(i, 0));
                p2 = GridSpace.GetDrawingCoord(new Point(i, mData.Level.Size.Y));
                g.DrawLine(pen, new Point(p1.X + offset.X, p1.Y + offset.Y),
                    new Point(p2.X + offset.X, p2.Y + offset.Y));
            }

            for (int i = 0; i <= mData.Level.Size.Y; i++)
            {
                p1 = GridSpace.GetDrawingCoord(new Point(0, i));
                p2 = GridSpace.GetDrawingCoord(new Point(mData.Level.Size.X, i));
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

            sc_Properties.Panel1.Refresh();
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

            sc_Properties.Panel1.Refresh();
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

            //TODO: Add code to launch game with sample level as a parameter.
            //FORNOW: Open a dialog to show play functionality is not working.
            MessageBox.Show("Coming Soon!");
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
            sc_Properties.Panel1.Refresh();
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
            sc_Properties.Panel1.Refresh();
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
            sc_Properties.Panel1.Refresh();
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
            mData.SelectedEntities.Clear();

            ArrayList entities = mData.Level.Paste();

            foreach (Entity entity in entities)
                mData.SelectedEntities.Add(entity);

            mData.Level.Copy(mData.SelectedEntities);
            sc_Properties.Panel1.Refresh();
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
            GridSpace.ZoomIn();
            Point pixelSize = GridSpace.GetDrawingCoord(mData.Level.Size);
            sc_Properties.Panel1.AutoScrollMinSize = new Size(pixelSize);
            sc_Properties.Panel1.Refresh();
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
            GridSpace.ZoomOut();
            Point pixelSize = GridSpace.GetDrawingCoord(mData.Level.Size);
            sc_Properties.Panel1.AutoScrollMinSize = new Size(pixelSize);
            sc_Properties.Panel1.Refresh();
        }

        /*
         * MousePosToGrid
         * 
         * Gets the converted version of the mouse position
         */
        private Point MousePosToGrid(Panel p, MouseEventArgs e)
        {
            return GridSpace.GetScaledGridCoord(new Point(-p.DisplayRectangle.X + e.X,
                             -p.DisplayRectangle.Y + e.Y));
        }

        /*
         * MousePosToGrid
         * 
         * Gets the converted version of the mouse position
         */
        private Point MousePosToGrid(Panel p, Point mousePos)
        {
            return GridSpace.GetScaledGridCoord(new Point(-p.DisplayRectangle.X + mousePos.X,
                             -p.DisplayRectangle.Y + mousePos.Y));
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
            else if (e.Button == MouseButtons.Right)
                mCurrentTool.RightMouseDown(ref mData, MousePosToGrid(p, e));
            sc_Properties.Panel1.Refresh();
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
            sc_Properties.Panel1.Refresh();
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
            Point mousePos = MousePosToGrid(p, e);

            //if (!VerifyEntityLocation()) return; Work in progress
            if ((mousePos.X >= mData.Level.Size.X || mousePos.Y >= mData.Level.Size.Y ||
               mousePos.X < 0 || mousePos.Y < 0))
            { tslbl_gridLoc.Text = "Out of Grid Bounds"; return; }

            mCurrentTool.MouseMove(ref mData, p, mousePos);

            tslbl_gridLoc.Text = mousePos.ToString();
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

        /*
         * PaintTool
         * 
         * Switches the current tool to paint
         * 
         * Normal Event parameters
         */
        private void PaintTool(object sender, EventArgs e)
        {
            mCurrentTool = TOOL_PAINT;
        }

        /*
         * DepaintTool
         * 
         * Switches the current tool to depaint
         * 
         * Normal Event parameters
         */
        private void DepaintTool(object sender, EventArgs e)
        {
            mCurrentTool = TOOL_DEPAINT;
        }

        private void ChangeBackground(object sender, EventArgs e)
        {
            ImportForm imageSelectDialog = new ImportForm();
            if(imageSelectDialog.ShowDialog() == DialogResult.OK)
            {
                pb_bg.Image = imageSelectDialog.SelectedImage;
                if (pb_bg.Image == null) return;

                float ratioWidth = (float)pb_bg.Image.Size.Height / pb_bg.Image.Width;
                pb_bg.Width = 75;
                pb_bg.Height = (int)(pb_bg.Width * ratioWidth);

                int xLoc = sc_HorizontalProperties.Panel2.Width / 2 - pb_bg.Width / 2;
                pb_bg.Location = new Point(xLoc,pb_bg.Location.Y);
            }
        }

        private void ChangeEntity(object sender, EventArgs e)
        {
            CreateEntity entityDialog = new CreateEntity();
            if (entityDialog.ShowDialog() == DialogResult.OK)
            {
                mData.OnDeck = entityDialog.SelectedEntity;
                if(mData.OnDeck == null) return;
                pb_Entity.Image = mData.OnDeck.Texture;
                lbl_entityName.Text = mData.OnDeck.Name;
                lbl_entityType.Text = mData.OnDeck.Type;
                cb_entityPaintable.Checked = mData.OnDeck.Paintable;
                cb_entityVisible.Checked = mData.OnDeck.Visible;
            }
        }

        private bool VerifyEntityLocation()
        {
            if (mData.SelectedEntities.Count == 0) return true;
            Point maxPoint = ((Entity)mData.SelectedEntities[0]).Location;
            Point minPoint = maxPoint;
            foreach (Entity entity in mData.SelectedEntities)
            {
                maxPoint.X = Math.Max(maxPoint.X, entity.Location.X);
                maxPoint.Y = Math.Max(maxPoint.Y, entity.Location.Y);
                minPoint.X = Math.Min(minPoint.X, entity.Location.X);
                minPoint.Y = Math.Min(minPoint.Y, entity.Location.Y);
            }
            if (maxPoint.X > mData.Level.Size.X || maxPoint.Y > mData.Level.Size.Y ||
               minPoint.X < 0 || minPoint.Y < 0)
                 return false;

            return true;
        }

        private void GridScroll(object sender, ScrollEventArgs e)
        {
            didScroll = true;
        }
    }
}
