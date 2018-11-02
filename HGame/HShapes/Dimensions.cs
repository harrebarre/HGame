using Microsoft.Xna.Framework;

namespace HGame.HShapes
{
    internal class Dimensions
    {
        public Fixture Center;
        public Fixture CenterFront;
        public Fixture CenterRear;
        public Fixture CenterLeft;
        public Fixture CenterRight;

        public Fixture FrontLeft;
        public Fixture FrontRight;
        public Fixture RearLeft;
        public Fixture RearRight;

        public Vector2 Forward => Vector2.Normalize(CenterFront.Position - CenterRear.Position);
        public Vector2 Backward => Vector2.Normalize(CenterRear.Position - CenterFront.Position);

        public Dimensions(Vector2 center, Vector2 centerFront, Vector2 centerRear, Vector2 centerLeft, Vector2 centerRight, Vector2 frontLeft, Vector2 frontRight, Vector2 rearLeft, Vector2 rearRight)
        {
            Center = new Fixture(center);
            CenterFront = new Fixture(centerFront);
            CenterRear = new Fixture(centerRear);
            CenterLeft = new Fixture(centerLeft);
            CenterRight = new Fixture(centerRight);
            FrontLeft = new Fixture(frontLeft);
            FrontRight = new Fixture(frontRight);
            RearLeft = new Fixture(rearLeft);
            RearRight = new Fixture(rearRight);
        }

        public virtual void Update(Vector2 center, Vector2 centerFront, Vector2 centerRear, Vector2 centerLeft, Vector2 centerRight, Vector2 frontLeft, Vector2 frontRight, Vector2 rearLeft, Vector2 rearRight)
        {
            Center.Update(center);
            CenterFront.Update(centerFront);
            CenterRear.Update(centerRear);
            CenterLeft.Update(centerLeft);
            CenterRight.Update(centerRight);
            FrontLeft.Update(frontLeft);
            FrontRight.Update(frontRight);
            RearLeft.Update(rearLeft);
            RearRight.Update(rearRight);
        }
    }
}
