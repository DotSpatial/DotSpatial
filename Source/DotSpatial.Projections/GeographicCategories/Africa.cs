// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
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
    /// This class contains predefined CoordinateSystems for Africa.
    /// </summary>
    public class Africa : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
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
        public readonly ProjectionInfo Cadastre1997;
        public readonly ProjectionInfo Camacupa;
        public readonly ProjectionInfo Cape;
        public readonly ProjectionInfo Carthage;
        public readonly ProjectionInfo CarthageGrads;
        public readonly ProjectionInfo CarthageParis;
        public readonly ProjectionInfo Conakry1905;
        public readonly ProjectionInfo CotedIvoire;
        public readonly ProjectionInfo Dabola1981;
        public readonly ProjectionInfo Douala;
        public readonly ProjectionInfo Douala1948;
        public readonly ProjectionInfo Egypt1907;
        public readonly ProjectionInfo Egypt1930;
        public readonly ProjectionInfo EgyptGulfofSuezS650TL;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EuropeanLibyanDatum1979;
        public readonly ProjectionInfo Garoua;
        public readonly ProjectionInfo Hartebeesthoek1994;
        public readonly ProjectionInfo IGC1962Arcofthe6thParallelSouth;
        public readonly ProjectionInfo IGCB1955;
        public readonly ProjectionInfo IGNAstro1960;
        public readonly ProjectionInfo Jouik1961;
        public readonly ProjectionInfo Kasai1953;
        public readonly ProjectionInfo Katanga1955;
        public readonly ProjectionInfo Kousseri;
        public readonly ProjectionInfo KuwaitOilCompany;
        public readonly ProjectionInfo KuwaitUtility;
        public readonly ProjectionInfo Leigon;
        public readonly ProjectionInfo LePouce1934;
        public readonly ProjectionInfo LGD2006;
        public readonly ProjectionInfo Liberia1964;
        public readonly ProjectionInfo Locodjo1965;
        public readonly ProjectionInfo Lome;
        public readonly ProjectionInfo Madzansua;
        public readonly ProjectionInfo Mahe1971;
        public readonly ProjectionInfo Malongo1987;
        public readonly ProjectionInfo Manoca;
        public readonly ProjectionInfo Manoca1962;
        public readonly ProjectionInfo Massawa;
        public readonly ProjectionInfo Mauritania1999;
        public readonly ProjectionInfo Merchich;
        public readonly ProjectionInfo MerchichDegrees;
        public readonly ProjectionInfo Mhast1951;
        public readonly ProjectionInfo MhastOffshore;
        public readonly ProjectionInfo MhastOnshore;
        public readonly ProjectionInfo Minna;
        public readonly ProjectionInfo Moznet;
        public readonly ProjectionInfo MPoraloko;
        public readonly ProjectionInfo Nahrwan1967;
        public readonly ProjectionInfo NationalGeodeticNetworkKuwait;
        public readonly ProjectionInfo NordSahara1959;
        public readonly ProjectionInfo NordSahara1959Paris;
        public readonly ProjectionInfo Nouakchott1965;
        public readonly ProjectionInfo Observatario;
        public readonly ProjectionInfo Palestine1923;
        public readonly ProjectionInfo Point58;
        public readonly ProjectionInfo PointeNoire;
        public readonly ProjectionInfo Qatar1948;
        public readonly ProjectionInfo Qatar1974;
        public readonly ProjectionInfo RGM04;
        public readonly ProjectionInfo RGRDC2005;
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
        public readonly ProjectionInfo Voirol1875Grads;
        public readonly ProjectionInfo Voirol1875Paris;
        public readonly ProjectionInfo Voirol1879Degrees;
        public readonly ProjectionInfo Voirol1879Grads;
        public readonly ProjectionInfo Voirol1879Paris;
        public readonly ProjectionInfo VoirolUnifie1960;
        public readonly ProjectionInfo VoirolUnifie1960Degrees;
        public readonly ProjectionInfo VoirolUnifie1960Paris;
        public readonly ProjectionInfo YemenNGN1996;
        public readonly ProjectionInfo Yoff;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Africa.
        /// </summary>
        public Africa()
        {
            Abidjan1987 = ProjectionInfo.FromEpsgCode(4143).SetNames("", "GCS_Abidjan_1987", "D_Abidjan_1987");
            Accra = ProjectionInfo.FromEpsgCode(4168).SetNames("", "GCS_Accra", "D_Accra");
            Adindan = ProjectionInfo.FromEpsgCode(4201).SetNames("", "GCS_Adindan", "D_Adindan");
            Afgooye = ProjectionInfo.FromEpsgCode(4205).SetNames("", "GCS_Afgooye", "D_Afgooye");
            Agadez = ProjectionInfo.FromEpsgCode(4206).SetNames("", "GCS_Agadez", "D_Agadez");
            AinelAbd1970 = ProjectionInfo.FromEpsgCode(4204).SetNames("", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            Arc1950 = ProjectionInfo.FromEpsgCode(4209).SetNames("", "GCS_Arc_1950", "D_Arc_1950");
            Arc1960 = ProjectionInfo.FromEpsgCode(4210).SetNames("", "GCS_Arc_1960", "D_Arc_1960");
            AyabelleLighthouse = ProjectionInfo.FromEpsgCode(4713).SetNames("", "GCS_Ayabelle", "D_Ayabelle");
            Beduaram = ProjectionInfo.FromEpsgCode(4213).SetNames("", "GCS_Beduaram", "D_Beduaram");
            Bissau = ProjectionInfo.FromEpsgCode(4165).SetNames("", "GCS_Bissau", "D_Bissau");
            Cadastre1997 = ProjectionInfo.FromEpsgCode(4475).SetNames("", "GCS_Cadastre_1997", "D_Cadastre_1997");
            Camacupa = ProjectionInfo.FromEpsgCode(4220).SetNames("", "GCS_Camacupa", "D_Camacupa");
            Cape = ProjectionInfo.FromEpsgCode(4222).SetNames("", "GCS_Cape", "D_Cape");
            Carthage = ProjectionInfo.FromEpsgCode(4223).SetNames("", "GCS_Carthage", "D_Carthage");
            CarthageGrads = ProjectionInfo.FromAuthorityCode("ESRI", 37225).SetNames("", "GCS_Carthage_Grad", "D_Carthage"); // missing
            CarthageParis = ProjectionInfo.FromEpsgCode(4816).SetNames("", "GCS_Carthage_Paris", "D_Carthage");
            Conakry1905 = ProjectionInfo.FromEpsgCode(4315).SetNames("", "GCS_Conakry_1905", "D_Conakry_1905");
            CotedIvoire = ProjectionInfo.FromEpsgCode(4226).SetNames("", "GCS_Cote_d_Ivoire", "D_Cote_d_Ivoire");
            Dabola1981 = ProjectionInfo.FromEpsgCode(4155).SetNames("", "GCS_Dabola_1981", "D_Dabola_1981");
            Douala = ProjectionInfo.FromEpsgCode(4228).SetNames("", "GCS_Douala", "D_Douala");
            Douala1948 = ProjectionInfo.FromEpsgCode(4192).SetNames("", "GCS_Douala_1948", "D_Douala_1948");
            Egypt1907 = ProjectionInfo.FromEpsgCode(4229).SetNames("", "GCS_Egypt_1907", "D_Egypt_1907");
            Egypt1930 = ProjectionInfo.FromEpsgCode(4199).SetNames("", "GCS_Egypt_1930", "D_Egypt_1930");
            EgyptGulfofSuezS650TL = ProjectionInfo.FromEpsgCode(4706).SetNames("", "GCS_Egypt_Gulf_of_Suez_S-650_TL", "D_Egypt_Gulf_of_Suez_S-650_TL");
            EuropeanDatum1950 = ProjectionInfo.FromEpsgCode(4230).SetNames("", "GCS_European_1950", "D_European_1950");
            EuropeanLibyanDatum1979 = ProjectionInfo.FromEpsgCode(4159).SetNames("", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            Garoua = ProjectionInfo.FromEpsgCode(4234).SetNames("", "GCS_Garoua", "D_Garoua");
            Hartebeesthoek1994 = ProjectionInfo.FromEpsgCode(4148).SetNames("", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994");
            IGC1962Arcofthe6thParallelSouth = ProjectionInfo.FromEpsgCode(4697).SetNames("", "GCS_IGC_1962_6th_Parallel_South", "D_IGC_1962_Arc_of_the_6th_Parallel_South");
            IGCB1955 = ProjectionInfo.FromEpsgCode(4701).SetNames("", "GCS_IGCB_1955", "D_Institut_Geographique_du_Congo_Belge_1955");
            IGNAstro1960 = ProjectionInfo.FromEpsgCode(4700).SetNames("", "GCS_IGN_Astro_1960", "D_IGN_Astro_1960");
            Jouik1961 = ProjectionInfo.FromEpsgCode(4679).SetNames("", "GCS_Jouik_1961", "D_Jouik_1961");
            Kasai1953 = ProjectionInfo.FromEpsgCode(4696).SetNames("", "GCS_Kasai_1953", "D_Kasai_1953");
            Katanga1955 = ProjectionInfo.FromEpsgCode(4695).SetNames("", "GCS_Katanga_1955", "D_Katanga_1955");
            Kousseri = ProjectionInfo.FromEpsgCode(4198).SetNames("", "GCS_Kousseri", "D_Kousseri");
            KuwaitOilCompany = ProjectionInfo.FromEpsgCode(4246).SetNames("", "GCS_Kuwait_Oil_Company", "D_Kuwait_Oil_Company");
            KuwaitUtility = ProjectionInfo.FromEpsgCode(4319).SetNames("", "GCS_KUDAMS", "D_Kuwait_Utility");
            Leigon = ProjectionInfo.FromEpsgCode(4250).SetNames("", "GCS_Leigon", "D_Leigon");
            LePouce1934 = ProjectionInfo.FromEpsgCode(4699).SetNames("", "GCS_Le_Pouce_1934", "D_Le_Pouce_1934");
            LGD2006 = ProjectionInfo.FromEpsgCode(4754).SetNames("", "GCS_LGD2006", "D_Libyan_Geodetic_Datum_2006");
            Liberia1964 = ProjectionInfo.FromEpsgCode(4251).SetNames("", "GCS_Liberia_1964", "D_Liberia_1964");
            Locodjo1965 = ProjectionInfo.FromEpsgCode(4142).SetNames("", "GCS_Locodjo_1965", "D_Locodjo_1965");
            Lome = ProjectionInfo.FromEpsgCode(4252).SetNames("", "GCS_Lome", "D_Lome");
            Madzansua = ProjectionInfo.FromEpsgCode(4128).SetNames("", "GCS_Madzansua", "D_Madzansua");
            Mahe1971 = ProjectionInfo.FromEpsgCode(4256).SetNames("", "GCS_Mahe_1971", "D_Mahe_1971");
            Malongo1987 = ProjectionInfo.FromEpsgCode(4259).SetNames("", "GCS_Malongo_1987", "D_Malongo_1987");
            Manoca = ProjectionInfo.FromEpsgCode(4260).SetNames("", "GCS_Manoca", "D_Manoca");
            Manoca1962 = ProjectionInfo.FromEpsgCode(4193).SetNames("", "GCS_Manoca_1962", "D_Manoca_1962");
            Massawa = ProjectionInfo.FromEpsgCode(4262).SetNames("", "GCS_Massawa", "D_Massawa");
            Mauritania1999 = ProjectionInfo.FromEpsgCode(4702).SetNames("", "GCS_Mauritania_1999", "D_Mauritania_1999");
            Merchich = ProjectionInfo.FromEpsgCode(4261).SetNames("", "GCS_Merchich", "D_Merchich");
            MerchichDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 104261).SetNames("", "GCS_Merchich_Degree", "D_Merchich");
            Mhast1951 = ProjectionInfo.FromEpsgCode(4703).SetNames("", "GCS_Mhast_1951", "D_Mhast_1951");
            MhastOffshore = ProjectionInfo.FromEpsgCode(4705).SetNames("", "GCS_Mhast_Offshore", "D_Mhast_Offshore");
            MhastOnshore = ProjectionInfo.FromEpsgCode(4704).SetNames("", "GCS_Mhast_Onshore", "D_Mhast_Onshore");
            Minna = ProjectionInfo.FromEpsgCode(4263).SetNames("", "GCS_Minna", "D_Minna");
            Moznet = ProjectionInfo.FromEpsgCode(4130).SetNames("", "GCS_Moznet", "D_Moznet");
            MPoraloko = ProjectionInfo.FromEpsgCode(4266).SetNames("", "GCS_Mporaloko", "D_Mporaloko");
            Nahrwan1967 = ProjectionInfo.FromEpsgCode(4270).SetNames("", "GCS_Nahrwan_1967", "D_Nahrwan_1967");
            NationalGeodeticNetworkKuwait = ProjectionInfo.FromEpsgCode(4318).SetNames("", "GCS_NGN", "D_NGN");
            NordSahara1959 = ProjectionInfo.FromEpsgCode(4307).SetNames("", "GCS_Nord_Sahara_1959", "D_Nord_Sahara_1959");
            NordSahara1959Paris = ProjectionInfo.FromEpsgCode(4819).SetNames("", "GCS_Nord_Sahara_1959_Paris", "D_Nord_Sahara_1959");
            Nouakchott1965 = ProjectionInfo.FromEpsgCode(4680).SetNames("", "GCS_Nouakchott_1965", "D_Nouakchott_1965");
            Observatario = ProjectionInfo.FromEpsgCode(4129).SetNames("", "GCS_Observatario", "D_Observatario");
            Palestine1923 = ProjectionInfo.FromEpsgCode(4281).SetNames("", "GCS_Palestine_1923", "D_Palestine_1923");
            Point58 = ProjectionInfo.FromEpsgCode(4620).SetNames("", "GCS_Point_58", "D_Point_58");
            PointeNoire = ProjectionInfo.FromEpsgCode(4282).SetNames("", "GCS_Pointe_Noire", "D_Pointe_Noire");
            Qatar1948 = ProjectionInfo.FromEpsgCode(4286).SetNames("", "GCS_Qatar_1948", "D_Qatar_1948");
            Qatar1974 = ProjectionInfo.FromEpsgCode(4285).SetNames("", "GCS_Qatar_1974", "D_Qatar");
            RGM04 = ProjectionInfo.FromAuthorityCode("EPSG", 4469).SetNames("", "GCS_RGM_2004", "D_Reseau_Geodesique_de_Mayotte_2004"); // missing
            RGRDC2005 = ProjectionInfo.FromEpsgCode(4046).SetNames("", "GCS_RGRDC_2005", "D_Reseau_Geodesique_de_la_RDC_2005");
            Schwarzeck = ProjectionInfo.FromEpsgCode(4293).SetNames("", "GCS_Schwarzeck", "D_Schwarzeck");
            SierraLeone1924 = ProjectionInfo.FromEpsgCode(4174).SetNames("", "GCS_Sierra_Leone_1924", "D_Sierra_Leone_1924");
            SierraLeone1960 = ProjectionInfo.FromAuthorityCode("ESRI", 104103).SetNames("", "GCS_Sierra_Leone_1960", "D_Sierra_Leone_1960");
            SierraLeone1968 = ProjectionInfo.FromEpsgCode(4175).SetNames("", "GCS_Sierra_Leone_1968", "D_Sierra_Leone_1968");
            SouthYemen = ProjectionInfo.FromEpsgCode(4164).SetNames("", "GCS_South_Yemen", "D_South_Yemen");
            Sudan = ProjectionInfo.FromEpsgCode(4296).SetNames("", "GCS_Sudan", "D_Sudan");
            Tananarive1925 = ProjectionInfo.FromEpsgCode(4297).SetNames("", "GCS_Tananarive_1925", "D_Tananarive_1925");
            Tananarive1925Paris = ProjectionInfo.FromEpsgCode(4810).SetNames("", "GCS_Tananarive_1925_Paris", "D_Tananarive_1925");
            Tete = ProjectionInfo.FromEpsgCode(4127).SetNames("", "GCS_Tete", "D_Tete");
            TrucialCoast1948 = ProjectionInfo.FromEpsgCode(4303).SetNames("", "GCS_Trucial_Coast_1948", "D_Trucial_Coast_1948");
            Voirol1875 = ProjectionInfo.FromEpsgCode(4304).SetNames("", "GCS_Voirol_1875", "D_Voirol_1875");
            Voirol1875Grads = ProjectionInfo.FromAuthorityCode("ESRI", 104139).SetNames("", "GCS_Voirol_1875_Grad", "D_Voirol_1875"); // missing
            Voirol1875Paris = ProjectionInfo.FromEpsgCode(4811).SetNames("", "GCS_Voirol_1875_Paris", "D_Voirol_1875");
            Voirol1879Degrees = ProjectionInfo.FromEpsgCode(4671).SetNames("", "GCS_Voirol_1879", "D_Voirol_1879");
            Voirol1879Grads = ProjectionInfo.FromAuthorityCode("ESRI", 104140).SetNames("", "GCS_Voirol_1879_Grad", "D_Voirol_1879"); // missing
            Voirol1879Paris = ProjectionInfo.FromEpsgCode(4821).SetNames("", "GCS_Voirol_1879_Paris", "D_Voirol_1879");
            VoirolUnifie1960 = ProjectionInfo.FromAuthorityCode("EPSG", 4305).SetNames("", "GCS_Voirol_Unifie_1960", "D_Voirol_Unifie_1960"); // missing
            VoirolUnifie1960Degrees = ProjectionInfo.FromAuthorityCode("ESRI", 104305).SetNames("", "GCS_Voirol_Unifie_1960_Degree", "D_Voirol_Unifie_1960"); // missing
            VoirolUnifie1960Paris = ProjectionInfo.FromEpsgCode(4812).SetNames("", "GCS_Voirol_Unifie_1960_Paris", "D_Voirol_Unifie_1960");
            YemenNGN1996 = ProjectionInfo.FromEpsgCode(4163).SetNames("", "GCS_Yemen_NGN_1996", "D_Yemen_NGN_1996");
            Yoff = ProjectionInfo.FromEpsgCode(4310).SetNames("", "GCS_Yoff", "D_Yoff");
        }

        #endregion
    }
}

#pragma warning restore 1591
