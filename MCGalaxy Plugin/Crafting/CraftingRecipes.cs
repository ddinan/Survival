using System;
using System.Collections.Generic;
using System.Linq;
namespace Survival.Crafting
{
    internal class List
    {
        public static List<CraftingRecipe> Recipes = new List<CraftingRecipe>();
        private static string _recipesLocation = "./text/survivalPlugin/crafts.txt";

        internal static void Create(CraftingRecipe Recipe)
        {
            Recipes.Add(Recipe);
            Save();
        }

        internal static void Remove(string craftName)
        {
            int index = Recipes.FindIndex(i => i.name == craftName);
            if (index == -1) return;

            Recipes.RemoveAt(index);
            Save();
        }

        internal static void AddIngredient(string craftName,Item ingredient)
        {
            int index = Recipes.FindIndex(i => i.name == craftName);
            if (index == -1) return;

            Recipes[index].requiredIngredients.Add(ingredient);
            Save();
        }

        internal static void RemoveIngredient(string craftName, ushort itemID)
        {
            int index = Recipes.FindIndex(i => i.name == craftName);
            if (index == -1) return;

            int index2 = Recipes[index].requiredIngredients.FindIndex(i => i.ID == itemID);
            if (index2 == -1) return;

            Recipes[index].requiredIngredients.RemoveAt(index2);
            Save();
        }

        private static void Save()
        {
            System.IO.FileInfo filedir = new System.IO.FileInfo(_recipesLocation);
            filedir.Directory.Create();
            System.IO.File.WriteAllText(_recipesLocation, Helpers.Serialize<List<CraftingRecipe>>(Recipes));
        }

        internal static void Load()
        {

            if (System.IO.File.Exists(_recipesLocation))
            {
                string json = System.IO.File.ReadAllText(_recipesLocation);
                Recipes = Helpers.Deserialize<List<CraftingRecipe>>(json);
            }

        }
    }
}
