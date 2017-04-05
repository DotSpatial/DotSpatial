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
    /// This class contains predefined CoordinateSystems for Earth.
    /// </summary>
    public class Earth : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Moon2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Earth.
        /// </summary>
        public Earth()
        {
            Moon2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104903).SetNames("", "GCS_Moon_2000", "D_Moon_2000"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591