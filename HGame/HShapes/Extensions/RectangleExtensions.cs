using Microsoft.Xna.Framework;

namespace HGame.HShapes.Extensions
{
    public static class RectangleExtensions
    {
        public static float Diagonal(this Rectangle rectangle)
        {
            var diagonal = Geometrics.Hypothenuse(rectangle.Height, rectangle.Width);
            return diagonal;
        }
    }
}
