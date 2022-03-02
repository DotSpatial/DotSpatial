// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 10:26:47 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// Represents object that can save and parse itself to\from ESRI string.
    /// </summary>
    public interface IEsriString
    {
        #region Methods

        /// <summary>
        /// Writes the value in the format that it would appear in within a prj file
        /// </summary>
        /// <returns>The a nested portion of the total esri string.</returns>
        string ToEsriString();

        /// <summary>
        /// This reads the string and attempts to parse the relavent values.
        /// </summary>
        /// <param name="esriString">The string to read</param>
        void ParseEsriString(string esriString);

        #endregion
    }
}