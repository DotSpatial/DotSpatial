// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 3:22:06 PM
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
    /// Africa
    /// </summary>
    public class Africa : CoordinateSystemCategory
    {
        #region Private Variables

        /// <summary>
        ///Abidjan 1987
        /// </summary>
        public readonly ProjectionInfo Abidjan1987;
        public readonly ProjectionInfo Accra;
        public readonly ProjectionInfo Adindan;
        public readonly ProjectionInfo Afgooye;
        public readonly ProjectionInfo Agadez;
        public readonly ProjectionInfo AinelAbd1970;
        public readonly ProjectionInfo Arc1950;
        public readonly ProjectionInfo Arc1960;
        public readonly ProjectionInfo AyabelleLighthouse;
        public readonly ProjectionInfo Beduaram;
        public readonly ProjectionInfo Bissau;
        public readonly ProjectionInfo Camacupa;
        public readonly ProjectionInfo Cape;
        public readonly ProjectionInfo Carthage;
        public readonly ProjectionInfo CarthageParis;
        public readonly ProjectionInfo Carthagedegrees;
        public readonly ProjectionInfo Conakry1905;
        public readonly ProjectionInfo CotedIvoire;
        public readonly ProjectionInfo Dabola;
        public readonly ProjectionInfo Douala;
        public readonly ProjectionInfo Douala1948;
        public readonly ProjectionInfo Egypt1907;
        public readonly ProjectionInfo Egypt1930;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EuropeanLibyanDatum1979;
        public readonly ProjectionInfo Garoua;
        public readonly ProjectionInfo Hartebeesthoek1994;
        public readonly ProjectionInfo Kousseri;
        public readonly ProjectionInfo KuwaitOilCompany;
        public readonly ProjectionInfo KuwaitUtility;
        public readonly ProjectionInfo Leigon;
        public readonly ProjectionInfo Liberia1964;
        public readonly ProjectionInfo Locodjo1965;
        public readonly ProjectionInfo Lome;
        public readonly ProjectionInfo Madzansua;
        public readonly ProjectionInfo Mahe1971;
        public readonly ProjectionInfo Malongo1987;
        public readonly ProjectionInfo Manoca;
        public readonly ProjectionInfo Manoca1962;
        public readonly ProjectionInfo Massawa;
        public readonly ProjectionInfo Merchich;
        public readonly ProjectionInfo Merchichdegrees;
        public readonly ProjectionInfo Mhast;
        public readonly ProjectionInfo Minna;
        public readonly ProjectionInfo Moznet;
        public readonly ProjectionInfo Mporaloko;
        public readonly ProjectionInfo Nahrwan1967;
        public readonly ProjectionInfo NationalGeodeticNetworkKuwait;
        public readonly ProjectionInfo NordSahara1959;
        public readonly ProjectionInfo NordSahara1959Paris;
        public readonly ProjectionInfo Observatario;
        public readonly ProjectionInfo Oman;
        public readonly ProjectionInfo PDO1993;
        public readonly ProjectionInfo Palestine1923;
        public readonly ProjectionInfo Point58;
        public readonly ProjectionInfo PointeNoire;
        public readonly ProjectionInfo Qatar;
        public readonly ProjectionInfo Qatar1948;
        public readonly ProjectionInfo Schwarzeck;
        public readonly ProjectionInfo SierraLeone1924;
        public readonly ProjectionInfo SierraLeone1960;
        public readonly ProjectionInfo SierraLeone1968;
        public readonly ProjectionInfo SouthYemen;
        public readonly ProjectionInfo Sudan;
        public readonly ProjectionInfo Tananarive1925;
        public readonly ProjectionInfo Tananarive1925Paris;
        public readonly ProjectionInfo Tete;
        public readonly ProjectionInfo TrucialCoast1948;
        public readonly ProjectionInfo Voirol1875;
        public readonly ProjectionInfo Voirol1875Paris;
        public readonly ProjectionInfo Voirol1875degrees;
        public readonly ProjectionInfo VoirolUnifie1960;
        public readonly ProjectionInfo VoirolUnifie1960Paris;
        public readonly ProjectionInfo VoirolUnifie1960degrees;
        public readonly ProjectionInfo YemenNGN1996;
        public readonly ProjectionInfo Yoff;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Africa
        /// </summary>
        public Africa()
        {
            Abidjan1987 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Accra = ProjectionInfo.FromProj4String("+proj=longlat +a=6378300 +b=6356751.689189189 +no_defs ");
            Adindan = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Afgooye = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Agadez = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            AinelAbd1970 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Arc1950 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.145 +b=6356514.966395495 +no_defs ");
            Arc1960 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            AyabelleLighthouse = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Beduaram = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Bissau = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Camacupa = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Cape = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.145 +b=6356514.966395495 +no_defs ");
            Carthage = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Carthagedegrees = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            CarthageParis = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +no_defs ");
            Conakry1905 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            CotedIvoire = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Dabola = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Douala = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Douala1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Egypt1907 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=helmert +no_defs ");
            Egypt1930 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1950 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            EuropeanLibyanDatum1979 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Garoua = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Hartebeesthoek1994 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            Kousseri = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            KuwaitOilCompany = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            KuwaitUtility = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Leigon = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Liberia1964 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Locodjo1965 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Lome = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Madzansua = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Mahe1971 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Malongo1987 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Manoca = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Manoca1962 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Massawa = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Merchich = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Merchichdegrees = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Mhast = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Minna = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Moznet = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            Mporaloko = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Nahrwan1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            NationalGeodeticNetworkKuwait = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            NordSahara1959 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            NordSahara1959Paris = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +pm=2.337229166666667 +no_defs ");
            Observatario = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Oman = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Palestine1923 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378300.79 +b=6356566.430000036 +no_defs ");
            PDO1993 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Point58 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            PointeNoire = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Qatar = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Qatar1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=helmert +no_defs ");
            Schwarzeck = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bess_nam +no_defs ");
            SierraLeone1924 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378300 +b=6356751.689189189 +no_defs ");
            SierraLeone1960 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            SierraLeone1968 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            SouthYemen = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Sudan = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Tananarive1925 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Tananarive1925Paris = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +pm=2.337229166666667 +no_defs ");
            Tete = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            TrucialCoast1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=helmert +no_defs ");
            Voirol1875 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Voirol1875degrees = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Voirol1875Paris = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +no_defs ");
            VoirolUnifie1960 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            VoirolUnifie1960degrees = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            VoirolUnifie1960Paris = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +pm=2.337229166666667 +no_defs ");
            YemenNGN1996 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            Yoff = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");

            Abidjan1987.GeographicInfo.Name = "GCS_Abidjan_1987";
            Accra.GeographicInfo.Name = "GCS_Accra";
            Adindan.GeographicInfo.Name = "GCS_Adindan";
            Afgooye.GeographicInfo.Name = "GCS_Afgooye";
            Agadez.GeographicInfo.Name = "GCS_Agadez";
            AinelAbd1970.GeographicInfo.Name = "GCS_Ain_el_Abd_1970";
            Arc1950.GeographicInfo.Name = "GCS_Arc_1950";
            Arc1960.GeographicInfo.Name = "GCS_Arc_1960";
            AyabelleLighthouse.GeographicInfo.Name = "GCS_Ayabelle";
            Beduaram.GeographicInfo.Name = "GCS_Beduaram";
            Bissau.GeographicInfo.Name = "GCS_Bissau";
            Camacupa.GeographicInfo.Name = "GCS_Camacupa";
            Cape.GeographicInfo.Name = "GCS_Cape";
            Carthage.GeographicInfo.Name = "GCS_Carthage";
            Carthagedegrees.GeographicInfo.Name = "GCS_Carthage_Degree";
            CarthageParis.GeographicInfo.Name = "GCS_Carthage_Paris";
            Conakry1905.GeographicInfo.Name = "GCS_Conakry_1905";
            CotedIvoire.GeographicInfo.Name = "GCS_Cote_d_Ivoire";
            Dabola.GeographicInfo.Name = "GCS_Dabola";
            Douala.GeographicInfo.Name = "GCS_Douala";
            Douala1948.GeographicInfo.Name = "GCS_Douala_1948";
            Egypt1907.GeographicInfo.Name = "GCS_Egypt_1907";
            Egypt1930.GeographicInfo.Name = "GCS_Egypt_1930";
            EuropeanDatum1950.GeographicInfo.Name = "GCS_European_1950";
            EuropeanLibyanDatum1979.GeographicInfo.Name = "GCS_European_Libyan_Datum_1979";
            Garoua.GeographicInfo.Name = "GCS_Garoua";
            Hartebeesthoek1994.GeographicInfo.Name = "GCS_Hartebeesthoek_1994";
            Kousseri.GeographicInfo.Name = "GCS_Kousseri";
            KuwaitOilCompany.GeographicInfo.Name = "GCS_Kuwait_Oil_Company";
            KuwaitUtility.GeographicInfo.Name = "GCS_KUDAMS";
            Leigon.GeographicInfo.Name = "GCS_Leigon";
            Liberia1964.GeographicInfo.Name = "GCS_Liberia_1964";
            Locodjo1965.GeographicInfo.Name = "GCS_Locodjo_1965";
            Lome.GeographicInfo.Name = "GCS_Lome";
            Madzansua.GeographicInfo.Name = "GCS_Madzansua";
            Mahe1971.GeographicInfo.Name = "GCS_Mahe_1971";
            Malongo1987.GeographicInfo.Name = "GCS_Malongo_1987";
            Manoca.GeographicInfo.Name = "GCS_Manoca";
            Manoca1962.GeographicInfo.Name = "GCS_Manoca_1962";
            Massawa.GeographicInfo.Name = "GCS_Massawa";
            Merchich.GeographicInfo.Name = "GCS_Merchich";
            Merchichdegrees.GeographicInfo.Name = "GCS_Merchich_Degree";
            Mhast.GeographicInfo.Name = "GCS_Mhast_Offshore";
            Minna.GeographicInfo.Name = "GCS_Minna";
            Moznet.GeographicInfo.Name = "GCS_Moznet";
            Nahrwan1967.GeographicInfo.Name = "GCS_Nahrwan_1967";
            NationalGeodeticNetworkKuwait.GeographicInfo.Name = "GCS_NGN";
            NordSahara1959.GeographicInfo.Name = "GCS_Nord_Sahara_1959";
            NordSahara1959Paris.GeographicInfo.Name = "GCS_Nord_Sahara_1959_Paris";
            Observatario.GeographicInfo.Name = "GCS_Observatario";
            Oman.GeographicInfo.Name = "GCS_Oman";
            Palestine1923.GeographicInfo.Name = "GCS_Palestine_1923";
            PDO1993.GeographicInfo.Name = "GCS_PDO_1993";
            Point58.GeographicInfo.Name = "GCS_Point_58";
            PointeNoire.GeographicInfo.Name = "GCS_Pointe_Noire";
            Qatar.GeographicInfo.Name = "GCS_Qatar";
            Qatar1948.GeographicInfo.Name = "GCS_Qatar_1948";
            Schwarzeck.GeographicInfo.Name = "GCS_Schwarzeck";
            SierraLeone1924.GeographicInfo.Name = "GCS_Sierra_Leone_1924";
            SierraLeone1960.GeographicInfo.Name = "GCS_Sierra_Leone_1960";
            SierraLeone1968.GeographicInfo.Name = "GCS_Sierra_Leone_1968";
            SouthYemen.GeographicInfo.Name = "GCS_South_Yemen";
            Sudan.GeographicInfo.Name = "GCS_Sudan";
            Tananarive1925.GeographicInfo.Name = "GCS_Tananarive_1925";
            Tananarive1925Paris.GeographicInfo.Name = "GCS_Tananarive_1925_Paris";
            Tete.GeographicInfo.Name = "GCS_Tete";
            TrucialCoast1948.GeographicInfo.Name = "GCS_Trucial_Coast_1948";
            Voirol1875.GeographicInfo.Name = "GCS_Voirol_1875";
            Voirol1875degrees.GeographicInfo.Name = "GCS_Voirol_1875_Degree";
            Voirol1875Paris.GeographicInfo.Name = "GCS_Voirol_1875_Paris";
            VoirolUnifie1960.GeographicInfo.Name = "GCS_Voirol_Unifie_1960";
            VoirolUnifie1960degrees.GeographicInfo.Name = "GCS_Voirol_Unifie_1960_Degree";
            VoirolUnifie1960Paris.GeographicInfo.Name = "GCS_Voirol_Unifie_1960_Paris";
            YemenNGN1996.GeographicInfo.Name = "GCS_Yemen_NGN_1996";
            Yoff.GeographicInfo.Name = "GCS_Yoff";

            Abidjan1987.GeographicInfo.Datum.Name = "D_Abidjan_1987";
            Accra.GeographicInfo.Datum.Name = "D_Accra";
            Adindan.GeographicInfo.Datum.Name = "D_Adindan";
            Afgooye.GeographicInfo.Datum.Name = "D_Afgooye";
            Agadez.GeographicInfo.Datum.Name = "D_Agadez";
            AinelAbd1970.GeographicInfo.Datum.Name = "D_Ain_el_Abd_1970";
            Arc1950.GeographicInfo.Datum.Name = "D_Arc_1950";
            Arc1960.GeographicInfo.Datum.Name = "D_Arc_1960";
            AyabelleLighthouse.GeographicInfo.Datum.Name = "D_Ayabelle";
            Beduaram.GeographicInfo.Datum.Name = "D_Beduaram";
            Bissau.GeographicInfo.Datum.Name = "D_Bissau";
            Camacupa.GeographicInfo.Datum.Name = "D_Camacupa";
            Cape.GeographicInfo.Datum.Name = "D_Cape";
            Carthage.GeographicInfo.Datum.Name = "D_Carthage";
            Carthagedegrees.GeographicInfo.Datum.Name = "D_Carthage";
            CarthageParis.GeographicInfo.Datum.Name = "D_Carthage";
            Conakry1905.GeographicInfo.Datum.Name = "D_Conakry_1905";
            CotedIvoire.GeographicInfo.Datum.Name = "D_Cote_d_Ivoire";
            Dabola.GeographicInfo.Datum.Name = "D_Dabola";
            Douala.GeographicInfo.Datum.Name = "D_Douala";
            Douala1948.GeographicInfo.Datum.Name = "D_Douala_1948";
            Egypt1907.GeographicInfo.Datum.Name = "D_Egypt_1907";
            Egypt1930.GeographicInfo.Datum.Name = "D_Egypt_1930";
            EuropeanDatum1950.GeographicInfo.Datum.Name = "D_European_1950";
            EuropeanLibyanDatum1979.GeographicInfo.Datum.Name = "D_European_Libyan_1979";
            Garoua.GeographicInfo.Datum.Name = "D_Garoua";
            Hartebeesthoek1994.GeographicInfo.Datum.Name = "D_Hartebeesthoek_1994";
            Kousseri.GeographicInfo.Datum.Name = "D_Kousseri";
            KuwaitOilCompany.GeographicInfo.Datum.Name = "D_Kuwait_Oil_Company";
            KuwaitUtility.GeographicInfo.Datum.Name = "D_Kuwait_Utility";
            Leigon.GeographicInfo.Datum.Name = "D_Leigon";
            Liberia1964.GeographicInfo.Datum.Name = "D_Liberia_1964";
            Locodjo1965.GeographicInfo.Datum.Name = "D_Locodjo_1965";
            Lome.GeographicInfo.Datum.Name = "D_Lome";
            Madzansua.GeographicInfo.Datum.Name = "D_Madzansua";
            Mahe1971.GeographicInfo.Datum.Name = "D_Mahe_1971";
            Malongo1987.GeographicInfo.Datum.Name = "D_Malongo_1987";
            Manoca.GeographicInfo.Datum.Name = "D_Manoca";
            Manoca1962.GeographicInfo.Datum.Name = "D_Manoca_1962";
            Massawa.GeographicInfo.Datum.Name = "D_Massawa";
            Merchich.GeographicInfo.Datum.Name = "D_Merchich";
            Merchichdegrees.GeographicInfo.Datum.Name = "D_Merchich";
            Mhast.GeographicInfo.Datum.Name = "D_Mhast_Offshore";
            Minna.GeographicInfo.Datum.Name = "D_Minna";
            Moznet.GeographicInfo.Datum.Name = "D_Moznet";
            Nahrwan1967.GeographicInfo.Datum.Name = "D_Nahrwan_1967";
            NationalGeodeticNetworkKuwait.GeographicInfo.Datum.Name = "D_NGN";
            NordSahara1959.GeographicInfo.Datum.Name = "D_Nord_Sahara_1959";
            NordSahara1959Paris.GeographicInfo.Datum.Name = "D_Nord_Sahara_1959";
            Observatario.GeographicInfo.Datum.Name = "D_Observatario";
            Oman.GeographicInfo.Datum.Name = "D_Oman";
            Palestine1923.GeographicInfo.Datum.Name = "D_Palestine_1923";
            PDO1993.GeographicInfo.Datum.Name = "D_PDO_1993";
            Point58.GeographicInfo.Datum.Name = "D_Point_58";
            PointeNoire.GeographicInfo.Datum.Name = "D_Pointe_Noire";
            Qatar.GeographicInfo.Datum.Name = "D_Qatar";
            Qatar1948.GeographicInfo.Datum.Name = "D_Qatar_1948";
            Schwarzeck.GeographicInfo.Datum.Name = "D_Schwarzeck";
            SierraLeone1924.GeographicInfo.Datum.Name = "D_Sierra_Leone_1924";
            SierraLeone1960.GeographicInfo.Datum.Name = "D_Sierra_Leone_1960";
            SierraLeone1968.GeographicInfo.Datum.Name = "D_Sierra_Leone_1968";
            SouthYemen.GeographicInfo.Datum.Name = "D_South_Yemen";
            Sudan.GeographicInfo.Datum.Name = "D_Sudan";
            Tananarive1925.GeographicInfo.Datum.Name = "D_Tananarive_1925";
            Tananarive1925Paris.GeographicInfo.Datum.Name = "D_Tananarive_1925";
            Tete.GeographicInfo.Datum.Name = "D_Tete";
            TrucialCoast1948.GeographicInfo.Datum.Name = "D_Trucial_Coast_1948";
            Voirol1875.GeographicInfo.Datum.Name = "D_Voirol_1875";
            Voirol1875degrees.GeographicInfo.Datum.Name = "D_Voirol_1875";
            Voirol1875Paris.GeographicInfo.Datum.Name = "D_Voirol_1875";
            VoirolUnifie1960.GeographicInfo.Datum.Name = "D_Voirol_Unifie_1960";
            VoirolUnifie1960degrees.GeographicInfo.Datum.Name = "D_Voirol_Unifie_1960";
            VoirolUnifie1960Paris.GeographicInfo.Datum.Name = "D_Voirol_Unifie_1960";
            YemenNGN1996.GeographicInfo.Datum.Name = "D_Yemen_NGN_1996";
            Yoff.GeographicInfo.Datum.Name = "D_Yoff";
        }

        #endregion
    }

#pragma warning restore 1591
}