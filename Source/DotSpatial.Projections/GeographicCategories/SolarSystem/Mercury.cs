// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories.SolarSystem
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Mercury.
    /// </summary>
    public class Mercury : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Mercury2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Mercury.
        /// </summary>
        public Mercury()
        {
            Mercury2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104900).SetNames("", "GCS_Mercury_2000", "D_Mercury_2000"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591