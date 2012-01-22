using System;
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
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures
{
    public class Chunk
    {
        public ChunkIndex Index { get; private set; }

        Block[, ,] Data;

        public Block this[BlockIndex index]
        {
            get
            {
                return this[index.X, index.Y, index.Z];
            }
            set
            {
                this[index.X, index.Y, index.Z] = value;
            }
        }

        public Block this[int x, int y, int z]
        {
            get
            {
                if (x >= 0 && x < Constants.World.ChunkSize && y >= 0 && y < Constants.World.ChunkSize && z >= 0 && z < Constants.World.ChunkSize)
                {
                    return Data[x, y, z];
                }
                else
                {
                    throw new Exception("Index was out of bounds!");
                }
            }
            set
            {
                if (x >= 0 && x < Constants.World.ChunkSize && y >= 0 && y < Constants.World.ChunkSize && z >= 0 && z < Constants.World.ChunkSize)
                {
                    Data[x, y, z] = value;
                }
                else
                {
                    throw new Exception("Index was out of bounds!");
                }
            }
        }

        public Chunk(ChunkIndex index)
        {
            Index = index;
            Data = new Block[Constants.World.ChunkSize, Constants.World.ChunkSize, Constants.World.ChunkSize];
        }

        public void SetBlock(BlockIndex index, Block value, bool updateAdjacentChunks)
        {
            if (index.X >= 0 && index.X < Constants.World.ChunkSize && index.Y >= 0 && index.Y < Constants.World.ChunkSize && index.Z >= 0 && index.Z < Constants.World.ChunkSize)
            {
                Data[index.X, index.Y, index.Z] = value;

                if (updateAdjacentChunks)
                {
                    if (index.X == 0)
                    {
                        Chunk chunk = Constants.World.Current.GetChunk(Index - ChunkIndex.UnitX);
                    }
                    else if (index.X == Constants.World.ChunkSize - 1)
                    {
                        Chunk chunk = Constants.World.Current.GetChunk(Index + ChunkIndex.UnitX);
                    }

                    if (index.Y == 0)
                    {
                        Chunk chunk = Constants.World.Current.GetChunk(Index - ChunkIndex.UnitY);
                    }
                    else if (index.Y == Constants.World.ChunkSize - 1)
                    {
                        Chunk chunk = Constants.World.Current.GetChunk(Index + ChunkIndex.UnitY);
                    }

                    if (index.Z == 0)
                    {
                        Chunk chunk = Constants.World.Current.GetChunk(Index - ChunkIndex.UnitZ);
                    }
                    else if (index.Z == Constants.World.ChunkSize - 1)
                    {
                        Chunk chunk = Constants.World.Current.GetChunk(Index + ChunkIndex.UnitZ);
                    }
                }
            }
        }
    }
}
