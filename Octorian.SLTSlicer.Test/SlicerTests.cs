using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Octorian.STLSlicer.SLA;
using QuantumConcepts.Formats.StereoLithography;
using System.IO;

namespace Octorian.STLSlicer.Test
{
    public class SlicerTests
    {
        private static readonly string _projectPath = $"{Path.GetDirectoryName(Directory.GetCurrentDirectory())}\\..\\..\\Assets\\";
        private static readonly string _testPyramidPath = $"{_projectPath}Test Pyramid.stl";

        private SLASlicer _slicer;

        public SlicerTests()
        {
            bool _failed;
            do
            {
                _failed = false;
                try
                {
                    using (var stlStream = new FileStream(_testPyramidPath, FileMode.Open))
                    {
                        var stl = STLDocument.Read(stlStream, true);
                        if (stl == null)
                        {
                            throw new FileNotFoundException($"File not found: {_testPyramidPath}");
                        }

                        _slicer = new SLASlicer(stl);
                    }

                }
                catch (IOException)
                {
                    _failed = true;
                }
            } while (_failed);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(2.8745f)]
        [InlineData(8.2f)]
        [InlineData(-6.5)]
        [InlineData(-10)]
        public void GetSliceAtZIndex_Works(float z)
        {
            var slice = _slicer.GetSliceAtZIndex(z);
        }
    }
}
