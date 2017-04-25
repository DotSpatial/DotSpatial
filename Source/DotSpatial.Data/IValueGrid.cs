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
    /// An interface specification for any of the multiple types of IDataBlock.
    /// </summary>
    public interface IValueGrid
    {
        /// <summary>
        /// Gets or sets a value at the 0 row, 0 column index.
        /// </summary>
        /// <param name="row">The 0 based vertical row index from the top</param>
        /// <param name="column">The 0 based horizontal column index from the left</param>
        /// <returns>An object reference to the actual value in the data member.</returns>
        double this[int row, int column] { get; set; }

        /// <summary>
        /// Boolean, gets or sets the flag indicating if the values have been changed
        /// since the last time this flag was set to false.
        /// </summary>
        bool Updated { get; set; }
    }
}