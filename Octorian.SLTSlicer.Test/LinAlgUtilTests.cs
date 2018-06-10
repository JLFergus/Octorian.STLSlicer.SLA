using System.IO;
using Xunit;
using Octorian.STLSlicer.SLA.Utils;
using QuantumConcepts.Formats.StereoLithography;
using System.Collections.Generic;
using Octorian.STLSlicer.SLA;

namespace Octorian.STLSlicer.Test
{
    public class LinAlgUtilTests
    {
        private static readonly string _projectPath = $"{Path.GetDirectoryName(Directory.GetCurrentDirectory())}\\..\\..\\Assets\\";
        private static readonly string _testPyramidPath = $"{_projectPath}Test Pyramid.stl";
        private STLDocument _stl;

        private void LoadSTL()
        {
            bool _failed;
            do
            {
                _failed = false;
                try
                {
                    using (var _stlStream = new FileStream(_testPyramidPath, FileMode.Open))
                    {
                        _stl = STLDocument.Read(_stlStream, true);
                        if (_stl == null)
                        {
                            throw new FileNotFoundException($"File not found: {_testPyramidPath}");
                        }
                    }

                }
                catch (IOException)
                {
                    _failed = true;
                }
            } while (_failed);
        }

        #region Find Facets
        [Fact]
        public void FindFacetsIntersectingZIndex_ReturnsAppropriateFacets()
        {
            LoadSTL();
            Assert.Equal(4, LinAlgUtils.FindFacetsIntersectingZIndex(_stl, 2).Count);
        }

        [Fact]
        public void FindFacetsIntersectingZIndex_IgnoresFlatFacets_FindsEdgesAndPoints()
        {
            LoadSTL();
            // there are technically 6 facets at this z, but 2 of them are flat
            Assert.Equal(4, LinAlgUtils.FindFacetsIntersectingZIndex(_stl, 10).Count);
            // sould also find single points
            Assert.Equal(4, LinAlgUtils.FindFacetsIntersectingZIndex(_stl, 8).Count);
        }

        [Fact]
        public void FindFlatFacetsAtZIndex_FindsFlatFacets_Either_Orientation()
        {
            LoadSTL();
            Assert.Equal(2, LinAlgUtils.FindFlatFacetsAtZIndex(_stl, 10).Count);
            Assert.Equal(2, LinAlgUtils.FindFlatFacetsAtZIndex(_stl, -10).Count);
            Assert.Empty(LinAlgUtils.FindFlatFacetsAtZIndex(_stl, 2));
        }

        [Fact]
        public void FindTopFlatFacetsAtZIndex_FindsTopFacets_IgnoresBottomFacets()
        {
            LoadSTL();
            // Should Find the right facets at the top and bottom
            Assert.Equal(2, LinAlgUtils.FindTopFlatFacetsAtZIndex(_stl, 10).Count);
            // Should ignore the facets facing the wrong direction
            Assert.Empty(LinAlgUtils.FindTopFlatFacetsAtZIndex(_stl, -10));
        }

        [Fact]
        public void FindBottomFlatFacetsAtZIndex_FindsBottomFacets_IgnoresTopFacets()
        {
            LoadSTL();
            // Should Find the right facets at the top and bottom
            Assert.Equal(2, LinAlgUtils.FindBottomFlatFacetsAtZIndex(_stl, -10).Count);
            // Should ignore the facets facing the wrong direction
            Assert.Empty(LinAlgUtils.FindBottomFlatFacetsAtZIndex(_stl, 10));
        }
        #endregion

        #region Line Math
        [Fact]
        public void CalculateDimensionalValueAtIndex_CalculatesProperValuesAtMultipleAngles()
        {
            // easy math, 45dg all positive
            var p1 = new SlicePoint { X = 0, Y = 0 };
            var p2 = new SlicePoint { X = 10, Y = 10 };
            // crosses both axes
            var p3 = new SlicePoint { X = -3, Y = -1 };
            var p4 = new SlicePoint { X = 10, Y = 7 };
            // crosses Y-Axis, heading down
            var p5 = new SlicePoint { X = -5, Y = -2 };
            var p6 = new SlicePoint { X = 16, Y = -14 };

            // we shouldn't be using this for flat lines, but I'll make sure it works, regardless
            var p7 = new SlicePoint { X = 10, Y = 22 };
            var p8 = new SlicePoint { X = 4, Y = 22 };

            // Each time we should find the same value  regardless of which point is placed first
            Assert.Equal(5, LinAlgUtils.CalculateDimensionalValueAtIndex(p1, p2, 5));
            Assert.Equal(5, LinAlgUtils.CalculateDimensionalValueAtIndex(p2, p1, 5));

            Assert.Equal(3, LinAlgUtils.CalculateDimensionalValueAtIndex(p3, p4, 3.5f));
            Assert.Equal(3, LinAlgUtils.CalculateDimensionalValueAtIndex(p4, p3, 3.5f));

            Assert.Equal(-10, LinAlgUtils.CalculateDimensionalValueAtIndex(p5, p6, 9));
            Assert.Equal(-10, LinAlgUtils.CalculateDimensionalValueAtIndex(p6, p5, 9));

            Assert.Equal(22, LinAlgUtils.CalculateDimensionalValueAtIndex(p7, p8, 16));
            Assert.Equal(22, LinAlgUtils.CalculateDimensionalValueAtIndex(p8, p7, 16));
        }

        [Fact]
        public void CalculateZIntercept_GetsZValuesFromAnyTwoFacets()
        {
            // easy math, 45deg, all positive, from origin
            var v1 = new Vertex { X = 0, Y = 0, Z = 0 };
            var v2 = new Vertex { X = 10, Y = 10, Z = 10 };
            var p1 = new SlicePoint { X = 5, Y = 5 };
            var z1 = 5;
            // crosses x- and y- axes, negative z-slope
            var v3 = new Vertex { X = -5, Y = -10, Z = 15 };
            var v4 = new Vertex { X = 7, Y = 10, Z = 7 };
            var p2 = new SlicePoint { X = 4, Y = 5 };
            var z2 = 9;
            // crosses z-axis
            var v5 = new Vertex { X = 11, Y = 6, Z = -3 };
            var v6 = new Vertex { X = 17, Y = -4, Z = 7 };
            var p3 = new SlicePoint { X = 14, Y = 1 };
            var z3 = 2;

            // Again, this should work regardless of direction
            var rp1 = LinAlgUtils.CalculateZIntercept(v1, v2, z1);
            var rp2 = LinAlgUtils.CalculateZIntercept(v1, v2, z1);
            Assert.True(rp1 == rp2 && rp1 == p1);

            rp1 = LinAlgUtils.CalculateZIntercept(v3, v4, z2);
            rp2 = LinAlgUtils.CalculateZIntercept(v4, v3, z2);
            Assert.True(rp1 == rp2 && rp1 == p2);

            rp1 = LinAlgUtils.CalculateZIntercept(v5, v6, z3);
            rp2 = LinAlgUtils.CalculateZIntercept(v6, v5, z3);
            Assert.True(rp1 == rp2 && rp1 == p3);

        }

        [Fact]
        public void CreateLineFromFacetAtZIndex_CreatesAccurateLine()
        {
            var l1 = new SliceLine
            {
                Points = new List<SlicePoint>
                {
                    new SlicePoint(2, 2),
                    new SlicePoint(-2, -2)
                }
            };
            var f1 = new Facet
            {
                Vertices = new List<Vertex>
                {
                    new Vertex { X = 0, Y = 0, Z = 0 },
                    new Vertex { X = 5, Y = 5, Z = 5 },
                    new Vertex { X = -5, Y = -5, Z = 5 }
                }
            };


        }
        #endregion
    }
}
