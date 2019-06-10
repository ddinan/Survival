using MCGalaxy;

namespace Survival.Commands
{
    public class CmdDrop
    {

        private SurvivalPlayers _playersList;
        private SurvivalMaps _survivalMaps;

        public CmdDrop(SurvivalPlayers list, SurvivalMaps maps)
        {
            _playersList = list;
            _survivalMaps = maps;
        }

        public void Execute(Player p,string[] args)
        {
            if (!_survivalMaps.Maps.Contains(p.Level.name)) return;

            int dropQuantity = 1;
            if (args.Length > 0) {
                dropQuantity = int.TryParse(args[0], out dropQuantity) ? int.Parse(args[0]) : 1;
            }

            if (dropQuantity < 1) return;

            _playersList.PlayerMap[p].Consume(p.GetHeldBlock(),dropQuantity);
        }

    }
}
