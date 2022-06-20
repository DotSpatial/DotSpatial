// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a line connected by two points on Earth's surface.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Segment : IFormattable
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        public static readonly Segment Empty = new(Position.Empty, Position.Empty);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified end points.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public Segment(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the distance from the starting point to the end point.
        /// </summary>
        public Distance Distance => Start.DistanceTo(End);

        /// <summary>
        /// Returns the bearing from the start to the end of the line.
        /// </summary>
        public Azimuth Bearing => Start.BearingTo(End);

        /// <summary>
        /// Returns the starting point of the segment.
        /// </summary>
        public Position Start { get; }

        /// <summary>
        /// Returns the end point of the segment.
        /// </summary>
        public Position End { get; }

        /// <summary>
        /// Returns the location halfway from the start to the end point.
        /// </summary>
        public Position Midpoint
        {
            get => new(Start.Latitude.Add(End.Latitude.DecimalDegrees).Multiply(0.5),
                                    Start.Longitude.Add(End.Longitude.DecimalDegrees).Multiply(0.5));
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
            if (Start.Equals(End))
            {
                return position.DistanceTo(Start);
            }

            Position delta = End.Subtract(Start);
            double ratio = ((position.Longitude.DecimalDegrees - Start.Longitude.DecimalDegrees)
                * delta.Longitude.DecimalDegrees + (position.Latitude.DecimalDegrees - Start.Latitude.DecimalDegrees)
                * delta.Latitude.DecimalDegrees) / (delta.Longitude.DecimalDegrees * delta.Longitude.DecimalDegrees + delta.Latitude.DecimalDegrees
                * delta.Latitude.DecimalDegrees);
            if (ratio < 0)
            {
                return position.DistanceTo(Start);
            }

            if (ratio > 1)
            {
                return position.DistanceTo(End);
            }

            Position destination = new(
                new Latitude((1 - ratio) * Start.Latitude.DecimalDegrees + ratio * End.Latitude.DecimalDegrees),
                new Longitude((1 - ratio) * Start.Longitude.DecimalDegrees + ratio * End.Longitude.DecimalDegrees));
            return position.DistanceTo(destination);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            return Start.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator + " "
                + End.ToString(format, formatProvider);
        }

        #endregion IFormattable Members
    }
}