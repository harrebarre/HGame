using HGame.Colors.Gradients;
using Microsoft.Xna.Framework;

namespace HGame.Colors.Systems
{
    public class GradientColorSystem : ColorSystem
    {
        public Gradient Gradient;

        public Color Average => Gradient.Average;

        public GradientColorSystem(Gradient gradient) : base(Color.White)
        {
            Gradient = gradient;
            Color = Average;
        }

        public Color InspectColor(int lookup)
        {
            Color = Gradient.InspectColor(lookup);

            return ExtractColor();
        }

        public Color InspectColor(float lookup)
        {
            Color = Gradient.InspectColor(lookup);

            return ExtractColor();
        }

        public Color AverageColor()
        {
            Color = Average;

            return ExtractColor();
        }
    }
}
