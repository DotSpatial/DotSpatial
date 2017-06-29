using System;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning
{
	/// <summary>Represents a collection of interpolated coordinates using realistic acceleration and deceleration.</summary>
	/// <remarks>
	/// 	<para>This class is used by several controls in the DotSpatial.Positioning namespace to give
	///     them a more realistic behavior. This class will interpolate coordinates between a
	///     given start and end point according to an interpolation technique, and return them
	///     as an array. Then, controls and other elements can be moved smoothly by applying
	///     the calculated values.</para>
	/// 	<para>Instances of this class are likely to be thread safe because the class uses
	///     thread synchronization when recalculating interpolated values.</para>
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class Interpolator2D
    {
        private Position _Minimum = Position.Empty;
		private Position _Maximum = Position.Empty;
		private int _Count = 1;
		private InterpolationMethod _InterpolationMethod = InterpolationMethod.Linear;
		private Position[] _Values;
		private Interpolator XValues;
		private Interpolator YValues;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public Interpolator2D()
		{
		}

		/// <summary>
		/// Creates a new instance using the specified start and end points.
		/// </summary>
		/// <param name="minimum">The starting point of the interpolated series.</param>
		/// <param name="maximum">The ending point of the interpolated series.</param>
		/// <param name="count">The number of points to calculate between the start and end.</param>
		/// <remarks>This constructor provides a way to define the bounds of the interpolator,
		/// as well as its number of points.  A higher level of points yield a smoother
		/// result but take longer to iterate through.</remarks>
		////[CLSCompliant(false)]
		public Interpolator2D(Position minimum, Position maximum, int count)
		{
			Count = count;
			_Minimum = minimum;
			_Maximum = maximum;
			Recalculate();
		}

		public Interpolator2D(int count, InterpolationMethod mode)
		{
			_Count = count;
			_InterpolationMethod = mode;
			Recalculate();
		}

		/// <summary>
		/// Creates a new instance using the specified end points, count, and interpolation technique.
		/// </summary>
		/// <param name="minimum">The starting point of the interpolated series.</param>
		/// <param name="maximum">The ending point of the interpolated series.</param>
		/// <param name="count">The number of points to calculate between the start and end.</param>
		/// <param name="mode">The interpolation technique to use for calculating intermediate points.</param>
		////[CLSCompliant(false)]
		public Interpolator2D(Position minimum, Position maximum, int count, InterpolationMethod mode) 
			: this(minimum, maximum, count)
		{
			_InterpolationMethod = mode;
			Recalculate();
		}

		/// <summary>
		/// Returns the starting point of the series.
		/// </summary>
		/// <remarks>Interpolated values are calculated between this point and the end point 
		/// stored in the <see cref="Maximum"></see> property.  Changing this property causes
		/// the series to be recalculated.</remarks>
		////[CLSCompliant(false)]
		public Position Minimum
		{
			get
			{
				return _Minimum;
			}
			set
			{
				if(_Minimum.Equals(value)) return;
				_Minimum = value;
				Recalculate();
			}
		}

		/// <summary>
		/// Returns the ending point of the series.
		/// </summary>
		/// <remarks>Interpolated values are calculated between this point and the start point 
		/// stored in the <see cref="Minimum"></see> property.  Changing this property causes
		/// the series to be recalculated.</remarks>
		////[CLSCompliant(false)]
		public Position Maximum
		{
			get
			{
				return _Maximum;
			}
			set
			{
				if(_Maximum.Equals(value)) return;
				_Maximum = value;
				Recalculate();
			}
		}

		/// <summary>
		/// Returns a Position object from the interpolated series.
		/// </summary>
		public Position this[int index]
		{
			get
			{
				return (Position)_Values[index];
			}
		}

		/// <summary>
		/// Returns the number of calculated positions in the series.
		/// </summary>
		public int Count
		{
			get
			{
				return _Count;
			}
			set
			{
				if (_Count == value) return;
				_Count = value;
				// Redefine the array
				_Values = new Position[_Count];
				// Recalculate the array
				Recalculate();
			}
		}

		/// <summary>
		/// Indicates the interpolation technique used to calculate intermediate points.
		/// </summary>
		/// <remarks>This property controls the acceleration and deceleration techniques
		/// used when calculating intermediate points.  Changing this property causes the
		/// series to be recalculated.</remarks>
		public InterpolationMethod InterpolationMethod
		{
			get
			{
				return _InterpolationMethod;
			}
			set
			{
				if (_InterpolationMethod == value) return;
				_InterpolationMethod = value;
				Recalculate();
			}
		}

		// Recalculates all values according to the specified mode
		private void Recalculate()
		{	
			// Reinitialize X values
            XValues = new Interpolator(_Minimum.Longitude.DecimalDegrees, _Maximum.Longitude.DecimalDegrees, Count);
			// Reinitialize Y values
            YValues = new Interpolator(_Minimum.Latitude.DecimalDegrees, _Maximum.Latitude.DecimalDegrees, Count);
			// Convert the arrays into a MapRoute
			for(int Iteration = 0; Iteration < Count; Iteration++)
			{
				// Add a new Position to the value collection
				_Values[Iteration] = new Position(new Latitude(YValues[Iteration]), new Longitude(XValues[Iteration]));
			}
		}

		public void Swap()
		{
			Position Temp = _Minimum;
			_Minimum = _Maximum;
			_Maximum = Temp;
			Recalculate();
		}
	}
}