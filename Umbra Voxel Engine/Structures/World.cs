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
    public class World
    {
        Chunk[, ,] LoadedChunks;

        public ChunkIndex Offset { get; private set; }

        public string Path
        {
            get
            {
                return Constants.Content.Data.WorldPath + Name + @"/";
            }
        }
        public string Name { get; private set; }

        public World(string name)
        {
            Name = name;

            LoadedChunks = new Chunk[Constants.World.WorldSize, Constants.World.WorldSize, Constants.World.WorldSize];
            Offset = ChunkIndex.One * (-Constants.World.WorldSize / 2) + ChunkIndex.UnitY * (Constants.Engines.Physics.Player.Position.Y / Constants.World.ChunkSize);
        }

        public void Initialize()
        {
            for (int x = 0; x < Constants.World.WorldSize + 3; x++)
            {
                for (int y = 0; y < Constants.World.WorldSize + 3; y++)
                {
                    if (x < Constants.World.WorldSize && y < Constants.World.WorldSize)
                    {
                        for (int z = 0; z < Constants.World.WorldSize; z++)
                        {
                            LoadedChunks[x, y, z] = ChunkManager.ObtainChunk(Offset + new ChunkIndex(x, y, z));
                        }
                    }
                }
            }
        }

        public void OffsetChunks(ChunkIndex offset)
        {
            if (offset == ChunkIndex.Zero)
            {
                return;
            }

            Offset += offset;

            Chunk[, ,] newArray = new Chunk[Constants.World.WorldSize, Constants.World.WorldSize, Constants.World.WorldSize];

            for (int x = 0; x < Constants.World.WorldSize + 3; x++)
            {
                for (int y = 0; y < Constants.World.WorldSize + 3; y++)
                {
                    if (x < Constants.World.WorldSize && y < Constants.World.WorldSize)
                    {
                        for (int z = 0; z < Constants.World.WorldSize; z++)
                        {
                            int newX = x + offset.X;
                            int newY = y + offset.Y;
                            int newZ = z + offset.Z;

                            if (newX < 0 || newX >= Constants.World.WorldSize || newY < 0 || newY >= Constants.World.WorldSize || newZ < 0 || newZ >= Constants.World.WorldSize)
                            {
                                ChunkManager.UnloadChunk(LoadedChunks[Constants.World.WorldSize - x - 1, Constants.World.WorldSize - y - 1, Constants.World.WorldSize - z - 1]);
                                newArray[x, y, z] = ChunkManager.ObtainChunk(Offset + new ChunkIndex(x, y, z));
                            }
                            else
                            {
                                newArray[x, y, z] = LoadedChunks[newX, newY, newZ];
                            }
                        }
                    }
                }
            }

            LoadedChunks = newArray;
        }

        public Chunk GetChunk(ChunkIndex index)
        {
            ChunkIndex indexRelative = index - Offset;

            if (indexRelative.X < 0 || indexRelative.X >= Constants.World.WorldSize || indexRelative.Y < 0 || indexRelative.Y >= Constants.World.WorldSize || indexRelative.Z < 0 || indexRelative.Z >= Constants.World.WorldSize)
            {
                return null;
            }

            Chunk returnChunk = LoadedChunks[indexRelative.X, indexRelative.Y, indexRelative.Z];

            if (returnChunk.Index != index)
            {
                return null;
            }

            return returnChunk;
        }

        public Block GetBlock(BlockIndex index)
        {
            Chunk chunk = GetChunk(new ChunkIndex(index.Position));

            if (chunk != null)
            {
                return chunk[index - chunk.Index];
            }
            else
            {
                return Block.Vacuum;
            }
        }

        public void SetBlock(BlockIndex index, Block type)
        {
            SetBlock(index, type, true);
        }

        public void SetBlock(BlockIndex index, Block type, bool rebuildOctree)
        {
            Chunk chunk = GetChunk(new ChunkIndex(index.Position));

            if (chunk != null)
            {
                if (rebuildOctree)
                {
                    chunk.SetBlock(index - chunk.Index, type, true);
                }
                else
                {
                    chunk[index - chunk.Index] = type;
                }
            }
        }

        public Chunk[, ,] GetChunks()
        {
            return LoadedChunks;
        }
    }
}
