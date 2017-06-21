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
using System.Text;

namespace DotSpatial.Topology
{
    /// <summary>
    /// The <c>ICoordinateSequence</c> implementation that <c>Geometry</c>s use by default.
    /// In this implementation, Coordinates returned by ToArray and Coordinate are live --
    /// modifications to them are actually changing the
    /// CoordinateSequence's underlying data.
    /// </summary>
    [Serializable]
    public class CoordinateArraySequence : ICoordinateSequence
    {
        /// <summary>
        /// Increments the version with the understanding that we are using integers.  If the user
        /// uses too many version, it will cycle around.  The statistical odds of accidentally
        /// reaching the same value as a previous version should be small enough to prevent
        /// conflicts.
        /// </summary>
        private void IncrementVersion()
        {
            if (_versionId == int.MaxValue)
            {
                _versionId = int.MinValue;
            }
            else
            {
                _versionId += 1;
            }
        }

        #region Nested type: Enumerator

        /// <summary>
        /// CoordinateArraySequenceEnumerator
        /// </summary>
        public class Enumerator : IEnumerator<Coordinate>
        {
            #region Private Variables

            readonly IEnumerator _baseEnumerator;

            #endregion

            #region Constructors

            /// <summary>
            /// Creates a new instance of CoordinateArraySequenceEnumerator
            /// </summary>
            public Enumerator(IEnumerator inBaseEnumerator)
            {
                _baseEnumerator = inBaseEnumerator;
            }

            #endregion

            #region IEnumerator<Coordinate> Members

            /// <summary>
            /// Gets the current member
            /// </summary>
            public Coordinate Current
            {
                get { return _baseEnumerator.Current as Coordinate; }
            }

            object IEnumerator.Current
            {
                get { return _baseEnumerator.Current; }
            }

            /// <summary>
            /// Does nothing
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Advances the enumerator to the next member
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return _baseEnumerator.MoveNext();
            }

            /// <summary>
            /// Resets the enumerator to the original position
            /// </summary>
            public void Reset()
            {
                _baseEnumerator.Reset();
            }

            #endregion
        }

        #endregion

        #region Private Variables

        private Coordinate[] _coordinates;
        private int _versionId;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new array sequence dimensioned with exactly 1 coordinate that is empty.
        /// </summary>
        public CoordinateArraySequence()
        {
            Coordinate[] coords = new Coordinate[1];
            coords[0] = new Coordinate();
            Configure(coords);
        }

        /// <summary>
        /// Creates a new array sequence dimensioned with exactly 1 coordinate, which is set to the coordinate
        /// </summary>
        /// <param name="coordinate">The ICoordinate to use when constructing this sequence</param>
        public CoordinateArraySequence(Coordinate coordinate)
        {
            Coordinate[] coords = new Coordinate[1];
            coords[0] = coordinate;
            Configure(coords);
        }

        /// <summary>
        /// Creates a new CoordinateArraySequence whose members are
        /// </summary>
        /// <param name="coordinates">The existing list whose members will be copied to the array.</param>
        public CoordinateArraySequence(IEnumerable<Coordinate> coordinates)
        {
            List<Coordinate> listcoords = new List<Coordinate>();
            foreach (Coordinate c in coordinates)
            {
                listcoords.Add(c);
            }
            Coordinate[] coords = listcoords.ToArray();
            Configure(coords);
        }

        /// <summary>
        /// Constructs a sequence based on the given array (the array is not copied).
        /// </summary>
        /// <param name="coordinates">The coordinate array that will be referenced.</param>
        public CoordinateArraySequence(Coordinate[] coordinates)
        {
            Coordinate[] coords = coordinates;
            if (coordinates == null)
            {
                coords = new Coordinate[1];
                coords[0] = new Coordinate();
            }
            Configure(coords);
        }

        /// <summary>
        /// Constructs a sequence of a given size, populated with new Coordinates.
        /// </summary>
        /// <param name="size">The size of the sequence to create.</param>
        public CoordinateArraySequence(int size)
        {
            Coordinate[] coords = new Coordinate[size];

            for (int i = 0; i < size; i++)
            {
                coords[i] = new Coordinate();
            }

            Configure(coords);
        }

        /// <summary>
        /// Constructs a sequence based on the given array (the array is not copied).
        /// </summary>
        /// <param name="coordSeq">The coordinate array that will be referenced.</param>
        public CoordinateArraySequence(ICoordinateSequence coordSeq)
        {
            Coordinate[] coords = coordSeq != null ? new Coordinate[coordSeq.Count] : new Coordinate[0];
            for (int i = 0; i < coords.Length; i++)
            {
                if (coordSeq != null) coords[i] = coordSeq[i].Clone() as Coordinate;
            }
            Configure(coords);
        }

        // This is called by all of the constructors to try to reduce redundancy
        private void Configure(Coordinate[] inCoords)
        {
            _coordinates = inCoords;
            _versionId = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a single item to the array.  This is very slow for arrays because it has to
        /// copy all the values into a new array.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(Coordinate item)
        {
            Coordinate[] copy = new Coordinate[_coordinates.Length + 1];
            _coordinates.CopyTo(copy, 0);
            copy[_coordinates.Length] = item;
            IncrementVersion();
        }

        /// <summary>
        /// Clears the array, reducing the measure to 1 empty coordinate.
        /// </summary>
        public virtual void Clear()
        {
            _coordinates = new Coordinate[1];
            _coordinates[0] = new Coordinate();
            IncrementVersion();
        }

        /// <summary>
        /// Tests to see if the array contains the specified item
        /// </summary>
        /// <param name="item">the ICoordinate to test</param>
        /// <returns>True if the value is in the array.</returns>
        public virtual bool Contains(Coordinate item)
        {
            foreach (Coordinate coord in _coordinates)
            {
                if (coord == item) return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a deep copy of the object.
        /// </summary>
        /// <returns>The deep copy.</returns>
        public virtual object Clone()
        {
            Coordinate[] cloneCoordinates = new Coordinate[Count];
            for (int i = 0; i < _coordinates.Length; i++)
                cloneCoordinates[i] = new Coordinate(_coordinates[i]);
            return new CoordinateArraySequence(cloneCoordinates);
        }

        /// <summary>
        /// Copies all of the members from this sequence into the specifeid array.
        /// </summary>
        /// <param name="array">The array of Icoordinates to copy values to.</param>
        /// <param name="arrayIndex">The starting index</param>
        public virtual void CopyTo(Coordinate[] array, int arrayIndex)
        {
            _coordinates.CopyTo(array, arrayIndex);
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
            for (int i = 0; i < _coordinates.Length; i++)
            {
                cEnv.ExpandToInclude(_coordinates[i]);
            }
            return cEnv;
        }

        /// <summary>
        /// Returns an enumerator object for cycling through each of the various members of the array.
        /// BEWARE!  This will bypass the VersionID code.  Since ICoordinateSequence does not contain
        /// the GetEnumerator methods, using an ICoordinateSequence should safely advance the version
        /// during changes.
        /// </summary>
        /// <returns>An Enumerator for this array.</returns>
        public virtual IEnumerator<Coordinate> GetEnumerator()
        {
            return new Enumerator(_coordinates.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _coordinates.GetEnumerator();
        }

        /// <summary>
        /// Returns the ordinate of a coordinate in this sequence.
        /// Ordinate indices 0 and 1 are assumed to be X and Y.
        /// Ordinates indices greater than 1 have user-defined semantics
        /// (for instance, they may contain other dimensions or measure values).
        /// </summary>
        /// <param name="index">The coordinate index in the sequence.</param>
        /// <param name="ordinate">The ordinate index in the coordinate (in range [0, dimension-1]).</param>
        /// <returns></returns>
        public virtual double GetOrdinate(int index, Ordinate ordinate)
        {
            switch (ordinate)
            {
                case Ordinate.X: return _coordinates[index].X;
                case Ordinate.Y: return _coordinates[index].Y;
                case Ordinate.Z: return _coordinates[index].Z;
                default:
                    return Double.NaN;
            }
        }

        /// <summary>
        /// For arrays, this is very inneficient.  This will copy all the members
        /// except for the item to a new array.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>Boolean, true if a match was found and an item was removed, false otherwise.</returns>
        public virtual bool Remove(Coordinate item)
        {
            Coordinate[] copy = new Coordinate[_coordinates.Length - 1];
            bool result = false;
            for (int i = 0; i < _coordinates.Length; i++)
            {
                // Only remove the first item that matches
                if (item == _coordinates[i] && result == false)
                {
                    // We found the first match
                    result = true;
                }
                else
                {
                    if (result)
                    {
                        // index is one off because we have removed a member
                        copy[i - 1] = _coordinates[i];
                    }
                    else
                    {
                        copy[i] = _coordinates[i];
                    }
                }
            }
            if (result) _coordinates = copy;
            return result;
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
                case Ordinate.X: _coordinates[index].X = value; break;
                case Ordinate.Y: _coordinates[index].Y = value; break;
                case Ordinate.Z: _coordinates[index].Z = value; break;
                case Ordinate.M: _coordinates[index].M = value; break;
                default:
                    throw new ArgumentException("invalid ordinate index: " + ordinate);
            }
            IncrementVersion();
        }

        /// <summary>
        ///This method exposes the internal Array of Coordinate Objects.
        /// </summary>
        /// <returns></returns>
        public virtual Coordinate[] ToCoordinateArray()
        {
            return _coordinates;
        }

        /// <summary>
        /// Returns the string representation of the coordinate array.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_coordinates.Length > 0)
            {
                StringBuilder strBuf = new StringBuilder(17 * _coordinates.Length);
                strBuf.Append('(');
                strBuf.Append(_coordinates[0]);
                for (int i = 1; i < _coordinates.Length; i++)
                {
                    strBuf.Append(", ");
                    strBuf.Append(_coordinates[i]);
                }
                strBuf.Append(')');
                return strBuf.ToString();
            }
            return "()";
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
                return _coordinates.Length;
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
                return 3;
            }
        }

        /// <summary>
        /// CoordinateArraySequences are not ReadOnly
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Direct accessor to an ICoordinate
        /// </summary>
        /// <param name="index">An integer index in this array sequence</param>
        /// <returns>An ICoordinate for the specified index</returns>
        public virtual Coordinate this[int index]
        {
            get
            {
                return _coordinates[index];
            }
            set
            {
                _coordinates[index] = value;
                IncrementVersion();
            }
        }

        /// <summary>
        /// Every time a member is added to, subtracted from, or edited in the sequence,
        /// this VersionID is incremented.  This doesn't uniquely separate this sequence
        /// from other sequences, but rather acts as a way to rapidly track if changes
        /// have occured against a cached version.
        /// </summary>
        public int VersionID
        {
            get { return _versionId; }
        }

        #endregion
    }
}