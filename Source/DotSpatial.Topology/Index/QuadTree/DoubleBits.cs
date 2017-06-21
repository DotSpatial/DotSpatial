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
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Index.Quadtree
{
    /// <summary>
    /// DoubleBits manipulates Double numbers
    /// by using bit manipulation and bit-field extraction.
    /// For some operations (such as determining the exponent)
    /// this is more accurate than using mathematical operations
    /// (which suffer from round-off error).
    /// The algorithms and constants in this class
    /// apply only to IEEE-754 double-precision floating point format.
    /// </summary>
    public class DoubleBits
    {
        /// <summary>
        ///
        /// </summary>
        public const int EXPONENT_BIAS = 1023;

        private readonly double _x;
        private long _xBits;

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        public DoubleBits(double x)
        {
            _x = x;
            _xBits = BitConverter.DoubleToInt64Bits(x);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double Double
        {
            get
            {
                return BitConverter.Int64BitsToDouble(_xBits);
            }
        }

        /// <summary>
        /// Determines the exponent for the number.
        /// </summary>
        public virtual int BiasedExponent
        {
            get
            {
                int signExp = (int)(_xBits >> 52);
                int exp = signExp & 0x07ff;
                return exp;
            }
        }

        /// <summary>
        /// Determines the exponent for the number.
        /// </summary>
        public virtual int Exponent
        {
            get
            {
                return BiasedExponent - EXPONENT_BIAS;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double PowerOf2(int exp)
        {
            if (exp > 1023 || exp < -1022)
                throw new ArgumentException("Exponent out of bounds");
            long expBias = exp + EXPONENT_BIAS;
            long bits = expBias << 52;
            return BitConverter.Int64BitsToDouble(bits);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int GetExponent(double d)
        {
            DoubleBits db = new DoubleBits(d);
            return db.Exponent;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double TruncateToPowerOfTwo(double d)
        {
            DoubleBits db = new DoubleBits(d);
            db.ZeroLowerBits(52);
            return db.Double;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToBinaryString(double d)
        {
            DoubleBits db = new DoubleBits(d);
            return db.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static double MaximumCommonMantissa(double d1, double d2)
        {
            if (d1 == 0.0 || d2 == 0.0)
                return 0.0;

            DoubleBits db1 = new DoubleBits(d1);
            DoubleBits db2 = new DoubleBits(d2);

            if (db1.Exponent != db2.Exponent)
                return 0.0;

            int maxCommon = db1.NumCommonMantissaBits(db2);
            db1.ZeroLowerBits(64 - (12 + maxCommon));
            return db1.Double;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nBits"></param>
        public virtual void ZeroLowerBits(int nBits)
        {
            long invMask = (1L << nBits) - 1L;
            long mask = ~invMask;
            _xBits &= mask;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual int GetBit(int i)
        {
            long mask = (1L << i);
            return (_xBits & mask) != 0 ? 1 : 0;
        }

        /// <summary>
        /// This computes the number of common most-significant bits in the mantissa.
        /// It does not count the hidden bit, which is always 1.
        /// It does not determine whether the numbers have the same exponent - if they do
        /// not, the value computed by this function is meaningless.
        /// </summary>
        /// <param name="db"></param>
        /// <returns> The number of common most-significant mantissa bits.</returns>
        public virtual int NumCommonMantissaBits(DoubleBits db)
        {
            for (int i = 0; i < 52; i++)
            {
                if (GetBit(i) != db.GetBit(i))
                    return i;
            }
            return 52;
        }

        /// <summary>
        /// A representation of the Double bits formatted for easy readability.
        /// </summary>
        public override string ToString()
        {
            string numStr = HexConverter.ConvertAny2Any(_xBits.ToString(), 10, 2);

            // 64 zeroes!
            const string zero64 = "0000000000000000000000000000000000000000000000000000000000000000";
            string padStr = zero64 + numStr;
            string bitStr = padStr.Substring(padStr.Length - 64);
            string str = bitStr.Substring(0, 1) + "  "
                + bitStr.Substring(1, 12) + "(" + Exponent + ") "
                + bitStr.Substring(12)
                + " [ " + _x + " ]";
            return str;
        }
    }
}