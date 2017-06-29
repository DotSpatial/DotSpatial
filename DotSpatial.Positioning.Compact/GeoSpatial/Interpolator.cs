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
        CubicEaseIn,
        CubicEaseOut,
        CubicEaseInOut,
        QuarticEaseIn,
        ExponentialEaseIn,
        ExponentialEaseOut
    }

	/// <summary>Calculates intermediate values between two bounding values.</summary>
	/// <remarks>
	/// This powerful class provides the ability to interpolate values based on varying
	/// interpolation techniques. This class is used primarily to simulate realistic motion by
	/// accelerating and decelerating. This class is also used to calculate intermediate values
	/// for features such as image georeferencing and estimating precision errors.
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class Interpolator
    {
        private int _Count;
        private double _Minimum;
        private double _Maximum;
        private InterpolationMethod _InterpolationMethod;
        private double[] _Values;
        private readonly object SyncRoot = new object();

        public Interpolator()
        {
			lock(SyncRoot)
			{
				_Count = 10;
				_InterpolationMethod = InterpolationMethod.Linear;
				Recalculate();
			}
        }

        public Interpolator(int count, InterpolationMethod mode)
        {
			lock(SyncRoot)
			{
				_Count = count;
				_InterpolationMethod = mode;
				Recalculate();
			}
        }

        public Interpolator(double minimum, double maximum, int count)
        {
			lock(SyncRoot)
			{
				_Minimum = minimum;
				_Maximum = maximum;
				_Count = count;
				Recalculate();
			}
        }

        public Interpolator(double minimum, double maximum, int count, InterpolationMethod mode)
        {
			lock(SyncRoot)
			{
				_Minimum = minimum;
				_Maximum = maximum;
				_Count = count;
				_InterpolationMethod = mode;
				Recalculate();
			}
        }

		/// <summary>Controls the smallest number in the sequence.</summary>
        public double Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
				lock(SyncRoot)
				{
					if (_Minimum == value)
						return;
					_Minimum = value;
					Recalculate();
				}
            }
        }

		/// <summary>Controls the largest number in the sequence.</summary>
        public double Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
				lock(SyncRoot)
				{
					if (_Maximum == value) return;
					_Maximum = value;
					Recalculate();
				}
            }
        }

		/// <summary>Controls the acceleration and/or deceleration technique used.</summary>
        public InterpolationMethod InterpolationMethod
        {
            get
            {
                return _InterpolationMethod;
            }
            set
            {
				lock(SyncRoot)
				{
					if (_InterpolationMethod == value) return;
					_InterpolationMethod = value;
					Recalculate();
				}
            }
        }

		/// <summary>Controls the number of interpolated values.</summary>
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
				lock(SyncRoot)
				{
					if (_Count == value) 
						return;
					_Count = value;
					// Recalculate the array
					Recalculate();
				}
            }
        }

		/// <summary>Reverses the interpolated sequence.</summary>
        public void Swap()
        {
			lock(SyncRoot)
			{
				double Temp = _Minimum;
				_Minimum = _Maximum;
				_Maximum = Temp;
				Recalculate();
			}
        }

		/// <summary>Returns a number from the interpolated sequence.</summary>
        public double this[int index]
        {
            get
            {				
				//lock(SyncRoot)
				{
					if (index > Count - 1)
						return _Values[Count - 1];
					else if (index < 0)
						return _Values[0];
					else
						return _Values[index];
				}
            }
        }

        // Recalculates all values according to the specified mode
        private void Recalculate()
        {
            // Recalculate the entire array
            _Values = new double[_Count];
            if (_Count == 1)
            {
                // If min != max should probably be an error.
                _Values[0] = _Maximum;
            }
            else
            {
                for (int i = 0; i < _Count; i++)
                {
                    _Values[i] = CalculateValue(i);
                }
            }
        }

        private double CalculateValue(double index)
        {
            // The count needs to be zero based, like the index (or we need to calc +1 values to get both poles).
            int zeroCount = _Count - 1;

            //adjust formula to selected algoritm from combobox
            switch (this.InterpolationMethod)
            {
                case InterpolationMethod.Snap:
                    return _Maximum;
                case InterpolationMethod.Linear:
                    // Simple linear values - no acceleration or deceleration 
                    return ((_Maximum - _Minimum) * index / zeroCount + _Minimum);
                case InterpolationMethod.QuadraticEaseIn:
                    // Quadratic (Time ^ 2) easing in - accelerating from zero velocity
                    return ((_Maximum - _Minimum) * (index /= zeroCount) * index + _Minimum);
                case InterpolationMethod.QuadraticEaseOut:
                    // Quadratic (Index^2) easing out - decelerating to zero velocity
                    return (-(_Maximum - _Minimum) * (index = index / zeroCount) * (index - 2) + _Minimum);
                case InterpolationMethod.QuadraticEaseInAndOut:
                    // Quadratic easing in/out - acceleration until halfway, then deceleration
                    if ((index /= zeroCount * 0.5) < 1)
                    {
                        return ((_Maximum - _Minimum) * 0.5 * index * index + _Minimum);
                    }
                    else
                    {
                        return (-(_Maximum - _Minimum) * 0.5 * ((--index) * (index - 2) - 1) + _Minimum);
                    }
                case InterpolationMethod.CubicEaseIn:
                    // Cubic easing in - accelerating from zero velocity
                    return ((_Maximum - _Minimum) * (index /= zeroCount) * index * index + _Minimum);
                case InterpolationMethod.CubicEaseOut:
                    // Cubic easing in - accelerating from zero velocity
                    return ((_Maximum - _Minimum) * ((index = index / zeroCount - 1) * index * index + 1) + _Minimum);
                case InterpolationMethod.CubicEaseInOut:
                    // Cubic easing in - accelerating from zero velocity
                    if ((index /= zeroCount * 0.5) < 1)
                    {
                        return ((_Maximum - _Minimum) * 0.5 * index * index * index + _Minimum);
                    }
                    else
                    {
                        return ((_Maximum - _Minimum) * 0.5 * ((index -= 2) * index * index + 2) + _Minimum);
                    }
                case InterpolationMethod.QuarticEaseIn:
                    // Quartic easing in - accelerating from zero velocity
                    return ((_Maximum - _Minimum) * (index /= zeroCount) * index * index * index + _Minimum);
                case InterpolationMethod.ExponentialEaseIn:
                    // Exponential (2^Index) easing in - accelerating from zero velocity
                    if (index == 0)
                    {
                        return _Minimum;
                    }
                    else
                    {
                        return ((_Maximum - _Minimum) * Math.Pow(2, (10 * (index / zeroCount - 1))) + _Minimum);
                    }
                case InterpolationMethod.ExponentialEaseOut:
                    // exponential (2^Index) easing out - decelerating to zero velocity
                    if (index == zeroCount)
                    {
                        return (_Minimum + (_Maximum - _Minimum));
                    }
                    else
                    {
                        return ((_Maximum - _Minimum) * (-Math.Pow(2, -10 * index / zeroCount) + 1) + _Minimum);
                    }
                default:
                    return 0;
            }
        }
    }
}
