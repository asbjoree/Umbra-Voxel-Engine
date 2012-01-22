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

namespace Umbra.Utilities.Landscape.Utilities
{
    static public class NoiseMaps
    {
        #region - Static Noise -
        static public float GetByValuesFast(int a, int b = 1, int c = 2, int d = 3)
        {
            return Math.Abs((float)(((
                ((float)a + 0.1F) *
                ((float)b - 0.1F) *
                ((float)c + 0.1F) *
                ((float)d - 0.1F) +
                ((float)b + 0.1F) *
                ((float)c - 0.1F) *
                ((float)d + 0.1F))) % 10000)) / 10000.0F;
        }

        static public float GetByValues(int a, int b = 1, int c = 2, int d = 3)
        {
            return Math.Abs((float)(((
                ((float)a + 0.1F) *
                ((float)b - 0.1F) *
                ((float)c + 0.1F) *
                ((float)d - 0.1F) +
                ((float)b + 0.1F) *
                ((float)c - 0.1F) *
                ((float)d + 0.1F)).ToString().GetHashCode()) % 10000)) / 10000.0F;
        }

        static public int GetTreeSeeds(int x, int y, int seed)
        {
            if (GetByValues(x, y, seed) >= Constants.Landscape.Vegetation.TreeDensity)
            {
                return 0;
            }
            else
            {
                return (int)(GetByValuesFast(x, y, seed + 1) * Constants.Landscape.Vegetation.TreeVaryHeight) + Constants.Landscape.Vegetation.TreeMinHeight;
            }
        }

        #endregion

        #region - Perlin -

        static public float GetPerlin(int x, int y, int seed)
        {
            return GetPerlin(x, y, Constants.Landscape.TerrainStretch, seed);
        }

        static public float GetPerlin(int x, int y, int depth, int seed)
        {
            float returnValue = 0.0F;

            for (int level = 1; level <= depth; level++)
            {
                returnValue += GetBilinearlyInterpolated(x, y, (int)Math.Pow(2, level + 1), seed + level) * (float)(Math.Pow(2, level) / Math.Pow(2, depth + 1));
            }

            return returnValue;
        }
        #endregion

        #region - Interpolated -

        static public float GetBilinearlyInterpolated(int x, int y, int squareSize, int seed)
        {
            int xInSquare = Mathematics.AbsModulo(x, squareSize);
            int yInSquare = Mathematics.AbsModulo(y, squareSize);

            float[,] data = new float[2, 2];

            data[0, 0] = GetByValuesFast(x - xInSquare, y - yInSquare, seed);
            data[0, 1] = GetByValuesFast(x - xInSquare, y - yInSquare + squareSize, seed);
            data[1, 0] = GetByValuesFast(x - xInSquare + squareSize, y - yInSquare, seed);
            data[1, 1] = GetByValuesFast(x - xInSquare + squareSize, y - yInSquare + squareSize, seed);

            return Interpolation.Bilinear(data, (float)Math.Abs(xInSquare) / (float)(squareSize), (float)Math.Abs(yInSquare) / (float)(squareSize));
        }

        static public float GetTrilinearlyInterpolated(int x, int y, int z, int cubeSize, int seed)
        {
            int xInCube = Mathematics.AbsModulo(x, cubeSize);
            int yInCube = Mathematics.AbsModulo(y, cubeSize);
            int zInCube = Mathematics.AbsModulo(z, cubeSize);

            float[,,] data = new float[2, 2, 2];

            data[0, 0, 0] = GetByValuesFast(x - xInCube, y - yInCube, z - zInCube, seed);
            data[0, 0, 1] = GetByValuesFast(x - xInCube, y - yInCube, z - zInCube + cubeSize, seed);
            data[0, 1, 0] = GetByValuesFast(x - xInCube, y - yInCube + cubeSize, z - zInCube, seed);
            data[0, 1, 1] = GetByValuesFast(x - xInCube, y - yInCube + cubeSize, z - zInCube + cubeSize, seed);
            data[1, 0, 0] = GetByValuesFast(x - xInCube + cubeSize, y - yInCube, z - zInCube, seed);
            data[1, 0, 1] = GetByValuesFast(x - xInCube + cubeSize, y - yInCube, z - zInCube + cubeSize, seed);
            data[1, 1, 0] = GetByValuesFast(x - xInCube + cubeSize, y - yInCube + cubeSize, z - zInCube, seed);
            data[1, 1, 1] = GetByValuesFast(x - xInCube + cubeSize, y - yInCube + cubeSize, z - zInCube + cubeSize, seed);

            return Interpolation.Trilinear(data,
                (float)Math.Abs(xInCube) / (float)(cubeSize),
                (float)Math.Abs(yInCube) / (float)(cubeSize),
                (float)Math.Abs(zInCube) / (float)(cubeSize));
        }

        static public float GetBicubiclyInterpolated(int x, int y, int squareSize, int seed)
        {
            int xInSquare = Mathematics.AbsModulo(y, squareSize);
            int yInSquare = Mathematics.AbsModulo(y, squareSize);

            float[,] data = new float[4, 4];

            data[0, 0] = GetByValuesFast(x - xInSquare - squareSize, y - yInSquare - squareSize, seed);
            data[0, 1] = GetByValuesFast(x - xInSquare - squareSize, y - yInSquare, seed);
            data[0, 2] = GetByValuesFast(x - xInSquare - squareSize, y - yInSquare + squareSize, seed);
            data[0, 3] = GetByValuesFast(x - xInSquare - squareSize, y - yInSquare + squareSize * 2, seed);

            data[1, 0] = GetByValuesFast(x - xInSquare, y - yInSquare - squareSize, seed);
            data[1, 1] = GetByValuesFast(x - xInSquare, y - yInSquare, seed);
            data[1, 2] = GetByValuesFast(x - xInSquare, y - yInSquare + squareSize, seed);
            data[1, 3] = GetByValuesFast(x - xInSquare, y + y - yInSquare + squareSize * 2, seed);

            data[2, 0] = GetByValuesFast(x - xInSquare + squareSize, y - yInSquare - squareSize, seed);
            data[2, 1] = GetByValuesFast(x - xInSquare + squareSize, y - yInSquare, seed);
            data[2, 2] = GetByValuesFast(x - xInSquare + squareSize, y - yInSquare + squareSize, seed);
            data[2, 3] = GetByValuesFast(x - xInSquare + squareSize, y - yInSquare + squareSize * 2, seed);

            data[3, 0] = GetByValuesFast(x - xInSquare + squareSize * 2, y - yInSquare - squareSize, seed);
            data[3, 1] = GetByValuesFast(x - xInSquare + squareSize * 2, y - yInSquare, seed);
            data[3, 2] = GetByValuesFast(x - xInSquare + squareSize * 2, y - yInSquare + squareSize, seed);
            data[3, 3] = GetByValuesFast(x - xInSquare + squareSize * 2, y - yInSquare + squareSize * 2, seed);

            Interpolation.UpdateBicubicCoefficients(data);

            return Interpolation.Bicubic((float)Math.Abs(xInSquare) / (float)(squareSize), (float)Math.Abs(yInSquare) / (float)(squareSize));
        }
        #endregion


    }
}