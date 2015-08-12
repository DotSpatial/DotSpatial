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

using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Topology.Geometries.Implementation
{
    /// <summary>
    /// An ICoordinateSequence based on a list instead of an array.
    /// </summary>
    public class CoordinateListSequence : ICoordinateSequence
    {
        #region Fields

        private List<Coordinate> _internalList;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a CoordinateListSequence
        /// </summary>
        /// <param name="coordinate"></param>
        public CoordinateListSequence(Coordinate coordinate)
        {
            _internalList = new List<Coordinate> { coordinate };
        }

        /// <summary>
        /// Creates a new instance of a CoordinateListSequence
        /// </summary>
        public CoordinateListSequence()
        {
            _internalList = new List<Coordinate>();
        }

        /// <summary>
        /// Creates a new instance of a CoordinateListSequence
        /// </summary>
        /// <param name="coordinates">If this is a List of ICoordinates, the sequence will be a shallow list.
        /// Otherwise, a List is created and shallow copies of each coordinate is added.</param>
        public CoordinateListSequence(IEnumerable<Coordinate> coordinates)
        {
            _internalList = coordinates as List<Coordinate> ?? coordinates.ToList();
        }

        /// <summary>
        /// Creates a new instance of CoordinateListSequence
        /// </summary>
        /// <param name="sequence">The sequence to use</param>
        public CoordinateListSequence(ICoordinateSequence sequence)
        {
            List<Coordinate> list = new List<Coordinate>();
            for (int i = 0; i <= sequence.Count; i++)
            {
                list.Add(sequence[i].Clone() as Coordinate);
            }
            _internalList = list;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an integer representing the number of ICoordinates currently stored in this list
        /// </summary>
        public int Count
        {
            get { return _internalList.Count; }
        }

        /// <summary>
        /// This always returns 0
        /// </summary>
        public int Dimension
        {
            get { return 0; }
        }

        public Ordinates Ordinates { get; private set; }

        /// <summary>
        /// The CoordinateListSequence is not readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Direct accessor to an ICoordinate
        /// </summary>
        /// <param name="index">An integer index in this array sequence</param>
        /// <returns>An ICoordinate for the specified index</returns>
        public virtual Coordinate this[int index]
        {
            get
            {
                return _internalList[index];
            }
            set
            {
                _internalList[index] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an item to the end of the CoordinateList
        /// </summary>
        /// <param name="item">An ICoordinate to add</param>
        public void Add(Coordinate item)
        {
            _internalList.Add(item);
        }

        /// <summary>
        /// Clears the list entirely.
        /// </summary>
        public void Clear()
        {
            _internalList = new List<Coordinate>();
        }

        /// <summary>
        /// Creates a deep copy of this object
        /// </summary>
        /// <returns>CoordinateListSequence</returns>
        public object Clone()
        {
            return new CoordinateListSequence(_internalList);
        }

        /// <summary>
        /// Tests wheather or not the specified ICoordinate is contained in the list.
        /// </summary>
        /// <param name="item">An ICoordinate to test</param>
        /// <returns>Boolean, true if the list contains the item.</returns>
        public bool Contains(Coordinate item)
        {
            return _internalList.Contains(item);
        }

        /// <summary>
        /// Copies all the members of this list into the specified array.
        /// </summary>
        /// <param name="array">The array to copy values to.</param>
        /// <param name="arrayIndex">The 0 based integer index in the array at which copying begins</param>
        public void CopyTo(Coordinate[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Expands the given Envelope to include the coordinates in the sequence.
        /// Does not modify the original envelope, but rather returns a copy of the
        /// original envelope after being modified.
        /// </summary>
        /// <param name="env">The envelope use as the starting point for expansion.  This envelope will not be modified.</param>
        /// <returns>The newly created envelope that is the expanded version of the original.</returns>
        public IEnvelope ExpandEnvelope(IEnvelope env)
        {
            IEnvelope temp = env.Copy();
            foreach (Coordinate coord in _internalList)
            {
                temp.ExpandToInclude(coord);
            }
            return temp;
        }

        /// <summary>
        /// Returns (possibly a copy of) the ith Coordinate in this collection.
        /// Whether or not the Coordinate returned is the actual underlying
        /// Coordinate or merely a copy depends on the implementation.
        /// Note that in the future the semantics of this method may change
        /// to guarantee that the Coordinate returned is always a copy. Callers are
        /// advised not to assume that they can modify a CoordinateSequence by
        /// modifying the Coordinate returned by this method.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Coordinate GetCoordinate(int i)
        {
            return _internalList[i];
        }

        /// <summary>
        /// Copies the ordinate values of the coordinate at the given index to coord.
        /// </summary>
        /// <param name="index">Index of the coordinate whose ordinates should be copied.</param>
        /// <param name="coord">Coordinate the values get copied to.</param>
        public void GetCoordinate(int index, Coordinate coord)
        {
            coord = _internalList[index].Copy();
        }

        /// <summary>
        /// Returns a copy of the i'th coordinate in this sequence.
        /// This method optimizes the situation where the caller is
        /// going to make a copy anyway - if the implementation
        /// has already created a new Coordinate object, no further copy is needed.
        /// </summary>
        /// <param name="i">The index of the coordinate to retrieve.</param>
        /// <returns>A copy of the i'th coordinate in the sequence</returns>
        public Coordinate GetCoordinateCopy(int i)
        {
            return _internalList[i].Copy();
        }

        /// <summary>
        /// Obtains an enumerator for cycling through the list.  BEWARE!  This
        /// will return an underlying list enumerator, and so code that uses
        /// a foreach process or an enumerator will bypass the Version incrementing code.
        /// </summary>
        /// <returns>An Enumerator object for cycling through the list.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return _internalList.GetEnumerator();
        //}

        /// <summary>
        /// Returns the ordinate of a coordinate in this sequence.
        /// Ordinate indices 0 and 1 are assumed to be X and Y.
        /// Ordinate indices greater than 1 have user-defined semantics
        /// (for instance, they may contain other dimensions or measure values).         
        /// </summary>
        /// <remarks>
        /// If the sequence does not provide value for the required ordinate, the implementation <b>must not</b> throw an exception, it should return <see cref="Coordinate.NullOrdinate"/>.
        /// </remarks>
        /// <param name="index">The coordinate index in the sequence.</param>
        /// <param name="ordinate">The ordinate index in the coordinate (in range [0, dimension-1]).</param>
        /// <returns>The ordinate value, or <see cref="Coordinate.NullOrdinate"/> if the sequence does not provide values for <paramref name="ordinate"/>"/></returns>       
        public double GetOrdinate(int index, Ordinate ordinate)
        {
            return _internalList[index][ordinate];
        }

        /// <summary>
        /// Returns ordinate X (0) of the specified coordinate.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The value of the X ordinate in the index'th coordinate.</returns>
        public double GetX(int index)
        {
            return _internalList[index].X;
        }

        /// <summary>
        /// Returns ordinate Y (1) of the specified coordinate.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The value of the Y ordinate in the index'th coordinate.</returns>
        public double GetY(int index)
        {
            return _internalList[index].Y;
        }

        /// <summary>
        /// Removes the specified ICoordinate from the list.
        /// </summary>
        /// <param name="item">An ICoordinate that should be removed from the list</param>
        /// <returns>Boolean, true if the item was successfully removed.</returns>
        public bool Remove(Coordinate item)
        {
            return _internalList.Remove(item);
        }

        /// <summary>
        /// Creates a reversed version of this coordinate sequence with cloned <see cref="Coordinate"/>s.
        /// </summary>
        /// <returns>A reversed version of this sequence</returns>
        public ICoordinateSequence Reversed()
        {
            List<Coordinate> l = _internalList.CloneList();
            l.Reverse();
            return new CoordinateListSequence(l);
        }

        /// <summary>
        /// Sets the value for a given ordinate of a coordinate in this sequence.       
        /// </summary>
        /// <remarks>
        /// If the sequence can't store the ordinate value, the implementation <b>must not</b> throw an exception, it should simply ignore the call.
        /// </remarks>
        /// <param name="index">The coordinate index in the sequence.</param>
        /// <param name="ordinate">The ordinate index in the coordinate (in range [0, dimension-1]).</param>
        /// <param name="value">The new ordinate value.</param>       
        public void SetOrdinate(int index, Ordinate ordinate, double value)
        {
            _internalList[index][ordinate] = value;
        }

        /// <summary>
        /// Copies all the members to an ICoordiante[]
        /// </summary>
        /// <returns></returns>
        public Coordinate[] ToCoordinateArray()
        {
            return _internalList.ToArray();
        }

        #endregion
    }
}