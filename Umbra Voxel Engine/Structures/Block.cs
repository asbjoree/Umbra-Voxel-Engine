using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures
{
	public struct Block
	{
		private BlockType InternalType { get; set; }
		public byte Type
		{
			get
			{
				return (byte)InternalType;
			}
		}
		public byte Data { get; set; }

		public Block(byte type, byte data)
			: this()
		{
			InternalType = (BlockType)type;
			Data = data;
		}

		private Block(BlockType type, byte data)
			: this()
		{
			InternalType = type;
			Data = data;
		}

		static public Block Air { get { return new Block(BlockType.Air, 0); } }
		static public Block Grass { get { return new Block(BlockType.Grass, 0); } }
		static public Block Stone { get { return new Block(BlockType.Stone, 0); } }
		static public Block Dirt { get { return new Block(BlockType.Dirt, 0); } }
		static public Block Water { get { return new Block(BlockType.Water, 0); } }
		static public Block Sand { get { return new Block(BlockType.Sand, 0); } }
		static public Block Leaves { get { return new Block(BlockType.Leaves, 0); } }
		static public Block Lava { get { return new Block(BlockType.Lava, 0); } }
		static public Block Log { get { return new Block(BlockType.Log, 0); } }
		static public Block Vacuum { get { return new Block(BlockType.Vacuum, 0); } }

		public byte[] Bytes { get { return new byte[] { (byte)InternalType, Data }; } }

		public byte GetFace(Direction direction)
		{
			switch (InternalType)
			{
				case BlockType.Grass: return new byte[] { 1, 1, 0, 3, 1, 1 }[direction.Value];
				case BlockType.Stone: return 2;
				case BlockType.Dirt: return 3;
				case BlockType.Water: return 4;
				case BlockType.Sand: return 5;
				case BlockType.Leaves: return 6;
				case BlockType.Lava: return 7;
				case BlockType.Log: return new byte[] { 8, 8, 9, 9, 8, 8 }[direction.Value];
				default: return 0;
			}
		}

		static public byte GetFaceShade(BlockIndex index, Direction direction)
		{
			if (Constants.Graphics.AmbientOcclusionEnabled)
			{
				double maxDist = 6;
				double dist = 0;
				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);

				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() + direction.GetPerpendicularRight(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);
				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() - direction.GetPerpendicularRight(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);
				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() + direction.GetPerpendicularLeft(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);
				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() - direction.GetPerpendicularLeft(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);

				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() + direction.GetPerpendicularRight() + direction.GetPerpendicularLeft(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);
				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() + direction.GetPerpendicularRight() - direction.GetPerpendicularLeft(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);
				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() - direction.GetPerpendicularRight() + direction.GetPerpendicularLeft(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);
				dist += Lighting.GetRayIntersectDistance(maxDist, direction.GetVector3() - direction.GetPerpendicularRight() - direction.GetPerpendicularLeft(), (index.Position + new Vector3d(0.5, 0.5, 0.5)) + direction.GetVector3() * 0.6);

				dist /= 9;

				byte baseShade = (byte)Math.Ceiling(Math.Sqrt(Math.Pow(maxDist, 2)*3));
				baseShade -= (byte)(dist / 21 * 10);

				return baseShade;
			}
			else
			{
				return 0;
			}
		}

		public bool Translucency
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return true;
					case BlockType.Water: return true;
					case BlockType.Leaves: return true;
					case BlockType.Vacuum: return false;
					default: return false;
				}
			}
		}

		public bool Solidity
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return false;
					case BlockType.Water: return false;
					case BlockType.Lava: return false;
					default: return true;
				}
			}
		}

		public BlockVisibility Visibility
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return BlockVisibility.Invisible;
					case BlockType.Water: return BlockVisibility.Translucent;
					case BlockType.Leaves: return BlockVisibility.Translucent;
					case BlockType.Vacuum: return BlockVisibility.Invisible;
					default: return BlockVisibility.Opaque;
				}
			}
		}


		public float Viscosity
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return 1.2F;
					case BlockType.Water: return 1000.0F;
					case BlockType.Lava: return 2600.0F;
					case BlockType.Vacuum: return 0.0F;
					default: return 0.0F;
				}
			}
		}

		public float Density
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return 1.225F;
					case BlockType.Grass: return 1920.0F;
					case BlockType.Stone: return 2700.0F;
					case BlockType.Dirt: return 1922.0F;
					case BlockType.Water: return 1000.0F;
					case BlockType.Sand: return 1602.0F;
					case BlockType.Leaves: return 8.5F;
					case BlockType.Lava: return 2600.0F;
					case BlockType.Log: return 700.0F;
					case BlockType.Vacuum: return 0.0F;
					default: return 0.0F;
				}
			}
		}

		public float KineticFrictionCoefficient
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return 0.0F;
					case BlockType.Grass: return 1.0F;
					case BlockType.Stone: return 1.0F;
					case BlockType.Dirt: return 1.0F;
					case BlockType.Water: return 0.5F;
					case BlockType.Sand: return 1.3F;
					case BlockType.Leaves: return 1.2F;
					case BlockType.Lava: return 0.0F;
					case BlockType.Log: return 1.0F;
					case BlockType.Vacuum: return 0.0F;
					default: return 0.0F;
				}
			}
		}

		public float GripCoefficient
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return 0.01F;
					case BlockType.Grass: return 1.0F;
					case BlockType.Stone: return 1.0F;
					case BlockType.Dirt: return 1.0F;
					case BlockType.Water: return 0.2F;
					case BlockType.Sand: return 0.7F;
					case BlockType.Leaves: return 0.8F;
					case BlockType.Lava: return 0.1F;
					case BlockType.Log: return 1.0F;
					case BlockType.Vacuum: return 0.0F;
					default: return 0.0F;
				}
			}
		}

		public string Name
		{
			get
			{
				switch (InternalType)
				{
					case BlockType.Air: return "air";
					case BlockType.Grass: return "grass";
					case BlockType.Stone: return "stone";
					case BlockType.Dirt: return "dirt";
					case BlockType.Water: return "water";
					case BlockType.Sand: return "sand";
					case BlockType.Leaves: return "leaves";
					case BlockType.Lava: return "lava";
					case BlockType.Log: return "log";
					case BlockType.Vacuum: return "vacuum";
					default: return "unused";
				}
			}
		}

		static public Block GetFromName(string name)
		{
			switch (name)
			{
				case "air": return Air;
				case "grass": return Grass;
				case "stone": return Stone;
				case "dirt": return Dirt;
				case "water": return Water;
				case "sand": return Sand;
				case "leaves": return Leaves;
				case "lava": return Lava;
				case "log": return Log;
				case "vacuum": return Vacuum;
				default: return Vacuum;
			}
		}

		public static bool operator ==(Block part1, Block part2)
		{
			return (part1.InternalType == part2.InternalType);
		}

		public static bool operator !=(Block part1, Block part2)
		{
			return !(part1.InternalType == part2.InternalType);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object o)
		{
			return o == (object)this;
		}

		static public string[] GetBlockTexturePaths()
		{
			string Path = Constants.Content.Textures.Packs.CurrentPackPath;

			string[] returnValue = new string[256];

			for (int i = 0; i < returnValue.Length; i++)
			{
				returnValue[i] = Path + "grass1.png";
			}

			returnValue[Grass.GetFace(Direction.Up)] = Path + "grass1.png";
			returnValue[Grass.GetFace(Direction.Right)] = Path + "grass2.png";
			returnValue[Stone.GetFace(Direction.Up)] = Path + "stone.png";
			returnValue[Dirt.GetFace(Direction.Up)] = Path + "dirt.png";
			returnValue[Sand.GetFace(Direction.Up)] = Path + "sand.png";

			returnValue[Water.GetFace(Direction.Up)] = Path + "water.png";
			returnValue[Leaves.GetFace(Direction.Up)] = Path + "leaves.png";
			returnValue[Lava.GetFace(Direction.Up)] = Path + "lava.png";

			returnValue[Log.GetFace(Direction.Right)] = Path + "log1.png";
			returnValue[Log.GetFace(Direction.Up)] = Path + "log2.png";

			return returnValue;
		}

		private enum BlockType : byte
		{
			Air,
			Grass,
			Stone,
			Dirt,
			Water,
			Sand,
			Leaves,
			Lava,
			Log,
			Vacuum = 255
		}
	}
}
