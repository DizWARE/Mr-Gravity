using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MrGravity.LevelEditor.EntityCreationForm;
using MrGravity.LevelEditor.GuiTools;
using MrGravity.LevelEditor.Properties;

namespace MrGravity.LevelEditor
{
    public sealed partial class MainForm : Form
    {
        private EditorData _mData;

        /**Editor tools**/
        private static readonly SingleSelect ToolSelect = new SingleSelect();
        private static readonly MultiSelect ToolMultiselect = new MultiSelect();
        private static readonly AddEntity ToolAdd = new AddEntity();
        private static readonly RemoveEntity ToolRemove = new RemoveEntity();
        private static readonly PaintEntity ToolPaint = new PaintEntity();
        private static readonly DepaintEntity ToolDepaint = new DepaintEntity();

        private ITool _mCurrentTool = ToolSelect;

        //Backbuffer stuff
        private Bitmap _offScreenBmp;
        private Graphics _offScreenDc;

        /*
         * TempGUI
         * 
         * Creates a Windows form that functions as a level editor.
         */
        public MainForm()
        {
            InitializeComponent();

            var background = Image.FromFile("..\\..\\..\\..\\MrGravity\\Content\\Images\\Backgrounds\\blank.png");
            background.Tag = "Backgrounds\\blank";
            _mData = new EditorData(new ArrayList(), null, 
                new Level("New Level", new Point(10, 10),
                     background));

            SetStyle(ControlStyles.DoubleBuffer |
                           ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer,
                          true);

            pb_bg.Image = _mData.Level.Background;

            DoubleBuffered = true;

            PrepareBackbuffer();

            // Code for keyboard input -JRH
            KeyPreview = true;
            KeyPress += MainForm_KeyPress;

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
                var cp = base.CreateParams;
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
            var gridSize = new Size(GridSpace.GetDrawingCoord(_mData.Level.Size));
            _offScreenBmp = new Bitmap(Math.Min(gridSize.Width,p.DisplayRectangle.Width+100), 
                Math.Min(gridSize.Height,p.DisplayRectangle.Height+100));
            _offScreenDc = Graphics.FromImage(_offScreenBmp);
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
            _mData.Level.Name = tb_name.Text;
            _mData.Level.Resize(int.Parse(tb_rows.Text), 
                          int.Parse(tb_cols.Text));

            if(pb_bg.Image != null)
                _mData.Level.Background = pb_bg.Image;

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
            var pixelSize = GridSpace.GetDrawingCoord(_mData.Level.Size);
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
            var tb = (TextBox)sender;
            var final = tb.Text.Where(char.IsDigit).Aggregate("", (current, c) => current + c);
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
            var p = (Panel)sender;
            var g = e.Graphics;
            var pen = new Pen(Brushes.Gray);
            Point p1, p2;
            var offset = new Point(p.DisplayRectangle.X, p.DisplayRectangle.Y);
            _offScreenDc.Clear(Color.Ivory);

            _mData.Level.Draw(_offScreenDc, offset);

            var pen2 = new Pen(Color.FromArgb(55, Color.Blue));
            
            foreach(Entity selectedEntity in _mData.SelectedEntities)
                _offScreenDc.FillRectangle(pen2.Brush,GridSpace.GetDrawingRegion(selectedEntity.Location,offset));

            if (ToolMultiselect.Selecting)
            {

                var pen3 = new Pen(Color.FromArgb(100, Color.Black));
                pen3.DashStyle = DashStyle.Dash;
                pen3.Width = 4;

                var initial = ToolMultiselect.Initial;
                var current = ToolMultiselect.Previous;

                var topLeft = GridSpace.GetDrawingCoord(new Point(Math.Min(initial.X, current.X), Math.Min(initial.Y, current.Y)));
                var bottomRight = GridSpace.GetDrawingCoord(new Point(Math.Max(initial.X, current.X) + 1, Math.Max(initial.Y, current.Y) + 1));
                var rect = new Rectangle(topLeft, new Size(Point.Subtract(bottomRight, new Size(topLeft))));

                rect.X += offset.X;
                rect.Y += offset.Y;

                _offScreenDc.DrawRectangle(pen3, rect);
            }

            //Draw rows
            for (var i = 0; i <= _mData.Level.Size.X; i++)
            {
                p1 = GridSpace.GetDrawingCoord(new Point(i, 0));
                p2 = GridSpace.GetDrawingCoord(new Point(i, _mData.Level.Size.Y));
                _offScreenDc.DrawLine(pen, new Point(p1.X + offset.X, p1.Y + offset.Y),
                    new Point(p2.X + offset.X, p2.Y + offset.Y));
            }

            //Draw columns
            for (var i = 0; i <= _mData.Level.Size.Y; i++)
            {
                p1 = GridSpace.GetDrawingCoord(new Point(0, i));
                p2 = GridSpace.GetDrawingCoord(new Point(_mData.Level.Size.X, i));
                _offScreenDc.DrawLine(pen, new Point(p1.X + offset.X, p1.Y + offset.Y),
                    new Point(p2.X + offset.X, p2.Y + offset.Y));
            }

            g.DrawImage(_offScreenBmp, 0, 0);
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
                var f = (FormClosingEventArgs)e;
                result = MessageBox.Show(@"Do you want to save?", @"Quit", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                    _mData.Level.Save();

                if (result == DialogResult.Cancel)
                    f.Cancel = true;
            }
            catch
            {
                Close();
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
            _mData.Level.Save();
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
            var result = MessageBox.Show(@"Do you want to save?", @"New", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                _mData.Level.Save();

            if (result != DialogResult.Cancel)
            {

                var background = Image.FromFile(@"..\..\..\..\MrGravity\\Content\\Images\\Backgrounds\\blank.png");
                background.Tag = @"Backgrounds\blank";
                _mData.Level = new Level(@"New Level", new Point(10, 10),
                         background);
                pb_bg.Image = background;
                _mData.SelectedEntities.Clear();
                tb_cols.Text = tb_rows.Text = @"10";
                tb_name.Text = @"New Level";
                sc_Properties.Panel1.Invalidate();
                var pixelSize = GridSpace.GetPixelCoord(_mData.Level.Size);
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
            var result = MessageBox.Show(@"Do you want to save?", @"Open", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                _mData.Level.Save();

            if (result != DialogResult.Cancel)
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = @"XML Files|*.xml";
                dialog.Title = @"Select a level File";
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                var di = new DirectoryInfo(Directory.GetCurrentDirectory());

                di = di.Parent.Parent.Parent.Parent;
                dialog.InitialDirectory = di.FullName + @"\MrGravity\Content\Levels";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _mData.SelectedEntities.Clear();
                    _mData.Level = new Level(dialog.FileName);
                    UpdatePanel();

                    tb_cols.Text = _mData.Level.Size.X.ToString();
                    tb_rows.Text = _mData.Level.Size.Y.ToString();

                    tb_name.Text = _mData.Level.Name;

                    UpdateBackgroundPreview(_mData.Level.Background);
                    
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
            _mData.Level.Save();

            var game = new Process();

            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            dir = dir.Parent?.Parent;

            game.StartInfo.FileName = dir?.FullName + "\\RunGame.bat";
            game.StartInfo.Arguments = "\"" + _mData.Level.Name + ".xml\"";
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
            _mData.Level.Undo();
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
            _mData.Level.Redo();
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
            _mData.Level.Cut(_mData.SelectedEntities);
            _mData.SelectedEntities.Clear();
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
            _mData.Level.Copy(_mData.SelectedEntities);
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
            _mData.SelectedEntities.Clear();

            var entities = _mData.Level.Paste();

            foreach (Entity entity in entities)
                _mData.SelectedEntities.Add(entity);

            _mData.Level.Copy(_mData.SelectedEntities);
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
            var p = (Panel)sender;
            if (e.Button == MouseButtons.Left)
                _mCurrentTool.LeftMouseDown(ref _mData, MousePosToGrid(p, e));
            else if (e.Button == MouseButtons.Right)
                _mCurrentTool.RightMouseDown(ref _mData, MousePosToGrid(p, e));
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
            var p = (Panel)sender;
            if (e.Button == MouseButtons.Left)
                _mCurrentTool.LeftMouseUp(ref _mData, MousePosToGrid(p, e));
            else if (e.Button == MouseButtons.Right)
                _mCurrentTool.RightMouseUp(ref _mData, MousePosToGrid(p, e));
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
            var p = (Panel)sender;
            var mousePos = MousePosToGrid(p, e);

            VerifyEntityLocation();
            if ((mousePos.X >= _mData.Level.Size.X || mousePos.Y >= _mData.Level.Size.Y ||
               mousePos.X < 0 || mousePos.Y < 0))
            { tslbl_gridLoc.Text = @"Out of Grid Bounds"; return; }

            _mCurrentTool.MouseMove(ref _mData, p, mousePos);

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
            _mCurrentTool = ToolSelect;
            pb_CurrentTool.Image = Resources.select;
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
            _mCurrentTool = ToolMultiselect;
            pb_CurrentTool.Image = Resources.multiselect;
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
            _mCurrentTool = ToolAdd;
            pb_CurrentTool.Image = Resources.addentity;
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
            _mCurrentTool = ToolRemove;
            pb_CurrentTool.Image = Resources.removeentity;
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
            _mCurrentTool = ToolPaint;
            pb_CurrentTool.Image = Resources.paint;
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
            _mCurrentTool = ToolDepaint;
            pb_CurrentTool.Image = Resources.depaint;
        }

        /*
         * ChangeBackground
         * 
         * Launches an image selection dialog box, and changes the background to the selection
         */
        private void ChangeBackground(object sender, EventArgs e)
        {
            var imageSelectDialog = new ImportForm();
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

            var ratioWidth = (float)pb_bg.Image.Size.Height / pb_bg.Image.Width;
            pb_bg.Width = 75;
            pb_bg.Height = (int)(pb_bg.Width * ratioWidth);

            var xLoc = sc_HorizontalProperties.Panel2.Width / 2 - pb_bg.Width / 2;
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
            var entityDialog = new CreateEntity();

            //If the dialog comes back successfully, change the On-deck etity and update info
            if (entityDialog.ShowDialog() == DialogResult.OK)
            {
                _mData.OnDeck = entityDialog.SelectedEntity;
                if(_mData.OnDeck == null) return;
                pb_Entity.Image = _mData.OnDeck.Texture;
                lbl_entityName.Text = _mData.OnDeck.Name;
                lbl_entityType.Text = _mData.OnDeck.Type;
                cb_entityPaintable.Checked = _mData.OnDeck.Paintable;
                lbl_collision.Text = _mData.OnDeck.CollisionType;
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
            if (_mData.SelectedEntities.Count == 0) return;
            var maxPoint = ((Entity)_mData.SelectedEntities[0]).Location;
            var minPoint = maxPoint;
            var invalidate = false;
            foreach (Entity entity in _mData.SelectedEntities)
            {
                maxPoint.X = Math.Max(maxPoint.X, entity.Location.X);
                maxPoint.Y = Math.Max(maxPoint.Y, entity.Location.Y);
                minPoint.X = Math.Min(minPoint.X, entity.Location.X);
                minPoint.Y = Math.Min(minPoint.Y, entity.Location.Y);
            }

            if (maxPoint.X >= _mData.Level.Size.X)
            {
                foreach (Entity entity in _mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X - 1, entity.Location.Y);
                invalidate = true;
            }
            if (maxPoint.Y >= _mData.Level.Size.Y)
            {
                foreach (Entity entity in _mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X, entity.Location.Y - 1);
                invalidate = true;
            }
            if (minPoint.X < 0)
            {
                foreach (Entity entity in _mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X + 1, entity.Location.Y);
                invalidate = true;
            }
            if (minPoint.Y < 0)
            {
                foreach (Entity entity in _mData.SelectedEntities)
                    entity.Location = new Point(entity.Location.X, entity.Location.Y + 1);
                invalidate = true;
            }
            if(invalidate)
                sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
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

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
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
                case '1':
                    _mCurrentTool = ToolSelect;
                    pb_CurrentTool.Image = Resources.select;
                    break;

                //m, M or 2 = multi-select tool
                case 'm':
                case 'M':
                case '2':
                    _mCurrentTool = ToolMultiselect;
                    pb_CurrentTool.Image = Resources.multiselect;
                    break;

                //a, A, or 3 = add entity
                case 'a':
                case 'A':
                case '3':
                    _mCurrentTool = ToolAdd;
                    pb_CurrentTool.Image = Resources.addentity;
                    break;

                //r, R, or 4 = remove entity
                case 'r':
                case 'R':
                case '4':
                    _mCurrentTool = ToolRemove;
                    pb_CurrentTool.Image = Resources.removeentity;
                    break;

                //p, P, or 5 = paint entity
                case 'p':
                case 'P':
                case '5':
                    _mCurrentTool = ToolPaint;
                    pb_CurrentTool.Image = Resources.paint;
                    break;

                //d, D, or 6 = depaint entity
                case 'd':
                case 'D':
                case '6':
                    _mCurrentTool = ToolDepaint;
                    pb_CurrentTool.Image = Resources.depaint;
                    break;
            }
            _mData.CtrlHeld = false;
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
                ToolRemove.LeftMouseUp(ref _mData, new Point(-10, -10));
                sc_Properties.Panel1.Invalidate(sc_Properties.Panel1.DisplayRectangle);
            }
            
            //TODO - Add a CTRL Click
            if (e.Modifiers == Keys.Control)
                _mData.CtrlHeld = true;
        }

        /*
         * Form_KeyUp
         * 
         * Handles the key up event. In this case, links the ctrl key to some actions
         */
        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.ControlKey)
                _mData.CtrlHeld = false;
        }
    }
}
