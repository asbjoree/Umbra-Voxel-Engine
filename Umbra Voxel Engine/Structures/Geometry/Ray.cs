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

namespace Umbra.Structures.Geometry
{
    public class Ray
    {
        public Vector3d Origin { get; private set; }
        public Vector3d Direction { get; private set; }

        public Ray(Vector3d origin, Vector3d direction)
        {
            Origin = origin;
            if (direction == Vector3d.Zero)
            {
                throw new Exception("Direction cannot be a zero vector");
            }
            Direction = direction;
            Direction.Normalize();
        }

        public double? Intersects(BoundingBox boundingBox)
        {
            double tNear = int.MinValue;
            double tFar = int.MaxValue;

            #region - X -
            if (Direction.X == 0 && Origin.X < boundingBox.Min.X && Origin.X > boundingBox.Max.X)
            {
                // Ray is paralell and outside planes
                return null;
            }
            else
            {
                double t1 = (boundingBox.Min.X - Origin.X) / Direction.X;
                double t2 = (boundingBox.Max.X - Origin.X) / Direction.X;

                if (t1 > t2)
                {
                    MathHelper.Swap(ref t1, ref t2);
                }
                if (t1 > tNear)
                {
                    tNear = t1;
                }
                if (t2 < tFar)
                {
                    tFar = t2;
                }
                if (tNear > tFar || tFar < 0)
                {
                    return null;
                }
            }
            #endregion

            #region - Y -
            if (Direction.Y == 0 && Origin.Y < boundingBox.Min.Y && Origin.Y > boundingBox.Max.Y)
            {
                // Ray is paralell and outside planes
                return null;
            }
            else
            {
                double t1 = (boundingBox.Min.Y - Origin.Y) / Direction.Y;
                double t2 = (boundingBox.Max.Y - Origin.Y) / Direction.Y;

                if (t1 > t2)
                {
                    MathHelper.Swap(ref t1, ref t2);
                }
                if (t1 > tNear)
                {
                    tNear = t1;
                }
                if (t2 < tFar)
                {
                    tFar = t2;
                }
                if (tNear > tFar || tFar < 0)
                {
                    return null;
                }
            }
            #endregion

            #region - Z -
            if (Direction.Z == 0 && Origin.Z < boundingBox.Min.Z && Origin.Z > boundingBox.Max.Z)
            {
                // Ray is paralell and outside planes
                return null;
            }
            else
            {
                double t1 = (boundingBox.Min.Z - Origin.Z) / Direction.Z;
                double t2 = (boundingBox.Max.Z - Origin.Z) / Direction.Z;

                if (t1 > t2)
                {
                    MathHelper.Swap(ref t1, ref t2);
                }
                if (t1 > tNear)
                {
                    tNear = t1;
                }
                if (t2 < tFar)
                {
                    tFar = t2;
                }
                if (tNear > tFar || tFar < 0)
                {
                    return null;
                }
            }
            #endregion

            return tNear;
        }
    }
}
