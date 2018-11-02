using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HShapes
{
    public sealed class Triangle : Shape
    {
        public float Area;
        private List<Vector2> _points;

        /// <summary>
        /// constructor for a triangle with three points
        /// </summary>
        /// <param name="p1">first edge of the triangle</param>
        /// <param name="p2">second edge of the triangle</param>
        /// <param name="p3">third edge of the triangle</param>
        /// <param name="proxRec">the proximity rectangle for quick determining if distance is close enough to warrant a collision check</param>
        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3, Rectangle proxRec) : base(proxRec)
        {
            _points = new List<Vector2> { p1, p2, p3 };
        }

        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3) : base()
        {
            _points = new List<Vector2> { p1, p2, p3 };
            GenerateProximityRectangle();
        }

        public List<Vector2> GetPoints => Vectorials.FromLocalPosition(_points.ToList(), Position);

        public override bool Contains(Vector2 point)
        {
            return Collisions.QuadAndPoint(Points, point, Area);
        }

        public override void Draw(SpriteBatch sB, Color color, float layerDepth)
        {
            Textorials.DrawLineBetweenNodes(sB, Points, color, 1, layerDepth);
            foreach (var point in Points)
                Textorials.DrawCross(sB, point, color, layerDepth);
        }

        private void SetPoints(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            _points = new List<Vector2> { p1, p2, p3 };
            Position = Vector2.Zero;
            Points = GetPoints;
            Area = Geometrics.ShapeArea(Points);
        }

        private void SetRelativePoints(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 origin)
        {
            _points = Vectorials.ToLocalPosition(new List<Vector2> { p1, p2, p3 }, origin);
            Position = origin;
            Points = GetPoints;
            Area = Geometrics.ShapeArea(_points);
        }

        public override string ToString()
        {
            return "triangle";
        }

        public override void UpdateShape(float rotDiff)
        {
            Vectorials.TranslateAroundPoint(_points, Vector2.Zero, rotDiff);
            Points = GetPoints;
        }
    }
}