#if PocketPC

using System.Globalization;
using System;
using System.Drawing;

namespace GeoFramework
{
    /// <summary>
    /// Represents a coordinate with single precision.
    /// </summary>
    public struct PointF : IEquatable<PointF>, IFormattable 
    {
        private float _x;
        private float _y;

        #region Fields

        public static PointF Empty = new PointF(0, 0);

        #endregion

        #region Constructors

        public PointF(float x, float y)
        {
            this._x = x;
            this._y = y;
        }

        #endregion

        #region Public Properties

        public float X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        public float Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

        public bool IsEmpty
        {
            get { return this.Equals(PointF.Empty); }
        }

        #endregion

        #region Public Methods

        public Point ToPoint()
        {
            return new Point((int)_x, (int)_y);
        }

        #endregion

        #region Static Methods

        public static PointF FromSize(Size size)
        {
            return new PointF(size.Width, size.Height);
        }

        public static PointF FromSize(SizeF size)
        {
            return new PointF(size.Width, size.Height);
        }

        #endregion

        #region Operators

        public static bool operator ==(PointF left, PointF right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PointF left, PointF right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is PointF)
                return Equals((PointF)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region IEquatable<PointF> Members

        public bool Equals(PointF other)
        {            
            return _x.Equals(other.X)
                && _y.Equals(other.Y);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;
            return _x.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator
                + " " + _y.ToString(format, formatProvider);
        }

        #endregion
    }
}

#endif