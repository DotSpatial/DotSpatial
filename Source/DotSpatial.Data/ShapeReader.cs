// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/10/2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A class for cycling pages of shapes from a large dataset
    /// </summary>
    public class ShapeReader : IEnumerable<Dictionary<int, Shape>>
    {
        #region Fields

        private readonly IShapeSource _source;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeReader"/> class for paging through a shape source.
        /// </summary>
        /// <param name="source">The IShapeSource to cycle through.</param>
        public ShapeReader(IShapeSource source)
        {
            _source = source;
            PageSize = 50000;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the envelope. If this is null, then no envelope is used.
        /// </summary>
        public Envelope Envelope { get; set; }

        /// <summary>
        /// Gets the feature type of the feature source.
        /// </summary>
        public FeatureType FeatureType => _source.FeatureType;

        /// <summary>
        /// Gets or sets the integer count of shapes that should be allowed to appear on a single page of results.
        /// This is the maximum, and the actual number of shapes may be considerably smaller than this.
        /// </summary>
        public int PageSize { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IEnumerator<Dictionary<int, Shape>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Classes

        /// <summary>
        /// Creates an enumerator for pages of shapes returned as dictionaries.
        /// </summary>
        private class Enumerator : IEnumerator<Dictionary<int, Shape>>
        {
            #region Fields

            private readonly Envelope _envelope;
            private readonly int _pageSize;
            private readonly IShapeSource _source;
            private int _count;
            private int _index;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> class which can be used to cycle very large datasets.
            /// </summary>
            /// <param name="parent">The parent shape reader.</param>
            public Enumerator(ShapeReader parent)
            {
                _source = parent._source;
                _pageSize = parent.PageSize;
                _envelope = parent.Envelope;
                Reset();
            }

            #endregion

            #region Properties

            /// <inheritdoc />
            public Dictionary<int, Shape> Current { get; private set; }

            object IEnumerator.Current => Current;

            #endregion

            #region Methods

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                if (_count < 0) _count = _source.GetShapeCount();
                if (_index >= _count) return false;

                Current = _source.GetShapes(ref _index, _pageSize, _envelope);
                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                Current = null;
                _index = 0;
                _count = -1;
            }

            #endregion
        }

        #endregion
    }
}