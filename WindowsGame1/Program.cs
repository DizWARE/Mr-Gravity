using System;

namespace GravityShift
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
//            using (GravityShiftMain game = new GravityShiftMain())
//            {
//                game.Run();
//            }
            using ( Menu menu = new Menu())
            {
                menu.Run();
            }
        }
    }
}

