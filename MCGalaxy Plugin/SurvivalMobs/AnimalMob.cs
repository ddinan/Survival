using System;
using MCGalaxy;
using MCGalaxy.Maths;

namespace Survival
{
    public class AnimalMob : SurvivalMob
    {

        private int _count = 0;

        public AnimalMob(int X, int Y, int Z, string model, string map, SurvivalPlayers playersListRef, byte sID, SurvivalMobs mobsListRef) 
            : base(X, Y, Z, model, map, playersListRef,sID,mobsListRef) {
        }

        public override int Update()
        {

            Player p = Helpers.getClosestPlayerFrom(_pos.X,_pos.Y,_pos.Z,_survivalPlayers,_curMap);
            if (p != null)
            {
                int dist = Helpers.calculDistance(p.Pos, _pos);
                if (dist > 2048) {
                    _survivalMobs.Remove(ID);
                    return 1;
                }
                if (dist > 256 || !_beenHurted)
                {
                    _count++;
                    if (_count > 100) {
                        _count = 0;
                        Random rnd = new Random();
                        _speedX = rnd.Next(-2,3);
                        _speedZ = rnd.Next(-2,3);
                    }

                    int a2 = _pos.X - _pos.X + _speedX;
                    int b2 = _pos.Z - _pos.Z + _speedZ;

                    Vec3F32 dir2 = new Vec3F32(a2, 0, b2);
                    dir2 = Vec3F32.Normalise(dir2);
                    DirUtils.GetYawPitch(dir2, out _rot.RotY, out _rot.HeadX);

                    UpdatePosition();
                    return 0;
                }

                int a = _pos.X - p.Pos.X;
                int b = _pos.Z - p.Pos.Z;

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
