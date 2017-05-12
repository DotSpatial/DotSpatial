// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/3/2008 6:38:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Globalization;

namespace DotSpatial.Data
{
    /// <summary>
    /// CulturePreferences
    /// </summary>
    public static class CulturePreferences
    {
        /// <summary>
        /// Gets or sets the CultureInformation. This culture information is useful for things like Number Formatting.
        /// This defaults to CurrentCulture, but can be specified through preferences or whatever.
        /// </summary>
        public static CultureInfo CultureInformation { get; set; } = CultureInfo.CurrentCulture;
    }
}