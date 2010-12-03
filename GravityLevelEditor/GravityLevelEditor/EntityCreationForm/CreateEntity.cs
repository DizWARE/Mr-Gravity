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
        ArrayList mFilteredEntities;
        public Entity SelectedEntity 
        { 
            get 
            { 
                if(mAllEntities.Count > 0)
                    return (Entity)mFilteredEntities[mSelectedIndex]; 

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
            mFilteredEntities = new ArrayList();
            lb_entitySelect.Items.Clear();

            ImportEntityList();

            lb_filter.SelectedIndex = 0;
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
            string type = "";
            if(!lb_filter.SelectedItem.Equals("(None)")) type = lb_filter.SelectedItem.ToString();
            mAllEntities.Add(new Entity(type, "New", type, true,
                new Dictionary<string, string>(),
                Image.FromFile("..\\..\\Content\\defaultImage.png")));
            mFilteredEntities.Add(mAllEntities[mAllEntities.Count - 1]);
            mSelectedIndex = mFilteredEntities.Count - 1;
            lb_entitySelect.Items.Add(mFilteredEntities[mSelectedIndex]);
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
                mFilteredEntities[lb_entitySelect.SelectedIndex] == null) return;

            mSelectedIndex = lb_entitySelect.SelectedIndex;

            tb_name.Text = SelectedEntity.Name;
            if (SelectedEntity.Type != "")
                cb_type.Text = SelectedEntity.Type;
            else
                cb_type.Text = "Select Type";
            ckb_paintable.Checked = SelectedEntity.Paintable;
            if (SelectedEntity.CollisionType != "")
                cb_collisionType.Text = SelectedEntity.CollisionType;
            else
                cb_collisionType.Text = "Select Collision Type";
            pb_texture.Image = SelectedEntity.Texture;
        }

        /*
         * DeleteSelected
         * 
         * Removes the selected entity from the Entity list
         */
        private void DeleteSelected(object sender, EventArgs e)
        {
            if (mFilteredEntities.Count == 0 || SelectedEntity == null) return;
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
            if (mFilteredEntities.Count == 0 || SelectedEntity == null) return;
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
                PrepareTrigger();
                PrepareObjects();
                UpdateView();
            }
        }

        private void PrepareTrigger()
        {
            if (SelectedEntity.Type == "Triggers")
            {
                if(!SelectedEntity.Properties.ContainsKey("Width")) SelectedEntity.Properties.Add("Width", "3");
                if(!SelectedEntity.Properties.ContainsKey("Height")) SelectedEntity.Properties.Add("Height", "3");
            }
            else
            {
                if(SelectedEntity.Properties.ContainsKey("Width")) SelectedEntity.Properties.Remove("Width");
                if(SelectedEntity.Properties.ContainsKey("Height")) SelectedEntity.Properties.Remove("Height");
            }
        }

        private void PrepareObjects()
        {
            if ((SelectedEntity.Type == "Physics Objects" || SelectedEntity.Type == "Static Objects" || SelectedEntity.Type == "Triggers"))
            {
                if (!SelectedEntity.Properties.ContainsKey("Shape")) SelectedEntity.Properties.Add("Shape", "Square");
                if (!SelectedEntity.Properties.ContainsKey("Mass") && SelectedEntity.Type == "Physics Objects") 
                    SelectedEntity.Properties.Add("Mass", "1");
            }
            else
            {
                if (SelectedEntity.Properties.ContainsKey("Shape")) SelectedEntity.Properties.Remove("Shape");
                if (SelectedEntity.Properties.ContainsKey("Mass")) SelectedEntity.Properties.Remove("Mass");
            }
        }

        /*
         * CollisionTypeChanged
         * 
         * Handles when the user changes the Collision Type
         */
        private void CollisionTypeChanged(object sender, EventArgs e)
        {
            if (lb_entitySelect.SelectedIndex > -1)
            {
                SelectedEntity.CollisionType = cb_collisionType.Text;
                UpdateView();
            }
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
            mFilteredEntities.Clear();
            
            //If (None) than show all items, Othewise only show of the selected type
            if("(None)".Equals(lb_filter.SelectedItem))
                foreach (Entity entity in mAllEntities)
                {
                    lb_entitySelect.Items.Add(entity);
                    mFilteredEntities.Add(entity);
                }
            else
                foreach (Entity entity in mAllEntities)
                    if (entity.Type.Equals(lb_filter.SelectedItem))
                    {
                        lb_entitySelect.Items.Add(entity);
                        mFilteredEntities.Add(entity);
                    }

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
            cb_collisionType.Text = "Select Collision Type";
            ckb_paintable.Checked = false;
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
            mFilteredEntities.Clear();
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
                        mFilteredEntities.Add(createdEntity);
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
