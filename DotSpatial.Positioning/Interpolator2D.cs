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
#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a collection of interpolated coordinates using realistic acceleration and deceleration.
    /// </summary>
    /// <remarks><para>This class is used by several controls in the DotSpatial.Positioning namespace to give
    /// them a more realistic behavior. This class will interpolate coordinates between a
    /// given start and end point according to an interpolation technique, and return them
    /// as an array. Then, controls and other elements can be moved smoothly by applying
    /// the calculated values.</para>
    ///   <para>Instances of this class are likely to be thread safe because the class uses
    /// thread synchronization when recalculating interpolated values.</para></remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class Interpolator2D
    {
        /// <summary>
        ///
        /// </summary>
        private Position _minimum = Position.Empty;
        /// <summary>
        ///
        /// </summary>
        private Position _maximum = Position.Empty;
        /// <summary>
        ///
        /// </summary>
        private int _count = 1;
        /// <summary>
        ///
        /// </summary>
        private InterpolationMethod _interpolationMethod = InterpolationMethod.Linear;
        /// <summary>
        ///
        /// </summary>
        private Position[] _values;
        /// <summary>
        ///
        /// </summary>
        private Interpolator _xValues;
        /// <summary>
        ///
        /// </summary>
        private Interpolator _yValues;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public Interpolator2D()
        {
        }

        /// <summary>
        /// Creates a new instance using the specified start and end points.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="count">The count.</param>
        /// <remarks>This constructor provides a way to define the bounds of the interpolator,
        /// as well as its number of points.  A higher level of points yield a smoother
        /// result but take longer to iterate through.</remarks>
        ////[CLSCompliant(false)]
        public Interpolator2D(Position minimum, Position maximum, int count)
        {
            Count = count;
            _minimum = minimum;
            _maximum = maximum;
            Recalculate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpolator2D"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="mode">The mode.</param>
        public Interpolator2D(int count, InterpolationMethod mode)
        {
            _count = count;
            _interpolationMethod = mode;
            Recalculate();
        }

        /// <summary>
        /// Creates a new instance using the specified end points, count, and interpolation technique.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="count">The count.</param>
        /// <param name="mode">The mode.</param>
        ////[CLSCompliant(false)]
        public Interpolator2D(Position minimum, Position maximum, int count, InterpolationMethod mode)
            : this(minimum, maximum, count)
        {
            _interpolationMethod = mode;
            Recalculate();
        }

        /// <summary>
        /// Returns the starting point of the series.
        /// </summary>
        /// <value>The minimum.</value>
        /// <remarks>Interpolated values are calculated between this point and the end point
        /// stored in the <see cref="Maximum"></see> property.  Changing this property causes
        /// the series to be recalculated.</remarks>
        ////[CLSCompliant(false)]
        public Position Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                if (_minimum.Equals(value)) return;
                _minimum = value;
                Recalculate();
            }
        }

        /// <summary>
        /// Returns the ending point of the series.
        /// </summary>
        /// <value>The maximum.</value>
        /// <remarks>Interpolated values are calculated between this point and the start point
        /// stored in the <see cref="Minimum"></see> property.  Changing this property causes
        /// the series to be recalculated.</remarks>
        ////[CLSCompliant(false)]
        public Position Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                if (_maximum.Equals(value)) return;
                _maximum = value;
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
                return _values[index];
            }
        }

        /// <summary>
        /// Returns the number of calculated positions in the series.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (_count == value) return;
                _count = value;
                // Redefine the array
                _values = new Position[_count];
                // Recalculate the array
                Recalculate();
            }
        }

        /// <summary>
        /// Indicates the interpolation technique used to calculate intermediate points.
        /// </summary>
        /// <value>The interpolation method.</value>
        /// <remarks>This property controls the acceleration and deceleration techniques
        /// used when calculating intermediate points.  Changing this property causes the
        /// series to be recalculated.</remarks>
        public InterpolationMethod InterpolationMethod
        {
            get
            {
                return _interpolationMethod;
            }
            set
            {
                if (_interpolationMethod == value) return;
                _interpolationMethod = value;
                Recalculate();
            }
        }

        // Recalculates all values according to the specified mode
        /// <summary>
        /// Recalculates this instance.
        /// </summary>
        private void Recalculate()
        {
            // Reinitialize X values
            _xValues = new Interpolator(_minimum.Longitude.DecimalDegrees, _maximum.Longitude.DecimalDegrees, Count);
            // Reinitialize Y values
            _yValues = new Interpolator(_minimum.Latitude.DecimalDegrees, _maximum.Latitude.DecimalDegrees, Count);
            // Convert the arrays into a MapRoute
            for (int iteration = 0; iteration < Count; iteration++)
            {
                // Add a new Position to the value collection
                _values[iteration] = new Position(new Latitude(_yValues[iteration]), new Longitude(_xValues[iteration]));
            }
        }

        /// <summary>
        /// Swaps this instance.
        /// </summary>
        public void Swap()
        {
            Position temp = _minimum;
            _minimum = _maximum;
            _maximum = temp;
            Recalculate();
        }
    }
}