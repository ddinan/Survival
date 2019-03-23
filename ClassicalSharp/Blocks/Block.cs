// Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
using System;

#if USE16_BIT
using BlockID = System.UInt16;
#else
using BlockID = System.Byte;
#endif

namespace ClassicalSharp {
	
	/// <summary> Enumeration of all blocks in Minecraft Classic, including CPE ones. </summary>
	public static class Block {

#pragma warning	disable 1591
		public const BlockID Air = 0;
		public const BlockID Stone = 1;
		public const BlockID Grass = 2;
		public const BlockID Dirt = 3;
		public const BlockID Cobblestone = 4;
		public const BlockID Wood = 5;
		public const BlockID Sapling = 6;
		public const BlockID Bedrock = 7;
		public const BlockID Water = 8;
		public const BlockID StillWater = 9;
		public const BlockID Lava = 10;
		public const BlockID StillLava = 11;
		public const BlockID Sand = 12;
		public const BlockID Gravel = 13;
		public const BlockID GoldOre = 14;
		public const BlockID IronOre = 15;
		public const BlockID CoalOre = 16;
		public const BlockID Log = 17;
		public const BlockID Leaves = 18;
		public const BlockID Sponge = 19;
		public const BlockID Glass = 20;
		public const BlockID Red = 21;
		public const BlockID Orange = 22;
		public const BlockID Yellow = 23;
		public const BlockID Lime = 24;
		public const BlockID Green = 25;
		public const BlockID Teal = 26;
		public const BlockID Aqua = 27;
		public const BlockID Cyan = 28;
		public const BlockID Blue = 29;
		public const BlockID Indigo = 30;
		public const BlockID Violet = 31;
		public const BlockID Magenta = 32;
		public const BlockID Pink = 33;
		public const BlockID Black = 34;
		public const BlockID Gray = 35;
		public const BlockID White = 36;
		public const BlockID Dandelion = 37;
		public const BlockID Rose = 38;
		public const BlockID BrownMushroom = 39;
		public const BlockID RedMushroom = 40;
		public const BlockID Gold = 41;
		public const BlockID Iron = 42;
		public const BlockID DoubleSlab = 43;
		public const BlockID Slab = 44;
		public const BlockID Brick = 45;
		public const BlockID TNT = 46;
		public const BlockID Bookshelf = 47;
		public const BlockID MossyRocks = 48;
		public const BlockID Obsidian = 49;
		
		public const BlockID CobblestoneSlab = 50;
		public const BlockID Rope = 51;
		public const BlockID Sandstone = 52;
		public const BlockID Snow = 53;
		public const BlockID Fire = 54;
		public const BlockID LightPink = 55;
		public const BlockID ForestGreen = 56;
		public const BlockID Brown = 57;
		public const BlockID DeepBlue = 58;
		public const BlockID Turquoise = 59;
		public const BlockID Ice = 60;
		public const BlockID CeramicTile = 61;
		public const BlockID Magma = 62;
		public const BlockID Pillar = 63;
		public const BlockID Crate = 64;
		public const BlockID StoneBrick = 65;
        public const BlockID DiamondOre = 66;
        public const BlockID Diamond = 67;
        public const BlockID Stick = 68;
        public const BlockID WoodPick = 69;
        public const BlockID StonePick = 70;
        public const BlockID RedstoneOre = 71;
        public const BlockID DoorBase = 72;
        public const BlockID DoorBase1 = 73;
        public const BlockID DoorBase2 = 74;
        public const BlockID DoorBase3 = 75;
        public const BlockID DoorTop = 76;
        public const BlockID DoorTop1 = 77;
        public const BlockID DoorTop2 = 78;
        public const BlockID DoorTop3 = 79;
        public const BlockID TrapDoor = 80;
        public const BlockID TrapDoorOpen = 81;
        public const BlockID Ladder1 = 82;
        public const BlockID Ladder2 = 83;
        public const BlockID Ladder3 = 84;
        public const BlockID Ladder4 = 85;
        public const BlockID WoodSword = 86;
        public const BlockID StoneSword = 87;
        public const BlockID WoodShovel = 88;
        public const BlockID StoneShovel = 89;
        public const BlockID WoodAxe = 90;
        public const BlockID StoneAxe = 91;
        public const BlockID Redstone = 92;
        public const BlockID RedstoneGOff = 93;
        public const BlockID RedstoneGOff1 = 94;
        public const BlockID RedstoneGOff2 = 95;
        public const BlockID RedstoneGOff3 = 96;
        public const BlockID RedstoneGOff4 = 97;
        public const BlockID IronSword = 98;
        public const BlockID IronShovel = 99;
        public const BlockID IronPick = 100;
        public const BlockID IronAxe = 101;
        public const BlockID DiamondSword = 102;
        public const BlockID DiamondShovel = 103;
        public const BlockID DiamondPick = 104;
        public const BlockID DiamondAxe = 105;
        public const BlockID Chest1 = 106;
        public const BlockID Chest2 = 107;
        public const BlockID Chest3 = 108;
        public const BlockID Chest4 = 109;

#pragma warning restore 1591

        internal const string Names = "Air Stone Grass Dirt Cobblestone Wood Sapling Bedrock Water StillWater Lava" +
			" StillLava Sand Gravel GoldOre IronOre CoalOre Log Leaves Sponge Glass Red Orange Yellow Lime Green" +
			" Teal Aqua Cyan Blue Indigo Violet Magenta Pink Black Gray White Dandelion Rose BrownMushroom RedMushroom" +
			" Gold Iron DoubleSlab Slab Brick TNT Bookshelf MossyRocks Obsidian CobblestoneSlab Rope Sandstone" +
            " Snow Fire LightPink ForestGreen Brown DeepBlue Turquoise Ice CeramicTile Magma Pillar Crate StoneBrick DiamondOre Diamond Stick WoodPick StonePick RedstoneOre DoorBase-N DoorBase-S DoorBase-W DoorBase-E DoorTop-N DoorTop-S DoorTop-W DoorTop-E TrapDoor TrapDoorOpen Ladder-N Ladder-S Ladder-W Ladder-E WoodSword StoneSword WoodShovel StoneShovel WoodAxe StoneAxe Redstone RedstoneGOff RedstoneGOff1-N RedstoneGOff1-S RedstoneGOff1-W RedstoneGOff1-E IronSword IronShovel IronPick IronAxe DiamondSword DiamondShovel DiamondPick DiamondAxe Chest-N Chest-S Chest-W Chest-E";		
		
		/// <summary> Max block ID used in original classic. </summary>
		public const BlockID MaxOriginalBlock = Block.Chest4;
		
		/// <summary> Number of blocks in original classic. </summary>
		public const int OriginalCount = MaxOriginalBlock + 1;
		
		/// <summary> Max block ID used in original classic plus CPE blocks. </summary>
		public const BlockID MaxCpeBlock = Block.Chest4;

        /// <summary> Number of blocks in original classic plus CPE blocks. </summary>		
        public const int CpeCount = MaxCpeBlock + 1;
		
		#if USE16_BIT
		public const BlockID MaxDefinedBlock = 0xFFF;
		#else
		public const BlockID MaxDefinedBlock =  0xFF;
		#endif
		
		public const int Count = MaxDefinedBlock + 1;
		public const BlockID Invalid = MaxDefinedBlock;
	}
}