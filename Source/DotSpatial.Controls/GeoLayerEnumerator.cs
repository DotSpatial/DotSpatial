// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2008 11:01:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public class GeoLayerEnumerator : IEnumerator<IMapLayer>
    {
        readonly IEnumerator<ILayer> _internalEnumerator;

        /// <summary>
        /// Creates a new instance of LayerEnumerator
        /// </summary>
        public GeoLayerEnumerator(IEnumerator<ILayer> source)
        {
            _internalEnumerator = source;
        }

        #region IEnumerator<IMapLayer> Members

        /// <summary>
        /// Retrieves the current member as an ILegendItem
        /// </summary>
        public IMapLayer Current
        {
            get
            {
                return _internalEnumerator.Current as IMapLayer;
            }
        }

        object IEnumerator.Current
        {
            get { return _internalEnumerator.Current; }
        }

        /// <summary>
        /// Calls the Dispose method
        /// </summary>
        public void Dispose()
        {
            _internalEnumerator.Dispose();
        }

        /// <summary>
        /// Moves to the next member
        /// </summary>
        /// <returns>boolean, true if the enumerator was able to advance</returns>
        public bool MoveNext()
        {
            while (_internalEnumerator.MoveNext())
            {
                var result = _internalEnumerator.Current as IMapLayer;
                if (result != null) return true;
            }
            return false;
        }

        /// <summary>
        /// Resets to before the first member
        /// </summary>
        public void Reset()
        {
            _internalEnumerator.Reset();
        }

        #endregion
    }
}