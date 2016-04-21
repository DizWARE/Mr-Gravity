using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.EntityCreationForm
{
    public partial class AdditionalProperties : Form
    {
        private string _mPreviousKey;
        private bool _mEditable = true;
        public bool Editable
        {
            set
            {
                _mEditable = value;
                tb_name.Enabled = _mEditable;
                b_add.Enabled = _mEditable;
                b_remove.Enabled = _mEditable;
            }
        }

        public Dictionary<string, string> Properties { get; set; }

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

            Properties = new Dictionary<string, string>();
            if (props.Count > 0)
                for (var i = 0; i < props.Count; i++)
                {
                    
                    var name = props.ElementAt(i).Key;
                    var value = props.ElementAt(i).Value;
                    Properties.Add(name, value);
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
            DialogResult = DialogResult.OK;
            Close();
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
                Properties.Add("New", "Property");
                lb_properties.Items.Add("New/Property");
                lb_properties.SelectedIndex++;
                tb_name.Text = _mPreviousKey = "New";
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

            var index = lb_properties.SelectedIndex;
            Properties.Remove(_mPreviousKey);
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
            if (lb_properties.SelectedIndex == -1) return;
            if (!_mEditable) { EditValue(); return; }
            if (Properties.ContainsKey(tb_name.Text)) { EditValue(); return; }
            Properties.Remove(_mPreviousKey);
            Properties.Add(tb_name.Text, tb_value.Text);
            _mPreviousKey = tb_name.Text;

            UpdateView();
        }

        private void EditValue()
        {
            Properties[tb_name.Text] = tb_value.Text;
            UpdateView();
        }

        /*
         * UpdateView
         * 
         * Updates the list view with the text wise changes
         */
        private void UpdateView()
        {
            var index = lb_properties.SelectedIndex;
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
            var splitIndex = lb_properties.SelectedItem.ToString().IndexOf('/');
            tb_name.Text = lb_properties.SelectedItem.ToString().Substring(0, splitIndex);
            tb_value.Text = lb_properties.SelectedItem.ToString().Substring(splitIndex + 1);
            _mPreviousKey = tb_name.Text;
        }


    }
}
