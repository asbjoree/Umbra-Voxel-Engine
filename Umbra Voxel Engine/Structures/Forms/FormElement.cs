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
    interface IFormElement
    {
        void Render(Rectangle clientFrame);
    }

    interface IControl : IFormElement
    {
        void Update();
    }

    class Panel : IFormElement
    {
        int BackgroundID;

        List<IFormElement> SubElements;

        public Bitmap Background
        {
            set
            {
                RenderHelp.CreateTexture2D(out BackgroundID, value);
            }
        }

        public Panel()
        {
            SubElements = new List<IFormElement>();
        }

        virtual public void Render(Rectangle clientFrame)
        {
            RenderHelp.RenderTexture(BackgroundID, clientFrame);

            foreach (IFormElement element in SubElements)
            {
                element.Render(clientFrame);
            }
        }

        public void Update()
        {
            foreach (IFormElement element in SubElements)
            {
                if (element is IControl)
                {
                    ((IControl)element).Update();
                }
            }
        }
    }
}
