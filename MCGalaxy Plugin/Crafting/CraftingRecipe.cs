using System.Collections.Generic;

namespace Survival.Crafting
{
    public class CraftingRecipe
    {
        public string name { get; set; }
        public string category { get; set; }
        public int craftCount { get; set; }
        public ushort itemID { get; set; }
        public List<Item> requiredIngredients { get; set; }

        public CraftingRecipe(string cName,string cCategory,ushort cItemID,int cCraftCount)
        {
            name = cName;
            category = cCategory;
            craftCount = cCraftCount;
            itemID = cItemID;
            requiredIngredients = new List<Item>();
        }

        public CraftingRecipe() {
            name = "";
            category = "";
            craftCount = 0;
            itemID = 0;
            requiredIngredients = new List<Item>();
        }
    }
}
