// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.Drawing.Drawing2D;

namespace DotSpatial.Positioning
{
    // TODO: Can _Elements be used instead of _M values?  There appears to be duplication of values.

    /// <summary>
    /// Represents a three-dimensional double-precision matrix.
    /// </summary>
    public sealed class SquareMatrix3D : IEquatable<SquareMatrix3D>
    {
        /// <summary>
        ///
        /// </summary>
        private double _m11;
        /// <summary>
        ///
        /// </summary>
        private double _m12;
        /// <summary>
        ///
        /// </summary>
        private double _m13;
        /// <summary>
        ///
        /// </summary>
        private double _m21;
        /// <summary>
        ///
        /// </summary>
        private double _m22;
        /// <summary>
        ///
        /// </summary>
        private double _m23;
        /// <summary>
        ///
        /// </summary>
        private double _m31;
        /// <summary>
        ///
        /// </summary>
        private double _m32;
        /// <summary>
        ///
        /// </summary>
        private double _m33;

        /// <summary>
        ///
        /// </summary>
        private double[] _elements;

        #region Constructors

        /// <summary>
        /// Creates a matrix as an identity matrix
        /// </summary>
        public SquareMatrix3D()
            : this(
            1, 0, 0, //row 1
            0, 1, 0, //row 2
            0, 0, 1 //row 3
            )
        { }

        /// <summary>
        /// Creates a matrix with the indicated elements
        /// </summary>
        /// <param name="m11">The M11.</param>
        /// <param name="m12">The M12.</param>
        /// <param name="m13">The M13.</param>
        /// <param name="m21">The M21.</param>
        /// <param name="m22">The M22.</param>
        /// <param name="m23">The M23.</param>
        /// <param name="m31">The M31.</param>
        /// <param name="m32">The M32.</param>
        /// <param name="m33">The M33.</param>
        public SquareMatrix3D(
    double m11, double m12, double m13,
    double m21, double m22, double m23,
    double m31, double m32, double m33)
        {
            _m11 = m11;
            _m12 = m12;
            _m13 = m13;
            _m21 = m21;
            _m22 = m22;
            _m23 = m23;
            _m31 = m31;
            _m32 = m32;
            _m33 = m33;

            Elementary();
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Elementaries this instance.
        /// </summary>
        private void Elementary()
        {
            _elements = new[] {
                _m11, _m12, _m13,
                _m21, _m22, _m23,
                _m31, _m32, _m33};
        }

        #endregion Private Methods

        #region Public Properties

        /// <summary>
        /// Elements
        /// </summary>
        public double[] Elements
        {
            get { return _elements; }
        }

        /// <summary>
        /// Indicates whether or not this is an identity matrix
        /// </summary>
        public bool IsIdentity
        {
            get { return Equals(new SquareMatrix3D()); }
        }

        /// <summary>
        /// Indicates whether or not this matrix is invertable.
        /// </summary>
        public bool IsInvertable
        {
            get { return Determinant() != 0; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Ctreates an exact copy of this matrix.
        /// </summary>
        /// <returns>A cloned matrix.</returns>
        public SquareMatrix3D Clone()
        {
            return new SquareMatrix3D(
                _m11, _m12, _m13,
                _m21, _m22, _m23,
                _m31, _m32, _m33);
        }

        /// <summary>
        /// Multiplies this matrix with the supplied matrix, using a prepended matrix order
        /// </summary>
        /// <param name="matrix">The matrix to multiply with this matrix.</param>
        public void Multiply(SquareMatrix3D matrix)
        {
            Multiply(matrix, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Multiplies this matrix with the supplied matrix.
        /// </summary>
        /// <param name="matrix">The matrix to multiply with this matrix.</param>
        /// <param name="matrixOrder">The order in which to carry out the operation.</param>
        public void Multiply(SquareMatrix3D matrix, MatrixOrder matrixOrder)
        {
            // Matrix placholders
            SquareMatrix3D a;
            SquareMatrix3D b;

            // Order the operation based on the enum
            if (matrixOrder == MatrixOrder.Append)
            {
                // Multiply this matrix with the parameter matrix
                a = this;
                b = matrix;
            }
            else
            {
                // Multiply the parameter matrix with this matrix.
                a = matrix;
                b = this;
            }

            // Use doubles for intermediate calcs. Precision goes way up.
            double m11 = (a._m11 * b._m11) + (a._m12 * b._m21) + (a._m13 * b._m31);
            double m12 = (a._m11 * b._m12) + (a._m12 * b._m22) + (a._m13 * b._m32);
            double m13 = (a._m11 * b._m13) + (a._m12 * b._m23) + (a._m13 * b._m33);

            double m21 = (a._m21 * b._m11) + (a._m22 * b._m21) + (a._m23 * b._m31);
            double m22 = (a._m21 * b._m12) + (a._m22 * b._m22) + (a._m23 * b._m31);
            double m23 = (a._m21 * b._m13) + (a._m22 * b._m23) + (a._m23 * b._m33);

            double m31 = (a._m31 * b._m11) + (a._m32 * b._m21) + (a._m33 * b._m31);
            double m32 = (a._m31 * b._m12) + (a._m32 * b._m22) + (a._m33 * b._m31);
            double m33 = (a._m31 * b._m13) + (a._m32 * b._m23) + (a._m33 * b._m33);

            // Push calc'd values to this matrix
            _m11 = m11;
            _m12 = m12;
            _m13 = m13;

            _m21 = m21;
            _m22 = m22;
            _m23 = m23;

            _m31 = m31;
            _m32 = m32;
            _m33 = m33;

            Elementary();
        }

        /// <summary>
        /// Transforms the vector
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns></returns>
        public CartesianPoint TransformVector(CartesianPoint vector)
        {
            double x = (
                vector.X.ToMeters().Value * _m11 +
                vector.Y.ToMeters().Value * _m21 +
                vector.Z.ToMeters().Value * _m31);
            double y = (
                vector.X.ToMeters().Value * _m12 +
                vector.Y.ToMeters().Value * _m22 +
                vector.Z.ToMeters().Value * _m32);
            double z = (
                vector.X.ToMeters().Value * _m13 +
                vector.Y.ToMeters().Value * _m23 +
                vector.Z.ToMeters().Value * _m33);

            return new CartesianPoint(
                new Distance(x, DistanceUnit.Meters),
                new Distance(y, DistanceUnit.Meters),
                new Distance(z, DistanceUnit.Meters));
        }

        /// <summary>
        /// Transform the array of vectors
        /// </summary>
        /// <param name="vectors">The vectors.</param>
        public void TransformVectors(CartesianPoint[] vectors)
        {
            int limit = vectors.Length;
            for (int i = 0; i < limit; i++)
            {
                vectors[i] = TransformVector(vectors[i]);
            }
        }

        /// <summary>
        /// Inverts this matrix if it is invertable.
        /// </summary>
        /// <remarks>If the matrix is not invertable, this method throws an exception.</remarks>
        public void Invert()
        {
            double det = Determinant();

            // A 0 determinant matrix cannot be inverted
            if (det != 0)
            {
                // And her's why! div by zero...
                double oodet = 1.0 / det;

                double m11 = (_m22 * _m33 - _m23 * _m32) * oodet;
                double m12 = (_m13 * _m32 - _m12 * _m33) * oodet;
                double m13 = (_m12 * _m23 - _m13 * _m22) * oodet;

                double m21 = (_m23 * _m31 - _m21 * _m33) * oodet;
                double m22 = (_m11 * _m33 - _m13 * _m31) * oodet;
                double m23 = (_m13 * _m21 - _m11 * _m23) * oodet;

                double m31 = (_m21 * _m32 - _m22 * _m31) * oodet;
                double m32 = (_m12 * _m31 - _m11 * _m32) * oodet;
                double m33 = (_m11 * _m22 - _m12 * _m21) * oodet;

                _m11 = m11;
                _m12 = m12;
                _m13 = m13;

                _m21 = m21;
                _m22 = m22;
                _m23 = m23;

                _m31 = m31;
                _m32 = m32;
                _m33 = m33;

                Elementary();
            }
            else
            {
                // We should just throw "Parameter not valid", like GDI does.
                throw new ArgumentException("The matrix cannot be inverted (Neo would fall on his head). Use IsInvertable to check whether or not a matrix can be inverted.");
            }
        }

        /// <summary>
        /// Calculates the determinat of this matrix.
        /// </summary>
        /// <returns>The signed area of the parallelagram described by this matrix.</returns>
        /// <remarks>The determinant is a scalar value typically used to invert a matrix. As a signed area, it can also be used to
        /// identify "flipped" orientations, like mirroring. A negative determinant indicates that a matrix is "flipped".</remarks>
        private double Determinant()
        {
            return
                _m11 * (_m22 * _m33 - _m23 * _m32) +
                _m11 * (_m23 * _m31 - _m21 * _m33) +
                _m11 * (_m21 * _m32 - _m22 * _m31);
        }

        /// <summary>
        /// Resests the matrix to the identity matrix.
        /// </summary>
        public void Reset()
        {
            _m11 = 1; _m12 = 0; _m13 = 0;
            _m21 = 0; _m22 = 1; _m23 = 0;
            _m31 = 0; _m32 = 0; _m33 = 1;

            Elementary();
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                "m11={0:r}, m12={1:r}, m13={2:r}, m21={3:r}, m22={4:r}, m23={5:r}, m31={6:r}, m32={7:r}, m33={8:r}",
                _m11, _m12, _m13,
                _m21, _m22, _m23,
                _m31, _m32, _m33);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _m11.GetHashCode() ^ _m12.GetHashCode() ^ _m13.GetHashCode() ^
                _m21.GetHashCode() ^ _m22.GetHashCode() ^ _m23.GetHashCode() ^
                _m31.GetHashCode() ^ _m32.GetHashCode() ^ _m33.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SquareMatrix3D)
                return Equals((SquareMatrix3D)obj);
            return false;
        }

        #endregion Overrides

        #region Static Properties

        /* "Unfortunately, no one can be told what the matrix is...you have to see it for yourself."
         *
         *    | m11  m12  m13 | = x scale, y shear x, z shear x
         *    | m21  m22  m13 | = x shear y, y scale, z shear y
         *    | m21  m22  m13 | = x shear z, y shear z, z scale
         *
         */

        // A static readonly field doesn't work here since Matrix is mutable.

        /// <summary>
        /// Returns an identity matrix.
        /// </summary>
        public static SquareMatrix3D Identity
        {
            get { return new SquareMatrix3D(); }
        }

        #endregion Static Properties

        #region Static Methods

        /// <summary>
        /// Creates the transpose matrix of a matrix
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns></returns>
        public static SquareMatrix3D Transpose(SquareMatrix3D a)
        {
            return new SquareMatrix3D(
                a.Elements[0],
                a.Elements[3],
                a.Elements[6],
                a.Elements[1],
                a.Elements[4],
                a.Elements[7],
                a.Elements[2],
                a.Elements[5],
                a.Elements[8]);
        }

        /// <summary>
        /// Square Matrix 3D Invert
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns></returns>
        public static SquareMatrix3D Invert(SquareMatrix3D a)
        {
            SquareMatrix3D b = a.Clone();
            b.Invert();
            return b;
        }

        /// <summary>
        /// Defaults the specified default value.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static SquareMatrix3D Default(double defaultValue)
        {
            return new SquareMatrix3D(
                defaultValue, defaultValue, defaultValue,
                defaultValue, defaultValue, defaultValue,
                defaultValue, defaultValue, defaultValue);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="v">The v.</param>
        /// <returns>The result of the operator.</returns>
        public static CartesianPoint operator *(SquareMatrix3D a, CartesianPoint v)
        {
            return a.TransformVector(v);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static SquareMatrix3D operator *(SquareMatrix3D a, SquareMatrix3D b)
        {
            // TODO: Call instance method Multiply in object A
            return Multiply(a, b);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static SquareMatrix3D operator +(SquareMatrix3D a, SquareMatrix3D b)
        {
            // TODO: Call instance method Add in object A
            return Add(a, b);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static SquareMatrix3D operator -(SquareMatrix3D a, SquareMatrix3D b)
        {
            // TODO: Call instance method Subtract in object A
            return Subtract(a, b);
        }

        // TODO: Make Multiply an instance member.

        /// <summary>
        /// Multiplies the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static SquareMatrix3D Multiply(SquareMatrix3D a, SquareMatrix3D b)
        {
            SquareMatrix3D c = a.Clone();
            c.Multiply(b);
            return c;
        }

        // TODO: Make Add an instance member.

        /// <summary>
        /// Adds the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static SquareMatrix3D Add(SquareMatrix3D a, SquareMatrix3D b)
        {
            return new SquareMatrix3D(
                a.Elements[0] + b.Elements[0],
                a.Elements[1] + b.Elements[1],
                a.Elements[2] + b.Elements[2],
                a.Elements[3] + b.Elements[3],
                a.Elements[4] + b.Elements[4],
                a.Elements[5] + b.Elements[5],
                a.Elements[6] + b.Elements[6],
                a.Elements[7] + b.Elements[7],
                a.Elements[8] + b.Elements[8]);
        }

        // TODO: Make Subtract an instance member.

        /// <summary>
        /// Subtracts the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static SquareMatrix3D Subtract(SquareMatrix3D a, SquareMatrix3D b)
        {
            return new SquareMatrix3D(
                a.Elements[0] - b.Elements[0],
                a.Elements[1] - b.Elements[1],
                a.Elements[2] - b.Elements[2],
                a.Elements[3] - b.Elements[3],
                a.Elements[4] - b.Elements[4],
                a.Elements[5] - b.Elements[5],
                a.Elements[6] - b.Elements[6],
                a.Elements[7] - b.Elements[7],
                a.Elements[8] - b.Elements[8]);
        }

        #endregion Operators

        #region IEquatable<SquareMatrix3D> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(SquareMatrix3D other)
        {
            return
                _m11 == other._m11 &&
                _m12 == other._m12 &&
                _m13 == other._m13 &&
                _m21 == other._m21 &&
                _m22 == other._m22 &&
                _m23 == other._m23 &&
                _m31 == other._m31 &&
                _m32 == other._m32 &&
                _m33 == other._m33;
        }

        #endregion IEquatable<SquareMatrix3D> Members

        #region Unused Code (Commented Out)

        ///// <summary>
        ///// Prepend a rotation to this matrix.
        ///// </summary>
        ///// <param name="angle"> Amount, in degrees, to rotate. </param>
        //public void Rotate(float angle)
        //{
        //    this.Rotate(angle, MatrixOrder.Prepend);
        //}

        ///// <summary>
        ///// Apply a rotation to this matrix.
        ///// </summary>
        ///// <param name="angle"> Amount, in degrees, to rotate. </param>
        ///// <param name="matrixOrder"> The order in which to carry out the operation. </param>
        //public void Rotate(float angle, MatrixOrder matrixOrder)
        //{
        //    double a = angle * Radian.RadiansPerDegree;
        //    this.Multiply(new SquareMatrix3D(
        //        (float)Math.Cos(a), (float)Math.Sin(a),
        //        -(float)Math.Sin(a), (float)Math.Cos(a),
        //        0, 0),
        //        matrixOrder);
        //}

        #endregion Unused Code (Commented Out)
    }
}