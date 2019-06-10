using MCGalaxy;
using Survival.ActionBlocks;
using System.Collections.Generic;

namespace Survival
{
    public static class ActionsBlocks
    {
        public static List<ActionBlock> ActionBlocks = new List<ActionBlock>();
        private static string _actionBlocksLocation = "./text/survivalPlugin/actionblocks.txt";
        public static SurvivalPlayers survivalPlayers;

        public static void Init(ushort ID) {
            ActionBlocks.Add(new ActionBlock(ID));
            Save();
        }

        public static void Remove(ushort ID) {
            int index = ActionBlocks.FindIndex(i => i.BlockID == ID);
            ActionBlocks.RemoveAt(index);
            Save();
        }

        public static string getList(Player p) {              
            string list = "%f";
            foreach (ActionBlock b in ActionBlocks)
            {
                list += Block.GetName(p,b.BlockID) + "[" + Helpers.IDConvert2(b.BlockID) + "]" + "\n%f";
            }
            return list;
        }

        public static string getActionsList(Player p, ushort BlockID)
        {
            int index = ActionBlocks.FindIndex(i => i.BlockID == BlockID);
            string list = "%a";
            int k = 0;
            foreach (Trigger t in ActionBlocks[index].triggers)
            {
                list += "&e" + k++ + ") &bON&a " + "[type:" + t.type + "]\n&a";
                int j = 0;
                foreach (Action a in t.actions)
                {
                    list += "&e-> " + j++ + ") &bDO&a [type:" + a.ActionTypeID + "] [args:" + a.args + "]\n&a";
                }
            }
            return list;
        }

        public static void AddTrigger(ushort ID,byte triggerType)
        {
            int index = ActionBlocks.FindIndex(i => i.BlockID == ID);

            int triggerID = ActionBlocks[index].triggers.Count;

            ActionBlocks[index].triggers.Add(new Trigger(triggerID,triggerType));
            Save();

        }

        public static void RemoveTrigger(ushort ID, int triggerID)
        {
            int index = ActionBlocks.FindIndex(i => i.BlockID == ID);

            ActionBlocks[index].triggers.RemoveAt(triggerID);
            Save();
        }

        public static void RemoveAction(ushort ID, int triggerID, int actionID)
        {
            int index = ActionBlocks.FindIndex(i => i.BlockID == ID);

            ActionBlocks[index].triggers[triggerID].actions.RemoveAt(actionID);
            Save();
        }

        public static void AddAction(ushort ID, int triggerID,byte Action,string args)
        {
            int blockIndex = ActionBlocks.FindIndex(i => i.BlockID == ID);

            ActionBlocks[blockIndex].triggers[triggerID].actions.Add(new Action(Action,args));
            Save();

        }

        public static bool isActionBlockTrigger(ushort ID,int triggerType) {
            int index = ActionBlocks.FindIndex(i => i.BlockID == ID);

            if (index == -1) return false;

            int triggerIndex = ActionBlocks[index].triggers.FindIndex(i => i.type == triggerType);

            if (triggerIndex == -1) return false;

            return true;
        }

        public static void Triggering(Player p, ushort ID, int triggerType , ushort x, ushort y, ushort z) {
            int index = ActionBlocks.FindIndex(i => i.BlockID == ID);
            int triggerIndex = ActionBlocks[index].triggers.FindIndex(i => i.type == triggerType);

            foreach (Action action in ActionBlocks[index].triggers[triggerIndex].actions) {
                ActionTypes.Triggering(p,action.ActionTypeID,action.args,x,y,z, survivalPlayers, ID);
            }
        }

        private static void Save()
        {
            System.IO.FileInfo filedir = new System.IO.FileInfo(_actionBlocksLocation);
            filedir.Directory.Create();
            System.IO.File.WriteAllText(_actionBlocksLocation, Helpers.Serialize<List<ActionBlock>>(ActionBlocks));
        }

        internal static void Load()
        {

            if (System.IO.File.Exists(_actionBlocksLocation))
            {
                string json = System.IO.File.ReadAllText(_actionBlocksLocation);
                ActionBlocks = Helpers.Deserialize<List<ActionBlock>>(json);
            }

        }
    }
}
