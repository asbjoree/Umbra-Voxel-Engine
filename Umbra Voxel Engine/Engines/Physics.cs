using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
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
using Umbra.Implementations.Graphics;
using Console = Umbra.Implementations.Graphics.Console;


namespace Umbra.Engines
{
	public class Physics : Engine
	{
		List<PhysicsObject> PhysicsObjects;

		public Player Player { get { return (Player)PhysicsObjects.First(); } }
		public Thread CalculationsThread { get; private set; }

		public Physics()
		{
			PhysicsObjects = new List<PhysicsObject>();
			PhysicsObjects.Add(new Player());


			CalculationsThread = new Thread(new ThreadStart(RunCalculationThread));
			CalculationsThread.Start();
		}

		public void AbortThread()
		{
			CalculationsThread.Abort();
		}

		public void RunCalculationThread()
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();

			while (true)
			{
				long currentTime = timer.ElapsedMilliseconds;

				if (timer.ElapsedMilliseconds >= (Constants.Physics.TimeStep * 1000.0))
				{
					timer.Restart();
					foreach (PhysicsObject currentObject in PhysicsObjects)
					{
						currentObject.Update();
						if (currentObject.PhysicsEnabled)
						{
							UpdateVelocity(currentObject);
							UpdatePosition(currentObject);
						}
						currentObject.ResetAccelerationAccumulator();
					}
				}
			}
		}

		public override void Update(FrameEventArgs e)
		{
			if (!Variables.Player.NoclipEnabled && Constants.World.DynamicWorld)
			{
				if (new ChunkIndex(Constants.Engines.Physics.Player.Position) != new ChunkIndex(ChunkManager.WorldCenter))
				{
					ChunkManager.UpdateCenter(new ChunkIndex(Constants.Engines.Physics.Player.Position));
				}
			}

			base.Update(e);
		}

		private void UpdateVelocity(PhysicsObject currentObject)
		{

			// To avoid errors (specifically, NaN), remove velocity if too small.
			if (Player.Velocity.Length <= Constants.Physics.MinSpeed)
			{
				Player.Velocity = Vector3d.Zero;
			}

			// Gravity
			currentObject.Accelerate(-Vector3d.UnitY * Constants.Physics.Gravity);

			// Buoyancy
			currentObject.Accelerate(Vector3d.UnitY * currentObject.BuoyancyMagnitude);

			// Surface friction
			Vector3d horizontalVelocity = Vector3d.Multiply(currentObject.Velocity, new Vector3d(1, 0, 1));

			if (IsOnGround(currentObject) && horizontalVelocity != Vector3d.Zero)
			{
				currentObject.Accelerate((-Vector3d.Normalize(horizontalVelocity) * currentObject.KineticFrictionCoefficient * Constants.Physics.Gravity) * horizontalVelocity.Length * Constants.Physics.FrictionSignificance / Constants.Physics.GripSignificance);
			}

			// Update velocity
			currentObject.ApplyAcceleration();
		}

		private void UpdatePosition(PhysicsObject obj)
		{
			Vector3d newPos = obj.Position + obj.Velocity * Constants.Physics.TimeStep;

			if (PlaceFree(obj, newPos))
			{
				obj.Position = newPos;
			}
			else
			{
				UpdatePositionOneDimension(obj, ref obj.Position.Y, ref obj.Velocity.Y, Vector3d.UnitY);

				if (Math.Abs(obj.Velocity.X) > Math.Abs(obj.Velocity.Z))
				{
					UpdatePositionOneDimension(obj, ref obj.Position.X, ref obj.Velocity.X, Vector3d.UnitX);
					UpdatePositionOneDimension(obj, ref obj.Position.Z, ref obj.Velocity.Z, Vector3d.UnitZ);
				}
				else
				{
					UpdatePositionOneDimension(obj, ref obj.Position.Z, ref obj.Velocity.Z, Vector3d.UnitZ);
					UpdatePositionOneDimension(obj, ref obj.Position.X, ref obj.Velocity.X, Vector3d.UnitX);
				}
			}
		}

		private bool PlaceFree(PhysicsObject obj, Vector3d position)
		{
			foreach (BlockIndex index in obj.BoundingBox.At(position).IntersectionIndices)
			{
				if (ChunkManager.GetBlock(index).Solidity)
				{
					return false;
				}
			}

			return true;
		}

		private void UpdatePositionOneDimension(PhysicsObject obj, ref double position, ref double velocity, Vector3d axis)
		{
			while (!PlaceFree(obj, velocity * Constants.Physics.TimeStep * axis + obj.Position))
			{
				if (Math.Round(velocity, 4) != 0.0)
				{
					velocity = velocity / 1.5;
				}
				else
				{
					velocity = 0.0;
					return;
				}
			}

			position += velocity * Constants.Physics.TimeStep;
		}

		public List<BlockIndex> BlocksBeneath(PhysicsObject obj)
		{
			List<BlockIndex> returnList = new List<BlockIndex>();

			for (int x = (int)Math.Floor(obj.BoundingBox.Min.X); x <= Math.Floor(obj.BoundingBox.Max.X); x++)
			{
				for (int z = (int)Math.Floor(obj.BoundingBox.Min.Z); z <= Math.Floor(obj.BoundingBox.Max.Z); z++)
				{
					returnList.Add(new BlockIndex(x, (int)Math.Floor(obj.Position.Y - Constants.Player.MinDistanceToGround), z));
				}
			}

			return returnList;
		}

		public bool IsOnGround(PhysicsObject obj)
		{
			return !PlaceFree(obj, obj.Position - Vector3d.UnitY * Constants.Player.MinDistanceToGround);
		}
	}
}
