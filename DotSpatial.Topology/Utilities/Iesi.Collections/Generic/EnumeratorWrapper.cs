// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// Simple Wrapper for wrapping an regular Enumerator as a generic Enumberator&lt;T&gt;
    /// </summary>
    /// <typeparam name="T">The type of the enumeration wrapper.</typeparam>
    /// <exception cref="InvalidCastException">
    /// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
    /// </exception>
    public struct EnumeratorWrapper<T> : IEnumerator<T>
    {
        private IEnumerator _innerEnumerator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="toWrap"></param>
        public EnumeratorWrapper(IEnumerator toWrap)
        {
            _innerEnumerator = toWrap;
        }

        #region IEnumerator<T> Members

        /// <summary>
        /// The current member being enumerated
        /// </summary>
        public T Current
        {
            get { return (T)_innerEnumerator.Current; }
        }

        /// <summary>
        /// Disposes the innerEnumerator
        /// </summary>
        public void Dispose()
        {
            _innerEnumerator = null;
        }

        object IEnumerator.Current
        {
            get { return _innerEnumerator.Current; }
        }

        /// <summary>
        /// Moves to the next element
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            return _innerEnumerator.MoveNext();
        }

        /// <summary>
        /// Resets the enumerator to the starting position
        /// </summary>
        public void Reset()
        {
            _innerEnumerator.Reset();
        }

        #endregion
    }
}