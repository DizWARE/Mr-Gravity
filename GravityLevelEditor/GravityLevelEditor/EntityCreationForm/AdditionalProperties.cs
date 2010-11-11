using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GravityLevelEditor.EntityCreationForm
{
    public partial class AdditionalProperties : Form
    {
        private string mPreviousKey;
        private CreateEntity mForm;

        private Dictionary<string, string> mProperties;
        public Dictionary<string, string> Properties
        {
            get { return mProperties; }
            set { mProperties = value; }
        }

        /*
         * AdditionalProperties
         * 
         * Constructor for AdditionalProperties form.  Enables user to 
         * add/delete/modify properties of a current entity.
         * 
         * Dictionary<string, string> props: list of current properties the 
         *                                   selected entity owns
         */
        public AdditionalProperties(Dictionary<string, string> props)
        {
            InitializeComponent();

            mProperties = new Dictionary<string, string>();
            if (props.Count > 0)
                for (int i = 0; i < props.Count; i++)
                {
                    string name = mProperties.ElementAt(i).Key;
                    string value = mProperties.ElementAt(i).Value;
                    lb_properties.Items.Add(name + "/" + value);
                }
        }

        /*
         * Ok
         * 
         * Close the additional properties form.
         */
        private void Ok(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
         * NewProperty
         * 
         * Create a new property and add it to the listbox and
         * property list.
         */
        private void NewProperty(object sender, EventArgs e)
        {
            try
            {
                mProperties.Add("New", "Property");
                lb_properties.Items.Add("New/Property");
                lb_properties.SelectedIndex++;
                tb_name.Text = mPreviousKey = "New";
                tb_value.Text = "Property";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Property already defined!", "Error", MessageBoxButtons.OK);
            }
        }

        /*
         * RemoveProperty
         * 
         * Remove the selected property from the property list and
         * the listbox.
         */
        private void RemoveProperty(object sender, EventArgs e)
        {
            if (lb_properties.SelectedIndex == -1) return;

            int index = lb_properties.SelectedIndex;
            mProperties.Remove(mPreviousKey);
            lb_properties.Items.RemoveAt(index);
            lb_properties.SelectedIndex = index-1;
            if (lb_properties.Items.Count == 0)
                lb_properties.SelectedIndex = -1;
            else if (lb_properties.SelectedIndex < 0)
                lb_properties.SelectedIndex++;
        }

        /*
         * Apply
         * 
         * Update the properties list and the listbox with
         * the new key and value.
         */
        private void Apply(object sender, EventArgs e)
        {
            if (lb_properties.SelectedIndex == -1 || mProperties.ContainsKey(tb_name.Text)) return;
            mProperties.Remove(mPreviousKey);
            mProperties.Add(tb_name.Text, tb_value.Text);
            mPreviousKey = tb_name.Text;

            UpdateView();
        }

        /*
         * UpdateView
         * 
         * Updates the list view with the text wise changes
         */
        private void UpdateView()
        {
            int index = lb_properties.SelectedIndex;
            if (index < 0) return;
            lb_properties.Items.Insert(index + 1, tb_name.Text + "/" + tb_value.Text);
            lb_properties.Items.RemoveAt(index);
            lb_properties.SelectedIndex = index;
        }

        /*
         * IndexChanged
         * 
         * Event handler for an index change inside the properties listbox.
         */
        private void IndexChanged(object sender, EventArgs e)
        {
            if (lb_properties.SelectedIndex == -1) return;
            int splitIndex = lb_properties.SelectedItem.ToString().IndexOf('/');
            tb_name.Text = lb_properties.SelectedItem.ToString().Substring(0, splitIndex);
            tb_value.Text = lb_properties.SelectedItem.ToString().Substring(splitIndex + 1);
        }

        /*
         * Closing
         * 
         * Copies the properties over to the entity on closing.
         */
        private void Closing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
