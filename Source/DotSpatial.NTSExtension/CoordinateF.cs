// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using GeoAPI.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// A lightweight class used to store coordinates on the 2-dimensional Cartesian plane.
    /// It is distinct from <c>Point</c>, which is a subclass of <c>Geometry</c>.
    /// Unlike objects of type <c>Point</c> (which contain additional information such as an envelope, a precision model, and spatial reference
    /// system information), a <c>Coordinate</c> only contains ordinate values and accessor methods.
    /// <c>Coordinate</c>s are two-dimensional points, with an additional z-ordinate. NTS does not support any operations on the z-ordinate except
    /// the basic accessor functions. Constructed coordinates will have a z-ordinate of <c>NaN</c>. The standard comparison functions will ignore
    /// the z-ordinate.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class CoordinateF
    {
        #region Fields

        private float _m;
        private float _x;
        private float _y;
        private float _z;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateF"/> class by using the X, Y and Z terms of a FloatVector.
        /// </summary>
        /// <param name="floatVector">FloatVector used for initialization.</param>
        public CoordinateF(FloatVector3 floatVector)
        {
            _x = floatVector.X;
            _y = floatVector.Y;
            _z = floatVector.Z;
            _m = float.NaN;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateF"/> class with the given (x, y, z, m) values.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        /// <param name="m">Measure value.</param>
        public CoordinateF(float x, float y, float z, float m)
        {
            _x = x;
            _y = y;
            _z = z;
            _m = m;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateF"/> class with the given (x, y, z) values.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        public CoordinateF(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
            _m = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateF"/> class from any Coordinate object.
        /// </summary>
        /// <param name="coordinate">The Vector.IPoint interface to construct a coordinate from</param>
        public CoordinateF(Coordinate coordinate)
        {
            _x = Convert.ToSingle(coordinate.X);
            _y = Convert.ToSingle(coordinate.Y);
            _z = Convert.ToSingle(coordinate.Z);
            _m = Convert.ToSingle(coordinate.M);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateF"/> class at (0, 0, 0, 0).
        /// </summary>
        public CoordinateF()
            : this(0.0F, 0.0F, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateF"/> class at (x, y, 0, 0).
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public CoordinateF(float x, float y)
            : this(x, y, 0F, 0F)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <c>Coordinate</c>s (x, y, z, m) values.
        /// </summary>
        [Browsable(false)]
        public CoordinateF CoordinateValue
        {
            get
            {
                return this;
            }

            set
            {
                _x = value.X;
                _y = value.Y;
                _z = value.Z;
                _m = value.M;
            }
        }

        /// <summary>
        /// Gets or sets the Measure coordinate.
        /// </summary>
        public float M
        {
            get
            {
                return _m;
            }

            set
            {
                _m = value;
            }
        }

        /// <summary>
        /// Gets the number of ordinates. For now this is 3D.
        /// </summary>
        public int NumOrdinates => 3;

        /// <summary>
        /// Gets or sets the values of this CoordinateF using an array of double values
        /// </summary>
        public double[] Values
        {
            get
            {
                return new double[] { Convert.ToSingle(_x), Convert.ToSingle(_y), Convert.ToSingle(_z) };
            }

            set
            {
                if (value.GetLength(0) > 0) _x = Convert.ToSingle(value[0]);
                if (value.GetLength(0) > 1) _y = Convert.ToSingle(value[1]);
                if (value.GetLength(0) > 2) _z = Convert.ToSingle(value[2]);
            }
        }

        /// <summary>
        /// Gets or sets the x value. If you only have the interface, it must involve conversions to and from a float.
        /// This may cause errors if the value being set is outside the range of the float values.
        /// </summary>
        public float X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public float Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        /// <summary>
        /// Gets or sets the Z coordinate.
        /// </summary>
        public float Z
        {
            get
            {
                return _z;
            }

            set
            {
                _z = value;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the double value corresponding to the specific ordinate.
        /// </summary>
        /// <param name="index">Index of the value that should be gotten / set.</param>
        /// <returns>The value from the given index or 0 if the index is out of bounds.</returns>
        public double this[int index]
        {
            get
            {
                if (index == 0) return Convert.ToDouble(_x);
                if (index == 1) return Convert.ToDouble(_y);
                if (index == 2) return Convert.ToDouble(_z);
                if (index == 3) return Convert.ToDouble(_m);
                return 0;
            }

            set
            {
                if (index == 0) _x = Convert.ToSingle(value);
                if (index == 1) _y = Convert.ToSingle(value);
                if (index == 2) _z = Convert.ToSingle(value);
                if (index == 3) _m = Convert.ToSingle(value);
            }
        }

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="coord2">The second coordinate for the operation.</param>
        public static CoordinateF operator +(CoordinateF coord1, Coordinate coord2)
        {
            // returns Coordinate as a specific implementatino of ICoordinate
            return new CoordinateF(coord1.X + Convert.ToSingle(coord2.X), coord1.Y + Convert.ToSingle(coord2.Y), coord1.Z + Convert.ToSingle(coord2.Z));
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="d">The float to add.</param>
        public static CoordinateF operator +(CoordinateF coord1, float d)
        {
            return new CoordinateF(coord1.X + d, coord1.Y + d, coord1.Z + d);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        /// <param name="d">The float to add.</param>
        /// <param name="coord1">The first coordinate for the operation</param>
        public static CoordinateF operator +(float d, CoordinateF coord1)
        {
            return coord1 + d;
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="coord2">The second coordinate for the operation.</param>
        public static CoordinateF operator /(CoordinateF coord1, Coordinate coord2)
        {
            return new CoordinateF(coord1.X / Convert.ToSingle(coord2.X), coord1.Y / Convert.ToSingle(coord2.Y), coord1.Z / Convert.ToSingle(coord2.Z));
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="d">The float to devide.</param>
        public static CoordinateF operator /(CoordinateF coord1, float d)
        {
            return new CoordinateF(coord1.X / d, coord1.Y / d, coord1.Z / d);
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        /// <param name="d">The float to devide.</param>
        /// <param name="coord1">The first coordinate for the operation</param>
        public static CoordinateF operator /(float d, CoordinateF coord1)
        {
            return coord1 / d;
        }

        /// <summary>
        /// Checks whether the Coordinates are equal.
        /// </summary>
        /// <param name="obj1">First object to compare.</param>
        /// <param name="obj2">Second object to compare.</param>
        /// <returns>True, if the objects are equal.</returns>
        public static bool operator ==(CoordinateF obj1, Coordinate obj2)
        {
            return Equals(obj1, new CoordinateF(obj2));
        }

        /// <summary>
        /// Checks whether the Coordinates are not equal.
        /// </summary>
        /// <param name="obj1">First object to compare.</param>
        /// <param name="obj2">Second object to compare.</param>
        /// <returns>True, if the objects do not equal.</returns>
        public static bool operator !=(CoordinateF obj1, Coordinate obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="coord2">The second coordinate for the operation.</param>
        public static CoordinateF operator *(CoordinateF coord1, Coordinate coord2)
        {
            return new CoordinateF(coord1.X * Convert.ToSingle(coord2.X), coord1.Y * Convert.ToSingle(coord2.Y), coord1.Z * Convert.ToSingle(coord2.Z));
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="d">The float to multiply.</param>
        public static CoordinateF operator *(CoordinateF coord1, float d)
        {
            return new CoordinateF(coord1.X * d, coord1.Y * d, coord1.Z * d);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        /// <param name="d">The float to multiply.</param>
        /// <param name="coord1">The first coordinate for the operation</param>
        public static CoordinateF operator *(float d, CoordinateF coord1)
        {
            return coord1 * d;
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="coord2">The second coordinate for the operation.</param>
        public static CoordinateF operator -(CoordinateF coord1, Coordinate coord2)
        {
            return new CoordinateF(coord1.X - Convert.ToSingle(coord2.X), coord1.Y - Convert.ToSingle(coord2.Y), coord1.Z - Convert.ToSingle(coord2.Z));
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        /// <param name="coord1">The first coordinate for the operation</param>
        /// <param name="d">The float to subtract.</param>
        public static CoordinateF operator -(CoordinateF coord1, float d)
        {
            return new CoordinateF(coord1.X - d, coord1.Y - d, coord1.Z - d);
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        /// <param name="d">The float to subtract.</param>
        /// <param name="coord1">The first coordinate for the operation</param>
        public static CoordinateF operator -(float d, CoordinateF coord1)
        {
            return coord1 - d;
        }

        #region Methods

        /// <summary>
        /// Return HashCode.
        /// </summary>
        /// <param name="x">Value from HashCode computation.</param>
        /// <returns>The resulting hash.</returns>
        public static int GetHashCode(double x)
        {
            long f = BitConverter.DoubleToInt64Bits(x);
            return (int)(f ^ (f >> 32));
        }

        /// <summary>
        /// Create a new object as copy of this instance.
        /// </summary>
        /// <returns>A copy of this instance.</returns>
        public object Clone()
        {
            return new Coordinate(_x, _y, _z, _m);
        }

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
        public int CompareTo(object other)
        {
            Coordinate otherCoord = other as Coordinate;
            if (otherCoord == null) throw new ArgumentException(TopologyText.ArgumentCouldNotBeCast_S1_S2.Replace("%S1", "other").Replace("%S2", "ICoordinate"));
            if (_x < otherCoord.X) return -1;
            if (_x > otherCoord.X) return 1;
            if (_y < otherCoord.Y) return -1;
            if (_y > otherCoord.Y) return 1;
            return 0;
        }

        /// <summary>
        /// Creates a new Coordinate copy of this instance.
        /// </summary>
        /// <returns>A copy of this instance.</returns>
        public Coordinate Copy()
        {
            return new Coordinate(_x, _y, _z, _m);
        }

        /// <summary>
        /// Returns Euclidean 2D distance from ICoordinate p.
        /// </summary>
        /// <param name="coordinate"><i>ICoordinate</i> with which to do the distance comparison.</param>
        /// <returns>Double, the distance between the two locations.</returns>
        public double Distance(Coordinate coordinate)
        {
            double dx = _x - coordinate.X;
            double dy = _y - coordinate.Y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Returns <c>true</c> if <c>other</c> has the same values for the x and y ordinates.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// </summary>
        /// <param name="other"><c>Coordinate</c> with which to do the comparison.</param>
        /// <returns><c>true</c> if <c>other</c> is a <c>Coordinate</c> with the same values for the x and y ordinates.</returns>
        public override bool Equals(object other)
        {
            var coord = other as CoordinateF;
            if (coord == null) return false;
            return Equals2D(coord);
        }

        /// <summary>
        /// Returns whether the planar projections of the two <i>Coordinate</i>s are equal.
        /// </summary>
        /// <param name="coordinate"><i>ICoordinate</i> with which to do the 2D comparison.</param>
        /// <returns>
        /// <c>true</c> if the x- and y-coordinates are equal;
        /// the Z coordinates do not have to be equal.
        /// </returns>
        public bool Equals2D(CoordinateF coordinate)
        {
            if (coordinate == null) return false;
            return _x == coordinate.X && _y == coordinate.Y;
        }

        /// <summary>
        /// Returns true if other has the same values for x, y and z.
        /// </summary>
        /// <param name="other"><i>ICoordinate</i> with which to do the 3D comparison.</param>
        /// <returns><c>true</c> if <c>other</c> is a <c>ICoordinate</c> with the same values for x, y and z.</returns>
        public bool Equals3D(CoordinateF other)
        {
            if (other == null) return false;
            return (_x == other.X) && (_y == other.X) && ((_z == other.Z) || (double.IsNaN(_z) && double.IsNaN(other.Z)));
        }

        /// <summary>
        /// Return HashCode.
        /// </summary>
        /// <returns>The generated hash code.</returns>
        public override int GetHashCode()
        {
            int result = 17;
            result = (37 * result) + GetHashCode(X);
            result = (37 * result) + GetHashCode(Y);
            return result;
        }

        /// <summary>
        /// Returns the distance that is appropriate for N dimensions. In otherwords, if this point is
        /// three dimensional, then all three dimensions will be used for calculating the distance.
        /// </summary>
        /// <param name="coordinate">The coordinate to compare to this coordinate</param>
        /// <returns>A double valued distance measure that is invariant to the number of coordinates</returns>
        /// <exception cref="CoordinateMismatchException">The number of dimensions does not match between the points.</exception>
        public double HyperDistance(Coordinate coordinate)
        {
            if (double.IsNaN(coordinate.Z)) throw new CoordinateMismatchException();
            double diff = coordinate.X - Convert.ToDouble(_x);
            double sqrDist = diff * diff;
            diff = coordinate.Y - Convert.ToDouble(_y);
            sqrDist += diff * diff;
            diff = coordinate.Z - Convert.ToDouble(_z);
            sqrDist += diff * diff;

            return Math.Sqrt(sqrDist);
        }

        /* END ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Gets an array of double values for each of the ordinates
        /// </summary>
        /// <returns>An array of double values for each of the ordinates.</returns>
        public double[] ToArray()
        {
            return new double[] { Convert.ToSingle(_x), Convert.ToSingle(_y), Convert.ToSingle(_z) };
        }

        /// <summary>
        /// Returns a <c>string</c> of the form <I>(x, y, z)</I> .
        /// </summary>
        /// <returns><c>string</c> of the form <I>(x, y, z)</I></returns>
        public override string ToString()
        {
            return "(" + _x + ", " + _y + ", " + _z + ")";
        }

        #endregion
    }
}