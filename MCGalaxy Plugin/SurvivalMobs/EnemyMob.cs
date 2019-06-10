using System;
using MCGalaxy;
using MCGalaxy.Maths;

namespace Survival
{
    public class EnemyMob : SurvivalMob
    {

        private int _attackCount = 0;

        public EnemyMob(int X, int Y, int Z, string model, string map, SurvivalPlayers playersListRef, byte sID, SurvivalMobs mobsListRef) 
            : base(X, Y, Z, model, map, playersListRef,sID,mobsListRef) {
        }

        private void Attack() {
            foreach (Player pl in _survivalPlayers.PlayerMap.Keys) {
                SurvivalPlayer sPl = _survivalPlayers.PlayerMap[pl];
                int dist = Helpers.calculDistance(pl.Pos, _pos);
                if (dist > _width+_height) continue;
                if (HitTestPlayer(pl)) {
                    sPl.SubHp(1);
                    if (sPl.getHp() <= 0) sPl.killPlayer("has been killed by a monster.");
                }
            }
        }

        public override int Update()
        {
            _attackCount++;
            if (_attackCount > 15) {
                _attackCount = 0;
                Attack();
            }

            Player p = Helpers.getClosestPlayerFrom(_pos.X,_pos.Y,_pos.Z,_survivalPlayers,_curMap);
            if (p != null)
            {
                int dist = Helpers.calculDistance(p.Pos, _pos);
                if (dist > 2048) {
                    _survivalMobs.Remove(ID);
                    return 1;
                }
                if (dist > 740)
                {
                    _speedX = 0;
                    _speedZ = 0;
                    UpdatePosition();
                    return 0;
                }

                int a = p.Pos.X - _pos.X;
                int b = p.Pos.Z - _pos.Z;

                int angle = (int)(Math.Atan2(b, a));

                _speedX = (int)(Math.Cos(angle) * 5);
                _speedZ = (int)(Math.Sin(angle) * 5);

                Vec3F32 dir = new Vec3F32(a, 0, b);
                dir = Vec3F32.Normalise(dir);
                DirUtils.GetYawPitch(dir, out _rot.RotY, out _rot.HeadX);
            }
            UpdatePosition();
            return 0;
        }

    }
}
