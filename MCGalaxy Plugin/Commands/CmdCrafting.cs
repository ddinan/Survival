using MCGalaxy;

namespace Survival.Commands
{
    public class CmdCrafting
    {
        public void Execute(Player p, string[] args)
        {
            if (p.Rank < CmdsPermissions.get(2)) { p.SendMessage("You don't have the permission to use this command."); return; }

            if (args.Length < 2) { help(p); return; }
            try
            {
                ushort ID;
                switch (args[1])
                {
                    case "create":

                        ID = Helpers.IDConvert(ushort.Parse(args[4]));

                        RecipesRegistry.Create(args[2], args[3], ID, int.Parse(args[5]));
                        p.SendMessage("Craft " + args[2] + " created.");
                        break;

                    case "delete":
                        RecipesRegistry.Remove(args[2]);
                        p.SendMessage("Craft " + args[2] + " deleted.");
                        break;

                    case "add":

                        ID = Helpers.IDConvert(ushort.Parse(args[3]));

                        RecipesRegistry.AddIngredient(args[2], ID, int.Parse(args[4]));
                        p.SendMessage("Ingredient " + Block.GetName(p, ID) + " x" + args[4] + " added.");
                        break;

                    case "remove":

                        ID = Helpers.IDConvert(ushort.Parse(args[3]));

                        RecipesRegistry.RemoveIngredient(args[2], ID);
                        p.SendMessage("Ingredient " + Block.GetName(p, ID) + " removed.");
                        break;

                    default:
                        help(p);
                        break;
                }
            }
            catch { help(p); }
        }

        public void help(Player p) {
            Helpers.SendTextBlockToPlayer(p,
            "------Correct Usage------\n" +
            "Create a craft with:\n" +
            "%a/survival crafting create <CraftName> <Category> <ItemID> <Quantity>\n" +
            "Remove a craft with:\n" +
            "%a/survival crafting delete <CraftName>\n" +
            "Add a ingredient to a craft with:\n" +
            "%a/survival crafting add <CraftName> <IngredientID> <Quantity>\n" +
            "Remove a ingredient to a craft with:\n" +
            "%a/survival crafting remove <CraftName> <IngredientID>\n");
        }

    }
}
