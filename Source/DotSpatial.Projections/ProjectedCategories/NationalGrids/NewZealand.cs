// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:52:35 PM
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
    /// This class contains predefined CoordinateSystems for NewZealand.
    /// </summary>
    public class NewZealand : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo ChathamIslands1979MapGrid;
        public readonly ProjectionInfo NewZealandMapGrid;
        public readonly ProjectionInfo NewZealandNorthIsland;
        public readonly ProjectionInfo NewZealandSouthIsland;
        public readonly ProjectionInfo NZGD1949AmuriCircuit;
        public readonly ProjectionInfo NZGD1949BayofPlentyCircuit;
        public readonly ProjectionInfo NZGD1949BluffCircuit;
        public readonly ProjectionInfo NZGD1949BullerCircuit;
        public readonly ProjectionInfo NZGD1949CollingwoodCircuit;
        public readonly ProjectionInfo NZGD1949GawlerCircuit;
        public readonly ProjectionInfo NZGD1949GreyCircuit;
        public readonly ProjectionInfo NZGD1949HawkesBayCircuit;
        public readonly ProjectionInfo NZGD1949HokitikaCircuit;
        public readonly ProjectionInfo NZGD1949JacksonsBayCircuit;
        public readonly ProjectionInfo NZGD1949KarameaCircuit;
        public readonly ProjectionInfo NZGD1949LindisPeakCircuit;
        public readonly ProjectionInfo NZGD1949MarlboroughCircuit;
        public readonly ProjectionInfo NZGD1949MountEdenCircuit;
        public readonly ProjectionInfo NZGD1949MountNicholasCircuit;
        public readonly ProjectionInfo NZGD1949MountPleasantCircuit;
        public readonly ProjectionInfo NZGD1949MountYorkCircuit;
        public readonly ProjectionInfo NZGD1949NelsonCircuit;
        public readonly ProjectionInfo NZGD1949NorthTaieriCircuit;
        public readonly ProjectionInfo NZGD1949ObservationPointCircuit;
        public readonly ProjectionInfo NZGD1949OkaritoCircuit;
        public readonly ProjectionInfo NZGD1949PovertyBayCircuit;
        public readonly ProjectionInfo NZGD1949TaranakiCircuit;
        public readonly ProjectionInfo NZGD1949TimaruCircuit;
        public readonly ProjectionInfo NZGD1949TuhirangiCircuit;
        public readonly ProjectionInfo NZGD1949UTMZone58S;
        public readonly ProjectionInfo NZGD1949UTMZone59S;
        public readonly ProjectionInfo NZGD1949UTMZone60S;
        public readonly ProjectionInfo NZGD1949WairarapaCircuit;
        public readonly ProjectionInfo NZGD1949WanganuiCircuit;
        public readonly ProjectionInfo NZGD1949WellingtonCircuit;
        public readonly ProjectionInfo NZGD2000AmuriCircuit;
        public readonly ProjectionInfo NZGD2000AntipodesIslandsTM2000;
        public readonly ProjectionInfo NZGD2000AucklandIslandsTM2000;
        public readonly ProjectionInfo NZGD2000BayofPlentyCircuit;
        public readonly ProjectionInfo NZGD2000BluffCircuit;
        public readonly ProjectionInfo NZGD2000BullerCircuit;
        public readonly ProjectionInfo NZGD2000CampbellIslandTM2000;
        public readonly ProjectionInfo NZGD2000ChathamIslandCircuit;
        public readonly ProjectionInfo NZGD2000ChathamIslandsTM2000;
        public readonly ProjectionInfo NZGD2000CollingwoodCircuit;
        public readonly ProjectionInfo NZGD2000GawlerCircuit;
        public readonly ProjectionInfo NZGD2000GreyCircuit;
        public readonly ProjectionInfo NZGD2000HawkesBayCircuit;
        public readonly ProjectionInfo NZGD2000HokitikaCircuit;
        public readonly ProjectionInfo NZGD2000JacksonsBayCircuit;
        public readonly ProjectionInfo NZGD2000KarameaCircuit;
        public readonly ProjectionInfo NZGD2000LindisPeakCircuit;
        public readonly ProjectionInfo NZGD2000MarlboroughCircuit;
        public readonly ProjectionInfo NZGD2000MountEdenCircuit;
        public readonly ProjectionInfo NZGD2000MountNicholasCircuit;
        public readonly ProjectionInfo NZGD2000MountPleasantCircuit;
        public readonly ProjectionInfo NZGD2000MountYorkCircuit;
        public readonly ProjectionInfo NZGD2000NelsonCircuit;
        public readonly ProjectionInfo NZGD2000NewZealandTransverseMercator;
        public readonly ProjectionInfo NZGD2000NorthTaieriCircuit;
        public readonly ProjectionInfo NZGD2000NZContinentalShelf2000;
        public readonly ProjectionInfo NZGD2000ObservationPointCircuit;
        public readonly ProjectionInfo NZGD2000OkaritoCircuit;
        public readonly ProjectionInfo NZGD2000PovertyBayCircuit;
        public readonly ProjectionInfo NZGD2000RaoulIslandTM2000;
        public readonly ProjectionInfo NZGD2000TaranakiCircuit;
        public readonly ProjectionInfo NZGD2000TimaruCircuit;
        public readonly ProjectionInfo NZGD2000TuhirangiCircuit;
        public readonly ProjectionInfo NZGD2000UTMZone58S;
        public readonly ProjectionInfo NZGD2000UTMZone59S;
        public readonly ProjectionInfo NZGD2000UTMZone60S;
        public readonly ProjectionInfo NZGD2000WairarapaCircuit;
        public readonly ProjectionInfo NZGD2000WanganuiCircuit;
        public readonly ProjectionInfo NZGD2000WellingtonCircuit;
        public readonly ProjectionInfo WGS1984Mercator41;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NewZealand.
        /// </summary>
        public NewZealand()
        {
            ChathamIslands1979MapGrid = ProjectionInfo.FromAuthorityCode("EPSG", 102111).SetNames("Chatham_Islands_1979_Map_Grid", "GCS_Chatham_Islands_1979", "D_Chatham_Islands_1979"); // missing
            NewZealandMapGrid = ProjectionInfo.FromEpsgCode(27200).SetNames("GD_1949_New_Zealand_Map_Grid", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NewZealandNorthIsland = ProjectionInfo.FromEpsgCode(27291).SetNames("New_Zealand_North_Island", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NewZealandSouthIsland = ProjectionInfo.FromEpsgCode(27292).SetNames("New_Zealand_South_Island", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949AmuriCircuit = ProjectionInfo.FromEpsgCode(27219).SetNames("NZGD_1949_Amuri_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949BayofPlentyCircuit = ProjectionInfo.FromEpsgCode(27206).SetNames("NZGD_1949_Bay_of_Plenty_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949BluffCircuit = ProjectionInfo.FromEpsgCode(27232).SetNames("NZGD_1949_Bluff_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949BullerCircuit = ProjectionInfo.FromEpsgCode(27217).SetNames("NZGD_1949_Buller_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949CollingwoodCircuit = ProjectionInfo.FromEpsgCode(27214).SetNames("NZGD_1949_Collingwood_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949GawlerCircuit = ProjectionInfo.FromEpsgCode(27225).SetNames("NZGD_1949_Gawler_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949GreyCircuit = ProjectionInfo.FromEpsgCode(27218).SetNames("NZGD_1949_Grey_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949HawkesBayCircuit = ProjectionInfo.FromEpsgCode(27208).SetNames("NZGD_1949_Hawkes_Bay_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949HokitikaCircuit = ProjectionInfo.FromEpsgCode(27221).SetNames("NZGD_1949_Hokitika_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949JacksonsBayCircuit = ProjectionInfo.FromEpsgCode(27223).SetNames("NZGD_1949_Jacksons_Bay_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949KarameaCircuit = ProjectionInfo.FromEpsgCode(27216).SetNames("NZGD_1949_Karamea_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949LindisPeakCircuit = ProjectionInfo.FromEpsgCode(27227).SetNames("NZGD_1949_Lindis_Peak_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949MarlboroughCircuit = ProjectionInfo.FromEpsgCode(27220).SetNames("NZGD_1949_Marlborough_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949MountEdenCircuit = ProjectionInfo.FromEpsgCode(27205).SetNames("NZGD_1949_Mount_Eden_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949MountNicholasCircuit = ProjectionInfo.FromEpsgCode(27228).SetNames("NZGD_1949_Mount_Nicholas_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949MountPleasantCircuit = ProjectionInfo.FromEpsgCode(27224).SetNames("NZGD_1949_Mount_Pleasant_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949MountYorkCircuit = ProjectionInfo.FromEpsgCode(27229).SetNames("NZGD_1949_Mount_York_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949NelsonCircuit = ProjectionInfo.FromEpsgCode(27215).SetNames("NZGD_1949_Nelson_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949NorthTaieriCircuit = ProjectionInfo.FromEpsgCode(27231).SetNames("NZGD_1949_North_Taieri_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949ObservationPointCircuit = ProjectionInfo.FromEpsgCode(27230).SetNames("NZGD_1949_Observation_Point_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949OkaritoCircuit = ProjectionInfo.FromEpsgCode(27222).SetNames("NZGD_1949_Okarito_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949PovertyBayCircuit = ProjectionInfo.FromEpsgCode(27207).SetNames("NZGD_1949_Poverty_Bay_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949TaranakiCircuit = ProjectionInfo.FromEpsgCode(27209).SetNames("NZGD_1949_Taranaki_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949TimaruCircuit = ProjectionInfo.FromEpsgCode(27226).SetNames("NZGD_1949_Timaru_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949TuhirangiCircuit = ProjectionInfo.FromEpsgCode(27210).SetNames("NZGD_1949_Tuhirangi_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949UTMZone58S = ProjectionInfo.FromEpsgCode(27258).SetNames("NZGD_1949_UTM_Zone_58S", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949UTMZone59S = ProjectionInfo.FromEpsgCode(27259).SetNames("NZGD_1949_UTM_Zone_59S", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949UTMZone60S = ProjectionInfo.FromEpsgCode(27260).SetNames("NZGD_1949_UTM_Zone_60S", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949WairarapaCircuit = ProjectionInfo.FromEpsgCode(27212).SetNames("NZGD_1949_Wairarapa_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949WanganuiCircuit = ProjectionInfo.FromEpsgCode(27211).SetNames("NZGD_1949_Wanganui_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD1949WellingtonCircuit = ProjectionInfo.FromEpsgCode(27213).SetNames("NZGD_1949_Wellington_Circuit", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD2000AmuriCircuit = ProjectionInfo.FromEpsgCode(2119).SetNames("NZGD_2000_Amuri_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000AntipodesIslandsTM2000 = ProjectionInfo.FromEpsgCode(3790).SetNames("NZGD_2000_Antipodes_Islands_TM_2000", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000AucklandIslandsTM2000 = ProjectionInfo.FromEpsgCode(3788).SetNames("NZGD_2000_Auckland_Islands_TM_2000", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000BayofPlentyCircuit = ProjectionInfo.FromEpsgCode(2106).SetNames("NZGD_2000_Bay_of_Plenty_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000BluffCircuit = ProjectionInfo.FromEpsgCode(2132).SetNames("NZGD_2000_Bluff_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000BullerCircuit = ProjectionInfo.FromEpsgCode(2117).SetNames("NZGD_2000_Buller_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000CampbellIslandTM2000 = ProjectionInfo.FromEpsgCode(3789).SetNames("NZGD_2000_Campbell_Island_TM_2000", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000ChathamIslandCircuit = ProjectionInfo.FromEpsgCode(3764).SetNames("NZGD_2000_Chatham_Island_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000ChathamIslandsTM2000 = ProjectionInfo.FromEpsgCode(3793).SetNames("NZGD_2000_Chatham_Islands_TM_2000", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000CollingwoodCircuit = ProjectionInfo.FromEpsgCode(2114).SetNames("NZGD_2000_Collingwood_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000GawlerCircuit = ProjectionInfo.FromEpsgCode(2125).SetNames("NZGD_2000_Gawler_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000GreyCircuit = ProjectionInfo.FromEpsgCode(2118).SetNames("NZGD_2000_Grey_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000HawkesBayCircuit = ProjectionInfo.FromEpsgCode(2108).SetNames("NZGD_2000_Hawkes_Bay_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000HokitikaCircuit = ProjectionInfo.FromEpsgCode(2121).SetNames("NZGD_2000_Hokitika_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000JacksonsBayCircuit = ProjectionInfo.FromEpsgCode(2123).SetNames("NZGD_2000_Jacksons_Bay_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000KarameaCircuit = ProjectionInfo.FromEpsgCode(2116).SetNames("NZGD_2000_Karamea_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000LindisPeakCircuit = ProjectionInfo.FromEpsgCode(2127).SetNames("NZGD_2000_Lindis_Peak_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000MarlboroughCircuit = ProjectionInfo.FromEpsgCode(2120).SetNames("NZGD_2000_Marlborough_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000MountEdenCircuit = ProjectionInfo.FromEpsgCode(2105).SetNames("NZGD_2000_Mount_Eden_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000MountNicholasCircuit = ProjectionInfo.FromEpsgCode(2128).SetNames("NZGD_2000_Mount_Nicholas_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000MountPleasantCircuit = ProjectionInfo.FromEpsgCode(2124).SetNames("NZGD_2000_Mount_Pleasant_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000MountYorkCircuit = ProjectionInfo.FromEpsgCode(2129).SetNames("NZGD_2000_Mount_York_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000NelsonCircuit = ProjectionInfo.FromEpsgCode(2115).SetNames("NZGD_2000_Nelson_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000NewZealandTransverseMercator = ProjectionInfo.FromEpsgCode(2193).SetNames("NZGD_2000_New_Zealand_Transverse_Mercator", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000NorthTaieriCircuit = ProjectionInfo.FromEpsgCode(2131).SetNames("NZGD_2000_North_Taieri_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000NZContinentalShelf2000 = ProjectionInfo.FromEpsgCode(3851).SetNames("NZGD_2000_NZ_Continental_Shelf_2000", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000ObservationPointCircuit = ProjectionInfo.FromEpsgCode(2130).SetNames("NZGD_2000_Observation_Point_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000OkaritoCircuit = ProjectionInfo.FromEpsgCode(2122).SetNames("NZGD_2000_Okarito_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000PovertyBayCircuit = ProjectionInfo.FromEpsgCode(2107).SetNames("NZGD_2000_Poverty_Bay_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000RaoulIslandTM2000 = ProjectionInfo.FromEpsgCode(3791).SetNames("NZGD_2000_Raoul_Island_TM_2000", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000TaranakiCircuit = ProjectionInfo.FromEpsgCode(2109).SetNames("NZGD_2000_Taranaki_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000TimaruCircuit = ProjectionInfo.FromEpsgCode(2126).SetNames("NZGD_2000_Timaru_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000TuhirangiCircuit = ProjectionInfo.FromEpsgCode(2110).SetNames("NZGD_2000_Tuhirangi_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000UTMZone58S = ProjectionInfo.FromEpsgCode(2133).SetNames("NZGD_2000_UTM_Zone_58S", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000UTMZone59S = ProjectionInfo.FromEpsgCode(2134).SetNames("NZGD_2000_UTM_Zone_59S", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000UTMZone60S = ProjectionInfo.FromEpsgCode(2135).SetNames("NZGD_2000_UTM_Zone_60S", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000WairarapaCircuit = ProjectionInfo.FromEpsgCode(2112).SetNames("NZGD_2000_Wairarapa_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000WanganuiCircuit = ProjectionInfo.FromEpsgCode(2111).SetNames("NZGD_2000_Wanganui_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            NZGD2000WellingtonCircuit = ProjectionInfo.FromEpsgCode(2113).SetNames("NZGD_2000_Wellington_Circuit", "GCS_NZGD_2000", "D_NZGD_2000");
            WGS1984Mercator41 = ProjectionInfo.FromEpsgCode(3994).SetNames("WGS_1984_Mercator_41", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591