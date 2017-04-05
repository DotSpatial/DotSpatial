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
    /// This class contains predefined CoordinateSystems for Africa.
    /// </summary>
    public class Africa : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Abidjan1987TM5NW;
        public readonly ProjectionInfo AccraGhanaGrid;
        public readonly ProjectionInfo AccraTM1NW;
        public readonly ProjectionInfo BeduaramTM13NE;
        public readonly ProjectionInfo CamacupaTM1130SE;
        public readonly ProjectionInfo CamacupaTM12SE;
        public readonly ProjectionInfo CarthageTM11NE;
        public readonly ProjectionInfo Douala1948AOFWest;
        public readonly ProjectionInfo EgyptBlueBelt;
        public readonly ProjectionInfo EgyptExtendedPurpleBelt;
        public readonly ProjectionInfo EgyptGulfofSuezS650TLRedBelt;
        public readonly ProjectionInfo EgyptPurpleBelt;
        public readonly ProjectionInfo EgyptRedBelt;
        public readonly ProjectionInfo GhanaMetreGrid;
        public readonly ProjectionInfo IGC1962CongoTMZone12;
        public readonly ProjectionInfo IGC1962CongoTMZone14;
        public readonly ProjectionInfo IGC1962CongoTMZone16;
        public readonly ProjectionInfo IGC1962CongoTMZone18;
        public readonly ProjectionInfo IGC1962CongoTMZone20;
        public readonly ProjectionInfo IGC1962CongoTMZone22;
        public readonly ProjectionInfo IGC1962CongoTMZone24;
        public readonly ProjectionInfo IGC1962CongoTMZone26;
        public readonly ProjectionInfo IGC1962CongoTMZone28;
        public readonly ProjectionInfo IGC1962CongoTMZone30;
        public readonly ProjectionInfo IGCB1955CongoTMZone12;
        public readonly ProjectionInfo IGCB1955CongoTMZone14;
        public readonly ProjectionInfo IGCB1955CongoTMZone16;
        public readonly ProjectionInfo Kasai1953CongoTMZone22;
        public readonly ProjectionInfo Kasai1953CongoTMZone24;
        public readonly ProjectionInfo Katanga1955KatangaGaussA;
        public readonly ProjectionInfo Katanga1955KatangaGaussB;
        public readonly ProjectionInfo Katanga1955KatangaGaussC;
        public readonly ProjectionInfo Katanga1955KatangaGaussD;
        public readonly ProjectionInfo Katanga1955KatangaLambert;
        public readonly ProjectionInfo Katanga1955KatangaTM;
        public readonly ProjectionInfo Locodjo1965TM5NW;
        public readonly ProjectionInfo MerchichSaharaNord;
        public readonly ProjectionInfo MerchichSaharaSud;
        public readonly ProjectionInfo NigeriaEastBelt;
        public readonly ProjectionInfo NigeriaMidBelt;
        public readonly ProjectionInfo NigeriaWestBelt;
        public readonly ProjectionInfo NordAlgerie;
        public readonly ProjectionInfo NordAlgerieancienne;
        public readonly ProjectionInfo NordAlgerieAncienneDegrees;
        public readonly ProjectionInfo NordAlgerieDegrees;
        public readonly ProjectionInfo NordMaroc;
        public readonly ProjectionInfo NordMarocDegrees;
        public readonly ProjectionInfo NordSahara1959VoirolUnifieNord;
        public readonly ProjectionInfo NordSahara1959VoirolUnifieSud;
        public readonly ProjectionInfo NordTunisie;
        public readonly ProjectionInfo RGRDC2005CongoTMZone12;
        public readonly ProjectionInfo RGRDC2005CongoTMZone14;
        public readonly ProjectionInfo RGRDC2005CongoTMZone16;
        public readonly ProjectionInfo RGRDC2005CongoTMZone18;
        public readonly ProjectionInfo RGRDC2005CongoTMZone20;
        public readonly ProjectionInfo RGRDC2005CongoTMZone22;
        public readonly ProjectionInfo RGRDC2005CongoTMZone24;
        public readonly ProjectionInfo RGRDC2005CongoTMZone26;
        public readonly ProjectionInfo RGRDC2005CongoTMZone28;
        public readonly ProjectionInfo Sahara;
        public readonly ProjectionInfo SaharaDegrees;
        public readonly ProjectionInfo SierraLeone1924NewColonyGrid;
        public readonly ProjectionInfo SierraLeone1924NewWarOfficeGrid;
        public readonly ProjectionInfo SudAlgerie;
        public readonly ProjectionInfo SudAlgerieAncienne;
        public readonly ProjectionInfo SudAlgerieAncienneDegree;
        public readonly ProjectionInfo SudAlgerieDegrees;
        public readonly ProjectionInfo SudMaroc;
        public readonly ProjectionInfo SudMarocDegrees;
        public readonly ProjectionInfo SudTunisie;
        //public readonly ProjectionInfo Tananarive1925LabordeGrid;
        //public readonly ProjectionInfo Tananarive1925ParisLabordeGrid;
        public readonly ProjectionInfo Voirol1879NordAlgerieAncienne;
        public readonly ProjectionInfo Voirol1879SudAlgerieAncienne;
        public readonly ProjectionInfo WGS1984TM36SE;
        public readonly ProjectionInfo WGS1984TM6NE;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Africa.
        /// </summary>
        public Africa()
        {
            Abidjan1987TM5NW = ProjectionInfo.FromEpsgCode(2165).SetNames("Abidjan_1987_TM_5_NW", "GCS_Abidjan_1987", "D_Abidjan_1987");
            AccraGhanaGrid = ProjectionInfo.FromEpsgCode(2136).SetNames("Accra_Ghana_Grid", "GCS_Accra", "D_Accra");
            AccraTM1NW = ProjectionInfo.FromEpsgCode(2137).SetNames("Accra_TM_1_NW", "GCS_Accra", "D_Accra");
            BeduaramTM13NE = ProjectionInfo.FromEpsgCode(2931).SetNames("Beduaram_TM_13_NE", "GCS_Beduaram", "D_Beduaram");
            CamacupaTM1130SE = ProjectionInfo.FromEpsgCode(22091).SetNames("Camacupa_TM_11_30_SE", "GCS_Camacupa", "D_Camacupa");
            CamacupaTM12SE = ProjectionInfo.FromEpsgCode(22092).SetNames("Camacupa_TM_12_SE", "GCS_Camacupa", "D_Camacupa");
            CarthageTM11NE = ProjectionInfo.FromEpsgCode(2088).SetNames("Carthage_TM_11_NE", "GCS_Carthage", "D_Carthage");
            Douala1948AOFWest = ProjectionInfo.FromEpsgCode(3119).SetNames("Douala_1948_AEF_West", "GCS_Douala_1948", "D_Douala_1948");
            EgyptBlueBelt = ProjectionInfo.FromEpsgCode(22991).SetNames("Egypt_Blue_Belt", "GCS_Egypt_1907", "D_Egypt_1907");
            EgyptExtendedPurpleBelt = ProjectionInfo.FromEpsgCode(22994).SetNames("Egypt_Extended_Purple_Belt", "GCS_Egypt_1907", "D_Egypt_1907");
            EgyptGulfofSuezS650TLRedBelt = ProjectionInfo.FromEpsgCode(3355).SetNames("Egypt_Gulf_of_Suez_S-650_TL_Red_Belt", "GCS_Egypt_Gulf_of_Suez_S-650_TL", "D_Egypt_Gulf_of_Suez_S-650_TL");
            EgyptPurpleBelt = ProjectionInfo.FromEpsgCode(22993).SetNames("Egypt_Purple_Belt", "GCS_Egypt_1907", "D_Egypt_1907");
            EgyptRedBelt = ProjectionInfo.FromEpsgCode(22992).SetNames("Egypt_Red_Belt", "GCS_Egypt_1907", "D_Egypt_1907");
            GhanaMetreGrid = ProjectionInfo.FromEpsgCode(25000).SetNames("Ghana_Metre_Grid", "GCS_Leigon", "D_Leigon");
            IGC1962CongoTMZone12 = ProjectionInfo.FromEpsgCode(3318).SetNames("IGC_1962_Congo_TM_Zone_12", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone14 = ProjectionInfo.FromEpsgCode(3319).SetNames("IGC_1962_Congo_TM_Zone_14", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone16 = ProjectionInfo.FromEpsgCode(3320).SetNames("IGC_1962_Congo_TM_Zone_16", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone18 = ProjectionInfo.FromEpsgCode(3321).SetNames("IGC_1962_Congo_TM_Zone_18", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone20 = ProjectionInfo.FromEpsgCode(3322).SetNames("IGC_1962_Congo_TM_Zone_20", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone22 = ProjectionInfo.FromEpsgCode(3323).SetNames("IGC_1962_Congo_TM_Zone_22", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone24 = ProjectionInfo.FromEpsgCode(3324).SetNames("IGC_1962_Congo_TM_Zone_24", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone26 = ProjectionInfo.FromEpsgCode(3325).SetNames("IGC_1962_Congo_TM_Zone_26", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone28 = ProjectionInfo.FromEpsgCode(3326).SetNames("IGC_1962_Congo_TM_Zone_28", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGC1962CongoTMZone30 = ProjectionInfo.FromEpsgCode(3327).SetNames("IGC_1962_Congo_TM_Zone_30", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGCB1955CongoTMZone12 = ProjectionInfo.FromEpsgCode(3339).SetNames("IGCB_1955_Congo_TM_Zone_12", "GCS_IGCB_1955", "D_Institut_Geographique_du_Congo_Belge_1955");
            IGCB1955CongoTMZone14 = ProjectionInfo.FromEpsgCode(3340).SetNames("IGCB_1955_Congo_TM_Zone_14", "GCS_IGCB_1955", "D_Institut_Geographique_du_Congo_Belge_1955");
            IGCB1955CongoTMZone16 = ProjectionInfo.FromEpsgCode(3341).SetNames("IGCB_1955_Congo_TM_Zone_16", "GCS_IGCB_1955", "D_Institut_Geographique_du_Congo_Belge_1955");
            Kasai1953CongoTMZone22 = ProjectionInfo.FromEpsgCode(3316).SetNames("Kasai_1953_Congo_TM_Zone_22", "GCS_Kasai_1953", "D_Kasai_1953");
            Kasai1953CongoTMZone24 = ProjectionInfo.FromEpsgCode(3317).SetNames("Kasai_1953_Congo_TM_Zone_24", "GCS_Kasai_1953", "D_Kasai_1953");
            Katanga1955KatangaGaussA = ProjectionInfo.FromEpsgCode(3986).SetNames("Katanga_1955_Katanga_Gauss_Zone_A", "GCS_Katanga_1955", "D_Katanga_1955");
            Katanga1955KatangaGaussB = ProjectionInfo.FromEpsgCode(3987).SetNames("Katanga_1955_Katanga_Gauss_Zone_B", "GCS_Katanga_1955", "D_Katanga_1955");
            Katanga1955KatangaGaussC = ProjectionInfo.FromEpsgCode(3988).SetNames("Katanga_1955_Katanga_Gauss_Zone_C", "GCS_Katanga_1955", "D_Katanga_1955");
            Katanga1955KatangaGaussD = ProjectionInfo.FromEpsgCode(3989).SetNames("Katanga_1955_Katanga_Gauss_Zone_D", "GCS_Katanga_1955", "D_Katanga_1955");
            Katanga1955KatangaLambert = ProjectionInfo.FromAuthorityCode("EPSG", 102762).SetNames("Katanga_1955_Katanga_Lambert", "GCS_Katanga_1955", "D_Katanga_1955"); // missing
            Katanga1955KatangaTM = ProjectionInfo.FromEpsgCode(3315).SetNames("Katanga_1955_Katanga_TM", "GCS_Katanga_1955", "D_Katanga_1955");
            Locodjo1965TM5NW = ProjectionInfo.FromEpsgCode(2164).SetNames("Locodjo_1965_TM_5_NW", "GCS_Locodjo_1965", "D_Locodjo_1965");
            MerchichSaharaNord = ProjectionInfo.FromEpsgCode(26194).SetNames("Merchich_Sahara_Nord", "GCS_Merchich", "D_Merchich");
            MerchichSaharaSud = ProjectionInfo.FromEpsgCode(26195).SetNames("Merchich_Sahara_Sud", "GCS_Merchich", "D_Merchich");
            NigeriaEastBelt = ProjectionInfo.FromEpsgCode(26393).SetNames("Nigeria_East_Belt", "GCS_Minna", "D_Minna");
            NigeriaMidBelt = ProjectionInfo.FromEpsgCode(26392).SetNames("Nigeria_Mid_Belt", "GCS_Minna", "D_Minna");
            NigeriaWestBelt = ProjectionInfo.FromEpsgCode(26391).SetNames("Nigeria_West_Belt", "GCS_Minna", "D_Minna");
            NordAlgerie = ProjectionInfo.FromAuthorityCode("EPSG", 30591).SetNames("Nord_Algerie", "GCS_Voirol_Unifie_1960", "D_Voirol_Unifie_1960"); // missing
            NordAlgerieancienne = ProjectionInfo.FromEpsgCode(30491).SetNames("Nord_Algerie_Ancienne", "GCS_Voirol_1875", "D_Voirol_1875");
            NordAlgerieAncienneDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102491).SetNames("Nord_Algerie_Ancienne_Degree", "GCS_Voirol_1875", "D_Voirol_1875");
            NordAlgerieDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102591).SetNames("Nord_Algerie_Degree", "GCS_Voirol_Unifie_1960_Degree", "D_Voirol_Unifie_1960");
            NordMaroc = ProjectionInfo.FromEpsgCode(26191).SetNames("Nord_Maroc", "GCS_Merchich", "D_Merchich");
            NordMarocDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102191).SetNames("Nord_Maroc_Degree", "GCS_Merchich_Degree", "D_Merchich");
            NordSahara1959VoirolUnifieNord = ProjectionInfo.FromEpsgCode(30791).SetNames("Nord_Sahara_1959_Voirol_Unifie_Nord", "GCS_Nord_Sahara_1959", "D_Nord_Sahara_1959");
            NordSahara1959VoirolUnifieSud = ProjectionInfo.FromEpsgCode(30792).SetNames("Nord_Sahara_1959_Voirol_Unifie_Sud", "GCS_Nord_Sahara_1959", "D_Nord_Sahara_1959");
            NordTunisie = ProjectionInfo.FromEpsgCode(22391).SetNames("Nord_Tunisie", "GCS_Carthage", "D_Carthage");
            RGRDC2005CongoTMZone12 = ProjectionInfo.FromAuthorityCode("EPSG", 103201).SetNames("RGRDC_2005_Congo_TM_Zone_12", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone14 = ProjectionInfo.FromAuthorityCode("EPSG", 103202).SetNames("RGRDC_2005_Congo_TM_Zone_14", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone16 = ProjectionInfo.FromAuthorityCode("EPSG", 103203).SetNames("RGRDC_2005_Congo_TM_Zone_16", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone18 = ProjectionInfo.FromAuthorityCode("EPSG", 103204).SetNames("RGRDC_2005_Congo_TM_Zone_18", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone20 = ProjectionInfo.FromAuthorityCode("EPSG", 103205).SetNames("RGRDC_2005_Congo_TM_Zone_20", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone22 = ProjectionInfo.FromAuthorityCode("EPSG", 103206).SetNames("RGRDC_2005_Congo_TM_Zone_22", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone24 = ProjectionInfo.FromAuthorityCode("EPSG", 103207).SetNames("RGRDC_2005_Congo_TM_Zone_24", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone26 = ProjectionInfo.FromAuthorityCode("EPSG", 103208).SetNames("RGRDC_2005_Congo_TM_Zone_26", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005CongoTMZone28 = ProjectionInfo.FromAuthorityCode("EPSG", 103209).SetNames("RGRDC_2005_Congo_TM_Zone_28", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            Sahara = ProjectionInfo.FromEpsgCode(26193).SetNames("Sahara", "GCS_Merchich", "D_Merchich");
            SaharaDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102193).SetNames("Sahara_Degree", "GCS_Merchich_Degree", "D_Merchich");
            SierraLeone1924NewColonyGrid = ProjectionInfo.FromEpsgCode(2159).SetNames("Sierra_Leone_1924_New_Colony_Grid", "GCS_Sierra_Leone_1924", "D_Sierra_Leone_1924");
            SierraLeone1924NewWarOfficeGrid = ProjectionInfo.FromEpsgCode(2160).SetNames("Sierra_Leone_1924_New_War_Office_Grid", "GCS_Sierra_Leone_1924", "D_Sierra_Leone_1924");
            SudAlgerie = ProjectionInfo.FromAuthorityCode("EPSG", 30592).SetNames("Sud_Algerie", "GCS_Voirol_Unifie_1960", "D_Voirol_Unifie_1960"); // missing
            SudAlgerieAncienne = ProjectionInfo.FromEpsgCode(30492).SetNames("Sud_Algerie_Ancienne", "GCS_Voirol_1875", "D_Voirol_1875");
            SudAlgerieAncienneDegree = ProjectionInfo.FromAuthorityCode("ESRI", 102492).SetNames("Sud_Algerie_Ancienne_Degree", "GCS_Voirol_1875", "D_Voirol_1875");
            SudAlgerieDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102592).SetNames("Sud_Algerie_Degree", "GCS_Voirol_Unifie_1960_Degree", "D_Voirol_Unifie_1960");
            SudMaroc = ProjectionInfo.FromEpsgCode(26192).SetNames("Sud_Maroc", "GCS_Merchich", "D_Merchich");
            SudMarocDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102192).SetNames("Sud_Maroc_Degree", "GCS_Merchich_Degree", "D_Merchich");
            SudTunisie = ProjectionInfo.FromEpsgCode(22392).SetNames("Sud_Tunisie", "GCS_Carthage", "D_Carthage");
            // Tananarive1925LabordeGrid = ProjectionInfo.FromAuthorityCode("ESRI", 102590).SetNames("Tananarive_1925_Laborde_Grid", "GCS_Tananarive_1925", "D_Tananarive_1925"); // projection not found
            // Tananarive1925ParisLabordeGrid = ProjectionInfo.FromAuthorityCode("EPSG", 29701).SetNames("Tananarive_1925_Paris_Laborde_Grid", "GCS_Tananarive_1925_Paris", "D_Tananarive_1925"); // projection not found
            Voirol1879NordAlgerieAncienne = ProjectionInfo.FromEpsgCode(30493).SetNames("Voirol_1879_Nord_Algerie_Ancienne", "GCS_Voirol_1879", "D_Voirol_1879");
            Voirol1879SudAlgerieAncienne = ProjectionInfo.FromEpsgCode(30494).SetNames("Voirol_1879_Sud_Algerie_Ancienne", "GCS_Voirol_1879", "D_Voirol_1879");
            WGS1984TM36SE = ProjectionInfo.FromEpsgCode(32766).SetNames("WGS_1984_TM_36_SE", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984TM6NE = ProjectionInfo.FromEpsgCode(2311).SetNames("WGS_1984_TM_6_NE", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591