namespace HGame.Colors
{
    public struct HSV
    {
        public float H;
        public float S;
        public float V;

        public HSV(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }

        public static HSV White => new HSV(0, 0, 1);
    }
}
