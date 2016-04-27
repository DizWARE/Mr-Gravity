using MrGravity.MISC_Code;

namespace MrGravity
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            using (var game = new MrGravityMain())
            {
                if (args.Length > 0)
                {
                    game.LevelLocation = args[0];
                    game.CurrentState = GameStates.InGame;
                }

                game.Run();
            }
        }
    }
}

