using MCGalaxy;

namespace Survival.Commands
{
    public class CmdCmdsWhitelist
    {
        public void Execute(Player p, string[] args)
        {
            if (p.Rank < CmdsPermissions.get(1)) { p.SendMessage("You don't have the permission to use this command."); return; }
            if (args.Length < 2) { help(p); return; }
            string cmd = "";
            switch (args[1])
            {
                case "add":

                    cmd = args[2];
                    if (args.Length>3) cmd += " " + args[3];
                    CmdsWhitelistRegistry.Add(cmd);
                    p.SendMessage("Added " + cmd + ".");
                    break;
                case "remove":
                    cmd = args[2];
                    if (args.Length>3) cmd += " " + args[3];
                    CmdsWhitelistRegistry.Remove(cmd);
                    p.SendMessage("Removed " + cmd + ".");
                    break;
                case "list":
                    Helpers.SendTextBlockToPlayer(p, "%eCommands Whitelisted:\n" + CmdsWhitelistRegistry.getListString());
                    break;
                default:
                    help(p);
                    break;
            }
        }

        private void help(Player p)
        {
            Helpers.SendTextBlockToPlayer(p,
             "------Correct Usage------\n" +
            "%a/survival cmds add <name>\n" +
            "%a/survival cmds remove <name>\n" +
            "%a/survival cmds list\n");
        }
    }
}
