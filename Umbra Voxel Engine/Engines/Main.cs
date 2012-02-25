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

namespace Umbra.Engines
{
	public class Main : GameWindow
	{
		List<Engine> Engines;

		public Main(GraphicsMode mode, string title, GameWindowFlags flags)
			: base((int)Constants.Graphics.ScreenResolution.X, (int)Constants.Graphics.ScreenResolution.Y, mode, title, flags)
		{
			Engines = new List<Engine>();
			Constants.SetupEngines(this);
			Constants.Engines.Input.SetMouseShow(false);
		}
			
		public void AddEngine(Engine engine)
		{
			Engines.Add(engine);
			engine.SetGame(this);
		}

		protected override void OnLoad(EventArgs e)
		{
			foreach (Engine engine in Engines)
			{
				engine.Initialize(e);
			}

			Variables.Game.IsInitialized = true;
			base.OnLoad(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			foreach (Engine engine in Engines)
			{
				engine.Update(e);
			}

			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.ClearColor(Variables.Graphics.ScreenClearColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			foreach (Engine engine in Engines)
			{
				engine.Render(e);
			}

			GL.Finish();
			SwapBuffers();

			base.OnRenderFrame(e);
		}
	}
}
