// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:32:25 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Jason Dick          |  12/5/2011 |  Corrected parameters for all counties.
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.CountySystems.Minnesota
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Meters.
    /// </summary>
    public class Meters : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983HARNAdjMNAitkinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNAnokaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeckerMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBentonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBigStoneMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBlueEarthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBrownMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarltonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarverMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNChippewaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNChisagoMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNClayMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNClearwaterMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCottonwoodMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCrowWingMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNDakotaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNDodgeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNDouglasMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNFaribaultMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNFillmoreMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNFreebornMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNGoodhueMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNGrantMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNHennepinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNHoustonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNHubbardMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNIsantiMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNJacksonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKanabecMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKandiyohiMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKittsonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKoochichingMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLacQuiParleMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLeSueurMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLincolnMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLyonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMahnomenMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMarshallMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMartinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMcLeodMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMeekerMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMilleLacsMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMorrisonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMowerMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMurrayMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNNicolletMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNNoblesMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNNormanMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNOlmstedMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNOttertailMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPenningtonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPineMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPipestoneMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPolkMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPopeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRamseyMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedLakeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedwoodMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRenvilleMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRiceMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRockMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRoseauMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNScottMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSherburneMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSibleyMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStearnsMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSteeleMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStevensMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisCentralMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSwiftMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNToddMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNTraverseMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWabashaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWadenaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWasecaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWashingtonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWatonwanMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWilkinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWinonaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWrightMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNYellowMedicineMeters;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Meters.
        /// </summary>
        public Meters()
        {
            NAD1983HARNAdjMNAitkinMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103600).SetNames("NAD_1983_HARN_Adj_MN_Aitkin_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNAnokaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103608).SetNames("NAD_1983_HARN_Adj_MN_Anoka_Meters", "GCS_NAD_1983_HARN_Adj_MN_Anoka", "D_NAD_1983_HARN_Adj_MN_Anoka"); // missing
            NAD1983HARNAdjMNBeckerMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103609).SetNames("NAD_1983_HARN_Adj_MN_Becker_Meters", "GCS_NAD_1983_HARN_Adj_MN_Becker", "D_NAD_1983_HARN_Adj_MN_Becker"); // missing
            NAD1983HARNAdjMNBeltramiNorthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103610).SetNames("NAD_1983_HARN_Adj_MN_Beltrami_North_Meters", "GCS_NAD_1983_HARN_Adj_MN_Beltrami_North", "D_NAD_1983_HARN_Adj_MN_Beltrami_North"); // missing
            NAD1983HARNAdjMNBeltramiSouthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103611).SetNames("NAD_1983_HARN_Adj_MN_Beltrami_South_Meters", "GCS_NAD_1983_HARN_Adj_MN_Beltrami_South", "D_NAD_1983_HARN_Adj_MN_Beltrami_South"); // missing
            NAD1983HARNAdjMNBentonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103612).SetNames("NAD_1983_HARN_Adj_MN_Benton_Meters", "GCS_NAD_1983_HARN_Adj_MN_Benton", "D_NAD_1983_HARN_Adj_MN_Benton"); // missing
            NAD1983HARNAdjMNBigStoneMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103613).SetNames("NAD_1983_HARN_Adj_MN_Big_Stone_Meters", "GCS_NAD_1983_HARN_Adj_MN_Big_Stone", "D_NAD_1983_HARN_Adj_MN_Big_Stone"); // missing
            NAD1983HARNAdjMNBlueEarthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103614).SetNames("NAD_1983_HARN_Adj_MN_Blue_Earth_Meters", "GCS_NAD_1983_HARN_Adj_MN_Blue_Earth", "D_NAD_1983_HARN_Adj_MN_Blue_Earth"); // missing
            NAD1983HARNAdjMNBrownMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103615).SetNames("NAD_1983_HARN_Adj_MN_Brown_Meters", "GCS_NAD_1983_HARN_Adj_MN_Brown", "D_NAD_1983_HARN_Adj_MN_Brown"); // missing
            NAD1983HARNAdjMNCarltonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103616).SetNames("NAD_1983_HARN_Adj_MN_Carlton_Meters", "GCS_NAD_1983_HARN_Adj_MN_Carlton", "D_NAD_1983_HARN_Adj_MN_Carlton"); // missing
            NAD1983HARNAdjMNCarverMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103617).SetNames("NAD_1983_HARN_Adj_MN_Carver_Meters", "GCS_NAD_1983_HARN_Adj_MN_Carver", "D_NAD_1983_HARN_Adj_MN_Carver"); // missing
            NAD1983HARNAdjMNCassNorthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103618).SetNames("NAD_1983_HARN_Adj_MN_Cass_North_Meters", "GCS_NAD_1983_HARN_Adj_MN_Cass_North", "D_NAD_1983_HARN_Adj_MN_Cass_North"); // missing
            NAD1983HARNAdjMNCassSouthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103619).SetNames("NAD_1983_HARN_Adj_MN_Cass_South_Meters", "GCS_NAD_1983_HARN_Adj_MN_Cass_South", "D_NAD_1983_HARN_Adj_MN_Cass_South"); // missing
            NAD1983HARNAdjMNChippewaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103620).SetNames("NAD_1983_HARN_Adj_MN_Chippewa_Meters", "GCS_NAD_1983_HARN_Adj_MN_Chippewa", "D_NAD_1983_HARN_Adj_MN_Chippewa"); // missing
            NAD1983HARNAdjMNChisagoMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103621).SetNames("NAD_1983_HARN_Adj_MN_Chisago_Meters", "GCS_NAD_1983_HARN_Adj_MN_Chisago", "D_NAD_1983_HARN_Adj_MN_Chisago"); // missing
            NAD1983HARNAdjMNClayMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103601).SetNames("NAD_1983_HARN_Adj_MN_Clay_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNClearwaterMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103602).SetNames("NAD_1983_HARN_Adj_MN_Clearwater_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNCookNorthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103622).SetNames("NAD_1983_HARN_Adj_MN_Cook_North_Meters", "GCS_NAD_1983_HARN_Adj_MN_Cook_North", "D_NAD_1983_HARN_Adj_MN_Cook_North"); // missing
            NAD1983HARNAdjMNCookSouthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103623).SetNames("NAD_1983_HARN_Adj_MN_Cook_South_Meters", "GCS_NAD_1983_HARN_Adj_MN_Cook_South", "D_NAD_1983_HARN_Adj_MN_Cook_South"); // missing
            NAD1983HARNAdjMNCottonwoodMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103624).SetNames("NAD_1983_HARN_Adj_MN_Cottonwood_Meters", "GCS_NAD_1983_HARN_Adj_MN_Cottonwood", "D_NAD_1983_HARN_Adj_MN_Cottonwood"); // missing
            NAD1983HARNAdjMNCrowWingMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103625).SetNames("NAD_1983_HARN_Adj_MN_Crow_Wing_Meters", "GCS_NAD_1983_HARN_Adj_MN_Crow_Wing", "D_NAD_1983_HARN_Adj_MN_Crow_Wing"); // missing
            NAD1983HARNAdjMNDakotaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103626).SetNames("NAD_1983_HARN_Adj_MN_Dakota_Meters", "GCS_NAD_1983_HARN_Adj_MN_Dakota", "D_NAD_1983_HARN_Adj_MN_Dakota"); // missing
            NAD1983HARNAdjMNDodgeMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103627).SetNames("NAD_1983_HARN_Adj_MN_Dodge_Meters", "GCS_NAD_1983_HARN_Adj_MN_Dodge", "D_NAD_1983_HARN_Adj_MN_Dodge"); // missing
            NAD1983HARNAdjMNDouglasMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103628).SetNames("NAD_1983_HARN_Adj_MN_Douglas_Meters", "GCS_NAD_1983_HARN_Adj_MN_Douglas", "D_NAD_1983_HARN_Adj_MN_Douglas"); // missing
            NAD1983HARNAdjMNFaribaultMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103629).SetNames("NAD_1983_HARN_Adj_MN_Faribault_Meters", "GCS_NAD_1983_HARN_Adj_MN_Faribault", "D_NAD_1983_HARN_Adj_MN_Faribault"); // missing
            NAD1983HARNAdjMNFillmoreMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103630).SetNames("NAD_1983_HARN_Adj_MN_Fillmore_Meters", "GCS_NAD_1983_HARN_Adj_MN_Fillmore", "D_NAD_1983_HARN_Adj_MN_Fillmore"); // missing
            NAD1983HARNAdjMNFreebornMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103631).SetNames("NAD_1983_HARN_Adj_MN_Freeborn_Meters", "GCS_NAD_1983_HARN_Adj_MN_Freeborn", "D_NAD_1983_HARN_Adj_MN_Freeborn"); // missing
            NAD1983HARNAdjMNGoodhueMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103632).SetNames("NAD_1983_HARN_Adj_MN_Goodhue_Meters", "GCS_NAD_1983_HARN_Adj_MN_Goodhue", "D_NAD_1983_HARN_Adj_MN_Goodhue"); // missing
            NAD1983HARNAdjMNGrantMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103633).SetNames("NAD_1983_HARN_Adj_MN_Grant_Meters", "GCS_NAD_1983_HARN_Adj_MN_Grant", "D_NAD_1983_HARN_Adj_MN_Grant"); // missing
            NAD1983HARNAdjMNHennepinMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103634).SetNames("NAD_1983_HARN_Adj_MN_Hennepin_Meters", "GCS_NAD_1983_HARN_Adj_MN_Hennepin", "D_NAD_1983_HARN_Adj_MN_Hennepin"); // missing
            NAD1983HARNAdjMNHoustonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103635).SetNames("NAD_1983_HARN_Adj_MN_Houston_Meters", "GCS_NAD_1983_HARN_Adj_MN_Houston", "D_NAD_1983_HARN_Adj_MN_Houston"); // missing
            NAD1983HARNAdjMNHubbardMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103603).SetNames("NAD_1983_HARN_Adj_MN_Hubbard_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNIsantiMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103636).SetNames("NAD_1983_HARN_Adj_MN_Isanti_Meters", "GCS_NAD_1983_HARN_Adj_MN_Isanti", "D_NAD_1983_HARN_Adj_MN_Isanti"); // missing
            NAD1983HARNAdjMNItascaNorthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103637).SetNames("NAD_1983_HARN_Adj_MN_Itasca_North_Meters", "GCS_NAD_1983_HARN_Adj_MN_Itasca_North", "D_NAD_1983_HARN_Adj_MN_Itasca_North"); // missing
            NAD1983HARNAdjMNItascaSouthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103638).SetNames("NAD_1983_HARN_Adj_MN_Itasca_South_Meters", "GCS_NAD_1983_HARN_Adj_MN_Itasca_South", "D_NAD_1983_HARN_Adj_MN_Itasca_South"); // missing
            NAD1983HARNAdjMNJacksonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103639).SetNames("NAD_1983_HARN_Adj_MN_Jackson_Meters", "GCS_NAD_1983_HARN_Adj_MN_Jackson", "D_NAD_1983_HARN_Adj_MN_Jackson"); // missing
            NAD1983HARNAdjMNKanabecMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103640).SetNames("NAD_1983_HARN_Adj_MN_Kanabec_Meters", "GCS_NAD_1983_HARN_Adj_MN_Kanabec", "D_NAD_1983_HARN_Adj_MN_Kanabec"); // missing
            NAD1983HARNAdjMNKandiyohiMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103641).SetNames("NAD_1983_HARN_Adj_MN_Kandiyohi_Meters", "GCS_NAD_1983_HARN_Adj_MN_Kandiyohi", "D_NAD_1983_HARN_Adj_MN_Kandiyohi"); // missing
            NAD1983HARNAdjMNKittsonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103642).SetNames("NAD_1983_HARN_Adj_MN_Kittson_Meters", "GCS_NAD_1983_HARN_Adj_MN_Kittson", "D_NAD_1983_HARN_Adj_MN_Kittson"); // missing
            NAD1983HARNAdjMNKoochichingMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103643).SetNames("NAD_1983_HARN_Adj_MN_Koochiching_Meters", "GCS_NAD_1983_HARN_Adj_MN_Koochiching", "D_NAD_1983_HARN_Adj_MN_Koochiching"); // missing
            NAD1983HARNAdjMNLacQuiParleMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103644).SetNames("NAD_1983_HARN_Adj_MN_Lac_Qui_Parle_Meters", "GCS_NAD_1983_HARN_Adj_MN_Lac_Qui_Parle", "D_NAD_1983_HARN_Adj_MN_Lac_Qui_Parle"); // missing
            NAD1983HARNAdjMNLakeMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103604).SetNames("NAD_1983_HARN_Adj_MN_Lake_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNLakeoftheWoodsNorthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103645).SetNames("NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North_Meters", "GCS_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North", "D_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North"); // missing
            NAD1983HARNAdjMNLakeoftheWoodsSouthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103646).SetNames("NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South_Meters", "GCS_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South", "D_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South"); // missing
            NAD1983HARNAdjMNLeSueurMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103647).SetNames("NAD_1983_HARN_Adj_MN_Le_Sueur_Meters", "GCS_NAD_1983_HARN_Adj_MN_Le_Sueur", "D_NAD_1983_HARN_Adj_MN_Le_Sueur"); // missing
            NAD1983HARNAdjMNLincolnMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103648).SetNames("NAD_1983_HARN_Adj_MN_Lincoln_Meters", "GCS_NAD_1983_HARN_Adj_MN_Lincoln", "D_NAD_1983_HARN_Adj_MN_Lincoln"); // missing
            NAD1983HARNAdjMNLyonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103649).SetNames("NAD_1983_HARN_Adj_MN_Lyon_Meters", "GCS_NAD_1983_HARN_Adj_MN_Lyon", "D_NAD_1983_HARN_Adj_MN_Lyon"); // missing
            NAD1983HARNAdjMNMahnomenMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103651).SetNames("NAD_1983_HARN_Adj_MN_Mahnomen_Meters", "GCS_NAD_1983_HARN_Adj_MN_Mahnomen", "D_NAD_1983_HARN_Adj_MN_Mahnomen"); // missing
            NAD1983HARNAdjMNMarshallMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103652).SetNames("NAD_1983_HARN_Adj_MN_Marshall_Meters", "GCS_NAD_1983_HARN_Adj_MN_Marshall", "D_NAD_1983_HARN_Adj_MN_Marshall"); // missing
            NAD1983HARNAdjMNMartinMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103653).SetNames("NAD_1983_HARN_Adj_MN_Martin_Meters", "GCS_NAD_1983_HARN_Adj_MN_Martin", "D_NAD_1983_HARN_Adj_MN_Martin"); // missing
            NAD1983HARNAdjMNMcLeodMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103650).SetNames("NAD_1983_HARN_Adj_MN_McLeod_Meters", "GCS_NAD_1983_HARN_Adj_MN_McLeod", "D_NAD_1983_HARN_Adj_MN_McLeod"); // missing
            NAD1983HARNAdjMNMeekerMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103654).SetNames("NAD_1983_HARN_Adj_MN_Meeker_Meters", "GCS_NAD_1983_HARN_Adj_MN_Meeker", "D_NAD_1983_HARN_Adj_MN_Meeker"); // missing
            NAD1983HARNAdjMNMilleLacsMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103605).SetNames("NAD_1983_HARN_Adj_MN_Mille_Lacs_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNMorrisonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103655).SetNames("NAD_1983_HARN_Adj_MN_Morrison_Meters", "GCS_NAD_1983_HARN_Adj_MN_Morrison", "D_NAD_1983_HARN_Adj_MN_Morrison"); // missing
            NAD1983HARNAdjMNMowerMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103656).SetNames("NAD_1983_HARN_Adj_MN_Mower_Meters", "GCS_NAD_1983_HARN_Adj_MN_Mower", "D_NAD_1983_HARN_Adj_MN_Mower"); // missing
            NAD1983HARNAdjMNMurrayMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103657).SetNames("NAD_1983_HARN_Adj_MN_Murray_Meters", "GCS_NAD_1983_HARN_Adj_MN_Murray", "D_NAD_1983_HARN_Adj_MN_Murray"); // missing
            NAD1983HARNAdjMNNicolletMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103658).SetNames("NAD_1983_HARN_Adj_MN_Nicollet_Meters", "GCS_NAD_1983_HARN_Adj_MN_Nicollet", "D_NAD_1983_HARN_Adj_MN_Nicollet"); // missing
            NAD1983HARNAdjMNNoblesMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103659).SetNames("NAD_1983_HARN_Adj_MN_Nobles_Meters", "GCS_NAD_1983_HARN_Adj_MN_Nobles", "D_NAD_1983_HARN_Adj_MN_Nobles"); // missing
            NAD1983HARNAdjMNNormanMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103660).SetNames("NAD_1983_HARN_Adj_MN_Norman_Meters", "GCS_NAD_1983_HARN_Adj_MN_Norman", "D_NAD_1983_HARN_Adj_MN_Norman"); // missing
            NAD1983HARNAdjMNOlmstedMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103661).SetNames("NAD_1983_HARN_Adj_MN_Olmsted_Meters", "GCS_NAD_1983_HARN_Adj_MN_Olmsted", "D_NAD_1983_HARN_Adj_MN_Olmsted"); // missing
            NAD1983HARNAdjMNOttertailMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103662).SetNames("NAD_1983_HARN_Adj_MN_Ottertail_Meters", "GCS_NAD_1983_HARN_Adj_MN_Ottertail", "D_NAD_1983_HARN_Adj_MN_Ottertail"); // missing
            NAD1983HARNAdjMNPenningtonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103663).SetNames("NAD_1983_HARN_Adj_MN_Pennington_Meters", "GCS_NAD_1983_HARN_Adj_MN_Pennington", "D_NAD_1983_HARN_Adj_MN_Pennington"); // missing
            NAD1983HARNAdjMNPineMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103664).SetNames("NAD_1983_HARN_Adj_MN_Pine_Meters", "GCS_NAD_1983_HARN_Adj_MN_Pine", "D_NAD_1983_HARN_Adj_MN_Pine"); // missing
            NAD1983HARNAdjMNPipestoneMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103665).SetNames("NAD_1983_HARN_Adj_MN_Pipestone_Meters", "GCS_NAD_1983_HARN_Adj_MN_Pipestone", "D_NAD_1983_HARN_Adj_MN_Pipestone"); // missing
            NAD1983HARNAdjMNPolkMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103666).SetNames("NAD_1983_HARN_Adj_MN_Polk_Meters", "GCS_NAD_1983_HARN_Adj_MN_Polk", "D_NAD_1983_HARN_Adj_MN_Polk"); // missing
            NAD1983HARNAdjMNPopeMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103667).SetNames("NAD_1983_HARN_Adj_MN_Pope_Meters", "GCS_NAD_1983_HARN_Adj_MN_Pope", "D_NAD_1983_HARN_Adj_MN_Pope"); // missing
            NAD1983HARNAdjMNRamseyMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103668).SetNames("NAD_1983_HARN_Adj_MN_Ramsey_Meters", "GCS_NAD_1983_HARN_Adj_MN_Ramsey", "D_NAD_1983_HARN_Adj_MN_Ramsey"); // missing
            NAD1983HARNAdjMNRedLakeMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103669).SetNames("NAD_1983_HARN_Adj_MN_Red_Lake_Meters", "GCS_NAD_1983_HARN_Adj_MN_Red_Lake", "D_NAD_1983_HARN_Adj_MN_Red_Lake"); // missing
            NAD1983HARNAdjMNRedwoodMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103670).SetNames("NAD_1983_HARN_Adj_MN_Redwood_Meters", "GCS_NAD_1983_HARN_Adj_MN_Redwood", "D_NAD_1983_HARN_Adj_MN_Redwood"); // missing
            NAD1983HARNAdjMNRenvilleMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103671).SetNames("NAD_1983_HARN_Adj_MN_Renville_Meters", "GCS_NAD_1983_HARN_Adj_MN_Renville", "D_NAD_1983_HARN_Adj_MN_Renville"); // missing
            NAD1983HARNAdjMNRiceMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103672).SetNames("NAD_1983_HARN_Adj_MN_Rice_Meters", "GCS_NAD_1983_HARN_Adj_MN_Rice", "D_NAD_1983_HARN_Adj_MN_Rice"); // missing
            NAD1983HARNAdjMNRockMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103673).SetNames("NAD_1983_HARN_Adj_MN_Rock_Meters", "GCS_NAD_1983_HARN_Adj_MN_Rock", "D_NAD_1983_HARN_Adj_MN_Rock"); // missing
            NAD1983HARNAdjMNRoseauMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103674).SetNames("NAD_1983_HARN_Adj_MN_Roseau_Meters", "GCS_NAD_1983_HARN_Adj_MN_Roseau", "D_NAD_1983_HARN_Adj_MN_Roseau"); // missing
            NAD1983HARNAdjMNScottMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103678).SetNames("NAD_1983_HARN_Adj_MN_Scott_Meters", "GCS_NAD_1983_HARN_Adj_MN_Scott", "D_NAD_1983_HARN_Adj_MN_Scott"); // missing
            NAD1983HARNAdjMNSherburneMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103679).SetNames("NAD_1983_HARN_Adj_MN_Sherburne_Meters", "GCS_NAD_1983_HARN_Adj_MN_Sherburne", "D_NAD_1983_HARN_Adj_MN_Sherburne"); // missing
            NAD1983HARNAdjMNSibleyMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103680).SetNames("NAD_1983_HARN_Adj_MN_Sibley_Meters", "GCS_NAD_1983_HARN_Adj_MN_Sibley", "D_NAD_1983_HARN_Adj_MN_Sibley"); // missing
            NAD1983HARNAdjMNStearnsMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103681).SetNames("NAD_1983_HARN_Adj_MN_Stearns_Meters", "GCS_NAD_1983_HARN_Adj_MN_Stearns", "D_NAD_1983_HARN_Adj_MN_Stearns"); // missing
            NAD1983HARNAdjMNSteeleMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103682).SetNames("NAD_1983_HARN_Adj_MN_Steele_Meters", "GCS_NAD_1983_HARN_Adj_MN_Steele", "D_NAD_1983_HARN_Adj_MN_Steele"); // missing
            NAD1983HARNAdjMNStevensMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103683).SetNames("NAD_1983_HARN_Adj_MN_Stevens_Meters", "GCS_NAD_1983_HARN_Adj_MN_Stevens", "D_NAD_1983_HARN_Adj_MN_Stevens"); // missing
            NAD1983HARNAdjMNStLouisCentralMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103676).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_Central_Meters", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_Central", "D_NAD_1983_HARN_Adj_MN_St_Louis_Central"); // missing
            NAD1983HARNAdjMNStLouisMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103694).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_CS96_Meters", "GCS_NAD_1983_HARN_Adj_MN_St_Louis", "D_NAD_1983_HARN_Adj_MN_St_Louis"); // missing
            NAD1983HARNAdjMNStLouisNorthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103675).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_North_Meters", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_North", "D_NAD_1983_HARN_Adj_MN_St_Louis_North"); // missing
            NAD1983HARNAdjMNStLouisSouthMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103677).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_South_Meters", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_South", "D_NAD_1983_HARN_Adj_MN_St_Louis_South"); // missing
            NAD1983HARNAdjMNSwiftMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103684).SetNames("NAD_1983_HARN_Adj_MN_Swift_Meters", "GCS_NAD_1983_HARN_Adj_MN_Swift", "D_NAD_1983_HARN_Adj_MN_Swift"); // missing
            NAD1983HARNAdjMNToddMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103685).SetNames("NAD_1983_HARN_Adj_MN_Todd_Meters", "GCS_NAD_1983_HARN_Adj_MN_Todd", "D_NAD_1983_HARN_Adj_MN_Todd"); // missing
            NAD1983HARNAdjMNTraverseMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103686).SetNames("NAD_1983_HARN_Adj_MN_Traverse_Meters", "GCS_NAD_1983_HARN_Adj_MN_Traverse", "D_NAD_1983_HARN_Adj_MN_Traverse"); // missing
            NAD1983HARNAdjMNWabashaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103687).SetNames("NAD_1983_HARN_Adj_MN_Wabasha_Meters", "GCS_NAD_1983_HARN_Adj_MN_Wabasha", "D_NAD_1983_HARN_Adj_MN_Wabasha"); // missing
            NAD1983HARNAdjMNWadenaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103688).SetNames("NAD_1983_HARN_Adj_MN_Wadena_Meters", "GCS_NAD_1983_HARN_Adj_MN_Wadena", "D_NAD_1983_HARN_Adj_MN_Wadena"); // missing
            NAD1983HARNAdjMNWasecaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103689).SetNames("NAD_1983_HARN_Adj_MN_Waseca_Meters", "GCS_NAD_1983_HARN_Adj_MN_Waseca", "D_NAD_1983_HARN_Adj_MN_Waseca"); // missing
            NAD1983HARNAdjMNWashingtonMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103606).SetNames("NAD_1983_HARN_Adj_MN_Washington_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNWatonwanMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103690).SetNames("NAD_1983_HARN_Adj_MN_Watonwan_Meters", "GCS_NAD_1983_HARN_Adj_MN_Watonwan", "D_NAD_1983_HARN_Adj_MN_Watonwan"); // missing
            NAD1983HARNAdjMNWilkinMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103607).SetNames("NAD_1983_HARN_Adj_MN_Wilkin_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNWinonaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103691).SetNames("NAD_1983_HARN_Adj_MN_Winona_Meters", "GCS_NAD_1983_HARN_Adj_MN_Winona", "D_NAD_1983_HARN_Adj_MN_Winona"); // missing
            NAD1983HARNAdjMNWrightMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103692).SetNames("NAD_1983_HARN_Adj_MN_Wright_Meters", "GCS_NAD_1983_HARN_Adj_MN_Wright", "D_NAD_1983_HARN_Adj_MN_Wright"); // missing
            NAD1983HARNAdjMNYellowMedicineMeters = ProjectionInfo.FromAuthorityCode("ESRI", 103693).SetNames("NAD_1983_HARN_Adj_MN_Yellow_Medicine_Meters", "GCS_NAD_1983_HARN_Adj_MN_Yellow_Medicine", "D_NAD_1983_HARN_Adj_MN_Yellow_Medicine"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591