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

namespace DotSpatial.Projections.ProjectedCategories.StatePlane
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for NAD1983CORS96USFeet.
    /// </summary>
    public class NAD1983CORS96USFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983CORS96StatePlaneArkansasNorthFIPS0301USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneArkansasSouthFIPS0302USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIFIPS0401USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIIFIPS0402USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIIIFIPS0403USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIVFIPS0404USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaVFIPS0405USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaVIFIPS0406USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneColoradoCentralFIPS0502USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneColoradoNorthFIPS0501USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneColoradoSouthFIPS0503USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneConnecticutFIPS0600USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneDelawareFIPS0700USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneFloridaEastFIPS0901USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneFloridaNorthFIPS0903USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneFloridaWestFIPS0902USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneGeorgiaEastFIPS1001USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneGeorgiaWestFIPS1002USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIdahoCentralFIPS1102USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIdahoEastFIPS1101USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIdahoWestFIPS1103USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIllinoisEastFIPS1201USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIllinoisWestFIPS1202USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIndianaEastFIPS1301USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIndianaWestFIPS1302USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIowaNorthFIPS1401USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIowaSouthFIPS1402USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKansasNorthFIPS1501USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKansasSouthFIPS1502USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKentuckyFIPS1600USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKentuckyNorthFIPS1601USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKentuckySouthFIPS1602USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneLouisianaNorthFIPS1701USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneLouisianaSouthFIPS1702USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMaineEastFIPS1801USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMaineWestFIPS1802USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMarylandFIPS1900USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMassachusettsFIPS2001USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMassachusettsIslFIPS2002USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMinnesotaCentralFIPS2202USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMinnesotaNorthFIPS2201USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMinnesotaSouthFIPS2203USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMississippiEastFIPS2301USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMississippiWestFIPS2302USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNebraskaFIPS2600USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNevadaCentralFIPS2702USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNevadaEastFIPS2701USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNevadaWestFIPS2703USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewHampshireFIPS2800USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewJerseyFIPS2900USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewMexicoCentralFIPS3002USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewMexicoEastFIPS3001USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewMexicoWestFIPS3003USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkCentralFIPS3102USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkEastFIPS3101USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkLongIslFIPS3104USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkWestFIPS3103USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNorthCarolinaFIPS3200USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOhioNorthFIPS3401USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOhioSouthFIPS3402USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOklahomaNorthFIPS3501USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOklahomaSouthFIPS3502USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlanePennsylvaniaNorthFIPS3701USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlanePennsylvaniaSouthFIPS3702USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneRhodeIslandFIPS3800USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneSouthDakotaNFIPS4001USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneSouthDakotaSFIPS4002USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTennesseeFIPS4100USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasCentralFIPS4203USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasNCentralFIPS4202USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasNorthFIPS4201USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasSCentralFIPS4204USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasSouthFIPS4205USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneUtahCentralFIPS4302USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneUtahNorthFIPS4301USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneUtahSouthFIPS4303USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneVirginiaNorthFIPS4501USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneVirginiaSouthFIPS4502USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWashingtonNorthFIPS4601USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWashingtonSouthFIPS4602USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWestVirginiaNFIPS4701USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWestVirginiaSFIPS4702USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWisconsinCentralFIPS4802USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWisconsinNorthFIPS4801USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWisconsinSouthFIPS4803USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingEastFIPS4901USFeet;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingECentralFIPS4902USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingWCentralFIPS4903USFt;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingWestFIPS4904USFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983CORS96USFeet.
        /// </summary>
        public NAD1983CORS96USFeet()
        {
            NAD1983CORS96StatePlaneArkansasNorthFIPS0301USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103230).SetNames("NAD_1983_CORS96_StatePlane_Arkansas_North_FIPS_0301_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneArkansasSouthFIPS0302USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103231).SetNames("NAD_1983_CORS96_StatePlane_Arkansas_South_FIPS_0302_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIFIPS0401USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103238).SetNames("NAD_1983_CORS96_StatePlane_California_I_FIPS_0401_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIIFIPS0402USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103239).SetNames("NAD_1983_CORS96_StatePlane_California_II_FIPS_0402_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIIIFIPS0403USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103240).SetNames("NAD_1983_CORS96_StatePlane_California_III_FIPS_0403_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIVFIPS0404USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103241).SetNames("NAD_1983_CORS96_StatePlane_California_IV_FIPS_0404_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaVFIPS0405USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103242).SetNames("NAD_1983_CORS96_StatePlane_California_V_FIPS_0405_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaVIFIPS0406USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103243).SetNames("NAD_1983_CORS96_StatePlane_California_VI_FIPS_0406_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneColoradoCentralFIPS0502USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103248).SetNames("NAD_1983_CORS96_StatePlane_Colorado_Central_FIPS_0502_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneColoradoNorthFIPS0501USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103247).SetNames("NAD_1983_CORS96_StatePlane_Colorado_North_FIPS_0501_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneColoradoSouthFIPS0503USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103249).SetNames("NAD_1983_CORS96_StatePlane_Colorado_South_FIPS_0503_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneConnecticutFIPS0600USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103251).SetNames("NAD_1983_CORS96_StatePlane_Connecticut_FIPS_0600_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneDelawareFIPS0700USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103253).SetNames("NAD_1983_CORS96_StatePlane_Delaware_FIPS_0700_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneFloridaEastFIPS0901USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103257).SetNames("NAD_1983_CORS96_StatePlane_Florida_East_FIPS_0901_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneFloridaNorthFIPS0903USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103259).SetNames("NAD_1983_CORS96_StatePlane_Florida_North_FIPS_0903_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneFloridaWestFIPS0902USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103258).SetNames("NAD_1983_CORS96_StatePlane_Florida_West_FIPS_0902_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneGeorgiaEastFIPS1001USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103262).SetNames("NAD_1983_CORS96_StatePlane_Georgia_East_FIPS_1001_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneGeorgiaWestFIPS1002USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103263).SetNames("NAD_1983_CORS96_StatePlane_Georgia_West_FIPS_1002_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIdahoCentralFIPS1102USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103268).SetNames("NAD_1983_CORS96_StatePlane_Idaho_Central_FIPS_1102_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIdahoEastFIPS1101USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103267).SetNames("NAD_1983_CORS96_StatePlane_Idaho_East_FIPS_1101_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIdahoWestFIPS1103USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103269).SetNames("NAD_1983_CORS96_StatePlane_Idaho_West_FIPS_1103_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIllinoisEastFIPS1201USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103272).SetNames("NAD_1983_CORS96_StatePlane_Illinois_East_FIPS_1201_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIllinoisWestFIPS1202USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103273).SetNames("NAD_1983_CORS96_StatePlane_Illinois_West_FIPS_1202_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIndianaEastFIPS1301USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103276).SetNames("NAD_1983_CORS96_StatePlane_Indiana_East_FIPS_1301_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIndianaWestFIPS1302USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103277).SetNames("NAD_1983_CORS96_StatePlane_Indiana_West_FIPS_1302_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIowaNorthFIPS1401USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103280).SetNames("NAD_1983_CORS96_StatePlane_Iowa_North_FIPS_1401_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIowaSouthFIPS1402USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103281).SetNames("NAD_1983_CORS96_StatePlane_Iowa_South_FIPS_1402_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKansasNorthFIPS1501USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103284).SetNames("NAD_1983_CORS96_StatePlane_Kansas_North_FIPS_1501_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKansasSouthFIPS1502USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103285).SetNames("NAD_1983_CORS96_StatePlane_Kansas_South_FIPS_1502_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKentuckyFIPS1600USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103289).SetNames("NAD_1983_CORS96_StatePlane_Kentucky_FIPS_1600_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKentuckyNorthFIPS1601USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103287).SetNames("NAD_1983_CORS96_StatePlane_Kentucky_North_FIPS_1601_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKentuckySouthFIPS1602USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103291).SetNames("NAD_1983_CORS96_StatePlane_Kentucky_South_FIPS_1602_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneLouisianaNorthFIPS1701USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103294).SetNames("NAD_1983_CORS96_StatePlane_Louisiana_North_FIPS_1701_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneLouisianaSouthFIPS1702USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103295).SetNames("NAD_1983_CORS96_StatePlane_Louisiana_South_FIPS_1702_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMaineEastFIPS1801USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103298).SetNames("NAD_1983_CORS96_StatePlane_Maine_East_FIPS_1801_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMaineWestFIPS1802USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103299).SetNames("NAD_1983_CORS96_StatePlane_Maine_West_FIPS_1802_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMarylandFIPS1900USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103376).SetNames("NAD_1983_CORS96_StatePlane_Maryland_FIPS_1900_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMassachusettsFIPS2001USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103379).SetNames("NAD_1983_CORS96_StatePlane_Massachusetts_Mnld_FIPS_2001_FtUS", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMassachusettsIslFIPS2002USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103380).SetNames("NAD_1983_CORS96_StatePlane_Massachusetts_Isl_FIPS_2002_FtUS", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMinnesotaCentralFIPS2202USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103391).SetNames("NAD_1983_CORS96_StatePlane_Minnesota_Central_FIPS_2202_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMinnesotaNorthFIPS2201USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103390).SetNames("NAD_1983_CORS96_StatePlane_Minnesota_North_FIPS_2201_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMinnesotaSouthFIPS2203USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103392).SetNames("NAD_1983_CORS96_StatePlane_Minnesota_South_FIPS_2203_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMississippiEastFIPS2301USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103395).SetNames("NAD_1983_CORS96_StatePlane_Mississippi_East_FIPS_2301_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMississippiWestFIPS2302USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103396).SetNames("NAD_1983_CORS96_StatePlane_Mississippi_West_FIPS_2302_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNebraskaFIPS2600USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103475).SetNames("NAD_1983_CORS96_StatePlane_Nebraska_FIPS_2600_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNevadaCentralFIPS2702USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103480).SetNames("NAD_1983_CORS96_StatePlane_Nevada_Central_FIPS_2702_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNevadaEastFIPS2701USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103479).SetNames("NAD_1983_CORS96_StatePlane_Nevada_East_FIPS_2701_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNevadaWestFIPS2703USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103481).SetNames("NAD_1983_CORS96_StatePlane_Nevada_West_FIPS_2703_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewHampshireFIPS2800USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103483).SetNames("NAD_1983_CORS96_StatePlane_New_Hampshire_FIPS_2800_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewJerseyFIPS2900USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103485).SetNames("NAD_1983_CORS96_StatePlane_New_Jersey_FIPS_2900_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewMexicoCentralFIPS3002USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103490).SetNames("NAD_1983_CORS96_StatePlane_New_Mexico_Central_FIPS_3002_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewMexicoEastFIPS3001USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103489).SetNames("NAD_1983_CORS96_StatePlane_New_Mexico_East_FIPS_3001_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewMexicoWestFIPS3003USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103491).SetNames("NAD_1983_CORS96_StatePlane_New_Mexico_West_FIPS_3003_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkCentralFIPS3102USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103497).SetNames("NAD_1983_CORS96_StatePlane_New_York_Central_FIPS_3102_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkEastFIPS3101USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103496).SetNames("NAD_1983_CORS96_StatePlane_New_York_East_FIPS_3101_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkLongIslFIPS3104USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103499).SetNames("NAD_1983_CORS96_StatePlane_New_York_Long_Isl_FIPS_3104_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkWestFIPS3103USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103498).SetNames("NAD_1983_CORS96_StatePlane_New_York_West_FIPS_3103_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNorthCarolinaFIPS3200USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103501).SetNames("NAD_1983_CORS96_StatePlane_North_Carolina_FIPS_3200_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOhioNorthFIPS3401USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103508).SetNames("NAD_1983_CORS96_StatePlane_Ohio_North_FIPS_3401_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOhioSouthFIPS3402USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103509).SetNames("NAD_1983_CORS96_StatePlane_Ohio_South_FIPS_3402_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOklahomaNorthFIPS3501USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103512).SetNames("NAD_1983_CORS96_StatePlane_Oklahoma_North_FIPS_3501_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOklahomaSouthFIPS3502USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103513).SetNames("NAD_1983_CORS96_StatePlane_Oklahoma_South_FIPS_3502_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlanePennsylvaniaNorthFIPS3701USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103515).SetNames("NAD_1983_CORS96_StatePlane_Pennsylvania_North_FIPS_3701_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlanePennsylvaniaSouthFIPS3702USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103517).SetNames("NAD_1983_CORS96_StatePlane_Pennsylvania_South_FIPS_3702_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneRhodeIslandFIPS3800USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103519).SetNames("NAD_1983_CORS96_StatePlane_Rhode_Island_FIPS_3800_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneSouthDakotaNFIPS4001USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103524).SetNames("NAD_1983_CORS96_StatePlane_South_Dakota_North_FIPS_4001_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneSouthDakotaSFIPS4002USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103525).SetNames("NAD_1983_CORS96_StatePlane_South_Dakota_South_FIPS_4002_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTennesseeFIPS4100USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103527).SetNames("NAD_1983_CORS96_StatePlane_Tennessee_FIPS_4100_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasCentralFIPS4203USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103546).SetNames("NAD_1983_CORS96_StatePlane_Texas_Central_FIPS_4203_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasNCentralFIPS4202USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103545).SetNames("NAD_1983_CORS96_StatePlane_Texas_North_Central_FIPS_4202_FtUS", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasNorthFIPS4201USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103544).SetNames("NAD_1983_CORS96_StatePlane_Texas_North_FIPS_4201_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasSCentralFIPS4204USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103547).SetNames("NAD_1983_CORS96_StatePlane_Texas_South_Central_FIPS_4204_FtUS", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasSouthFIPS4205USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103548).SetNames("NAD_1983_CORS96_StatePlane_Texas_South_FIPS_4205_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneUtahCentralFIPS4302USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103556).SetNames("NAD_1983_CORS96_StatePlane_Utah_Central_FIPS_4302_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneUtahNorthFIPS4301USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103555).SetNames("NAD_1983_CORS96_StatePlane_Utah_North_FIPS_4301_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneUtahSouthFIPS4303USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103557).SetNames("NAD_1983_CORS96_StatePlane_Utah_South_FIPS_4303_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneVirginiaNorthFIPS4501USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103561).SetNames("NAD_1983_CORS96_StatePlane_Virginia_North_FIPS_4501_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneVirginiaSouthFIPS4502USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103562).SetNames("NAD_1983_CORS96_StatePlane_Virginia_South_FIPS_4502_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWashingtonNorthFIPS4601USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103565).SetNames("NAD_1983_CORS96_StatePlane_Washington_North_FIPS_4601_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWashingtonSouthFIPS4602USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103566).SetNames("NAD_1983_CORS96_StatePlane_Washington_South_FIPS_4602_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWestVirginiaNFIPS4701USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103569).SetNames("NAD_1983_CORS96_StatePlane_West_Virginia_North_FIPS_4701_FtUS", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWestVirginiaSFIPS4702USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103570).SetNames("NAD_1983_CORS96_StatePlane_West_Virginia_South_FIPS_4702_FtUS", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWisconsinCentralFIPS4802USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103575).SetNames("NAD_1983_CORS96_StatePlane_Wisconsin_Central_FIPS_4802_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWisconsinNorthFIPS4801USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103574).SetNames("NAD_1983_CORS96_StatePlane_Wisconsin_North_FIPS_4801_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWisconsinSouthFIPS4803USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103576).SetNames("NAD_1983_CORS96_StatePlane_Wisconsin_South_FIPS_4803_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingEastFIPS4901USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103581).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_East_FIPS_4901_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingECentralFIPS4902USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103582).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_E_Central_FIPS_4902_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingWCentralFIPS4903USFt = ProjectionInfo.FromAuthorityCode("ESRI", 103583).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_W_Central_FIPS_4903_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingWestFIPS4904USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103585).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_West_FIPS_4904_Ft_US", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591