// Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
using System;
using ClassicalSharp.Gui.Screens;
using ClassicalSharp.Entities;
using OpenTK;
using System.Text;
#if __MonoCS__
using Ionic.Zlib;
#else
using System.IO.Compression;
#endif

#if USE16_BIT
using BlockID = System.UInt16;
#else
using BlockID = System.Byte;
#endif

namespace ClassicalSharp.Network.Protocols
{

    /// <summary> Implements the packets for the original classic. </summary>
    public sealed class ClassicProtocol : IProtocol
    {

        public ClassicProtocol(Game game) : base(game) { }

        public override void Init()
        {
            gzippedMap = new FixedBufferStream(net.reader.buffer);
            Reset();
        }

        public override void Reset()
        {
            
            net.Set(Opcode.Handshake, HandleHandshake, 131);
            net.Set(Opcode.Ping, HandlePing, 1);
            net.Set(Opcode.LevelInit, HandleLevelInit, 1);
            net.Set(Opcode.LevelDataChunk, HandleLevelDataChunk, 1028);
            net.Set(Opcode.LevelFinalise, HandleLevelFinalise, 7);
            net.Set(Opcode.SetBlock, HandleSetBlock, 8);

            net.Set(Opcode.AddEntity, HandleAddEntity, 74);
            net.Set(Opcode.EntityTeleport, HandleEntityTeleport, 10);
            net.Set(Opcode.RelPosAndOrientationUpdate, HandleRelPosAndOrientationUpdate, 7);
            net.Set(Opcode.RelPosUpdate, HandleRelPositionUpdate, 5);
            net.Set(Opcode.OrientationUpdate, HandleOrientationUpdate, 4);
            net.Set(Opcode.RemoveEntity, HandleRemoveEntity, 2);

            net.Set(Opcode.Message, HandleMessage, 66);
            net.Set(Opcode.loadinventory, loadinventory, 66);
            net.Set(Opcode.setinventory, setinventory, 5);
            net.Set(Opcode.Kick, HandleKick, 65);
            net.Set(Opcode.SetPermission, HandleSetPermission, 2);
        }

        DateTime mapReceiveStart;
        DeflateStream gzipStream;
        GZipHeaderReader gzipHeader;
        int mapSizeIndex, mapIndex;
        byte[] mapSize = new byte[4], map;
        FixedBufferStream gzippedMap;
        Screen prevScreen;
        bool prevCursorVisible;

        byte[] hotbarsave = new byte[100];


        string hotbardatasend;
        string hotbardatacountsend;
        string hotbardurasend;

        string inventorydatasend;
        string inventorydatacountsend;
        string inventorydurasend;

        string inventorydatasend2;
        string inventorydatacountsend2;
        string inventorydurasend2;

        string inventorydatasend3;
        string inventorydatacountsend3;
        string inventorydurasend3;

        void saveinventory()
        {
            hotbardatasend = "";
            hotbardatacountsend = "";
            hotbardurasend = "";

            inventorydatasend = "";
            inventorydatacountsend = "";
            inventorydurasend = "";

            inventorydatasend2 = "";
            inventorydatacountsend2 = "";
            inventorydurasend2 = "";

            inventorydatasend3 = "";
            inventorydatacountsend3 = "";
            inventorydurasend3 = "";

            if (game.Server.IsSinglePlayer)
            {
                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "Inventory.txt", Mode.survivalinventory.invBlocks);
                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "Inventorycount.txt", Mode.survivalinventory.inv2Count);



                for (int i = 0; i < 9; i++)
                {

                    hotbarsave[i] = game.Inventory[i];

                }

                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "hotbar.txt", hotbarsave);
                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "hotbarcount.txt", Mode.survivalinventory.invCount);
            }
            else
            {

                for(int i = 0; i < 9; i++)
                {

                    hotbardatasend = hotbardatasend + game.Inventory[i] + ";";
                    hotbardatacountsend = hotbardatacountsend + Mode.survivalinventory.invCount[i] + ";";
                    hotbardurasend = hotbardurasend + Mode.survivalinventory.invdurability[i] + ";";



                }

                game.Server.SendInventory(hotbardatasend, 0);
                game.Server.SendInventory(hotbardatacountsend, 1);
                game.Server.SendInventory(hotbardurasend, 4);

                for (int i = 0; i < 10; i++)
                {


                    inventorydatasend = inventorydatasend + Mode.survivalinventory.invBlocks[i] + ";";
                    inventorydatacountsend = inventorydatacountsend + Mode.survivalinventory.inv2Count[i] + ";";
                    inventorydurasend = inventorydurasend + Mode.survivalinventory.inv2durability[i] + ";";

                    inventorydatasend2 = inventorydatasend2 + Mode.survivalinventory.invBlocks[i + 10] + ";";
                    inventorydatacountsend2 = inventorydatacountsend2 + Mode.survivalinventory.inv2Count[i + 10] + ";";
                    inventorydurasend2 = inventorydurasend2 + Mode.survivalinventory.inv2durability[i + 10] + ";";

                    inventorydatasend3 = inventorydatasend3 + Mode.survivalinventory.invBlocks[i + 20] + ";";
                    inventorydatacountsend3 = inventorydatacountsend3 + Mode.survivalinventory.inv2Count[i + 20] + ";";
                    inventorydurasend3 = inventorydurasend3 + Mode.survivalinventory.inv2durability[i + 20] + ";";


                }



                game.Server.SendInventory(inventorydatasend, 2);
                game.Server.SendInventory(inventorydatacountsend, 3);
                game.Server.SendInventory(inventorydurasend, 5);

                game.Server.SendInventory(inventorydatasend2, 6);
                game.Server.SendInventory(inventorydatacountsend2, 7);
                game.Server.SendInventory(inventorydurasend2, 8);

                game.Server.SendInventory(inventorydatasend3, 9);
                game.Server.SendInventory(inventorydatacountsend3, 10);
                game.Server.SendInventory(inventorydurasend3, 11);
            }

        }



        #region Read

        void HandleHandshake()
        {
            byte protocolVer = reader.ReadUInt8();
            net.ServerName = reader.ReadString();
            net.ServerMotd = reader.ReadString();
            game.Chat.SetLogName(net.ServerName);

            game.LocalPlayer.Hacks.SetUserType(reader.ReadUInt8());
            game.LocalPlayer.Hacks.ParseHackFlags(net.ServerName, net.ServerMotd);
            game.LocalPlayer.CheckHacksConsistency();
            game.Events.RaiseHackPermissionsChanged();
        }

        void HandlePing() { }

        void HandleLevelInit()
        {
            if (gzipStream != null) return;
            game.World.Reset();
            prevScreen = game.Gui.activeScreen;
            if (prevScreen is LoadingMapScreen)
                prevScreen = null;
            prevCursorVisible = game.CursorVisible;

            game.Gui.SetNewScreen(new LoadingMapScreen(game, net.ServerName, net.ServerMotd), false);
            net.wom.CheckMotd();
            net.receivedFirstPosition = false;
            gzipHeader = new GZipHeaderReader();

            // Workaround because built in mono stream assumes that the end of stream
            // has been reached the first time a read call returns 0. (MS.NET doesn't)
#if __MonoCS__
			gzipStream = new DeflateStream(gzippedMap, true);
#else
            gzipStream = new DeflateStream(gzippedMap, CompressionMode.Decompress);
            if (OpenTK.Configuration.RunningOnMono)
            {
                throw new InvalidOperationException("You must compile ClassicalSharp with __MonoCS__ defined " +
                                                    "to run on Mono, due to a limitation in Mono.");
            }
#endif

            mapSizeIndex = 0;
            mapIndex = 0;
            mapReceiveStart = DateTime.UtcNow;
            net.task.Interval = 1.0 / 60;
        }

        void HandleLevelDataChunk()
        {
            // Workaround for some servers that send LevelDataChunk before LevelInit
            // due to their async packet sending behaviour.
            if (gzipStream == null) HandleLevelInit();
            int usedLength = reader.ReadUInt16();
            gzippedMap.pos = 0;
            gzippedMap.bufferPos = reader.index;
            gzippedMap.len = usedLength;

            if (gzipHeader.done || gzipHeader.ReadHeader(gzippedMap))
            {
                if (mapSizeIndex < 4)
                {
                    mapSizeIndex += gzipStream.Read(mapSize, mapSizeIndex, 4 - mapSizeIndex);
                }

                if (mapSizeIndex == 4)
                {
                    if (map == null)
                    {
                        int size = mapSize[0] << 24 | mapSize[1] << 16 | mapSize[2] << 8 | mapSize[3];
                        map = new byte[size];
                    }
                    mapIndex += gzipStream.Read(map, mapIndex, map.Length - mapIndex);
                }
            }

            reader.Skip(1025); // also skip progress since we calculate client side
            float progress = map == null ? 0 : (float)mapIndex / map.Length;
            game.WorldEvents.RaiseMapLoading(progress);
        }

        void HandleLevelFinalise()
        {
            net.task.Interval = 1.0 / 20;
            game.Gui.SetNewScreen(null);
            game.Gui.activeScreen = prevScreen;
            if (prevScreen != null && prevCursorVisible != game.CursorVisible)
            {
                game.CursorVisible = prevCursorVisible;
            }
            prevScreen = null;

            int mapWidth = reader.ReadUInt16();
            int mapHeight = reader.ReadUInt16();
            int mapLength = reader.ReadUInt16();

            double loadingMs = (DateTime.UtcNow - mapReceiveStart).TotalMilliseconds;
            Utils.LogDebug("map loading took: " + loadingMs);

#if USE16_BIT
			game.World.SetNewMap(Utils.UInt8sToUInt16s(map), mapWidth, mapHeight, mapLength);
#else
            game.World.SetNewMap(map, mapWidth, mapHeight, mapLength);
#endif
            game.WorldEvents.RaiseOnNewMapLoaded();

            map = null;
            gzipStream.Dispose();
            net.wom.CheckSendWomID();
            gzipStream = null;
            GC.Collect();
        }

        void HandleSetBlock()
        {
            int x = reader.ReadUInt16();
            int y = reader.ReadUInt16();
            int z = reader.ReadUInt16();
            byte block = reader.ReadUInt8();

#if DEBUG_BLOCKS
			if (game.World.IsNotLoaded) {
				Utils.LogDebug("Server tried to update a block while still sending us the map!");
			} else if (!game.World.IsValidPos(x, y, z)) {
				Utils.LogDebug("Server tried to update a block at an invalid position!");
			} else {
				game.UpdateBlock(x, y, z, block);
			}
#else
            if (!game.World.IsNotLoaded && game.World.IsValidPos(x, y, z))
            {
                game.UpdateBlock(x, y, z, block);
            }
#endif
        }

        void HandleAddEntity()
        {
            byte id = reader.ReadUInt8();
            string name = reader.ReadString();
            string skin = name;
            net.CheckName(id, ref name, ref skin);
            net.AddEntity(id, name, skin, true);

            if (!net.addEntityHack) return;
            // Workaround for some servers that declare they support ExtPlayerList,
            // but doesn't send ExtAddPlayerName packets.
            net.AddTablistEntry(id, name, name, "Players", 0);
            net.needRemoveNames[id >> 3] |= (byte)(1 << (id & 0x7));
        }

        void HandleEntityTeleport()
        {
            byte id = reader.ReadUInt8();
            ReadAbsoluteLocation(id, true);
        }

        void HandleRelPosAndOrientationUpdate()
        {
            byte id = reader.ReadUInt8();
            float x = reader.ReadInt8() / 32f;
            float y = reader.ReadInt8() / 32f;
            float z = reader.ReadInt8() / 32f;

            float rotY = (float)Utils.PackedToDegrees(reader.ReadUInt8());
            float headX = (float)Utils.PackedToDegrees(reader.ReadUInt8());
            LocationUpdate update = LocationUpdate.MakePosAndOri(x, y, z, rotY, headX, true);
            net.UpdateLocation(id, update, true);
        }

        void HandleRelPositionUpdate()
        {
            
            byte id = reader.ReadUInt8();
            float x = reader.ReadInt8() / 32f;
            float y = reader.ReadInt8() / 32f;
            float z = reader.ReadInt8() / 32f;

            LocationUpdate update = LocationUpdate.MakePos(x, y, z, true);
            net.UpdateLocation(id, update, true);
        }

        void HandleOrientationUpdate()
        {
            byte id = reader.ReadUInt8();
            float rotY = (float)Utils.PackedToDegrees(reader.ReadUInt8());
            float headX = (float)Utils.PackedToDegrees(reader.ReadUInt8());

            LocationUpdate update = LocationUpdate.MakeOri(rotY, headX);
            net.UpdateLocation(id, update, true);
        }

        void HandleRemoveEntity()
        {
            byte id = reader.ReadUInt8();
            net.RemoveEntity(id);
        }

        void HandleMessage()
        {
            byte type = reader.ReadUInt8();
            // Original vanilla server uses player ids in message types, 255 for server messages.
            bool prepend = !net.cpeData.useMessageTypes && type == 0xFF;

            if (!net.cpeData.useMessageTypes) type = (byte)MessageType.Normal;
            string text = reader.ReadChatString(ref type);
            if (prepend) text = "&e" + text;

            if (!text.StartsWith("^detail.user", StringComparison.OrdinalIgnoreCase))
                game.Chat.Add(text, (MessageType)type);
        }

        void setinventory()
        {



            byte invnb = reader.ReadUInt8();
            byte invslot = reader.ReadUInt8();
            byte blockid = reader.ReadUInt8();
            byte blocknb = reader.ReadUInt8();

            if (invnb == 0)
            {
                game.Inventory[invslot] = blockid;
                Mode.survivalinventory.invCount[invslot] = blocknb;

            }

            if (invnb == 1)
            {
                Mode.survivalinventory.invBlocks[invslot] = blockid;
                Mode.survivalinventory.inv2Count[invslot] = blocknb;

            }

            saveinventory();
            
        }

        void loadinventory()
        {
            byte invnb = reader.ReadUInt8();
            string inv = reader.ReadString();

            

            if (invnb == 0)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9;i++)
                {
                    game.Inventory[i] = byte.Parse(hotbar[i]);
                }
            }


            if (invnb == 1)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.invCount[i] = byte.Parse(hotbar[i]);
                }
            }

            if (invnb == 4)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.invdurability[i] = byte.Parse(hotbar[i]);
                }
            }




            if (invnb == 2)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.invBlocks[i] = byte.Parse(hotbar[i]);
                }
            }

            if (invnb == 3)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.inv2Count[i] = byte.Parse(hotbar[i]);
                }
            }

            

            if (invnb == 5)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.inv2durability[i] = byte.Parse(hotbar[i]);
                }
            }






            if (invnb == 9)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.invBlocks[i+9] = byte.Parse(hotbar[i]);
                }
            }

            if (invnb == 10)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.inv2Count[i+9] = byte.Parse(hotbar[i]);
                }
            }



            if (invnb == 11)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.inv2durability[i+9] = byte.Parse(hotbar[i]);
                }
            }




            if (invnb == 12)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                

                for (int i = 0; i < 10; i++)
                {
                    
                    Mode.survivalinventory.invBlocks[i+18] = byte.Parse(hotbar[i]);
                }
               
            }

            if (invnb == 13)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 10; i++)
                {
                    Mode.survivalinventory.inv2Count[i+18] = byte.Parse(hotbar[i]);
                }
            }



            if (invnb == 14)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');

                for (int i = 0; i < 10; i++)
                {
                    Mode.survivalinventory.inv2durability[i+18] = byte.Parse(hotbar[i]);
                }
               
            }





            //chests

           

            if (invnb == 6)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.chestBlocks[i] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 15)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.chestBlocks[i+9] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 16)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 10; i++)
                {
                    Mode.survivalinventory.chestBlocks[i + 18] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 7)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.chestCount[i] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 17)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.chestCount[i+9] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 18)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 10; i++)
                {
                    Mode.survivalinventory.chestCount[i + 18] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 8)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.chestdurability[i] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 19)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 9; i++)
                {
                    Mode.survivalinventory.chestdurability[i+9] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

            if (invnb == 20)
            {

                byte[] saveBytes = Encoding.UTF8.GetBytes(inv);
                string[] hotbar = Encoding.UTF8.GetString(saveBytes).Split(';');


                for (int i = 0; i < 10; i++)
                {
                    Mode.survivalinventory.chestdurability[i + 18] = byte.Parse(hotbar[i]);
                }

                Mode.survivalinventory.iscrafting = false;
                Mode.survivalinventory.ischest = true;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));

            }

        }

        void HandleKick()
        {
            string reason = reader.ReadString();
            game.Disconnect("&eLost connection to the server", reason);
            net.Dispose();
        }

        void HandleSetPermission()
        {
            game.LocalPlayer.Hacks.SetUserType(reader.ReadUInt8());
        }

        internal void ReadAbsoluteLocation(byte id, bool interpolate)
        {
            Vector3 P = reader.ReadPosition(id);
            float rotY = (float)Utils.PackedToDegrees(reader.ReadUInt8());
            float headX = (float)Utils.PackedToDegrees(reader.ReadUInt8());

            if (id == EntityList.SelfID) net.receivedFirstPosition = true;
            LocationUpdate update = LocationUpdate.MakePosAndOri(P, rotY, headX, false);
            net.UpdateLocation(id, update, interpolate);
        }
        #endregion

        #region Write

        internal void SendChat(string text, bool partial)
        {
            int payload = !net.SupportsPartialMessages ? 0xFF : (partial ? 1 : 0);
            writer.WriteUInt8((byte)Opcode.Message);

            writer.WriteUInt8((byte)payload);
            writer.WriteString(text);
            net.SendPacket();
        }

        internal void SendInventory(string inv, byte invnb) //++++++++++++
        {
            writer.WriteUInt8((byte)Opcode.SendInventory);

            writer.WriteUInt8(invnb);
            writer.WriteString(inv);
            net.SendPacket();
            
        }

        internal void SendChests(string inv, byte invnb) //++++++++++++
        {
            writer.WriteUInt8((byte)Opcode.SendChests);

            writer.WriteUInt8(invnb);
            writer.WriteString(inv);
            writer.WriteString(game.SelectedPos.BlockPos + "");
            net.SendPacket();

        }

        internal void SendPosition(Vector3 pos, float rotY, float headX)
        {
            int payload = net.cpeData.sendHeldBlock ? game.Inventory.Selected : 0xFF;
            writer.WriteUInt8((byte)Opcode.EntityTeleport);

            writer.WriteUInt8((byte)payload); // held block when using HeldBlock, otherwise just 255
            writer.WritePosition(pos);
            writer.WriteUInt8((byte)Utils.DegreesToPacked(rotY));
            writer.WriteUInt8((byte)Utils.DegreesToPacked(headX));
            net.SendPacket();
        }

        internal void SendSetBlock(int x, int y, int z, bool place, BlockID block)
        {
            writer.WriteUInt8((byte)Opcode.SetBlockClient);

            writer.WriteInt16((short)x);
            writer.WriteInt16((short)y);
            writer.WriteInt16((short)z);
            writer.WriteUInt8(place ? (byte)1 : (byte)0);

#if USE16_BIT
			writer.WriteUInt8((byte)block);
#else
            writer.WriteUInt8(block);
#endif
            net.SendPacket();
        }

        internal void SendLogin(string username, string verKey)
        {
            byte payload = game.UseCPE ? (byte)0x42 : (byte)0x00;
            writer.WriteUInt8((byte)Opcode.Handshake);

            writer.WriteUInt8(7); // protocol version
            writer.WriteString(username);
            writer.WriteString(verKey);
            writer.WriteUInt8(payload);
            net.SendPacket();
        }

        #endregion
    }
}