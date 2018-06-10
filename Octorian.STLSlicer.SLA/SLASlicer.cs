using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using QuantumConcepts.Formats.StereoLithography;
using Octorian.STLSlicer.SLA.Utils;

namespace Octorian.STLSlicer.SLA
{
    public class SLASlicer
    {
        protected STLDocument _stl;
        protected Dictionary<float, Slice> _slices = new Dictionary<float, Slice>();

        private float? _highX, _lowX, _highY, _lowY, _highZ, _lowZ;

        #region constructors
        public SLASlicer(STLDocument stl) { _stl = stl; }

        public SLASlicer(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                _stl = STLDocument.Read(stream, true);
                if (_stl == null)
                    throw new FileNotFoundException($"File not found: {path}");
            }
        }
        #endregion

        #region range properties
        public float HighX
        {
            get
            {
                if (_highX == null)
                    _highX = RangeUtils.CalculateHighX(_stl);

                return (float)_highX;
            }
        }

        public float LowX
        {
            get
            {
                if (_lowX == null)
                    _lowX = RangeUtils.CalculateLowX(_stl);

                return (float)_lowX;
            }
        }

        public float HighY
        {
            get
            {
                if (_highY == null)
                    _highY = RangeUtils.CalculateHighY(_stl);

                return (float)_highY;
            }
        }

        public float LowY
        {
            get
            {
                if (_lowY == null)
                    _lowY = RangeUtils.CalculateLowY(_stl);

                return (float)_lowY;
            }
        }

        public float HighZ
        {
            get
            {
                if (_highZ == null)
                    _highZ = RangeUtils.CalculateHighZ(_stl);

                return (float) _highZ;
            }
        }

        public float LowZ
        {
            get
            {
                if (_lowZ == null)
                    _lowZ = RangeUtils.CalculateLowZ(_stl);

                return (float)_lowZ;
            }
        }
        #endregion

        #region Slice Methods
        public Slice GetSliceAtZIndex(float z)
        {
            // cache this in the event you have to retrieve a single value more than once,
            // we don't want to have to do this math again
            if (!_slices.Keys.Contains(z))
            {
                var facets = LinAlgUtils.FindFacetsIntersectingZIndex(_stl, z);
                _slices[z] = new Slice
                {
                    Lines = facets.Select(f => LinAlgUtils.CreateLineFromFacetAtZIndex(f, z)).ToList()
                };
            }

            return _slices[z];
        }

        public Dictionary<float, Slice> SliceModel(float layerHeight)
        {
            var newDict = new Dictionary<float, Slice>();
            for (var z = RangeUtils.CalculateLowZ(_stl); z <= RangeUtils.CalculateHighZ(_stl); z+= layerHeight)
            {
                if (_slices.Keys.Contains(z))
                    newDict[z] = _slices[z];
                else
                {
                    newDict[z] = GetSliceAtZIndex(z);
                    _slices[z] = newDict[z];
                }
            }
            return newDict;
        }
        #endregion
    }
}
