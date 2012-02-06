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
    static public class BlockCursor
    {
        static public BlockIndex GetToDestroy()
        {
            Vector3d outVar;
            return GetCursorIntersect(Constants.Player.BlockEditing.Reach, out outVar);
        }

        static public BlockIndex GetToCreate()
        {
            Vector3d intersection;
            BlockIndex targetBlock = GetCursorIntersect(Constants.Player.BlockEditing.Reach, out intersection);

            if (targetBlock == null)
            {
                return null;
            }

            Vector3d distanceFromCenter = intersection - (targetBlock.Position + Vector3d.One / 2.0);
            BlockIndex targetIndex = BlockIndex.UnitY;

            if (Math.Abs(distanceFromCenter.Y) > Math.Max(Math.Abs(distanceFromCenter.X), Math.Abs(distanceFromCenter.Z)))
            {
                targetIndex = BlockIndex.UnitY * Math.Sign(distanceFromCenter.Y);
            }
            else if (Math.Abs(distanceFromCenter.X) > Math.Max(Math.Abs(distanceFromCenter.Y), Math.Abs(distanceFromCenter.Z)))
            {
                targetIndex = BlockIndex.UnitX * Math.Sign(distanceFromCenter.X);
            }
            else if (Math.Abs(distanceFromCenter.Z) > Math.Max(Math.Abs(distanceFromCenter.X), Math.Abs(distanceFromCenter.Y)))
            {
                targetIndex = BlockIndex.UnitZ * Math.Sign(distanceFromCenter.Z);
            }

            return targetBlock + targetIndex;
        }

        static private BlockIndex GetCursorIntersect(double maxReach, out Vector3d intersectionPoint)
        {
            Vector3d direction = Vector3d.Transform(-Vector3d.UnitZ, Constants.Engines.Physics.Player.FirstPersonCamera.Rotation);
            Vector3d startPosition = Constants.Engines.Physics.Player.FirstPersonCamera.Position;

            return GetCursorIntersect(maxReach, direction, startPosition, out intersectionPoint);
        }

        static private BlockIndex GetCursorIntersect(double maxReach, Vector3d direction, Vector3d startPosition, out Vector3d intersectionPoint)
        {
            Ray ray = new Ray(startPosition, direction);
            double distance = 0.0;
            BlockIndex index;
            double? intersect;

            while (distance <= Constants.Player.BlockEditing.Reach)
            {
                index = new BlockIndex(direction * distance + startPosition);

                intersect = index.GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index;
                }

                intersect = (index + BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index + BlockIndex.UnitX).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitX;
                }
                intersect = (index - BlockIndex.UnitX).GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index - BlockIndex.UnitX).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitX;
                }
                intersect = (index + BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index + BlockIndex.UnitY).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitY;
                }
                intersect = (index - BlockIndex.UnitY).GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index - BlockIndex.UnitY).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitY;
                }
                intersect = (index + BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index + BlockIndex.UnitZ).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index + BlockIndex.UnitZ;
                }
                intersect = (index - BlockIndex.UnitZ).GetBoundingBox().Intersects(ray);
                if (intersect.HasValue && ChunkManager.GetBlock(index - BlockIndex.UnitZ).Solidity)
                {
                    intersectionPoint = intersect.Value * direction + startPosition;
                    return index - BlockIndex.UnitZ;
                }

                distance += 0.5F;
            }

            intersectionPoint = Vector3d.Zero;
            return null;
        }
    }
}
