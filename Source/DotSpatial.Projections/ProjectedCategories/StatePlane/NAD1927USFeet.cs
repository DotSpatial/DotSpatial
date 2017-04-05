// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:56:40 PM
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
    /// This class contains predefined CoordinateSystems for NAD1927USFeet.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class NAD1927USFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1927StatePlaneAlabamaEastFIPS0101;
        public readonly ProjectionInfo NAD1927StatePlaneAlabamaWestFIPS0102;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska10FIPS5010;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska1FIPS5001;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska2FIPS5002;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska3FIPS5003;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska4FIPS5004;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska5FIPS5005;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska6FIPS5006;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska7FIPS5007;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska8FIPS5008;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska9FIPS5009;
        public readonly ProjectionInfo NAD1927StatePlaneArizonaCentralFIPS0202;
        public readonly ProjectionInfo NAD1927StatePlaneArizonaEastFIPS0201;
        public readonly ProjectionInfo NAD1927StatePlaneArizonaWestFIPS0203;
        public readonly ProjectionInfo NAD1927StatePlaneArkansasNorthFIPS0301;
        public readonly ProjectionInfo NAD1927StatePlaneArkansasSouthFIPS0302;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIFIPS0401;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIIFIPS0402;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIIIFIPS0403;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIVFIPS0404;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaVFIPS0405;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaVIFIPS0406;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaVIIFIPS0407;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaVVentura;
        public readonly ProjectionInfo NAD1927StatePlaneColoradoCentralFIPS0502;
        public readonly ProjectionInfo NAD1927StatePlaneColoradoNorthFIPS0501;
        public readonly ProjectionInfo NAD1927StatePlaneColoradoSouthFIPS0503;
        public readonly ProjectionInfo NAD1927StatePlaneConnecticutFIPS0600;
        public readonly ProjectionInfo NAD1927StatePlaneDelawareFIPS0700;
        public readonly ProjectionInfo NAD1927StatePlaneFloridaEastFIPS0901;
        public readonly ProjectionInfo NAD1927StatePlaneFloridaNorthFIPS0903;
        public readonly ProjectionInfo NAD1927StatePlaneFloridaWestFIPS0902;
        public readonly ProjectionInfo NAD1927StatePlaneGeorgiaEastFIPS1001;
        public readonly ProjectionInfo NAD1927StatePlaneGeorgiaWestFIPS1002;
        public readonly ProjectionInfo NAD1927StatePlaneGuamFIPS5400;
        public readonly ProjectionInfo NAD1927StatePlaneIdahoCentralFIPS1102;
        public readonly ProjectionInfo NAD1927StatePlaneIdahoEastFIPS1101;
        public readonly ProjectionInfo NAD1927StatePlaneIdahoWestFIPS1103;
        public readonly ProjectionInfo NAD1927StatePlaneIllinoisEastFIPS1201;
        public readonly ProjectionInfo NAD1927StatePlaneIllinoisWestFIPS1202;
        public readonly ProjectionInfo NAD1927StatePlaneIndianaEastFIPS1301;
        public readonly ProjectionInfo NAD1927StatePlaneIndianaWestFIPS1302;
        public readonly ProjectionInfo NAD1927StatePlaneIowaNorthFIPS1401;
        public readonly ProjectionInfo NAD1927StatePlaneIowaSouthFIPS1402;
        public readonly ProjectionInfo NAD1927StatePlaneKansasNorthFIPS1501;
        public readonly ProjectionInfo NAD1927StatePlaneKansasSouthFIPS1502;
        public readonly ProjectionInfo NAD1927StatePlaneKentuckyNorthFIPS1601;
        public readonly ProjectionInfo NAD1927StatePlaneKentuckySouthFIPS1602;
        public readonly ProjectionInfo NAD1927StatePlaneLouisianaNorthFIPS1701;
        public readonly ProjectionInfo NAD1927StatePlaneLouisianaOffshoreFIPS1703;
        public readonly ProjectionInfo NAD1927StatePlaneLouisianaSouthFIPS1702;
        public readonly ProjectionInfo NAD1927StatePlaneMaineEastFIPS1801;
        public readonly ProjectionInfo NAD1927StatePlaneMaineWestFIPS1802;
        public readonly ProjectionInfo NAD1927StatePlaneMarylandFIPS1900;
        public readonly ProjectionInfo NAD1927StatePlaneMassachusettsFIPS2001;
        public readonly ProjectionInfo NAD1927StatePlaneMassachusettsIslFIPS2002;
        public readonly ProjectionInfo NAD1927StatePlaneMichiganCentralFIPS2112;
        public readonly ProjectionInfo NAD1927StatePlaneMichiganNorthFIPS2111;
        public readonly ProjectionInfo NAD1927StatePlaneMichiganSouthFIPS2113;
        public readonly ProjectionInfo NAD1927StatePlaneMinnesotaCentralFIPS2202;
        public readonly ProjectionInfo NAD1927StatePlaneMinnesotaNorthFIPS2201;
        public readonly ProjectionInfo NAD1927StatePlaneMinnesotaSouthFIPS2203;
        public readonly ProjectionInfo NAD1927StatePlaneMississippiEastFIPS2301;
        public readonly ProjectionInfo NAD1927StatePlaneMississippiWestFIPS2302;
        public readonly ProjectionInfo NAD1927StatePlaneMissouriCentralFIPS2402;
        public readonly ProjectionInfo NAD1927StatePlaneMissouriEastFIPS2401;
        public readonly ProjectionInfo NAD1927StatePlaneMissouriWestFIPS2403;
        public readonly ProjectionInfo NAD1927StatePlaneMontanaCentralFIPS2502;
        public readonly ProjectionInfo NAD1927StatePlaneMontanaNorthFIPS2501;
        public readonly ProjectionInfo NAD1927StatePlaneMontanaSouthFIPS2503;
        public readonly ProjectionInfo NAD1927StatePlaneNebraskaNorthFIPS2601;
        public readonly ProjectionInfo NAD1927StatePlaneNebraskaSouthFIPS2602;
        public readonly ProjectionInfo NAD1927StatePlaneNevadaCentralFIPS2702;
        public readonly ProjectionInfo NAD1927StatePlaneNevadaEastFIPS2701;
        public readonly ProjectionInfo NAD1927StatePlaneNevadaWestFIPS2703;
        public readonly ProjectionInfo NAD1927StatePlaneNewHampshireFIPS2800;
        public readonly ProjectionInfo NAD1927StatePlaneNewJerseyFIPS2900;
        public readonly ProjectionInfo NAD1927StatePlaneNewMexicoCentralFIPS3002;
        public readonly ProjectionInfo NAD1927StatePlaneNewMexicoEastFIPS3001;
        public readonly ProjectionInfo NAD1927StatePlaneNewMexicoWestFIPS3003;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkCentralFIPS3102;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkEastFIPS3101;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkLongIslFIPS3104;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkWestFIPS3103;
        public readonly ProjectionInfo NAD1927StatePlaneNorthCarolinaFIPS3200;
        public readonly ProjectionInfo NAD1927StatePlaneNorthDakotaNFIPS3301;
        public readonly ProjectionInfo NAD1927StatePlaneNorthDakotaSFIPS3302;
        public readonly ProjectionInfo NAD1927StatePlaneOhioNorthFIPS3401;
        public readonly ProjectionInfo NAD1927StatePlaneOhioSouthFIPS3402;
        public readonly ProjectionInfo NAD1927StatePlaneOklahomaNorthFIPS3501;
        public readonly ProjectionInfo NAD1927StatePlaneOklahomaSouthFIPS3502;
        public readonly ProjectionInfo NAD1927StatePlaneOregonNorthFIPS3601;
        public readonly ProjectionInfo NAD1927StatePlaneOregonSouthFIPS3602;
        public readonly ProjectionInfo NAD1927StatePlanePennsylvaniaNorthFIPS3701;
        public readonly ProjectionInfo NAD1927StatePlanePennsylvaniaSouthFIPS3702;
        public readonly ProjectionInfo NAD1927StatePlanePuertoRicoFIPS5201;
        public readonly ProjectionInfo NAD1927StatePlaneRhodeIslandFIPS3800;
        public readonly ProjectionInfo NAD1927StatePlaneSouthCarolinaNFIPS3901;
        public readonly ProjectionInfo NAD1927StatePlaneSouthCarolinaSFIPS3902;
        public readonly ProjectionInfo NAD1927StatePlaneSouthDakotaNFIPS4001;
        public readonly ProjectionInfo NAD1927StatePlaneSouthDakotaSFIPS4002;
        public readonly ProjectionInfo NAD1927StatePlaneTennesseeFIPS4100;
        public readonly ProjectionInfo NAD1927StatePlaneTexasCentralFIPS4203;
        public readonly ProjectionInfo NAD1927StatePlaneTexasNCentralFIPS4202;
        public readonly ProjectionInfo NAD1927StatePlaneTexasNorthFIPS4201;
        public readonly ProjectionInfo NAD1927StatePlaneTexasSCentralFIPS4204;
        public readonly ProjectionInfo NAD1927StatePlaneTexasSouthFIPS4205;
        public readonly ProjectionInfo NAD1927StatePlaneUtahCentralFIPS4302;
        public readonly ProjectionInfo NAD1927StatePlaneUtahNorthFIPS4301;
        public readonly ProjectionInfo NAD1927StatePlaneUtahSouthFIPS4303;
        public readonly ProjectionInfo NAD1927StatePlaneVermontFIPS3400;
        public readonly ProjectionInfo NAD1927StatePlaneVirginiaNorthFIPS4501;
        public readonly ProjectionInfo NAD1927StatePlaneVirginiaSouthFIPS4502;
        public readonly ProjectionInfo NAD1927StatePlaneVirginIslStCroixFIPS5202;
        public readonly ProjectionInfo NAD1927StatePlaneWashingtonNorthFIPS4601;
        public readonly ProjectionInfo NAD1927StatePlaneWashingtonSouthFIPS4602;
        public readonly ProjectionInfo NAD1927StatePlaneWestVirginiaNFIPS4701;
        public readonly ProjectionInfo NAD1927StatePlaneWestVirginiaSFIPS4702;
        public readonly ProjectionInfo NAD1927StatePlaneWisconsinCentralFIPS4802;
        public readonly ProjectionInfo NAD1927StatePlaneWisconsinNorthFIPS4801;
        public readonly ProjectionInfo NAD1927StatePlaneWisconsinSouthFIPS4803;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingEastFIPS4901;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingECentralFIPS4902;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingWCentralFIPS4903;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingWestFIPS4904;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1927USFeet.
        /// </summary>
        public NAD1927USFeet()
        {
            NAD1927StatePlaneAlabamaEastFIPS0101 = ProjectionInfo.FromEpsgCode(26729).SetNames("NAD_1927_StatePlane_Alabama_East_FIPS_0101", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlabamaWestFIPS0102 = ProjectionInfo.FromEpsgCode(26730).SetNames("NAD_1927_StatePlane_Alabama_West_FIPS_0102", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska10FIPS5010 = ProjectionInfo.FromEpsgCode(26740).SetNames("NAD_1927_StatePlane_Alaska_10_FIPS_5010", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska1FIPS5001 = ProjectionInfo.FromEpsgCode(26731).SetNames("NAD_1927_StatePlane_Alaska_1_FIPS_5001", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska2FIPS5002 = ProjectionInfo.FromEpsgCode(26732).SetNames("NAD_1927_StatePlane_Alaska_2_FIPS_5002", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska3FIPS5003 = ProjectionInfo.FromEpsgCode(26733).SetNames("NAD_1927_StatePlane_Alaska_3_FIPS_5003", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska4FIPS5004 = ProjectionInfo.FromEpsgCode(26734).SetNames("NAD_1927_StatePlane_Alaska_4_FIPS_5004", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska5FIPS5005 = ProjectionInfo.FromEpsgCode(26735).SetNames("NAD_1927_StatePlane_Alaska_5_FIPS_5005", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska6FIPS5006 = ProjectionInfo.FromEpsgCode(26736).SetNames("NAD_1927_StatePlane_Alaska_6_FIPS_5006", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska7FIPS5007 = ProjectionInfo.FromEpsgCode(26737).SetNames("NAD_1927_StatePlane_Alaska_7_FIPS_5007", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska8FIPS5008 = ProjectionInfo.FromEpsgCode(26738).SetNames("NAD_1927_StatePlane_Alaska_8_FIPS_5008", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneAlaska9FIPS5009 = ProjectionInfo.FromEpsgCode(26739).SetNames("NAD_1927_StatePlane_Alaska_9_FIPS_5009", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneArizonaCentralFIPS0202 = ProjectionInfo.FromEpsgCode(26749).SetNames("NAD_1927_StatePlane_Arizona_Central_FIPS_0202", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneArizonaEastFIPS0201 = ProjectionInfo.FromEpsgCode(26748).SetNames("NAD_1927_StatePlane_Arizona_East_FIPS_0201", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneArizonaWestFIPS0203 = ProjectionInfo.FromEpsgCode(26750).SetNames("NAD_1927_StatePlane_Arizona_West_FIPS_0203", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneArkansasNorthFIPS0301 = ProjectionInfo.FromEpsgCode(26751).SetNames("NAD_1927_StatePlane_Arkansas_North_FIPS_0301", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneArkansasSouthFIPS0302 = ProjectionInfo.FromEpsgCode(26752).SetNames("NAD_1927_StatePlane_Arkansas_South_FIPS_0302", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaIFIPS0401 = ProjectionInfo.FromEpsgCode(26741).SetNames("NAD_1927_StatePlane_California_I_FIPS_0401", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaIIFIPS0402 = ProjectionInfo.FromEpsgCode(26742).SetNames("NAD_1927_StatePlane_California_II_FIPS_0402", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaIIIFIPS0403 = ProjectionInfo.FromEpsgCode(26743).SetNames("NAD_1927_StatePlane_California_III_FIPS_0403", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaIVFIPS0404 = ProjectionInfo.FromEpsgCode(26744).SetNames("NAD_1927_StatePlane_California_IV_FIPS_0404", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaVFIPS0405 = ProjectionInfo.FromEpsgCode(26745).SetNames("NAD_1927_StatePlane_California_V_FIPS_0405", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaVIFIPS0406 = ProjectionInfo.FromEpsgCode(26746).SetNames("NAD_1927_StatePlane_California_VI_FIPS_0406", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaVIIFIPS0407 = ProjectionInfo.FromEpsgCode(26799).SetNames("NAD_1927_StatePlane_California_VII_FIPS_0407", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneCaliforniaVVentura = ProjectionInfo.FromAuthorityCode("ESRI", 102699).SetNames("NAD_1927_StatePlane_California_V_Ventura", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927StatePlaneColoradoCentralFIPS0502 = ProjectionInfo.FromEpsgCode(26754).SetNames("NAD_1927_StatePlane_Colorado_Central_FIPS_0502", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneColoradoNorthFIPS0501 = ProjectionInfo.FromEpsgCode(26753).SetNames("NAD_1927_StatePlane_Colorado_North_FIPS_0501", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneColoradoSouthFIPS0503 = ProjectionInfo.FromEpsgCode(26755).SetNames("NAD_1927_StatePlane_Colorado_South_FIPS_0503", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneConnecticutFIPS0600 = ProjectionInfo.FromEpsgCode(26756).SetNames("NAD_1927_StatePlane_Connecticut_FIPS_0600", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneDelawareFIPS0700 = ProjectionInfo.FromEpsgCode(26757).SetNames("NAD_1927_StatePlane_Delaware_FIPS_0700", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneFloridaEastFIPS0901 = ProjectionInfo.FromEpsgCode(26758).SetNames("NAD_1927_StatePlane_Florida_East_FIPS_0901", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneFloridaNorthFIPS0903 = ProjectionInfo.FromEpsgCode(26760).SetNames("NAD_1927_StatePlane_Florida_North_FIPS_0903", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneFloridaWestFIPS0902 = ProjectionInfo.FromEpsgCode(26759).SetNames("NAD_1927_StatePlane_Florida_West_FIPS_0902", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneGeorgiaEastFIPS1001 = ProjectionInfo.FromEpsgCode(26766).SetNames("NAD_1927_StatePlane_Georgia_East_FIPS_1001", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneGeorgiaWestFIPS1002 = ProjectionInfo.FromEpsgCode(26767).SetNames("NAD_1927_StatePlane_Georgia_West_FIPS_1002", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneGuamFIPS5400 = ProjectionInfo.FromAuthorityCode("ESRI", 65061).SetNames("NAD_1927_StatePlane_Guam_FIPS_5400", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIdahoCentralFIPS1102 = ProjectionInfo.FromEpsgCode(26769).SetNames("NAD_1927_StatePlane_Idaho_Central_FIPS_1102", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIdahoEastFIPS1101 = ProjectionInfo.FromEpsgCode(26768).SetNames("NAD_1927_StatePlane_Idaho_East_FIPS_1101", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIdahoWestFIPS1103 = ProjectionInfo.FromEpsgCode(26770).SetNames("NAD_1927_StatePlane_Idaho_West_FIPS_1103", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIllinoisEastFIPS1201 = ProjectionInfo.FromEpsgCode(26771).SetNames("NAD_1927_StatePlane_Illinois_East_FIPS_1201", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIllinoisWestFIPS1202 = ProjectionInfo.FromEpsgCode(26772).SetNames("NAD_1927_StatePlane_Illinois_West_FIPS_1202", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIndianaEastFIPS1301 = ProjectionInfo.FromEpsgCode(26773).SetNames("NAD_1927_StatePlane_Indiana_East_FIPS_1301", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIndianaWestFIPS1302 = ProjectionInfo.FromEpsgCode(26774).SetNames("NAD_1927_StatePlane_Indiana_West_FIPS_1302", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIowaNorthFIPS1401 = ProjectionInfo.FromEpsgCode(26775).SetNames("NAD_1927_StatePlane_Iowa_North_FIPS_1401", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneIowaSouthFIPS1402 = ProjectionInfo.FromEpsgCode(26776).SetNames("NAD_1927_StatePlane_Iowa_South_FIPS_1402", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneKansasNorthFIPS1501 = ProjectionInfo.FromEpsgCode(26777).SetNames("NAD_1927_StatePlane_Kansas_North_FIPS_1501", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneKansasSouthFIPS1502 = ProjectionInfo.FromEpsgCode(26778).SetNames("NAD_1927_StatePlane_Kansas_South_FIPS_1502", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneKentuckyNorthFIPS1601 = ProjectionInfo.FromEpsgCode(26779).SetNames("NAD_1927_StatePlane_Kentucky_North_FIPS_1601", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneKentuckySouthFIPS1602 = ProjectionInfo.FromEpsgCode(26780).SetNames("NAD_1927_StatePlane_Kentucky_South_FIPS_1602", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneLouisianaNorthFIPS1701 = ProjectionInfo.FromEpsgCode(26781).SetNames("NAD_1927_StatePlane_Louisiana_North_FIPS_1701", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneLouisianaOffshoreFIPS1703 = ProjectionInfo.FromEpsgCode(32099).SetNames("NAD_1927_StatePlane_Louisiana_Offshore_FIPS_1703", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneLouisianaSouthFIPS1702 = ProjectionInfo.FromEpsgCode(26782).SetNames("NAD_1927_StatePlane_Louisiana_South_FIPS_1702", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMaineEastFIPS1801 = ProjectionInfo.FromEpsgCode(26783).SetNames("NAD_1927_StatePlane_Maine_East_FIPS_1801", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMaineWestFIPS1802 = ProjectionInfo.FromEpsgCode(26784).SetNames("NAD_1927_StatePlane_Maine_West_FIPS_1802", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMarylandFIPS1900 = ProjectionInfo.FromEpsgCode(26785).SetNames("NAD_1927_StatePlane_Maryland_FIPS_1900", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMassachusettsFIPS2001 = ProjectionInfo.FromEpsgCode(26786).SetNames("NAD_1927_StatePlane_Massachusetts_Mainland_FIPS_2001", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMassachusettsIslFIPS2002 = ProjectionInfo.FromEpsgCode(26787).SetNames("NAD_1927_StatePlane_Massachusetts_Island_FIPS_2002", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMichiganCentralFIPS2112 = ProjectionInfo.FromAuthorityCode("EPSG", 26789).SetNames("NAD_1927_StatePlane_Michigan_Central_FIPS_2112", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927StatePlaneMichiganNorthFIPS2111 = ProjectionInfo.FromAuthorityCode("EPSG", 26788).SetNames("NAD_1927_StatePlane_Michigan_North_FIPS_2111", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927StatePlaneMichiganSouthFIPS2113 = ProjectionInfo.FromAuthorityCode("EPSG", 26790).SetNames("NAD_1927_StatePlane_Michigan_South_FIPS_2113", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927StatePlaneMinnesotaCentralFIPS2202 = ProjectionInfo.FromEpsgCode(26792).SetNames("NAD_1927_StatePlane_Minnesota_Central_FIPS_2202", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMinnesotaNorthFIPS2201 = ProjectionInfo.FromEpsgCode(26791).SetNames("NAD_1927_StatePlane_Minnesota_North_FIPS_2201", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMinnesotaSouthFIPS2203 = ProjectionInfo.FromEpsgCode(26793).SetNames("NAD_1927_StatePlane_Minnesota_South_FIPS_2203", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMississippiEastFIPS2301 = ProjectionInfo.FromEpsgCode(26794).SetNames("NAD_1927_StatePlane_Mississippi_East_FIPS_2301", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMississippiWestFIPS2302 = ProjectionInfo.FromEpsgCode(26795).SetNames("NAD_1927_StatePlane_Mississippi_West_FIPS_2302", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMissouriCentralFIPS2402 = ProjectionInfo.FromEpsgCode(26797).SetNames("NAD_1927_StatePlane_Missouri_Central_FIPS_2402", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMissouriEastFIPS2401 = ProjectionInfo.FromEpsgCode(26796).SetNames("NAD_1927_StatePlane_Missouri_East_FIPS_2401", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMissouriWestFIPS2403 = ProjectionInfo.FromEpsgCode(26798).SetNames("NAD_1927_StatePlane_Missouri_West_FIPS_2403", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMontanaCentralFIPS2502 = ProjectionInfo.FromEpsgCode(32002).SetNames("NAD_1927_StatePlane_Montana_Central_FIPS_2502", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMontanaNorthFIPS2501 = ProjectionInfo.FromEpsgCode(32001).SetNames("NAD_1927_StatePlane_Montana_North_FIPS_2501", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneMontanaSouthFIPS2503 = ProjectionInfo.FromEpsgCode(32003).SetNames("NAD_1927_StatePlane_Montana_South_FIPS_2503", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNebraskaNorthFIPS2601 = ProjectionInfo.FromEpsgCode(32005).SetNames("NAD_1927_StatePlane_Nebraska_North_FIPS_2601", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNebraskaSouthFIPS2602 = ProjectionInfo.FromEpsgCode(32006).SetNames("NAD_1927_StatePlane_Nebraska_South_FIPS_2602", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNevadaCentralFIPS2702 = ProjectionInfo.FromEpsgCode(32008).SetNames("NAD_1927_StatePlane_Nevada_Central_FIPS_2702", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNevadaEastFIPS2701 = ProjectionInfo.FromEpsgCode(32007).SetNames("NAD_1927_StatePlane_Nevada_East_FIPS_2701", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNevadaWestFIPS2703 = ProjectionInfo.FromEpsgCode(32009).SetNames("NAD_1927_StatePlane_Nevada_West_FIPS_2703", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewHampshireFIPS2800 = ProjectionInfo.FromEpsgCode(32010).SetNames("NAD_1927_StatePlane_New_Hampshire_FIPS_2800", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewJerseyFIPS2900 = ProjectionInfo.FromEpsgCode(32011).SetNames("NAD_1927_StatePlane_New_Jersey_FIPS_2900", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewMexicoCentralFIPS3002 = ProjectionInfo.FromEpsgCode(32013).SetNames("NAD_1927_StatePlane_New_Mexico_Central_FIPS_3002", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewMexicoEastFIPS3001 = ProjectionInfo.FromEpsgCode(32012).SetNames("NAD_1927_StatePlane_New_Mexico_East_FIPS_3001", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewMexicoWestFIPS3003 = ProjectionInfo.FromEpsgCode(32014).SetNames("NAD_1927_StatePlane_New_Mexico_West_FIPS_3003", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewYorkCentralFIPS3102 = ProjectionInfo.FromEpsgCode(32016).SetNames("NAD_1927_StatePlane_New_York_Central_FIPS_3102", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewYorkEastFIPS3101 = ProjectionInfo.FromEpsgCode(32015).SetNames("NAD_1927_StatePlane_New_York_East_FIPS_3101", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewYorkLongIslFIPS3104 = ProjectionInfo.FromEpsgCode(32018).SetNames("NAD_1927_StatePlane_New_York_Long_Island_FIPS_3104", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNewYorkWestFIPS3103 = ProjectionInfo.FromEpsgCode(32017).SetNames("NAD_1927_StatePlane_New_York_West_FIPS_3103", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNorthCarolinaFIPS3200 = ProjectionInfo.FromEpsgCode(32019).SetNames("NAD_1927_StatePlane_North_Carolina_FIPS_3200", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNorthDakotaNFIPS3301 = ProjectionInfo.FromEpsgCode(32020).SetNames("NAD_1927_StatePlane_North_Dakota_North_FIPS_3301", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneNorthDakotaSFIPS3302 = ProjectionInfo.FromEpsgCode(32021).SetNames("NAD_1927_StatePlane_North_Dakota_South_FIPS_3302", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneOhioNorthFIPS3401 = ProjectionInfo.FromEpsgCode(32022).SetNames("NAD_1927_StatePlane_Ohio_North_FIPS_3401", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneOhioSouthFIPS3402 = ProjectionInfo.FromEpsgCode(32023).SetNames("NAD_1927_StatePlane_Ohio_South_FIPS_3402", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneOklahomaNorthFIPS3501 = ProjectionInfo.FromEpsgCode(32024).SetNames("NAD_1927_StatePlane_Oklahoma_North_FIPS_3501", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneOklahomaSouthFIPS3502 = ProjectionInfo.FromEpsgCode(32025).SetNames("NAD_1927_StatePlane_Oklahoma_South_FIPS_3502", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneOregonNorthFIPS3601 = ProjectionInfo.FromEpsgCode(32026).SetNames("NAD_1927_StatePlane_Oregon_North_FIPS_3601", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneOregonSouthFIPS3602 = ProjectionInfo.FromEpsgCode(32027).SetNames("NAD_1927_StatePlane_Oregon_South_FIPS_3602", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlanePennsylvaniaNorthFIPS3701 = ProjectionInfo.FromEpsgCode(32028).SetNames("NAD_1927_StatePlane_Pennsylvania_North_FIPS_3701", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlanePennsylvaniaSouthFIPS3702 = ProjectionInfo.FromEpsgCode(32029).SetNames("NAD_1927_StatePlane_Pennsylvania_South_FIPS_3702", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlanePuertoRicoFIPS5201 = ProjectionInfo.FromAuthorityCode("EPSG", 32059).SetNames("NAD_1927_StatePlane_Puerto_Rico_FIPS_5201", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927StatePlaneRhodeIslandFIPS3800 = ProjectionInfo.FromEpsgCode(32030).SetNames("NAD_1927_StatePlane_Rhode_Island_FIPS_3800", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneSouthCarolinaNFIPS3901 = ProjectionInfo.FromEpsgCode(32031).SetNames("NAD_1927_StatePlane_South_Carolina_North_FIPS_3901", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneSouthCarolinaSFIPS3902 = ProjectionInfo.FromEpsgCode(32033).SetNames("NAD_1927_StatePlane_South_Carolina_South_FIPS_3902", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneSouthDakotaNFIPS4001 = ProjectionInfo.FromEpsgCode(32034).SetNames("NAD_1927_StatePlane_South_Dakota_North_FIPS_4001", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneSouthDakotaSFIPS4002 = ProjectionInfo.FromEpsgCode(32035).SetNames("NAD_1927_StatePlane_South_Dakota_South_FIPS_4002", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneTennesseeFIPS4100 = ProjectionInfo.FromEpsgCode(2204).SetNames("NAD_1927_StatePlane_Tennessee_FIPS_4100", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneTexasCentralFIPS4203 = ProjectionInfo.FromEpsgCode(32039).SetNames("NAD_1927_StatePlane_Texas_Central_FIPS_4203", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneTexasNCentralFIPS4202 = ProjectionInfo.FromEpsgCode(32038).SetNames("NAD_1927_StatePlane_Texas_North_Central_FIPS_4202", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneTexasNorthFIPS4201 = ProjectionInfo.FromEpsgCode(32037).SetNames("NAD_1927_StatePlane_Texas_North_FIPS_4201", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneTexasSCentralFIPS4204 = ProjectionInfo.FromEpsgCode(32040).SetNames("NAD_1927_StatePlane_Texas_South_Central_FIPS_4204", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneTexasSouthFIPS4205 = ProjectionInfo.FromEpsgCode(32041).SetNames("NAD_1927_StatePlane_Texas_South_FIPS_4205", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneUtahCentralFIPS4302 = ProjectionInfo.FromEpsgCode(32043).SetNames("NAD_1927_StatePlane_Utah_Central_FIPS_4302", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneUtahNorthFIPS4301 = ProjectionInfo.FromEpsgCode(32042).SetNames("NAD_1927_StatePlane_Utah_North_FIPS_4301", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneUtahSouthFIPS4303 = ProjectionInfo.FromEpsgCode(32044).SetNames("NAD_1927_StatePlane_Utah_South_FIPS_4303", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneVermontFIPS3400 = ProjectionInfo.FromEpsgCode(32045).SetNames("NAD_1927_StatePlane_Vermont_FIPS_4400", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneVirginiaNorthFIPS4501 = ProjectionInfo.FromEpsgCode(32046).SetNames("NAD_1927_StatePlane_Virginia_North_FIPS_4501", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneVirginiaSouthFIPS4502 = ProjectionInfo.FromEpsgCode(32047).SetNames("NAD_1927_StatePlane_Virginia_South_FIPS_4502", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneVirginIslStCroixFIPS5202 = ProjectionInfo.FromAuthorityCode("EPSG", 32060).SetNames("NAD_1927_StatePlane_Virgin_Islands_St_Croix_FIPS_5202", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927StatePlaneWashingtonNorthFIPS4601 = ProjectionInfo.FromEpsgCode(32048).SetNames("NAD_1927_StatePlane_Washington_North_FIPS_4601", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWashingtonSouthFIPS4602 = ProjectionInfo.FromEpsgCode(32049).SetNames("NAD_1927_StatePlane_Washington_South_FIPS_4602", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWestVirginiaNFIPS4701 = ProjectionInfo.FromEpsgCode(32050).SetNames("NAD_1927_StatePlane_West_Virginia_North_FIPS_4701", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWestVirginiaSFIPS4702 = ProjectionInfo.FromEpsgCode(32051).SetNames("NAD_1927_StatePlane_West_Virginia_South_FIPS_4702", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWisconsinCentralFIPS4802 = ProjectionInfo.FromEpsgCode(32053).SetNames("NAD_1927_StatePlane_Wisconsin_Central_FIPS_4802", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWisconsinNorthFIPS4801 = ProjectionInfo.FromEpsgCode(32052).SetNames("NAD_1927_StatePlane_Wisconsin_North_FIPS_4801", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWisconsinSouthFIPS4803 = ProjectionInfo.FromEpsgCode(32054).SetNames("NAD_1927_StatePlane_Wisconsin_South_FIPS_4803", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWyomingEastFIPS4901 = ProjectionInfo.FromEpsgCode(32055).SetNames("NAD_1927_StatePlane_Wyoming_East_FIPS_4901", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWyomingECentralFIPS4902 = ProjectionInfo.FromEpsgCode(32056).SetNames("NAD_1927_StatePlane_Wyoming_East_Central_FIPS_4902", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWyomingWCentralFIPS4903 = ProjectionInfo.FromEpsgCode(32057).SetNames("NAD_1927_StatePlane_Wyoming_West_Central_FIPS_4903", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927StatePlaneWyomingWestFIPS4904 = ProjectionInfo.FromEpsgCode(32058).SetNames("NAD_1927_StatePlane_Wyoming_West_FIPS_4904", "GCS_North_American_1927", "D_North_American_1927");
        }

        #endregion
    }
}

#pragma warning restore 1591