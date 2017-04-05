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
    /// This class contains predefined CoordinateSystems for NAD1983NSRS2007USFeet.
    /// </summary>
    public class NAD1983NSRS2007USFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArkansasNorthFIPS0301USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneArkansasSouthFIPS0302USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIFIPS0401USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIIFIPS0402USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIIIFIPS0403USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaIVFIPS0404USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaVFIPS0405USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneCaliforniaVIFIPS0406USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneColoradoCentralFIPS0502USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneColoradoNorthFIPS0501USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneColoradoSouthFIPS0503USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneConnecticutFIPS0600USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneDelawareFIPS0700USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneFloridaEastFIPS0901USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneFloridaNorthFIPS0903USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneFloridaWestFIPS0902USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneGeorgiaEastFIPS1001USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneGeorgiaWestFIPS1002USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIdahoCentralFIPS1102USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIdahoEastFIPS1101USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIdahoWestFIPS1103USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIllinoisEastFIPS1201USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIllinoisWestFIPS1202USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIndianaEastFIPS1301USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIndianaWestFIPS1302USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIowaNorthFIPS1401USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneIowaSouthFIPS1402USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKansasNorthFIPS1501USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKansasSouthFIPS1502USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKentuckyFIPS1600USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKentuckyNorthFIPS1601USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneKentuckySouthFIPS1602USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneLouisianaNorthFIPS1701USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneLouisianaSouthFIPS1702USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMaineEastFIPS1801USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMaineWestFIPS1802USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMarylandFIPS1900USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMassachusettsFIPS2001USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMassachusettsIslFIPS2002USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMinnesotaCentralFIPS2202USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMinnesotaNorthFIPS2201USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMinnesotaSouthFIPS2203USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMississippiEastFIPS2301USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneMississippiWestFIPS2302USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNebraskaFIPS2600USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNevadaCentralFIPS2702USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNevadaEastFIPS2701USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNevadaWestFIPS2703USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewHampshireFIPS2800USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewJerseyFIPS2900USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewMexicoCentralFIPS3002USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewMexicoEastFIPS3001USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewMexicoWestFIPS3003USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkCentralFIPS3102USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkEastFIPS3101USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkLongIslFIPS3104USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNewYorkWestFIPS3103USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneNorthCarolinaFIPS3200USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOhioNorthFIPS3401USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOhioSouthFIPS3402USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOklahomaNorthFIPS3501USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneOklahomaSouthFIPS3502USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlanePennsylvaniaNorthFIPS3701USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlanePennsylvaniaSouthFIPS3702USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneRhodeIslandFIPS3800USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneSouthDakotaNFIPS4001USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneSouthDakotaSFIPS4002USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTennesseeFIPS4100USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasCentralFIPS4203USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasNCentralFIPS4202USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasNorthFIPS4201USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasSCentralFIPS4204USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneTexasSouthFIPS4205USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahCentralFIPS4302USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahNorthFIPS4301USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneUtahSouthFIPS4303USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneVirginiaNorthFIPS4501USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneVirginiaSouthFIPS4502USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWashingtonNorthFIPS4601USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWashingtonSouthFIPS4602USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWestVirginiaNFIPS4701USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWestVirginiaSFIPS4702USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWisconsinCentralFIPS4802USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWisconsinNorthFIPS4801USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWisconsinSouthFIPS4803USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingEastFIPS4901USFeet;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingECentralFIPS4902USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingWCentralFIPS4903USFt;
        public readonly ProjectionInfo NAD1983NSRS2007StatePlaneWyomingWestFIPS4904USFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NAD1983NSRS2007USFeet.
        /// </summary>
        public NAD1983NSRS2007USFeet()
        {
            NAD1983NSRS2007StatePlaneArkansasNorthFIPS0301USFeet = ProjectionInfo.FromEpsgCode(3485).SetNames("NAD_1983_NSRS2007_StatePlane_Arkansas_North_FIPS_0301_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneArkansasSouthFIPS0302USFeet = ProjectionInfo.FromEpsgCode(3487).SetNames("NAD_1983_NSRS2007_StatePlane_Arkansas_South_FIPS_0302_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIFIPS0401USFeet = ProjectionInfo.FromEpsgCode(3490).SetNames("NAD_1983_NSRS2007_StatePlane_California_I_FIPS_0401_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIIFIPS0402USFeet = ProjectionInfo.FromEpsgCode(3492).SetNames("NAD_1983_NSRS2007_StatePlane_California_II_FIPS_0402_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIIIFIPS0403USFeet = ProjectionInfo.FromEpsgCode(3494).SetNames("NAD_1983_NSRS2007_StatePlane_California_III_FIPS_0403_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaIVFIPS0404USFeet = ProjectionInfo.FromEpsgCode(3496).SetNames("NAD_1983_NSRS2007_StatePlane_California_IV_FIPS_0404_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaVFIPS0405USFeet = ProjectionInfo.FromEpsgCode(3498).SetNames("NAD_1983_NSRS2007_StatePlane_California_V_FIPS_0405_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneCaliforniaVIFIPS0406USFeet = ProjectionInfo.FromEpsgCode(3500).SetNames("NAD_1983_NSRS2007_StatePlane_California_VI_FIPS_0406_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneColoradoCentralFIPS0502USFt = ProjectionInfo.FromEpsgCode(3502).SetNames("NAD_1983_NSRS2007_StatePlane_Colorado_Central_FIPS_0502_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneColoradoNorthFIPS0501USFeet = ProjectionInfo.FromEpsgCode(3504).SetNames("NAD_1983_NSRS2007_StatePlane_Colorado_North_FIPS_0501_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneColoradoSouthFIPS0503USFeet = ProjectionInfo.FromEpsgCode(3506).SetNames("NAD_1983_NSRS2007_StatePlane_Colorado_South_FIPS_0503_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneConnecticutFIPS0600USFeet = ProjectionInfo.FromEpsgCode(3508).SetNames("NAD_1983_NSRS2007_StatePlane_Connecticut_FIPS_0600_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneDelawareFIPS0700USFeet = ProjectionInfo.FromEpsgCode(3510).SetNames("NAD_1983_NSRS2007_StatePlane_Delaware_FIPS_0700_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneFloridaEastFIPS0901USFeet = ProjectionInfo.FromEpsgCode(3512).SetNames("NAD_1983_NSRS2007_StatePlane_Florida_East_FIPS_0901_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneFloridaNorthFIPS0903USFeet = ProjectionInfo.FromEpsgCode(3515).SetNames("NAD_1983_NSRS2007_StatePlane_Florida_North_FIPS_0903_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneFloridaWestFIPS0902USFeet = ProjectionInfo.FromEpsgCode(3517).SetNames("NAD_1983_NSRS2007_StatePlane_Florida_West_FIPS_0902_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneGeorgiaEastFIPS1001USFeet = ProjectionInfo.FromEpsgCode(3519).SetNames("NAD_1983_NSRS2007_StatePlane_Georgia_East_FIPS_1001_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneGeorgiaWestFIPS1002USFeet = ProjectionInfo.FromEpsgCode(3521).SetNames("NAD_1983_NSRS2007_StatePlane_Georgia_West_FIPS_1002_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIdahoCentralFIPS1102USFeet = ProjectionInfo.FromEpsgCode(3523).SetNames("NAD_1983_NSRS2007_StatePlane_Idaho_Central_FIPS_1102_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIdahoEastFIPS1101USFeet = ProjectionInfo.FromEpsgCode(3525).SetNames("NAD_1983_NSRS2007_StatePlane_Idaho_East_FIPS_1101_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIdahoWestFIPS1103USFeet = ProjectionInfo.FromEpsgCode(3527).SetNames("NAD_1983_NSRS2007_StatePlane_Idaho_West_FIPS_1103_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIllinoisEastFIPS1201USFeet = ProjectionInfo.FromEpsgCode(3529).SetNames("NAD_1983_NSRS2007_StatePlane_Illinois_East_FIPS_1201_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIllinoisWestFIPS1202USFeet = ProjectionInfo.FromEpsgCode(3531).SetNames("NAD_1983_NSRS2007_StatePlane_Illinois_West_FIPS_1202_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIndianaEastFIPS1301USFeet = ProjectionInfo.FromEpsgCode(3533).SetNames("NAD_1983_NSRS2007_StatePlane_Indiana_East_FIPS_1301_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIndianaWestFIPS1302USFeet = ProjectionInfo.FromEpsgCode(3535).SetNames("NAD_1983_NSRS2007_StatePlane_Indiana_West_FIPS_1302_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIowaNorthFIPS1401USFeet = ProjectionInfo.FromEpsgCode(3537).SetNames("NAD_1983_NSRS2007_StatePlane_Iowa_North_FIPS_1401_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneIowaSouthFIPS1402USFeet = ProjectionInfo.FromEpsgCode(3539).SetNames("NAD_1983_NSRS2007_StatePlane_Iowa_South_FIPS_1402_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKansasNorthFIPS1501USFeet = ProjectionInfo.FromEpsgCode(3541).SetNames("NAD_1983_NSRS2007_StatePlane_Kansas_North_FIPS_1501_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKansasSouthFIPS1502USFeet = ProjectionInfo.FromEpsgCode(3543).SetNames("NAD_1983_NSRS2007_StatePlane_Kansas_South_FIPS_1502_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKentuckyFIPS1600USFeet = ProjectionInfo.FromEpsgCode(3547).SetNames("NAD_1983_NSRS2007_StatePlane_Kentucky_FIPS_1600_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKentuckyNorthFIPS1601USFeet = ProjectionInfo.FromEpsgCode(3545).SetNames("NAD_1983_NSRS2007_StatePlane_Kentucky_North_FIPS_1601_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneKentuckySouthFIPS1602USFeet = ProjectionInfo.FromEpsgCode(3549).SetNames("NAD_1983_NSRS2007_StatePlane_Kentucky_South_FIPS_1602_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneLouisianaNorthFIPS1701USFt = ProjectionInfo.FromEpsgCode(3551).SetNames("NAD_1983_NSRS2007_StatePlane_Louisiana_North_FIPS_1701_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneLouisianaSouthFIPS1702USFt = ProjectionInfo.FromEpsgCode(3553).SetNames("NAD_1983_NSRS2007_StatePlane_Louisiana_South_FIPS_1702_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMaineEastFIPS1801USFeet = ProjectionInfo.FromEpsgCode(26863).SetNames("NAD_1983_NSRS2007_StatePlane_Maine_East_FIPS_1801_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMaineWestFIPS1802USFeet = ProjectionInfo.FromEpsgCode(26864).SetNames("NAD_1983_NSRS2007_StatePlane_Maine_West_FIPS_1802_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMarylandFIPS1900USFeet = ProjectionInfo.FromEpsgCode(3582).SetNames("NAD_1983_NSRS2007_StatePlane_Maryland_FIPS_1900_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMassachusettsFIPS2001USFeet = ProjectionInfo.FromEpsgCode(3586).SetNames("NAD_1983_NSRS2007_StatePlane_Massachusetts_Mnld_FIPS_2001_FtUS", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMassachusettsIslFIPS2002USFt = ProjectionInfo.FromEpsgCode(3584).SetNames("NAD_1983_NSRS2007_StatePlane_Massachusetts_Isl_FIPS_2002_FtUS", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMinnesotaCentralFIPS2202USFt = ProjectionInfo.FromEpsgCode(26866).SetNames("NAD_1983_NSRS2007_StatePlane_Minnesota_Central_FIPS_2202_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMinnesotaNorthFIPS2201USFt = ProjectionInfo.FromEpsgCode(26865).SetNames("NAD_1983_NSRS2007_StatePlane_Minnesota_North_FIPS_2201_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMinnesotaSouthFIPS2203USFt = ProjectionInfo.FromEpsgCode(26867).SetNames("NAD_1983_NSRS2007_StatePlane_Minnesota_South_FIPS_2203_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMississippiEastFIPS2301USFt = ProjectionInfo.FromEpsgCode(3598).SetNames("NAD_1983_NSRS2007_StatePlane_Mississippi_East_FIPS_2301_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneMississippiWestFIPS2302USFt = ProjectionInfo.FromEpsgCode(3600).SetNames("NAD_1983_NSRS2007_StatePlane_Mississippi_West_FIPS_2302_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNebraskaFIPS2600USFeet = ProjectionInfo.FromEpsgCode(26868).SetNames("NAD_1983_NSRS2007_StatePlane_Nebraska_FIPS_2600_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNevadaCentralFIPS2702USFeet = ProjectionInfo.FromEpsgCode(3608).SetNames("NAD_1983_NSRS2007_StatePlane_Nevada_Central_FIPS_2702_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNevadaEastFIPS2701USFeet = ProjectionInfo.FromEpsgCode(3610).SetNames("NAD_1983_NSRS2007_StatePlane_Nevada_East_FIPS_2701_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNevadaWestFIPS2703USFeet = ProjectionInfo.FromEpsgCode(3612).SetNames("NAD_1983_NSRS2007_StatePlane_Nevada_West_FIPS_2703_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewHampshireFIPS2800USFeet = ProjectionInfo.FromEpsgCode(3614).SetNames("NAD_1983_NSRS2007_StatePlane_New_Hampshire_FIPS_2800_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewJerseyFIPS2900USFeet = ProjectionInfo.FromEpsgCode(3616).SetNames("NAD_1983_NSRS2007_StatePlane_New_Jersey_FIPS_2900_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewMexicoCentralFIPS3002USFt = ProjectionInfo.FromEpsgCode(3618).SetNames("NAD_1983_NSRS2007_StatePlane_New_Mexico_Central_FIPS_3002_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewMexicoEastFIPS3001USFt = ProjectionInfo.FromEpsgCode(3620).SetNames("NAD_1983_NSRS2007_StatePlane_New_Mexico_East_FIPS_3001_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewMexicoWestFIPS3003USFt = ProjectionInfo.FromEpsgCode(3622).SetNames("NAD_1983_NSRS2007_StatePlane_New_Mexico_West_FIPS_3003_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkCentralFIPS3102USFt = ProjectionInfo.FromEpsgCode(3624).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_Central_FIPS_3102_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkEastFIPS3101USFeet = ProjectionInfo.FromEpsgCode(3626).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_East_FIPS_3101_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkLongIslFIPS3104USFt = ProjectionInfo.FromEpsgCode(3628).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_Long_Isl_FIPS_3104_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNewYorkWestFIPS3103USFeet = ProjectionInfo.FromEpsgCode(3630).SetNames("NAD_1983_NSRS2007_StatePlane_New_York_West_FIPS_3103_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneNorthCarolinaFIPS3200USFeet = ProjectionInfo.FromEpsgCode(3632).SetNames("NAD_1983_NSRS2007_StatePlane_North_Carolina_FIPS_3200_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOhioNorthFIPS3401USFeet = ProjectionInfo.FromEpsgCode(3728).SetNames("NAD_1983_NSRS2007_StatePlane_Ohio_North_FIPS_3401_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOhioSouthFIPS3402USFeet = ProjectionInfo.FromEpsgCode(3729).SetNames("NAD_1983_NSRS2007_StatePlane_Ohio_South_FIPS_3402_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOklahomaNorthFIPS3501USFeet = ProjectionInfo.FromEpsgCode(3640).SetNames("NAD_1983_NSRS2007_StatePlane_Oklahoma_North_FIPS_3501_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneOklahomaSouthFIPS3502USFeet = ProjectionInfo.FromEpsgCode(3642).SetNames("NAD_1983_NSRS2007_StatePlane_Oklahoma_South_FIPS_3502_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlanePennsylvaniaNorthFIPS3701USFt = ProjectionInfo.FromEpsgCode(3650).SetNames("NAD_1983_NSRS2007_StatePlane_Pennsylvania_North_FIPS_3701_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlanePennsylvaniaSouthFIPS3702USFt = ProjectionInfo.FromEpsgCode(3652).SetNames("NAD_1983_NSRS2007_StatePlane_Pennsylvania_South_FIPS_3702_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneRhodeIslandFIPS3800USFeet = ProjectionInfo.FromEpsgCode(3654).SetNames("NAD_1983_NSRS2007_StatePlane_Rhode_Island_FIPS_3800_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneSouthDakotaNFIPS4001USFeet = ProjectionInfo.FromEpsgCode(3658).SetNames("NAD_1983_NSRS2007_StatePlane_South_Dakota_North_FIPS_4001_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneSouthDakotaSFIPS4002USFeet = ProjectionInfo.FromEpsgCode(3660).SetNames("NAD_1983_NSRS2007_StatePlane_South_Dakota_South_FIPS_4002_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTennesseeFIPS4100USFeet = ProjectionInfo.FromEpsgCode(3662).SetNames("NAD_1983_NSRS2007_StatePlane_Tennessee_FIPS_4100_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasCentralFIPS4203USFeet = ProjectionInfo.FromEpsgCode(3664).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_Central_FIPS_4203_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasNCentralFIPS4202USFt = ProjectionInfo.FromEpsgCode(3670).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_North_Central_FIPS_4202_FtUS", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasNorthFIPS4201USFeet = ProjectionInfo.FromEpsgCode(3668).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_North_FIPS_4201_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasSCentralFIPS4204USFt = ProjectionInfo.FromEpsgCode(3674).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_South_Central_FIPS_4204_FtUS", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneTexasSouthFIPS4205USFeet = ProjectionInfo.FromEpsgCode(3672).SetNames("NAD_1983_NSRS2007_StatePlane_Texas_South_FIPS_4205_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahCentralFIPS4302USFeet = ProjectionInfo.FromEpsgCode(3677).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_Central_FIPS_4302_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahNorthFIPS4301USFeet = ProjectionInfo.FromEpsgCode(3680).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_North_FIPS_4301_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneUtahSouthFIPS4303USFeet = ProjectionInfo.FromEpsgCode(3683).SetNames("NAD_1983_NSRS2007_StatePlane_Utah_South_FIPS_4303_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneVirginiaNorthFIPS4501USFeet = ProjectionInfo.FromEpsgCode(3686).SetNames("NAD_1983_NSRS2007_StatePlane_Virginia_North_FIPS_4501_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneVirginiaSouthFIPS4502USFeet = ProjectionInfo.FromEpsgCode(3688).SetNames("NAD_1983_NSRS2007_StatePlane_Virginia_South_FIPS_4502_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWashingtonNorthFIPS4601USFt = ProjectionInfo.FromEpsgCode(3690).SetNames("NAD_1983_NSRS2007_StatePlane_Washington_North_FIPS_4601_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWashingtonSouthFIPS4602USFt = ProjectionInfo.FromEpsgCode(3692).SetNames("NAD_1983_NSRS2007_StatePlane_Washington_South_FIPS_4602_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWestVirginiaNFIPS4701USFt = ProjectionInfo.FromEpsgCode(26869).SetNames("NAD_1983_NSRS2007_StatePlane_West_Virginia_North_FIPS_4701_FtUS", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWestVirginiaSFIPS4702USFt = ProjectionInfo.FromEpsgCode(26870).SetNames("NAD_1983_NSRS2007_StatePlane_West_Virginia_South_FIPS_4702_FtUS", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWisconsinCentralFIPS4802USFt = ProjectionInfo.FromEpsgCode(3696).SetNames("NAD_1983_NSRS2007_StatePlane_Wisconsin_Central_FIPS_4802_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWisconsinNorthFIPS4801USFt = ProjectionInfo.FromEpsgCode(3698).SetNames("NAD_1983_NSRS2007_StatePlane_Wisconsin_North_FIPS_4801_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWisconsinSouthFIPS4803USFt = ProjectionInfo.FromEpsgCode(3700).SetNames("NAD_1983_NSRS2007_StatePlane_Wisconsin_South_FIPS_4803_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingEastFIPS4901USFeet = ProjectionInfo.FromEpsgCode(3730).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_East_FIPS_4901_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingECentralFIPS4902USFt = ProjectionInfo.FromEpsgCode(3731).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_E_Central_FIPS_4902_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingWCentralFIPS4903USFt = ProjectionInfo.FromEpsgCode(3732).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_W_Central_FIPS_4903_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007StatePlaneWyomingWestFIPS4904USFeet = ProjectionInfo.FromEpsgCode(3733).SetNames("NAD_1983_NSRS2007_StatePlane_Wyoming_West_FIPS_4904_Ft_US", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
        }

        #endregion
    }
}

#pragma warning restore 1591