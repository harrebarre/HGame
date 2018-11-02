using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGame.Colors;
using HGame.HDrawing;
using HGame.HMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace HGame
{
    internal static class Camera
    {
        public static Matrix TranslationMatrix;

        public static Matrix SetTranslationMatrix =>
            Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
            Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));

        public static Matrix TranslationMatrixNoZoom;

        public static Matrix SetNoZoomTranslationMatrix =>
            Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));

        public static Matrix ZoomMatrix => Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));

        static Vector2 _position;
        public static Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public static Vector2 TopLeftPos => ScreenToWorld(new Vector2(0, 0));
        public static float Zoom;
        public static float TargetZoom;
        public static float ZoomMin;
        public static float ZoomMax;
        public static float Rotation;

        private static Viewport _view;
        public static int ViewportWidth => _view.Width;
        public static int ViewportHeight => _view.Height;
        public static int ViewportWorldWidth => (int)Math.Round(_view.Width / Zoom);
        public static int ViewportWorldHeight => (int)Math.Round(_view.Height / Zoom);
        private static Vector2 ViewportCenter => new Vector2(_view.Width / 2, _view.Height / 2);
        private static Vector2 ViewportWorldCenter => new Vector2(ViewportWorldWidth / 2, ViewportWorldHeight / 2);

        public static Rectangle Bounds;
        public static Rectangle SetBounds => new Rectangle((int)Math.Round(Position.X - ViewportWorldCenter.X), (int)Math.Round(Position.Y - ViewportWorldCenter.Y), ViewportWorldWidth, ViewportWorldHeight);

        public static bool InsideBounds(Vector2 subject) => Bounds.Contains(subject);
        public static bool InsideBounds(Rectangle subject) => Bounds.Intersects(subject);

        private static Vector2 GetDir(Vector2 target, Vector2 pos)
        {
            return Vector2.Normalize(target - pos);
        }

        private static float GetSpeed(Vector2 target, Vector2 pos)
        {
            var distanceX = Math.Abs(target.X - pos.X);
            var distanceY = Math.Abs(target.Y - pos.Y);
            var distanceZ = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            if (distanceZ < 1f)
                return 0;
            else
                return MathHelper.Clamp(distanceZ / 10, 1f, float.MaxValue);
        }

        public static void Initialize(Vector2 position, Viewport view)
        {
            Position = position;
            Rotation = 0f;

            Zoom = 1f;
            TargetZoom = Zoom;

            UpdateView(view);

            ZoomMin = 0.5f;
            ZoomMax = 2f;
        }

        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }

        public static void AdjustZoom(float amount)
        {
            TargetZoom += amount;
            if (TargetZoom > ZoomMax)
                TargetZoom = ZoomMax;
            if (TargetZoom < ZoomMin)
                TargetZoom = ZoomMin;

            Zoom = Funct.LinearApproach(Zoom, TargetZoom, 0.01f, 0.01f);

            Position = Position; // CHECK POSITIONS CURRENT VALUE AGAINST ZOOM IF IT'S VALID
        }

        public static void MoveCamera(Vector2 cameraMovement)
        {
            var newPosition = Position + cameraMovement;

            // CLAMP LOGIC HERE

            Position = newPosition;
        }

        public static void UpdatePos(GameTime gT)
        {
            var speed = 4f;
            var key = HKeyboard.KeyState.GetPressedKeys();

            if (key.Contains(Keys.Up))
                MoveCamera(new Vector2(0, -1) * (float)(gT.ElapsedGameTime.TotalSeconds * 60) * speed);
            if (key.Contains(Keys.Down))
                MoveCamera(new Vector2(0, 1) * (float)(gT.ElapsedGameTime.TotalSeconds * 60) * speed);
            if (key.Contains(Keys.Left))
                MoveCamera(new Vector2(-1, 0) * (float)(gT.ElapsedGameTime.TotalSeconds * 60) * speed);
            if (key.Contains(Keys.Right))
                MoveCamera(new Vector2(1, 0) * (float)(gT.ElapsedGameTime.TotalSeconds * 60) * speed);
        }

        public static void UpdateScale(GameTime gT)
        {
            var scrollValue = HMouse.ScrollWheelTransitionValue;

            AdjustZoom(scrollValue / 1200f);
        }

        public static void UpdateView(Viewport view)
        {
            _view = view;
            Bounds = SetBounds;
            TranslationMatrix = SetTranslationMatrix;
            TranslationMatrixNoZoom = SetNoZoomTranslationMatrix;
        }

        public static void Update(GameTime gT, Viewport view)
        {
            UpdatePos(gT);
            UpdateScale(gT);
            UpdateView(view);
        }

        public static void DebugDraw(SpriteBatch sB)
        {
            sB.DrawRectangle(Bounds.ToRectangleF(), HColor.Emerald.RGB, 1f, 1f);
            Textorials.DrawX(sB, TopLeftPos + ViewportWorldCenter, HColor.Clouds.RGB);
            Textorials.DrawArrow(sB, Position, TopLeftPos, Color.Azure);
        }
    }
}
