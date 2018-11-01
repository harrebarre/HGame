using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HGame
{
    public class SysInfo
    {
        public static int VWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int ThisVWidth;

        public static int VHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static int ThisVHeight;

        public static float TimeCoefficient; // should be roughly equal to 1 if the game isn't running sluggishly (60 frames per second)

        public static void Update(GameTime gT)
        {
            TimeCoefficient = (float)gT.ElapsedGameTime.TotalSeconds * 60;
        }
    }
}
