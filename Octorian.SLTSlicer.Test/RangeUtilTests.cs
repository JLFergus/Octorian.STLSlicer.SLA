using System.IO;
using Xunit;
using Octorian.STLSlicer.SLA.Utils;
using QuantumConcepts.Formats.StereoLithography;
using System;

namespace Octorian.STLSlicer.Test
{
    public class RangeUtilTests
    {
        private static readonly string _projectPath = $"{Path.GetDirectoryName(Directory.GetCurrentDirectory())}\\..\\..\\Assets\\";
        private static readonly string _testPyramidPath = $"{_projectPath}Test Pyramid.stl";
        readonly STLDocument _stl;

        public RangeUtilTests()
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

        [Fact]
        public void CalculateHighX_CalculateLowX_ReturnsOuterXBounds()
        {
            Assert.Equal(10, RangeUtils.CalculateHighX(_stl));
            Assert.Equal(-10, RangeUtils.CalculateLowX(_stl));
        }
        [Fact]
        public void CalculateHighY_CalculateLowY_ReturnsOuterYBounds()
        {
            Assert.Equal(10, RangeUtils.CalculateHighY(_stl));
            Assert.Equal(-10, RangeUtils.CalculateLowY(_stl));
        }
        [Fact]
        public void CalculateHighZ_CalculateLowZ_ReturnsOuterZBounds()
        {
            Assert.Equal(10, RangeUtils.CalculateHighZ(_stl));
            Assert.Equal(-10, RangeUtils.CalculateLowZ(_stl));
        }
    }
}
