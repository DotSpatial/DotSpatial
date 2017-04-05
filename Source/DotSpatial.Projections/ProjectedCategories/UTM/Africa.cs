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

namespace DotSpatial.Projections.ProjectedCategories.UTM
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Africa.
    /// </summary>
    public class Africa : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Abidjan1987UTMZone29N;
        public readonly ProjectionInfo Abidjan1987UTMZone30N;
        public readonly ProjectionInfo AdindanUTMZone35N;
        public readonly ProjectionInfo AdindanUTMZone36N;
        public readonly ProjectionInfo AdindanUTMZone37N;
        public readonly ProjectionInfo AdindanUTMZone38N;
        public readonly ProjectionInfo Arc1950UTMZone34S;
        public readonly ProjectionInfo Arc1950UTMZone35S;
        public readonly ProjectionInfo Arc1950UTMZone36S;
        public readonly ProjectionInfo Arc1960UTMZone35N;
        public readonly ProjectionInfo Arc1960UTMZone35S;
        public readonly ProjectionInfo Arc1960UTMZone36N;
        public readonly ProjectionInfo Arc1960UTMZone36S;
        public readonly ProjectionInfo Arc1960UTMZone37N;
        public readonly ProjectionInfo Arc1960UTMZone37S;
        public readonly ProjectionInfo BissauUTMZone28N;
        public readonly ProjectionInfo CamacupaUTMZone32S;
        public readonly ProjectionInfo CamacupaUTMZone33S;
        public readonly ProjectionInfo CapeUTMZone34S;
        public readonly ProjectionInfo CapeUTMZone35S;
        public readonly ProjectionInfo CapeUTMZone36S;
        public readonly ProjectionInfo CarthageUTMZone32N;
        public readonly ProjectionInfo Conakry1905UTMZone28N;
        public readonly ProjectionInfo Conakry1905UTMZone29N;
        public readonly ProjectionInfo GarouaUTMZone33N;
        public readonly ProjectionInfo IGCB1955CongoUTMZone33S;
        public readonly ProjectionInfo IGNAstro1960UTMZone28N;
        public readonly ProjectionInfo IGNAstro1960UTMZone29N;
        public readonly ProjectionInfo IGNAstro1960UTMZone30N;
        public readonly ProjectionInfo KousseriUTMZone33N;
        public readonly ProjectionInfo LGD2006UTMZone32N;
        public readonly ProjectionInfo LGD2006UTMZone33N;
        public readonly ProjectionInfo LGD2006UTMZone34N;
        public readonly ProjectionInfo LGD2006UTMZone35N;
        public readonly ProjectionInfo Locodjo1965UTMZone29N;
        public readonly ProjectionInfo Locodjo1965UTMZone30N;
        public readonly ProjectionInfo LomeUTMZone31N;
        public readonly ProjectionInfo Malongo1987UTMZone32S;
        public readonly ProjectionInfo Manoca1962UTMZone32N;
        public readonly ProjectionInfo MassawaUTMZone37N;
        public readonly ProjectionInfo Mauritania1999UTMZone28N;
        public readonly ProjectionInfo Mauritania1999UTMZone29N;
        public readonly ProjectionInfo Mauritania1999UTMZone30N;
        public readonly ProjectionInfo MerchichDegreesUTMZone28N;
        public readonly ProjectionInfo MhastOffshoreUTMZone32S;
        public readonly ProjectionInfo MhastOnshoreUTMZone32S;
        public readonly ProjectionInfo MinnaUTMZone31N;
        public readonly ProjectionInfo MinnaUTMZone32N;
        public readonly ProjectionInfo MoznetUTMZone36S;
        public readonly ProjectionInfo MoznetUTMZone37S;
        public readonly ProjectionInfo MPoralokoUTMZone32N;
        public readonly ProjectionInfo MPoralokoUTMZone32S;
        public readonly ProjectionInfo NGNUTMZone38N;
        public readonly ProjectionInfo NGNUTMZone39N;
        public readonly ProjectionInfo NordSahara1959UTMZone29N;
        public readonly ProjectionInfo NordSahara1959UTMZone30N;
        public readonly ProjectionInfo NordSahara1959UTMZone31N;
        public readonly ProjectionInfo NordSahara1959UTMZone32N;
        public readonly ProjectionInfo PointeNoireUTMZone32S;
        public readonly ProjectionInfo RGRDC2005UTMZone33S;
        public readonly ProjectionInfo RGRDC2005UTMZone34S;
        public readonly ProjectionInfo RGRDC2005UTMZone35S;
        public readonly ProjectionInfo SchwarzeckUTMZone33S;
        public readonly ProjectionInfo SierraLeone1968UTMZone28N;
        public readonly ProjectionInfo SierraLeone1968UTMZone29N;
        public readonly ProjectionInfo SudanUTMZone35N;
        public readonly ProjectionInfo SudanUTMZone36N;
        public readonly ProjectionInfo Tananarive1925UTMZone38S;
        public readonly ProjectionInfo Tananarive1925UTMZone39S;
        public readonly ProjectionInfo TeteUTMZone36S;
        public readonly ProjectionInfo TeteUTMZone37S;
        public readonly ProjectionInfo Yoff1972UTMZone28N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Africa.
        /// </summary>
        public Africa()
        {
            Abidjan1987UTMZone29N = ProjectionInfo.FromEpsgCode(2043).SetNames("Abidjan_1987_UTM_Zone_29N", "GCS_Abidjan_1987", "D_Abidjan_1987");
            Abidjan1987UTMZone30N = ProjectionInfo.FromEpsgCode(2041).SetNames("Abidjan_1987_UTM_Zone_30N", "GCS_Abidjan_1987", "D_Abidjan_1987");
            AdindanUTMZone35N = ProjectionInfo.FromEpsgCode(20135).SetNames("Adindan_UTM_Zone_35N", "GCS_Adindan", "D_Adindan");
            AdindanUTMZone36N = ProjectionInfo.FromEpsgCode(20136).SetNames("Adindan_UTM_Zone_36N", "GCS_Adindan", "D_Adindan");
            AdindanUTMZone37N = ProjectionInfo.FromEpsgCode(20137).SetNames("Adindan_UTM_Zone_37N", "GCS_Adindan", "D_Adindan");
            AdindanUTMZone38N = ProjectionInfo.FromEpsgCode(20138).SetNames("Adindan_UTM_Zone_38N", "GCS_Adindan", "D_Adindan");
            Arc1950UTMZone34S = ProjectionInfo.FromEpsgCode(20934).SetNames("Arc_1950_UTM_Zone_34S", "GCS_Arc_1950", "D_Arc_1950");
            Arc1950UTMZone35S = ProjectionInfo.FromEpsgCode(20935).SetNames("Arc_1950_UTM_Zone_35S", "GCS_Arc_1950", "D_Arc_1950");
            Arc1950UTMZone36S = ProjectionInfo.FromEpsgCode(20936).SetNames("Arc_1950_UTM_Zone_36S", "GCS_Arc_1950", "D_Arc_1950");
            Arc1960UTMZone35N = ProjectionInfo.FromEpsgCode(21095).SetNames("Arc_1960_UTM_Zone_35N", "GCS_Arc_1960", "D_Arc_1960");
            Arc1960UTMZone35S = ProjectionInfo.FromEpsgCode(21035).SetNames("Arc_1960_UTM_Zone_35S", "GCS_Arc_1960", "D_Arc_1960");
            Arc1960UTMZone36N = ProjectionInfo.FromEpsgCode(21096).SetNames("Arc_1960_UTM_Zone_36N", "GCS_Arc_1960", "D_Arc_1960");
            Arc1960UTMZone36S = ProjectionInfo.FromEpsgCode(21036).SetNames("Arc_1960_UTM_Zone_36S", "GCS_Arc_1960", "D_Arc_1960");
            Arc1960UTMZone37N = ProjectionInfo.FromEpsgCode(21097).SetNames("Arc_1960_UTM_Zone_37N", "GCS_Arc_1960", "D_Arc_1960");
            Arc1960UTMZone37S = ProjectionInfo.FromEpsgCode(21037).SetNames("Arc_1960_UTM_Zone_37S", "GCS_Arc_1960", "D_Arc_1960");
            BissauUTMZone28N = ProjectionInfo.FromEpsgCode(2095).SetNames("Bissau_UTM_Zone_28N", "GCS_Bissau", "D_Bissau");
            CamacupaUTMZone32S = ProjectionInfo.FromEpsgCode(22032).SetNames("Camacupa_UTM_Zone_32S", "GCS_Camacupa", "D_Camacupa");
            CamacupaUTMZone33S = ProjectionInfo.FromEpsgCode(22033).SetNames("Camacupa_UTM_Zone_33S", "GCS_Camacupa", "D_Camacupa");
            CapeUTMZone34S = ProjectionInfo.FromEpsgCode(22234).SetNames("Cape_UTM_Zone_34S", "GCS_Cape", "D_Cape");
            CapeUTMZone35S = ProjectionInfo.FromEpsgCode(22235).SetNames("Cape_UTM_Zone_35S", "GCS_Cape", "D_Cape");
            CapeUTMZone36S = ProjectionInfo.FromEpsgCode(22236).SetNames("Cape_UTM_Zone_36S", "GCS_Cape", "D_Cape");
            CarthageUTMZone32N = ProjectionInfo.FromEpsgCode(22332).SetNames("Carthage_UTM_Zone_32N", "GCS_Carthage", "D_Carthage");
            Conakry1905UTMZone28N = ProjectionInfo.FromEpsgCode(31528).SetNames("Conakry_1905_UTM_Zone_28N", "GCS_Conakry_1905", "D_Conakry_1905");
            Conakry1905UTMZone29N = ProjectionInfo.FromEpsgCode(31529).SetNames("Conakry_1905_UTM_Zone_29N", "GCS_Conakry_1905", "D_Conakry_1905");
            GarouaUTMZone33N = ProjectionInfo.FromEpsgCode(2312).SetNames("Garoua_UTM_Zone_33N", "GCS_Garoua", "D_Garoua");
            IGCB1955CongoUTMZone33S = ProjectionInfo.FromEpsgCode(3342).SetNames("IGCB_1955_UTM_Zone_33S", "GCS_IGCB_1955", "D_Institut_Geographique_du_Congo_Belge_1955");
            IGNAstro1960UTMZone28N = ProjectionInfo.FromEpsgCode(3367).SetNames("IGN_Astro_1960_UTM_Zone_28N", "GCS_IGN_Astro_1960", "D_IGN_Astro_1960");
            IGNAstro1960UTMZone29N = ProjectionInfo.FromEpsgCode(3368).SetNames("IGN_Astro_1960_UTM_Zone_29N", "GCS_IGN_Astro_1960", "D_IGN_Astro_1960");
            IGNAstro1960UTMZone30N = ProjectionInfo.FromEpsgCode(3369).SetNames("IGN_Astro_1960_UTM_Zone_30N", "GCS_IGN_Astro_1960", "D_IGN_Astro_1960");
            KousseriUTMZone33N = ProjectionInfo.FromEpsgCode(2313).SetNames("Kousseri_UTM_Zone_33N", "GCS_Kousseri", "D_Kousseri");
            LGD2006UTMZone32N = ProjectionInfo.FromEpsgCode(3199).SetNames("LGD2006_UTM_Zone_32N", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006UTMZone33N = ProjectionInfo.FromEpsgCode(3201).SetNames("LGD2006_UTM_Zone_33N", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006UTMZone34N = ProjectionInfo.FromEpsgCode(3202).SetNames("LGD2006_UTM_Zone_34N", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            LGD2006UTMZone35N = ProjectionInfo.FromEpsgCode(3203).SetNames("LGD2006_UTM_Zone_35N", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            Locodjo1965UTMZone29N = ProjectionInfo.FromEpsgCode(2042).SetNames("Locodjo_1965_UTM_Zone_29N", "GCS_Locodjo_1965", "D_Locodjo_1965");
            Locodjo1965UTMZone30N = ProjectionInfo.FromEpsgCode(2040).SetNames("Locodjo_1965_UTM_Zone_30N", "GCS_Locodjo_1965", "D_Locodjo_1965");
            LomeUTMZone31N = ProjectionInfo.FromEpsgCode(25231).SetNames("Lome_UTM_Zone_31N", "GCS_Lome", "D_Lome");
            Malongo1987UTMZone32S = ProjectionInfo.FromEpsgCode(25932).SetNames("Malongo_1987_UTM_Zone_32S", "GCS_Malongo_1987", "D_Malongo_1987");
            Manoca1962UTMZone32N = ProjectionInfo.FromEpsgCode(2215).SetNames("Manoca_1962_UTM_Zone_32N", "GCS_Manoca_1962", "D_Manoca_1962");
            MassawaUTMZone37N = ProjectionInfo.FromEpsgCode(26237).SetNames("Massawa_UTM_Zone_37N", "GCS_Massawa", "D_Massawa");
            Mauritania1999UTMZone28N = ProjectionInfo.FromEpsgCode(3343).SetNames("Mauritania_1999_UTM_Zone_28N", "GCS_Mauritania_1999", "D_Mauritania_1999");
            Mauritania1999UTMZone29N = ProjectionInfo.FromEpsgCode(3344).SetNames("Mauritania_1999_UTM_Zone_29N", "GCS_Mauritania_1999", "D_Mauritania_1999");
            Mauritania1999UTMZone30N = ProjectionInfo.FromEpsgCode(3345).SetNames("Mauritania_1999_UTM_Zone_30N", "GCS_Mauritania_1999", "D_Mauritania_1999");
            MerchichDegreesUTMZone28N = ProjectionInfo.FromAuthorityCode("ESRI", 102144).SetNames("Merchich_Degree_UTM_Zone_28N", "GCS_Merchich_Degree", "D_Merchich"); // missing
            MhastOffshoreUTMZone32S = ProjectionInfo.FromEpsgCode(3354).SetNames("Mhast_Offshore_UTM_Zone_32S", "GCS_Mhast_Offshore", "D_Mhast_Offshore");
            MhastOnshoreUTMZone32S = ProjectionInfo.FromEpsgCode(3353).SetNames("Mhast_Onshore_UTM_Zone_32S", "GCS_Mhast_Onshore", "D_Mhast_Onshore");
            MinnaUTMZone31N = ProjectionInfo.FromEpsgCode(26331).SetNames("Minna_UTM_Zone_31N", "GCS_Minna", "D_Minna");
            MinnaUTMZone32N = ProjectionInfo.FromEpsgCode(26332).SetNames("Minna_UTM_Zone_32N", "GCS_Minna", "D_Minna");
            MoznetUTMZone36S = ProjectionInfo.FromEpsgCode(3036).SetNames("Moznet_UTM_Zone_36S", "GCS_Moznet", "D_Moznet");
            MoznetUTMZone37S = ProjectionInfo.FromEpsgCode(3037).SetNames("Moznet_UTM_Zone_37S", "GCS_Moznet", "D_Moznet");
            MPoralokoUTMZone32N = ProjectionInfo.FromEpsgCode(26632).SetNames("Mporaloko_UTM_Zone_32N", "GCS_Mporaloko", "D_Mporaloko");
            MPoralokoUTMZone32S = ProjectionInfo.FromEpsgCode(26692).SetNames("Mporaloko_UTM_Zone_32S", "GCS_Mporaloko", "D_Mporaloko");
            NGNUTMZone38N = ProjectionInfo.FromEpsgCode(31838).SetNames("NGN_UTM_Zone_38N", "GCS_NGN", "D_NGN");
            NGNUTMZone39N = ProjectionInfo.FromEpsgCode(31839).SetNames("NGN_UTM_Zone_39N", "GCS_NGN", "D_NGN");
            NordSahara1959UTMZone29N = ProjectionInfo.FromEpsgCode(30729).SetNames("Nord_Sahara_1959_UTM_Zone_29N", "GCS_Nord_Sahara_1959", "D_Nord_Sahara_1959");
            NordSahara1959UTMZone30N = ProjectionInfo.FromEpsgCode(30730).SetNames("Nord_Sahara_1959_UTM_Zone_30N", "GCS_Nord_Sahara_1959", "D_Nord_Sahara_1959");
            NordSahara1959UTMZone31N = ProjectionInfo.FromEpsgCode(30731).SetNames("Nord_Sahara_1959_UTM_Zone_31N", "GCS_Nord_Sahara_1959", "D_Nord_Sahara_1959");
            NordSahara1959UTMZone32N = ProjectionInfo.FromEpsgCode(30732).SetNames("Nord_Sahara_1959_UTM_Zone_32N", "GCS_Nord_Sahara_1959", "D_Nord_Sahara_1959");
            PointeNoireUTMZone32S = ProjectionInfo.FromEpsgCode(28232).SetNames("Pointe_Noire_UTM_Zone_32S", "GCS_Pointe_Noire", "D_Pointe_Noire");
            RGRDC2005UTMZone33S = ProjectionInfo.FromAuthorityCode("EPSG", 103210).SetNames("RGRDC_2005_UTM_Zone_33S", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005UTMZone34S = ProjectionInfo.FromAuthorityCode("EPSG", 103211).SetNames("RGRDC_2005_UTM_Zone_34S", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            RGRDC2005UTMZone35S = ProjectionInfo.FromAuthorityCode("EPSG", 103212).SetNames("RGRDC_2005_UTM_Zone_35S", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005"); // missing
            SchwarzeckUTMZone33S = ProjectionInfo.FromEpsgCode(29333).SetNames("Schwarzeck_UTM_Zone_33S", "GCS_Schwarzeck", "D_Schwarzeck");
            SierraLeone1968UTMZone28N = ProjectionInfo.FromEpsgCode(2161).SetNames("Sierra_Leone_1968_UTM_Zone_28N", "GCS_Sierra_Leone_1968", "D_Sierra_Leone_1968");
            SierraLeone1968UTMZone29N = ProjectionInfo.FromEpsgCode(2162).SetNames("Sierra_Leone_1968_UTM_Zone_29N", "GCS_Sierra_Leone_1968", "D_Sierra_Leone_1968");
            SudanUTMZone35N = ProjectionInfo.FromEpsgCode(29635).SetNames("Sudan_UTM_Zone_35N", "GCS_Sudan", "D_Sudan");
            SudanUTMZone36N = ProjectionInfo.FromEpsgCode(29636).SetNames("Sudan_UTM_Zone_36N", "GCS_Sudan", "D_Sudan");
            Tananarive1925UTMZone38S = ProjectionInfo.FromEpsgCode(29738).SetNames("Tananarive_1925_UTM_Zone_38S", "GCS_Tananarive_1925", "D_Tananarive_1925");
            Tananarive1925UTMZone39S = ProjectionInfo.FromEpsgCode(29739).SetNames("Tananarive_1925_UTM_Zone_39S", "GCS_Tananarive_1925", "D_Tananarive_1925");
            TeteUTMZone36S = ProjectionInfo.FromEpsgCode(2736).SetNames("Tete_UTM_Zone_36S", "GCS_Tete", "D_Tete");
            TeteUTMZone37S = ProjectionInfo.FromEpsgCode(2737).SetNames("Tete_UTM_Zone_37S", "GCS_Tete", "D_Tete");
            Yoff1972UTMZone28N = ProjectionInfo.FromEpsgCode(31028).SetNames("Yoff_1972_UTM_Zone_28N", "GCS_Yoff", "D_Yoff");
        }

        #endregion
    }
}

#pragma warning restore 1591