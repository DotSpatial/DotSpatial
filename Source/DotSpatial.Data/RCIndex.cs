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
        /// The zero based integer column index
        /// </summary>
        public int Column;

        /// <summary>
        /// The zero based integer row index
        /// </summary>
        public int Row;

        /// <summary>
        /// Creates a new RcIndex structure with the specified coordinates
        /// </summary>
        /// <param name="row">The integer row index</param>
        /// <param name="column">The integer column index</param>
        public RcIndex(int row, int column)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Returns a new RcIndex that is defined as empty when both indices are int.
        /// </summary>
        public static RcIndex Empty
        {
            get { return new RcIndex(int.MinValue, int.MinValue); }
        }

        /// <summary>
        /// Gets a boolean that is true if either row or column index has no value
        /// </summary>
        /// <returns>Boolean, true if either row or column has no value</returns>
        public bool IsEmpty()
        {
            return (Row == int.MinValue && Column == int.MinValue);
        }

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
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Column == b.Column && a.Row == b.Row;
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
    }
}