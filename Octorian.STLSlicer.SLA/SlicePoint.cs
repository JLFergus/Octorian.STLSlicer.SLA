using System;

namespace Octorian.STLSlicer.SLA
{
    public class SlicePoint : IEquatable<SlicePoint>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public SlicePoint() { }
        public SlicePoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Equality Operators
        public static bool operator == (SlicePoint p1, SlicePoint p2)
        {
            if (ReferenceEquals(p1, p2))
                return true;

            return p1?.X == p2?.X && p1?.Y == p2?.Y;
        }

        public static bool operator != (SlicePoint p1, SlicePoint p2)
        {
            if (ReferenceEquals(p1, p2))
                return false;

            return p1?.X != p2?.X || p1?.Y != p2?.Y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SlicePoint);
        }

        public bool Equals(SlicePoint other)
        {
            return other != null &&
                   X == other?.X &&
                   Y == other?.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
        #endregion
    }
}