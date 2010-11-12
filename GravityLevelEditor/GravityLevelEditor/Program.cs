using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GravityLevelEditor.EntityCreationForm;

namespace GravityLevelEditor
{
    static class Program
    {
        /*
         * Main
         * 
         * The main entry point for the application.
         */
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
