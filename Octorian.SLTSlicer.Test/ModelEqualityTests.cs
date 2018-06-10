using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Octorian.STLSlicer.SLA;
using Octorian.STLSlicer.Test.Assets;

namespace Octorian.STLSlicer.Test
{
    public class ModelEqualityTests
    {
        [Fact]
        public void SlicePoint_Equals_Works()
        {
            // Make sure the same variable shows up as equal
            Assert.Equal(TestPoints.p1, TestPoints.p1);
            // Needs to be able to tell when two points are not equal
            Assert.NotEqual(TestPoints.p1, TestPoints.p2);
            // Two points with the same values should register as equal
            Assert.Equal(TestPoints.p3a, TestPoints.p3b);
            // operators should work, too
            Assert.True(TestPoints.p3a == TestPoints.p3b);
            Assert.True(TestPoints.p1 != TestPoints.p2);
        }

        [Fact]
        public void SliceLine_Equals_Works()
        {
            // same object
            Assert.Equal(TestLines.l1a, TestLines.l1a);
            // different objects with same values
            Assert.Equal(TestLines.l1a, TestLines.l1b);
            // shouldn't matter what order the points are in
            Assert.Equal(TestLines.l1a, TestLines.l1c);
            // different Normal values should fail (note this should never happen but still)
            Assert.NotEqual(TestLines.l1a, TestLines.l2a);
            // transposing the x- and y-coordinate should fail
            Assert.NotEqual(TestLines.l1a, TestLines.l2b);
            // operators should work, too
            Assert.True(TestLines.l1a == TestLines.l1c);
            Assert.True(TestLines.l1a != TestLines.l2a);
        }


    }
}
