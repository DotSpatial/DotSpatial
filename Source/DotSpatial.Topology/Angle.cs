// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology
{
    /// <summary>
    /// A geometric angle mesured in degrees or radians
    /// the angle will wrap around, so setting larger values will
    /// result in an appropriate angle.
    /// </summary>
    public struct Angle
    {
        #region Variables

        /// <summary>
        /// The value of 3.14159 or whatever from Math.PI
        /// </summary>
        public const double PI = Math.PI;
        double _rad;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an angle with the radians specified
        /// </summary>
        /// <param name="radians">The angle in radians</param>
        public Angle(double radians)
        {
            if (radians > 2 * Math.PI || radians < -2 * Math.PI)
            {
                _rad = radians % (Math.PI * 2);
                return;
            }
            _rad = radians;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle in degrees, ranging from -360 to 360
        /// </summary>
        public double Degrees
        {
            get
            {
                return _rad * 180 / Math.PI;
            }
            set
            {
                if (value > 360 || value < -360)
                {
                    _rad = (value % 360) * Math.PI / 180;
                    return;
                }
                _rad = value * Math.PI / 180;
            }
        }

        /// <summary>
        /// Gets or sets the angle in degrees ranging from 0 to 360
        /// </summary>
        public double DegreesPos
        {
            get
            {
                double dg = _rad * 180 / Math.PI;
                if (dg < 0)
                {
                    return 360 + dg;
                }
                return dg;
            }
            set
            {
                double dg = value;

                if (value > 360 || value < -360)
                {
                    dg = (dg % 360);
                }
                if (dg < 360)
                {
                    dg = 360 - dg;
                }
                _rad = dg * Math.PI / 180;
            }
        }

        /// <summary>
        /// Only allows values from -2PI to 2PI.
        /// </summary>
        public double Radians
        {
            get
            {
                return _rad;
            }
            set
            {
                if (value > 2 * Math.PI || value < -2 * Math.PI)
                {
                    _rad = value % (Math.PI * 2);
                    return;
                }
                _rad = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a new instance of the Angle class with the same angle as this object.
        /// </summary>
        /// <returns>Angle which has the same values</returns>
        public Angle Copy()
        {
            Angle newAngle = new Angle(_rad);
            return newAngle;
        }

        /// <summary>
        /// False for anything that is not an angle.
        /// Tests two angles to see if they have the same value.
        /// </summary>
        /// <param name="obj">An object to test.</param>
        /// <returns>Boolean, true if the angles have the same value.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Angle)) return false;
            Angle a = (Angle)obj;
            if (a.Radians == Radians) return true;
            return false;
        }

        /// <summary>
        /// Gets a hash code
        /// </summary>
        /// <returns>Int hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region ---------------------- Operators ---------------------------

        /// <summary>
        /// Returns a new angle object with an angle of Value in radians
        /// </summary>
        /// <param name="value">The double value indicating the angle</param>
        /// <returns>An Angle structure with the specified value</returns>
        public static explicit operator Angle(double value)
        {
            return new Angle(value);
        }

        /// <summary>
        /// Returns a double specifying the radian value of the angle
        /// </summary>
        /// <param name="value">The angle structure to determine the angle of</param>
        /// <returns>A Double with the angle in radians</returns>
        public static explicit operator double(Angle value)
        {
            return value.Radians;
        }

        /// <summary>
        /// Returns true if the two angles are equal to each other.
        /// </summary>
        /// <param name="a">An angle to compare</param>
        /// <param name="b">A second angle.</param>
        /// <returns>Boolean, true if they are equal.</returns>
        public static bool operator ==(Angle a, Angle b)
        {
            return (a.Radians == b.Radians);
        }

        /// <summary>
        /// Returns true if the two angles are equal to each other.
        /// </summary>
        /// <param name="a">An angle to compare</param>
        /// <param name="b">A second angle.</param>
        /// <returns>Boolean, true if they are equal.</returns>
        public static bool operator !=(Angle a, Angle b)
        {
            return (a.Radians != b.Radians);
        }

        /// <summary>
        /// Returns the sum of the two angles, cycling if greater than 2 pi.
        /// </summary>
        /// <param name="a">An angle to add</param>
        /// <param name="b">A second angle to add</param>
        /// <returns>A new Angle structure equal to the sum of the two angles</returns>
        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle(a.Radians + b.Radians);
        }

        /// <summary>
        /// Returns the difference of two angles.
        /// </summary>
        /// <param name="a">An angle to subtract from</param>
        /// <param name="b">The angle to subtract</param>
        /// <returns>A new angle structure with a sum equal to the two angles</returns>
        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle(a.Radians - b.Radians);
        }

        /// <summary>
        /// Divides angle A by angle B
        /// </summary>
        /// <param name="a">An angle to divide</param>
        /// <param name="b">An angle to divide into A</param>
        /// <returns>A new angle with the quotient of the division</returns>
        public static Angle operator /(Angle a, Angle b)
        {
            return new Angle(a.Radians / b.Radians);
        }

        /// <summary>
        /// Multiplies angle A by Angle B.
        /// </summary>
        /// <param name="a">An angle to multiply</param>
        /// <param name="b">A second angle to multiply.</param>
        /// <returns>A new angle with the product of the two angles.</returns>
        public static Angle operator *(Angle a, Angle b)
        {
            return new Angle(a.Radians * b.Radians);
        }

        #endregion

        #region -------------------- TRIG OVERLOADS -------------------------

        /// <summary>
        /// Returns the mathematical Cos of the angle specified
        /// </summary>
        /// <param name="value">The Angle to find the cosign of</param>
        /// <returns>Double, the cosign of the angle specified</returns>
        public static double Cos(Angle value)
        {
            return Math.Cos(value.Radians);
        }

        /// <summary>
        /// Returns the mathematical Sin of the angle specified
        /// </summary>
        /// <param name="value">The Angle to find the Sin of</param>
        /// <returns>Double, the Sin of the Angle</returns>
        public static double Sin(Angle value)
        {
            return Math.Sin(value.Radians);
        }

        /// <summary>
        /// Returns the mathematical Tan of the angle specified
        /// </summary>
        /// <param name="value">The Angle to find the Tan of</param>
        /// <returns>Double, the Tan of the Angle</returns>
        public static double Tan(Angle value)
        {
            return Math.Sin(value.Radians);
        }

        /// <summary>
        /// Returns the mathematical ATan of the value specified
        /// </summary>
        /// <param name="value">The Double to find the ATan of</param>
        /// <returns>Angle, the ATan of the Value specified</returns>
        public static Angle ATan(double value)
        {
            return new Angle(Math.Atan(value));
        }

        /// <summary>
        /// Returns the mathematical ACos of the value specified
        /// </summary>
        /// <param name="value">The Double to find the ACos of</param>
        /// <returns>Angle, the ACos of the Value specified</returns>
        public static Angle ACos(double value)
        {
            return new Angle(Math.Acos(value));
        }

        /// <summary>
        /// Returns the mathematical ASin of the value specified
        /// </summary>
        /// <param name="value">The Double to find the ASin of</param>
        /// <returns>Angle, the ASin of the Value specified</returns>
        public static Angle ASin(double value)
        {
            return new Angle(Math.Asin(value));
        }

        #endregion
    }
}