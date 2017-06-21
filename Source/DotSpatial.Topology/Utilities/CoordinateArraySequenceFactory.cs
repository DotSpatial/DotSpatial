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
using System.Collections.Generic;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Creates CoordinateSequences represented as an array of Coordinates.
    /// </summary>
    [Serializable]
    public sealed class CoordinateArraySequenceFactory : ICoordinateSequenceFactory
    {
        /// <summary>
        /// This appears to be part of a singleton for a coordinate array sequence factory
        /// </summary>
        public static readonly CoordinateArraySequenceFactory Instance = new CoordinateArraySequenceFactory();

        /// <summary>
        ///
        /// </summary>
        private CoordinateArraySequenceFactory() { }

        #region ICoordinateSequenceFactory Members

        /// <summary>
        ///  Returns a CoordinateArraySequence based on the given array (the array is not copied).
        /// </summary>
        /// <param name="coordinates">the coordinates, which may not be null nor contain null elements.</param>
        /// <returns></returns>
        public ICoordinateSequence Create(IEnumerable<Coordinate> coordinates)
        {
            return new CoordinateArraySequence(coordinates);
        }

        /// <summary>
        /// Constructs a new coordinate sequence using a single coordinate.
        /// </summary>
        /// <param name="coord">A single coordinate to be used in a coordinate sequence factory.</param>
        /// <returns>A valid ICoordinateSequence.</returns>
        public ICoordinateSequence Create(Coordinate coord)
        {
            return new CoordinateArraySequence(coord);
        }

        /// <summary>
        /// Constructs either an array with 1 member or an empty list, depending on the implementation.
        /// </summary>
        /// <returns>A new seqeunce</returns>
        public ICoordinateSequence Create()
        {
            return new CoordinateArraySequence();
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordSeq"></param>
        /// <returns></returns>
        public ICoordinateSequence Create(ICoordinateSequence coordSeq)
        {
            return new CoordinateArraySequence(coordSeq);
        }
    }
}