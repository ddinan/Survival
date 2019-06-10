using MCGalaxy;
namespace Survival.Commands
{
    public class CmdSetPerms
    {

        private LevelPermission CmdSetPermRank = LevelPermission.Admin;

        public void Execute(Player p, string[] args)
        {
            if (p.Rank < CmdSetPermRank) { p.SendMessage("You don't have the permission to use this command."); return; }
            if (args.Length < 2) { help(p); return; }
            try
            {
                switch (args[1])
                {
                    case "blocks":
                        CmdsPermissions.set(0, int.Parse(args[2]));
                        break;
                    case "crafting":
                        CmdsPermissions.set(2, int.Parse(args[2]));
                        break;
                    case "tools":
                        CmdsPermissions.set(4, int.Parse(args[2]));
                        break;
                    case "maps":
                        CmdsPermissions.set(3, int.Parse(args[2]));
                        break;
                    case "cmds":
                        CmdsPermissions.set(1, int.Parse(args[2]));
                        break;
                    case "actions":
                        CmdsPermissions.set(5, int.Parse(args[2]));
                        break;
                    default:
                        help(p);
                        return;
                }
                p.SendMessage("Permission set to " + args[2] + " for this command.");
            }
            catch {
                help(p);
            }
        }

        private void help(Player p)
        {
            Helpers.SendTextBlockToPlayer(p,
             "------Correct Usage------\n" +
            "%a/survival cmdperm <cmd> <Rank Permission> \n" +
            "%eExample: /survival cmdperm blocks 100\n"+
            "%e100 is the permission level of the admin rank.");
        }
    }
}
