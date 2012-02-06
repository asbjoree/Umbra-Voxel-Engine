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
	class CompassPanel : Panel
	{
		static private int TextureID;
		static private Bitmap Texture;

		static public Form GetCompass
		{
			get
			{
				Form Compass = new Form(
					(int)Constants.Graphics.ScreenResolution.X - Constants.Overlay.Compass.FrameSize.X,
					0, 
					Constants.Overlay.Compass.FrameSize.X, 
					Constants.Overlay.Compass.FrameSize.Y
					);
				Compass.Content = new CompassPanel();
				Compass.HasFrame = false;
				Compass.Resizable = false;

				Compass.Show();
				return Compass;
			}
		}

		public CompassPanel()
		{
			Texture = Content.Load<Bitmap>(Constants.Content.Textures.CompassFilename);

			RenderHelp.CreateTexture2D(out TextureID, Texture);
		}

		public override void Render(Rectangle clientFrame)
		{
			// Draw frame
			RenderHelp.RenderTexture(
				TextureID,
				Texture.Size,
				clientFrame.Location,
				new Rectangle(0, 0, (int)Constants.Overlay.Compass.FrameSize.X, (int)Constants.Overlay.Compass.FrameSize.Y));



			int degrees = (int)Mathematics.WrapAngleDegrees(MathHelper.RadiansToDegrees(-(float)Constants.Engines.Physics.Player.FirstPersonCamera.Direction) - 62.0); // -62.0 offsets the compass to show the right direction

			Rectangle mainRectangle = new Rectangle();
			mainRectangle.Y = (int)Constants.Overlay.Compass.FrameSize.Y;
			mainRectangle.Height = (int)Constants.Overlay.Compass.StripWindowSize.Y;
			mainRectangle.X = degrees;
			mainRectangle.Width = Constants.Overlay.Compass.StripWindowSize.X;

			// Draw main strip

			Point stripLocation = new Point(clientFrame.X + Constants.Overlay.Compass.StripOffset.X, clientFrame.Y + Constants.Overlay.Compass.StripOffset.Y);

			RenderHelp.RenderTexture(TextureID, Texture.Size, stripLocation, mainRectangle);
		}
	}
}
