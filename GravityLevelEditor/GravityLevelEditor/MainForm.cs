using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GravityLevelEditor
{
    public partial class MainForm : Form
    {

        public string[] folders = { "Enemies", "Background", "Tiles" };
        public string imageLocation = "..\\..\\..\\..\\WindowsGame1\\Content\\";

        private string baseContentPath = null;

        /*
         * MainForm()
         *
         * Constructor for GUI portion of Level Editor.
         */
        public MainForm()
        {
            InitializeComponent();
        }
    }
}
