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
using System.IO;
using System.Diagnostics;

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

        //Backbuffer stuff
        private Bitmap offScreenBmp;
        private Graphics offScreenDC;

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

            this.SetStyle(ControlStyles.DoubleBuffer |
                           ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer,
                          true);

            this.DoubleBuffered = true;

            PrepareBackbuffer();

            // Code for keyboard input -JRH
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(MainForm_KeyPress);

            time_updater.Start();
        }
        
        /*
         * CreateParams
         * 
         * More flicker fixes. This one does help quite a bit
         */
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
         * PrepareBackbuffer
         * 
         * Prepares the backbuffer for drawing
         */
        private void PrepareBackbuffer()
        {
            Panel p = sc_Properties.Panel1;
            Size gridSize = new Size(GridSpace.GetDrawingCoord(mData.Level.Size));
            offScreenBmp = new Bitmap(Math.Min(gridSize.Width,p.DisplayRectangle.Width), 
                Math.Min(gridSize.Height,p.DisplayRectangle.Height+100));
            offScreenDC = Graphics.FromImage(offScreenBmp);
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
            //Apply level changes
            mData.Level.Name = tb_name.Text;
            mData.Level.Resize(int.Parse(tb_rows.Text), 
                          int.Parse(tb_cols.Text));

            if(pb_bg.Image != null)
                mData.Level.Background = pb_bg.Image;

            UpdatePanel();
        }

        /*
         * UpdatePanel
         * 
         * Prepare Backbuffer and resize the panels
         */
        private void UpdatePanel()
        {
            PrepareBackbuffer();

            //Update sceen
            Point pixelSize = GridSpace.GetDrawingCoord(mData.Level.Size);
            sc_Properties.Panel1.AutoScrollMinSize = new Size(pixelSize);
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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
            offScreenDC.Clear(Color.Ivory);

            mData.Level.Draw(offScreenDC, offset);

            Pen pen2 = new Pen(Color.FromArgb(55, Color.Blue));
            
            foreach(Entity selectedEntity in mData.SelectedEntities)
                offScreenDC.FillRectangle(pen2.Brush,GridSpace.GetDrawingRegion(selectedEntity.Location,offset));

            if (TOOL_MULTISELECT.Selecting)
            {

                Pen pen3 = new Pen(Color.FromArgb(100, Color.Black));
                pen3.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                pen3.Width = 4;

                Point initial = TOOL_MULTISELECT.Initial;
                Point current = TOOL_MULTISELECT.Previous;

                Point topLeft = GridSpace.GetDrawingCoord(new Point(Math.Min(initial.X, current.X), Math.Min(initial.Y, current.Y)));
                Point bottomRight = GridSpace.GetDrawingCoord(new Point(Math.Max(initial.X, current.X) + 1, Math.Max(initial.Y, current.Y) + 1));
                Rectangle rect = new Rectangle(topLeft, new Size(Point.Subtract(bottomRight, new Size(topLeft))));

                rect.X += offset.X;
                rect.Y += offset.Y;

                offScreenDC.DrawRectangle(pen3, rect);
            }

            //Draw rows
            for (int i = 0; i <= mData.Level.Size.X; i++)
            {
                p1 = GridSpace.GetDrawingCoord(new Point(i, 0));
                p2 = GridSpace.GetDrawingCoord(new Point(i, mData.Level.Size.Y));
                offScreenDC.DrawLine(pen, new Point(p1.X + offset.X, p1.Y + offset.Y),
                    new Point(p2.X + offset.X, p2.Y + offset.Y));
            }

            //Draw columns
            for (int i = 0; i <= mData.Level.Size.Y; i++)
            {
                p1 = GridSpace.GetDrawingCoord(new Point(0, i));
                p2 = GridSpace.GetDrawingCoord(new Point(mData.Level.Size.X, i));
                offScreenDC.DrawLine(pen, new Point(p1.X + offset.X, p1.Y + offset.Y),
                    new Point(p2.X + offset.X, p2.Y + offset.Y));
            }

            g.DrawImage(offScreenBmp, 0, 0);
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

            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "XML Files|*.xml";
                dialog.Title = "Select a level File";
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());

                di = di.Parent.Parent.Parent.Parent;
                dialog.InitialDirectory = di.FullName + "\\WindowsGame1\\Content\\Levels";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    mData.SelectedEntities.Clear();
                    mData.Level = new Level(dialog.FileName);
                    UpdatePanel();

                    tb_cols.Text = mData.Level.Size.X.ToString();
                    tb_rows.Text = mData.Level.Size.Y.ToString();

                    tb_name.Text = mData.Level.Name;

                    UpdateBackgroundPreview(mData.Level.Background);
                    
                }
            }

            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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

            Process game = new Process();

            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            dir = dir.Parent.Parent;

            game.StartInfo.FileName = dir.FullName + "\\RunGame.bat";
            game.StartInfo.Arguments = "\"" + mData.Level.Name + ".xml\"";
            game.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            game.StartInfo.ErrorDialog = true;
            game.Start();
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
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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
            UpdatePanel();
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
            UpdatePanel();
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
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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

            VerifyEntityLocation();
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
            pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.select;
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
            pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.multiselect;
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
            pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.addentity;
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
            pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.removeentity;
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
            pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.paint;
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
            pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.depaint;
        }

        /*
         * ChangeBackground
         * 
         * Launches an image selection dialog box, and changes the background to the selection
         */
        private void ChangeBackground(object sender, EventArgs e)
        {
            ImportForm imageSelectDialog = new ImportForm();
            if(imageSelectDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateBackgroundPreview(imageSelectDialog.SelectedImage);
            }
        }

        /*
         * Update the background image in the preview box
         */
        private void UpdateBackgroundPreview(Image image)
        {
            pb_bg.Image = image;
            if (pb_bg.Image == null) return;

            float ratioWidth = (float)pb_bg.Image.Size.Height / pb_bg.Image.Width;
            pb_bg.Width = 75;
            pb_bg.Height = (int)(pb_bg.Width * ratioWidth);

            int xLoc = sc_HorizontalProperties.Panel2.Width / 2 - pb_bg.Width / 2;
            pb_bg.Location = new Point(xLoc, pb_bg.Location.Y);
        }

        /*
         * ChangeEntity
         * 
         * Launch the Dialog selection screen
         * 
         * -Typical event parameters
         */
        private void ChangeEntity(object sender, EventArgs e)
        {
            CreateEntity entityDialog = new CreateEntity();

            //If the dialog comes back successfully, change the On-deck etity and update info
            if (entityDialog.ShowDialog() == DialogResult.OK)
            {
                mData.OnDeck = entityDialog.SelectedEntity;
                if(mData.OnDeck == null) return;
                pb_Entity.Image = mData.OnDeck.Texture;
                lbl_entityName.Text = mData.OnDeck.Name;
                lbl_entityType.Text = mData.OnDeck.Type;
                cb_entityPaintable.Checked = mData.OnDeck.Paintable;
                cb_entityHazardous.Checked = mData.OnDeck.Hazardous;
                cb_entityVisible.Checked = mData.OnDeck.Visible;
            }
        }

        /*
         * VerifyEntityLocation
         * 
         * Checks to make sure nothing is out of bounds. If something is, move it back on 
         * the grid
         */
        private void VerifyEntityLocation()
        {
            if (mData.SelectedEntities.Count == 0) return;
            Point maxPoint = ((Entity)mData.SelectedEntities[0]).Location;
            Point minPoint = maxPoint;
            bool invalidate = false;
            foreach (Entity entity in mData.SelectedEntities)
            {
                maxPoint.X = Math.Max(maxPoint.X, entity.Location.X);
                maxPoint.Y = Math.Max(maxPoint.Y, entity.Location.Y);
                minPoint.X = Math.Min(minPoint.X, entity.Location.X);
                minPoint.Y = Math.Min(minPoint.Y, entity.Location.Y);
            }

            if (maxPoint.X >= mData.Level.Size.X)
            {
                foreach (Entity entity in mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X - 1, entity.Location.Y);
                invalidate = true;
            }
            if (maxPoint.Y >= mData.Level.Size.Y)
            {
                foreach (Entity entity in mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X, entity.Location.Y - 1);
                invalidate = true;
            }
            if (minPoint.X < 0)
            {
                foreach (Entity entity in mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X + 1, entity.Location.Y);
                invalidate = true;
            }
            if (minPoint.Y < 0)
            {
                foreach (Entity entity in mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X, entity.Location.Y + 1);
                invalidate = true;
            }
            if(invalidate)
                sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);

            return;
        }

        /*
         * GridScroll
         * 
         * Helps make scrolling as smooth as possible
         * 
         * -Typical scroll event parameters
         */
        private void GridScroll(object sender, ScrollEventArgs e)
        {
            sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
        }

        /*
         * MainForm_KeyPress
         * 
         * Handle keypresses
         * 
         * Default 
         */
        void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                switch (e.KeyChar)
                {
                    //ctrl-s
                    case (char)019:
                        mi_save.PerformClick();
                        break;

                    //ctrl-c
                    case (char)003:
                        mi_copy.PerformClick();
                        break;

                    //ctrl-x
                    case (char)024:
                        mi_cut.PerformClick();
                        break;

                    //ctrl-v
                    case (char)022:
                        mi_paste.PerformClick();
                        break;

                    //ctrl-z
                    case (char)026:
                        mi_undo.PerformClick();
                        break;

                    //ctrl-y
                    case (char)025:
                        mi_redo.PerformClick();
                        break;

                    //ctrl-n
                    case (char)014:
                        mi_new.PerformClick();
                        break;

                    //ctrl-o
                    case (char)015:
                        mi_open.PerformClick();
                        break;

                    //ctrl-p
                    case (char)016:
                        mi_play.PerformClick();
                        break;

                    //ctrl-q
                    case (char)017:
                        mi_quit.PerformClick();
                        break;

                    default:
                        break;
                }
            }

            switch (e.KeyChar)
            {
                //zoom in
                case '=':
                case '+':
                    mi_zoomIn.PerformClick();
                    break;
                
                //zoom out
                case '-':
                case '_':
                    mi_zoomOut.PerformClick();
                    break;

                //s, S, or 1 = select tool
                case 's':
                case 'S':
                case '1':;
                    mCurrentTool = TOOL_SELECT;
                    pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.select;
                    break;

                //m, M or 2 = multi-select tool
                case 'm':
                case 'M':
                case '2':
                    mCurrentTool = TOOL_MULTISELECT;
                    pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.multiselect;
                    break;

                //a, A, or 3 = add entity
                case 'a':
                case 'A':
                case '3':
                    mCurrentTool = TOOL_ADD;
                    pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.addentity;
                    break;

                //r, R, or 4 = remove entity
                case 'r':
                case 'R':
                case '4':
                    mCurrentTool = TOOL_REMOVE;
                    pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.removeentity;
                    break;

                //p, P, or 5 = paint entity
                case 'p':
                case 'P':
                case '5':
                    mCurrentTool = TOOL_PAINT;
                    pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.paint;
                    break;

                //d, D, or 6 = depaint entity
                case 'd':
                case 'D':
                case '6':
                    mCurrentTool = TOOL_DEPAINT;
                    pb_CurrentTool.Image = global::GravityLevelEditor.Properties.Resources.depaint;
                    break;

                default:
                    break;
            }
            mData.CTRLHeld = false;
        }

        /*
         * Form_KeyDown
         * 
         * Handles the key down event. In this case, links the delete key and ctrl key to some actions
         */
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                TOOL_REMOVE.LeftMouseUp(ref mData, new Point(-10, -10));
                sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
            }
            
            //TODO - Add a CTRL Click
            if (e.Modifiers == Keys.Control)
                mData.CTRLHeld = true;
        }

        /*
         * Form_KeyUp
         * 
         * Handles the key up event. In this case, links the ctrl key to some actions
         */
        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.ControlKey)
                mData.CTRLHeld = false;
        }
    }
}
