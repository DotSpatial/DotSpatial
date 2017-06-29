// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.Forms.dll
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
using System.Drawing;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime
    /// <summary>Represents a coordinate measured relative to the center of a circle.</summary>
    /// <remarks>
    /// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
    ///     immutable (its properties can only be changed via constructors).</para>
    /// </remarks>
    [TypeConverter("DotSpatial.Positioning.Design.PolarCoordinateConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=3ed3cdf4fdda3400")]
#endif
    public struct PolarCoordinate : IFormattable, IXmlSerializable
    {
        private Azimuth _origin;
        private readonly PolarCoordinateOrientation _orientation;
        private readonly float _r;
        private Angle _theta;

        /// <summary>Represents a polar coordinate with no value.</summary>
        public static readonly PolarCoordinate Empty = new PolarCoordinate(0, Angle.Empty);
        /// <summary>Represents a polar coordinate at the center of a circle.</summary>
        public static readonly PolarCoordinate Center = new PolarCoordinate(0, Angle.Empty);

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified radius and angle.
        /// </summary>
        /// <param name="r">A <strong>Double</strong> indicating a radius.  Increasing values represent a distance further away from the center of a circle.</param>
        /// <param name="theta">An <strong>Angle</strong> representing a direction from the center of a circle.</param>
        /// <remarks>The radius "r," when combined with an angle "theta" will create a coordinate relative to
        /// the center of a circle.  By default, an angle of zero represents the top of the circle ("North") and
        /// increasing angles wind clockwise around the circle.</remarks>
        public PolarCoordinate(float r, Angle theta)
            : this(r, theta, Azimuth.North, PolarCoordinateOrientation.Clockwise)
        { }

        /// <summary>
        /// Creates a new instance using the specified radius and angle.
        /// </summary>
        /// <param name="r">A <strong>Double</strong> indicating a radius.  Increasing values represent a distance further away from the center of a circle.</param>
        /// <param name="theta">An <strong>Angle</strong> representing a direction from the center of a circle.</param>
        /// <remarks>The radius "r," when combined with an angle "theta" will create a coordinate relative to
        /// the center of a circle.  By default, an angle of zero represents the top of the circle ("North") and
        /// increasing angles wind clockwise around the circle.</remarks>
        public PolarCoordinate(float r, double theta)
            : this(r, theta, Azimuth.North, PolarCoordinateOrientation.Clockwise)
        { }

        /// <summary>
        /// Creates a new instance using the specified radius, angle, origin and winding direction.
        /// </summary>
        /// <param name="r">A <strong>Double</strong> indicating a radius.  Increasing values represent a distance further away from the center of a circle.</param>
        /// <param name="theta">An <strong>Angle</strong> representing a direction from the center of a circle.</param>
        /// <param name="origin">An <strong>Azimuth</strong> indicating which compass direction is associated with zero degrees.  (Typically North.)</param>
        /// <param name="orientation">A <strong>PolarCoordinateOrientation</strong> value indicating whether increasing Theta values wind clockwise or counter-clockwise.</param>
        /// <remarks>The radius "r," when combined with an angle "theta" will create a coordinate relative to
        /// the center of a circle.  The BearingOrigin will associate a compass direction with zero degrees (0°), but this value is typically "North".</remarks>
        public PolarCoordinate(float r, Angle theta, Azimuth origin, PolarCoordinateOrientation orientation)
            : this(r, theta.DecimalDegrees, origin, orientation)
        { }

        /// <summary>
        /// Creates a new Polar Coordinate
        /// </summary>
        /// <param name="r"></param>
        /// <param name="theta"></param>
        /// <param name="origin"></param>
        /// <param name="orientation"></param>
        public PolarCoordinate(float r, double theta, Azimuth origin, PolarCoordinateOrientation orientation)
        {
            _r = r;
            _theta = new Angle(theta);
            _origin = origin;
            _orientation = orientation;
        }

        /// <summary>
        /// Creates a new instance by converting the specified string.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing a polar coordinate in the current culture.</param>
        public PolarCoordinate(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by converting the specified string.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing a polar coordinate in any culture.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object describing how to parse the specified string.</param>
        public PolarCoordinate(string value, CultureInfo culture)
        {
            // Split the two values based on the list separator
            string[] values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());
            // The first value is R, the second is Theta
            _r = float.Parse(values[0], culture);
            _theta = Angle.Parse(values[1], culture);
            _origin = Azimuth.North;
            _orientation = PolarCoordinateOrientation.Clockwise;
        }

        #endregion

        #region Public Members

        /// <summary>Converts the current instance to a pixel coordinate.</summary>
        ////[CLSCompliant(false)]
        public Point ToPoint()
        {
            // First, convert to the North/CW orientation
            // then convert to pixel  (same as Cartesian but -Y)
            Radian adj = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise).Theta.ToRadians();
            return new Point(Convert.ToInt32(R * adj.Cosine().Value),
                Convert.ToInt32(-R * adj.Sine().Value));
        }

#if !PocketPC

        /// <summary>Converts the current instance to a precise pixel coordinate.</summary>
        public PointF ToPointF()
        {
            // First, convert to the North/CW orientation
            // then convert to pixel  (same as Cartesian but -Y)
            Radian adj = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise).Theta.ToRadians();
            return new PointF(Convert.ToSingle(R * adj.Cosine().Value),
                Convert.ToSingle(-R * adj.Sine().Value));
        }

#endif

        /// <summary>Converts the current instance to a highly-precise pixel coordinate.</summary>
        public PointD ToPointD()
        {
            // First, convert to the North/CW orientation
            // then convert to pixel  (same as Cartesian but -Y)
            Radian adj = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise).Theta.ToRadians();
            return new PointD(R * adj.Cosine().Value, -R * adj.Sine().Value);
        }

        /// <summary>Converts the current instance to a highly-precise Cartesian coordinate.</summary>
        public PointD ToCartesianPointD()
        {
            // First, convert to the North/CW orientation
            PolarCoordinate adjustedValue = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise);
            // Now convert to pixel  (same as Cartesian but -Y)
            return new PointD(R * Math.Cos(adjustedValue.Theta.ToRadians().Value),
                (R * Math.Sin(adjustedValue.Theta.ToRadians().Value)));
        }

        /// <summary>
        /// Returns <em>Theta</em>, the amount of clockwise rotation of the
        /// coordinate.
        /// </summary>
        public Angle Theta
        {
            get
            {
                return _theta;
            }
        }

        /// <summary>
        /// Applies rotation to the existing coordinate.
        /// </summary>
        /// <param name="angle">The amount of rotation to apply (above zero for clockwise).</param>
        /// <returns>A <strong>PolarCoordinate</strong> adjusted by the specified rotation amount.</returns>
        public PolarCoordinate Rotate(double angle)
        {
            if (angle == 0)
                return this;
            return new PolarCoordinate(_r, _theta.Add(angle), _origin, _orientation);
        }

        /// <summary>
        /// Sets the rotation of the angle
        /// </summary>
        /// <param name="angle">The angle to set</param>
        /// <returns>the PolarCoordinate</returns>
        public PolarCoordinate SetRotation(double angle)
        {
            return new PolarCoordinate(_r, new Angle(angle), _origin, _orientation);
        }

        /// <summary>
        /// Returns <em>R</em>, the distance away from the center of an imaginary
        /// circle.
        /// </summary>
        public float R
        {
            get
            {
                return _r;
            }
        }

        /// <summary>
        /// Returns the compass direction which matches zero degrees.
        /// </summary>
        public Azimuth Origin
        {
            get
            {
                return _origin;
            }
        }

        /// <summary>
        /// Returns whether positive values are applied in a clockwise or counter-clockwise direction.
        /// </summary>
        public PolarCoordinateOrientation Orientation
        {
            get
            {
                return _orientation;
            }
        }

        /// <summary>
        /// Returns the current instance adjusted to the specified orientation and
        /// origin.
        /// </summary>
        public PolarCoordinate ToOrientation(Azimuth origin, PolarCoordinateOrientation orientation)
        {
            if (_orientation.Equals(orientation) && _origin.Equals(origin))
                return this;
            // Make a copy of the angle
            double newAngle = Theta.DecimalDegrees;
            // Has the CW/CCW orientation changed?
            if (Orientation != orientation)
                // Yes.  Subtract the angle from 360
                newAngle = 360 - newAngle;
            if (Origin != origin)
            {
                // Add the offset to the angle and normalize
                newAngle -= 360 - origin.DecimalDegrees - Origin.DecimalDegrees;
            }
            // And return the new coordinate
            return new PolarCoordinate(_r, new Angle(newAngle), origin, orientation);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Parses the value as a PolarCoordinate
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PolarCoordinate Parse(string value)
        {
            return new PolarCoordinate(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses the value in the specified culter as a polar coordinate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static PolarCoordinate Parse(string value, CultureInfo culture)
        {
            return new PolarCoordinate(value, culture);
        }

        #endregion

        /// <inheritdocs/>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        ///<summary>
        /// Customizes the string with a format provider
        ///</summary>
        ///<param name="format"></param>
        ///<param name="formatProvider"></param>
        ///<returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            return R.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " "
                + Theta.ToString(format, culture);
        }

        #region Conversions

        /// <summary>
        /// Polar Coordinate
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator PolarCoordinate(string value)
        {
            return Parse(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator string(PolarCoordinate value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Point
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Point(PolarCoordinate value)
        {
            return value.ToPoint();
        }

#if !PocketPC

        /// <summary>
        /// PointF
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator PointF(PolarCoordinate value)
        {
            return value.ToPointF();
        }

#endif

        /// <summary>
        /// PointD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator PointD(PolarCoordinate value)
        {
            return value.ToPointD();
        }

        #endregion

        #region IFormattable Members

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format, formatProvider);
        }

        #endregion

        #region IXmlSerializable Members

#if !PocketPC || (PocketPC && Framework20)

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
        }

#endif

        #endregion
    }

    /// <summary>
    /// Controls the winding direction of increasing angular values.
    /// </summary>
    /// <remarks>This enumeration is used by the <strong>PolarCoordinate</strong> class
    /// to determine where a coordinate is on a circle.</remarks>
    public enum PolarCoordinateOrientation
    {
        /// <summary>
        /// Increasing angles are further clockwise around the circle.
        /// </summary>
        Clockwise,
        /// <summary>
        /// Increasing angles are further counter-clockwise around the circle.
        /// </summary>
        Counterclockwise
    }

    /// <summary>
    /// Polar Coordinate Orientation Event Args
    /// </summary>
    public sealed class PolarCoordinateOrientationEventArgs : EventArgs
    {
        private readonly PolarCoordinateOrientation _pOrientation;

        /// <summary>
        /// Creates a new instance of the Polar coordinate orientation event arguments
        /// </summary>
        /// <param name="orientation"></param>
        public PolarCoordinateOrientationEventArgs(PolarCoordinateOrientation orientation)
        {
            _pOrientation = orientation;
        }

        /// <summary>
        /// The orientation
        /// </summary>
        public PolarCoordinateOrientation Orientation
        {
            get
            {
                return _pOrientation;
            }
        }
    }
}