using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame.HShapes
{
    /// <summary>
    /// base class for declaring what shape an object is
    /// </summary>
    public class Shape
    {
        public List<Vector2> Points;
        public Vector2 Position;
        public Rectangle ProxRec;
        private float _rotation;

        public Shape(Rectangle proxRec)
        {
            SetProxRec(proxRec);
        }

        public Shape()
        {
            
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = MathHelper.WrapAngle(value);
        }

        public virtual bool CollidesWith(List<Vector2> points)
        {
            return false;
        }

        public virtual bool Contains(Vector2 point)
        {
            return false;
        }

        public virtual void Draw(SpriteBatch sB, Color color, float layerDepth)
        {
        }

        public virtual void GenerateProximityRectangle()
        {
            ProxRec = GenerateProximityRectangle(Points);
        }

        public static Rectangle GenerateProximityRectangle(List<Vector2> points)
        {
            if (points.Count <= 0)
                return new Rectangle();

            float xMin = points[0].X, yMin = points[0].Y, xMax = points[0].X, yMax = points[0].Y;

            foreach (var p in points)
            {
                xMin = p.X < xMin ? p.X : xMin;
                yMin = p.Y < yMin ? p.Y : yMin;
                xMax = p.X > xMax ? p.X : xMax;
                yMax = p.Y > yMax ? p.Y : yMax;
            }

            var x = (int)Math.Floor(xMin);
            var y = (int)Math.Floor(yMin);
            var width = (int)Math.Ceiling(Math.Abs(xMax) - Math.Abs(xMin));
            var height = (int)Math.Ceiling(Math.Abs(yMax) - Math.Abs(yMin));

            return new Rectangle(x, y, width, height);
        }

        public void SetProxRec(Rectangle proxRec)
        {
            ProxRec = proxRec;
        }

        public override string ToString()
        {
            return "shape";
        }

        public virtual MonoGame.Extended.Shapes.Polygon ToPolygonF()
        {
            var polygonF = new MonoGame.Extended.Shapes.Polygon(Points);
            return polygonF;
        }

        public virtual void ChangeShape(List<Vector2> points)
        {
            Points = points;
        }

        public virtual void Update()
        {
            
        }

        public virtual void UpdatePlace(Vector2 position, float rotation)
        {
            Position = position;
            UpdateShape(rotation - Rotation); // we only want the difference in rotation to apply in the translation
            Rotation = rotation;
            GenerateProximityRectangle();
        }

        public virtual void UpdateShape(float rotDiff)
        {
            if (Math.Abs(rotDiff) > float.Epsilon)
                Vectorials.TranslateAroundPoint(Points, Position, rotDiff);
            GenerateProximityRectangle();
        }
    }
}