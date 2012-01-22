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

namespace Umbra.Definitions
{
    public enum TimeOfDay : byte
    {
        SunRise,
        Day,
        SunSet,
        Night
    }

    public enum FaceValidation
    {
        NoFaces,
        ThisFace,
        OtherFace,
        BothFaces,
        Indeterminate
    }

    public enum ConsoleState : byte
    {
        Open,
        Closed,
        FadeIn,
        FadeOut
    }

    public enum BlockVisibility
    {
        Invisible,
        Translucent,
        Opaque
    }

    public enum GraphingVariable
    {
        PlayerPositionX,
        PlayerPositionY,
        PlayerPositionZ,
        PlayerVelocityX,
        PlayerVelocityY,
        PlayerVelocityZ,
        PlayerAccelerationX,
        PlayerAccelerationY,
        PlayerAccelerationZ,
        RAM
    }

    public enum ScalingMode
    {
        NoScaling,
        ScaleToFit,
        FreeMove,
        FreeZoom,
        FullFree
    }

    public struct ConsoleMessage
    {
        public string Message;
        public double Timestamp;
        //public SpriteString SpriteString;
        public Color Color;

        public ConsoleMessage(string message, double timestamp, Color color)
        {
            Message = message;
            Timestamp = timestamp;
            Color = color;

            //SpriteString = new SpriteString(Console.Font, message);
        }
    }

    public class Direction
    {
        private enum Dir : byte
        {
            Right,
            Left,
            Up,
            Down,
            Backward,
            Forward
        }

        public byte Value
        {
            get
            {
                return (byte)DirectionEnum;
            }
        }

        Dir DirectionEnum;

        private Direction(Dir direction)
        {
            DirectionEnum = direction;
        }

        static public Direction Backward { get { return new Direction(Dir.Backward); } }
        static public Direction Forward { get { return new Direction(Dir.Forward); } }
        static public Direction Left { get { return new Direction(Dir.Left); } }
        static public Direction Right { get { return new Direction(Dir.Right); } }
        static public Direction Up { get { return new Direction(Dir.Up); } }
        static public Direction Down { get { return new Direction(Dir.Down); } }

        static public Vector3d GetFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        return Direction.Right.GetVector3();
                    }

                case 1:
                    {
                        return Direction.Left.GetVector3();
                    }

                case 2:
                    {
                        return Direction.Up.GetVector3();
                    }

                case 3:
                    {
                        return Direction.Down.GetVector3();
                    }

                case 4:
                    {
                        return Direction.Backward.GetVector3();
                    }

                case 5:
                    {
                        return Direction.Forward.GetVector3();
                    }

                default:
                    {
                        throw new IndexOutOfRangeException();
                    }

            }
        }

        public Direction Opposite()
        {
            switch (DirectionEnum)
            {
                case Dir.Backward: return Direction.Forward;
                case Dir.Forward: return Direction.Backward;
                case Dir.Left: return Direction.Right;
                case Dir.Right: return Direction.Left;
                case Dir.Up: return Direction.Down;
                case Dir.Down: return Direction.Up;
                default: throw new Exception("This shouldn't happen.");
            }
        }

        public Vector3d GetVector3()
        {
            switch (DirectionEnum)
            {
                case Dir.Backward: return Vector3d.UnitZ;
                case Dir.Forward: return -Vector3d.UnitZ;
                case Dir.Right: return Vector3d.UnitX;
                case Dir.Left: return -Vector3d.UnitX;
                case Dir.Up: return Vector3d.UnitY;
                case Dir.Down: return -Vector3d.UnitY;
                default: throw new Exception("This shouldn't happen.");
            }
        }

        public Vector3d GetPerpendicularRight()
        {
            switch (DirectionEnum)
            {
                case Dir.Backward: return Vector3d.UnitX;
                case Dir.Forward: return -Vector3d.UnitX;
                case Dir.Right: return Vector3d.UnitY;
                case Dir.Left: return -Vector3d.UnitY;
                case Dir.Up: return Vector3d.UnitZ;
                case Dir.Down: return -Vector3d.UnitZ;
                default: throw new Exception("This shouldn't happen.");
            }
        }

        public Vector3d GetPerpendicularLeft()
        {
            switch (DirectionEnum)
            {
                case Dir.Backward: return Vector3d.UnitY;
                case Dir.Forward: return -Vector3d.UnitY;
                case Dir.Right: return Vector3d.UnitZ;
                case Dir.Left: return -Vector3d.UnitZ;
                case Dir.Up: return Vector3d.UnitX;
                case Dir.Down: return -Vector3d.UnitX;
                default: throw new Exception("This shouldn't happen.");
            }
        }

        static public bool operator ==(Direction dir1, Direction dir2)
        {
            return (dir1.DirectionEnum == dir2.DirectionEnum);
        }

        static public bool operator !=(Direction dir1, Direction dir2)
        {
            return (dir1.DirectionEnum != dir2.DirectionEnum);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
