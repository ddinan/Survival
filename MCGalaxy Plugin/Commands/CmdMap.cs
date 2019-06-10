using MCGalaxy;

namespace Survival.Commands
{
    public class CmdMap
    {

        private SurvivalPlayers _playersList;
        private SurvivalMaps _survivalMaps;

        public CmdMap(SurvivalPlayers list, SurvivalMaps maps) {
            _playersList = list;
            _survivalMaps = maps;
        }

        public void Execute(Player p,string[] args) {
            if (p.Rank < CmdsPermissions.get(3)) { p.SendMessage("You don't have the permission to use this command."); return; }
            if (args.Length < 2) { help(p); return; }
            switch (args[1])
            {
                case "add":
                    
                    Player[] online = PlayerInfo.Online.Items;
                    foreach (Player pl in online)
                    {
                        if(pl.level.name == args[2]) _playersList.PlayerMap[pl] = new SurvivalPlayer(pl, _survivalMaps);
                    }

                    _survivalMaps.Maps.AddMap(args[2]);
                    p.SendMessage("Added " + args[2] + ".");
                    break;
                case "remove":
                    _survivalMaps.Maps.RemoveMap(args[2]);
                    p.SendMessage("Removed " + args[2] + ".");
                    break;
                case "list":
                    Helpers.SendTextBlockToPlayer(p,"%eSurvival Maps List:\n" + _survivalMaps.Maps.getListString());
                    break;
                default:
                    help(p);
                    break;
            }
        }

        private void help(Player p) {
            Helpers.SendTextBlockToPlayer(p,
             "------Correct Usage------\n" +
            "%a/survival maps add <name>\n" +
            "%a/survival maps remove <name>\n"+
            "%a/survival maps list\n");
        }

    }

}
