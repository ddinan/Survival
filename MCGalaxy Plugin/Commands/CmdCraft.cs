using MCGalaxy;

namespace Survival.Commands
{
    public class CmdCraft
    {

        private SurvivalPlayers _playersList;
        private SurvivalMaps _survivalMaps;

        public CmdCraft(SurvivalPlayers list, SurvivalMaps maps)
        {
            _playersList = list;
            _survivalMaps = maps;
        }

        public void Execute(Player p, string[] args)
        {
            if (!_survivalMaps.Maps.Contains(p.Level.name)) return;
            if (args[0] == "") { help(p); return; }
         
            Crafting.CraftingRecipe recipe = Crafting.List.Recipes.Find(x => x.name == args[0]);
            if (recipe == null) { p.SendMessage("Unknown craft '" + args[0] + "'"); return; }

            int count = 1;
            if (args.Length > 1) { 
                bool parsedCount = int.TryParse(args[1],out count);
                if (!parsedCount) count = 1;
            }

            int craftedInfo;
            craftedInfo = _playersList.PlayerMap[p].CraftItem(recipe,count);

            if (craftedInfo == 1)
            {
                p.SendMessage("Crafted " + recipe.craftCount + " " + recipe.name + ". (x" + count + ")");
            }
            else if (craftedInfo == 0) {
                p.SendMessage("Not enough resources.");
            }
            else if (craftedInfo == 2) {
                p.SendMessage("Inventory Full.");
            }
        }

        public void help(Player p) {
            Helpers.SendTextBlockToPlayer(p, 
                "%a/craft [name] [multiplier] \n"+
                "%a/craftlist%e, for the craft list."
                );
        }
    }
}
