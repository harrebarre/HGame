using Microsoft.Xna.Framework;

namespace HGame.HMath.MathObjects
{
    public struct Angle
    {
        private int _degrees;

        private float _radians;

        private Vector2 _vector;

        public Angle(Vector2 vector)
        {
            _degrees = 0;
            _radians = 0f;
            _vector = Vector2.UnitX;

            Vector = vector;
        }

        public Angle(float radians)
        {
            _degrees = 0;
            _radians = 0f;
            _vector = Vector2.UnitX;

            Radians = radians;
        }

        public Angle(int degrees)
        {
            _degrees = 0;
            _radians = 0f;
            _vector = Vector2.UnitX;

            Degrees = degrees;
        }

        public int Degrees
        {
            get => _degrees;
            set
            {
                _radians = Funct.DegreesToRadians(value);
                _degrees = value;
                _vector = Funct.AngleToVector2(_radians);
            }
        }

        public float Radians
        {
            get => _radians;
            set
            {
                _radians = value;
                _degrees = Funct.RadiansToDegrees(value);
                _vector = Funct.AngleToVector2(value);
            }
        }

        public Vector2 Vector
        {
            get => _vector;
            set
            {
                value = value != Vector2.Zero ? Vector2.Normalize(value) : Vector2.UnitX;

                _radians = Funct.Vector2ToAngle(value);
                _degrees = Funct.RadiansToDegrees(_radians);
                _vector = value;
            }
        }

        public float PlusQuarterPi => MathHelper.WrapAngle(Radians + H.π / 4);

        public float MinusQuarterPi => MathHelper.WrapAngle(Radians - H.π / 4);

        public float PlusHalfPi => MathHelper.WrapAngle(Radians + H.π / 2);

        public float MinusHalfPi => MathHelper.WrapAngle(Radians - H.π / 2);

        public int Plus45Deg => Funct.RadiansToDegrees(PlusQuarterPi);

        public int Minus45Deg => Funct.RadiansToDegrees(MinusQuarterPi);

        public int Plus90Deg => Funct.RadiansToDegrees(PlusHalfPi);

        public int Minus90Deg => Funct.RadiansToDegrees(MinusHalfPi);

        public Vector2 PlusHalfPlanar => Funct.AngleToVector2(PlusQuarterPi);

        public Vector2 MinusHalfPlanar => Funct.AngleToVector2(MinusQuarterPi);

        public Vector2 PlusOnePlanar => Funct.AngleToVector2(PlusHalfPi);

        public Vector2 MinusOnePlanar => Funct.AngleToVector2(MinusHalfPi);

        public Angle DiagonalLeft => new Angle(MinusQuarterPi);

        public Angle DiagonalRight => new Angle(PlusQuarterPi);

        public Angle Left => new Angle(MinusHalfPi);

        public Angle Right => new Angle(PlusHalfPi);

        public float InvertedRadians => Funct.InvertAngle(Radians);

        public float InvertedDegrees => Funct.RadiansToDegrees(InvertedRadians);

        public Vector2 InvertedVector => Funct.AngleToVector2(InvertedRadians);

        public Angle Invert => new Angle(InvertedRadians);

        public static Angle NewRandom => new Angle(Funct.GetRandom(0, 360));

        public static Angle operator +(Angle angleA, Angle angleB)
        {
            return new Angle(MathHelper.WrapAngle(angleA.Radians + angleB.Radians));
        }

        public static Angle operator -(Angle angleA, Angle angleB)
        {
            return new Angle(MathHelper.WrapAngle(angleA.Radians - angleB.Radians));
        }

        public static Angle operator +(Angle angleA, float radiansB)
        {
            return new Angle(MathHelper.WrapAngle(angleA.Radians + radiansB));
        }

        public static Angle operator -(Angle angleA, float radiansB)
        {
            return new Angle(MathHelper.WrapAngle(angleA.Radians - radiansB));
        }

        public static Angle operator +(Angle angleA, int degreesB)
        {
            return new Angle(MathHelper.WrapAngle(angleA.Radians + Funct.DegreesToRadians(degreesB)));
        }

        public static Angle operator -(Angle angleA, int degreesB)
        {
            return new Angle(MathHelper.WrapAngle(angleA.Radians - Funct.DegreesToRadians(degreesB)));
        }
    }
}