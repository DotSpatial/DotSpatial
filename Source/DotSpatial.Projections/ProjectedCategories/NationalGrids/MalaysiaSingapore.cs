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

namespace DotSpatial.Projections.ProjectedCategories.NationalGrids
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for MalaysiaSingapore.
    /// </summary>
    public class MalaysiaSingapore : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        // public readonly ProjectionInfo GDBD2009GEORSO;
        public readonly ProjectionInfo GDM2000BRSO;
        public readonly ProjectionInfo GDM2000Johor;
        public readonly ProjectionInfo GDM2000KedahPerlis;
        public readonly ProjectionInfo GDM2000Kelantan;
        public readonly ProjectionInfo GDM2000MRSO;
        public readonly ProjectionInfo GDM2000NegeriSembilanMelaka;
        public readonly ProjectionInfo GDM2000Pahang;
        public readonly ProjectionInfo GDM2000Perak;
        public readonly ProjectionInfo GDM2000PulauPinangSeberangPerai;
        public readonly ProjectionInfo GDM2000Selangor;
        public readonly ProjectionInfo GDM2000Terengganu;
        public readonly ProjectionInfo KertauRSOMalayaBenoitChains1895B;
        //public readonly ProjectionInfo KertauRSOMalayaMeters;
        public readonly ProjectionInfo KertauRSORSOMalayaChains1922Truncated;
        public readonly ProjectionInfo KertauRSORSOMalayaMeters;
        public readonly ProjectionInfo KertauSingaporeGrid;
        public readonly ProjectionInfo SVY21SingaporeTM;
        public readonly ProjectionInfo Timbalai1948RSOBorneoMeters;
        public readonly ProjectionInfo Timbalai1948RSOBorneoSearsChains;
        public readonly ProjectionInfo Timbalai1948RSOBorneoSearsFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of MalaysiaSingapore.
        /// </summary>
        public MalaysiaSingapore()
        {
            // GDBD2009GEORSO = ProjectionInfo.FromAuthorityCode("EPSG", 102490).SetNames("GDBD2009_GEORSO", "GCS_GDBD2009", "D_GDBD2009"); // projection not found
            GDM2000BRSO = ProjectionInfo.FromEpsgCode(3376).SetNames("GDM_2000_BRSO_East_Malaysia", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000Johor = ProjectionInfo.FromEpsgCode(3377).SetNames("GDM_2000_State_Cassini_Johor", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000KedahPerlis = ProjectionInfo.FromEpsgCode(3383).SetNames("GDM_2000_State_Cassini_Perlis", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000Kelantan = ProjectionInfo.FromEpsgCode(3385).SetNames("GDM_2000_State_Cassini_Kelantan", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000MRSO = ProjectionInfo.FromEpsgCode(3375).SetNames("GDM_2000_MRSO_Peninsular_Malaysia", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000NegeriSembilanMelaka = ProjectionInfo.FromEpsgCode(3378).SetNames("GDM_2000_State_Cassini_Negeri_Sembilan_and_Melaka", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000Pahang = ProjectionInfo.FromEpsgCode(3379).SetNames("GDM_2000_State_Cassini_Pahang", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000Perak = ProjectionInfo.FromEpsgCode(3384).SetNames("GDM_2000_State_Cassini_Perak", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000PulauPinangSeberangPerai = ProjectionInfo.FromEpsgCode(3382).SetNames("GDM_2000_State_Cassini_Pulau_Pinang_and_Seberang_Perai", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000Selangor = ProjectionInfo.FromEpsgCode(3380).SetNames("GDM_2000_State_Cassini_Selangor", "GCS_GDM_2000", "D_GDM_2000");
            GDM2000Terengganu = ProjectionInfo.FromEpsgCode(3381).SetNames("GDM_2000_State_Cassini_Terengganu", "GCS_GDM_2000", "D_GDM_2000");
            KertauRSOMalayaBenoitChains1895B = ProjectionInfo.FromEpsgCode(24571).SetNames("Kertau_RSO_Malaya_Chains", "GCS_Kertau", "D_Kertau");
            // KertauRSOMalayaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102062).SetNames("Kertau_RSO_Malaya_Meters", "GCS_Kertau", "D_Kertau"); // projection not found
            KertauRSORSOMalayaChains1922Truncated = ProjectionInfo.FromEpsgCode(3167).SetNames("Kertau_RSO_RSO_Malaya_ChSears1922trunc", "GCS_Kertau_RSO", "D_Kertau_RSO");
            KertauRSORSOMalayaMeters = ProjectionInfo.FromEpsgCode(3168).SetNames("Kertau_RSO_RSO_Malaya", "GCS_Kertau_RSO", "D_Kertau_RSO");
            KertauSingaporeGrid = ProjectionInfo.FromEpsgCode(24500).SetNames("Kertau_Singapore_Grid", "GCS_Kertau", "D_Kertau");
            SVY21SingaporeTM = ProjectionInfo.FromEpsgCode(3414).SetNames("SVY21_Singapore_TM", "GCS_SVY21", "D_SVY21");
            Timbalai1948RSOBorneoMeters = ProjectionInfo.FromEpsgCode(29873).SetNames("Timbalai_1948_RSO_Borneo_Meters", "GCS_Timbalai_1948", "D_Timbalai_1948");
            Timbalai1948RSOBorneoSearsChains = ProjectionInfo.FromEpsgCode(29871).SetNames("Timbalai_1948_RSO_Borneo_Chains", "GCS_Timbalai_1948", "D_Timbalai_1948");
            Timbalai1948RSOBorneoSearsFeet = ProjectionInfo.FromEpsgCode(29872).SetNames("Timbalai_1948_RSO_Borneo_Feet", "GCS_Timbalai_1948", "D_Timbalai_1948");
        }

        #endregion
    }
}

#pragma warning restore 1591