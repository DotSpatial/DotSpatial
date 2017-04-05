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

namespace DotSpatial.Projections.ProjectedCategories.CountySystems.Minnesota
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for USFeet.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class USFeet : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983HARNAdjMNAitkinUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNAnokaUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeckerUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiNorthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiSouthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBentonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBigStoneUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBlueEarthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBrownUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarltonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarverUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassNorthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassSouthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNChippewaUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNChisagoUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNClayUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNClearwaterUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookNorthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookSouthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCottonwoodUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCrowWingUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNDakotaUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNDodgeUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNDouglasUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNFaribaultUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNFillmoreUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNFreebornUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNGoodhueUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNGrantUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNHennepinUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNHoustonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNHubbardUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNIsantiUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaNorthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaSouthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNJacksonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKanabecUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKandiyohiUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKittsonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKoochichingUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLacQuiParleUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsNorthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsSouthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLeSueurUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLincolnUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLyonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMahnomenUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMarshallUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMartinUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMcLeodUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMeekerUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMilleLacsUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMorrisonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMowerUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMurrayUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNicolletUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNoblesUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNormanUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNOlmstedUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNOttertailUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPenningtonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPineUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPipestoneUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPolkUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPopeUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRamseyUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedLakeUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedwoodUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRenvilleUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRiceUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRockUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRoseauUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNScottUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSherburneUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSibleyUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStearnsUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSteeleUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStevensUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisCentralUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisNorthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisSouthUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSwiftUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNToddUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNTraverseUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWabashaUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWadenaUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWasecaUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWashingtonUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWatonwanUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWilkinUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWinonaUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWrightUSFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNYellowMedicineUSFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of USFeet.
        /// </summary>
        public USFeet()
        {
            NAD1983HARNAdjMNAitkinUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103700).SetNames("NAD_1983_HARN_Adj_MN_Aitkin_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNAnokaUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103708).SetNames("NAD_1983_HARN_Adj_MN_Anoka_Feet", "GCS_NAD_1983_HARN_Adj_MN_Anoka", "D_NAD_1983_HARN_Adj_MN_Anoka"); // missing
            NAD1983HARNAdjMNBeckerUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103709).SetNames("NAD_1983_HARN_Adj_MN_Becker_Feet", "GCS_NAD_1983_HARN_Adj_MN_Becker", "D_NAD_1983_HARN_Adj_MN_Becker"); // missing
            NAD1983HARNAdjMNBeltramiNorthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103710).SetNames("NAD_1983_HARN_Adj_MN_Beltrami_North_Feet", "GCS_NAD_1983_HARN_Adj_MN_Beltrami_North", "D_NAD_1983_HARN_Adj_MN_Beltrami_North"); // missing
            NAD1983HARNAdjMNBeltramiSouthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103711).SetNames("NAD_1983_HARN_Adj_MN_Beltrami_South_Feet", "GCS_NAD_1983_HARN_Adj_MN_Beltrami_South", "D_NAD_1983_HARN_Adj_MN_Beltrami_South"); // missing
            NAD1983HARNAdjMNBentonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103712).SetNames("NAD_1983_HARN_Adj_MN_Benton_Feet", "GCS_NAD_1983_HARN_Adj_MN_Benton", "D_NAD_1983_HARN_Adj_MN_Benton"); // missing
            NAD1983HARNAdjMNBigStoneUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103713).SetNames("NAD_1983_HARN_Adj_MN_Big_Stone_Feet", "GCS_NAD_1983_HARN_Adj_MN_Big_Stone", "D_NAD_1983_HARN_Adj_MN_Big_Stone"); // missing
            NAD1983HARNAdjMNBlueEarthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103714).SetNames("NAD_1983_HARN_Adj_MN_Blue_Earth_Feet", "GCS_NAD_1983_HARN_Adj_MN_Blue_Earth", "D_NAD_1983_HARN_Adj_MN_Blue_Earth"); // missing
            NAD1983HARNAdjMNBrownUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103715).SetNames("NAD_1983_HARN_Adj_MN_Brown_Feet", "GCS_NAD_1983_HARN_Adj_MN_Brown", "D_NAD_1983_HARN_Adj_MN_Brown"); // missing
            NAD1983HARNAdjMNCarltonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103716).SetNames("NAD_1983_HARN_Adj_MN_Carlton_Feet", "GCS_NAD_1983_HARN_Adj_MN_Carlton", "D_NAD_1983_HARN_Adj_MN_Carlton"); // missing
            NAD1983HARNAdjMNCarverUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103717).SetNames("NAD_1983_HARN_Adj_MN_Carver_Feet", "GCS_NAD_1983_HARN_Adj_MN_Carver", "D_NAD_1983_HARN_Adj_MN_Carver"); // missing
            NAD1983HARNAdjMNCassNorthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103718).SetNames("NAD_1983_HARN_Adj_MN_Cass_North_Feet", "GCS_NAD_1983_HARN_Adj_MN_Cass_North", "D_NAD_1983_HARN_Adj_MN_Cass_North"); // missing
            NAD1983HARNAdjMNCassSouthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103719).SetNames("NAD_1983_HARN_Adj_MN_Cass_South_Feet", "GCS_NAD_1983_HARN_Adj_MN_Cass_South", "D_NAD_1983_HARN_Adj_MN_Cass_South"); // missing
            NAD1983HARNAdjMNChippewaUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103720).SetNames("NAD_1983_HARN_Adj_MN_Chippewa_Feet", "GCS_NAD_1983_HARN_Adj_MN_Chippewa", "D_NAD_1983_HARN_Adj_MN_Chippewa"); // missing
            NAD1983HARNAdjMNChisagoUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103721).SetNames("NAD_1983_HARN_Adj_MN_Chisago_Feet", "GCS_NAD_1983_HARN_Adj_MN_Chisago", "D_NAD_1983_HARN_Adj_MN_Chisago"); // missing
            NAD1983HARNAdjMNClayUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103701).SetNames("NAD_1983_HARN_Adj_MN_Clay_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNClearwaterUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103702).SetNames("NAD_1983_HARN_Adj_MN_Clearwater_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNCookNorthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103722).SetNames("NAD_1983_HARN_Adj_MN_Cook_North_Feet", "GCS_NAD_1983_HARN_Adj_MN_Cook_North", "D_NAD_1983_HARN_Adj_MN_Cook_North"); // missing
            NAD1983HARNAdjMNCookSouthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103723).SetNames("NAD_1983_HARN_Adj_MN_Cook_South_Feet", "GCS_NAD_1983_HARN_Adj_MN_Cook_South", "D_NAD_1983_HARN_Adj_MN_Cook_South"); // missing
            NAD1983HARNAdjMNCottonwoodUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103724).SetNames("NAD_1983_HARN_Adj_MN_Cottonwood_Feet", "GCS_NAD_1983_HARN_Adj_MN_Cottonwood", "D_NAD_1983_HARN_Adj_MN_Cottonwood"); // missing
            NAD1983HARNAdjMNCrowWingUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103725).SetNames("NAD_1983_HARN_Adj_MN_Crow_Wing_Feet", "GCS_NAD_1983_HARN_Adj_MN_Crow_Wing", "D_NAD_1983_HARN_Adj_MN_Crow_Wing"); // missing
            NAD1983HARNAdjMNDakotaUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103726).SetNames("NAD_1983_HARN_Adj_MN_Dakota_Feet", "GCS_NAD_1983_HARN_Adj_MN_Dakota", "D_NAD_1983_HARN_Adj_MN_Dakota"); // missing
            NAD1983HARNAdjMNDodgeUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103727).SetNames("NAD_1983_HARN_Adj_MN_Dodge_Feet", "GCS_NAD_1983_HARN_Adj_MN_Dodge", "D_NAD_1983_HARN_Adj_MN_Dodge"); // missing
            NAD1983HARNAdjMNDouglasUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103728).SetNames("NAD_1983_HARN_Adj_MN_Douglas_Feet", "GCS_NAD_1983_HARN_Adj_MN_Douglas", "D_NAD_1983_HARN_Adj_MN_Douglas"); // missing
            NAD1983HARNAdjMNFaribaultUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103729).SetNames("NAD_1983_HARN_Adj_MN_Faribault_Feet", "GCS_NAD_1983_HARN_Adj_MN_Faribault", "D_NAD_1983_HARN_Adj_MN_Faribault"); // missing
            NAD1983HARNAdjMNFillmoreUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103730).SetNames("NAD_1983_HARN_Adj_MN_Fillmore_Feet", "GCS_NAD_1983_HARN_Adj_MN_Fillmore", "D_NAD_1983_HARN_Adj_MN_Fillmore"); // missing
            NAD1983HARNAdjMNFreebornUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103731).SetNames("NAD_1983_HARN_Adj_MN_Freeborn_Feet", "GCS_NAD_1983_HARN_Adj_MN_Freeborn", "D_NAD_1983_HARN_Adj_MN_Freeborn"); // missing
            NAD1983HARNAdjMNGoodhueUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103732).SetNames("NAD_1983_HARN_Adj_MN_Goodhue_Feet", "GCS_NAD_1983_HARN_Adj_MN_Goodhue", "D_NAD_1983_HARN_Adj_MN_Goodhue"); // missing
            NAD1983HARNAdjMNGrantUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103733).SetNames("NAD_1983_HARN_Adj_MN_Grant_Feet", "GCS_NAD_1983_HARN_Adj_MN_Grant", "D_NAD_1983_HARN_Adj_MN_Grant"); // missing
            NAD1983HARNAdjMNHennepinUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103734).SetNames("NAD_1983_HARN_Adj_MN_Hennepin_Feet", "GCS_NAD_1983_HARN_Adj_MN_Hennepin", "D_NAD_1983_HARN_Adj_MN_Hennepin"); // missing
            NAD1983HARNAdjMNHoustonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103735).SetNames("NAD_1983_HARN_Adj_MN_Houston_Feet", "GCS_NAD_1983_HARN_Adj_MN_Houston", "D_NAD_1983_HARN_Adj_MN_Houston"); // missing
            NAD1983HARNAdjMNHubbardUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103703).SetNames("NAD_1983_HARN_Adj_MN_Hubbard_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNIsantiUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103736).SetNames("NAD_1983_HARN_Adj_MN_Isanti_Feet", "GCS_NAD_1983_HARN_Adj_MN_Isanti", "D_NAD_1983_HARN_Adj_MN_Isanti"); // missing
            NAD1983HARNAdjMNItascaNorthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103737).SetNames("NAD_1983_HARN_Adj_MN_Itasca_North_Feet", "GCS_NAD_1983_HARN_Adj_MN_Itasca_North", "D_NAD_1983_HARN_Adj_MN_Itasca_North"); // missing
            NAD1983HARNAdjMNItascaSouthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103738).SetNames("NAD_1983_HARN_Adj_MN_Itasca_South_Feet", "GCS_NAD_1983_HARN_Adj_MN_Itasca_South", "D_NAD_1983_HARN_Adj_MN_Itasca_South"); // missing
            NAD1983HARNAdjMNJacksonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103739).SetNames("NAD_1983_HARN_Adj_MN_Jackson_Feet", "GCS_NAD_1983_HARN_Adj_MN_Jackson", "D_NAD_1983_HARN_Adj_MN_Jackson"); // missing
            NAD1983HARNAdjMNKanabecUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103740).SetNames("NAD_1983_HARN_Adj_MN_Kanabec_Feet", "GCS_NAD_1983_HARN_Adj_MN_Kanabec", "D_NAD_1983_HARN_Adj_MN_Kanabec"); // missing
            NAD1983HARNAdjMNKandiyohiUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103741).SetNames("NAD_1983_HARN_Adj_MN_Kandiyohi_Feet", "GCS_NAD_1983_HARN_Adj_MN_Kandiyohi", "D_NAD_1983_HARN_Adj_MN_Kandiyohi"); // missing
            NAD1983HARNAdjMNKittsonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103742).SetNames("NAD_1983_HARN_Adj_MN_Kittson_Feet", "GCS_NAD_1983_HARN_Adj_MN_Kittson", "D_NAD_1983_HARN_Adj_MN_Kittson"); // missing
            NAD1983HARNAdjMNKoochichingUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103743).SetNames("NAD_1983_HARN_Adj_MN_Koochiching_Feet", "GCS_NAD_1983_HARN_Adj_MN_Koochiching", "D_NAD_1983_HARN_Adj_MN_Koochiching"); // missing
            NAD1983HARNAdjMNLacQuiParleUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103744).SetNames("NAD_1983_HARN_Adj_MN_Lac_Qui_Parle_Feet", "GCS_NAD_1983_HARN_Adj_MN_Lac_Qui_Parle", "D_NAD_1983_HARN_Adj_MN_Lac_Qui_Parle"); // missing
            NAD1983HARNAdjMNLakeoftheWoodsNorthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103745).SetNames("NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North_Feet", "GCS_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North", "D_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North"); // missing
            NAD1983HARNAdjMNLakeoftheWoodsSouthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103746).SetNames("NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South_Feet", "GCS_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South", "D_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South"); // missing
            NAD1983HARNAdjMNLakeUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103704).SetNames("NAD_1983_HARN_Adj_MN_Lake_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNLeSueurUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103747).SetNames("NAD_1983_HARN_Adj_MN_Le_Sueur_Feet", "GCS_NAD_1983_HARN_Adj_MN_Le_Sueur", "D_NAD_1983_HARN_Adj_MN_Le_Sueur"); // missing
            NAD1983HARNAdjMNLincolnUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103748).SetNames("NAD_1983_HARN_Adj_MN_Lincoln_Feet", "GCS_NAD_1983_HARN_Adj_MN_Lincoln", "D_NAD_1983_HARN_Adj_MN_Lincoln"); // missing
            NAD1983HARNAdjMNLyonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103749).SetNames("NAD_1983_HARN_Adj_MN_Lyon_Feet", "GCS_NAD_1983_HARN_Adj_MN_Lyon", "D_NAD_1983_HARN_Adj_MN_Lyon"); // missing
            NAD1983HARNAdjMNMahnomenUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103751).SetNames("NAD_1983_HARN_Adj_MN_Mahnomen_Feet", "GCS_NAD_1983_HARN_Adj_MN_Mahnomen", "D_NAD_1983_HARN_Adj_MN_Mahnomen"); // missing
            NAD1983HARNAdjMNMarshallUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103752).SetNames("NAD_1983_HARN_Adj_MN_Marshall_Feet", "GCS_NAD_1983_HARN_Adj_MN_Marshall", "D_NAD_1983_HARN_Adj_MN_Marshall"); // missing
            NAD1983HARNAdjMNMartinUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103753).SetNames("NAD_1983_HARN_Adj_MN_Martin_Feet", "GCS_NAD_1983_HARN_Adj_MN_Martin", "D_NAD_1983_HARN_Adj_MN_Martin"); // missing
            NAD1983HARNAdjMNMcLeodUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103750).SetNames("NAD_1983_HARN_Adj_MN_McLeod_Feet", "GCS_NAD_1983_HARN_Adj_MN_McLeod", "D_NAD_1983_HARN_Adj_MN_McLeod"); // missing
            NAD1983HARNAdjMNMeekerUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103754).SetNames("NAD_1983_HARN_Adj_MN_Meeker_Feet", "GCS_NAD_1983_HARN_Adj_MN_Meeker", "D_NAD_1983_HARN_Adj_MN_Meeker"); // missing
            NAD1983HARNAdjMNMilleLacsUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103705).SetNames("NAD_1983_HARN_Adj_MN_Mille_Lacs_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNMorrisonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103755).SetNames("NAD_1983_HARN_Adj_MN_Morrison_Feet", "GCS_NAD_1983_HARN_Adj_MN_Morrison", "D_NAD_1983_HARN_Adj_MN_Morrison"); // missing
            NAD1983HARNAdjMNMowerUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103756).SetNames("NAD_1983_HARN_Adj_MN_Mower_Feet", "GCS_NAD_1983_HARN_Adj_MN_Mower", "D_NAD_1983_HARN_Adj_MN_Mower"); // missing
            NAD1983HARNAdjMNMurrayUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103757).SetNames("NAD_1983_HARN_Adj_MN_Murray_Feet", "GCS_NAD_1983_HARN_Adj_MN_Murray", "D_NAD_1983_HARN_Adj_MN_Murray"); // missing
            NAD1983HARNAdjMNNicolletUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103758).SetNames("NAD_1983_HARN_Adj_MN_Nicollet_Feet", "GCS_NAD_1983_HARN_Adj_MN_Nicollet", "D_NAD_1983_HARN_Adj_MN_Nicollet"); // missing
            NAD1983HARNAdjMNNoblesUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103759).SetNames("NAD_1983_HARN_Adj_MN_Nobles_Feet", "GCS_NAD_1983_HARN_Adj_MN_Nobles", "D_NAD_1983_HARN_Adj_MN_Nobles"); // missing
            NAD1983HARNAdjMNNormanUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103760).SetNames("NAD_1983_HARN_Adj_MN_Norman_Feet", "GCS_NAD_1983_HARN_Adj_MN_Norman", "D_NAD_1983_HARN_Adj_MN_Norman"); // missing
            NAD1983HARNAdjMNOlmstedUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103761).SetNames("NAD_1983_HARN_Adj_MN_Olmsted_Feet", "GCS_NAD_1983_HARN_Adj_MN_Olmsted", "D_NAD_1983_HARN_Adj_MN_Olmsted"); // missing
            NAD1983HARNAdjMNOttertailUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103762).SetNames("NAD_1983_HARN_Adj_MN_Ottertail_Feet", "GCS_NAD_1983_HARN_Adj_MN_Ottertail", "D_NAD_1983_HARN_Adj_MN_Ottertail"); // missing
            NAD1983HARNAdjMNPenningtonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103763).SetNames("NAD_1983_HARN_Adj_MN_Pennington_Feet", "GCS_NAD_1983_HARN_Adj_MN_Pennington", "D_NAD_1983_HARN_Adj_MN_Pennington"); // missing
            NAD1983HARNAdjMNPineUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103764).SetNames("NAD_1983_HARN_Adj_MN_Pine_Feet", "GCS_NAD_1983_HARN_Adj_MN_Pine", "D_NAD_1983_HARN_Adj_MN_Pine"); // missing
            NAD1983HARNAdjMNPipestoneUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103765).SetNames("NAD_1983_HARN_Adj_MN_Pipestone_Feet", "GCS_NAD_1983_HARN_Adj_MN_Pipestone", "D_NAD_1983_HARN_Adj_MN_Pipestone"); // missing
            NAD1983HARNAdjMNPolkUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103766).SetNames("NAD_1983_HARN_Adj_MN_Polk_Feet", "GCS_NAD_1983_HARN_Adj_MN_Polk", "D_NAD_1983_HARN_Adj_MN_Polk"); // missing
            NAD1983HARNAdjMNPopeUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103767).SetNames("NAD_1983_HARN_Adj_MN_Pope_Feet", "GCS_NAD_1983_HARN_Adj_MN_Pope", "D_NAD_1983_HARN_Adj_MN_Pope"); // missing
            NAD1983HARNAdjMNRamseyUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103768).SetNames("NAD_1983_HARN_Adj_MN_Ramsey_Feet", "GCS_NAD_1983_HARN_Adj_MN_Ramsey", "D_NAD_1983_HARN_Adj_MN_Ramsey"); // missing
            NAD1983HARNAdjMNRedLakeUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103769).SetNames("NAD_1983_HARN_Adj_MN_Red_Lake_Feet", "GCS_NAD_1983_HARN_Adj_MN_Red_Lake", "D_NAD_1983_HARN_Adj_MN_Red_Lake"); // missing
            NAD1983HARNAdjMNRedwoodUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103770).SetNames("NAD_1983_HARN_Adj_MN_Redwood_Feet", "GCS_NAD_1983_HARN_Adj_MN_Redwood", "D_NAD_1983_HARN_Adj_MN_Redwood"); // missing
            NAD1983HARNAdjMNRenvilleUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103771).SetNames("NAD_1983_HARN_Adj_MN_Renville_Feet", "GCS_NAD_1983_HARN_Adj_MN_Renville", "D_NAD_1983_HARN_Adj_MN_Renville"); // missing
            NAD1983HARNAdjMNRiceUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103772).SetNames("NAD_1983_HARN_Adj_MN_Rice_Feet", "GCS_NAD_1983_HARN_Adj_MN_Rice", "D_NAD_1983_HARN_Adj_MN_Rice"); // missing
            NAD1983HARNAdjMNRockUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103773).SetNames("NAD_1983_HARN_Adj_MN_Rock_Feet", "GCS_NAD_1983_HARN_Adj_MN_Rock", "D_NAD_1983_HARN_Adj_MN_Rock"); // missing
            NAD1983HARNAdjMNRoseauUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103774).SetNames("NAD_1983_HARN_Adj_MN_Roseau_Feet", "GCS_NAD_1983_HARN_Adj_MN_Roseau", "D_NAD_1983_HARN_Adj_MN_Roseau"); // missing
            NAD1983HARNAdjMNScottUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103778).SetNames("NAD_1983_HARN_Adj_MN_Scott_Feet", "GCS_NAD_1983_HARN_Adj_MN_Scott", "D_NAD_1983_HARN_Adj_MN_Scott"); // missing
            NAD1983HARNAdjMNSherburneUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103779).SetNames("NAD_1983_HARN_Adj_MN_Sherburne_Feet", "GCS_NAD_1983_HARN_Adj_MN_Sherburne", "D_NAD_1983_HARN_Adj_MN_Sherburne"); // missing
            NAD1983HARNAdjMNSibleyUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103780).SetNames("NAD_1983_HARN_Adj_MN_Sibley_Feet", "GCS_NAD_1983_HARN_Adj_MN_Sibley", "D_NAD_1983_HARN_Adj_MN_Sibley"); // missing
            NAD1983HARNAdjMNStearnsUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103781).SetNames("NAD_1983_HARN_Adj_MN_Stearns_Feet", "GCS_NAD_1983_HARN_Adj_MN_Stearns", "D_NAD_1983_HARN_Adj_MN_Stearns"); // missing
            NAD1983HARNAdjMNSteeleUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103782).SetNames("NAD_1983_HARN_Adj_MN_Steele_Feet", "GCS_NAD_1983_HARN_Adj_MN_Steele", "D_NAD_1983_HARN_Adj_MN_Steele"); // missing
            NAD1983HARNAdjMNStevensUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103783).SetNames("NAD_1983_HARN_Adj_MN_Stevens_Feet", "GCS_NAD_1983_HARN_Adj_MN_Stevens", "D_NAD_1983_HARN_Adj_MN_Stevens"); // missing
            NAD1983HARNAdjMNStLouisCentralUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103776).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_Central_Feet", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_Central", "D_NAD_1983_HARN_Adj_MN_St_Louis_Central"); // missing
            NAD1983HARNAdjMNStLouisNorthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103775).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_North_Feet", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_North", "D_NAD_1983_HARN_Adj_MN_St_Louis_North"); // missing
            NAD1983HARNAdjMNStLouisSouthUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103777).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_South_Feet", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_South", "D_NAD_1983_HARN_Adj_MN_St_Louis_South"); // missing
            NAD1983HARNAdjMNStLouisUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103695).SetNames("NAD_1983_HARN_Adj_MN_St_Louis_CS96_Feet", "GCS_NAD_1983_HARN_Adj_MN_St_Louis", "D_NAD_1983_HARN_Adj_MN_St_Louis"); // missing
            NAD1983HARNAdjMNSwiftUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103784).SetNames("NAD_1983_HARN_Adj_MN_Swift_Feet", "GCS_NAD_1983_HARN_Adj_MN_Swift", "D_NAD_1983_HARN_Adj_MN_Swift"); // missing
            NAD1983HARNAdjMNToddUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103785).SetNames("NAD_1983_HARN_Adj_MN_Todd_Feet", "GCS_NAD_1983_HARN_Adj_MN_Todd", "D_NAD_1983_HARN_Adj_MN_Todd"); // missing
            NAD1983HARNAdjMNTraverseUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103786).SetNames("NAD_1983_HARN_Adj_MN_Traverse_Feet", "GCS_NAD_1983_HARN_Adj_MN_Traverse", "D_NAD_1983_HARN_Adj_MN_Traverse"); // missing
            NAD1983HARNAdjMNWabashaUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103787).SetNames("NAD_1983_HARN_Adj_MN_Wabasha_Feet", "GCS_NAD_1983_HARN_Adj_MN_Wabasha", "D_NAD_1983_HARN_Adj_MN_Wabasha"); // missing
            NAD1983HARNAdjMNWadenaUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103788).SetNames("NAD_1983_HARN_Adj_MN_Wadena_Feet", "GCS_NAD_1983_HARN_Adj_MN_Wadena", "D_NAD_1983_HARN_Adj_MN_Wadena"); // missing
            NAD1983HARNAdjMNWasecaUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103789).SetNames("NAD_1983_HARN_Adj_MN_Waseca_Feet", "GCS_NAD_1983_HARN_Adj_MN_Waseca", "D_NAD_1983_HARN_Adj_MN_Waseca"); // missing
            NAD1983HARNAdjMNWashingtonUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103706).SetNames("NAD_1983_HARN_Adj_MN_Washington_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNWatonwanUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103790).SetNames("NAD_1983_HARN_Adj_MN_Watonwan_Feet", "GCS_NAD_1983_HARN_Adj_MN_Watonwan", "D_NAD_1983_HARN_Adj_MN_Watonwan"); // missing
            NAD1983HARNAdjMNWilkinUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103707).SetNames("NAD_1983_HARN_Adj_MN_Wilkin_Feet", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983HARNAdjMNWinonaUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103791).SetNames("NAD_1983_HARN_Adj_MN_Winona_Feet", "GCS_NAD_1983_HARN_Adj_MN_Winona", "D_NAD_1983_HARN_Adj_MN_Winona"); // missing
            NAD1983HARNAdjMNWrightUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103792).SetNames("NAD_1983_HARN_Adj_MN_Wright_Feet", "GCS_NAD_1983_HARN_Adj_MN_Wright", "D_NAD_1983_HARN_Adj_MN_Wright"); // missing
            NAD1983HARNAdjMNYellowMedicineUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 103793).SetNames("NAD_1983_HARN_Adj_MN_Yellow_Medicine_Feet", "GCS_NAD_1983_HARN_Adj_MN_Yellow_Medicine", "D_NAD_1983_HARN_Adj_MN_Yellow_Medicine"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591