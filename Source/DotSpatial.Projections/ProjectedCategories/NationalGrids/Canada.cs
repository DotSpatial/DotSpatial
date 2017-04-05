// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:47:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.NationalGrids
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Canada.
    /// </summary>
    public class Canada : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo ATS1977MTM4NovaScotia;
        public readonly ProjectionInfo ATS1977MTM5NovaScotia;
        public readonly ProjectionInfo ATS1977NewBrunswickStereographic;
        public readonly ProjectionInfo NAD192710TMAEPForest;
        public readonly ProjectionInfo NAD192710TMAEPResource;
        public readonly ProjectionInfo NAD19273TM111;
        public readonly ProjectionInfo NAD19273TM114;
        public readonly ProjectionInfo NAD19273TM117;
        public readonly ProjectionInfo NAD19273TM120;
        public readonly ProjectionInfo NAD1927CGQ77MTM10SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM2SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM3SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM4SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM5SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM6SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM7SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM8SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM9SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77QuebecLambert;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone17N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone18N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone19N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone20N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone21N;
        public readonly ProjectionInfo NAD1927DEF1976MTM10;
        public readonly ProjectionInfo NAD1927DEF1976MTM11;
        public readonly ProjectionInfo NAD1927DEF1976MTM12;
        public readonly ProjectionInfo NAD1927DEF1976MTM13;
        public readonly ProjectionInfo NAD1927DEF1976MTM14;
        public readonly ProjectionInfo NAD1927DEF1976MTM15;
        public readonly ProjectionInfo NAD1927DEF1976MTM16;
        public readonly ProjectionInfo NAD1927DEF1976MTM17;
        public readonly ProjectionInfo NAD1927DEF1976MTM8;
        public readonly ProjectionInfo NAD1927DEF1976MTM9;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone15N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone16N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone17N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone18N;
        public readonly ProjectionInfo NAD1927MTM1;
        public readonly ProjectionInfo NAD1927MTM2;
        public readonly ProjectionInfo NAD1927MTM3;
        public readonly ProjectionInfo NAD1927MTM4;
        public readonly ProjectionInfo NAD1927MTM5;
        public readonly ProjectionInfo NAD1927MTM6;
        public readonly ProjectionInfo NAD1927MTQLambert;
        public readonly ProjectionInfo NAD1927QuebecLambert;
        public readonly ProjectionInfo NAD198310TMAEPForest;
        public readonly ProjectionInfo NAD198310TMAEPResource;
        public readonly ProjectionInfo NAD19833TM111;
        public readonly ProjectionInfo NAD19833TM114;
        public readonly ProjectionInfo NAD19833TM117;
        public readonly ProjectionInfo NAD19833TM120;
        public readonly ProjectionInfo NAD1983BCEnvironmentAlbers;
        public readonly ProjectionInfo NAD1983CSRS10TMAEPForest;
        public readonly ProjectionInfo NAD1983CSRS10TMAEPResource;
        public readonly ProjectionInfo NAD1983CSRS3TM111;
        public readonly ProjectionInfo NAD1983CSRS3TM114;
        public readonly ProjectionInfo NAD1983CSRS3TM117;
        public readonly ProjectionInfo NAD1983CSRS3TM120;
        public readonly ProjectionInfo NAD1983CSRSBCEnvironmentAlbers;
        public readonly ProjectionInfo NAD1983CSRSMTM1;
        public readonly ProjectionInfo NAD1983CSRSMTM10;
        public readonly ProjectionInfo NAD1983CSRSMTM11;
        public readonly ProjectionInfo NAD1983CSRSMTM12;
        public readonly ProjectionInfo NAD1983CSRSMTM13;
        public readonly ProjectionInfo NAD1983CSRSMTM14;
        public readonly ProjectionInfo NAD1983CSRSMTM15;
        public readonly ProjectionInfo NAD1983CSRSMTM16;
        public readonly ProjectionInfo NAD1983CSRSMTM17;
        public readonly ProjectionInfo NAD1983CSRSMTM2;
        public readonly ProjectionInfo NAD1983CSRSMTM2SCoPQ;
        public readonly ProjectionInfo NAD1983CSRSMTM3;
        public readonly ProjectionInfo NAD1983CSRSMTM4;
        public readonly ProjectionInfo NAD1983CSRSMTM5;
        public readonly ProjectionInfo NAD1983CSRSMTM6;
        public readonly ProjectionInfo NAD1983CSRSMTM7;
        public readonly ProjectionInfo NAD1983CSRSMTM8;
        public readonly ProjectionInfo NAD1983CSRSMTM9;
        public readonly ProjectionInfo NAD1983CSRSMTQLambert;
        public readonly ProjectionInfo NAD1983CSRSNewBrunswickStereographic;
        public readonly ProjectionInfo NAD1983CSRSNorthwestTerritoriesLambert;
        public readonly ProjectionInfo NAD1983CSRSOntarioMNRLambert;
        public readonly ProjectionInfo NAD1983CSRSPrinceEdwardIsland;
        public readonly ProjectionInfo NAD1983CSRSStatisticsCanadaLambert;
        public readonly ProjectionInfo NAD1983CSRSUTMZone10N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone11N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone12N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone13N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone14N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone15N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone16N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone17N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone18N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone19N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone20N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone21N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone22N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone7N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone8N;
        public readonly ProjectionInfo NAD1983CSRSUTMZone9N;
        public readonly ProjectionInfo NAD1983CSRSYukonAlbers;
        public readonly ProjectionInfo NAD1983MTM1;
        public readonly ProjectionInfo NAD1983MTM10;
        public readonly ProjectionInfo NAD1983MTM11;
        public readonly ProjectionInfo NAD1983MTM12;
        public readonly ProjectionInfo NAD1983MTM13;
        public readonly ProjectionInfo NAD1983MTM14;
        public readonly ProjectionInfo NAD1983MTM15;
        public readonly ProjectionInfo NAD1983MTM16;
        public readonly ProjectionInfo NAD1983MTM17;
        public readonly ProjectionInfo NAD1983MTM2;
        public readonly ProjectionInfo NAD1983MTM2SCoPQ;
        public readonly ProjectionInfo NAD1983MTM3;
        public readonly ProjectionInfo NAD1983MTM4;
        public readonly ProjectionInfo NAD1983MTM5;
        public readonly ProjectionInfo NAD1983MTM6;
        public readonly ProjectionInfo NAD1983MTM7;
        public readonly ProjectionInfo NAD1983MTM8;
        public readonly ProjectionInfo NAD1983MTM9;
        public readonly ProjectionInfo NAD1983MTQLambert;
        public readonly ProjectionInfo NAD1983NorthwestTerritoriesLambert;
        public readonly ProjectionInfo NAD1983OntarioMNRLambert;
        public readonly ProjectionInfo NAD1983QuebecLambert;
        public readonly ProjectionInfo NAD1983StatisticsCanadaLambert;
        public readonly ProjectionInfo NAD1983YukonAlbers;
        public readonly ProjectionInfo PrinceEdwardIslandStereographic;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Canada.
        /// </summary>
        public Canada()
        {
            ATS1977MTM4NovaScotia = ProjectionInfo.FromEpsgCode(2294).SetNames("ATS_1977_MTM_4_Nova_Scotia", "GCS_ATS_1977", "D_ATS_1977");
            ATS1977MTM5NovaScotia = ProjectionInfo.FromEpsgCode(2295).SetNames("ATS_1977_MTM_5_Nova_Scotia", "GCS_ATS_1977", "D_ATS_1977");
            ATS1977NewBrunswickStereographic = ProjectionInfo.FromEpsgCode(2200).SetNames("ATS_1977_New_Brunswick_Stereographic", "GCS_ATS_1977", "D_ATS_1977");
            NAD192710TMAEPForest = ProjectionInfo.FromAuthorityCode("ESRI", 102178).SetNames("NAD_1927_10TM_AEP_Forest", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD192710TMAEPResource = ProjectionInfo.FromAuthorityCode("ESRI", 102179).SetNames("NAD_1927_10TM_AEP_Resource", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD19273TM111 = ProjectionInfo.FromEpsgCode(3771).SetNames("NAD_1927_3TM_111", "GCS_North_American_1927", "D_North_American_1927");
            NAD19273TM114 = ProjectionInfo.FromEpsgCode(3772).SetNames("NAD_1927_3TM_114", "GCS_North_American_1927", "D_North_American_1927");
            NAD19273TM117 = ProjectionInfo.FromEpsgCode(3773).SetNames("NAD_1927_3TM_117", "GCS_North_American_1927", "D_North_American_1927");
            NAD19273TM120 = ProjectionInfo.FromEpsgCode(3800).SetNames("NAD_1927_3TM_120", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927CGQ77MTM10SCoPQ = ProjectionInfo.FromEpsgCode(2016).SetNames("NAD_1927_CGQ77_MTM_10_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM2SCoPQ = ProjectionInfo.FromEpsgCode(2008).SetNames("NAD_1927_CGQ77_MTM_2_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM3SCoPQ = ProjectionInfo.FromEpsgCode(2009).SetNames("NAD_1927_CGQ77_MTM_3_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM4SCoPQ = ProjectionInfo.FromEpsgCode(2010).SetNames("NAD_1927_CGQ77_MTM_4_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM5SCoPQ = ProjectionInfo.FromEpsgCode(2011).SetNames("NAD_1927_CGQ77_MTM_5_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM6SCoPQ = ProjectionInfo.FromEpsgCode(2012).SetNames("NAD_1927_CGQ77_MTM_6_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM7SCoPQ = ProjectionInfo.FromEpsgCode(2013).SetNames("NAD_1927_CGQ77_MTM_7_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM8SCoPQ = ProjectionInfo.FromEpsgCode(2014).SetNames("NAD_1927_CGQ77_MTM_8_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77MTM9SCoPQ = ProjectionInfo.FromEpsgCode(2015).SetNames("NAD_1927_CGQ77_MTM_9_SCoPQ", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77QuebecLambert = ProjectionInfo.FromEpsgCode(2138).SetNames("NAD_1927_CGQ77_Quebec_Lambert", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77UTMZone17N = ProjectionInfo.FromEpsgCode(2031).SetNames("NAD_1927_CGQ77_UTM_Zone_17N", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77UTMZone18N = ProjectionInfo.FromEpsgCode(2032).SetNames("NAD_1927_CGQ77_UTM_Zone_18N", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77UTMZone19N = ProjectionInfo.FromEpsgCode(2033).SetNames("NAD_1927_CGQ77_UTM_Zone_19N", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77UTMZone20N = ProjectionInfo.FromEpsgCode(2034).SetNames("NAD_1927_CGQ77_UTM_Zone_20N", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927CGQ77UTMZone21N = ProjectionInfo.FromEpsgCode(2035).SetNames("NAD_1927_CGQ77_UTM_Zone_21N", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927DEF1976MTM10 = ProjectionInfo.FromEpsgCode(2019).SetNames("NAD_1927_DEF_1976_MTM_10", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM11 = ProjectionInfo.FromEpsgCode(2020).SetNames("NAD_1927_DEF_1976_MTM_11", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM12 = ProjectionInfo.FromEpsgCode(2021).SetNames("NAD_1927_DEF_1976_MTM_12", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM13 = ProjectionInfo.FromEpsgCode(2022).SetNames("NAD_1927_DEF_1976_MTM_13", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM14 = ProjectionInfo.FromEpsgCode(2023).SetNames("NAD_1927_DEF_1976_MTM_14", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM15 = ProjectionInfo.FromEpsgCode(2024).SetNames("NAD_1927_DEF_1976_MTM_15", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM16 = ProjectionInfo.FromEpsgCode(2025).SetNames("NAD_1927_DEF_1976_MTM_16", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM17 = ProjectionInfo.FromEpsgCode(2026).SetNames("NAD_1927_DEF_1976_MTM_17", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM8 = ProjectionInfo.FromEpsgCode(2017).SetNames("NAD_1927_DEF_1976_MTM_8", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976MTM9 = ProjectionInfo.FromEpsgCode(2018).SetNames("NAD_1927_DEF_1976_MTM_9", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976UTMZone15N = ProjectionInfo.FromEpsgCode(2027).SetNames("NAD_1927_DEF_1976_UTM_Zone_15N", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976UTMZone16N = ProjectionInfo.FromEpsgCode(2028).SetNames("NAD_1927_DEF_1976_UTM_Zone_16N", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976UTMZone17N = ProjectionInfo.FromEpsgCode(2029).SetNames("NAD_1927_DEF_1976_UTM_Zone_17N", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927DEF1976UTMZone18N = ProjectionInfo.FromEpsgCode(2030).SetNames("NAD_1927_DEF_1976_UTM_Zone_18N", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1927MTM1 = ProjectionInfo.FromEpsgCode(32081).SetNames("NAD_1927_MTM_1", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927MTM2 = ProjectionInfo.FromEpsgCode(32082).SetNames("NAD_1927_MTM_2", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927MTM3 = ProjectionInfo.FromEpsgCode(32083).SetNames("NAD_1927_MTM_3", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927MTM4 = ProjectionInfo.FromEpsgCode(32084).SetNames("NAD_1927_MTM_4", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927MTM5 = ProjectionInfo.FromEpsgCode(32085).SetNames("NAD_1927_MTM_5", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927MTM6 = ProjectionInfo.FromEpsgCode(32086).SetNames("NAD_1927_MTM_6", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927MTQLambert = ProjectionInfo.FromEpsgCode(3797).SetNames("NAD_1927_MTQ_Lambert", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927QuebecLambert = ProjectionInfo.FromEpsgCode(32098).SetNames("NAD_1927_Quebec_Lambert", "GCS_North_American_1927", "D_North_American_1927");
            NAD198310TMAEPForest = ProjectionInfo.FromEpsgCode(3400).SetNames("NAD_1983_10TM_AEP_Forest", "GCS_North_American_1983", "D_North_American_1983");
            NAD198310TMAEPResource = ProjectionInfo.FromEpsgCode(3401).SetNames("NAD_1983_10TM_AEP_Resource", "GCS_North_American_1983", "D_North_American_1983");
            NAD19833TM111 = ProjectionInfo.FromEpsgCode(3775).SetNames("NAD_1983_3TM_111", "GCS_North_American_1983", "D_North_American_1983");
            NAD19833TM114 = ProjectionInfo.FromEpsgCode(3776).SetNames("NAD_1983_3TM_114", "GCS_North_American_1983", "D_North_American_1983");
            NAD19833TM117 = ProjectionInfo.FromEpsgCode(3777).SetNames("NAD_1983_3TM_117", "GCS_North_American_1983", "D_North_American_1983");
            NAD19833TM120 = ProjectionInfo.FromEpsgCode(3801).SetNames("NAD_1983_3TM_120", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983BCEnvironmentAlbers = ProjectionInfo.FromEpsgCode(3005).SetNames("NAD_1983_BC_Environment_Albers", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983CSRS10TMAEPForest = ProjectionInfo.FromEpsgCode(3402).SetNames("NAD_1983_CSRS_10TM_AEP_Forest", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRS10TMAEPResource = ProjectionInfo.FromEpsgCode(3403).SetNames("NAD_1983_CSRS_10TM_AEP_Resource", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRS3TM111 = ProjectionInfo.FromEpsgCode(3779).SetNames("NAD_1983_CSRS_3TM_111", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRS3TM114 = ProjectionInfo.FromEpsgCode(3780).SetNames("NAD_1983_CSRS_3TM_114", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRS3TM117 = ProjectionInfo.FromEpsgCode(3781).SetNames("NAD_1983_CSRS_3TM_117", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRS3TM120 = ProjectionInfo.FromEpsgCode(3802).SetNames("NAD_1983_CSRS_3TM_120", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSBCEnvironmentAlbers = ProjectionInfo.FromEpsgCode(3153).SetNames("NAD_1983_CSRS_BC_Environment_Albers", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM1 = ProjectionInfo.FromEpsgCode(26898).SetNames("NAD_1983_CSRS_MTM_1", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM10 = ProjectionInfo.FromEpsgCode(2952).SetNames("NAD_1983_CSRS_MTM_10", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM11 = ProjectionInfo.FromEpsgCode(26891).SetNames("NAD_1983_CSRS_MTM_11", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM12 = ProjectionInfo.FromEpsgCode(26892).SetNames("NAD_1983_CSRS_MTM_12", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM13 = ProjectionInfo.FromEpsgCode(26893).SetNames("NAD_1983_CSRS_MTM_13", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM14 = ProjectionInfo.FromEpsgCode(26894).SetNames("NAD_1983_CSRS_MTM_14", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM15 = ProjectionInfo.FromEpsgCode(26895).SetNames("NAD_1983_CSRS_MTM_15", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM16 = ProjectionInfo.FromEpsgCode(26896).SetNames("NAD_1983_CSRS_MTM_16", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM17 = ProjectionInfo.FromEpsgCode(26897).SetNames("NAD_1983_CSRS_MTM_17", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM2 = ProjectionInfo.FromEpsgCode(26899).SetNames("NAD_1983_CSRS_MTM_2", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM2SCoPQ = ProjectionInfo.FromEpsgCode(2944).SetNames("NAD_1983_CSRS_MTM_2_SCoPQ", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM3 = ProjectionInfo.FromEpsgCode(2945).SetNames("NAD_1983_CSRS_MTM_3", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM4 = ProjectionInfo.FromEpsgCode(2946).SetNames("NAD_1983_CSRS_MTM_4", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM5 = ProjectionInfo.FromEpsgCode(2947).SetNames("NAD_1983_CSRS_MTM_5", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM6 = ProjectionInfo.FromEpsgCode(2948).SetNames("NAD_1983_CSRS_MTM_6", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM7 = ProjectionInfo.FromEpsgCode(2949).SetNames("NAD_1983_CSRS_MTM_7", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM8 = ProjectionInfo.FromEpsgCode(2950).SetNames("NAD_1983_CSRS_MTM_8", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTM9 = ProjectionInfo.FromEpsgCode(2951).SetNames("NAD_1983_CSRS_MTM_9", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSMTQLambert = ProjectionInfo.FromEpsgCode(3799).SetNames("NAD_1983_CSRS_MTQ_Lambert", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSNewBrunswickStereographic = ProjectionInfo.FromEpsgCode(2953).SetNames("NAD_1983_CSRS_New_Brunswick_Stereographic", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSNorthwestTerritoriesLambert = ProjectionInfo.FromEpsgCode(3581).SetNames("NAD_1983_CSRS_Northwest_Territories_Lambert", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSOntarioMNRLambert = ProjectionInfo.FromEpsgCode(3162).SetNames("NAD_1983_CSRS_Ontario_MNR_Lambert", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSPrinceEdwardIsland = ProjectionInfo.FromEpsgCode(2954).SetNames("NAD_1983_CSRS_Prince_Edward_Island", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSStatisticsCanadaLambert = ProjectionInfo.FromEpsgCode(3348).SetNames("NAD_1983_CSRS_Statistics_Canada_Lambert", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone10N = ProjectionInfo.FromEpsgCode(3157).SetNames("NAD_1983_CSRS_UTM_Zone_10N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone11N = ProjectionInfo.FromEpsgCode(2955).SetNames("NAD_1983_CSRS_UTM_Zone_11N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone12N = ProjectionInfo.FromEpsgCode(2956).SetNames("NAD_1983_CSRS_UTM_Zone_12N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone13N = ProjectionInfo.FromEpsgCode(2957).SetNames("NAD_1983_CSRS_UTM_Zone_13N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone14N = ProjectionInfo.FromEpsgCode(3158).SetNames("NAD_1983_CSRS_UTM_Zone_14N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone15N = ProjectionInfo.FromEpsgCode(3159).SetNames("NAD_1983_CSRS_UTM_Zone_15N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone16N = ProjectionInfo.FromEpsgCode(3160).SetNames("NAD_1983_CSRS_UTM_Zone_16N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone17N = ProjectionInfo.FromEpsgCode(2958).SetNames("NAD_1983_CSRS_UTM_Zone_17N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone18N = ProjectionInfo.FromEpsgCode(2959).SetNames("NAD_1983_CSRS_UTM_Zone_18N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone19N = ProjectionInfo.FromEpsgCode(2960).SetNames("NAD_1983_CSRS_UTM_Zone_19N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone20N = ProjectionInfo.FromEpsgCode(2961).SetNames("NAD_1983_CSRS_UTM_Zone_20N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone21N = ProjectionInfo.FromEpsgCode(2962).SetNames("NAD_1983_CSRS_UTM_Zone_21N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone22N = ProjectionInfo.FromEpsgCode(3761).SetNames("NAD_1983_CSRS_UTM_Zone_22N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone7N = ProjectionInfo.FromEpsgCode(3154).SetNames("NAD_1983_CSRS_UTM_Zone_7N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone8N = ProjectionInfo.FromEpsgCode(3155).SetNames("NAD_1983_CSRS_UTM_Zone_8N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSUTMZone9N = ProjectionInfo.FromEpsgCode(3156).SetNames("NAD_1983_CSRS_UTM_Zone_9N", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983CSRSYukonAlbers = ProjectionInfo.FromEpsgCode(3579).SetNames("NAD_1983_CSRS_Yukon_Albers", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983MTM1 = ProjectionInfo.FromEpsgCode(32181).SetNames("NAD_1983_MTM_1", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM10 = ProjectionInfo.FromEpsgCode(32190).SetNames("NAD_1983_MTM_10", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM11 = ProjectionInfo.FromEpsgCode(32191).SetNames("NAD_1983_MTM_11", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM12 = ProjectionInfo.FromEpsgCode(32192).SetNames("NAD_1983_MTM_12", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM13 = ProjectionInfo.FromEpsgCode(32193).SetNames("NAD_1983_MTM_13", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM14 = ProjectionInfo.FromEpsgCode(32194).SetNames("NAD_1983_MTM_14", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM15 = ProjectionInfo.FromEpsgCode(32195).SetNames("NAD_1983_MTM_15", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM16 = ProjectionInfo.FromEpsgCode(32196).SetNames("NAD_1983_MTM_16", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM17 = ProjectionInfo.FromEpsgCode(32197).SetNames("NAD_1983_MTM_17", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM2 = ProjectionInfo.FromEpsgCode(32182).SetNames("NAD_1983_MTM_2", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM2SCoPQ = ProjectionInfo.FromEpsgCode(32180).SetNames("NAD_1983_MTM_2_SCoPQ", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM3 = ProjectionInfo.FromEpsgCode(32183).SetNames("NAD_1983_MTM_3", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM4 = ProjectionInfo.FromEpsgCode(32184).SetNames("NAD_1983_MTM_4", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM5 = ProjectionInfo.FromEpsgCode(32185).SetNames("NAD_1983_MTM_5", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM6 = ProjectionInfo.FromEpsgCode(32186).SetNames("NAD_1983_MTM_6", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM7 = ProjectionInfo.FromEpsgCode(32187).SetNames("NAD_1983_MTM_7", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM8 = ProjectionInfo.FromEpsgCode(32188).SetNames("NAD_1983_MTM_8", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTM9 = ProjectionInfo.FromEpsgCode(32189).SetNames("NAD_1983_MTM_9", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MTQLambert = ProjectionInfo.FromEpsgCode(3798).SetNames("NAD_1983_MTQ_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983NorthwestTerritoriesLambert = ProjectionInfo.FromEpsgCode(3580).SetNames("NAD_1983_Northwest_Territories_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983OntarioMNRLambert = ProjectionInfo.FromEpsgCode(3161).SetNames("NAD_1983_Ontario_MNR_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983QuebecLambert = ProjectionInfo.FromEpsgCode(32198).SetNames("NAD_1983_Quebec_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatisticsCanadaLambert = ProjectionInfo.FromEpsgCode(3347).SetNames("NAD_1983_Statistics_Canada_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983YukonAlbers = ProjectionInfo.FromEpsgCode(3578).SetNames("NAD_1983_Yukon_Albers", "GCS_North_American_1983", "D_North_American_1983");
            PrinceEdwardIslandStereographic = ProjectionInfo.FromEpsgCode(2290).SetNames("Prince_Edward_Island_Stereographic", "GCS_ATS_1977", "D_ATS_1977");
        }

        #endregion
    }
}

#pragma warning restore 1591