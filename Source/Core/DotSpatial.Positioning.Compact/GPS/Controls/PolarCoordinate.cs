using System;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Security.Permissions;
#if !PocketPC || DesignTime
using System.Runtime.Serialization;
using System.ComponentModel;
#endif
using System.Reflection;

namespace DotSpatial.Positioning
{
    /// <summary>Represents a coordinate measured relative to the center of a circle.</summary>
    /// <remarks>
    /// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
    ///     immutable (its properties can only be changed via constructors).</para>
    /// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.PolarCoordinateConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=3ed3cdf4fdda3400")]
#endif
    public struct PolarCoordinate : IFormattable, IXmlSerializable
    {
        private Azimuth _Origin;
        private PolarCoordinateOrientation _Orientation;
        private float _R;
        private Angle _Theta;

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

        public PolarCoordinate(float r, double theta, Azimuth origin, PolarCoordinateOrientation orientation)
        {
            _R = r;
            _Theta = new Angle(theta);
            _Origin = origin;
            _Orientation = orientation;
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
            string[] Values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());
            // The first value is R, the second is Theta
            _R = float.Parse(Values[0], culture);
            _Theta = Angle.Parse(Values[1], culture);
            _Origin = Azimuth.North;
            _Orientation = PolarCoordinateOrientation.Clockwise;
        }

        #endregion

        #region Public Members

        /// <summary>Converts the current instance to a pixel coordinate.</summary>
        ////[CLSCompliant(false)]
        public Point ToPoint()
        {
            // First, convert to the North/CW orientation
            // then convert to pixel  (same as Cartesian but -Y)
            Radian Adj = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise).Theta.ToRadians();
            return new Point(Convert.ToInt32(R * Adj.Cosine().Value),
                Convert.ToInt32(-R * Adj.Sine().Value));
        }

#if !PocketPC
        /// <summary>Converts the current instance to a precise pixel coordinate.</summary>
        public PointF ToPointF()
        {
            // First, convert to the North/CW orientation
            // then convert to pixel  (same as Cartesian but -Y)
            Radian Adj = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise).Theta.ToRadians();
            return new PointF(Convert.ToSingle(R * Adj.Cosine().Value),
                Convert.ToSingle(-R * Adj.Sine().Value));
        }
#endif

        /// <summary>Converts the current instance to a highly-precise pixel coordinate.</summary>
        public PointD ToPointD()
        {
            // First, convert to the North/CW orientation
            // then convert to pixel  (same as Cartesian but -Y)
            Radian Adj = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise).Theta.ToRadians();
            return new PointD(R * Adj.Cosine().Value, -R * Adj.Sine().Value);
        }

        /// <summary>Converts the current instance to a highly-precise Cartesian coordinate.</summary>
        public PointD ToCartesianPointD()
        {
            // First, convert to the North/CW orientation
            PolarCoordinate AdjustedValue = ToOrientation(Azimuth.East, PolarCoordinateOrientation.Counterclockwise);
            // Now convert to pixel  (same as Cartesian but -Y)
            return new PointD(R * Math.Cos(AdjustedValue.Theta.ToRadians().Value),
                (R * Math.Sin(AdjustedValue.Theta.ToRadians().Value)));
        }

        /// <summary>
        /// Returns <em>Theta</em>, the amount of clockwise rotation of the
        /// coordinate.
        /// </summary>
        public Angle Theta
        {
            get
            {
                return _Theta;
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
            else
                return new PolarCoordinate(_R, _Theta.Add(angle), _Origin, _Orientation);
        }

        public PolarCoordinate SetRotation(double angle)
        {
            return new PolarCoordinate(_R, new Angle(angle), _Origin, _Orientation);
        }

        /// <summary>
        /// Returns <em>R</em>, the distance away from the center of an imaginary
        /// circle.
        /// </summary>
        public float R
        {
            get
            {
                return _R;
            }
        }

        /// <summary>
        /// Returns the compass direction which matches zero degrees.
        /// </summary>
        public Azimuth Origin
        {
            get
            {
                return _Origin;
            }
        }

        /// <summary>
        /// Returns whether positive values are applied in a clockwise or counter-clockwise direction.
        /// </summary>
        public PolarCoordinateOrientation Orientation
        {
            get
            {
                return _Orientation;
            }
        }

        /// <summary>
        /// Returns the current instance adjusted to the specified orientation and
        /// origin.
        /// </summary>
        public PolarCoordinate ToOrientation(Azimuth origin, PolarCoordinateOrientation orientation)
        {
            if (_Orientation.Equals(orientation) && _Origin.Equals(origin))
                return this;
            // Make a copy of the angle
            double NewAngle = Theta.DecimalDegrees;
            // Has the CW/CCW orientation changed?
            if (Orientation != orientation)
                // Yes.  Subtract the angle from 360
                NewAngle = 360 - NewAngle;
            if (Origin != origin)
            {
                // Add the offset to the angle and normalize
                NewAngle -= 360 - origin.DecimalDegrees - Origin.DecimalDegrees;
            }
            // And return the new coordinate
            return new PolarCoordinate(_R, new Angle(NewAngle), origin, orientation);
        }

        #endregion

        #region Static Methods

        public static PolarCoordinate Parse(string value)
        {
            return new PolarCoordinate(value, CultureInfo.CurrentCulture);
        }

        public static PolarCoordinate Parse(string value, CultureInfo culture)
        {
            return new PolarCoordinate(value, culture);
        }

        #endregion

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            return R.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " "
                + Theta.ToString(format, culture);
        }

        #region Conversions

        public static explicit operator PolarCoordinate(string value)
        {
            return PolarCoordinate.Parse(value, CultureInfo.CurrentCulture);
        }

        public static explicit operator string(PolarCoordinate value)
        {
            return value.ToString();
        }

        ////[CLSCompliant(false)]
        public static explicit operator Point(PolarCoordinate value)
        {
            return value.ToPoint();
        }

#if !PocketPC
        public static explicit operator PointF(PolarCoordinate value)
        {
            return value.ToPointF();
        }
#endif

        public static explicit operator PointD(PolarCoordinate value)
        {
            return value.ToPointD();
        }

        #endregion

        #region IFormattable Members

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return this.ToString(format, (CultureInfo)formatProvider);
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

    public sealed class PolarCoordinateOrientationEventArgs : EventArgs
    {
        private PolarCoordinateOrientation pOrientation;

        public PolarCoordinateOrientationEventArgs(PolarCoordinateOrientation orientation)
        {
            pOrientation = orientation;
        }

        public PolarCoordinateOrientation Orientation
        {
            get
            {
                return pOrientation;
            }
        }
    }

}
