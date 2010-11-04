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
        private Dictionary<String, String> mProperties;

        public Dictionary<String, String> Properties
        {
            get { return mProperties; }
            set { mProperties = value; }
        }

        public AdditionalProperties()
        {
            InitializeComponent();
        }
    }
}
