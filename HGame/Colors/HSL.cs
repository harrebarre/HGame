namespace HGame.Colors
{
    public class HSL
    {
        public float H;
        public float S;
        public float L;

        public HSL(float h, float s, float l)
        {
            H = h;
            S = s;
            L = l;
        }

        public static HSL White => new HSL(0, 1, 1);
    }
}
