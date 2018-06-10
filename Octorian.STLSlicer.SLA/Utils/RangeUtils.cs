using System;
using System.Collections.Generic;
using System.Text;
using QuantumConcepts.Formats.StereoLithography;

namespace Octorian.STLSlicer.SLA.Utils
{
    public static class RangeUtils
    {
        public static float CalculateHighX(STLDocument stl)
        {
            float highestX = stl.Facets[0].Vertices[0].X;
            stl.Facets.ForEach(f => f.Vertices.ForEach(v =>
            {
                if (v.X > highestX)
                    highestX = v.X;
            }));
            return highestX;
        }
        public static float CalculateLowX(STLDocument stl)
        {
            float lowestX = stl.Facets[0].Vertices[0].X;
            stl.Facets.ForEach(f => f.Vertices.ForEach(v =>
            {
                if (v.X < lowestX)
                    lowestX = v.X;
            }));
            return lowestX;
        }

        public static float CalculateHighY(STLDocument stl)
        {
            float highestY = stl.Facets[0].Vertices[0].Y;
            stl.Facets.ForEach(f => f.Vertices.ForEach(v =>
            {
                if (v.Y > highestY)
                    highestY = v.Y;
            }));
            return highestY;
        }
        public static float CalculateLowY(STLDocument stl)
        {
            float lowestY = stl.Facets[0].Vertices[0].Y;
            stl.Facets.ForEach(f => f.Vertices.ForEach(v =>
            {
                if (v.Y < lowestY)
                    lowestY = v.Y;
            }));
            return lowestY;
        }

        public static float CalculateHighZ(STLDocument stl)
        {
            float highestZ = stl.Facets[0].Vertices[0].Z;
            stl.Facets.ForEach(f => f.Vertices.ForEach(v =>
            {
                if (v.Z > highestZ)
                    highestZ = v.Z;
            }));
            return highestZ;
        }
        public static float CalculateLowZ(STLDocument stl)
        {
            float lowestZ = stl.Facets[0].Vertices[0].Z;
            stl.Facets.ForEach(f => f.Vertices.ForEach(v =>
            {
                if (v.Z < lowestZ)
                    lowestZ = v.Z;
            }));
            return lowestZ;
        }
    }
}
