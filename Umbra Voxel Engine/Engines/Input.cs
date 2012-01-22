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

namespace Umbra.Engines
{
    public class Input : Engine
    {

        KeyboardDevice Keyboard { get; set; }
        MouseDevice Mouse { get; set; }
        Point MouseDelta { get; set; }

        public Point MousePosition
        {
            get
            {
                return new Point(Mouse.X, Mouse.Y);
            }
        }

        Dictionary<Key, string> KeyBinds;

        public override void Initialize(EventArgs e)
        {
            Keyboard = Main.Keyboard;
            Mouse = Main.Mouse;

            Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyboardKeyDown);
            Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(KeyboardKeyUp);
            Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(MouseButtonDown);
            Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Constants.Engines.Overlay.MouseButtonDown);

            CenterMouse();

            KeyBinds = new Dictionary<Key, string>();

            KeyBinds.Add(Key.Escape, "exit");
            KeyBinds.Add(Key.Enter, "console_toggle");
            KeyBinds.Add(Key.Q, "noclip");
            KeyBinds.Add(Key.F, "flashlight");

            KeyBinds.Add(Key.W, "+player_move_forward");
            KeyBinds.Add(Key.S, "+player_move_backward");
            KeyBinds.Add(Key.A, "+player_move_left");
            KeyBinds.Add(Key.D, "+player_move_right");
            KeyBinds.Add(Key.Space, "+player_move_jump");

            base.Initialize(e);
        }

        public void SetMouseShow(bool show)
        {
            if (show)
            {
                System.Windows.Forms.Cursor.Show();
            }
            else
            {
                System.Windows.Forms.Cursor.Hide();
            }
        }

        public void CenterMouse()
        {
            System.Windows.Forms.Cursor.Position = Main.PointToScreen(new Point((Main.ClientSize.Width / 2), (Main.ClientSize.Height / 2)));
        }

        void MouseMove()
        {
            MouseDelta = new Point(Mouse.X - (Main.ClientSize.Width / 2), Mouse.Y - (Main.ClientSize.Height / 2));
            CenterMouse();

            Constants.Engines.Physics.Player.FirstPersonCamera.UpdateMouse(MouseDelta);
        }

        void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Main.Focused)
            {
                return;
            }
        }

        void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!Main.Focused)
            {
                return;
            }

            if (Variables.Game.IsActive)
            {
                if (KeyBinds.ContainsKey(e.Key))
                {
                    Console.Execute(KeyBinds[e.Key]);
                }
            }
            else if (Variables.Overlay.Console.IsActive)
            {
                Console.Input(e, Keyboard);
            }
        }

        void KeyboardKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (!Main.Focused)
            {
                return;
            }

            if (Variables.Game.IsActive && KeyBinds.ContainsKey(e.Key) && KeyBinds[e.Key][0] == '+')
            {
                Console.Execute('-' + KeyBinds[e.Key].Substring(1));
            }
        }


        public override void Update(FrameEventArgs e)
        {
            if (Variables.Game.IsActive && Main.Focused)
            {
                MouseMove();
                Constants.Engines.Physics.Player.UpdateMouse(Mouse);
            }

            base.Update(e);
        }
    }
}
