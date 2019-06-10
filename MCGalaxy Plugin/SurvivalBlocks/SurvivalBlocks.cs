using System.Collections.Generic;

namespace Survival.SurvivalBlocks
{
    internal class List
    {
        public static List<SurvivalBlock> Blocks = new List<SurvivalBlock>();
        private static string _blocksLocation = "./text/survivalPlugin/blocks.txt";

        internal static void Add(SurvivalBlock survivalBlock)
        {
            Blocks.Add(survivalBlock);
            Save();
        }

        internal static void Remove(ushort ID)
        {
            Blocks.RemoveAll(x => x.ID == ID);
            Save();
        }

        private static void Save() {
            System.IO.FileInfo filedir = new System.IO.FileInfo(_blocksLocation);
            filedir.Directory.Create();
            System.IO.File.WriteAllText(_blocksLocation, Helpers.Serialize<List<SurvivalBlock>>(Blocks));
        }

        internal static void Load() {

            if (System.IO.File.Exists(_blocksLocation))
            {
                string json = System.IO.File.ReadAllText(_blocksLocation);
                Blocks = Helpers.Deserialize<List<SurvivalBlock>>(json);
            }

        }
    }
}
