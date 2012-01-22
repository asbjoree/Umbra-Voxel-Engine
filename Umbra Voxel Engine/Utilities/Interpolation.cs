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
using Umbra.Utilities.Landscape;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Utilities
{
    static public class Interpolation
    {
        static public double Linear(double min, double max, double amount)
        {
            return min + amount * (max - min);
        }

        static public float Linear(float min, float max, float amount)
        {
            return min + amount * (max - min);
        }

        static public float Bilinear(float[,] Data, float x, float y)
        {
            return Data[0, 0] * (1 - x) * (1 - y) + Data[1, 0] * x * (1 - y) + Data[0, 1] * (1 - x) * y + Data[1, 1] * x * y;
        }

        static public float Trilinear(float[, ,] Data, float x, float y, float z)
        {
            float edge1 = Linear(Data[0, 0, 0], Data[1, 0, 0], x);
            float edge2 = Linear(Data[0, 1, 0], Data[1, 1, 0], x);
            float edge3 = Linear(Data[0, 0, 1], Data[1, 0, 1], x);
            float edge4 = Linear(Data[0, 1, 1], Data[1, 1, 1], x);

            float face1 = Linear(edge1, edge2, y);
            float face2 = Linear(edge3, edge4, y);

            return Linear(face1, face2, z);
        }

        static public float Bicubic(float x, float y)
        {
            float x2 = x * x;
            float x3 = x2 * x;
            float y2 = y * y;
            float y3 = y2 * y;

            return (float)((a00 + a01 * y + a02 * y2 + a03 * y3) +
                   (a10 + a11 * y + a12 * y2 + a13 * y3) * x +
                   (a20 + a21 * y + a22 * y2 + a23 * y3) * x2 +
                   (a30 + a31 * y + a32 * y2 + a33 * y3) * x3);

        }

        static private float a00, a01, a02, a03;
        static private float a10, a11, a12, a13;
        static private float a20, a21, a22, a23;
        static private float a30, a31, a32, a33;

        static public void UpdateBicubicCoefficients(float[,] p)
        {
            a00 = (float)(p[1, 1]);
            a01 = (float)(-.5 * p[1, 0] + .5 * p[1, 2]);
            a02 = (float)(p[1, 0] - 2.5 * p[1, 1] + 2 * p[1, 2] - .5 * p[1, 3]);
            a03 = (float)(-.5 * p[1, 0] + 1.5 * p[1, 1] - 1.5 * p[1, 2] + .5 * p[1, 3]);
            a10 = (float)(-.5 * p[0, 1] + .5 * p[2, 1]);
            a11 = (float)(.25 * p[0, 0] - .25 * p[0, 2] - .25 * p[2, 0] + .25 * p[2, 2]);
            a12 = (float)(-.5 * p[0, 0] + 1.25 * p[0, 1] - p[0, 2] + .25 * p[0, 3] + .5 * p[2, 0] - 1.25 * p[2, 1] + p[2, 2] - .25 * p[2, 3]);
            a13 = (float)(.25 * p[0, 0] - .75 * p[0, 1] + .75 * p[0, 2] - .25 * p[0, 3] - .25 * p[2, 0] + .75 * p[2, 1] - .75 * p[2, 2] + .25 * p[2, 3]);
            a20 = (float)(p[0, 1] - 2.5 * p[1, 1] + 2 * p[2, 1] - .5 * p[3, 1]);
            a21 = (float)(-.5 * p[0, 0] + .5 * p[0, 2] + 1.25 * p[1, 0] - 1.25 * p[1, 2] - p[2, 0] + p[2, 2] + .25 * p[3, 0] - .25 * p[3, 2]);
            a22 = (float)(p[0, 0] - 2.5 * p[0, 1] + 2 * p[0, 2] - .5 * p[0, 3] - 2.5 * p[1, 0] + 6.25 * p[1, 1] - 5 * p[1, 2] + 1.25 * p[1, 3] + 2 * p[2, 0] - 5 * p[2, 1] + 4 * p[2, 2] - p[2, 3] - .5 * p[3, 0] + 1.25 * p[3, 1] - p[3, 2] + .25 * p[3, 3]);
            a23 = (float)(-.5 * p[0, 0] + 1.5 * p[0, 1] - 1.5 * p[0, 2] + .5 * p[0, 3] + 1.25 * p[1, 0] - 3.75 * p[1, 1] + 3.75 * p[1, 2] - 1.25 * p[1, 3] - p[2, 0] + 3 * p[2, 1] - 3 * p[2, 2] + p[2, 3] + .25 * p[3, 0] - .75 * p[3, 1] + .75 * p[3, 2] - .25 * p[3, 3]);
            a30 = (float)(-.5 * p[0, 1] + 1.5 * p[1, 1] - 1.5 * p[2, 1] + .5 * p[3, 1]);
            a31 = (float)(.25 * p[0, 0] - .25 * p[0, 2] - .75 * p[1, 0] + .75 * p[1, 2] + .75 * p[2, 0] - .75 * p[2, 2] - .25 * p[3, 0] + .25 * p[3, 2]);
            a32 = (float)(-.5 * p[0, 0] + 1.25 * p[0, 1] - p[0, 2] + .25 * p[0, 3] + 1.5 * p[1, 0] - 3.75 * p[1, 1] + 3 * p[1, 2] - .75 * p[1, 3] - 1.5 * p[2, 0] + 3.75 * p[2, 1] - 3 * p[2, 2] + .75 * p[2, 3] + .5 * p[3, 0] - 1.25 * p[3, 1] + p[3, 2] - .25 * p[3, 3]);
            a33 = (float)(.25 * p[0, 0] - .75 * p[0, 1] + .75 * p[0, 2] - .25 * p[0, 3] - .75 * p[1, 0] + 2.25 * p[1, 1] - 2.25 * p[1, 2] + .75 * p[1, 3] + .75 * p[2, 0] - 2.25 * p[2, 1] + 2.25 * p[2, 2] - .75 * p[2, 3] - .25 * p[3, 0] + .75 * p[3, 1] - .75 * p[3, 2] + .25 * p[3, 3]);
        }
    }
}
