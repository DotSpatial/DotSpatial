// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:03:19 PM
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
    /// This class contains predefined CoordinateSystems for NAD1983HARNUSFeet.
    /// </summary>
    public class NAD1983HARNUSFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983HARNStatePlaneArkansasNorthFIPS0301USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArkansasSouthFIPS0302USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIFIPS0401USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIFIPS0402USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIIFIPS0403USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIVFIPS0404USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVFIPS0405USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVIFIPS0406USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoCentralFIPS0502USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoNorthFIPS0501USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoSouthFIPS0503USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneConnecticutFIPS0600USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneDelawareFIPS0700USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaEastFIPS0901USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaNorthFIPS0903USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaWestFIPS0902USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaEastFIPS1001USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaWestFIPS1002USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii1FIPS5101USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii2FIPS5102USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii3FIPS5103USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii4FIPS5104USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii5FIPS5105USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoCentralFIPS1102USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoEastFIPS1101USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoWestFIPS1103USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIllinoisEastFIPS1201USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIllinoisWestFIPS1202USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaEastFIPS1301USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaWestFIPS1302USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIowaNorthFIPS1401USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIowaSouthFIPS1402USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKansasNorthFIPS1501USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKansasSouthFIPS1502USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckyFIPS1600USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckyNorthFIPS1601USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckySouthFIPS1602USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneLouisianaNorthFIPS1701USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneLouisianaSouthFIPS1702USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMaineEastFIPS1801USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMaineWestFIPS1802USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMarylandFIPS1900USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsFIPS2001USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsIslFIPS2002USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaCentralFIPS2202USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaNorthFIPS2201USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaSouthFIPS2203USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiEastFIPS2301USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiWestFIPS2302USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNebraskaFIPS2600USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaCentralFIPS2702USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaEastFIPS2701USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaWestFIPS2703USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewHampshireFIPS2800USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewJerseyFIPS2900USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoCentralFIPS3002USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoEastFIPS3001USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoWestFIPS3003USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkCentralFIPS3102USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkEastFIPS3101USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkLongIslFIPS3104USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkWestFIPS3103USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthCarolinaFIPS3200USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOhioNorthFIPS3401USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOhioSouthFIPS3402USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaNorthFIPS3501USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaSouthFIPS3502USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlanePennsylvaniaNorthFIPS3701USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlanePennsylvaniaSouthFIPS3702USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneRhodeIslandFIPS3800USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthDakotaNFIPS4001USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthDakotaSFIPS4002USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTennesseeFIPS4100USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasCentralFIPS4203USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNCentralFIPS4202USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthFIPS4201USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSCentralFIPS4204USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthFIPS4205USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahCentralFIPS4302USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahNorthFIPS4301USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahSouthFIPS4303USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaNorthFIPS4501USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaSouthFIPS4502USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonNorthFIPS4601USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonSouthFIPS4602USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWestVirginiaNFIPS4701USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWestVirginiaSFIPS4702USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinCentralFIPS4802USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinNorthFIPS4801USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinSouthFIPS4803USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingEastFIPS4901USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingECentralFIPS4902USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingWCentralFIPS4903USFeet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingWestFIPS4904USFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983HARNUSFeet.
        /// </summary>
        public NAD1983HARNUSFeet()
        {
            NAD1983HARNStatePlaneArkansasNorthFIPS0301USFeet = ProjectionInfo.FromEpsgCode(3441).SetNames("NAD_1983_HARN_StatePlane_Arkansas_North_FIPS_0301_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArkansasSouthFIPS0302USFeet = ProjectionInfo.FromEpsgCode(3442).SetNames("NAD_1983_HARN_StatePlane_Arkansas_South_FIPS_0302_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIFIPS0401USFeet = ProjectionInfo.FromEpsgCode(2870).SetNames("NAD_1983_HARN_StatePlane_California_I_FIPS_0401_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402USFeet = ProjectionInfo.FromEpsgCode(2871).SetNames("NAD_1983_HARN_StatePlane_California_II_FIPS_0402_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403USFeet = ProjectionInfo.FromEpsgCode(2872).SetNames("NAD_1983_HARN_StatePlane_California_III_FIPS_0403_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404USFeet = ProjectionInfo.FromEpsgCode(2873).SetNames("NAD_1983_HARN_StatePlane_California_IV_FIPS_0404_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaVFIPS0405USFeet = ProjectionInfo.FromEpsgCode(2874).SetNames("NAD_1983_HARN_StatePlane_California_V_FIPS_0405_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406USFeet = ProjectionInfo.FromEpsgCode(2875).SetNames("NAD_1983_HARN_StatePlane_California_VI_FIPS_0406_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneColoradoCentralFIPS0502USFeet = ProjectionInfo.FromEpsgCode(2877).SetNames("NAD_1983_HARN_StatePlane_Colorado_Central_FIPS_0502_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneColoradoNorthFIPS0501USFeet = ProjectionInfo.FromEpsgCode(2876).SetNames("NAD_1983_HARN_StatePlane_Colorado_North_FIPS_0501_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneColoradoSouthFIPS0503USFeet = ProjectionInfo.FromEpsgCode(2878).SetNames("NAD_1983_HARN_StatePlane_Colorado_South_FIPS_0503_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneConnecticutFIPS0600USFeet = ProjectionInfo.FromEpsgCode(2879).SetNames("NAD_1983_HARN_StatePlane_Connecticut_FIPS_0600_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneDelawareFIPS0700USFeet = ProjectionInfo.FromEpsgCode(2880).SetNames("NAD_1983_HARN_StatePlane_Delaware_FIPS_0700_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneFloridaEastFIPS0901USFeet = ProjectionInfo.FromEpsgCode(2881).SetNames("NAD_1983_HARN_StatePlane_Florida_East_FIPS_0901_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneFloridaNorthFIPS0903USFeet = ProjectionInfo.FromEpsgCode(2883).SetNames("NAD_1983_HARN_StatePlane_Florida_North_FIPS_0903_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneFloridaWestFIPS0902USFeet = ProjectionInfo.FromEpsgCode(2882).SetNames("NAD_1983_HARN_StatePlane_Florida_West_FIPS_0902_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001USFeet = ProjectionInfo.FromEpsgCode(2884).SetNames("NAD_1983_HARN_StatePlane_Georgia_East_FIPS_1001_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002USFeet = ProjectionInfo.FromEpsgCode(2885).SetNames("NAD_1983_HARN_StatePlane_Georgia_West_FIPS_1002_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneHawaii1FIPS5101USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102461).SetNames("NAD_1983_HARN_StatePlane_Hawaii_1_FIPS_5101_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNStatePlaneHawaii2FIPS5102USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102462).SetNames("NAD_1983_HARN_StatePlane_Hawaii_2_FIPS_5102_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNStatePlaneHawaii3FIPS5103USFeet = ProjectionInfo.FromEpsgCode(3760).SetNames("NAD_1983_HARN_StatePlane_Hawaii_3_FIPS_5103_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneHawaii4FIPS5104USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102464).SetNames("NAD_1983_HARN_StatePlane_Hawaii_4_FIPS_5104_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNStatePlaneHawaii5FIPS5105USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102465).SetNames("NAD_1983_HARN_StatePlane_Hawaii_5_FIPS_5105_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNStatePlaneIdahoCentralFIPS1102USFeet = ProjectionInfo.FromEpsgCode(2887).SetNames("NAD_1983_HARN_StatePlane_Idaho_Central_FIPS_1102_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIdahoEastFIPS1101USFeet = ProjectionInfo.FromEpsgCode(2886).SetNames("NAD_1983_HARN_StatePlane_Idaho_East_FIPS_1101_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIdahoWestFIPS1103USFeet = ProjectionInfo.FromEpsgCode(2888).SetNames("NAD_1983_HARN_StatePlane_Idaho_West_FIPS_1103_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIllinoisEastFIPS1201USFeet = ProjectionInfo.FromEpsgCode(3443).SetNames("NAD_1983_HARN_StatePlane_Illinois_East_FIPS_1201_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIllinoisWestFIPS1202USFeet = ProjectionInfo.FromEpsgCode(3444).SetNames("NAD_1983_HARN_StatePlane_Illinois_West_FIPS_1202_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIndianaEastFIPS1301USFeet = ProjectionInfo.FromEpsgCode(2967).SetNames("NAD_1983_HARN_StatePlane_Indiana_East_FIPS_1301_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIndianaWestFIPS1302USFeet = ProjectionInfo.FromEpsgCode(2968).SetNames("NAD_1983_HARN_StatePlane_Indiana_West_FIPS_1302_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIowaNorthFIPS1401USFeet = ProjectionInfo.FromEpsgCode(3425).SetNames("NAD_1983_HARN_StatePlane_Iowa_North_FIPS_1401_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIowaSouthFIPS1402USFeet = ProjectionInfo.FromEpsgCode(3426).SetNames("NAD_1983_HARN_StatePlane_Iowa_South_FIPS_1402_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKansasNorthFIPS1501USFeet = ProjectionInfo.FromEpsgCode(3427).SetNames("NAD_1983_HARN_StatePlane_Kansas_North_FIPS_1501_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKansasSouthFIPS1502USFeet = ProjectionInfo.FromEpsgCode(3428).SetNames("NAD_1983_HARN_StatePlane_Kansas_South_FIPS_1502_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKentuckyFIPS1600USFeet = ProjectionInfo.FromEpsgCode(3091).SetNames("NAD_1983_HARN_StatePlane_Kentucky_FIPS_1600_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601USFeet = ProjectionInfo.FromEpsgCode(2891).SetNames("NAD_1983_HARN_StatePlane_Kentucky_North_FIPS_1601_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKentuckySouthFIPS1602USFeet = ProjectionInfo.FromEpsgCode(2892).SetNames("NAD_1983_HARN_StatePlane_Kentucky_South_FIPS_1602_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneLouisianaNorthFIPS1701USFeet = ProjectionInfo.FromEpsgCode(3456).SetNames("NAD_1983_HARN_StatePlane_Louisiana_North_FIPS_1701_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneLouisianaSouthFIPS1702USFeet = ProjectionInfo.FromEpsgCode(3457).SetNames("NAD_1983_HARN_StatePlane_Louisiana_South_FIPS_1702_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMaineEastFIPS1801USFeet = ProjectionInfo.FromEpsgCode(26855).SetNames("NAD_1983_HARN_StatePlane_Maine_East_FIPS_1801_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMaineWestFIPS1802USFeet = ProjectionInfo.FromEpsgCode(26856).SetNames("NAD_1983_HARN_StatePlane_Maine_West_FIPS_1802_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMarylandFIPS1900USFeet = ProjectionInfo.FromEpsgCode(2893).SetNames("NAD_1983_HARN_StatePlane_Maryland_FIPS_1900_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMassachusettsFIPS2001USFeet = ProjectionInfo.FromEpsgCode(2894).SetNames("NAD_1983_HARN_StatePlane_Massachusetts_Mainland_FIPS_2001_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMassachusettsIslFIPS2002USFeet = ProjectionInfo.FromEpsgCode(2895).SetNames("NAD_1983_HARN_StatePlane_Massachusetts_Island_FIPS_2002_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMinnesotaCentralFIPS2202USFeet = ProjectionInfo.FromEpsgCode(26858).SetNames("NAD_1983_HARN_StatePlane_Minnesota_Central_FIPS_2202_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMinnesotaNorthFIPS2201USFeet = ProjectionInfo.FromEpsgCode(26857).SetNames("NAD_1983_HARN_StatePlane_Minnesota_North_FIPS_2201_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMinnesotaSouthFIPS2203USFeet = ProjectionInfo.FromEpsgCode(26859).SetNames("NAD_1983_HARN_StatePlane_Minnesota_South_FIPS_2203_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMississippiEastFIPS2301USFeet = ProjectionInfo.FromEpsgCode(2899).SetNames("NAD_1983_HARN_StatePlane_Mississippi_East_FIPS_2301_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMississippiWestFIPS2302USFeet = ProjectionInfo.FromEpsgCode(2900).SetNames("NAD_1983_HARN_StatePlane_Mississippi_West_FIPS_2302_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNebraskaFIPS2600USFeet = ProjectionInfo.FromEpsgCode(26860).SetNames("NAD_1983_HARN_StatePlane_Nebraska_FIPS_2600_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNevadaCentralFIPS2702USFeet = ProjectionInfo.FromEpsgCode(3430).SetNames("NAD_1983_HARN_StatePlane_Nevada_Central_FIPS_2702_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNevadaEastFIPS2701USFeet = ProjectionInfo.FromEpsgCode(3429).SetNames("NAD_1983_HARN_StatePlane_Nevada_East_FIPS_2701_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNevadaWestFIPS2703USFeet = ProjectionInfo.FromEpsgCode(3431).SetNames("NAD_1983_HARN_StatePlane_Nevada_West_FIPS_2703_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewHampshireFIPS2800USFeet = ProjectionInfo.FromEpsgCode(3445).SetNames("NAD_1983_HARN_StatePlane_New_Hampshire_FIPS_2800_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewJerseyFIPS2900USFeet = ProjectionInfo.FromEpsgCode(3432).SetNames("NAD_1983_HARN_StatePlane_New_Jersey_FIPS_2900_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002USFeet = ProjectionInfo.FromEpsgCode(2903).SetNames("NAD_1983_HARN_StatePlane_New_Mexico_Central_FIPS_3002_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001USFeet = ProjectionInfo.FromEpsgCode(2902).SetNames("NAD_1983_HARN_StatePlane_New_Mexico_East_FIPS_3001_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003USFeet = ProjectionInfo.FromEpsgCode(2904).SetNames("NAD_1983_HARN_StatePlane_New_Mexico_West_FIPS_3003_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102USFeet = ProjectionInfo.FromEpsgCode(2906).SetNames("NAD_1983_HARN_StatePlane_New_York_Central_FIPS_3102_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkEastFIPS3101USFeet = ProjectionInfo.FromEpsgCode(2905).SetNames("NAD_1983_HARN_StatePlane_New_York_East_FIPS_3101_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkLongIslFIPS3104USFeet = ProjectionInfo.FromEpsgCode(2908).SetNames("NAD_1983_HARN_StatePlane_New_York_Long_Island_FIPS_3104_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkWestFIPS3103USFeet = ProjectionInfo.FromEpsgCode(2907).SetNames("NAD_1983_HARN_StatePlane_New_York_West_FIPS_3103_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNorthCarolinaFIPS3200USFeet = ProjectionInfo.FromEpsgCode(3404).SetNames("NAD_1983_HARN_StatePlane_North_Carolina_FIPS_3200_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOhioNorthFIPS3401USFeet = ProjectionInfo.FromEpsgCode(3753).SetNames("NAD_1983_HARN_StatePlane_Ohio_North_FIPS_3401_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOhioSouthFIPS3402USFeet = ProjectionInfo.FromEpsgCode(3754).SetNames("NAD_1983_HARN_StatePlane_Ohio_South_FIPS_3402_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501USFeet = ProjectionInfo.FromEpsgCode(2911).SetNames("NAD_1983_HARN_StatePlane_Oklahoma_North_FIPS_3501_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502USFeet = ProjectionInfo.FromEpsgCode(2912).SetNames("NAD_1983_HARN_StatePlane_Oklahoma_South_FIPS_3502_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlanePennsylvaniaNorthFIPS3701USFeet = ProjectionInfo.FromEpsgCode(3363).SetNames("NAD_1983_HARN_StatePlane_Pennsylvania_North_FIPS_3701_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlanePennsylvaniaSouthFIPS3702USFeet = ProjectionInfo.FromEpsgCode(3365).SetNames("NAD_1983_HARN_StatePlane_Pennsylvania_South_FIPS_3702_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneRhodeIslandFIPS3800USFeet = ProjectionInfo.FromEpsgCode(3446).SetNames("NAD_1983_HARN_StatePlane_Rhode_Island_FIPS_3800_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneSouthDakotaNFIPS4001USFeet = ProjectionInfo.FromEpsgCode(3458).SetNames("NAD_1983_HARN_StatePlane_South_Dakota_North_FIPS_4001_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneSouthDakotaSFIPS4002USFeet = ProjectionInfo.FromEpsgCode(3459).SetNames("NAD_1983_HARN_StatePlane_South_Dakota_South_FIPS_4002_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTennesseeFIPS4100USFeet = ProjectionInfo.FromEpsgCode(2915).SetNames("NAD_1983_HARN_StatePlane_Tennessee_FIPS_4100_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasCentralFIPS4203USFeet = ProjectionInfo.FromEpsgCode(2918).SetNames("NAD_1983_HARN_StatePlane_Texas_Central_FIPS_4203_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasNCentralFIPS4202USFeet = ProjectionInfo.FromEpsgCode(2917).SetNames("NAD_1983_HARN_StatePlane_Texas_North_Central_FIPS_4202_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasNorthFIPS4201USFeet = ProjectionInfo.FromEpsgCode(2916).SetNames("NAD_1983_HARN_StatePlane_Texas_North_FIPS_4201_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasSCentralFIPS4204USFeet = ProjectionInfo.FromEpsgCode(2919).SetNames("NAD_1983_HARN_StatePlane_Texas_South_Central_FIPS_4204_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasSouthFIPS4205USFeet = ProjectionInfo.FromEpsgCode(2920).SetNames("NAD_1983_HARN_StatePlane_Texas_South_FIPS_4205_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahCentralFIPS4302USFeet = ProjectionInfo.FromEpsgCode(3569).SetNames("NAD_1983_HARN_StatePlane_Utah_Central_FIPS_4302_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahNorthFIPS4301USFeet = ProjectionInfo.FromEpsgCode(3568).SetNames("NAD_1983_HARN_StatePlane_Utah_North_FIPS_4301_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahSouthFIPS4303USFeet = ProjectionInfo.FromEpsgCode(3570).SetNames("NAD_1983_HARN_StatePlane_Utah_South_FIPS_4303_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501USFeet = ProjectionInfo.FromEpsgCode(2924).SetNames("NAD_1983_HARN_StatePlane_Virginia_North_FIPS_4501_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502USFeet = ProjectionInfo.FromEpsgCode(2925).SetNames("NAD_1983_HARN_StatePlane_Virginia_South_FIPS_4502_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601USFeet = ProjectionInfo.FromEpsgCode(2926).SetNames("NAD_1983_HARN_StatePlane_Washington_North_FIPS_4601_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602USFeet = ProjectionInfo.FromEpsgCode(2927).SetNames("NAD_1983_HARN_StatePlane_Washington_South_FIPS_4602_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWestVirginiaNFIPS4701USFeet = ProjectionInfo.FromEpsgCode(26861).SetNames("NAD_1983_HARN_StatePlane_West_Virginia_North_FIPS_4701_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWestVirginiaSFIPS4702USFeet = ProjectionInfo.FromEpsgCode(26862).SetNames("NAD_1983_HARN_StatePlane_West_Virginia_South_FIPS_4702_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802USFeet = ProjectionInfo.FromEpsgCode(2929).SetNames("NAD_1983_HARN_StatePlane_Wisconsin_Central_FIPS_4802_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801USFeet = ProjectionInfo.FromEpsgCode(2928).SetNames("NAD_1983_HARN_StatePlane_Wisconsin_North_FIPS_4801_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803USFeet = ProjectionInfo.FromEpsgCode(2930).SetNames("NAD_1983_HARN_StatePlane_Wisconsin_South_FIPS_4803_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingEastFIPS4901USFeet = ProjectionInfo.FromEpsgCode(3755).SetNames("NAD_1983_HARN_StatePlane_Wyoming_East_FIPS_4901_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingECentralFIPS4902USFeet = ProjectionInfo.FromEpsgCode(3756).SetNames("NAD_1983_HARN_StatePlane_Wyoming_East_Central_FIPS_4902_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingWCentralFIPS4903USFeet = ProjectionInfo.FromEpsgCode(3757).SetNames("NAD_1983_HARN_StatePlane_Wyoming_West_Central_FIPS_4903_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingWestFIPS4904USFeet = ProjectionInfo.FromEpsgCode(3758).SetNames("NAD_1983_HARN_StatePlane_Wyoming_West_FIPS_4904_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
        }

        #endregion
    }
}

#pragma warning restore 1591