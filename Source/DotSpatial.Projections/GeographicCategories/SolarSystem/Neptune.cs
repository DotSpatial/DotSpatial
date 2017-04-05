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
    /// This class contains predefined CoordinateSystems for Neptune.
    /// </summary>
    public class Neptune : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Despina2000;
        public readonly ProjectionInfo Galatea2000;
        public readonly ProjectionInfo Larissa2000;
        public readonly ProjectionInfo Naiad2000;
        public readonly ProjectionInfo Neptune2000;
        public readonly ProjectionInfo Nereid2000;
        public readonly ProjectionInfo Proteus2000;
        public readonly ProjectionInfo Thalassa2000;
        public readonly ProjectionInfo Triton2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Neptune.
        /// </summary>
        public Neptune()
        {
            Despina2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104961).SetNames("", "GCS_Despina_2000", "D_Despina_2000"); // missing
            Galatea2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104962).SetNames("", "GCS_Galatea_2000", "D_Galatea_2000"); // missing
            Larissa2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104963).SetNames("", "GCS_Larissa_2000", "D_Larissa_2000"); // missing
            Naiad2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104964).SetNames("", "GCS_Naiad_2000", "D_Naiad_2000"); // missing
            Neptune2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104960).SetNames("", "GCS_Neptune_2000", "D_Neptune_2000"); // missing
            Nereid2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104965).SetNames("", "GCS_Nereid_2000", "D_Nereid_2000"); // missing
            Proteus2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104966).SetNames("", "GCS_Proteus_2000", "D_Proteus_2000"); // missing
            Thalassa2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104967).SetNames("", "GCS_Thalassa_2000", "D_Thalassa_2000"); // missing
            Triton2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104968).SetNames("", "GCS_Triton_2000", "D_Triton_2000"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591