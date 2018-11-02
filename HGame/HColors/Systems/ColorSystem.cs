using System.Collections.Generic;
using HGame.Colors.Overlays;
using Microsoft.Xna.Framework;

namespace HGame.Colors.Systems
{
    public class ColorSystem
    {
        public List<Overlay> Overlays;

        public Color Color;

        public ColorSystem()
        {
            Color = Color.White;
            Overlays = new List<Overlay>();
        }

        public ColorSystem(Color color)
        {
            Color = color;
            Overlays = new List<Overlay>();
        }

        public ColorSystem(HSV color)
        {
            Color = color.ToRGB();
            Overlays = new List<Overlay>();
        }

        public ColorSystem(HSL color)
        {
            Color = color.ToRGB();
            Overlays = new List<Overlay>();
        }

        public Color ExtractColor()
        {
            var color = Color;

            foreach (var overlay in Overlays)
            {
                color = overlay.Apply(color);
            }

            return color;
        }
    }
}
