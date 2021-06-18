#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace Hasel
{
    public static class Scoop
    {
        public static KeyboardInput Keyboard { get; private set; }
        public static MouseInput Mouse { get; private set; }

        internal static void Initialise()
        {
            Keyboard = new KeyboardInput();
            Mouse = new MouseInput();
        }

        internal static void Update()
        {
            Keyboard.Update();
            Mouse.Update();
        }

        public class KeyboardInput
        {
            public KeyboardState PreviousState;
            public KeyboardState CurrentState;

            internal void Update()
            {
                PreviousState = CurrentState;
                CurrentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }

            #region Basic Checks
            public bool Check(Keys key)
            {
                return CurrentState.IsKeyDown(key);
            }
            public bool Pressed(Keys key)
            {
                return CurrentState.IsKeyDown(key) && !PreviousState.IsKeyDown(key);
            }

            public bool Released(Keys key)
            {
                return !CurrentState.IsKeyDown(key) && PreviousState.IsKeyDown(key);
            }

            #endregion
            #region Convenience Checks

            public bool Check(Keys keyA, Keys keyB)
            {
                return Check(keyA) || Check(keyB);
            }

            public bool Pressed(Keys keyA, Keys keyB)
            {
                return Pressed(keyA) || Pressed(keyB);
            }

            public bool Released(Keys keyA, Keys keyB)
            {
                return Released(keyA) || Released(keyB);
            }

            public bool Check(Keys keyA, Keys keyB, Keys keyC)
            {
                return Check(keyA) || Check(keyB) || Check(keyC);
            }

            public bool Pressed(Keys keyA, Keys keyB, Keys keyC)
            {
                return Pressed(keyA) || Pressed(keyB) || Pressed(keyC);
            }

            public bool Released(Keys keyA, Keys keyB, Keys keyC)
            {
                return Released(keyA) || Released(keyB) || Released(keyC);
            }
            #endregion
            #region Axis Checks
            public int AxisCheck(Keys NEGATIVE, Keys POSITIVE)
            {
                if (Check(NEGATIVE))
                {
                    if (Check(POSITIVE))
                        return 0;
                    else
                        return -1;
                }
                else if (Check(POSITIVE))
                    return 1;
                else
                    return 0;
            }

            public int AxisCheck(Keys NEGATIVE, Keys POSITIVE, int BOTH)
            {
                if (Check(NEGATIVE))
                {
                    if (Check(POSITIVE))
                        return BOTH;
                    else
                        return -1;
                }
                else if (Check(POSITIVE))
                    return 1;
                else
                    return 0;
            }
            #endregion
        }
        public class MouseInput
        {
            public MouseState PreviousState;
            public MouseState CurrentState;

            internal MouseInput()
            {
                PreviousState = new MouseState();
                CurrentState = new MouseState();
            }
            internal void Update()
            {
                PreviousState = CurrentState;
                CurrentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            }
            #region Basic Button Checks
            public bool CheckLeft
            {
                get { return CurrentState.LeftButton == ButtonState.Pressed; }
            }

            public bool CheckRight
            {
                get { return CurrentState.RightButton == ButtonState.Pressed; }
            }

            public bool CheckMiddle
            {
                get { return CurrentState.MiddleButton == ButtonState.Pressed; }
            }

            public bool PressedLeft
            {
                get { return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released; }
            }

            public bool PressedRight
            {
                get { return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released; }
            }

            public bool PressedMiddle
            {
                get { return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released; }
            }

            public bool ReleasedLeftButton
            {
                get { return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed; }
            }

            public bool ReleasedRightButton
            {
                get { return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed; }
            }

            public bool ReleasedMiddleButton
            {
                get { return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed; }
            }
            #endregion
            #region Wheel
            public int Wheel
            {
                get { return CurrentState.ScrollWheelValue; }
            }

            public int WheelDelta
            {
                get { return CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue; }
            }
            #endregion
            #region Positional
            public float X
            {
                get { return Position.X; }
                set { Position = new Vector2(value, Position.Y); }
            }
            public float Y
            {
                get { return Position.Y; }
                set { Position = new Vector2(Position.X, value); }
            }
            public Vector2 Position
            {
                get
                {
                    return new Vector2(CurrentState.X, CurrentState.Y);
                }
                set
                {
                    Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)Math.Round(value.X), (int)Math.Round(value.Y));
                }
            }
            #endregion
        }
    }
}
 