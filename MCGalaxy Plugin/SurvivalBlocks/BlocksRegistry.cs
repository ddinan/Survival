using MCGalaxy;
using Survival.SurvivalBlocks;

namespace Survival
{
    public static class BlocksRegistry
    {
        public static void Add(ushort ID,int hardness,ushort givenID,int givenQuantity,string category) {
            List.Add(new SurvivalBlock(ID,hardness,givenID,givenQuantity,category)); 
        }

        public static void Remove(ushort ID)
        {
            List.Remove(ID);
        }

        public static SurvivalBlock getByID(ushort ID)
        {
            return List.Blocks.Find(i => i.ID == ID);
        }

        public static string getBlocksList(Player p)
        {
            string constructedList = "Blocks List: \n";
            foreach (SurvivalBlock block in List.Blocks)
            {
                constructedList += "%a" + Block.GetName(p,block.ID) + "[" + Helpers.IDConvert2(block.ID) + "]" + "%e: "
                + "&eH: &f" + block.hardness + " &eG_ID: &f" + block.givenID + " &eG_QU: &f" + block.givenQuantity + " &eCA: &f" + block.category + "\n";
            }
            return constructedList;
        }

        internal static void Load() {
            List.Load();
        }
    }
}
