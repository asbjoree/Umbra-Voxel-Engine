using System;
using System.Linq;
using System.Text;
using System.Drawing;
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

namespace Umbra.Implementations
{
    static public class Lighting
    {
        static public double GetRayIntersectDistance(double maxValue, Vector3d direction, Vector3d startPosition)
        {
            Ray ray = new Ray(startPosition, direction);
            double distance = 0.0;
            BlockIndex index;
            double? intersect;

			while (distance <= maxValue)
            {
                index = new BlockIndex(direction * distance + startPosition);

                intersect = index.GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index).Solidity)
                {
                    return intersect.Value;
                }

                intersect = (index + BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
				if (intersect.HasValue && ChunkManager.GetBlock(index + BlockIndex.UnitX).Solidity)
                {
                    return intersect.Value;
                }
                intersect = (index - BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
				if (intersect.HasValue && ChunkManager.GetBlock(index - BlockIndex.UnitX).Solidity)
                {
                    return intersect.Value;
                }
                intersect = (index + BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
				if (intersect.HasValue && ChunkManager.GetBlock(index + BlockIndex.UnitY).Solidity)
                {
                    return intersect.Value;
                }
                intersect = (index - BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
				if (intersect.HasValue && ChunkManager.GetBlock(index - BlockIndex.UnitY).Solidity)
                {
                    return intersect.Value;
                }
                intersect = (index + BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
				if (intersect.HasValue && ChunkManager.GetBlock(index + BlockIndex.UnitZ).Solidity)
                {
                    return intersect.Value;
                }
                intersect = (index - BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
				if (intersect.HasValue && ChunkManager.GetBlock(index - BlockIndex.UnitZ).Solidity)
                {
                    return intersect.Value;
                }

                distance += 0.5F;
            }

            return maxValue;
        }
    }
}
