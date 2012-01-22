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
using Umbra.Utilities.Landscape;
using Console = Umbra.Implementations.Graphics.Console;

namespace Umbra.Structures
{
	public class Player : PhysicsObject
	{
		public Camera FirstPersonCamera { get; private set; }
		float CurrentAlterDelay;
		public bool IsReleased;
		Vector3d MoveDirection;

		public Player()
			: base(Constants.Player.Spawn, new Vector3d(Constants.Player.Physics.Box.Width, Constants.Player.Physics.Box.Height, Constants.Player.Physics.Box.Width), Constants.Player.Physics.Mass)
		{
			FirstPersonCamera = new Camera(this.Position);
			CurrentAlterDelay = Constants.Controls.AlterDelay;
			IsReleased = false;
			MoveDirection = Vector3d.Zero;
			PhysicsEnabled = false;
		}

		public Matrix4 ViewMatrix
		{
			get
			{
				return FirstPersonCamera.GetView();
			}
		}

		public Matrix4 ProjectionMatrix
		{
			get
			{
				return FirstPersonCamera.GetProjection();
			}
		}

		public void Initialize()
		{
			Position.Y = Math.Ceiling(Math.Max(TerrainGenerator.GetLandscapeHeight((int)Position.X, (int)Position.Z), (double)Constants.Landscape.WaterLevel));
			IsReleased = false;
		}

		public void Release()
		{

			while (Constants.World.Current.GetBlock(new BlockIndex(Position + Vector3d.UnitY)) != Block.Air || Constants.World.Current.GetBlock(new BlockIndex(Position + Vector3d.UnitY * 2)) != Block.Air)
			{
				Position.Y++;
			}
			Position.Y += 1.0;

			IsReleased = true;
			Variables.Player.NoclipEnabled = false;
			PhysicsEnabled = true;
		}

		public override void Update()
		{

			PhysicsEnabled = !Variables.Player.NoclipEnabled;

			if (!Variables.Player.NoclipEnabled && Constants.World.DynamicWorld)
			{
				Vector3d currentWorldCenter = Constants.World.Current.Offset.Position + Vector3d.One * ((double)Constants.World.WorldSize / 2.0) * (double)Constants.World.ChunkSize;

				if (new ChunkIndex(Position) != new ChunkIndex(currentWorldCenter))
				{
					if ((Position - currentWorldCenter).Length > Constants.World.UpdateLengthFromCenter)
					{
						Constants.World.Current.OffsetChunks(new ChunkIndex(Position - currentWorldCenter));
					}
				}
			}

			UpdateMovement();
			UpdateCamera();


			base.Update();
		}

		public void UpdateMouse(MouseDevice mouse)
		{
			if (!Constants.Controls.CanPlaceBlocks)
			{
				return;
			}

			if (mouse[MouseButton.Left])
			{
				if (CurrentAlterDelay == 0)
				{
					BlockIndex target = BlockCursor.GetToDestroy();
					if (target != null)
					{
						Constants.World.Current.SetBlock(target, Block.Air);
						CurrentAlterDelay = Constants.Controls.AlterDelay;
					}
				}
			}
			else if (mouse[MouseButton.Right])
			{
				if (CurrentAlterDelay == 0)
				{
					BlockIndex target = BlockCursor.GetToCreate();
					if (target != null && !BoundingBox.Intersects(target.GetBoundingBox()))
					{
						Constants.World.Current.SetBlock(target, Variables.Player.BlockEditing.CurrentCursorBlock);
						CurrentAlterDelay = Constants.Controls.AlterDelay;
					}
				}
			}
			else
			{
				CurrentAlterDelay = 0;
			}

			if (CurrentAlterDelay > 0)
			{
				CurrentAlterDelay--;
			}
		}

		public void UpdateMovement()
		{
			if (MoveDirection != Vector3d.Zero)
			{
				// Add player movement
				if (Variables.Player.NoclipEnabled)
				{
					// Noclip
					Position += Vector3d.Transform(MoveDirection, FirstPersonCamera.Rotation);
				}
				else
				{

					Vector3d horizontalVelocity = Vector3d.Multiply(Velocity, new Vector3d(1, 0, 1));

					if (Constants.Engines.Physics.IsOnGround(this))
					{
						// Walking on ground

						Vector3d newVelocity = horizontalVelocity + Vector3d.Transform(MoveDirection, Matrix4d.CreateRotationY(FirstPersonCamera.Direction)) * (Constants.Player.Physics.Movement.WalkMagnitude);

						if (newVelocity != Vector3d.Zero)
						{
							newVelocity = Vector3d.Normalize(newVelocity) * Math.Min(newVelocity.Length, Constants.Player.Physics.Movement.MaxSpeed);
						}

						newVelocity.Y = 0;

						Accelerate((newVelocity - horizontalVelocity) * Constants.Physics.GripSignificance * GripCoefficient);
					}
					else
					{
						// In air or swimming

						Vector3d newVelocity = horizontalVelocity + Vector3d.Transform(MoveDirection, Matrix4d.CreateRotationY(FirstPersonCamera.Direction)) * (Constants.Player.Physics.Movement.SwimMagnitude);

						if (newVelocity != Vector3d.Zero)
						{
							newVelocity = Vector3d.Normalize(newVelocity) * Math.Min(newVelocity.Length, Constants.Player.Physics.Movement.MaxSpeed);
						}

						Accelerate((newVelocity - horizontalVelocity) * GripCoefficient * Constants.Physics.GripSignificance);
					}
				}
				MoveDirection = Vector3d.Zero;
			}
		}

		public void MoveForward()
		{
			MoveDirection.Z = Math.Max(MoveDirection.Z - 1.0, -1.0);
		}

		public void MoveBackward()
		{
			MoveDirection.Z = Math.Min(MoveDirection.Z + 1.0, 1.0);
		}

		public void MoveLeft()
		{
			MoveDirection.X = Math.Max(MoveDirection.X - 1.0, -1.0);
		}

		public void MoveRight()
		{
			MoveDirection.X = Math.Min(MoveDirection.X + 1.0, 1.0);
		}

		public void Jump()
		{
			if (Variables.Player.NoclipEnabled)
			{
				MoveDirection.Y = Math.Min(MoveDirection.Y + 1.0, 1.0);
			}
			else if (Constants.Engines.Physics.IsOnGround(this))
			{
				if (Velocity.Y == 0)
				{
					SetVelocity(new Vector3d(Velocity.X, Constants.Player.Physics.Movement.JumpVelocity, Velocity.Z));
				}
			}
		}

		private void UpdateCamera()
		{
			FirstPersonCamera.Position = Position + Vector3d.Multiply(Dimensions / 2.0F, new Vector3d(1, 0, 1)) + Vector3d.UnitY * Constants.Player.Physics.EyeHeight;
		}

		public float GetViewType()
		{
			Block block = Constants.World.Current.GetBlock(new BlockIndex(Constants.Engines.Physics.Player.FirstPersonCamera.Position));

			if (block == Block.Water)
			{
				return 1.0F;
			}
			else if (block == Block.Lava)
			{
				return 2.0F;
			}
			else
			{
				return 0.0F;
			}
		}
	}
}
