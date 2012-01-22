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

namespace Umbra.Structures
{
    public class Engine
    {
        protected Main Main { get; set; }

        public void SetGame(Main main)
        {
            Main = main;
        }

        public virtual void Initialize(EventArgs e)
        {
        }

        public virtual void Update(FrameEventArgs e)
        {
        }

        public virtual void Render(FrameEventArgs e)
        {
        }
    }
}
