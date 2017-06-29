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

namespace DotSpatial.Topology.Precision
{
    /// <summary>
    /// Determines the maximum number of common most-significant
    /// bits in the mantissa of one or numbers.
    /// Can be used to compute the double-precision number which
    /// is represented by the common bits.
    /// If there are no common bits, the number computed is 0.0.
    /// </summary>
    public class CommonBits
    {
        private long _commonBits;
        private int _commonMantissaBitsCount = 53;
        private long _commonSignExp;
        private bool _isFirst = true;

        /// <summary>
        ///
        /// </summary>
        public virtual double Common
        {
            get
            {
                return BitConverter.Int64BitsToDouble(_commonBits);
            }
        }

        /// <summary>
        /// Computes the bit pattern for the sign and exponent of a
        /// double-precision number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The bit pattern for the sign and exponent.</returns>
        public static long SignExpBits(long num)
        {
            return num >> 52;
        }

        /// <summary>
        /// This computes the number of common most-significant bits in the mantissas
        /// of two double-precision numbers.
        /// It does not count the hidden bit, which is always 1.
        /// It does not determine whether the numbers have the same exponent - if they do
        /// not, the value computed by this function is meaningless.
        /// </summary>
        /// <param name="num1"></param>
        /// /// <param name="num2"></param>
        /// <returns>The number of common most-significant mantissa bits.</returns>
        public static int NumCommonMostSigMantissaBits(long num1, long num2)
        {
            int count = 0;
            for (int i = 52; i >= 0; i--)
            {
                if (GetBit(num1, i) != GetBit(num2, i))
                    return count;
                count++;
            }
            return 52;
        }

        /// <summary>
        /// Zeroes the lower n bits of a bitstring.
        /// </summary>
        /// <param name="bits">The bitstring to alter.</param>
        /// <param name="nBits">the number of bits to zero.</param>
        /// <returns>The zeroed bitstring.</returns>
        public static long ZeroLowerBits(long bits, int nBits)
        {
            long invMask = (1L << nBits) - 1L;
            long mask = ~invMask;
            long zeroed = bits & mask;
            return zeroed;
        }

        /// <summary>
        /// Extracts the i'th bit of a bitstring.
        /// </summary>
        /// <param name="bits">The bitstring to extract from.</param>
        /// <param name="i">The bit to extract.</param>
        /// <returns>The value of the extracted bit.</returns>
        public static int GetBit(long bits, int i)
        {
            long mask = (1L << i);
            return (bits & mask) != 0 ? 1 : 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="num"></param>
        public virtual void Add(double num)
        {
            long numBits = BitConverter.DoubleToInt64Bits(num);
            if (_isFirst)
            {
                _commonBits = numBits;
                _commonSignExp = SignExpBits(_commonBits);
                _isFirst = false;
                return;
            }

            long numSignExp = SignExpBits(numBits);
            if (numSignExp != _commonSignExp)
            {
                _commonBits = 0;
                return;
            }
            _commonMantissaBitsCount = NumCommonMostSigMantissaBits(_commonBits, numBits);
            _commonBits = ZeroLowerBits(_commonBits, 64 - (12 + _commonMantissaBitsCount));
        }

        /// <summary>
        /// A representation of the Double bits formatted for easy readability
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public virtual string ToString(long bits)
        {
            double x = BitConverter.Int64BitsToDouble(bits);
            string numStr = HexConverter.ConvertAny2Any(bits.ToString(), 10, 2);
            string padStr = "0000000000000000000000000000000000000000000000000000000000000000" + numStr;
            string bitStr = padStr.Substring(padStr.Length - 64);
            string str = bitStr.Substring(0, 1) + "  " + bitStr.Substring(1, 12) + "(exp) "
                         + bitStr.Substring(12) + " [ " + x + " ]";
            return str;
        }
    }
}