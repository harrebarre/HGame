using System;
using Microsoft.Xna.Framework;

namespace HGame.Colors.Gradients
{
    public class Gradient
    {
        public HSV BaseHSV;
        public HSV TipHSV;

        public int Length;

        public Color Average;

        protected Color[] Values;

        public Gradient()
        {
        }

        public Gradient(HSV baseHSV, HSV tipHSV, int length)
        {
            BaseHSV = baseHSV;
            TipHSV = tipHSV;
            Length = length;

            GenerateColors();
        }

        public virtual Color InspectColor(int lookup)
        {
            lookup = (lookup < 0) ? 0 : (lookup >= Length) ? Length - 1 : lookup;

            return Values[lookup];
        }

        public virtual Color InspectColor(float lookup)
        {
            lookup = (lookup < 0) ? 0 : (lookup >= Length) ? Length - 1 : lookup;

            var floor = (int)Math.Floor(lookup);
            var ceiling = (int)Math.Ceiling(lookup);

            var colorA = Values[floor];
            var colorB = Values[ceiling];

            if (floor == ceiling)
                return colorA;

            var color = HColor.Lerp(colorA, colorB, lookup - floor);

            return color;
        }

        private void GenerateColors()
        {
            Values = new Color[Length];

            for (var i = 0; i < Length; i++)
            {
                var color = HColor.Lerp(BaseHSV, TipHSV, Funct.GetLerp(i, 0, Length));
                Values[i] = color.ToRGB();
            }

            Average = HColor.Average(Values);
        }
    }
}