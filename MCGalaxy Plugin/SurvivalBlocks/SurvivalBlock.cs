namespace Survival.SurvivalBlocks
{
    public class SurvivalBlock
    {
        public ushort ID { get; set; }
        public int hardness { get; set; }
        public ushort givenID { get; set; }
        public int givenQuantity { get; set; }
        public string category { get; set; }

        public SurvivalBlock(ushort blockID,int blockHardness,ushort blockGivenID,int blockGivenQuantity,string blockCategory)
        {
            ID = blockID;
            hardness = blockHardness;
            givenID = blockGivenID;
            givenQuantity = blockGivenQuantity;
            category = blockCategory;
        }

        public SurvivalBlock() {
            ID = 0;
            hardness = 0;
            givenID = 0;
            givenQuantity = 0;
            category = "";
        }
    }
}
