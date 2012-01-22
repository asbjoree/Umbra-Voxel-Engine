using System;
using System.IO;
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

namespace Umbra.Implementations
{
	static public class Content
	{
		static public object Load(string assetName)
		{
			string extension = assetName.Substring(assetName.LastIndexOf('.'));

			switch (extension)
			{
				case ".png":
				{
					return (object)LoadBitmap(assetName);
				}

				default:
				{
					throw new FileLoadException("The file extension \"" + extension + "\" is not supported!");
				}
			}
		}

		static private Bitmap LoadBitmap(string assetName)
		{
			FileExists(assetName);

			return new Bitmap(assetName);
		}

		static private void FileExists(string assetName)
		{
			if (!File.Exists(assetName))
			{
				throw new FileNotFoundException("The file \"" + assetName + "\" could not be located!");
			}
		}

		static public object[] GetTexturePacks()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Constants.Content.Textures.Packs.Path);

			List<object> returnValue = new List<object>();

			foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
			{
				returnValue.Add(dir.Name);
			}

			return returnValue.ToArray();
		}
	}
}
