using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
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
using Umbra.Structures.Forms;
using Umbra.Definitions.Globals;
using Umbra.Implementations.Graphics;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Engines
{
	public class Overlay : Engine
	{
		public int BlankTextureID;
		List<Form> Forms;

		public Overlay()
		{
			Bitmap BlankTexture = new Bitmap(1, 1);
			BlankTexture.SetPixel(0, 0, Color.White);
			RenderHelp.CreateTexture2D(out BlankTextureID, BlankTexture);

			Forms = new List<Form>();

			Forms.Add(CompassPanel.GetCompass);
			Forms.Add(CrosshairPanel.GetCrosshair);
		}

		public void MouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!Main.Focused)
			{
				return;
			}
		}

		public override void Update(FrameEventArgs e)
		{
			Console.Update(e);
			Popup.Update(e);

			foreach (Form form in Forms)
			{
				form.Update();
			}

			base.Update(e);
		}

		double FPS = 0;
		double FPSaccum = 0;
		int i = 0;

		public override void Render(FrameEventArgs e)
		{
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Constants.Graphics.ScreenResolution.X, Constants.Graphics.ScreenResolution.Y, 0, -1.0, 1.0);

			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			Console.Render(e);
			Popup.Render(e);

			foreach (Form form in Forms)
			{
				form.Render();
			}
			FPSaccum += Math.Round(1.0 / e.Time, 0);
			if(i++ > 25)
			{
				i = 0;
				FPS = FPSaccum / 25;
				FPSaccum = 0;
			}

			if (Variables.Overlay.DisplayFPS)
			{
				SpriteString.Render(FPS + "", Point.Empty, Color.Yellow);
			}
			if (Variables.Game.DeveloperMode)
			{
				// Memory
				string memoryUsage = (int)(System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024) + " kB";
				SpriteString.Render(memoryUsage, new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(memoryUsage).X - 10, 100), Color.Yellow);

				// Position
				string[] position = { 
										"Px: " + Math.Round(Constants.Engines.Physics.Player.Position.X, 1), 
										"Py: " + Math.Round(Constants.Engines.Physics.Player.Position.Y, 1), 
										"Pz: " + Math.Round(Constants.Engines.Physics.Player.Position.Z, 1) 
									};

				SpriteString.Render(position[0], new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(position[0]).X - 10, 130), Color.Yellow);
				SpriteString.Render(position[1], new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(position[1]).X - 10, 150), Color.Yellow);
				SpriteString.Render(position[2], new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(position[2]).X - 10, 170), Color.Yellow);


				// Velocity
				string[] velocity = { 
										"Vx: " + Math.Round(Constants.Engines.Physics.Player.Velocity.X, 2), 
										"Vy: " + Math.Round(Constants.Engines.Physics.Player.Velocity.Y, 2), 
										"Vz: " + Math.Round(Constants.Engines.Physics.Player.Velocity.Z, 2) 
									};

				SpriteString.Render(velocity[0], new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(velocity[0]).X - 10, 200), Color.Yellow);
				SpriteString.Render(velocity[1], new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(velocity[1]).X - 10, 220), Color.Yellow);
				SpriteString.Render(velocity[2], new Point((int)Constants.Graphics.ScreenResolution.X - SpriteString.Measure(velocity[2]).X - 10, 240), Color.Yellow);

			}

			GL.Disable(EnableCap.Texture2D);
			GL.Disable(EnableCap.Blend);
			base.Render(e);
		}
	}
}
