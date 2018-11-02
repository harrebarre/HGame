using Microsoft.Xna.Framework;

namespace HGame.Colors.Overlays
{
    public class Overlay
    {
        public virtual Color Apply(Color input)
        {
            var output = input;

            return output;
        }
    }
}
