using MCGalaxy;
using MCGalaxy.Blocks;
using MCGalaxy.Network;
using System;
using System.Linq;
using System.Collections.Generic;
using Survival.SurvivalBlocks;

namespace Survival
{
    public class SurvivalMob
    {

        public byte ID { get; set; } = 0;

        public int hp { get; set;} = 20;

        protected Position _pos;
        public Position pos { get { return _pos; } }
        protected Orientation _rot;

        private bool _canJump = false;

        protected bool _beenHurted = false;

        private int _refreshClientCount = 0;

        private int _swimCount = 0;

        private string _model = "";
        protected string _curMap = "";

        protected int _speedX = 0;
        protected int _speedY = 0;
        protected int _speedZ = 0;

        protected int _VelX = 0;
        protected int _VelY = 0;
        protected int _VelZ = 0;

        protected int _width = 32;
        protected int _height = 64;

        protected SurvivalPlayers _survivalPlayers;
        protected SurvivalMobs _survivalMobs;

        public SurvivalMob(int X, int Y, int Z, string model, string map,  SurvivalPlayers playersListRef, byte sID, SurvivalMobs mobsListRef) {
            ID = sID;
            _pos = new Position(X,Y,Z);
            _rot = new Orientation();
            _model = model;
            _curMap = map;
            _survivalPlayers = playersListRef;
            _survivalMobs = mobsListRef;
            _speedX = 4;
        }

        public bool HitTestMap() {
            Level map = LevelInfo.FindExact(_curMap);

            int minX = _pos.X/32 - 2;
            int maxX = _pos.X/32 + 2;

            int minY = _pos.Y/32 - 2;
            int maxY = _pos.Y/32 + 2;

            int minZ = _pos.Z/32 - 2;
            int maxZ = _pos.Z/32 + 2;

            for (int i = minY; i <= maxY; i++) {
                for (int j = minX; j <= maxX; j++) {
                    for (int k = minZ; k <= maxZ; k++){
                        if (!CollideType.IsSolid(map.CollideType((map.GetBlock((ushort)(j), (ushort)(i), (ushort)(k)))))) continue;
                        bool touchX = _pos.X + _width > (j * 32) && _pos.X < ((j+1) * 32);
                        bool touchY = (_pos.Y-32) + _height > (i * 32) && (_pos.Y-32) < ((i+1) * 32);
                        bool touchZ = _pos.Z + _width > (k * 32) && _pos.Z < ((k+1) * 32);
                        if (touchX && touchY && touchZ) return true;
                    }
                }
            }

            return false;
        }

        public bool HitTestPlayer(Player p) {
            bool touchX = _pos.X + _width+16 > p.Pos.X + p.ModelBB.BlockMin.X && _pos.X-16 < p.Pos.X + p.ModelBB.BlockMax.X;
            bool touchY = _pos.Y + _height > p.Pos.Y + p.ModelBB.BlockMin.Y && _pos.Y < p.Pos.Y + p.ModelBB.BlockMax.Y;
            bool touchZ = _pos.Z + _width+16 > p.Pos.Z + p.ModelBB.BlockMin.Z && _pos.Z-16 < p.Pos.Z + p.ModelBB.BlockMax.Z;
            if (touchX && touchY && touchZ) return true;
            return false;
        }

        public void createClientEntity() {
            foreach (Player p in _survivalPlayers.PlayerMap.Keys) {
                if (p.level.name != _curMap) continue;
                p.Send(Packet.AddEntity(ID, "", _pos, _rot, true, true));
                p.Send(Packet.ChangeModel(ID, _model, true));
            }
        }

        public void deleteClientEntity(){
            foreach (Player p in _survivalPlayers.PlayerMap.Keys) {
                p.Send(Packet.RemoveEntity(ID));
            }
        }

        public void HurtByPlayer(int damage, Player p) {
            //knockback
            int a = p.Pos.X - _pos.X;
            int b = p.Pos.Z - _pos.Z;

            int angle = (int)(Math.Atan2(b, a));

            _VelX = -(int)(Math.Cos(angle) * 15);
            _VelZ = -(int)(Math.Sin(angle) * 15);
            _VelY = 11;

            //Damage
            Hurt(damage,p);

            _beenHurted = true;
        }

        private void dropItem(Player p) {
            List<SurvivalBlock> bList = new List<SurvivalBlock>();
            foreach (SurvivalBlock b in List.Blocks) {
                if (b.category == _model) {
                    bList.Add(b);
                }
            }
            if (bList.Count == 0) return;

            Random rnd = new Random();
            ushort rndB = bList[rnd.Next(0,bList.Count)].ID;
            Level curLevel = LevelInfo.FindExact(_curMap);
            if(curLevel.IsAirAt((ushort)_pos.BlockX, (ushort)_pos.BlockY, (ushort)_pos.BlockZ)) {
                curLevel.UpdateBlock(p,(ushort)_pos.BlockX, (ushort)_pos.BlockY, (ushort)_pos.BlockZ,rndB);
            }

        }

        private void Hurt(int damage, Player p) {
            hp -= damage;
            if (hp <= 0) {
                _survivalMobs.Remove(ID);
                dropItem(p);
                return;
            }
        }

        public virtual int Update() { return 0; }

        public void Jump() {
            if (_canJump) { 
                _canJump = false;
                _speedY = 9;
            }
        }

        public void UpdatePosition() {

            //Refresh every 30secs to prevent invisible mobs bug
            _refreshClientCount++;
            if (_refreshClientCount >= 1000) {
                _refreshClientCount = 0;
                createClientEntity();
            }

            //Swiming
            Level map = LevelInfo.FindExact(_curMap);
            ushort block = map.GetBlock((ushort)_pos.BlockX, (ushort)_pos.BlockY, (ushort)_pos.BlockZ);
            byte collide = map.CollideType(block);
            Random rnd = new Random();
            if (collide == CollideType.SwimThrough) {
                _swimCount++;
                if(_swimCount > 20) {
                    _speedY=3;
                    if (_swimCount > 60) {
                        _swimCount = 0;
                    }
                } else {
                    _speedY=-1;
                }
            }

            _speedY--;

            if (_VelX > 0) _VelX--;
            if (_VelX < 0) _VelX++;

            if (_VelY > 0) _VelY--;
            if (_VelY < 0) _VelY++;

            if (_VelZ > 0) _VelZ--;
            if (_VelZ < 0) _VelZ++;

            int oldX = _pos.X;
            _pos.X += _speedX+_VelX;
            if (HitTestMap()) {
                _pos.X = oldX;
                Jump();
            }

            int oldY = _pos.Y;
            _pos.Y += _speedY+_VelY;
            if (HitTestMap()) {
                if(_speedY < 0) { 
                    _canJump = true;
                }
                _speedY = 0;
                _pos.Y = oldY;
            }

            int oldZ = _pos.Z;
            _pos.Z += _speedZ+_VelZ;
            if (HitTestMap()) {
                _pos.Z = oldZ;
                Jump();
            }

            foreach (Player p in _survivalPlayers.PlayerMap.Keys) {
                if (p.level.name != _curMap) continue;
                Position fakePos = new Position(_pos.X+16,_pos.Y+17,_pos.Z+16);
                p.Send(Packet.Teleport(ID, fakePos, _rot, true));
            }

        }


    }
}
