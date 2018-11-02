using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HMath
{
    /// <summary>
    /// ::: TEXTORIALS :::
    /// </summary>
    public static class Textorials
    {
        public static void DrawArrow(SpriteBatch sB, Vector2 start, float force, float direction, Color color, float thickness = 1, float layerDepth = LayerDepths.UserInterface)
            => DrawArrow(sB, start, force, direction, force / 10, color, thickness, layerDepth);
        public static void DrawArrow(SpriteBatch sB, Vector2 start, float force, float direction, float width, Color color, float thickness = 1, float layerDepth = LayerDepths.UserInterface)
            => DrawArrow(sB, start, new Vector2(force * (float)Math.Cos(direction), force * (float)Math.Sin(direction)), width, color, thickness, layerDepth);
        public static void DrawArrow(SpriteBatch sB, Vector2 start, Vector2 finish, Color color, float thickness = 1, float layerDepth = LayerDepths.UserInterface)
            => DrawArrow(sB, start, finish, Vector2.Distance(start, finish) / 10, color, thickness, layerDepth);
        public static void DrawArrow(SpriteBatch sB, Vector2 start, Vector2 finish, float width, Color color, float thickness = 1, float layerDepth = LayerDepths.UserInterface)
        {
            DrawLine(sB, start, finish, color, thickness, layerDepth);
            var direction = Funct.Vector2ToAngle(Vector2.Normalize(finish - start));
            var leftPointer = Funct.InvertAngle(direction) + H.π / 4;
            var rightPointer = Funct.InvertAngle(direction) - H.π / 4;
            var leftPointerPos = new Vector2(width * (float)Math.Cos(leftPointer), width * (float)Math.Sin(leftPointer));
            var rightPointerPos = new Vector2(width * (float)Math.Cos(rightPointer), width * (float)Math.Sin(rightPointer));
            DrawLine(sB, finish, finish + leftPointerPos, color, thickness, layerDepth);
            DrawLine(sB, finish, finish + rightPointerPos, color, thickness, layerDepth);
        }

        public static void DrawCross(SpriteBatch sB, Vector2 position, Color color, float layerDepth = LayerDepths.UserInterface)
        {
            sB.Draw(Symbols.White.Cross, position, null, color, 0f, GetTextureCenter(Symbols.White.Cross, Vector2.Zero), 1f, SpriteEffects.None, layerDepth);
        }
        public static void DrawCross(SpriteBatch sB, Vector2 position, float rotation, Color color, float layerDepth = LayerDepths.UserInterface)
        {
            sB.Draw(Symbols.White.Cross, position, null, color, rotation, GetTextureCenter(Symbols.White.Cross, Vector2.Zero), 1f, SpriteEffects.None, layerDepth);
        }

        public static void DrawLine(SpriteBatch sB, Vector2 start, Vector2 finish, Color color, float thickness = 1, float layerDepth = LayerDepths.UserInterface)
        {
            sB.DrawLine(start, finish, color, thickness, layerDepth);
        }

        public static void DrawLineBetweenNodes(SpriteBatch sB, IList<Vector2> nodes, Color color, float thickness = 1, float layerDepth = LayerDepths.UserInterface)
        {
            if (nodes == null || nodes.Count <= 0)
                return;

            for (var i = 0; i < nodes.Count; i++)
            {
                var pIndex = i + 1 >= nodes.Count ? 0 : i + 1;

                var start = nodes[i];
                var end = nodes[pIndex];

                DrawLine(sB, start, end, color, thickness, layerDepth);
            }
        }

        public static void DrawLineThroughNodes(SpriteBatch sB, IList<Vector2> nodes, Color color, float thickness = 1, float layerDepth = LayerDepths.UserInterface)
        {
            if (nodes == null || nodes.Count <= 0)
                return;

            for (var i = 0; i < nodes.Count - 1; i++)
            {
                var start = nodes[i];
                var end = nodes[i + 1];

                DrawLine(sB, start, end, color, thickness, layerDepth);
            }
        }

        public static void DrawO(SpriteBatch sB, Vector2 position, Color color, float layerDepth = LayerDepths.UserInterface)
        {
            sB.Draw(Game1.O, position, null, color, 0f, GetTextureCenter(Game1.O, Vector2.Zero), 1f, SpriteEffects.None, layerDepth);
        }

        public static void DrawTexture(SpriteBatch sB, Texture2D texture, Vector2 position) => DrawTexture(sB, texture, position, 0f, Color.White, LayerDepths.UserInterface);

        public static void DrawTexture(SpriteBatch sB, Texture2D texture, Vector2 position, float rotation, Color color, float layerDepth)
        {
            sB.Draw(texture, position, null, color, 0f, GetTextureCenter(texture, Vector2.Zero), 1f, SpriteEffects.None, layerDepth);
        }

        public static void DrawX(SpriteBatch sB, Vector2 position, Color color, float rotation = 0f, float layerDepth = LayerDepths.UserInterface)
        {
            sB.Draw(Game1.X, position, null, color, rotation, GetTextureCenter(Game1.X, Vector2.Zero), 1f, SpriteEffects.None, layerDepth);
        }

        public static Texture2D GenerateCircleTexture(GraphicsDevice graphicsDevice, int radius, Color color, float sharpness)
        {
            var diameter = radius * 2;
            var circleTexture = new Texture2D(graphicsDevice, diameter, diameter, false, SurfaceFormat.Color);
            var colorData = new Color[circleTexture.Width * circleTexture.Height];
            var center = new Vector2(radius);
            for (var colIndex = 0; colIndex < circleTexture.Width; colIndex++)
            {
                for (var rowIndex = 0; rowIndex < circleTexture.Height; rowIndex++)
                {
                    var position = new Vector2(colIndex, rowIndex);
                    var distance = Vector2.Distance(center, position);

                    // hermite iterpolation
                    var x = distance / diameter;
                    var edge0 = radius * sharpness / diameter;
                    var edge1 = radius / (float)diameter;
                    var temp = MathHelper.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
                    var result = temp * temp * (3.0f - 2f * temp);

                    colorData[rowIndex * circleTexture.Width + colIndex] = color * (1f - result);
                }
            }
            circleTexture.SetData(colorData);

            return circleTexture;
        }

        public static Vector2 GetCenterFront(Texture2D texture, Vector2 position, float rotation)
        {
            return position + texture.Width / 2f * Funct.AngleToVector2(rotation);
        }

        public static Vector2 GetCenterLeft(Texture2D texture, Vector2 position, float rotation)
        {
            return position + texture.Height / 2f * Funct.AngleToVector2(rotation - H.π / 2);
        }

        public static Vector2 GetCenterRear(Texture2D texture, Vector2 position, float rotation)
        {
            return position - texture.Width / 2f * Funct.AngleToVector2(rotation);
        }

        public static Vector2 GetCenterRight(Texture2D texture, Vector2 position, float rotation)
        {
            return position + texture.Height / 2f * Funct.AngleToVector2(rotation + H.π / 2);
        }

        /// <summary>
        /// finds the center point in a texture, calculated in relative world-space
        /// </summary>
        /// <param name="texture">the texture to be examined</param>
        /// <param name="position">the top-left position of the texture (Vector2.Zero for local)</param>
        /// <returns>relative world-space coordinates for the center of the texture</returns>
        public static Vector2 GetTextureCenter(Texture2D texture, Vector2 position)
        {
            // half of the textures length and half it's width places the vector precisely in the middle
            return new Vector2(position.X + texture.Width / 2f, position.Y + texture.Height / 2f);
        }
    }
}