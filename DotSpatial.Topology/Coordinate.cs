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

namespace DotSpatial.Topology
{
    /// <summary>
    /// The original idea of comming up with a coordinate that used public accessors and an
    /// ICoordinate interface was actually quite slow, given that every access point was
    /// multiplied across many instances.
    /// </summary>
    [Serializable]
    public class Coordinate : ICloneable, IComparable<Coordinate>, IComparable
    {
        #region Private Variables

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
        public Coordinate()
        {
            X = double.NaN;
            Y = double.NaN;
            Z = double.NaN;
            M = double.NaN;
        }

        /// <summary>
        /// Creates a 2D coordinate with NaN for the Z and M values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
            Z = double.NaN;
            M = double.NaN;
        }

        /// <summary>
        /// Creates a new coordinate with the specified values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Coordinate(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            M = double.NaN;
        }

        /// <summary>
        /// Creates a new coordinate with the values in X, Y, Z order, or X, Y order.
        /// </summary>
        /// <param name="values"></param>
        public Coordinate(double[] values)
        {
            if (values.Length > 0) X = values[0];
            if (values.Length > 1) Y = values[1];
            if (values.Length > 2) Z = values[2];
        }

        /// <summary>
        /// Constructs a new instance of the coordinate
        /// </summary>
        /// <param name="c"></param>
        public Coordinate(ICoordinate c)
        {
            double[] values = c.Values;
            if (values.Length > 0) X = values[0];
            if (values.Length > 1) Y = values[1];
            if (values.Length > 2) Z = values[2];
            M = c.M;
        }

        /// <summary>
        /// Constructs a new instance of the coordinate
        /// </summary>
        /// <param name="c"></param>
        public Coordinate(Coordinate c)
        {
            X = c.X;
            Y = c.Y;
            Z = c.Z;
            M = c.M;
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

        #region Methods

        /// <summary>
        /// Compares the X, Y and Z values with the specified item.
        /// If the Z value is NaN then only the X and Y values are considered
        /// </summary>
        /// <param name="obj">This should be either a Coordiante or an ICoordinate</param>
        /// <returns>Boolean, true if the two are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is Coordinate == false)
            {
                ICoordinate ic = obj as ICoordinate;
                if (ic == null) return false;
                if (double.IsNaN(Z) || double.IsNaN(ic.Z)) return (ic.X == X && ic.Y == Y);
                return (ic.X == X && ic.Y == Y && ic.Z == Z);
            }
            Coordinate c = (Coordinate)obj;
            if (double.IsNaN(Z) || double.IsNaN(c.Z)) return (c.X == X && c.Y == Y);
            return (c.X == X && c.Y == Y && c.Z == Z);
        }

        /// <summary>
        /// returns the simple base.GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the underlying two dimensional array of coordinates
        /// </summary>
        /// <returns></returns>
        public double[] ToArray()
        {
            return double.IsNaN(Z) ? new[] { X, Y } : new[] { X, Y, Z };
        }

        /// <summary>
        /// Returns Euclidean 2D distance from ICoordinate p.
        /// </summary>
        /// <param name="end"><i>ICoordinate</i> with which to do the distance comparison.</param>
        /// <returns>Double, the distance between the two locations.</returns>
        public double Distance(Coordinate end)
        {
            double dx = end.X - X;
            double dy = end.Y - Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Returns whether the planar projections of the two <i>Coordinate</i>s are equal.
        /// </summary>
        /// <param name="b">The ICoordinate with which to do the 2D comparison.</param>
        /// <returns>
        /// <c>true</c> if the x- and y-coordinates are equal;
        /// the Z coordinates do not have to be equal.
        /// </returns>
        public bool Equals2D(Coordinate b)
        {
            if (X == b.X && Y == b.Y) return true;
            return false;
        }

        /// <summary>
        /// Returns true if other has the same values for x, y and z.
        /// </summary>
        /// <param name="b">The second ICoordiante for a 3D comparison</param>
        /// <returns><c>true</c> if <c>other</c> is a <c>ICoordinate</c> with the same values for x, y and z.</returns>
        public bool Equals3D(Coordinate b)
        {
            return (X == b.X) && (Y == b.Y) && ((Z == b.Z)
                                                || (Double.IsNaN(Z) && Double.IsNaN(b.Z)));
        }

        /// <summary>
        /// Returns the distance that is appropriate for N dimensions.  In otherwords, if this point is
        /// three dimensional, then all three dimensions will be used for calculating the distance.
        /// </summary>
        /// <param name="b">The coordinate to compare to this coordinate</param>
        /// <returns>A double valued distance measure that is invariant to the number of coordinates</returns>
        /// <exception cref="CoordinateMismatchException">The number of dimensions does not match between the points.</exception>
        public double HyperDistance(Coordinate b)
        {
            if (NumOrdinates != b.NumOrdinates) throw new CoordinateMismatchException();
            double sqrDist = 0;
            double[] aVals = ToArray();
            double[] bVals = b.ToArray();
            for (int i = 0; i < NumOrdinates; i++)
            {
                double diff = bVals[i] - aVals[i];
                sqrDist += diff * diff;
            }
            return Math.Sqrt(sqrDist);
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

        #region Properties

        /// <summary>
        /// Gets a pre-defined coordinate that has double.NaN for all the values.
        /// </summary>
        public static Coordinate Empty
        {
            get { return new Coordinate(double.NaN, double.NaN); }
        }

        /// <summary>
        /// Gets or sets the double value associated with the specified ordinate index
        /// </summary>
        /// <param name="index">The zero-based integer index of the ordinate</param>
        /// <returns>A double value representing a single ordinate</returns>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        return;
                    case 1:
                        Y = value;
                        return;
                    case 2:
                        Z = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
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

        #region IComparable Members

        /// <summary>
        /// Compares this object with the specified object for order.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// Returns
        ///    -1 : this.x lowerthan other.x || ((this.x == other.x) AND (this.y lowerthan other.y))
        ///    0  : this.x == other.x AND this.y = other.y
        ///    1  : this.x greaterthan other.x || ((this.x == other.x) AND (this.y greaterthan other.y))
        /// </summary>
        /// <param name="other"><c>Coordinate</c> with which this <c>Coordinate</c> is being compared.</param>
        /// <returns>
        /// A negative integer, zero, or a positive integer as this <c>Coordinate</c>
        ///         is less than, equal to, or greater than the specified <c>Coordinate</c>.
        /// </returns>
        int IComparable.CompareTo(object other)
        {
            Coordinate otherCoord = other as Coordinate;
            if (otherCoord == null) throw new ArgumentException(TopologyText.ArgumentCouldNotBeCast_S1_S2.Replace("%S1", "other").Replace("%S2", "ICoordinate"));
            if (X < otherCoord.X)
                return -1;
            if (X > otherCoord.X)
                return 1;
            if (Y < otherCoord.Y)
                return -1;
            if (Y > otherCoord.Y)
                return 1;
            return 0;
        }

        #endregion

        #region IComparable<Coordinate> Members

        /// <summary>
        /// Compares this object with the specified object for order.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// Returns
        ///    -1 : this.x lowerthan other.x || ((this.x == other.x) AND (this.y lowerthan other.y))
        ///    0  : this.x == other.x AND this.y = other.y
        ///    1  : this.x greaterthan other.x || ((this.x == other.x) AND (this.y greaterthan other.y))
        /// </summary>
        /// <param name="other"><c>Coordinate</c> with which this <c>Coordinate</c> is being compared.</param>
        /// <returns>
        /// A negative integer, zero, or a positive integer as this <c>Coordinate</c>
        ///         is less than, equal to, or greater than the specified <c>Coordinate</c>.
        /// </returns>
        public virtual int CompareTo(Coordinate other)
        {
            Coordinate otherCoord = other;
            if (otherCoord == null) throw new ArgumentException(TopologyText.ArgumentCouldNotBeCast_S1_S2.Replace("%S1", "other").Replace("%S2", "ICoordinate"));
            if (X < otherCoord.X)
                return -1;
            if (X > otherCoord.X)
                return 1;
            if (Y < otherCoord.Y)
                return -1;
            if (Y > otherCoord.Y)
                return 1;
            return 0;
        }

        #endregion

        /// <summary>
        /// If either X or Y is defined as NaN, then this coordinate is considered empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return (double.IsNaN(X) || double.IsNaN(Y));
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        ///  Duplicates this coordinate
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

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
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(Coordinate coord1, Coordinate coord2)
        {
            // returns Coordinate as a specific implementatino of ICoordinate
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
    }
}