// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:57:57 PM
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
    /// This class contains predefined CoordinateSystems for NAD1983Meters.
    /// </summary>
    public class NAD1983Meters : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983Maine2000CentralZoneMeters;
        public readonly ProjectionInfo NAD1983Maine2000EastZoneMeters;
        public readonly ProjectionInfo NAD1983Maine2000WestZoneMeters;
        public readonly ProjectionInfo NAD1983StatePlaneAlabamaEastFIPS0101Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlabamaWestFIPS0102Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska10FIPS5010Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska1FIPS5001Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska2FIPS5002Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska3FIPS5003Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska4FIPS5004Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska5FIPS5005Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska6FIPS5006Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska7FIPS5007Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska8FIPS5008Meters;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska9FIPS5009Meters;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaCentralFIPS0202Meters;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaEastFIPS0201Meters;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaWestFIPS0203Meters;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasNorthFIPS0301Meters;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasSouthFIPS0302Meters;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIFIPS0401Meters;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIFIPS0402Meters;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIIFIPS0403Meters;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIVFIPS0404Meters;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVFIPS0405Meters;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVIFIPS0406Meters;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoCentralFIPS0502Meters;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoNorthFIPS0501Meters;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoSouthFIPS0503Meters;
        public readonly ProjectionInfo NAD1983StatePlaneConnecticutFIPS0600Meters;
        public readonly ProjectionInfo NAD1983StatePlaneDelawareFIPS0700Meters;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaEastFIPS0901Meters;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaNorthFIPS0903Meters;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaWestFIPS0902Meters;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaEastFIPS1001Meters;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaWestFIPS1002Meters;
        public readonly ProjectionInfo NAD1983StatePlaneGuamFIPS5400Meters;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii1FIPS5101Meters;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii2FIPS5102Meters;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii3FIPS5103Meters;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii4FIPS5104Meters;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii5FIPS5105Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoCentralFIPS1102Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoEastFIPS1101Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoWestFIPS1103Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisEastFIPS1201Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisWestFIPS1202Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaEastFIPS1301Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaWestFIPS1302Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIowaNorthFIPS1401Meters;
        public readonly ProjectionInfo NAD1983StatePlaneIowaSouthFIPS1402Meters;
        public readonly ProjectionInfo NAD1983StatePlaneKansasNorthFIPS1501Meters;
        public readonly ProjectionInfo NAD1983StatePlaneKansasSouthFIPS1502Meters;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyFIPS1600Meters;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyNorthFIPS1601Meters;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckySouthFIPS1602Meters;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaNorthFIPS1701Meters;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaOffshoreFIPS1703Meters;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaSouthFIPS1702Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMaineEastFIPS1801Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMaineWestFIPS1802Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMarylandFIPS1900Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsFIPS2001Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsIslFIPS2002Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganCentralFIPS2112Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganNorthFIPS2111Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganSouthFIPS2113Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaCentralFIPS2202Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaNorthFIPS2201Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaSouthFIPS2203Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiEastFIPS2301Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiWestFIPS2302Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriCentralFIPS2402Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriEastFIPS2401Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriWestFIPS2403Meters;
        public readonly ProjectionInfo NAD1983StatePlaneMontanaFIPS2500Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNebraskaFIPS2600Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaCentralFIPS2702Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaEastFIPS2701Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaWestFIPS2703Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewHampshireFIPS2800Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewJerseyFIPS2900Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoCentralFIPS3002Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoEastFIPS3001Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoWestFIPS3003Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkCentralFIPS3102Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkEastFIPS3101Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkLongIslFIPS3104Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkWestFIPS3103Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNorthCarolinaFIPS3200Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaNFIPS3301Meters;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaSFIPS3302Meters;
        public readonly ProjectionInfo NAD1983StatePlaneOhioNorthFIPS3401Meters;
        public readonly ProjectionInfo NAD1983StatePlaneOhioSouthFIPS3402Meters;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaNorthFIPS3501Meters;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaSouthFIPS3502Meters;
        public readonly ProjectionInfo NAD1983StatePlaneOregonNorthFIPS3601Meters;
        public readonly ProjectionInfo NAD1983StatePlaneOregonSouthFIPS3602Meters;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaNorthFIPS3701Meters;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaSouthFIPS3702Meters;
        public readonly ProjectionInfo NAD1983StatePlanePuertoRicoVirginIslFIPS5200Meters;
        public readonly ProjectionInfo NAD1983StatePlaneRhodeIslandFIPS3800Meters;
        public readonly ProjectionInfo NAD1983StatePlaneSouthCarolinaFIPS3900Meters;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaNFIPS4001Meters;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaSFIPS4002Meters;
        public readonly ProjectionInfo NAD1983StatePlaneTennesseeFIPS4100Meters;
        public readonly ProjectionInfo NAD1983StatePlaneTexasCentralFIPS4203Meters;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNCentralFIPS4202Meters;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNorthFIPS4201Meters;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSCentralFIPS4204Meters;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSouthFIPS4205Meters;
        public readonly ProjectionInfo NAD1983StatePlaneUtahCentralFIPS4302Meters;
        public readonly ProjectionInfo NAD1983StatePlaneUtahNorthFIPS4301Meters;
        public readonly ProjectionInfo NAD1983StatePlaneUtahSouthFIPS4303Meters;
        public readonly ProjectionInfo NAD1983StatePlaneVermontFIPS4400Meters;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaNorthFIPS4501Meters;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaSouthFIPS4502Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonNorthFIPS4601Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonSouthFIPS4602Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaNFIPS4701Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaSFIPS4702Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinCentralFIPS4802Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinNorthFIPS4801Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinSouthFIPS4803Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingEastFIPS4901Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingECentralFIPS4902Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWCentralFIPS4903Meters;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWestFIPS4904Meters;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983Meters.
        /// </summary>
        public NAD1983Meters()
        {
            NAD1983Maine2000CentralZoneMeters = ProjectionInfo.FromEpsgCode(3463).SetNames("NAD_1983_Maine_2000_Central_Zone", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983Maine2000EastZoneMeters = ProjectionInfo.FromEpsgCode(3072).SetNames("NAD_1983_Maine_2000_East_Zone", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983Maine2000WestZoneMeters = ProjectionInfo.FromEpsgCode(3074).SetNames("NAD_1983_Maine_2000_West_Zone", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlabamaEastFIPS0101Meters = ProjectionInfo.FromEpsgCode(26929).SetNames("NAD_1983_StatePlane_Alabama_East_FIPS_0101", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlabamaWestFIPS0102Meters = ProjectionInfo.FromEpsgCode(26930).SetNames("NAD_1983_StatePlane_Alabama_West_FIPS_0102", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska10FIPS5010Meters = ProjectionInfo.FromEpsgCode(26940).SetNames("NAD_1983_StatePlane_Alaska_10_FIPS_5010", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska1FIPS5001Meters = ProjectionInfo.FromEpsgCode(26931).SetNames("NAD_1983_StatePlane_Alaska_1_FIPS_5001", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska2FIPS5002Meters = ProjectionInfo.FromEpsgCode(26932).SetNames("NAD_1983_StatePlane_Alaska_2_FIPS_5002", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska3FIPS5003Meters = ProjectionInfo.FromEpsgCode(26933).SetNames("NAD_1983_StatePlane_Alaska_3_FIPS_5003", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska4FIPS5004Meters = ProjectionInfo.FromEpsgCode(26934).SetNames("NAD_1983_StatePlane_Alaska_4_FIPS_5004", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska5FIPS5005Meters = ProjectionInfo.FromEpsgCode(26935).SetNames("NAD_1983_StatePlane_Alaska_5_FIPS_5005", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska6FIPS5006Meters = ProjectionInfo.FromEpsgCode(26936).SetNames("NAD_1983_StatePlane_Alaska_6_FIPS_5006", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska7FIPS5007Meters = ProjectionInfo.FromEpsgCode(26937).SetNames("NAD_1983_StatePlane_Alaska_7_FIPS_5007", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska8FIPS5008Meters = ProjectionInfo.FromEpsgCode(26938).SetNames("NAD_1983_StatePlane_Alaska_8_FIPS_5008", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska9FIPS5009Meters = ProjectionInfo.FromEpsgCode(26939).SetNames("NAD_1983_StatePlane_Alaska_9_FIPS_5009", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaCentralFIPS0202Meters = ProjectionInfo.FromEpsgCode(26949).SetNames("NAD_1983_StatePlane_Arizona_Central_FIPS_0202", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaEastFIPS0201Meters = ProjectionInfo.FromEpsgCode(26948).SetNames("NAD_1983_StatePlane_Arizona_East_FIPS_0201", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaWestFIPS0203Meters = ProjectionInfo.FromEpsgCode(26950).SetNames("NAD_1983_StatePlane_Arizona_West_FIPS_0203", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArkansasNorthFIPS0301Meters = ProjectionInfo.FromEpsgCode(26951).SetNames("NAD_1983_StatePlane_Arkansas_North_FIPS_0301", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArkansasSouthFIPS0302Meters = ProjectionInfo.FromEpsgCode(26952).SetNames("NAD_1983_StatePlane_Arkansas_South_FIPS_0302", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIFIPS0401Meters = ProjectionInfo.FromEpsgCode(26941).SetNames("NAD_1983_StatePlane_California_I_FIPS_0401", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIIFIPS0402Meters = ProjectionInfo.FromEpsgCode(26942).SetNames("NAD_1983_StatePlane_California_II_FIPS_0402", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIIIFIPS0403Meters = ProjectionInfo.FromEpsgCode(26943).SetNames("NAD_1983_StatePlane_California_III_FIPS_0403", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIVFIPS0404Meters = ProjectionInfo.FromEpsgCode(26944).SetNames("NAD_1983_StatePlane_California_IV_FIPS_0404", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaVFIPS0405Meters = ProjectionInfo.FromEpsgCode(26945).SetNames("NAD_1983_StatePlane_California_V_FIPS_0405", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaVIFIPS0406Meters = ProjectionInfo.FromEpsgCode(26946).SetNames("NAD_1983_StatePlane_California_VI_FIPS_0406", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneColoradoCentralFIPS0502Meters = ProjectionInfo.FromEpsgCode(26954).SetNames("NAD_1983_StatePlane_Colorado_Central_FIPS_0502", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneColoradoNorthFIPS0501Meters = ProjectionInfo.FromEpsgCode(26953).SetNames("NAD_1983_StatePlane_Colorado_North_FIPS_0501", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneColoradoSouthFIPS0503Meters = ProjectionInfo.FromEpsgCode(26955).SetNames("NAD_1983_StatePlane_Colorado_South_FIPS_0503", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneConnecticutFIPS0600Meters = ProjectionInfo.FromEpsgCode(26956).SetNames("NAD_1983_StatePlane_Connecticut_FIPS_0600", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneDelawareFIPS0700Meters = ProjectionInfo.FromEpsgCode(26957).SetNames("NAD_1983_StatePlane_Delaware_FIPS_0700", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneFloridaEastFIPS0901Meters = ProjectionInfo.FromEpsgCode(26958).SetNames("NAD_1983_StatePlane_Florida_East_FIPS_0901", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneFloridaNorthFIPS0903Meters = ProjectionInfo.FromEpsgCode(26960).SetNames("NAD_1983_StatePlane_Florida_North_FIPS_0903", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneFloridaWestFIPS0902Meters = ProjectionInfo.FromEpsgCode(26959).SetNames("NAD_1983_StatePlane_Florida_West_FIPS_0902", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneGeorgiaEastFIPS1001Meters = ProjectionInfo.FromEpsgCode(26966).SetNames("NAD_1983_StatePlane_Georgia_East_FIPS_1001", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneGeorgiaWestFIPS1002Meters = ProjectionInfo.FromEpsgCode(26967).SetNames("NAD_1983_StatePlane_Georgia_West_FIPS_1002", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneGuamFIPS5400Meters = ProjectionInfo.FromAuthorityCode("ESRI", 65161).SetNames("NAD_1983_StatePlane_Guam_FIPS_5400", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii1FIPS5101Meters = ProjectionInfo.FromEpsgCode(26961).SetNames("NAD_1983_StatePlane_Hawaii_1_FIPS_5101", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii2FIPS5102Meters = ProjectionInfo.FromEpsgCode(26962).SetNames("NAD_1983_StatePlane_Hawaii_2_FIPS_5102", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii3FIPS5103Meters = ProjectionInfo.FromEpsgCode(26963).SetNames("NAD_1983_StatePlane_Hawaii_3_FIPS_5103", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii4FIPS5104Meters = ProjectionInfo.FromEpsgCode(26964).SetNames("NAD_1983_StatePlane_Hawaii_4_FIPS_5104", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii5FIPS5105Meters = ProjectionInfo.FromEpsgCode(26965).SetNames("NAD_1983_StatePlane_Hawaii_5_FIPS_5105", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIdahoCentralFIPS1102Meters = ProjectionInfo.FromEpsgCode(26969).SetNames("NAD_1983_StatePlane_Idaho_Central_FIPS_1102", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIdahoEastFIPS1101Meters = ProjectionInfo.FromEpsgCode(26968).SetNames("NAD_1983_StatePlane_Idaho_East_FIPS_1101", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIdahoWestFIPS1103Meters = ProjectionInfo.FromEpsgCode(26970).SetNames("NAD_1983_StatePlane_Idaho_West_FIPS_1103", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIllinoisEastFIPS1201Meters = ProjectionInfo.FromEpsgCode(26971).SetNames("NAD_1983_StatePlane_Illinois_East_FIPS_1201", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIllinoisWestFIPS1202Meters = ProjectionInfo.FromEpsgCode(26972).SetNames("NAD_1983_StatePlane_Illinois_West_FIPS_1202", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIndianaEastFIPS1301Meters = ProjectionInfo.FromEpsgCode(26973).SetNames("NAD_1983_StatePlane_Indiana_East_FIPS_1301", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIndianaWestFIPS1302Meters = ProjectionInfo.FromEpsgCode(26974).SetNames("NAD_1983_StatePlane_Indiana_West_FIPS_1302", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIowaNorthFIPS1401Meters = ProjectionInfo.FromEpsgCode(26975).SetNames("NAD_1983_StatePlane_Iowa_North_FIPS_1401", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIowaSouthFIPS1402Meters = ProjectionInfo.FromEpsgCode(26976).SetNames("NAD_1983_StatePlane_Iowa_South_FIPS_1402", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKansasNorthFIPS1501Meters = ProjectionInfo.FromEpsgCode(26977).SetNames("NAD_1983_StatePlane_Kansas_North_FIPS_1501", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKansasSouthFIPS1502Meters = ProjectionInfo.FromEpsgCode(26978).SetNames("NAD_1983_StatePlane_Kansas_South_FIPS_1502", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKentuckyFIPS1600Meters = ProjectionInfo.FromEpsgCode(3088).SetNames("NAD_1983_StatePlane_Kentucky_FIPS_1600", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKentuckyNorthFIPS1601Meters = ProjectionInfo.FromEpsgCode(2205).SetNames("NAD_1983_StatePlane_Kentucky_North_FIPS_1601", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKentuckySouthFIPS1602Meters = ProjectionInfo.FromEpsgCode(26980).SetNames("NAD_1983_StatePlane_Kentucky_South_FIPS_1602", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneLouisianaNorthFIPS1701Meters = ProjectionInfo.FromEpsgCode(26981).SetNames("NAD_1983_StatePlane_Louisiana_North_FIPS_1701", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneLouisianaOffshoreFIPS1703Meters = ProjectionInfo.FromEpsgCode(32199).SetNames("NAD_1983_StatePlane_Louisiana_Offshore_FIPS_1703", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneLouisianaSouthFIPS1702Meters = ProjectionInfo.FromEpsgCode(26982).SetNames("NAD_1983_StatePlane_Louisiana_South_FIPS_1702", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMaineEastFIPS1801Meters = ProjectionInfo.FromEpsgCode(26983).SetNames("NAD_1983_StatePlane_Maine_East_FIPS_1801", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMaineWestFIPS1802Meters = ProjectionInfo.FromEpsgCode(26984).SetNames("NAD_1983_StatePlane_Maine_West_FIPS_1802", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMarylandFIPS1900Meters = ProjectionInfo.FromEpsgCode(26985).SetNames("NAD_1983_StatePlane_Maryland_FIPS_1900", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMassachusettsFIPS2001Meters = ProjectionInfo.FromEpsgCode(26986).SetNames("NAD_1983_StatePlane_Massachusetts_Mainland_FIPS_2001", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMassachusettsIslFIPS2002Meters = ProjectionInfo.FromEpsgCode(26987).SetNames("NAD_1983_StatePlane_Massachusetts_Island_FIPS_2002", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganCentralFIPS2112Meters = ProjectionInfo.FromEpsgCode(26989).SetNames("NAD_1983_StatePlane_Michigan_Central_FIPS_2112", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganNorthFIPS2111Meters = ProjectionInfo.FromEpsgCode(26988).SetNames("NAD_1983_StatePlane_Michigan_North_FIPS_2111", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganSouthFIPS2113Meters = ProjectionInfo.FromEpsgCode(26990).SetNames("NAD_1983_StatePlane_Michigan_South_FIPS_2113", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMinnesotaCentralFIPS2202Meters = ProjectionInfo.FromEpsgCode(26992).SetNames("NAD_1983_StatePlane_Minnesota_Central_FIPS_2202", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMinnesotaNorthFIPS2201Meters = ProjectionInfo.FromEpsgCode(26991).SetNames("NAD_1983_StatePlane_Minnesota_North_FIPS_2201", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMinnesotaSouthFIPS2203Meters = ProjectionInfo.FromEpsgCode(26993).SetNames("NAD_1983_StatePlane_Minnesota_South_FIPS_2203", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMississippiEastFIPS2301Meters = ProjectionInfo.FromEpsgCode(26994).SetNames("NAD_1983_StatePlane_Mississippi_East_FIPS_2301", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMississippiWestFIPS2302Meters = ProjectionInfo.FromEpsgCode(26995).SetNames("NAD_1983_StatePlane_Mississippi_West_FIPS_2302", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMissouriCentralFIPS2402Meters = ProjectionInfo.FromEpsgCode(26997).SetNames("NAD_1983_StatePlane_Missouri_Central_FIPS_2402", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMissouriEastFIPS2401Meters = ProjectionInfo.FromEpsgCode(26996).SetNames("NAD_1983_StatePlane_Missouri_East_FIPS_2401", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMissouriWestFIPS2403Meters = ProjectionInfo.FromEpsgCode(26998).SetNames("NAD_1983_StatePlane_Missouri_West_FIPS_2403", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMontanaFIPS2500Meters = ProjectionInfo.FromEpsgCode(32100).SetNames("NAD_1983_StatePlane_Montana_FIPS_2500", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNebraskaFIPS2600Meters = ProjectionInfo.FromEpsgCode(32104).SetNames("NAD_1983_StatePlane_Nebraska_FIPS_2600", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNevadaCentralFIPS2702Meters = ProjectionInfo.FromEpsgCode(32108).SetNames("NAD_1983_StatePlane_Nevada_Central_FIPS_2702", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNevadaEastFIPS2701Meters = ProjectionInfo.FromEpsgCode(32107).SetNames("NAD_1983_StatePlane_Nevada_East_FIPS_2701", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNevadaWestFIPS2703Meters = ProjectionInfo.FromEpsgCode(32109).SetNames("NAD_1983_StatePlane_Nevada_West_FIPS_2703", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewHampshireFIPS2800Meters = ProjectionInfo.FromEpsgCode(32110).SetNames("NAD_1983_StatePlane_New_Hampshire_FIPS_2800", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewJerseyFIPS2900Meters = ProjectionInfo.FromEpsgCode(32111).SetNames("NAD_1983_StatePlane_New_Jersey_FIPS_2900", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewMexicoCentralFIPS3002Meters = ProjectionInfo.FromEpsgCode(32113).SetNames("NAD_1983_StatePlane_New_Mexico_Central_FIPS_3002", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewMexicoEastFIPS3001Meters = ProjectionInfo.FromEpsgCode(32112).SetNames("NAD_1983_StatePlane_New_Mexico_East_FIPS_3001", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewMexicoWestFIPS3003Meters = ProjectionInfo.FromEpsgCode(32114).SetNames("NAD_1983_StatePlane_New_Mexico_West_FIPS_3003", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkCentralFIPS3102Meters = ProjectionInfo.FromEpsgCode(32116).SetNames("NAD_1983_StatePlane_New_York_Central_FIPS_3102", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkEastFIPS3101Meters = ProjectionInfo.FromEpsgCode(32115).SetNames("NAD_1983_StatePlane_New_York_East_FIPS_3101", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkLongIslFIPS3104Meters = ProjectionInfo.FromEpsgCode(32118).SetNames("NAD_1983_StatePlane_New_York_Long_Island_FIPS_3104", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkWestFIPS3103Meters = ProjectionInfo.FromEpsgCode(32117).SetNames("NAD_1983_StatePlane_New_York_West_FIPS_3103", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthCarolinaFIPS3200Meters = ProjectionInfo.FromEpsgCode(32119).SetNames("NAD_1983_StatePlane_North_Carolina_FIPS_3200", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthDakotaNFIPS3301Meters = ProjectionInfo.FromEpsgCode(32120).SetNames("NAD_1983_StatePlane_North_Dakota_North_FIPS_3301", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthDakotaSFIPS3302Meters = ProjectionInfo.FromEpsgCode(32121).SetNames("NAD_1983_StatePlane_North_Dakota_South_FIPS_3302", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOhioNorthFIPS3401Meters = ProjectionInfo.FromEpsgCode(32122).SetNames("NAD_1983_StatePlane_Ohio_North_FIPS_3401", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOhioSouthFIPS3402Meters = ProjectionInfo.FromEpsgCode(32123).SetNames("NAD_1983_StatePlane_Ohio_South_FIPS_3402", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOklahomaNorthFIPS3501Meters = ProjectionInfo.FromEpsgCode(32124).SetNames("NAD_1983_StatePlane_Oklahoma_North_FIPS_3501", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOklahomaSouthFIPS3502Meters = ProjectionInfo.FromEpsgCode(32125).SetNames("NAD_1983_StatePlane_Oklahoma_South_FIPS_3502", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOregonNorthFIPS3601Meters = ProjectionInfo.FromEpsgCode(32126).SetNames("NAD_1983_StatePlane_Oregon_North_FIPS_3601", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOregonSouthFIPS3602Meters = ProjectionInfo.FromEpsgCode(32127).SetNames("NAD_1983_StatePlane_Oregon_South_FIPS_3602", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlanePennsylvaniaNorthFIPS3701Meters = ProjectionInfo.FromEpsgCode(32128).SetNames("NAD_1983_StatePlane_Pennsylvania_North_FIPS_3701", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlanePennsylvaniaSouthFIPS3702Meters = ProjectionInfo.FromEpsgCode(32129).SetNames("NAD_1983_StatePlane_Pennsylvania_South_FIPS_3702", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlanePuertoRicoVirginIslFIPS5200Meters = ProjectionInfo.FromEpsgCode(32161).SetNames("NAD_1983_StatePlane_Puerto_Rico_Virgin_Islands_FIPS_5200", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneRhodeIslandFIPS3800Meters = ProjectionInfo.FromEpsgCode(32130).SetNames("NAD_1983_StatePlane_Rhode_Island_FIPS_3800", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneSouthCarolinaFIPS3900Meters = ProjectionInfo.FromEpsgCode(32133).SetNames("NAD_1983_StatePlane_South_Carolina_FIPS_3900", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneSouthDakotaNFIPS4001Meters = ProjectionInfo.FromEpsgCode(32134).SetNames("NAD_1983_StatePlane_South_Dakota_North_FIPS_4001", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneSouthDakotaSFIPS4002Meters = ProjectionInfo.FromEpsgCode(32135).SetNames("NAD_1983_StatePlane_South_Dakota_South_FIPS_4002", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTennesseeFIPS4100Meters = ProjectionInfo.FromEpsgCode(32136).SetNames("NAD_1983_StatePlane_Tennessee_FIPS_4100", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasCentralFIPS4203Meters = ProjectionInfo.FromEpsgCode(32139).SetNames("NAD_1983_StatePlane_Texas_Central_FIPS_4203", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasNCentralFIPS4202Meters = ProjectionInfo.FromEpsgCode(32138).SetNames("NAD_1983_StatePlane_Texas_North_Central_FIPS_4202", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasNorthFIPS4201Meters = ProjectionInfo.FromEpsgCode(32137).SetNames("NAD_1983_StatePlane_Texas_North_FIPS_4201", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasSCentralFIPS4204Meters = ProjectionInfo.FromEpsgCode(32140).SetNames("NAD_1983_StatePlane_Texas_South_Central_FIPS_4204", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasSouthFIPS4205Meters = ProjectionInfo.FromEpsgCode(32141).SetNames("NAD_1983_StatePlane_Texas_South_FIPS_4205", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahCentralFIPS4302Meters = ProjectionInfo.FromEpsgCode(32143).SetNames("NAD_1983_StatePlane_Utah_Central_FIPS_4302", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahNorthFIPS4301Meters = ProjectionInfo.FromEpsgCode(32142).SetNames("NAD_1983_StatePlane_Utah_North_FIPS_4301", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahSouthFIPS4303Meters = ProjectionInfo.FromEpsgCode(32144).SetNames("NAD_1983_StatePlane_Utah_South_FIPS_4303", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneVermontFIPS4400Meters = ProjectionInfo.FromEpsgCode(32145).SetNames("NAD_1983_StatePlane_Vermont_FIPS_4400", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneVirginiaNorthFIPS4501Meters = ProjectionInfo.FromEpsgCode(32146).SetNames("NAD_1983_StatePlane_Virginia_North_FIPS_4501", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneVirginiaSouthFIPS4502Meters = ProjectionInfo.FromEpsgCode(32147).SetNames("NAD_1983_StatePlane_Virginia_South_FIPS_4502", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWashingtonNorthFIPS4601Meters = ProjectionInfo.FromEpsgCode(32148).SetNames("NAD_1983_StatePlane_Washington_North_FIPS_4601", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWashingtonSouthFIPS4602Meters = ProjectionInfo.FromEpsgCode(32149).SetNames("NAD_1983_StatePlane_Washington_South_FIPS_4602", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWestVirginiaNFIPS4701Meters = ProjectionInfo.FromEpsgCode(32150).SetNames("NAD_1983_StatePlane_West_Virginia_North_FIPS_4701", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWestVirginiaSFIPS4702Meters = ProjectionInfo.FromEpsgCode(32151).SetNames("NAD_1983_StatePlane_West_Virginia_South_FIPS_4702", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWisconsinCentralFIPS4802Meters = ProjectionInfo.FromEpsgCode(32153).SetNames("NAD_1983_StatePlane_Wisconsin_Central_FIPS_4802", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWisconsinNorthFIPS4801Meters = ProjectionInfo.FromEpsgCode(32152).SetNames("NAD_1983_StatePlane_Wisconsin_North_FIPS_4801", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWisconsinSouthFIPS4803Meters = ProjectionInfo.FromEpsgCode(32154).SetNames("NAD_1983_StatePlane_Wisconsin_South_FIPS_4803", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingEastFIPS4901Meters = ProjectionInfo.FromEpsgCode(32155).SetNames("NAD_1983_StatePlane_Wyoming_East_FIPS_4901", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingECentralFIPS4902Meters = ProjectionInfo.FromEpsgCode(32156).SetNames("NAD_1983_StatePlane_Wyoming_East_Central_FIPS_4902", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingWCentralFIPS4903Meters = ProjectionInfo.FromEpsgCode(32157).SetNames("NAD_1983_StatePlane_Wyoming_West_Central_FIPS_4903", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingWestFIPS4904Meters = ProjectionInfo.FromEpsgCode(32158).SetNames("NAD_1983_StatePlane_Wyoming_West_FIPS_4904", "GCS_North_American_1983", "D_North_American_1983");
        }

        #endregion
    }
}

#pragma warning restore 1591