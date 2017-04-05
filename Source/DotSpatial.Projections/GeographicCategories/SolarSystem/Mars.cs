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
    /// This class contains predefined CoordinateSystems for Mars.
    /// </summary>
    public class Mars : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Deimos2000;
        public readonly ProjectionInfo Mars1979;
        public readonly ProjectionInfo Mars2000;
        public readonly ProjectionInfo Phobos2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Mars.
        /// </summary>
        public Mars()
        {
            Deimos2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104906).SetNames("", "GCS_Deimos_2000", "D_Deimos_2000"); // missing
            Mars1979 = ProjectionInfo.FromAuthorityCode("ESRI", 104904).SetNames("", "GCS_Mars_1979", "D_Mars_1979"); // missing
            Mars2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104905).SetNames("", "GCS_Mars_2000", "D_Mars_2000"); // missing
            Phobos2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104907).SetNames("", "GCS_Phobos_2000", "D_Phobos_2000"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591