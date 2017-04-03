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
        private readonly IShapeSource _source;

        /// <summary>
        /// Creates a new reader class for paging through a shape source.
        /// </summary>
        /// <param name="source">The IShapeSource to cycle through.</param>
        public ShapeReader(IShapeSource source)
        {
            _source = source;
            PageSize = 50000;
        }

        /// <summary>
        /// Gets or sets the integer count of shapes that should be allowed to appear on a single page of results.
        /// This is the maximum, and the actual number of shapes may be considerably smaller than this.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the envelope.  If this is null, then no envelope is used.
        /// </summary>
        public Envelope Envelope { get; set; }

        /// <summary>
        /// Gets the feature type of the feature source.
        /// </summary>
        public FeatureType FeatureType
        {
            get { return _source.FeatureType; }
        }

        #region IEnumerable<Dictionary<int,Shape>> Members

        /// <inheritdocs/>
        public IEnumerator<Dictionary<int, Shape>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Nested type: Enumerator

        /// <summary>
        /// Creates an enumerator for pages of shapes returned as dictionaries.
        /// </summary>
        public class Enumerator : IEnumerator<Dictionary<int, Shape>>
        {
            private readonly Envelope _envelope;
            private readonly int _pageSize;
            private readonly IShapeSource _source;
            private int _count;
            private int _index;
            private Dictionary<int, Shape> _page;

            /// <summary>
            /// Creates ShapePageSet.Enumerator which can be used to cycle very large datasets.
            /// </summary>
            /// <param name="parent"></param>
            public Enumerator(ShapeReader parent)
            {
                _source = parent._source;
                _pageSize = parent.PageSize;
                _envelope = parent.Envelope;
                Reset();
            }

            #region IEnumerator<Dictionary<int,Shape>> Members

            /// <inheritdocs/>
            public Dictionary<int, Shape> Current
            {
                get { return _page; }
            }

            object IEnumerator.Current
            {
                get { return _page; }
            }

            /// <inheritdocs/>
            public void Dispose()
            {
            }

            /// <inheritdocs/>
            public bool MoveNext()
            {
                if (_count < 0) _count = _source.GetShapeCount();
                if (_index >= _count) return false;
                _page = _source.GetShapes(ref _index, _pageSize, _envelope);
                return true;
            }

            /// <inheritdocs/>
            public void Reset()
            {
                _page = null;
                _index = 0;
                _count = -1;
            }

            #endregion
        }

        #endregion
    }
}