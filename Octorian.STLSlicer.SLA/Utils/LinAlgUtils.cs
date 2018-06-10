using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using QuantumConcepts.Formats.StereoLithography;

namespace Octorian.STLSlicer.SLA.Utils
{
    public static class LinAlgUtils
    {
        #region Find Facets
        public static List<Facet> FindFacetsIntersectingZIndex(STLDocument stl, float z)
        {
            return stl.Facets.Where(f => f.Vertices.Any(v => v.Z <= z) && f.Vertices.Any(v => v.Z >= z) && Math.Abs(f.Normal.Z) != 1 ).ToList();
        }

        public static List<Facet> FindFlatFacetsAtZIndex(STLDocument stl, float z)
        {
            return stl.Facets.Where(f => f.Vertices.All(v => v.Z == z)).ToList();
        }

        public static List<Facet> FindTopFlatFacetsAtZIndex(STLDocument stl, float z)
        {
            return stl.Facets.Where(f => f.Vertices.Any(v => v.Z == z) && f.Normal.Z == 1).ToList();
        }
        public static List<Facet> FindBottomFlatFacetsAtZIndex(STLDocument stl, float z)
        {
            return stl.Facets.Where(f => f.Vertices.Any(v => v.Z == z) && f.Normal.Z == -1).ToList();
        }
        #endregion

        public static SliceLine CreateLineFromFacetAtZIndex(Facet facet, float z)
        {
            // This won't work for flat horizontal plane vertices, so throw if that's what we've got
            if (Math.Abs(facet.Normal.Z) == 1)
                throw new Exception("Cannot fine lines for flat horizontal planes");
            var verts = facet.Vertices;
            var returnLine = new SliceLine { Normal = new SlicePoint { X = facet.Normal.X, Y = facet.Normal.Y } };
            // If two vertices are ON the z-index, just return that line
            if (verts[0].Z == z && verts[1].Z == z)
            {
                returnLine.Points.Add(new SlicePoint { X = verts[0].X, Y = verts[0].Y });
                returnLine.Points.Add(new SlicePoint { X = verts[1].X, Y = verts[1].Y });
                return returnLine;
            }
            if (verts[2].Z == z && verts[1].Z == z)
            {
                returnLine.Points.Add(new SlicePoint { X = verts[2].X, Y = verts[2].Y });
                returnLine.Points.Add(new SlicePoint { X = verts[1].X, Y = verts[1].Y });
                return returnLine;
            }
            if (verts[2].Z == z && verts[0].Z == z)
            {
                returnLine.Points.Add(new SlicePoint { X = verts[2].X, Y = verts[2].Y });
                returnLine.Points.Add(new SlicePoint { X = verts[0].X, Y = verts[0].Y });
                return returnLine;
            }

            // find the two points
            // check first combination: v[0]-v[1]
            if ((verts[0].Z - z) / (verts[1].Z - z) < 0)
                returnLine.Points.Add(CalculateZIntercept(verts[0], verts[1], z));
            if ((verts[2].Z - z) / (verts[1].Z - z) < 0)
                returnLine.Points.Add(CalculateZIntercept(verts[2], verts[1], z));

            if (returnLine.Points.Count < 2)
                returnLine.Points.Add(CalculateZIntercept(verts[0], verts[2], z));

            return returnLine;

        }

        public static SlicePoint CalculateZIntercept(Vertex v1, Vertex v2, float z)
        {
            var returnX = CalculateDimensionalValueAtIndex(new SlicePoint(v1.Z, v1.X), new SlicePoint(v2.Z, v2.X), z);
            var returnY = CalculateDimensionalValueAtIndex(new SlicePoint(v1.Z, v1.Y), new SlicePoint(v2.Z, v2.Y), z);
            return new SlicePoint
            {
                X = returnX,
                Y = returnY
            };
        }

        public static float CalculateDimensionalValueAtIndex(SlicePoint p1, SlicePoint p2, float z, string precision = "0000")
        {
            var slope = (p1.Y - p2.Y) / (p1.X - p2.X);
            var intercept = p1.Y - (slope * p1.X);
            var rawVal = slope * z + intercept;
            // using floats we end up with some infinitesimal rounding errors, 
            // so we need to set the precision to something reasonable. Default is 1/100th of a micron
            // I'm sure there's a better way than converting it to a string and then back to a float,
            // but that's what I've got right now, so that's what I'm doing.
            var strVal = rawVal.ToString($"0.{precision}");
            return float.Parse(strVal, CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
