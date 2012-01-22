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

namespace Umbra.Structures.Forms
{
    class Handle
    {
        public Rectangle HandleRectangle;
        Point? Grip;
        bool DynamicGrip;

        public Handle(Rectangle handleRectangle, bool dynamicGrip)
        {
            HandleRectangle = handleRectangle;
            Grip = null;
            DynamicGrip = dynamicGrip;

            Constants.Engines.Main.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Click);
            Constants.Engines.Main.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Release);
        }

        public void Click(object sender, MouseButtonEventArgs e)
        {
            if (HandleRectangle.Contains(e.Position))
            {
                if (DynamicGrip)
                {
                    Grip = new Point(e.Position.X - HandleRectangle.X, e.Position.Y - HandleRectangle.Y);
                }
                else
                {
                    Grip = new Point(HandleRectangle.Width / 2, HandleRectangle.Height / 2);
                }
            }
        }

        public void Release(object sender, MouseButtonEventArgs e)
        {
            Grip = null;
        }

        public bool Update()
        {
            if (Grip.HasValue)
            {
                HandleRectangle.Location = new Point(Constants.Engines.Input.MousePosition.X - Grip.Value.X, Constants.Engines.Input.MousePosition.Y - Grip.Value.Y);

                return true;
            }

            return false;
        }
    }
}
