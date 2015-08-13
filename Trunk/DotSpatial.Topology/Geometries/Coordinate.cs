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
using DotSpatial.Serialization;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// A lightweight class used to store coordinates on the 2-dimensional Cartesian plane.
    /// <para>
    /// It is distinct from <see cref="IPoint"/>, which is a subclass of <see cref="IGeometry"/>.
    /// Unlike objects of type <see cref="IPoint"/> (which contain additional
    /// information such as an envelope, a precision model, and spatial reference
    /// system information), a <other>Coordinate</other> only contains ordinate values
    /// and propertied.
    /// </para>
    /// <para>
    /// <other>Coordinate</other>s are two-dimensional points, with an additional Z-ordinate.    
    /// If an Z-ordinate value is not specified or not defined,
    /// constructed coordinates have a Z-ordinate of <code>NaN</code>
    /// (which is also the value of <see cref="NullOrdinate"/>).
    /// </para>
    /// </summary>
    /// <remarks>
    /// Apart from the basic accessor functions, NTS supports
    /// only specific operations involving the Z-ordinate.
    /// </remarks>
    [Serializable]
    public class Coordinate : ICloneable, IComparable<Coordinate>, IEquatable<Coordinate>, IComparable
    {
        #region Constant Fields

        ///<summary>
        /// The value used to indicate a null or missing ordinate value.
        /// In particular, used for the value of ordinates for dimensions
        /// greater than the defined dimension of a coordinate.
        ///</summary>
        public const double NullOrdinate = Double.NaN;

        #endregion

        #region Fields

        /// <summary>
        /// An optional place holder for a measure value if needed
        /// </summary>
        [Serialize("M")]
        public double M;

        /// <summary>
        /// The X or horizontal, or longitudinal ordinate
        /// </summary>
        [Serialize("X")]
        public double X;

        /// <summary>
        /// The Y or vertical, or latitudinal ordinate
        /// </summary>
        [Serialize("Y")]
        public double Y;

        /// <summary>
        /// The Z or up or altitude ordinate
        /// </summary>
        [Serialize("Z")]
        public double Z;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an empty coordinate
        /// </summary>
        public Coordinate() : this(NullOrdinate, NullOrdinate, NullOrdinate, NullOrdinate) { }

        /// <summary>
        /// Creates a 2D coordinate with NaN for the Z and M values
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public Coordinate(double x, double y) : this(x, y, NullOrdinate, NullOrdinate) { }

        /// <summary>
        /// Creates a new coordinate with the specified values.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        public Coordinate(double x, double y, double z) : this(x, y, z, NullOrdinate) { }

        /// <summary>
        /// Creates a new coordinate with the values in X, Y, Z order, or X, Y order.
        /// </summary>
        /// <param name="values"></param>
        public Coordinate(double[] values)
        {
            if (values == null) return;
            for (int i = 0; i < Math.Min(values.Length, 3); i++)
                this[i] = values[i];
        }

        /// <summary>
        /// Constructs a new instance of the coordinate
        /// </summary>
        /// <param name="c"></param>
        public Coordinate(ICoordinate c)
        {
            if (c == null || c.Values == null) return;
            for (int i = 0; i < Math.Min(c.Values.Length, 3); i++)
                this[i] = c.Values[i];
        }

        /// <summary>
        /// Constructs a new instance of the coordinate
        /// </summary>
        /// <param name="c"></param>
        public Coordinate(Coordinate c)
        {
            if (c == null) return;
            double[] values = c.ToArray();
            for (int i = 0; i < Math.Min(values.Length, 3); i++)
                this[i] = values[i];
        }

        /// <summary>
        /// Creates a new instance of Coordinate
        /// </summary>
        public Coordinate(double x, double y, double z, double m)
        {
            X = x;
            Y = y;
            Z = z;
            M = m;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets <other>Coordinate</other>s (x,y,z) values.
        /// </summary>
        public Coordinate CoordinateValue
        {
            get { return this; }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets a pre-defined coordinate that has double.NaN for all the values.
        /// </summary>
        public static Coordinate Empty
        {
            get { return new Coordinate(); }
        }

        /// <summary>
        /// This is not a true length, but simply tests the Z value.  If the Z value
        /// is NaN then the value is 2.  Otherwise this is 3.
        /// </summary>
        public int NumOrdinates
        {
            get
            {
                return double.IsNaN(Z) ? 2 : 3;
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the double value associated with the specified ordinate index
        /// </summary>
        /// <param name="index">The zero-based integer index of the ordinate</param>
        /// <returns>A double value representing a single ordinate</returns>
        public double this[int index]
        {
            get
            {
                if (index < 0 || index > 3) throw new ArgumentOutOfRangeException("index");
                return this[(Ordinate)index];
            }
            set
            {
                if (index < 0 || index > 3) throw new ArgumentOutOfRangeException("index");
                this[(Ordinate)index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ordinate value for the given index.
        /// The supported values for the index are 
        /// <see cref="Ordinate.X"/>, <see cref="Ordinate.Y"/> and <see cref="Ordinate.Z"/>.
        /// </summary>
        /// <param name="ordinateIndex">The ordinate index</param>
        /// <returns>The ordinate value</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="ordinateIndex"/> is not in the valid range.</exception>
        public double this[Ordinate ordinateIndex]
        {
            get
            {
                switch (ordinateIndex)
                {
                    case Ordinate.X:
                        return X;
                    case Ordinate.Y:
                        return Y;
                    case Ordinate.Z:
                        return Z;
                    case Ordinate.M:
                        return M;
                }
                throw new ArgumentOutOfRangeException("ordinateIndex");
            }
            set
            {
                switch (ordinateIndex)
                {
                    case Ordinate.X:
                        X = value;
                        return;
                    case Ordinate.Y:
                        Y = value;
                        return;
                    case Ordinate.Z:
                        Z = value;
                        return;
                    case Ordinate.M:
                        M = value;
                        return;
                }
                throw new ArgumentOutOfRangeException("ordinateIndex");
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(Coordinate coord1, Coordinate coord2)
        {
            // returns Coordinate as a specific implementation of ICoordinate
            return new Coordinate(coord1.X + coord2.X, coord1.Y + coord2.Y, coord1.Z + coord2.Z);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X + d, coord1.Y + d, coord1.Z + d);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(double d, Coordinate coord1)
        {
            return coord1 + d;
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X / coord2.X, coord1.Y / coord2.Y, coord1.Z / coord2.Z);
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X / d, coord1.Y / d, coord1.Z / d);
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(double d, Coordinate coord1)
        {
            return coord1 / d;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(Coordinate obj1, Coordinate obj2)
        {
            return Equals(obj1, obj2);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(Coordinate obj1, Coordinate obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X * coord2.X, coord1.Y * coord2.Y, coord1.Z * coord2.Z);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X * d, coord1.Y * d, coord1.Z * d);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(double d, Coordinate coord1)
        {
            return coord1 * d;
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X - coord2.X, coord1.Y - coord2.Y, coord1.Z - coord2.Z);
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X - d, coord1.Y - d, coord1.Z - d);
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(double d, Coordinate coord1)
        {
            return coord1 - d;
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Duplicates this coordinate
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Compares this object with the specified object for order.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// Returns
        ///   -1  : this.x lowerthan other.x || ((this.x == other.x) AND (this.y lowerthan other.y))
        ///    0  : this.x == other.x AND this.y = other.y
        ///    1  : this.x greaterthan other.x || ((this.x == other.x) AND (this.y greaterthan other.y))
        /// </summary>
        /// <param name="other"><other>Coordinate</other> with which this <other>Coordinate</other> is being compared.</param>
        /// <returns>
        /// A negative integer, zero, or a positive integer as this <other>Coordinate</other>
        ///         is less than, equal to, or greater than the specified <other>Coordinate</other>.
        /// </returns>
        public virtual int CompareTo(Coordinate other)
        {
            if (other == null) return 1;
            if (X < other.X)
                return -1;
            if (X > other.X)
                return 1;
            if (Y < other.Y)
                return -1;
            return Y > other.Y ? 1 : 0;
        }

        /// <summary>
        /// Compares this object with the specified object for order.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// Returns
        ///   -1  : this.x lowerthan other.x || ((this.x == other.x) AND (this.y lowerthan other.y))
        ///    0  : this.x == other.x AND this.y = other.y
        ///    1  : this.x greaterthan other.x || ((this.x == other.x) AND (this.y greaterthan other.y))
        /// </summary>
        /// <param name="other"><other>Coordinate</other> with which this <other>Coordinate</other> is being compared.</param>
        /// <returns>
        /// A negative integer, zero, or a positive integer as this <other>Coordinate</other>
        ///         is less than, equal to, or greater than the specified <other>Coordinate</other>.
        /// </returns>
        int IComparable.CompareTo(object other)
        {
            Coordinate otherCoord = other as Coordinate;
            if (otherCoord == null) throw new ArgumentException(TopologyText.ArgumentCouldNotBeCast_S1_S2.Replace("%S1", "other").Replace("%S2", "ICoordinate"));
            return CompareTo(otherCoord);
        }

        /// <summary>
        /// Computes the 2-dimensional Euclidean distance to another location.
        /// </summary>
        /// <param name="other">A <see cref="Coordinate"/> with which to do the distance comparison.</param>
        /// <returns>the 2-dimensional Euclidean distance between the locations.</returns>
        /// <remarks>The Z-ordinate is ignored.</remarks>
        public double Distance(Coordinate other)
        {
            if (other == null) throw new ArgumentNullException("other");
            var dx = X - other.X;
            var dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Computes the 3-dimensional Euclidean distance to another location.
        /// </summary>
        /// <param name="other">A <see cref="Coordinate"/> with which to do the distance comparison.</param>
        /// <returns>the 3-dimensional Euclidean distance between the locations.</returns>
        public double Distance3D(Coordinate other)
        {
            if (other == null) throw new ArgumentNullException("other");
            double dx = X - other.X;
            double dy = Y - other.Y;
            double dz = Z - other.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Tests if another coordinate has the same value for Z, within a tolerance.
        /// </summary>
        /// <param name="c">A <see cref="Coordinate"/>.</param>
        /// <param name="tolerance">The tolerance value.</param>
        /// <returns><c>true</c> if the Z ordinates are within the given tolerance.</returns>
        public bool EqualInZ(Coordinate c, double tolerance)
        {
            return EqualsWithTolerance(this.Z, c.Z, tolerance);
        }

        /// <summary>
        /// Compares the X, Y and Z values with the specified item.
        /// If the Z value of this object or other is NaN then only the X and Y values are considered.
        /// </summary>
        /// <param name="obj">This should be either a Coordiante or an ICoordinate</param>
        /// <returns>Boolean, true if the two are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Coordinate c = obj as Coordinate;
            if (c != null) return Equals(c);

            ICoordinate ic = obj as ICoordinate;
            if (ic == null) return false;
            if (double.IsNaN(Z) || double.IsNaN(ic.Z)) return (ic.X == X && ic.Y == Y);
            return (ic.X == X && ic.Y == Y && ic.Z == Z);
        }

        /// <summary>
        /// Compares the X, Y and Z values with the specified item.
        /// If the Z value of this object or other is NaN then only the X and Y values are considered.
        /// </summary>
        /// <param name="other">Coordinate to compare with.</param>
        /// <returns>Boolean, true if the two are equal</returns>
        public bool Equals(Coordinate other)
        {
            if (other == null) return false;
            return (double.IsNaN(Z) || double.IsNaN(other.Z)) ? Equals2D(other) : Equals3D(other);
        }

        /// <summary>
        /// Returns whether the planar projections of the two <other>Coordinate</other>s are equal.
        ///</summary>
        /// <param name="other"><other>Coordinate</other> with which to do the 2D comparison.</param>
        /// <returns>
        /// <other>true</other> if the x- and y-coordinates are equal;
        /// the Z coordinates do not have to be equal.
        /// </returns>
        public bool Equals2D(Coordinate other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Tests if another coordinate has the same value for X and Y, within a tolerance.
        /// </summary>
        /// <param name="other">A <see cref="Coordinate"/>.</param>
        /// <param name="tolerance">The tolerance value.</param>
        /// <returns><c>true</c> if the X and Y ordinates are within the given tolerance.</returns>
        /// <remarks>The Z ordinate is ignored.</remarks>
        public bool Equals2D(Coordinate other, double tolerance)
        {
            if (other == null) return false;
            return EqualsWithTolerance(X, other.X, tolerance) && EqualsWithTolerance(Y, other.Y, tolerance);
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="other"/> has the same values for X, Y and Z.
        /// </summary>
        /// <param name="other">A <see cref="Coordinate"/> with which to do the 3D comparison.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="other"/> is a <see cref="Coordinate"/> with the same values for X, Y and Z.
        /// </returns>
        public bool Equals3D(Coordinate other)
        {
            if (other == null) return false;
            return (X == other.X) && (Y == other.Y) && ((Z == other.Z) || (double.IsNaN(Z) && double.IsNaN(other.Z)));
        }

        /// <summary>
        /// Checks whether the difference between x1 and x2 is smaller than tolerance.
        /// </summary>
        /// <param name="x1">First value used for check.</param>
        /// <param name="x2">Second value used for check.</param>
        /// <param name="tolerance">The difference must me smaller this value, for the x-values to be considered equal.</param>
        /// <returns></returns>
        private static bool EqualsWithTolerance(double x1, double x2, double tolerance)
        {
            return Math.Abs(x1 - x2) <= tolerance;
        }

        /// <summary>
        /// Computes a hash code for a double value, using the algorithm from
        /// Joshua Bloch's book <i>Effective Java"</i>.
        /// </summary>
        /// <param name="value">A hashcode for the double value</param>
        public static int GetHashCode(double value)
        {
            /*
             * From the java language specification, it says:
             *
             * The value of n>>>s is n right-shifted s bit positions with zero-extension.
             * If n is positive, then the result is the same as that of n>>s; if n is
             * negative, the result is equal to that of the expression (n>>s)+(2<<~s) if
             * the type of the left-hand operand is int
             */
            long f = BitConverter.DoubleToInt64Bits(value);
            return (int)(f ^ (f >> 32));
        }

        /// <summary>
        /// Gets a hashcode for this coordinate.
        /// </summary>
        /// <returns>A hashcode for this coordinate.</returns>
        public override int GetHashCode()
        {
            int result = 17;
            // ReSharper disable NonReadonlyFieldInGetHashCode
            result = 37 * result + GetHashCode(X);
            result = 37 * result + GetHashCode(Y);
            if (!double.IsNaN(Z)) result = 37 * result + GetHashCode(Z);
            // ReSharper restore NonReadonlyFieldInGetHashCode
            return result;
        }

        /// <summary>
        /// Returns the distance that is appropriate for N dimensions.  In otherwords, if this point is
        /// three dimensional, then all three dimensions will be used for calculating the distance.
        /// </summary>
        /// <param name="other">The coordinate to compare to this coordinate</param>
        /// <returns>A double valued distance measure that is invariant to the number of coordinates</returns>
        /// <exception cref="CoordinateMismatchException">The number of dimensions does not match between the points.</exception>
        public double HyperDistance(Coordinate other)
        {
            if (other == null) throw new ArgumentNullException("other");
            if (NumOrdinates != other.NumOrdinates) throw new CoordinateMismatchException();
            double sqrDist = 0;
            double[] aVals = ToArray();
            double[] bVals = other.ToArray();
            for (int i = 0; i < NumOrdinates; i++)
            {
                double diff = bVals[i] - aVals[i];
                sqrDist += diff * diff;
            }
            return Math.Sqrt(sqrDist);
        }

        /// <summary>
        /// If either X or Y is defined as NaN, then this coordinate is considered empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return (double.IsNaN(X) || double.IsNaN(Y));
        }

        /// <summary>
        /// Returns an array with the X, Y, Z, M value. M is only included if M and Z are not NAN. Z is only included if it isn't NAN.
        /// </summary>
        /// <returns>An array with the X, Y, Z, M value. M is only included if M and Z are not NAN. Z is only included if it isn't NAN.</returns>
        public double[] ToArray()
        {
            return double.IsNaN(Z) ? new[] { X, Y } : double.IsNaN(M) ? new[] { X, Y, Z } : new[] { X, Y, Z, M };
        }

        /// <summary>
        /// Returns a <c>string</c> of the form <I>(x, y, z)</I> .
        /// </summary>
        /// <returns><c>string</c> of the form <I>(x, y, z)</I></returns>
        public new string ToString()
        {
            string result = "(" + X;
            for (int i = 1; i < NumOrdinates; i++)
            {
                result += ", " + this[i];
            }
            result += ")";
            return result;
        }

        #endregion
    }
}