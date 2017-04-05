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
    /// This class contains predefined CoordinateSystems for NAD1983CORS96Meters.
    /// </summary>
    public class NAD1983CORS96Meters : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983CORS96Maine2000CentralZoneMeters;
        public readonly ProjectionInfo NAD1983CORS96Maine2000EastZoneMeters;
        public readonly ProjectionInfo NAD1983CORS96Maine2000WestZoneMeters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlabamaEastFIPS0101Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlabamaWestFIPS0102Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska10FIPS5010Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska1FIPS5001Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska2FIPS5002Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska3FIPS5003Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska4FIPS5004Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska5FIPS5005Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska6FIPS5006Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska7FIPS5007Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska8FIPS5008Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneAlaska9FIPS5009Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneArizonaCentralFIPS0202Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneArizonaEastFIPS0201Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneArizonaWestFIPS0203Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneArkansasNorthFIPS0301Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneArkansasSouthFIPS0302Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIFIPS0401Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIIFIPS0402Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIIIFIPS0403Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaIVFIPS0404Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaVFIPS0405Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneCaliforniaVIFIPS0406Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneColoradoCentralFIPS0502Meter;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneColoradoNorthFIPS0501Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneColoradoSouthFIPS0503Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneConnecticutFIPS0600Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneDelawareFIPS0700Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneFloridaEastFIPS0901Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneFloridaNorthFIPS0903Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneFloridaWestFIPS0902Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneGeorgiaEastFIPS1001Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneGeorgiaWestFIPS1002Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIdahoCentralFIPS1102Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIdahoEastFIPS1101Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIdahoWestFIPS1103Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIllinoisEastFIPS1201Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIllinoisWestFIPS1202Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIndianaEastFIPS1301Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIndianaWestFIPS1302Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIowaNorthFIPS1401Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneIowaSouthFIPS1402Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKansasNorthFIPS1501Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKansasSouthFIPS1502Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKentuckyFIPS1600Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKentuckyNorthFIPS1601Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneKentuckySouthFIPS1602Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneLouisianaNorthFIPS1701Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneLouisianaSouthFIPS1702Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMaineEastFIPS1801Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMaineWestFIPS1802Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMarylandFIPS1900Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMassachusettsFIPS2001Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMassachusettsIslFIPS2002M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMichiganCentralFIPS2112M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMichiganNorthFIPS2111Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMichiganSouthFIPS2113Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMinnesotaCentralFIPS2202M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMinnesotaNorthFIPS2201Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMinnesotaSouthFIPS2203Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMississippiEastFIPS2301M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMississippiWestFIPS2302M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMissouriCentralFIPS2402M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMissouriEastFIPS2401Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMissouriWestFIPS2403Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneMontanaFIPS2500Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNebraskaFIPS2600Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNevadaCentralFIPS2702Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNevadaEastFIPS2701Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNevadaWestFIPS2703Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewHampshireFIPS2800Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewJerseyFIPS2900Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewMexicoCentralFIPS3002M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewMexicoEastFIPS3001Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewMexicoWestFIPS3003Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkCentralFIPS3102M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkEastFIPS3101Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkLongIslFIPS3104M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNewYorkWestFIPS3103Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNorthCarolinaFIPS3200Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNorthDakotaNFIPS3301Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneNorthDakotaSFIPS3302Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOhioNorthFIPS3401Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOhioSouthFIPS3402Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOklahomaNorthFIPS3501Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOklahomaSouthFIPS3502Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOregonNorthFIPS3601Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneOregonSouthFIPS3602Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlanePennsylvaniaNorthFIPS3701M;
        public readonly ProjectionInfo NAD1983CORS96StatePlanePennsylvaniaSouthFIPS3702M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneRhodeIslandFIPS3800Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneSouthCarolinaFIPS3900Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneSouthDakotaNFIPS4001Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneSouthDakotaSFIPS4002Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTennesseeFIPS4100Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasCentralFIPS4203Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasNCentralFIPS4202Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasNorthFIPS4201Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasSCentralFIPS4204Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneTexasSouthFIPS4205Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneUtahCentralFIPS4302Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneUtahNorthFIPS4301Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneUtahSouthFIPS4303Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneVermontFIPS4400Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneVirginiaNorthFIPS4501Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneVirginiaSouthFIPS4502Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWashingtonNorthFIPS4601M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWashingtonSouthFIPS4602M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWestVirginiaNFIPS4701Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWestVirginiaSFIPS4702Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWisconsinCentralFIPS4802M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWisconsinNorthFIPS4801Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWisconsinSouthFIPS4803Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingEastFIPS4901Meters;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingECentralFIPS4902M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingWCentralFIPS4903M;
        public readonly ProjectionInfo NAD1983CORS96StatePlaneWyomingWestFIPS4904Meters;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983CORS96Meters.
        /// </summary>
        public NAD1983CORS96Meters()
        {
            NAD1983CORS96Maine2000CentralZoneMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103373).SetNames("NAD_1983_CORS96_Maine_2000_Central_Zone", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96Maine2000EastZoneMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103372).SetNames("NAD_1983_CORS96_Maine_2000_East_Zone", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96Maine2000WestZoneMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103374).SetNames("NAD_1983_CORS96_Maine_2000_West_Zone", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlabamaEastFIPS0101Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103220).SetNames("NAD_1983_CORS96_StatePlane_Alabama_East_FIPS_0101", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlabamaWestFIPS0102Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103221).SetNames("NAD_1983_CORS96_StatePlane_Alabama_West_FIPS_0102", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska10FIPS5010Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102375).SetNames("NAD_1983_CORS96_StatePlane_Alaska_10_FIPS_5010", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska1FIPS5001Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102366).SetNames("NAD_1983_CORS96_StatePlane_Alaska_1_FIPS_5001", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska2FIPS5002Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102367).SetNames("NAD_1983_CORS96_StatePlane_Alaska_2_FIPS_5002", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska3FIPS5003Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102368).SetNames("NAD_1983_CORS96_StatePlane_Alaska_3_FIPS_5003", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska4FIPS5004Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102369).SetNames("NAD_1983_CORS96_StatePlane_Alaska_4_FIPS_5004", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska5FIPS5005Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102370).SetNames("NAD_1983_CORS96_StatePlane_Alaska_5_FIPS_5005", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska6FIPS5006Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102371).SetNames("NAD_1983_CORS96_StatePlane_Alaska_6_FIPS_5006", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska7FIPS5007Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102372).SetNames("NAD_1983_CORS96_StatePlane_Alaska_7_FIPS_5007", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska8FIPS5008Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102373).SetNames("NAD_1983_CORS96_StatePlane_Alaska_8_FIPS_5008", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneAlaska9FIPS5009Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102374).SetNames("NAD_1983_CORS96_StatePlane_Alaska_9_FIPS_5009", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneArizonaCentralFIPS0202Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103223).SetNames("NAD_1983_CORS96_StatePlane_Arizona_Central_FIPS_0202", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneArizonaEastFIPS0201Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103222).SetNames("NAD_1983_CORS96_StatePlane_Arizona_East_FIPS_0201", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneArizonaWestFIPS0203Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103224).SetNames("NAD_1983_CORS96_StatePlane_Arizona_West_FIPS_0203", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneArkansasNorthFIPS0301Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103228).SetNames("NAD_1983_CORS96_StatePlane_Arkansas_North_FIPS_0301", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneArkansasSouthFIPS0302Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103229).SetNames("NAD_1983_CORS96_StatePlane_Arkansas_South_FIPS_0302", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIFIPS0401Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103232).SetNames("NAD_1983_CORS96_StatePlane_California_I_FIPS_0401", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIIFIPS0402Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103233).SetNames("NAD_1983_CORS96_StatePlane_California_II_FIPS_0402", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIIIFIPS0403Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103234).SetNames("NAD_1983_CORS96_StatePlane_California_III_FIPS_0403", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaIVFIPS0404Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103235).SetNames("NAD_1983_CORS96_StatePlane_California_IV_FIPS_0404", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaVFIPS0405Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103236).SetNames("NAD_1983_CORS96_StatePlane_California_V_FIPS_0405", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneCaliforniaVIFIPS0406Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103237).SetNames("NAD_1983_CORS96_StatePlane_California_VI_FIPS_0406", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneColoradoCentralFIPS0502Meter = ProjectionInfo.FromAuthorityCode("ESRI", 103245).SetNames("NAD_1983_CORS96_StatePlane_Colorado_Central_FIPS_0502", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneColoradoNorthFIPS0501Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103244).SetNames("NAD_1983_CORS96_StatePlane_Colorado_North_FIPS_0501", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneColoradoSouthFIPS0503Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103246).SetNames("NAD_1983_CORS96_StatePlane_Colorado_South_FIPS_0503", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneConnecticutFIPS0600Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103250).SetNames("NAD_1983_CORS96_StatePlane_Connecticut_FIPS_0600", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneDelawareFIPS0700Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103252).SetNames("NAD_1983_CORS96_StatePlane_Delaware_FIPS_0700", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneFloridaEastFIPS0901Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103254).SetNames("NAD_1983_CORS96_StatePlane_Florida_East_FIPS_0901", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneFloridaNorthFIPS0903Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103256).SetNames("NAD_1983_CORS96_StatePlane_Florida_North_FIPS_0903", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneFloridaWestFIPS0902Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103255).SetNames("NAD_1983_CORS96_StatePlane_Florida_West_FIPS_0902", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneGeorgiaEastFIPS1001Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103260).SetNames("NAD_1983_CORS96_StatePlane_Georgia_East_FIPS_1001", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneGeorgiaWestFIPS1002Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103261).SetNames("NAD_1983_CORS96_StatePlane_Georgia_West_FIPS_1002", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIdahoCentralFIPS1102Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103265).SetNames("NAD_1983_CORS96_StatePlane_Idaho_Central_FIPS_1102", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIdahoEastFIPS1101Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103264).SetNames("NAD_1983_CORS96_StatePlane_Idaho_East_FIPS_1101", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIdahoWestFIPS1103Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103266).SetNames("NAD_1983_CORS96_StatePlane_Idaho_West_FIPS_1103", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIllinoisEastFIPS1201Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103270).SetNames("NAD_1983_CORS96_StatePlane_Illinois_East_FIPS_1201", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIllinoisWestFIPS1202Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103271).SetNames("NAD_1983_CORS96_StatePlane_Illinois_West_FIPS_1202", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIndianaEastFIPS1301Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103274).SetNames("NAD_1983_CORS96_StatePlane_Indiana_East_FIPS_1301", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIndianaWestFIPS1302Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103275).SetNames("NAD_1983_CORS96_StatePlane_Indiana_West_FIPS_1302", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIowaNorthFIPS1401Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103278).SetNames("NAD_1983_CORS96_StatePlane_Iowa_North_FIPS_1401", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneIowaSouthFIPS1402Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103279).SetNames("NAD_1983_CORS96_StatePlane_Iowa_South_FIPS_1402", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKansasNorthFIPS1501Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103282).SetNames("NAD_1983_CORS96_StatePlane_Kansas_North_FIPS_1501", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKansasSouthFIPS1502Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103283).SetNames("NAD_1983_CORS96_StatePlane_Kansas_South_FIPS_1502", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKentuckyFIPS1600Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103288).SetNames("NAD_1983_CORS96_StatePlane_Kentucky_FIPS_1600", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKentuckyNorthFIPS1601Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103286).SetNames("NAD_1983_CORS96_StatePlane_Kentucky_North_FIPS_1601", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneKentuckySouthFIPS1602Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103290).SetNames("NAD_1983_CORS96_StatePlane_Kentucky_South_FIPS_1602", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneLouisianaNorthFIPS1701Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103292).SetNames("NAD_1983_CORS96_StatePlane_Louisiana_North_FIPS_1701", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneLouisianaSouthFIPS1702Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103293).SetNames("NAD_1983_CORS96_StatePlane_Louisiana_South_FIPS_1702", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMaineEastFIPS1801Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103296).SetNames("NAD_1983_CORS96_StatePlane_Maine_East_FIPS_1801", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMaineWestFIPS1802Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103297).SetNames("NAD_1983_CORS96_StatePlane_Maine_West_FIPS_1802", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMarylandFIPS1900Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103375).SetNames("NAD_1983_CORS96_StatePlane_Maryland_FIPS_1900", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMassachusettsFIPS2001Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103377).SetNames("NAD_1983_CORS96_StatePlane_Massachusetts_Mainland_FIPS_2001", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMassachusettsIslFIPS2002M = ProjectionInfo.FromAuthorityCode("ESRI", 103378).SetNames("NAD_1983_CORS96_StatePlane_Massachusetts_Island_FIPS_2002", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMichiganCentralFIPS2112M = ProjectionInfo.FromAuthorityCode("ESRI", 103382).SetNames("NAD_1983_CORS96_StatePlane_Michigan_Central_FIPS_2112", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMichiganNorthFIPS2111Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103381).SetNames("NAD_1983_CORS96_StatePlane_Michigan_North_FIPS_2111", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMichiganSouthFIPS2113Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103383).SetNames("NAD_1983_CORS96_StatePlane_Michigan_South_FIPS_2113", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMinnesotaCentralFIPS2202M = ProjectionInfo.FromAuthorityCode("ESRI", 103388).SetNames("NAD_1983_CORS96_StatePlane_Minnesota_Central_FIPS_2202", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMinnesotaNorthFIPS2201Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103387).SetNames("NAD_1983_CORS96_StatePlane_Minnesota_North_FIPS_2201", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMinnesotaSouthFIPS2203Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103389).SetNames("NAD_1983_CORS96_StatePlane_Minnesota_South_FIPS_2203", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMississippiEastFIPS2301M = ProjectionInfo.FromAuthorityCode("ESRI", 103393).SetNames("NAD_1983_CORS96_StatePlane_Mississippi_East_FIPS_2301", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMississippiWestFIPS2302M = ProjectionInfo.FromAuthorityCode("ESRI", 103394).SetNames("NAD_1983_CORS96_StatePlane_Mississippi_West_FIPS_2302", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMissouriCentralFIPS2402M = ProjectionInfo.FromAuthorityCode("ESRI", 103398).SetNames("NAD_1983_CORS96_StatePlane_Missouri_Central_FIPS_2402", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMissouriEastFIPS2401Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103397).SetNames("NAD_1983_CORS96_StatePlane_Missouri_East_FIPS_2401", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMissouriWestFIPS2403Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103399).SetNames("NAD_1983_CORS96_StatePlane_Missouri_West_FIPS_2403", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneMontanaFIPS2500Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103472).SetNames("NAD_1983_CORS96_StatePlane_Montana_FIPS_2500", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNebraskaFIPS2600Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103474).SetNames("NAD_1983_CORS96_StatePlane_Nebraska_FIPS_2600", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNevadaCentralFIPS2702Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103477).SetNames("NAD_1983_CORS96_StatePlane_Nevada_Central_FIPS_2702", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNevadaEastFIPS2701Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103476).SetNames("NAD_1983_CORS96_StatePlane_Nevada_East_FIPS_2701", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNevadaWestFIPS2703Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103478).SetNames("NAD_1983_CORS96_StatePlane_Nevada_West_FIPS_2703", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewHampshireFIPS2800Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103482).SetNames("NAD_1983_CORS96_StatePlane_New_Hampshire_FIPS_2800", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewJerseyFIPS2900Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103484).SetNames("NAD_1983_CORS96_StatePlane_New_Jersey_FIPS_2900", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewMexicoCentralFIPS3002M = ProjectionInfo.FromAuthorityCode("ESRI", 103487).SetNames("NAD_1983_CORS96_StatePlane_New_Mexico_Central_FIPS_3002", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewMexicoEastFIPS3001Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103486).SetNames("NAD_1983_CORS96_StatePlane_New_Mexico_East_FIPS_3001", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewMexicoWestFIPS3003Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103488).SetNames("NAD_1983_CORS96_StatePlane_New_Mexico_West_FIPS_3003", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkCentralFIPS3102M = ProjectionInfo.FromAuthorityCode("ESRI", 103493).SetNames("NAD_1983_CORS96_StatePlane_New_York_Central_FIPS_3102", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkEastFIPS3101Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103492).SetNames("NAD_1983_CORS96_StatePlane_New_York_East_FIPS_3101", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkLongIslFIPS3104M = ProjectionInfo.FromAuthorityCode("ESRI", 103495).SetNames("NAD_1983_CORS96_StatePlane_New_York_Long_Island_FIPS_3104", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNewYorkWestFIPS3103Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103494).SetNames("NAD_1983_CORS96_StatePlane_New_York_West_FIPS_3103", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNorthCarolinaFIPS3200Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103500).SetNames("NAD_1983_CORS96_StatePlane_North_Carolina_FIPS_3200", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNorthDakotaNFIPS3301Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103502).SetNames("NAD_1983_CORS96_StatePlane_North_Dakota_North_FIPS_3301", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneNorthDakotaSFIPS3302Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103503).SetNames("NAD_1983_CORS96_StatePlane_North_Dakota_South_FIPS_3302", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOhioNorthFIPS3401Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103506).SetNames("NAD_1983_CORS96_StatePlane_Ohio_North_FIPS_3401", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOhioSouthFIPS3402Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103507).SetNames("NAD_1983_CORS96_StatePlane_Ohio_South_FIPS_3402", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOklahomaNorthFIPS3501Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103510).SetNames("NAD_1983_CORS96_StatePlane_Oklahoma_North_FIPS_3501", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOklahomaSouthFIPS3502Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103511).SetNames("NAD_1983_CORS96_StatePlane_Oklahoma_South_FIPS_3502", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOregonNorthFIPS3601Meters = ProjectionInfo.FromAuthorityCode("EPSG", 102376).SetNames("NAD_1983_CORS96_StatePlane_Oregon_North_FIPS_3601", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneOregonSouthFIPS3602Meters = ProjectionInfo.FromAuthorityCode("EPSG", 102377).SetNames("NAD_1983_CORS96_StatePlane_Oregon_South_FIPS_3602", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlanePennsylvaniaNorthFIPS3701M = ProjectionInfo.FromAuthorityCode("ESRI", 103514).SetNames("NAD_1983_CORS96_StatePlane_Pennsylvania_North_FIPS_3701", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlanePennsylvaniaSouthFIPS3702M = ProjectionInfo.FromAuthorityCode("ESRI", 103516).SetNames("NAD_1983_CORS96_StatePlane_Pennsylvania_South_FIPS_3702", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneRhodeIslandFIPS3800Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103518).SetNames("NAD_1983_CORS96_StatePlane_Rhode_Island_FIPS_3800", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneSouthCarolinaFIPS3900Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103520).SetNames("NAD_1983_CORS96_StatePlane_South_Carolina_FIPS_3900", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneSouthDakotaNFIPS4001Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103522).SetNames("NAD_1983_CORS96_StatePlane_South_Dakota_North_FIPS_4001", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneSouthDakotaSFIPS4002Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103523).SetNames("NAD_1983_CORS96_StatePlane_South_Dakota_South_FIPS_4002", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTennesseeFIPS4100Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103526).SetNames("NAD_1983_CORS96_StatePlane_Tennessee_FIPS_4100", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasCentralFIPS4203Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103541).SetNames("NAD_1983_CORS96_StatePlane_Texas_Central_FIPS_4203", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasNCentralFIPS4202Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103540).SetNames("NAD_1983_CORS96_StatePlane_Texas_North_Central_FIPS_4202", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasNorthFIPS4201Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103539).SetNames("NAD_1983_CORS96_StatePlane_Texas_North_FIPS_4201", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasSCentralFIPS4204Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103542).SetNames("NAD_1983_CORS96_StatePlane_Texas_South_Central_FIPS_4204", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneTexasSouthFIPS4205Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103543).SetNames("NAD_1983_CORS96_StatePlane_Texas_South_FIPS_4205", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneUtahCentralFIPS4302Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103550).SetNames("NAD_1983_CORS96_StatePlane_Utah_Central_FIPS_4302", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneUtahNorthFIPS4301Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103549).SetNames("NAD_1983_CORS96_StatePlane_Utah_North_FIPS_4301", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneUtahSouthFIPS4303Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103551).SetNames("NAD_1983_CORS96_StatePlane_Utah_South_FIPS_4303", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneVermontFIPS4400Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103558).SetNames("NAD_1983_CORS96_StatePlane_Vermont_FIPS_4400", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneVirginiaNorthFIPS4501Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103559).SetNames("NAD_1983_CORS96_StatePlane_Virginia_North_FIPS_4501", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneVirginiaSouthFIPS4502Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103560).SetNames("NAD_1983_CORS96_StatePlane_Virginia_South_FIPS_4502", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWashingtonNorthFIPS4601M = ProjectionInfo.FromAuthorityCode("ESRI", 103563).SetNames("NAD_1983_CORS96_StatePlane_Washington_North_FIPS_4601", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWashingtonSouthFIPS4602M = ProjectionInfo.FromAuthorityCode("ESRI", 103564).SetNames("NAD_1983_CORS96_StatePlane_Washington_South_FIPS_4602", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWestVirginiaNFIPS4701Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103567).SetNames("NAD_1983_CORS96_StatePlane_West_Virginia_North_FIPS_4701", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWestVirginiaSFIPS4702Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103568).SetNames("NAD_1983_CORS96_StatePlane_West_Virginia_South_FIPS_4702", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWisconsinCentralFIPS4802M = ProjectionInfo.FromAuthorityCode("ESRI", 103572).SetNames("NAD_1983_CORS96_StatePlane_Wisconsin_Central_FIPS_4802", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWisconsinNorthFIPS4801Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103571).SetNames("NAD_1983_CORS96_StatePlane_Wisconsin_North_FIPS_4801", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWisconsinSouthFIPS4803Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103573).SetNames("NAD_1983_CORS96_StatePlane_Wisconsin_South_FIPS_4803", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingEastFIPS4901Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103577).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_East_FIPS_4901", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingECentralFIPS4902M = ProjectionInfo.FromAuthorityCode("ESRI", 103578).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_East_Central_FIPS_4902", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingWCentralFIPS4903M = ProjectionInfo.FromAuthorityCode("ESRI", 103579).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_West_Central_FIPS_4903", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96StatePlaneWyomingWestFIPS4904Meters = ProjectionInfo.FromAuthorityCode("ESRI", 103580).SetNames("NAD_1983_CORS96_StatePlane_Wyoming_West_FIPS_4904", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591