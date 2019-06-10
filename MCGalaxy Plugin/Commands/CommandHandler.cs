using MCGalaxy;
using Survival.Commands;

namespace Survival
{
    public class CommandHandler
    {

        private SurvivalPlayers _playersList;
        private SurvivalMaps _survivalMaps;

        //Commands Objects
        private CmdTools _cmdTools;
        private CmdSetPerms _cmdSetPerms;
        private CmdMap _cmdMap;
        private CmdInventory _cmdInventory;
        private CmdDrop _cmdDrop;
        private CmdCraftList _cmdCraftList;
        private CmdCrafting _cmdCrafting;
        private CmdCraft _cmdCraft;
        private CmdCmdsWhitelist _cmdCmdsWhitelist;
        private CmdBlock _cmdBlock;
        private CmdActionBlocks _cmdActionBlocks;
        private CmdChest _cmdChest;
        

        public CommandHandler(SurvivalPlayers list, SurvivalMaps maps) {
            _playersList = list;
            _survivalMaps = maps;

            _cmdTools = new CmdTools();
            _cmdSetPerms = new CmdSetPerms();
            _cmdMap = new CmdMap(list,_survivalMaps);
            _cmdInventory = new CmdInventory(list,_survivalMaps);
            _cmdDrop = new CmdDrop(list,_survivalMaps);
            _cmdCraftList = new CmdCraftList();
            _cmdCrafting = new CmdCrafting();
            _cmdCraft = new CmdCraft(list,_survivalMaps);
            _cmdCmdsWhitelist = new CmdCmdsWhitelist();
            _cmdBlock = new CmdBlock();
            _cmdActionBlocks = new CmdActionBlocks();
            _cmdChest = new CmdChest(list, _survivalMaps);
        }

        public void HandleCommand(Player p, string cmd, string arg, CommandData data)
        {
            
            if (_survivalMaps.Maps.Contains(p.level.name) && p.Rank < LevelPermission.Operator) {
                if(cmd != "rules" && cmd!= "agree" && cmd != "Rules" && cmd != "help" && cmd != "survival" && cmd != "craft" && cmd != "inventory" //Default allowed commands.
                    && cmd != "craftlist" && cmd != "drop" && cmd != "main" && cmd != "goto" && cmd != "g" && cmd != "spawn") {

                    if (!CmdsWhitelist.CmdsWhitelist.Commands.Contains(cmd)) {

                        p.SendMessage("Only Op and Op+ can use this command here.");
                        p.cancelcommand = true;
                    }
                }
            }

            string[] args = arg.Split(' ');
            if (cmd == "craft")
            {
                p.cancelcommand = true;
                _cmdCraft.Execute(p, args);
                return;
            }
            else if (cmd == "inventory")
            {
                p.cancelcommand = true;
                _cmdInventory.Execute(p);
                return;
            }
            else if (cmd == "craftlist") {
                p.cancelcommand = true;
                _cmdCraftList.Execute(p, args);
                return;
            } else if (cmd == "drop") {
                p.cancelcommand = true;
                _cmdDrop.Execute(p,args);
                return;
            } else if(cmd == "chest") {
                p.cancelcommand = true;
                _cmdChest.Execute(p,args);
                return;
            }

            
            if (args.Length == 0) { help(p); return; }

            if (cmd == "survival")
            {
                p.cancelcommand = true;
                switch (args[0])
                {
                    case "op":
                        helpOp(p);
                        break;
                    case "maps":
                        _cmdMap.Execute(p, args);
                        break;
                    case "cmds":
                        _cmdCmdsWhitelist.Execute(p, args);
                        break;
                    case "cmdperm":
                        _cmdSetPerms.Execute(p, args);
                        break;
                    case "blocks":
                        _cmdBlock.Execute(p,args);
                        break;
                    case "crafting":
                        _cmdCrafting.Execute(p,args);
                        break;
                    case "tools":
                        _cmdTools.Execute(p, args);
                        break;
                    case "actions":
                        _cmdActionBlocks.Execute(p, args);
                        break;
                    default:
                        help(p);
                        break;
                }
            }
        }

        private void helpOp(Player p) {
            Helpers.SendTextBlockToPlayer(p,
            "Available commands:\n" +
            "%a/survival maps\n" +
            "%a/survival cmds\n" +
            "%a/survival cmdperm\n" +
            "%a/survival blocks\n" +
            "%a/survival crafting\n" +
            "%a/survival tools\n" +
            "%a/survival actions\n"
            );
        }

        private void help(Player p) {
            Helpers.SendTextBlockToPlayer(p,
            "Available commands:\n" +
            "%a/craft [Item] [Multiplier]\n" +
            "%a/craftlist\n"+
            "%a/inventory\n" +
            "%a/drop [Quantity]\n" +
            "%a/chest\n");
        }
    }
}
