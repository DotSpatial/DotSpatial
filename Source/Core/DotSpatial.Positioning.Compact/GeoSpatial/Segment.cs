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
    // TODO: Make this a structure once Position becomes a structure.

	/// <summary>
	/// Represents a line connected by two points on Earth's surface.
	/// </summary>
#if !PocketPC || DesignTime
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public struct Segment : IFormattable
	{
        private readonly Position _Start;
        private readonly Position _End;

        #region Fields

        public static readonly Segment Empty = new Segment(Position.Empty, Position.Empty);

        #endregion

        #region Constructors

		/// <summary>Creates a new instance using the specified end points.</summary>
		public Segment(Position start, Position end)
		{
			_Start = start;
			_End = end;
        }

        #endregion

        #region Public Properties

        /// <summary>
		/// Returns the distance from the starting point to the end point.
		/// </summary>
		public Distance Distance
		{
			get
			{
				return _Start.DistanceTo(_End);
			}
		}

		/// <summary>
		/// Returns the bearing from the start to the end of the line.
		/// </summary>
		public Azimuth Bearing
		{
			get
			{
				return _Start.BearingTo(_End);
			}
		}

		/// <summary>
		/// Returns the starting point of the segment.
		/// </summary>
		public Position Start
		{
			get
			{
				return _Start;
			}
		}

		/// <summary>
		/// Returns the end point of the segment.
		/// </summary>
		public Position End
		{
			get
			{
				return _End;
			}
		}

		/// <summary>Returns the location halfway from the start to the end point.</summary>
		public Position Midpoint
		{
			get
			{
				return new Position(_Start.Latitude.Add(_End.Latitude.DecimalDegrees).Multiply(0.5),
                                    _Start.Longitude.Add(_End.Longitude.DecimalDegrees).Multiply(0.5));
			}
        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Returns the distance from the segment to the specified position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		/// <remarks>This method analyzes the relative position of the segment to the line to determine the
		/// best mathematical approach.</remarks>
		public Distance DistanceTo(Position position)
		{
			if(_Start.Equals(_End))
				return position.DistanceTo(_Start);
			Position Delta = _End.Subtract(_Start);
			double Ratio = ((position.Longitude.DecimalDegrees - _Start.Longitude.DecimalDegrees) 
                * Delta.Longitude.DecimalDegrees + (position.Latitude.DecimalDegrees - _Start.Latitude.DecimalDegrees) 
                * Delta.Latitude.DecimalDegrees) / (Delta.Longitude.DecimalDegrees * Delta.Longitude.DecimalDegrees + Delta.Latitude.DecimalDegrees 
                * Delta.Latitude.DecimalDegrees);
			if(Ratio < 0)
				return position.DistanceTo(_Start);
			else if(Ratio > 1)
				return position.DistanceTo(_End);
			else
			{
				Position Destination = new Position(
                    new Latitude((1 - Ratio) * _Start.Latitude.DecimalDegrees + Ratio * _End.Latitude.DecimalDegrees),
					new Longitude((1 - Ratio) * _Start.Longitude.DecimalDegrees + Ratio * _End.Longitude.DecimalDegrees));
				return position.DistanceTo(Destination);
			}
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
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

            return _Start.ToString(format, formatProvider) 
                + culture.TextInfo.ListSeparator + " "
                + _End.ToString(format, formatProvider);
        }

        #endregion
    }
}
