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
    /// This class contains predefined CoordinateSystems for Asia.
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AinelAbd1970AramcoLambert2;
        public readonly ProjectionInfo AinelAbdAramcoLambert;
        public readonly ProjectionInfo BahrainStateGrid;
        public readonly ProjectionInfo DeirezZorLevantStereographic;
        public readonly ProjectionInfo DeirezZorLevantZone;
        public readonly ProjectionInfo DeirezZorSyriaLambert;
        public readonly ProjectionInfo ED1950IraqNationalGrid;
        public readonly ProjectionInfo ED1950JordanTM;
        // public readonly ProjectionInfo EverestModified1969RSOMalayaMeters;
        public readonly ProjectionInfo FD1958Iraq;
        public readonly ProjectionInfo Gulshan303BangladeshTM;
        public readonly ProjectionInfo Hanoi1972GK106NE;
        public readonly ProjectionInfo HongKong1963GridSystem;
        public readonly ProjectionInfo HongKong1980Grid;
        public readonly ProjectionInfo Indian1960TM106NE;
        public readonly ProjectionInfo IsraelTMGrid;
        public readonly ProjectionInfo JordanJTM;
        public readonly ProjectionInfo KandawalaCeylonBeltIndianYards1937;
        public readonly ProjectionInfo KandawalaCeylonBeltMeters;
        public readonly ProjectionInfo Korean1985KoreaCentralBelt;
        public readonly ProjectionInfo Korean1985KoreaEastBelt;
        public readonly ProjectionInfo Korean1985KoreaWestBelt;
        public readonly ProjectionInfo KUDAMSKTM;
        public readonly ProjectionInfo KuwaitOilCoLambert;
        public readonly ProjectionInfo Nahrwan1934IraqZone;
        public readonly ProjectionInfo NepalNagarkotTM;
        public readonly ProjectionInfo ObservatorioMeteorologico1965MacauGrid;
        public readonly ProjectionInfo Palestine1923IsraelCSGrid;
        public readonly ProjectionInfo Palestine1923PalestineBelt;
        public readonly ProjectionInfo Palestine1923PalestineGrid;
        public readonly ProjectionInfo PhilippinesZoneI;
        public readonly ProjectionInfo PhilippinesZoneII;
        public readonly ProjectionInfo PhilippinesZoneIII;
        public readonly ProjectionInfo PhilippinesZoneIV;
        public readonly ProjectionInfo PhilippinesZoneV;
        public readonly ProjectionInfo PRS1992PhilippinesZoneI;
        public readonly ProjectionInfo PRS1992PhilippinesZoneII;
        public readonly ProjectionInfo PRS1992PhilippinesZoneIII;
        public readonly ProjectionInfo PRS1992PhilippinesZoneIV;
        public readonly ProjectionInfo PRS1992PhilippinesZoneV;
        public readonly ProjectionInfo Pulkovo1942CaspianSeaMercator;
        public readonly ProjectionInfo Qatar1948QatarGrid;
        public readonly ProjectionInfo QatarNationalGrid;
        public readonly ProjectionInfo QND1995QatarNationalGrid;
        public readonly ProjectionInfo RassadiranNakhleTaqi;
        public readonly ProjectionInfo TWD1967TMPenghu;
        public readonly ProjectionInfo TWD1967TMTaiwan;
        public readonly ProjectionInfo TWD1997TMPenghu;
        public readonly ProjectionInfo TWD1997TMTaiwan;
        public readonly ProjectionInfo WGS1972BESouthChinaSeaLambert;
        public readonly ProjectionInfo WGS1972TM106NE;
        public readonly ProjectionInfo WGS1984DubaiLocalTM;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia.
        /// </summary>
        public Asia()
        {
            AinelAbd1970AramcoLambert2 = ProjectionInfo.FromAuthorityCode("ESRI", 102204).SetNames("Ain_el_Abd_1970_Aramco_Lambert_2", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970"); // missing
            AinelAbdAramcoLambert = ProjectionInfo.FromEpsgCode(2318).SetNames("Ain_el_Abd_Aramco_Lambert", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            BahrainStateGrid = ProjectionInfo.FromEpsgCode(20499).SetNames("Bahrain_State_Grid", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            DeirezZorLevantStereographic = ProjectionInfo.FromEpsgCode(22780).SetNames("Deir_ez_Zor_Levant_Stereographic", "GCS_Deir_ez_Zor", "D_Deir_ez_Zor");
            DeirezZorLevantZone = ProjectionInfo.FromEpsgCode(22700).SetNames("Deir_ez_Zor_Levant_Zone", "GCS_Deir_ez_Zor", "D_Deir_ez_Zor");
            DeirezZorSyriaLambert = ProjectionInfo.FromEpsgCode(22770).SetNames("Deir_ez_Zor_Syria_Lambert", "GCS_Deir_ez_Zor", "D_Deir_ez_Zor");
            ED1950IraqNationalGrid = ProjectionInfo.FromEpsgCode(3893).SetNames("ED_1950_Iraq_National_Grid", "GCS_European_1950", "D_European_1950");
            ED1950JordanTM = ProjectionInfo.FromEpsgCode(3066).SetNames("ED_1950_Jordan_TM", "GCS_European_1950", "D_European_1950");
            // EverestModified1969RSOMalayaMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102061).SetNames("Everest_Modified_1969_RSO_Malaya_Meters", "GCS_Everest_Modified_1969", "D_Everest_Modified_1969"); // projection not  found
            FD1958Iraq = ProjectionInfo.FromEpsgCode(3200).SetNames("FD_1958_Iraq", "GCS_FD_1958", "D_FD_1958");
            Gulshan303BangladeshTM = ProjectionInfo.FromEpsgCode(3106).SetNames("Gulshan_303_Bangladesh_TM", "GCS_Gulshan_303", "D_Gulshan_303");
            Hanoi1972GK106NE = ProjectionInfo.FromEpsgCode(2093).SetNames("Hanoi_1972_GK_106_NE", "GCS_Hanoi_1972", "D_Hanoi_1972");
            HongKong1963GridSystem = ProjectionInfo.FromEpsgCode(3407).SetNames("Hong_Kong_1963_Grid_System", "GCS_Hong_Kong_1963", "D_Hong_Kong_1963");
            HongKong1980Grid = ProjectionInfo.FromEpsgCode(2326).SetNames("Hong_Kong_1980_Grid", "GCS_Hong_Kong_1980", "D_Hong_Kong_1980");
            Indian1960TM106NE = ProjectionInfo.FromEpsgCode(3176).SetNames("Indian_1960_TM_106NE", "GCS_Indian_1960", "D_Indian_1960");
            IsraelTMGrid = ProjectionInfo.FromEpsgCode(2039).SetNames("Israel_TM_Grid", "GCS_Israel", "D_Israel");
            JordanJTM = ProjectionInfo.FromAuthorityCode("ESRI", 102158).SetNames("Jordan_JTM", "GCS_Jordan", "D_Jordan"); // missing
            KandawalaCeylonBeltIndianYards1937 = ProjectionInfo.FromAuthorityCode("ESRI", 102064).SetNames("Kandawala_Ceylon_Belt_Indian_Yards_1937", "GCS_Kandawala", "D_Kandawala"); // missing
            KandawalaCeylonBeltMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102063).SetNames("Kandawala_Ceylon_Belt_Meters", "GCS_Kandawala", "D_Kandawala"); // missing
            Korean1985KoreaCentralBelt = ProjectionInfo.FromEpsgCode(2097).SetNames("Korean_1985_Korea_Central_Belt", "GCS_Korean_Datum_1985", "D_Korean_Datum_1985");
            Korean1985KoreaEastBelt = ProjectionInfo.FromEpsgCode(2096).SetNames("Korean_1985_Korea_East_Belt", "GCS_Korean_Datum_1985", "D_Korean_Datum_1985");
            Korean1985KoreaWestBelt = ProjectionInfo.FromEpsgCode(2098).SetNames("Korean_1985_Korea_West_Belt", "GCS_Korean_Datum_1985", "D_Korean_Datum_1985");
            KUDAMSKTM = ProjectionInfo.FromEpsgCode(31901).SetNames("KUDAMS_KTM", "GCS_KUDAMS", "D_Kuwait_Utility");
            KuwaitOilCoLambert = ProjectionInfo.FromEpsgCode(24600).SetNames("KOC_Lambert", "GCS_Kuwait_Oil_Company", "D_Kuwait_Oil_Company");
            Nahrwan1934IraqZone = ProjectionInfo.FromEpsgCode(3394).SetNames("Nahrwan_1934_Iraq_Zone", "GCS_Nahrwan_1934", "D_Nahrwan_1934");
            NepalNagarkotTM = ProjectionInfo.FromAuthorityCode("ESRI", 102306).SetNames("Nepal_Nagarkot_TM", "GCS_Nepal_Nagarkot", "D_Nepal_Nagarkot"); // missing
            ObservatorioMeteorologico1965MacauGrid = ProjectionInfo.FromAuthorityCode("ESRI", 102159).SetNames("Observatorio_Meteorologico_1965_Macau_Grid", "GCS_Observatorio_Meteorologico_1965", "D_Observatorio_Meteorologico_1965"); // missing
            Palestine1923IsraelCSGrid = ProjectionInfo.FromEpsgCode(28193).SetNames("Palestine_1923_Israel_CS_Grid", "GCS_Palestine_1923", "D_Palestine_1923");
            Palestine1923PalestineBelt = ProjectionInfo.FromEpsgCode(28192).SetNames("Palestine_1923_Palestine_Belt", "GCS_Palestine_1923", "D_Palestine_1923");
            Palestine1923PalestineGrid = ProjectionInfo.FromEpsgCode(28191).SetNames("Palestine_1923_Palestine_Grid", "GCS_Palestine_1923", "D_Palestine_1923");
            PhilippinesZoneI = ProjectionInfo.FromEpsgCode(25391).SetNames("Philippines_Zone_I", "GCS_Luzon_1911", "D_Luzon_1911");
            PhilippinesZoneII = ProjectionInfo.FromEpsgCode(25392).SetNames("Philippines_Zone_II", "GCS_Luzon_1911", "D_Luzon_1911");
            PhilippinesZoneIII = ProjectionInfo.FromEpsgCode(25393).SetNames("Philippines_Zone_III", "GCS_Luzon_1911", "D_Luzon_1911");
            PhilippinesZoneIV = ProjectionInfo.FromEpsgCode(25394).SetNames("Philippines_Zone_IV", "GCS_Luzon_1911", "D_Luzon_1911");
            PhilippinesZoneV = ProjectionInfo.FromEpsgCode(25395).SetNames("Philippines_Zone_V", "GCS_Luzon_1911", "D_Luzon_1911");
            PRS1992PhilippinesZoneI = ProjectionInfo.FromEpsgCode(3121).SetNames("PRS_1992_Philippines_Zone_I", "GCS_PRS_1992", "D_Philippine_Reference_System_1992");
            PRS1992PhilippinesZoneII = ProjectionInfo.FromEpsgCode(3122).SetNames("PRS_1992_Philippines_Zone_II", "GCS_PRS_1992", "D_Philippine_Reference_System_1992");
            PRS1992PhilippinesZoneIII = ProjectionInfo.FromEpsgCode(3123).SetNames("PRS_1992_Philippines_Zone_III", "GCS_PRS_1992", "D_Philippine_Reference_System_1992");
            PRS1992PhilippinesZoneIV = ProjectionInfo.FromEpsgCode(3124).SetNames("PRS_1992_Philippines_Zone_IV", "GCS_PRS_1992", "D_Philippine_Reference_System_1992");
            PRS1992PhilippinesZoneV = ProjectionInfo.FromEpsgCode(3125).SetNames("PRS_1992_Philippines_Zone_V", "GCS_PRS_1992", "D_Philippine_Reference_System_1992");
            Pulkovo1942CaspianSeaMercator = ProjectionInfo.FromEpsgCode(3388).SetNames("Pulkovo_1942_Caspian_Sea_Mercator", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Qatar1948QatarGrid = ProjectionInfo.FromEpsgCode(2099).SetNames("Qatar_1948_Qatar_Grid", "GCS_Qatar_1948", "D_Qatar_1948");
            QatarNationalGrid = ProjectionInfo.FromEpsgCode(28600).SetNames("Qatar_National_Grid", "GCS_Qatar_1974", "D_Qatar");
            QND1995QatarNationalGrid = ProjectionInfo.FromEpsgCode(2932).SetNames("QND_1995_Qatar_National_Grid", "GCS_QND_1995", "D_QND_1995");
            RassadiranNakhleTaqi = ProjectionInfo.FromEpsgCode(2057).SetNames("Rassadiran_Nakhl_e_Taqi", "GCS_Rassadiran", "D_Rassadiran");
            TWD1967TMPenghu = ProjectionInfo.FromAuthorityCode("EPSG", 102442).SetNames("TWD_1967_TM_Penghu", "GCS_TWD_1967", "D_TWD_1967"); // missing
            TWD1967TMTaiwan = ProjectionInfo.FromAuthorityCode("EPSG", 102441).SetNames("TWD_1967_TM_Taiwan", "GCS_TWD_1967", "D_TWD_1967"); // missing
            TWD1997TMPenghu = ProjectionInfo.FromAuthorityCode("EPSG", 102444).SetNames("TWD_1997_TM_Penghu", "GCS_TWD_1997", "D_TWD_1997"); // missing
            TWD1997TMTaiwan = ProjectionInfo.FromAuthorityCode("EPSG", 102443).SetNames("TWD_1997_TM_Taiwan", "GCS_TWD_1997", "D_TWD_1997"); // missing
            WGS1972BESouthChinaSeaLambert = ProjectionInfo.FromEpsgCode(3415).SetNames("WGS_1972_BE_South_China_Sea_Lambert", "GCS_WGS_1972_BE", "D_WGS_1972_BE");
            WGS1972TM106NE = ProjectionInfo.FromEpsgCode(2094).SetNames("WGS_1972_BE_TM_106_NE", "GCS_WGS_1972_BE", "D_WGS_1972_BE");
            WGS1984DubaiLocalTM = ProjectionInfo.FromEpsgCode(3997).SetNames("WGS_1984_Dubai_Local_TM", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591