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
    /// Asia
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Fields

        public readonly ProjectionInfo AinelAbd1970;
        public readonly ProjectionInfo Batavia;
        public readonly ProjectionInfo BataviaJakarta;
        public readonly ProjectionInfo Beijing1954;
        public readonly ProjectionInfo BukitRimpah;
        public readonly ProjectionInfo DeirezZor;
        public readonly ProjectionInfo European1950ED77;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo Everest1830;
        public readonly ProjectionInfo EverestBangladesh;
        public readonly ProjectionInfo EverestIndiaandNepal;
        public readonly ProjectionInfo EverestModified;
        public readonly ProjectionInfo Everestdef1962;
        public readonly ProjectionInfo Everestdef1967;
        public readonly ProjectionInfo Everestdef1975;
        public readonly ProjectionInfo FD1958;
        public readonly ProjectionInfo Fahud;
        public readonly ProjectionInfo Gandajika1970;
        public readonly ProjectionInfo GunungSegara;
        public readonly ProjectionInfo GunungSegaraJakarta;
        public readonly ProjectionInfo Hanoi1972;
        public readonly ProjectionInfo HeratNorth;
        public readonly ProjectionInfo HongKong1963;
        public readonly ProjectionInfo HongKong1980;
        public readonly ProjectionInfo HuTzuShan;
        public readonly ProjectionInfo IGM1995;
        public readonly ProjectionInfo IKBD1992;
        public readonly ProjectionInfo Indian1954;
        public readonly ProjectionInfo Indian1960;
        public readonly ProjectionInfo Indian1975;
        public readonly ProjectionInfo IndonesianDatum1974;
        public readonly ProjectionInfo Israel;
        public readonly ProjectionInfo JGD2000;
        public readonly ProjectionInfo JGD2011;
        public readonly ProjectionInfo Jordan;
        public readonly ProjectionInfo Kalianpur1880;
        public readonly ProjectionInfo Kalianpur1937;
        public readonly ProjectionInfo Kalianpur1962;
        public readonly ProjectionInfo Kalianpur1975;
        public readonly ProjectionInfo Kandawala;
        public readonly ProjectionInfo Kertau;
        public readonly ProjectionInfo KoreanDatum1985;
        public readonly ProjectionInfo KoreanDatum1995;
        public readonly ProjectionInfo KuwaitOilCompany;
        public readonly ProjectionInfo KuwaitUtility;
        public readonly ProjectionInfo Luzon1911;
        public readonly ProjectionInfo Makassar;
        public readonly ProjectionInfo MakassarJakarta;
        public readonly ProjectionInfo Nahrwan1967;
        public readonly ProjectionInfo NationalGeodeticNetworkKuwait;
        public readonly ProjectionInfo ObservatorioMeteorologico1965;
        public readonly ProjectionInfo Oman;
        public readonly ProjectionInfo Padang1884;
        public readonly ProjectionInfo Padang1884Jakarta;
        public readonly ProjectionInfo Palestine1923;
        public readonly ProjectionInfo Pulkovo1942;
        public readonly ProjectionInfo Pulkovo1995;
        public readonly ProjectionInfo QND1995;
        public readonly ProjectionInfo Qatar;
        public readonly ProjectionInfo Qatar1948;
        public readonly ProjectionInfo Rassadiran;
        public readonly ProjectionInfo Samboja;
        public readonly ProjectionInfo Segora;
        public readonly ProjectionInfo Serindung;
        public readonly ProjectionInfo SouthAsiaSingapore;
        public readonly ProjectionInfo Timbalai1948;
        public readonly ProjectionInfo Tokyo;
        public readonly ProjectionInfo TrucialCoast1948;
        public readonly ProjectionInfo Xian1980;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia
        /// </summary>
        public Asia()
        {
            AinelAbd1970 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Batavia = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            BataviaJakarta = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Beijing1954 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            BukitRimpah = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            DeirezZor = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            European1950ED77 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1950 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            EverestBangladesh = ProjectionInfo.FromProj4String("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            EverestIndiaandNepal = ProjectionInfo.FromProj4String("+proj=longlat +a=6377301.243 +b=6356100.230165384 +no_defs ");
            Everestdef1962 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377301.243 +b=6356100.230165384 +no_defs ");
            Everestdef1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=evrstSS +no_defs ");
            Everestdef1975 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377299.151 +b=6356098.145120132 +no_defs ");
            Everest1830 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377299.36 +b=6356098.35162804 +no_defs ");
            EverestModified = ProjectionInfo.FromProj4String("+proj=longlat +a=6377304.063 +b=6356103.041812424 +no_defs ");
            Fahud = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            FD1958 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Gandajika1970 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            GunungSegara = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            GunungSegaraJakarta = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Hanoi1972 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            HeratNorth = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            HongKong1963 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            HongKong1980 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            HuTzuShan = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            IGM1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            IKBD1992 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            Indian1954 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Indian1960 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Indian1975 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            IndonesianDatum1974 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378160 +b=6356774.50408554 +no_defs ");
            Israel = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            JGD2000 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            JGD2011 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Jordan = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Kalianpur1880 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377299.36 +b=6356098.35162804 +no_defs ");
            Kalianpur1937 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Kalianpur1962 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377301.243 +b=6356100.230165384 +no_defs ");
            Kalianpur1975 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377299.151 +b=6356098.145120132 +no_defs ");
            Kandawala = ProjectionInfo.FromProj4String("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Kertau = ProjectionInfo.FromProj4String("+proj=longlat +a=6377304.063 +b=6356103.038993155 +no_defs ");
            KoreanDatum1985 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            KoreanDatum1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            KuwaitOilCompany = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            KuwaitUtility = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Luzon1911 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Makassar = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            MakassarJakarta = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Nahrwan1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            NationalGeodeticNetworkKuwait = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            ObservatorioMeteorologico1965 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Oman = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Padang1884 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Padang1884Jakarta = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Palestine1923 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378300.79 +b=6356566.430000036 +no_defs ");
            Pulkovo1942 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Qatar = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Qatar1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=helmert +no_defs ");
            QND1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Rassadiran = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Samboja = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Segora = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Serindung = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            SouthAsiaSingapore = ProjectionInfo.FromProj4String("+proj=longlat +ellps=fschr60m +no_defs ");
            Timbalai1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=evrstSS +no_defs ");
            Tokyo = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            TrucialCoast1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=helmert +no_defs ");
            Xian1980 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378140 +b=6356755.288157528 +no_defs ");

            AinelAbd1970.Name = "GCS_Ain_el_Abd_1970";
            AinelAbd1970.GeographicInfo.Name = "GCS_Ain_el_Abd_1970";
            Batavia.Name = "GCS_Batavia";
            Batavia.GeographicInfo.Name = "GCS_Batavia";
            BataviaJakarta.Name = "GCS_Batavia_Jakarta";
            BataviaJakarta.GeographicInfo.Name = "GCS_Batavia_Jakarta";
            Beijing1954.Name = "GCS_Beijing_1954";
            Beijing1954.GeographicInfo.Name = "GCS_Beijing_1954";
            BukitRimpah.Name = "GCS_Bukit_Rimpah";
            BukitRimpah.GeographicInfo.Name = "GCS_Bukit_Rimpah";
            DeirezZor.Name = "GCS_Deir_ez_Zor";
            DeirezZor.GeographicInfo.Name = "GCS_Deir_ez_Zor";
            European1950ED77.Name = "GCS_European_1950_ED77";
            European1950ED77.GeographicInfo.Name = "GCS_European_1950_ED77";
            EuropeanDatum1950.Name = "GCS_European_1950";
            EuropeanDatum1950.GeographicInfo.Name = "GCS_European_1950";
            EverestBangladesh.Name = "GCS_Everest_Bangladesh";
            EverestBangladesh.GeographicInfo.Name = "GCS_Everest_Bangladesh";
            EverestIndiaandNepal.Name = "GCS_Everest_India_Nepal";
            EverestIndiaandNepal.GeographicInfo.Name = "GCS_Everest_India_Nepal";
            Everestdef1962.Name = "GCS_Everest_def_1962";
            Everestdef1962.GeographicInfo.Name = "GCS_Everest_def_1962";
            Everestdef1967.Name = "GCS_Everest_def_1967";
            Everestdef1967.GeographicInfo.Name = "GCS_Everest_def_1967";
            Everestdef1975.Name = "GCS_Everest_def_1975";
            Everestdef1975.GeographicInfo.Name = "GCS_Everest_def_1975";
            Everest1830.Name = "GCS_Everest_1830";
            Everest1830.GeographicInfo.Name = "GCS_Everest_1830";
            EverestModified.Name = "GCS_Everest_Modified";
            EverestModified.GeographicInfo.Name = "GCS_Everest_Modified";
            Fahud.Name = "GCS_Fahud";
            Fahud.GeographicInfo.Name = "GCS_Fahud";
            FD1958.Name = "GCS_FD_1958";
            FD1958.GeographicInfo.Name = "GCS_FD_1958";
            Gandajika1970.Name = "GCS_Gandajika_1970";
            Gandajika1970.GeographicInfo.Name = "GCS_Gandajika_1970";
            GunungSegara.Name = "GCS_Gunung_Segara";
            GunungSegara.GeographicInfo.Name = "GCS_Gunung_Segara";
            GunungSegaraJakarta.Name = "GCS_Gunung_Segara_Jakarta";
            GunungSegaraJakarta.GeographicInfo.Name = "GCS_Gunung_Segara_Jakarta";
            Hanoi1972.Name = "GCS_Hanoi_1972";
            Hanoi1972.GeographicInfo.Name = "GCS_Hanoi_1972";
            HeratNorth.Name = "GCS_Herat_North";
            HeratNorth.GeographicInfo.Name = "GCS_Herat_North";
            HongKong1963.Name = "GCS_Hong_Kong_1963";
            HongKong1963.GeographicInfo.Name = "GCS_Hong_Kong_1963";
            HongKong1980.Name = "GCS_Hong_Kong_1980";
            HongKong1980.GeographicInfo.Name = "GCS_Hong_Kong_1980";
            HuTzuShan.Name = "GCS_Hu_Tzu_Shan";
            HuTzuShan.GeographicInfo.Name = "GCS_Hu_Tzu_Shan";
            IGM1995.Name = "GCS_IGM_1995";
            IGM1995.GeographicInfo.Name = "GCS_IGM_1995";
            IKBD1992.Name = "GCS_IKBD_1992";
            IKBD1992.GeographicInfo.Name = "GCS_IKBD_1992";
            Indian1954.Name = "GCS_Indian_1954";
            Indian1954.GeographicInfo.Name = "GCS_Indian_1954";
            Indian1960.Name = "GCS_Indian_1960";
            Indian1960.GeographicInfo.Name = "GCS_Indian_1960";
            Indian1975.Name = "GCS_Indian_1975";
            Indian1975.GeographicInfo.Name = "GCS_Indian_1975";
            IndonesianDatum1974.Name = "GCS_Indonesian_1974";
            IndonesianDatum1974.GeographicInfo.Name = "GCS_Indonesian_1974";
            Israel.Name = "GCS_Israel";
            Israel.GeographicInfo.Name = "GCS_Israel";
            JGD2000.Name = "GCS_JGD_2000";
            JGD2000.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2011.Name = "GCS_JGD_2011";
            JGD2011.GeographicInfo.Name = "GCS_JGD_2011";
            Jordan.Name = "GCS_Jordan";
            Jordan.GeographicInfo.Name = "GCS_Jordan";
            Kalianpur1880.Name = "GCS_Kalianpur_1880";
            Kalianpur1880.GeographicInfo.Name = "GCS_Kalianpur_1880";
            Kalianpur1937.Name = "GCS_Kalianpur_1937";
            Kalianpur1937.GeographicInfo.Name = "GCS_Kalianpur_1937";
            Kalianpur1962.Name = "GCS_Kalianpur_1962";
            Kalianpur1962.GeographicInfo.Name = "GCS_Kalianpur_1962";
            Kalianpur1975.Name = "GCS_Kalianpur_1975";
            Kalianpur1975.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kandawala.Name = "GCS_Kandawala";
            Kandawala.GeographicInfo.Name = "GCS_Kandawala";
            Kertau.Name = "GCS_Kertau";
            Kertau.GeographicInfo.Name = "GCS_Kertau";
            KoreanDatum1985.Name = "GCS_Korean_Datum_1985";
            KoreanDatum1985.GeographicInfo.Name = "GCS_Korean_Datum_1985";
            KoreanDatum1995.Name = "GCS_Korean_Datum_1995";
            KoreanDatum1995.GeographicInfo.Name = "GCS_Korean_Datum_1995";
            KuwaitOilCompany.Name = "GCS_Kuwait_Oil_Company";
            KuwaitOilCompany.GeographicInfo.Name = "GCS_Kuwait_Oil_Company";
            KuwaitUtility.Name = "GCS_KUDAMS";
            KuwaitUtility.GeographicInfo.Name = "GCS_KUDAMS";
            Luzon1911.Name = "GCS_Luzon_1911";
            Luzon1911.GeographicInfo.Name = "GCS_Luzon_1911";
            Makassar.Name = "GCS_Makassar";
            Makassar.GeographicInfo.Name = "GCS_Makassar";
            MakassarJakarta.Name = "GCS_Makassar_Jakarta";
            MakassarJakarta.GeographicInfo.Name = "GCS_Makassar_Jakarta";
            Nahrwan1967.Name = "GCS_Nahrwan_1967";
            Nahrwan1967.GeographicInfo.Name = "GCS_Nahrwan_1967";
            NationalGeodeticNetworkKuwait.Name = "GCS_NGN";
            NationalGeodeticNetworkKuwait.GeographicInfo.Name = "GCS_NGN";
            ObservatorioMeteorologico1965.Name = "GCS_Observatorio_Meteorologico_1965";
            ObservatorioMeteorologico1965.GeographicInfo.Name = "GCS_Observatorio_Meteorologico_1965";
            Oman.Name = "GCS_Oman";
            Oman.GeographicInfo.Name = "GCS_Oman";
            Padang1884.Name = "GCS_Padang_1884";
            Padang1884.GeographicInfo.Name = "GCS_Padang_1884";
            Padang1884Jakarta.Name = "GCS_Padang_1884_Jakarta";
            Padang1884Jakarta.GeographicInfo.Name = "GCS_Padang_1884_Jakarta";
            Palestine1923.Name = "GCS_Palestine_1923";
            Palestine1923.GeographicInfo.Name = "GCS_Palestine_1923";
            Pulkovo1942.Name = "GCS_Pulkovo_1942";
            Pulkovo1942.GeographicInfo.Name = "GCS_Pulkovo_1942";
            Pulkovo1995.Name = "GCS_Pulkovo_1995";
            Pulkovo1995.GeographicInfo.Name = "GCS_Pulkovo_1995";
            Qatar.Name = "GCS_Qatar";
            Qatar.GeographicInfo.Name = "GCS_Qatar";
            Qatar1948.Name = "GCS_Qatar_1948";
            Qatar1948.GeographicInfo.Name = "GCS_Qatar_1948";
            QND1995.Name = "GCS_QND_1995";
            QND1995.GeographicInfo.Name = "GCS_QND_1995";
            Rassadiran.Name = "GCS_Rassadiran";
            Rassadiran.GeographicInfo.Name = "GCS_Rassadiran";
            Samboja.Name = "GCS_Samboja";
            Samboja.GeographicInfo.Name = "GCS_Samboja";
            Segora.Name = "GCS_Segora";
            Segora.GeographicInfo.Name = "GCS_Segora";
            Serindung.Name = "GCS_Serindung";
            Serindung.GeographicInfo.Name = "GCS_Serindung";
            SouthAsiaSingapore.Name = "GCS_South_Asia_Singapore";
            SouthAsiaSingapore.GeographicInfo.Name = "GCS_South_Asia_Singapore";
            Timbalai1948.Name = "GCS_Timbalai_1948";
            Timbalai1948.GeographicInfo.Name = "GCS_Timbalai_1948";
            Tokyo.Name = "GCS_Tokyo";
            Tokyo.GeographicInfo.Name = "GCS_Tokyo";
            TrucialCoast1948.Name = "GCS_Trucial_Coast_1948";
            TrucialCoast1948.GeographicInfo.Name = "GCS_Trucial_Coast_1948";
            Xian1980.Name = "GCS_Xian_1980";
            Xian1980.GeographicInfo.Name = "GCS_Xian_1980";

            AinelAbd1970.GeographicInfo.Datum.Name = "D_Ain_el_Abd_1970";
            Batavia.GeographicInfo.Datum.Name = "D_Batavia";
            BataviaJakarta.GeographicInfo.Datum.Name = "D_Batavia";
            Beijing1954.GeographicInfo.Datum.Name = "D_Beijing_1954";
            BukitRimpah.GeographicInfo.Datum.Name = "D_Bukit_Rimpah";
            DeirezZor.GeographicInfo.Datum.Name = "D_Deir_ez_Zor";
            European1950ED77.GeographicInfo.Datum.Name = "D_European_1950_ED77";
            EuropeanDatum1950.GeographicInfo.Datum.Name = "D_European_1950";
            EverestBangladesh.GeographicInfo.Datum.Name = "D_Everest_Bangladesh";
            EverestIndiaandNepal.GeographicInfo.Datum.Name = "D_Everest_India_Nepal";
            Everestdef1962.GeographicInfo.Datum.Name = "D_Everest_Def_1962";
            Everestdef1967.GeographicInfo.Datum.Name = "D_Everest_Def_1967";
            Everestdef1975.GeographicInfo.Datum.Name = "D_Everest_Def_1975";
            Everest1830.GeographicInfo.Datum.Name = "D_Everest_1830";
            EverestModified.GeographicInfo.Datum.Name = "D_Everest_Modified";
            Fahud.GeographicInfo.Datum.Name = "D_Fahud";
            FD1958.GeographicInfo.Datum.Name = "D_FD_1958";
            Gandajika1970.GeographicInfo.Datum.Name = "D_Gandajika_1970";
            GunungSegara.GeographicInfo.Datum.Name = "D_Gunung_Segara";
            GunungSegaraJakarta.GeographicInfo.Datum.Name = "D_Gunung_Segara";
            Hanoi1972.GeographicInfo.Datum.Name = "D_Hanoi_1972";
            HeratNorth.GeographicInfo.Datum.Name = "D_Herat_North";
            HongKong1963.GeographicInfo.Datum.Name = "D_Hong_Kong_1963";
            HongKong1980.GeographicInfo.Datum.Name = "D_Hong_Kong_1980";
            HuTzuShan.GeographicInfo.Datum.Name = "D_Hu_Tzu_Shan";
            IGM1995.GeographicInfo.Datum.Name = "D_IGM_1995";
            IKBD1992.GeographicInfo.Datum.Name = "D_Iraq_Kuwait_Boundary_1992";
            Indian1954.GeographicInfo.Datum.Name = "D_Indian_1954";
            Indian1960.GeographicInfo.Datum.Name = "D_Indian_1960";
            Indian1975.GeographicInfo.Datum.Name = "D_Indian_1975";
            IndonesianDatum1974.GeographicInfo.Datum.Name = "D_Indonesian_1974";
            Israel.GeographicInfo.Datum.Name = "D_Israel";
            JGD2000.GeographicInfo.Datum.Name = "D_JGD_2000";
            Jordan.GeographicInfo.Datum.Name = "D_Jordan";
            Kalianpur1880.GeographicInfo.Datum.Name = "D_Kalianpur_1880";
            Kalianpur1937.GeographicInfo.Datum.Name = "D_Kalianpur_1937";
            Kalianpur1962.GeographicInfo.Datum.Name = "D_Kalianpur_1962";
            Kalianpur1975.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kandawala.GeographicInfo.Datum.Name = "D_Kandawala";
            Kertau.GeographicInfo.Datum.Name = "D_Kertau";
            KoreanDatum1985.GeographicInfo.Datum.Name = "D_Korean_Datum_1985";
            KoreanDatum1995.GeographicInfo.Datum.Name = "D_Korean_Datum_1995";
            KuwaitOilCompany.GeographicInfo.Datum.Name = "D_Kuwait_Oil_Company";
            KuwaitUtility.GeographicInfo.Datum.Name = "D_Kuwait_Utility";
            Luzon1911.GeographicInfo.Datum.Name = "D_Luzon_1911";
            Makassar.GeographicInfo.Datum.Name = "D_Makassar";
            MakassarJakarta.GeographicInfo.Datum.Name = "D_Makassar";
            Nahrwan1967.GeographicInfo.Datum.Name = "D_Nahrwan_1967";
            NationalGeodeticNetworkKuwait.GeographicInfo.Datum.Name = "D_NGN";
            ObservatorioMeteorologico1965.GeographicInfo.Datum.Name = "D_Observatorio_Meteorologico_1965";
            Oman.GeographicInfo.Datum.Name = "D_Oman";
            Padang1884.GeographicInfo.Datum.Name = "D_Padang_1884";
            Padang1884Jakarta.GeographicInfo.Datum.Name = "D_Padang_1884";
            Palestine1923.GeographicInfo.Datum.Name = "D_Palestine_1923";
            Pulkovo1942.GeographicInfo.Datum.Name = "D_Pulkovo_1942";
            Pulkovo1995.GeographicInfo.Datum.Name = "D_Pulkovo_1995";
            Qatar.GeographicInfo.Datum.Name = "D_Qatar";
            Qatar1948.GeographicInfo.Datum.Name = "D_Qatar_1948";
            QND1995.GeographicInfo.Datum.Name = "D_QND_1995";
            Rassadiran.GeographicInfo.Datum.Name = "D_Rassadiran";
            Samboja.GeographicInfo.Datum.Name = "D_Samboja";
            Segora.GeographicInfo.Datum.Name = "D_Segora";
            Serindung.GeographicInfo.Datum.Name = "D_Serindung";
            SouthAsiaSingapore.GeographicInfo.Datum.Name = "D_South_Asia_Singapore";
            Timbalai1948.GeographicInfo.Datum.Name = "D_Timbalai_1948";
            Tokyo.GeographicInfo.Datum.Name = "D_Tokyo";
            TrucialCoast1948.GeographicInfo.Datum.Name = "D_Trucial_Coast_1948";
            Xian1980.GeographicInfo.Datum.Name = "D_Xian_1980";
        }

        #endregion
    }
}

#pragma warning restore 1591