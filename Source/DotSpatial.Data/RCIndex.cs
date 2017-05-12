// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/17/2008 6:42:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// A Row, Column indexer for some return types.
    /// </summary>
    public struct RcIndex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RcIndex"/> struct.
        /// </summary>
        /// <param name="row">The integer row index</param>
        /// <param name="column">The integer column index</param>
        public RcIndex(int row, int column)
            : this()
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Gets a new RcIndex that is defined as empty when both indices are int.
        /// </summary>
        public static RcIndex Empty => new RcIndex(int.MinValue, int.MinValue);

        /// <summary>
        /// Gets the zero based integer column index.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Gets the zero based integer row index.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(RcIndex a, RcIndex b)
        {
            return Equals(a, b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(RcIndex a, RcIndex b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Gets a boolean that is true if either row or column index has no value
        /// </summary>
        /// <returns>Boolean, true if either row or column has no value</returns>
        public bool IsEmpty()
        {
            return Row == int.MinValue && Column == int.MinValue;
        }

        /// <summary>
        /// Checks whether this and other are equal.
        /// </summary>
        /// <param name="other">The other RcIndex.</param>
        /// <returns>True if both are equal.</returns>
        public bool Equals(RcIndex other)
        {
            return Column == other.Column && Row == other.Row;
        }

        /// <summary>
        /// Checks whether this and other are equal.
        /// </summary>
        /// <param name="obj">The other RcIndex.</param>
        /// <returns>True if both are equal.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            return obj is RcIndex && Equals((RcIndex)obj);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return (Column * 397) ^ Row;
        }
    }
}