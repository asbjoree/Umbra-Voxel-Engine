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
    public class BoundingBox
    {
        public Vector3d Min { get; private set; }
        public Vector3d Max { get; private set; }

        public List<BlockIndex> IntersectionIndices
        {
            get
            {
                List<BlockIndex> returnList = new List<BlockIndex>();

                for (int x = (int)Math.Floor(Min.X); x <= Math.Floor(Max.X); x++)
                {
                    for (int y = (int)Math.Floor(Min.Y); y <= Math.Floor(Max.Y); y++)
                    {
                        for (int z = (int)Math.Floor(Min.Z); z <= Math.Floor(Max.Z); z++)
                        {
                            if (Intersects(new BlockIndex(x, y, z).GetBoundingBox()))
                            {
                                returnList.Add(new BlockIndex(x, y, z));
                            }
                        }
                    }
                }

                return returnList;
            }
        }

        public double SurfaceArea
        {
            get
            {
                return 2.0 * ((Max.X - Min.X) * (Max.Y - Min.Y) + (Max.X - Min.X) * (Max.Z - Min.Z) + (Max.Y - Min.Y) * (Max.Z - Min.Z));
            }
        }

        public BoundingBox(Vector3d min, Vector3d max)
        {
            Min = min;
            Max = max;
        }

        public bool Intersects(BoundingBox box)
        {
            if (box.Min.X >= Max.X || box.Max.X <= Min.X)
            {
                return false;
            }

            if (box.Min.Y >= Max.Y || box.Max.Y <= Min.Y)
            {
                return false;
            }

            if (box.Min.Z >= Max.Z || box.Max.Z <= Min.Z)
            {
                return false;
            }

            return true;
        }

        public double? Intersects(Ray ray)
        {
            return ray.Intersects(this);
        }

        public double IntersectionVolume(BoundingBox box)
        {
            double intersectionVolume = 1;

            if (!Intersects(box))
            {
                return 0;
            }

            List<double> points = new List<double>();
            points.Add(Min.X);
            points.Add(box.Min.X);
            points.Add(Max.X);
            points.Add(box.Max.X);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            points.Clear();
            points.Add(Min.Y);
            points.Add(box.Min.Y);
            points.Add(Max.Y);
            points.Add(box.Max.Y);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            points.Clear();
            points.Add(Min.Z);
            points.Add(box.Min.Z);
            points.Add(Max.Z);
            points.Add(box.Max.Z);
            points.Sort();
            intersectionVolume *= points[2] - points[1];

            return intersectionVolume;
        }

        public BoundingBox At(Vector3d pos)
        {
            return new BoundingBox(pos, (Max - Min) + pos);
        }

        public bool Contains(Vector3d point)
        {
            if (point.X > Max.X || point.X < Min.X)
            {
                return false;
            }

            if (point.Y > Max.Y || point.Y < Min.Y)
            {
                return false;
            }

            if (point.Z > Max.Z || point.Z < Min.Z)
            {
                return false;
            }

            return true;
        }
    }
}
