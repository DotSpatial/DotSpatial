// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:59:34 PM
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
    /// This class contains predefined CoordinateSystems for NAD1983USFeet.
    /// </summary>
    public class NAD1983USFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983StatePlaneAlabamaEastFIPS0101USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlabamaWestFIPS0102USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska10FIPS5010USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska1FIPS5001USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska2FIPS5002USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska3FIPS5003USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska4FIPS5004USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska5FIPS5005USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska6FIPS5006USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska7FIPS5007USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska8FIPS5008USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska9FIPS5009USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaCentralFIPS0202USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaEastFIPS0201USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaWestFIPS0203USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasNorthFIPS0301USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasSouthFIPS0302USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIFIPS0401USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIFIPS0402USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIIFIPS0403USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIVFIPS0404USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVFIPS0405USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVIFIPS0406USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoCentralFIPS0502USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoNorthFIPS0501USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoSouthFIPS0503USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneConnecticutFIPS0600USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneDelawareFIPS0700USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaEastFIPS0901USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaNorthFIPS0903USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaWestFIPS0902USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaEastFIPS1001USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaWestFIPS1002USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneGuamFIPS5400USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii1FIPS5101USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii2FIPS5102USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii3FIPS5103USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii4FIPS5104USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii5FIPS5105USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoCentralFIPS1102USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoEastFIPS1101USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoWestFIPS1103USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisEastFIPS1201USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisWestFIPS1202USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaEastFIPS1301USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaWestFIPS1302USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIowaNorthFIPS1401USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneIowaSouthFIPS1402USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneKansasNorthFIPS1501USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneKansasSouthFIPS1502USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyFIPS1600USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyNorthFIPS1601USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckySouthFIPS1602USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaNorthFIPS1701USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaOffshoreFIPS1703USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaSouthFIPS1702USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMaineEastFIPS1801USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMaineWestFIPS1802USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMarylandFIPS1900USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsFIPS2001USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsIslFIPS2002USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganCentralFIPS2112USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganNorthFIPS2111USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganSouthFIPS2113USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaCentralFIPS2202USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaNorthFIPS2201USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaSouthFIPS2203USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiEastFIPS2301USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiWestFIPS2302USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriCentralFIPS2402USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriEastFIPS2401USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriWestFIPS2403USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneMontanaFIPS2500USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNebraskaFIPS2600USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaCentralFIPS2702USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaEastFIPS2701USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaWestFIPS2703USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewHampshireFIPS2800USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewJerseyFIPS2900USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoCentralFIPS3002USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoEastFIPS3001USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoWestFIPS3003USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkCentralFIPS3102USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkEastFIPS3101USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkLongIslFIPS3104USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkWestFIPS3103USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthCarolinaFIPS3200USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaNFIPS3301USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaSFIPS3302USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOhioNorthFIPS3401USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOhioSouthFIPS3402USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaNorthFIPS3501USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaSouthFIPS3502USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOregonNorthFIPS3601USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneOregonSouthFIPS3602USFeet;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaNorthFIPS3701USFeet;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaSouthFIPS3702USFeet;
        public readonly ProjectionInfo NAD1983StatePlanePuertoRicoVirginIslFIPS5200USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneRhodeIslandFIPS3800USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneSouthCarolinaFIPS3900USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaNFIPS4001USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaSFIPS4002USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneTennesseeFIPS4100USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasCentralFIPS4203USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNCentralFIPS4202USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNorthFIPS4201USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSCentralFIPS4204USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSouthFIPS4205USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahCentralFIPS4302USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahNorthFIPS4301USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahSouthFIPS4303USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneVermontFIPS4400USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaNorthFIPS4501USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaSouthFIPS4502USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonNorthFIPS4601USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonSouthFIPS4602USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaNFIPS4701USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaSFIPS4702USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinCentralFIPS4802USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinNorthFIPS4801USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinSouthFIPS4803USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingEastFIPS4901USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingECentralFIPS4902USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWCentralFIPS4903USFeet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWestFIPS4904USFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983USFeet.
        /// </summary>
        public NAD1983USFeet()
        {
            NAD1983StatePlaneAlabamaEastFIPS0101USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102629).SetNames("NAD_1983_StatePlane_Alabama_East_FIPS_0101_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlabamaWestFIPS0102USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102630).SetNames("NAD_1983_StatePlane_Alabama_West_FIPS_0102_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska10FIPS5010USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102640).SetNames("NAD_1983_StatePlane_Alaska_10_FIPS_5010_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska1FIPS5001USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102631).SetNames("NAD_1983_StatePlane_Alaska_1_FIPS_5001_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska2FIPS5002USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102632).SetNames("NAD_1983_StatePlane_Alaska_2_FIPS_5002_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska3FIPS5003USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102633).SetNames("NAD_1983_StatePlane_Alaska_3_FIPS_5003_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska4FIPS5004USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102634).SetNames("NAD_1983_StatePlane_Alaska_4_FIPS_5004_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska5FIPS5005USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102635).SetNames("NAD_1983_StatePlane_Alaska_5_FIPS_5005_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska6FIPS5006USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102636).SetNames("NAD_1983_StatePlane_Alaska_6_FIPS_5006_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska7FIPS5007USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102637).SetNames("NAD_1983_StatePlane_Alaska_7_FIPS_5007_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska8FIPS5008USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102638).SetNames("NAD_1983_StatePlane_Alaska_8_FIPS_5008_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneAlaska9FIPS5009USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102639).SetNames("NAD_1983_StatePlane_Alaska_9_FIPS_5009_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaCentralFIPS0202USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102649).SetNames("NAD_1983_StatePlane_Arizona_Central_FIPS_0202_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaEastFIPS0201USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102648).SetNames("NAD_1983_StatePlane_Arizona_East_FIPS_0201_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArizonaWestFIPS0203USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102650).SetNames("NAD_1983_StatePlane_Arizona_West_FIPS_0203_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArkansasNorthFIPS0301USFeet = ProjectionInfo.FromEpsgCode(3433).SetNames("NAD_1983_StatePlane_Arkansas_North_FIPS_0301_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneArkansasSouthFIPS0302USFeet = ProjectionInfo.FromEpsgCode(3434).SetNames("NAD_1983_StatePlane_Arkansas_South_FIPS_0302_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIFIPS0401USFeet = ProjectionInfo.FromEpsgCode(2225).SetNames("NAD_1983_StatePlane_California_I_FIPS_0401_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIIFIPS0402USFeet = ProjectionInfo.FromEpsgCode(2226).SetNames("NAD_1983_StatePlane_California_II_FIPS_0402_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIIIFIPS0403USFeet = ProjectionInfo.FromEpsgCode(2227).SetNames("NAD_1983_StatePlane_California_III_FIPS_0403_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaIVFIPS0404USFeet = ProjectionInfo.FromEpsgCode(2228).SetNames("NAD_1983_StatePlane_California_IV_FIPS_0404_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaVFIPS0405USFeet = ProjectionInfo.FromEpsgCode(2229).SetNames("NAD_1983_StatePlane_California_V_FIPS_0405_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneCaliforniaVIFIPS0406USFeet = ProjectionInfo.FromEpsgCode(2230).SetNames("NAD_1983_StatePlane_California_VI_FIPS_0406_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneColoradoCentralFIPS0502USFeet = ProjectionInfo.FromEpsgCode(2232).SetNames("NAD_1983_StatePlane_Colorado_Central_FIPS_0502_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneColoradoNorthFIPS0501USFeet = ProjectionInfo.FromEpsgCode(2231).SetNames("NAD_1983_StatePlane_Colorado_North_FIPS_0501_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneColoradoSouthFIPS0503USFeet = ProjectionInfo.FromEpsgCode(2233).SetNames("NAD_1983_StatePlane_Colorado_South_FIPS_0503_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneConnecticutFIPS0600USFeet = ProjectionInfo.FromEpsgCode(2234).SetNames("NAD_1983_StatePlane_Connecticut_FIPS_0600_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneDelawareFIPS0700USFeet = ProjectionInfo.FromEpsgCode(2235).SetNames("NAD_1983_StatePlane_Delaware_FIPS_0700_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneFloridaEastFIPS0901USFeet = ProjectionInfo.FromEpsgCode(2236).SetNames("NAD_1983_StatePlane_Florida_East_FIPS_0901_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneFloridaNorthFIPS0903USFeet = ProjectionInfo.FromEpsgCode(2238).SetNames("NAD_1983_StatePlane_Florida_North_FIPS_0903_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneFloridaWestFIPS0902USFeet = ProjectionInfo.FromEpsgCode(2237).SetNames("NAD_1983_StatePlane_Florida_West_FIPS_0902_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneGeorgiaEastFIPS1001USFeet = ProjectionInfo.FromEpsgCode(2239).SetNames("NAD_1983_StatePlane_Georgia_East_FIPS_1001_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneGeorgiaWestFIPS1002USFeet = ProjectionInfo.FromEpsgCode(2240).SetNames("NAD_1983_StatePlane_Georgia_West_FIPS_1002_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneGuamFIPS5400USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102766).SetNames("NAD_1983_StatePlane_Guam_FIPS_5400_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii1FIPS5101USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102661).SetNames("NAD_1983_StatePlane_Hawaii_1_FIPS_5101_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii2FIPS5102USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102662).SetNames("NAD_1983_StatePlane_Hawaii_2_FIPS_5102_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii3FIPS5103USFeet = ProjectionInfo.FromEpsgCode(3759).SetNames("NAD_1983_StatePlane_Hawaii_3_FIPS_5103_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii4FIPS5104USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102664).SetNames("NAD_1983_StatePlane_Hawaii_4_FIPS_5104_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneHawaii5FIPS5105USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102665).SetNames("NAD_1983_StatePlane_Hawaii_5_FIPS_5105_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIdahoCentralFIPS1102USFeet = ProjectionInfo.FromEpsgCode(2242).SetNames("NAD_1983_StatePlane_Idaho_Central_FIPS_1102_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIdahoEastFIPS1101USFeet = ProjectionInfo.FromEpsgCode(2241).SetNames("NAD_1983_StatePlane_Idaho_East_FIPS_1101_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIdahoWestFIPS1103USFeet = ProjectionInfo.FromEpsgCode(2243).SetNames("NAD_1983_StatePlane_Idaho_West_FIPS_1103_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIllinoisEastFIPS1201USFeet = ProjectionInfo.FromEpsgCode(3435).SetNames("NAD_1983_StatePlane_Illinois_East_FIPS_1201_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIllinoisWestFIPS1202USFeet = ProjectionInfo.FromEpsgCode(3436).SetNames("NAD_1983_StatePlane_Illinois_West_FIPS_1202_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIndianaEastFIPS1301USFeet = ProjectionInfo.FromEpsgCode(2965).SetNames("NAD_1983_StatePlane_Indiana_East_FIPS_1301_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIndianaWestFIPS1302USFeet = ProjectionInfo.FromEpsgCode(2966).SetNames("NAD_1983_StatePlane_Indiana_West_FIPS_1302_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIowaNorthFIPS1401USFeet = ProjectionInfo.FromEpsgCode(3417).SetNames("NAD_1983_StatePlane_Iowa_North_FIPS_1401_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneIowaSouthFIPS1402USFeet = ProjectionInfo.FromEpsgCode(3418).SetNames("NAD_1983_StatePlane_Iowa_South_FIPS_1402_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKansasNorthFIPS1501USFeet = ProjectionInfo.FromEpsgCode(3419).SetNames("NAD_1983_StatePlane_Kansas_North_FIPS_1501_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKansasSouthFIPS1502USFeet = ProjectionInfo.FromEpsgCode(3420).SetNames("NAD_1983_StatePlane_Kansas_South_FIPS_1502_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKentuckyFIPS1600USFeet = ProjectionInfo.FromEpsgCode(3089).SetNames("NAD_1983_StatePlane_Kentucky_FIPS_1600_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKentuckyNorthFIPS1601USFeet = ProjectionInfo.FromEpsgCode(2246).SetNames("NAD_1983_StatePlane_Kentucky_North_FIPS_1601_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneKentuckySouthFIPS1602USFeet = ProjectionInfo.FromEpsgCode(2247).SetNames("NAD_1983_StatePlane_Kentucky_South_FIPS_1602_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneLouisianaNorthFIPS1701USFeet = ProjectionInfo.FromEpsgCode(3451).SetNames("NAD_1983_StatePlane_Louisiana_North_FIPS_1701_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneLouisianaOffshoreFIPS1703USFeet = ProjectionInfo.FromEpsgCode(3453).SetNames("NAD_1983_StatePlane_Louisiana_Offshore_FIPS_1703_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneLouisianaSouthFIPS1702USFeet = ProjectionInfo.FromEpsgCode(3452).SetNames("NAD_1983_StatePlane_Louisiana_South_FIPS_1702_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMaineEastFIPS1801USFeet = ProjectionInfo.FromEpsgCode(26847).SetNames("NAD_1983_StatePlane_Maine_East_FIPS_1801_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMaineWestFIPS1802USFeet = ProjectionInfo.FromEpsgCode(26848).SetNames("NAD_1983_StatePlane_Maine_West_FIPS_1802_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMarylandFIPS1900USFeet = ProjectionInfo.FromEpsgCode(2248).SetNames("NAD_1983_StatePlane_Maryland_FIPS_1900_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMassachusettsFIPS2001USFeet = ProjectionInfo.FromEpsgCode(2249).SetNames("NAD_1983_StatePlane_Massachusetts_Mainland_FIPS_2001_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMassachusettsIslFIPS2002USFeet = ProjectionInfo.FromEpsgCode(2250).SetNames("NAD_1983_StatePlane_Massachusetts_Island_FIPS_2002_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganCentralFIPS2112USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102689).SetNames("NAD_1983_StatePlane_Michigan_Central_FIPS_2112_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganNorthFIPS2111USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102688).SetNames("NAD_1983_StatePlane_Michigan_North_FIPS_2111_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMichiganSouthFIPS2113USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102690).SetNames("NAD_1983_StatePlane_Michigan_South_FIPS_2113_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMinnesotaCentralFIPS2202USFeet = ProjectionInfo.FromEpsgCode(26850).SetNames("NAD_1983_StatePlane_Minnesota_Central_FIPS_2202_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMinnesotaNorthFIPS2201USFeet = ProjectionInfo.FromEpsgCode(26849).SetNames("NAD_1983_StatePlane_Minnesota_North_FIPS_2201_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMinnesotaSouthFIPS2203USFeet = ProjectionInfo.FromEpsgCode(26851).SetNames("NAD_1983_StatePlane_Minnesota_South_FIPS_2203_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMississippiEastFIPS2301USFeet = ProjectionInfo.FromEpsgCode(2254).SetNames("NAD_1983_StatePlane_Mississippi_East_FIPS_2301_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMississippiWestFIPS2302USFeet = ProjectionInfo.FromEpsgCode(2255).SetNames("NAD_1983_StatePlane_Mississippi_West_FIPS_2302_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMissouriCentralFIPS2402USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102697).SetNames("NAD_1983_StatePlane_Missouri_Central_FIPS_2402_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMissouriEastFIPS2401USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102696).SetNames("NAD_1983_StatePlane_Missouri_East_FIPS_2401_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMissouriWestFIPS2403USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102698).SetNames("NAD_1983_StatePlane_Missouri_West_FIPS_2403_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneMontanaFIPS2500USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102700).SetNames("NAD_1983_StatePlane_Montana_FIPS_2500_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNebraskaFIPS2600USFeet = ProjectionInfo.FromEpsgCode(26852).SetNames("NAD_1983_StatePlane_Nebraska_FIPS_2600_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNevadaCentralFIPS2702USFeet = ProjectionInfo.FromEpsgCode(3422).SetNames("NAD_1983_StatePlane_Nevada_Central_FIPS_2702_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNevadaEastFIPS2701USFeet = ProjectionInfo.FromEpsgCode(3421).SetNames("NAD_1983_StatePlane_Nevada_East_FIPS_2701_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNevadaWestFIPS2703USFeet = ProjectionInfo.FromEpsgCode(3423).SetNames("NAD_1983_StatePlane_Nevada_West_FIPS_2703_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewHampshireFIPS2800USFeet = ProjectionInfo.FromEpsgCode(3437).SetNames("NAD_1983_StatePlane_New_Hampshire_FIPS_2800_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewJerseyFIPS2900USFeet = ProjectionInfo.FromEpsgCode(3424).SetNames("NAD_1983_StatePlane_New_Jersey_FIPS_2900_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewMexicoCentralFIPS3002USFeet = ProjectionInfo.FromEpsgCode(2258).SetNames("NAD_1983_StatePlane_New_Mexico_Central_FIPS_3002_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewMexicoEastFIPS3001USFeet = ProjectionInfo.FromEpsgCode(2257).SetNames("NAD_1983_StatePlane_New_Mexico_East_FIPS_3001_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewMexicoWestFIPS3003USFeet = ProjectionInfo.FromEpsgCode(2259).SetNames("NAD_1983_StatePlane_New_Mexico_West_FIPS_3003_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkCentralFIPS3102USFeet = ProjectionInfo.FromEpsgCode(2261).SetNames("NAD_1983_StatePlane_New_York_Central_FIPS_3102_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkEastFIPS3101USFeet = ProjectionInfo.FromEpsgCode(2260).SetNames("NAD_1983_StatePlane_New_York_East_FIPS_3101_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkLongIslFIPS3104USFeet = ProjectionInfo.FromEpsgCode(2263).SetNames("NAD_1983_StatePlane_New_York_Long_Island_FIPS_3104_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNewYorkWestFIPS3103USFeet = ProjectionInfo.FromEpsgCode(2262).SetNames("NAD_1983_StatePlane_New_York_West_FIPS_3103_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthCarolinaFIPS3200USFeet = ProjectionInfo.FromEpsgCode(2264).SetNames("NAD_1983_StatePlane_North_Carolina_FIPS_3200_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthDakotaNFIPS3301USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102720).SetNames("NAD_1983_StatePlane_North_Dakota_North_FIPS_3301_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneNorthDakotaSFIPS3302USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102721).SetNames("NAD_1983_StatePlane_North_Dakota_South_FIPS_3302_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOhioNorthFIPS3401USFeet = ProjectionInfo.FromEpsgCode(3734).SetNames("NAD_1983_StatePlane_Ohio_North_FIPS_3401_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOhioSouthFIPS3402USFeet = ProjectionInfo.FromEpsgCode(3735).SetNames("NAD_1983_StatePlane_Ohio_South_FIPS_3402_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOklahomaNorthFIPS3501USFeet = ProjectionInfo.FromEpsgCode(2267).SetNames("NAD_1983_StatePlane_Oklahoma_North_FIPS_3501_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOklahomaSouthFIPS3502USFeet = ProjectionInfo.FromEpsgCode(2268).SetNames("NAD_1983_StatePlane_Oklahoma_South_FIPS_3502_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOregonNorthFIPS3601USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102726).SetNames("NAD_1983_StatePlane_Oregon_North_FIPS_3601_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneOregonSouthFIPS3602USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102727).SetNames("NAD_1983_StatePlane_Oregon_South_FIPS_3602_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlanePennsylvaniaNorthFIPS3701USFeet = ProjectionInfo.FromEpsgCode(2271).SetNames("NAD_1983_StatePlane_Pennsylvania_North_FIPS_3701_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlanePennsylvaniaSouthFIPS3702USFeet = ProjectionInfo.FromEpsgCode(2272).SetNames("NAD_1983_StatePlane_Pennsylvania_South_FIPS_3702_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlanePuertoRicoVirginIslFIPS5200USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102761).SetNames("NAD_1983_StatePlane_Puerto_Rico_Virgin_Islands_FIPS_5200_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneRhodeIslandFIPS3800USFeet = ProjectionInfo.FromEpsgCode(3438).SetNames("NAD_1983_StatePlane_Rhode_Island_FIPS_3800_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneSouthCarolinaFIPS3900USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102733).SetNames("NAD_1983_StatePlane_South_Carolina_FIPS_3900_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneSouthDakotaNFIPS4001USFeet = ProjectionInfo.FromEpsgCode(3454).SetNames("NAD_1983_StatePlane_South_Dakota_North_FIPS_4001_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneSouthDakotaSFIPS4002USFeet = ProjectionInfo.FromEpsgCode(3455).SetNames("NAD_1983_StatePlane_South_Dakota_South_FIPS_4002_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTennesseeFIPS4100USFeet = ProjectionInfo.FromEpsgCode(2274).SetNames("NAD_1983_StatePlane_Tennessee_FIPS_4100_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasCentralFIPS4203USFeet = ProjectionInfo.FromEpsgCode(2277).SetNames("NAD_1983_StatePlane_Texas_Central_FIPS_4203_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasNCentralFIPS4202USFeet = ProjectionInfo.FromEpsgCode(2276).SetNames("NAD_1983_StatePlane_Texas_North_Central_FIPS_4202_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasNorthFIPS4201USFeet = ProjectionInfo.FromEpsgCode(2275).SetNames("NAD_1983_StatePlane_Texas_North_FIPS_4201_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasSCentralFIPS4204USFeet = ProjectionInfo.FromEpsgCode(2278).SetNames("NAD_1983_StatePlane_Texas_South_Central_FIPS_4204_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneTexasSouthFIPS4205USFeet = ProjectionInfo.FromEpsgCode(2279).SetNames("NAD_1983_StatePlane_Texas_South_FIPS_4205_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahCentralFIPS4302USFeet = ProjectionInfo.FromEpsgCode(3566).SetNames("NAD_1983_StatePlane_Utah_Central_FIPS_4302_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahNorthFIPS4301USFeet = ProjectionInfo.FromEpsgCode(3560).SetNames("NAD_1983_StatePlane_Utah_North_FIPS_4301_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneUtahSouthFIPS4303USFeet = ProjectionInfo.FromEpsgCode(3567).SetNames("NAD_1983_StatePlane_Utah_South_FIPS_4303_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneVermontFIPS4400USFeet = ProjectionInfo.FromAuthorityCode("EPSG", 102745).SetNames("NAD_1983_StatePlane_Vermont_FIPS_4400_Feet", "GCS_North_American_1983", "D_North_American_1983"); // missing
            NAD1983StatePlaneVirginiaNorthFIPS4501USFeet = ProjectionInfo.FromEpsgCode(2283).SetNames("NAD_1983_StatePlane_Virginia_North_FIPS_4501_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneVirginiaSouthFIPS4502USFeet = ProjectionInfo.FromEpsgCode(2284).SetNames("NAD_1983_StatePlane_Virginia_South_FIPS_4502_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWashingtonNorthFIPS4601USFeet = ProjectionInfo.FromEpsgCode(2285).SetNames("NAD_1983_StatePlane_Washington_North_FIPS_4601_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWashingtonSouthFIPS4602USFeet = ProjectionInfo.FromEpsgCode(2286).SetNames("NAD_1983_StatePlane_Washington_South_FIPS_4602_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWestVirginiaNFIPS4701USFeet = ProjectionInfo.FromEpsgCode(26853).SetNames("NAD_1983_StatePlane_West_Virginia_North_FIPS_4701_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWestVirginiaSFIPS4702USFeet = ProjectionInfo.FromEpsgCode(26854).SetNames("NAD_1983_StatePlane_West_Virginia_South_FIPS_4702_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWisconsinCentralFIPS4802USFeet = ProjectionInfo.FromEpsgCode(2288).SetNames("NAD_1983_StatePlane_Wisconsin_Central_FIPS_4802_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWisconsinNorthFIPS4801USFeet = ProjectionInfo.FromEpsgCode(2287).SetNames("NAD_1983_StatePlane_Wisconsin_North_FIPS_4801_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWisconsinSouthFIPS4803USFeet = ProjectionInfo.FromEpsgCode(2289).SetNames("NAD_1983_StatePlane_Wisconsin_South_FIPS_4803_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingEastFIPS4901USFeet = ProjectionInfo.FromEpsgCode(3736).SetNames("NAD_1983_StatePlane_Wyoming_East_FIPS_4901_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingECentralFIPS4902USFeet = ProjectionInfo.FromEpsgCode(3737).SetNames("NAD_1983_StatePlane_Wyoming_East_Central_FIPS_4902_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingWCentralFIPS4903USFeet = ProjectionInfo.FromEpsgCode(3738).SetNames("NAD_1983_StatePlane_Wyoming_West_Central_FIPS_4903_Feet", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983StatePlaneWyomingWestFIPS4904USFeet = ProjectionInfo.FromEpsgCode(3739).SetNames("NAD_1983_StatePlane_Wyoming_West_FIPS_4904_Feet", "GCS_North_American_1983", "D_North_American_1983");
        }

        #endregion
    }
}

#pragma warning restore 1591