using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Linq;

namespace GravityLevelEditor.EntityCreationForm
{
    partial class CreateEntity : Form
    {
        private int mSelectedIndex;
        ArrayList mAllEntities;
        public Entity SelectedEntity 
        { 
            get 
            { 
                if(mAllEntities.Count > 0)
                    return (Entity)mAllEntities[mSelectedIndex]; 

                return null;
            } 
        
        }       

        /*
         * CreateEnity
         * 
         * Constructor for this form. Loads all entities from the entities list
         */
        public CreateEntity()
        {
            InitializeComponent();

            mAllEntities = new ArrayList();
            lb_entitySelect.Items.Clear();

            ImportEntityList();
            //lb_entitySelect.ValueMember = "Name";
            //Load Entity List and fill mAllEntities here.
        }

        /*
         * CreateNew
         * 
         * Event for when the Create Entity button is pressed
         * Create a new Entity in the entity list
         */
        private void CreateNew(object sender, EventArgs e)
        {
            mAllEntities.Add(new Entity("", "New", true, true,
                new Dictionary<string, string>(),
                Image.FromFile("..\\..\\Content\\defaultImage.png")));
            mSelectedIndex = mAllEntities.Count - 1;
            lb_entitySelect.Items.Add(mAllEntities[mSelectedIndex]);
            lb_entitySelect.SelectedItem = SelectedEntity;
        }

        /*
         * SelectEntity
         * 
         * Event for when an Entity option is selected
         * 
         * Updates all form data with what is stored in entity
         */
        private void SelectEntity(object sender, EventArgs e)
        {
            if (lb_entitySelect.SelectedIndex < 0 || 
                mAllEntities[lb_entitySelect.SelectedIndex] == null) return;

            mSelectedIndex = lb_entitySelect.SelectedIndex;

            tb_name.Text = SelectedEntity.Name;
            if (SelectedEntity.Type != "")
                cb_type.Text = SelectedEntity.Type;
            else
                cb_type.Text = "Select Type";
            ckb_visible.Checked = SelectedEntity.Visible;
            ckb_paintable.Checked = SelectedEntity.Paintable;
            pb_texture.Image = SelectedEntity.Texture;
        }

        /*
         * DeleteSelected
         * 
         * Removes the selected entity from the Entity list
         */
        private void DeleteSelected(object sender, EventArgs e)
        {
            if (mAllEntities.Count == 0 || SelectedEntity == null) return;
            int index = lb_entitySelect.SelectedIndex;
            lb_entitySelect.Items.Remove(SelectedEntity);
            mAllEntities.Remove(SelectedEntity);

            //Correct the entity selection after the delete
            if (index < lb_entitySelect.Items.Count)
                lb_entitySelect.SelectedIndex = index;
            else
                lb_entitySelect.SelectedIndex = lb_entitySelect.Items.Count - 1;

            mSelectedIndex = lb_entitySelect.SelectedIndex;
        }

        /*
         * AdditionalProperties
         * 
         * Launches the Additional properties dialog when the button is pressed
         */
        private void AdditionalProperties(object sender, EventArgs e)
        {
            if (mAllEntities.Count == 0 || SelectedEntity == null) return;
            AdditionalProperties properties = new AdditionalProperties(SelectedEntity.Properties);
            properties.Location = Point.Add(this.Location, new Size(50, 50));
            if (properties.ShowDialog() == DialogResult.OK)
                SelectedEntity.Properties = properties.Properties;
        }

        /*
         * OK
         * 
         * Handles the click event when the ok button is pressed
         * 
         * Sould export the entity list and send off the data that it was accepted
         */
        private void Ok(object sender, EventArgs e)
        {
            ExportEntityList();

            DialogResult = DialogResult.OK;
            this.Close();
        }

        /*
         * NameChange
         * 
         * Handles changes on the TextBox
         * Changes the Entity name
         */
        private void NameChange(object sender, EventArgs e)
        {
            if(lb_entitySelect.SelectedIndex > -1)
                SelectedEntity.Name = tb_name.Text;
        }

        /*
         * EnterDown
         * 
         * Commits the name change in the text box when user presses enter
         */
        private void EnterDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cb_type.Focus();
                e.SuppressKeyPress = true;
            }
        }

        /*
         * TypeChanged
         * 
         * Handles when the user changes the Type
         */
        private void TypeChanged(object sender, EventArgs e)
        {
            if (lb_entitySelect.SelectedIndex > -1)
            {
                SelectedEntity.Type = cb_type.Text;
                UpdateView();
            }
        }

        /*
         * SetVisible
         * 
         * Sets if this entity will be visible in the game
         */
        private void SetVisible(object sender, EventArgs e)
        {
            if (lb_entitySelect.SelectedIndex > -1)
                SelectedEntity.Visible = ckb_visible.Checked;
        }

        /*
         * SetPaintable
         * 
         * Sets whether this entity is paintable or not
         */
        private void SetPaintable(object sender, EventArgs e)
        {
            if (lb_entitySelect.SelectedIndex > -1)
                SelectedEntity.Paintable = ckb_paintable.Checked;
        }

        /*
         * Rename
         * 
         * Push the name change onto the Entity list when leaving the Text Box
         */
        private void Rename(object sender, EventArgs e)
        {
            UpdateView();
        }

        /*
         * UpdateView
         * 
         * Updates the list view with the text wise changes
         */
        private void UpdateView()
        {
            int index = lb_entitySelect.SelectedIndex;
            if (index < 0) return;
            lb_entitySelect.Items.Insert(index + 1, SelectedEntity);
            lb_entitySelect.Items.RemoveAt(index);
            lb_entitySelect.SelectedIndex = index;
        }

        /*
         * FilterSelected
         * 
         * Filter the Entities that are in view by type
         */
        private void FilterSelected(object sender, EventArgs e)
        {
            lb_entitySelect.Items.Clear();
            
            //If (None) than show all items, Othewise only show of the selected type
            if("(None)".Equals(lb_filter.SelectedItem))
                foreach (Entity entity in mAllEntities)
                    lb_entitySelect.Items.Add(entity);
            else
                foreach (Entity entity in mAllEntities)
                    if(entity.Type.Equals(lb_filter.SelectedItem))
                        lb_entitySelect.Items.Add(entity);

            if (lb_entitySelect.Items.Count > 0)
                lb_entitySelect.SelectedIndex = 0;
            else
                ClearProperties();

        }

        /*
         * ClearProperties
         * 
         * Clears the Form values for the entity properties
         */
        private void ClearProperties()
        {
            lb_entitySelect.SelectedItem = " ";
            tb_name.Text = "";
            cb_type.Text = "Select Type";
            pb_texture.Image = null;
            ckb_paintable.Checked = false;
            ckb_visible.Checked = false;
        }

        private void ChangeImage(object sender, EventArgs e)
        {
            ImportForm artSelector = new ImportForm();
            if (lb_entitySelect.SelectedIndex != -1 &&
                artSelector.ShowDialog() == DialogResult.OK)
            {
                if (artSelector.SelectedImage != null)
                    pb_texture.Image = artSelector.SelectedImage;
                else
                    pb_texture.Image = Image.FromFile("..\\..\\Content\\defaultImage.png");

                SelectedEntity.Texture = pb_texture.Image;
            }
        }

        
        /*
         * ImportEntityList
         * 
         * Loads a list of entities used by the level Editor from an XDocument.
         * 
         * XDocument entityList: the XML file containing information on the entities to be loaded.
         */
        private void ImportEntityList()
        {
            mAllEntities.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

            directoryInfo = directoryInfo.Parent.Parent;
            if (directoryInfo.GetDirectories("Entities").Length == 0)
                 directoryInfo.CreateSubdirectory("Entities");

            XDocument entityList;
            try
            {
                entityList = XDocument.Load(
                    directoryInfo.GetDirectories("Entities")[0].FullName + "\\" + "entityList.xml");
            }
            catch (IOException exception) { return; }

            foreach (XElement e in entityList.Elements())
            {
                if (e.Name == "Entities")
                {
                    foreach (XElement entity in e.Elements())
                    {
                        Entity createdEntity = new Entity(entity);
                        mAllEntities.Add(createdEntity);
                        lb_entitySelect.Items.Add(createdEntity);
                    }
                }
            }

            if(lb_entitySelect.Items.Count > 0)
                lb_entitySelect.SelectedIndex = 0;
        }

        /*
         * ExportEntityList
         * 
         * Creates an XML representation of all the Entities used in the level editor.
         * 
         */
        private void ExportEntityList()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

            directoryInfo = directoryInfo.Parent.Parent;
            if (directoryInfo.GetDirectories("Entities").Length == 0)
                directoryInfo.CreateSubdirectory("Entities");

            XDocument entityList = new XDocument();

            XElement entityTree = new XElement("Entities");
            foreach (Entity entity in mAllEntities)
            {
                entityTree.Add(entity.Export());
            }

            entityList.Add(entityTree);

            entityList.Save(
                directoryInfo.GetDirectories("Entities")[0].FullName + "\\" + "entityList.xml");


        }
    }
}
