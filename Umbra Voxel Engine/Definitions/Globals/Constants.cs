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
using Umbra.Utilities.Landscape;
using Umbra.Implementations.Graphics;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Definitions.Globals
{
	static public class Constants
	{

		static public class Engines
		{
			static public Umbra.Engines.Graphics Graphics;
			static public Umbra.Engines.Input Input;
			static public Umbra.Engines.Main Main;
			static public Umbra.Engines.Overlay Overlay;
			static public Umbra.Engines.Physics Physics;
			static public Umbra.Engines.Audio Sound;
		}

		static public void SetupEngines(Main main)
		{
			Engines.Graphics = new Umbra.Engines.Graphics();
			Engines.Input = new Umbra.Engines.Input();
			Engines.Main = main;
			Engines.Overlay = new Umbra.Engines.Overlay();
			Engines.Physics = new Umbra.Engines.Physics();
			Engines.Sound = new Umbra.Engines.Audio();

			Engines.Main.AddEngine(Engines.Input);
			Engines.Main.AddEngine(Engines.Physics);
			Engines.Main.AddEngine(Engines.Graphics);
			Engines.Main.AddEngine(Engines.Overlay);
			Engines.Main.AddEngine(Engines.Sound);


			TerrainGenerator.Initialize(Landscape.WorldSeed);
			Engines.Physics.Player.Initialize();

			ConsoleFunctions.Initialize();
			ChunkManager.Initialize();
			ClockTime.SetTimeOfDay(TimeOfDay.Day);

			Console.Initialize();
			SpriteString.Initialize();
		}

		static public class Overlay
		{

			static public Font DefaultFont = new Font("Lucida Console", 11, FontStyle.Regular);
			static public int DefaultFontWidth = 9; 

			static public class Console
			{
				static public int CharacterLimit = 32;
				static public Rectangle DefaultArea = new Rectangle(0, (int)Graphics.ScreenResolution.Y / 2, 290, (int)Graphics.ScreenResolution.Y / 2);
				static public double FadeSpeed = 0.5; // Seconds
				static public double Timeout = 10.0; // Seconds
				static public int MessageQuantity = 19;
			}

			static public class Popup
			{
				static public int Timein = 1; // Seconds
				static public int Timeout = 3; // Seconds
			}

			static public class Compass
			{
				static public Point FrameSize = new Point(144, 85);
				static public Point ScreenPosition = new Point((int)Graphics.ScreenResolution.X - FrameSize.X - 10, 10);
				static public Point StripOffset = new Point(10, 10);
				static public Point StripWindowSize = new Point(FrameSize.X - StripOffset.X * 2, FrameSize.Y - StripOffset.Y * 2);
			}
		}

		static public class Controls
		{
			static public double MouseSensitivityInv = 400.0;
			static public float AlterDelay = 30.0F;            // Delay between editing blocks (mS)
			static public bool SmoothCameraEnabled = false;
			static public bool CanPlaceBlocks = false;
			static public bool NoclipAllowed = true;
			static public float SmoothCameraResponse = 0.4F;
			static public Block[] PlacableBlocks = {
													Block.Grass,
													Block.Stone,
													Block.Dirt,
													Block.Water,
													Block.Leaves,
													Block.Lava,
													Block.Sand,
													Block.Log
											   };
		}

		static public class Graphics
		{
			static public Vector2 ScreenResolution;
			static public float AspectRatio
			{
				get
				{
					return ScreenResolution.X / ScreenResolution.Y;
				}
			}

			static public bool AntiAliasingEnabled = true;
			static public bool AmbientOcclusionEnabled = false;
			static public float CameraNearPlane = 0.01F;
			static public float CameraFarPlane = 64000.0F;
			static public float FieldOfView = MathHelper.DegreesToRadians(60);
			static public bool EnableFullScreen = false;
			static public int BlockCursorType = 1;  // 0-No cursor, 1-Dark block, 2-Wireframe, 3-Both
			static public bool AnisotropicFilteringEnabled = false;

			
			static public class Lighting
			{
				static public Vector3 DiffuseLightDirection
				{
					get
					{
						return Vector3.Normalize(new Vector3(0.8F, 1.0F, 0.6F));
					}
				}

				static readonly public float DayFaceLightCoef = 1F;
				static readonly public float NightFaceLightCoef = 0F;
			}

			static public class Fog
			{
				static readonly public float DayFogStart = World.WorldSize * World.ChunkSize / 2 - 80;
				static readonly public float NightFogStart = 0;
				static readonly public float DayFogEnd = World.WorldSize * World.ChunkSize / 2 - 20;
				static readonly public float NightFogEnd = World.WorldSize * World.ChunkSize / 5;
			}

			static public class DayNight
			{
				static readonly public Color DayColor = Color.CornflowerBlue;
				static readonly public Color NightColor = Color.Black;

				static public float DayDuration = 16;           //seconds
				static public float NightDuration = 6;          //seconds
				static public float TransitionDuration = 1;     //Happens twice every cycle
				static public float TotalDuration = DayDuration + NightDuration + 2 * TransitionDuration;
			}

			static public class Forms
			{
				static public int MinimumHeight = 52;
				static public int MinimumWidth = 60;
			}
		}

		static public class Launcher
		{
			static public bool Enabled = true;
			static public bool ReleaseModeEnabled = false;
		}

		static public class World
		{
			static public string Name = "default";
			static public int ChunkSize = 32;
			static public int WorldSize = 5;
			static public bool DynamicWorld = true;
			static public bool SaveDynamicWorld = false;
			static public int UpdateLengthFromCenter = ChunkSize;
		}

		static public class Physics
		{
			static public double Gravity = 20.0;
			static public double FrictionSignificance = 30.0;
			static public double GripSignificance = 70.0;
			static public double MinSpeed = 0.0005;
			static public double TimeStep = 0.01; // Seconds between each update
		}

		static public class Landscape
		{
			static public string WorldSeed = "";
			static public int TerrainStretch = 8;           // Area taken into account = 2^stretch, currently 256 blocks;
			static public float PerlinBicubicWeight = 0.7F; // 0.0F = Total perlin, 1.0F = Total Bicubic
			static public float WorldHeightAmplitude = 256.0F;
			static public int WorldHeightOffset = (int)(-WorldHeightAmplitude / 2.0F); 

			static public bool WaterEnabled = true;
			static public int WaterLevel = 0;

			static public int SandLevel = WaterLevel + 3;
			static public bool CavesEnabled = false;

			static public class Vegetation
			{
				static public bool TreesEnabled = true;
				static public int TreeMinHeight = 7;        // Tree height = (random 0-1) * TreeVaryHeight + TreeMinHeight
				static public int TreeVaryHeight = 8;       // Tree height will vary from TreeMinHeight to TreeVaryHeight + TreeMinHeight
				static public float TreeDensity = 0.05F;    // If a tree can be placed at a location, this is the chance that it will grow there.
			}
		}

		static public class Player
		{
			static public Vector3d Spawn = new Vector3d(16, 0, 16);
			static public double MinDistanceToGround = 0.02;

			static public class Physics
			{
				static public double Mass = 350.0;

				static public class Box
				{
					static public double Width = 0.6;
					static public double Height = 1.8;
				}

				static public double EyeHeight = 1.5;

				static public class Movement
				{
					static public double NoclipSpeed = 0.3;

					static public double WalkMagnitude = 4.0;
					static public double SwimMagnitude = 20.0;	// Swimming and movement in air (movement in non-solid blocks)
					static public double MaxSpeed = 4.0;
					static public double JumpVelocity = 6.7;
				}
			}

			static public class Camera
			{
				static public class Bobbing
				{
					static public float Speed = 0.4F;
					static public float Magnitude = 0.7F;
				}
			}

			static public class BlockEditing
			{
				static public Block StartBlock = Block.Stone;
				static public double Reach = 10.0;
			}
		}

		static public class Content
		{
			static public string MainPath = @"content/";

			static public class Textures
			{
				static public string Path = MainPath + @"textures/";
				static public class Packs
				{
					static public string Path = Textures.Path + @"texture packs/";
					static public string CurrentPackPath = Path + @"standard/";
				}
				static public string CrosshairFilename = Path + @"crosshair.png";
				static public string CompassFilename = Path + @"compass.png";
			}

			static public class Data
			{
				static public string WorldPath = Directory.GetCurrentDirectory() + @"/worlds/";
				static public string ChunkFilePath = @"chunks/";
				static public string ChunkFileExtension = @".cnk";
			}

			static public class Shaders
			{
				static private string Path = MainPath + @"shaders/";
				static public string Raytracer = Path + @"/raytracer.c";
			}
		}
	}
}
