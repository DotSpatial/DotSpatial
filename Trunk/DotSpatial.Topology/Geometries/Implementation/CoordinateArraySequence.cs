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
using System.Linq;
using System.Text;

namespace DotSpatial.Topology.Geometries.Implementation
{
    /// <summary>
    /// A <see cref="ICoordinateSequence"/> backed by an array of <see cref="Coordinate"/>s.
    /// This is the implementation that <see cref="IGeometry"/>s use by default.
    /// <para/>
    /// Coordinates returned by <see cref="ToCoordinateArray"/>, <see cref="GetCoordinate(int)"/> and <see cref="GetCoordinate(int, Coordinate)"/> are live --
    /// modifications to them are actually changing the
    /// CoordinateSequence's underlying data.
    /// A dimension may be specified for the coordinates in the sequence,
    /// which may be 2 or 3.
    /// The actual coordinates will always have 3 ordinates,
    /// but the dimension is useful as metadata in some situations. 
    /// </summary>
    [Serializable]
    public class CoordinateArraySequence : ICoordinateSequence
    {
        #region Fields

        protected Coordinate[] Coordinates;
        /**
         * The actual dimension of the coordinates in the sequence.
         * Allowable values are 2 or 3.
         */
        private readonly int _dimension = 3;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new array sequence dimensioned with exactly 1 coordinate that is empty.
        /// </summary>
        public CoordinateArraySequence()
        { //TODO check whether this can be removed
            Coordinates = new Coordinate[1];
            Coordinates[0] = new Coordinate();
        }

        /// <summary>
        /// Creates a new array sequence dimensioned with exactly 1 coordinate, which is set to the coordinate
        /// </summary>
        /// <param name="coordinate">The ICoordinate to use when constructing this sequence</param>
        public CoordinateArraySequence(Coordinate coordinate)
        {//TODO check whether this can be removed
            Coordinates = new Coordinate[1];
            Coordinates[0] = coordinate;
        }

        /// <summary>
        /// Creates a new CoordinateArraySequence whose members are
        /// </summary>
        /// <param name="coordinates">The existing list whose members will be copied to the array.</param>
        public CoordinateArraySequence(IEnumerable<Coordinate> coordinates)
        {//TODO check whether this can be removed
            List<Coordinate> listcoords = new List<Coordinate>();
            foreach (Coordinate c in coordinates)
            {
                listcoords.Add(c);
            }
            Coordinates = listcoords.ToArray();
        }

        /// <summary>
        /// Constructs a sequence based on the given array of <see cref="Coordinate"/>s.
        /// The coordinate dimension is 3
        /// </summary>
        /// <remarks>
        /// The array is not copied.
        /// </remarks>
        /// <param name="coordinates">The coordinate array that will be referenced.</param>
        public CoordinateArraySequence(Coordinate[] coordinates) 
            : this(coordinates, 3) { }

        /// <summary>
        /// Constructs a sequence based on the given array 
        /// of <see cref="Coordinate"/>s.
        /// </summary>
        /// <remarks>The Array is not copied</remarks>
        /// <param name="coordinates">The coordinate array that will be referenced.</param>
        /// <param name="dimension">The dimension of the coordinates</param>
        public CoordinateArraySequence(Coordinate[] coordinates, int dimension)
        {
            Coordinates = coordinates;
            _dimension = dimension;
            if (coordinates == null)
                Coordinates = new Coordinate[0];
        }

        /// <summary>
        /// Constructs a sequence of a given size, populated with new Coordinates.
        /// </summary>
        /// <param name="size">The size of the sequence to create.</param>
        public CoordinateArraySequence(int size)
            : this(size, 3) { }

        /// <summary>
        /// Constructs a sequence of a given <paramref name="size"/>, populated 
        /// with new <see cref="Coordinate"/>s of the given <paramref name="dimension"/>.
        /// </summary>
        /// <param name="size">The size of the sequence to create.</param>
        /// <param name="dimension">the dimension of the coordinates</param>
        public CoordinateArraySequence(int size, int dimension)
        {
            Coordinates = new Coordinate[size];
            _dimension = dimension;
            for (var i = 0; i < size; i++)
                Coordinates[i] = new Coordinate();
        }

        /// <summary>
        /// Creates a new sequence based on a deep copy of the given <see cref="ICoordinateSequence"/>.
        /// </summary>
        /// <param name="coordSeq">The coordinate sequence that will be copied</param>
        public CoordinateArraySequence(ICoordinateSequence coordSeq)
        {
            if (coordSeq == null)
            {
                Coordinates = new Coordinate[0];
                return;
            }

            _dimension = coordSeq.Dimension;
            Coordinates = new Coordinate[coordSeq.Count];

            for (var i = 0; i < Coordinates.Length; i++) 
                Coordinates[i] = coordSeq.GetCoordinateCopy(i);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the length of the coordinate sequence.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return Coordinates.Length;
            }
        }

        /// <summary>
        /// Returns the dimension (number of ordinates in each coordinate) for this sequence.
        /// </summary>
        /// <value></value>
        public virtual int Dimension
        {
            get
            {
                return _dimension;
            }
        }

        public Ordinates Ordinates
        {
            get
            {
                return _dimension == 3 
                    ? Ordinates.XYZ 
                    : Ordinates.XY;
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Direct accessor to an ICoordinate
        /// </summary>
        /// <param name="index">An integer index in this array sequence</param>
        /// <returns>An ICoordinate for the specified index</returns>
        public virtual Coordinate this[int index]
        {//TODO check for remove
            get
            {
                return Coordinates[index];
            }
            set
            {
                Coordinates[index] = value;
                }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a single item to the array.  This is very slow for arrays because it has to
        /// copy all the values into a new array.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(Coordinate item)
        {//TODO check for remove
            Coordinate[] copy = new Coordinate[Coordinates.Length + 1];
            Coordinates.CopyTo(copy, 0);
            copy[Coordinates.Length] = item;
        }

        /// <summary>
        /// Clears the array, reducing the measure to 1 empty coordinate.
        /// </summary>
        public virtual void Clear()
        {//TODO check for remove
            Coordinates = new Coordinate[1];
            Coordinates[0] = new Coordinate();
        }

        /// <summary>
        /// Creates a deep copy of the object.
        /// </summary>
        /// <returns>The deep copy.</returns>
        public virtual object Clone()
        {
            Coordinate[] cloneCoordinates = GetClonedCoordinates();
            return new CoordinateArraySequence(cloneCoordinates);
        }

        /// <summary>
        /// Tests to see if the array contains the specified item
        /// </summary>
        /// <param name="item">the ICoordinate to test</param>
        /// <returns>True if the value is in the array.</returns>
        public virtual bool Contains(Coordinate item)
        {
            foreach (Coordinate coord in Coordinates)
            {
                if (coord == item) return true;
            }
            return false;
        }

        /// <summary>
        /// Copies all of the members from this sequence into the specifeid array.
        /// </summary>
        /// <param name="array">The array of Icoordinates to copy values to.</param>
        /// <param name="arrayIndex">The starting index</param>
        public virtual void CopyTo(Coordinate[] array, int arrayIndex)
        {
            Coordinates.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Expands the given Envelope to include the coordinates in the sequence.
        /// Does not modify the original envelope, but rather returns a copy of the
        /// original envelope after being modified.
        /// </summary>
        /// <param name="env">The envelope use as the starting point for expansion.  This envelope will not be modified.</param>
        /// <returns>The newly created envelope that is the expanded version of the original.</returns>
        public virtual IEnvelope ExpandEnvelope(IEnvelope env)
        {
            IEnvelope cEnv = env.Copy();
            for (int i = 0; i < Coordinates.Length; i++)
                cEnv.ExpandToInclude(Coordinates[i]);
            return cEnv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Coordinate[] GetClonedCoordinates() 
        {
            Coordinate[] cloneCoordinates = new Coordinate[Count];
            for (int i = 0; i < Coordinates.Length; i++) 
                cloneCoordinates[i] = (Coordinate) Coordinates[i].Clone();
            return cloneCoordinates;
        }

        /// <summary>
        /// Get the Coordinate with index i.
        /// </summary>
        /// <param name="i">The index of the coordinate.</param>
        /// <returns>The requested Coordinate instance.</returns>
        public Coordinate GetCoordinate(int i) 
        {
            return Coordinates[i];
        }

        /// <summary>
        /// Copies the i'th coordinate in the sequence to the supplied Coordinate.
        /// </summary>
        /// <param name="index">The index of the coordinate to copy.</param>
        /// <param name="coord">A Coordinate to receive the value.</param>
        public void GetCoordinate(int index, Coordinate coord) 
        {
            coord.X = Coordinates[index].X;
            coord.Y = Coordinates[index].Y;
            coord.Z = Coordinates[index].Z;
        }

        /// <summary>
        /// Get a copy of the Coordinate with index i.
        /// </summary>
        /// <param name="i">The index of the coordinate.</param>
        /// <returns>A copy of the requested Coordinate.</returns>
        public virtual Coordinate GetCoordinateCopy(int i) 
        {
            return new Coordinate(Coordinates[i]);
        }

        ///// <summary>
        ///// Returns an enumerator object for cycling through each of the various members of the array.
        ///// </summary>
        ///// <returns>An Enumerator for this array.</returns>
        //public virtual IEnumerator<Coordinate> GetEnumerator()
        //{ //TODO check for remove
        //    return new Enumerator(_coordinates.GetEnumerator());
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{//TODO check for remove
        //    return _coordinates.GetEnumerator();
        //}

        /// <summary>
        /// Returns the ordinate of a coordinate in this sequence.
        /// Ordinate indices 0 and 1 are assumed to be X and Y.
        /// Ordinate indices greater than 1 have user-defined semantics
        /// (for instance, they may contain other dimensions or measure values).
        /// </summary>
        /// <param name="index">The coordinate index in the sequence.</param>
        /// <param name="ordinate">The ordinate index in the coordinate (in range [0, dimension-1]).</param>
        /// <returns></returns>
        public virtual double GetOrdinate(int index, Ordinate ordinate)
        {
            switch (ordinate)
            {
                case Ordinate.X:  
                    return Coordinates[index].X;
                case Ordinate.Y:  
                    return Coordinates[index].Y;
                case Ordinate.Z:  
                    return Coordinates[index].Z;
                default:
                    return Double.NaN;
            }
        }

        /// <summary>
        /// Returns ordinate X (0) of the specified coordinate.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>
        /// The value of the X ordinate in the index'th coordinate.
        /// </returns>
        public double GetX(int index) 
        {
            return Coordinates[index].X;
        }

        /// <summary>
        /// Returns ordinate Y (1) of the specified coordinate.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>
        /// The value of the Y ordinate in the index'th coordinate.
        /// </returns>
        public double GetY(int index) 
        {
            return Coordinates[index].Y;
        }

        /// <summary>
        /// For arrays, this is very inneficient.  This will copy all the members
        /// except for the item to a new array.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>Boolean, true if a match was found and an item was removed, false otherwise.</returns>
        public virtual bool Remove(Coordinate item)
        { //todo kontrollieren, ob es entfernt werden muss, da in nts nicht enthalten
            Coordinate[] copy = new Coordinate[Coordinates.Length - 1];
            bool result = false;
            for (int i = 0; i < Coordinates.Length; i++)
            {
                // Only remove the first item that matches
                if (item == Coordinates[i] && result == false)
                {
                    // We found the first match
                    result = true;
                }
                else
                {
                    if (result)
                    {
                        // index is one off because we have removed a member
                        copy[i - 1] = Coordinates[i];
                    }
                    else
                    {
                        copy[i] = Coordinates[i];
                    }
                }
            }
            if (result) Coordinates = copy;
            return result;
        }

        public ICoordinateSequence Reversed()
        {
            var coordinates = new Coordinate[Count];
            for (var i = 0; i < Count; i++ )
            {
                coordinates[Count - i - 1] = new Coordinate(Coordinates[i]);
            }
            return new CoordinateArraySequence(coordinates);
        }

        /// <summary>
        /// Sets the value for a given ordinate of a coordinate in this sequence.
        /// </summary>
        /// <param name="index">The coordinate index in the sequence.</param>
        /// <param name="ordinate">The ordinate index in the coordinate (in range [0, dimension-1]).</param>
        /// <param name="value">The new ordinate value.</param>
        public virtual void SetOrdinate(int index, Ordinate ordinate, double value)
        {
            switch (ordinate)
            {
                case Ordinate.X:  
                    Coordinates[index].X = value;
                    break;
                case Ordinate.Y: 
                    Coordinates[index].Y = value;
                    break;
                case Ordinate.Z: 
                    Coordinates[index].Z = value;
                    break;
            }
        }

        /// <summary>
        ///This method exposes the internal Array of Coordinate Objects.       
        /// </summary>
        /// <returns></returns>
        public virtual Coordinate[] ToCoordinateArray()
        {
            return Coordinates;
        }

        /// <summary>
        /// Returns the internal Array as List.
        /// </summary>
        /// <returns></returns>
        public IList<Coordinate> ToList()
        {
            return Coordinates.ToList();
        }

        /// <summary>
        /// Returns the string representation of the coordinate array.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Coordinates.Length > 0) 
            {
                StringBuilder strBuf = new StringBuilder(17 * Coordinates.Length);
                strBuf.Append('(');
                strBuf.Append(Coordinates[0]);
                for (int i = 1; i < Coordinates.Length; i++) 
                {
                    strBuf.Append(", ");
                    strBuf.Append(Coordinates[i]);
                }
                strBuf.Append(')');
                return strBuf.ToString();
            } 
            else return "()";
        }

        #endregion
    }
}