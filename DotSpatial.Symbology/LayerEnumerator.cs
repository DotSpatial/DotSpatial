// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2008 4:36:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LayerEnumerator
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class LayerLegendEnumerator : IEnumerator<ILegendItem>
    {
        readonly IEnumerator<ILayer> _internalEnumerator;

        /// <summary>
        /// Creates a new instance of LayerEnumerator
        /// </summary>
        public LayerLegendEnumerator(IEnumerator<ILayer> source)
        {
            _internalEnumerator = source;
        }

        #region IEnumerator<ILegendItem> Members

        /// <summary>
        /// Retrieves the current member as an ILegendItem
        /// </summary>
        public ILegendItem Current
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
        /// Returns the current member as an object
        /// </summary>
        object IEnumerator.Current
        {
            get { return _internalEnumerator.Current; }
        }

        /// <summary>
        /// Moves to the next member
        /// </summary>
        /// <returns>boolean, true if the enumerator was able to advance</returns>
        public bool MoveNext()
        {
            return _internalEnumerator.MoveNext();
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