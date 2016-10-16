// ********************************************************************************************************
// Product Name: DotSpatial.Common.dll
// Description:  A shared module for DotSpatial libraries
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Darrel Brown. Created 9/10/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|----------------------------------------------------------------------
//
// ********************************************************************************************************

namespace DotSpatial.Serialization
{
    /// <summary>
    /// Base class for formatters responsible for converting objects to and from string values.
    /// </summary>
    public abstract class SerializationFormatter
    {
        /// <summary>
        /// Converts an object to a string value.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <returns>The string representation of the given object.</returns>
        public abstract string ToString(object value);

        /// <summary>
        /// Converts a string representation of an object back into the original object form.
        /// </summary>
        /// <param name="value">The string representation of an object.</param>
        /// <returns>The object represented by the given string.</returns>
        public abstract object FromString(string value);
    }
}