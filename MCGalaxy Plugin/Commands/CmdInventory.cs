using MCGalaxy;

namespace Survival.Commands
{
    public class CmdInventory
    {

        private SurvivalPlayers _playersList;
        private SurvivalMaps _survivalMaps;

        public CmdInventory(SurvivalPlayers list, SurvivalMaps maps)
        {
            _playersList = list;
            _survivalMaps = maps;
        }

        public void Execute(Player p)
        {
            if (!_survivalMaps.Maps.Contains(p.Level.name)) return;
            Helpers.SendTextBlockToPlayer(p,
                _playersList.PlayerMap[p].getInventoryString());
        }
        
    }
}
