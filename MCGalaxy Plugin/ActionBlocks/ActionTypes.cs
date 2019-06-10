using MCGalaxy;
using System;
using System.Timers;

namespace Survival.ActionBlocks
{
    public class ActionTypes
    {
        public static void Triggering(Player p,byte actionType,string args, ushort x, ushort y, ushort z, SurvivalPlayers survivalPlayers, ushort blockID){
            try{
                string[] argsA = args.Split(' ');
                int offsetX = 0;
                int offsetY = 0;
                int offsetZ = 0;
                switch (actionType) {
                    case 0:
                        p.SendMessage(args);
                        break;
                    case 1:
                        if (argsA.Length > 1) {
                            offsetX = int.Parse(argsA[1]);
                            offsetY = int.Parse(argsA[2]);
                            offsetZ = int.Parse(argsA[3]);
                            if (argsA.Length > 4) {
                                string[] BlocksConditions = argsA[4].Split(',');
                                bool pass = false;
                                for (int i = 0; i < BlocksConditions.Length; i++) {
                                    ushort b = Helpers.IDConvert(ushort.Parse(BlocksConditions[i]));
                                    if (b == p.level.GetBlock((ushort)(x + offsetX), (ushort)(y + offsetY), (ushort)(z + offsetZ))) {
                                        pass = true;
                                    }
                                }
                                if (!pass) return;
                            }
                        }
                        
                        if (argsA.Length > 5) {
                            if (bool.Parse(argsA[5])) {
                                offsetX = 0;
                                offsetY = 0;
                                offsetZ = 0;
                            }
                        }

                        p.level.UpdateBlock(p, (ushort)(x + offsetX), (ushort)(y + offsetY), (ushort)(z + offsetZ), ushort.Parse(argsA[0]));
                        break;
                    case 2:
                        offsetX = int.Parse(argsA[0]);
                        offsetY = int.Parse(argsA[1]);
                        offsetZ = int.Parse(argsA[2]);

                        string[] BConditions = argsA[3].Split(',');
                        bool Bpass = false;
                        for (int i = 0; i < BConditions.Length; i++)
                        {
                            ushort b = Helpers.IDConvert(ushort.Parse(BConditions[i]));
                            if (b == p.level.GetBlock((ushort)(x + offsetX), (ushort)(y + offsetY), (ushort)(z + offsetZ)))
                            {
                                Bpass = true;
                            }
                        }
                        if (!Bpass) return;

                        ushort BlockAt = p.level.GetBlock((ushort)(x + offsetX), (ushort)(y + offsetY), (ushort)(z + offsetZ));
                        if (ActionsBlocks.isActionBlockTrigger(BlockAt, 4))
                        {
                            Timer t = new Timer();
                            t.Interval = 100; //In milliseconds here
                            t.AutoReset = true; //Stops it from repeating
                            t.Elapsed += (sender, e) => DoAction2(sender, e, p,BlockAt,(ushort)(x + offsetX), (ushort)(y + offsetY), (ushort)(z + offsetZ));
                            t.Start();

                           
                        }
                        break;
                    case 3:
                    //---------------------------
                        ushort[] blocks = { ushort.Parse(argsA[0]), ushort.Parse(argsA[1]), ushort.Parse(argsA[2]) };
                        int[] pos = { 0, 0, 0, 0 }; //N,W,S,E

                        ushort uB = blocks[0];

                        for (int cx = -1; cx <= 1; cx++) {
                            for (int cz = -1; cz <= 1; cz++)
                            {
                                if (cx == 0 && cz == 0) continue;
                                if (cx + cz == 1 || cx + cz == -1) {
                                    for (int i = 0; i < blocks.Length; i++) {
                                        if (p.level.GetBlock((ushort)(x + cx), y, (ushort)(z + cz)) == blocks[i]) {
                                            if (cx == 1) pos[1] = 1;
                                            if (cx == -1) pos[3] = 1;

                                            if (cz == 1) pos[0] = 1;
                                            if (cz == -1) pos[2] = 1;
                                        }
                                    }
                                }
                            }
                        }

                        string sPos = pos[0] + " " + pos[1] + " " + pos[2] + " " + pos[3];

                        if (sPos == "1 0 0 0" || sPos == "1 0 1 0" || sPos == "0 0 1 0") {
                            uB = blocks[1];
                        }
                        else if(sPos == "0 1 0 0" || sPos == "0 1 0 1" || sPos == "0 0 0 1") {
                            
                            uB = blocks[2];
                        }

                        p.level.UpdateBlock(p, x, y, z , uB);
                        break;
                    //-------------------------------------------
                    case 4:
                        ushort id = Helpers.IDConvert(ushort.Parse(argsA[0]));
                        int hp = int.Parse(argsA[1]);
                        
                        SurvivalPlayer sp = survivalPlayers.PlayerMap[p];

                        if (sp.haveItem(id, 1)) {
                            sp.Consume(id,1);
                            sp.AddHp(hp);
                        }
                        break;
                    case 5:
                        SurvivalPlayer sp2 = survivalPlayers.PlayerMap[p];

                        string folderPath = "./text/survivalPlugin/chests/" + p.level.name + "/";
                        string chestPath = folderPath + p.truename + "-" + x + "," + y + "," + z;
                        if (!System.IO.File.Exists(chestPath)) {
                            Inventory rawInv = new Inventory(27);
                            rawInv.Save(chestPath);
                            p.SendMessage("You are now owning this chest.");
                        }

                        sp2.SelectChest(x,y,z);
                        break;
                    case 6:
                        SurvivalPlayer sp3 = survivalPlayers.PlayerMap[p];

                        string folderPath2 = "./text/survivalPlugin/chests/" + p.level.name + "/";
                        string chestPath2 = folderPath2 + p.truename + "-" + x + "," + y + "," + z;
                        if (System.IO.File.Exists(chestPath2))
                        {
                            System.IO.File.Delete(chestPath2);
                            sp3.chestSelected = new Position(-1,-1,-1);
                        }
                        else {
                            p.SendMessage("You are not owning this chest.");
                            p.cancelBlock = true;
                            p.RevertBlock(x,y,z);
                            sp3.Consume(Helpers.getNorthBlock(blockID,p), 1);         
                        }
                        break;
                    default:
                        p.SendMessage("Invalid Action.");
                        break;
                }
            }
            catch { p.SendMessage("Invalid arguments."); }
        }

        private static void DoAction2(object sender, ElapsedEventArgs e,Player p,ushort BlockAt,ushort x,ushort y, ushort z)
        {
            ActionsBlocks.Triggering(p, BlockAt, 4, x,y,z);
        }
    }
}
