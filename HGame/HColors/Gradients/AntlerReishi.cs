using HGame.HMath;
using Microsoft.Xna.Framework;

namespace HGame.Colors.Gradients
{
    public class AntlerReishi : Gradient
    {
        public HSL AbsoluteHSL;

        public AntlerReishi(HSL absoluteHSL, int length)
        {
            AbsoluteHSL = absoluteHSL;
            Length = length;

            GenerateColors();
        }

        private void GenerateColors()
        {
            Values = new Color[Length];

            for (var i = 0; i < Length; i++)
            {
                var color = new HSL(AbsoluteHSL.H, AbsoluteHSL.S, Funct.GetLerp(i, 0, Length));
                Values[i] = color.ToRGB();
            }
        }
    }
}