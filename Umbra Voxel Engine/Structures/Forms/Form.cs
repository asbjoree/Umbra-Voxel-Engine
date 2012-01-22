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
	class Form
	{
		Rectangle FormRectangle;
		Handle DragHandle;
		Handle ResizeHandle;
		bool IsOpen;


		#region - Properties -
		public bool HasFrame { get; set; }
		public bool Resizable { get; set; }
		public bool Dragable { get; set; }
		public Panel Content { get; set; }
		public string Title { get; set; }

		Corner _ResizeHandlePosition;
		public Corner ResizeHandlePosition
		{
			get
			{
				return _ResizeHandlePosition;
			}

			set
			{
				_ResizeHandlePosition = value;
				if (ResizeHandle != null)
				{
					ResizeHandle.HandleRectangle = ResizeHandleFrame;
				}
			}
		}

		Rectangle TopHandleFrame
		{
			get
			{
				if (Dragable || Title != "")
				{
					return new Rectangle(FormRectangle.X + 3, FormRectangle.Y + 3, FormRectangle.Width - 6, 20);
				}
				else
				{
					return Rectangle.Empty;
				}
			}
		}

		Rectangle ResizeHandleFrame
		{
			get
			{
				switch (ResizeHandlePosition)
				{
					case Corner.TopLeft:
					{
						return new Rectangle(FormRectangle.X, FormRectangle.Y, 4, 4);
					}
					case Corner.TopRight:
					{
						return new Rectangle(FormRectangle.X + FormRectangle.Width - 4, FormRectangle.Y, 4, 4);
					}
					case Corner.BottomLeft:
					{
						return new Rectangle(FormRectangle.X, FormRectangle.Y + FormRectangle.Height - 4, 4, 4);
					}
					case Corner.BottomRight:
					{
						return new Rectangle(FormRectangle.X + FormRectangle.Width - 4, FormRectangle.Y + FormRectangle.Height - 4, 4, 4);
					}
					default:
					{
						return Rectangle.Empty;
					}
				}
			}
		}

		Rectangle ClientFrame
		{
			get
			{
				if (HasFrame)
				{
					if (Dragable || Title != "")
					{
						return new Rectangle(FormRectangle.X + 3, FormRectangle.Y + 26, FormRectangle.Width - 6, FormRectangle.Height - 29);
					}
					else
					{
						return new Rectangle(FormRectangle.X + 3, FormRectangle.Y + 3, FormRectangle.Width - 6, FormRectangle.Height - 6);
					}
				}
				else
				{
					return FormRectangle;
				}
			}
		}

		#endregion

		public Form(int x, int y, int width, int height)
		{
			FormRectangle = new Rectangle(x, y, width, height);
			Content = null;

			IsOpen = false;
			HasFrame = true;
			Resizable = true;
			Dragable = true;
			Title = "Form";
			ResizeHandlePosition = Corner.BottomRight;

			ResizeHandle = new Handle(ResizeHandleFrame, false);
		}

		public void Show()
		{
			IsOpen = true;

			if (HasFrame)
			{
				DragHandle = new Handle(TopHandleFrame, true);
			}
			else
			{
				DragHandle = new Handle(FormRectangle, true);
			}
		}

		public void Render()
		{
			if (!IsOpen)
			{
				return;
			}

			if (HasFrame)
			{
				// Main body
				RenderHelp.RenderTexture(Constants.Engines.Overlay.BlankTextureID, FormRectangle, Color.FromArgb(120, Color.Black));

				// Top handle
				RenderHelp.RenderTexture(Constants.Engines.Overlay.BlankTextureID, TopHandleFrame, Color.FromArgb(120, Color.Black));

				SpriteString.Render(Title.Substring(0, Math.Max(0, Math.Min(Title.Length, (FormRectangle.Width - 10) / SpriteString.Measure(" ").X))), new Point(FormRectangle.Location.X + 5, FormRectangle.Location.Y + 5), Color.White);

			}

			if (Resizable)
			{
				// Resize handle
				RenderHelp.RenderTexture(
					Constants.Engines.Overlay.BlankTextureID,
					ResizeHandleFrame,
					Color.FromArgb(120, Color.Black));
			}

			// Client frame
			Content.Render(ClientFrame);

		}

		public void Update()
		{
			if (Resizable && ResizeHandle.Update())
			{
				switch (ResizeHandlePosition)
				{
					case Corner.BottomRight:
					{
						FormRectangle.Size = new Size(
							Math.Max(ResizeHandle.HandleRectangle.Right - FormRectangle.Left, 50),
							Math.Max(ResizeHandle.HandleRectangle.Bottom - FormRectangle.Top, 50)
							);
						break;
					}
					case Corner.BottomLeft:
					{
						FormRectangle.Size = new Size(
							Math.Max(FormRectangle.Right - ResizeHandle.HandleRectangle.Left, 50),
							Math.Max(ResizeHandle.HandleRectangle.Bottom - FormRectangle.Top, 50)
							);
						FormRectangle.X = ResizeHandle.HandleRectangle.X;
						break;
					}
					case Corner.TopRight:
					{
						FormRectangle.Size = new Size(
							ResizeHandle.HandleRectangle.Right - FormRectangle.X,
							FormRectangle.Bottom - ResizeHandle.HandleRectangle.Top
							);
						FormRectangle.Y = ResizeHandle.HandleRectangle.Y;
						break;
					}
					case Corner.TopLeft:
					{
						FormRectangle.Size = new Size(
							FormRectangle.Right - ResizeHandle.HandleRectangle.Left,
							FormRectangle.Bottom - ResizeHandle.HandleRectangle.Top
							);
						FormRectangle.X = ResizeHandle.HandleRectangle.X;
						FormRectangle.Y = ResizeHandle.HandleRectangle.Y;
						break;
					}
				}

				ResizeHandle.HandleRectangle = ResizeHandleFrame;
				DragHandle.HandleRectangle = TopHandleFrame;
			}

			if (Dragable && DragHandle.Update())
			{
				FormRectangle.X = DragHandle.HandleRectangle.X - 3;
				FormRectangle.Y = DragHandle.HandleRectangle.Y - 3;

				ResizeHandle.HandleRectangle = ResizeHandleFrame;

				if (HasFrame)
				{
					DragHandle.HandleRectangle = TopHandleFrame;
				}
				else
				{

					DragHandle.HandleRectangle = FormRectangle;
				}
			}
			Content.Update();
		}
	}

	enum Corner
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}
}
