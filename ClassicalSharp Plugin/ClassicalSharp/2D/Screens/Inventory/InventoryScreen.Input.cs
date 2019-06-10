// Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
using System;
using System.Drawing;
using ClassicalSharp.GraphicsAPI;
using OpenTK.Input;



namespace ClassicalSharp.Gui.Screens {
	public partial class InventoryScreen : Screen {
        int selIndex2;
        Point lastpos;


        public override bool HandlesAllInput { get { return true; } }
		
		public override bool HandlesMouseMove(int mouseX, int mouseY) {
			if (draggingMouse) {
				mouseY -= TableY;
				scrollY = 0;
				
				return true;
			}
			
			selIndex = -1;
			if (Contains(startX, startY + 3, blocksPerRow * blockSize,
			             maxRows * blockSize - 3 * 2, mouseX, mouseY)) {
				for (int i = 0; i < blocksTable.Length; i++) {
					int x, y;
					GetCoords(i, out x, out y);
					
					if (Contains(x, y, blockSize, blockSize, mouseX, mouseY)) {
						selIndex = i;
						break;
					}
				}
			}
			RecreateBlockInfoTexture();
            
			return true;
		}
		
		public override bool HandlesMouseClick(int mouseX, int mouseY, MouseButton button) {
			if (draggingMouse || game.Gui.hudScreen.hotbar.HandlesMouseClick(mouseX, mouseY, button))
				return true;
            

            int scrollX = TableX + TableWidth;
			if (button == MouseButton.Left && mouseX >= scrollX && mouseX < scrollX + scrollWidth) {
				//ScrollbarClick(mouseY);
                
            } else if (button == MouseButton.Left) {
				if (selIndex != -1) {

                    if ((Options.GetBool(OptionsKey.SurvivalMode, false)) == true || (Options.GetBool(OptionsKey.SurvivalMode, false)) == false)
                    {

                        if (!Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest) //if not crafting
                        {
                            if (blocksTable[selIndex] != Block.Bedrock)
                            {
                                byte oldblock;
                                byte oldcount;
                                byte olddurability;
                                oldblock = game.Inventory.Selected;
                                oldcount = Mode.survivalinventory.invCount[game.Inventory.SelectedIndex];
                                olddurability = Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex];

                                if (oldblock != Mode.survivalinventory.invBlocks[selIndex + 1])
                                {

                                    game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.invBlocks[selIndex + 1]; //set block in hand to block in inventory
                                    Mode.survivalinventory.invBlocks[selIndex + 1] = oldblock; //set block in inventory to block in hand

                                    Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.inv2Count[selIndex + 1];
                                    Mode.survivalinventory.inv2Count[selIndex + 1] = oldcount;

                                    Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.inv2durability[selIndex + 1];
                                    Mode.survivalinventory.inv2durability[selIndex + 1] = olddurability;
                                }
                                else
                                {


                                    if (!Mode.survivalinventory.Tools.Contains(game.Inventory.Selected))
                                    {
                                        if (Mode.survivalinventory.inv2Count[selIndex + 1] + oldcount > 64)
                                        {
                                            game.Inventory[game.Inventory.SelectedIndex] = 0;
                                            Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = 0;

                                            game.Inventory[game.Inventory.SelectedIndex] = oldblock;
                                            Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = (byte)((Mode.survivalinventory.inv2Count[selIndex + 1] + oldcount) - 64);
                                            Mode.survivalinventory.inv2Count[selIndex + 1] = 64;
                                        }
                                        else
                                        {

                                            game.Inventory[game.Inventory.SelectedIndex] = 0;
                                            Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = 0;

                                            Mode.survivalinventory.inv2Count[selIndex + 1] += oldcount;
                                        }

                                    }


                                }

                                game.Events.RaiseHeldBlockChanged();
                            }
                        }

                        else if (!Mode.survivalinventory.iscrafting && Mode.survivalinventory.ischest) //if chest
                        {
                            if (blocksTable[selIndex] != Block.Bedrock)
                            {
                                byte oldblock;
                                byte oldcount;
                                byte olddurability;
                                oldblock = game.Inventory.Selected;
                                oldcount = Mode.survivalinventory.invCount[game.Inventory.SelectedIndex];
                                olddurability = Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex];

                                if (oldblock != Mode.survivalinventory.chestBlocks[selIndex + 1])
                                {

                                    game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.chestBlocks[selIndex + 1]; //set block in hand to block in inventory
                                    Mode.survivalinventory.chestBlocks[selIndex + 1] = oldblock; //set block in inventory to block in hand

                                    Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.chestCount[selIndex + 1];
                                    Mode.survivalinventory.chestCount[selIndex + 1] = oldcount;

                                    Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.chestdurability[selIndex + 1];
                                    Mode.survivalinventory.chestdurability[selIndex + 1] = olddurability;
                                }
                                else
                                {


                                    if (!Mode.survivalinventory.Tools.Contains(game.Inventory.Selected))
                                    {
                                        if (Mode.survivalinventory.chestCount[selIndex + 1] + oldcount > 64)
                                        {
                                            game.Inventory[game.Inventory.SelectedIndex] = 0;
                                            Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = 0;

                                            game.Inventory[game.Inventory.SelectedIndex] = oldblock;
                                            Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = (byte)((Mode.survivalinventory.chestCount[selIndex + 1] + oldcount) - 64);
                                            Mode.survivalinventory.chestCount[selIndex + 1] = 64;
                                        }
                                        else
                                        {

                                            game.Inventory[game.Inventory.SelectedIndex] = 0;
                                            Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = 0;

                                            Mode.survivalinventory.chestCount[selIndex + 1] += oldcount;
                                        }

                                    }


                                }

                                game.Events.RaiseHeldBlockChanged();
                            }
                        }

                        else if (Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest) ///////////////////if crafting///////////////////////////
                        {
                            

                            int craftgrid3;
                            int craftgrid4;
                            int craftgrid5;
                            int craftgrid6;
                            int craftgrid7;
                            int craftgrid8;
                            int ix;
                            if (game.UseCPE)
                            {
                                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                                craftgrid3 = 9;
                                craftgrid4 = 10;
                                craftgrid5 = 11;
                                craftgrid6 = 18;
                                craftgrid7 = 19;
                                craftgrid8 = 20;
                            }
                            else
                            {
                                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                                craftgrid3 = 9;
                                craftgrid4 = 10;
                                craftgrid5 = 11;
                                craftgrid6 = 18;
                                craftgrid7 = 19;
                                craftgrid8 = 20;
                            }


                           
                            if (selIndex == 0) { selIndex2 = ix + 1; }
                            if (selIndex == 1) { selIndex2 = ix + 2; }
                            if (selIndex == 2) { selIndex2 = ix + 3; }
                            if (selIndex == craftgrid3) { selIndex2 = ix + 4; }
                            if (selIndex == craftgrid4) { selIndex2 = ix + 5; }
                            if (selIndex == craftgrid5) { selIndex2 = ix + 6; }
                            if (selIndex == craftgrid6) { selIndex2 = ix + 7; }
                            if (selIndex == craftgrid7) { selIndex2 = ix + 8; }
                            if (selIndex == craftgrid8) { selIndex2 = ix + 9; }
                            if (selIndex == craftgrid5 + 2) { selIndex2 = ix + 10;}

                                

                            if (blocksTable[selIndex] != Block.Bedrock)
                            {


                                //
                                byte oldblock;
                                oldblock = game.Inventory.Selected;
                                byte oldcount;
                                oldcount = Mode.survivalinventory.invCount[game.Inventory.SelectedIndex];
                                byte olddurability;
                                olddurability = Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex];

                                if (selIndex2 != ix + 10)
                                {

                                    if (oldblock != Mode.survivalinventory.invBlocks[selIndex2])
                                    {
                                        game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.invBlocks[selIndex2]; //set block in hand to block in inventory
                                        Mode.survivalinventory.invBlocks[selIndex2] = oldblock; //set block in inventory to block in hand



                                        Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.inv2Count[selIndex2];
                                        Mode.survivalinventory.inv2Count[selIndex2] = oldcount;

                                        Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.inv2durability[selIndex2];
                                        Mode.survivalinventory.inv2durability[selIndex2] = olddurability;
                                        game.Events.RaiseHeldBlockChanged();
                                        Recipes();
                                    }
                                    else
                                    {
                                        if (!Mode.survivalinventory.Tools.Contains(game.Inventory.Selected))
                                        {
                                            if (Mode.survivalinventory.inv2Count[selIndex2] + oldcount > 64)
                                            {
                                                game.Inventory[game.Inventory.SelectedIndex] = 0;
                                                Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = 0;

                                                game.Inventory[game.Inventory.SelectedIndex] = oldblock;
                                                Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = (byte)((Mode.survivalinventory.inv2Count[selIndex2] + oldcount) - 64);
                                                Mode.survivalinventory.inv2Count[selIndex2] = 64;

                                            }
                                            else
                                            {

                                                game.Inventory[game.Inventory.SelectedIndex] = 0;
                                                Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = 0;

                                                Mode.survivalinventory.inv2Count[selIndex2] += oldcount;
                                            }
                                            game.Events.RaiseHeldBlockChanged();
                                            Recipes();
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    if (game.Inventory.Selected == 0)
                                    {
                                       
                                        game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.invBlocks[selIndex2]; //set block in hand to block in inventory
                                        Mode.survivalinventory.invBlocks[selIndex2] = oldblock; //set block in inventory to block in hand


                                        
                                        Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.inv2Count[selIndex2];
                                        Mode.survivalinventory.inv2Count[selIndex2] = oldcount;

                                        Mode.survivalinventory.invdurability[game.Inventory.SelectedIndex] = ClassicalSharp.Mode.survivalinventory.inv2durability[selIndex2];
                                        Mode.survivalinventory.inv2durability[selIndex2] = olddurability;
                                       
                                        game.Events.RaiseHeldBlockChanged();
                                        Recipes();
                                    }

                                    else if (game.Inventory.Selected == Mode.survivalinventory.invBlocks[selIndex2])
                                    {
                                        if (!Mode.survivalinventory.Tools.Contains(game.Inventory.Selected))
                                        {
                                            game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.invBlocks[selIndex2]; //set block in hand to block in inventory
                                            Mode.survivalinventory.invBlocks[selIndex2] = oldblock; //set block in inventory to block in hand


                                            if (ClassicalSharp.Mode.survivalinventory.inv2Count[selIndex2] + Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] < 65)
                                            {
                                                Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = (byte)(ClassicalSharp.Mode.survivalinventory.inv2Count[selIndex2] + Mode.survivalinventory.invCount[game.Inventory.SelectedIndex]);
                                                Mode.survivalinventory.inv2Count[selIndex2] = oldcount;
                                            }
                                            game.Events.RaiseHeldBlockChanged();
                                            Recipes();
                                        }
                                        
                                       
                                    }
                                   
                                }

                               
                                



                            }
                        }


                        
                    } else //if creative
                    {
                        game.Inventory.Selected = blocksTable[selIndex];
                    }
                }


                else if (Contains(TableX, TableY, TableWidth, TableHeight, mouseX, mouseY)) {
					return true;
				}
				
				

                    lastpos = game.DesktopCursorPos;
                    game.Gui.SetNewScreen(null);
                    game.Gui.SetNewScreen(new InventoryScreen(game));

                    saveinventory();

                    game.DesktopCursorPos = lastpos;
            }
            ////***
            else if (button == MouseButton.Right) //if Right Click
            {
                
                if (selIndex != -1)
                {
                    
                    if ((Options.GetBool(OptionsKey.SurvivalMode, false)) == true || (Options.GetBool(OptionsKey.SurvivalMode, false)) == false)
                    {

                        if (!Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest) //if not crafting
                        {


                            byte oldcount;
                            byte oldcount2;

                            oldcount = Mode.survivalinventory.inv2Count[selIndex + 1];
                            oldcount2 = Mode.survivalinventory.invCount[game.Inventory.SelectedIndex];

                            if (game.Inventory[game.Inventory.SelectedIndex] == 0)
                            {
                                if (oldcount > 1)
                                {
                                    Mode.survivalinventory.inv2Count[selIndex + 1] = (byte)(Math.Floor(((decimal)Mode.survivalinventory.inv2Count[selIndex + 1] / 2)));
                                    game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.invBlocks[selIndex + 1];
                                    Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = (byte)(Math.Ceiling(((decimal)oldcount / 2)));
                                }
                            }
                            else
                            {
                                if (Mode.survivalinventory.invBlocks[selIndex + 1] == 0 || Mode.survivalinventory.invBlocks[selIndex + 1] == game.Inventory.Selected)
                                {
                                    if (oldcount2 > 1)
                                    {

                                        Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] -= 1;
                                        Mode.survivalinventory.invBlocks[selIndex + 1] = game.Inventory[game.Inventory.SelectedIndex];
                                        Mode.survivalinventory.inv2Count[selIndex + 1] += 1;

                                    }
                                }
                                

                            }


                            game.Events.RaiseHeldBlockChanged();
                        }

                        else if (!Mode.survivalinventory.iscrafting && Mode.survivalinventory.ischest) //if chest
                        {


                            byte oldcount;
                            byte oldcount2;

                            oldcount = Mode.survivalinventory.chestCount[selIndex + 1];
                            oldcount2 = Mode.survivalinventory.invCount[game.Inventory.SelectedIndex];

                            if (game.Inventory[game.Inventory.SelectedIndex] == 0)
                            {
                                if (oldcount > 1)
                                {
                                    Mode.survivalinventory.chestCount[selIndex + 1] = (byte)(Math.Floor(((decimal)Mode.survivalinventory.chestCount[selIndex + 1] / 2)));
                                    game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.chestBlocks[selIndex + 1];
                                    Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = (byte)(Math.Ceiling(((decimal)oldcount / 2)));
                                }
                            }
                            else
                            {
                                if (Mode.survivalinventory.chestBlocks[selIndex + 1] == 0 || Mode.survivalinventory.chestBlocks[selIndex + 1] == game.Inventory.Selected)
                                {
                                    if (oldcount2 > 1)
                                    {

                                        Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] -= 1;
                                        Mode.survivalinventory.chestBlocks[selIndex + 1] = game.Inventory[game.Inventory.SelectedIndex];
                                        Mode.survivalinventory.chestCount[selIndex + 1] += 1;

                                    }
                                }


                            }


                            game.Events.RaiseHeldBlockChanged();
                        }
                        else if (Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest)///////////////////if crafting///////////////////////////
                        {


                            int craftgrid3;
                            int craftgrid4;
                            int craftgrid5;
                            int craftgrid6;
                            int craftgrid7;
                            int craftgrid8;
                            int ix;
                            if (game.UseCPE)
                            {
                                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                                craftgrid3 = 9;
                                craftgrid4 = 10;
                                craftgrid5 = 11;
                                craftgrid6 = 18;
                                craftgrid7 = 19;
                                craftgrid8 = 20;
                            }
                            else
                            {
                                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                                craftgrid3 = 9;
                                craftgrid4 = 10;
                                craftgrid5 = 11;
                                craftgrid6 = 18;
                                craftgrid7 = 19;
                                craftgrid8 = 20;
                            }



                            if (selIndex == 0) { selIndex2 = ix + 1; }
                            if (selIndex == 1) { selIndex2 = ix + 2; }
                            if (selIndex == 2) { selIndex2 = ix + 3; }
                            if (selIndex == craftgrid3) { selIndex2 = ix + 4; }
                            if (selIndex == craftgrid4) { selIndex2 = ix + 5; }
                            if (selIndex == craftgrid5) { selIndex2 = ix + 6; }
                            if (selIndex == craftgrid6) { selIndex2 = ix + 7; }
                            if (selIndex == craftgrid7) { selIndex2 = ix + 8; }
                            if (selIndex == craftgrid8) { selIndex2 = ix + 9; }
                            if (selIndex == craftgrid5 + 2) { selIndex2 = ix + 10; }



                            if (blocksTable[selIndex] != Block.Bedrock)
                            {


                                //

                                byte oldcount;
                                byte oldcount2;
                                oldcount = Mode.survivalinventory.inv2Count[selIndex2];
                                oldcount2 = Mode.survivalinventory.invCount[game.Inventory.SelectedIndex];

                                if (selIndex2 != ix + 10)
                                {
                                    if (game.Inventory.Selected == 0)
                                    {
                                        if (oldcount > 1)
                                        {
                                            Mode.survivalinventory.inv2Count[selIndex2] = (byte)(Math.Floor(((decimal)Mode.survivalinventory.inv2Count[selIndex2] / 2)));
                                            game.Inventory[game.Inventory.SelectedIndex] = Mode.survivalinventory.invBlocks[selIndex2];
                                            Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] = (byte)(Math.Ceiling(((decimal)oldcount / 2)));
                                            game.Events.RaiseHeldBlockChanged();
                                        }
                                    }
                                    else
                                    {
                                        if (Mode.survivalinventory.invBlocks[selIndex2] == 0 || Mode.survivalinventory.invBlocks[selIndex2] == game.Inventory.Selected)
                                        {
                                            if (oldcount2 > 1)
                                            {

                                                Mode.survivalinventory.invCount[game.Inventory.SelectedIndex] -= 1;
                                                Mode.survivalinventory.invBlocks[selIndex2] = game.Inventory[game.Inventory.SelectedIndex];
                                                Mode.survivalinventory.inv2Count[selIndex2] += 1;

                                            }
                                            game.Events.RaiseHeldBlockChanged();
                                            Recipes();
                                        }

                                    }
                                }
                            }
                        }



                            }
                    
                }


                else if (Contains(TableX, TableY, TableWidth, TableHeight, mouseX, mouseY))
                {
                    return true;
                }

                bool hotbar = game.IsKeyDown(Key.AltLeft) || game.IsKeyDown(Key.AltRight);
                if (!hotbar)
                lastpos = game.DesktopCursorPos;
                game.Gui.SetNewScreen(null);
                game.Gui.SetNewScreen(new InventoryScreen(game));
                saveinventory();
                game.DesktopCursorPos = lastpos;
            }
            ////****
            return true;
		}

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

                for (int i = 0; i < 9; i++)
                {

                   
                    inventorydatasend = inventorydatasend + Mode.survivalinventory.invBlocks[i] + ";";
                    inventorydatacountsend = inventorydatacountsend + Mode.survivalinventory.inv2Count[i] + ";";
                    inventorydurasend = inventorydurasend + Mode.survivalinventory.inv2durability[i] + ";";

                    inventorydatasend2 = inventorydatasend2 + Mode.survivalinventory.invBlocks[i+9] + ";";
                    inventorydatacountsend2 = inventorydatacountsend2 + Mode.survivalinventory.inv2Count[i+9] + ";";
                    inventorydurasend2 = inventorydurasend2 + Mode.survivalinventory.inv2durability[i+9] + ";";

                   


                }

                for (int i = 0; i < 10; i++)
                {
                    inventorydatasend3 = inventorydatasend3 + Mode.survivalinventory.invBlocks[i + 18] + ";";
                    inventorydatacountsend3 = inventorydatacountsend3 + Mode.survivalinventory.inv2Count[i + 18] + ";";
                    inventorydurasend3 = inventorydurasend3 + Mode.survivalinventory.inv2durability[i + 18] + ";";
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
            if (Mode.survivalinventory.ischest)
            {
                savechest();
            }
        }


        string chestdatasend;
        string chestdatacountsend;
        string chestdurasend;

        string chestdatasend2;
        string chestdatacountsend2;
        string chestdurasend2;

        string chestdatasend3;
        string chestdatacountsend3;
        string chestdurasend3;

        void savechest()
        {

            if (game.Server.IsSinglePlayer)
            {
                System.IO.File.WriteAllBytes("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestBlocks.txt", Mode.survivalinventory.chestBlocks);
                System.IO.File.WriteAllBytes("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestCount.txt", Mode.survivalinventory.chestCount);
                System.IO.File.WriteAllBytes("maps/chests/" + game.World.Uuid + "-" + game.SelectedPos.BlockPos + "ChestDurability.txt", Mode.survivalinventory.chestdurability);
            }
            else
            {

                for (int i = 0; i < 9; i++)
                {

                    chestdatasend = chestdatasend + Mode.survivalinventory.chestBlocks[i] + ";";
                    chestdatacountsend = chestdatacountsend + Mode.survivalinventory.chestCount[i] + ";";
                    chestdurasend = chestdurasend + Mode.survivalinventory.chestdurability[i] + ";";

                    chestdatasend2 = chestdatasend2 + Mode.survivalinventory.chestBlocks[i+9] + ";";
                    chestdatacountsend2 = chestdatacountsend2 + Mode.survivalinventory.chestCount[i+9] + ";";
                    chestdurasend2 = chestdurasend2 + Mode.survivalinventory.chestdurability[i+9] + ";";


                }

                for (int i = 0; i < 10; i++)
                {

                    chestdatasend3 = chestdatasend3 + Mode.survivalinventory.chestBlocks[i + 18] + ";";
                    chestdatacountsend3 = chestdatacountsend3 + Mode.survivalinventory.chestCount[i + 18] + ";";
                    chestdurasend3 = chestdurasend3 + Mode.survivalinventory.chestdurability[i + 18] + ";";

                }

                game.Server.SendChests(chestdatasend,0);
                game.Server.SendChests(chestdatacountsend, 1);
                game.Server.SendChests(chestdurasend, 2);

                game.Server.SendChests(chestdatasend2, 3);
                game.Server.SendChests(chestdatacountsend2, 4);
                game.Server.SendChests(chestdurasend2, 5);

                game.Server.SendChests(chestdatasend3, 6);
                game.Server.SendChests(chestdatacountsend3, 7);
                game.Server.SendChests(chestdurasend3, 8);


            }
        } 

        public void RemoveFromCraftingGrid(string startrecipe)
        {
            //Remove everything when crafted *Not Working*

            

            int craftgrid3;
            int craftgrid4;
            int craftgrid5;
            int craftgrid6;
            int craftgrid7;
            int craftgrid8;
            int ix;
            if (game.UseCPE)
            {
                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                craftgrid3 = 9;
                craftgrid4 = 10;
                craftgrid5 = 11;
                craftgrid6 = 18;
                craftgrid7 = 19;
                craftgrid8 = 20;
            }
            else
            {
                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                craftgrid3 = 9;
                craftgrid4 = 10;
                craftgrid5 = 11;
                craftgrid6 = 18;
                craftgrid7 = 19;
                craftgrid8 = 20;
            }

            if (selIndex == craftgrid5 + 2)
            {


                for (int c = 0; c < 10; c++)
                {
                    if (Mode.survivalinventory.inv2Count[ix + c] > 0)
                    {
                        Mode.survivalinventory.inv2Count[ix + c] -= 1;
                       
                    }
                    
                   
                }

                for (int c = 0; c < 10; c++)
                {
                   
                    if (Mode.survivalinventory.inv2Count[ix + c] < 1)
                    {
                        Mode.survivalinventory.invBlocks[ix + c] = 0;
                        Mode.survivalinventory.inv2durability[ix + 10] = 0;
                    }
                }


                string newrecipe = GenRecipe();

                

                if (newrecipe != startrecipe)
                {
                    Mode.survivalinventory.invBlocks[ix + 10] = 0;
                    Mode.survivalinventory.inv2Count[ix + 10] = 0;
                    Mode.survivalinventory.inv2durability[ix + 10] = 0;
                }
            }
            
        }

        string GenRecipe()
        {

            int ix;
            if (game.UseCPE)
            {
                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
            }
            else
            {
                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
            }
            //craftingRecipe

            string[] craftinggrid = new string[9];
            craftinggrid[0] = "" + Mode.survivalinventory.invBlocks[ix + 1];
            craftinggrid[1] = "" + Mode.survivalinventory.invBlocks[ix + 2];
            craftinggrid[2] = "" + Mode.survivalinventory.invBlocks[ix + 3];
            craftinggrid[3] = "" + Mode.survivalinventory.invBlocks[ix + 4];
            craftinggrid[4] = "" + Mode.survivalinventory.invBlocks[ix + 5];
            craftinggrid[5] = "" + Mode.survivalinventory.invBlocks[ix + 6];
            craftinggrid[6] = "" + Mode.survivalinventory.invBlocks[ix + 7];
            craftinggrid[7] = "" + Mode.survivalinventory.invBlocks[ix + 8];
            craftinggrid[8] = "" + Mode.survivalinventory.invBlocks[ix + 9];


            int sizex = 0;
            int sizey = 0;
            string sizetotal;
            int tempsizex = 0;
            int tempsizey = 0;
            for (int x = 0; x < 3; x++)
            {
                if (craftinggrid[x] != "0")
                {
                    sizex++;

                }
            }
            for (int x = 0; x < 3; x++)
            {
                if (craftinggrid[x + 3] != "0")
                {
                    tempsizex++;
                    if(sizex < tempsizex)
                    {
                    sizex = tempsizex;
                    }
                    

                }
            }
            tempsizex = 0;
            for (int x = 0; x < 3; x++)
            {
                if (craftinggrid[x + 6] != "0")
                {
                    tempsizex++;
                    if (sizex < tempsizex)
                    {
                        sizex = tempsizex;
                    }
                }
            }
            //
            for (int x = 0; x < 3; x++)
            {
                if (craftinggrid[x * 3] != "0")
                {
                    tempsizey++;
                    if (sizey < tempsizey)
                    {
                        sizey = tempsizey;
                    }

                }
            }
            tempsizey = 0;

            for (int x = 0; x < 3; x++)
            {
                if (craftinggrid[x * 3 + 1] != "0")
                {
                    tempsizey++;
                    if (sizey < tempsizey)
                    {
                        sizey = tempsizey;
                    }

                }
            }
            tempsizey = 0;
            for (int x = 0; x < 3; x++)
            {
                if (craftinggrid[x * 3 + 2] != "0")
                {
                    tempsizey++;
                    if (sizey < tempsizey)
                    {
                        sizey = tempsizey;
                    }
                }


            }


            //If Id = air set string to "" (nothing)
            for (int g = 0; g < 9; g++)
            {
                if (craftinggrid[g] == "0")
                {
                    craftinggrid[g] = "";
                }
            }

           

            sizetotal = "(" + sizex + "x" + sizey + ")";

            //Store the grid items in a string
            string recipe;
            recipe = sizetotal + craftinggrid[0] + "" + craftinggrid[1] + "" + craftinggrid[2] + "" + craftinggrid[3] + "" + craftinggrid[4] + "" + craftinggrid[5] + "" + craftinggrid[6] + "" + craftinggrid[7] + "" + craftinggrid[8];
           

            //game.Chat.Add("Recipe Code:" + recipe);
            return recipe;
        }

        string Recipes()
        {

            int ix;
            if (game.UseCPE)
            {
                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
            }
            else
            {
                ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
            }

            string recipe = GenRecipe();

            //Recipes
            if (recipe == "(1x1)17")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Wood;
                Mode.survivalinventory.inv2Count[ix + 10] = 4;
                RemoveFromCraftingGrid(recipe);
                
            }
            else if (recipe == "(1x2)3736")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Yellow;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(1x2)3836")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Red;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(1x2)636")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Green;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(3x3)121212121212121212")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Glass;
                Mode.survivalinventory.inv2Count[ix + 10] = 3;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(3x3)131313131313131313")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Brick;
                Mode.survivalinventory.inv2Count[ix + 10] = 3;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(3x1)444")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Slab;
                Mode.survivalinventory.inv2Count[ix + 10] = 6;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(1x1)43")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Slab;
                Mode.survivalinventory.inv2Count[ix + 10] = 2;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(3x3)131213121312131213")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.TNT;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(1x2)55")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Stick;
                Mode.survivalinventory.inv2Count[ix + 10] = 4;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(2x3)555555")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.DoorBase;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(3x2)555555")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.TrapDoor;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(3x3)68686868686868")
            {
                Mode.survivalinventory.invBlocks[ix + 10] = Block.Ladder1;
                Mode.survivalinventory.inv2Count[ix + 10] = 3;
                RemoveFromCraftingGrid(recipe);
            }
            else if (recipe == "(3x3)5556868")
            {
                
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.WoodPick;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 33;
                    RemoveFromCraftingGrid(recipe);
                
            }
            else if (recipe == "(3x3)4446868") 
            {
                
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.StonePick;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 65;
                    RemoveFromCraftingGrid(recipe);
                
            }
            else if (recipe == "(1x3)5568")
            {
                
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.WoodSword;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 33;
                    RemoveFromCraftingGrid(recipe);
                
            }
            else if (recipe == "(1x3)4468")
            {
                
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.StoneSword;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 65;
                    RemoveFromCraftingGrid(recipe);
                
            }
            else if (recipe == "(1x3)56868")
            {

                
                    RemoveFromCraftingGrid(recipe);
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.WoodShovel;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 33;
                    
                    
                
            }
            else if (recipe == "(1x3)46868")
            {
                
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.StoneShovel;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 65;
                    RemoveFromCraftingGrid(recipe);
                    
                
            }
            else if (recipe == "(2x3)5556868")
            {
                
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.WoodAxe;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 33;
                    RemoveFromCraftingGrid(recipe);
                
            }
            else if (recipe == "(2x3)4446868")
            {
               
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.StoneAxe;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 65;
                    RemoveFromCraftingGrid(recipe);
                
            }
            else if (recipe == "(1x3)424268")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.IronSword;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 129;
                RemoveFromCraftingGrid(recipe);

            }
            else if (recipe == "(1x3)426868")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.IronShovel;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 129;
                RemoveFromCraftingGrid(recipe);


            }
            else if (recipe == "(2x3)4242426868")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.IronAxe;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 129;
                RemoveFromCraftingGrid(recipe);

            }
            else if (recipe == "(3x3)4242426868")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.IronPick;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 129;
                RemoveFromCraftingGrid(recipe);

            }

            else if (recipe == "(1x3)676768")
            {
                
                    Mode.survivalinventory.invBlocks[ix + 10] = Block.DiamondSword;
                    Mode.survivalinventory.inv2Count[ix + 10] = 1;
                    Mode.survivalinventory.inv2durability[ix + 10] = 255;
                    RemoveFromCraftingGrid(recipe);
                

            }
            else if (recipe == "(1x3)676868")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.DiamondShovel;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 255;
                RemoveFromCraftingGrid(recipe);


            }
            else if (recipe == "(2x3)6767676868")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.DiamondAxe;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 255;
                RemoveFromCraftingGrid(recipe);

            }
            else if (recipe == "(3x3)6767676868")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.DiamondPick;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 255;
                RemoveFromCraftingGrid(recipe);

            }
            else if (recipe == "(2x2)4444")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.StoneBrick;
                Mode.survivalinventory.inv2Count[ix + 10] = 4;
                Mode.survivalinventory.inv2durability[ix + 10] = 0;
                RemoveFromCraftingGrid(recipe);

            }
            else if (recipe == "(3x3)555363636555")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.Bookshelf;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 0;
                RemoveFromCraftingGrid(recipe);

            }
            else if (recipe == "(3x3)55555555")
            {

                Mode.survivalinventory.invBlocks[ix + 10] = Block.Chest2;
                Mode.survivalinventory.inv2Count[ix + 10] = 1;
                Mode.survivalinventory.inv2durability[ix + 10] = 0;
                RemoveFromCraftingGrid(recipe);

            }

            else
            {
                Mode.survivalinventory.invBlocks[ix + 10] = 0;
                Mode.survivalinventory.inv2Count[ix + 10] = 0;
            }
            
            return recipe;
        }

		// We want the user to be able to press B to exit the inventory menu
		// however since we use KeyRepeat = true, we must wait until the first time they released B
		// before marking the next press as closing the menu
		bool releasedInv;
		public override bool HandlesKeyDown(Key key) {
			if (key == game.Mapping(KeyBind.PauseOrExit)) {
				game.Gui.SetNewScreen(null);
			} else if (key == game.Mapping(KeyBind.Inventory) && releasedInv) {
				game.Gui.SetNewScreen(null);
			} else if (key == Key.Enter && selIndex != -1) {
				game.Inventory.Selected = blocksTable[selIndex];
				game.Gui.SetNewScreen(null);
			} else if ((key == Key.Left || key == Key.Keypad4) && selIndex != -1) {
				ArrowKeyMove(-1);
			} else if ((key == Key.Right || key == Key.Keypad6) && selIndex != -1) {
				ArrowKeyMove(1);
			} else if ((key == Key.Up || key == Key.Keypad8) && selIndex != -1) {
				ArrowKeyMove(-blocksPerRow);
			} else if ((key == Key.Down || key == Key.Keypad2) && selIndex != -1) {
				ArrowKeyMove(blocksPerRow);
			} else if (game.Gui.hudScreen.hotbar.HandlesKeyDown(key)) {
			}
			return true;
		}
		
		public override bool HandlesKeyUp(Key key) {
			if (key == game.Mapping(KeyBind.Inventory)) {
				releasedInv = true; return true;
			}
			return game.Gui.hudScreen.hotbar.HandlesKeyUp(key);
		}
		 
		void ArrowKeyMove(int delta) {
			int startIndex = selIndex;
			selIndex += delta;
			if (selIndex < 0)
				selIndex -= delta;
			if (selIndex >= blocksTable.Length)
				selIndex -= delta;
			
			int scrollDelta = (selIndex / blocksPerRow) - (startIndex / blocksPerRow);
			scrollY += scrollDelta;
			ClampScrollY();
			RecreateBlockInfoTexture();
			MoveCursorToSelected();
		}
	}
}
