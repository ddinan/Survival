using MCGalaxy;

namespace Survival.Commands
{
    public class CmdBlock
    {
        public void Execute(Player p,string[] args) {
            if (p.Rank < CmdsPermissions.get(0)) { p.SendMessage("You don't have the permission to use this command."); return; }

            if (args.Length < 2) { help(p); return; }

            ushort id = 0;
            if (args.Length > 2) { id = ushort.Parse(args[2]); }
            try
            {
                switch (args[1])
                {
                    case "add":
                        if (args.Length < 4) { help(p); return; }

                        ushort givenID = id;
                        int givenQuantity = 1;
                        string category = "";

                        if (args.Length > 4)
                        {
                            givenID = ushort.TryParse(args[4], out givenID) ? ushort.Parse(args[4]) : id;
                        }

                        if (args.Length > 5)
                        {
                            givenQuantity = int.TryParse(args[5], out givenQuantity) ? int.Parse(args[5]) : 1;
                        }

                        if (args.Length > 6)
                        {
                            category = args[6];
                        }

                        givenID = Helpers.IDConvert(givenID);
                        id = Helpers.IDConvert(id);

                        BlocksRegistry.Add(id, int.Parse(args[3]), givenID, givenQuantity, category);

                        Helpers.SendTextBlockToPlayer(p, "Set hardness of " + id + " to " + int.Parse(args[3]) + ".\n" +
                        "Set given ID of " + id + " to " + givenID + ".\n" +
                        "Set given Quantity of " + id + " to " + givenQuantity + ".\n"+
                        "Set Category of " + id + " to " + category + ".");

                        break;
                    case "remove":
                        id = Helpers.IDConvert(id);
                        BlocksRegistry.Remove(id);
                        p.SendMessage("Removed the settings for " + id + ".");
                        break;
                    case "list":
                        Helpers.SendTextBlockToPlayer(p, BlocksRegistry.getBlocksList(p));
                        break;
                    default:
                        help(p);
                        break;
                }
            }
            catch { help(p); }
        }

        private void help(Player p) {
            Helpers.SendTextBlockToPlayer(p,
             "------Correct Usage------\n" +
            "%a/survival blocks add <id> <hardness> <ID gave> <Quantity gave> <category>\n" +
            "%a/survival blocks remove <id>\n" +
            "%a/survival blocks list\n");
        }

    }

}
