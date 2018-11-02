using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HShapes
{
    public sealed class Circle : Shape
    {
        public float Radius;
        private List<Vector2> _points;

        /// <summary>
        /// constructor for a circle with a center and a radius
        /// </summary>
        /// <param name="center">center point of the circle</param>
        /// <param name="radius">length to the circles outer edge</param>
        /// <param name="points">number of points the circumference is divided into</param>
        /// <param name="proxRec">proximity rectangle for quick determining if a collision check is necessary</param>
        public Circle(Vector2 center, float radius, int points, Rectangle proxRec, float offset = 0f) : base(proxRec)
        {
            Position = center;
            Radius = radius;
            SetRelativePoints(GeneratePoints(center, radius, points, offset), center);
        }

        public Circle(Vector2 center, float radius, int points, float offset = 0f) : base()
        {
            Position = center;
            Radius = radius;
            SetRelativePoints(GeneratePoints(center, radius, points, offset), center);
            GenerateProximityRectangle();
        }

        public Vector2 Center => Position;

        public List<Vector2> GetPoints => Vectorials.FromLocalPosition(_points.ToList(), Position);

        public override bool CollidesWith(List<Vector2> points)
        {
            //TODO: IMPLEMENT THIS
            //return Collisions.CircleAndPoint(Center, Radius, points);
            return false;
        }

        public override bool Contains(Vector2 point)
        {
            return Collisions.CircleAndPoint(Center, Radius, point);
        }

        public override void Draw(SpriteBatch sB, Color color, float layerDepth)
        {
            if (Points.Count > 0)
            {
                Textorials.DrawLineBetweenNodes(sB, Points, color, 1, layerDepth);
                foreach (var point in Points)
                    Textorials.DrawCross(sB, point, color, layerDepth);
            }
            else
            {
                sB.DrawCircle(Center, Radius, (int)Math.Max(Math.Ceiling(H.π * (Radius / 6)), 8), ColorH.Alizarin.RGB, 1f, 1f);
            }
        }

        public override void GenerateProximityRectangle()
        {
            float x = Center.X - Radius, y = Center.Y - Radius;
            float width = Radius * 2, height = Radius * 2;

            ProxRec = new Rectangle((int)Math.Floor(x), (int)Math.Floor(y), (int)Math.Ceiling(width), (int)Math.Ceiling(height));
        }

        public override string ToString()
        {
            return "circle";
        }

        public override void UpdateShape(float rotDiff)
        {
            if (Math.Abs(rotDiff) > float.Epsilon)
                Vectorials.TranslateAroundPoint(_points, Vector2.Zero, rotDiff);
            Points = GetPoints;
        }

        private static List<Vector2> GeneratePoints(Vector2 center, float radius, int points, float offset)
        {
            var fraction = MathHelper.TwoPi / points;
            var pointsOnCircle = new List<Vector2>();
            for (var i = 0; i < points; i++)
                pointsOnCircle.Add(Geometrics.PointOnCircle(center, radius, fraction * i));

            for (var i = 0; i < pointsOnCircle.Count; i++)
            {
                pointsOnCircle[i] = Vectorials.RotateAroundPoint(pointsOnCircle[i], center, offset);
            }

            return pointsOnCircle;
        }

        private void SetRelativePoints(List<Vector2> points, Vector2 origin)
        {
            _points = Vectorials.ToLocalPosition(points, origin);
            Position = origin;
            Points = GetPoints;
        }
    }
}