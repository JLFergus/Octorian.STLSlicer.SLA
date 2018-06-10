using System;
using System.Collections.Generic;
using System.Linq;

namespace Octorian.STLSlicer.SLA
{
    public class SliceLine : IEquatable<SliceLine>
    {
        public List<SlicePoint> Points { get; set; } = new List<SlicePoint>();
        public SlicePoint Normal { get; set; }

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