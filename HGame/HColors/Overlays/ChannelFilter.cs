using Microsoft.Xna.Framework;

namespace HGame.Colors.Overlays
{
    public class ChannelFilter : Overlay
    {
        public Color Filter;

        public ChannelFilter(Color filter)
        {
            Filter = filter;
        }

        public override Color Apply(Color input)
        {
            var output = HColor.Mixer(input, Filter);

            return output;
        }
    }
}
