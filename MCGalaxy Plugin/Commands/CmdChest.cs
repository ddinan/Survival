using MCGalaxy;

namespace Survival.Commands
{
    public class CmdChest
    {

        private SurvivalPlayers _playersList;
        private SurvivalMaps _survivalMaps;

        public CmdChest(SurvivalPlayers list, SurvivalMaps maps)
        {
            _playersList = list;
            _survivalMaps = maps;
        }

        private Inventory GetChest(Player p) {
            SurvivalPlayer sP = _playersList.PlayerMap[p];
            Inventory cInv = new Inventory(27);
            string folderPath = "./text/survivalPlugin/chests/" + p.level.name + "/";
            string chestPath = folderPath + p.truename + "-" + sP.chestSelected.X + "," + sP.chestSelected.Y + "," + sP.chestSelected.Z;
            if (!cInv.Load(chestPath))
            {
                p.Message("Chest not owned by you!");
                return null;
            }
            return cInv;
        }

        private void SaveChest(Player p,Inventory inv) {
            SurvivalPlayer sP = _playersList.PlayerMap[p];
            string folderPath = "./text/survivalPlugin/chests/" + p.level.name + "/";
            string chestPath = folderPath + p.truename + "-" + sP.chestSelected.X + "," + sP.chestSelected.Y + "," + sP.chestSelected.Z;
           
            if (System.IO.File.Exists(chestPath))
            {
                inv.Save(chestPath);
            }
        }

        private bool checkChestDist(SurvivalPlayer sP) {
            int dist = Helpers.calculDistance(sP.chestSelected,new Position(sP.thisPlayer.Pos.BlockX, sP.thisPlayer.Pos.BlockY,sP.thisPlayer.Pos.BlockZ));
            return dist < 8;
            
        }

        public void Execute(Player p,string[] args)
        {
            if (!_survivalMaps.Maps.Contains(p.Level.name)) return;

            SurvivalPlayer sP = _playersList.PlayerMap[p];
            try {
                switch (args[0]) {
                    case "see":
                        if (sP.chestSelected.X == -1)
                        {
                            p.Message("First, select a chest. (Right click on it)");
                            break;
                        }
                        if (!checkChestDist(sP))
                        {
                            p.Message("Too far from the selected chest.");
                            break;
                        }
                        Inventory cInv = GetChest(p);
                        if (cInv == null) return;
                        p.Message(cInv.getString(p));
                        break;
                    case "place":
                        if (sP.chestSelected.X == -1)
                        {
                            p.Message("First, select a chest. (Right click on it)");
                            break;
                        }
                        if (!checkChestDist(sP))
                        {
                            p.Message("Too far from the selected chest.");
                            break;
                        }
                        ushort id = Block.Parse(p, args[1]);
                        int quantity = int.Parse(args[2]);

                        Inventory cInv2 = GetChest(p);
                        if (cInv2 == null) return;

                        if (quantity <= 0) return;
                        if (sP.haveItem(id, quantity)) {
                            int durability = sP.getDurability(id);
                            if (!cInv2.AddItem(id, quantity, durability)){
                                p.Message("Chest Full.");
                                break;
                            }
                            sP.Consume(id, quantity);
                            SaveChest(p,cInv2);
                        }
                        break;
                    case "take":
                        if (sP.chestSelected.X == -1)
                        {
                            p.Message("First, select a chest. (Right click on it)");
                            break;
                        }
                        if (!checkChestDist(sP))
                        {
                            p.Message("Too far from the selected chest.");
                            break;
                        }
                        ushort id2 = Block.Parse(p,args[1]);
                        int quantity2 = int.Parse(args[2]);

                        Inventory cInv3 = GetChest(p);
                        if (cInv3 == null) return;

                        if (quantity2 <= 0) return;
                        if (cInv3.contains(id2, quantity2))
                        {
                            int durability = cInv3.getDurability(id2);
                            if (!sP.Collect(id2, quantity2, durability)) {
                                p.Message("Inventory Full.");
                                break;
                            }
                            cInv3.RemoveItem(id2, quantity2);
                            SaveChest(p, cInv3);
                        }

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
            "%a/chest see\n" +
            "%a/chest take <block> <quantity>\n" +
            "%a/chest place <block> <quantity>\n");
        }

    }
}
