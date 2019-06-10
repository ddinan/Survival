using MCGalaxy;
using System;
using System.Collections.Generic;

namespace Survival
{
    public class MobsSpawningHandler
    {

        private SurvivalMobs _survivalMobs;
        private SurvivalPlayers _survivalPlayers;
        private SurvivalTime _survivalTime;

        private readonly string[] monsterModels = { "zombie", "skeleton","spider" };
        private readonly string[] animalModels = { "pig", "sheep" };

        public MobsSpawningHandler(SurvivalMobs mobsListRef, SurvivalPlayers playersListRef, SurvivalTime time) {
            _survivalMobs = mobsListRef;
            _survivalPlayers = playersListRef;
            _survivalTime = time;
        }

        private List<Level> getLevelsList() {
            List<Level> lvls = new List<Level>();
            
            //Build Survival Level List in wich there are players
            foreach (Player p in _survivalPlayers.PlayerMap.Keys)
            {
                if (lvls.FindIndex(x => x == p.level) != -1) continue;
                lvls.Add(p.level);
            }

            return lvls;
        }

        private int getMobCountAround(int x, int z) {
            //Verify mob count
            int mobCount = 0;
            foreach (SurvivalMob mob in _survivalMobs.list)
            {
                int a = x - mob.pos.BlockX;
                int b = z - mob.pos.BlockZ;

                int dist = (int)Math.Sqrt(a * a + b * b);

                if (dist <= 50) mobCount++;
            }
            return mobCount;
        }

        public void DoSpawning() {

            Random rnd = new Random();
            if (rnd.Next(0, 4) == 0) {
                DoRandomEnemySpawn();
            }
            else {
                if(_survivalTime.curTime == 0) {
                    DoRandomAnimalSpawn();
                }
            }
        }

        public void DoRandomEnemySpawn() {

            Random rnd = new Random();

            List<Level> lvls = getLevelsList();
            if (lvls.Count == 0) return;

            //Get random level in wich the spawn will be done
            Level curLvl = lvls[rnd.Next(0, lvls.Count)];

            //Random X & Z
            ushort rx = (ushort)rnd.Next(0, curLvl.Length);
            ushort rz = (ushort)rnd.Next(0, curLvl.Width);

            int limitAround = 5;
            if (_survivalTime.curTime != 0) {
                limitAround = 7;
            }

            if (getMobCountAround(rx,rz) > limitAround) return;

            //Get Y
            ushort ry = 999;

            for (ushort i = 0; i < curLvl.Height; i++) {
                if (curLvl.IsAirAt(rx, i, rz) && curLvl.IsAirAt(rx, (ushort)(i + 1), rz)) {
                    ry = (ushort)(i+2);
                    break;
                }
            }
            if (ry == 999) return;

            //Verify distance with neareast player (Don't spawn if too close or too far (15 - 64 blocks))
            Player closestP = Helpers.getClosestPlayerFrom(rx, ry, rz, _survivalPlayers, curLvl.name);
            int distP = Helpers.calculDistance(closestP.Pos, new Position(rx * 32, ry * 32, rz * 32));
            if (distP < 480 || distP > 2048) return;

            //Random Monster
            string curModel = monsterModels[rnd.Next(0,monsterModels.Length)];
            if (_survivalTime.curTime == 0) curModel = "zombie"; //Only Zombie at day time

            //Add
            _survivalMobs.Add(rx,ry,rz, curModel, curLvl.name,0,20);
        }

        public void DoRandomAnimalSpawn() {
            Random rnd = new Random();

            List<Level> lvls = getLevelsList();
            if (lvls.Count == 0) return;

            //Get random level in wich the spawn will be done
            Level curLvl = lvls[rnd.Next(0, lvls.Count)];

            //Random X & Z
            ushort rx = (ushort)rnd.Next(0, curLvl.Length);
            ushort rz = (ushort)rnd.Next(0, curLvl.Width);

            if (getMobCountAround(rx, rz) > 5) return;

            //Get Y
            ushort ry = 999;

            for (ushort i = curLvl.Height; i > 0; i--)
            {
                if (curLvl.GetBlock(rx, i, rz) == 2)
                {
                    ry = (ushort)(i + 3);
                    break;
                }
            }
            if (ry == 999) return;

            //Verify distance with neareast player (Don't spawn if too close or too far (15 - 64 blocks))
            Player closestP = Helpers.getClosestPlayerFrom(rx, ry, rz, _survivalPlayers, curLvl.name);
            int distP = Helpers.calculDistance(closestP.Pos, new Position(rx * 32, ry * 32, rz * 32));
            if (distP < 480 || distP > 2048) return;

            //Random Animal
            string curModel = animalModels[rnd.Next(0, animalModels.Length)];

            //Add
            _survivalMobs.Add(rx, ry, rz, curModel, curLvl.name, 1, 10);

        }


    }
}
