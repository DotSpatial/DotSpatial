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

namespace DotSpatial.Projections.ProjectedCategories.UTM
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for NorthAmerica.
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo ATS1977UTMZone19N;
        public readonly ProjectionInfo ATS1977UTMZone20N;
        public readonly ProjectionInfo GrandCayman1959UTMZone17N;
        public readonly ProjectionInfo Greenland1996UTMZone18N;
        public readonly ProjectionInfo Greenland1996UTMZone19N;
        public readonly ProjectionInfo Greenland1996UTMZone20N;
        public readonly ProjectionInfo Greenland1996UTMZone21N;
        public readonly ProjectionInfo Greenland1996UTMZone22N;
        public readonly ProjectionInfo Greenland1996UTMZone23N;
        public readonly ProjectionInfo Greenland1996UTMZone24N;
        public readonly ProjectionInfo Greenland1996UTMZone25N;
        public readonly ProjectionInfo Greenland1996UTMZone26N;
        public readonly ProjectionInfo Greenland1996UTMZone27N;
        public readonly ProjectionInfo Greenland1996UTMZone28N;
        public readonly ProjectionInfo Greenland1996UTMZone29N;
        public readonly ProjectionInfo JAD2001UTMZone17N;
        public readonly ProjectionInfo JAD2001UTMZone18N;
        public readonly ProjectionInfo LittleCayman1961UTMZone17N;
        public readonly ProjectionInfo NAD1927BLMZone14NUSFeet;
        public readonly ProjectionInfo NAD1927BLMZone15NUSFeet;
        public readonly ProjectionInfo NAD1927BLMZone16NUSFeet;
        public readonly ProjectionInfo NAD1927BLMZone17NUSFeet;
        public readonly ProjectionInfo NAD1983BLMZone14NUSFeet;
        public readonly ProjectionInfo NAD1983BLMZone15NUSFeet;
        public readonly ProjectionInfo NAD1983BLMZone16NUSFeet;
        public readonly ProjectionInfo NAD1983BLMZone17NUSFeet;
        public readonly ProjectionInfo NAD1983CORS96UTMZone10N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone11N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone12N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone13N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone14N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone15N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone16N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone17N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone18N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone19N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone1N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone2N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone3N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone4N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone59N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone5N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone60N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone6N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone7N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone8N;
        public readonly ProjectionInfo NAD1983CORS96UTMZone9N;
        public readonly ProjectionInfo NAD1983HARNUTMZone10N;
        public readonly ProjectionInfo NAD1983HARNUTMZone11N;
        public readonly ProjectionInfo NAD1983HARNUTMZone12N;
        public readonly ProjectionInfo NAD1983HARNUTMZone13N;
        public readonly ProjectionInfo NAD1983HARNUTMZone14N;
        public readonly ProjectionInfo NAD1983HARNUTMZone15N;
        public readonly ProjectionInfo NAD1983HARNUTMZone16N;
        public readonly ProjectionInfo NAD1983HARNUTMZone17N;
        public readonly ProjectionInfo NAD1983HARNUTMZone18N;
        public readonly ProjectionInfo NAD1983HARNUTMZone19N;
        public readonly ProjectionInfo NAD1983HARNUTMZone2S;
        public readonly ProjectionInfo NAD1983HARNUTMZone4N;
        public readonly ProjectionInfo NAD1983HARNUTMZone5N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone10N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone11N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone12N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone13N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone14N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone15N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone16N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone17N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone18N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone19N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone1N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone2N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone3N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone4N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone59N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone5N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone60N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone6N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone7N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone8N;
        public readonly ProjectionInfo NAD1983NSRS2007UTMZone9N;
        public readonly ProjectionInfo NAD1983PACP00UTMZone2S;
        public readonly ProjectionInfo NAD1983PACP00UTMZone4N;
        public readonly ProjectionInfo NAD1983PACP00UTMZone5N;
        public readonly ProjectionInfo PuertoRicoUTMZone20N;
        public readonly ProjectionInfo Qornoq1927UTMZone22N;
        public readonly ProjectionInfo Qornoq1927UTMZone23N;
        public readonly ProjectionInfo SaintPierreetMiquelon1950UTMZone21N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica.
        /// </summary>
        public NorthAmerica()
        {
            ATS1977UTMZone19N = ProjectionInfo.FromEpsgCode(2219).SetNames("ATS_1977_UTM_Zone_19N", "GCS_ATS_1977", "D_ATS_1977");
            ATS1977UTMZone20N = ProjectionInfo.FromEpsgCode(2220).SetNames("ATS_1977_UTM_Zone_20N", "GCS_ATS_1977", "D_ATS_1977");
            GrandCayman1959UTMZone17N = ProjectionInfo.FromEpsgCode(3356).SetNames("Grand_Cayman_1959_UTM_Zone_17N", "GCS_Grand_Cayman_1959", "D_Grand_Cayman_1959");
            Greenland1996UTMZone18N = ProjectionInfo.FromEpsgCode(3178).SetNames("Greenland_1996_UTM_Zone_18N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone19N = ProjectionInfo.FromEpsgCode(3179).SetNames("Greenland_1996_UTM_Zone_19N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone20N = ProjectionInfo.FromEpsgCode(3180).SetNames("Greenland_1996_UTM_Zone_20N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone21N = ProjectionInfo.FromEpsgCode(3181).SetNames("Greenland_1996_UTM_Zone_21N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone22N = ProjectionInfo.FromEpsgCode(3182).SetNames("Greenland_1996_UTM_Zone_22N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone23N = ProjectionInfo.FromEpsgCode(3183).SetNames("Greenland_1996_UTM_Zone_23N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone24N = ProjectionInfo.FromEpsgCode(3184).SetNames("Greenland_1996_UTM_Zone_24N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone25N = ProjectionInfo.FromEpsgCode(3185).SetNames("Greenland_1996_UTM_Zone_25N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone26N = ProjectionInfo.FromEpsgCode(3186).SetNames("Greenland_1996_UTM_Zone_26N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone27N = ProjectionInfo.FromEpsgCode(3187).SetNames("Greenland_1996_UTM_Zone_27N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone28N = ProjectionInfo.FromEpsgCode(3188).SetNames("Greenland_1996_UTM_Zone_28N", "GCS_Greenland_1996", "D_Greenland_1996");
            Greenland1996UTMZone29N = ProjectionInfo.FromEpsgCode(3189).SetNames("Greenland_1996_UTM_Zone_29N", "GCS_Greenland_1996", "D_Greenland_1996");
            JAD2001UTMZone17N = ProjectionInfo.FromEpsgCode(3449).SetNames("JAD_2001_UTM_Zone_17N", "GCS_JAD_2001", "D_Jamaica_2001");
            JAD2001UTMZone18N = ProjectionInfo.FromEpsgCode(3450).SetNames("JAD_2001_UTM_Zone_18N", "GCS_JAD_2001", "D_Jamaica_2001");
            LittleCayman1961UTMZone17N = ProjectionInfo.FromEpsgCode(3357).SetNames("Little_Cayman_1961_UTM_Zone_17N", "GCS_Little_Cayman_1961", "D_Little_Cayman_1961");
            NAD1927BLMZone14NUSFeet = ProjectionInfo.FromEpsgCode(32064).SetNames("NAD_1927_BLM_Zone_14N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927BLMZone15NUSFeet = ProjectionInfo.FromEpsgCode(32065).SetNames("NAD_1927_BLM_Zone_15N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927BLMZone16NUSFeet = ProjectionInfo.FromEpsgCode(32066).SetNames("NAD_1927_BLM_Zone_16N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927BLMZone17NUSFeet = ProjectionInfo.FromEpsgCode(32067).SetNames("NAD_1927_BLM_Zone_17N", "GCS_North_American_1927", "D_North_American_1927");
            NAD1983BLMZone14NUSFeet = ProjectionInfo.FromEpsgCode(32164).SetNames("NAD_1983_BLM_Zone_14N_ftUS", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983BLMZone15NUSFeet = ProjectionInfo.FromEpsgCode(32165).SetNames("NAD_1983_BLM_Zone_15N_ftUS", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983BLMZone16NUSFeet = ProjectionInfo.FromEpsgCode(32166).SetNames("NAD_1983_BLM_Zone_16N_ftUS", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983BLMZone17NUSFeet = ProjectionInfo.FromEpsgCode(32167).SetNames("NAD_1983_BLM_Zone_17N_ftUS", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983CORS96UTMZone10N = ProjectionInfo.FromAuthorityCode("ESRI", 102410).SetNames("NAD_1983_CORS96_UTM_Zone_10N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone11N = ProjectionInfo.FromAuthorityCode("ESRI", 102411).SetNames("NAD_1983_CORS96_UTM_Zone_11N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone12N = ProjectionInfo.FromAuthorityCode("ESRI", 102412).SetNames("NAD_1983_CORS96_UTM_Zone_12N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone13N = ProjectionInfo.FromAuthorityCode("ESRI", 102413).SetNames("NAD_1983_CORS96_UTM_Zone_13N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone14N = ProjectionInfo.FromAuthorityCode("ESRI", 102414).SetNames("NAD_1983_CORS96_UTM_Zone_14N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone15N = ProjectionInfo.FromAuthorityCode("ESRI", 102415).SetNames("NAD_1983_CORS96_UTM_Zone_15N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone16N = ProjectionInfo.FromAuthorityCode("ESRI", 102416).SetNames("NAD_1983_CORS96_UTM_Zone_16N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone17N = ProjectionInfo.FromAuthorityCode("ESRI", 102417).SetNames("NAD_1983_CORS96_UTM_Zone_17N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone18N = ProjectionInfo.FromAuthorityCode("ESRI", 102418).SetNames("NAD_1983_CORS96_UTM_Zone_18N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone19N = ProjectionInfo.FromAuthorityCode("ESRI", 102419).SetNames("NAD_1983_CORS96_UTM_Zone_19N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone1N = ProjectionInfo.FromAuthorityCode("ESRI", 102401).SetNames("NAD_1983_CORS96_UTM_Zone_1N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone2N = ProjectionInfo.FromAuthorityCode("ESRI", 102402).SetNames("NAD_1983_CORS96_UTM_Zone_2N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone3N = ProjectionInfo.FromAuthorityCode("ESRI", 102403).SetNames("NAD_1983_CORS96_UTM_Zone_3N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone4N = ProjectionInfo.FromAuthorityCode("ESRI", 102404).SetNames("NAD_1983_CORS96_UTM_Zone_4N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone59N = ProjectionInfo.FromAuthorityCode("ESRI", 102364).SetNames("NAD_1983_CORS96_UTM_Zone_59N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone5N = ProjectionInfo.FromAuthorityCode("ESRI", 102405).SetNames("NAD_1983_CORS96_UTM_Zone_5N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone60N = ProjectionInfo.FromAuthorityCode("ESRI", 102365).SetNames("NAD_1983_CORS96_UTM_Zone_60N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone6N = ProjectionInfo.FromAuthorityCode("ESRI", 102406).SetNames("NAD_1983_CORS96_UTM_Zone_6N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone7N = ProjectionInfo.FromAuthorityCode("ESRI", 102407).SetNames("NAD_1983_CORS96_UTM_Zone_7N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone8N = ProjectionInfo.FromAuthorityCode("ESRI", 102408).SetNames("NAD_1983_CORS96_UTM_Zone_8N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96UTMZone9N = ProjectionInfo.FromAuthorityCode("ESRI", 102409).SetNames("NAD_1983_CORS96_UTM_Zone_9N", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983HARNUTMZone10N = ProjectionInfo.FromEpsgCode(3740).SetNames("NAD_1983_HARN_UTM_Zone_10N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone11N = ProjectionInfo.FromEpsgCode(3741).SetNames("NAD_1983_HARN_UTM_Zone_11N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone12N = ProjectionInfo.FromEpsgCode(3742).SetNames("NAD_1983_HARN_UTM_Zone_12N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone13N = ProjectionInfo.FromEpsgCode(3743).SetNames("NAD_1983_HARN_UTM_Zone_13N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone14N = ProjectionInfo.FromEpsgCode(3744).SetNames("NAD_1983_HARN_UTM_Zone_14N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone15N = ProjectionInfo.FromEpsgCode(3745).SetNames("NAD_1983_HARN_UTM_Zone_15N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone16N = ProjectionInfo.FromEpsgCode(3746).SetNames("NAD_1983_HARN_UTM_Zone_16N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone17N = ProjectionInfo.FromEpsgCode(3747).SetNames("NAD_1983_HARN_UTM_Zone_17N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone18N = ProjectionInfo.FromEpsgCode(3748).SetNames("NAD_1983_HARN_UTM_Zone_18N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone19N = ProjectionInfo.FromEpsgCode(3749).SetNames("NAD_1983_HARN_UTM_Zone_19N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone2S = ProjectionInfo.FromEpsgCode(2195).SetNames("NAD_1983_HARN_UTM_Zone_2S", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone4N = ProjectionInfo.FromEpsgCode(3750).SetNames("NAD_1983_HARN_UTM_Zone_4N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNUTMZone5N = ProjectionInfo.FromEpsgCode(3751).SetNames("NAD_1983_HARN_UTM_Zone_5N", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983NSRS2007UTMZone10N = ProjectionInfo.FromEpsgCode(3717).SetNames("NAD_1983_NSRS2007_UTM_Zone_10N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone11N = ProjectionInfo.FromEpsgCode(3718).SetNames("NAD_1983_NSRS2007_UTM_Zone_11N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone12N = ProjectionInfo.FromEpsgCode(3719).SetNames("NAD_1983_NSRS2007_UTM_Zone_12N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone13N = ProjectionInfo.FromEpsgCode(3720).SetNames("NAD_1983_NSRS2007_UTM_Zone_13N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone14N = ProjectionInfo.FromEpsgCode(3721).SetNames("NAD_1983_NSRS2007_UTM_Zone_14N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone15N = ProjectionInfo.FromEpsgCode(3722).SetNames("NAD_1983_NSRS2007_UTM_Zone_15N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone16N = ProjectionInfo.FromEpsgCode(3723).SetNames("NAD_1983_NSRS2007_UTM_Zone_16N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone17N = ProjectionInfo.FromEpsgCode(3724).SetNames("NAD_1983_NSRS2007_UTM_Zone_17N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone18N = ProjectionInfo.FromEpsgCode(3725).SetNames("NAD_1983_NSRS2007_UTM_Zone_18N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone19N = ProjectionInfo.FromEpsgCode(3726).SetNames("NAD_1983_NSRS2007_UTM_Zone_19N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone1N = ProjectionInfo.FromEpsgCode(3708).SetNames("NAD_1983_NSRS2007_UTM_Zone_1N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone2N = ProjectionInfo.FromEpsgCode(3709).SetNames("NAD_1983_NSRS2007_UTM_Zone_2N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone3N = ProjectionInfo.FromEpsgCode(3710).SetNames("NAD_1983_NSRS2007_UTM_Zone_3N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone4N = ProjectionInfo.FromEpsgCode(3711).SetNames("NAD_1983_NSRS2007_UTM_Zone_4N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone59N = ProjectionInfo.FromEpsgCode(3706).SetNames("NAD_1983_NSRS2007_UTM_Zone_59N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone5N = ProjectionInfo.FromEpsgCode(3712).SetNames("NAD_1983_NSRS2007_UTM_Zone_5N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone60N = ProjectionInfo.FromEpsgCode(3707).SetNames("NAD_1983_NSRS2007_UTM_Zone_60N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone6N = ProjectionInfo.FromEpsgCode(3713).SetNames("NAD_1983_NSRS2007_UTM_Zone_6N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone7N = ProjectionInfo.FromEpsgCode(3714).SetNames("NAD_1983_NSRS2007_UTM_Zone_7N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone8N = ProjectionInfo.FromEpsgCode(3715).SetNames("NAD_1983_NSRS2007_UTM_Zone_8N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007UTMZone9N = ProjectionInfo.FromEpsgCode(3716).SetNames("NAD_1983_NSRS2007_UTM_Zone_9N", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983PACP00UTMZone2S = ProjectionInfo.FromAuthorityCode("ESRI", 102703).SetNames("NAD_1983_PACP00_UTM_Zone_2S", "GCS_NAD_1983_PACP00", "D_NAD_1983_PACP00"); // missing
            NAD1983PACP00UTMZone4N = ProjectionInfo.FromAuthorityCode("ESRI", 102701).SetNames("NAD_1983_PACP00_UTM_Zone_4N", "GCS_NAD_1983_PACP00", "D_NAD_1983_PACP00"); // missing
            NAD1983PACP00UTMZone5N = ProjectionInfo.FromAuthorityCode("ESRI", 102702).SetNames("NAD_1983_PACP00_UTM_Zone_5N", "GCS_NAD_1983_PACP00", "D_NAD_1983_PACP00"); // missing
            PuertoRicoUTMZone20N = ProjectionInfo.FromEpsgCode(3920).SetNames("Puerto_Rico_UTM_Zone_20N", "GCS_Puerto_Rico", "D_Puerto_Rico");
            Qornoq1927UTMZone22N = ProjectionInfo.FromEpsgCode(2216).SetNames("Qornoq_1927_UTM_Zone_22N", "GCS_Qornoq_1927", "D_Qornoq_1927");
            Qornoq1927UTMZone23N = ProjectionInfo.FromEpsgCode(2217).SetNames("Qornoq_1927_UTM_Zone_23N", "GCS_Qornoq_1927", "D_Qornoq_1927");
            SaintPierreetMiquelon1950UTMZone21N = ProjectionInfo.FromEpsgCode(2987).SetNames("Saint_Pierre_et_Miquelon_1950_UTM_21N", "GCS_Saint_Pierre_et_Miquelon_1950", "D_Saint_Pierre_et_Miquelon_1950");
        }

        #endregion
    }
}

#pragma warning restore 1591