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

namespace Umbra.Definitions
{
	public struct ChunkIndex
	{
		public int X;
		public int Y;
		public int Z;

		public Vector3d Position
		{
			get { return new Vector3d(X * Constants.World.ChunkSize, Y * Constants.World.ChunkSize, Z * Constants.World.ChunkSize); }
		}

		public ChunkIndex(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public ChunkIndex(Vector2 position)
		{
			X = (int)Math.Floor((double)position.X / (double)Constants.World.ChunkSize);
			Y = 0;
			Z = (int)Math.Floor((double)position.Y / (double)Constants.World.ChunkSize);
		}

		public ChunkIndex(Vector3 position)
		{
			X = (int)Math.Floor((double)position.X / (double)Constants.World.ChunkSize);
			Y = (int)Math.Floor((double)position.Y / (double)Constants.World.ChunkSize);
			Z = (int)Math.Floor((double)position.Z / (double)Constants.World.ChunkSize);
		}

		public ChunkIndex(Vector3d position)
		{
			X = (int)Math.Floor(position.X / (double)Constants.World.ChunkSize);
			Y = (int)Math.Floor(position.Y / (double)Constants.World.ChunkSize);
			Z = (int)Math.Floor(position.Z / (double)Constants.World.ChunkSize);
		}

		public ChunkIndex(BlockIndex index)
		{
			X = (int)Math.Floor((float)index.X / 32);
			Y = (int)Math.Floor((float)index.Y / 32);
			Z = (int)Math.Floor((float)index.Z / 32);
		}

		public BlockIndex ToBlockIndex()
		{
			return new BlockIndex(X * Constants.World.ChunkSize, Y * Constants.World.ChunkSize, Z * Constants.World.ChunkSize);
		}

		public float DistanceFromOrigo()
		{
			return (float)Math.Sqrt(Math.Pow(X * Constants.World.ChunkSize, 2) + Math.Pow(Y * Constants.World.ChunkSize, 2) + Math.Pow(Z * Constants.World.ChunkSize, 2));
		}

		public static ChunkIndex Zero { get { return new ChunkIndex(0, 0, 0); } }
		public static ChunkIndex UnitX { get { return new ChunkIndex(1, 0, 0); } }
		public static ChunkIndex UnitY { get { return new ChunkIndex(0, 1, 0); } }
		public static ChunkIndex UnitZ { get { return new ChunkIndex(0, 0, 1); } }
		public static ChunkIndex One { get { return new ChunkIndex(1, 1, 1); } }

		public static ChunkIndex operator +(ChunkIndex part1, ChunkIndex part2)
		{
			return new ChunkIndex(part1.X + part2.X, part1.Y + part2.Y, part1.Z + part2.Z);
		}

		public static ChunkIndex operator -(ChunkIndex part1, ChunkIndex part2)
		{
			return new ChunkIndex(part1.X - part2.X, part1.Y - part2.Y, part1.Z - part2.Z);
		}

		public static ChunkIndex operator *(ChunkIndex part1, int part2)
		{
			return new ChunkIndex(part1.X * part2, part1.Y * part2, part1.Z * part2);
		}

		public static ChunkIndex operator *(ChunkIndex part1, float part2)
		{
			return new ChunkIndex((int)((float)part1.X * part2), (int)((float)part1.Y * part2), (int)((float)part1.Z * part2));
		}

		public static ChunkIndex operator *(ChunkIndex part1, double part2)
		{
			return new ChunkIndex((int)((double)part1.X * part2), (int)((double)part1.Y * part2), (int)((double)part1.Z * part2));
		}

		public static ChunkIndex operator *(ChunkIndex part1, ChunkIndex part2)
		{
			return new ChunkIndex(part1.X * part2.X, part1.Y * part2.Y, part1.Z * part2.Z);
		}

		public static ChunkIndex operator /(ChunkIndex part1, float part2)
		{
			return new ChunkIndex((int)((float)part1.X / part2), (int)((float)part1.Y / part2), (int)((float)part1.Z / part2));
		}
		public static ChunkIndex operator %(ChunkIndex part1, int part2)
		{
			return new ChunkIndex(part1.X % part2, part1.Y % part2, part1.Z % part2);
		}

		public static bool operator ==(ChunkIndex part1, ChunkIndex part2)
		{
			return (part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
		}

		public static bool operator !=(ChunkIndex part1, ChunkIndex part2)
		{
			return !(part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return X + "_" + Y + "_" + Z;
		}
	}

	public class BlockIndex
	{
		public int X;
		public int Y;
		public int Z;

		public Vector3d Position
		{
			get { return new Vector3d(X, Y, Z); }
		}

		public static BlockIndex Zero { get { return new BlockIndex(0, 0, 0); } }
		public static BlockIndex UnitX { get { return new BlockIndex(1, 0, 0); } }
		public static BlockIndex UnitY { get { return new BlockIndex(0, 1, 0); } }
		public static BlockIndex UnitZ { get { return new BlockIndex(0, 0, 1); } }
		public static BlockIndex One { get { return new BlockIndex(1, 1, 1); } }

		public BlockIndex(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public BlockIndex(Vector3 position)
		{
			X = (int)Math.Floor(position.X);
			Y = (int)Math.Floor(position.Y);
			Z = (int)Math.Floor(position.Z);
		}

		public BlockIndex(Vector3d position)
		{
			X = (int)Math.Floor(position.X);
			Y = (int)Math.Floor(position.Y);
			Z = (int)Math.Floor(position.Z);
		}

		public ChunkIndex ToChunkIndex()
		{
			return new ChunkIndex((int)Math.Floor((float)X / Constants.World.ChunkSize), (int)Math.Floor((float)Y / Constants.World.ChunkSize), (int)Math.Floor((float)Z / Constants.World.ChunkSize));
		}

		public BoundingBox GetBoundingBox()
		{
			return new BoundingBox(this.Position, (this + BlockIndex.One).Position);
		}

		public static BlockIndex operator +(BlockIndex part1, BlockIndex part2)
		{
			return new BlockIndex(part1.X + part2.X, part1.Y + part2.Y, part1.Z + part2.Z);
		}

		public static BlockIndex operator +(BlockIndex part1, ChunkIndex part2)
		{
			return new BlockIndex(part1.X + part2.X * Constants.World.ChunkSize, part1.Y + part2.Y * Constants.World.ChunkSize, part1.Z + part2.Z * Constants.World.ChunkSize);
		}

		public static BlockIndex operator -(BlockIndex part1, BlockIndex part2)
		{
			return new BlockIndex(part1.X - part2.X, part1.Y - part2.Y, part1.Z - part2.Z);
		}

		public static BlockIndex operator -(BlockIndex part1, ChunkIndex part2)
		{
			return new BlockIndex(part1.X - part2.X * Constants.World.ChunkSize, part1.Y - part2.Y * Constants.World.ChunkSize, part1.Z - part2.Z * Constants.World.ChunkSize);
		}

		public static BlockIndex operator *(BlockIndex part1, int part2)
		{
			return new BlockIndex(part1.X * part2, part1.Y * part2, part1.Z * part2);
		}

		public static BlockIndex operator *(BlockIndex part1, ChunkIndex part2)
		{
			return new BlockIndex(part1.X * part2.X * Constants.World.ChunkSize, part1.Y * part2.Y * Constants.World.ChunkSize, part1.Z * part2.Z * Constants.World.ChunkSize);
		}

		public static BlockIndex operator /(BlockIndex part1, int part2)
		{
			return new BlockIndex(part1.X / part2, part1.Y / part2, part1.Z / part2);
		}

		public static BlockIndex operator %(BlockIndex part1, int part2)
		{
			return new BlockIndex(part1.X % part2, part1.Y % part2, part1.Z % part2);
		}

		public static BlockIndex operator -(BlockIndex part)
		{
			return BlockIndex.Zero - part;
		}

		public static bool operator ==(BlockIndex part1, BlockIndex part2)
		{
			if (object.Equals(part1, null) != object.Equals(part2, null))
			{
				return false;
			}
			else if (object.Equals(part1, null) || object.Equals(part2, null))
			{
				return true;
			}
			return (part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
		}

		public static bool operator !=(BlockIndex part1, BlockIndex part2)
		{
			if (object.Equals(part1, null) != object.Equals(part2, null))
			{
				return true;
			}
			else if (object.Equals(part1, null) || object.Equals(part2, null))
			{
				return false;
			}
			return !(part1.X == part2.X && part1.Y == part2.Y && part1.Z == part2.Z);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
