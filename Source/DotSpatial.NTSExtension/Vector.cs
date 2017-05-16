// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// Contains a magnitude and direction
    /// Supports more fundamental calculations than LineSegment, rather than topological functions
    /// </summary>
    public class Vector : Coordinate
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class that is empty.
        /// </summary>
        public Vector()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class where the X, Y and Z terms are the same as the specified coordinate.
        /// </summary>
        /// <param name="coord">The ICoordinate to use</param>
        public Vector(Coordinate coord)
        {
            X = coord.X;
            Y = coord.Y;
            Z = coord.Z;
            RemoveNan();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class from a line segment, assuming that the direction is from the start point to the end point.
        /// </summary>
        /// <param name="inLineSegment">A Topology.LineSegment object to turn into a vector</param>
        public Vector(LineSegment inLineSegment)
        {
            X = inLineSegment.P1.X - inLineSegment.P0.X;
            Y = inLineSegment.P1.Y - inLineSegment.P0.Y;
            Z = inLineSegment.P1.Z - inLineSegment.P0.Z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class that points from the start coordinate to the end coordinate and
        /// uses the distance between the two coordinates to form its length.
        /// </summary>
        /// <param name="startCoord">The start coordinate</param>
        /// <param name="endCoord">The end coordinate for the vector</param>
        public Vector(Coordinate startCoord, Coordinate endCoord)
        {
            X = endCoord.X - startCoord.X;
            Y = endCoord.Y - startCoord.Y;
            Z = endCoord.Z - startCoord.Z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class from X1, Y1 to X2, Y2.
        /// </summary>
        /// <param name="x1">Double, The X coordinate of the start point for the vector</param>
        /// <param name="y1">Double, The Y coordinate of the start point for the vector </param>
        /// <param name="z1">Double, the Z coordinate of the start point for the vector</param>
        /// <param name="x2">Double, The X coordinate of the end point for the vector</param>
        /// <param name="y2">Double, The Y coordinate of the end point for the vector</param>
        /// <param name="z2">Double, the Z coordinate of the end point for the vector</param>
        public Vector(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            X = x2 - x1;
            Y = y2 - y1;
            Z = z2 - z1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class from the origin to the x, y, z coordinates.
        /// </summary>
        /// <param name="x">Double, the X coordinate from the origin</param>
        /// <param name="y">Double, the Y coordinate from the origin</param>
        /// <param name="z">Double, the Z coordinate from the origin</param>
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class from the origin with the new magnitude and directions specified.
        /// </summary>
        /// <param name="newMagnitude">Double, the length of the vector</param>
        /// <param name="theta">The angle in the x-y plane</param>
        /// <param name="phi">The angle in the z direction</param>
        public Vector(double newMagnitude, Angle theta, Angle phi)
        {
            X = newMagnitude * Angle.Cos(theta) * Angle.Cos(phi);
            Y = newMagnitude * Angle.Sin(theta) * Angle.Cos(phi);
            Z = newMagnitude * Angle.Sin(phi);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class in the X-Y plane with angle Theta.
        /// </summary>
        /// <param name="newMagnitude">Double, The magnitude of the vector</param>
        /// <param name="theta">Angle, The direction measured counterclockwise from Positive X Axis </param>
        public Vector(double newMagnitude, Angle theta)
        {
            X = newMagnitude * Angle.Cos(theta);
            Y = newMagnitude * Angle.Sin(theta);
            Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class from a vector that can be longer or shorter than 3 ordinates.
        /// If an X, Y or Z value is not specified, it will become 0. Values greater than the Z ordinate are lost.
        /// </summary>
        /// <param name="vect">Vector used for initialization.</param>
        public Vector(Vector vect)
        {
            X = vect.X;
            Y = vect.Y;
            Z = vect.Z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class based on the first three values on the first row of the
        /// matrix. This is useful for working with the result of a transformation matrix.
        /// </summary>
        /// <param name="mat">An IMatrixD that should represent the vector</param>
        public Vector(IMatrixD mat)
        {
            X = mat.Values[0, 0];
            Y = mat.Values[0, 1];
            Z = mat.Values[0, 2];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Euclidean distance from the origin to the tip of the 3 dimensional vector
        /// Setting the magntiude won't change the direction.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            }

            set
            {
                // preserves the angles, but adjusts the magnitude
                // if existing magnitude is 0, assume X direction.
                double mag = Length;
                if (mag == 0)
                {
                    X = value;
                    return;
                }

                double ratio = value / mag;
                X = X * ratio;
                Y = Y * ratio;
                Z = Z * ratio;
            }
        }

        /// <summary>
        /// Gets or sets the magnitude of the projection of the vector onto the base.X-Y plane
        /// Setting this magnitude will not affect Z, which should be adjusted separately
        /// </summary>
        public double Length2D
        {
            get
            {
                return Math.Sqrt((X * X) + (Y * Y));
            }

            set
            {
                // preserves the angles, but adjusts the magnitude
                // if existing magnitude is 0, assume base.X direction.
                double mag = Length2D;
                if (mag == 0)
                {
                    X = value;
                    return;
                }

                double ratio = value / mag;
                X = X * ratio;
                Y = Y * ratio;
            }
        }

        /// <summary>
        /// Gets or sets the angle above the X-Y plane. Positive towards positive Z.
        /// Values are in radians from -Pi/2 to Pi/2
        /// Setting this value when no magnitude exists results in a unit vector with angle phi in the X direction.
        /// </summary>
        public double Phi
        {
            get
            {
                double mag = Length;
                if (mag == 0) return 0;
                return Math.Asin(Z / mag);
            }

            set
            {
                // Preserves the existing magnitude unless it is 0
                double mag = Length;
                if (mag == 0)
                {
                    // Create a unit vector in X and Z
                    X = Math.Cos(value);
                    Z = Math.Sin(value);
                    return;
                }

                Z = mag * Math.Sin(value);
                Length2D = mag * Math.Cos(value);
            }
        }

        /// <summary>
        /// Gets or sets the angle in the X-Y plane. 0 along the positive X axis, and increasing counterclockwise
        /// Values are in Radians. Setting this value when no X-Y magnitude exists results in a unit vector
        /// between X and Y, but does not affect Z, so you may have something other than a unit vector in 3-D.
        /// Set theta before phi in order to obtain a unit vector in 3-D space.
        /// </summary>
        public double Theta
        {
            get
            {
                double mag = Length2D;
                if (mag == 0) return 0;
                double theta = Math.Atan(Y / X);
                if (X < 0) theta = Math.PI - theta;
                if (X > 0 && Y < 0)
                {
                    // Turn -Pi/2 -> 0 into 3Pi/2 -> 2Pi
                    theta = (Math.PI * 2) + theta;
                }

                return theta;
            }

            set
            {
                double mag = Length2D;
                if (mag == 0)
                {
                    X = Math.Cos(value);
                    Y = Math.Sin(value);
                    return;
                }

                X = mag * Math.Cos(value);
                Y = mag * Math.Sin(value);
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds the vectors U and V using vector addition, which adds the corresponding components
        /// </summary>
        /// <param name="u">One vector to be added</param>
        /// <param name="v">A second vector to be added</param>
        /// <returns>The sum of the vectors</returns>
        public static Vector operator +(Vector u, Vector v)
        {
            return new Vector(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        }

        /// <summary>
        /// Tests equality of the X, Y, and Z members.
        /// </summary>
        /// <param name="u">The left hand side vector to test for equality.</param>
        /// <param name="v">The right hand side vector to test for equality.</param>
        /// <returns>Returns true if X, Y and Z are equal.</returns>
        public static bool operator ==(Vector u, Vector v)
        {
            if (v == null && u == null) return true;
            if (v == null ^ u == null) return false;

            return u.X == v.X && u.Y == v.Y && u.Z == v.Z;
        }

        /// <summary>
        /// Returns the Cross Product of two vectors U and V
        /// </summary>
        /// <param name="u">Vector, the first input vector</param>
        /// <param name="v">Vector, the second input vector</param>
        /// <returns>A Vector containing the cross product of U and V</returns>
        public static Vector operator ^(Vector u, Vector v)
        {
            return new Vector { X = (u.Y * v.Z) - (u.Z * v.Y), Y = (u.Z * v.X) - (u.X * v.Z), Z = (u.X * v.Y) - (u.Y * v.X) };
        }

        /// <summary>
        /// Tests inequality of the X, Y and Z members.
        /// </summary>
        /// <param name="u">The left hand side vector to test inequality for.</param>
        /// <param name="v">The right hand side vector to test inequality for</param>
        /// <returns>Returns true if X, Y and Z are equal</returns>
        public static bool operator !=(Vector u, Vector v)
        {
            if (u == null & v == null) return true;
            if (u == null ^ v == null) return false;

            return !(u.X == v.X && u.Y == v.Y && u.Z == v.Z);
        }

        /// <summary>
        /// Returns the Inner Product also known as the dot product of two vectors, U and V
        /// </summary>
        /// <param name="u">The input vector</param>
        /// <param name="v">The vector to take the inner product against U</param>
        /// <returns>a Double containing the dot product of U and V</returns>
        public static double operator *(Vector u, Vector v)
        {
            return (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);
        }

        /// <summary>
        /// Multiplies the vectors U and V using vector multiplication,
        /// which adds the corresponding components
        /// </summary>
        /// <param name="scalar">A scalar to multpy to the vector</param>
        /// <param name="v">A vector to be multiplied</param>
        /// <returns>The scalar product for the vectors</returns>
        public static Vector operator *(double scalar, Vector v)
        {
            return new Vector(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        /// <summary>
        /// Multiplies each component of vector U by the Scalar value
        /// </summary>
        /// <param name="u">A vector representing the vector to be multiplied</param>
        /// <param name="scalar">Double, the scalar value to mulitiply the vector components by</param>
        /// <returns>A Vector representing the vector product of vector U and the Scalar</returns>
        public static Vector operator *(Vector u, double scalar)
        {
            return new Vector(u.X * scalar, u.Y * scalar, u.Z * scalar);
        }

        /// <summary>
        /// Subtracts Vector V from Vector U
        /// </summary>
        /// <param name="u">A Vector to subtract from</param>
        /// <param name="v">A Vector to subtract</param>
        /// <returns>The Vector difference U - V</returns>
        public static Vector operator -(Vector u, Vector v)
        {
            return new Vector(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the vectors U and V using vector addition, which adds the corresponding components.
        /// </summary>
        /// <param name="u">One vector to be added</param>
        /// <param name="v">A second vector to be added</param>
        /// <returns>The resulting vector.</returns>
        public static Vector Add(Vector u, Vector v)
        {
            return new Vector(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        }

        /// <summary>
        /// Returns the Cross Product of two vectors U and V
        /// </summary>
        /// <param name="u">Vector, the first input vector</param>
        /// <param name="v">Vector, the second input vector</param>
        /// <returns>A Vector containing the cross product of U and V</returns>
        public static Vector CrossProduct(Vector u, Vector v)
        {
            return new Vector { X = (u.Y * v.Z) - (u.Z * v.Y), Y = (u.Z * v.X) - (u.X * v.Z), Z = (u.X * v.Y) - (u.Y * v.X) };
        }

        /// <summary>
        /// Multiplies each component of vector U by the Scalar value
        /// </summary>
        /// <param name="u">A vector representing the vector to be multiplied</param>
        /// <param name="scalar">Double, the scalar value to mulitiply the vector components by</param>
        /// <returns>A Vector representing the vector product of vector U and the Scalar</returns>
        public static Vector Divide(Vector u, double scalar)
        {
            if (scalar == 0) throw new ArgumentException("Divisor cannot be 0.");
            return new Vector(u.X / scalar, u.Y / scalar, u.Z / scalar);
        }

        /// <summary>
        /// Returns the Inner Product also known as the dot product of two vectors, U and V
        /// </summary>
        /// <param name="u">The input vector</param>
        /// <param name="v">The vector to take the inner product against U</param>
        /// <returns>a Double containing the dot product of U and V</returns>
        public static double DotProduct(Vector u, Vector v)
        {
            return (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);
        }

        /// <summary>
        /// Multiplies each component of vector U by the Scalar value
        /// </summary>
        /// <param name="u">A vector representing the vector to be multiplied</param>
        /// <param name="scalar">Double, the scalar value to mulitiply the vector components by</param>
        /// <returns>A Vector representing the vector product of vector U and the Scalar</returns>
        public static Vector Multiply(Vector u, double scalar)
        {
            return new Vector(u.X * scalar, u.Y * scalar, u.Z * scalar);
        }

        /// <summary>
        /// Non-static version of taking the square distance for a vector
        /// </summary>
        /// <param name="u">The vector to find the square of the distance of</param>
        /// <returns>Double, the square of the distance</returns>
        public static double Norm2(Vector u)
        {
            double n2 = (u.X * u.X) + (u.Y * u.Y) + (u.Z * u.Z);
            return n2;
        }

        /// <summary>
        /// Subtracts Vector V from Vector U
        /// </summary>
        /// <param name="u">A Vector to subtract from</param>
        /// <param name="v">A Vector to subtract</param>
        /// <returns>The Vector difference U - V</returns>
        public static Vector Subtract(Vector u, Vector v)
        {
            return new Vector(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        }

        /// <summary>
        /// Adds each of the elements of V to the elements of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to add to this vector</param>
        /// <returns>A vector result from the addition</returns>
        public Vector Add(Vector v)
        {
            return new Vector(X + v.X, Y + v.Y, Z + v.Z);
        }

        /// <summary>
        /// Returns the cross product of this vector with the specified vector V
        /// </summary>
        /// <param name="v">The vector to perform a cross product against</param>
        /// <returns>A vector result from the inner product</returns>
        public Vector Cross(Vector v)
        {
            return new Vector { X = (Y * v.Z) - (Z * v.Y), Y = (Z * v.X) - (X * v.Z), Z = (X * v.Y) - (Y * v.X) };
        }

        /// <summary>
        /// Returns the dot product of this vector with V2
        /// </summary>
        /// <param name="v">The vector to perform an inner product against</param>
        /// <returns>A Double result from the inner product</returns>
        public double Dot(Vector v)
        {
            return (X * v.X) + (Y * v.Y) + (Z * v.Z);
        }

        /// <summary>
        /// Override  for definition of equality for vectors
        /// </summary>
        /// <param name="v">A vector to compare with</param>
        /// <returns>true if the X, Y, and Z coordinates are all equal</returns>
        public bool Equals(Vector v)
        {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }

        /// <summary>
        /// Checks first to make sure that both objects are vectors. If they are,
        /// then it checks to determine whether or not the X, Y and Z values are equal.
        /// </summary>
        /// <param name="vect">The object to test against</param>
        /// <returns>True if the </returns>
        public override bool Equals(object vect)
        {
            if (vect == null) return false;

            Vector v = vect as Vector;
            if (v == null) return false;

            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }

        /// <summary>
        /// Returns the hash code.. or something
        /// </summary>
        /// <returns>A hash code I guess</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Compares the values of each element, and if all the elements are equal, returns true.
        /// </summary>
        /// <param name="v">The vector to compare against this vector.</param>
        /// <returns>Boolean, true if all the elements have the same value.</returns>
        public bool Intersects(Vector v)
        {
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }

        /// <summary>
        /// Returns the scalar product of this vector against a scalar
        /// </summary>
        /// <param name="scalar">Double, a value to multiply against all the members of this vector</param>
        /// <returns>A vector multiplied by the scalar</returns>
        public Vector Multiply(double scalar)
        {
            return new Vector(X * scalar, Y * scalar, Z * scalar);
        }

        /// <summary>
        /// Returns the square of the distance of the vector without taking the square root
        /// This is the same as doting the vector with itself
        /// </summary>
        /// <returns>Double, the square of the distance between the vectors</returns>
        public double Norm2()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        public void Normalize()
        {
            // Chris M 2/4/2007
            double length = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            if (length <= 0) return;

            X = X / length;
            Y = Y / length;
            Z = Z / length;
        }

        /// <summary>
        /// Rotates the vector about the X axis as though the tail of the vector were at the origin
        /// </summary>
        /// <param name="degrees">The angle in degrees to rotate counter-clockwise when looking at the origin from the positive axis.</param>
        /// <returns>A new Vector that has been rotated</returns>
        public virtual Vector RotateX(double degrees)
        {
            IMatrixD m = ToMatrix();
            IMatrixD res = m.Multiply(Matrix4.RotationX(degrees));
            return new Vector(res);
        }

        /// <summary>
        /// Rotates the vector about the Y axis as though the tail of the vector were at the origin
        /// </summary>
        /// <param name="degrees">The angle in degrees to rotate counter-clockwise when looking at the origin from the positive axis.</param>
        /// <returns>A new Vector that has been rotated</returns>
        public virtual Vector RotateY(double degrees)
        {
            IMatrixD m = ToMatrix();
            IMatrixD res = m.Multiply(Matrix4.RotationY(degrees));
            return new Vector(res);
        }

        /// <summary>
        /// Rotates the vector about the Z axis as though the tail of the vector were at the origin
        /// </summary>
        /// <param name="degrees">The angle in degrees to rotate counter-clockwise when looking at the origin from the positive axis.</param>
        /// <returns>A new Vector that has been rotated</returns>
        public virtual Vector RotateZ(double degrees)
        {
            IMatrixD m = ToMatrix();
            IMatrixD res = m.Multiply(Matrix4.RotationZ(degrees));
            return new Vector(res);
        }

        /// <summary>
        /// Subtracts each element of V from each element of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to subtract from this vector</param>
        /// <returns>A vector result from the subtraction</returns>
        public Vector Subtract(Vector v)
        {
            return new Vector(X - v.X, Y - v.Y, Z - v.Z);
        }

        /// <summary>
        /// Returns an ICoordinate from this vector, where the X, Y and Z value match the values in this vector
        /// </summary>
        /// <returns>an ICoordinate, where the X, Y and Z value match the values in this vector</returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y, Z);
        }

        /// <summary>
        /// Returns a new segment from this vector, where the StartPoint is 0, 0, 0 and the End Point is the tip of this vector.
        /// </summary>
        /// <returns>A new segment from this vector, where the StartPoint is 0, 0, 0 and the End Point is the tip of this vector.</returns>
        public LineSegment ToLineSegment()
        {
            return new LineSegment(new Coordinate(0, 0, 0), ToCoordinate());
        }

        /// <summary>
        /// Rotations and transformations work by applying matrix mathematics, so this creates a 1 x 4 version of this vector.
        /// The 4th value is always 1, and allows for the translation terms to work.
        /// </summary>
        /// <returns>A 1 x 4 version of this vector</returns>
        public IMatrixD ToMatrix()
        {
            IMatrixD mat = new MatrixD(1, 4);
            double[,] m = mat.Values;
            m[0, 0] = this[Ordinate.X];
            m[0, 1] = this[Ordinate.Y];
            m[0, 2] = this[Ordinate.Z];
            m[0, 3] = 1;
            return mat;
        }

        /// <summary>
        /// Assuming the vector starts at the origin of 0, 0, 0, this function returns
        /// a Point representing the tip of the vector.
        /// </summary>
        /// <returns>A Point representing the tip of the vector.</returns>
        public IPoint ToPoint()
        {
            return new Point(X, Y, Z);
        }

        /// <summary>
        /// Transforms a point that has 3 dimensions by multiplying it by the specified 3 x 3 matrix in the upper left,
        /// but treats the bottom row as supplying the translation coordinates.
        /// </summary>
        /// <param name="transformMatrix">Transformation matrix for transformation.</param>
        /// <returns>The resulting vector.</returns>
        public Vector TransformCoordinate(IMatrix4 transformMatrix)
        {
            IMatrixD m = ToMatrix();
            IMatrixD res = m.Multiply(transformMatrix);

            // the output vector will have the coordinates arranged in columns rather than rows.
            double[,] results = res.Values;
            return new Vector(results[0, 0], results[0, 1], results[0, 2]);
        }

        private void RemoveNan()
        {
            if (double.IsNaN(X)) X = 0;
            if (double.IsNaN(Y)) Y = 0;
            if (double.IsNaN(Z)) Z = 0;
        }

        #endregion
    }
}