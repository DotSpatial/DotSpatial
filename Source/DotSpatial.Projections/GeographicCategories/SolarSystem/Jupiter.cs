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
    /// This class contains predefined CoordinateSystems for Jupiter.
    /// </summary>
    public class Jupiter : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Adrastea2000;
        public readonly ProjectionInfo Amalthea2000;
        public readonly ProjectionInfo Ananke2000;
        public readonly ProjectionInfo Callisto2000;
        public readonly ProjectionInfo Carme2000;
        public readonly ProjectionInfo Elara2000;
        public readonly ProjectionInfo Europa2000;
        public readonly ProjectionInfo Ganymede2000;
        public readonly ProjectionInfo Himalia2000;
        public readonly ProjectionInfo Io2000;
        public readonly ProjectionInfo Jupiter2000;
        public readonly ProjectionInfo Leda2000;
        public readonly ProjectionInfo Lysithea2000;
        public readonly ProjectionInfo Metis2000;
        public readonly ProjectionInfo Pasiphae2000;
        public readonly ProjectionInfo Sinope2000;
        public readonly ProjectionInfo Thebe2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Jupiter.
        /// </summary>
        public Jupiter()
        {
            Adrastea2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104909).SetNames("", "GCS_Adrastea_2000", "D_Adrastea_2000"); // missing
            Amalthea2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104910).SetNames("", "GCS_Amalthea_2000", "D_Amalthea_2000"); // missing
            Ananke2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104911).SetNames("", "GCS_Ananke_2000", "D_Ananke_2000"); // missing
            Callisto2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104912).SetNames("", "GCS_Callisto_2000", "D_Callisto_2000"); // missing
            Carme2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104913).SetNames("", "GCS_Carme_2000", "D_Carme_2000"); // missing
            Elara2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104914).SetNames("", "GCS_Elara_2000", "D_Elara_2000"); // missing
            Europa2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104915).SetNames("", "GCS_Europa_2000", "D_Europa_2000"); // missing
            Ganymede2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104916).SetNames("", "GCS_Ganymede_2000", "D_Ganymede_2000"); // missing
            Himalia2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104917).SetNames("", "GCS_Himalia_2000", "D_Himalia_2000"); // missing
            Io2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104918).SetNames("", "GCS_Io_2000", "D_Io_2000"); // missing
            Jupiter2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104908).SetNames("", "GCS_Jupiter_2000", "D_Jupiter_2000"); // missing
            Leda2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104919).SetNames("", "GCS_Leda_2000", "D_Leda_2000"); // missing
            Lysithea2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104920).SetNames("", "GCS_Lysithea_2000", "D_Lysithea_2000"); // missing
            Metis2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104921).SetNames("", "GCS_Metis_2000", "D_Metis_2000"); // missing
            Pasiphae2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104922).SetNames("", "GCS_Pasiphae_2000", "D_Pasiphae_2000"); // missing
            Sinope2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104923).SetNames("", "GCS_Sinope_2000", "D_Sinope_2000"); // missing
            Thebe2000 = ProjectionInfo.FromAuthorityCode("ESRI", 104924).SetNames("", "GCS_Thebe_2000", "D_Thebe_2000"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591