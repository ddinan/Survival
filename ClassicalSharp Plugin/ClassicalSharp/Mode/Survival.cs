// Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
using System;
using ClassicalSharp.Entities;
using ClassicalSharp.Entities.Mobs;
using ClassicalSharp.Gui.Widgets;
using ClassicalSharp.Gui.Screens;
using OpenTK;
using OpenTK.Input;
using System.Timers;
using System.Text;



#if USE16_BIT
using BlockID = System.UInt16;
#else
using BlockID = System.Byte;
#endif

namespace ClassicalSharp.Mode {

    public sealed class SurvivalGameMode : survivalinventory, IGameMode {

        Game game;
        int score = 0;
        Random rnd = new Random();
        byte block2;

        Vector3I pos;
        BlockID oldblock;
        float blockhp = 10;
        SoundType blocksound;
        int oldwidth;
        int oldlength;
        int curblockhand;

        int pickeff = 100;
        int swordeff = 100;
        int shoveleff = 100;
        int axeeff = 100;




        //load sounds
        System.Media.SoundPlayer phurtsound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\player_hurt.wav");
        System.Media.SoundPlayer zombiehurtsound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\zombie_hurt.wav");
        System.Media.SoundPlayer skeletonhurtsound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\skeleton_hurt.wav");
        System.Media.SoundPlayer skeletonhurt2sound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\skeleton_hurt2.wav");
        System.Media.SoundPlayer skeletonhurt3sound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\skeleton_hurt3.wav");
        System.Media.SoundPlayer skeletonhurt4sound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\skeleton_hurt4.wav");
        System.Media.SoundPlayer spiderhurtsound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\spider_hurt.wav");
        System.Media.SoundPlayer spiderhurt2sound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\spider_hurt2.wav");
        System.Media.SoundPlayer creeperhurtsound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\creeper_hurt.wav");
        System.Media.SoundPlayer creeperhurt2sound = new System.Media.SoundPlayer(System.Environment.CurrentDirectory + @"\audio\creeper_hurt2.wav");
        //
        public SurvivalGameMode() { invCount[8] = 10;} // tnt

        public bool HandlesKeyDown(Key key)
        {
            if (key == game.Input.Keys[KeyBind.Inventory])
            {
                iscrafting = false;
                ischest = false;
                game.Gui.SetNewScreen(new InventoryScreen(game));
               
                return true;
            }

            if (key == game.Input.Keys[KeyBind.Craft])
            {
                iscrafting = true;
                ischest = false;
                game.Gui.SetNewScreen(new InventoryScreen(game));
                
                return true;
            }

            
                return false;
            
        } //Open Inventory & Crafting

        public void PickLeft(BlockID old) {
            if (BreakTimer.Enabled == false)
            {

                GetBlockHp(old);
                pos = game.SelectedPos.BlockPos;
                oldblock = old;
                BreakTimer.Interval = 100;
                BreakTimer.Enabled = true;
                curblockhand = game.Inventory.Selected;
            }

            game.AudioPlayer.PlayStepSound(blocksound);
        } //Start Break Blocks

        public void GetBlockHp(BlockID block) //breaking sound too
        {
            GetToolEff();
            if (block == Block.Dirt) //Dirt
            {

                blockhp = (shoveleff * 8)/100;
                blocksound = SoundType.Grass;

            }
            else if (block == Block.Grass) //Grass
            {

                blockhp = (shoveleff * 8) / 100;
                blocksound = SoundType.Grass;
            }
            else if (block == Block.Stone || block == Block.Slab) //Stone
            {

                blockhp = (pickeff * 110) / 100;
                blocksound = SoundType.Stone;

            }
            else if (block == Block.CoalOre) //Coal
            {

                blockhp = (pickeff * 115) / 100;
                blocksound = SoundType.Stone;
            }
            else if (block == Block.IronOre || block == Block.Iron) //Iron
            {

                blockhp = (pickeff * 120) / 100;
                blocksound = SoundType.Stone;
            }
            else if (block == Block.RedstoneOre) //Iron
            {

                blockhp = (pickeff * 125) / 100;
                blocksound = SoundType.Stone;
            }
            else if (block == Block.GoldOre || block == Block.Gold) //Gold
            {

                blockhp = (pickeff * 125) / 100;
                blocksound = SoundType.Stone;
            }
            else if (block == Block.DiamondOre || block == Block.Diamond) //Diamond
            {

                blockhp = (pickeff * 140) / 100;
                blocksound = SoundType.Stone;
            }
            else if (block == Block.Cobblestone) //CobbleStone
            {

                blockhp = (pickeff * 110) / 100;
                blocksound = SoundType.Stone;
            }
            else if (block == Block.Obsidian) //Obsidian
            {

                blockhp = (pickeff * 260) / 100;
                blocksound = SoundType.Stone;
            }
            else if (block == Block.Log) //Log
            {

                blockhp = (axeeff * 25)/100;
                blocksound = SoundType.Wood;
            }
            else if (block == Block.Wood) //Wood
            {

                blockhp = (axeeff * 25) / 100;
                blocksound = SoundType.Wood;
            }
            else if (block == Block.Chest1 || block == Block.Chest2 || block == Block.Chest3 || block == Block.Chest4) //Chest
            {

                blockhp = (axeeff * 45) / 100;
                blocksound = SoundType.Wood;
            }
            else if (block == Block.DoorBase || block == Block.DoorBase1 || block == Block.DoorBase2 || block == Block.DoorBase3 || block == Block.DoorTop || block == Block.DoorTop1 || block == Block.DoorTop2 || block == Block.DoorTop3 || block == Block.TrapDoor || block == Block.TrapDoorOpen) //Door & TrapDoor
            {

                blockhp = (axeeff * 25) / 100;
                blocksound = SoundType.Wood;
            }
            else if (block == Block.Sand) //Sand
            {

                blockhp = (shoveleff * 8) / 100;
                blocksound = SoundType.Sand;
            }
            else if (block == Block.Gravel) //Gravel
            {

                blockhp = (shoveleff * 8) / 100;
                blocksound = SoundType.Gravel;
            }
            else if (block == Block.Leaves) //Leaves
            {

                blockhp = (swordeff * 1) / 100; ;
                blocksound = SoundType.None;
            }
            else if (block == Block.TNT) //TNT
            {

                blockhp = 0;
                blocksound = SoundType.None;
            }
            else //Others
            {
                blockhp = 1;
                blocksound = SoundType.None;
            }
        }

        public short GetDamage()
        {

            int Tool = game.Inventory.Selected;
            short damage = 2;

            if (Tool == Block.WoodPick)
            {
                damage = 3;
            }

            else if (Tool == Block.StonePick)
            {
                damage = 4;
            }
            else if (Tool == Block.WoodSword)
            {
                damage = 4;
            }
            else if (Tool == Block.StoneSword)
            {
                damage = 5;
            }
            else if (Tool == Block.WoodShovel)
            {
                damage = 2;
            }
            else if (Tool == Block.StoneShovel)
            {
                damage = 3;
            }
            else if (Tool == Block.WoodAxe)
            {
                damage = 3;
            }
            else if (Tool == Block.StoneAxe)
            {
                damage = 4;
            }
            else if (Tool == Block.IronSword)
            {
                damage = 6;
            }
            else if (Tool == Block.IronShovel)
            {
                damage = 3;
            }
            else if (Tool == Block.IronAxe)
            {
                damage = 5;
            }
            else if (Tool == Block.IronPick)
            {
                damage = 4;
            }
            else if (Tool == Block.DiamondSword)
            {
                damage = 7;
            }
            else if (Tool == Block.DiamondPick)
            {
                damage = 5;
            }
            else if (Tool == Block.DiamondShovel)
            {
                damage = 4;
            }
            else if (Tool == Block.DiamondAxe)
            {
                damage = 6;
            }
            else
            {
                damage = 1;
            }

            return damage;
        }

        public void GetToolEff()
        {
            int Tool = game.Inventory.Selected;
            
            //Pickaxe
            if (Tool == Block.WoodPick)
            {
                pickeff = 16;
            }

            else if (Tool == Block.StonePick)
            {
                pickeff = 9;
            }
            else if (Tool == Block.IronPick)
            {
                pickeff = 5;
            }
            else if (Tool == Block.DiamondPick)
            {
                pickeff = 4;
            }
            else
            {
                pickeff = 100;
            }
            //Sword
            if (Tool == Block.WoodSword)
            {
                swordeff = 65;
            }
            else if (Tool == Block.StoneSword)
            {
                swordeff = 65;
            }
            else if (Tool == Block.IronSword)
            {
                swordeff = 65;
            }
            else if (Tool == Block.DiamondSword)
            {
                swordeff = 65;
            }
            else
            {
                swordeff = 100;
            }
            //Shovel
            if (Tool == Block.WoodShovel)
            {
                shoveleff = 50;
            }
            else if (Tool == Block.StoneShovel)
            {
                shoveleff = 30;
            }
            else if (Tool == Block.IronShovel)
            {
                shoveleff = 25;
            }
            else if (Tool == Block.DiamondShovel)
            {
                shoveleff = 15;
            }
            else
            {
                shoveleff = 100;
            }
            //Axe
            if (Tool == Block.WoodAxe)
            {
                axeeff = 50;
            }
            else if (Tool == Block.StoneAxe)
            {
                axeeff = 25;
            }
            else if (Tool == Block.IronAxe)
            {
                axeeff = 20;
            }
            else if (Tool == Block.DiamondAxe)
            {
                axeeff = 15;
            }
            else
            {
                axeeff = 100;
            }

        }

        public void BlockBreaking(object source, ElapsedEventArgs e)
        {

            blockhp--;
            if (blockhp <= 0)
            {


                game.UpdateBlock(pos.X, pos.Y, pos.Z, 0);
                game.UserEvents.RaiseBlockChanged(pos, oldblock, 0);
                if (oldblock == Block.Chest1 || oldblock == Block.Chest2 || oldblock == Block.Chest3 || oldblock == Block.Chest4)
                {
                    if (game.Server.IsSinglePlayer)
                    {
                        HandleDelete(oldblock);
                    }                
                }
                else
                {
                    HandleDelete(oldblock);

                }
                BreakTimer.Enabled = false;
                blockhp = 10;
                saveinventory();

                if (oldblock == Block.DoorBase || oldblock == Block.DoorBase1 || oldblock == Block.DoorBase2 || oldblock == Block.DoorBase3 )
                {

                    Vector3I pos2;
                    pos2 = pos;

                    pos2.Y = pos.Y + 1;

                    game.UpdateBlock(pos.X, pos.Y+1, pos.Z, 0);
                    game.UserEvents.RaiseBlockChanged(pos2, oldblock, 0);

                }

                if (oldblock == Block.DoorTop || oldblock == Block.DoorTop1 || oldblock == Block.DoorTop2 || oldblock == Block.DoorTop3)
                {

                    Vector3I pos2;
                    pos2 = pos;

                    pos2.Y = pos.Y - 1;

                    game.UpdateBlock(pos.X, pos.Y - 1, pos.Z, 0);
                    game.UserEvents.RaiseBlockChanged(pos2, oldblock, 0);

                }

            }
        } //Breaking Blocks

        public void PickMiddle(BlockID old) {
        } //Do Nothing

        public void PickRight(BlockID old, BlockID block) {
            int b = game.Inventory.Selected;

            int blocklooked = game.SelectedPos.Block;

            if (game.Server.IsSinglePlayer)
            {
                if (blocklooked == Block.Chest1 || blocklooked == Block.Chest2 || blocklooked == Block.Chest3 || blocklooked == Block.Chest4)
                {

                    iscrafting = false;
                    ischest = true;
                    loadchest();
                    game.Gui.SetNewScreen(new InventoryScreen(game));



                }
            }
            if (blocklooked != Block.DoorBase && blocklooked != Block.DoorBase1 && blocklooked != Block.DoorBase2 && blocklooked != Block.DoorBase3 && blocklooked != Block.DoorTop && blocklooked != Block.DoorTop1 && blocklooked != Block.DoorTop2 && blocklooked != Block.DoorTop3 && blocklooked != Block.Chest1 && blocklooked != Block.Chest2 && blocklooked != Block.Chest3 && blocklooked != Block.Chest4)
            {
                BlockID blocktoplace;

                blocktoplace = AutoRotate.RotateBlock(game,HandlePlace(b));

                

                if (!Mode.survivalinventory.Items.Contains(b) || b == Block.Redstone)
                {
                    int index = game.Inventory.SelectedIndex, offset = game.Inventory.Offset;
                    if (invCount[offset + index] == 0) return;

                    if (b != Block.DoorBase)
                    {

                        if (blocktoplace != Block.RedstoneGOff)
                        {
                            Vector3I pos = game.SelectedPos.TranslatedPos;
                            game.UpdateBlock(pos.X, pos.Y, pos.Z, blocktoplace);
                            game.UserEvents.RaiseBlockChanged(pos, old, blocktoplace);
                            
                        }
                        else
                        {

                            PlaceRedstone(game.SelectedPos.TranslatedPos,old); 

                        }
                    }
                    else
                    {
                        Vector3I pos = game.SelectedPos.TranslatedPos;
                        Vector3I pos2 = pos;
                        pos2.Y = pos.Y + 1;


                        game.UpdateBlock(pos.X, pos.Y, pos.Z, block);
                        game.UserEvents.RaiseBlockChanged(pos, old, block);

                        if (game.World.GetBlock(pos) == Block.DoorBase) { block2 = Block.DoorTop; }
                        if (game.World.GetBlock(pos) == Block.DoorBase1) { block2 = Block.DoorTop1; }
                        if (game.World.GetBlock(pos) == Block.DoorBase2) { block2 = Block.DoorTop2; }
                        if (game.World.GetBlock(pos) == Block.DoorBase3) { block2 = Block.DoorTop3; }

                        game.UpdateBlock(pos.X, pos.Y + 1, pos.Z, block2);
                        game.UserEvents.RaiseBlockChanged(pos2, old, block2);


                    }

                    invCount[offset + index]--;
                    if (invCount[offset + index] != 0) return;

                    // bypass HeldBlock's normal behaviour
                    game.Inventory[index] = Block.Air;
                    game.Events.RaiseHeldBlockChanged();
                    
                }

                
            }
            
        } //Place Blocks

        public void PlaceRedstone(Vector3I pos , BlockID old)
        {
   
            game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff);
            game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff);

            //West East
            if(game.World.GetBlock(pos.X+1,pos.Y,pos.Z) == Block.RedstoneGOff || game.World.GetBlock(pos.X + 1, pos.Y, pos.Z) == Block.RedstoneGOff3)
            {
                Vector3I Newpos = pos;
                Newpos.X = pos.X + 1;

                game.UpdateBlock(pos.X+1, pos.Y, pos.Z, Block.RedstoneGOff3);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff3);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff3);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff3);

            }
            if (game.World.GetBlock(pos.X - 1, pos.Y, pos.Z) == Block.RedstoneGOff || game.World.GetBlock(pos.X - 1, pos.Y, pos.Z) == Block.RedstoneGOff3)
            {
                Vector3I Newpos = pos;
                Newpos.X = pos.X - 1;

                game.UpdateBlock(pos.X - 1, pos.Y, pos.Z, Block.RedstoneGOff3);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff3);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff3);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff3);

            }
            //
            if (game.World.GetBlock(pos.X + 1, pos.Y, pos.Z) == Block.RedstoneGOff || game.World.GetBlock(pos.X + 1, pos.Y, pos.Z) == Block.RedstoneGOff1)
            {
                Vector3I Newpos = pos;
                Newpos.X = pos.X + 1;

                game.UpdateBlock(pos.X + 1, pos.Y, pos.Z, Block.RedstoneGOff);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff3);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff3);

            }
            if (game.World.GetBlock(pos.X - 1, pos.Y, pos.Z) == Block.RedstoneGOff || game.World.GetBlock(pos.X - 1, pos.Y, pos.Z) == Block.RedstoneGOff1)
            {
                Vector3I Newpos = pos;
                Newpos.X = pos.X - 1;

                game.UpdateBlock(pos.X - 1, pos.Y, pos.Z, Block.RedstoneGOff);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff3);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff3);

            }
            //North Sud
            if (game.World.GetBlock(pos.X, pos.Y, pos.Z + 1) == Block.RedstoneGOff || game.World.GetBlock(pos.X, pos.Y, pos.Z + 1) == Block.RedstoneGOff1)
            {
                Vector3I Newpos = pos;
                Newpos.Z = pos.Z + 1;

                game.UpdateBlock(pos.X, pos.Y, pos.Z+1, Block.RedstoneGOff1);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff1);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff1);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff1);

            }
            if (game.World.GetBlock(pos.X, pos.Y, pos.Z - 1) == Block.RedstoneGOff || game.World.GetBlock(pos.X, pos.Y, pos.Z - 1) == Block.RedstoneGOff1)
            {
                Vector3I Newpos = pos;
                Newpos.Z = pos.Z - 1;

                game.UpdateBlock(pos.X, pos.Y, pos.Z - 1, Block.RedstoneGOff1);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff1);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff1);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff1);

            }
            //
            if (game.World.GetBlock(pos.X, pos.Y, pos.Z + 1) == Block.RedstoneGOff || game.World.GetBlock(pos.X, pos.Y, pos.Z + 1) == Block.RedstoneGOff3)
            {
                Vector3I Newpos = pos;
                Newpos.Z = pos.Z + 1;

                game.UpdateBlock(pos.X, pos.Y, pos.Z + 1, Block.RedstoneGOff);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff1);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff1);

            }
            if (game.World.GetBlock(pos.X, pos.Y, pos.Z - 1) == Block.RedstoneGOff || game.World.GetBlock(pos.X, pos.Y, pos.Z - 1) == Block.RedstoneGOff3)
            {
                Vector3I Newpos = pos;
                Newpos.Z = pos.Z - 1;

                game.UpdateBlock(pos.X, pos.Y, pos.Z - 1, Block.RedstoneGOff);
                game.UserEvents.RaiseBlockChanged(Newpos, old, Block.RedstoneGOff);

                game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff1);
                game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff1);

            }
            //Center
            for (int x = -1; x < 2; x++)
            {


                if (x != 0)
                {

                    if (game.World.GetBlock(pos.X + x, pos.Y, pos.Z) == Block.RedstoneGOff3)
                    {
                        
                        for (int z = -1; z < 2; z++)
                        {
                            if (z != 0)
                            {
                               
                                if (game.World.GetBlock(pos.X, pos.Y, pos.Z + z) == Block.RedstoneGOff1)
                                {

                                    game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.RedstoneGOff);
                                    game.UserEvents.RaiseBlockChanged(pos, old, Block.RedstoneGOff);
;

                                }
                            }
                        }
                    }


                }

            }


            return;

        }

        public bool PickEntity(byte id) {

            

            mobsAttTimer.Enabled = false;
            mobsAttTimer.Enabled = true;

            float deltaX = game.LocalPlayer.Position.X - game.Entities[id].Position.X;
            float deltaY = game.LocalPlayer.Position.Y - game.Entities[id].Position.Y;
            float deltaZ = game.LocalPlayer.Position.Z - game.Entities[id].Position.Z;

            float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

            if (game.Entities[id].ModelName == "humanoid") { }
            else
            {
                if (distance < 4)
                {

                    if ((Options.GetBool(OptionsKey.UseSound, true)))
                    {
                        if (game.Entities[id].ModelName == "sheep" || game.Entities[id].ModelName == "pigs")
                        {

                        }
                        if (game.Entities[id].ModelName == "zombie")//sound zombie
                        {
                            zombiehurtsound.Play();

                        }
                        if (game.Entities[id].ModelName == "spider")//sound spider
                        {
                            Random rnd = new Random();
                            int soundnb = rnd.Next(1, 3);

                            if (soundnb == 1)
                            {

                                spiderhurtsound.Play();
                            }
                            else if (soundnb == 2)
                            {

                                spiderhurt2sound.Play();
                            }
                        }
                        if (game.Entities[id].ModelName == "skeleton") //sound skeleton
                        {
                            Random rnd = new Random();
                            int soundnb = rnd.Next(1, 5);

                            if (soundnb == 1)
                            {
                                skeletonhurtsound.Play();
                            }
                            else if (soundnb == 2)
                            {
                                skeletonhurt2sound.Play();
                            }
                            else if (soundnb == 3)
                            {
                                skeletonhurt3sound.Play();
                            }
                            else if (soundnb == 4)
                            {
                                skeletonhurt4sound.Play();
                            }
                        }
                        if (game.Entities[id].ModelName == "creeper") //sound creeper
                        {
                            Random rnd = new Random();
                            int soundnb = rnd.Next(1, 3);

                            if (soundnb == 1)
                            {

                                creeperhurtsound.Play();
                            }
                            else if (soundnb == 2)
                            {

                                creeperhurt2sound.Play();
                            }


                        }
                    }


                    if (game.Entities[id].Health > 1)
                    {
                        game.Entities[id].Health -= GetDamage();

                        if (Mode.survivalinventory.Tools.Contains(game.Inventory.Selected))
                        {
                            Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex] -= 2;
                        }

                        Entity entity = game.Entities[id];
                        Entity player = game.Entities[EntityList.SelfID];

                        Vector3 delta = player.Position - entity.Position;
                        delta.Y = 0.0f;
                        delta = Vector3.Normalize(delta) * 0.6f;
                        delta.Y = -0.4f;

                        entity.Velocity -= delta;

                        
                    }
                    else
                    {
                        if (game.Entities[id].ModelName == "sheep")
                        {
                            AddToHotbar(Block.White, rnd.Next(1, 3));
                            score += 10;
                        }
                        if (game.Entities[id].ModelName == "pigs")
                        {
                            AddToHotbar(Block.BrownMushroom, rnd.Next(0, 3));
                            score += 10;
                        }
                        if (game.Entities[id].ModelName == "zombie")
                        {
                            score += 80;
                        }
                        if (game.Entities[id].ModelName == "spider")
                        {
                            score += 105;
                        }
                        if (game.Entities[id].ModelName == "skeleton")
                        {
                             score += 120;
                        }
                        if (game.Entities[id].ModelName == "creeper")
                        {
                            score += 200;
                        }

                        UpdateScore();
                        if (game.Server.IsSinglePlayer)
                        {
                            game.Entities.RemoveEntity(id);
                        }
                    }

                    
                }
            }
            return true;
        } //Hit Mobs

        public void UpdateScore()
        {
            game.Chat.Add("&fScore: &e" + score, MessageType.Status1);
            
        } //Update Score

        //Define timers
        System.Timers.Timer BreakTimer = new System.Timers.Timer();
        System.Timers.Timer mobsAttTimer = new System.Timers.Timer();
        System.Timers.Timer FallingDamTimer = new System.Timers.Timer();
        System.Timers.Timer ButtonTimer = new System.Timers.Timer();
        System.Timers.Timer myTimer4 = new System.Timers.Timer();

        public void InitTimer()
        {
            BreakTimer.Elapsed += new ElapsedEventHandler(BlockBreaking);
            

            mobsAttTimer.Elapsed += new ElapsedEventHandler(MobsAttacks);
            mobsAttTimer.Interval = 500;
            mobsAttTimer.Enabled = true;

            FallingDamTimer.Elapsed += new ElapsedEventHandler(FallingDamage);
            FallingDamTimer.Interval = 50;
            FallingDamTimer.Enabled = true;

            ButtonTimer.Elapsed += new ElapsedEventHandler(isButtondown);
            ButtonTimer.Interval = 100;
            ButtonTimer.Enabled = true;

        } //Init Timers

        public void isButtondown(object source, ElapsedEventArgs e)
        {
            if (game.Input.IsMousePressed(MouseButton.Left) == false)
            {

                BreakTimer.Enabled = false;
            }


            if (game.Input.IsMousePressed(MouseButton.Right) == true)
            {

                int index = game.Inventory.SelectedIndex;
                if (game.LocalPlayer.Health < 20)
                {
                    if (invCount[index] > 0)
                    {

                        useItem(game.Inventory[index], index);

                    }
                }

                int blocklooked = game.SelectedPos.Block;

                

                if (blocklooked == Block.TrapDoor)
                {
                    Vector3I pos = game.SelectedPos.BlockPos;
                    game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.TrapDoorOpen);
                    game.UserEvents.RaiseBlockChanged(pos, Block.TrapDoor, Block.TrapDoorOpen);

                }

                if (blocklooked == Block.TrapDoorOpen)
                {
                    Vector3I pos = game.SelectedPos.BlockPos;
                    game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.TrapDoor);
                    game.UserEvents.RaiseBlockChanged(pos, Block.TrapDoorOpen, Block.TrapDoor);

                }

                if (blocklooked == Block.DoorBase || blocklooked == Block.DoorBase1)
                {
                    Vector3I pos = game.SelectedPos.BlockPos;
                    Vector3I pos2 = pos;
                    pos2.Y = pos.Y + 1;

                    game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.DoorBase3);
                    game.UserEvents.RaiseBlockChanged(pos, (byte)blocklooked, Block.DoorBase3);

                    game.UpdateBlock(pos2.X, pos2.Y, pos2.Z, Block.DoorTop3);
                    game.UserEvents.RaiseBlockChanged(pos2, game.World.GetBlock(pos2), Block.DoorTop3);


                }

                if (blocklooked == Block.DoorBase2 || blocklooked == Block.DoorBase3)
                {
                    Vector3I pos = game.SelectedPos.BlockPos;
                    Vector3I pos2 = pos;
                    pos2.Y = pos.Y + 1;



                    game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.DoorBase1);
                    game.UserEvents.RaiseBlockChanged(pos, (byte)blocklooked, Block.DoorBase1);

                    game.UpdateBlock(pos2.X, pos2.Y, pos2.Z, Block.DoorTop1);
                    game.UserEvents.RaiseBlockChanged(pos2, game.World.GetBlock(pos2), Block.DoorTop1);

                }

                if (blocklooked == Block.DoorTop || blocklooked == Block.DoorTop1)
                {
                    Vector3I pos = game.SelectedPos.BlockPos;
                    Vector3I pos2 = pos;
                    pos2.Y = pos.Y - 1;



                    game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.DoorTop3);
                    game.UserEvents.RaiseBlockChanged(pos, (byte)blocklooked, Block.DoorTop3);

                    game.UpdateBlock(pos2.X, pos2.Y, pos2.Z, Block.DoorBase3);
                    game.UserEvents.RaiseBlockChanged(pos2, game.World.GetBlock(pos2), Block.DoorBase3);

                }

                if (blocklooked == Block.DoorTop2 || blocklooked == Block.DoorTop3)
                {
                    Vector3I pos = game.SelectedPos.BlockPos;
                    Vector3I pos2 = pos;
                    pos2.Y = pos.Y - 1;

                    game.UpdateBlock(pos.X, pos.Y, pos.Z, Block.DoorTop1);
                    game.UserEvents.RaiseBlockChanged(pos, (byte)blocklooked, Block.DoorTop1);

                    game.UpdateBlock(pos2.X, pos2.Y, pos2.Z, Block.DoorBase1);
                    game.UserEvents.RaiseBlockChanged(pos2, game.World.GetBlock(pos2), Block.DoorBase1);

                }

            }

            if (pos != game.SelectedPos.BlockPos) {
                BreakTimer.Enabled = false;
            }

            if (curblockhand != game.Inventory.Selected)
            {
                BreakTimer.Enabled = false;
            }

           
        } //Button Down Actions

        private void MobsAttacks(object source, ElapsedEventArgs e)
        {

            
            for (byte id = 0; id < 150; id++) {
                if (game.Entities[id] != null)
                {
                    
                        if (game.Entities[id].ModelName == "zombie" || game.Entities[id].ModelName == "skeleton" || game.Entities[id].ModelName == "spider" || game.Entities[id].ModelName == "creeper")
                        {
                            if (game.LocalPlayer.Position.X < game.Entities[id].Position.X + 1)
                            {
                                if (game.LocalPlayer.Position.X > game.Entities[id].Position.X - 1)
                                {
                                    if (game.LocalPlayer.Position.Z < game.Entities[id].Position.Z + 1)
                                    {
                                        if (game.LocalPlayer.Position.Z > game.Entities[id].Position.Z - 1)
                                        {
                                            if (game.LocalPlayer.Position.Y < game.Entities[id].Position.Y + 1)
                                            {
                                               if (game.LocalPlayer.Position.Y > game.Entities[id].Position.Y - 1)
                                               {

                                                if (game.LocalPlayer.Health - game.Entities[id].Damage < 1)
                                                {
                                                    game.LocalPlayer.Health = 0;
                                                    verifyplayerhp();
                                                }
                                                else
                                                {
                                                    game.LocalPlayer.Health -= game.Entities[id].Damage;
                                                    game.LocalPlayer.Velocity = CalculVelocityPlayer(game.LocalPlayer.Position, id);
                                                    verifyplayerhp();
                                                }

                                               
                                                if ((Options.GetBool(OptionsKey.UseSound, true))) phurtsound.Play();
                                            }

                                            }

                                        }

                                    }

                                }

                            }
                        }
                    
                }

                    
                }

           


        } //Mobs Attacking


        public Widget MakeHotbar() { return new SurvivalHotbarWidget(game); } //Create Hotbar



        Vector3 CalculVelocityPlayer(Vector3 old, byte id)
        {
            Vector3 newvel = old;

            if (game.Entities[id].Position.X < game.LocalPlayer.Position.X)
            {
                newvel.X = 0.3f;
            }
            else
            {
                newvel.X = -0.3f;
            }

            if (game.Entities[id].Position.Z < game.LocalPlayer.Position.Z)
            {
                newvel.Z = 0.3f;
            }
            else
            {
                newvel.Z = -0.3f;
            }

            newvel.Y = 0.4f;

            return newvel;
        } //Calcul Velocity of the player (When Mobs Hit)

        void HandleDelete(BlockID old) {

              if (old == Block.IronOre) {
                AddToHotbar(Block.Iron, 1);
            } else if (old == Block.GoldOre) {
                AddToHotbar(Block.Gold, 1);
            } else if (old == Block.DiamondOre) {
                AddToHotbar(Block.Diamond, 1);
            } else if (old == Block.Grass) {
                AddToHotbar(Block.Dirt, 1);
            } else if (old == Block.Stone) {
                AddToHotbar(Block.Cobblestone, 1);
            } else if (old == Block.RedstoneOre) {
                AddToHotbar(Block.Redstone, rnd.Next(1, 8));
            } else if (old == Block.RedstoneGOff || old == Block.RedstoneGOff1 || old == Block.RedstoneGOff2 || old == Block.RedstoneGOff3 || old == Block.RedstoneGOff4) {
                AddToHotbar(Block.Redstone, 1);
            } else if (old == Block.Leaves) {
            if (rnd.Next(1, 16) == 1) { // TODO: is this chance accurate?
                AddToHotbar(Block.Sapling, 1);
            }
            } else if (old == Block.TNT) {
            } 
            else if (old == Block.DoorBase1 || old == Block.DoorBase2 || old == Block.DoorBase3 || old == Block.DoorTop || old == Block.DoorTop1 || old == Block.DoorTop2 || old == Block.DoorTop3)
            {
                AddToHotbar(Block.DoorBase, 1);
            }
            
            else if (old == Block.TrapDoorOpen)
            {
                AddToHotbar(Block.TrapDoor, 1);
            }

            else if (old == Block.Ladder2 || old == Block.Ladder3 || old == Block.Ladder4)
            {
                AddToHotbar(Block.Ladder1, 1);
            }
            else if (old == Block.Chest1 || old == Block.Chest2 || old == Block.Chest3 || old == Block.Chest4)
            {
                System.IO.File.Delete("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestBlocks.txt");
                System.IO.File.Delete("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestCount.txt");
                System.IO.File.Delete("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestDurability.txt");
                AddToHotbar(Block.Chest2, 1);
            }

            else {
                AddToHotbar(old, 1);
            }


            if (Tools.Contains(game.Inventory.Selected))
            {
                survivalinventory.invdurability[game.Inventory.SelectedIndex] -= 1;

                if (survivalinventory.invdurability[game.Inventory.SelectedIndex] < 1)
                {

                    game.Inventory[game.Inventory.SelectedIndex] = 0;
                    Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = 0;
                }
            }
        }  //Pick The Good Block on Delete

        BlockID HandlePlace(int block)
        {
            BlockID NewBlock;

            if (block == Block.Redstone)
            {
                NewBlock = Block.RedstoneGOff;
                
            }
           
            else
            {
                NewBlock = (byte)block;
            }

            saveinventory();
            return NewBlock;
        }

        void AddToHotbar(BlockID block, int count)
        {
            int counter = 0;
            while (counter < 36)
            {
                if (counter < 9)
                {
                    if (game.Inventory[counter] == block && invCount[counter] < 64)
                    {

                        invCount[counter] += (byte)count;

                        break;

                    }


                    if (game.Inventory[counter] == Block.Air)
                    {

                        game.Inventory[counter] = block;
                        invCount[counter] += (byte)count;

                        break;
                    }
                }else
                {
                    if (invBlocks[counter-8] == block && inv2Count[counter-8] < 64)
                    {

                        inv2Count[counter-8] += (byte)count;

                        break;

                    }


                    if (invBlocks[counter-8] == Block.Air)
                    {

                        invBlocks[counter - 8] = block;
                        inv2Count[counter-8] += (byte)count;

                        break;
                    }

                }
                counter = counter + 1;
            }
            
        } //Add Blocks to Hotbar

        //Falling Damage
        float lastPositionY = 0f;
        public static float FallDistance = 0f;
        public static bool grounded = false;
        

        private void FallingDamage(object source, ElapsedEventArgs e)
        {
            if (game.LocalPlayer.Health > 0)
            {
            
                grounded = false;

                if (game.LocalPlayer.Hacks.Flying || game.LocalPlayer.Hacks.Noclip || game.LocalPlayer.Hacks.Speeding)
                {
                    FallDistance = 0;
                    lastPositionY = 0;

                }

                if (grounded)
                {
                    FallDistance = 0;
                    lastPositionY = 0;
                }
                else
                {
                    if (lastPositionY > game.LocalPlayer.Position.Y)
                    {

                        FallDistance += lastPositionY - game.LocalPlayer.Position.Y;


                    }
                    lastPositionY = game.LocalPlayer.Position.Y;
                }

                int BlockX = (int)game.LocalPlayer.Position.X;
                int BlockY = (int)game.LocalPlayer.Position.Y - 1;
                int BlockZ = (int)game.LocalPlayer.Position.Z;

                if (game.World.GetBlock(BlockX, BlockY, BlockZ) == Block.Air) { }
                else
                if (game.World.GetBlock(BlockX, BlockY, BlockZ) == Block.Water || game.World.GetBlock(BlockX, BlockY + 1, BlockZ) == Block.Lava || game.World.GetBlock(BlockX, BlockY + 1, BlockZ) == Block.StillWater || game.World.GetBlock(BlockX, BlockY + 1, BlockZ) == Block.StillLava || game.World.GetBlock(BlockX, BlockY + 1, BlockZ) == Block.Rope)
                {
                    FallDistance = 0;
                    lastPositionY = game.LocalPlayer.Position.Y;
                }
                else
                {

                    if (FallDistance > 3)
                    {

                        for (int i = 0; i < FallDistance - 3; i++)
                        {
                            if (game.LocalPlayer.Health > 0)
                            {
                                game.LocalPlayer.Health -= 1;
                               
                            }

                        }
                      
                            
                            verifyplayerhp();
                        

                        if ((Options.GetBool(OptionsKey.UseSound, true))) phurtsound.Play();
                    }

                    grounded = true;
                    FallDistance = 0;




                }

            }
        }

        void DeathMessage()
        {

            game.Chat.Add("You died.", MessageType.Normal);
            game.Chat.Add("Score: " + score, MessageType.Normal);
            

            return;
        
        }

        void Respawning()
        {


            game.LocalPlayer.Velocity.X = 0;
            game.LocalPlayer.Velocity.Y = 0;
            game.LocalPlayer.Velocity.Z = 0;

            Vector3 Newpos;
            Newpos.X = (float)game.World.Width / 2;
            Newpos.Z = (float)game.World.Length / 2;

            Newpos = Respawn.FindSpawnPosition(game, (int)Newpos.Z, (int)Newpos.X, game.LocalPlayer.Size);


            LocationUpdate update = LocationUpdate.MakePos(Newpos, false);
            game.LocalPlayer.SetLocation(update, true);



        }

        void verifyplayerhp()
        {



            if (game.LocalPlayer.Health < 1)
            {

                game.LocalPlayer.Health = 20;
                FallDistance = 0;
                lastPositionY = 0;

                Respawning();


               // DeathMessage();

               // score = 0;

              // UpdateScore();


            }

            return;
        } //Verify Player HP

        public void useItem(BlockID type,int index)
        {
            BlockID block = type;

            if (block == Block.RedMushroom) { 
                if (game.LocalPlayer.Health < 20)
                    {
                        game.LocalPlayer.Health -= 4;
                        verifyplayerhp();

                    invCount[index]--;
                    if (invCount[index] <= 0)
                    {
                        game.Inventory[index] = Block.Air;
                        game.Events.RaiseHeldBlockChanged();
                    }

                    
                }   
                   }

            else if (block == Block.BrownMushroom) { 
                    if (game.LocalPlayer.Health < 20)
                    
                        {
                            game.LocalPlayer.Health += 5;

                            invCount[index]--;
                             if (invCount[index] <= 0)
                             {
                                 game.Inventory[index] = Block.Air;
                                 game.Events.RaiseHeldBlockChanged();
                             }
                    
                        }
                            
                    }

           if (game.LocalPlayer.Health > 20)
            {
                game.LocalPlayer.Health = 20;
            }

        } //On Use Item

        void SetMobsStats(int i)
        {
            string m = game.Entities[i].ModelName;

            if (m == "zombie" || m == "skeleton" || m == "creeper")
            {

                game.Entities[i].Health = 20;
                game.Entities[i].Damage = 3;

            }

            if (m == "spider")
            {

                game.Entities[i].Health = 16;
                game.Entities[i].Damage = 2;
            }

            if (m == "pig")
            {

                game.Entities[i].Health = 10;

            }

            if (m == "sheep")
            {

                game.Entities[i].Health = 8;

            }


            if (m == "chicken")
            {

                game.Entities[i].Health = 4;

            }

        }

        byte[] temphotbarload = new byte[20];
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

                for (int i = 0; i < 9; i++)
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

        void loadinventory()
        {

            if (!System.IO.File.Exists("maps/" + game.World.Uuid + "Inventory.txt"))
            {
                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "Inventory.txt", Mode.survivalinventory.invBlocks);

            }

            if (!System.IO.File.Exists("maps/" + game.World.Uuid + "Inventorycount.txt"))
            {
                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "Inventorycount.txt", Mode.survivalinventory.inv2Count);

            }

            if (!System.IO.File.Exists("maps/" + game.World.Uuid + "hotbarcount.txt"))
            {
                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "hotbarcount.txt", Mode.survivalinventory.invCount);

            }

            if (!System.IO.File.Exists("maps/" + game.World.Uuid + "hotbar.txt"))
            {
                for (int i = 0; i < 9; i++)
                {

                    hotbarsave[i] = game.Inventory[i];


                }

                System.IO.File.WriteAllBytes("maps/" + game.World.Uuid + "hotbar.txt", hotbarsave);

            }

            Mode.survivalinventory.invBlocks = System.IO.File.ReadAllBytes("maps/" + game.World.Uuid + "Inventory.txt");
            Mode.survivalinventory.inv2Count = System.IO.File.ReadAllBytes("maps/" + game.World.Uuid + "Inventorycount.txt");
            Mode.survivalinventory.invCount = System.IO.File.ReadAllBytes("maps/" + game.World.Uuid + "hotbarcount.txt");



            temphotbarload = System.IO.File.ReadAllBytes("maps/" + game.World.Uuid + "hotbar.txt");
            for (int i = 0;i < 9;i++)
            {

                game.Inventory[i] = temphotbarload[i];

            }
           

        }

        void loadchest()
        {
            if (game.Server.IsSinglePlayer)
            {
                byte[] blankbytes = new byte[150];

                if (!System.IO.File.Exists("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestBlocks.txt"))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestBlocks.txt");
                    file.Directory.Create();
                    System.IO.File.WriteAllBytes(file.FullName, blankbytes);

                }

                if (!System.IO.File.Exists("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestCount.txt"))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestCount.txt");
                    file.Directory.Create();
                    System.IO.File.WriteAllBytes(file.FullName, blankbytes);

                }

                if (!System.IO.File.Exists("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestDurability.txt"))
                {
                    System.IO.FileInfo file = new System.IO.FileInfo("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestDurability.txt");
                    file.Directory.Create();
                    System.IO.File.WriteAllBytes(file.FullName, blankbytes);

                }

                chestBlocks = null;
                chestCount = null;
                chestdurability = null;

                chestBlocks = new byte[150];
                chestCount = new byte[150];
                chestdurability = new byte[150];

                Mode.survivalinventory.chestBlocks = System.IO.File.ReadAllBytes("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestBlocks.txt");
                Mode.survivalinventory.chestCount = System.IO.File.ReadAllBytes("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestCount.txt");
                Mode.survivalinventory.chestdurability = System.IO.File.ReadAllBytes("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestDurability.txt");

            }
        }

        public void OnNewMapLoaded(Game game) {


            



                game.Chat.Add("&fScore: &e" + score, MessageType.Status1);
                string[] models = { "sheep", "pig", "skeleton", "zombie", "creeper", "spider" };
                for (int i = 0; i < ((int)((game.World.Length + game.World.Width / 2) / 5)); i++)
                {

                if (i < 155)
                {
                    if (game.Server.IsSinglePlayer)
                    {
                        MobEntity fail = new MobEntity(game, models[rnd.Next(models.Length)]);
                        float x = rnd.Next(0, game.World.Width) + 0.5f;
                        float z = rnd.Next(0, game.World.Length) + 0.5f;

                        Vector3 pos = Respawn.FindSpawnPosition(game, x, z, fail.Size);
                        fail.SetLocation(LocationUpdate.MakePos(pos, false), false);
                        game.Entities[i] = fail;
                        SetMobsStats(i);
                    }
                }
                
            }
            
                oldwidth = game.World.Width;
                oldlength = game.World.Length;

            if (game.Server.IsSinglePlayer)
            {
                loadinventory();
            }

        } //On New map

       

        public void Init(Game game) {


            
           

            zombiehurtsound.LoadAsync();
            skeletonhurtsound.LoadAsync();
            skeletonhurt2sound.LoadAsync();
            skeletonhurt3sound.LoadAsync();
            skeletonhurt4sound.LoadAsync();
            spiderhurtsound.LoadAsync();
            spiderhurt2sound.LoadAsync();
            creeperhurtsound.LoadAsync();
            creeperhurt2sound.LoadAsync();

            this.game = game;
            BlockID[] hotbar = game.Inventory.Hotbar;
            for (int i = 0; i < hotbar.Length; i++)
                hotbar[i] = Block.Air;
                hotbar[8] = Block.TNT;
            game.Server.AppName += " (survival)";
            InitTimer();





            

        } //Init Survival


       



        public void Ready(Game game) {
          
        }
        public void Reset(Game game) { }
        public void OnNewMap(Game game) { }
        public void Dispose() { }




    }
}
