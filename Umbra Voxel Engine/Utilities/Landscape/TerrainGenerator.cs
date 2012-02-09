using System;
using System.Linq;
using System.Text;
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
using Umbra.Utilities.Landscape.Utilities;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Utilities.Landscape
{
	static public class TerrainGenerator
	{
		static private int Seed;

		static public void Initialize(string seed)
		{
			Seed = seed.GetHashCode();

			if (seed == "")
			{
				Seed = (int)System.Diagnostics.Stopwatch.GetTimestamp();
			}

			Vegetation.Initialize(Seed);
		}

		static public int GetLandscapeHeight(int x, int y)
		{
			ChunkIndex i = new BlockIndex(x, 0, y).ToChunkIndex();

			return GetHeightMap(i)[x - i.ToBlockIndex().X, y - i.ToBlockIndex().Z];
		}

		static private int[,] GetHeightMap(ChunkIndex index)
		{
			int squareSize = (int)Math.Pow(2, Constants.Landscape.TerrainStretch);

			int x = (int)index.Position.X;
			int y = (int)index.Position.Z;



			int xInSquare = Mathematics.AbsModulo(x, squareSize);
			int yInSquare = Mathematics.AbsModulo(y, squareSize);

			float[,] dataPoints = new float[4, 4];

			dataPoints[0, 0] = NoiseMaps.GetPerlin(x - xInSquare - squareSize, y - yInSquare - squareSize, Seed);
			dataPoints[0, 1] = NoiseMaps.GetPerlin(x - xInSquare - squareSize, y - yInSquare, Seed);
			dataPoints[0, 2] = NoiseMaps.GetPerlin(x - xInSquare - squareSize, y - yInSquare + squareSize, Seed);
			dataPoints[0, 3] = NoiseMaps.GetPerlin(x - xInSquare - squareSize, y - yInSquare + squareSize * 2, Seed);

			dataPoints[1, 0] = NoiseMaps.GetPerlin(x - xInSquare, y - yInSquare - squareSize, Seed);
			dataPoints[1, 1] = NoiseMaps.GetPerlin(x - xInSquare, y - yInSquare, Seed);
			dataPoints[1, 2] = NoiseMaps.GetPerlin(x - xInSquare, y - yInSquare + squareSize, Seed);
			dataPoints[1, 3] = NoiseMaps.GetPerlin(x - xInSquare, y + y - yInSquare + squareSize * 2, Seed);

			dataPoints[2, 0] = NoiseMaps.GetPerlin(x - xInSquare + squareSize, y - yInSquare - squareSize, Seed);
			dataPoints[2, 1] = NoiseMaps.GetPerlin(x - xInSquare + squareSize, y - yInSquare, Seed);
			dataPoints[2, 2] = NoiseMaps.GetPerlin(x - xInSquare + squareSize, y - yInSquare + squareSize, Seed);
			dataPoints[2, 3] = NoiseMaps.GetPerlin(x - xInSquare + squareSize, y - yInSquare + squareSize * 2, Seed);

			dataPoints[3, 0] = NoiseMaps.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare - squareSize, Seed);
			dataPoints[3, 1] = NoiseMaps.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare, Seed);
			dataPoints[3, 2] = NoiseMaps.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare + squareSize, Seed);
			dataPoints[3, 3] = NoiseMaps.GetPerlin(x - xInSquare + squareSize * 2, y - yInSquare + squareSize * 2, Seed);

			Interpolation.UpdateBicubicCoefficients(dataPoints);


			int[,] data = new int[Constants.World.ChunkSize, Constants.World.ChunkSize];

			int xBlockInSquare = 0;
			int yBlockInSquare = 0;

			for (int blockX = 0; blockX < Constants.World.ChunkSize; blockX++)
			{
				for (int blockY = 0; blockY < Constants.World.ChunkSize; blockY++)
				{
					xBlockInSquare = blockX + xInSquare;
					yBlockInSquare = blockY + yInSquare;

					data[blockX, blockY] = (int)(Interpolation.Linear(
						NoiseMaps.GetPerlin(x + blockX, y + blockY, Seed),
						Interpolation.Bicubic((float)xBlockInSquare / (float)squareSize, (float)yBlockInSquare / (float)squareSize),
						Constants.Landscape.PerlinBicubicWeight
						) * Constants.Landscape.WorldHeightAmplitude) + Constants.Landscape.WorldHeightOffset;
				}
			}

			return data;
		}

		static public void SetChunkTerrain(Chunk chunk)
		{
			float density;
			int terrainHeight;
			int absoluteHeight;
			int relativeHeight;

			int[,] heightMap = GetHeightMap(chunk.Index);

			for (int x = 0; x < Constants.World.ChunkSize; x++)
			{
				for (int z = 0; z < Constants.World.ChunkSize; z++)
				{
					for (int y = 0; y < Constants.World.ChunkSize; y++)
					{
						absoluteHeight = chunk.Index.Y * 32 + y;
						terrainHeight = heightMap[x, z];
						relativeHeight = absoluteHeight - terrainHeight;

						if (absoluteHeight >= terrainHeight)
						{
							if (absoluteHeight <= Constants.Landscape.WaterLevel)
							{
								chunk[x, y, z] = Block.Water;
							}
							else
							{
								chunk[x, y, z] = Block.Air;
							}
						}
						else
						{
							if (Constants.Landscape.CavesEnabled)
							{
								density = NoiseMaps.GetTrilinearlyInterpolated((int)chunk.Index.Position.X + x, (int)chunk.Index.Position.Y + y, (int)chunk.Index.Position.Z + z, 16, Seed);

								if (density < GetTresholdFromHeight(relativeHeight))
								{
									chunk[x, y, z] = Block.Air;
									continue;
								}
							}
							if (absoluteHeight < Constants.Landscape.SandLevel && relativeHeight >= -4)
							{
								chunk[x, y, z] = Block.Sand;
							}
							else if (relativeHeight >= -5)
							{
								if (relativeHeight == -1)
								{
									chunk[x, y, z] = Block.Grass;
								}
								else
								{
									chunk[x, y, z] = Block.Dirt;
								}
							}
							else
							{
								chunk[x, y, z] = Block.Stone;
							}
						}
					}
				}
			}
		}

		static public float GetTresholdFromHeight(int height)
		{
			if (height >= 0)
			{
				return 1.0F;
			}
			else if (height > -256)
			{
				return (float)Interpolation.Linear(0.2F, 0.4F, (float)height / -256.0F);
			}
			else
			{
				return 0.4F;
			}
		}
	}
}
