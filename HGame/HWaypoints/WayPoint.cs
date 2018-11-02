using System;
using HGame.Colors;
using HGame.HMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HWaypoints
{
    internal class Waypoint
    {
        public static Waypoint Empty => new Waypoint();

        private Color _color;
        public Color Color { get => Traversable ? _color : HColor.Mixer(_color, new Color(255, 155, 155)); set => _color = value; }

        public Vector2 Position;

        public Rectangle ColRect;

        public bool Traversable;

        public Waypoint()
        {
        }

        public Waypoint(Vector2 position, int spacing, Color color)
        {
            Position = position;
            ColRect = new Rectangle((int)position.X - (int)Math.Round((double)spacing / 2), (int)position.Y - (int)Math.Round((double)spacing / 2), spacing, spacing);
            Traversable = true;
            Color = color;
        }

        public void DrawBoundingBox(SpriteBatch sB)
        {
            var colour = Color.ToVector3() * 255;
            var opacity = 0.4f;
            Textorials.DrawLineBetweenNodes(sB, Geometrics.RectangleToPoints(ColRect), new Color((int)(colour.X * opacity), (int)(colour.Y * opacity), (int)(colour.Z * opacity)), 1, LayerDepths.Floor);
        }

        public void Draw(SpriteBatch sB, Texture2D texture)
        {
            sB.Draw(texture, Position, null, Color, 0f, Textorials.GetTextureCenter(texture, Vector2.Zero), 1f, SpriteEffects.None, LayerDepths.Floor);
        }
    }
}