// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:16:32 PM
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
    /// This class contains predefined CoordinateSystems for Saturn.
    /// </summary>
    public class Saturn : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Atlas2000;
        public readonly ProjectionInfo Calypso2000;
        public readonly ProjectionInfo Dione2000;
        public readonly ProjectionInfo Enceladus2000;
        public readonly ProjectionInfo Epimetheus2000;
        public readonly ProjectionInfo Helene2000;
        public readonly ProjectionInfo Hyperion2000;
        public readonly ProjectionInfo Iapetus2000;
        public readonly ProjectionInfo Janus2000;
        public readonly ProjectionInfo Mimas2000;
        public readonly ProjectionInfo Pan2000;
        public readonly ProjectionInfo Pandora2000;
        public readonly ProjectionInfo Phoebe2000;
        public readonly ProjectionInfo Prometheus2000;
        public readonly ProjectionInfo Rhea2000;
        public readonly ProjectionInfo Saturn2000;
        public readonly ProjectionInfo Telesto2000;
        public readonly ProjectionInfo Tethys2000;
        public readonly ProjectionInfo Titan2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Saturn.
        /// </summary>
        public Saturn()
        {
            Atlas2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104926).SetNames("", "GCS_Atlas_2000", "D_Atlas_2000"); // missing
            Calypso2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104927).SetNames("", "GCS_Calypso_2000", "D_Calypso_2000"); // missing
            Dione2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104928).SetNames("", "GCS_Dione_2000", "D_Dione_2000"); // missing
            Enceladus2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104929).SetNames("", "GCS_Enceladus_2000", "D_Enceladus_2000"); // missing
            Epimetheus2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104930).SetNames("", "GCS_Epimetheus_2000", "D_Epimetheus_2000"); // missing
            Helene2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104931).SetNames("", "GCS_Helene_2000", "D_Helene_2000"); // missing
            Hyperion2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104932).SetNames("", "GCS_Hyperion_2000", "D_Hyperion_2000"); // missing
            Iapetus2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104933).SetNames("", "GCS_Iapetus_2000", "D_Iapetus_2000"); // missing
            Janus2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104934).SetNames("", "GCS_Janus_2000", "D_Janus_2000"); // missing
            Mimas2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104935).SetNames("", "GCS_Mimas_2000", "D_Mimas_2000"); // missing
            Pan2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104936).SetNames("", "GCS_Pan_2000", "D_Pan_2000"); // missing
            Pandora2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104937).SetNames("", "GCS_Pandora_2000", "D_Pandora_2000"); // missing
            Phoebe2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104938).SetNames("", "GCS_Phoebe_2000", "D_Phoebe_2000"); // missing
            Prometheus2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104939).SetNames("", "GCS_Prometheus_2000", "D_Prometheus_2000"); // missing
            Rhea2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104940).SetNames("", "GCS_Rhea_2000", "D_Rhea_2000"); // missing
            Saturn2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104925).SetNames("", "GCS_Saturn_2000", "D_Saturn_2000"); // missing
            Telesto2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104941).SetNames("", "GCS_Telesto_2000", "D_Telesto_2000"); // missing
            Tethys2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104942).SetNames("", "GCS_Tethys_2000", "D_Tethys_2000"); // missing
            Titan2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104943).SetNames("", "GCS_Titan_2000", "D_Titan_2000"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591