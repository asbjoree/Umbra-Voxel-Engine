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

namespace Umbra.Utilities
{
    static class Mathematics
    {
        static public int AbsModulo(int value, int floor)
        {
            if (value > 0)
            {
                return value % floor;
            }
            else if (value < 0)
            {
                if (value % floor == 0)
                {
                    return 0;
                }
                else
                {
                    return floor + (value % floor);
                }
            }
            else
            {
                return 0;
            }
        }

        static public double WrapAngleRadians(double value)
        {
            double returnvalue = value % (Math.PI * 2);
            if (returnvalue <= 0)
            {
                returnvalue = (Math.PI * 2) + returnvalue;
            }

            return returnvalue;
        }

        static public double WrapAngleDegrees(double value)
        {
            double returnvalue = value % 360;
            if (returnvalue < 0)
            {
                returnvalue = 360 + returnvalue;
            }

            return returnvalue;
        }

        static public double Clamp(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

		static public Vector3d ToVector3D(Vector3 vec)
		{
			return new Vector3d(vec.X, vec.Y, vec.Z);
		}
    }
}
