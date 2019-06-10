using MCGalaxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Survival.Commands
{
    public class CmdTools
    {
        public void Execute(Player p, string[] args)
        {
            if (p.Rank < CmdsPermissions.get(4)) { p.SendMessage("You don't have the permission to use this command."); return; }

            if (args.Length < 2) { help(p); return; }
            try
            {
                ushort ID;
                switch (args[1])
                {
                    case "create":

                        ID = Helpers.IDConvert(ushort.Parse(args[2]));

                        ToolsRegistry.Create(ID, int.Parse(args[3]), float.Parse(args[4], CultureInfo.InvariantCulture), int.Parse(args[5]));
                        break;
                    case "delete":

                        ID = Helpers.IDConvert(ushort.Parse(args[2]));

                        ToolsRegistry.Remove(ID);
                        break;
                    case "categories":

                        ID = Helpers.IDConvert(ushort.Parse(args[3]));

                        switch (args[2])
                        {
                            case "add":
                                ToolsRegistry.AddCategory(ID, args[4]);
                                break;
                            case "remove":
                                ToolsRegistry.RemoveCategory(ID, args[4]);
                                break;
                            default:
                                help(p);
                                break;
                        }
                        break;
                    case "list":
                        Helpers.SendTextBlockToPlayer(p, ToolsRegistry.getToolsList(p));
                        break;
                    default:
                        help(p);
                        break;
                }
            }
            catch {
                help(p);
            }

        }

        private void help(Player p)
        {
            Helpers.SendTextBlockToPlayer(p, 
            "------Correct Usage------\n" +
            "Create a tool with:\n" +
            "%a/survival tools create <ItemID> <Durability> <BreakSpeed> <Damage>\n" +
            "Remove a tool with:\n" +
            "%a/survival tools delete <ItemID>\n" +
            "Add a Category to a tool with:\n" +
            "%a/survival tools categories add <ItemID> <Category>\n" +
             "Remove a Category to a tool with:\n" +
            "%a/survival tools categories remove <ItemID> <Category>\n" +
             "For the tools list:\n" +
            "%a/survival tools list\n"
            );
        }
    }
}
