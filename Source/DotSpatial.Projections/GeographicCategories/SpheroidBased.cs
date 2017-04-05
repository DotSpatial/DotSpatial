// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 2:20:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for SpheroidBased.
    /// </summary>
    public class SpheroidBased : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Airy1830;
        public readonly ProjectionInfo Airymodified;
        public readonly ProjectionInfo AustralianNational;
        public readonly ProjectionInfo Authalicsphere;
        public readonly ProjectionInfo AuthalicsphereARCINFO;
        public readonly ProjectionInfo AverageTerrestrialSystem1977;
        public readonly ProjectionInfo Bessel1841;
        public readonly ProjectionInfo Besselmodified;
        public readonly ProjectionInfo BesselNamibia;
        public readonly ProjectionInfo Clarke1858;
        public readonly ProjectionInfo Clarke1866;
        public readonly ProjectionInfo Clarke1866AuthalicSphere;
        public readonly ProjectionInfo Clarke1866Michigan;
        public readonly ProjectionInfo Clarke1880;
        public readonly ProjectionInfo Clarke1880Arc;
        public readonly ProjectionInfo Clarke1880Benoit;
        public readonly ProjectionInfo Clarke1880IGN;
        public readonly ProjectionInfo Clarke1880RGS;
        public readonly ProjectionInfo Clarke1880SGA;
        public readonly ProjectionInfo Everest1830;
        public readonly ProjectionInfo EverestDefinition1937;
        public readonly ProjectionInfo EverestDefinition1962;
        public readonly ProjectionInfo EverestDefinition1967;
        public readonly ProjectionInfo EverestDefinition1975;
        public readonly ProjectionInfo EverestModified;
        public readonly ProjectionInfo Everestmodified1969;
        public readonly ProjectionInfo Fischer1960;
        public readonly ProjectionInfo Fischer1968;
        public readonly ProjectionInfo Fischermodified;
        public readonly ProjectionInfo GEMgravitypotentialmodel;
        public readonly ProjectionInfo GRS1967;
        public readonly ProjectionInfo GRS1980;
        public readonly ProjectionInfo GRS1980AuthalicSphere;
        public readonly ProjectionInfo Helmert1906;
        public readonly ProjectionInfo Hough1960;
        public readonly ProjectionInfo Hughes1980;
        public readonly ProjectionInfo IndonesianNational;
        public readonly ProjectionInfo International1924;
        public readonly ProjectionInfo International1924AuthalicSphere;
        public readonly ProjectionInfo Krasovsky1940;
        public readonly ProjectionInfo OSU1986geoidalmodel;
        public readonly ProjectionInfo OSU1991geoidalmodel;
        public readonly ProjectionInfo Plessis1817;
        public readonly ProjectionInfo SphereEMEP;
        public readonly ProjectionInfo Struve1860;
        public readonly ProjectionInfo Transitpreciseephemeris;
        public readonly ProjectionInfo Walbeck;
        public readonly ProjectionInfo WarOffice;
        public readonly ProjectionInfo WGS1966;
        public readonly ProjectionInfo WGS1984MajorAuxiliarySphere;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SpheroidBased.
        /// </summary>
        public SpheroidBased()
        {
            Airy1830 = ProjectionInfo.FromEpsgCode(4001).SetNames("", "GCS_Airy_1830", "D_Airy_1830");
            Airymodified = ProjectionInfo.FromEpsgCode(4002).SetNames("", "GCS_Airy_Modified", "D_Airy_Modified");
            AustralianNational = ProjectionInfo.FromEpsgCode(4003).SetNames("", "GCS_Australian", "D_Australian");
            Authalicsphere = ProjectionInfo.FromEpsgCode(4035).SetNames("", "GCS_Sphere", "D_Sphere");
            AuthalicsphereARCINFO = ProjectionInfo.FromAuthorityCode("ESRI", 37008).SetNames("", "GCS_Sphere_ARC_INFO", "D_Sphere_ARC_INFO");
            AverageTerrestrialSystem1977 = ProjectionInfo.FromEpsgCode(4122).SetNames("", "GCS_ATS_1977", "D_ATS_1977");
            Bessel1841 = ProjectionInfo.FromEpsgCode(4004).SetNames("", "GCS_Bessel_1841", "D_Bessel_1841");
            Besselmodified = ProjectionInfo.FromEpsgCode(4005).SetNames("", "GCS_Bessel_Modified", "D_Bessel_Modified");
            BesselNamibia = ProjectionInfo.FromEpsgCode(4006).SetNames("", "GCS_Bessel_Namibia", "D_Bessel_Namibia");
            Clarke1858 = ProjectionInfo.FromEpsgCode(4007).SetNames("", "GCS_Clarke_1858", "D_Clarke_1858");
            Clarke1866 = ProjectionInfo.FromEpsgCode(4008).SetNames("", "GCS_Clarke_1866", "D_Clarke_1866");
            Clarke1866AuthalicSphere = ProjectionInfo.FromEpsgCode(4052).SetNames("", "GCS_Sphere_Clarke_1866_Authalic", "D_Sphere_Clarke_1866_Authalic");
            Clarke1866Michigan = ProjectionInfo.FromEpsgCode(4009).SetNames("", "GCS_Clarke_1866_Michigan", "D_Clarke_1866_Michigan");
            Clarke1880 = ProjectionInfo.FromEpsgCode(4034).SetNames("", "GCS_Clarke_1880", "D_Clarke_1880");
            Clarke1880Arc = ProjectionInfo.FromEpsgCode(4013).SetNames("", "GCS_Clarke_1880_Arc", "D_Clarke_1880_Arc");
            Clarke1880Benoit = ProjectionInfo.FromEpsgCode(4010).SetNames("", "GCS_Clarke_1880_Benoit", "D_Clarke_1880_Benoit");
            Clarke1880IGN = ProjectionInfo.FromEpsgCode(4011).SetNames("", "GCS_Clarke_1880_IGN", "D_Clarke_1880_IGN");
            Clarke1880RGS = ProjectionInfo.FromEpsgCode(4012).SetNames("", "GCS_Clarke_1880_RGS", "D_Clarke_1880_RGS");
            Clarke1880SGA = ProjectionInfo.FromEpsgCode(4014).SetNames("", "GCS_Clarke_1880_SGA", "D_Clarke_1880_SGA");
            Everest1830 = ProjectionInfo.FromEpsgCode(4042).SetNames("", "GCS_Everest_1830", "D_Everest_1830");
            EverestDefinition1937 = ProjectionInfo.FromEpsgCode(4015).SetNames("", "GCS_Everest_Adj_1937", "D_Everest_Adj_1937");
            EverestDefinition1962 = ProjectionInfo.FromEpsgCode(4044).SetNames("", "GCS_Everest_def_1962", "D_Everest_Def_1962");
            EverestDefinition1967 = ProjectionInfo.FromEpsgCode(4016).SetNames("", "GCS_Everest_def_1967", "D_Everest_Def_1967");
            EverestDefinition1975 = ProjectionInfo.FromEpsgCode(4045).SetNames("", "GCS_Everest_def_1975", "D_Everest_Def_1975");
            EverestModified = ProjectionInfo.FromEpsgCode(4018).SetNames("", "GCS_Everest_Modified", "D_Everest_Modified");
            Everestmodified1969 = ProjectionInfo.FromAuthorityCode("ESRI", 37006).SetNames("", "GCS_Everest_Modified_1969", "D_Everest_Modified_1969");
            Fischer1960 = ProjectionInfo.FromAuthorityCode("ESRI", 37002).SetNames("", "GCS_Fischer_1960", "D_Fischer_1960");
            Fischer1968 = ProjectionInfo.FromAuthorityCode("ESRI", 37003).SetNames("", "GCS_Fischer_1968", "D_Fischer_1968");
            Fischermodified = ProjectionInfo.FromAuthorityCode("ESRI", 37004).SetNames("", "GCS_Fischer_Modified", "D_Fischer_Modified");
            GEMgravitypotentialmodel = ProjectionInfo.FromEpsgCode(4031).SetNames("", "GCS_GEM_10C", "D_GEM_10C");
            GRS1967 = ProjectionInfo.FromEpsgCode(4036).SetNames("", "GCS_GRS_1967", "D_GRS_1967");
            GRS1980 = ProjectionInfo.FromEpsgCode(4019).SetNames("", "GCS_GRS_1980", "D_GRS_1980");
            GRS1980AuthalicSphere = ProjectionInfo.FromEpsgCode(4047).SetNames("", "GCS_Sphere_GRS_1980_Authalic", "D_Sphere_GRS_1980_Authalic");
            Helmert1906 = ProjectionInfo.FromEpsgCode(4020).SetNames("", "GCS_Helmert_1906", "D_Helmert_1906");
            Hough1960 = ProjectionInfo.FromAuthorityCode("ESRI", 37005).SetNames("", "GCS_Hough_1960", "D_Hough_1960");
            Hughes1980 = ProjectionInfo.FromEpsgCode(4054).SetNames("", "GCS_Hughes_1980", "D_Hughes_1980");
            IndonesianNational = ProjectionInfo.FromEpsgCode(4021).SetNames("", "GCS_Indonesian", "D_Indonesian");
            International1924 = ProjectionInfo.FromEpsgCode(4022).SetNames("", "GCS_International_1924", "D_International_1924");
            International1924AuthalicSphere = ProjectionInfo.FromEpsgCode(4053).SetNames("", "GCS_Sphere_International_1924_Authalic", "D_Sphere_International_1924_Authalic");
            Krasovsky1940 = ProjectionInfo.FromEpsgCode(4024).SetNames("", "GCS_Krasovsky_1940", "D_Krasovsky_1940");
            OSU1986geoidalmodel = ProjectionInfo.FromEpsgCode(4032).SetNames("", "GCS_OSU_86F", "D_OSU_86F");
            OSU1991geoidalmodel = ProjectionInfo.FromEpsgCode(4033).SetNames("", "GCS_OSU_91A", "D_OSU_91A");
            Plessis1817 = ProjectionInfo.FromEpsgCode(4027).SetNames("", "GCS_Plessis_1817", "D_Plessis_1817");
            SphereEMEP = ProjectionInfo.FromAuthorityCode("ESRI", 104128).SetNames("", "GCS_Sphere_EMEP", "D_Sphere_EMEP"); // missing
            Struve1860 = ProjectionInfo.FromEpsgCode(4028).SetNames("", "GCS_Struve_1860", "D_Struve_1860");
            Transitpreciseephemeris = ProjectionInfo.FromEpsgCode(4025).SetNames("", "GCS_NWL_9D", "D_NWL_9D");
            Walbeck = ProjectionInfo.FromAuthorityCode("ESRI", 37007).SetNames("", "GCS_Walbeck", "D_Walbeck");
            WarOffice = ProjectionInfo.FromEpsgCode(4029).SetNames("", "GCS_War_Office", "D_War_Office");
            WGS1966 = ProjectionInfo.FromEpsgCode(4760).SetNames("", "GCS_WGS_1966", "D_WGS_1966");
            WGS1984MajorAuxiliarySphere = ProjectionInfo.FromAuthorityCode("EPSG", 104199).SetNames("", "GCS_WGS_1984_Major_Auxiliary_Sphere", "D_WGS_1984_Major_Auxiliary_Sphere"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591