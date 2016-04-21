namespace MrGravity
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            using (var game = new GravityShiftMain())
            {
                if (args.Length > 0)
                { game.LevelLocation = args[0]; game.DisableMenu(); }
                game.Run();
                
            }
        }
    }
}

