// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// A float based 3 dimensional vector class, implementing all interesting features of vectors.
    /// </summary>
    public struct FloatVector3
    {
        #region Fields

        /// <summary>
        /// X.
        /// </summary>
        public float X;

        /// <summary>
        /// Y.
        /// </summary>
        public float Y;

        /// <summary>
        /// Z.
        /// </summary>
        public float Z;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatVector3"/> struct.
        /// Copies the X, Y and Z values from the CoordinateF without doing a conversion.
        /// </summary>
        /// <param name="coord">X, Y Z.</param>
        public FloatVector3(CoordinateF coord)
        {
            X = coord.X;
            Y = coord.Y;
            Z = coord.Z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatVector3"/> struct with the specified values.
        /// </summary>
        /// <param name="xValue">X.</param>
        /// <param name="yValue">Y.</param>
        /// <param name="zValue">Z.</param>
        public FloatVector3(float xValue, float yValue, float zValue)
        {
            X = xValue;
            Y = yValue;
            Z = zValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatVector3"/> structusing the X, Y and Z values from the coordinate.
        /// </summary>
        /// <param name="coord">The coordinate to obtain X, Y and Z values from.</param>
        public FloatVector3(Coordinate coord)
        {
            X = Convert.ToSingle(coord.X);
            Y = Convert.ToSingle(coord.Y);
            Z = Convert.ToSingle(coord.Z);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public float Length => Convert.ToSingle(Math.Sqrt((X * X) + (Y * Y) + (Z * Z)));

        /// <summary>
        /// Gets the square of length of this vector.
        /// </summary>
        public float LengthSq => Convert.ToSingle((X * X) + (Y * Y) + (Z * Z));

        #endregion

        #region Operators

        /// <summary>
        /// Adds the vectors lhs and V using vector addition, which adds the corresponding components.
        /// </summary>
        /// <param name="lhs">One vector to be added.</param>
        /// <param name="rhs">A second vector to be added.</param>
        /// <returns>The sum of the vectors.</returns>
        public static FloatVector3 operator +(FloatVector3 lhs, FloatVector3 rhs)
        {
            return new FloatVector3
            {
                X = lhs.X + rhs.X,
                Y = lhs.Y + rhs.Y,
                Z = lhs.Z + rhs.Z
            };
        }

        /// <summary>
        /// Divides the components of vector lhs by the respective components
        /// ov vector V and returns the resulting vector.
        /// </summary>
        /// <param name="lhs">FloatVector3 Dividend (Numbers to be divided).</param>
        /// <param name="rhs">FloatVector3 Divisor (Numbers to divide by).</param>
        /// <returns>A FloatVector3 quotient of lhs and V.</returns>
        /// <remarks>To prevent divide by 0, if a 0 is in V, it will return 0 in the result.</remarks>
        public static FloatVector3 operator /(FloatVector3 lhs, FloatVector3 rhs)
        {
            FloatVector3 result = default(FloatVector3);
            if (rhs.X > 0) result.X = lhs.X / rhs.X;
            if (rhs.Y > 0) result.Y = lhs.Y / rhs.Y;
            if (rhs.Z > 0) result.Z = lhs.Z / rhs.Z;
            return result;
        }

        /// <summary>
        /// Multiplies each component of vector lhs by the Scalar value.
        /// </summary>
        /// <param name="lhs">A vector representing the vector to be multiplied.</param>
        /// <param name="scalar">Double, the scalar value to mulitiply the vector components by.</param>
        /// <returns>A FloatVector3 representing the vector product of vector lhs and the Scalar.</returns>
        /// <exception cref="ArgumentException">Thrown if scalar is 0.</exception>
        public static FloatVector3 operator /(FloatVector3 lhs, float scalar)
        {
            if (scalar == 0) throw new ArgumentException("Divisor cannot be 0.");

            return new FloatVector3
            {
                X = lhs.X / scalar,
                Y = lhs.Y / scalar,
                Z = lhs.Z / scalar
            };
        }

        /// <summary>
        /// Tests two float vectors for equality.
        /// </summary>
        /// <param name="lhs">The left hand side FloatVector3 to test.</param>
        /// <param name="rhs">The right hand side FloatVector3 to test.</param>
        /// <returns>Returns true if X, Y and Z are all equal.</returns>
        public static bool operator ==(FloatVector3 lhs, FloatVector3 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
        }

        /// <summary>
        /// Returns the Cross Product of two vectors lhs and rhs.
        /// </summary>
        /// <param name="lhs">Vector, the first input vector.</param>
        /// <param name="rhs">Vector, the second input vector.</param>
        /// <returns>A FloatVector3 containing the cross product of lhs and V.</returns>
        public static FloatVector3 operator ^(FloatVector3 lhs, FloatVector3 rhs)
        {
            return new FloatVector3
            {
                X = (lhs.Y * rhs.Z) - (lhs.Z * rhs.Y),
                Y = (lhs.Z * rhs.X) - (lhs.X * rhs.Z),
                Z = (lhs.X * rhs.Y) - (lhs.Y * rhs.X)
            };
        }

        /// <summary>
        /// Tests two float vectors for inequality.
        /// </summary>
        /// <param name="lhs">The left hand side FloatVector3 to test.</param>
        /// <param name="rhs">The right hand side FloatVector3 to test.</param>
        /// <returns>Returns true if any of X, Y and Z are unequal.</returns>
        public static bool operator !=(FloatVector3 lhs, FloatVector3 rhs)
        {
            return lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;
        }

        /// <summary>
        /// Returns the Inner Product also known as the dot product of two vectors, lhs and V.
        /// </summary>
        /// <param name="lhs">The input vector.</param>
        /// <param name="rhs">The vector to take the inner product against lhs.</param>
        /// <returns>a Double containing the dot product of lhs and V.</returns>
        public static float operator *(FloatVector3 lhs, FloatVector3 rhs)
        {
            return (lhs.X * rhs.X) + (lhs.Y * rhs.Y) + (lhs.Z * rhs.Z);
        }

        /// <summary>
        /// Multiplies the vectors lhs and V using vector multiplication,
        /// which adds the corresponding components.
        /// </summary>
        /// <param name="scalar">A scalar to multpy to the vector.</param>
        /// <param name="rhs">A vector to be multiplied.</param>
        /// <returns>The scalar product for the vectors.</returns>
        public static FloatVector3 operator *(float scalar, FloatVector3 rhs)
        {
            return new FloatVector3
            {
                X = scalar * rhs.X,
                Y = scalar * rhs.Y,
                Z = scalar * rhs.Z
            };
        }

        /// <summary>
        /// Multiplies each component of vector lhs by the Scalar value.
        /// </summary>
        /// <param name="lhs">A vector representing the vector to be multiplied.</param>
        /// <param name="scalar">Double, the scalar value to mulitiply the vector components by.</param>
        /// <returns>A FloatVector3 representing the vector product of vector lhs and the Scalar.</returns>
        public static FloatVector3 operator *(FloatVector3 lhs, float scalar)
        {
            return new FloatVector3
            {
                X = lhs.X * scalar,
                Y = lhs.Y * scalar,
                Z = lhs.Z * scalar
            };
        }

        /// <summary>
        /// Subtracts FloatVector3 V from FloatVector3 lhs.
        /// </summary>
        /// <param name="lhs">A FloatVector3 to subtract from.</param>
        /// <param name="rhs">A FloatVector3 to subtract.</param>
        /// <returns>The FloatVector3 difference lhs - V.</returns>
        public static FloatVector3 operator -(FloatVector3 lhs, FloatVector3 rhs)
        {
            FloatVector3 result;
            result.X = lhs.X - rhs.X;
            result.Y = lhs.Y - rhs.Y;
            result.Z = lhs.Z - rhs.Z;
            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds all the scalar members of the the two vectors.
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>The resulting vector.</returns>
        public static FloatVector3 Add(FloatVector3 lhs, FloatVector3 rhs)
        {
            return new FloatVector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        /// <summary>
        /// Returns the Cross Product of two vectors lhs and V.
        /// </summary>
        /// <param name="lhs">Vector, the first input vector.</param>
        /// <param name="rhs">Vector, the second input vector.</param>
        /// <returns>A FloatVector3 containing the cross product of lhs and V.</returns>
        public static FloatVector3 CrossProduct(FloatVector3 lhs, FloatVector3 rhs)
        {
            FloatVector3 result = new()
            {
                X = (lhs.Y * rhs.Z) - (lhs.Z * rhs.Y),
                Y = (lhs.Z * rhs.X) - (lhs.X * rhs.Z),
                Z = (lhs.X * rhs.Y) - (lhs.Y * rhs.X)
            };
            return result;
        }

        /// <summary>
        /// Multiplies all the scalar members of the the two vectors.
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>The resulting value.</returns>
        public static float Dot(FloatVector3 lhs, FloatVector3 rhs)
        {
            return (lhs.X * rhs.X) + (lhs.Y * rhs.Y) + (lhs.Z * rhs.Z);
        }

        /// <summary>
        /// Multiplies the source vector by a scalar.
        /// </summary>
        /// <param name="source">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The resulting vector.</returns>
        public static FloatVector3 Multiply(FloatVector3 source, float scalar)
        {
            return new FloatVector3(source.X * scalar, source.Y * scalar, source.Z * scalar);
        }

        /// <summary>
        /// Subtracts all the scalar members of the the two vectors.
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>The resulting vector.</returns>
        public static FloatVector3 Subtract(FloatVector3 lhs, FloatVector3 rhs)
        {
            return new FloatVector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        /// <summary>
        /// Adds the specified v.
        /// </summary>
        /// <param name="vector">A FloatVector3 to add to this vector.</param>
        public void Add(FloatVector3 vector)
        {
            X += vector.X;
            Y += vector.Y;
            Z += vector.Z;
        }

        /// <summary>
        /// Tests to see if the specified object has the same X, Y and Z values.
        /// </summary>
        /// <param name="obj">object to test against.</param>
        /// <returns>True, if the X, Y, Z values of the two vectors are equal.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is FloatVector3)) return false;
            FloatVector3 fv = (FloatVector3)obj;
            return fv.X == X && fv.Y == Y && fv.Z == Z;
        }

        /// <summary>
        /// Not sure what I should be doing here since Int can't really contain this much info very well.
        /// </summary>
        /// <returns>The generated hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Multiplies this vector by a scalar value.
        /// </summary>
        /// <param name="scalar">The scalar to multiply by.</param>
        public void Multiply(float scalar)
        {
            X *= scalar;
            Y *= scalar;
            Z *= scalar;
        }

        /// <summary>
        /// Normalizes the vectors.
        /// </summary>
        public void Normalize()
        {
            float length = Length;
            X /= length;
            Y /= length;
            Z /= length;
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="vector">A FloatVector3.</param>
        public void Subtract(FloatVector3 vector)
        {
            X -= vector.X;
            Y -= vector.Y;
            Z -= vector.Z;
        }

        #endregion
    }
}