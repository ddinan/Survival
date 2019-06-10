using MCGalaxy;

namespace Survival.Commands
{
    public class CmdCraftList
    {
        public void Execute(Player p, string[] args)
        {
            Helpers.SendTextBlockToPlayer(p,
                RecipesRegistry.getRecipesList(p,args[0]));
        }
        
    }
}
