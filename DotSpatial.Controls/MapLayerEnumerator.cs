using System.Collections;
using System.Collections.Generic;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Transforms an IMapLayer enumerator into an ILayer Enumerator
    /// </summary>
    public class MapLayerEnumerator : IEnumerator<ILayer>
    {
        private readonly IEnumerator<IMapLayer> _enumerator;

        /// <summary>
        /// Creates a new instance of the MapLayerEnumerator
        /// </summary>
        /// <param name="subEnumerator"></param>
        public MapLayerEnumerator(IEnumerator<IMapLayer> subEnumerator)
        {
            _enumerator = subEnumerator;
        }

        #region IEnumerator<ILayer> Members

        /// <inheritdoc />
        public ILayer Current
        {
            get { return _enumerator.Current; }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _enumerator.Dispose();
        }

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        /// <inheritdoc />
        public void Reset()
        {
            _enumerator.Reset();
        }

        #endregion
    }
}