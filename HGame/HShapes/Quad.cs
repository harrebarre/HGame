using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HShapes
{
    public sealed class Quad : Shape
    {
        public float Area;
        private List<Vector2> _points;

        /// <summary>
        /// constructor for a quadrilateral with a starting point, a width and a height
        /// </summary>
        /// <param name="p1">first edge of the quad</param>
        /// <param name="p2">second edge of the quad</param>
        /// <param name="p3">third edge of the quad</param>
        /// <param name="p4">fourth edge of the quad</param>
        /// <param name="proxRec">proximity rectangle for quick determining if a collision check is necessary</param>
        public Quad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Rectangle proxRec) : base(proxRec)
        {
            SetPoints(p1, p2, p3, p4);
        }

        public Quad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 origin, Rectangle proxRec) : base(proxRec)
        {
            SetRelativePoints(p1, p2, p3, p4, origin);
        }

        public Quad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) : base()
        {
            SetPoints(p1, p2, p3, p4);
            GenerateProximityRectangle();
        }

        public Quad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 origin) : base()
        {
            SetRelativePoints(p1, p2, p3, p4, origin);
            GenerateProximityRectangle();
        }

        public List<Vector2> GetPoints => Vectorials.FromLocalPosition(_points.ToList(), Position);

        public override bool CollidesWith(List<Vector2> points)
        {
            return Collisions.QuadAndPoints(Points, points);
        }

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

        private void SetPoints(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            _points = new List<Vector2> { p1, p2, p3, p4 };
            Position = Vector2.Zero;
            Points = GetPoints;
            Area = Geometrics.ShapeArea(_points);
        }

        private void SetRelativePoints(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 origin)
        {
            _points = Vectorials.ToLocalPosition(new List<Vector2> { p1, p2, p3, p4 }, origin);
            Position = origin;
            Points = GetPoints;
            Area = Geometrics.ShapeArea(_points);
        }

        public override string ToString()
        {
            return "quad";
        }

        public override void UpdateShape(float rotDiff)
        {
            Vectorials.TranslateAroundPoint(_points, Vector2.Zero, rotDiff);
            Points = GetPoints;
        }
    }
}