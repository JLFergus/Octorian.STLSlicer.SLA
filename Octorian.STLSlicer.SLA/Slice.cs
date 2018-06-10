using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Octorian.STLSlicer.SLA
{
    public class Slice : IEquatable<Slice>
    {
        public List<SliceLine> Lines { get; set; } = new List<SliceLine>();

        public bool ValidateShapeIntegrity()
        {
            // A shape has to have at least three lines to close
            if (Lines.Count < 3)
                return false;

            var allPoints = Lines.SelectMany(l => l.Points).ToList();
            var pointFreq = new Dictionary<SlicePoint, int>();

            // There can't be any hanging lines. Verify every point connects to at least one other line.
            foreach (var p in allPoints)
            {
                if (!pointFreq.Keys.Contains(p))
                    pointFreq[p] = 1;
                else
                    pointFreq[p]++;
            }

            foreach (var p in pointFreq.Keys)
            {
                if (pointFreq[p] < 2)
                    return false;
            }

            return true;
        }

        #region Equality Operators
        public static bool operator == (Slice s1, Slice s2)
        {
            if (ReferenceEquals(s1, s2))
                return true;

            foreach(SliceLine l in s1?.Lines)
            {
                if ((bool)!s2?.Lines.Any(l2 => l == l2))
                    return false;
            }

            return true;
        }

        public static bool operator != (Slice s1, Slice s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Slice);
        }

        public bool Equals(Slice other)
        {
            return other != null && this == other;
        }

        public override int GetHashCode()
        {
            return -713085124 + EqualityComparer<List<SliceLine>>.Default.GetHashCode(Lines);
        }
        #endregion
    }
}
