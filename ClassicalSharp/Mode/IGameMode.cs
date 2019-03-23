// Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
using System;
using System.Collections.Generic;
using ClassicalSharp.Gui.Widgets;
using OpenTK.Input;

#if USE16_BIT
using BlockID = System.UInt16;
#else
using BlockID = System.Byte;
#endif

namespace ClassicalSharp.Mode {

    public class survivalinventory
    {

       static internal byte[] invCount = new byte[Inventory.BlocksPerRow  * Inventory.Rows ];
        static internal byte[] invdurability = new byte[Inventory.BlocksPerRow * Inventory.Rows];
        static internal byte[] inv2Count = new byte[150];
        static internal byte[] invBlocks = new byte[150];
        static internal byte[] inv2durability = new byte[150];
        static internal byte[] chestCount = new byte[150];
        static internal byte[] chestBlocks = new byte[150];
        static internal byte[] chestdurability = new byte[150];
        static public bool iscrafting;
        static public bool ischest;
        static public int blocknb = 97;
        static public int lessblocknbclassic = 7;
        static public List<int> Tools = new List<int>(new int[] { Block.WoodPick, Block.StonePick, Block.WoodSword, Block.StoneSword, Block.WoodShovel, Block.StoneShovel, Block.WoodAxe, Block.StoneAxe, Block.IronSword, Block.IronShovel , Block.IronPick, Block.IronAxe , Block.DiamondSword , Block.DiamondShovel , Block.DiamondPick , Block.DiamondAxe });
        static public List<int> Items = new List<int>(new int[] { Block.Stick, Block.WoodPick, Block.StonePick, Block.WoodSword, Block.StoneSword, Block.WoodShovel, Block.StoneShovel, Block.WoodAxe, Block.StoneAxe, Block.Redstone, Block.IronSword, Block.IronShovel, Block.IronPick, Block.IronAxe, Block.DiamondSword, Block.DiamondShovel, Block.DiamondPick, Block.DiamondAxe });




    }

	public interface IGameMode : IGameComponent {

        
        bool HandlesKeyDown(Key key);
        void PickLeft(BlockID old);
		void PickMiddle(BlockID old);
		void PickRight(BlockID old, BlockID block);
		bool PickEntity(byte id);
		Widget MakeHotbar();
	}
}
