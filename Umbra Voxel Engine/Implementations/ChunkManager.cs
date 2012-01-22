using System;
using System.IO;
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
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Umbra.Utilities.Threading;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Implementations
{
    static public class ChunkManager
    {
        static Thread SetupThread;
        static public SetupThread Setup;

        static public void InitializeThreads()
        {
            Setup = new SetupThread();
            SetupThread = new Thread(new ThreadStart(Setup.Run));
            SetupThread.Start();
        }

        static public void AbortThreads()
        {
            SetupThread.Abort();
        }

        static string GetChunkPath()
        {
            return Constants.World.Current.Path + Constants.Content.Data.ChunkFilePath;
        }

        static string GetChunkPath(ChunkIndex index)
        {
            return Constants.World.Current.Path + Constants.Content.Data.ChunkFilePath + @"\" + index.ToString() + Constants.Content.Data.ChunkFileExtension;
        }

        static bool IsChunkOnDisk(ChunkIndex index)
        {
            return File.Exists(GetChunkPath(index));
        }

        static public Chunk ObtainChunk(ChunkIndex index)
        {
            Chunk chunk = new Chunk(index);

            if (IsChunkOnDisk(index))
            {
                Setup.AddToLoad(chunk);
            }
            else
            {
                Setup.AddToGeneration(chunk);
            }

            return chunk;
        }

        static public void LoadChunkImmediate(Chunk chunk)
        {
            if (!IsChunkOnDisk(chunk.Index))
            {
                return;
            }

            FileStream stream = File.Open(GetChunkPath(chunk.Index), FileMode.Open);

            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.World.ChunkSize; y++)
                {
                    for (int z = 0; z < Constants.World.ChunkSize; z++)
                    {
                        chunk[x, y, z] = new Block((byte)stream.ReadByte(), (byte)stream.ReadByte());
                    }
                }
            }

            stream.Close();
        }

        static public void UnloadChunk(Chunk chunk)
        {
            if (Constants.World.SaveDynamicWorld)
            {
                Setup.AddToUnload(chunk);
            }
        }


        static public void StoreChunkImmediate(Chunk chunk)
        {
            if (!Directory.Exists(GetChunkPath()))
            {
                Directory.CreateDirectory(GetChunkPath());
            }

            FileStream stream = File.Open(GetChunkPath(chunk.Index), FileMode.Create);

            for (int x = 0; x < Constants.World.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.World.ChunkSize; y++)
                {
                    for (int z = 0; z < Constants.World.ChunkSize; z++)
                    {
                        stream.Write(chunk[x, y, z].Bytes, 0, 2);
                    }
                }
            }

            stream.Close();
        }
    }
}
