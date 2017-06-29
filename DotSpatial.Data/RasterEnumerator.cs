// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2008 9:28:36 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// RasterEnumerator
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    internal class RasterEnumerator<T> : IEnumerator<Raster<T>>, IEnumerator<IRaster> where T : struct, IEquatable<T>, IComparable<T>
    {
        #region Private Variables

        IEnumerator<Raster<T>> _enumerator;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RasterEnumerator
        /// </summary>
        /// <param name="list">The list to build an enumarator for</param>
        public RasterEnumerator(List<Raster<T>> list)
        {
            _enumerator = list.GetEnumerator();
        }

        #endregion

        #region IEnumerator<IRaster> Members

        IRaster IEnumerator<IRaster>.Current
        {
            get { return _enumerator.Current; }
        }

        #endregion

        #region IEnumerator<Raster<T>> Members

        /// <summary>
        /// Retrieves the current IntRaster from this calculator.
        /// </summary>
        public Raster<T> Current
        {
            get { return _enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }

        void IDisposable.Dispose()
        {
            _enumerator.Dispose();
        }

        bool IEnumerator.MoveNext()
        {
            return _enumerator.MoveNext();
        }

        void IEnumerator.Reset()
        {
            _enumerator.Reset();
        }

        #endregion

        /// <summary>
        /// Disposes any unmanaged memory objects
        /// </summary>
        public void Dispose()
        {
            _enumerator.Dispose();
        }

        /// <summary>
        /// Advances the enumerator to the next member.
        /// </summary>
        /// <returns>A boolean which is false if there are no more members in the list.</returns>
        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        /// <summary>
        /// Resets the enumerator to the position before the start of the list.
        /// </summary>
        public void Reset()
        {
            _enumerator.Reset();
        }
    }
}