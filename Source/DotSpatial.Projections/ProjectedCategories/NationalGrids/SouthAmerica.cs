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
    /// This class contains predefined CoordinateSystems for SouthAmerica.
    /// </summary>
    public class SouthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo BogotaCiudadBogota;
        public readonly ProjectionInfo ColombiaBogotaZone;
        public readonly ProjectionInfo ColombiaEastZone;
        public readonly ProjectionInfo ColombiaECentralZone;
        public readonly ProjectionInfo ColombiaWestWestZone;
        public readonly ProjectionInfo ColombiaWestZone;
        public readonly ProjectionInfo LakeMaracaiboGrid;
        public readonly ProjectionInfo LakeMaracaiboGridM1;
        public readonly ProjectionInfo LakeMaracaiboGridM3;
        public readonly ProjectionInfo LakeMaracaiboLaRosaGrid;
        public readonly ProjectionInfo MAGNACiudadBogota;
        public readonly ProjectionInfo MAGNAColombiaBogota;
        public readonly ProjectionInfo MAGNAColombiaEste;
        public readonly ProjectionInfo MAGNAColombiaEsteEste;
        public readonly ProjectionInfo MAGNAColombiaOeste;
        public readonly ProjectionInfo MAGNAColombiaOesteOeste;
        public readonly ProjectionInfo PeruCentralZone;
        public readonly ProjectionInfo PeruEastZone;
        public readonly ProjectionInfo PeruWestZone;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatum1956ICNRegional;
        public readonly ProjectionInfo SAD1969BrazilPolyconic;
        public readonly ProjectionInfo Trinidad1903TrinidadGridClarkeFeet;
        public readonly ProjectionInfo Trinidad1903TrinidadGridLinksClarke;
        public readonly ProjectionInfo ZanderijSurinameOldTM;
        public readonly ProjectionInfo ZanderijSurinameTM;
        public readonly ProjectionInfo ZanderijTM54NW;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAmerica.
        /// </summary>
        public SouthAmerica()
        {
            BogotaCiudadBogota = ProjectionInfo.FromAuthorityCode("ESRI", 102232).SetNames("Bogota_Ciudad_Bogota", "GCS_Bogota", "D_Bogota"); // missing
            ColombiaBogotaZone = ProjectionInfo.FromEpsgCode(21897).SetNames("Colombia_Bogota_Zone", "GCS_Bogota", "D_Bogota");
            ColombiaEastZone = ProjectionInfo.FromEpsgCode(21899).SetNames("Colombia_East_Zone", "GCS_Bogota", "D_Bogota");
            ColombiaECentralZone = ProjectionInfo.FromEpsgCode(21898).SetNames("Colombia_East_Central_Zone", "GCS_Bogota", "D_Bogota");
            ColombiaWestWestZone = ProjectionInfo.FromAuthorityCode("ESRI", 102231).SetNames("Colombia_West_West_Zone", "GCS_Bogota", "D_Bogota"); // missing
            ColombiaWestZone = ProjectionInfo.FromEpsgCode(21896).SetNames("Colombia_West_Zone", "GCS_Bogota", "D_Bogota");
            LakeMaracaiboGrid = ProjectionInfo.FromEpsgCode(2102).SetNames("Lake_Maracaibo_Grid", "GCS_Lake", "D_Lake");
            LakeMaracaiboGridM1 = ProjectionInfo.FromEpsgCode(2101).SetNames("Lake_Maracaibo_Grid_M1", "GCS_Lake", "D_Lake");
            LakeMaracaiboGridM3 = ProjectionInfo.FromEpsgCode(2103).SetNames("Lake_Maracaibo_Grid_M3", "GCS_Lake", "D_Lake");
            LakeMaracaiboLaRosaGrid = ProjectionInfo.FromEpsgCode(2104).SetNames("Lake_Maracaibo_La_Rosa_Grid", "GCS_Lake", "D_Lake");
            MAGNACiudadBogota = ProjectionInfo.FromAuthorityCode("ESRI", 102233).SetNames("MAGNA_Ciudad_Bogota", "GCS_MAGNA", "D_MAGNA"); // missing
            MAGNAColombiaBogota = ProjectionInfo.FromEpsgCode(3116).SetNames("MAGNA_Colombia_Bogota", "GCS_MAGNA", "D_MAGNA");
            MAGNAColombiaEste = ProjectionInfo.FromEpsgCode(3117).SetNames("MAGNA_Colombia_Este", "GCS_MAGNA", "D_MAGNA");
            MAGNAColombiaEsteEste = ProjectionInfo.FromEpsgCode(3118).SetNames("MAGNA_Colombia_Este_Este", "GCS_MAGNA", "D_MAGNA");
            MAGNAColombiaOeste = ProjectionInfo.FromEpsgCode(3115).SetNames("MAGNA_Colombia_Oeste", "GCS_MAGNA", "D_MAGNA");
            MAGNAColombiaOesteOeste = ProjectionInfo.FromEpsgCode(3114).SetNames("MAGNA_Colombia_Oeste_Oeste", "GCS_MAGNA", "D_MAGNA");
            PeruCentralZone = ProjectionInfo.FromEpsgCode(24892).SetNames("Peru_Central_Zone", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            PeruEastZone = ProjectionInfo.FromEpsgCode(24893).SetNames("Peru_East_Zone", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            PeruWestZone = ProjectionInfo.FromEpsgCode(24891).SetNames("Peru_West_Zone", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatum1956ICNRegional = ProjectionInfo.FromEpsgCode(2317).SetNames("PSAD_1956_ICN_Regional", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            SAD1969BrazilPolyconic = ProjectionInfo.FromEpsgCode(29101).SetNames("SAD_1969_Brazil_Polyconic", "GCS_South_American_1969", "D_South_American_1969");
            Trinidad1903TrinidadGridClarkeFeet = ProjectionInfo.FromEpsgCode(2314).SetNames("Trinidad_1903_Trinidad_Grid_Feet_Clarke", "GCS_Trinidad_1903", "D_Trinidad_1903");
            Trinidad1903TrinidadGridLinksClarke = ProjectionInfo.FromEpsgCode(30200).SetNames("Trinidad_1903_Trinidad_Grid", "GCS_Trinidad_1903", "D_Trinidad_1903");
            ZanderijSurinameOldTM = ProjectionInfo.FromEpsgCode(31170).SetNames("Zanderij_Suriname_Old_TM", "GCS_Zanderij", "D_Zanderij");
            ZanderijSurinameTM = ProjectionInfo.FromEpsgCode(31171).SetNames("Zanderij_Suriname_TM", "GCS_Zanderij", "D_Zanderij");
            ZanderijTM54NW = ProjectionInfo.FromEpsgCode(31154).SetNames("Zanderij_TM_54_NW", "GCS_Zanderij", "D_Zanderij");
        }

        #endregion
    }
}

#pragma warning restore 1591