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
    /// This class contains predefined CoordinateSystems for NAD1983NSRS2007Meters.
    /// </summary>
    public class NAD1983NSRS2007Meters : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983NSRS2007Maine2000CentralZoneMeters;
        public readonly ProjectionInfo NAD1983NSRS2007Maine2000EastZoneMeters;
        public readonly ProjectionInfo NAD1983NSRS2007Maine2000WestZoneMeters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlabamaEastFIPS0101Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlabamaWestFIPS0102Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska10FIPS5010Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska1FIPS5001Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska2FIPS5002Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska3FIPS5003Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska4FIPS5004Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska5FIPS5005Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska6FIPS5006Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska7FIPS5007Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska8FIPS5008Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneAlaska9FIPS5009Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArizonaCentralFIPS0202Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArizonaEastFIPS0201Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArizonaWestFIPS0203Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArkansasNorthFIPS0301Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArkansasSouthFIPS0302Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIFIPS0401Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIIFIPS0402Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIIIFIPS0403Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIVFIPS0404Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaVFIPS0405Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaVIFIPS0406Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneColoradoCentralFIPS0502Meter;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneColoradoNorthFIPS0501Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneColoradoSouthFIPS0503Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneConnecticutFIPS0600Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneDelawareFIPS0700Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneFloridaEastFIPS0901Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneFloridaNorthFIPS0903Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneFloridaWestFIPS0902Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneGeorgiaEastFIPS1001Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneGeorgiaWestFIPS1002Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIdahoCentralFIPS1102Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIdahoEastFIPS1101Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIdahoWestFIPS1103Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIllinoisEastFIPS1201Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIllinoisWestFIPS1202Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIndianaEastFIPS1301Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIndianaWestFIPS1302Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIowaNorthFIPS1401Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIowaSouthFIPS1402Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKansasNorthFIPS1501Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKansasSouthFIPS1502Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKentuckyFIPS1600Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKentuckyNorthFIPS1601Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKentuckySouthFIPS1602Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneLouisianaNorthFIPS1701Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneLouisianaSouthFIPS1702Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMaineEastFIPS1801Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMaineWestFIPS1802Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMarylandFIPS1900Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMassachusettsFIPS2001Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMassachusettsIslFIPS2002M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMichiganCentralFIPS2112M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMichiganNorthFIPS2111Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMichiganSouthFIPS2113Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMinnesotaCentralFIPS2202M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMinnesotaNorthFIPS2201Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMinnesotaSouthFIPS2203Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMississippiEastFIPS2301M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMississippiWestFIPS2302M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMissouriCentralFIPS2402M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMissouriEastFIPS2401Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMissouriWestFIPS2403Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMontanaFIPS2500Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNebraskaFIPS2600Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNevadaCentralFIPS2702Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNevadaEastFIPS2701Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNevadaWestFIPS2703Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewHampshireFIPS2800Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewJerseyFIPS2900Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewMexicoCentralFIPS3002M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewMexicoEastFIPS3001Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewMexicoWestFIPS3003Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkCentralFIPS3102M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkEastFIPS3101Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkLongIslFIPS3104M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkWestFIPS3103Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNorthCarolinaFIPS3200Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNorthDakotaNFIPS3301Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNorthDakotaSFIPS3302Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOhioNorthFIPS3401Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOhioSouthFIPS3402Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOklahomaNorthFIPS3501Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOklahomaSouthFIPS3502Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOregonNorthFIPS3601Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOregonSouthFIPS3602Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlanePennsylvaniaNorthFIPS3701M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlanePennsylvaniaSouthFIPS3702M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlanePRVIFIPS5200M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneRhodeIslandFIPS3800Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneSouthCarolinaFIPS3900Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneSouthDakotaNFIPS4001Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneSouthDakotaSFIPS4002Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTennesseeFIPS4100Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasCentralFIPS4203Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasNCentralFIPS4202Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasNorthFIPS4201Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasSCentralFIPS4204Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasSouthFIPS4205Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahCentralFIPS4302Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahNorthFIPS4301Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahSouthFIPS4303Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneVermontFIPS4400Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneVirginiaNorthFIPS4501Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneVirginiaSouthFIPS4502Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWashingtonNorthFIPS4601M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWashingtonSouthFIPS4602M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWestVirginiaNFIPS4701Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWestVirginiaSFIPS4702Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWisconsinCentralFIPS4802M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWisconsinNorthFIPS4801Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWisconsinSouthFIPS4803Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingEastFIPS4901Meters;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingECentralFIPS4902M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingWCentralFIPS4903M;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingWestFIPS4904Meters;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983NSRS2007Meters.
        /// </summary>
        public NAD1983NSRS2007Meters()
        {
            NAD1983NSRS2007Maine2000CentralZoneMeters = ProjectionInfo.FromEpsgCode(3554).SetNames("NAD_1983_NSRS2007_Maine_2000_Central_Zone", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007Maine2000EastZoneMeters = ProjectionInfo.FromEpsgCode(3555).SetNames("NAD_1983_NSRS2007_Maine_2000_East_Zone", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007Maine2000WestZoneMeters = ProjectionInfo.FromEpsgCode(3556).SetNames("NAD_1983_NSRS2007_Maine_2000_West_Zone", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlabamaEastFIPS0101Meters = ProjectionInfo.FromEpsgCode(3465).SetNames("NAD_1983_NSRS2007_StatePlane_Alabama_East_FIPS_0101", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlabamaWestFIPS0102Meters = ProjectionInfo.FromEpsgCode(3466).SetNames("NAD_1983_NSRS2007_StatePlane_Alabama_West_FIPS_0102", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska10FIPS5010Meters = ProjectionInfo.FromEpsgCode(3477).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_10_FIPS_5010", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska1FIPS5001Meters = ProjectionInfo.FromEpsgCode(3468).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_1_FIPS_5001", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska2FIPS5002Meters = ProjectionInfo.FromEpsgCode(3469).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_2_FIPS_5002", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska3FIPS5003Meters = ProjectionInfo.FromEpsgCode(3470).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_3_FIPS_5003", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska4FIPS5004Meters = ProjectionInfo.FromEpsgCode(3471).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_4_FIPS_5004", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska5FIPS5005Meters = ProjectionInfo.FromEpsgCode(3472).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_5_FIPS_5005", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska6FIPS5006Meters = ProjectionInfo.FromEpsgCode(3473).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_6_FIPS_5006", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska7FIPS5007Meters = ProjectionInfo.FromEpsgCode(3474).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_7_FIPS_5007", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska8FIPS5008Meters = ProjectionInfo.FromEpsgCode(3475).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_8_FIPS_5008", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneAlaska9FIPS5009Meters = ProjectionInfo.FromEpsgCode(3476).SetNames("NAD_1983_NSRS2007_StatePlane_Alaska_9_FIPS_5009", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArizonaCentralFIPS0202Meters = ProjectionInfo.FromEpsgCode(3478).SetNames("NAD_1983_NSRS2007_StatePlane_Arizona_Central_FIPS_0202", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArizonaEastFIPS0201Meters = ProjectionInfo.FromEpsgCode(3480).SetNames("NAD_1983_NSRS2007_StatePlane_Arizona_East_FIPS_0201", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArizonaWestFIPS0203Meters = ProjectionInfo.FromEpsgCode(3482).SetNames("NAD_1983_NSRS2007_StatePlane_Arizona_West_FIPS_0203", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArkansasNorthFIPS0301Meters = ProjectionInfo.FromEpsgCode(3484).SetNames("NAD_1983_NSRS2007_StatePlane_Arkansas_North_FIPS_0301", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArkansasSouthFIPS0302Meters = ProjectionInfo.FromEpsgCode(3486).SetNames("NAD_1983_NSRS2007_StatePlane_Arkansas_South_FIPS_0302", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIFIPS0401Meters = ProjectionInfo.FromEpsgCode(3489).SetNames("NAD_1983_NSRS2007_StatePlane_California_I_FIPS_0401", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIIFIPS0402Meters = ProjectionInfo.FromEpsgCode(3491).SetNames("NAD_1983_NSRS2007_StatePlane_California_II_FIPS_0402", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIIIFIPS0403Meters = ProjectionInfo.FromEpsgCode(3493).SetNames("NAD_1983_NSRS2007_StatePlane_California_III_FIPS_0403", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIVFIPS0404Meters = ProjectionInfo.FromEpsgCode(3495).SetNames("NAD_1983_NSRS2007_StatePlane_California_IV_FIPS_0404", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaVFIPS0405Meters = ProjectionInfo.FromEpsgCode(3497).SetNames("NAD_1983_NSRS2007_StatePlane_California_V_FIPS_0405", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaVIFIPS0406Meters = ProjectionInfo.FromEpsgCode(3499).SetNames("NAD_1983_NSRS2007_StatePlane_California_VI_FIPS_0406", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneColoradoCentralFIPS0502Meter = ProjectionInfo.FromEpsgCode(3501).SetNames("NAD_1983_NSRS2007_StatePlane_Colorado_Central_FIPS_0502", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneColoradoNorthFIPS0501Meters = ProjectionInfo.FromEpsgCode(3503).SetNames("NAD_1983_NSRS2007_StatePlane_Colorado_North_FIPS_0501", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneColoradoSouthFIPS0503Meters = ProjectionInfo.FromEpsgCode(3505).SetNames("NAD_1983_NSRS2007_StatePlane_Colorado_South_FIPS_0503", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneConnecticutFIPS0600Meters = ProjectionInfo.FromEpsgCode(3507).SetNames("NAD_1983_NSRS2007_StatePlane_Connecticut_FIPS_0600", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneDelawareFIPS0700Meters = ProjectionInfo.FromEpsgCode(3509).SetNames("NAD_1983_NSRS2007_StatePlane_Delaware_FIPS_0700", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneFloridaEastFIPS0901Meters = ProjectionInfo.FromEpsgCode(3511).SetNames("NAD_1983_NSRS2007_StatePlane_Florida_East_FIPS_0901", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneFloridaNorthFIPS0903Meters = ProjectionInfo.FromEpsgCode(3514).SetNames("NAD_1983_NSRS2007_StatePlane_Florida_North_FIPS_0903", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneFloridaWestFIPS0902Meters = ProjectionInfo.FromEpsgCode(3516).SetNames("NAD_1983_NSRS2007_StatePlane_Florida_West_FIPS_0902", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneGeorgiaEastFIPS1001Meters = ProjectionInfo.FromEpsgCode(3518).SetNames("NAD_1983_NSRS2007_StatePlane_Georgia_East_FIPS_1001", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneGeorgiaWestFIPS1002Meters = ProjectionInfo.FromEpsgCode(3520).SetNames("NAD_1983_NSRS2007_StatePlane_Georgia_West_FIPS_1002", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIdahoCentralFIPS1102Meters = ProjectionInfo.FromEpsgCode(3522).SetNames("NAD_1983_NSRS2007_StatePlane_Idaho_Central_FIPS_1102", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIdahoEastFIPS1101Meters = ProjectionInfo.FromEpsgCode(3524).SetNames("NAD_1983_NSRS2007_StatePlane_Idaho_East_FIPS_1101", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIdahoWestFIPS1103Meters = ProjectionInfo.FromEpsgCode(3526).SetNames("NAD_1983_NSRS2007_StatePlane_Idaho_West_FIPS_1103", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIllinoisEastFIPS1201Meters = ProjectionInfo.FromEpsgCode(3528).SetNames("NAD_1983_NSRS2007_StatePlane_Illinois_East_FIPS_1201", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIllinoisWestFIPS1202Meters = ProjectionInfo.FromEpsgCode(3530).SetNames("NAD_1983_NSRS2007_StatePlane_Illinois_West_FIPS_1202", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIndianaEastFIPS1301Meters = ProjectionInfo.FromEpsgCode(3532).SetNames("NAD_1983_NSRS2007_StatePlane_Indiana_East_FIPS_1301", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIndianaWestFIPS1302Meters = ProjectionInfo.FromEpsgCode(3534).SetNames("NAD_1983_NSRS2007_StatePlane_Indiana_West_FIPS_1302", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIowaNorthFIPS1401Meters = ProjectionInfo.FromEpsgCode(3536).SetNames("NAD_1983_NSRS2007_StatePlane_Iowa_North_FIPS_1401", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIowaSouthFIPS1402Meters = ProjectionInfo.FromEpsgCode(3538).SetNames("NAD_1983_NSRS2007_StatePlane_Iowa_South_FIPS_1402", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKansasNorthFIPS1501Meters = ProjectionInfo.FromEpsgCode(3540).SetNames("NAD_1983_NSRS2007_StatePlane_Kansas_North_FIPS_1501", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKansasSouthFIPS1502Meters = ProjectionInfo.FromEpsgCode(3542).SetNames("NAD_1983_NSRS2007_StatePlane_Kansas_South_FIPS_1502", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKentuckyFIPS1600Meters = ProjectionInfo.FromEpsgCode(3546).SetNames("NAD_1983_NSRS2007_StatePlane_Kentucky_FIPS_1600", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKentuckyNorthFIPS1601Meters = ProjectionInfo.FromEpsgCode(3544).SetNames("NAD_1983_NSRS2007_StatePlane_Kentucky_North_FIPS_1601", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKentuckySouthFIPS1602Meters = ProjectionInfo.FromEpsgCode(3548).SetNames("NAD_1983_NSRS2007_StatePlane_Kentucky_South_FIPS_1602", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneLouisianaNorthFIPS1701Meters = ProjectionInfo.FromEpsgCode(3550).SetNames("NAD_1983_NSRS2007_StatePlane_Louisiana_North_FIPS_1701", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneLouisianaSouthFIPS1702Meters = ProjectionInfo.FromEpsgCode(3552).SetNames("NAD_1983_NSRS2007_StatePlane_Louisiana_South_FIPS_1702", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMaineEastFIPS1801Meters = ProjectionInfo.FromEpsgCode(3557).SetNames("NAD_1983_NSRS2007_StatePlane_Maine_East_FIPS_1801", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMaineWestFIPS1802Meters = ProjectionInfo.FromEpsgCode(3558).SetNames("NAD_1983_NSRS2007_StatePlane_Maine_West_FIPS_1802", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMarylandFIPS1900Meters = ProjectionInfo.FromEpsgCode(3559).SetNames("NAD_1983_NSRS2007_StatePlane_Maryland_FIPS_1900", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMassachusettsFIPS2001Meters = ProjectionInfo.FromEpsgCode(3585).SetNames("NAD_1983_NSRS2007_StatePlane_Massachusetts_Mainland_FIPS_2001", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMassachusettsIslFIPS2002M = ProjectionInfo.FromEpsgCode(3583).SetNames("NAD_1983_NSRS2007_StatePlane_Massachusetts_Island_FIPS_2002", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMichiganCentralFIPS2112M = ProjectionInfo.FromEpsgCode(3587).SetNames("NAD_1983_NSRS2007_StatePlane_Michigan_Central_FIPS_2112", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMichiganNorthFIPS2111Meters = ProjectionInfo.FromEpsgCode(3589).SetNames("NAD_1983_NSRS2007_StatePlane_Michigan_North_FIPS_2111", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMichiganSouthFIPS2113Meters = ProjectionInfo.FromEpsgCode(3592).SetNames("NAD_1983_NSRS2007_StatePlane_Michigan_South_FIPS_2113", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMinnesotaCentralFIPS2202M = ProjectionInfo.FromEpsgCode(3594).SetNames("NAD_1983_NSRS2007_StatePlane_Minnesota_Central_FIPS_2202", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMinnesotaNorthFIPS2201Meters = ProjectionInfo.FromEpsgCode(3595).SetNames("NAD_1983_NSRS2007_StatePlane_Minnesota_North_FIPS_2201", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMinnesotaSouthFIPS2203Meters = ProjectionInfo.FromEpsgCode(3596).SetNames("NAD_1983_NSRS2007_StatePlane_Minnesota_South_FIPS_2203", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMississippiEastFIPS2301M = ProjectionInfo.FromEpsgCode(3597).SetNames("NAD_1983_NSRS2007_StatePlane_Mississippi_East_FIPS_2301", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMississippiWestFIPS2302M = ProjectionInfo.FromEpsgCode(3599).SetNames("NAD_1983_NSRS2007_StatePlane_Mississippi_West_FIPS_2302", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMissouriCentralFIPS2402M = ProjectionInfo.FromEpsgCode(3601).SetNames("NAD_1983_NSRS2007_StatePlane_Missouri_Central_FIPS_2402", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMissouriEastFIPS2401Meters = ProjectionInfo.FromEpsgCode(3602).SetNames("NAD_1983_NSRS2007_StatePlane_Missouri_East_FIPS_2401", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMissouriWestFIPS2403Meters = ProjectionInfo.FromEpsgCode(3603).SetNames("NAD_1983_NSRS2007_StatePlane_Missouri_West_FIPS_2403", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMontanaFIPS2500Meters = ProjectionInfo.FromEpsgCode(3604).SetNames("NAD_1983_NSRS2007_StatePlane_Montana_FIPS_2500", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNebraskaFIPS2600Meters = ProjectionInfo.FromEpsgCode(3606).SetNames("NAD_1983_NSRS2007_StatePlane_Nebraska_FIPS_2600", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNevadaCentralFIPS2702Meters = ProjectionInfo.FromEpsgCode(3607).SetNames("NAD_1983_NSRS2007_StatePlane_Nevada_Central_FIPS_2702", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNevadaEastFIPS2701Meters = ProjectionInfo.FromEpsgCode(3609).SetNames("NAD_1983_NSRS2007_StatePlane_Nevada_East_FIPS_2701", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNevadaWestFIPS2703Meters = ProjectionInfo.FromEpsgCode(3611).SetNames("NAD_1983_NSRS2007_StatePlane_Nevada_West_FIPS_2703", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewHampshireFIPS2800Meters = ProjectionInfo.FromEpsgCode(3613).SetNames("NAD_1983_NSRS2007_StatePlane_New_Hampshire_FIPS_2800", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewJerseyFIPS2900Meters = ProjectionInfo.FromEpsgCode(3615).SetNames("NAD_1983_NSRS2007_StatePlane_New_Jersey_FIPS_2900", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewMexicoCentralFIPS3002M = ProjectionInfo.FromEpsgCode(3617).SetNames("NAD_1983_NSRS2007_StatePlane_New_Mexico_Central_FIPS_3002", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewMexicoEastFIPS3001Meters = ProjectionInfo.FromEpsgCode(3619).SetNames("NAD_1983_NSRS2007_StatePlane_New_Mexico_East_FIPS_3001", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewMexicoWestFIPS3003Meters = ProjectionInfo.FromEpsgCode(3621).SetNames("NAD_1983_NSRS2007_StatePlane_New_Mexico_West_FIPS_3003", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkCentralFIPS3102M = ProjectionInfo.FromEpsgCode(3623).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_Central_FIPS_3102", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkEastFIPS3101Meters = ProjectionInfo.FromEpsgCode(3625).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_East_FIPS_3101", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkLongIslFIPS3104M = ProjectionInfo.FromEpsgCode(3627).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_Long_Island_FIPS_3104", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkWestFIPS3103Meters = ProjectionInfo.FromEpsgCode(3629).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_West_FIPS_3103", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNorthCarolinaFIPS3200Meters = ProjectionInfo.FromEpsgCode(3631).SetNames("NAD_1983_NSRS2007_StatePlane_North_Carolina_FIPS_3200", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNorthDakotaNFIPS3301Meters = ProjectionInfo.FromEpsgCode(3633).SetNames("NAD_1983_NSRS2007_StatePlane_North_Dakota_North_FIPS_3301", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNorthDakotaSFIPS3302Meters = ProjectionInfo.FromEpsgCode(3635).SetNames("NAD_1983_NSRS2007_StatePlane_North_Dakota_South_FIPS_3302", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOhioNorthFIPS3401Meters = ProjectionInfo.FromEpsgCode(3637).SetNames("NAD_1983_NSRS2007_StatePlane_Ohio_North_FIPS_3401", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOhioSouthFIPS3402Meters = ProjectionInfo.FromEpsgCode(3638).SetNames("NAD_1983_NSRS2007_StatePlane_Ohio_South_FIPS_3402", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOklahomaNorthFIPS3501Meters = ProjectionInfo.FromEpsgCode(3639).SetNames("NAD_1983_NSRS2007_StatePlane_Oklahoma_North_FIPS_3501", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOklahomaSouthFIPS3502Meters = ProjectionInfo.FromEpsgCode(3641).SetNames("NAD_1983_NSRS2007_StatePlane_Oklahoma_South_FIPS_3502", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOregonNorthFIPS3601Meters = ProjectionInfo.FromEpsgCode(3645).SetNames("NAD_1983_NSRS2007_StatePlane_Oregon_North_FIPS_3601", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOregonSouthFIPS3602Meters = ProjectionInfo.FromEpsgCode(3647).SetNames("NAD_1983_NSRS2007_StatePlane_Oregon_South_FIPS_3602", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlanePennsylvaniaNorthFIPS3701M = ProjectionInfo.FromEpsgCode(3649).SetNames("NAD_1983_NSRS2007_StatePlane_Pennsylvania_North_FIPS_3701", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlanePennsylvaniaSouthFIPS3702M = ProjectionInfo.FromEpsgCode(3651).SetNames("NAD_1983_NSRS2007_StatePlane_Pennsylvania_South_FIPS_3702", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlanePRVIFIPS5200M = ProjectionInfo.FromAuthorityCode("EPSG", 102647).SetNames("NAD_1983_NSRS2007_StatePlane_Puerto_Rico_Virgin_Isls_FIPS_5200", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007"); // missing
            NAD1983NSRS2007StatePlaneRhodeIslandFIPS3800Meters = ProjectionInfo.FromEpsgCode(3653).SetNames("NAD_1983_NSRS2007_StatePlane_Rhode_Island_FIPS_3800", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneSouthCarolinaFIPS3900Meters = ProjectionInfo.FromEpsgCode(3655).SetNames("NAD_1983_NSRS2007_StatePlane_South_Carolina_FIPS_3900", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneSouthDakotaNFIPS4001Meters = ProjectionInfo.FromEpsgCode(3657).SetNames("NAD_1983_NSRS2007_StatePlane_South_Dakota_North_FIPS_4001", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneSouthDakotaSFIPS4002Meters = ProjectionInfo.FromEpsgCode(3659).SetNames("NAD_1983_NSRS2007_StatePlane_South_Dakota_South_FIPS_4002", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTennesseeFIPS4100Meters = ProjectionInfo.FromEpsgCode(3661).SetNames("NAD_1983_NSRS2007_StatePlane_Tennessee_FIPS_4100", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasCentralFIPS4203Meters = ProjectionInfo.FromEpsgCode(3663).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_Central_FIPS_4203", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasNCentralFIPS4202Meters = ProjectionInfo.FromEpsgCode(3669).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_North_Central_FIPS_4202", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasNorthFIPS4201Meters = ProjectionInfo.FromEpsgCode(3667).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_North_FIPS_4201", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasSCentralFIPS4204Meters = ProjectionInfo.FromEpsgCode(3673).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_South_Central_FIPS_4204", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasSouthFIPS4205Meters = ProjectionInfo.FromEpsgCode(3671).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_South_FIPS_4205", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahCentralFIPS4302Meters = ProjectionInfo.FromEpsgCode(3675).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_Central_FIPS_4302", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahNorthFIPS4301Meters = ProjectionInfo.FromEpsgCode(3678).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_North_FIPS_4301", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahSouthFIPS4303Meters = ProjectionInfo.FromEpsgCode(3681).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_South_FIPS_4303", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneVermontFIPS4400Meters = ProjectionInfo.FromEpsgCode(3684).SetNames("NAD_1983_NSRS2007_StatePlane_Vermont_FIPS_4400", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneVirginiaNorthFIPS4501Meters = ProjectionInfo.FromEpsgCode(3685).SetNames("NAD_1983_NSRS2007_StatePlane_Virginia_North_FIPS_4501", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneVirginiaSouthFIPS4502Meters = ProjectionInfo.FromEpsgCode(3687).SetNames("NAD_1983_NSRS2007_StatePlane_Virginia_South_FIPS_4502", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWashingtonNorthFIPS4601M = ProjectionInfo.FromEpsgCode(3689).SetNames("NAD_1983_NSRS2007_StatePlane_Washington_North_FIPS_4601", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWashingtonSouthFIPS4602M = ProjectionInfo.FromEpsgCode(3691).SetNames("NAD_1983_NSRS2007_StatePlane_Washington_South_FIPS_4602", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWestVirginiaNFIPS4701Meters = ProjectionInfo.FromEpsgCode(3693).SetNames("NAD_1983_NSRS2007_StatePlane_West_Virginia_North_FIPS_4701", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWestVirginiaSFIPS4702Meters = ProjectionInfo.FromEpsgCode(3694).SetNames("NAD_1983_NSRS2007_StatePlane_West_Virginia_South_FIPS_4702", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWisconsinCentralFIPS4802M = ProjectionInfo.FromEpsgCode(3695).SetNames("NAD_1983_NSRS2007_StatePlane_Wisconsin_Central_FIPS_4802", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWisconsinNorthFIPS4801Meters = ProjectionInfo.FromEpsgCode(3697).SetNames("NAD_1983_NSRS2007_StatePlane_Wisconsin_North_FIPS_4801", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWisconsinSouthFIPS4803Meters = ProjectionInfo.FromEpsgCode(3699).SetNames("NAD_1983_NSRS2007_StatePlane_Wisconsin_South_FIPS_4803", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingEastFIPS4901Meters = ProjectionInfo.FromEpsgCode(3702).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_East_FIPS_4901", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingECentralFIPS4902M = ProjectionInfo.FromEpsgCode(3703).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_East_Central_FIPS_4902", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingWCentralFIPS4903M = ProjectionInfo.FromEpsgCode(3704).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_West_Central_FIPS_4903", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingWestFIPS4904Meters = ProjectionInfo.FromEpsgCode(3705).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_West_FIPS_4904", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
        }

        #endregion
    }
}

#pragma warning restore 1591