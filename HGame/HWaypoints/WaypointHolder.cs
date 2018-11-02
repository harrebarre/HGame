using System;
using System.Collections.Generic;
using System.Linq;
using HGame.Colors;
using HGame.HMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HWaypoints
{
    internal class WaypointHolder
    {
        public const int Height = 8000;
        public const int Spacing = 20;
        public const float DiagonalSpacing = 20f * 1.4142f;
        public const int Width = 8000;
        public static Waypoint[,] Waypoints;
        public static Waypoint[,] WaypointsInView;
        public static int X;
        public static int Y;
        private static Random _rand;
        private static Texture2D _waypointTex;
        private static Point CamPointPos;
        private static Point CamTopLeftPointPos;
        private static Point OldCamPointPos;
        private static Point OldCamTopLeftPointPos;
        private static (int X, int Y) WaypointsOnScreen;
        private static Color RandomizeColor => new Color(_rand.Next(HColor.Envato.R, HColor.Envato.R + 6), _rand.Next(HColor.Envato.G, HColor.Envato.G + 6), _rand.Next(HColor.Envato.B, HColor.Envato.B + 6));

        public static void Draw(SpriteBatch sB)
        {
            DrawWaypoints(sB);
        }

        public static void Initialize()
        {
            _rand = new Random();

            X = Width / Spacing;
            Y = Height / Spacing;
            Waypoints = new Waypoint[X, Y];
            for (var x = 0; x < X; x++)
                for (var y = 0; y < Y; y++)
                    Waypoints[x, y] = new Waypoint(new Vector2(x * Spacing, y * Spacing), Spacing, RandomizeColor);

            UpdateWaypointsInView();
        }

        public static void LoadTextures(Texture2D wayPointTex)
        {
            _waypointTex = wayPointTex;
        }

        public static void SetWaypointsInView()
        {
            WaypointsOnScreen = ((int)Math.Ceiling((float)Camera.ViewportWorldWidth / Spacing), (int)Math.Ceiling((float)Camera.ViewportWorldHeight / Spacing));

            WaypointsInView = Funct.BoxSelect(Waypoints, CamTopLeftPointPos.X, CamTopLeftPointPos.Y, CamTopLeftPointPos.X + WaypointsOnScreen.X, CamTopLeftPointPos.Y + WaypointsOnScreen.Y);
        }

        public static void Update(GameTime gT)
        {
            UpdateWaypointsInView();
        }

        private static void DrawWaypoints(SpriteBatch sB) // DON'T USE, NOT OPTIMIZED, DRAWS E V E R Y T H I N G ! ! !
        {
            for (var x = 0; x < X; x++)
                for (var y = 0; y < Y; y++)
                    Waypoints[x, y].Draw(sB, _waypointTex);

            for (var x = 0; x < X; x++)
                for (var y = 0; y < Y; y++)
                    Waypoints[x, y].DrawBoundingBox(sB);
        }

        private static Color GetWaypointColor(Waypoint wayPoint)
        {
            return wayPoint.Traversable ? HColor.Leaf.RGB : HColor.INNOVATE.RGB;
        }

        private static void IsWaypointInView(Waypoint waypoint)
        {
        }

        #region HelperMethods

        public static Vector2 ClampVectorToPoint(Vector2 vector)
        {
            return GetPointAtVector(vector).ToVector2() * Spacing;
        }

        public static Vector2 GetFloatingPointAtVector(Vector2 vector)
        {
            var x = vector.X / Spacing;
            var y = vector.Y / Spacing;
            return new Vector2(x, y);
        }

        public static List<Point> GetPointsAtVectors(List<Vector2> vectors)
        {
            var points = new List<Point>();

            foreach (var vector in vectors)
                points.Add(GetPointAtVector(vector));

            return points;
        }

        public static Point GetPointAtVector(Vector2 vector)
        {
            var x = (int)Math.Round(vector.X / Spacing);
            var y = (int)Math.Round(vector.Y / Spacing);
            return new Point(x, y);
        }

        public static Waypoint GetWaypointAtPoint(Point point)
        {
            //TODO: A better solution is probably required.
            var x = Math.Min(Math.Max(point.X, 0), Waypoints.GetLength(0) - 1);
            var y = Math.Min(Math.Max(point.Y, 0), Waypoints.GetLength(1) - 1);
            return Waypoints[x, y];
        }

        public static List<Point> GetWaypointsAtArea(Vector2 vector, float squareRadius)
        {
            var point = GetPointAtVector(vector);
            var range = (int)Math.Round((double)squareRadius / Spacing); // IF PROBLEM = NOT ENOUGH NODES ARE PICKED UP => MATH.CEILING - OPPOSITE => MATH.FLOOR

            var returns = new List<Point>();
            for (var x = point.X - range; x < point.X + range; x++)
            {
                for (var y = point.Y - range; y < point.Y + range; y++)
                {
                    returns.Add(new Point(x, y));
                }
            }
            return returns;
        }

        public static Vector2 GetVectorAtPoint(Point point)
        {
            var x = point.X * Spacing;
            var y = point.Y * Spacing;
            return new Vector2(x, y);
        }

        public static List<Vector2> GetVectorsAtPoints(List<Point> points)
        {
            return points.Select(GetVectorAtPoint).ToList();
        }

        public static bool IsPointTraversable(Point point)
        {
            return true;//GetWaypointAtPoint(point).Traversable;
        }

        #endregion HelperMethods

        private static void UpdateWaypointsInView()
        {
            CamPointPos = GetPointAtVector(Camera.Position);
            CamTopLeftPointPos = GetPointAtVector(Camera.TopLeftPos);

            if (CamPointPos != OldCamPointPos || CamTopLeftPointPos != OldCamTopLeftPointPos)
            {
                SetWaypointsInView();
            }

            OldCamPointPos = CamPointPos;
            OldCamTopLeftPointPos = CamTopLeftPointPos;
        }
    }
}