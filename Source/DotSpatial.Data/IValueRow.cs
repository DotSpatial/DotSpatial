// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// A public interface definition for a single row of values that should be supported
    /// by any of the generic data row types.
    /// </summary>
    public interface IValueRow
    {
        /// <summary>
        /// Gets or sets the value in the position of column.
        /// </summary>
        /// <param name="cell">The 0 based integer column index to access on this row.</param>
        /// <returns>An object reference to the actual data value, which can be many types.</returns>
        double this[int cell] { get; set; }
    }
}