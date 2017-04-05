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

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for AtlanticOcean.
    /// </summary>
    public class AtlanticOcean : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AscensionIsland1958;
        public readonly ProjectionInfo AstroDOS714;
        public readonly ProjectionInfo AzoresCentral1948;
        public readonly ProjectionInfo AzoresCentral1995;
        public readonly ProjectionInfo AzoresOccidental1939;
        public readonly ProjectionInfo AzoresOriental1940;
        public readonly ProjectionInfo AzoresOriental1995;
        public readonly ProjectionInfo Bermuda1957;
        public readonly ProjectionInfo Bermuda2000;
        public readonly ProjectionInfo GraciosaBaseSW1948;
        public readonly ProjectionInfo Hjorsey1955;
        public readonly ProjectionInfo ISTS061Astro1968;
        public readonly ProjectionInfo Madeira1936;
        public readonly ProjectionInfo ObservMeteorologico1939;
        public readonly ProjectionInfo PicodeLasNieves;
        public readonly ProjectionInfo PortoSanto1936;
        public readonly ProjectionInfo PortoSanto1995;
        public readonly ProjectionInfo Principe;
        public readonly ProjectionInfo PTRA08;
        public readonly ProjectionInfo REGCAN95;
        public readonly ProjectionInfo RGSPM06;
        public readonly ProjectionInfo SaoBraz;
        public readonly ProjectionInfo SaoTome;
        public readonly ProjectionInfo SapperHill1943;
        public readonly ProjectionInfo SelvagemGrande1938;
        public readonly ProjectionInfo TristanAstro1968;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AtlanticOcean.
        /// </summary>
        public AtlanticOcean()
        {
            AscensionIsland1958 = ProjectionInfo.FromEpsgCode(4712).SetNames("", "GCS_Ascension_Island_1958", "D_Ascension_Island_1958");
            AstroDOS714 = ProjectionInfo.FromEpsgCode(4710).SetNames("", "GCS_DOS_71_4", "D_DOS_71_4");
            AzoresCentral1948 = ProjectionInfo.FromEpsgCode(4183).SetNames("", "GCS_Azores_Central_1948", "D_Azores_Central_Islands_1948");
            AzoresCentral1995 = ProjectionInfo.FromEpsgCode(4665).SetNames("", "GCS_Azores_Central_1995", "D_Azores_Central_Islands_1995");
            AzoresOccidental1939 = ProjectionInfo.FromEpsgCode(4182).SetNames("", "GCS_Azores_Occidental_1939", "D_Azores_Occidental_Islands_1939");
            AzoresOriental1940 = ProjectionInfo.FromEpsgCode(4184).SetNames("", "GCS_Azores_Oriental_1940", "D_Azores_Oriental_Islands_1940");
            AzoresOriental1995 = ProjectionInfo.FromEpsgCode(4664).SetNames("", "GCS_Azores_Oriental_1995", "D_Azores_Oriental_Islands_1995");
            Bermuda1957 = ProjectionInfo.FromEpsgCode(4216).SetNames("", "GCS_Bermuda_1957", "D_Bermuda_1957");
            Bermuda2000 = ProjectionInfo.FromEpsgCode(4762).SetNames("", "GCS_Bermuda_2000", "D_Bermuda_2000");
            GraciosaBaseSW1948 = ProjectionInfo.FromAuthorityCode("ESRI", 37241).SetNames("", "GCS_Graciosa_Base_SW_1948", "D_Graciosa_Base_SW_1948");
            Hjorsey1955 = ProjectionInfo.FromEpsgCode(4658).SetNames("", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            ISTS061Astro1968 = ProjectionInfo.FromEpsgCode(4722).SetNames("", "GCS_ISTS_061_1968", "D_ISTS_061_1968");
            Madeira1936 = ProjectionInfo.FromEpsgCode(4185).SetNames("", "GCS_Madeira_1936", "D_Madeira_1936");
            ObservMeteorologico1939 = ProjectionInfo.FromAuthorityCode("ESRI", 37245).SetNames("", "GCS_Observatorio_Meteorologico_1939", "D_Observatorio_Meteorologico_1939");
            PicodeLasNieves = ProjectionInfo.FromEpsgCode(4728).SetNames("", "GCS_Pico_de_Las_Nieves", "D_Pico_de_Las_Nieves");
            PortoSanto1936 = ProjectionInfo.FromEpsgCode(4615).SetNames("", "GCS_Porto_Santo_1936", "D_Porto_Santo_1936");
            PortoSanto1995 = ProjectionInfo.FromEpsgCode(4663).SetNames("", "GCS_Porto_Santo_1995", "D_Porto_Santo_1995");
            Principe = ProjectionInfo.FromEpsgCode(4824).SetNames("", "GCS_Principe", "D_Principe");
            PTRA08 = ProjectionInfo.FromAuthorityCode("EPSG", 104142).SetNames("", "GCS_PTRA08", "D_PTRA08"); // missing
            REGCAN95 = ProjectionInfo.FromEpsgCode(4081).SetNames("", "GCS_REGCAN95", "D_Red_Geodesica_de_Canarias_1995");
            RGSPM06 = ProjectionInfo.FromAuthorityCode("EPSG", 4466).SetNames("", "GCS_RGSPM_2006", "D_Reseau_Geodesique_de_St_Pierre_et_Miquelon_2006"); // missing
            SaoBraz = ProjectionInfo.FromAuthorityCode("ESRI", 37249).SetNames("", "GCS_Sao_Braz", "D_Sao_Braz");
            SaoTome = ProjectionInfo.FromEpsgCode(4823).SetNames("", "GCS_Sao_Tome", "D_Sao_Tome");
            SapperHill1943 = ProjectionInfo.FromEpsgCode(4292).SetNames("", "GCS_Sapper_Hill_1943", "D_Sapper_Hill_1943");
            SelvagemGrande1938 = ProjectionInfo.FromEpsgCode(4616).SetNames("", "GCS_Selvagem_Grande_1938", "D_Selvagem_Grande_1938");
            TristanAstro1968 = ProjectionInfo.FromEpsgCode(4734).SetNames("", "GCS_Tristan_1968", "D_Tristan_1968");
        }

        #endregion
    }
}

#pragma warning restore 1591