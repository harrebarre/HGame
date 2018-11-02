using System;
using HGame.HMath;
using Microsoft.Xna.Framework;

namespace HGame.Colors
{
    public static class ColorHelper
    {
        public static Color Lerp(this Color color1, Color color2, float ratio)
        {
            return Color.Lerp(color1, color2, ratio);
        }

        public static HSV Lerp(this HSV color1, HSV color2, float ratio)
        {
            var hue = Funct.Lerp(color1.H, color2.H, ratio);
            var saturation = Funct.Lerp(color1.S, color2.S, ratio);
            var value = Funct.Lerp(color1.V, color2.V, ratio);

            return new HSV(hue, saturation, value);
        }

        public static Color Mix(this Color color1, Color color2)
        {
            var c1 = color1.ToVector3();
            var c2 = color2.ToVector3();
            var red = c1.X * c2.X;
            var green = c1.Y * c2.Y;
            var blue = c1.Z * c2.Z;
            return new Color(red, green, blue);
        }

        public static HSV Mix(this HSV color1, HSV color2)
        {
            var hue = color1.H * color2.H;
            var saturation = color1.S * color2.S;
            var value = color1.V * color2.V;

            return new HSV(hue, saturation, value);
        }

        public static HSV ToHSV(this Color rgb)
        {
            var R = (float) rgb.R;
            var G = (float) rgb.G;
            var B = (float) rgb.B;

            var r = (R / 255);
            var g = (G / 255);
            var b = (B / 255);

            var min = Math.Min(Math.Min(r, g), b);
            var max = Math.Max(Math.Max(r, g), b);
            var deltaMax = max - min;

            var H = 0f;
            var S = 0f;
            var V = max;

            if (deltaMax == 0)
                return new HSV(H, S, V); // results in a grey color, no chroma

            S = deltaMax / max;

            var deltaR = (((max - r) / 6) + (deltaMax / 2f)) / deltaMax;
            var deltaG = (((max - g) / 6) + (deltaMax / 2f)) / deltaMax;
            var deltaB = (((max - b) / 6) + (deltaMax / 2f)) / deltaMax;

            if      (r == max) H = deltaB - deltaG;
            else if (g == max) H = (1f / 3f) + deltaR - deltaB;
            else if (b == max) H = (2f / 3f) + deltaG - deltaR;

            if (H < 0) H += 1;
            if (H > 1) H -= 1;

            return new HSV(H, S, V);
        }

        public static Color ToRGB(this HSV hsv)
        {
            var H = hsv.H;
            var S = hsv.S;
            var V = hsv.V;

            if (S == 0)
                return new Color(V * 255, V * 255, V * 255);

            var h = H * 6;
            if (h == 6) h = 0;
            var i = (int) Math.Floor(h);
            var one = V * (1 - S);
            var two = V * (1 - S * (h - i));
            var three = V * (1 - S * (1 - (h - i)));

            float r, g, b;

            if      (i == 0) { r = V;     g = three; b = one; }
            else if (i == 1) { r = two;   g = V;     b = one; }
            else if (i == 2) { r = one;   g = V;     b = three; }
            else if (i == 3) { r = one;   g = two;   b = V; }
            else if (i == 4) { r = three; g = one;   b = V; }
            else             { r = V;     g = one;   b = two; }

            var R = (int) (r * 255);
            var G = (int) (g * 255);
            var B = (int) (b * 255);

            return new Color(R, G, B);
        }

        public static Color ToRGB(this HSL hsl)
        {
            var h = hsl.H;
            var s = hsl.S;
            var l = hsl.L;

            if (s == 0)
                return new Color(l * 255, l * 255, l * 255);

            var two = l < 0.5 ? l * (1 + s) : l + s - s * l;

            var one = 2 * l - two;

            var r = (int)Math.Round(255 * Hue2Rgb(one, two, h + 1f / 3f));
            var g = (int)Math.Round(255 * Hue2Rgb(one, two, h));
            var b = (int)Math.Round(255 * Hue2Rgb(one, two, h - 1f / 3f));

            return new Color(r, g, b);

            float Hue2Rgb(float v1, float v2, float vH)             //Function Hue_2_RGB
            {
                if (vH < 0) vH += 1;
                if (vH > 1) vH -= 1;
                if (6 * vH < 1) return v1 + (v2 - v1) * 6 * vH;
                if (2 * vH < 1) return v2;
                if (3 * vH < 2) return v1 + (v2 - v1) * (2f / 3f - vH) * 6;
                return v1;
            }
        }

        public static HSL ToHSL(this HSV hsv)
        {
            var rgb = hsv.ToRGB();
            return rgb.ToHSL();
            //var h = hsv.H;
            //var s = hsv.S * hsv.V / ((h = (2 - hsv.S) * hsv.V) < 1 ? h : 2 - h);
            //var l = h / 2;
            //return new HSL(h, s, l);
        }

        public static HSL ToHSL(this Color rgb)
        {
            var r = rgb.R / 255f;
            var g = rgb.G / 255f;
            var b = rgb.B / 255f;

            var min = Math.Min(Math.Min(r, g), b);    
            var max = Math.Max(Math.Max(r, g), b);
            var deltaMax = max - min;

            var l = (max + min) / 2;

            if (deltaMax == 0)
                return new HSL(0, 0, l);

            var s = l < 0.5 ? deltaMax / (max + min) : deltaMax / (2 - max - min);

            var deltaR = ((max - r) / 6 + deltaMax / 2) / deltaMax;
            var deltaG = ((max - g) / 6 + deltaMax / 2) / deltaMax;
            var deltaB = ((max - b) / 6 + deltaMax / 2) / deltaMax;

            var h = r == max ? deltaB - deltaG : 
                    g == max ? 1f / 3f + deltaR - deltaB
                             : 2f / 3f + deltaG - deltaR;

            if (h < 0) h += 1;
            if (h > 1) h -= 1;

            return new HSL(h, s, l);
        }

        public static HSV ToHSV(this HSL hsl)
        {
            var rgb = hsl.ToRGB();
            return rgb.ToHSV();
            //var hue = hsl.H;
            //var light = hsl.L;
            //var sat = hsl.S * light < .5 ? light : 1 - light;

            //sat = 2 * sat / (light + sat);
            //var value = light + sat;
            //return new HSV(hue, sat, value);
        }
    }
}