// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:15:48 PM
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
    /// This class contains predefined CoordinateSystems for PacificOcean.
    /// </summary>
    public class PacificOcean : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AlaskanIslands;
        public readonly ProjectionInfo AmericanSamoa1962;
        public readonly ProjectionInfo AstroBeaconE1945;
        public readonly ProjectionInfo AstronomicalStation1952;
        public readonly ProjectionInfo BabSouth;
        public readonly ProjectionInfo BellevueIGN;
        public readonly ProjectionInfo CantonAstro1966;
        public readonly ProjectionInfo ChathamIslandAstro1971;
        public readonly ProjectionInfo DOS1968;
        public readonly ProjectionInfo EasterIsland1967;
        public readonly ProjectionInfo FatuIva1972;
        public readonly ProjectionInfo Fiji1956;
        public readonly ProjectionInfo Fiji1986;
        public readonly ProjectionInfo Guam1963;
        public readonly ProjectionInfo GUX1Astro;
        public readonly ProjectionInfo IGN53Mare;
        public readonly ProjectionInfo IGN56Lifou;
        public readonly ProjectionInfo IGN63HivaOa;
        public readonly ProjectionInfo IGN72GrandeTerre;
        public readonly ProjectionInfo IGN72NukuHiva;
        public readonly ProjectionInfo JohnstonIsland1961;
        public readonly ProjectionInfo KusaieAstro1951;
        public readonly ProjectionInfo Majuro;
        public readonly ProjectionInfo Maupiti1983;
        public readonly ProjectionInfo MidwayAstro1961;
        public readonly ProjectionInfo Moorea1987;
        public readonly ProjectionInfo MOP78;
        public readonly ProjectionInfo NAD1983MARP00;
        public readonly ProjectionInfo NAD1983PACP00;
        public readonly ProjectionInfo NEA74Noumea;
        public readonly ProjectionInfo OldHawaiian;
        public readonly ProjectionInfo OldHawaiianIntl1924;
        public readonly ProjectionInfo Pitcairn2006;
        public readonly ProjectionInfo PitcairnAstro1967;
        public readonly ProjectionInfo Pohnpei;
        public readonly ProjectionInfo RGNC1991;
        public readonly ProjectionInfo RGNC199193;
        public readonly ProjectionInfo RGPF;
        public readonly ProjectionInfo SantoDOS1965;
        public readonly ProjectionInfo Solomon1968;
        public readonly ProjectionInfo ST71Belep;
        public readonly ProjectionInfo ST84IledesPins;
        public readonly ProjectionInfo ST87Ouvea;
        public readonly ProjectionInfo Tahaa1954;
        public readonly ProjectionInfo Tahiti;
        public readonly ProjectionInfo Tahiti1979;
        public readonly ProjectionInfo TernIslandAstro1961;
        public readonly ProjectionInfo VanuaLevu1915;
        public readonly ProjectionInfo VitiLevu1912;
        public readonly ProjectionInfo VitiLevu1916;
        public readonly ProjectionInfo WakeEniwetok1960;
        public readonly ProjectionInfo WakeIslandAstro1952;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PacificOcean.
        /// </summary>
        public PacificOcean()
        {
            AlaskanIslands = ProjectionInfo.FromAuthorityCode("ESRI", 37260).SetNames("", "GCS_Alaskan_Islands", "D_Alaskan_Islands");
            AmericanSamoa1962 = ProjectionInfo.FromEpsgCode(4169).SetNames("", "GCS_American_Samoa_1962", "D_American_Samoa_1962");
            AstroBeaconE1945 = ProjectionInfo.FromEpsgCode(4709).SetNames("", "GCS_Beacon_E_1945", "D_Beacon_E_1945");
            AstronomicalStation1952 = ProjectionInfo.FromEpsgCode(4711).SetNames("", "GCS_Astro_1952", "D_Astro_1952");
            BabSouth = ProjectionInfo.FromAuthorityCode("ESRI", 104112).SetNames("", "GCS_Bab_South", "D_Bab_South"); // missing
            BellevueIGN = ProjectionInfo.FromEpsgCode(4714).SetNames("", "GCS_Bellevue_IGN", "D_Bellevue_IGN");
            CantonAstro1966 = ProjectionInfo.FromEpsgCode(4716).SetNames("", "GCS_Canton_1966", "D_Canton_1966");
            ChathamIslandAstro1971 = ProjectionInfo.FromEpsgCode(4672).SetNames("", "GCS_Chatham_Island_1971", "D_Chatham_Island_1971");
            DOS1968 = ProjectionInfo.FromAuthorityCode("ESRI", 37218).SetNames("", "GCS_DOS_1968", "D_DOS_1968");
            EasterIsland1967 = ProjectionInfo.FromEpsgCode(4719).SetNames("", "GCS_Easter_Island_1967", "D_Easter_Island_1967");
            FatuIva1972 = ProjectionInfo.FromEpsgCode(4688).SetNames("", "GCS_Fatu_Iva_1972", "D_Fatu_Iva_1972");
            Fiji1956 = ProjectionInfo.FromEpsgCode(4721).SetNames("", "GCS_Fiji_1956", "D_Fiji_1956");
            Fiji1986 = ProjectionInfo.FromEpsgCode(4720).SetNames("", "GCS_Fiji_1986", "D_Fiji_1986");
            Guam1963 = ProjectionInfo.FromEpsgCode(4675).SetNames("", "GCS_Guam_1963", "D_Guam_1963");
            GUX1Astro = ProjectionInfo.FromAuthorityCode("ESRI", 37221).SetNames("", "GCS_GUX_1", "D_GUX_1");
            IGN53Mare = ProjectionInfo.FromEpsgCode(4641).SetNames("", "GCS_IGN53_Mare", "D_IGN53_Mare");
            IGN56Lifou = ProjectionInfo.FromEpsgCode(4633).SetNames("", "GCS_IGN56_Lifou", "D_IGN56_Lifou");
            IGN63HivaOa = ProjectionInfo.FromEpsgCode(4689).SetNames("", "GCS_IGN63_Hiva_Oa", "D_IGN63_Hiva_Oa");
            IGN72GrandeTerre = ProjectionInfo.FromEpsgCode(4662).SetNames("", "GCS_IGN72_Grande_Terre", "D_IGN72_Grande_Terre");
            IGN72NukuHiva = ProjectionInfo.FromEpsgCode(4630).SetNames("", "GCS_IGN72_Nuku_Hiva", "D_IGN72_Nuku_Hiva");
            JohnstonIsland1961 = ProjectionInfo.FromEpsgCode(4725).SetNames("", "GCS_Johnston_Island_1961", "D_Johnston_Island_1961");
            KusaieAstro1951 = ProjectionInfo.FromEpsgCode(4735).SetNames("", "GCS_Kusaie_1951", "D_Kusaie_1951");
            Majuro = ProjectionInfo.FromAuthorityCode("ESRI", 104113).SetNames("", "GCS_Majuro", "D_Majuro"); // missing
            Maupiti1983 = ProjectionInfo.FromEpsgCode(4692).SetNames("", "GCS_Maupiti_1983", "D_Maupiti_1983");
            MidwayAstro1961 = ProjectionInfo.FromEpsgCode(4727).SetNames("", "GCS_Midway_1961", "D_Midway_1961");
            Moorea1987 = ProjectionInfo.FromEpsgCode(4691).SetNames("", "GCS_Moorea_1987", "D_Moorea_1987");
            MOP78 = ProjectionInfo.FromEpsgCode(4639).SetNames("", "GCS_MOP78", "D_MOP78");
            NAD1983MARP00 = ProjectionInfo.FromAuthorityCode("ESRI", 104260).SetNames("", "GCS_NAD_1983_MARP00", "D_NAD_1983_MARP00"); // missing
            NAD1983PACP00 = ProjectionInfo.FromAuthorityCode("ESRI", 104259).SetNames("", "GCS_NAD_1983_PACP00", "D_NAD_1983_PACP00"); // missing
            NEA74Noumea = ProjectionInfo.FromEpsgCode(4644).SetNames("", "GCS_NEA74_Noumea", "D_NEA74_Noumea");
            OldHawaiian = ProjectionInfo.FromEpsgCode(4135).SetNames("", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            OldHawaiianIntl1924 = ProjectionInfo.FromAuthorityCode("ESRI", 104138).SetNames("", "GCS_Old_Hawaiian_Intl_1924", "D_Old_Hawaiian_Intl_1924"); // missing
            Pitcairn2006 = ProjectionInfo.FromEpsgCode(4763).SetNames("", "GCS_Pitcairn_2006", "D_Pitcairn_2006");
            PitcairnAstro1967 = ProjectionInfo.FromEpsgCode(4729).SetNames("", "GCS_Pitcairn_1967", "D_Pitcairn_1967");
            Pohnpei = ProjectionInfo.FromAuthorityCode("ESRI", 104109).SetNames("", "GCS_Pohnpei", "D_Pohnpei"); // missing
            RGNC1991 = ProjectionInfo.FromEpsgCode(4645).SetNames("", "GCS_RGNC_1991", "D_RGNC_1991");
            RGNC199193 = ProjectionInfo.FromEpsgCode(4749).SetNames("", "GCS_RGNC_1991-93", "D_Reseau_Geodesique_de_Nouvelle_Caledonie_1991-93");
            RGPF = ProjectionInfo.FromEpsgCode(4687).SetNames("", "GCS_RGPF", "D_Reseau_Geodesique_de_la_Polynesie_Francaise");
            SantoDOS1965 = ProjectionInfo.FromEpsgCode(4730).SetNames("", "GCS_Santo_DOS_1965", "D_Santo_DOS_1965");
            Solomon1968 = ProjectionInfo.FromEpsgCode(4718).SetNames("", "GCS_Solomon_1968", "D_Solomon_1968");
            ST71Belep = ProjectionInfo.FromEpsgCode(4643).SetNames("", "GCS_ST71_Belep", "D_ST71_Belep");
            ST84IledesPins = ProjectionInfo.FromEpsgCode(4642).SetNames("", "GCS_ST84_Ile_des_Pins", "D_ST84_Ile_des_Pins");
            ST87Ouvea = ProjectionInfo.FromEpsgCode(4750).SetNames("", "GCS_ST87_Ouvea", "D_ST87_Ouvea");
            Tahaa1954 = ProjectionInfo.FromEpsgCode(4629).SetNames("", "GCS_Tahaa_1954", "D_Tahaa_1954");
            Tahiti = ProjectionInfo.FromEpsgCode(4628).SetNames("", "GCS_Tahiti_1952", "D_Tahiti_1952");
            Tahiti1979 = ProjectionInfo.FromEpsgCode(4690).SetNames("", "GCS_Tahiti_1979", "D_Tahiti_1979");
            TernIslandAstro1961 = ProjectionInfo.FromEpsgCode(4707).SetNames("", "GCS_Tern_Island_1961", "D_Tern_Island_1961");
            VanuaLevu1915 = ProjectionInfo.FromEpsgCode(4748).SetNames("", "GCS_Vanua_Levu_1915", "D_Vanua_Levu_1915");
            VitiLevu1912 = ProjectionInfo.FromEpsgCode(4752).SetNames("", "GCS_Viti_Levu_1912", "D_Viti_Levu_1912");
            VitiLevu1916 = ProjectionInfo.FromEpsgCode(4731).SetNames("", "GCS_Viti_Levu_1916", "D_Viti_Levu_1916");
            WakeEniwetok1960 = ProjectionInfo.FromEpsgCode(4732).SetNames("", "GCS_Wake_Eniwetok_1960", "D_Wake_Eniwetok_1960");
            WakeIslandAstro1952 = ProjectionInfo.FromEpsgCode(4733).SetNames("", "GCS_Wake_Island_1952", "D_Wake_Island_1952");
        }

        #endregion
    }
}

#pragma warning restore 1591