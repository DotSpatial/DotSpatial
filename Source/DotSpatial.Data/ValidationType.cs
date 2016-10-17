// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/29/2008 5:57:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    public enum ValidationType
    {
        /// <summary>
        /// No validation will be performed
        /// </summary>
        None,
        /// <summary>
        /// Any string value, including null will be accepted
        /// </summary>
        String,
        /// <summary>
        /// Only values that can be parsed to byte values will be accepted
        /// </summary>
        Byte,
        /// <summary>
        /// Only values that can be parsed to short values will be accepted
        /// </summary>
        Short,
        /// <summary>
        /// Only values that can be parsed to integer values will be accepted
        /// </summary>
        Integer,
        /// <summary>
        /// Only values that can be parsed to float values will be accepted
        /// </summary>
        Float,
        /// <summary>
        /// Only values that can be parsed to double values will be accepted
        /// </summary>
        Double,
        /// <summary>
        /// Only values that can be parsed to positive short values will be accepted
        /// </summary>
        PositiveShort,
        /// <summary>
        /// Only values that can be parsed to positive integer values will be accepted
        /// </summary>
        PositiveInteger,
        /// <summary>
        /// Only values that can be parsed to positive float values will be accepted
        /// </summary>
        PositiveFloat,
        /// <summary>
        /// Only values that can be parsed to positive double values will be accepted
        /// </summary>
        PositiveDouble,
    }
}