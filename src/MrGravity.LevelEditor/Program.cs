using System;
using System.Windows.Forms;

namespace MrGravity.LevelEditor
{
    internal static class Program
    {
        /*
         * Main
         * 
         * The main entry point for the application.
         */
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
