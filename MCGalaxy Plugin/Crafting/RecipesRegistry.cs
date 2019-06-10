using Survival.Crafting;
using MCGalaxy;

namespace Survival
{
    public static class RecipesRegistry
    {
        public static void Create(string name,string category,ushort ID, int count)
        {

            List.Create(new CraftingRecipe(name,category,ID,count));
        }

        public static void Remove(string name)
        {
            List.Remove(name);
        }

        public static void AddIngredient(string name,ushort ID,int count) {

            List.AddIngredient(name,new Item(ID,count,0));
        }

        public static void RemoveIngredient(string name, ushort ID)
        {
            List.RemoveIngredient(name,ID);
        }

        public static string getRecipesList(Player p,string category) {
            if (category == null || category == "") {
                string categories = "";
                foreach (CraftingRecipe recipe in List.Recipes) {
                    if (!categories.Contains(recipe.category)) categories += recipe.category + "\n%a";
                }
                return "Craft categories: \n/Craftlist [category]\n%a" + categories;
            }

            string constructedList = "Crafts List: \n";
            foreach(CraftingRecipe recipe in List.Recipes) {
                if (recipe.category != category) continue;
                constructedList += "%a" + recipe.name + (recipe.craftCount == 1 ? "" : "[%fx"+ recipe.craftCount + "%a]") + "%e: ";
                foreach (Item item in recipe.requiredIngredients) {
                    try
                    {
                        constructedList += Block.GetName(p, item.ID) + "[%fx" + item.quantity + "%e] ";
                    }
                    catch { }
                }
                constructedList += "\n";
            }
            return constructedList;
        }


        internal static void Load()
        {
            List.Load();
        }

    }
}
