using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GravityLevelEditor
{
    class EditorData
    {
        private ArrayList mSelectedEntities;
        private Entity mOnDeck;
        private Level mLevel;
        private bool mCTRLHeld = false;

        /*
         * SelectedEntities
         * 
         * Gets or sets the currently selected entities
         */
        public ArrayList SelectedEntities
        {
            get { return mSelectedEntities; }
            set { mSelectedEntities = value;}
        }

        /*
         * OnDeck
         * 
         * Gets or sets the Entity that is currently on deck
         */
        public Entity OnDeck
        {
            get { return mOnDeck; }
            set { mOnDeck = value;}
        }

        /*
         * Level
         * 
         * Gets or sets the current level
         */
        public Level Level
        {
            get { return mLevel; }
            set { mLevel = value;}
        }

        public bool CTRLHeld
        {
            get { return mCTRLHeld; }
            set { mCTRLHeld = value; }
        }

        /*
         * EditorData
         * 
         * Constructor for a passing wrapper between the editor and its tools
         */
        public EditorData(ArrayList selectedEntities, Entity onDeck, Level level)
        {
            mSelectedEntities = selectedEntities;
            mOnDeck = onDeck;
            mLevel = level;
        }
    }
}
