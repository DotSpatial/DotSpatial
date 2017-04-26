// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2009 4:55:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Ported and cleaned up the raw source from code project
// ********************************************************************************************************

using System;

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The VEvent.
    /// </summary>
    internal abstract class VEvent : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public abstract double Y { get; }

        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        protected abstract double X { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Compares this event to the other event.
        /// </summary>
        /// <param name="obj">Second VEvent to compare.</param>
        /// <returns>True, if the events are equal.</returns>
        /// <exception cref="ArgumentException">Thrown if obj is not a VEvent</exception>
        public int CompareTo(object obj)
        {
            if (!(obj is VEvent))
                throw new ArgumentException("obj is not a VEvent!");
            int i = Y.CompareTo(((VEvent)obj).Y);
            return i != 0 ? i : X.CompareTo(((VEvent)obj).X);
        }

        #endregion
    }
}