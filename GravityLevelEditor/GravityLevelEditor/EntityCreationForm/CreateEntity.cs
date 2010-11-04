using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace GravityLevelEditor.EntityCreationForm
{
    public partial class CreateEntity : Form
    {
        Entity mSelectedEntity;

        ArrayList mAllEntities;
        ArrayList mFilteredEntities;

        public CreateEntity()
        {
            InitializeComponent();

            mAllEntities = new ArrayList();

            //Load Entity List and fill AllEntities here

            lb_entitySelect.Items.Clear();
        }

        private void CreateNew(object sender, EventArgs e)
        {
            mSelectedEntity = new Entity("", "New", true, true, 
                new Dictionary<string, string>(), Image.FromFile("..\\..\\Content\\defaultImage.png"));
            lb_entitySelect.Items.Add(mSelectedEntity);
            lb_entitySelect.SelectedItem = mSelectedEntity;
        }

        private void AdditionalProperties(object sender, EventArgs e)
        {
            if (mSelectedEntity == null) return;
            AdditionalProperties properties = new AdditionalProperties();
            properties.Location = Point.Add(this.Location, new Size(50, 50));
            properties.Properties = mSelectedEntity.Properties;
            if (properties.ShowDialog() == DialogResult.OK) 
                mSelectedEntity.Properties = properties.Properties;
        }

        private void Ok(object sender, EventArgs e)
        {
            //Export Entity list
        }

        private void DeleteSelected(object sender, EventArgs e)
        {
            if (mSelectedEntity == null) return;
            int index = lb_entitySelect.SelectedIndex;
            lb_entitySelect.Items.Remove(mSelectedEntity);
            if (index < lb_entitySelect.Items.Count)
                lb_entitySelect.SelectedIndex = index;
            else
                lb_entitySelect.SelectedIndex = index - 1;

            mSelectedEntity = (Entity)lb_entitySelect.SelectedItem;
        }

        private void SelectEntity(object sender, EventArgs e)
        {
            mSelectedEntity = (Entity)lb_entitySelect.SelectedItem;
            if (mSelectedEntity == null) return;

            tb_name.Text = mSelectedEntity.Name;
            if (mSelectedEntity.Type != "")
                cb_type.SelectedItem = mSelectedEntity.Type;
            else
                cb_type.Text = "Select Type";
            ckb_visible.Checked = mSelectedEntity.Visible;
            ckb_paintable.Checked = mSelectedEntity.Paintable;
            pb_texture.Image = mSelectedEntity.Texture;
        }
    }
}
