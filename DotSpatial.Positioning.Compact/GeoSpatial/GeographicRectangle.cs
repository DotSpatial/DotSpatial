using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning
{
	/// <summary>
	/// Represents a rectangular shape on Earth's surface.
	/// </summary>
	/// <remarks>
	/// 	<para>This class is used to represent a square (or close to a square) shape on
	///     Earth's surface. This class is typically used during mapping applications to zoom
	///     into a particular area on Earth. This class looks nearly identical to the Rectangle
	///     class in the .NET framework, except that it's bounding points are defined as
	///     <strong>Position</strong> objects instead of <strong>Point</strong> objects.</para>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
	///     immutable (it's properties can only be set via constructors).</para>
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.GeographicRectangleConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct GeographicRectangle : IFormattable, IEquatable<GeographicRectangle>, IXmlSerializable
    {
        private Latitude _Top;
        private Latitude _Bottom;
        private Longitude _Left;
        private Longitude _Right;
        private Position _Center;

        #region Fields

        /// <summary>Represents a GeographicRectangle having no size.</summary>
        public static readonly GeographicRectangle Empty = new GeographicRectangle(Latitude.Empty, Longitude.Empty, Latitude.Empty, Longitude.Empty);
        /// <summary>Represents a rectangle that encompasses all of Earth's surface.</summary>
        public static readonly GeographicRectangle Maximum = new GeographicRectangle(Latitude.Maximum, Longitude.Minimum, Latitude.Minimum, Longitude.Maximum);

        #endregion

		#region Constructors

		/// <summary>Creates a new instance using the specified location and size.</summary>
		/// <returns>
		/// A <strong>GeographicRectangle</strong> set to the specified location and
		/// size.
		/// </returns>
		/// <remarks>
		/// This constructor defines a rectangle which expands east and south of the
		/// specified location.
		/// </remarks>
		/// <example>
		///     This example creates a new <strong>GeographicRectangle</strong> starting at 39°N
		///     105°W which is 2° wide and 5° tall. 
		///     <code lang="VB" title="[New Example]">
		/// Dim NorthwestCorner As New Position("39N 105W")
		/// Dim RectangleSize As New GeographicSize(2, 5)
		/// Dim Rectangle As New GeographicRectangle(NorthwestCorner, RectangleSize)
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// Position NorthwestCorner = new Position("39N,105W");
		/// GeographicSize RectangleSize = new GeographicSize(2, 5);
		/// GeographicRectangle Rectangle = new GeographicRectangle(NorthwestCorner, RectangleSize);
		///     </code>
		/// </example>
		public GeographicRectangle(Position location, GeographicSize size)
		{
            _Left = location.Longitude;
            _Right = location.TranslateTo(Azimuth.East, size.Width).Longitude;
            _Top = location.Latitude;
            _Bottom = location.TranslateTo(Azimuth.South, size.Height).Latitude;
            _Center = Position.Invalid;
            _Center = Hypotenuse.Midpoint;
        }

		/// <summary>
		/// Creates a new instance using the specified location, width, and height.
		/// </summary>
		/// <remarks>
		/// This constructor defines a rectangle which expands east and south of the
		/// specified location.
		/// </remarks>
		/// <example>
		///     This example creates a new <strong>GeographicRectangle</strong> starting at 39°N
		///     105°W which is 2° wide and 5° tall. 
		///     <code lang="VB" title="[New Example]">
		/// Dim NorthwestCorner As New Position("39N 105W")
		/// Dim RectangleWidth As Distance = Distance.FromKilometers(1)
        /// Dim RectangleHeight As Distance = Distance.FromKilometers(1)
		/// Dim Rectangle As New GeographicRectangle(NorthwestCorner, RectangleWidth, RectangleHeight)
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// Position NorthwestCorner = new Position("39N 105W");
        /// Distance RectangleWidth = Distance.FromKilometers(1);
        /// Distance RectangleHeight = Distance.FromKilometers(1);
		/// GeographicRectangle Rectangle = new GeographicRectangle(NorthwestCorner, RectangleWidth, RectangleHeight);
		///     </code>
		/// </example>
		/// <returns>
		/// A <strong>GeographicRectangle</strong> set to the specified location and
		/// size.
		/// </returns>
		public GeographicRectangle(Position location, Distance width, Distance height)
		{
            _Left = location.Longitude;
            _Right = location.TranslateTo(Azimuth.East, width).Longitude;
            _Top = location.Latitude;
            _Bottom = location.TranslateTo(Azimuth.South, height).Latitude;

            _Center = Position.Invalid;
            _Center = Hypotenuse.Midpoint;
        }

		/// <summary>
		/// Creates a new instance using the specified northwest and southeast
		/// coordinates.
		/// </summary>
		/// <remarks>
		/// This constructor takes the specified parameters and calculates the width and
		/// height of the rectangle. If the two points are backwards (meaning that the right-most
		/// point is west of the left-most point), they are automatically swapped before creating
		/// the rectangle.
		/// </remarks>
		/// <returns>A <strong>GeographicRectangle</strong> defined by the two endpoints.</returns>
		/// <example>
		///     This example creates a new <strong>GeographicRectangle</strong> starting at 39°N
		///     105°W and ending at 37°N 100°W (2° wide and 5° tall). 
		///     <code lang="VB" title="[New Example]">
		/// Dim NorthwestCorner As New Position("39N 105W")
		/// Dim SoutheastCorner As New Position("37N 100W")
		/// Dim Rectangle As New GeographicRectangle(NorthwestCorner, SoutheastCorner)
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// Position NorthwestCorner = new Position("39N 105W");
		/// Position SoutheastCorner = new Position("37N 100W");
		/// GeographicRectangle Rectangle = new GeographicRectangle(NorthwestCorner, SoutheastCorner);
		///     </code>
		/// </example>
		public GeographicRectangle(Position northwest, Position southeast) 
		{
            _Left = northwest.Longitude;
            _Right = southeast.Longitude;
            _Top = northwest.Latitude;
            _Bottom = southeast.Latitude;

            _Center = Position.Invalid;
            _Center = Hypotenuse.Midpoint;
        }

		/// <summary>Creates a new instance by converting the specified string.</summary>
		/// <returns>
		/// A <strong>GeographicRectangle</strong> matching the specified string
		/// value.
		/// </returns>
		/// <remarks>
		/// This constructor attempts to parse the specified string into a rectangle. The
		/// current culture is used to interpret the string -- use the list separator of the
		/// current culture (which may not necessarily be a comma). This constructor can accept any
		/// output created via the <strong>ToString</strong> method.
		/// </remarks>
		/// <example>
		///     This example creates a new rectangle at 39°N, 105° extending two degrees south and
		///     five degrees east to 37°N, 100°W. 
		///     <code lang="VB" title="[New Example]">
		/// Dim Rectangle As New GeographicRectangle("39N,105W,37N,100W")
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// GeographicRectangle Rectangle = new GeographicRectangle("39N,105W,37N,100W");
		///     </code>
		/// </example>
        public GeographicRectangle(string value) 
            : this(value, CultureInfo.CurrentCulture)
        {}

		/// <summary>
		/// Creates a new instance by converting the specified string in the given
		/// culture.
		/// </summary>
		/// <returns>
		/// This constructor attempts to parse the specified string into a rectangle. The
		/// specified culture is used to interpret the string. This constructor can accept any
		/// output created via the <strong>ToString</strong> method.
		/// </returns>
		/// <example>
		///     This example creates a new rectangle at 39°N, 105° extending two degrees south and
		///     five degrees east to 37°N, 100°W. 
		///     <code lang="VB" title="[New Example]">
		/// Dim Rectangle As New GeographicRectangle("39N,105W,37N,100W", CultureInfo.CurrentCulture)
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// GeographicRectangle Rectangle = new GeographicRectangle("39N,105W,37N,100W", CultureInfo.CurrentCulture);
		///     </code>
		/// </example>
        public GeographicRectangle(string value, CultureInfo culture)
        {
            // Default to zero if value is null
            if (string.IsNullOrEmpty(value))
            {
                _Top = Latitude.Empty;
                _Bottom = Latitude.Empty;
                _Left = Longitude.Empty;
                _Right = Longitude.Empty;

                _Center = Position.Invalid;
                _Center = Hypotenuse.Midpoint;
                return;
            }

            // Default to the current culture
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

			// Split the string into words
			string[] Values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());
			
            // How many words are there?
			switch(Values.Length)
			{
				case 4:
					// Extract each item
					try
					{
                        bool IsTopHandled = false;
                        bool IsLeftHandled = false;
                        bool IsRightHandled = false;
                        bool IsBottomHandled = false;

                        Latitude TempTop = Latitude.Empty;
                        Longitude TempLeft = Longitude.Empty;
                        Latitude TempBottom = Latitude.Empty;
                        Longitude TempRight = Longitude.Empty;

                        for (int index = 0; index < Values.Length; index++)
                        {
                            // Is this a latitude or longitude?
                            string word = Values[index];
#if Framework20
                            if (word.IndexOf("W", StringComparison.InvariantCultureIgnoreCase) != -1
                                || word.IndexOf("E", StringComparison.InvariantCultureIgnoreCase) != -1)
#else
							if (word.IndexOf("W") != -1
								|| word.IndexOf("E") != -1)
#endif
                            {
                                if (IsLeftHandled && IsRightHandled)
                                    throw new FormatException("A GeographicRectangle object could not be converted from a string because more than two longitude values were encountered.  Only two are allowed.");

                                // Longitude.  Is the left handled?
                                if (IsLeftHandled)
                                {
                                    // This is the right side
                                    TempRight = Longitude.Parse(word, culture);
                                    IsRightHandled = true;
                                }
                                else
                                {
                                    // This is the left side
                                    TempLeft = Longitude.Parse(word, culture);
                                    IsLeftHandled = true;
                                }
                            }
#if Framework20
                            else if (word.IndexOf("N", StringComparison.InvariantCultureIgnoreCase) != -1
                                || word.IndexOf("S", StringComparison.InvariantCultureIgnoreCase) != -1)
#else
							else if (word.IndexOf("N") != -1
								|| word.IndexOf("S") != -1)
#endif
                            {
                                if (IsTopHandled && IsBottomHandled)
                                    throw new FormatException("A GeographicRectangle object could not be converted from a string because more than two latitude values were encountered.  Only two are allowed.");

                                // Longitude.  Is the left handled?
                                if (IsTopHandled)
                                {
                                    // This is the bottom side
                                    TempBottom = Latitude.Parse(word, culture);
                                    IsBottomHandled = true;
                                }
                                else
                                {
                                    // This is the top side
                                    TempTop = Latitude.Parse(word, culture);
                                    IsTopHandled = true;
                                }
                            }
                            else
                            {
                                throw new FormatException("A GeographicRectangle object could not be created because a number could not be categorized as a latitude or longitude.  Add a N, S, E, or W letter to resolve the issue.");
                            }
                        }

                        // Flip any coordinates which are backwards
                        _Top = TempTop.DecimalDegrees > TempBottom.DecimalDegrees ? TempTop : TempBottom;
                        _Left = TempLeft.DecimalDegrees < TempRight.DecimalDegrees ? TempLeft : TempRight;
                        _Bottom = TempBottom.DecimalDegrees < TempTop.DecimalDegrees ? TempBottom : TempTop;
                        _Right = TempRight.DecimalDegrees > TempLeft.DecimalDegrees ? TempRight : TempLeft;

                        _Center = Position.Invalid;
                        _Center = Hypotenuse.Midpoint;
                        break;
					}
					catch
					{
						throw;
					}
				default:
					throw new FormatException("The specified value could not be parsed into a GeographicRectangle object because four delimited values are required (Top, Left, Bottom, Right).");
			}
        }

		/// <summary>
		/// Creates a new instance using the specified latitudes and longitudes.
		/// </summary>
		/// <example>
		///     This example creates a new <strong>GeographicRectangle</strong> by specifying each
		///     side individually. 
		///     <code lang="VB" title="[New Example]">
		/// Dim Left As New Longitude(-105)
		/// Dim Top As New Latitude(39)
		/// Dim Right As New Longitude(-100)
		/// Dim Top As New Latitude(37)
		/// Dim Rectangle As New GeographicRectangle(Left, Top, Right, Bottom)
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// Longitude Left = new Longitude(-105);
		/// Latitude Top = new Latitude(39);
		/// Longitude Right = new Longitude(-100);
		/// Latitude Top = new Latitude(37);
		/// GeographicRectangle Rectangle = new GeographicRectangle(Left, Top, Right, Bottom);
		///     </code>
		/// </example>
		/// <returns>A <strong>GeographicRectangle</strong> bound by the specified values.</returns>
		/// <remarks>
		/// If the left and right, or top and bottom values are backwards, they are
		/// automatically swapped before creating the rectangle.
		/// </remarks>
		public GeographicRectangle(Longitude left, Latitude top, Longitude right, Latitude bottom) 
            : this(top, left, bottom, right)
		{}
		
		/// <summary>Creates a new instance using the specified latitudes and longitudes.</summary>
		/// <remarks>
		/// If the left and right, or top and bottom values are backwards, they are
		/// automatically swapped before creating the rectangle.
		/// </remarks>
		/// <returns>A <strong>GeographicRectangle</strong> bound by the specified values.</returns>
		/// <example>
		/// 	<code lang="VB" title="[New Example]">
		/// Dim Left As New Longitude(-105)
		/// Dim Top As New Latitude(39)
		/// Dim Right As New Longitude(-100)
		/// Dim Top As New Latitude(37)
		/// Dim Rectangle As New GeographicRectangle(Left, Top, Right, Bottom)
		///     </code>
		/// 	<code lang="CS" title="[New Example]">
		/// Latitude Top = new Latitude(39);
		/// Longitude Left = new Longitude(-105);
		/// Latitude Bottom = new Latitude(37);
		/// Longitude Right = new Longitude(-100);
		/// GeographicRectangle Rectangle = new GeographicRectangle(Top, Left, Bottom, Right);
		///     </code>
		/// </example>
        public GeographicRectangle(Latitude top, Longitude left, Latitude bottom, Longitude right)
        {
            _Left = left;
            _Right = right;
            _Top = top;
            _Bottom = bottom;

            _Center = Position.Invalid;
            _Center = Hypotenuse.Midpoint;
        }

        public GeographicRectangle(double left, double top, double right, double bottom)
        {
            _Left = new Longitude(left);
            _Right = new Longitude(right);
            _Top = new Latitude(top);
            _Bottom = new Latitude(bottom);

            _Center = Position.Invalid;
            _Center = Hypotenuse.Midpoint;
        }
        /// <summary>
        /// Creates a new instance from a block of Geography Markup Language (GML).
        /// </summary>
        /// <param name="reader"></param>
        public GeographicRectangle(XmlReader reader)
        {
            // Initialize all fields
            _Top = Latitude.Invalid;
            _Bottom = Latitude.Invalid;
            _Left = Longitude.Invalid;
            _Right = Longitude.Invalid;
            _Center = Position.Invalid;

            // Deserialize the object from XML
            ReadXml(reader);
        }

		#endregion

        #region Public Properties

        /// <summary>Returns the southern-most side of the rectangle.</summary>
        /// <value>A <see cref="Latitude"></see> object marking the southern-most latitude.</value>
        public Latitude Top
        {
            get
            {
                return _Top;
            }
        }

        /// <summary>Returns the southern-most latitude of the rectangle.</summary>
        /// <value>A <see cref="Latitude"></see> object marking the southern-most latitude.</value>
        public Latitude Bottom
        {
            get
            {
                return _Bottom;
            }
        }

        /// <summary>Returns the western-most side of the rectangle.</summary>
        /// <value>A <strong>Longitude</strong> indicating the left side of the rectangle.</value>
        public Longitude Left
        {
            get
            {
                return _Left;
            }
        }

        /// <value>A <strong>Longitude</strong> indicating the right side of the rectangle.</value>
        /// <summary>Returns the eastern-most side of the rectangle.</summary>
        public Longitude Right
        {
            get
            {
                return _Right;
            }
        }

        /// <summary>Returns the geographic center of the rectangle.</summary>
        public Position Center
        {
            get
            {
                return _Center;
            }
        }

        /// <summary>Returns the aspect ratio of the rectangle.</summary>
        /// <remarks>
        /// This property returns the ratio of the GeographicRectangles width to its height (width / height).  This
        /// property gives an indication of the GeographicRectangle's shape.  An aspect ratio of one indicates
        /// a square, whereas an aspect ratio of two indicates a GeographicRectangle which is twice as wide as
        /// it is high.  
        /// </remarks>
        public float AspectRatio
        {
            get
            {
                return Convert.ToSingle(WidthDegrees.DecimalDegrees / HeightDegrees.DecimalDegrees);
            }
        }

        /// <summary>Indicates if the rectangle has any value.</summary>
        /// <value>
        /// A <strong>Boolean</strong>, <strong>True</strong> if a metor the size of Rhode
        /// Island is about to crash into the Pacific Ocean just off the coast of Nicaragua but
        /// there will be no casualties because everyone was warned plenty of time in
        /// advance.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return _Top.IsEmpty && _Bottom.IsEmpty && _Left.IsEmpty && _Right.IsEmpty;
            }
        }

        /// <summary>
        /// Returns the rectangle's hypotenuse. 
        /// </summary>
        /// <remarks>The hypotenuse of a rectangle is a line connecting its northwest corner with its southeast corner.</remarks>
        public Segment Hypotenuse
        {
            get
            {
                return new Segment(Northwest, Southeast);
            }
        }


        /// <summary>
        /// Returns the distance from the left to the right at the rectangle's middle latitude.
        /// </summary>
        public Distance Width
        {
            get
            {
                // Return the calculated distance
                return WestCenter.DistanceTo(EastCenter).ToLocalUnitType();
            }
        }

        /// <summary>
        /// Returns the distance from the top to the bottom at the rectangle's middle longitude.
        /// </summary>
        public Distance Height
        {
            get
            {
                // Return the calculated distance
                return NorthCenter.DistanceTo(SouthCenter).ToLocalUnitType();
            }
        }

        public Latitude HeightDegrees
        {
            get
            {
                return _Top.Subtract(_Bottom);
            }
        }

        public Longitude WidthDegrees
        {
            get
            {
                return _Right.Subtract(_Left);
            }
        }

        /// <summary>Returns the width and height of the rectangle.</summary>
        public GeographicSize Size
        {
            get
            {
                return new GeographicSize(Width, Height);
            }
        }

        /// <summary>Returns the northwestern corner of the rectangle.</summary>
        public Position Northwest
        {
            get
            {
                return new Position(_Left, _Top);
            }
        }

        /// <summary>
        /// Returns a point on the northern side, centered horizontally.
        /// </summary>
        public Position NorthCenter
        {
            get
            {
                return new Position(_Center.Longitude, _Top);
            }
        }

        /// <summary>
        /// Returns a point on the southern side, centered horizontally.
        /// </summary>
        public Position SouthCenter
        {
            get
            {
                return new Position(_Center.Longitude, _Bottom);
            }
        }

        /// <summary>
        /// Returns a point on the western side, centered vertically.
        /// </summary>
        public Position WestCenter
        {
            get
            {
                return new Position(_Left, Center.Latitude);
            }
        }

        /// <summary>
        /// Returns a point on the eastern side, centered vertically.
        /// </summary>
        public Position EastCenter
        {
            get
            {
                return new Position(_Right, Center.Latitude);
            }
        }

        /// <summary>Returns the northeastern corner of the rectangle.</summary>
        public Position Northeast
        {
            get
            {
                return new Position(_Right, _Top);
            }
        }

        /// <summary>Returns the southwestern corner of the rectangle.</summary>
        public Position Southwest
        {
            get
            {
                return new Position(_Left, _Bottom);
            }
        }

        /// <summary>Returns the southeastern corner of the rectangle.</summary>
        public Position Southeast
        {
            get
            {
                return new Position(_Right, _Bottom);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>Moves the rectangle in the specified direction by the specified distance.</summary>
        /// <returns>
        /// A new <strong>GeographicRectangle</strong> translated by the specified direction
        /// and distance.
        /// </returns>
        /// <remarks>
        /// This method is used to shift a rectangle to a new location while preserving its
        /// size.
        /// </remarks>
        /// <example>
        ///     This example defines a rectangle then shifts it Northeast by fifty kilometers. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("30N,105W,1°,5°")
        /// Rectangle = Rectangle.Translate(Azimuth.Northeast, New Distance(50, DistanceUnit.Kilometers))
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("30N,105W,1°,5°");
        /// Rectangle = Rectangle.Translate(Azimuth.Northeast, New Distance(50, DistanceUnit.Kilometers));
        ///     </code>
        /// </example>
        public GeographicRectangle TranslateTo(Azimuth direction, Distance distance)
        {
            return TranslateTo(direction.DecimalDegrees, distance);
        }

        /// <summary>Moves the rectangle in the specified direction by the specified distance.</summary>
        /// <returns>
        /// A new <strong>GeographicRectangle</strong> translated by the specified direction
        /// and distance.
        /// </returns>
        /// <remarks>
        /// This method is used to shift a rectangle to a new location while preserving its
        /// size.
        /// </remarks>
        /// <example>
        ///     This example defines a rectangle then shifts it Northeast by fifty kilometers. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("30N,105W,1°,5°")
        /// Rectangle = Rectangle.Translate(new Angle(45), New Distance(50, DistanceUnit.Kilometers))
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("30N,105W,1°,5°");
        /// Rectangle = Rectangle.Translate(new Angle(45), New Distance(50, DistanceUnit.Kilometers));
        ///     </code>
        /// </example>
        public GeographicRectangle TranslateTo(Angle direction, Distance distance)
        {
            return TranslateTo(direction.DecimalDegrees, distance);
        }

        /// <summary>Moves the rectangle in the specified direction by the specified distance.</summary>
        /// <returns>
        /// A new <strong>GeographicRectangle</strong> translated by the specified direction
        /// and distance.
        /// </returns>
        /// <remarks>
        /// This method is used to shift a rectangle to a new location while preserving its
        /// size.
        /// </remarks>
        /// <example>
        ///     This example defines a rectangle then shifts it Northeast by fifty kilometers. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("30N,105W,1°,5°")
        /// Rectangle = Rectangle.Translate(45, New Distance(50, DistanceUnit.Kilometers))
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("30N,105W,1°,5°");
        /// Rectangle = Rectangle.Translate(45, New Distance(50, DistanceUnit.Kilometers));
        ///     </code>
        /// </example>
        public GeographicRectangle TranslateTo(double direction, Distance distance)
        {
            // Find the new translation point
            return new GeographicRectangle(Northwest.TranslateTo(direction, distance), Size);
        }

        /// <summary>Returns the points which form the rectangle.</summary>
        public Position[] ToArray()
        {
            return new Position[] { Northwest, Northeast, Southeast, Southwest };
        }

        /// <summary>Indicates if the rectangle does not intersect the specified rectangle.</summary>
        public bool IsDisjointedFrom(GeographicRectangle rectangle)
        {
            return !IsOverlapping(rectangle);
        }

        /// <summary>
        /// Indicates if the specified GeographicRectangle is entirely within the current GeographicRectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool IsEnclosing(GeographicRectangle rectangle)
        {
            return (rectangle.Left.DecimalDegrees >= _Left.DecimalDegrees
                && rectangle.Right.DecimalDegrees <= _Right.DecimalDegrees
                && rectangle.Top.DecimalDegrees <= _Top.DecimalDegrees
                && rectangle.Bottom.DecimalDegrees >= _Bottom.DecimalDegrees);
        }

        public bool IsEnclosing(Position position)
        {
            return ((position.Longitude.DecimalDegrees >= _Left.DecimalDegrees)
                && (position.Latitude.DecimalDegrees <= _Top.DecimalDegrees)
                && (position.Longitude.DecimalDegrees <= _Right.DecimalDegrees)
                && (position.Latitude.DecimalDegrees >= _Bottom.DecimalDegrees));
        }

        /// <summary>
        /// Moves the GeographicRectangle so that the specified position is at its center.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GeographicRectangle CenterOn(Position position)
        {
            // Get the difference from the current center to the new one
            Position Offset = position.Subtract(Center);

            // Return a new rectangle with corners shifted by this offset
            return new GeographicRectangle(
                _Left.Add(Offset.Longitude),
                _Top.Add(Offset.Latitude),
                _Right.Add(Offset.Longitude),
                _Bottom.Add(Offset.Latitude));
        }

        /// <summary>
        /// Creates a new rectangle of the specified size, centered on the specified position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static GeographicRectangle FromCenter(Position position, GeographicSize size)
        {
            return new GeographicRectangle(position, position).Inflate(size);
        }

        /// <summary>
        /// Expands the edges of the GeographicRectangle to contain the specified position.
        /// </summary>
        /// <param name="position">A <strong>Position</strong> object to surround.</param>
        /// <returns>A <strong>GeographicRectangle</strong> which contains the specified position.</returns>
        /// <remarks>If the specified position is already enclosed, the current instance will be returned.</remarks>
        public GeographicRectangle UnionWith(Position position)
        {
            // Does the box already contain the position?  If so, do nothing
            if (IsEnclosing(position)) return this;
            // Return the expanded box
            return new GeographicRectangle(
                _Top.DecimalDegrees > position.Latitude.DecimalDegrees ? _Top : position.Latitude,
                _Left.DecimalDegrees < position.Longitude.DecimalDegrees ? _Left : position.Longitude,
                _Bottom.DecimalDegrees < position.Latitude.DecimalDegrees ? _Bottom : position.Latitude,
                _Right.DecimalDegrees > position.Longitude.DecimalDegrees ? _Right : position.Longitude);
        }


        /// <summary>
        /// Increases the size of the rectangle while maintaining its center point.
        /// </summary>
        /// <returns></returns>
        public GeographicRectangle Inflate(GeographicSize size)
        {
            // Calculate half of the width and height
            Distance HalfWidth = size.Width.Multiply(0.5);
            Distance HalfHeight = size.Height.Multiply(0.5);

            // Calculate new edges
            Position NewNorthwest = Northwest.TranslateTo(Azimuth.West, HalfWidth).TranslateTo(Azimuth.North, HalfHeight);
            Position NewSoutheast = Southeast.TranslateTo(Azimuth.East, HalfWidth).TranslateTo(Azimuth.South, HalfHeight);

            // Build a new GeographicRectangle expanded outward from the center
            return new GeographicRectangle(NewNorthwest, NewSoutheast);
        }

        /// <summary>
        /// Increases the width and height of the rectangle by the specified amount.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public GeographicRectangle Inflate(Longitude width, Latitude height)
        {
            // Calculate the new values
            Longitude HalfWidthOffset = width.Multiply(0.5);
            Latitude HalfHeightOffset = height.Multiply(0.5);

            // Build a new GeographicRectangle expanded outward from the center
            return new GeographicRectangle(
                _Left.Subtract(HalfWidthOffset),
                _Top.Add(HalfHeightOffset),
                _Right.Add(HalfWidthOffset),
                _Bottom.Subtract(HalfHeightOffset));
        }

        /// <summary>
        /// Increases the width and height of the rectangle by the specified amount.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public GeographicRectangle Inflate(Latitude height, Longitude width)
        {
            // Calculate the new values
            Longitude HalfWidthOffset = width.Multiply(0.5);
            Latitude HalfHeightOffset = height.Multiply(0.5);

            // Build a new GeographicRectangle expanded outward from the center
            return new GeographicRectangle(
                _Left.Subtract(HalfWidthOffset),
                _Top.Add(HalfHeightOffset),
                _Right.Add(HalfWidthOffset),
                _Bottom.Subtract(HalfHeightOffset));
        }

        /// <summary>
        /// Calculates the rectangle created by the intersection of this and another rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public GeographicRectangle IntersectionOf(GeographicRectangle rectangle)
        {
            Longitude LeftSide = Longitude.Empty;
            Longitude RightSide = Longitude.Empty;
            Latitude TopSide = Latitude.Empty;
            Latitude BottomSide = Latitude.Empty;

            if (rectangle.Left >= _Left && rectangle.Left <= _Right)
            {
                // The rectangle overlaps the current instance on its left side

                /*    +------------+            +-------------+
                 *    |   this     |            |  this       |
                 *    |            |            |    +-----+  |
                 *    |      +------------+     |    |  R  |  |
                 *    |      |  R  |      |     |    +-----+  |
                 *    +------+-----+      |     +-------------+
                 *           |  rectangle |
                 *           +------------+
                 */

                // The left side of the parameter is the left side of the result ("R")
                LeftSide = rectangle.Left;

                // Does the right side also fall within the box?
                if (rectangle.Right >= _Left && rectangle.Right <= _Right)
                {
                    // Yes.  Use the right size from the parameter rectangle
                    RightSide = rectangle.Right;
                }
                else
                {
                    // No.  Use the right size from this instance
                    RightSide = _Right;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Top <= _Top && rectangle.Top >= _Bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    TopSide = rectangle.Top;
                }
                else
                {
                    // No. Use the top of the current instance
                    TopSide = _Top;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Bottom <= _Top && rectangle.Bottom >= _Bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    BottomSide = rectangle.Bottom;
                }
                else
                {
                    // No. Use the top of the current instance
                    BottomSide = _Bottom;
                }

                // Return the result
                return new GeographicRectangle(TopSide, LeftSide, BottomSide, RightSide);

            }
            else if (rectangle.Right >= _Left && rectangle.Right <= _Right)
            {
                // The rectangle overlaps the current instance on its right side

                /*          +------------+
                 *          |   this     |
                 *          |            |
                 *    +------------+     |
                 *    |     |   R  |     |
                 *    |     +------+-----+    
                 *    |  rectangle |
                 *    +------------+
                 */

                // Use the left side as the final overlapping point
                RightSide = rectangle.Right;


                // Does the right side also fall within the box?
                if (rectangle.Left >= _Left && rectangle.Left <= _Right)
                {
                    // Yes.  Use the right size from the parameter rectangle
                    LeftSide = rectangle.Left;
                }
                else
                {
                    // No.  Use the right size from this instance
                    LeftSide = _Left;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Top <= _Top && rectangle.Top >= _Bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    TopSide = rectangle.Top;
                }
                else
                {
                    // No. Use the top of the current instance
                    TopSide = _Top;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Bottom <= _Top && rectangle.Bottom >= _Bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    BottomSide = rectangle.Bottom;
                }
                else
                {
                    // No. Use the top of the current instance
                    BottomSide = _Bottom;
                }

                // Return the result
                return new GeographicRectangle(TopSide, LeftSide, BottomSide, RightSide);

            }
            else
            {
                // No overlapping!
                return GeographicRectangle.Empty;
            }
        }

        /// <summary>Indicates if the rectangle's border crosses or shares the border of the specified rectangle.</summary>
        public bool IsIntersectingWith(GeographicRectangle rectangle)
        {
            // For an intersection, at least one of four points of the rectangle must reside 
            // inside of the other.

            // Is the target rectangle entirely within this one?  If so, return false
            // because there's no *intersection* (only an overlap).
            if (rectangle.Left > _Left && rectangle.Right < _Right
                && rectangle.Top < _Top && rectangle.Bottom > _Bottom)
                return false;

            // Test that one or more sides are within.
            return
                ((rectangle.Left >= _Left && rectangle.Left <= _Right)
                || (rectangle.Right >= _Left && rectangle.Right <= _Right))
                &&
                ((rectangle.Top <= _Top && rectangle.Top >= _Bottom)
                || (rectangle.Bottom <= _Top && rectangle.Bottom >= _Bottom));

            //{
            //    return true;
            //}

            //{
            //    /*    +------------+
            //     *    |   this     |
            //     *    |            |
            //     *    |      |     |
            //     *    |      |  R  |      |     |    +-----+  |
            //     *    +------+-----+      |     +-------------+
            //     *           |  rectangle |
            //     *           +------------+
            //     */


            //    /*    +------------+            +-------------+
            //     *    |   this     |            |  this       |
            //     *    |            |            |    +-----+  |
            //     *    |      +------------+     |    |  R  |  |
            //     *    |      |  R  |      |     |    +-----+  |
            //     *    +------+-----+      |     +-------------+
            //     *           |  rectangle |
            //     *           +------------+
            //     */



            //    // If the bottom or right border is beyond the edge, it's intersecting
            //    if (rectangle.Bottom <= _Bottom || rectangle.Right >= _Right)
            //        return true;
            //}

            //// Does the right edge intersect?
            //if (rectangle.Right >= _Left && rectangle.Right <= _Right)
            //{
            //    // The rectangle overlaps the current instance on its right side

            //    /*          +------------+
            //     *          |   this     |
            //     *          |            |
            //     *    +------------+     |
            //     *    |     |   R  |     |
            //     *    |     +------+-----+    
            //     *    |  rectangle |
            //     *    +------------+
            //     */

            //    // If the left or bottom is beyond the edge, it's intersecting
            //    if (rectangle.Bottom <= _Bottom || rectangle.Left <= _Left)
            //        return true;
            //}

            //// Is the left edge of the rectangle between our left and right borders?
            //if (rectangle.Left >= _Left && rectangle.Left <= _Right)
            //{
            //    // Yes.  But is the right edge also with the borders?  If so, there is no
            //    // intersection.  An overlap, yes, but not an intersection.
            //    if (rectangle.Right <= _Right)
            //        return false;
            //    // The right-most edge is NOT also within the borders, so an intersection
            //    // could still be present.  How about the top border?
            //    if (rectangle.Top <= _Top && rectangle.Top >= _Bottom)
            //        return true;
            //    // The top border is not intersecting.  How about the bottom?
            //    if (rectangle.Bottom <= _Top && rectangle.Bottom >= _Bottom)
            //        return true;
            //}
            //else if (rectangle.Right >= _Left && rectangle.Right <= _Right)
            //{
            //    // Yes.  But is the right edge also with the borders?  If so, there is no
            //    // intersection.  An overlap, yes, but not an intersection.
            //    if (rectangle.Left >= _Left)
            //        return false;
            //    if (rectangle.Top <= _Top && rectangle.Top >= _Bottom)
            //        return true;
            //    else if (rectangle.Bottom <= _Top && rectangle.Bottom >= _Bottom)
            //        return true;
            //}
            ////else if (rectangle.Left <= _Left && rectangle.Right >= _Right
            ////            && rectangle.Top >= _Top && rectangle.Bottom <= _Bottom)
            ////{
            ////    return true;
            ////}
            //return false;
        }

        /// <summary>
        /// Indicates if the specified GeographicRectangle shares any of the same 2D space as the current instance.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool IsOverlapping(GeographicRectangle rectangle)
        {
            return !(rectangle.Top.DecimalDegrees < _Bottom.DecimalDegrees
                || rectangle.Bottom.DecimalDegrees > _Top.DecimalDegrees
                || rectangle.Left.DecimalDegrees > _Right.DecimalDegrees
                || rectangle.Right.DecimalDegrees < _Left.DecimalDegrees);

        }

        /// <summary>
        /// Indicates if the specified Position is within the current instance.
        /// </summary>
        /// <param name="position">A <strong>Position</strong> to test against the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the position is inside of the current rectangle.</returns>
        public bool IsOverlapping(Position position)
        {
            return !(position.Longitude.DecimalDegrees < _Left.DecimalDegrees
                || position.Longitude.DecimalDegrees > _Right.DecimalDegrees
                || position.Latitude.DecimalDegrees > _Top.DecimalDegrees
                || position.Latitude.DecimalDegrees < _Bottom.DecimalDegrees);
        }

        /// <summary>
        /// Returns the combination of the current instance with the specified rectangle.
        /// </summary>
        /// <param name="rectangle">A <strong>GeographicRectangle</strong> to merge with the current instance.</param>
        /// <returns>A <strong>GeographicRectangle</strong> enclosing both rectangles.</returns>
        /// <remarks>This method returns the smallest possible rectangle which encloses the current instance as well as the specified rectangle.</remarks>
        public GeographicRectangle UnionWith(GeographicRectangle rectangle)
        {
            return new GeographicRectangle(
                rectangle.Top.DecimalDegrees > _Top.DecimalDegrees ? rectangle.Top : _Top,
                rectangle.Left.DecimalDegrees < _Left.DecimalDegrees ? rectangle.Left : _Left,
                rectangle.Bottom.DecimalDegrees < _Bottom.DecimalDegrees ? rectangle.Bottom : _Bottom,
                rectangle.Right.DecimalDegrees > _Right.DecimalDegrees ? rectangle.Right : _Right);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is GeographicRectangle)
                return Equals((GeographicRectangle)obj);
            return false;
        }

        /// <summary>Returns a unique code of the rectangle for hash tables.</summary>
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

        /// <summary>Returns a rectangle which encloses the two specified rectangles.</summary>
        /// <returns>
        /// A <strong>GeographicRectangle</strong> as a result of merging the two
        /// rectangles.
        /// </returns>
        /// <remarks>
        /// This method is typically used to combine two individual shapes into a single
        /// shape.
        /// </remarks>
        /// <param name="first">A <strong>GeographicRectangle</strong> to merge with the second rectangle.</param>
        /// <param name="second">A <strong>GeographicRectangle</strong> to merge with the first rectangle.</param>
        public static GeographicRectangle UnionWith(GeographicRectangle first, GeographicRectangle second)
        {
            return first.UnionWith(second);
        }

        /// <summary>
        /// Returns the GeographicRectangle formed by the intersection of the two specified GeographicRectangles.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static GeographicRectangle IntersectionOf(GeographicRectangle first, GeographicRectangle second)
        {
            return first.IntersectionOf(second);
        }

        /// <summary>
        /// Returns a rectangle which encloses the specified points.
        /// </summary>
        /// <param name="positions">An array of PointD objects to enclose.</param>
        /// <returns>A <strong>RectangleD</strong> object enclosing the specified points.</returns>
        /// <remarks>This method is typically used to calculate a rectangle surrounding
        /// points which have been rotated.  For example, if a rectangle is rotated by 45°, the
        /// total width it occupies is greater than it's own width.</remarks>
        public static GeographicRectangle FromArray(Position[] positions)
        {
            if (positions == null || positions.Length == 0)
                return GeographicRectangle.Empty;

            // Calculate the new bounds
            Latitude Top = positions[0].Latitude;
            Longitude Left = positions[0].Longitude;
            Latitude Bottom = positions[0].Latitude;
            Longitude Right = positions[0].Longitude;

            // Loop through all items in the collection
            int limit = positions.Length;
            for (int index = 1; index < limit; index++)
            {
                Position item = positions[index];

                if (item.Longitude < Left)
                    Left = item.Longitude;
                else if (item.Longitude > Right)
                    Right = item.Longitude;
                if (item.Latitude > Top)
                    Top = item.Latitude;
                else if (item.Latitude < Bottom)
                    Bottom = item.Latitude;
            }

            // Build a new rectangle
            return new GeographicRectangle(Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Parses a string into a GeographicRectangle object.
        /// </summary>
        /// <param name="value">A <string>String</string> specifying geographic coordinates defining a rectangle.</param>
        /// <returns>A <strong>GeographicRectangle</strong> object using the specified coordinates.</returns>
        /// <remarks>This powerful method will convert points defining a rectangle in the form of a string into
        /// a GeographicRectangle object.  The string can be </remarks>
        public static GeographicRectangle Parse(string value)
        {
            return new GeographicRectangle(value, CultureInfo.CurrentCulture);
        }

        public static GeographicRectangle Parse(string value, CultureInfo culture)
        {
            return new GeographicRectangle(value, culture);
        }

        #endregion

        #region Conversions

        public static explicit operator GeographicRectangle(string value)
        {
            return new GeographicRectangle(value, CultureInfo.CurrentCulture);
        }

        public static explicit operator string(GeographicRectangle value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region IEquatable<GeographicRectangle>

        public bool Equals(GeographicRectangle other)
        {
            // The objects are equivalent if their bounds are equivalent
            return _Left.DecimalDegrees.Equals(other.Left.DecimalDegrees)
                && _Right.DecimalDegrees.Equals(other.Right.DecimalDegrees)
                && _Top.DecimalDegrees.Equals(other.Top.DecimalDegrees)
                && _Bottom.DecimalDegrees.Equals(other.Bottom.DecimalDegrees);
        }

        #endregion

        #region IFormattable Members


        public string ToString(string format, IFormatProvider formatProvider)
		{
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            return _Top.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " " + _Left.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " " + _Bottom.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " " + _Right.ToString(format, culture);
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            /* This class uses the GML 3.0 object "gml:envelope" to serialize
             * its information.  The format is as follows:
             * 
             * <gml:envelope>
             *      <lowerCorner><gml:pos>X Y</gml:pos></lowerCorner>
             *      <upperCorner><gml:pos>X Y</gml:pos></lowerCorner>
             * </gml:envelope>
             * 
             * Envelope defines an extent using a pair of positions defining opposite corners in arbitrary dimensions. The
             * first direct position is the "lower corner" (a coordinate position consisting of all the minimal ordinates for each
             * dimension for all points within the envelope), the second one the "upper corner" (a coordinate position
             * consisting of all the maximal ordinates for each dimension for all points within the envelope).
             * 
             */

            // <gml:envelope>
            writer.WriteStartElement(Xml.GmlXmlPrefix, "envelope", Xml.GmlXmlNamespace);
            // <lowerCorner>
            writer.WriteStartElement(Xml.GmlXmlPrefix, "lowerCorner", Xml.GmlXmlNamespace);
            // <gml:pos>X Y</gml:pos>
            Southwest.WriteXml(writer);
            // </lowerCorner>
            writer.WriteEndElement();
            // <upperCorner>
            writer.WriteStartElement(Xml.GmlXmlPrefix, "upperCorner", Xml.GmlXmlNamespace);
            // <gml:pos>X Y</gml:pos>
            Northeast.WriteXml(writer);
            // </upperCorner>
            writer.WriteEndElement();
            // </gml:envelope>
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            /* This class uses the GML 3.0 object "gml:envelope" to serialize
              * its information.  The format is as follows:
              * 
              * <gml:envelope>
              *      <lowerCorner><gml:pos>X Y</gml:pos></lowerCorner>
              *      <upperCorner><gml:pos>X Y</gml:pos></lowerCorner>
              * </gml:envelope>
              * 
              * ... but we should also de-serialize deprecated or nested values, such as:
              * 
              * <gml:boundedBy><gml:Envelope /></gml:boundedBy>
              * <gml:box><gml:x /><gml:y /></gml:box>
              * 
              * Envelope defines an extent using a pair of positions defining opposite corners in arbitrary dimensions. The
              * first direct position is the "lower corner" (a coordinate position consisting of all the minimal ordinates for each
              * dimension for all points within the envelope), the second one the "upper corner" (a coordinate position
              * consisting of all the maximal ordinates for each dimension for all points within the envelope).
              * 
              */

            _Top = Latitude.Empty;
            _Bottom = Latitude.Empty;
            _Left = Longitude.Empty;
            _Right = Longitude.Empty;
            _Center = Position.Invalid;

            // Move to the <gml:Envelope>, <gml:boundedBy>, or <gml:Box> element
            if (!reader.IsStartElement("envelope", Xml.GmlXmlNamespace)
                && !reader.IsStartElement("Envelope", Xml.GmlXmlNamespace)
                && !reader.IsStartElement("boundedBy", Xml.GmlXmlNamespace)
                && !reader.IsStartElement("Box", Xml.GmlXmlNamespace))
                reader.ReadStartElement();

            switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
            {
                case "envelope":

                    #region <gml:Envelope>

                    Position southWest = Position.Invalid;
                    Position northEast = Position.Invalid;

                    // Read the element, <gml:envelope>
                    reader.ReadStartElement();

                    // Read the next two elements, either "lowerCorner" or "upperCorner"
                    for (int index = 0; index < 2; index++)
                    {
                        switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "pos":
                                // There is probably one or two <gml:pos> objects
                                if (southWest.IsInvalid)
                                    southWest = new Position(reader);
                                else
                                    northEast = new Position(reader);
                                break;
                            case "lowercorner":
                                // Read the start element, <lowerCorner>
                                reader.ReadStartElement();

                                // Read the position
                                southWest = new Position(reader);

                                // Read the end element </lowerCorner>
                                reader.ReadEndElement();
                                break;
                            case "uppercorner":
                                // Read the start element, <lowerCorner>
                                reader.ReadStartElement();

                                // Read the position
                                northEast = new Position(reader);

                                // Read the end element </lowerCorner>
                                reader.ReadEndElement();
                                break;
                            default:
                                // Skip this unknown element
                                break;
                        }
                    }

                    // Calculate bounds
                    _Left = southWest.Longitude < northEast.Longitude ? southWest.Longitude : northEast.Longitude;
                    _Right = northEast.Longitude > southWest.Longitude ? northEast.Longitude : southWest.Longitude;
                    _Top = northEast.Latitude > southWest.Latitude ? northEast.Latitude : southWest.Latitude;
                    _Bottom = southWest.Latitude < northEast.Latitude ? southWest.Latitude : northEast.Latitude;

                    // Lastly, calculate the center
                    _Center = Position.Invalid;
                    _Center = Hypotenuse.Midpoint;

                    // Read the end element, </gml:envelope>
                    reader.ReadEndElement();

                    #endregion

                    return;
                case "boundedby":

                    #region <gml:boundedBy>

                    // Read the start element, <gml:boundedBy>
                    reader.ReadStartElement();

                    // Make a recursive call into here to read <gml:envelope>
                    GeographicRectangle value = new GeographicRectangle(reader);

                    // Copy the values
                    _Left = value.Left;
                    _Top = value.Top;
                    _Right = value.Right;
                    _Bottom = value.Bottom;
                    _Center = value.Center;

                    // Read the end element, </gml:boundedBy>
                    reader.ReadEndElement();

                    #endregion

                    return;
                case "box":

                    #region <gml:Box>

                    // Read the start element, <gml:boundedBy>
                    reader.ReadStartElement();

                    // Read the coordinates
                    Position Corner1 = new Position(reader);
                    Position Corner2 = new Position(reader);

                    // Set the values
                    if (Corner1.Latitude < Corner2.Latitude)
                    {
                        _Bottom = Corner1.Latitude;
                        _Top = Corner2.Latitude;
                    }
                    else
                    {
                        _Bottom = Corner2.Latitude;
                        _Top = Corner1.Latitude;
                    }

                    if (Corner1.Longitude < Corner2.Longitude)
                    {
                        _Left = Corner1.Longitude;
                        _Right = Corner2.Longitude;
                    }
                    else
                    {
                        _Left = Corner2.Longitude;
                        _Right = Corner1.Longitude;
                    }

                    // Lastly, calculate the center
                    _Center = Position.Invalid;
                    _Center = Hypotenuse.Midpoint;

                    // Read the end element, </gml:boundedBy>
                    reader.ReadEndElement();

                    #endregion

                    return;
            }
        }

        #endregion
    }
}
