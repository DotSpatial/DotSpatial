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
    /// This class contains predefined CoordinateSystems for Oceans.
    /// </summary>
    public class Oceans : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AmericanSamoa1962UTMZone2S;
        public readonly ProjectionInfo AzoresCentral1948UTMZone26N;
        public readonly ProjectionInfo AzoresCentral1995UTMZone26N;
        public readonly ProjectionInfo AzoresOccidental1939UTMZone25N;
        public readonly ProjectionInfo AzoresOriental1940UTMZone26N;
        public readonly ProjectionInfo AzoresOriental1995UTMZone26N;
        public readonly ProjectionInfo Bermuda1957UTMZone20N;
        public readonly ProjectionInfo Combani1950UTMZone38S;
        public readonly ProjectionInfo FatuIva1972UTMZone7S;
        public readonly ProjectionInfo Fiji1956UTMZone1S;
        public readonly ProjectionInfo Fiji1956UTMZone60S;
        public readonly ProjectionInfo FortDesaixUTMZone20N;
        public readonly ProjectionInfo FortMarigotUTMZone20N;
        public readonly ProjectionInfo GraciosaBaseSW1948UTMZone26N;
        public readonly ProjectionInfo GrandComorosUTMZone38S;
        public readonly ProjectionInfo Hjorsey1955UTMZone26N;
        public readonly ProjectionInfo Hjorsey1955UTMZone27N;
        public readonly ProjectionInfo Hjorsey1955UTMZone28N;
        public readonly ProjectionInfo IGN53MareUTMZone58S;
        public readonly ProjectionInfo IGN53MareUTMZone59S;
        public readonly ProjectionInfo IGN56LifouUTMZone58S;
        public readonly ProjectionInfo IGN63HivaOaUTMZone7S;
        public readonly ProjectionInfo IGN72GrandeTerreUTMZone58S;
        public readonly ProjectionInfo IGN72NukuHivaUTMZone7S;
        public readonly ProjectionInfo JAD2001UTMZone17N;
        public readonly ProjectionInfo JAD2001UTMZone18N;
        public readonly ProjectionInfo KerguelenIsland1949UTMZone42S;
        public readonly ProjectionInfo Madeira1936UTMZone28N;
        public readonly ProjectionInfo Maupiti1983UTMZone5S;
        public readonly ProjectionInfo Moorea1987UTMZone6S;
        public readonly ProjectionInfo MOP78UTMZone1S;
        public readonly ProjectionInfo NEA74NoumeaUTMZone58S;
        public readonly ProjectionInfo ObservMeteorologico1939UTMZone25N;
        public readonly ProjectionInfo OldHawaiianUTMZone4N;
        public readonly ProjectionInfo OldHawaiianUTMZone5N;
        public readonly ProjectionInfo Pitcairn1967UTMZone9S;
        public readonly ProjectionInfo PortoSanto1936UTMZone28N;
        public readonly ProjectionInfo PortoSanto1995UTMZone28N;
        public readonly ProjectionInfo PTRA08UTMZone25N;
        public readonly ProjectionInfo PTRA08UTMZone26N;
        public readonly ProjectionInfo PTRA08UTMZone28N;
        public readonly ProjectionInfo PuertoRicoUTMZone20N;
        public readonly ProjectionInfo RGNC199193UTMZone57S;
        public readonly ProjectionInfo RGNC199193UTMZone58S;
        public readonly ProjectionInfo RGNC199193UTMZone59S;
        public readonly ProjectionInfo RGPFUTMZone5S;
        public readonly ProjectionInfo RGPFUTMZone6S;
        public readonly ProjectionInfo RGPFUTMZone7S;
        public readonly ProjectionInfo RGPFUTMZone8S;
        public readonly ProjectionInfo RGR1992UTMZone40S;
        public readonly ProjectionInfo RRAF1991UTMZone20N;
        public readonly ProjectionInfo SainteAnneUTMZone20N;
        public readonly ProjectionInfo SaoBrazUTMZone26N;
        public readonly ProjectionInfo SapperHill1943UTMZone20S;
        public readonly ProjectionInfo SapperHill1943UTMZone21S;
        public readonly ProjectionInfo SelvagemGrande1938UTMZone28N;
        public readonly ProjectionInfo ST71BelepUTMZone58S;
        public readonly ProjectionInfo ST84IledesPinsUTMZone58S;
        public readonly ProjectionInfo ST87OuveaUTMZone58S;
        public readonly ProjectionInfo TahaaUTMZone5S;
        public readonly ProjectionInfo Tahiti1979UTMZone6S;
        public readonly ProjectionInfo TahitiUTMZone6S;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Oceans.
        /// </summary>
        public Oceans()
        {
            AmericanSamoa1962UTMZone2S = ProjectionInfo.FromAuthorityCode("ESRI", 102116).SetNames("American_Samoa_1962_UTM_Zone_2S", "GCS_American_Samoa_1962", "D_American_Samoa_1962"); // missing
            AzoresCentral1948UTMZone26N = ProjectionInfo.FromEpsgCode(2189).SetNames("Azores_Central_1948_UTM_Zone_26N", "GCS_Azores_Central_1948", "D_Azores_Central_Islands_1948");
            AzoresCentral1995UTMZone26N = ProjectionInfo.FromEpsgCode(3063).SetNames("Azores_Central_1995_UTM_Zone_26N", "GCS_Azores_Central_1995", "D_Azores_Central_Islands_1995");
            AzoresOccidental1939UTMZone25N = ProjectionInfo.FromEpsgCode(2188).SetNames("Azores_Occidental_1939_UTM_Zone_25N", "GCS_Azores_Occidental_1939", "D_Azores_Occidental_Islands_1939");
            AzoresOriental1940UTMZone26N = ProjectionInfo.FromEpsgCode(2190).SetNames("Azores_Oriental_1940_UTM_Zone_26N", "GCS_Azores_Oriental_1940", "D_Azores_Oriental_Islands_1940");
            AzoresOriental1995UTMZone26N = ProjectionInfo.FromEpsgCode(3062).SetNames("Azores_Oriental_1995_UTM_Zone_26N", "GCS_Azores_Oriental_1995", "D_Azores_Oriental_Islands_1995");
            Bermuda1957UTMZone20N = ProjectionInfo.FromEpsgCode(3769).SetNames("Bermuda_1957_UTM_Zone_20N", "GCS_Bermuda_1957", "D_Bermuda_1957");
            Combani1950UTMZone38S = ProjectionInfo.FromEpsgCode(2980).SetNames("Combani_1950_UTM_38S", "GCS_Combani_1950", "D_Combani_1950");
            FatuIva1972UTMZone7S = ProjectionInfo.FromEpsgCode(3303).SetNames("Fatu_Iva_1972_UTM_Zone_7S", "GCS_Fatu_Iva_1972", "D_Fatu_Iva_1972");
            Fiji1956UTMZone1S = ProjectionInfo.FromEpsgCode(3142).SetNames("Fiji_1956_UTM_Zone_1S", "GCS_Fiji_1956", "D_Fiji_1956");
            Fiji1956UTMZone60S = ProjectionInfo.FromEpsgCode(3141).SetNames("Fiji_1956_UTM_Zone_60S", "GCS_Fiji_1956", "D_Fiji_1956");
            FortDesaixUTMZone20N = ProjectionInfo.FromEpsgCode(2973).SetNames("Fort_Desaix_UTM_20N", "GCS_Fort_Desaix", "D_Fort_Desaix");
            FortMarigotUTMZone20N = ProjectionInfo.FromEpsgCode(2969).SetNames("Fort_Marigot_UTM_20N", "GCS_Fort_Marigot", "D_Fort_Marigot");
            GraciosaBaseSW1948UTMZone26N = ProjectionInfo.FromAuthorityCode("ESRI", 102162).SetNames("Graciosa_Base_SW_1948_UTM_Zone_26N", "GCS_Graciosa_Base_SW_1948", "D_Graciosa_Base_SW_1948");
            GrandComorosUTMZone38S = ProjectionInfo.FromEpsgCode(2999).SetNames("Grand_Comoros_UTM_38S", "GCS_Grand_Comoros", "D_Grand_Comoros");
            Hjorsey1955UTMZone26N = ProjectionInfo.FromEpsgCode(3054).SetNames("Hjorsey_1955_UTM_Zone_26N", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            Hjorsey1955UTMZone27N = ProjectionInfo.FromEpsgCode(3055).SetNames("Hjorsey_1955_UTM_Zone_27N", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            Hjorsey1955UTMZone28N = ProjectionInfo.FromEpsgCode(3056).SetNames("Hjorsey_1955_UTM_Zone_28N", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            IGN53MareUTMZone58S = ProjectionInfo.FromEpsgCode(2995).SetNames("IGN53_Mare_UTM_58S", "GCS_IGN53_Mare", "D_IGN53_Mare");
            IGN53MareUTMZone59S = ProjectionInfo.FromEpsgCode(3172).SetNames("IGN53_Mare_UTM_Zone_59S", "GCS_IGN53_Mare", "D_IGN53_Mare");
            IGN56LifouUTMZone58S = ProjectionInfo.FromEpsgCode(2981).SetNames("IGN56_Lifou_UTM_58S", "GCS_IGN56_Lifou", "D_IGN56_Lifou");
            IGN63HivaOaUTMZone7S = ProjectionInfo.FromEpsgCode(3302).SetNames("IGN63_Hiva_Oa_UTM_Zone_7S", "GCS_IGN63_Hiva_Oa", "D_IGN63_Hiva_Oa");
            IGN72GrandeTerreUTMZone58S = ProjectionInfo.FromEpsgCode(3060).SetNames("IGN72_Grande_Terre_UTM_58S", "GCS_IGN72_Grande_Terre", "D_IGN72_Grande_Terre");
            IGN72NukuHivaUTMZone7S = ProjectionInfo.FromEpsgCode(2978).SetNames("IGN72_Nuku_Hiva_UTM_7S", "GCS_IGN72_Nuku_Hiva", "D_IGN72_Nuku_Hiva");
            JAD2001UTMZone17N = ProjectionInfo.FromEpsgCode(3449).SetNames("JAD_2001_UTM_Zone_17N", "GCS_JAD_2001", "D_Jamaica_2001");
            JAD2001UTMZone18N = ProjectionInfo.FromEpsgCode(3450).SetNames("JAD_2001_UTM_Zone_18N", "GCS_JAD_2001", "D_Jamaica_2001");
            KerguelenIsland1949UTMZone42S = ProjectionInfo.FromEpsgCode(3336).SetNames("Kerguelen_Island_1949_UTM_42S", "GCS_Kerguelen_Island_1949", "D_Kerguelen_Island_1949");
            Madeira1936UTMZone28N = ProjectionInfo.FromEpsgCode(2191).SetNames("Madeira_1936_UTM_Zone_28N", "GCS_Madeira_1936", "D_Madeira_1936");
            Maupiti1983UTMZone5S = ProjectionInfo.FromEpsgCode(3306).SetNames("Maupiti_1983_UTM_Zone_5S", "GCS_Maupiti_1983", "D_Maupiti_1983");
            Moorea1987UTMZone6S = ProjectionInfo.FromEpsgCode(3305).SetNames("Moorea_1987_UTM_Zone_6S", "GCS_Moorea_1987", "D_Moorea_1987");
            MOP78UTMZone1S = ProjectionInfo.FromEpsgCode(2988).SetNames("MOP78_UTM_1S", "GCS_MOP78", "D_MOP78");
            NEA74NoumeaUTMZone58S = ProjectionInfo.FromEpsgCode(2998).SetNames("NEA74_Noumea_UTM_58S", "GCS_NEA74_Noumea", "D_NEA74_Noumea");
            ObservMeteorologico1939UTMZone25N = ProjectionInfo.FromAuthorityCode("ESRI", 102166).SetNames("Observatorio_Meteorologico_1939_UTM_Zone_25N", "GCS_Observatorio_Meteorologico_1939", "D_Observatorio_Meteorologico_1939");
            OldHawaiianUTMZone4N = ProjectionInfo.FromAuthorityCode("ESRI", 102114).SetNames("Old_Hawaiian_UTM_Zone_4N", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            OldHawaiianUTMZone5N = ProjectionInfo.FromAuthorityCode("ESRI", 102115).SetNames("Old_Hawaiian_UTM_Zone_5N", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            Pitcairn1967UTMZone9S = ProjectionInfo.FromEpsgCode(3784).SetNames("Pitcairn_1967_UTM_Zone_9S", "GCS_Pitcairn_1967", "D_Pitcairn_1967");
            PortoSanto1936UTMZone28N = ProjectionInfo.FromEpsgCode(2942).SetNames("Porto_Santo_1936_UTM_Zone_28N", "GCS_Porto_Santo_1936", "D_Porto_Santo_1936");
            PortoSanto1995UTMZone28N = ProjectionInfo.FromEpsgCode(3061).SetNames("Porto_Santo_1995_UTM_Zone_28N", "GCS_Porto_Santo_1995", "D_Porto_Santo_1995");
            PTRA08UTMZone25N = ProjectionInfo.FromAuthorityCode("EPSG", 102331).SetNames("PTRA08_UTM_Zone_25N", "GCS_PTRA08", "D_PTRA08"); // missing
            PTRA08UTMZone26N = ProjectionInfo.FromAuthorityCode("EPSG", 102332).SetNames("PTRA08_UTM_Zone_26N", "GCS_PTRA08", "D_PTRA08"); // missing
            PTRA08UTMZone28N = ProjectionInfo.FromAuthorityCode("EPSG", 102333).SetNames("PTRA08_UTM_Zone_28N", "GCS_PTRA08", "D_PTRA08"); // missing
            PuertoRicoUTMZone20N = ProjectionInfo.FromEpsgCode(3920).SetNames("Puerto_Rico_UTM_Zone_20N", "GCS_Puerto_Rico", "D_Puerto_Rico");
            RGNC199193UTMZone57S = ProjectionInfo.FromEpsgCode(3169).SetNames("RGNC_1991-93_UTM_Zone_57S", "GCS_RGNC_1991-93", "D_Reseau_Geodesique_de_Nouvelle_Caledonie_1991-93");
            RGNC199193UTMZone58S = ProjectionInfo.FromEpsgCode(3170).SetNames("RGNC_1991-93_UTM_Zone_58S", "GCS_RGNC_1991-93", "D_Reseau_Geodesique_de_Nouvelle_Caledonie_1991-93");
            RGNC199193UTMZone59S = ProjectionInfo.FromEpsgCode(3171).SetNames("RGNC_1991-93_UTM_Zone_59S", "GCS_RGNC_1991-93", "D_Reseau_Geodesique_de_Nouvelle_Caledonie_1991-93");
            RGPFUTMZone5S = ProjectionInfo.FromEpsgCode(3296).SetNames("RGPF_UTM_Zone_5S", "GCS_RGPF", "D_Reseau_Geodesique_de_la_Polynesie_Francaise");
            RGPFUTMZone6S = ProjectionInfo.FromEpsgCode(3297).SetNames("RGPF_UTM_Zone_6S", "GCS_RGPF", "D_Reseau_Geodesique_de_la_Polynesie_Francaise");
            RGPFUTMZone7S = ProjectionInfo.FromEpsgCode(3298).SetNames("RGPF_UTM_Zone_7S", "GCS_RGPF", "D_Reseau_Geodesique_de_la_Polynesie_Francaise");
            RGPFUTMZone8S = ProjectionInfo.FromEpsgCode(3299).SetNames("RGPF_UTM_Zone_8S", "GCS_RGPF", "D_Reseau_Geodesique_de_la_Polynesie_Francaise");
            RGR1992UTMZone40S = ProjectionInfo.FromEpsgCode(2975).SetNames("RGR_1992_UTM_40S", "GCS_RGR_1992", "D_RGR_1992");
            RRAF1991UTMZone20N = ProjectionInfo.FromEpsgCode(2989).SetNames("RRAF_1991_UTM_20N_incorrect_spheroid", "GCS_RRAF_1991_incorrect_spheroid", "D_RRAF_1991_incorrect_spheroid");
            SainteAnneUTMZone20N = ProjectionInfo.FromEpsgCode(2970).SetNames("Sainte_Anne_UTM_20N", "GCS_Sainte_Anne", "D_Sainte_Anne");
            SaoBrazUTMZone26N = ProjectionInfo.FromAuthorityCode("ESRI", 102168).SetNames("Sao_Braz_UTM_Zone_26N", "GCS_Sao_Braz", "D_Sao_Braz");
            SapperHill1943UTMZone20S = ProjectionInfo.FromEpsgCode(29220).SetNames("Sapper_Hill_1943_UTM_Zone_20S", "GCS_Sapper_Hill_1943", "D_Sapper_Hill_1943");
            SapperHill1943UTMZone21S = ProjectionInfo.FromEpsgCode(29221).SetNames("Sapper_Hill_1943_UTM_Zone_21S", "GCS_Sapper_Hill_1943", "D_Sapper_Hill_1943");
            SelvagemGrande1938UTMZone28N = ProjectionInfo.FromEpsgCode(2943).SetNames("Selvagem_Grande_1938_UTM_Zone_28N", "GCS_Selvagem_Grande_1938", "D_Selvagem_Grande_1938");
            ST71BelepUTMZone58S = ProjectionInfo.FromEpsgCode(2997).SetNames("ST71_Belep_UTM_58S", "GCS_ST71_Belep", "D_ST71_Belep");
            ST84IledesPinsUTMZone58S = ProjectionInfo.FromEpsgCode(2996).SetNames("ST84_Ile_des_Pins_UTM_58S", "GCS_ST84_Ile_des_Pins", "D_ST84_Ile_des_Pins");
            ST87OuveaUTMZone58S = ProjectionInfo.FromEpsgCode(3164).SetNames("ST87_Ouvea_UTM_58S", "GCS_ST87_Ouvea", "D_ST87_Ouvea");
            TahaaUTMZone5S = ProjectionInfo.FromEpsgCode(2977).SetNames("Tahaa_1954_UTM_5S", "GCS_Tahaa_1954", "D_Tahaa_1954");
            Tahiti1979UTMZone6S = ProjectionInfo.FromEpsgCode(3304).SetNames("Tahiti_1979_UTM_Zone_6S", "GCS_Tahiti_1979", "D_Tahiti_1979");
            TahitiUTMZone6S = ProjectionInfo.FromEpsgCode(2976).SetNames("Tahiti_1952_UTM_6S", "GCS_Tahiti_1952", "D_Tahiti_1952");
        }

        #endregion
    }
}

#pragma warning restore 1591