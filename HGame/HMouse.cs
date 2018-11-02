using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGame.Colors;
using HGame.HMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HGame
{
    internal static class HMouse
    {
        private static Texture2D _tint;
        private static Texture2D _tintFlip;

        public static MouseState MouseState;
        private static MouseState _oldMouseState;

        public static int X;
        public static int Y;

        public static Vector2 ScreenPos => new Vector2(X, Y);
        public static Vector2 Position => ScreenPos;//Camera.ScreenToWorld(ScreenPos);

        public static int ScrollWheelValue => MouseState.ScrollWheelValue;
        public static int LastScrollWheelValue;
        public static int ScrollWheelTransitionValue;

        public static bool LeftClick => MouseState.LeftButton == ButtonState.Pressed;
        public static Vector2 LeftClickPos;
        public static Vector2 LeftClickScreenPos;

        public static bool RightClick => MouseState.RightButton == ButtonState.Pressed;
        public static Vector2 RightClickPos;
        public static Vector2 RightClickScreenPos;

        public static bool ForwardClick => MouseState.XButton1 == ButtonState.Pressed;
        public static bool BackwardClick => MouseState.XButton2 == ButtonState.Pressed;

        public static Rectangle Selection;
        public static Rectangle ScreenSelection;

        private static void SetPositon(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        private static void UpdatePosition()
        {
            X = MouseState.Position.X;
            Y = MouseState.Position.Y;
        }

        public static void LoadTextures(Texture2D tint, Texture2D tintFlip)
        {
            _tint = tint;
            _tintFlip = tintFlip;
        }

        private static void MouseHover()
        {
        }

        #region LeftMouseButton

        private static void LeftClicked()
        {
            LeftClickPos = Position; // remember where the click happened
            LeftClickScreenPos = ScreenPos;
        }

        private static void LeftHeld()
        {
            if (Vector2.Distance(LeftClickPos, Position) > 20) // if the mouse moved X units after being clicked
            {
                var distX = (int)Vector2.Distance(new Vector2(LeftClickPos.X, 0), new Vector2(Position.X, 0));
                var distY = (int)Vector2.Distance(new Vector2(0, LeftClickPos.Y), new Vector2(0, Position.Y));
                var top = Position.Y < LeftClickPos.Y ? (int)Position.Y : (int)LeftClickPos.Y;
                var left = Position.X < LeftClickPos.X ? (int)Position.X : (int)LeftClickPos.X;
                Selection = new Rectangle(left, top, distX, distY);

                //ScreenSelection = new Rectangle(Camera.WorldToScreen(new Vector2(left, top)).ToPoint(), new Point((int)Math.Round(distX * Camera.Zoom), (int)Math.Round(distY * Camera.Zoom)));
                ScreenSelection = new Rectangle(new Vector2(left, top).ToPoint(), new Point((int)Math.Round(distX * Camera.Zoom), (int)Math.Round(distY * Camera.Zoom)));
            }
        }

        private static void LeftReleased()
        {
            if (Selection != Rectangle.Empty) // if a selection was made
            {
                // do something
            }

            Selection = Rectangle.Empty;
        }

        #endregion LeftMouseButton

        #region RightMouseButton

        private static void RightClicked()
        {
            RightClickPos = Position; // remember where the click happened
            RightClickScreenPos = ScreenPos;
        }

        private static void RightHeld()
        {

        }

        private static void RightReleased()
        {
        }

        #endregion RightMouseButton

        public static void Update(MouseState mouseState)
        {
            MouseState = mouseState;

            UpdatePosition();

            MouseHover();

            if (MouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released) // DO ON FIRST CLICK
                LeftClicked();

            if (LeftClick) // while it's being held
                LeftHeld();

            if (_oldMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released) // do on release
                LeftReleased();

            if (MouseState.RightButton == ButtonState.Pressed && _oldMouseState.RightButton == ButtonState.Released) // do on FIRST click
                RightClicked();

            if (RightClick) // while it's being held
                RightHeld();

            if (_oldMouseState.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released) // do on release
                RightReleased();

            ScrollWheelTransitionValue = ScrollWheelValue - LastScrollWheelValue;
            LastScrollWheelValue = ScrollWheelValue;
            _oldMouseState = MouseState; // remember until next frame
        }

        private static void DrawSelectionLines(SpriteBatch sB)
        {
            var nodes = Geometrics.RectangleToPoints(ScreenSelection);

            Textorials.DrawLineBetweenNodes(sB, nodes, HColor.Pomegranate.RGB);
        }

        private static void DrawSelectionBox(SpriteBatch sB)
        {
            var tex = ScreenPos.X < LeftClickScreenPos.X && ScreenPos.Y > LeftClickScreenPos.Y || ScreenPos.Y < LeftClickScreenPos.Y && ScreenPos.X > LeftClickScreenPos.X ? _tint : _tintFlip;

            sB.Draw(tex, new Rectangle(ScreenSelection.Left + 1, ScreenSelection.Top + 1, ScreenSelection.Width - 2, ScreenSelection.Height - 2),
                         new Rectangle(ScreenSelection.Left + 1, ScreenSelection.Top + 1, ScreenSelection.Width - 2, ScreenSelection.Height - 2),
                    HColor.Pomegranate.RGB, 0f, Vector2.Zero, SpriteEffects.None, LayerDepths.UserInterface);
        }

        public static void Draw(SpriteBatch sB)
        {
            if (!Selection.IsEmpty)
            {
                if (Selection.Width != 0 && Selection.Height != 0)
                    DrawSelectionBox(sB);
                DrawSelectionLines(sB);
            }
        }
    }
}
