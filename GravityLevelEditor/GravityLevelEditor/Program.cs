using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ImportForm());
        }
    }
}
