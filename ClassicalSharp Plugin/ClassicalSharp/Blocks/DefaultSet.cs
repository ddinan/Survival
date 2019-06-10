// Copyright 2014-2017 ClassicalSharp | Licensed under BSD-3
using System;
using OpenTK;

#if USE16_BIT
using BlockID = System.UInt16;
#else
using BlockID = System.Byte;
#endif

namespace ClassicalSharp.Blocks {
	
	/// <summary> Stores default properties for blocks in Minecraft Classic. </summary>
	public static class DefaultSet {
		
		public static float Height(BlockID b) {
			if (b == Block.Slab) return 8/16f;
			if (b == Block.CobblestoneSlab) return 8/16f;
            if (b == Block.Snow) return 2/16f;
            if(b == Block.TrapDoor) return 3 / 16f;
            if (b == Block.RedstoneGOff) return 1 / 16f;
            if (b == Block.RedstoneGOff1) return 1 / 16f;
            if (b == Block.RedstoneGOff2) return 1 / 16f;
            if (b == Block.RedstoneGOff3) return 1 / 16f;
            if (b == Block.RedstoneGOff4) return 1 / 16f;

            return 1;
		}

        public static float WidthX(BlockID b)
        {
            
            if (b == Block.DoorBase || b == Block.DoorBase1 || b == Block.DoorTop || b == Block.DoorTop1) return 3 / 16f;
            if (b == Block.Ladder3 || b == Block.Ladder4) return 1 / 16f;
            if (b == Block.TrapDoorOpen) return 3 / 16f;
            
            return 1;
        }

        public static float WidthZ(BlockID b)
        {

            if (b == Block.DoorBase2 || b == Block.DoorBase3 || b == Block.DoorTop2 || b == Block.DoorTop3) return 3 / 16f;
            if (b == Block.Ladder1 || b == Block.Ladder2) return 1 / 16f;
            return 1;
        }



        public static bool FullBright(BlockID b) {
			return b == Block.Lava || b == Block.StillLava
				|| b == Block.Magma || b == Block.Fire;
		}
		
		public static float FogDensity(BlockID b) {
			if (b == Block.Water || b == Block.StillWater)
				return 0.1f;
			if (b == Block.Lava || b == Block.StillLava)
				return 2f;
			return 0;
		}
		
		public static FastColour FogColour(BlockID b) {
			if (b == Block.Water || b == Block.StillWater)
				return new FastColour(5, 5, 51);
			if (b == Block.Lava || b == Block.StillLava)
				return new FastColour(153, 25, 0);
			return default(FastColour);
		}
		
		public static CollideType Collide(BlockID b) {
			if (b >= Block.Water && b <= Block.StillLava)
				return CollideType.SwimThrough;
			if (b == Block.Snow || b == Block.Air || Draw(b) == DrawType.Sprite || b == Block.Ladder1 || b == Block.Ladder2 || b == Block.Ladder3 || b == Block.Ladder4)
				return CollideType.WalkThrough;
			return CollideType.Solid;
		}
		
		public static bool BlocksLight(BlockID b) {
			return !(b == Block.Glass || b == Block.Leaves 
			         || b == Block.Air || Draw(b) == DrawType.Sprite || b == Block.Ladder1 || b == Block.Ladder2 || b == Block.Ladder3 || b == Block.Ladder4 || b == Block.RedstoneGOff || b == Block.RedstoneGOff1 || b == Block.RedstoneGOff2 || b == Block.RedstoneGOff3 || b == Block.RedstoneGOff4);
		}

		public static SoundType StepSound(BlockID b) {
			if (b == Block.Glass) return SoundType.Stone;
			if (b == Block.Rope) return SoundType.Cloth;			
			if (Draw(b) == DrawType.Sprite) return SoundType.None;
			return DigSound(b);
		}
		
		
		public static byte Draw(BlockID b) {
			if (b == Block.Air || b == Block.Invalid) return DrawType.Gas;
			if (b == Block.Leaves) return DrawType.TransparentThick;

			if (b == Block.Ice || b == Block.Water || b == Block.StillWater) 
				return DrawType.Translucent;
			if (b == Block.Glass || b == Block.Leaves || b == Block.TrapDoor || b == Block.TrapDoorOpen || b == Block.DoorTop || b == Block.DoorTop1 || b == Block.DoorTop2 || b == Block.DoorTop3 || b == Block.RedstoneGOff || b == Block.RedstoneGOff1 || b == Block.RedstoneGOff2 || b == Block.RedstoneGOff3 || b == Block.RedstoneGOff4)
				return DrawType.Transparent;
			
			if (b >= Block.Dandelion && b <= Block.RedMushroom)
				return DrawType.Sprite;
            if (b == Block.Sapling || b == Block.Rope || b == Block.Fire)
				return DrawType.Sprite;
            if (Mode.survivalinventory.Items.Contains(b))
                return DrawType.Sprite;
			return DrawType.Opaque;
		}		

		public static SoundType DigSound(BlockID b) {
			if (b >= Block.Red && b <= Block.White) 
				return SoundType.Cloth;
			if (b >= Block.LightPink && b <= Block.Turquoise) 
				return SoundType.Cloth;
			if (b == Block.Iron || b == Block.Gold)
				return SoundType.Metal;
			
			if (b == Block.Bookshelf || b == Block.Wood 
			   || b == Block.Log || b == Block.Crate || b == Block.Fire)
				return SoundType.Wood;
			
			if (b == Block.Rope) return SoundType.Cloth;
			if (b == Block.Sand) return SoundType.Sand;
			if (b == Block.Snow) return SoundType.Snow;
			if (b == Block.Glass) return SoundType.Glass;
			if (b == Block.Dirt || b == Block.Gravel)
				return SoundType.Gravel;
			
			if (b == Block.Grass || b == Block.Sapling || b == Block.TNT
			   || b == Block.Leaves || b == Block.Sponge)
				return SoundType.Grass;
			
			if (b >= Block.Dandelion && b <= Block.RedMushroom)
				return SoundType.Grass;
			if (b >= Block.Water && b <= Block.StillLava)
				return SoundType.None;
			if (b >= Block.Stone && b <= Block.StoneBrick)
				return SoundType.Stone;
			return SoundType.None;
		}
	}
}