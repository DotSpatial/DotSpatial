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
    /// This class contains predefined CoordinateSystems for Oceans.
    /// </summary>
    public class Oceans : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AmericanSamoa1962SamoaLambert;
        public readonly ProjectionInfo Anguilla1957BritishWestIndiesGrid;
        public readonly ProjectionInfo Antigua1943BritishWestIndiesGrid;
        public readonly ProjectionInfo BabSouthPalauAzimuthalEquidistant;
        public readonly ProjectionInfo Barbados1938BarbadosGrid;
        public readonly ProjectionInfo Barbados1938BritishWestIndiesGrid;
        public readonly ProjectionInfo Bermuda2000NationalGrid;
        public readonly ProjectionInfo Dominica1945BritishWestIndiesGrid;
        public readonly ProjectionInfo Fiji1986FijiMapGrid;
        public readonly ProjectionInfo Grenada1953BritishWestIndiesGrid;
        public readonly ProjectionInfo Guam1963YapIslands;
        public readonly ProjectionInfo JAD2001JamaicaGrid;
        public readonly ProjectionInfo Jamaica1875OldGrid;
        public readonly ProjectionInfo JamaicaGrid;
        public readonly ProjectionInfo LePouce1934MauritiusGrid;
        public readonly ProjectionInfo Montserrat1958BritishWestIndiesGrid;
        public readonly ProjectionInfo MountDillonTobagoGrid;
        public readonly ProjectionInfo NAD1927CubaNorte;
        public readonly ProjectionInfo NAD1927CubaSur;
        public readonly ProjectionInfo NEA74NoumeaLambert;
        public readonly ProjectionInfo NEA74NoumeaLambert2;
        public readonly ProjectionInfo Pitcairn2006PitcairnTM2006;
        public readonly ProjectionInfo PohnpeiAzEq1971;
        public readonly ProjectionInfo Reunion1947TMReunion;
        public readonly ProjectionInfo RGNC199193LambertNewCaledonia;
        public readonly ProjectionInfo RGNC1991LambertNewCaledonia;
        public readonly ProjectionInfo SaipanAzEq1969;
        public readonly ProjectionInfo StKitts1955BritishWestIndiesGrid;
        public readonly ProjectionInfo StLucia1955BritishWestIndiesGrid;
        public readonly ProjectionInfo StVincent1945BritishWestIndiesGrid;
        public readonly ProjectionInfo WGS1984CapeVerdeGrid;
        public readonly ProjectionInfo WGS1984SouthGeorgiaLambert;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Oceans.
        /// </summary>
        public Oceans()
        {
            AmericanSamoa1962SamoaLambert = ProjectionInfo.FromEpsgCode(3102).SetNames("Samoa_1962_Samoa_Lambert", "GCS_American_Samoa_1962", "D_American_Samoa_1962");
            Anguilla1957BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2000).SetNames("Anguilla_1957_British_West_Indies_Grid", "GCS_Anguilla_1957", "D_Anguilla_1957");
            Antigua1943BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2001).SetNames("Antigua_1943_British_West_Indies_Grid", "GCS_Antigua_1943", "D_Antigua_1943");
            BabSouthPalauAzimuthalEquidistant = ProjectionInfo.FromAuthorityCode("ESRI", 102096).SetNames("Bab_South_Palau_Azimuthal_Equidistant", "GCS_Bab_South", "D_Bab_South"); // missing
            Barbados1938BarbadosGrid = ProjectionInfo.FromEpsgCode(21292).SetNames("Barbados_1938_Barbados_Grid", "GCS_Barbados_1938", "D_Barbados_1938");
            Barbados1938BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(21291).SetNames("Barbados_1938_British_West_Indies_Grid", "GCS_Barbados_1938", "D_Barbados_1938");
            Bermuda2000NationalGrid = ProjectionInfo.FromEpsgCode(3770).SetNames("Bermuda_2000_National_Grid", "GCS_Bermuda_2000", "D_Bermuda_2000");
            Dominica1945BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2002).SetNames("Dominica_1945_British_West_Indies_Grid", "GCS_Dominica_1945", "D_Dominica_1945");
            Fiji1986FijiMapGrid = ProjectionInfo.FromEpsgCode(3460).SetNames("Fiji_1986_Fiji_Map_Grid", "GCS_Fiji_1986", "D_Fiji_1986");
            Grenada1953BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2003).SetNames("Grenada_1953_British_West_Indies_Grid", "GCS_Grenada_1953", "D_Grenada_1953");
            Guam1963YapIslands = ProjectionInfo.FromAuthorityCode("EPSG", 3295).SetNames("Guam_1963_Yap_Islands", "GCS_Guam_1963", "D_Guam_1963"); // missing
            JAD2001JamaicaGrid = ProjectionInfo.FromEpsgCode(3448).SetNames("JAD_2001_Jamaica_Grid", "GCS_JAD_2001", "D_Jamaica_2001");
            Jamaica1875OldGrid = ProjectionInfo.FromEpsgCode(24100).SetNames("Jamaica_1875_Old_Grid", "GCS_Jamaica_1875", "D_Jamaica_1875");
            JamaicaGrid = ProjectionInfo.FromEpsgCode(24200).SetNames("Jamaica_Grid", "GCS_Jamaica_1969", "D_Jamaica_1969");
            LePouce1934MauritiusGrid = ProjectionInfo.FromEpsgCode(3337).SetNames("Le_Pouce_1934_Mauritius_Grid", "GCS_Le_Pouce_1934", "D_Le_Pouce_1934");
            Montserrat1958BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2004).SetNames("Montserrat_1958_British_West_Indies_Grid", "GCS_Montserrat_1958", "D_Montserrat_1958");
            MountDillonTobagoGrid = ProjectionInfo.FromEpsgCode(2066).SetNames("Mount_Dillon_Tobago_Grid", "GCS_Mount_Dillon", "D_Mount_Dillon");
            NAD1927CubaNorte = ProjectionInfo.FromEpsgCode(2085).SetNames("NAD_1927_Cuba_Norte", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927CubaSur = ProjectionInfo.FromEpsgCode(2086).SetNames("NAD_1927_Cuba_Sur", "GCS_North_American_1927", "D_North_American_1927");
            NEA74NoumeaLambert = ProjectionInfo.FromEpsgCode(3165).SetNames("NEA74_Noumea_Lambert", "GCS_NEA74_Noumea", "D_NEA74_Noumea");
            NEA74NoumeaLambert2 = ProjectionInfo.FromEpsgCode(3166).SetNames("NEA74_Noumea_Lambert_2", "GCS_NEA74_Noumea", "D_NEA74_Noumea");
            Pitcairn2006PitcairnTM2006 = ProjectionInfo.FromEpsgCode(3783).SetNames("Pitcairn_2006_Pitcairn_TM_2006", "GCS_Pitcairn_2006", "D_Pitcairn_2006");
            PohnpeiAzEq1971 = ProjectionInfo.FromAuthorityCode("ESRI", 102237).SetNames("Pohnpei_Az_Eq_1971", "GCS_Pohnpei", "D_Pohnpei"); // missing
            Reunion1947TMReunion = ProjectionInfo.FromEpsgCode(3727).SetNames("Reunion_1947_TM_Reunion", "GCS_Reunion_1947", "D_Reunion_1947");
            RGNC199193LambertNewCaledonia = ProjectionInfo.FromEpsgCode(3163).SetNames("RGNC_1991_93_Lambert_New_Caledonia", "GCS_RGNC_1991-93", "D_Reseau_Geodesique_de_Nouvelle_Caledonie_1991-93");
            RGNC1991LambertNewCaledonia = ProjectionInfo.FromEpsgCode(2984).SetNames("RGNC_1991_Lambert_New_Caledonia", "GCS_RGNC_1991", "D_RGNC_1991");
            SaipanAzEq1969 = ProjectionInfo.FromAuthorityCode("ESRI", 102238).SetNames("Saipan_Az_Eq_1969", "GCS_Guam_1963", "D_Guam_1963"); // missing
            StKitts1955BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2005).SetNames("St_Kitts_1955_British_West_Indies_Grid", "GCS_St_Kitts_1955", "D_St_Kitts_1955");
            StLucia1955BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2006).SetNames("St_Lucia_1955_British_West_Indies_Grid", "GCS_St_Lucia_1955", "D_St_Lucia_1955");
            StVincent1945BritishWestIndiesGrid = ProjectionInfo.FromEpsgCode(2007).SetNames("St_Vincent_1945_British_West_Indies_Grid", "GCS_St_Vincent_1945", "D_St_Vincent_1945");
            WGS1984CapeVerdeGrid = ProjectionInfo.FromAuthorityCode("EPSG", 102214).SetNames("WGS_1984_Cape_Verde_Grid", "GCS_WGS_1984", "D_WGS_1984"); // missing
            WGS1984SouthGeorgiaLambert = ProjectionInfo.FromEpsgCode(3762).SetNames("WGS_1984_South_Georgia_Lambert", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591