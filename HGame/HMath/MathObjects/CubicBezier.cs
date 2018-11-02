using Microsoft.Xna.Framework;

namespace HGame.HMath.MathObjects
{
    internal class CubicBezier
    {


        public static Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float lerp) =>
            GetPoint(new[] { p1, p2, p3, p4 }, lerp);

        public static Vector3 GetPoint(Vector3[] points, float lerp)
        {
            var omt = 1f - lerp;
            var omt2 = omt * omt;
            var lerp2 = lerp * lerp;

            // OPTIMIZED VERSION
            var vector = points[0] * (omt2 * omt) +
                         points[1] * (3f * omt2 * lerp) +
                         points[2] * (3f * omt * lerp2) +
                         points[3] * (lerp2 * lerp);

            // FULL VERSION
            //var vector = points[0] * (float)Math.Pow(1 - lerp, 3) +
            //             points[1] * (3 * (float)Math.Pow(1 - lerp, 2) * lerp) +
            //             points[2] * (3 * (float)Math.Pow(1 - lerp, Math.Pow(lerp, 2))) +
            //             points[3] * (float)Math.Pow(lerp, 3);

            return vector;
        }

        public static Vector3 GetPointTangent(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float lerp) =>
            GetPointTangent(new[] { p1, p2, p3, p4 }, lerp);

        public static Vector3 GetPointTangent(Vector3[] points, float lerp)
        {
            var omt = 1f - lerp;
            var omt2 = omt * omt;
            var lerp2 = lerp * lerp;

            // OPTIMIZED VERSION
            var tangent = points[0] * -omt2 +
                         points[1] * (3f * omt2 - 2f * omt) +
                         points[2] * (-3f * lerp2 + 2f * lerp) +
                         points[3] * lerp2;

            // FULL VERSION
            //var vector = points[0] * -(float)Math.Pow(1 - lerp, 2) +
            //             points[1] * (lerp * (3 * lerp - 4) + 1) +
            //             points[2] * (-(float)Math.Pow(3 * lerp, 2) + 2 * lerp) +
            //             points[3] * (float)Math.Pow(lerp, 2);

            return Vectorials.SafeNormalize(tangent);
        }

        public static Vector3 GetPointNormal(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float lerp, Vector3 up) =>
            GetPointNormal(new[] { p1, p2, p3, p4 }, lerp, up);

        public static Vector3 GetPointNormal(Vector3[] points, float lerp, Vector3 up)
        {
            var tangent = GetPointTangent(points, lerp);
            var binormal = Vectorials.SafeNormalize(Vector3.Cross(up, tangent));
            var normal = Vector3.Cross(tangent, binormal);

            return normal;
        }

        public static Quaternion GetOrientation(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float lerp, Vector3 up) =>
            GetOrientation(new[] {p1, p2, p3, p4}, lerp, up);

        public static Quaternion GetOrientation(Vector3[] points, float lerp, Vector3 up)
        {
            var tangent = GetPointTangent(points, lerp);
            var normal = GetPointNormal(points, lerp, up);

            var orientation = Quaternion.LookRotation(tangent, normal);
            return orientation;
        }
    }
}
