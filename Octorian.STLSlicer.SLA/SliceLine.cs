using System;
using System.Collections.Generic;
using System.Linq;

namespace Octorian.STLSlicer.SLA
{
    public class SliceLine : IEquatable<SliceLine>
    {
        private SlicePoint _normal;
        public List<SlicePoint> Points { get; set; } = new List<SlicePoint>();

        public SlicePoint Normal
        {
            get => _normal;
            // not really setting it with the value, but using it to calculate based on the points
            set => _normal = CalculateNormal(value);
        }

        public SlicePoint CalculateNormal(SlicePoint normal = null)
        {
            if (!Validate())
                throw new InvalidOperationException("Can't calculate Normal without points set");

            if (normal == null)
                normal  = _normal;
            if (normal == null)
                throw new InvalidOperationException("Can't calculate Normal without a starting point");

            // calculate normal slopes 
            var dX = Points[0].Y - Points[1].Y;
            var dY = Points[0].X - Points[1].X;
            // determine normal direction
            var xDir = normal.X >= 0 ? 1 : -1;
            var yDir = normal.Y >= 0 ? 1 : -1;
            // check for delta 0 in either direction
            if (dX == 0 && dY == 0)
                return new SlicePoint(0, 0);
            if (dX == 0)
                return new SlicePoint(0, yDir);
            if (dY == 0)
                return new SlicePoint(xDir, 0);

            // if there aren't any zeroes, calculate the hypotenuse
            var hypotenuse = (float) Math.Sqrt((dX*dX) + (dY*dY));
            // use cross-multiplication and solve for x & y to find normal values where hypotenuse == 1
            dX = Math.Abs(dX / hypotenuse) * xDir;
            dY = Math.Abs(dY / hypotenuse) * yDir;
            return new SlicePoint(dX, dY);
        }

        public bool Validate()
        {
            return Points != null && (Points.Count == 2);
        }


        #region Equality Operators
        public static bool operator == (SliceLine l1, SliceLine l2)
        {
            if (ReferenceEquals(l1, l2))
                return true;
            
            if (l1?.Normal != l2?.Normal || l1?.Points?.Count != l2?.Points?.Count)
                return false;

            foreach (var point in l1?.Points)
            {
                if ((bool)!l2?.Points?.Any(p => p == point))
                    return false;
            }

            return true;
        }

        public static bool operator !=(SliceLine l1, SliceLine l2)
        {
            return !(l1 == l2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SliceLine);
        }

        public bool Equals(SliceLine other)
        {
            return other != null && this == other;
                   //EqualityComparer<List<SlicePoint>>.Default.Equals(Points, other?.Points) &&
                   //EqualityComparer<SlicePoint>.Default.Equals(Normal, other?.Normal);
        }

        public override int GetHashCode()
        {
            var hashCode = 1971777096;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<SlicePoint>>.Default.GetHashCode(Points);
            hashCode = hashCode * -1521134295 + EqualityComparer<SlicePoint>.Default.GetHashCode(Normal);
            return hashCode;
        }
        #endregion
    }
}