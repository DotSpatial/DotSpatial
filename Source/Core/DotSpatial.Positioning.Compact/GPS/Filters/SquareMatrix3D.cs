using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Positioning
{
    // TODO: Can _Elements be used instead of _M values?  There appears to be duplication of values.

    /// <summary>
    /// Represents a three-dimensional double-precision matrix.
    /// </summary>
    public sealed class SquareMatrix3D : IEquatable<SquareMatrix3D>
    {
        private double _m11;
        private double _m12;
        private double _m13;
        private double _m21;
        private double _m22;
        private double _m23;
        private double _m31;
        private double _m32;
        private double _m33;

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
        /// <param name="m11"> Row one, column one </param>
        /// <param name="m12"> Row one, column two </param>
        /// <param name="m13"> Row one, column three</param>
        /// <param name="m21"> Row two, column one </param>
        /// <param name="m22"> Row two, column two </param>
        /// <param name="m23"> Row two, column three</param>
        /// <param name="m31"> Row three, column one </param>
        /// <param name="m32"> Row three, column two </param>
        /// <param name="m33"> Row three, column three </param>
        public SquareMatrix3D(
            double m11, double m12, double m13,
            double m21, double m22, double m23,
            double m31, double m32, double m33)
        {
            this._m11 = m11;
            this._m12 = m12;
            this._m13 = m13;
            this._m21 = m21;
            this._m22 = m22;
            this._m23 = m23;
            this._m31 = m31;
            this._m32 = m32;
            this._m33 = m33;

            this.Elementary();
        }

        #endregion

        #region Private Methods

        private void Elementary()
        {
            this._elements = new double[] { 
                this._m11, this._m12, this._m13,
                this._m21, this._m22, this._m23,
                this._m31, this._m32, this._m33};
        }

        #endregion

        #region Public Properties

        public double[] Elements
        {
            get { return this._elements; }
        }

        /// <summary>
        /// Indicates whether or not this is an identity matrix
        /// </summary>
        public bool IsIdentity
        {
            get { return this.Equals(new SquareMatrix3D()); }
        }

        /// <summary>
        /// Indicates whether or not this matrix is invertable.
        /// </summary>
        public bool IsInvertable
        {
            get { return this.Determinant() != 0; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ctreates an exact copy of this matrix.
        /// </summary>
        /// <returns> A cloned matrix. </returns>
        public SquareMatrix3D Clone()
        {
            return new SquareMatrix3D(
                this._m11, this._m12, this._m13,
                this._m21, this._m22, this._m23,
                this._m31, this._m32, this._m33);
        }


        /// <summary>
        /// Multiplies this matrix with the supplied matrix, using a prepended matrix order
        /// </summary>
        /// <param name="matrix"> The matrix to multiply with this matrix. </param>
        public void Multiply(SquareMatrix3D matrix)
        {
            this.Multiply(matrix, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Multiplies this matrix with the supplied matrix.
        /// </summary>
        /// <param name="matrix"> The matrix to multiply with this matrix. </param>
        /// <param name="matrixOrder"> The order in which to carry out the operation. </param>
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
            this._m11 = m11;
            this._m12 = m12;
            this._m13 = m13;

            this._m21 = m21;
            this._m22 = m22;
            this._m23 = m23;

            this._m31 = m31;
            this._m32 = m32;
            this._m33 = m33;

            this.Elementary();
        }

        private void Normalize()
        {
            double mta = _m11 * _m11 + _m12 * _m12 + _m13 * _m13;
            if (mta != 1 && mta != 0)
            {
                double na = 1.0f / Math.Sqrt(mta);
                this._m11 *= na;
                this._m12 *= na;
                this._m13 *= na;
            }

            double mtb = _m21 * _m21 + _m22 * _m22 + _m23 * _m23;
            if (mtb != 1 && mtb != 0)
            {
                double nb = 1.0f / Math.Sqrt(mtb);
                this._m21 *= nb;
                this._m22 *= nb;
                this._m23 *= nb;
            }

            double mtc = _m31 * _m31 + _m32 * _m32 + _m33 * _m33;
            if (mtc != 1 && mtb != 0)
            {
                double nc = 1.0f / Math.Sqrt(mtc);
                this._m31 *= nc;
                this._m32 *= nc;
                this._m33 *= nc;
            }
        }

        public CartesianPoint TransformVector(CartesianPoint vector)
        {
            double x = (
                vector.X.ToMeters().Value * this._m11 +
                vector.Y.ToMeters().Value * this._m21 +
                vector.Z.ToMeters().Value * this._m31);
            double y = (
                vector.X.ToMeters().Value * this._m12 +
                vector.Y.ToMeters().Value * this._m22 +
                vector.Z.ToMeters().Value * this._m32);
            double z = (
                vector.X.ToMeters().Value * this._m13 +
                vector.Y.ToMeters().Value * this._m23 +
                vector.Z.ToMeters().Value * this._m33);

            return new CartesianPoint(
                new Distance(x, DistanceUnit.Meters),
                new Distance(y, DistanceUnit.Meters),
                new Distance(z, DistanceUnit.Meters));
        }

        /// <summary>
        /// Transform the array of vectors
        /// </summary>
        /// <param name="points"></param>
        public void TransformVectors(CartesianPoint[] vectors)
        {
            int limit = vectors.Length;
            for (int i = 0; i < limit; i++)
            {
                vectors[i] = this.TransformVector(vectors[i]);
            }
        }

        /// <summary>
        /// Inverts this matrix if it is invertable. 
        /// </summary>
        /// <remarks>
        /// If the matrix is not invertable, this method throws an exception.
        /// </remarks>
        public void Invert()
        {
            double det = this.Determinant();

            // A 0 determinant matrix cannot be inverted
            if (det != 0)
            {
                // And her's why! div by zero...
                double oodet = 1.0 / det;

                double m11 = (this._m22 * this._m33 - this._m23 * this._m32) * oodet;
                double m12 = (this._m13 * this._m32 - this._m12 * this._m33) * oodet;
                double m13 = (this._m12 * this._m23 - this._m13 * this._m22) * oodet;

                double m21 = (this._m23 * this._m31 - this._m21 * this._m33) * oodet;
                double m22 = (this._m11 * this._m33 - this._m13 * this._m31) * oodet;
                double m23 = (this._m13 * this._m21 - this._m11 * this._m23) * oodet;

                double m31 = (this._m21 * this._m32 - this._m22 * this._m31) * oodet;
                double m32 = (this._m12 * this._m31 - this._m11 * this._m32) * oodet;
                double m33 = (this._m11 * this._m22 - this._m12 * this._m21) * oodet;

                this._m11 = m11;
                this._m12 = m12;
                this._m13 = m13;

                this._m21 = m21;
                this._m22 = m22;
                this._m23 = m23;

                this._m31 = m31;
                this._m32 = m32;
                this._m33 = m33;

                this.Elementary();
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
        /// <returns> The signed area of the parallelagram described by this matrix. </returns>
        /// <remarks>
        /// The determinant is a scalar value typically used to invert a matrix. As a signed area, it can also be used to
        /// identify "flipped" orientations, like mirroring. A negative determinant indicates that a matrix is "flipped".
        /// </remarks>
        private double Determinant()
        {
            return
                this._m11 * (this._m22 * this._m33 - this._m23 * this._m32) +
                this._m11 * (this._m23 * this._m31 - this._m21 * this._m33) +
                this._m11 * (this._m21 * this._m32 - this._m22 * this._m31);
        }

        /// <summary>
        /// Resests the matrix to the identity matrix.
        /// </summary>
        public void Reset()
        {
            this._m11 = 1; this._m12 = 0; this._m13 = 0;
            this._m21 = 0; this._m22 = 1; this._m23 = 0;
            this._m31 = 0; this._m32 = 0; this._m33 = 1;

            this.Elementary();
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format(
                "m11={0:r}, m12={1:r}, m13={2:r}, m21={3:r}, m22={4:r}, m23={5:r}, m31={6:r}, m32={7:r}, m33={8:r}",
                this._m11, this._m12, this._m13,
                this._m21, this._m22, this._m23,
                this._m31, this._m32, this._m33);
        }

        public override int GetHashCode()
        {
            return _m11.GetHashCode() ^ _m12.GetHashCode() ^ _m13.GetHashCode() ^
                _m21.GetHashCode() ^ _m22.GetHashCode() ^ _m23.GetHashCode() ^
                _m31.GetHashCode() ^ _m32.GetHashCode() ^ _m33.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SquareMatrix3D)
                return Equals((SquareMatrix3D)obj);
            return false;
        }

        #endregion

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

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates the transpose matrix of a matrix 
        /// </summary>
        public static SquareMatrix3D Transpose(SquareMatrix3D A)
        {
            return new SquareMatrix3D(
                A.Elements[0],
                A.Elements[3],
                A.Elements[6],
                A.Elements[1],
                A.Elements[4],
                A.Elements[7],
                A.Elements[2],
                A.Elements[5],
                A.Elements[8]);
        }

        public static SquareMatrix3D Invert(SquareMatrix3D A)
        {
            SquareMatrix3D b = A.Clone();
            b.Invert();
            return b;
        }

        public static SquareMatrix3D Default(double defaultValue)
        {
            return new SquareMatrix3D(
                defaultValue, defaultValue, defaultValue,
                defaultValue, defaultValue, defaultValue,
                defaultValue, defaultValue, defaultValue);
        }

        #endregion

        #region Operators

        public static CartesianPoint operator *(SquareMatrix3D a, CartesianPoint v)
        {
            return a.TransformVector(v);
        }

        public static SquareMatrix3D operator *(SquareMatrix3D a, SquareMatrix3D b)
        {
            // TODO: Call instance method Multiply in object A
            return Multiply(a, b);
        }

        public static SquareMatrix3D operator +(SquareMatrix3D a, SquareMatrix3D b)
        {
            // TODO: Call instance method Add in object A
            return Add(a, b);
        }

        public static SquareMatrix3D operator -(SquareMatrix3D a, SquareMatrix3D b)
        {
            // TODO: Call instance method Subtract in object A
            return Subtract(a, b);
        }

        // TODO: Make Multiply an instance member.

        public static SquareMatrix3D Multiply(SquareMatrix3D a, SquareMatrix3D b)
        {
            SquareMatrix3D c = a.Clone();
            c.Multiply(b);
            return c;
        }

        // TODO: Make Add an instance member.

        public static SquareMatrix3D Add(SquareMatrix3D A, SquareMatrix3D B)
        {
            return new SquareMatrix3D(
                A.Elements[0] + B.Elements[0],
                A.Elements[1] + B.Elements[1],
                A.Elements[2] + B.Elements[2],
                A.Elements[3] + B.Elements[3],
                A.Elements[4] + B.Elements[4],
                A.Elements[5] + B.Elements[5],
                A.Elements[6] + B.Elements[6],
                A.Elements[7] + B.Elements[7],
                A.Elements[8] + B.Elements[8]);
        }

        // TODO: Make Subtract an instance member.

        public static SquareMatrix3D Subtract(SquareMatrix3D A, SquareMatrix3D B)
        {
            return new SquareMatrix3D(
                A.Elements[0] - B.Elements[0],
                A.Elements[1] - B.Elements[1],
                A.Elements[2] - B.Elements[2],
                A.Elements[3] - B.Elements[3],
                A.Elements[4] - B.Elements[4],
                A.Elements[5] - B.Elements[5],
                A.Elements[6] - B.Elements[6],
                A.Elements[7] - B.Elements[7],
                A.Elements[8] - B.Elements[8]);
        }

        #endregion

        #region IEquatable<SquareMatrix3D> Members

        public bool Equals(SquareMatrix3D other)
        {
            return
                this._m11 == other._m11 &&
                this._m12 == other._m12 &&
                this._m13 == other._m13 &&
                this._m21 == other._m21 &&
                this._m22 == other._m22 &&
                this._m23 == other._m23 &&
                this._m31 == other._m31 &&
                this._m32 == other._m32 &&
                this._m33 == other._m33;
        }

        #endregion


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

        #endregion
    }
}