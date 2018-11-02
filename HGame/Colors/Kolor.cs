using Microsoft.Xna.Framework;

namespace HGame.Colors
{
    public struct Kolor
    {
        public string HEX;

        public HSV HSV;
        public HSL HSL;
        public Color RGB;

        public Kolor(Color rgb, string hex)
        {
            HEX = hex;
            RGB = rgb;
            HSV = RGB.ToHSV();
            HSL = RGB.ToHSL();
        }

        public Kolor(Vector3 rgb, string hex)
        {
            HEX = hex;
            RGB = new Color(rgb);
            HSV = RGB.ToHSV();
            HSL = RGB.ToHSL();
        }

        public Kolor(HSV hsv, string hex)
        {
            HEX = hex;
            RGB = hsv.ToRGB();
            HSV = hsv;
            HSL = HSV.ToHSL();
        }

        public int B => RGB.B;
        public int G => RGB.G;
        public float H => HSV.H;
        public int R => RGB.R;
        public float S => HSV.S;
        public float V => HSV.V;
    }
}
