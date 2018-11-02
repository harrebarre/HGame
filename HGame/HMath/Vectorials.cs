using System;
using System.Collections.Generic;
using System.Linq;
using HGame.HMath.MathObjects;
using HGame.HWaypoints;
using Microsoft.Xna.Framework;

namespace HGame.HMath
{
    /// <summary>
    /// ::: VECTORIALS :::
    /// </summary>
    public static class Vectorials
    {
        public static bool Adjacent(Vector2 v1, Vector2 v2)
        {
            var distX = Math.Abs(v1.X - v2.X);
            var distY = Math.Abs(v1.Y - v2.Y);
            return distX <= 1 && distY <= 1 && (Math.Abs(distX) > float.Epsilon || Math.Abs(distY) > float.Epsilon);
        }

        public static bool CheckBresenhamLine(Vector2 v1, Vector2 v2) => CheckBresenhamLine(v1.ToPoint(), v2.ToPoint());

        public static bool CheckBresenhamLine(Point p1, Point p2)
        {
            var swapXY = Math.Abs(p2.Y - p1.Y) > Math.Abs(p2.X - p1.X);
            if (swapXY)
            {
                // swap x and y
                var tmp = p1.X; p1.X = p1.Y; p1.Y = tmp; // swap p1.X and p1.Y
                tmp = p2.X; p2.X = p2.Y; p2.Y = tmp; // swap p2.X and p2.Y
            }

            if (p1.X > p2.X)
            {
                // make sure p1.X < p2.X
                var tmp = p1.X; p1.X = p2.X; p2.X = tmp; // swap p1.X and p2.X
                tmp = p1.Y; p1.Y = p2.Y; p2.Y = tmp; // swap p1.Y and p2.Y
            }

            var deltax = p2.X - p1.X;
            var deltay = Math.Floor(Math.Abs((double)(p2.Y - p1.Y)));
            var error = Math.Floor((double)deltax / 2);
            var y = p1.Y;
            var ystep = p1.Y < p2.Y ? 1 : -1;

            for (var x = p1.X; x < p2.X + 1; x++)
            {
                var point = swapXY ? new Point(y, x) : new Point(x, y);

                if (!WaypointHolder.IsPointTraversable(point))
                    return false;

                error -= deltay;
                if (error < 0)
                {
                    y = y + ystep;
                    error = error + deltax;
                }
            }

            return true;
        }

        public static Vector2 ClockwiseDirectionFromVectorRelativeToPoint(Vector2 vector, Vector2 point)
        {
            var angle = (float)Math.Atan2(vector.Y - point.Y, vector.X - point.X);
            return Funct.AngleToVector2(MathHelper.WrapAngle(angle - H.π / 2));
        }

        /// <summary>
        /// condeses vectors to a set distance apart on an assumed local position relative to origin
        /// </summary>
        /// <param name="vectors">localized vectors</param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<Vector2> CondenseVectors(List<Vector2> vectors, float radius) =>
            CondenseVectors(vectors, Enumerable.Repeat(radius, vectors.Count).ToList());

        /// <summary>
        /// condeses vectors to a set distance apart on an assumed local position relative to origin
        /// </summary>
        /// <param name="inputVectors">localized vectors</param>
        /// <param name="radii"></param>
        /// <returns></returns>
        public static List<Vector2> CondenseVectors(List<Vector2> inputVectors, List<float> radii)
        {
            var vectors = inputVectors.ToList(); // this method accidentally altered the vectors in the input list, since a list is a reference, oops
            (int Index, float Distance) closestVector = (0, float.MaxValue);

            for (var i = 0; i < vectors.Count; i++) // find the vector closest to the middle aka the origin
            {
                var distanceFromOrigin = vectors[i].Length();
                if (distanceFromOrigin < closestVector.Distance)
                    closestVector = (i, distanceFromOrigin);
            }

            vectors[closestVector.Index] = Vector2.Zero; // the vector closest to the origin becomes the origin
            var originIndex = closestVector.Index;

            for (var i = 0; i < vectors.Count; i++) // place all vectors on the radius of origin
            {
                if (i == originIndex)
                    continue;

                var angleFromOrigin = (float)Math.Atan2(vectors[i].Y, vectors[i].X);
                var radius = radii[i] > radii[originIndex] ? radii[i] : radii[originIndex];
                vectors[i] = Geometrics.PointOnCircle(Vector2.Zero, radius, angleFromOrigin);
            }

            // ALL THE DIFFERENT SCENARIOS
            // 1: IF(this vector inside another vectors circle AND that vectors circle intersects the origin vector circle) place the vector where the other vector it's inside and the origin vectors radii meet

            var continueLoop = true;
            const int MAX_LOOPS = 600;
            var loops = 0;

            //TODO: Optimize this
            while (continueLoop)
            {
                if (loops >= MAX_LOOPS)
                {
                    System.Diagnostics.Debug.WriteLine("THE VECTOR CONDENSER IS EXPERIENCING HEAVY LOAD");
                    break;
                }
                continueLoop = false;

                for (var i = 0; i < vectors.Count; i++) // check all vectors to see if they're closer to eachother than the origin, if so move them away
                {
                    if (i == originIndex) // don't do this for the vector assigned as the origin point
                        continue;

                    var intersections = new List<int>();

                    for (var j = 0; j < vectors.Count; j++) // check this vector (i) against all other vectors (j)
                    {
                        if (j == originIndex || j == i)
                            continue;

                        var radius = radii[i] > radii[j] ? radii[i] : radii[j];

                        if (Vector2.DistanceSquared(vectors[i], vectors[j]) < radius * radius) // if they intersect (the distance between them become smaller than the radius)
                        {
                            intersections.Add(j);
                        }
                    }

                    if (intersections.Count > 0) // continue looping until no vector has any intersections
                        continueLoop = true;

                    foreach (var intersecting in intersections) // check against all intersections
                    {
                        if (!(Vector2.DistanceSquared(vectors[i], vectors[intersecting]) < (radii[i] > radii[intersecting] ? radii[i] * radii[i] : radii[intersecting] * radii[intersecting]))) // check to see they're still intersecting, if not, continue
                            continue;

                        //TODO: THE CIRCLE ONE IS BROKEN
                        //if (Vector2.Distance(vectors[originIndex], vectors[intersecting]) < (radii[originIndex] > radii[intersecting] ? radii[originIndex] : radii[intersecting]) * 2) // if the intersecting vectors circle intersects the origin circle
                        //{
                        //    Geometrics.FindCircleCircleIntersections(vectors[originIndex], radii[originIndex], vectors[intersecting], radii[intersecting], out var intersectingPoint1, out var intersectingPoint2);

                        //    var closestIntersectingPoint =
                        //        Vector2.Distance(vectors[originIndex], intersectingPoint1) < radii[originIndex] // if the first intersecting point intersects with the origin point
                        //            ? intersectingPoint2 :
                        //        Vector2.Distance(vectors[i], intersectingPoint1) < Vector2.Distance(vectors[i], intersectingPoint2) // the the point closest to this vector (i)
                        //            ? intersectingPoint1
                        //            : intersectingPoint2;

                        //    vectors[i] = closestIntersectingPoint;
                        //}
                        //else // draw a line through this vector (i) from the origin, and place this vector on the closest intersection point that isn't inside the origins circle
                        {
                            var extraDistance = radii[i] + radii[intersecting];
                            var directionFromOrigin = Vector2.Normalize(vectors[i]);
                            var endPoint = vectors[i] + directionFromOrigin * extraDistance;

                            var result = Geometrics.FindLineCircleIntersections(vectors[intersecting], radii[intersecting], vectors[originIndex],
                                endPoint, out var intersectingPoint1, out var intersectingPoint2);

                            switch (result) // the number of possible intersection points
                            {
                                case 0: // these are no longer considered to intersect, due to a change made to either one of them during this operation loop
                                    continue;
                                case 1:
                                    vectors[i] = intersectingPoint1;
                                    break;
                                case 2:
                                    var closestIntersectingPoint =
                                        Vector2.DistanceSquared(vectors[originIndex], intersectingPoint1) < radii[originIndex] * radii[originIndex] // if the first intersecting point intersects with the origin point
                                            ? intersectingPoint2 :
                                        Vector2.DistanceSquared(vectors[i], intersectingPoint1) < Vector2.DistanceSquared(vectors[i], intersectingPoint2) // take the closest one TODO: TAKE THE CLOSEST ONE TO THE ORIGIN (that doesn't intersect), NOT THE POINT(i)!
                                            ? intersectingPoint1
                                            : intersectingPoint2;

                                    vectors[i] = closestIntersectingPoint;
                                    break;
                            }
                        }
                    }
                }
                loops++;
            }

            return vectors;
        }

        public static Vector2 CounterClockwiseDirectionFromVectorRelativeToPoint(Vector2 vector, Vector2 point)
        {
            var angle = (float)Math.Atan2(vector.Y - point.Y, vector.X - point.X);
            return Funct.AngleToVector2(MathHelper.WrapAngle(angle + H.π / 2));
        }

        public static float Cross(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public static (Vector2 Direction, bool Is) Diagonal(Vector2 v1, Vector2 v2)
        {
            var composite = new Vector2(Math.Abs(v1.X) - Math.Abs(v2.X), Math.Abs(v1.Y) - Math.Abs(v2.Y));

            if (composite == Vector2.Zero)
                return (Vector2.Zero, true);

            if (Math.Abs(Math.Abs(composite.X) - Math.Abs(composite.Y)) < float.Epsilon)
                return (Vector2.Normalize(v2 - v1), true);

            return (Vector2.Zero, false);
        }

        public static Vector2 SafeNormalize(Vector2 v)
        {
            if (v == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(v);
        }

        public static Vector2 SafeNormalize(Vector2 v1, Vector2 v2)
        {
            if (v1 == v2)
                return Vector2.Zero;
            else
                return Vector2.Normalize(v2 - v1);
        }

        public static Vector3 SafeNormalize(Vector3 v)
        {
            if (v == Vector3.Zero)
                return Vector3.Zero;
            else
                return Vector3.Normalize(v);
        }

        public static Vector3 SafeNormalize(Vector3 v1, Vector3 v2)
        {
            if (v1 == v2)
                return Vector3.Zero;
            else
                return Vector3.Normalize(v2 - v1);
        }

        /// <summary>
        /// calculates the distance between two vectors in world-space
        /// </summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        /// <returns>the distance between v1 and v2</returns>
        public static float DistanceBetweenVectors(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y).Length();
        }

        /// <summary>
        /// returns an estimate of the distance between two points |(H) value|
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float Euclidean(Point p1, Point p2) // thing moving in constant distance in any direction on a plane (any-angle movement)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        public static List<Point> FilterRedundantOctilinearLinePoints(List<Point> points) =>
            VectorListToPointList(FilterRedundantOctilinearLinePoints(PointListToVectorList(points)));

        public static List<Vector2> FilterRedundantOctilinearLinePoints(List<Vector2> vectors)
        {
            for (var i = 0; i < vectors.Count - 1; i++)
            {
                var p = vectors[i];
                var watchList = new List<Vector2>();

                bool loop;
                var j = 1;
                do
                {
                    loop = false;
                    var pNext = vectors[i + j];
                    var octilinear = Octilinear(p, pNext);
                    if (octilinear.Is)
                    {
                        loop = true;
                        watchList.Add(vectors[i + j]);
                        j++;
                    }
                    if (i + j >= vectors.Count - 1)
                        loop = false;
                } while (loop);

                for (var l = 0; l < watchList.Count - 1; l++)
                    vectors.Remove(watchList[l]);
            }

            return vectors;
        }

        public static List<Vector2> FromLocalPosition(List<Vector2> points, Vector2 origin)
        {
            for (var i = 0; i < points.Count; i++)
                points[i] += origin;
            return points;
        }

        public static Angle GetAngle(Vector2 v1, Vector2 v2)
        {
            return new Angle(Funct.Vector2ToAngle(SafeNormalize(v1, v2)));

            return new Angle((float)Math.Atan2(v2.Y - v1.Y, v2.X - v1.X));
        }

        public static Angle GetAngle(Vector2 v1, Vector2 v2, Vector2 v3) //OBS DON'T WORK, ALWAYS ANGLE 0
        {
            var v1X = v1.X - v3.X;
            var v1Y = v1.Y - v3.Y;
            var v2X = v2.X - v3.X;
            var v2Y = v2.Y - v3.Y;

            return new Angle((float)(Math.Atan2(v1X, v1Y) - Math.Atan2(v2X, v2Y)));

            //var p1 = v1 - v2;
            //var p2 = v3 - v2;
            //var angle = -(180 / H.π) * Math.Atan2(p1.X * p2.Y - p1.Y * p2.X, p1.X * p2.X + p1.Y * p2.Y);
            //return new Angle((float)angle);
        }

        public static Path GetBresenhamLine(Vector2 v1, Vector2 v2) => GetBresenhamLine(v1.ToPoint(), v2.ToPoint());

        public static Path GetBresenhamLine(Point p1, Point p2)
        {
            var pts = new List<Point>();

            var swapXY = Math.Abs(p2.Y - p1.Y) > Math.Abs(p2.X - p1.X);
            if (swapXY)
            {
                // swap x and y
                var temp = p1.X; p1.X = p1.Y; p1.Y = temp; // swap p1.X and p1.Y
                temp = p2.X; p2.X = p2.Y; p2.Y = temp; // swap p2.X and p2.Y
            }

            var flip = false;
            if (p1.X > p2.X)
            {
                // make sure p1.X < p2.X
                var temp = p1.X; p1.X = p2.X; p2.X = temp; // swap p1.X and p2.X
                temp = p1.Y; p1.Y = p2.Y; p2.Y = temp; // swap p1.Y and p2.Y
                flip = true;
            }

            var deltaX = p2.X - p1.X;
            var deltaY = Math.Floor(Math.Abs((double)(p2.Y - p1.Y)));
            var error = Math.Floor((double)deltaX / 2);
            var y = p1.Y;
            var ystep = p1.Y < p2.Y ? 1 : -1;

            for (var x = p1.X; x < p2.X + 1; x++)
            {
                pts.Add(swapXY ? new Point(y, x) : new Point(x, y));
                error -= deltaY;
                if (error < 0)
                {
                    y = y + ystep;
                    error = error + deltaX;
                }
            }

            if (flip)
                pts.Reverse();

            return new Path(pts);
        }

        public static Vector2 GetVectorAverage(IEnumerable<Vector2> vectors)
        {
            var average = Vector2.Zero;
            var count = 0;
            using (var enumerator = vectors.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    average += current;
                    count++;
                }
            }
            return average / count;
        }

        /// <summary>
        /// calculates the distance between all vectors in a list, in rising order
        /// </summary>
        /// <param name="nodes">the vector nodes</param>
        /// <returns>the length of a straight line through all vectors</returns>
        public static float NodeSequenceLength(List<Vector2> nodes)
        {
            var distance = 0f;
            for (var i = 0; i < nodes.Count - 1; i++)
                distance += DistanceBetweenVectors(nodes[i], nodes[i + 1]);
            return distance;
        }

        public static (Vector2 Direction, bool Is) Octilinear(Vector2 v1, Vector2 v2)
        {
            var ifOrthogonal = Orthogonal(v1, v2);
            var ifDiagonal = Diagonal(v1, v2);

            return ifOrthogonal.Is ? ifOrthogonal : ifDiagonal.Is ? ifDiagonal : (Vector2.Zero, false);
        }

        public static (Vector2 Direction, bool Is) Orthogonal(Vector2 v1, Vector2 v2)
        {
            if (Math.Abs(v1.X - v2.X) < float.Epsilon && Math.Abs(v1.Y - v2.Y) < float.Epsilon)
                return (Vector2.Zero, true);

            if (Math.Abs(v1.X - v2.X) < float.Epsilon || Math.Abs(v1.Y - v2.Y) < float.Epsilon)
                return (Vector2.Normalize(v2 - v1), true);

            return (Vector2.Zero, false);
        }

        public static List<Vector2> PointListToVectorList(List<Point> points)
        {
            var vectors = new List<Vector2>();

            foreach (var point in points)
            {
                var vector = point.ToVector2();
                vectors.Add(vector);
            }

            return vectors;
        }

        public static Vector2 RandomDislocate(Vector2 vector, float magnitude)
        {
            return vector + Angle.NewRandom.Vector * magnitude;
        }

        public static Vector2 PlaceAroundPoint(Vector2 vector, Vector2 point, float angle)
        {
            var radius = Vector2.Distance(vector, point);
            vector.X = point.X + radius * (float)Math.Cos(angle);
            vector.Y = point.Y + radius * (float)Math.Sin(angle);
            return vector;
        }

        public static Vector2 RotateAroundPoint(Vector2 vector, Vector2 point, float incrementAngle)
        {
            var radius = Vector2.Distance(vector, point);
            var currentAngle = (float)Math.Atan2(vector.Y - point.Y, vector.X - point.X);
            vector.X = point.X + radius * (float)Math.Cos(currentAngle + incrementAngle);
            vector.Y = point.Y + radius * (float)Math.Sin(currentAngle + incrementAngle);
            return vector;
        }

        /// <summary>
        /// snaps a direction vector to the closest of two vectors
        /// </summary>
        /// <param name="v">the direction vector</param>
        /// <param name="snap1">the first snap position</param>
        /// <param name="snap2">the second snap position</param>
        /// <returns>the closest between snap1 and snap2</returns>
        public static Vector2 SnapVector(Vector2 v, Vector2 snap1, Vector2 snap2)
        {
            return Vector2.Distance(v, snap1) < Vector2.Distance(v, snap2) ? snap1 : snap2;
        }

        public static List<Vector2> ToLocalPosition(List<Vector2> points, Vector2 origin)
        {
            for (var i = 0; i < points.Count; i++)
                points[i] -= origin;
            return points;
        }

        public static Vector2 ToLocalPosition(this Vector2 point, Vector2 origin)
        {
            return point - origin;
        }

        public static List<Vector2> TranslateAroundPoint(List<Vector2> points, Vector2 origin, float angle)
        {
            for (var i = 0; i < points.Count; i++)
            {
                points[i] = Vector2.Transform(points[i], Matrix.CreateRotationZ(angle));
                points[i] += origin;
            }
            return points;
        }

        public static Point Vector2ToPoint(Vector2 v)
        {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }

        public static List<Point> VectorListToPointList(List<Vector2> vectors)
        {
            var points = new List<Point>();

            foreach (var vector in vectors)
            {
                var point = vector.ToPoint();
                points.Add(point);
            }

            return points;
        }
    }
}