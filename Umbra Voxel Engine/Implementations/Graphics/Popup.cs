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

namespace Umbra.Implementations.Graphics
{
    static public class Popup
    {
        static private double LastMessageTimeStamp = 0;
        static private double LastTimeStamp = 0;
        static private string LastMessage = "";
        static private int alpha = 0;

        static public void Post(string message)
        {
            LastMessage = message;
            LastMessageTimeStamp = LastTimeStamp;
        }
		
		static public void Post(object message)
        {
            LastMessage = message.ToString();
            LastMessageTimeStamp = LastTimeStamp;
        }

        static public void Update(FrameEventArgs e)
        {
            LastTimeStamp += e.Time;
        }

        static public void Render(FrameEventArgs e)
        {
            if (LastMessageTimeStamp != 0)
            {
                if (LastTimeStamp - LastMessageTimeStamp < Constants.Overlay.Popup.Timein)
                {
                    // Fade in
                    alpha += 4;
                    if (alpha >= 255)
                    {
                        alpha = 255;
                    }
                }
                else if (LastTimeStamp - LastMessageTimeStamp > Constants.Overlay.Popup.Timeout)
                {
                    // Fade out
                    alpha -= 4;
                    if (alpha < 0)
                    {
                        alpha = 0;
                    }
                }

                RenderHelp.RenderTexture(Constants.Engines.Overlay.BlankTextureID, new Rectangle(0, 140, (int)Constants.Graphics.ScreenResolution.X, (int)SpriteString.Measure(LastMessage).Y), Color.FromArgb(alpha / 3, 20, 20, 20));
                RenderHelp.RenderTexture(Constants.Engines.Overlay.BlankTextureID, new Rectangle(0, 139, (int)Constants.Graphics.ScreenResolution.X, 1), Color.FromArgb(alpha / 2, 200, 200, 200));
                RenderHelp.RenderTexture(Constants.Engines.Overlay.BlankTextureID, new Rectangle(0, 140 + (int)SpriteString.Measure(LastMessage).Y, (int)Constants.Graphics.ScreenResolution.X, 1), Color.FromArgb(alpha / 2, 200, 200, 200));

                SpriteString.Render(LastMessage, new Point((int)((Constants.Graphics.ScreenResolution.X - SpriteString.Measure(LastMessage).X) / 2), 141), Color.FromArgb(alpha, 255, 255, 255));
            }
        }
    }
}
