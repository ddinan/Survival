using System.Collections.Generic;

namespace Survival.ActionBlocks
{
    public class ActionBlock
    {
        public ushort BlockID { get; set; }
        public List<Trigger> triggers { get; set; }

        public ActionBlock(ushort bID)
        {
            BlockID = bID;
            triggers = new List<Trigger>();
        }

        public ActionBlock() {
            BlockID = 0;
            triggers = new List<Trigger>();
        }
    }
}
