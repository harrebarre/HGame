using System.Collections.Generic;
using System.Linq;
using HGame.HMath;
using Microsoft.Xna.Framework;

namespace HGame.Colors
{
    /// <summary>
    /// colors ordered by hue, then by saturation (ex green -> yellow and lighter green -> darker green)
    /// </summary>
    public static class HColor
    {
        //shades of light -> dark
        public static Kolor Clouds = new Kolor(new Color(236, 240, 241), "#ecf0f1"); // from brightest

        public static Kolor Silver = new Kolor(new Color(189, 195, 199), "#bdc3c7");

        public static Kolor Concrete = new Kolor(new Color(149, 165, 166), "#95a5a6");

        public static Kolor Asbetos = new Kolor(new Color(127, 140, 141), "#7f8c8d");

        public static Kolor Asphalt = new Kolor(new Color(52, 73, 94), "#34495e");

        public static Kolor Midnight = new Kolor(new Color(44, 62, 80), "#2c3e50");

        public static Kolor Brightcord = new Kolor(new Color(54, 57, 62), "#36393E");

        public static Kolor Envato = new Kolor(new Color(33, 42, 52), "#212a34");

        public static Kolor Managore = new Kolor(new Color(34, 32, 52), "#222034");

        public static Kolor Discord = new Kolor(new Color(30, 33, 36), "#1e2124"); // to darkest

        public static List<Kolor> Monogreys = new List<Kolor> { Clouds, Silver, Concrete, Asbetos, Asphalt, Midnight, Envato, Managore, Discord };

        // shades of green -> teal
        public static Kolor Emerald = new Kolor(new Color(46, 204, 113), "#2ecc71");

        public static Kolor Leaf = new Kolor(new Color(122, 172, 65), "#7aac41");

        public static Kolor Nephritis = new Kolor(new Color(39, 174, 96), "#27ae60");

        public static Kolor Turquoise = new Kolor(new Color(26, 188, 156), "#1abc9c");

        public static Kolor GreenSea = new Kolor(new Color(22, 160, 133), "#16a085");

        public static List<Kolor> Monogreens = new List<Kolor> {Emerald, Leaf, Nephritis, Turquoise, GreenSea};

        // shades of blue
        public static Kolor Alliterate = new Kolor(new Color(66, 253, 254), "#42FDFE");

        public static Kolor Mnemonic = new Kolor(new Color(0, 31, 246), "#001FF6");

        public static Kolor River = new Kolor(new Color(52, 152, 219), "#3498db");

        public static Kolor Belize = new Kolor(new Color(41, 128, 185), "#2980b9");

        public static List<Kolor> Monoblues = new List<Kolor> {Mnemonic, River, Belize};

        // shades of purple
        public static Kolor Amethyst = new Kolor(new Color(155, 89, 182), "#9b59b6");

        public static Kolor Wisteria = new Kolor(new Color(142, 68, 173), "#8e44ad");

        public static Kolor Shroud = new Kolor(new Color(84, 86, 160), "#5456a0");

        public static List<Kolor> Monopurples = new List<Kolor> {Amethyst, Wisteria, Shroud};

        // shades of yellow -> orange
        public static Kolor SunFlower = new Kolor(new Color(241, 196, 15), "#f1c40f");

        public static Kolor Orange = new Kolor(new Color(243, 156, 18), "#f39c12");

        public static Kolor Carrot = new Kolor(new Color(230, 126, 34), "#e67e22");

        public static Kolor Pumpkin = new Kolor(new Color(211, 84, 0), "#d35400");

        public static List<Kolor> Monoyellows = new List<Kolor> {SunFlower, Orange, Carrot, Pumpkin};

        // shades of red
        public static Kolor Alizarin = new Kolor(new Color(231, 76, 60), "#e74c3c");

        public static Kolor Pomegranate = new Kolor(new Color(192, 57, 43), "#c0392b");

        public static Kolor INNOVATE = new Kolor(new Color(236, 58, 72), "#ec3a48");

        public static List<Kolor> Monoreds = new List<Kolor> {Alizarin, Pomegranate, INNOVATE};

        // kolor collections
        public static List<Kolor> Monochromes = Funct.CombineLists(new List<List<Kolor>> {Monogreys, Monogreens, Monoblues, Monopurples, Monoyellows, Monoreds});

        public static Kolor RandomKolor(List<Kolor> kolors)
        {
            return kolors.RandomElement();
        }

        public static Kolor RandomKolor()
        {
            return Monochromes.RandomElement();
        }

        public static Color Average(IEnumerable<Color> colors)
        {
            var enumerable = colors as IList<Color> ?? colors.ToList();

            if (!enumerable.Any())
                return Color.White;

            var red = enumerable.Average(r => r.ToVector3().X);
            var green = enumerable.Average(g => g.ToVector3().Y);
            var blue = enumerable.Average(b => b.ToVector3().Z);

            return new Color(red, green, blue);
        }

        public static HSV Average(IEnumerable<HSV> colors)
        {
            var enumerable = colors as IList<HSV> ?? colors.ToList();

            if (enumerable.Any())
                return HSV.White;

            var hue = enumerable.Average(c => c.H);
            var saturation = enumerable.Average(c => c.S);
            var value = enumerable.Average(c => c.V);

            return new HSV(hue, saturation, value);
        }

        public static Color CompensateForGifIllumination(Color color)
        {
            const int luminosityConstant = 8;

            var red = color.R - luminosityConstant;
            var green = color.G - luminosityConstant;
            var blue = color.B - luminosityConstant;

            return new Color(red, green, blue);
        }

        public static Color Lerp(Color color1, Color color2, float ratio)
        {
            return Color.Lerp(color1, color2, ratio);
        }

        public static HSV Lerp(HSV color1, HSV color2, float ratio)
        {
            var hue = Funct.Lerp(color1.H, color2.H, ratio);
            var saturation = Funct.Lerp(color1.S, color2.S, ratio);
            var value = Funct.Lerp(color1.V, color2.V, ratio);

            return new HSV(hue, saturation, value);
        }

        public static Color Mixer(Color color1, Color color2)
        {
            var c1 = color1.ToVector3();
            var c2 = color2.ToVector3();
            var red = c1.X * c2.X;
            var green = c1.Y * c2.Y;
            var blue = c1.Z * c2.Z;
            return new Color(red, green, blue);
        }

        public static HSV Mixer(HSV color1, HSV color2)
        {
            var hue = color1.H * color2.H;
            var saturation = color1.S * color2.S;
            var value = color1.V * color2.V;

            return new HSV(hue, saturation, value);
        }

        public static Color RandomRGB(int lower = 0, int upper = 256)
        {
            var red = H.Random.Next(lower, upper);
            var green = H.Random.Next(lower, upper);
            var blue = H.Random.Next(lower, upper);

            return new Color(red, green, blue);
        }

        public static HSV RandomHSV(float s, float v)
        {
            var h = (float)H.Random.NextDouble();
            return new HSV(h, s, v);
        }
    }
}