using System.Collections.Generic;

namespace Survival.ActionBlocks
{
    public class Trigger
    {

        /*Types of triggers:
            0 -> Left click on block
            1 -> Right click on block
            2 -> Placed block
            3 -> Broke block
            4 -> Triggered by a actionblock
            5 -> Normal Right click
        */
        public byte type { get; set; }
        public List<Action> actions { get; set; }

        public Trigger(int ID,byte tType)
        {
            type = tType;
            actions = new List<Action>();
        }

        public Trigger() {
            type = 0;
            actions = new List<Action>();
        }
    }
}
