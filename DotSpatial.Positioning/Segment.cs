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

#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a line connected by two points on Earth's surface.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public struct Segment : IFormattable
    {
        /// <summary>
        ///
        /// </summary>
        private readonly Position _start;
        /// <summary>
        ///
        /// </summary>
        private readonly Position _end;

        #region Fields

        /// <summary>
        ///
        /// </summary>
        public static readonly Segment Empty = new Segment(Position.Empty, Position.Empty);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified end points.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public Segment(Position start, Position end)
        {
            _start = start;
            _end = end;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the distance from the starting point to the end point.
        /// </summary>
        public Distance Distance
        {
            get
            {
                return _start.DistanceTo(_end);
            }
        }

        /// <summary>
        /// Returns the bearing from the start to the end of the line.
        /// </summary>
        public Azimuth Bearing
        {
            get
            {
                return _start.BearingTo(_end);
            }
        }

        /// <summary>
        /// Returns the starting point of the segment.
        /// </summary>
        public Position Start
        {
            get
            {
                return _start;
            }
        }

        /// <summary>
        /// Returns the end point of the segment.
        /// </summary>
        public Position End
        {
            get
            {
                return _end;
            }
        }

        /// <summary>
        /// Returns the location halfway from the start to the end point.
        /// </summary>
        public Position Midpoint
        {
            get
            {
                return new Position(_start.Latitude.Add(_end.Latitude.DecimalDegrees).Multiply(0.5),
                                    _start.Longitude.Add(_end.Longitude.DecimalDegrees).Multiply(0.5));
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the distance from the segment to the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        /// <remarks>This method analyzes the relative position of the segment to the line to determine the
        /// best mathematical approach.</remarks>
        public Distance DistanceTo(Position position)
        {
            if (_start.Equals(_end))
                return position.DistanceTo(_start);
            Position delta = _end.Subtract(_start);
            double ratio = ((position.Longitude.DecimalDegrees - _start.Longitude.DecimalDegrees)
                * delta.Longitude.DecimalDegrees + (position.Latitude.DecimalDegrees - _start.Latitude.DecimalDegrees)
                * delta.Latitude.DecimalDegrees) / (delta.Longitude.DecimalDegrees * delta.Longitude.DecimalDegrees + delta.Latitude.DecimalDegrees
                * delta.Latitude.DecimalDegrees);
            if (ratio < 0)
                return position.DistanceTo(_start);
            if (ratio > 1)
                return position.DistanceTo(_end);
            Position destination = new Position(
                new Latitude((1 - ratio) * _start.Latitude.DecimalDegrees + ratio * _end.Latitude.DecimalDegrees),
                new Longitude((1 - ratio) * _start.Longitude.DecimalDegrees + ratio * _end.Longitude.DecimalDegrees));
            return position.DistanceTo(destination);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

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

            return _start.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator + " "
                + _end.ToString(format, formatProvider);
        }

        #endregion IFormattable Members
    }
}