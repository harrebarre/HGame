using Microsoft.Xna.Framework;

namespace HGame.HShapes
{
    public class Line
    {
        public Vector2 Start;

        public Vector2 End;

        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public Line(Vector2 start, float length, Angle direction)
        {
            Start = start;
            End = start + direction.Vector * length;
        }

        public bool Intersects(Line line)
        {
            return Collisions.LineLineIntersection(this, line);
        }

        public override string ToString()
        {
            return "line";
        }
    }
}
