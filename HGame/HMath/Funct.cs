using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace HGame.HMath
{
    public static class Funct
    {
        public static T AbsTypeLerp<T>(float value, T min, T middle, T max)
        {
            return value < 0.5f ? min : value > 0.5f ? max : middle;
        }

        /// <summary>
        /// turns an angle into a vector, simple!
        /// </summary>
        /// <param name="angle">angle between 0 and 2 (+n*2) radians</param>
        /// <returns>returns a vector between (1,1) and (-1,-1)</returns>
        public static Vector2 AngleToVector2(float angle)
        {
            // determine the proportional length of the x and y components for a vector
            angle = MathHelper.WrapAngle(angle);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float WrapFloat(float value, float min, float max)
        {
            if (value < min)
                value = max - (min - value) % (max - min);
            else
                value = min + (value - min) % (max - min);

            return value;
        }

        public static int WrapInt(int value, int min, int max)
        {
            if (value < min)
                value = max - (min - value) % (max - min);
            else
                value = min + (value - min) % (max - min);

            return value;
        }

        public static T[,] BoxSelect<T>(T[,] array, int rowMin, int colMin, int rowMax, int colMax) where T : new()
        {
            // Allocate the result array.
            var numRows = rowMax - rowMin + 1;
            var numCols = colMax - colMin + 1;
            var result = new T[numRows, numCols];

            // Get the number of columns in the values array.
            var totalCols = array.GetUpperBound(1) + 1;
            var fromIndex = rowMin * totalCols + colMin;
            var toIndex = 0;

            for (var row = 0; row <= numRows - 1; row++)
            {
                Array.Copy(array, fromIndex, result, toIndex, numCols);
                fromIndex += totalCols;
                toIndex += numCols;
            }

            return result;
        }

        /// <summary>
        /// returns the location in world-space of the outer point of a circle in the specified direction
        /// </summary>
        /// <param name="center">the center point of the circle</param>
        /// <param name="radius">the radius of the circle</param>
        /// <param name="destination">the direction of the point from the center</param>
        /// <returns>a world-space point representing the circles outer edge in a specified direction</returns>
        public static Vector2 CircleToPoint(Vector2 center, float radius, Vector2 destination)
        {
            destination = destination != Vector2.Zero ? destination : Vector2.Zero;
            // normalize the vector, reducing every component to a max of 1 and minimum of 0, relative to eachother (same angle)
            var dir = Vector2.Normalize(destination - center); // vector originating in the center and reaching outwards to the destination
            return center + radius * dir;
        }

        public static float ClampAngleToAnotherAngleWithinThreshold(float angleSubject, float angleTarget, float threshold)
        {
            var absDiff = GetAbsAngleDifference(angleSubject, angleTarget);

            if (absDiff < threshold)
                return angleSubject;

            var diff = GetAngleDifference(angleSubject, angleTarget);

            diff = MathHelper.Clamp(diff, -threshold, threshold);

            return MathHelper.WrapAngle(angleTarget + diff);
        }

        public static T[] CombineDimensions<T>(T[,] twoDimensionalArray)
        {
            var width = twoDimensionalArray.GetLength(0);
            var height = twoDimensionalArray.GetLength(1);

            var array = new T[width * height];

            var index = 0;

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    array[index++] = twoDimensionalArray[i, j];
                }
            }

            return array;
        }

        public static List<T> CombineLists<T>(IEnumerable<IEnumerable<T>> ienumerables)
        {
            var list = new List<T>();
            foreach (var ienumerable in ienumerables)
            {
                list.AddRange(ienumerable);
            }
            return list;
        }

        public static HashSet<T> CombineHashSets<T>(IEnumerable<HashSet<T>> hashSets)
        {
            var hashSetUnion = new HashSet<T>();
            foreach (var hashSet in hashSets)
            {
                hashSetUnion.UnionWith(hashSet);
            }
            return hashSetUnion;
        }

        /// <summary>
        /// converts from angles (0-359 degrees) into radians
        /// </summary>
        /// <param name="angle">angle between 0 and 359 (+n*360) degrees</param>
        /// <returns>a degree measured in radians</returns>
        public static float DegreesToRadians(float angle)
        {
            var circle = H.π * 2; // a circle's radius is equal to 2 pi
            return angle % circle; // simpler syntax to write the argument below
            //return (float)(Math.PI / 180) * -angle;
        }

        public static float AbsMax(float value, float max)
        {
            if (value >= max)
                return max;
            if (value <= -max)
                return -max;
            return value;
        }

        public static int AbsMax(int value, int max)
        {
            if (value >= max)
                return max;
            if (value <= -max)
                return -max;
            return value;
        }

        public static int FastAbs(int i)
        {
            return (i ^ (i >> 31)) - (i >> 31);
        }

        public static float FindClosestValue(float value, List<float> comparers)
        {
            var closest = float.MaxValue;
            foreach (var comparer in comparers)
            {
                var dist = ValueDist(value, comparer);
                closest = dist < closest ? dist : closest;
            }
            return closest;
        }

        /// <summary>
        /// gets the absolute angle difference from angleA to angleB
        /// </summary>
        public static float GetAbsAngleDifference(float angleA, float angleB)
        {
            var angleDifference = MathHelper.WrapAngle(angleA) - MathHelper.WrapAngle(angleB);
            angleDifference = MathHelper.WrapAngle(angleDifference);
            angleDifference = Math.Abs(angleDifference);
            return angleDifference;
        }

        /// <summary>
        /// gets the angle difference from angleA to angleB
        /// </summary>
        public static float GetAngleDifference(float angleA, float angleB)
        {
            var angleDifference = MathHelper.WrapAngle(angleA) - MathHelper.WrapAngle(angleB);
            angleDifference = MathHelper.WrapAngle(angleDifference);
            return angleDifference;
        }

        public static float GetLerp(float value, float min, float max)
        {
            if (value <= min)
                return 0;

            if (value >= max)
                return 1;

            if (min == max)
                return 0.5f;

            return (value - min) / (max - min);
        }

        public static int GetRandom(int minValue, int maxValue)
        {
            return H.Random.Next(minValue, maxValue);
        }

        public static float GetRandom(float minValue, float maxValue)
        {
            return minValue + (float)H.Random.NextDouble() * (maxValue - minValue);
        }

        public static float InvertAngle(float angle)
        {
            return MathHelper.WrapAngle(angle + H.π);
        }

        public static float Lerp(float valueA, float valueB, float lerp)
        {
            if (lerp <= 0)
                return valueA;

            if (lerp >= 1)
                return valueB;

            var diff = valueB - valueA;
            return valueA + (diff * lerp);
        }

        public static double Lerp(double valueA, double valueB, double lerp)
        {
            if (lerp <= 0)
                return valueA;

            if (lerp >= 1)
                return valueB;

            var diff = valueB - valueA;
            return valueA + (diff * lerp);
        }

        /// <summary>
        /// linearly approaches a target value
        /// </summary>
        /// <param name="value">the current value</param>
        /// <param name="target">the desired value</param>
        /// <param name="speed">the linear approach speed</param> // ??
        /// <returns></returns>
        public static float LinearApproach(float value, float target, float positiveSpeed, float negativeSpeed)
            => value < target - positiveSpeed ? value + positiveSpeed : value > target + negativeSpeed ? value - negativeSpeed : target;

        public static List<T> OrderByOddThenEven<T>(List<T> list)
        {
            var sequence = list.ToList();
            var evens = sequence.Where((item, index) => index % 2 == 0);
            var odds = sequence.Where((item, index) => index % 2 != 0);
            return evens.Concat(odds).ToList();
        }

        public static int RadiansToDegrees(float radians)
        {
            return (int)Math.Round((MathHelper.WrapAngle(radians) / H.π) * 180);
        }

        public static float ValueDist(float value1, float value2)
        {
            return Math.Abs(value1 - value2);
        }

        /// <summary>
        /// turns a vector between (1,1), (-1,-1) into an angle
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>an angle between 0 and 2 (+n*2) radians</returns>
        public static float Vector2ToAngle(Vector2 vector)
        {
            // returns the angle whose tangent is the quotient of the two components of the vector
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static T RandomElement<T>(this List<T> list)
        {
            var i = H.Random.Next(0, list.Count);
            return list[i];
        }

        public static float RandomFloatingNumber(float min, float max)
        {
            var delta = max - min;
            var result = min + H.Random.NextDouble() * delta;
            return (float)result;
        }
    }
}