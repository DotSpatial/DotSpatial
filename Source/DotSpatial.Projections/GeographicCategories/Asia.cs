// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:04:52 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Asia.
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AinelAbd1970;
        public readonly ProjectionInfo Batavia;
        public readonly ProjectionInfo BataviaJakarta;
        public readonly ProjectionInfo Beijing1954;
        public readonly ProjectionInfo BukitRimpah;
        public readonly ProjectionInfo ChinaGeodeticCoordinateSystem2000;
        public readonly ProjectionInfo DeirezZor;
        public readonly ProjectionInfo DGN1995;
        public readonly ProjectionInfo European1950ED77;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EverestBangladesh;
        public readonly ProjectionInfo EverestIndiaNepal;
        public readonly ProjectionInfo Fahud;
        public readonly ProjectionInfo FD1958;
        public readonly ProjectionInfo GDBD2009;
        public readonly ProjectionInfo GDM2000;
        public readonly ProjectionInfo Gulshan303;
        public readonly ProjectionInfo GunungSegara;
        public readonly ProjectionInfo GunungSegaraJakarta;
        public readonly ProjectionInfo Hanoi1972;
        public readonly ProjectionInfo HeratNorth;
        public readonly ProjectionInfo HongKong1963;
        public readonly ProjectionInfo HongKong196367;
        public readonly ProjectionInfo HongKong1980;
        public readonly ProjectionInfo HuTzuShan;
        public readonly ProjectionInfo IGM1995;
        public readonly ProjectionInfo IGRS;
        public readonly ProjectionInfo IKBD1992;
        public readonly ProjectionInfo Indian1954;
        public readonly ProjectionInfo Indian1960;
        public readonly ProjectionInfo Indian1975;
        public readonly ProjectionInfo IndonesianDatum1974;
        public readonly ProjectionInfo Israel;
        public readonly ProjectionInfo JGD2000;
        public readonly ProjectionInfo Jordan;
        public readonly ProjectionInfo Kalianpur1880;
        public readonly ProjectionInfo Kalianpur1937;
        public readonly ProjectionInfo Kalianpur1962;
        public readonly ProjectionInfo Kalianpur1975;
        public readonly ProjectionInfo Kandawala;
        public readonly ProjectionInfo Karbala1979Polservice;
        public readonly ProjectionInfo Kertau;
        public readonly ProjectionInfo KertauRSO;
        public readonly ProjectionInfo KoreaGeodeticDatum2000;
        public readonly ProjectionInfo KoreanDatum1985;
        public readonly ProjectionInfo KoreanDatum1995;
        public readonly ProjectionInfo KuwaitOilCompany;
        public readonly ProjectionInfo KuwaitUtility;
        public readonly ProjectionInfo Lao1993;
        public readonly ProjectionInfo Lao1997;
        public readonly ProjectionInfo Luzon1911;
        public readonly ProjectionInfo Makassar;
        public readonly ProjectionInfo MakassarJakarta;
        public readonly ProjectionInfo MONREF1997;
        public readonly ProjectionInfo MSK1942;
        public readonly ProjectionInfo Nahrwan1934;
        public readonly ProjectionInfo Nahrwan1967;
        public readonly ProjectionInfo NakhlEGhanem;
        public readonly ProjectionInfo NationalGeodeticNetworkKuwait;
        public readonly ProjectionInfo NepalNagarkot;
        public readonly ProjectionInfo NewBeijing;
        public readonly ProjectionInfo ObservatorioMeteorologico1965;
        public readonly ProjectionInfo Oman;
        public readonly ProjectionInfo Padang1884;
        public readonly ProjectionInfo Padang1884Jakarta;
        public readonly ProjectionInfo Palestine1923;
        public readonly ProjectionInfo PDO1993;
        public readonly ProjectionInfo PRS1992;
        public readonly ProjectionInfo Pulkovo1942;
        public readonly ProjectionInfo Pulkovo1995;
        public readonly ProjectionInfo Qatar1948;
        public readonly ProjectionInfo Qatar1974;
        public readonly ProjectionInfo QND1995;
        public readonly ProjectionInfo Rassadiran;
        public readonly ProjectionInfo Samboja;
        public readonly ProjectionInfo Segora;
        public readonly ProjectionInfo Serindung;
        public readonly ProjectionInfo SouthAsiaSingapore;
        public readonly ProjectionInfo SVY21;
        public readonly ProjectionInfo Taiwan1967;
        public readonly ProjectionInfo Taiwan1997;
        public readonly ProjectionInfo Timbalai1948;
        public readonly ProjectionInfo Tokyo;
        public readonly ProjectionInfo TrucialCoast1948;
        public readonly ProjectionInfo Vientiane1982;
        public readonly ProjectionInfo VN2000;
        public readonly ProjectionInfo Xian1980;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia.
        /// </summary>
        public Asia()
        {
            AinelAbd1970 = ProjectionInfo.FromEpsgCode(4204).SetNames("", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            Batavia = ProjectionInfo.FromEpsgCode(4211).SetNames("", "GCS_Batavia", "D_Batavia");
            BataviaJakarta = ProjectionInfo.FromEpsgCode(4813).SetNames("", "GCS_Batavia_Jakarta", "D_Batavia");
            Beijing1954 = ProjectionInfo.FromEpsgCode(4214).SetNames("", "GCS_Beijing_1954", "D_Beijing_1954");
            BukitRimpah = ProjectionInfo.FromEpsgCode(4219).SetNames("", "GCS_Bukit_Rimpah", "D_Bukit_Rimpah");
            ChinaGeodeticCoordinateSystem2000 = ProjectionInfo.FromEpsgCode(4490).SetNames("", "GCS_China_Geodetic_Coordinate_System_2000", "D_China_2000");
            DeirezZor = ProjectionInfo.FromEpsgCode(4227).SetNames("", "GCS_Deir_ez_Zor", "D_Deir_ez_Zor");
            DGN1995 = ProjectionInfo.FromEpsgCode(4755).SetNames("", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            European1950ED77 = ProjectionInfo.FromEpsgCode(4154).SetNames("", "GCS_European_1950_ED77", "D_European_1950_ED77");
            EuropeanDatum1950 = ProjectionInfo.FromEpsgCode(4230).SetNames("", "GCS_European_1950", "D_European_1950");
            EverestBangladesh = ProjectionInfo.FromAuthorityCode("ESRI", 37202).SetNames("", "GCS_Everest_Bangladesh", "D_Everest_Bangladesh");
            EverestIndiaNepal = ProjectionInfo.FromAuthorityCode("ESRI", 37203).SetNames("", "GCS_Everest_India_Nepal", "D_Everest_India_Nepal");
            Fahud = ProjectionInfo.FromEpsgCode(4232).SetNames("", "GCS_Fahud", "D_Fahud");
            FD1958 = ProjectionInfo.FromEpsgCode(4132).SetNames("", "GCS_FD_1958", "D_FD_1958");
            GDBD2009 = ProjectionInfo.FromAuthorityCode("EPSG", 104100).SetNames("", "GCS_GDBD2009", "D_GDBD2009"); // missing
            GDM2000 = ProjectionInfo.FromEpsgCode(4742).SetNames("", "GCS_GDM_2000", "D_GDM_2000");
            Gulshan303 = ProjectionInfo.FromEpsgCode(4682).SetNames("", "GCS_Gulshan_303", "D_Gulshan_303");
            GunungSegara = ProjectionInfo.FromEpsgCode(4613).SetNames("", "GCS_Gunung_Segara", "D_Gunung_Segara");
            GunungSegaraJakarta = ProjectionInfo.FromEpsgCode(4820).SetNames("", "GCS_Gunung_Segara_Jakarta", "D_Gunung_Segara");
            Hanoi1972 = ProjectionInfo.FromEpsgCode(4147).SetNames("", "GCS_Hanoi_1972", "D_Hanoi_1972");
            HeratNorth = ProjectionInfo.FromEpsgCode(4255).SetNames("", "GCS_Herat_North", "D_Herat_North");
            HongKong1963 = ProjectionInfo.FromEpsgCode(4738).SetNames("", "GCS_Hong_Kong_1963", "D_Hong_Kong_1963");
            HongKong196367 = ProjectionInfo.FromEpsgCode(4739).SetNames("", "GCS_Hong_Kong_1963_67", "D_Hong_Kong_1963_67");
            HongKong1980 = ProjectionInfo.FromEpsgCode(4611).SetNames("", "GCS_Hong_Kong_1980", "D_Hong_Kong_1980");
            HuTzuShan = ProjectionInfo.FromEpsgCode(4236).SetNames("", "GCS_Hu_Tzu_Shan", "D_Hu_Tzu_Shan");
            IGM1995 = ProjectionInfo.FromEpsgCode(4670).SetNames("", "GCS_IGM_1995", "D_IGM_1995");
            IGRS = ProjectionInfo.FromAuthorityCode("EPSG", 104991).SetNames("", "GCS_IGRS", "D_Iraqi_Geospatial_Reference_System"); // missing
            IKBD1992 = ProjectionInfo.FromEpsgCode(4667).SetNames("", "GCS_IKBD_1992", "D_Iraq_Kuwait_Boundary_1992");
            Indian1954 = ProjectionInfo.FromEpsgCode(4239).SetNames("", "GCS_Indian_1954", "D_Indian_1954");
            Indian1960 = ProjectionInfo.FromEpsgCode(4131).SetNames("", "GCS_Indian_1960", "D_Indian_1960");
            Indian1975 = ProjectionInfo.FromEpsgCode(4240).SetNames("", "GCS_Indian_1975", "D_Indian_1975");
            IndonesianDatum1974 = ProjectionInfo.FromEpsgCode(4238).SetNames("", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Israel = ProjectionInfo.FromEpsgCode(4141).SetNames("", "GCS_Israel", "D_Israel");
            JGD2000 = ProjectionInfo.FromEpsgCode(4612).SetNames("", "GCS_JGD_2000", "D_JGD_2000");
            Jordan = ProjectionInfo.FromAuthorityCode("ESRI", 104130).SetNames("", "GCS_Jordan", "D_Jordan"); // missing
            Kalianpur1880 = ProjectionInfo.FromEpsgCode(4243).SetNames("", "GCS_Kalianpur_1880", "D_Kalianpur_1880");
            Kalianpur1937 = ProjectionInfo.FromEpsgCode(4144).SetNames("", "GCS_Kalianpur_1937", "D_Kalianpur_1937");
            Kalianpur1962 = ProjectionInfo.FromEpsgCode(4145).SetNames("", "GCS_Kalianpur_1962", "D_Kalianpur_1962");
            Kalianpur1975 = ProjectionInfo.FromEpsgCode(4146).SetNames("", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kandawala = ProjectionInfo.FromEpsgCode(4244).SetNames("", "GCS_Kandawala", "D_Kandawala");
            Karbala1979Polservice = ProjectionInfo.FromEpsgCode(4743).SetNames("", "GCS_Karbala_1979_Polservice", "D_Karbala_1979_Polservice");
            Kertau = ProjectionInfo.FromEpsgCode(4245).SetNames("", "GCS_Kertau", "D_Kertau");
            KertauRSO = ProjectionInfo.FromEpsgCode(4751).SetNames("", "GCS_Kertau_RSO", "D_Kertau_RSO");
            KoreaGeodeticDatum2000 = ProjectionInfo.FromEpsgCode(4737).SetNames("", "GCS_Korea_2000", "D_Korea_2000");
            KoreanDatum1985 = ProjectionInfo.FromEpsgCode(4162).SetNames("", "GCS_Korean_Datum_1985", "D_Korean_Datum_1985");
            KoreanDatum1995 = ProjectionInfo.FromEpsgCode(4166).SetNames("", "GCS_Korean_Datum_1995", "D_Korean_Datum_1995");
            KuwaitOilCompany = ProjectionInfo.FromEpsgCode(4246).SetNames("", "GCS_Kuwait_Oil_Company", "D_Kuwait_Oil_Company");
            KuwaitUtility = ProjectionInfo.FromEpsgCode(4319).SetNames("", "GCS_KUDAMS", "D_Kuwait_Utility");
            Lao1993 = ProjectionInfo.FromEpsgCode(4677).SetNames("", "GCS_Lao_1993", "D_Lao_1993");
            Lao1997 = ProjectionInfo.FromEpsgCode(4678).SetNames("", "GCS_Lao_1997", "D_Lao_National_Datum_1997");
            Luzon1911 = ProjectionInfo.FromEpsgCode(4253).SetNames("", "GCS_Luzon_1911", "D_Luzon_1911");
            Makassar = ProjectionInfo.FromEpsgCode(4257).SetNames("", "GCS_Makassar", "D_Makassar");
            MakassarJakarta = ProjectionInfo.FromEpsgCode(4804).SetNames("", "GCS_Makassar_Jakarta", "D_Makassar");
            MONREF1997 = ProjectionInfo.FromAuthorityCode("ESRI", 104134).SetNames("", "GCS_MONREF_1997", "D_ITRF_2000"); // missing
            MSK1942 = ProjectionInfo.FromAuthorityCode("ESRI", 104135).SetNames("", "GCS_MSK_1942", "D_Pulkovo_1942"); // missing
            Nahrwan1934 = ProjectionInfo.FromEpsgCode(4744).SetNames("", "GCS_Nahrwan_1934", "D_Nahrwan_1934");
            Nahrwan1967 = ProjectionInfo.FromEpsgCode(4270).SetNames("", "GCS_Nahrwan_1967", "D_Nahrwan_1967");
            NakhlEGhanem = ProjectionInfo.FromEpsgCode(4693).SetNames("", "GCS_Nakhl-e_Ghanem", "D_Nakhl-e_Ghanem");
            NationalGeodeticNetworkKuwait = ProjectionInfo.FromEpsgCode(4318).SetNames("", "GCS_NGN", "D_NGN");
            NepalNagarkot = ProjectionInfo.FromAuthorityCode("EPSG", 104256).SetNames("", "GCS_Nepal_Nagarkot", "D_Nepal_Nagarkot"); // missing
            NewBeijing = ProjectionInfo.FromEpsgCode(4555).SetNames("", "GCS_New_Beijing", "D_New_Beijing");
            ObservatorioMeteorologico1965 = ProjectionInfo.FromAuthorityCode("ESRI", 104126).SetNames("", "GCS_Observatorio_Meteorologico_1965", "D_Observatorio_Meteorologico_1965"); // missing
            Oman = ProjectionInfo.FromAuthorityCode("ESRI", 37206).SetNames("", "GCS_Oman", "D_Oman");
            Padang1884 = ProjectionInfo.FromEpsgCode(4280).SetNames("", "GCS_Padang_1884", "D_Padang_1884");
            Padang1884Jakarta = ProjectionInfo.FromEpsgCode(4808).SetNames("", "GCS_Padang_1884_Jakarta", "D_Padang_1884");
            Palestine1923 = ProjectionInfo.FromEpsgCode(4281).SetNames("", "GCS_Palestine_1923", "D_Palestine_1923");
            PDO1993 = ProjectionInfo.FromEpsgCode(4134).SetNames("", "GCS_PDO_1993", "D_PDO_1993");
            PRS1992 = ProjectionInfo.FromEpsgCode(4683).SetNames("", "GCS_PRS_1992", "D_Philippine_Reference_System_1992");
            Pulkovo1942 = ProjectionInfo.FromEpsgCode(4284).SetNames("", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1995 = ProjectionInfo.FromEpsgCode(4200).SetNames("", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            Qatar1948 = ProjectionInfo.FromEpsgCode(4286).SetNames("", "GCS_Qatar_1948", "D_Qatar_1948");
            Qatar1974 = ProjectionInfo.FromEpsgCode(4285).SetNames("", "GCS_Qatar_1974", "D_Qatar");
            QND1995 = ProjectionInfo.FromEpsgCode(4614).SetNames("", "GCS_QND_1995", "D_QND_1995");
            Rassadiran = ProjectionInfo.FromEpsgCode(4153).SetNames("", "GCS_Rassadiran", "D_Rassadiran");
            Samboja = ProjectionInfo.FromEpsgCode(4125).SetNames("", "GCS_Samboja", "D_Samboja");
            Segora = ProjectionInfo.FromEpsgCode(4294).SetNames("", "GCS_Segora", "D_Segora");
            Serindung = ProjectionInfo.FromEpsgCode(4295).SetNames("", "GCS_Serindung", "D_Serindung");
            SouthAsiaSingapore = ProjectionInfo.FromAuthorityCode("ESRI", 37207).SetNames("", "GCS_South_Asia_Singapore", "D_South_Asia_Singapore");
            SVY21 = ProjectionInfo.FromEpsgCode(4757).SetNames("", "GCS_SVY21", "D_SVY21");
            Taiwan1967 = ProjectionInfo.FromAuthorityCode("EPSG", 104136).SetNames("", "GCS_TWD_1967", "D_TWD_1967"); // missing
            Taiwan1997 = ProjectionInfo.FromAuthorityCode("EPSG", 104137).SetNames("", "GCS_TWD_1997", "D_TWD_1997"); // missing
            Timbalai1948 = ProjectionInfo.FromEpsgCode(4298).SetNames("", "GCS_Timbalai_1948", "D_Timbalai_1948");
            Tokyo = ProjectionInfo.FromEpsgCode(4301).SetNames("", "GCS_Tokyo", "D_Tokyo");
            TrucialCoast1948 = ProjectionInfo.FromEpsgCode(4303).SetNames("", "GCS_Trucial_Coast_1948", "D_Trucial_Coast_1948");
            Vientiane1982 = ProjectionInfo.FromEpsgCode(4676).SetNames("", "GCS_Vientiane_1982", "D_Vientiane_1982");
            VN2000 = ProjectionInfo.FromEpsgCode(4756).SetNames("", "GCS_VN_2000", "D_Vietnam_2000");
            Xian1980 = ProjectionInfo.FromEpsgCode(4610).SetNames("", "GCS_Xian_1980", "D_Xian_1980");
        }

        #endregion
    }
}

#pragma warning restore 1591