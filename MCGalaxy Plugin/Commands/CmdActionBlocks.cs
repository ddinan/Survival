using MCGalaxy;

namespace Survival.Commands
{
    public class CmdActionBlocks
    {
        public void Execute(Player p, string[] args)
        {
            if (p.Rank < CmdsPermissions.get(5)) { p.SendMessage("You don't have the permission to use this command."); return; }
            try
            {
                ushort ID = 0;
                if (args.Length > 2) {
                    ID = Helpers.IDConvert(ushort.Parse(args[2]));
                }
                switch (args[1])
                {
                    case "create":
                        ActionsBlocks.Init(ID);
                        break;
                    case "remove":
                        ActionsBlocks.Remove(ID);
                        break;
                    case "addtrigger":
                        ActionsBlocks.AddTrigger(ID, byte.Parse(args[3]));
                        break;
                    case "removetrigger":
                        ActionsBlocks.RemoveTrigger(ID, int.Parse(args[3]));
                        break;
                    case "removeaction":
                        ActionsBlocks.RemoveAction(ID, int.Parse(args[3]), int.Parse(args[4]));
                        break;
                    case "addaction":

                        string strArg = "";
                        for (int i = 5; i < args.Length; i++) {
                            strArg += args[i] + " ";
                        }

                        ActionsBlocks.AddAction(ID, int.Parse(args[3]), byte.Parse(args[4]),strArg);
                        break;
                    case "list":
                        Helpers.SendTextBlockToPlayer(p,ActionsBlocks.getList(p));
                        break;
                    case "actionslist":
                        Helpers.SendTextBlockToPlayer(p,ActionsBlocks.getActionsList(p, ID));
                        break;
                    case "door":

                        ushort[] tempD = new ushort[8];
                        int iincr = 0;
                        for (int i = 2; i <= 9; i++) {
                            tempD[iincr++] = Helpers.IDConvert(ushort.Parse(args[i]));
                        }

                        ActionBlocks.Door.Builder(tempD);
                        break;
                    default:
                        help(p);
                        break;

                }
            } catch { help(p); }
        }

        private void help(Player p)
        {
            Helpers.SendTextBlockToPlayer(p,
            "------Correct Usage------\n" +
            "%a/survival actions create <Block ID>\n" +
            "%a/survival actions addtrigger <Block ID> <Trigger type>\n" +
            "%a/survival actions addaction <Block ID> <Trigger ID> <Action Type> <args>\n" +
             "%a/survival actions remove <Block ID>\n" +
            "%a/survival actions removetrigger <Block ID> <Trigger ID>\n" +
            "%a/survival actions removeaction <Block ID> <Trigger ID> <Action ID>\n" +
            "%a/survival actions list\n" +
             "%a/survival actions actionslist <Block ID>\n");
        }
    }
}
