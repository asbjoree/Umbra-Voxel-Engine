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
    public abstract class PhysicsObject
    {
        public bool PhysicsEnabled;

        public Vector3d Position;
        public Vector3d Velocity;
        public Vector3d AccelerationAccumulator { get; protected set; }
        public BoundingBox BoundingBox { get { return new BoundingBox(Position, Position + Dimensions); } }
        public Vector3d Dimensions { get; protected set; }

        public double Volume { get; protected set; }
        public double Mass { get; protected set; }
        public double DragCoefficient { get; protected set; }
        public double SurfaceFrictionCoefficient { get; protected set; }

        public double BuoyancyMagnitude
        {
            get
            {
				//double buoyancy = 0.0;

				//foreach (BlockIndex index in BoundingBox.IntersectionIndices)
				//{
				//    buoyancy += Constants.World.Current.GetBlock(index).Density * BoundingBox.IntersectionVolume(index.GetBoundingBox());
				//}

				//return (2.0 * Constants.Physics.Gravity * buoyancy) / (Mass + buoyancy) / Mass;
				return 0.0;
            }
        }

        public double AverageViscosity
        {
            get
            {
                double average = 0.0;

                foreach (BlockIndex index in BoundingBox.IntersectionIndices)
                {
                    average += Constants.World.Current.GetBlock(index).Viscosity * (index.GetBoundingBox().IntersectionVolume(BoundingBox) / Volume);
                }

                return average;
            }
        }

        public double KineticFrictionCoefficient
        {
            get
            {
                double maxFriction = 0.0;

                foreach (BlockIndex index in Constants.Engines.Physics.BlocksBeneath(this))
                {
                    if (maxFriction < Constants.World.Current.GetBlock(index).KineticFrictionCoefficient)
                    {
                        maxFriction = Constants.World.Current.GetBlock(index).KineticFrictionCoefficient;
                    }
                }

                return maxFriction;
            }
        }

        public double GripCoefficient
        {
            get
            {
                double maxGrip = 0.0;

                foreach (BlockIndex index in Constants.Engines.Physics.BlocksBeneath(this))
                {
                    if (maxGrip < Constants.World.Current.GetBlock(index).GripCoefficient)
                    {
                        maxGrip = Constants.World.Current.GetBlock(index).GripCoefficient;
                    }
                }

                return maxGrip;
            }
        }

        public PhysicsObject(Vector3d position, double mass)
        {
            Position = position;
            Dimensions = Vector3d.One;
            Velocity = Vector3d.Zero;
            AccelerationAccumulator = Vector3d.Zero;
            Volume = Dimensions.X * Dimensions.Y * Dimensions.Z;
            Mass = mass;
            PhysicsEnabled = true;
            DragCoefficient = 1;
        }

        public PhysicsObject(Vector3d position, Vector3d dimension, double mass)
        {
            Position = position;
            Dimensions = dimension;
            Velocity = Vector3d.Zero;
            AccelerationAccumulator = Vector3d.Zero;
            Volume = Dimensions.X * Dimensions.Y * Dimensions.Z;
            Mass = mass;
            PhysicsEnabled = true;
            DragCoefficient = 1;
        }

        public PhysicsObject(Vector3d position, Vector3d dimension, double mass, double volume)
        {
            Position = position;
            Dimensions = dimension;
            Velocity = Vector3d.Zero;
            AccelerationAccumulator = Vector3d.Zero;
            Volume = volume;
            Mass = mass;
            PhysicsEnabled = true;
            DragCoefficient = 1;
        }

        public void ResetAccelerationAccumulator()
        {
            AccelerationAccumulator = Vector3d.Zero;
        }

        public void Accelerate(Vector3d acceleration)
        {
			AccelerationAccumulator += acceleration;
        }

        public void ApplyAcceleration()
        {
            Velocity += AccelerationAccumulator * Constants.Physics.TimeStep;
        }

		public void SetVelocity(Vector3d velocity)
		{
			Velocity = velocity;
		}

        virtual public void Update()
        {
        }
    }
}
