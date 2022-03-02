using System;
using System.Drawing;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace GeoFramework
{
	/// <summary>Represents a highly-precise rectangle.</summary>
	/// <remarks>
	/// 	<para>This class functions similar to the <strong>RectangleF</strong> class in the
	///     <strong>System.Drawing</strong> namespace, except that it uses
	///     double-floating-point precision and is also supported on the Compact Framework
	///     edition of the <strong>GeoFramework</strong>.</para>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because it is
	///     immutable (its properties can only be changed during constructors).</para>
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("GeoFramework.Design.RectangleDConverter, GeoFramework.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=3ed3cdf4fdda3400")]
#endif
    public struct RectangleD : IFormattable, IEquatable<RectangleD>, IXmlSerializable
    {
        private readonly double _Top;
        private readonly double _Bottom;
        private readonly double _Left;
        private readonly double _Right;
        
		#region Fields

		/// <summary>
		/// Represents a RectangleD having no size.
		/// </summary>
		public static readonly RectangleD Empty = new RectangleD(0.0, 0.0, 0.0, 0.0);
		
        #endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance using the specified location, width, and height.
		/// </summary>
		/// <param name="location"></param>
		public RectangleD(PointD location, SizeD size) 
            : this(location.X, location.Y, location.X + size.Width, location.Y + size.Height)
		{}

		/// <summary>
		/// Creates a new instance using the specified location, width, and height.
		/// </summary>
		/// <param name="location"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public RectangleD(PointD location, double width, double height)
			: this(location.X, location.Y, location.X + width, location.Y + height)
		{}

		/// <summary>
		/// Creates a new instance using the specified upper-left and lower-right coordinates.
		/// </summary>
		public RectangleD(PointD upperLeft, PointD lowerRight) 
            : this(upperLeft.X, upperLeft.Y, lowerRight.X, lowerRight.Y)
		{}
        
		/// <summary>
		/// Creates a new instance using the specified latitudes and longitudes.
		/// </summary>
		public RectangleD(double left, double top, double right, double bottom) 
		{
			_Top = top < bottom ? top : bottom;
			_Left = left < right ? left : right;
			_Bottom = bottom > top ? bottom : top;
			_Right = right > left ? right : left;
		}

		public RectangleD(string value) 
            : this(value, CultureInfo.CurrentCulture)
		{}

		public RectangleD(string value, CultureInfo culture)
		{
            // Split the string into words
            string[] Values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());

            // How many words are there?
            if (Values.Length == 4)
            {
                // Extract each item
                _Top = double.Parse(Values[0], culture);
                _Left = double.Parse(Values[1], culture);
                _Bottom = double.Parse(Values[2], culture);
                _Right = double.Parse(Values[3], culture);
            }
            else
            {
                throw new FormatException(Properties.Resources.RectangleD_InvalidFormat);
            }
		}

        public RectangleD(XmlReader reader)
        {
            _Top = double.Parse(
               reader.GetAttribute("Top"), CultureInfo.InvariantCulture);
            _Bottom = double.Parse(
                reader.GetAttribute("Bottom"), CultureInfo.InvariantCulture);
            _Left = double.Parse(
                reader.GetAttribute("Left"), CultureInfo.InvariantCulture);
            _Right = double.Parse(
                reader.GetAttribute("Right"), CultureInfo.InvariantCulture);
        }

		#endregion

        #region Public Properties
        
		/// <summary>Returns the top side of the rectangle.</summary>
		/// <value>A <see cref="Latitude"></see> object marking the southern-most latitude.</value>
		public double Top
		{
			get
			{
				return _Top;
			}
		}

		/// <summary>Returns the bottom side of the rectangle.</summary>
		/// <value>A <see cref="Latitude"></see> object marking the southern-most latitude.</value>
		public double Bottom
		{
			get
			{
				return _Bottom;
			}
		}
        
		/// <summary>Returns the left side of the rectangle.</summary>
		public double Left
		{
			get
			{
				return _Left;
			}
		}

		/// <summary>Returns the right side of the rectangle.</summary>
		public double Right
		{
			get
			{
				return _Right;
			}
		}
        
		/// <summary>Returns the top-to-bottom size of the rectangle.</summary>
		public double Height
		{
			get
			{
				return _Bottom - _Top;
			}
		}
        
		/// <summary>Returns the left-to-right size of the rectangle.</summary>
		public double Width
		{
			get
			{
				return _Right - _Left;
			}
		}
        
		/// <summary>Returns the width and height of the rectangle.</summary>
		public SizeD Size
		{
			get
			{
				return new SizeD(Width, Height);
			}
		}

        /// <summary>Returns the point at the center of the rectangle.</summary>
        public PointD Center
        {
            get
            {
                return new PointD(_Left + Width * 0.5, _Top + Height * 0.5);
            }
        }

		/// <summary>Returns the point at the upper-left corner of the rectangle.</summary>
		public PointD UpperLeft
		{
			get
			{
				return new PointD(_Left, _Top);
			}
		}

		/// <summary>Returns the point at the upper-right corner of the rectangle.</summary>
		public PointD UpperRight
		{
			get
			{
				return new PointD(_Right, _Top);
			}
		}

		/// <summary>Returns the point at the lower-left corner of the rectangle.</summary>
		public PointD LowerLeft
		{
			get
			{
				return new PointD(_Left, _Bottom);
			}
		}

		/// <summary>Returns the point at the lower-right corner of the rectangle.</summary>
		public PointD LowerRight
		{
			get
			{
				return new PointD(_Right, _Bottom);
			}
		}

        /// <summary>Returns the ratio of the rectangle's width to its height.</summary>
        /// <remarks>
        /// This property returns the ratio of the RectangleDs width to its height (width / height).  This
        /// property gives an indication of the RectangleD's shape.  An aspect ratio of one indicates
        /// a square, whereas an aspect ratio of two indicates a RectangleD which is twice as wide as
        /// it is high.  
        /// </remarks>
        public double AspectRatio
        {
            get
            {
                return Width / Height;
            }
        }

		/// <summary>Indicates if the rectangle has any value.</summary>
		public bool IsEmpty
		{
			get
			{
				return (_Top == 0 && _Bottom == 0 && _Left == 0 && _Right == 0);
			}
		}

        #endregion

        #region Public Methods


        /// <summary>
        /// Changes the size and shape of the RectangleD to match the aspect ratio of the specified RectangleD.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <remarks>This method will expand a RectangleD outward, from its center point, until
        /// the ratio of its width to its height matches the specified value.</remarks>
        public RectangleD ToAspectRatio(SizeD size)
        {
            // Calculate the aspect ratio
            return ToAspectRatio(size.Width / size.Height);
        }


        /// <summary>
        /// Returns whether the specified rectangle is not overlapping the current
        /// instance.
        /// </summary>
        public bool IsDisjointedFrom(RectangleD rectangle)
        {
            return !IsOverlapping(rectangle);
        }

        /// <summary>
        /// Indicates if the specified RectangleD is entirely within the current RectangleD.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool IsEnclosing(RectangleD rectangle)
        {
            return !(rectangle.Left < _Left || rectangle.Right > _Right || rectangle.Top < _Top || rectangle.Bottom > _Bottom);
        }

        public bool IsEnclosing(PointD point)
        {
            return !(point.X < _Left || point.Y < _Top || point.X > _Right || point.Y > _Bottom);
        }

        /// <summary>Moves the rectangle so that the specified point is at its center.</summary>
        public RectangleD CenterOn(PointD point)
        {
            return new RectangleD(new PointD(point.X - (Width * 0.5), point.Y - (Height * 0.5)), Size);
        }

        public RectangleD Inflate(SizeD size)
        {
            return Inflate(size.Width, size.Height);
        }

        /// <summary>
        /// Returns a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public RectangleD Clone()
        {
            return new RectangleD(_Top, _Left, _Bottom, _Right);
        }

        /// <summary>
        /// Expands the edges of the RectangleD to contain the specified position.
        /// </summary>
        /// <param name="point">A <strong>PointD</strong> object to surround.</param>
        /// <returns>A <strong>RectangleD</strong> which contains the specified position.</returns>
        /// <remarks>If the specified position is already enclosed, the current instance will be returned.</remarks>
        public RectangleD UnionWith(PointD point)
        {
            // Does the box already contain the position?  If so, do nothing
            if (IsEnclosing(point)) return this;
            // Return the expanded box
            return new RectangleD(
                point.Y < _Top ? point.Y : _Top,
                point.X < _Left ? point.X : _Left,
                point.Y > _Bottom ? point.Y : _Bottom,
                point.X > _Right ? point.X : _Right);
        }

        /// <summary>
        /// Returns a rectangle enclosing the corner points of the current instance, rotated
        /// by the specified amount around a specific point.
        /// </summary>
        /// <returns>A new <strong>RectangleD</strong> containing the rotated rectangle.</returns>
        /// <remarks><para>When a rectangle is rotated, the total width and height it occupies may be larger
        /// than the rectangle's own width and height.  This method calculates the smallest rectangle
        /// which encloses the rotated rectangle.</para>
        /// 	<para>[TODO: Include before and after picture; this is confusing.]</para>
        /// </remarks>
        public RectangleD RotateAt(Angle angle, PointD center)
        {
            if (angle.IsEmpty) return this;
            // Rotate each corner point
            PointD[] Points = new PointD[] {UpperLeft.RotateAt(angle, center),
											UpperRight.RotateAt(angle, center),
											LowerRight.RotateAt(angle, center),
											LowerLeft.RotateAt(angle, center)};
            // Now return the smallest rectangle which encloses these points
            return RectangleD.FromArray(Points);
        }

        /// <summary>
        /// Returns a rectangle enclosing the corner points of the current instance, rotated
        /// by the specified amount.
        /// </summary>
        public RectangleD Rotate(Angle angle)
        {
            if (angle.IsEmpty) return this;
            // Rotate each corner point
            PointD[] Points = new PointD[] {UpperLeft.RotateAt(angle, Center),
											   UpperRight.RotateAt(angle, Center),
											   LowerRight.RotateAt(angle, Center),
											   LowerLeft.RotateAt(angle, Center)};
            // Now return the smallest rectangle which encloses these points
            return RectangleD.FromArray(Points);
        }

        /// <summary>Returns the corner points of the rectangle as an array.</summary>
        public PointD[] ToArray()
        {
            return new PointD[] { UpperLeft, UpperRight, LowerRight, LowerLeft, UpperLeft };
        }

        /// <summary>
        /// Changes the size and shape of the RectangleD to match the specified aspect ratio.
        /// </summary>
        /// <param name="aspectRatio"></param>
        /// <returns></returns>
        /// <remarks>This method will expand a RectangleD outward, from its center point, until
        /// the ratio of its width to its height matches the specified value.</remarks>
        public RectangleD ToAspectRatio(double aspectRatio)
        {
            double CurrentAspect = AspectRatio;
            // Do the values already match?
            if (CurrentAspect == aspectRatio) return this;
            // Is the new ratio higher or lower?
            if (aspectRatio > CurrentAspect)
            {
                // Inflate the RectangleD to the new height minus the current height
                // TESTS OK
                return Inflate(aspectRatio * Height - Width, 0);
            }
            else
            {
                // Inflate the RectangleD to the new height minus the current height
                return Inflate(0, Width / aspectRatio - Height);
            }
        }

        public RectangleD Translate(PointD offset)
        {
            return new RectangleD(UpperLeft.Add(offset), Size);
        }

        public RectangleD Translate(double offsetX, double offsetY)
        {
            return new RectangleD(UpperLeft.Add(offsetX, offsetY), Size);
        }

        /// <summary>
        /// Changes the size and shape of the RectangleD to match the aspect ratio of the specified RectangleD.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        /// <remarks>This method will expand a RectangleD outward, from its center point, until
        /// the ratio of its width to its height matches the specified value.</remarks>
        public RectangleD ToAspectRatio(RectangleD rectangle)
        {
            // Calculate the aspect ratio
            return ToAspectRatio(rectangle.Width / rectangle.Height);
        }

        public RectangleD Inflate(double widthOffset, double heightOffset)
        {
            widthOffset *= .5;
            heightOffset *= .5;

            double top = this._Top - heightOffset;
            double bottom = this._Bottom + heightOffset;
            double left = this._Left - widthOffset;
            double right = this._Right + widthOffset;
            if (top > bottom || right < left)
                return this;
            else
                return new RectangleD(left, top, right, bottom);
        }

        public RectangleD IntersectionOf(RectangleD rectangle)
        {
            // Return nothing if no intersection is possible
            if (!IsIntersectingWith(rectangle)) return RectangleD.Empty;
            // Return the rectangle representing the intersection
            double NewLeft = (rectangle.Left > _Left ? rectangle.Left : _Left);
            double NewTop = (rectangle.Top > _Top ? rectangle.Top : _Top);
            double NewRight = (rectangle.Right < _Right ? rectangle.Right : _Right);
            double NewBottom = (rectangle.Bottom < _Bottom ? rectangle.Bottom : _Bottom);
            return new RectangleD(NewTop, NewLeft, NewBottom, NewRight);
        }

        /// <summary>Returns whether the current instance overlaps the specified rectangle.</summary>
        public bool IsIntersectingWith(RectangleD rectangle)
        {
            if (rectangle.Left >= _Left && rectangle.Left <= _Right)
                if (rectangle.Top >= _Top && rectangle.Top <= _Bottom)
                    return true;
                else if (rectangle.Bottom >= _Top && rectangle.Bottom <= _Bottom)
                    return true;
                else if (rectangle.Right >= _Left && rectangle.Right <= _Right)
                    if (rectangle.Top >= _Top && rectangle.Top <= _Bottom)
                        return true;
                    else if (rectangle.Bottom >= _Top && rectangle.Bottom <= _Bottom)
                        return true;
            return false;
            //
            //			return ((rectangle.Left >= Left ) && (rectangle.Left <= Right) && (rectangle.Latitude >= Latitude) && (rectangle.Latitude <= Bottom))
            //				|| ((rectangle.Left + rectangle.Width >= Left ) && (rectangle.Left + rectangle.Width <= Left + Width) && (rectangle.Latitude >= Latitude) && (rectangle.Latitude <= Latitude + Height))
            //				|| ((rectangle.Left >= Left ) && (rectangle.Left <= Left + Width) && (rectangle.Latitude + rectangle.Height >= Latitude) && (rectangle.Latitude + rectangle.Height <= Latitude + Height))
            //				|| ((rectangle.Left + rectangle.Width >= Left ) && (rectangle.Left + rectangle.Width <= Left + Width) && (rectangle.Latitude + rectangle.Height >= Latitude) && (rectangle.Latitude + rectangle.Height <= Latitude + Height));
        }

        /// <summary>
        /// Indicates if the specified RectangleD shares any of the same 2D space as the current instance.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool IsOverlapping(RectangleD rectangle)
        {
            return !((rectangle.Top > _Bottom)
            || (rectangle.Bottom < _Top)
            || (rectangle.Left > _Right)
            || (rectangle.Right < _Left));
            //return true;

            //			if(rectangle.Top < Top)
            //				if(rectangle.Bottom < Top) return false;
            //			if(rectangle.Bottom > Bottom)
            //				if(rectangle.Top > Bottom) return false;
            //			if(rectangle.Left < Left)
            //				if(rectangle.Right < Left) return false;
            //			if(rectangle.Right > Right)
            //				if(rectangle.Left > Right) return false;
            //			return true;
        }

        /// <summary>
        /// Returns whether the current instance is surrounded on all sides by the specified
        /// rectangle.
        /// </summary>
        public bool IsInsideOf(RectangleD rectangle)
        {
            return (Left > rectangle.Left
                && Right < rectangle.Right
                && _Top > rectangle.Top
                && _Bottom < rectangle.Bottom);
        }

        /// <summary>
        /// Returns whether the current instance surrounds the center point of the specified
        /// rectangle.
        /// </summary>
        public bool IsEnclosingCenter(RectangleD rectangle)
        {
            return (rectangle.Left <= Center.X
                && rectangle.Right >= Center.X
                && rectangle.Top <= Center.Y
                && rectangle.Bottom >= Center.Y);
        }

        /// <summary>
        /// Returns whether the specified rectangle shares a side with the specified
        /// rectangle.
        /// </summary>
        /// <returns>A <strong>Boolean</strong>, true if the specified rectangle shares one
        /// (and only one) side.</returns>
        /// <remarks>The method will return false if the specified rectangle intersects with the
        /// current instance.</remarks>
        public bool IsAdjacentTo(RectangleD rectangle)
        {
            if (rectangle.Top == _Top)
                if (rectangle.Left >= _Left && rectangle.Left <= _Right)
                    return true;
                else if (rectangle.Right >= _Left && rectangle.Right <= _Right)
                    return true;
                else if (rectangle.Bottom == _Bottom)
                    if (rectangle.Left >= _Left && rectangle.Left <= _Right)
                        return true;
                    else if (rectangle.Right >= _Left && rectangle.Right <= _Right)
                        return true;
                    else if (rectangle.Left == _Left)
                        if (rectangle.Top >= _Top && rectangle.Top <= _Bottom)
                            return true;
                        else if (rectangle.Bottom >= _Top && rectangle.Bottom <= _Bottom)
                            return true;
                        else if (rectangle.Right == _Right)
                            if (rectangle.Top >= _Top && rectangle.Top <= _Bottom)
                                return true;
                            else if (rectangle.Bottom >= _Top && rectangle.Bottom <= _Bottom)
                                return true;
            return false;
        }

        public bool IsOverlapping(PointD point)
        {
            if (point.X < _Left) return false;
            if (point.X > _Right) return false;
            if (point.Y > _Top) return false;
            if (point.Y < _Bottom) return false;
            return true;
        }

        public RectangleD UnionWith(RectangleD rectangle)
        {
            // Build a new rectangle from these rectangles
            //Console.WriteLine("rectangle #1: " + ToString());
            //Console.WriteLine("rectangle #2: " + rectangle.ToString());

            return new RectangleD(rectangle.Top < _Top ? rectangle.Top : _Top,
                rectangle.Left < _Left ? rectangle.Left : _Left,
                rectangle.Bottom > _Bottom ? rectangle.Bottom : _Bottom,
                rectangle.Right > _Right ? rectangle.Right : _Right);

            //Console.WriteLine("Result: " + Result.ToString());
            //return Result;
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            // Return false if the value is null
            if (obj is RectangleD)
                return this.Equals((RectangleD)obj);
            return false;
        }

        /// <summary>Returns a unique code for this instance used in hash tables.</summary>
        public override int GetHashCode()
        {
            return _Left.GetHashCode() ^ _Right.GetHashCode() ^ _Top.GetHashCode() ^ _Bottom.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        public static RectangleD FromLTRB(double left, double top, double right, double bottom)
        {
            return new RectangleD(new PointD(left, top), new PointD(right, bottom));
        }

        /// <summary>
        /// Parses a string into a RectangleD object.
        /// </summary>
        /// <param name="value">A <string>String</string> specifying geographic coordinates defining a rectangle.</param>
        /// <returns>A <strong>RectangleD</strong> object using the specified coordinates.</returns>
        /// <remarks>This powerful method will convert points defining a rectangle in the form of a string into
        /// a RectangleD object.  The string can be </remarks>
        public static RectangleD Parse(string value)
        {
            return new RectangleD(value);
        }

        public static RectangleD Parse(string value, CultureInfo culture)
        {
            return new RectangleD(value, culture);
        }

        /// <summary>
        /// Returns the smallest possible RectangleD containing both specified RectangleDs.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static RectangleD UnionWith(RectangleD first, RectangleD second)
        {
            return first.UnionWith(second);
        }

        /// <summary>
        /// Returns a rectangle which encloses the specified points.
        /// </summary>
        /// <param name="points">An array of PointD objects to enclose.</param>
        /// <returns>A <strong>RectangleD</strong> object enclosing the specified points.</returns>
        /// <remarks>This method is typically used to calculate a rectangle surrounding
        /// points which have been rotated.  For example, if a rectangle is rotated by 45°, the
        /// total width it occupies is greater than it's own width.</remarks>
        public static RectangleD FromArray(PointD[] points)
        {
            // Start with the first point
            double top = points[0].Y;
            double left = points[0].X;
            double bottom = points[0].Y;
            double right = points[0].X;

            // Expand the points outward as limits are breached
            for (int index = 1; index < points.Length; index++)
            {
                PointD point = points[index];
                if (point.X < left)
                    left = point.X;
                else if (point.X > right)
                    right = point.X;
                if (point.Y < top)
                    top = point.Y;
                else if (point.Y > bottom)
                    bottom = point.Y;
            }

            // Build a new rectangle
            return new RectangleD(left, top, right, bottom);
        }

        /// <summary>
        /// Returns the RectangleD formed by the intersection of the two specified RectangleDs.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static RectangleD IntersectionOf(RectangleD first, RectangleD second)
        {
            return first.IntersectionOf(second);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Increases the size of the rectangle by the specified amount.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static RectangleD operator +(RectangleD left, SizeD size)
        {
            return new RectangleD(left.UpperLeft, left.Size.Add(size));
        }

        public static RectangleD operator +(RectangleD left, PointD location)
        {
            return new RectangleD(left.UpperLeft.Add(location), left.Size);
        }

        public static RectangleD operator -(RectangleD left, SizeD size)
        {
            return new RectangleD(left.UpperLeft, left.Size.Subtract(size));
        }

        public static RectangleD operator -(RectangleD left, PointD location)
        {
            return new RectangleD(left.UpperLeft.Subtract(location), left.Size);
        }

        public static RectangleD operator *(RectangleD left, SizeD size)
        {
            return new RectangleD(left.UpperLeft, left.Size.Multiply(size));
        }

        public static RectangleD operator *(RectangleD left, PointD location)
        {
            return new RectangleD(left.UpperLeft.Multiply(location), left.Size);
        }

        public static RectangleD operator /(RectangleD left, SizeD size)
        {
            return new RectangleD(left.UpperLeft, left.Size.Divide(size));
        }

        public static RectangleD operator /(RectangleD left, PointD location)
        {
            return new RectangleD(left.UpperLeft.Divide(location), left.Size);
        }

        public static bool operator ==(RectangleD left, RectangleD right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleD left, RectangleD right)
        {
            return !left.Equals(right);
        }

        public RectangleD Add(SizeD size)
        {
            return new RectangleD(UpperLeft, Size.Add(size));
        }

        public RectangleD Subtract(SizeD size)
        {
            return new RectangleD(UpperLeft, Size.Subtract(size));
        }

        public RectangleD Multiply(SizeD size)
        {
            return new RectangleD(UpperLeft, Size.Multiply(size));
        }

        public RectangleD Divide(SizeD size)
        {
            return new RectangleD(UpperLeft, Size.Divide(size));
        }

        public RectangleD Add(PointD position)
        {
            return new RectangleD(UpperLeft.Add(position), Size);
        }

        public RectangleD Subtract(PointD position)
        {
            return new RectangleD(UpperLeft.Subtract(position), Size);
        }

        public RectangleD Multiply(PointD position)
        {
            return new RectangleD(UpperLeft.Multiply(position), Size);
        }

        public RectangleD Divide(PointD position)
        {
            return new RectangleD(UpperLeft.Divide(position), Size);
        }

        #endregion

        #region Conversions

        public static explicit operator RectangleD(Rectangle value) 
		{
			return new RectangleD((double)value.Top, (double)value.Left, (double)value.Bottom, (double)value.Right);
		}

		public static explicit operator RectangleD(RectangleF value) 
		{
			return new RectangleD((double)value.Top, (double)value.Left, (double)value.Bottom, (double)value.Right);
		}

		public static explicit operator Rectangle(RectangleD value) 
		{
			return new Rectangle((int)value.Top, (int)value.Left, (int)(value.Bottom - value.Top), (int)(value.Right - value.Left));
		}

		public static explicit operator RectangleF(RectangleD value) 
		{
			return new RectangleF((float)value.Left, (float)value.Top, (float)(value.Width), (float)(value.Height));
        }

        #endregion

        #region IEquatable<RectangleD> Members

        public bool Equals(RectangleD other)
        {
            // The objects are equivalent if their bounds are equivalent
            return _Left == other.Left
                && _Right == other.Right
                && _Top == other.Top
                && _Bottom == other.Bottom;
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;
            return _Left.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator
                + _Top.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator
                + Width.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator
                + Height.ToString(format, formatProvider);
        }

		#endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Top",
                        _Top.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Bottom",
                        _Bottom.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Left",
                        _Left.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Right",
                        _Right.ToString("G17", CultureInfo.InvariantCulture));
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new InvalidOperationException("Use the RectangleD(XmlReader) constructor to create a new instance instead of calling ReadXml.");
        }

        #endregion


        #region Unused Code (Commented Out)

        //		/// <summary>
        //		/// Changes the size and shape of the RectangleD to match the aspect ratio of the specified RectangleD.
        //		/// </summary>
        //		/// <param name="aspectRatio"></param>
        //		/// <returns></returns>
        //		/// <remarks>This method will expand a RectangleD outward, from its center point, until
        //		/// the ratio of its width to its height matches the specified value.</remarks>
        //		public RectangleD ToAspectRatio(System.Drawing.Rectangle rectangle)
        //		{
        //			// Calculate the aspect ratio
        //			return ToAspectRatio((double)RectangleD.Width / (double)RectangleD.Height);
        //		}
        //
        //		/// <summary>
        //		/// Changes the size and shape of the RectangleD to match the aspect ratio of the specified RectangleD.
        //		/// </summary>
        //		/// <param name="aspectRatio"></param>
        //		/// <returns></returns>
        //		/// <remarks>This method will expand a RectangleD outward, from its center point, until
        //		/// the ratio of its width to its height matches the specified value.</remarks>
        //		public RectangleD ToAspectRatio(System.Drawing.RectangleF RectangleD)
        //		{
        //			// Calculate the aspect ratio
        //			return ToAspectRatio((double)RectangleD.Width / (double)RectangleD.Height);
        //		}

        //		/// <summary>
        //		/// Moves the entire RectangleD in the specified direction by the specified distance.
        //		/// </summary>
        //		/// <param name="direction"></param>
        //		/// <param name="distance"></param>
        //		/// <returns></returns>
        //		////[CLSCompliant(false)]
        //		public RectangleD TranslateTo(Angle direction, double distance)
        //		{
        //			// Find the new translation point
        //			return new RectangleD(UpperLeft.TranslateTo(direction, distance), Size);
        //		}

        #endregion
    }
}
