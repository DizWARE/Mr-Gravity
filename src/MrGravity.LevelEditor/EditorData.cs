using System.Collections;

namespace MrGravity.LevelEditor
{
    internal class EditorData
    {
        /*
         * SelectedEntities
         * 
         * Gets or sets the currently selected entities
         */
        public ArrayList SelectedEntities { get; set; }

        /*
         * OnDeck
         * 
         * Gets or sets the Entity that is currently on deck
         */
        public Entity OnDeck { get; set; }

        /*
         * Level
         * 
         * Gets or sets the current level
         */
        public Level Level { get; set; }

        public bool CtrlHeld { get; set; }

        /*
         * EditorData
         * 
         * Constructor for a passing wrapper between the editor and its tools
         */
        public EditorData(ArrayList selectedEntities, Entity onDeck, Level level)
        {
            SelectedEntities = selectedEntities;
            OnDeck = onDeck;
            Level = level;
        }
    }
}
