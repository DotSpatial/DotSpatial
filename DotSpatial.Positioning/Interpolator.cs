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

#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a collection of interpolated values using realistic acceleration and deceleration.
    /// </summary>
    /// <remarks>This enumeration is used by several DotSpatial.Positioning controls to smoothly transition from
    /// one value to another.  For example, the GPS SatelliteViewer uses acceleration to smoothly
    /// transition from one bearing to another, giving the appearance of a realistic compass.  This
    /// enumeration, when combined with the <see cref="Interpolator"></see> class lets you add smooth
    /// transitions to your own controls as well.</remarks>
    public enum InterpolationMethod
    {
        /// <summary>
        /// The transition occurs at a steady rate.
        /// </summary>
        Linear = 0,
        /// <summary>
        /// The transition is immediate; no interpolation takes place.
        /// </summary>
        Snap,
        /// <summary>
        /// The transition starts at zero and accelerates to the end using a quadratic formula.
        /// </summary>
        QuadraticEaseIn,
        /// <summary>
        /// The transition starts at high speed and decelerates to zero.
        /// </summary>
        QuadraticEaseOut,
        /// <summary>
        /// The transition accelerates to the halfway point, then decelerates to zero.
        /// </summary>
        QuadraticEaseInAndOut,
        /// <summary>
        ///
        /// </summary>
        CubicEaseIn,
        /// <summary>
        ///
        /// </summary>
        CubicEaseOut,
        /// <summary>
        ///
        /// </summary>
        CubicEaseInOut,
        /// <summary>
        ///
        /// </summary>
        QuarticEaseIn,
        /// <summary>
        ///
        /// </summary>
        ExponentialEaseIn,
        /// <summary>
        ///
        /// </summary>
        ExponentialEaseOut
    }

#if !PocketPC || DesignTime

    /// <summary>
    /// Calculates intermediate values between two bounding values.
    /// </summary>
    /// <remarks>This powerful class provides the ability to interpolate values based on varying
    /// interpolation techniques. This class is used primarily to simulate realistic motion by
    /// accelerating and decelerating. This class is also used to calculate intermediate values
    /// for features such as image georeferencing and estimating precision errors.</remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class Interpolator
    {
        /// <summary>
        ///
        /// </summary>
        private int _count;
        /// <summary>
        ///
        /// </summary>
        private double _minimum;
        /// <summary>
        ///
        /// </summary>
        private double _maximum;
        /// <summary>
        ///
        /// </summary>
        private InterpolationMethod _interpolationMethod;
        /// <summary>
        ///
        /// </summary>
        private double[] _values;
        /// <summary>
        ///
        /// </summary>
        private readonly object _syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Interpolator()
        {
            lock (_syncRoot)
            {
                _count = 10;
                _interpolationMethod = InterpolationMethod.Linear;
                Recalculate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpolator"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="mode">The mode.</param>
        public Interpolator(int count, InterpolationMethod mode)
        {
            lock (_syncRoot)
            {
                _count = count;
                _interpolationMethod = mode;
                Recalculate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpolator"/> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="count">The count.</param>
        public Interpolator(double minimum, double maximum, int count)
        {
            lock (_syncRoot)
            {
                _minimum = minimum;
                _maximum = maximum;
                _count = count;
                Recalculate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpolator"/> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="count">The count.</param>
        /// <param name="mode">The mode.</param>
        public Interpolator(double minimum, double maximum, int count, InterpolationMethod mode)
        {
            lock (_syncRoot)
            {
                _minimum = minimum;
                _maximum = maximum;
                _count = count;
                _interpolationMethod = mode;
                Recalculate();
            }
        }

        /// <summary>
        /// Controls the smallest number in the sequence.
        /// </summary>
        /// <value>The minimum.</value>
        public double Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                lock (_syncRoot)
                {
                    if (_minimum == value)
                        return;
                    _minimum = value;
                    Recalculate();
                }
            }
        }

        /// <summary>
        /// Controls the largest number in the sequence.
        /// </summary>
        /// <value>The maximum.</value>
        public double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                lock (_syncRoot)
                {
                    if (_maximum == value) return;
                    _maximum = value;
                    Recalculate();
                }
            }
        }

        /// <summary>
        /// Controls the acceleration and/or deceleration technique used.
        /// </summary>
        /// <value>The interpolation method.</value>
        public InterpolationMethod InterpolationMethod
        {
            get
            {
                return _interpolationMethod;
            }
            set
            {
                lock (_syncRoot)
                {
                    if (_interpolationMethod == value) return;
                    _interpolationMethod = value;
                    Recalculate();
                }
            }
        }

        /// <summary>
        /// Controls the number of interpolated values.
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
                lock (_syncRoot)
                {
                    if (_count == value)
                        return;
                    _count = value;
                    // Recalculate the array
                    Recalculate();
                }
            }
        }

        /// <summary>
        /// Reverses the interpolated sequence.
        /// </summary>
        public void Swap()
        {
            lock (_syncRoot)
            {
                double temp = _minimum;
                _minimum = _maximum;
                _maximum = temp;
                Recalculate();
            }
        }

        /// <summary>
        /// Returns a number from the interpolated sequence.
        /// </summary>
        public double this[int index]
        {
            get
            {
                //lock (SyncRoot)
                {
                    if (index > Count - 1)
                        return _values[Count - 1];
                    if (index < 0)
                        return _values[0];
                    return _values[index];
                }
            }
        }

        // Recalculates all values according to the specified mode
        /// <summary>
        /// Recalculates this instance.
        /// </summary>
        private void Recalculate()
        {
            // Recalculate the entire array
            _values = new double[_count];
            if (_count == 1)
            {
                // If min != max should probably be an error.
                _values[0] = _maximum;
            }
            else
            {
                for (int i = 0; i < _count; i++)
                {
                    _values[i] = CalculateValue(i);
                }
            }
        }

        /// <summary>
        /// Calculates the value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private double CalculateValue(double index)
        {
            // The count needs to be zero based, like the index (or we need to calc +1 values to get both poles).
            int zeroCount = _count - 1;

            //adjust formula to selected algoritm from combobox
            switch (InterpolationMethod)
            {
                case InterpolationMethod.Snap:
                    return _maximum;
                case InterpolationMethod.Linear:
                    // Simple linear values - no acceleration or deceleration
                    return ((_maximum - _minimum) * index / zeroCount + _minimum);
                case InterpolationMethod.QuadraticEaseIn:
                    // Quadratic (Time ^ 2) easing in - accelerating from zero velocity
                    return ((_maximum - _minimum) * (index /= zeroCount) * index + _minimum);
                case InterpolationMethod.QuadraticEaseOut:
                    // Quadratic (Index^2) easing out - decelerating to zero velocity
                    return (-(_maximum - _minimum) * (index = index / zeroCount) * (index - 2) + _minimum);
                case InterpolationMethod.QuadraticEaseInAndOut:
                    // Quadratic easing in/out - acceleration until halfway, then deceleration
                    if ((index /= zeroCount * 0.5) < 1)
                    {
                        return ((_maximum - _minimum) * 0.5 * index * index + _minimum);
                    }
                    return (-(_maximum - _minimum) * 0.5 * ((--index) * (index - 2) - 1) + _minimum);
                case InterpolationMethod.CubicEaseIn:
                    // Cubic easing in - accelerating from zero velocity
                    return ((_maximum - _minimum) * (index /= zeroCount) * index * index + _minimum);
                case InterpolationMethod.CubicEaseOut:
                    // Cubic easing in - accelerating from zero velocity
                    return ((_maximum - _minimum) * ((index = index / zeroCount - 1) * index * index + 1) + _minimum);
                case InterpolationMethod.CubicEaseInOut:
                    // Cubic easing in - accelerating from zero velocity
                    if ((index /= zeroCount * 0.5) < 1)
                    {
                        return ((_maximum - _minimum) * 0.5 * index * index * index + _minimum);
                    }
                    return ((_maximum - _minimum) * 0.5 * ((index -= 2) * index * index + 2) + _minimum);
                case InterpolationMethod.QuarticEaseIn:
                    // Quartic easing in - accelerating from zero velocity
                    return ((_maximum - _minimum) * (index /= zeroCount) * index * index * index + _minimum);
                case InterpolationMethod.ExponentialEaseIn:
                    // Exponential (2^Index) easing in - accelerating from zero velocity
                    if (index == 0)
                    {
                        return _minimum;
                    }
                    return ((_maximum - _minimum) * Math.Pow(2, (10 * (index / zeroCount - 1))) + _minimum);
                case InterpolationMethod.ExponentialEaseOut:
                    // exponential (2^Index) easing out - decelerating to zero velocity
                    if (index == zeroCount)
                    {
                        return (_minimum + (_maximum - _minimum));
                    }
                    return ((_maximum - _minimum) * (-Math.Pow(2, -10 * index / zeroCount) + 1) + _minimum);
                default:
                    return 0;
            }
        }
    }
}