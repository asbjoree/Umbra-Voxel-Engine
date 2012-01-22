using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

namespace Umbra
{
	static class Program
	{
		static public bool CodeClose = false;
		[STAThreadAttribute]
		static void Main()
		{

			if (Constants.Launcher.Enabled)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				Launcher Launcher = new Launcher();
				Application.Run(Launcher);
			}
			else
			{
				CodeClose = true;
			}


			if (Constants.Launcher.ReleaseModeEnabled)
			{
				try
				{
					RunEngine();
				}
				catch (Exception e)
				{
					Error("Internal Program Error!",
					"An error occurred while trying to run the game!" +
					"\n\n\tOpenGL version: " + GL.GetString(StringName.Version) +
					"\n\tGLSL version: " + GL.GetString(StringName.ShadingLanguageVersion) +
					"\n\tVendor: " + GL.GetString(StringName.Vendor) +
					"\n\tRenderer: " + GL.GetString(StringName.Renderer) +
					"\n\nError Message:\n" + e.Message +
					"\n\nDetails:\n" + e.StackTrace,
					true);

					Console.Execute("exit");
				}
			}
			else
			{
				RunEngine();
			}
		}

		static void RunEngine()
		{
			if (CodeClose)
			{
				Main UmbraEngine;

				if (Constants.Graphics.EnableFullScreen)
				{
					Constants.Graphics.ScreenResolution = new Vector2(SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);
					UmbraEngine = new Main(new GraphicsMode(), "Umbra Voxel Engine", GameWindowFlags.Fullscreen);
				}
				else
				{
					UmbraEngine = new Main(new GraphicsMode(), "Umbra Voxel Engine", GameWindowFlags.Default);
				}

				if (Constants.Graphics.AnisotropicFilteringEnabled && !GL.GetString(StringName.Extensions).Contains("GL_EXT_texture_filter_anisotropic"))
				{
					Error("Unsupported extension!", "Cannot enable anisotropic filtering.\n\nYour graphics card does not support the extension: \n\"GL_EXT_texture_filter_anisotropic\"", false);
					return;
				}

				UmbraEngine.Run(60.0, 60.0);
			}
		}


		static private void Error(string error, string errorMessage, bool shouldReport)
		{
			string message = errorMessage + (shouldReport ? "\n\n\nPlease report this error message.\nThanks in advance!\n\nCopy to clipboard?" : "");

			DialogResult result = System.Windows.Forms.MessageBox.Show(message, error, (shouldReport ? MessageBoxButtons.YesNo : MessageBoxButtons.OK));

			if (result == DialogResult.Yes)
			{
				Clipboard.SetText(errorMessage);
			}

		}
	}
}