using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace HGame.HMath
{
    /// <summary>
    /// ::: GEOMETRICS :::
    /// </summary>
    public static class Geometrics
    {
        /// <summary>
        /// returns an angle from a non-right triangle where the length three sides are known
        /// </summary>
        /// <param name="a">length of first side</param>
        /// <param name="b">length of second side</param>
        /// <param name="c">length of third side</param>
        /// <returns>the angle opposite of A</returns>
        public static float AngleFromSSSTriangle(float a, float b, float c)
        {
            return (float)Math.Acos((b * b + c * c - a * a) / (2 * b * c));
        }

        public static Tuple<int, int> ClosestPointPair(List<Vector2> points1, List<Vector2> points2)
        {
            var closestPair = new Tuple<int, int>(0, 0);
            var lowestDistance = float.MaxValue;

            for (var i = 0; i < points2.Count; i++)
            {
                for (var j = 0; j < points1.Count; j++)
                {
                    var currentPoint = points2[i];
                    var aheadPoint = points1[j];
                    var distance = Vector2.DistanceSquared(currentPoint, aheadPoint);
                    if (distance < lowestDistance)
                        closestPair = new Tuple<int, int>(i, j);
                    distance = distance < lowestDistance ? lowestDistance : distance; // ??
                }
            }

            return closestPair;
        }

        public static void FindCircleCircleIntersections(Vector2 circle1, float radius1, Vector2 circle2, float radius2, out Vector2 a1, out Vector2 a2)
        {
            /*
                  A1
                 /| \
             r1 / |  \ r2
               /  |   \
              /   |h   \
             /g1  |     \          (g1 means angle gamma1)
            C1----P-----C2
               d1   d2
            */
            var dx = circle1.X - circle2.X;
            var dy = circle1.Y - circle2.Y;
            var d = (float)Math.Sqrt(dx * dx + dy * dy); // d = |C1-C2|
            var gamma1 = (float)Math.Acos((radius2 * radius2 + d * d - radius1 * radius1) / (2 * radius2 * d)); // law of cosines
            var d1 = radius1 * (float)Math.Cos(gamma1); // basic math in right triangle
            var h = radius1 * (float)Math.Sin(gamma1);
            var px = circle1.X + (circle2.X - circle1.X) / d * d1;
            var py = circle1.Y + (circle2.Y - circle1.Y) / d * d1;
            // (-dy, dx) / d is (C2-C1) normalized and rotated by 90 degrees
            a1.X = px + (-dy) / d * h;
            a1.Y = py + (+dx) / d * h;
            a2.X = px - (-dy) / d * h;
            a2.Y = py - (+dx) / d * h;
        }

        // Find the points of intersection.
        public static int FindLineCircleIntersections(
            Vector2 circle, float radius,
            Vector2 point1, Vector2 point2,
            out Vector2 intersection1, out Vector2 intersection2)
        {
            float t;

            var dx = point2.X - point1.X;
            var dy = point2.Y - point1.Y;

            var A = dx * dx + dy * dy;
            var B = 2 * (dx * (point1.X - circle.X) + dy * (point1.Y - circle.Y));
            var C = (point1.X - circle.X) * (point1.X - circle.X) +
                      (point1.Y - circle.Y) * (point1.Y - circle.Y) -
                      radius * radius;

            var det = B * B - 4 * A * C;
            if (A <= 0.0000001 || det < 0)
            {
                // no real solutions
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            else if (det == 0)
            {
                // one solution
                t = -B / (2 * A);
                intersection1 =
                    new Vector2(point1.X + t * dx, point1.Y + t * dy);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 1;
            }
            else
            {
                // two solutions
                t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                intersection1 =
                    new Vector2(point1.X + t * dx, point1.Y + t * dy);
                t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                intersection2 =
                    new Vector2(point1.X + t * dx, point1.Y + t * dy);
                return 2;
            }
        }

        public static Vector2 GetShapeCenter(List<Vector2> points)
        {
            var average = points.Aggregate(Vector2.Zero, (current, t) => current + t);

            return average / points.Count;
        }

        public static float Hypothenuse(float a, float b)
                                    => (float)Math.Sqrt(a * a + b * b);

        public static Vector2 PointOnCircle(Vector2 center, float radius, float angle)
        {
            var vector = Vector2.Zero;
            vector.X = center.X + radius * (float)Math.Cos(angle);
            vector.Y = center.Y + radius * (float)Math.Sin(angle);
            return vector;
        }

        public static List<Vector2> RectangleToPoints(Rectangle rect)
        {
            var topLeft = new Vector2(rect.Left, rect.Top);
            var topRight = new Vector2(rect.Right, rect.Top);
            var bottomRight = new Vector2(rect.Right, rect.Bottom);
            var bottomLeft = new Vector2(rect.Left, rect.Bottom);
            return new List<Vector2> { topLeft, topRight, bottomRight, bottomLeft };
        }

        public static float ShapeArea(List<Vector2> points)
        {
            float area = 0;
            for (var i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;

                area += points[i].X * points[j].Y;
                area -= points[i].Y * points[j].X;
            }
            area /= 2;
            return area < 0 ? -area : area;
        }

        /// <summary>
        /// returns a side from a triangle where two sides and one angle are known
        /// </summary>
        /// <param name="sideB">length of side opposite of angle B</param>
        /// <param name="sideC">length of side opposite of angle C</param>
        /// <param name="angleA">angle in radians opposite of side A</param>
        /// <returns>side opposite of angle A</returns>
        public static float SideFromSASTriangle(float sideB, float sideC, float angleA)
        {
            var lengthA = (float)Math.Sqrt(sideB * sideB + sideC * sideC - 2 * (sideB * sideC * Math.Cos(angleA)));
            return lengthA;
        }

        public static float TriangleArea(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var points = new List<Vector2> { p1, p2, p3 };
            float area = 0;
            for (var i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;

                area += points[i].X * points[j].Y;
                area -= points[i].Y * points[j].X;
            }
            area /= 2;
            return area < 0 ? -area : area;
        }
    }
}