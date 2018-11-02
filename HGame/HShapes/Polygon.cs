using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HShapes
{
    internal sealed class Polygon : Shape
    {
        public List<Line> Lines;

        public Polygon(List<Vector2> points)
        {
            Lines = new List<Line>();
            ChangeShape(points);
        }

        public Polygon(List<Line> lines)
        {
            Lines = lines;
            ChangeShape(lines);
        }

        public override bool Contains(Vector2 point)
        {
            return Collisions.PolygonAndPoint(this, point);
        }

        public override bool CollidesWith(List<Vector2> points)
        {
            return base.CollidesWith(points);
        }

        public override void ChangeShape(List<Vector2> points)
        {
            Points = points;
            for (var i = 0; i < points.Count; i++) // assumes that lines are correctly in list order
            {
                var l = Funct.WrapInt(i + 1, 0, points.Count);

                var line = new Line(points[i], points[l]);
                Lines.Add(line);
            }
            GenerateProximityRectangle();
        }

        public void ChangeShape(List<Line> lines)
        {
            Points = new List<Vector2>();
            foreach (var line in lines)
            {
                Points.Add(line.Start);
                Points.Add(line.End);
            }
            Points = Points.Distinct().ToList();
            GenerateProximityRectangle();
        }

        public override void Draw(SpriteBatch sB, Color color, float layerDepth)
        {
            foreach (var point in Points)
            {
                Textorials.DrawCross(sB, point, Color.Blue);
            }
        }

        public override void Update()
        {
            Position = Vectorials.GetVectorAverage(Points);
            base.Update();
        }

        public override void UpdateShape(float rotDiff)
        {
            Position = Vectorials.GetVectorAverage(Points);
            base.UpdateShape(rotDiff);
        }

        public override string ToString()
        {
            return "polygon";
        }
    }
}
