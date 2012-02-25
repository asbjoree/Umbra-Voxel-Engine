using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
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
using Umbra.Utilities.Landscape;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Umbra.Utilities.Threading;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Implementations
{
	static public class ChunkManager
	{
		static Chunk[, ,] Chunks { get; set; }
		static public ChunkIndex CenterIndex { get; private set; }
		static public int DataID;

		static public Vector3d WorldCenter
		{
			get
			{
				return CenterIndex.Position + Vector3d.One * Constants.World.ChunkSize / 2;
			}
		}

		static public ChunkIndex MinLoaded
		{
			get
			{
				return new ChunkIndex(CenterIndex.X - Constants.World.WorldSize / 2, CenterIndex.Y - Constants.World.WorldSize / 2, CenterIndex.Z - Constants.World.WorldSize / 2);
			}
		}

		static public ChunkIndex MaxLoaded
		{
			get
			{
				return new ChunkIndex(CenterIndex.X + Constants.World.WorldSize / 2, CenterIndex.Y + Constants.World.WorldSize / 2, CenterIndex.Z + Constants.World.WorldSize / 2);
			}
		}

		static Thread SetupThread;
		static public SetupThread Setup;

		static public void Initialize()
		{
			InitializeThreads();

			Chunks = new Chunk[Constants.World.WorldSize, Constants.World.WorldSize, Constants.World.WorldSize];
			CenterIndex = new ChunkIndex(Constants.Engines.Physics.Player.Position);

			RenderHelp.CreateBlockData3DArray(out DataID);

			for (int x = -Constants.World.WorldSize / 2; x <= Constants.World.WorldSize / 2; x++)
			{
				for (int y = -Constants.World.WorldSize / 2; y <= Constants.World.WorldSize / 2; y++)
				{
					for (int z = -Constants.World.WorldSize / 2; z <= Constants.World.WorldSize / 2; z++)
					{
						Chunk chunk = ObtainChunk(CenterIndex + new ChunkIndex(x, y, z), true);
						ChunkIndex? index = GetArrayIndex(CenterIndex + new ChunkIndex(x, y, z));
						Chunks[index.Value.X, index.Value.Y, index.Value.Z] = chunk;
						TerrainGenerator.SetChunkTerrain(chunk);
						ChangeChunk(chunk);
					}
				}
			}
			//SetupThread.Start();
		}

		static public void InitializeThreads()
		{
			Setup = new SetupThread();
			SetupThread = new Thread(new ThreadStart(Setup.Run));
		}

		static public void AbortThreads()
		{
			SetupThread.Abort();
		}

		static public void UpdateCenter(ChunkIndex centerIndex)
		{
			for (int x = -Constants.World.WorldSize / 2; x <= Constants.World.WorldSize / 2; x++)
			{
				for (int y = -Constants.World.WorldSize / 2; y <= Constants.World.WorldSize / 2; y++)
				{
					for (int z = -Constants.World.WorldSize / 2; z <= Constants.World.WorldSize / 2; z++)
					{
						Chunk chunk = ObtainChunk(centerIndex + new ChunkIndex(x, y, z), true);
						ChunkIndex? index = GetArrayIndex(centerIndex + new ChunkIndex(x, y, z), centerIndex);
						Chunks[index.Value.X, index.Value.Y, index.Value.Z] = chunk;
						ChangeChunk(chunk);
					}
				}
			}

			CenterIndex = centerIndex;
		}

		static private ChunkIndex? GetArrayIndex(ChunkIndex index)
		{
			return GetArrayIndex(index, CenterIndex);
		}

		static private ChunkIndex? GetArrayIndex(ChunkIndex index, ChunkIndex center)
		{
			if (index.X >= center.X - Constants.World.WorldSize / 2 && index.X <= center.X + Constants.World.WorldSize / 2 &&
				index.Y >= center.Y - Constants.World.WorldSize / 2 && index.Y <= center.Y + Constants.World.WorldSize / 2 &&
				index.Z >= center.Z - Constants.World.WorldSize / 2 && index.Z <= center.Z + Constants.World.WorldSize / 2)
			{
				return new ChunkIndex(EuclideanModulo(index.X, Constants.World.WorldSize), EuclideanModulo(index.Y, Constants.World.WorldSize), EuclideanModulo(index.Z, Constants.World.WorldSize));
			}

			return null;
		}

		static private int EuclideanModulo(int dividend, int divisor)
		{
			int mod = dividend % divisor;
			return mod + (mod < 0 ? divisor : 0);
		}


		static public Chunk GetChunk(ChunkIndex index)
		{
			ChunkIndex? arrayIndex = GetArrayIndex(index);

			if (!arrayIndex.HasValue)
			{
				return null;
			}

			return Chunks[arrayIndex.Value.X, arrayIndex.Value.Y, arrayIndex.Value.Z];
		}

		static public Block GetBlock(BlockIndex index)
		{
			Chunk chunk = GetChunk(new ChunkIndex(index));


			if (chunk == null)
			{
				return Block.Vacuum;
			}

			return chunk[EuclideanModulo(index.X, Constants.World.ChunkSize), EuclideanModulo(index.Y, Constants.World.ChunkSize), EuclideanModulo(index.Z, Constants.World.ChunkSize)];
		}

		static public void SetBlock(BlockIndex index, Block block)
		{
			Chunk chunk = GetChunk(new ChunkIndex(index));

			if (chunk == null)
			{
				throw new Exception("You stupid asshole!");
			}

			chunk[EuclideanModulo(index.X, Constants.World.ChunkSize), EuclideanModulo(index.Y, Constants.World.ChunkSize), EuclideanModulo(index.Z, Constants.World.ChunkSize)] = block;

			ChunkIndex? actualIndex = GetArrayIndex(chunk.Index);

			if (actualIndex.HasValue)
			{
				GL.BindTexture(TextureTarget.Texture3D, DataID);
				GL.TexSubImage3D(TextureTarget.Texture3D, 0,
					actualIndex.Value.X * Constants.World.ChunkSize + EuclideanModulo(index.X, Constants.World.ChunkSize),
					actualIndex.Value.Y * Constants.World.ChunkSize + EuclideanModulo(index.Y, Constants.World.ChunkSize),
					actualIndex.Value.Z * Constants.World.ChunkSize + EuclideanModulo(index.Z, Constants.World.ChunkSize),
					1, 1, 1,
					OpenTK.Graphics.OpenGL.PixelFormat.Luminance, PixelType.UnsignedByte, new byte[,,] { { { block.ByteValue } } });
			}
		}

		static Chunk ObtainChunk(ChunkIndex index, bool setup = false)
		{
			ChunkIndex? actualIndex = GetArrayIndex(index);
			Chunk chunk;

			if (actualIndex.HasValue && !setup)
			{
				chunk = Chunks[actualIndex.Value.X, actualIndex.Value.Y, actualIndex.Value.Z];
			}
			else
			{
				chunk = new Chunk(index);

				if (IsChunkOnDisk(index))
				{
					Setup.AddToLoad(chunk);
				}
				else
				{
					TerrainGenerator.SetChunkTerrain(chunk);
					Console.Write("Chunk terrain generated!");
				}
			}

			return chunk;
		}

		static bool IsChunkOnDisk(ChunkIndex index)
		{
			return File.Exists(GetChunkFilename(index));
		}

		static string GetChunkFilename(ChunkIndex index)
		{
			return Constants.Content.Data.ChunkFilePath + index.ToString() + Constants.Content.Data.ChunkFileExtension;
		}

		static public Chunk LoadChunkImmediate(Chunk chunk)
		{
			FileStream chunkStream = File.OpenRead(GetChunkFilename(chunk.Index));

			for (int x = 0; x < Constants.World.ChunkSize; x++)
			{
				for (int y = 0; y < Constants.World.ChunkSize; y++)
				{
					for (int z = 0; z < Constants.World.ChunkSize; z++)
					{
						chunk[x, y, z] = new Block((byte)chunkStream.ReadByte());
					}
				}
			}

			return chunk;
		}

		static public Chunk LoadChunk(ChunkIndex index)
		{
			Chunk returnChunk = new Chunk(index);

			return returnChunk;
		}

		static public void SaveChunk(Chunk chunk)
		{
			FileStream chunkStream = File.OpenWrite(GetChunkFilename(chunk.Index));

			for (int x = 0; x < Constants.World.ChunkSize; x++)
			{
				for (int y = 0; y < Constants.World.ChunkSize; y++)
				{
					for (int z = 0; z < Constants.World.ChunkSize; z++)
					{
						chunkStream.WriteByte(chunk[x, y, z].ByteValue);
					}
				}
			}
		}

		static public void ChangeChunk(Chunk chunk)
		{
			ChunkIndex? arrayIndex = GetArrayIndex(chunk.Index);


			if (arrayIndex.HasValue)
			{
				GL.BindTexture(TextureTarget.Texture3D, DataID);

				GL.TexSubImage3D(TextureTarget.Texture3D, 0,
					arrayIndex.Value.X * Constants.World.ChunkSize, arrayIndex.Value.Y * Constants.World.ChunkSize, arrayIndex.Value.Z * Constants.World.ChunkSize,
					Constants.World.ChunkSize, Constants.World.ChunkSize, Constants.World.ChunkSize,
					OpenTK.Graphics.OpenGL.PixelFormat.Luminance, PixelType.UnsignedByte, chunk.GetRawData());

				GL.BindTexture(TextureTarget.Texture3D, 0);
			}
		}
	}
}
