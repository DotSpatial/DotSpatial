#if PocketPC

using System;
using System.Drawing;

namespace GeoFramework
{
    /// <summary>
    /// Indicates the insertion point for new transforms.
    /// </summary>
    public enum MatrixOrder
    {
        Append,
        Prepend,
    }

    /// <summary>
    /// Represents a mechanism for facilitating translation, scaling and rotation of coordinates.
    /// </summary>
    public sealed class Matrix : IDisposable, IEquatable<Matrix>, ICloneable<Matrix>
    {
        // TODO: Can _Elements be combined with the other variables?  They appear to have the same value.

        private float _m11;
        private float _m12;
        private float _dx;
        private float _m21;
        private float _m22;
        private float _dy;

        private float[] _elements;

        /* "Unfortunaately, no one can be told what the matrix is...you have to see it for yourself."
         * 
         *    | m11  m12 | = x scale, y shear
         *    | m21  m22 | = x shear, y scale
         *    | dx   dy  | = x translation, y translation
         *    
         */

        #region Constructors

        /// <summary>
        /// Creates a matrix as an identity matrix
        /// </summary>
        public Matrix()
            : this(1, 0, 0, 1, 0, 0)
        { }

        /// <summary>
        /// Creates a matrix with the indicated elements
        /// </summary>
        /// <param name="m11"> Row one, column one </param>
        /// <param name="m12"> Row one, column two </param>
        /// <param name="m21"> Row two, column one </param>
        /// <param name="m22"> Row two, column two </param>
        /// <param name="dx"> X translation (Row three, column one) </param>
        /// <param name="dy"> Y translation (Row three, column two) </param>
        public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
        {
            this._m11 = m11;
            this._m12 = m12;
            this._dx = dx;
            this._m21 = m21;
            this._m22 = m22;
            this._dy = dy;

            this._elements = new float[] { m11, m12, m21, m22, dx, dy };
        }

        #endregion

        #region Public Properties

        public float OffsetX
        {
            get { return this._dx; }
        }

        public float OffsetY
        {
            get { return this._dy; }
        }

        public float[] Elements
        {
            get { return this._elements; }
        }

        /// <summary>
        /// Indicates whether or not this is an identity matrix
        /// </summary>
        public bool IsIdentity
        {
            get { return this.Equals(new Matrix()); }
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
        /// Inverts this matrix if it is invertable. 
        /// </summary>
        /// <remarks>
        /// If the matrix is not invertable, this method throws an exception.
        /// </remarks>
        public void Invert()
        {
            float det = this.Determinant();

            // A 0 determinant matrix cannot be inverted
            if (det != 0)
            {
                // And her's why! div by zero...
                double oodet = 1.0 / det;

                // Use doubles for intermediate calcs. Precision goes way up 
                double m11 = this._m22 * oodet;
                double m12 = -this._m12 * oodet;

                double m21 = -this._m21 * oodet;
                double m22 = this._m11 * oodet;

                double dx = -(this._dx * m11 + this._dy * m21);
                double dy = -(this._dx * m12 + this._dy * m22);
                
                //double dx = (this._dy * m21) - (this._dx * m11);
                //double dy = (this._dx * m12) - (this._dy * m22);
                
                this._m11 = (float)m11;
                this._m12 = (float)m12;
                this._m21 = (float)m21;
                this._m22 = (float)m22;

                this._dx = (float)dx;
                this._dy = (float)dy;

                this._elements = new float[] { this._m11, this._m12, this._m21, this._m22, this._dx, this._dy };
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
        private float Determinant()
        {
            return (this._m11 * this._m22) - (this._m12 * this._m21);
        }

        /// <summary>
        /// Resests the matrix to the identity matrix.
        /// </summary>
        public void Reset()
        {
            this._m11 = 1;
            this._m12 = 0;
            this._m21 = 0;
            this._m22 = 1;
            this._dx = 0;
            this._dy = 0;

            this._elements = new float[] { this._m11, this._m12, this._m21, this._m22, this._dx, this._dy };
        }

        /// <summary>
        /// Multiplies this matrix with the supplied matrix, using a prepended matrix order
        /// </summary>
        /// <param name="matrix"> The matrix to multiply with this matrix. </param>
        public void Multiply(Matrix matrix)
        {
            this.Multiply(matrix, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Multiplies this matrix with the supplied matrix.
        /// </summary>
        /// <param name="matrix"> The matrix to multiply with this matrix. </param>
        /// <param name="matrixOrder"> The order in which to carry out the operation. </param>
        public void Multiply(Matrix matrix, MatrixOrder matrixOrder)
        {
            // Matrix placholders
            Matrix a;
            Matrix b;

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
            double m11 = (a._m11 * b._m11) + (a._m12 * b._m21);
            double m12 = (a._m11 * b._m12) + (a._m12 * b._m22);

            double m21 = (a._m21 * b._m11) + (a._m22 * b._m21);
            double m22 = (a._m21 * b._m12) + (a._m22 * b._m22);

            double dx = (a._dx * b._m11) + (a._dy * b._m21) + b._dx;
            double dy = (a._dx * b._m12) + (a._dy * b._m22) + b._dy;

            // Push calc'd values to this matrix
            this._m11 = (float)m11;
            this._m12 = (float)m12;
            this._m21 = (float)m21;
            this._m22 = (float)m22;

            this._dx = (float)dx;
            this._dy = (float)dy;

            this._elements = new float[] { this._m11, this._m12, this._m21, this._m22, this._dx, this._dy };
        }

        /// <summary>
        /// Translate this matrix by prepending the supplied values.
        /// </summary>
        /// <param name="xOffset"> Amount to translate in the X direction. </param>
        /// <param name="yOffset"> Amount to translate in the Y direction. </param>
        public void Translate(float xOffset, float yOffset)
        {
            this.Translate(xOffset, yOffset, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Translate this matrix using the supplied values.
        /// </summary>
        /// <param name="xOffset"> Amount to translate in the X direction. </param>
        /// <param name="yOffset"> Amount to translate in the Y direction. </param>
        /// <param name="matrixOrder"> The order in which to carry out the operation. </param>
        public void Translate(float xOffset, float yOffset, MatrixOrder matrixOrder)
        {
            // All operations are multiplications
            this.Multiply(new Matrix(1, 0, 0, 1, xOffset, yOffset), matrixOrder);
        }

        /// <summary>
        /// Scale this matrix by prepending the supplied values.
        /// </summary>
        /// <param name="xScale"> Amount to scale in the X direction. </param>
        /// <param name="yScale"> Amount to scale in the Y direction. </param>
        public void Scale(float xScale, float yScale)
        {
            this.Scale(xScale, yScale, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Scale this matrix using the supplied values.
        /// </summary>
        /// <param name="xScale"> Amount to scale in the X direction. </param>
        /// <param name="yScale"> Amount to scale in the Y direction. </param>
        /// <param name="matrixOrder"> The order in which to carry out the operation. </param>
        public void Scale(float xScale, float yScale, MatrixOrder matrixOrder)
        {
            // All operations are multiplications
            this.Multiply(new Matrix(xScale, 0, 0, yScale, 0, 0), matrixOrder);
        }

        /// <summary>
        /// Shear this matrix by prepending the supplied values.
        /// </summary>
        /// <param name="xShear"> Amount to shear in the X direction. </param>
        /// <param name="yShear"> Amount to shear in the Y direction. </param>
        public void Shear(float xShear, float yShear)
        {
            this.Shear(xShear, yShear, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Shear this matrix using the supplied values.
        /// </summary>
        /// <param name="xShear"> Amount to shear in the X direction. </param>
        /// <param name="yShear"> Amount to shear in the Y direction. </param>
        /// <param name="matrixOrder"> The order in which to carry out the operation. </param>
        public void Shear(float xShear, float yShear, MatrixOrder matrixOrder)
        {
            // All operations are multiplications
            this.Multiply(new Matrix(1, yShear, xShear, 1, 0, 0), matrixOrder);
        }

        /// <summary>
        /// Prepend a rotation to this matrix.
        /// </summary>
        /// <param name="angle"> Amount, in degrees, to rotate. </param>
        public void Rotate(float angle)
        {
            this.Rotate(angle, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Apply a rotation to this matrix.
        /// </summary>
        /// <param name="angle"> Amount, in degrees, to rotate. </param>
        /// <param name="matrixOrder"> The order in which to carry out the operation. </param>
        public void Rotate(float angle, MatrixOrder matrixOrder)
        {
            double a = angle * Radian.RadiansPerDegree;
            this.Multiply(new Matrix(
                (float)Math.Cos(a), (float)Math.Sin(a),
                -(float)Math.Sin(a), (float)Math.Cos(a),
                0, 0),
                matrixOrder);
        }

        /// <summary>
        /// Prepend a translated rotation to this matrix.
        /// </summary>
        /// <param name="angle"> Amount, in degrees, to rotate. </param>
        /// <param name="point"> The desired center of rotation. </param>
        public void RotateAt(float angle, PointF point)
        {
            this.RotateAt(angle, point, MatrixOrder.Prepend);
        }

        /// <summary>
        /// Apply a translated rotation to this matrix.
        /// </summary>
        /// <param name="angle"> Amount, in degrees, to rotate. </param>
        /// <param name="point"> The desired center of rotation. </param>
        /// <param name="matrixOrder"> The order in which to carry out the operation. </param>
        public void RotateAt(float angle, PointF point, MatrixOrder matrixOrder)
        {
            // Create the offset rotstion independant of our current matrix
            Matrix rotation = new Matrix();
            rotation.Translate(-point.X, -point.Y, MatrixOrder.Append);
            rotation.Rotate(angle, MatrixOrder.Append);
            rotation.Translate(point.X, point.Y, MatrixOrder.Append);

            // Combine the offset rotation matrix with the current matrix
            this.Multiply(rotation, matrixOrder);
        }

        /// <summary>
        /// Transform the array of points
        /// </summary>
        /// <param name="points"></param>
        public void TransformPoints(PointF[] points)
        {
            int limit = points.Length;
            for (int i = 0; i < limit; i++)
            {
                points[i] = this.TransformPoint(points[i]);
            }
        }

        /// <summary>
        /// Transform the array of points, without translation
        /// </summary>
        /// <param name="points"></param>
        public void TransformVectors(PointF[] points)
        {
            int limit = points.Length;
            for (int i = 0; i < limit; i++)
            {
                points[i] = this.TransformVector(points[i]);
            }
        }

        public RectangleF TransformRectangle(RectangleF rectangle)
        {
            PointF[] points = RectangleFHelper.Corners(rectangle);
            TransformPoints(points);
            return RectangleFHelper.ComputeBoundingBox(points);
        }
        
        public Rectangle TransformRectangleCF(RectangleF rectangle)
        {
            return Rectangle.Round(TransformRectangle(rectangle)); ;
        }

        /// <summary>
        /// Transform the array of points and round them
        /// </summary>
        /// <param name="points"></param>
        public Point[] TransformPointsCF(PointF[] points)
        {
            int limit = points.Length;
            Point[] result = new Point[limit];

            for (int i = 0; i < limit; i++)
            {
                result[i] = this.TransformPointCF(points[i]);
            }

            return result;
        }

        /// <summary>
        /// Transform the array of points without translation and round them
        /// </summary>
        /// <param name="points"></param>
        public Point[] TransformVectorsCF(PointF[] points)
        {
            int limit = points.Length;
            Point[] result = new Point[limit];

            for (int i = 0; i < limit; i++)
            {
                result[i] = this.TransformVectorCF(points[i]);
            }

            return result;
        }

        private PointF TransformPoint(PointF point)
        {
            double x = (point.X * this._m11 + point.Y * this._m21);
            double y = (point.X * this._m12 + point.Y * this._m22);

            point.X = (float)x + this._dx;
            point.Y = (float)y + this._dy;

            return point;
        }

        private PointF TransformVector(PointF point)
        {
            double x = (point.X * this._m11 + point.Y * this._m21);
            double y = (point.X * this._m12 + point.Y * this._m22);

            point.X = (float)x;
            point.Y = (float)y;

            return point;
        }

        private Point TransformPointCF(PointF point)
        {
            Point result = new Point();

            double x = (point.X * this._m11 + point.Y * this._m21);
            double y = (point.X * this._m12 + point.Y * this._m22);

            result.X = (int)Math.Round(x + this._dx);
            result.Y = (int)Math.Round(y + this._dy);

            return result;
        }

        private Point TransformVectorCF(PointF point)
        {
            Point result = new Point();

            double x = (point.X * this._m11 + point.Y * this._m21);
            double y = (point.X * this._m12 + point.Y * this._m22);

            result.X = (int)Math.Round(x);
            result.Y = (int)Math.Round(y);

            return result;
        }

        private void Normalize()
        {
            float mta = _m11 * _m11 + _m12 * _m12;
            if (mta != 1 && mta != 0)
            {
                float na = 1.0f / (float)Math.Sqrt(mta);
                this._m11 *= na;
                this._m12 *= na;
            }

            float mtb = _m21 * _m21 + _m22 * _m22;
            if (mtb != 1 && mtb != 0)
            {
                float nb = 1.0f / (float)Math.Sqrt(mtb);
                this._m11 *= nb;
                this._m12 *= nb;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0:r}, {1:r}, {2:r}, {3:r}, {4:r}, {5:r}",
                this._m11, this._m12, this._m21, this._m22, this._dx, this._dy);
        }

        public override int GetHashCode()
        {
            return _m11.GetHashCode() ^ _m12.GetHashCode()
                ^ _m21.GetHashCode() ^ _m22.GetHashCode()
                ^ _dx.GetHashCode() ^ _dy.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(object.ReferenceEquals(obj, null))
                return false;
            if (obj is Matrix)
                return Equals((Matrix)obj);
            return false;
        }

        #endregion

        #region Static Properties

        // A static readonly field doesn't work here since Matrix is mutable.
        /// <summary>
        /// Returns an identity (1,0,0,1,0,0) matrix.
        /// </summary>
        public static Matrix Identity
        {
            get { return new Matrix(); }
        }

        #endregion

        #region ICloneable<Matrix> Members

        /// <summary>
        /// Creates an exact copy of this matrix.
        /// </summary>
        /// <returns> A cloned matrix. </returns>
        public Matrix Clone()
        {
            return new Matrix(this._m11, this._m12, this._m21, this._m22, this._dx, this._dy);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        { 
            // This is here solely for compatibility with the Matrix class on the desktop.
        }

        #endregion

        #region IEquatable<Matrix> Members

        public bool Equals(Matrix other)
        {
            return
                this._dx == other._dx &&
                this._dy == other._dy &&
                this._m11 == other._m11 &&
                this._m12 == other._m12 &&
                this._m21 == other._m21 &&
                this._m22 == other._m22;
        }

        #endregion
    }
}

#endif