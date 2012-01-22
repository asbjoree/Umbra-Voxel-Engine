using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Umbra.Definitions;
using Umbra.Definitions.Globals;
using Umbra.Implementations;
using Umbra.Structures;
using Umbra.Structures.Graphics;

namespace Umbra.Engines
{
	public class Graphics : Engine
	{
		int TextureID;
		float time;

		public override void Initialize(EventArgs e)
		{
			Shaders.CompileShaders();
			RenderHelp.CreateTexture2DArray(out TextureID, Block.GetBlockTexturePaths());
			time = 0.0F;

			base.Initialize(e);
		}

		private void UseShader(int shader)
		{
			GL.UseProgram(shader);

			if (shader == 0)
			{
				return;
			}

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();

			RenderHelp.BindTexture(TextureID, TextureUnit.Texture0);

			byte[,,] test = new byte[,,] {{{ 1 }}};

			GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, 0, 1, 1, 1, PixelFormat.Luminance, PixelType.UnsignedByte, test);

			GL.Uniform3(Shaders.PositionID, (Vector3)Constants.Engines.Physics.Player.Position);

			GL.Uniform3(Shaders.LookAtID, (Vector3)Constants.Engines.Physics.Player.FirstPersonCamera.GetLookAt());

			GL.Uniform2(Shaders.ResolutionID, Constants.Graphics.ScreenResolution);
			GL.Uniform1(Shaders.TimeID, time += 0.1F);
		}

		public override void Render(FrameEventArgs e)
		{

			RenderQuad();

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			BlockIndex currentAim = BlockCursor.GetToDestroy();
			if (currentAim != null)
			{
				RenderCursor(currentAim);
			}


			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.CullFace);
			GL.Disable(EnableCap.Blend);

			base.Render(e);
		}

		void RenderQuad()
		{
			UseShader(Shaders.DefaultShaderProgram.ProgramID);

			GL.Begin(BeginMode.Quads);
			{
				GL.Vertex2(1.0, -1.0);
				GL.Vertex2(-1.0, -1.0);
				GL.Vertex2(-1.0, 1.0);
				GL.Vertex2(1.0, 1.0);
			}
			GL.End();

			UseShader(0);
		}

		void RenderCursor(BlockIndex currentAim)
		{
			if (Constants.Graphics.BlockCursorType == 0)
			{
				return;
			}


			if (currentAim == null)
			{
				return;
			}

			GL.MatrixMode(MatrixMode.Projection);
			Matrix4 proj = Constants.Engines.Physics.Player.ProjectionMatrix;
			GL.LoadMatrix(ref proj);

			GL.MatrixMode(MatrixMode.Modelview);
			Matrix4 view = Constants.Engines.Physics.Player.ViewMatrix;
			GL.LoadMatrix(ref view);

			if (Constants.Graphics.BlockCursorType == 1 || Constants.Graphics.BlockCursorType == 3)
			{
				GL.Color4(0.0, 0.0, 0.0, 0.2);
				GL.Begin(BeginMode.Quads);
				{
					GL.Vertex3(new Vector3d(1.0, 0.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 1.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 1.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 0.0, 1.0) + currentAim.Position);

					GL.Vertex3(new Vector3d(0.0, 0.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 1.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 1.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 0.0, 0.0) + currentAim.Position);

					GL.Vertex3(new Vector3d(1.0, 1.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 1.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 1.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 1.0, 1.0) + currentAim.Position);

					GL.Vertex3(new Vector3d(0.0, 0.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 0.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 0.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 0.0, 1.0) + currentAim.Position);

					GL.Vertex3(new Vector3d(1.0, 0.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 1.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 1.0, 1.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 0.0, 1.0) + currentAim.Position);

					GL.Vertex3(new Vector3d(0.0, 0.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0.0, 1.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 1.0, 0.0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1.0, 0.0, 0.0) + currentAim.Position);
				}
				GL.End();
			}

			if (Constants.Graphics.BlockCursorType == 2 || Constants.Graphics.BlockCursorType == 3)
			{
				GL.Disable(EnableCap.DepthTest);

				GL.Color4(0.0, 0.0, 0.0, 0.2);
				GL.Begin(BeginMode.Lines);
				{
					GL.Vertex3(new Vector3d(0, 0, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 0, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 0, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 0, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 1, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 1, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 1, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 1, 1) + currentAim.Position);

					GL.Vertex3(new Vector3d(0, 0, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 1, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 0, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 1, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 0, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 1, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 0, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 1, 1) + currentAim.Position);

					GL.Vertex3(new Vector3d(0, 0, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 0, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 1, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(0, 1, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 0, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 0, 1) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 1, 0) + currentAim.Position);
					GL.Vertex3(new Vector3d(1, 1, 1) + currentAim.Position);
				}
				GL.End();
				GL.Enable(EnableCap.DepthTest);
			}
		}

		public override void Update(FrameEventArgs e)
		{
			ClockTime.SetTimeOfDayGraphics(e);
			base.Update(e);
		}
	}
}
