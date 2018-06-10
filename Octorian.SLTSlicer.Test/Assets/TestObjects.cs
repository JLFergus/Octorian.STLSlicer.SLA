using System;
using System.Collections.Generic;
using System.Text;
using Octorian.STLSlicer.SLA;

namespace Octorian.STLSlicer.Test.Assets
{
    public static class TestPoints
    {
        public static SlicePoint p1 = new SlicePoint(0, 0);
        public static SlicePoint p2 = new SlicePoint(5, 3);
        public static SlicePoint p3a = new SlicePoint(-2.3456f, 3.9874f);
        public static SlicePoint p3b = new SlicePoint(-2.3456f, 3.9874f);
    }

    public static class TestLines
    {
        public static  SliceLine l1a = new SliceLine
        {
            Normal = new SlicePoint(.5f, .5f),
            Points = new List<SlicePoint>
                {
                    new SlicePoint(1,2),
                    new SlicePoint(3,3)
                }
        };
        public static SliceLine l1b = new SliceLine
        {
            Normal = new SlicePoint(.5f, .5f),
            Points = new List<SlicePoint>
                {
                    new SlicePoint(1,2),
                    new SlicePoint(3,3)
                }
        };
        public static SliceLine l1c = new SliceLine
        {
            Normal = new SlicePoint(.5f, .5f),
            Points = new List<SlicePoint>
                {
                    new SlicePoint(3,3),
                    new SlicePoint(1,2)
                }
        };
        public static SliceLine l2a = new SliceLine
        {
            Normal = new SlicePoint(.5f, 1),
            Points = new List<SlicePoint>
                {
                    new SlicePoint(1,2),
                    new SlicePoint(3,3)
                }
        };
        public static SliceLine l2b = new SliceLine
        {
            Normal = new SlicePoint(.5f, .5f),
            Points = new List<SlicePoint>
                {
                    new SlicePoint(2,1),
                    new SlicePoint(3,3)
                }
        };
    }
    
    public  class TestShapes
    {

    }
}
