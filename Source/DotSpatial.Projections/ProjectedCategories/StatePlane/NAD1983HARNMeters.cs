// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:01:59 PM
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
    /// This class contains predefined CoordinateSystems for NAD1983HARNMeters.
    /// </summary>
    public class NAD1983HARNMeters : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983HARNMaine2000CentralZoneMeters;
        public readonly ProjectionInfo NAD1983HARNMaine2000EastZoneMeters;
        public readonly ProjectionInfo NAD1983HARNMaine2000WestZoneMeters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneAlabamaEastFIPS0101Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneAlabamaWestFIPS0102Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaCentralFIPS0202Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaEastFIPS0201Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaWestFIPS0203Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArkansasNorthFIPS0301Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArkansasSouthFIPS0302Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIFIPS0401Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIFIPS0402Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIVFIPS0404Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVFIPS0405Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVIFIPS0406Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoCentralFIPS0502Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoNorthFIPS0501Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoSouthFIPS0503Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneConnecticutFIPS0600Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneDelawareFIPS0700Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaEastFIPS0901Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaNorthFIPS0903Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaWestFIPS0902Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaEastFIPS1001Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaWestFIPS1002Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii1FIPS5101Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii2FIPS5102Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii3FIPS5103Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii4FIPS5104Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii5FIPS5105Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoCentralFIPS1102Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoEastFIPS1101Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoWestFIPS1103Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIllinoisEastFIPS1201Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIllinoisWestFIPS1202Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaEastFIPS1301Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaWestFIPS1302Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIowaNorthFIPS1401Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIowaSouthFIPS1402Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKansasNorthFIPS1501Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKansasSouthFIPS1502Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckyFIPS1600Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckyNorthFIPS1601Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckySouthFIPS1602Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneLouisianaNorthFIPS1701Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneLouisianaSouthFIPS1702Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMaineEastFIPS1801Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMaineWestFIPS1802Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMarylandFIPS1900Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsFIPS2001Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsIslFIPS2002Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganCentralFIPS2112Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganNorthFIPS2111Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganSouthFIPS2113Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaCentralFIPS2202Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaNorthFIPS2201Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaSouthFIPS2203Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiEastFIPS2301Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiWestFIPS2302Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMissouriCentralFIPS2402Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMissouriEastFIPS2401Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMissouriWestFIPS2403Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMontanaFIPS2500Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNebraskaFIPS2600Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaCentralFIPS2702Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaEastFIPS2701Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaWestFIPS2703Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewHampshireFIPS2800Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewJerseyFIPS2900Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoEastFIPS3001Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoWestFIPS3003Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkCentralFIPS3102Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkEastFIPS3101Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkLongIslFIPS3104Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkWestFIPS3103Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthCarolinaFIPS3200Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaNFIPS3301Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaSFIPS3302Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOhioNorthFIPS3401Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOhioSouthFIPS3402Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaNorthFIPS3501Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaSouthFIPS3502Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonNorthFIPS3601Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonSouthFIPS3602Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlanePennsylvaniaNorthFIPS3701Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlanePennsylvaniaSouthFIPS3702Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlanePRVIFIPS5200Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneRhodeIslandFIPS3800Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthCarolinaFIPS3900Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthDakotaNFIPS4001Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthDakotaSFIPS4002Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTennesseeFIPS4100Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasCentralFIPS4203Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNCentralFIPS4202Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthFIPS4201Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSCentralFIPS4204Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthFIPS4205Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahCentralFIPS4302Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahNorthFIPS4301Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahSouthFIPS4303Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVermontFIPS4400Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaNorthFIPS4501Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaSouthFIPS4502Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonNorthFIPS4601Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonSouthFIPS4602Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWestVirginiaNFIPS4701Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWestVirginiaSFIPS4702Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinCentralFIPS4802Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinNorthFIPS4801Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinSouthFIPS4803Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingEastFIPS4901Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingECentralFIPS4902Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingWCentralFIPS4903Meters;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingWestFIPS4904Meters;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983HARNMeters.
        /// </summary>
        public NAD1983HARNMeters()
        {
            NAD1983HARNMaine2000CentralZoneMeters = ProjectionInfo.FromEpsgCode(3464).SetNames("NAD_1983_HARN_Maine_2000_Central_Zone", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNMaine2000EastZoneMeters = ProjectionInfo.FromEpsgCode(3075).SetNames("NAD_1983_HARN_Maine_2000_East_Zone", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNMaine2000WestZoneMeters = ProjectionInfo.FromEpsgCode(3077).SetNames("NAD_1983_HARN_Maine_2000_West_Zone", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneAlabamaEastFIPS0101Meters = ProjectionInfo.FromEpsgCode(2759).SetNames("NAD_1983_HARN_StatePlane_Alabama_East_FIPS_0101", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneAlabamaWestFIPS0102Meters = ProjectionInfo.FromEpsgCode(2760).SetNames("NAD_1983_HARN_StatePlane_Alabama_West_FIPS_0102", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArizonaCentralFIPS0202Meters = ProjectionInfo.FromEpsgCode(2762).SetNames("NAD_1983_HARN_StatePlane_Arizona_Central_FIPS_0202", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArizonaEastFIPS0201Meters = ProjectionInfo.FromEpsgCode(2761).SetNames("NAD_1983_HARN_StatePlane_Arizona_East_FIPS_0201", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArizonaWestFIPS0203Meters = ProjectionInfo.FromEpsgCode(2763).SetNames("NAD_1983_HARN_StatePlane_Arizona_West_FIPS_0203", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArkansasNorthFIPS0301Meters = ProjectionInfo.FromEpsgCode(2764).SetNames("NAD_1983_HARN_StatePlane_Arkansas_North_FIPS_0301", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneArkansasSouthFIPS0302Meters = ProjectionInfo.FromEpsgCode(2765).SetNames("NAD_1983_HARN_StatePlane_Arkansas_South_FIPS_0302", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIFIPS0401Meters = ProjectionInfo.FromEpsgCode(2766).SetNames("NAD_1983_HARN_StatePlane_California_I_FIPS_0401", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402Meters = ProjectionInfo.FromEpsgCode(2767).SetNames("NAD_1983_HARN_StatePlane_California_II_FIPS_0402", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Meters = ProjectionInfo.FromEpsgCode(2768).SetNames("NAD_1983_HARN_StatePlane_California_III_FIPS_0403", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404Meters = ProjectionInfo.FromEpsgCode(2769).SetNames("NAD_1983_HARN_StatePlane_California_IV_FIPS_0404", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaVFIPS0405Meters = ProjectionInfo.FromEpsgCode(2770).SetNames("NAD_1983_HARN_StatePlane_California_V_FIPS_0405", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406Meters = ProjectionInfo.FromEpsgCode(2771).SetNames("NAD_1983_HARN_StatePlane_California_VI_FIPS_0406", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneColoradoCentralFIPS0502Meters = ProjectionInfo.FromEpsgCode(2773).SetNames("NAD_1983_HARN_StatePlane_Colorado_Central_FIPS_0502", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneColoradoNorthFIPS0501Meters = ProjectionInfo.FromEpsgCode(2772).SetNames("NAD_1983_HARN_StatePlane_Colorado_North_FIPS_0501", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneColoradoSouthFIPS0503Meters = ProjectionInfo.FromEpsgCode(2774).SetNames("NAD_1983_HARN_StatePlane_Colorado_South_FIPS_0503", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneConnecticutFIPS0600Meters = ProjectionInfo.FromEpsgCode(2775).SetNames("NAD_1983_HARN_StatePlane_Connecticut_FIPS_0600", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneDelawareFIPS0700Meters = ProjectionInfo.FromEpsgCode(2776).SetNames("NAD_1983_HARN_StatePlane_Delaware_FIPS_0700", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneFloridaEastFIPS0901Meters = ProjectionInfo.FromEpsgCode(2777).SetNames("NAD_1983_HARN_StatePlane_Florida_East_FIPS_0901", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneFloridaNorthFIPS0903Meters = ProjectionInfo.FromEpsgCode(2779).SetNames("NAD_1983_HARN_StatePlane_Florida_North_FIPS_0903", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneFloridaWestFIPS0902Meters = ProjectionInfo.FromEpsgCode(2778).SetNames("NAD_1983_HARN_StatePlane_Florida_West_FIPS_0902", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001Meters = ProjectionInfo.FromEpsgCode(2780).SetNames("NAD_1983_HARN_StatePlane_Georgia_East_FIPS_1001", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002Meters = ProjectionInfo.FromEpsgCode(2781).SetNames("NAD_1983_HARN_StatePlane_Georgia_West_FIPS_1002", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneHawaii1FIPS5101Meters = ProjectionInfo.FromEpsgCode(2782).SetNames("NAD_1983_HARN_StatePlane_Hawaii_1_FIPS_5101", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneHawaii2FIPS5102Meters = ProjectionInfo.FromEpsgCode(2783).SetNames("NAD_1983_HARN_StatePlane_Hawaii_2_FIPS_5102", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneHawaii3FIPS5103Meters = ProjectionInfo.FromEpsgCode(2784).SetNames("NAD_1983_HARN_StatePlane_Hawaii_3_FIPS_5103", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneHawaii4FIPS5104Meters = ProjectionInfo.FromEpsgCode(2785).SetNames("NAD_1983_HARN_StatePlane_Hawaii_4_FIPS_5104", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneHawaii5FIPS5105Meters = ProjectionInfo.FromEpsgCode(2786).SetNames("NAD_1983_HARN_StatePlane_Hawaii_5_FIPS_5105", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIdahoCentralFIPS1102Meters = ProjectionInfo.FromEpsgCode(2788).SetNames("NAD_1983_HARN_StatePlane_Idaho_Central_FIPS_1102", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIdahoEastFIPS1101Meters = ProjectionInfo.FromEpsgCode(2787).SetNames("NAD_1983_HARN_StatePlane_Idaho_East_FIPS_1101", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIdahoWestFIPS1103Meters = ProjectionInfo.FromEpsgCode(2789).SetNames("NAD_1983_HARN_StatePlane_Idaho_West_FIPS_1103", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIllinoisEastFIPS1201Meters = ProjectionInfo.FromEpsgCode(2790).SetNames("NAD_1983_HARN_StatePlane_Illinois_East_FIPS_1201", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIllinoisWestFIPS1202Meters = ProjectionInfo.FromEpsgCode(2791).SetNames("NAD_1983_HARN_StatePlane_Illinois_West_FIPS_1202", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIndianaEastFIPS1301Meters = ProjectionInfo.FromEpsgCode(2792).SetNames("NAD_1983_HARN_StatePlane_Indiana_East_FIPS_1301", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIndianaWestFIPS1302Meters = ProjectionInfo.FromEpsgCode(2793).SetNames("NAD_1983_HARN_StatePlane_Indiana_West_FIPS_1302", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIowaNorthFIPS1401Meters = ProjectionInfo.FromEpsgCode(2794).SetNames("NAD_1983_HARN_StatePlane_Iowa_North_FIPS_1401", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneIowaSouthFIPS1402Meters = ProjectionInfo.FromEpsgCode(2795).SetNames("NAD_1983_HARN_StatePlane_Iowa_South_FIPS_1402", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKansasNorthFIPS1501Meters = ProjectionInfo.FromEpsgCode(2796).SetNames("NAD_1983_HARN_StatePlane_Kansas_North_FIPS_1501", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKansasSouthFIPS1502Meters = ProjectionInfo.FromEpsgCode(2797).SetNames("NAD_1983_HARN_StatePlane_Kansas_South_FIPS_1502", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKentuckyFIPS1600Meters = ProjectionInfo.FromEpsgCode(3090).SetNames("NAD_1983_HARN_StatePlane_Kentucky_FIPS_1600", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601Meters = ProjectionInfo.FromEpsgCode(2798).SetNames("NAD_1983_HARN_StatePlane_Kentucky_North_FIPS_1601", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneKentuckySouthFIPS1602Meters = ProjectionInfo.FromEpsgCode(2799).SetNames("NAD_1983_HARN_StatePlane_Kentucky_South_FIPS_1602", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneLouisianaNorthFIPS1701Meters = ProjectionInfo.FromEpsgCode(2800).SetNames("NAD_1983_HARN_StatePlane_Louisiana_North_FIPS_1701", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneLouisianaSouthFIPS1702Meters = ProjectionInfo.FromEpsgCode(2801).SetNames("NAD_1983_HARN_StatePlane_Louisiana_South_FIPS_1702", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMaineEastFIPS1801Meters = ProjectionInfo.FromEpsgCode(2802).SetNames("NAD_1983_HARN_StatePlane_Maine_East_FIPS_1801", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMaineWestFIPS1802Meters = ProjectionInfo.FromEpsgCode(2803).SetNames("NAD_1983_HARN_StatePlane_Maine_West_FIPS_1802", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMarylandFIPS1900Meters = ProjectionInfo.FromEpsgCode(2804).SetNames("NAD_1983_HARN_StatePlane_Maryland_FIPS_1900", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMassachusettsFIPS2001Meters = ProjectionInfo.FromEpsgCode(2805).SetNames("NAD_1983_HARN_StatePlane_Massachusetts_Mainland_FIPS_2001", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMassachusettsIslFIPS2002Meters = ProjectionInfo.FromEpsgCode(2806).SetNames("NAD_1983_HARN_StatePlane_Massachusetts_Island_FIPS_2002", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMichiganCentralFIPS2112Meters = ProjectionInfo.FromEpsgCode(2808).SetNames("NAD_1983_HARN_StatePlane_Michigan_Central_FIPS_2112", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMichiganNorthFIPS2111Meters = ProjectionInfo.FromEpsgCode(2807).SetNames("NAD_1983_HARN_StatePlane_Michigan_North_FIPS_2111", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMichiganSouthFIPS2113Meters = ProjectionInfo.FromEpsgCode(2809).SetNames("NAD_1983_HARN_StatePlane_Michigan_South_FIPS_2113", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMinnesotaCentralFIPS2202Meters = ProjectionInfo.FromEpsgCode(2811).SetNames("NAD_1983_HARN_StatePlane_Minnesota_Central_FIPS_2202", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMinnesotaNorthFIPS2201Meters = ProjectionInfo.FromEpsgCode(2810).SetNames("NAD_1983_HARN_StatePlane_Minnesota_North_FIPS_2201", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMinnesotaSouthFIPS2203Meters = ProjectionInfo.FromEpsgCode(2812).SetNames("NAD_1983_HARN_StatePlane_Minnesota_South_FIPS_2203", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMississippiEastFIPS2301Meters = ProjectionInfo.FromEpsgCode(2813).SetNames("NAD_1983_HARN_StatePlane_Mississippi_East_FIPS_2301", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMississippiWestFIPS2302Meters = ProjectionInfo.FromEpsgCode(2814).SetNames("NAD_1983_HARN_StatePlane_Mississippi_West_FIPS_2302", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMissouriCentralFIPS2402Meters = ProjectionInfo.FromEpsgCode(2816).SetNames("NAD_1983_HARN_StatePlane_Missouri_Central_FIPS_2402", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMissouriEastFIPS2401Meters = ProjectionInfo.FromEpsgCode(2815).SetNames("NAD_1983_HARN_StatePlane_Missouri_East_FIPS_2401", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMissouriWestFIPS2403Meters = ProjectionInfo.FromEpsgCode(2817).SetNames("NAD_1983_HARN_StatePlane_Missouri_West_FIPS_2403", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneMontanaFIPS2500Meters = ProjectionInfo.FromEpsgCode(2818).SetNames("NAD_1983_HARN_StatePlane_Montana_FIPS_2500", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNebraskaFIPS2600Meters = ProjectionInfo.FromEpsgCode(2819).SetNames("NAD_1983_HARN_StatePlane_Nebraska_FIPS_2600", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNevadaCentralFIPS2702Meters = ProjectionInfo.FromEpsgCode(2821).SetNames("NAD_1983_HARN_StatePlane_Nevada_Central_FIPS_2702", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNevadaEastFIPS2701Meters = ProjectionInfo.FromEpsgCode(2820).SetNames("NAD_1983_HARN_StatePlane_Nevada_East_FIPS_2701", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNevadaWestFIPS2703Meters = ProjectionInfo.FromEpsgCode(2822).SetNames("NAD_1983_HARN_StatePlane_Nevada_West_FIPS_2703", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewHampshireFIPS2800Meters = ProjectionInfo.FromEpsgCode(2823).SetNames("NAD_1983_HARN_StatePlane_New_Hampshire_FIPS_2800", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewJerseyFIPS2900Meters = ProjectionInfo.FromEpsgCode(2824).SetNames("NAD_1983_HARN_StatePlane_New_Jersey_FIPS_2900", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Meters = ProjectionInfo.FromEpsgCode(2826).SetNames("NAD_1983_HARN_StatePlane_New_Mexico_Central_FIPS_3002", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001Meters = ProjectionInfo.FromEpsgCode(2825).SetNames("NAD_1983_HARN_StatePlane_New_Mexico_East_FIPS_3001", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003Meters = ProjectionInfo.FromEpsgCode(2827).SetNames("NAD_1983_HARN_StatePlane_New_Mexico_West_FIPS_3003", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102Meters = ProjectionInfo.FromEpsgCode(2829).SetNames("NAD_1983_HARN_StatePlane_New_York_Central_FIPS_3102", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkEastFIPS3101Meters = ProjectionInfo.FromEpsgCode(2828).SetNames("NAD_1983_HARN_StatePlane_New_York_East_FIPS_3101", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkLongIslFIPS3104Meters = ProjectionInfo.FromEpsgCode(2831).SetNames("NAD_1983_HARN_StatePlane_New_York_Long_Island_FIPS_3104", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNewYorkWestFIPS3103Meters = ProjectionInfo.FromEpsgCode(2830).SetNames("NAD_1983_HARN_StatePlane_New_York_West_FIPS_3103", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNorthCarolinaFIPS3200Meters = ProjectionInfo.FromEpsgCode(3358).SetNames("NAD_1983_HARN_StatePlane_North_Carolina_FIPS_3200", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNorthDakotaNFIPS3301Meters = ProjectionInfo.FromEpsgCode(2832).SetNames("NAD_1983_HARN_StatePlane_North_Dakota_North_FIPS_3301", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneNorthDakotaSFIPS3302Meters = ProjectionInfo.FromEpsgCode(2833).SetNames("NAD_1983_HARN_StatePlane_North_Dakota_South_FIPS_3302", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOhioNorthFIPS3401Meters = ProjectionInfo.FromEpsgCode(2834).SetNames("NAD_1983_HARN_StatePlane_Ohio_North_FIPS_3401", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOhioSouthFIPS3402Meters = ProjectionInfo.FromEpsgCode(2835).SetNames("NAD_1983_HARN_StatePlane_Ohio_South_FIPS_3402", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501Meters = ProjectionInfo.FromEpsgCode(2836).SetNames("NAD_1983_HARN_StatePlane_Oklahoma_North_FIPS_3501", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502Meters = ProjectionInfo.FromEpsgCode(2837).SetNames("NAD_1983_HARN_StatePlane_Oklahoma_South_FIPS_3502", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOregonNorthFIPS3601Meters = ProjectionInfo.FromEpsgCode(2838).SetNames("NAD_1983_HARN_StatePlane_Oregon_North_FIPS_3601", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneOregonSouthFIPS3602Meters = ProjectionInfo.FromEpsgCode(2839).SetNames("NAD_1983_HARN_StatePlane_Oregon_South_FIPS_3602", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlanePennsylvaniaNorthFIPS3701Meters = ProjectionInfo.FromEpsgCode(3362).SetNames("NAD_1983_HARN_StatePlane_Pennsylvania_North_FIPS_3701", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlanePennsylvaniaSouthFIPS3702Meters = ProjectionInfo.FromEpsgCode(3364).SetNames("NAD_1983_HARN_StatePlane_Pennsylvania_South_FIPS_3702", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlanePRVIFIPS5200Meters = ProjectionInfo.FromEpsgCode(2866).SetNames("NAD_1983_HARN_StatePlane_Puerto_Rico_Virgin_Islands_FIPS_5200", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneRhodeIslandFIPS3800Meters = ProjectionInfo.FromEpsgCode(2840).SetNames("NAD_1983_HARN_StatePlane_Rhode_Island_FIPS_3800", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneSouthCarolinaFIPS3900Meters = ProjectionInfo.FromEpsgCode(3360).SetNames("NAD_1983_HARN_StatePlane_South_Carolina_FIPS_3900", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneSouthDakotaNFIPS4001Meters = ProjectionInfo.FromEpsgCode(2841).SetNames("NAD_1983_HARN_StatePlane_South_Dakota_North_FIPS_4001", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneSouthDakotaSFIPS4002Meters = ProjectionInfo.FromEpsgCode(2842).SetNames("NAD_1983_HARN_StatePlane_South_Dakota_South_FIPS_4002", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTennesseeFIPS4100Meters = ProjectionInfo.FromEpsgCode(2843).SetNames("NAD_1983_HARN_StatePlane_Tennessee_FIPS_4100", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasCentralFIPS4203Meters = ProjectionInfo.FromEpsgCode(2846).SetNames("NAD_1983_HARN_StatePlane_Texas_Central_FIPS_4203", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasNCentralFIPS4202Meters = ProjectionInfo.FromEpsgCode(2845).SetNames("NAD_1983_HARN_StatePlane_Texas_North_Central_FIPS_4202", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasNorthFIPS4201Meters = ProjectionInfo.FromEpsgCode(2844).SetNames("NAD_1983_HARN_StatePlane_Texas_North_FIPS_4201", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasSCentralFIPS4204Meters = ProjectionInfo.FromEpsgCode(2847).SetNames("NAD_1983_HARN_StatePlane_Texas_South_Central_FIPS_4204", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneTexasSouthFIPS4205Meters = ProjectionInfo.FromEpsgCode(2848).SetNames("NAD_1983_HARN_StatePlane_Texas_South_FIPS_4205", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahCentralFIPS4302Meters = ProjectionInfo.FromEpsgCode(2850).SetNames("NAD_1983_HARN_StatePlane_Utah_Central_FIPS_4302", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahNorthFIPS4301Meters = ProjectionInfo.FromEpsgCode(2849).SetNames("NAD_1983_HARN_StatePlane_Utah_North_FIPS_4301", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneUtahSouthFIPS4303Meters = ProjectionInfo.FromEpsgCode(2851).SetNames("NAD_1983_HARN_StatePlane_Utah_South_FIPS_4303", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneVermontFIPS4400Meters = ProjectionInfo.FromEpsgCode(2852).SetNames("NAD_1983_HARN_StatePlane_Vermont_FIPS_4400", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501Meters = ProjectionInfo.FromEpsgCode(2853).SetNames("NAD_1983_HARN_StatePlane_Virginia_North_FIPS_4501", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502Meters = ProjectionInfo.FromEpsgCode(2854).SetNames("NAD_1983_HARN_StatePlane_Virginia_South_FIPS_4502", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601Meters = ProjectionInfo.FromEpsgCode(2855).SetNames("NAD_1983_HARN_StatePlane_Washington_North_FIPS_4601", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602Meters = ProjectionInfo.FromEpsgCode(2856).SetNames("NAD_1983_HARN_StatePlane_Washington_South_FIPS_4602", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWestVirginiaNFIPS4701Meters = ProjectionInfo.FromEpsgCode(2857).SetNames("NAD_1983_HARN_StatePlane_West_Virginia_North_FIPS_4701", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWestVirginiaSFIPS4702Meters = ProjectionInfo.FromEpsgCode(2858).SetNames("NAD_1983_HARN_StatePlane_West_Virginia_South_FIPS_4702", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802Meters = ProjectionInfo.FromEpsgCode(2860).SetNames("NAD_1983_HARN_StatePlane_Wisconsin_Central_FIPS_4802", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801Meters = ProjectionInfo.FromEpsgCode(2859).SetNames("NAD_1983_HARN_StatePlane_Wisconsin_North_FIPS_4801", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803Meters = ProjectionInfo.FromEpsgCode(2861).SetNames("NAD_1983_HARN_StatePlane_Wisconsin_South_FIPS_4803", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingEastFIPS4901Meters = ProjectionInfo.FromEpsgCode(2862).SetNames("NAD_1983_HARN_StatePlane_Wyoming_East_FIPS_4901", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingECentralFIPS4902Meters = ProjectionInfo.FromEpsgCode(2863).SetNames("NAD_1983_HARN_StatePlane_Wyoming_East_Central_FIPS_4902", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingWCentralFIPS4903Meters = ProjectionInfo.FromEpsgCode(2864).SetNames("NAD_1983_HARN_StatePlane_Wyoming_West_Central_FIPS_4903", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNStatePlaneWyomingWestFIPS4904Meters = ProjectionInfo.FromEpsgCode(2865).SetNames("NAD_1983_HARN_StatePlane_Wyoming_West_FIPS_4904", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
        }

        #endregion
    }
}

#pragma warning restore 1591