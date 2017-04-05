// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:07:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories.CountySystems
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Minnesota.
    /// </summary>
    public class Minnesota : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1983HARNAdjMNAnoka;
        public readonly ProjectionInfo NAD1983HARNAdjMNBecker;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNBenton;
        public readonly ProjectionInfo NAD1983HARNAdjMNBigStone;
        public readonly ProjectionInfo NAD1983HARNAdjMNBlueEarth;
        public readonly ProjectionInfo NAD1983HARNAdjMNBrown;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarlton;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarver;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNChippewa;
        public readonly ProjectionInfo NAD1983HARNAdjMNChisago;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNCottonwood;
        public readonly ProjectionInfo NAD1983HARNAdjMNCrowWing;
        public readonly ProjectionInfo NAD1983HARNAdjMNDakota;
        public readonly ProjectionInfo NAD1983HARNAdjMNDodge;
        public readonly ProjectionInfo NAD1983HARNAdjMNDouglas;
        public readonly ProjectionInfo NAD1983HARNAdjMNFaribault;
        public readonly ProjectionInfo NAD1983HARNAdjMNFillmore;
        public readonly ProjectionInfo NAD1983HARNAdjMNFreeborn;
        public readonly ProjectionInfo NAD1983HARNAdjMNGoodhue;
        public readonly ProjectionInfo NAD1983HARNAdjMNGrant;
        public readonly ProjectionInfo NAD1983HARNAdjMNHennepin;
        public readonly ProjectionInfo NAD1983HARNAdjMNHouston;
        public readonly ProjectionInfo NAD1983HARNAdjMNIsanti;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNJackson;
        public readonly ProjectionInfo NAD1983HARNAdjMNKanabec;
        public readonly ProjectionInfo NAD1983HARNAdjMNKandiyohi;
        public readonly ProjectionInfo NAD1983HARNAdjMNKittson;
        public readonly ProjectionInfo NAD1983HARNAdjMNKoochiching;
        public readonly ProjectionInfo NAD1983HARNAdjMNLacQuiParle;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNLeSueur;
        public readonly ProjectionInfo NAD1983HARNAdjMNLincoln;
        public readonly ProjectionInfo NAD1983HARNAdjMNLyon;
        public readonly ProjectionInfo NAD1983HARNAdjMNMahnomen;
        public readonly ProjectionInfo NAD1983HARNAdjMNMarshall;
        public readonly ProjectionInfo NAD1983HARNAdjMNMartin;
        public readonly ProjectionInfo NAD1983HARNAdjMNMcLeod;
        public readonly ProjectionInfo NAD1983HARNAdjMNMeeker;
        public readonly ProjectionInfo NAD1983HARNAdjMNMorrison;
        public readonly ProjectionInfo NAD1983HARNAdjMNMower;
        public readonly ProjectionInfo NAD1983HARNAdjMNMurray;
        public readonly ProjectionInfo NAD1983HARNAdjMNNicollet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNobles;
        public readonly ProjectionInfo NAD1983HARNAdjMNNorman;
        public readonly ProjectionInfo NAD1983HARNAdjMNOlmsted;
        public readonly ProjectionInfo NAD1983HARNAdjMNOttertail;
        public readonly ProjectionInfo NAD1983HARNAdjMNPennington;
        public readonly ProjectionInfo NAD1983HARNAdjMNPine;
        public readonly ProjectionInfo NAD1983HARNAdjMNPipestone;
        public readonly ProjectionInfo NAD1983HARNAdjMNPolk;
        public readonly ProjectionInfo NAD1983HARNAdjMNPope;
        public readonly ProjectionInfo NAD1983HARNAdjMNRamsey;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedLake;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedwood;
        public readonly ProjectionInfo NAD1983HARNAdjMNRenville;
        public readonly ProjectionInfo NAD1983HARNAdjMNRice;
        public readonly ProjectionInfo NAD1983HARNAdjMNRock;
        public readonly ProjectionInfo NAD1983HARNAdjMNRoseau;
        public readonly ProjectionInfo NAD1983HARNAdjMNScott;
        public readonly ProjectionInfo NAD1983HARNAdjMNSherburne;
        public readonly ProjectionInfo NAD1983HARNAdjMNSibley;
        public readonly ProjectionInfo NAD1983HARNAdjMNStearns;
        public readonly ProjectionInfo NAD1983HARNAdjMNSteele;
        public readonly ProjectionInfo NAD1983HARNAdjMNStevens;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouis;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisCentral;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNSwift;
        public readonly ProjectionInfo NAD1983HARNAdjMNTodd;
        public readonly ProjectionInfo NAD1983HARNAdjMNTraverse;
        public readonly ProjectionInfo NAD1983HARNAdjMNWabasha;
        public readonly ProjectionInfo NAD1983HARNAdjMNWadena;
        public readonly ProjectionInfo NAD1983HARNAdjMNWaseca;
        public readonly ProjectionInfo NAD1983HARNAdjMNWatonwan;
        public readonly ProjectionInfo NAD1983HARNAdjMNWinona;
        public readonly ProjectionInfo NAD1983HARNAdjMNWright;
        public readonly ProjectionInfo NAD1983HARNAdjMNYellowMedicine;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Minnesota.
        /// </summary>
        public Minnesota()
        {
            NAD1983HARNAdjMNAnoka = ProjectionInfo.FromAuthorityCode("ESRI", 104700).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Anoka", "D_NAD_1983_HARN_Adj_MN_Anoka"); // missing
            NAD1983HARNAdjMNBecker = ProjectionInfo.FromAuthorityCode("ESRI", 104701).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Becker", "D_NAD_1983_HARN_Adj_MN_Becker"); // missing
            NAD1983HARNAdjMNBeltramiNorth = ProjectionInfo.FromAuthorityCode("ESRI", 104702).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Beltrami_North", "D_NAD_1983_HARN_Adj_MN_Beltrami_North"); // missing
            NAD1983HARNAdjMNBeltramiSouth = ProjectionInfo.FromAuthorityCode("ESRI", 104703).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Beltrami_South", "D_NAD_1983_HARN_Adj_MN_Beltrami_South"); // missing
            NAD1983HARNAdjMNBenton = ProjectionInfo.FromAuthorityCode("ESRI", 104704).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Benton", "D_NAD_1983_HARN_Adj_MN_Benton"); // missing
            NAD1983HARNAdjMNBigStone = ProjectionInfo.FromAuthorityCode("ESRI", 104705).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Big_Stone", "D_NAD_1983_HARN_Adj_MN_Big_Stone"); // missing
            NAD1983HARNAdjMNBlueEarth = ProjectionInfo.FromAuthorityCode("ESRI", 104706).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Blue_Earth", "D_NAD_1983_HARN_Adj_MN_Blue_Earth"); // missing
            NAD1983HARNAdjMNBrown = ProjectionInfo.FromAuthorityCode("ESRI", 104707).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Brown", "D_NAD_1983_HARN_Adj_MN_Brown"); // missing
            NAD1983HARNAdjMNCarlton = ProjectionInfo.FromAuthorityCode("ESRI", 104708).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Carlton", "D_NAD_1983_HARN_Adj_MN_Carlton"); // missing
            NAD1983HARNAdjMNCarver = ProjectionInfo.FromAuthorityCode("ESRI", 104709).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Carver", "D_NAD_1983_HARN_Adj_MN_Carver"); // missing
            NAD1983HARNAdjMNCassNorth = ProjectionInfo.FromAuthorityCode("ESRI", 104710).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Cass_North", "D_NAD_1983_HARN_Adj_MN_Cass_North"); // missing
            NAD1983HARNAdjMNCassSouth = ProjectionInfo.FromAuthorityCode("ESRI", 104711).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Cass_South", "D_NAD_1983_HARN_Adj_MN_Cass_South"); // missing
            NAD1983HARNAdjMNChippewa = ProjectionInfo.FromAuthorityCode("ESRI", 104712).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Chippewa", "D_NAD_1983_HARN_Adj_MN_Chippewa"); // missing
            NAD1983HARNAdjMNChisago = ProjectionInfo.FromAuthorityCode("ESRI", 104713).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Chisago", "D_NAD_1983_HARN_Adj_MN_Chisago"); // missing
            NAD1983HARNAdjMNCookNorth = ProjectionInfo.FromAuthorityCode("ESRI", 104714).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Cook_North", "D_NAD_1983_HARN_Adj_MN_Cook_North"); // missing
            NAD1983HARNAdjMNCookSouth = ProjectionInfo.FromAuthorityCode("ESRI", 104715).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Cook_South", "D_NAD_1983_HARN_Adj_MN_Cook_South"); // missing
            NAD1983HARNAdjMNCottonwood = ProjectionInfo.FromAuthorityCode("ESRI", 104716).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Cottonwood", "D_NAD_1983_HARN_Adj_MN_Cottonwood"); // missing
            NAD1983HARNAdjMNCrowWing = ProjectionInfo.FromAuthorityCode("ESRI", 104717).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Crow_Wing", "D_NAD_1983_HARN_Adj_MN_Crow_Wing"); // missing
            NAD1983HARNAdjMNDakota = ProjectionInfo.FromAuthorityCode("ESRI", 104718).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Dakota", "D_NAD_1983_HARN_Adj_MN_Dakota"); // missing
            NAD1983HARNAdjMNDodge = ProjectionInfo.FromAuthorityCode("ESRI", 104719).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Dodge", "D_NAD_1983_HARN_Adj_MN_Dodge"); // missing
            NAD1983HARNAdjMNDouglas = ProjectionInfo.FromAuthorityCode("ESRI", 104720).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Douglas", "D_NAD_1983_HARN_Adj_MN_Douglas"); // missing
            NAD1983HARNAdjMNFaribault = ProjectionInfo.FromAuthorityCode("ESRI", 104721).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Faribault", "D_NAD_1983_HARN_Adj_MN_Faribault"); // missing
            NAD1983HARNAdjMNFillmore = ProjectionInfo.FromAuthorityCode("ESRI", 104722).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Fillmore", "D_NAD_1983_HARN_Adj_MN_Fillmore"); // missing
            NAD1983HARNAdjMNFreeborn = ProjectionInfo.FromAuthorityCode("ESRI", 104723).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Freeborn", "D_NAD_1983_HARN_Adj_MN_Freeborn"); // missing
            NAD1983HARNAdjMNGoodhue = ProjectionInfo.FromAuthorityCode("ESRI", 104724).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Goodhue", "D_NAD_1983_HARN_Adj_MN_Goodhue"); // missing
            NAD1983HARNAdjMNGrant = ProjectionInfo.FromAuthorityCode("ESRI", 104725).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Grant", "D_NAD_1983_HARN_Adj_MN_Grant"); // missing
            NAD1983HARNAdjMNHennepin = ProjectionInfo.FromAuthorityCode("ESRI", 104726).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Hennepin", "D_NAD_1983_HARN_Adj_MN_Hennepin"); // missing
            NAD1983HARNAdjMNHouston = ProjectionInfo.FromAuthorityCode("ESRI", 104727).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Houston", "D_NAD_1983_HARN_Adj_MN_Houston"); // missing
            NAD1983HARNAdjMNIsanti = ProjectionInfo.FromAuthorityCode("ESRI", 104728).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Isanti", "D_NAD_1983_HARN_Adj_MN_Isanti"); // missing
            NAD1983HARNAdjMNItascaNorth = ProjectionInfo.FromAuthorityCode("ESRI", 104729).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Itasca_North", "D_NAD_1983_HARN_Adj_MN_Itasca_North"); // missing
            NAD1983HARNAdjMNItascaSouth = ProjectionInfo.FromAuthorityCode("ESRI", 104730).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Itasca_South", "D_NAD_1983_HARN_Adj_MN_Itasca_South"); // missing
            NAD1983HARNAdjMNJackson = ProjectionInfo.FromAuthorityCode("ESRI", 104731).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Jackson", "D_NAD_1983_HARN_Adj_MN_Jackson"); // missing
            NAD1983HARNAdjMNKanabec = ProjectionInfo.FromAuthorityCode("ESRI", 104732).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Kanabec", "D_NAD_1983_HARN_Adj_MN_Kanabec"); // missing
            NAD1983HARNAdjMNKandiyohi = ProjectionInfo.FromAuthorityCode("ESRI", 104733).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Kandiyohi", "D_NAD_1983_HARN_Adj_MN_Kandiyohi"); // missing
            NAD1983HARNAdjMNKittson = ProjectionInfo.FromAuthorityCode("ESRI", 104734).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Kittson", "D_NAD_1983_HARN_Adj_MN_Kittson"); // missing
            NAD1983HARNAdjMNKoochiching = ProjectionInfo.FromAuthorityCode("ESRI", 104735).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Koochiching", "D_NAD_1983_HARN_Adj_MN_Koochiching"); // missing
            NAD1983HARNAdjMNLacQuiParle = ProjectionInfo.FromAuthorityCode("ESRI", 104736).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Lac_Qui_Parle", "D_NAD_1983_HARN_Adj_MN_Lac_Qui_Parle"); // missing
            NAD1983HARNAdjMNLakeoftheWoodsNorth = ProjectionInfo.FromAuthorityCode("ESRI", 104737).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North", "D_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_North"); // missing
            NAD1983HARNAdjMNLakeoftheWoodsSouth = ProjectionInfo.FromAuthorityCode("ESRI", 104738).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South", "D_NAD_1983_HARN_Adj_MN_Lake_of_the_Woods_South"); // missing
            NAD1983HARNAdjMNLeSueur = ProjectionInfo.FromAuthorityCode("ESRI", 104739).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Le_Sueur", "D_NAD_1983_HARN_Adj_MN_Le_Sueur"); // missing
            NAD1983HARNAdjMNLincoln = ProjectionInfo.FromAuthorityCode("ESRI", 104740).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Lincoln", "D_NAD_1983_HARN_Adj_MN_Lincoln"); // missing
            NAD1983HARNAdjMNLyon = ProjectionInfo.FromAuthorityCode("ESRI", 104741).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Lyon", "D_NAD_1983_HARN_Adj_MN_Lyon"); // missing
            NAD1983HARNAdjMNMahnomen = ProjectionInfo.FromAuthorityCode("ESRI", 104743).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Mahnomen", "D_NAD_1983_HARN_Adj_MN_Mahnomen"); // missing
            NAD1983HARNAdjMNMarshall = ProjectionInfo.FromAuthorityCode("ESRI", 104744).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Marshall", "D_NAD_1983_HARN_Adj_MN_Marshall"); // missing
            NAD1983HARNAdjMNMartin = ProjectionInfo.FromAuthorityCode("ESRI", 104745).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Martin", "D_NAD_1983_HARN_Adj_MN_Martin"); // missing
            NAD1983HARNAdjMNMcLeod = ProjectionInfo.FromAuthorityCode("ESRI", 104742).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_McLeod", "D_NAD_1983_HARN_Adj_MN_McLeod"); // missing
            NAD1983HARNAdjMNMeeker = ProjectionInfo.FromAuthorityCode("ESRI", 104746).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Meeker", "D_NAD_1983_HARN_Adj_MN_Meeker"); // missing
            NAD1983HARNAdjMNMorrison = ProjectionInfo.FromAuthorityCode("ESRI", 104747).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Morrison", "D_NAD_1983_HARN_Adj_MN_Morrison"); // missing
            NAD1983HARNAdjMNMower = ProjectionInfo.FromAuthorityCode("ESRI", 104748).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Mower", "D_NAD_1983_HARN_Adj_MN_Mower"); // missing
            NAD1983HARNAdjMNMurray = ProjectionInfo.FromAuthorityCode("ESRI", 104749).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Murray", "D_NAD_1983_HARN_Adj_MN_Murray"); // missing
            NAD1983HARNAdjMNNicollet = ProjectionInfo.FromAuthorityCode("ESRI", 104750).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Nicollet", "D_NAD_1983_HARN_Adj_MN_Nicollet"); // missing
            NAD1983HARNAdjMNNobles = ProjectionInfo.FromAuthorityCode("ESRI", 104751).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Nobles", "D_NAD_1983_HARN_Adj_MN_Nobles"); // missing
            NAD1983HARNAdjMNNorman = ProjectionInfo.FromAuthorityCode("ESRI", 104752).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Norman", "D_NAD_1983_HARN_Adj_MN_Norman"); // missing
            NAD1983HARNAdjMNOlmsted = ProjectionInfo.FromAuthorityCode("ESRI", 104753).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Olmsted", "D_NAD_1983_HARN_Adj_MN_Olmsted"); // missing
            NAD1983HARNAdjMNOttertail = ProjectionInfo.FromAuthorityCode("ESRI", 104754).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Ottertail", "D_NAD_1983_HARN_Adj_MN_Ottertail"); // missing
            NAD1983HARNAdjMNPennington = ProjectionInfo.FromAuthorityCode("ESRI", 104755).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Pennington", "D_NAD_1983_HARN_Adj_MN_Pennington"); // missing
            NAD1983HARNAdjMNPine = ProjectionInfo.FromAuthorityCode("ESRI", 104756).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Pine", "D_NAD_1983_HARN_Adj_MN_Pine"); // missing
            NAD1983HARNAdjMNPipestone = ProjectionInfo.FromAuthorityCode("ESRI", 104757).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Pipestone", "D_NAD_1983_HARN_Adj_MN_Pipestone"); // missing
            NAD1983HARNAdjMNPolk = ProjectionInfo.FromAuthorityCode("ESRI", 104758).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Polk", "D_NAD_1983_HARN_Adj_MN_Polk"); // missing
            NAD1983HARNAdjMNPope = ProjectionInfo.FromAuthorityCode("ESRI", 104759).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Pope", "D_NAD_1983_HARN_Adj_MN_Pope"); // missing
            NAD1983HARNAdjMNRamsey = ProjectionInfo.FromAuthorityCode("ESRI", 104760).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Ramsey", "D_NAD_1983_HARN_Adj_MN_Ramsey"); // missing
            NAD1983HARNAdjMNRedLake = ProjectionInfo.FromAuthorityCode("ESRI", 104761).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Red_Lake", "D_NAD_1983_HARN_Adj_MN_Red_Lake"); // missing
            NAD1983HARNAdjMNRedwood = ProjectionInfo.FromAuthorityCode("ESRI", 104762).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Redwood", "D_NAD_1983_HARN_Adj_MN_Redwood"); // missing
            NAD1983HARNAdjMNRenville = ProjectionInfo.FromAuthorityCode("ESRI", 104763).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Renville", "D_NAD_1983_HARN_Adj_MN_Renville"); // missing
            NAD1983HARNAdjMNRice = ProjectionInfo.FromAuthorityCode("ESRI", 104764).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Rice", "D_NAD_1983_HARN_Adj_MN_Rice"); // missing
            NAD1983HARNAdjMNRock = ProjectionInfo.FromAuthorityCode("ESRI", 104765).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Rock", "D_NAD_1983_HARN_Adj_MN_Rock"); // missing
            NAD1983HARNAdjMNRoseau = ProjectionInfo.FromAuthorityCode("ESRI", 104766).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Roseau", "D_NAD_1983_HARN_Adj_MN_Roseau"); // missing
            NAD1983HARNAdjMNScott = ProjectionInfo.FromAuthorityCode("ESRI", 104770).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Scott", "D_NAD_1983_HARN_Adj_MN_Scott"); // missing
            NAD1983HARNAdjMNSherburne = ProjectionInfo.FromAuthorityCode("ESRI", 104771).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Sherburne", "D_NAD_1983_HARN_Adj_MN_Sherburne"); // missing
            NAD1983HARNAdjMNSibley = ProjectionInfo.FromAuthorityCode("ESRI", 104772).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Sibley", "D_NAD_1983_HARN_Adj_MN_Sibley"); // missing
            NAD1983HARNAdjMNStearns = ProjectionInfo.FromAuthorityCode("ESRI", 104773).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Stearns", "D_NAD_1983_HARN_Adj_MN_Stearns"); // missing
            NAD1983HARNAdjMNSteele = ProjectionInfo.FromAuthorityCode("ESRI", 104774).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Steele", "D_NAD_1983_HARN_Adj_MN_Steele"); // missing
            NAD1983HARNAdjMNStevens = ProjectionInfo.FromAuthorityCode("ESRI", 104775).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Stevens", "D_NAD_1983_HARN_Adj_MN_Stevens"); // missing
            NAD1983HARNAdjMNStLouis = ProjectionInfo.FromAuthorityCode("ESRI", 104786).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_St_Louis", "D_NAD_1983_HARN_Adj_MN_St_Louis"); // missing
            NAD1983HARNAdjMNStLouisCentral = ProjectionInfo.FromAuthorityCode("ESRI", 104768).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_Central", "D_NAD_1983_HARN_Adj_MN_St_Louis_Central"); // missing
            NAD1983HARNAdjMNStLouisNorth = ProjectionInfo.FromAuthorityCode("ESRI", 104767).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_North", "D_NAD_1983_HARN_Adj_MN_St_Louis_North"); // missing
            NAD1983HARNAdjMNStLouisSouth = ProjectionInfo.FromAuthorityCode("ESRI", 104769).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_St_Louis_South", "D_NAD_1983_HARN_Adj_MN_St_Louis_South"); // missing
            NAD1983HARNAdjMNSwift = ProjectionInfo.FromAuthorityCode("ESRI", 104776).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Swift", "D_NAD_1983_HARN_Adj_MN_Swift"); // missing
            NAD1983HARNAdjMNTodd = ProjectionInfo.FromAuthorityCode("ESRI", 104777).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Todd", "D_NAD_1983_HARN_Adj_MN_Todd"); // missing
            NAD1983HARNAdjMNTraverse = ProjectionInfo.FromAuthorityCode("ESRI", 104778).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Traverse", "D_NAD_1983_HARN_Adj_MN_Traverse"); // missing
            NAD1983HARNAdjMNWabasha = ProjectionInfo.FromAuthorityCode("ESRI", 104779).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Wabasha", "D_NAD_1983_HARN_Adj_MN_Wabasha"); // missing
            NAD1983HARNAdjMNWadena = ProjectionInfo.FromAuthorityCode("ESRI", 104780).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Wadena", "D_NAD_1983_HARN_Adj_MN_Wadena"); // missing
            NAD1983HARNAdjMNWaseca = ProjectionInfo.FromAuthorityCode("ESRI", 104781).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Waseca", "D_NAD_1983_HARN_Adj_MN_Waseca"); // missing
            NAD1983HARNAdjMNWatonwan = ProjectionInfo.FromAuthorityCode("ESRI", 104782).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Watonwan", "D_NAD_1983_HARN_Adj_MN_Watonwan"); // missing
            NAD1983HARNAdjMNWinona = ProjectionInfo.FromAuthorityCode("ESRI", 104783).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Winona", "D_NAD_1983_HARN_Adj_MN_Winona"); // missing
            NAD1983HARNAdjMNWright = ProjectionInfo.FromAuthorityCode("ESRI", 104784).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Wright", "D_NAD_1983_HARN_Adj_MN_Wright"); // missing
            NAD1983HARNAdjMNYellowMedicine = ProjectionInfo.FromAuthorityCode("ESRI", 104785).SetNames("", "GCS_NAD_1983_HARN_Adj_MN_Yellow_Medicine", "D_NAD_1983_HARN_Adj_MN_Yellow_Medicine"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591