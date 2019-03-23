// Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
using System;
using System.Drawing;
using ClassicalSharp.GraphicsAPI;
using OpenTK.Input;


#if USE16_BIT
using BlockID = System.UInt16;
#else
using BlockID = System.Byte;
#endif

namespace ClassicalSharp.Gui.Screens {
	public partial class InventoryScreen : Screen {
		
		public InventoryScreen(Game game) : base(game) {
			font = new Font(game.FontName, 16);
		}
		
		BlockID[] blocksTable;
		Texture blockInfoTexture;
		const int maxRows = 8;
		int blocksPerRow {
			get { return game.ClassicMode && !game.ClassicHacks ? 9 : 9; }
		}
		
		int selIndex, rows;
		int startX, blockSize;
        int startY;
		float selBlockExpand;
		readonly Font font;
		StringBuffer buffer = new StringBuffer(128);
		IsometricBlockDrawer drawer = new IsometricBlockDrawer();
		
		int TableX { get { return startX - 5 - 10; } }
		int TableY { get { return startY - 5 - 30 + 200; } }
		int TableWidth { get { return blocksPerRow * blockSize + 10 + 20; } }
		int TableHeight { get { return Math.Min(rows, maxRows) * blockSize + 10 + 40 - (blockSize*4); } }
		
		// These were sourced by taking a screenshot of vanilla
		// Then using paint to extract the colour components
		// Then using wolfram alpha to solve the glblendfunc equation
		static FastColour topCol = new FastColour(198, 198, 198, 255);
		static FastColour bottomCol = new FastColour(198, 198, 198, 255);
		static FastColour topSelCol = new FastColour(139, 139, 139, 255);
		static FastColour bottomSelCol = new FastColour(139, 139, 139, 255);

       
        

        static VertexP3fT2fC4b[] vertices = new VertexP3fT2fC4b[8 * 10 * (4 * 4)];
		int vb;
		public override void Render(double delta) {
			gfx.Draw2DQuad(TableX, TableY, TableWidth, TableHeight, topCol, bottomCol);
			if (rows > maxRows)
				DrawScrollbar();
			
			
				int x2, y2;
            for (int i = 0; i < 27; i++)
            {
                if (blocksTable[i] != Block.Bedrock)
                {
                    GetCoords(i, out x2, out y2);
                    float off = blockSize * 0.1f;
                    gfx.Draw2DQuad(x2 - off * -0.05f, y2 - off * -0.07f, blockSize - 2,
                                   blockSize - 2, topSelCol, bottomSelCol);
                }
            }
			
			gfx.Texturing = true;
			gfx.SetBatchFormat(VertexFormat.P3fT2fC4b);

            drawer.BeginBatch(game, vertices, vb);
			for (int i = 0; i < blocksTable.Length - 70; i++) {
                if (blocksTable[i] != Block.Bedrock)
                {
                    int x, y;
                    if (!GetCoords(i, out x, out y)) continue;

                    // We want to always draw the selected block on top of others
                    if (i == selIndex) continue;
                    drawer.DrawBatch(blocksTable[i], blockSize * 0.7f / 2f,
                                     x + blockSize / 2, y + blockSize / 2);

                    
                }
			}
			
			if (selIndex != -1) {
                if (selIndex < 27)
                {
                    if (blocksTable[selIndex] != Block.Bedrock)
                    {
                        int x, y;
                        GetCoords(selIndex, out x, out y);
                        drawer.DrawBatch(blocksTable[selIndex], (blockSize + selBlockExpand) * 0.7f / 2,
                                         x + blockSize / 2, y + blockSize / 2);







                    }
                }
			}
			drawer.EndBatch();
			
			if (blockInfoTexture.IsValid)
				blockInfoTexture.Render(gfx);
			gfx.Texturing = false;
		}

       

        bool GetCoords(int i, out int x, out int y) {
			int col = i % blocksPerRow;
			int row = i / blocksPerRow;
			x = startX + blockSize * col;
			y = (startY + blockSize * row + 3) + 200;
		    
			
			return row >= 0 && row < maxRows;
		}
		
		Point GetMouseCoords(int i) {
			int x, y;
			GetCoords(i, out x, out y);
			x += blockSize / 2; y += blockSize / 2;
			
			Point topLeft = game.PointToScreen(Point.Empty);
			x += topLeft.X; y += topLeft.Y;
			return new Point(x, y);
		}
		
		public override void Dispose() {
			font.Dispose();
			game.Events.BlockPermissionsChanged -= BlockPermissionsChanged;
			game.Keyboard.KeyRepeat = false;
			
			ContextLost();
			game.Graphics.ContextLost -= ContextLost;
			game.Graphics.ContextRecreated -= ContextRecreated;
		}
		
		public override void OnResize(int width, int height) {
			blockSize = (int)(50 * Math.Sqrt(game.GuiInventoryScale));
			selBlockExpand = (float)(25 * Math.Sqrt(game.GuiInventoryScale));
			
			int rowsUsed = Math.Min(maxRows, rows);
			startX = game.Width / 2 - (blockSize * blocksPerRow) / 2;
			startY = game.Height / 2 - (rowsUsed * blockSize) / 2;
			blockInfoTexture.X1 = startX + (blockSize * blocksPerRow) / 2 - blockInfoTexture.Width / 2;
			blockInfoTexture.Y1 = startY - blockInfoTexture.Height - 5;
		}
		
		public override void Init() {
			blockSize = (int)(50 * Math.Sqrt(game.GuiInventoryScale));
			selBlockExpand = (float)(25 * Math.Sqrt(game.GuiInventoryScale));
			game.Events.BlockPermissionsChanged += BlockPermissionsChanged;
			
			ContextRecreated();
			game.Graphics.ContextLost += ContextLost;
			game.Graphics.ContextRecreated += ContextRecreated;
			
			RecreateBlockTable();
			SetBlockTo(game.Inventory.Selected);
			game.Keyboard.KeyRepeat = true;


            

		}
		
		public void SetBlockTo(BlockID block) {
			selIndex = Array.IndexOf<BlockID>(blocksTable, block);
			scrollY = (selIndex / blocksPerRow) - (maxRows - 1);
			ClampScrollY();
			MoveCursorToSelected();
			RecreateBlockInfoTexture();
		}
		
		void MoveCursorToSelected() {
			if (selIndex == -1) return;
			//game.DesktopCursorPos = GetMouseCoords(selIndex);
		}

		void BlockPermissionsChanged(object sender, EventArgs e) {
			RecreateBlockTable();
			if (selIndex >= blocksTable.Length)
				selIndex = blocksTable.Length - 1;
			
			scrollY = selIndex / blocksPerRow;
			ClampScrollY();
			RecreateBlockInfoTexture();
		}

        void UpdateBlockInfoString(BlockID block)
        {
            if (selIndex < 27 && blocksTable[selIndex] != Block.Bedrock)
            {
               
                    int index = 0;
                    buffer.Clear();
                    buffer.Append(ref index, "&f");
                    string value = game.BlockInfo.Name[block];
                if (blocksTable[selIndex] != Block.Air)
                {
                    buffer.Append(ref index, value);
                }
                else
                {
                    if (!Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest)
                    {
                        buffer.Append(ref index, "Inventory");
                    }
                    else if (Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest)
                    {

                        buffer.Append(ref index, "Crafting");
                    }
                    else if (!Mode.survivalinventory.iscrafting && Mode.survivalinventory.ischest)
                    {
                        buffer.Append(ref index, "Chest");
                    }
                }


                if (blocksTable[selIndex] != Block.Air)
                {

                    if (!Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest)
                    {
                        buffer.Append(ref index, " (Quantity: ");
                        buffer.Append(ref index, Mode.survivalinventory.inv2Count[selIndex + 1] + ")");

                        buffer.Append(ref index, " (Durability: ");
                        buffer.Append(ref index, Mode.survivalinventory.inv2durability[selIndex + 1] + ")");

                    }
                    else if (!Mode.survivalinventory.iscrafting && Mode.survivalinventory.ischest)
                    {
                        buffer.Append(ref index, " (Quantity: ");
                        buffer.Append(ref index, Mode.survivalinventory.chestCount[selIndex + 1] + ")");

                        buffer.Append(ref index, " (Durability: ");
                        buffer.Append(ref index, Mode.survivalinventory.chestdurability[selIndex + 1] + ")");

                    }
                    else if (Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest)
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

                        buffer.Append(ref index, " (Quantity: ");
                        buffer.Append(ref index, Mode.survivalinventory.inv2Count[selIndex2] + ")");

                        buffer.Append(ref index, " (Durability: ");
                        buffer.Append(ref index, Mode.survivalinventory.inv2durability[selIndex2] + ")");
                    }
                }
            }
        }
		
		int lastCreatedIndex = -1000;
		void RecreateBlockInfoTexture() {
			if (selIndex == lastCreatedIndex || blocksTable == null) return;
			lastCreatedIndex = selIndex;
			
			gfx.DeleteTexture(ref blockInfoTexture);
			if (selIndex == -1) return;
			
			BlockID block = blocksTable[selIndex];
			UpdateBlockInfoString(block);
			string value = buffer.ToString();
			
			DrawTextArgs args = new DrawTextArgs(value, font, true);
			Size size = game.Drawer2D.MeasureSize(ref args);
			int x = startX + (blockSize * blocksPerRow) / 2 - size.Width / 2;
			int y = startY - size.Height - 5;
			
			args.SkipPartsCheck = true;
			blockInfoTexture = game.Drawer2D.MakeTextTexture(ref args, x, y+420);
		}
		
		void RecreateBlockTable() {
			int blocksCount = 0;
			int count = game.UseCPE ? Block.Count : Block.OriginalCount;
			for (int i = 1; i < count; i++) {
				BlockID block = game.Inventory.MapBlock(i);
				if (Show(block)) blocksCount++;
			}
			
			rows = Utils.CeilDiv(blocksCount, blocksPerRow);
			int rowsUsed = Math.Min(maxRows, rows);
			startX = game.Width / 2 - (blockSize * blocksPerRow) / 2;
			startY = game.Height / 2 - (rowsUsed * blockSize) / 2;
			blocksTable = new BlockID[blocksCount];
			
			int index = 0;
            /*if ((Options.GetBool(OptionsKey.SurvivalMode, false)) == false) //Creative inventory Set
            {
                for (int i = 1; i < count; i++)
                {
                    BlockID block = game.Inventory.MapBlock(i);
                    if (Show(block)) blocksTable[index++] = block; 

                }
            }*/

            if (!Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest) //Survival Inventory Set
            {
                int ix;
                if (game.UseCPE)
                {
                    ix = Mode.survivalinventory.blocknb;
                }
                else
                {
                    ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                }

                for (int i = 1; i < ix; i++)
                {

                    
                }


                for (int i = 1; i < ix; i++)
                {

                    BlockID block;
                    if (i > 27)
                    {
                        block = Block.Bedrock;
                    }
                    else
                    {
                        block = Mode.survivalinventory.invBlocks[i];
                    }
                    
                        blocksTable[index++] = block; 

                }

                
                
             }
            if (Mode.survivalinventory.iscrafting && !Mode.survivalinventory.ischest) //Crafting Inventory Set
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

               

                for (int i = 1; i < ix; i++)
                {

                    BlockID block = Mode.survivalinventory.invBlocks[i];
                    if (Show(block)) blocksTable[index++] = 7; blocksTable[0] = Mode.survivalinventory.invBlocks[ix+1];
                                                               blocksTable[1] = Mode.survivalinventory.invBlocks[ix+2];
                                                               blocksTable[2] = Mode.survivalinventory.invBlocks[ix+3];
                                                               blocksTable[craftgrid3] = Mode.survivalinventory.invBlocks[ix+4];
                                                               blocksTable[craftgrid4] = Mode.survivalinventory.invBlocks[ix+5];
                                                               blocksTable[craftgrid5] = Mode.survivalinventory.invBlocks[ix+6];
                                                               blocksTable[craftgrid6] = Mode.survivalinventory.invBlocks[ix+7];
                                                               blocksTable[craftgrid7] = Mode.survivalinventory.invBlocks[ix+8];
                                                               blocksTable[craftgrid8] = Mode.survivalinventory.invBlocks[ix+9];
                                                               blocksTable[craftgrid5 + 2] = Mode.survivalinventory.invBlocks[ix + 10];


                }
                
                


            }

            if (!Mode.survivalinventory.iscrafting && Mode.survivalinventory.ischest) //Chest Inventory Set
            {

                int ix;
                if (game.UseCPE)
                {
                    ix = Mode.survivalinventory.blocknb;
                }
                else
                {
                    ix = Mode.survivalinventory.blocknb - Mode.survivalinventory.lessblocknbclassic;
                }

                for (int i = 1; i < ix; i++)
                {

                    BlockID block;
                    if (i > 27)
                    {
                        block = Block.Bedrock;
                    }
                    else
                    {
                        block = Mode.survivalinventory.chestBlocks[i];
                    }

                    blocksTable[index++] = block;

                }


            }



        }
		
		bool Show(BlockID block) {
			if (game.PureClassic && IsHackBlock(block)) return false;
			if (block < Block.CpeCount) {
				int count = game.UseCPEBlocks ? Block.CpeCount : Block.OriginalCount;
				return block < count && game.BlockInfo.Name[block] != "Invalid";
			}
			return game.BlockInfo.Name[block] != "Invalid";
		}
		
		bool IsHackBlock(BlockID block) {
			return block == Block.DoubleSlab || block == Block.Bedrock ||
				block == Block.Grass || game.BlockInfo.IsLiquid(block);
		}
		
		protected override void ContextLost() { 
			gfx.DeleteVb(ref vb);
			gfx.DeleteTexture(ref blockInfoTexture);
			lastCreatedIndex = -1000;
		}
		
		protected override void ContextRecreated() {
			vb = gfx.CreateDynamicVb(VertexFormat.P3fT2fC4b, vertices.Length);
			RecreateBlockInfoTexture();
		}
	}
}
