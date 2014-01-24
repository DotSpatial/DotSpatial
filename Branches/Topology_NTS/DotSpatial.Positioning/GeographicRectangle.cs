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
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
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
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a rectangular shape on Earth's surface.
    /// </summary>
    /// <remarks><para>This class is used to represent a square (or close to a square) shape on
    /// Earth's surface. This class is typically used during mapping applications to zoom
    /// into a particular area on Earth. This class looks nearly identical to the Rectangle
    /// class in the .NET framework, except that it's bounding points are defined as
    ///   <strong>Position</strong> objects instead of <strong>Point</strong> objects.</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (it's properties can only be set via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.GeographicRectangleConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct GeographicRectangle : IFormattable, IEquatable<GeographicRectangle>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private Latitude _top;
        /// <summary>
        ///
        /// </summary>
        private Latitude _bottom;
        /// <summary>
        ///
        /// </summary>
        private Longitude _left;
        /// <summary>
        ///
        /// </summary>
        private Longitude _right;
        /// <summary>
        ///
        /// </summary>
        private Position _center;

        #region Fields

        /// <summary>
        /// Represents a GeographicRectangle having no size.
        /// </summary>
        public static readonly GeographicRectangle Empty = new GeographicRectangle(Latitude.Empty, Longitude.Empty, Latitude.Empty, Longitude.Empty);
        /// <summary>
        /// Represents a rectangle that encompasses all of Earth's surface.
        /// </summary>
        public static readonly GeographicRectangle Maximum = new GeographicRectangle(Latitude.Maximum, Longitude.Minimum, Latitude.Minimum, Longitude.Maximum);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified location and size.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// A <strong>GeographicRectangle</strong> set to the specified location and
        /// size.
        ///   </returns>
        ///
        /// <example>
        /// This example creates a new <strong>GeographicRectangle</strong> starting at 39°N
        /// 105°W which is 2° wide and 5° tall.
        ///   <code lang="VB" title="[New Example]">
        /// Dim NorthwestCorner As New Position("39N 105W")
        /// Dim RectangleSize As New GeographicSize(2, 5)
        /// Dim Rectangle As New GeographicRectangle(NorthwestCorner, RectangleSize)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Position NorthwestCorner = new Position("39N, 105W");
        /// GeographicSize RectangleSize = new GeographicSize(2, 5);
        /// GeographicRectangle Rectangle = new GeographicRectangle(NorthwestCorner, RectangleSize);
        ///   </code>
        ///   </example>
        /// <remarks>This constructor defines a rectangle which expands east and south of the
        /// specified location.</remarks>
        public GeographicRectangle(Position location, GeographicSize size)
        {
            _left = location.Longitude;
            _right = location.TranslateTo(Azimuth.East, size.Width).Longitude;
            _top = location.Latitude;
            _bottom = location.TranslateTo(Azimuth.South, size.Height).Latitude;
            _center = Position.Invalid;
            _center = Hypotenuse.Midpoint;
        }

        /// <summary>
        /// Creates a new instance using the specified location, width, and height.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <example>
        /// This example creates a new <strong>GeographicRectangle</strong> starting at 39°N
        /// 105°W which is 2° wide and 5° tall.
        ///   <code lang="VB" title="[New Example]">
        /// Dim NorthwestCorner As New Position("39N 105W")
        /// Dim RectangleWidth As Distance = Distance.FromKilometers(1)
        /// Dim RectangleHeight As Distance = Distance.FromKilometers(1)
        /// Dim Rectangle As New GeographicRectangle(NorthwestCorner, RectangleWidth, RectangleHeight)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Position NorthwestCorner = new Position("39N 105W");
        /// Distance RectangleWidth = Distance.FromKilometers(1);
        /// Distance RectangleHeight = Distance.FromKilometers(1);
        /// GeographicRectangle Rectangle = new GeographicRectangle(NorthwestCorner, RectangleWidth, RectangleHeight);
        ///   </code>
        ///   </example>
        ///
        /// <returns>
        /// A <strong>GeographicRectangle</strong> set to the specified location and
        /// size.
        ///   </returns>
        /// <remarks>This constructor defines a rectangle which expands east and south of the
        /// specified location.</remarks>
        public GeographicRectangle(Position location, Distance width, Distance height)
        {
            _left = location.Longitude;
            _right = location.TranslateTo(Azimuth.East, width).Longitude;
            _top = location.Latitude;
            _bottom = location.TranslateTo(Azimuth.South, height).Latitude;

            _center = Position.Invalid;
            _center = Hypotenuse.Midpoint;
        }

        /// <summary>
        /// Creates a new instance using the specified northwest and southeast
        /// coordinates.
        /// </summary>
        /// <param name="northwest">The northwest.</param>
        /// <param name="southeast">The southeast.</param>
        /// <returns>A <strong>GeographicRectangle</strong> defined by the two endpoints.</returns>
        ///
        /// <example>
        /// This example creates a new <strong>GeographicRectangle</strong> starting at 39°N
        /// 105°W and ending at 37°N 100°W (2° wide and 5° tall).
        ///   <code lang="VB" title="[New Example]">
        /// Dim NorthwestCorner As New Position("39N 105W")
        /// Dim SoutheastCorner As New Position("37N 100W")
        /// Dim Rectangle As New GeographicRectangle(NorthwestCorner, SoutheastCorner)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Position NorthwestCorner = new Position("39N 105W");
        /// Position SoutheastCorner = new Position("37N 100W");
        /// GeographicRectangle Rectangle = new GeographicRectangle(NorthwestCorner, SoutheastCorner);
        ///   </code>
        ///   </example>
        /// <remarks>This constructor takes the specified parameters and calculates the width and
        /// height of the rectangle. If the two points are backwards (meaning that the right-most
        /// point is west of the left-most point), they are automatically swapped before creating
        /// the rectangle.</remarks>
        public GeographicRectangle(Position northwest, Position southeast)
        {
            _left = northwest.Longitude;
            _right = southeast.Longitude;
            _top = northwest.Latitude;
            _bottom = southeast.Latitude;

            _center = Position.Invalid;
            _center = Hypotenuse.Midpoint;
        }

        /// <summary>
        /// Creates a new instance by converting the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <strong>GeographicRectangle</strong> matching the specified string
        /// value.
        ///   </returns>
        ///
        /// <example>
        /// This example creates a new rectangle at 39°N, 105° extending two degrees south and
        /// five degrees east to 37°N, 100°W.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("39N, 105W, 37N, 100W")
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("39N, 105W, 37N, 100W");
        ///   </code>
        ///   </example>
        /// <remarks>This constructor attempts to parse the specified string into a rectangle. The
        /// current culture is used to interpret the string -- use the list separator of the
        /// current culture (which may not necessarily be a comma). This constructor can accept any
        /// output created via the <strong>ToString</strong> method.</remarks>
        public GeographicRectangle(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by converting the specified string in the given
        /// culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// This constructor attempts to parse the specified string into a rectangle. The
        /// specified culture is used to interpret the string. This constructor can accept any
        /// output created via the <strong>ToString</strong> method.
        ///   </returns>
        ///
        /// <example>
        /// This example creates a new rectangle at 39°N, 105° extending two degrees south and
        /// five degrees east to 37°N, 100°W.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("39N, 105W, 37N, 100W", CultureInfo.CurrentCulture)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("39N, 105W, 37N, 100W", CultureInfo.CurrentCulture);
        ///   </code>
        ///   </example>
        public GeographicRectangle(string value, CultureInfo culture)
        {
            // Default to zero if value is null
            if (string.IsNullOrEmpty(value))
            {
                _top = Latitude.Empty;
                _bottom = Latitude.Empty;
                _left = Longitude.Empty;
                _right = Longitude.Empty;

                _center = Position.Invalid;
                _center = Hypotenuse.Midpoint;
                return;
            }

            // Default to the current culture
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            // Split the string into words
            string[] values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());

            // How many words are there?
            switch (values.Length)
            {
                case 4:
                    // Extract each item
                    bool isTopHandled = false;
                    bool isLeftHandled = false;
                    bool isRightHandled = false;
                    bool isBottomHandled = false;

                    Latitude tempTop = Latitude.Empty;
                    Longitude tempLeft = Longitude.Empty;
                    Latitude tempBottom = Latitude.Empty;
                    Longitude tempRight = Longitude.Empty;

                    foreach (string word in values)
                    {
#if Framework20
                        if (word.IndexOf("W", StringComparison.InvariantCultureIgnoreCase) != -1
                            || word.IndexOf("E", StringComparison.InvariantCultureIgnoreCase) != -1)
#else
							if (word.IndexOf("W") != -1
								|| word.IndexOf("E") != -1)
#endif
                        {
                            if (isLeftHandled && isRightHandled)
                                throw new FormatException("A GeographicRectangle object could not be converted from a string because more than two longitude values were encountered.  Only two are allowed.");

                            // Longitude.  Is the left handled?
                            if (isLeftHandled)
                            {
                                // This is the right side
                                tempRight = Longitude.Parse(word, culture);
                                isRightHandled = true;
                            }
                            else
                            {
                                // This is the left side
                                tempLeft = Longitude.Parse(word, culture);
                                isLeftHandled = true;
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
                            if (isTopHandled && isBottomHandled)
                                throw new FormatException("A GeographicRectangle object could not be converted from a string because more than two latitude values were encountered.  Only two are allowed.");

                            // Longitude.  Is the left handled?
                            if (isTopHandled)
                            {
                                // This is the bottom side
                                tempBottom = Latitude.Parse(word, culture);
                                isBottomHandled = true;
                            }
                            else
                            {
                                // This is the top side
                                tempTop = Latitude.Parse(word, culture);
                                isTopHandled = true;
                            }
                        }
                        else
                        {
                            throw new FormatException("A GeographicRectangle object could not be created because a number could not be categorized as a latitude or longitude.  Add a N, S, E, or W letter to resolve the issue.");
                        }
                    }

                    // Flip any coordinates which are backwards
                    _top = tempTop.DecimalDegrees > tempBottom.DecimalDegrees ? tempTop : tempBottom;
                    _left = tempLeft.DecimalDegrees < tempRight.DecimalDegrees ? tempLeft : tempRight;
                    _bottom = tempBottom.DecimalDegrees < tempTop.DecimalDegrees ? tempBottom : tempTop;
                    _right = tempRight.DecimalDegrees > tempLeft.DecimalDegrees ? tempRight : tempLeft;

                    _center = Position.Invalid;
                    _center = Hypotenuse.Midpoint;
                    break;
                default:
                    throw new FormatException("The specified value could not be parsed into a GeographicRectangle object because four delimited values are required (Top, Left, Bottom, Right).");
            }
        }

        /// <summary>
        /// Creates a new instance using the specified latitudes and longitudes.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <example>
        /// This example creates a new <strong>GeographicRectangle</strong> by specifying each
        /// side individually.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Left As New Longitude(-105)
        /// Dim Top As New Latitude(39)
        /// Dim Right As New Longitude(-100)
        /// Dim Top As New Latitude(37)
        /// Dim Rectangle As New GeographicRectangle(Left, Top, Right, Bottom)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Longitude Left = new Longitude(-105);
        /// Latitude Top = new Latitude(39);
        /// Longitude Right = new Longitude(-100);
        /// Latitude Top = new Latitude(37);
        /// GeographicRectangle Rectangle = new GeographicRectangle(Left, Top, Right, Bottom);
        ///   </code>
        ///   </example>
        ///
        /// <returns>A <strong>GeographicRectangle</strong> bound by the specified values.</returns>
        /// <remarks>If the left and right, or top and bottom values are backwards, they are
        /// automatically swapped before creating the rectangle.</remarks>
        public GeographicRectangle(Longitude left, Latitude top, Longitude right, Latitude bottom)
            : this(top, left, bottom, right)
        { }

        /// <summary>
        /// Creates a new instance using the specified latitudes and longitudes.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="left">The left.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="right">The right.</param>
        /// <returns>A <strong>GeographicRectangle</strong> bound by the specified values.</returns>
        ///
        /// <example>
        ///   <code lang="VB" title="[New Example]">
        /// Dim Left As New Longitude(-105)
        /// Dim Top As New Latitude(39)
        /// Dim Right As New Longitude(-100)
        /// Dim Top As New Latitude(37)
        /// Dim Rectangle As New GeographicRectangle(Left, Top, Right, Bottom)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Latitude Top = new Latitude(39);
        /// Longitude Left = new Longitude(-105);
        /// Latitude Bottom = new Latitude(37);
        /// Longitude Right = new Longitude(-100);
        /// GeographicRectangle Rectangle = new GeographicRectangle(Top, Left, Bottom, Right);
        ///   </code>
        ///   </example>
        /// <remarks>If the left and right, or top and bottom values are backwards, they are
        /// automatically swapped before creating the rectangle.</remarks>
        public GeographicRectangle(Latitude top, Longitude left, Latitude bottom, Longitude right)
        {
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;

            _center = Position.Invalid;
            _center = Hypotenuse.Midpoint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicRectangle"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public GeographicRectangle(double left, double top, double right, double bottom)
        {
            _left = new Longitude(left);
            _right = new Longitude(right);
            _top = new Latitude(top);
            _bottom = new Latitude(bottom);

            _center = Position.Invalid;
            _center = Hypotenuse.Midpoint;
        }

        /// <summary>
        /// Creates a new instance from a block of Geography Markup Language (GML).
        /// </summary>
        /// <param name="reader">The reader.</param>
        public GeographicRectangle(XmlReader reader)
        {
            // Initialize all fields
            _top = Latitude.Invalid;
            _bottom = Latitude.Invalid;
            _left = Longitude.Invalid;
            _right = Longitude.Invalid;
            _center = Position.Invalid;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the southern-most side of the rectangle.
        /// </summary>
        /// <value>A <see cref="Latitude"></see> object marking the southern-most latitude.</value>
        public Latitude Top
        {
            get
            {
                return _top;
            }
        }

        /// <summary>
        /// Returns the southern-most latitude of the rectangle.
        /// </summary>
        /// <value>A <see cref="Latitude"></see> object marking the southern-most latitude.</value>
        public Latitude Bottom
        {
            get
            {
                return _bottom;
            }
        }

        /// <summary>
        /// Returns the western-most side of the rectangle.
        /// </summary>
        /// <value>A <strong>Longitude</strong> indicating the left side of the rectangle.</value>
        public Longitude Left
        {
            get
            {
                return _left;
            }
        }

        /// <summary>
        /// Returns the eastern-most side of the rectangle.
        /// </summary>
        /// <value>A <strong>Longitude</strong> indicating the right side of the rectangle.</value>
        public Longitude Right
        {
            get
            {
                return _right;
            }
        }

        /// <summary>
        /// Returns the geographic center of the rectangle.
        /// </summary>
        public Position Center
        {
            get
            {
                return _center;
            }
        }

        /// <summary>
        /// Returns the aspect ratio of the rectangle.
        /// </summary>
        /// <remarks>This property returns the ratio of the GeographicRectangles width to its height (width / height).  This
        /// property gives an indication of the GeographicRectangle's shape.  An aspect ratio of one indicates
        /// a square, whereas an aspect ratio of two indicates a GeographicRectangle which is twice as wide as
        /// it is high.</remarks>
        public float AspectRatio
        {
            get
            {
                return Convert.ToSingle(WidthDegrees.DecimalDegrees / HeightDegrees.DecimalDegrees);
            }
        }

        /// <summary>
        /// Indicates if the rectangle has any value.
        /// </summary>
        /// <value>A <strong>Boolean</strong>, <strong>True</strong> if a metor the size of Rhode
        /// Island is about to crash into the Pacific Ocean just off the coast of Nicaragua but
        /// there will be no casualties because everyone was warned plenty of time in
        /// advance.</value>
        public bool IsEmpty
        {
            get
            {
                return _top.IsEmpty && _bottom.IsEmpty && _left.IsEmpty && _right.IsEmpty;
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

        /// <summary>
        /// Gets the height degrees.
        /// </summary>
        public Latitude HeightDegrees
        {
            get
            {
                return _top.Subtract(_bottom);
            }
        }

        /// <summary>
        /// Gets the width degrees.
        /// </summary>
        public Longitude WidthDegrees
        {
            get
            {
                return _right.Subtract(_left);
            }
        }

        /// <summary>
        /// Returns the width and height of the rectangle.
        /// </summary>
        public GeographicSize Size
        {
            get
            {
                return new GeographicSize(Width, Height);
            }
        }

        /// <summary>
        /// Returns the northwestern corner of the rectangle.
        /// </summary>
        public Position Northwest
        {
            get
            {
                return new Position(_left, _top);
            }
        }

        /// <summary>
        /// Returns a point on the northern side, centered horizontally.
        /// </summary>
        public Position NorthCenter
        {
            get
            {
                return new Position(_center.Longitude, _top);
            }
        }

        /// <summary>
        /// Returns a point on the southern side, centered horizontally.
        /// </summary>
        public Position SouthCenter
        {
            get
            {
                return new Position(_center.Longitude, _bottom);
            }
        }

        /// <summary>
        /// Returns a point on the western side, centered vertically.
        /// </summary>
        public Position WestCenter
        {
            get
            {
                return new Position(_left, Center.Latitude);
            }
        }

        /// <summary>
        /// Returns a point on the eastern side, centered vertically.
        /// </summary>
        public Position EastCenter
        {
            get
            {
                return new Position(_right, Center.Latitude);
            }
        }

        /// <summary>
        /// Returns the northeastern corner of the rectangle.
        /// </summary>
        public Position Northeast
        {
            get
            {
                return new Position(_right, _top);
            }
        }

        /// <summary>
        /// Returns the southwestern corner of the rectangle.
        /// </summary>
        public Position Southwest
        {
            get
            {
                return new Position(_left, _bottom);
            }
        }

        /// <summary>
        /// Returns the southeastern corner of the rectangle.
        /// </summary>
        public Position Southeast
        {
            get
            {
                return new Position(_right, _bottom);
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Moves the rectangle in the specified direction by the specified distance.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>A new <strong>GeographicRectangle</strong> translated by the specified direction
        /// and distance.</returns>
        /// <example>
        /// This example defines a rectangle then shifts it Northeast by fifty kilometers.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("30N, 105W, 1°, 5°")
        /// Rectangle = Rectangle.Translate(Azimuth.Northeast, New Distance(50, DistanceUnit.Kilometers))
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("30N, 105W, 1°, 5°");
        /// Rectangle = Rectangle.Translate(Azimuth.Northeast, New Distance(50, DistanceUnit.Kilometers));
        ///   </code>
        ///   </example>
        /// <remarks>This method is used to shift a rectangle to a new location while preserving its
        /// size.</remarks>
        public GeographicRectangle TranslateTo(Azimuth direction, Distance distance)
        {
            return TranslateTo(direction.DecimalDegrees, distance);
        }

        /// <summary>
        /// Moves the rectangle in the specified direction by the specified distance.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>A new <strong>GeographicRectangle</strong> translated by the specified direction
        /// and distance.</returns>
        /// <example>
        /// This example defines a rectangle then shifts it Northeast by fifty kilometers.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("30N, 105W, 1°, 5°")
        /// Rectangle = Rectangle.Translate(new Angle(45), New Distance(50, DistanceUnit.Kilometers))
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("30N, 105W, 1°, 5°");
        /// Rectangle = Rectangle.Translate(new Angle(45), New Distance(50, DistanceUnit.Kilometers));
        ///   </code>
        ///   </example>
        /// <remarks>This method is used to shift a rectangle to a new location while preserving its
        /// size.</remarks>
        public GeographicRectangle TranslateTo(Angle direction, Distance distance)
        {
            return TranslateTo(direction.DecimalDegrees, distance);
        }

        /// <summary>
        /// Moves the rectangle in the specified direction by the specified distance.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>A new <strong>GeographicRectangle</strong> translated by the specified direction
        /// and distance.</returns>
        /// <example>
        /// This example defines a rectangle then shifts it Northeast by fifty kilometers.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Rectangle As New GeographicRectangle("30N, 105W, 1°, 5°")
        /// Rectangle = Rectangle.Translate(45, New Distance(50, DistanceUnit.Kilometers))
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// GeographicRectangle Rectangle = new GeographicRectangle("30N, 105W, 1°, 5°");
        /// Rectangle = Rectangle.Translate(45, New Distance(50, DistanceUnit.Kilometers));
        ///   </code>
        ///   </example>
        /// <remarks>This method is used to shift a rectangle to a new location while preserving its
        /// size.</remarks>
        public GeographicRectangle TranslateTo(double direction, Distance distance)
        {
            // Find the new translation point
            return new GeographicRectangle(Northwest.TranslateTo(direction, distance), Size);
        }

        /// <summary>
        /// Returns the points which form the rectangle.
        /// </summary>
        /// <returns></returns>
        public Position[] ToArray()
        {
            return new[] { Northwest, Northeast, Southeast, Southwest };
        }

        /// <summary>
        /// Indicates if the rectangle does not intersect the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns><c>true</c> if [is disjointed from] [the specified rectangle]; otherwise, <c>false</c>.</returns>
        public bool IsDisjointedFrom(GeographicRectangle rectangle)
        {
            return !IsOverlapping(rectangle);
        }

        /// <summary>
        /// Indicates if the specified GeographicRectangle is entirely within the current GeographicRectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns><c>true</c> if the specified rectangle is enclosing; otherwise, <c>false</c>.</returns>
        public bool IsEnclosing(GeographicRectangle rectangle)
        {
            return (rectangle.Left.DecimalDegrees >= _left.DecimalDegrees
                && rectangle.Right.DecimalDegrees <= _right.DecimalDegrees
                && rectangle.Top.DecimalDegrees <= _top.DecimalDegrees
                && rectangle.Bottom.DecimalDegrees >= _bottom.DecimalDegrees);
        }

        /// <summary>
        /// Determines whether the specified position is enclosing.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns><c>true</c> if the specified position is enclosing; otherwise, <c>false</c>.</returns>
        public bool IsEnclosing(Position position)
        {
            return ((position.Longitude.DecimalDegrees >= _left.DecimalDegrees)
                && (position.Latitude.DecimalDegrees <= _top.DecimalDegrees)
                && (position.Longitude.DecimalDegrees <= _right.DecimalDegrees)
                && (position.Latitude.DecimalDegrees >= _bottom.DecimalDegrees));
        }

        /// <summary>
        /// Moves the GeographicRectangle so that the specified position is at its center.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public GeographicRectangle CenterOn(Position position)
        {
            // Get the difference from the current center to the new one
            Position offset = position.Subtract(Center);

            // Return a new rectangle with corners shifted by this offset
            return new GeographicRectangle(
                _left.Add(offset.Longitude),
                _top.Add(offset.Latitude),
                _right.Add(offset.Longitude),
                _bottom.Add(offset.Latitude));
        }

        /// <summary>
        /// Creates a new rectangle of the specified size, centered on the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
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
                _top.DecimalDegrees > position.Latitude.DecimalDegrees ? _top : position.Latitude,
                _left.DecimalDegrees < position.Longitude.DecimalDegrees ? _left : position.Longitude,
                _bottom.DecimalDegrees < position.Latitude.DecimalDegrees ? _bottom : position.Latitude,
                _right.DecimalDegrees > position.Longitude.DecimalDegrees ? _right : position.Longitude);
        }

        /// <summary>
        /// Increases the size of the rectangle while maintaining its center point.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public GeographicRectangle Inflate(GeographicSize size)
        {
            // Calculate half of the width and height
            Distance halfWidth = size.Width.Multiply(0.5);
            Distance halfHeight = size.Height.Multiply(0.5);

            // Calculate new edges
            Position newNorthwest = Northwest.TranslateTo(Azimuth.West, halfWidth).TranslateTo(Azimuth.North, halfHeight);
            Position newSoutheast = Southeast.TranslateTo(Azimuth.East, halfWidth).TranslateTo(Azimuth.South, halfHeight);

            // Build a new GeographicRectangle expanded outward from the center
            return new GeographicRectangle(newNorthwest, newSoutheast);
        }

        /// <summary>
        /// Increases the width and height of the rectangle by the specified amount.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public GeographicRectangle Inflate(Longitude width, Latitude height)
        {
            // Calculate the new values
            Longitude halfWidthOffset = width.Multiply(0.5);
            Latitude halfHeightOffset = height.Multiply(0.5);

            // Build a new GeographicRectangle expanded outward from the center
            return new GeographicRectangle(
                _left.Subtract(halfWidthOffset),
                _top.Add(halfHeightOffset),
                _right.Add(halfWidthOffset),
                _bottom.Subtract(halfHeightOffset));
        }

        /// <summary>
        /// Increases the width and height of the rectangle by the specified amount.
        /// </summary>
        /// <param name="height">The latitudinal height.</param>
        /// <param name="width">The longitudinal width.</param>
        /// <returns>A geographic size rectangle for the height and width specified.</returns>
        public GeographicRectangle Inflate(Latitude height, Longitude width)
        {
            // Calculate the new values
            Longitude halfWidthOffset = width.Multiply(0.5);
            Latitude halfHeightOffset = height.Multiply(0.5);

            // Build a new GeographicRectangle expanded outward from the center
            return new GeographicRectangle(
                _left.Subtract(halfWidthOffset),
                _top.Add(halfHeightOffset),
                _right.Add(halfWidthOffset),
                _bottom.Subtract(halfHeightOffset));
        }

        /// <summary>
        /// Calculates the rectangle created by the intersection of this and another rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns></returns>
        public GeographicRectangle IntersectionOf(GeographicRectangle rectangle)
        {
            Longitude leftSide;
            Longitude rightSide;
            Latitude topSide;
            Latitude bottomSide;

            if (rectangle.Left >= _left && rectangle.Left <= _right)
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
                leftSide = rectangle.Left;

                // Does the right side also fall within the box?
                if (rectangle.Right >= _left && rectangle.Right <= _right)
                {
                    // Yes.  Use the right size from the parameter rectangle
                    rightSide = rectangle.Right;
                }
                else
                {
                    // No.  Use the right size from this instance
                    rightSide = _right;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Top <= _top && rectangle.Top >= _bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    topSide = rectangle.Top;
                }
                else
                {
                    // No. Use the top of the current instance
                    topSide = _top;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Bottom <= _top && rectangle.Bottom >= _bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    bottomSide = rectangle.Bottom;
                }
                else
                {
                    // No. Use the top of the current instance
                    bottomSide = _bottom;
                }

                // Return the result
                return new GeographicRectangle(topSide, leftSide, bottomSide, rightSide);
            }
            if (rectangle.Right >= _left && rectangle.Right <= _right)
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
                rightSide = rectangle.Right;

                // Does the right side also fall within the box?
                if (rectangle.Left >= _left && rectangle.Left <= _right)
                {
                    // Yes.  Use the right size from the parameter rectangle
                    leftSide = rectangle.Left;
                }
                else
                {
                    // No.  Use the right size from this instance
                    leftSide = _left;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Top <= _top && rectangle.Top >= _bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    topSide = rectangle.Top;
                }
                else
                {
                    // No. Use the top of the current instance
                    topSide = _top;
                }

                // Does the top side of the box fall within the current instance?
                if (rectangle.Bottom <= _top && rectangle.Bottom >= _bottom)
                {
                    // Yes.  Use the top of the parameter rectangle
                    bottomSide = rectangle.Bottom;
                }
                else
                {
                    // No. Use the top of the current instance
                    bottomSide = _bottom;
                }

                // Return the result
                return new GeographicRectangle(topSide, leftSide, bottomSide, rightSide);
            }
            // No overlapping!
            return Empty;
        }

        /// <summary>
        /// Indicates if the rectangle's border crosses or shares the border of the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns><c>true</c> if [is intersecting with] [the specified rectangle]; otherwise, <c>false</c>.</returns>
        public bool IsIntersectingWith(GeographicRectangle rectangle)
        {
            // For an intersection, at least one of four points of the rectangle must reside
            // inside of the other.

            // Is the target rectangle entirely within this one?  If so, return false
            // because there's no *intersection* (only an overlap).
            if (rectangle.Left > _left && rectangle.Right < _right
                && rectangle.Top < _top && rectangle.Bottom > _bottom)
                return false;

            // Test that one or more sides are within.
            return
                ((rectangle.Left >= _left && rectangle.Left <= _right)
                || (rectangle.Right >= _left && rectangle.Right <= _right))
                &&
                ((rectangle.Top <= _top && rectangle.Top >= _bottom)
                || (rectangle.Bottom <= _top && rectangle.Bottom >= _bottom));

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
        /// <param name="rectangle">The rectangle.</param>
        /// <returns><c>true</c> if the specified rectangle is overlapping; otherwise, <c>false</c>.</returns>
        public bool IsOverlapping(GeographicRectangle rectangle)
        {
            return !(rectangle.Top.DecimalDegrees < _bottom.DecimalDegrees
                || rectangle.Bottom.DecimalDegrees > _top.DecimalDegrees
                || rectangle.Left.DecimalDegrees > _right.DecimalDegrees
                || rectangle.Right.DecimalDegrees < _left.DecimalDegrees);
        }

        /// <summary>
        /// Indicates if the specified Position is within the current instance.
        /// </summary>
        /// <param name="position">A <strong>Position</strong> to test against the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the position is inside of the current rectangle.</returns>
        public bool IsOverlapping(Position position)
        {
            return !(position.Longitude.DecimalDegrees < _left.DecimalDegrees
                || position.Longitude.DecimalDegrees > _right.DecimalDegrees
                || position.Latitude.DecimalDegrees > _top.DecimalDegrees
                || position.Latitude.DecimalDegrees < _bottom.DecimalDegrees);
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
                rectangle.Top.DecimalDegrees > _top.DecimalDegrees ? rectangle.Top : _top,
                rectangle.Left.DecimalDegrees < _left.DecimalDegrees ? rectangle.Left : _left,
                rectangle.Bottom.DecimalDegrees < _bottom.DecimalDegrees ? rectangle.Bottom : _bottom,
                rectangle.Right.DecimalDegrees > _right.DecimalDegrees ? rectangle.Right : _right);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is GeographicRectangle)
                return Equals((GeographicRectangle)obj);
            return false;
        }

        /// <summary>
        /// Returns a unique code of the rectangle for hash tables.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _right.GetHashCode() ^ _top.GetHashCode() ^ _bottom.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Returns a rectangle which encloses the two specified rectangles.
        /// </summary>
        /// <param name="first">A <strong>GeographicRectangle</strong> to merge with the second rectangle.</param>
        /// <param name="second">A <strong>GeographicRectangle</strong> to merge with the first rectangle.</param>
        /// <returns>A <strong>GeographicRectangle</strong> as a result of merging the two
        /// rectangles.</returns>
        /// <remarks>This method is typically used to combine two individual shapes into a single
        /// shape.</remarks>
        public static GeographicRectangle UnionWith(GeographicRectangle first, GeographicRectangle second)
        {
            return first.UnionWith(second);
        }

        /// <summary>
        /// Returns the GeographicRectangle formed by the intersection of the two specified GeographicRectangles.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
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
                return Empty;

            // Calculate the new bounds
            Latitude top = positions[0].Latitude;
            Longitude left = positions[0].Longitude;
            Latitude bottom = positions[0].Latitude;
            Longitude right = positions[0].Longitude;

            // Loop through all items in the collection
            int limit = positions.Length;
            for (int index = 1; index < limit; index++)
            {
                Position item = positions[index];

                if (item.Longitude < left)
                    left = item.Longitude;
                else if (item.Longitude > right)
                    right = item.Longitude;
                if (item.Latitude > top)
                    top = item.Latitude;
                else if (item.Latitude < bottom)
                    bottom = item.Latitude;
            }

            // Build a new rectangle
            return new GeographicRectangle(left, top, right, bottom);
        }

        /// <summary>
        /// Parses a string into a GeographicRectangle object.
        /// </summary>
        /// <param name="value">A <string>String</string> specifying geographic coordinates defining a rectangle.</param>
        /// <returns>A <strong>GeographicRectangle</strong> object using the specified coordinates.</returns>
        /// <remarks>This powerful method will convert points defining a rectangle in the form of a string into
        /// a GeographicRectangle object.  The string can be</remarks>
        public static GeographicRectangle Parse(string value)
        {
            return new GeographicRectangle(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static GeographicRectangle Parse(string value, CultureInfo culture)
        {
            return new GeographicRectangle(value, culture);
        }

        #endregion Static Methods

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.String"/> to <see cref="DotSpatial.Positioning.GeographicRectangle"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator GeographicRectangle(string value)
        {
            return new GeographicRectangle(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.GeographicRectangle"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(GeographicRectangle value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region IEquatable<GeographicRectangle>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(GeographicRectangle other)
        {
            // The objects are equivalent if their bounds are equivalent
            return _left.DecimalDegrees.Equals(other.Left.DecimalDegrees)
                && _right.DecimalDegrees.Equals(other.Right.DecimalDegrees)
                && _top.DecimalDegrees.Equals(other.Top.DecimalDegrees)
                && _bottom.DecimalDegrees.Equals(other.Bottom.DecimalDegrees);
        }

        #endregion IEquatable<GeographicRectangle>

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            return _top.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " " + _left.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " " + _bottom.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " " + _right.ToString(format, culture);
        }

        #endregion IFormattable Members

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.</returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
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
            writer.WriteStartElement(Xml.GML_XML_PREFIX, "envelope", Xml.GML_XML_NAMESPACE);
            // <lowerCorner>
            writer.WriteStartElement(Xml.GML_XML_PREFIX, "lowerCorner", Xml.GML_XML_NAMESPACE);
            // <gml:pos>X Y</gml:pos>
            Southwest.WriteXml(writer);
            // </lowerCorner>
            writer.WriteEndElement();
            // <upperCorner>
            writer.WriteStartElement(Xml.GML_XML_PREFIX, "upperCorner", Xml.GML_XML_NAMESPACE);
            // <gml:pos>X Y</gml:pos>
            Northeast.WriteXml(writer);
            // </upperCorner>
            writer.WriteEndElement();
            // </gml:envelope>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
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

            _top = Latitude.Empty;
            _bottom = Latitude.Empty;
            _left = Longitude.Empty;
            _right = Longitude.Empty;
            _center = Position.Invalid;

            // Move to the <gml:Envelope>, <gml:boundedBy>, or <gml:Box> element
            if (!reader.IsStartElement("envelope", Xml.GML_XML_NAMESPACE)
                && !reader.IsStartElement("Envelope", Xml.GML_XML_NAMESPACE)
                && !reader.IsStartElement("boundedBy", Xml.GML_XML_NAMESPACE)
                && !reader.IsStartElement("Box", Xml.GML_XML_NAMESPACE))
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
                    _left = southWest.Longitude < northEast.Longitude ? southWest.Longitude : northEast.Longitude;
                    _right = northEast.Longitude > southWest.Longitude ? northEast.Longitude : southWest.Longitude;
                    _top = northEast.Latitude > southWest.Latitude ? northEast.Latitude : southWest.Latitude;
                    _bottom = southWest.Latitude < northEast.Latitude ? southWest.Latitude : northEast.Latitude;

                    // Lastly, calculate the center
                    _center = Position.Invalid;
                    _center = Hypotenuse.Midpoint;

                    // Read the end element, </gml:envelope>
                    reader.ReadEndElement();

                    #endregion <gml:Envelope>

                    return;
                case "boundedby":

                    #region <gml:boundedBy>

                    // Read the start element, <gml:boundedBy>
                    reader.ReadStartElement();

                    // Make a recursive call into here to read <gml:envelope>
                    GeographicRectangle value = new GeographicRectangle(reader);

                    // Copy the values
                    _left = value.Left;
                    _top = value.Top;
                    _right = value.Right;
                    _bottom = value.Bottom;
                    _center = value.Center;

                    // Read the end element, </gml:boundedBy>
                    reader.ReadEndElement();

                    #endregion <gml:boundedBy>

                    return;
                case "box":

                    #region <gml:Box>

                    // Read the start element, <gml:boundedBy>
                    reader.ReadStartElement();

                    // Read the coordinates
                    Position corner1 = new Position(reader);
                    Position corner2 = new Position(reader);

                    // Set the values
                    if (corner1.Latitude < corner2.Latitude)
                    {
                        _bottom = corner1.Latitude;
                        _top = corner2.Latitude;
                    }
                    else
                    {
                        _bottom = corner2.Latitude;
                        _top = corner1.Latitude;
                    }

                    if (corner1.Longitude < corner2.Longitude)
                    {
                        _left = corner1.Longitude;
                        _right = corner2.Longitude;
                    }
                    else
                    {
                        _left = corner2.Longitude;
                        _right = corner1.Longitude;
                    }

                    // Lastly, calculate the center
                    _center = Position.Invalid;
                    _center = Hypotenuse.Midpoint;

                    // Read the end element, </gml:boundedBy>
                    reader.ReadEndElement();

                    #endregion <gml:Box>

                    return;
            }
        }

        #endregion IXmlSerializable Members
    }
}