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
    /// Oceans
    /// </summary>
    public class Oceans : CoordinateSystemCategory
    {
        #region Fields

        public readonly ProjectionInfo AlaskanIslands;
        public readonly ProjectionInfo AmericanSamoa1962;
        public readonly ProjectionInfo Anguilla1957;
        public readonly ProjectionInfo Anna1Astro1965;
        public readonly ProjectionInfo Antigua1943;
        public readonly ProjectionInfo AscensionIsland1958;
        public readonly ProjectionInfo AstroBeaconE1945;
        public readonly ProjectionInfo AstroDOS714;
        public readonly ProjectionInfo AstronomicalStation1952;
        public readonly ProjectionInfo AzoresCentral1948;
        public readonly ProjectionInfo AzoresCentral1995;
        public readonly ProjectionInfo AzoresOccidental1939;
        public readonly ProjectionInfo AzoresOriental1940;
        public readonly ProjectionInfo AzoresOriental1995;
        public readonly ProjectionInfo BabSouth;
        public readonly ProjectionInfo Barbados;
        public readonly ProjectionInfo Barbados1938;
        public readonly ProjectionInfo BellevueIGN;
        public readonly ProjectionInfo Bermuda1957;
        public readonly ProjectionInfo Bermuda2000;
        public readonly ProjectionInfo CSG1967;
        public readonly ProjectionInfo CantonAstro1966;
        public readonly ProjectionInfo ChathamIslandAstro1971;
        public readonly ProjectionInfo Combani1950;
        public readonly ProjectionInfo DOS1968;
        public readonly ProjectionInfo Dominica1945;
        public readonly ProjectionInfo EasterIsland1967;
        public readonly ProjectionInfo FortDesaix;
        public readonly ProjectionInfo FortMarigot;
        public readonly ProjectionInfo FortThomas1955;
        public readonly ProjectionInfo GUX1Astro;
        public readonly ProjectionInfo Gan1970;
        public readonly ProjectionInfo GraciosaBaseSW1948;
        public readonly ProjectionInfo GrandComoros;
        public readonly ProjectionInfo Grenada1953;
        public readonly ProjectionInfo Guam1963;
        public readonly ProjectionInfo Hjorsey1955;
        public readonly ProjectionInfo IGN53Mare;
        public readonly ProjectionInfo IGN56Lifou;
        public readonly ProjectionInfo IGN72GrandeTerre;
        public readonly ProjectionInfo IGN72NukuHiva;
        public readonly ProjectionInfo ISTS061Astro1968;
        public readonly ProjectionInfo ISTS073Astro1969;
        public readonly ProjectionInfo Jamaica1875;
        public readonly ProjectionInfo Jamaica1969;
        public readonly ProjectionInfo JohnstonIsland1961;
        public readonly ProjectionInfo K01949;
        public readonly ProjectionInfo KerguelenIsland1949;
        public readonly ProjectionInfo KusaieAstro1951;
        public readonly ProjectionInfo LC5Astro1961;
        public readonly ProjectionInfo MOP78;
        public readonly ProjectionInfo Madeira1936;
        public readonly ProjectionInfo Mahe1971;
        public readonly ProjectionInfo Majuro;
        public readonly ProjectionInfo MidwayAstro1961;
        public readonly ProjectionInfo Montserrat1958;
        public readonly ProjectionInfo NEA74Noumea;
        public readonly ProjectionInfo ObservMeteorologico1939;
        public readonly ProjectionInfo OldHawaiian;
        public readonly ProjectionInfo PicodeLasNieves;
        public readonly ProjectionInfo PitcairnAstro1967;
        public readonly ProjectionInfo PitondesNeiges;
        public readonly ProjectionInfo Pohnpei;
        public readonly ProjectionInfo PortoSanto1936;
        public readonly ProjectionInfo PortoSanto1995;
        public readonly ProjectionInfo PuertoRico;
        public readonly ProjectionInfo RGFG1995;
        public readonly ProjectionInfo RGNC1991;
        public readonly ProjectionInfo RGR1992;
        public readonly ProjectionInfo RRAF1991;
        public readonly ProjectionInfo Reunion;
        public readonly ProjectionInfo ST71Belep;
        public readonly ProjectionInfo ST84IledesPins;
        public readonly ProjectionInfo ST87Ouvea;
        public readonly ProjectionInfo SaintPierreetMiquelon1950;
        public readonly ProjectionInfo SainteAnne;
        public readonly ProjectionInfo SantoDOS1965;
        public readonly ProjectionInfo SaoBraz;
        public readonly ProjectionInfo SapperHill1943;
        public readonly ProjectionInfo SelvagemGrande1938;
        public readonly ProjectionInfo StKitts1955;
        public readonly ProjectionInfo StLucia1955;
        public readonly ProjectionInfo StVincent1945;
        public readonly ProjectionInfo Tahaa;
        public readonly ProjectionInfo Tahiti;
        public readonly ProjectionInfo TernIslandAstro1961;
        public readonly ProjectionInfo TristanAstro1968;
        public readonly ProjectionInfo VitiLevu1916;
        public readonly ProjectionInfo WakeEniwetok1960;
        public readonly ProjectionInfo WakeIslandAstro1952;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Oceans
        /// </summary>
        public Oceans()
        {
            AlaskanIslands = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            AmericanSamoa1962 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Anguilla1957 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Anna1Astro1965 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=aust_SA +no_defs ");
            Antigua1943 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            AscensionIsland1958 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AstroBeaconE1945 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AstroDOS714 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AstronomicalStation1952 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AzoresCentral1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AzoresCentral1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AzoresOccidental1939 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AzoresOriental1940 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            AzoresOriental1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            BabSouth = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Barbados = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Barbados1938 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            BellevueIGN = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Bermuda1957 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Bermuda2000 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            CantonAstro1966 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ChathamIslandAstro1971 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Combani1950 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            CSG1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Dominica1945 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            DOS1968 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            EasterIsland1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            FortDesaix = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            FortMarigot = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            FortThomas1955 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Gan1970 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            GraciosaBaseSW1948 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            GrandComoros = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Grenada1953 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Guam1963 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            GUX1Astro = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Hjorsey1955 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            IGN53Mare = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            IGN56Lifou = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            IGN72GrandeTerre = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            IGN72NukuHiva = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ISTS061Astro1968 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ISTS073Astro1969 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Jamaica1875 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.138 +b=6356514.959419348 +no_defs ");
            Jamaica1969 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            JohnstonIsland1961 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            K01949 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            KerguelenIsland1949 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            KusaieAstro1951 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            LC5Astro1961 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Madeira1936 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Mahe1971 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Majuro = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            MidwayAstro1961 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Montserrat1958 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            MOP78 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            NEA74Noumea = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ObservMeteorologico1939 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            OldHawaiian = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            PicodeLasNieves = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            PitcairnAstro1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            PitondesNeiges = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Pohnpei = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            PortoSanto1936 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            PortoSanto1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            PuertoRico = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Reunion = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            RGFG1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            RGNC1991 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            RGR1992 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            RRAF1991 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            SaintPierreetMiquelon1950 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            SainteAnne = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            SantoDOS1965 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            SaoBraz = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            SapperHill1943 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            SelvagemGrande1938 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            StKitts1955 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            StLucia1955 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            StVincent1945 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            ST71Belep = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ST84IledesPins = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ST87Ouvea = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Tahaa = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Tahiti = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            TernIslandAstro1961 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            TristanAstro1968 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            VitiLevu1916 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            WakeIslandAstro1952 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            WakeEniwetok1960 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378270 +b=6356794.343434343 +no_defs ");

            AlaskanIslands.GeographicInfo.Name = "GCS_Alaskan_Islands";
            AmericanSamoa1962.GeographicInfo.Name = "GCS_American_Samoa_1962";
            Anguilla1957.GeographicInfo.Name = "GCS_Anguilla_1957";
            Anna1Astro1965.GeographicInfo.Name = "GCS_Anna_1_1965";
            Antigua1943.GeographicInfo.Name = "GCS_Antigua_1943";
            AscensionIsland1958.GeographicInfo.Name = "GCS_Ascension_Island_1958";
            AstroBeaconE1945.GeographicInfo.Name = "GCS_Beacon_E_1945";
            AstroDOS714.GeographicInfo.Name = "GCS_DOS_71_4";
            AstronomicalStation1952.GeographicInfo.Name = "GCS_Astro_1952";
            AzoresCentral1948.GeographicInfo.Name = "GCS_Azores_Central_1948";
            AzoresCentral1995.GeographicInfo.Name = "GCS_Azores_Central_1995";
            AzoresOccidental1939.GeographicInfo.Name = "GCS_Azores_Occidental_1939";
            AzoresOriental1940.GeographicInfo.Name = "GCS_Azores_Oriental_1940";
            AzoresOriental1995.GeographicInfo.Name = "GCS_Azores_Oriental_1995";
            BabSouth.GeographicInfo.Name = "GCS_Bab_South";
            Barbados.GeographicInfo.Name = "GCS_Barbados";
            Barbados1938.GeographicInfo.Name = "GCS_Barbados_1938";
            BellevueIGN.GeographicInfo.Name = "GCS_Bellevue_IGN";
            Bermuda1957.GeographicInfo.Name = "GCS_Bermuda_1957";
            Bermuda2000.GeographicInfo.Name = "GCS_Bermuda_2000";
            CantonAstro1966.GeographicInfo.Name = "GCS_Canton_1966";
            ChathamIslandAstro1971.GeographicInfo.Name = "GCS_Chatham_Island_1971";
            Combani1950.GeographicInfo.Name = "GCS_Combani_1950";
            CSG1967.GeographicInfo.Name = "GCS_CSG_1967";
            Dominica1945.GeographicInfo.Name = "GCS_Dominica_1945";
            DOS1968.GeographicInfo.Name = "GCS_DOS_1968";
            EasterIsland1967.GeographicInfo.Name = "GCS_Easter_Island_1967";
            FortDesaix.GeographicInfo.Name = "GCS_Fort_Desaix";
            FortMarigot.GeographicInfo.Name = "GCS_Fort_Marigot";
            FortThomas1955.GeographicInfo.Name = "GCS_Fort_Thomas_1955";
            Gan1970.GeographicInfo.Name = "GCS_Gan_1970";
            GraciosaBaseSW1948.GeographicInfo.Name = "GCS_Graciosa_Base_SW_1948";
            GrandComoros.GeographicInfo.Name = "GCS_Grand_Comoros";
            Grenada1953.GeographicInfo.Name = "GCS_Grenada_1953";
            Guam1963.GeographicInfo.Name = "GCS_Guam_1963";
            GUX1Astro.GeographicInfo.Name = "GCS_GUX_1";
            Hjorsey1955.GeographicInfo.Name = "GCS_Hjorsey_1955";
            IGN53Mare.GeographicInfo.Name = "GCS_IGN53_Mare";
            IGN56Lifou.GeographicInfo.Name = "GCS_IGN56_Lifou";
            IGN72GrandeTerre.GeographicInfo.Name = "GCS_IGN72_Grande_Terre";
            IGN72NukuHiva.GeographicInfo.Name = "GCS_IGN72_Nuku_Hiva";
            ISTS061Astro1968.GeographicInfo.Name = "GCS_ISTS_061_1968";
            ISTS073Astro1969.GeographicInfo.Name = "GCS_ISTS_073_1969";
            Jamaica1875.GeographicInfo.Name = "GCS_Jamaica_1875";
            Jamaica1969.GeographicInfo.Name = "GCS_Jamaica_1969";
            JohnstonIsland1961.GeographicInfo.Name = "GCS_Johnston_Island_1961";
            K01949.GeographicInfo.Name = "GCS_K0_1949";
            KerguelenIsland1949.GeographicInfo.Name = "GCS_Kerguelen_Island_1949";
            KusaieAstro1951.GeographicInfo.Name = "GCS_Kusaie_1951";
            LC5Astro1961.GeographicInfo.Name = "GCS_LC5_1961";
            Madeira1936.GeographicInfo.Name = "GCS_Madeira_1936";
            Mahe1971.GeographicInfo.Name = "GCS_Mahe_1971";
            Majuro.GeographicInfo.Name = "GCS_Majuro";
            MidwayAstro1961.GeographicInfo.Name = "GCS_Midway_1961";
            Montserrat1958.GeographicInfo.Name = "GCS_Montserrat_1958";
            MOP78.GeographicInfo.Name = "GCS_MOP78";
            NEA74Noumea.GeographicInfo.Name = "GCS_NEA74_Noumea";
            ObservMeteorologico1939.GeographicInfo.Name = "GCS_Observ_Meteorologico_1939";
            OldHawaiian.GeographicInfo.Name = "GCS_Old_Hawaiian";
            PicodeLasNieves.GeographicInfo.Name = "GCS_Pico_de_Las_Nieves";
            PitcairnAstro1967.GeographicInfo.Name = "GCS_Pitcairn_1967";
            PitondesNeiges.GeographicInfo.Name = "GCS_Piton_des_Neiges";
            Pohnpei.GeographicInfo.Name = "GCS_Pohnpei";
            PortoSanto1936.GeographicInfo.Name = "GCS_Porto_Santo_1936";
            PortoSanto1995.GeographicInfo.Name = "GCS_Porto_Santo_1995";
            PuertoRico.GeographicInfo.Name = "GCS_Puerto_Rico";
            Reunion.GeographicInfo.Name = "GCS_Reunion";
            RGFG1995.GeographicInfo.Name = "GCS_RGFG_1995";
            RGNC1991.GeographicInfo.Name = "GCS_RGNC_1991";
            RGR1992.GeographicInfo.Name = "GCS_RGR_1992";
            RRAF1991.GeographicInfo.Name = "GCS_RRAF_1991";
            SaintPierreetMiquelon1950.GeographicInfo.Name = "GCS_Saint_Pierre_et_Miquelon_1950";
            SainteAnne.GeographicInfo.Name = "GCS_Sainte_Anne";
            SantoDOS1965.GeographicInfo.Name = "GCS_Santo_DOS_1965";
            SaoBraz.GeographicInfo.Name = "GCS_Sao_Braz";
            SapperHill1943.GeographicInfo.Name = "GCS_Sapper_Hill_1943";
            SelvagemGrande1938.GeographicInfo.Name = "GCS_Selvagem_Grande_1938";
            StKitts1955.GeographicInfo.Name = "GCS_St_Kitts_1955";
            StLucia1955.GeographicInfo.Name = "GCS_St_Lucia_1955";
            StVincent1945.GeographicInfo.Name = "GCS_St_Vincent_1945";
            ST71Belep.GeographicInfo.Name = "GCS_ST71_Belep";
            ST84IledesPins.GeographicInfo.Name = "GCS_ST84_Ile_des_Pins";
            ST87Ouvea.GeographicInfo.Name = "GCS_ST87_Ouvea";
            Tahaa.GeographicInfo.Name = "GCS_Tahaa";
            Tahiti.GeographicInfo.Name = "GCS_Tahiti";
            TernIslandAstro1961.GeographicInfo.Name = "GCS_Tern_Island_1961";
            TristanAstro1968.GeographicInfo.Name = "GCS_Tristan_1968";
            VitiLevu1916.GeographicInfo.Name = "GCS_Viti_Levu_1916";
            WakeIslandAstro1952.GeographicInfo.Name = "GCS_Wake_Island_1952";
            WakeEniwetok1960.GeographicInfo.Name = "GCS_Wake_Eniwetok_1960";

            AlaskanIslands.GeographicInfo.Datum.Name = "D_Alaskan_Islands";
            AmericanSamoa1962.GeographicInfo.Datum.Name = "D_American_Samoa_1962";
            Anguilla1957.GeographicInfo.Datum.Name = "D_Anguilla_1957";
            Anna1Astro1965.GeographicInfo.Datum.Name = "D_Anna_1_1965";
            Antigua1943.GeographicInfo.Datum.Name = "D_Antigua_1943";
            AscensionIsland1958.GeographicInfo.Datum.Name = "D_Ascension_Island_1958";
            AstroBeaconE1945.GeographicInfo.Datum.Name = "D_Beacon_E_1945";
            AstroDOS714.GeographicInfo.Datum.Name = "D_DOS_71_4";
            AstronomicalStation1952.GeographicInfo.Datum.Name = "D_Astro_1952";
            AzoresCentral1948.GeographicInfo.Datum.Name = "D_Azores_Central_Islands_1948";
            AzoresCentral1995.GeographicInfo.Datum.Name = "D_Azores_Central_Islands_1995";
            AzoresOccidental1939.GeographicInfo.Datum.Name = "D_Azores_Occidental_Islands_1939";
            AzoresOriental1940.GeographicInfo.Datum.Name = "D_Azores_Oriental_Islands_1940";
            AzoresOriental1995.GeographicInfo.Datum.Name = "D_Azores_Oriental_Islands_1995";
            BabSouth.GeographicInfo.Datum.Name = "D_Bab_South";
            Barbados.GeographicInfo.Datum.Name = "D_Barbados";
            Barbados1938.GeographicInfo.Datum.Name = "D_Barbados_1938";
            BellevueIGN.GeographicInfo.Datum.Name = "D_Bellevue_IGN";
            Bermuda1957.GeographicInfo.Datum.Name = "D_Bermuda_1957";
            Bermuda2000.GeographicInfo.Datum.Name = "D_Bermuda_2000";
            CantonAstro1966.GeographicInfo.Datum.Name = "D_Canton_1966";
            ChathamIslandAstro1971.GeographicInfo.Datum.Name = "D_Chatham_Island_1971";
            Combani1950.GeographicInfo.Datum.Name = "D_Combani_1950";
            CSG1967.GeographicInfo.Datum.Name = "D_CSG_1967";
            Dominica1945.GeographicInfo.Datum.Name = "D_Dominica_1945";
            DOS1968.GeographicInfo.Datum.Name = "D_DOS_1968";
            EasterIsland1967.GeographicInfo.Datum.Name = "D_Easter_Island_1967";
            FortDesaix.GeographicInfo.Datum.Name = "D_Fort_Desaix";
            FortMarigot.GeographicInfo.Datum.Name = "D_Fort_Marigot";
            FortThomas1955.GeographicInfo.Datum.Name = "D_Fort_Thomas_1955";
            Gan1970.GeographicInfo.Datum.Name = "D_Gan_1970";
            GraciosaBaseSW1948.GeographicInfo.Datum.Name = "D_Graciosa_Base_SW_1948";
            GrandComoros.GeographicInfo.Datum.Name = "D_Grand_Comoros";
            Grenada1953.GeographicInfo.Datum.Name = "D_Grenada_1953";
            Guam1963.GeographicInfo.Datum.Name = "D_Guam_1963";
            GUX1Astro.GeographicInfo.Datum.Name = "D_GUX_1";
            Hjorsey1955.GeographicInfo.Datum.Name = "D_Hjorsey_1955";
            IGN53Mare.GeographicInfo.Datum.Name = "D_IGN53_Mare";
            IGN56Lifou.GeographicInfo.Datum.Name = "D_IGN56_Lifou";
            IGN72GrandeTerre.GeographicInfo.Datum.Name = "D_IGN72_Grande_Terre";
            IGN72NukuHiva.GeographicInfo.Datum.Name = "D_IGN72_Nuku_Hiva";
            ISTS061Astro1968.GeographicInfo.Datum.Name = "D_ISTS_061_1968";
            ISTS073Astro1969.GeographicInfo.Datum.Name = "D_ISTS_073_1969";
            Jamaica1875.GeographicInfo.Datum.Name = "D_Jamaica_1875";
            Jamaica1969.GeographicInfo.Datum.Name = "D_Jamaica_1969";
            JohnstonIsland1961.GeographicInfo.Datum.Name = "D_Johnston_Island_1961";
            K01949.GeographicInfo.Datum.Name = "D_K0_1949";
            KerguelenIsland1949.GeographicInfo.Datum.Name = "D_Kerguelen_Island_1949";
            KusaieAstro1951.GeographicInfo.Datum.Name = "D_Kusaie_1951";
            LC5Astro1961.GeographicInfo.Datum.Name = "D_LC5_1961";
            Madeira1936.GeographicInfo.Datum.Name = "D_Madeira_1936";
            Mahe1971.GeographicInfo.Datum.Name = "D_Mahe_1971";
            Majuro.GeographicInfo.Datum.Name = "D_Majuro";
            MidwayAstro1961.GeographicInfo.Datum.Name = "D_Midway_1961";
            Montserrat1958.GeographicInfo.Datum.Name = "D_Montserrat_1958";
            MOP78.GeographicInfo.Datum.Name = "D_MOP78";
            NEA74Noumea.GeographicInfo.Datum.Name = "D_NEA74_Noumea";
            ObservMeteorologico1939.GeographicInfo.Datum.Name = "D_Observ_Meteorologico_1939";
            OldHawaiian.GeographicInfo.Datum.Name = "D_Old_Hawaiian";
            PicodeLasNieves.GeographicInfo.Datum.Name = "D_Pico_de_Las_Nieves";
            PitcairnAstro1967.GeographicInfo.Datum.Name = "D_Pitcairn_1967";
            PitondesNeiges.GeographicInfo.Datum.Name = "D_Piton_des_Neiges";
            Pohnpei.GeographicInfo.Datum.Name = "D_Pohnpei";
            PortoSanto1936.GeographicInfo.Datum.Name = "D_Porto_Santo_1936";
            PortoSanto1995.GeographicInfo.Datum.Name = "D_Porto_Santo_1995";
            PuertoRico.GeographicInfo.Datum.Name = "D_Puerto_Rico";
            Reunion.GeographicInfo.Datum.Name = "D_Reunion";
            RGFG1995.GeographicInfo.Datum.Name = "D_RGFG_1995";
            RGNC1991.GeographicInfo.Datum.Name = "D_RGNC_1991";
            RGR1992.GeographicInfo.Datum.Name = "D_RGR_1992";
            RRAF1991.GeographicInfo.Datum.Name = "D_RRAF_1991";
            SaintPierreetMiquelon1950.GeographicInfo.Datum.Name = "D_Saint_Pierre_et_Miquelon_1950";
            SainteAnne.GeographicInfo.Datum.Name = "D_Sainte_Anne";
            SantoDOS1965.GeographicInfo.Datum.Name = "D_Santo_DOS_1965";
            SaoBraz.GeographicInfo.Datum.Name = "D_Sao_Braz";
            SapperHill1943.GeographicInfo.Datum.Name = "D_Sapper_Hill_1943";
            SelvagemGrande1938.GeographicInfo.Datum.Name = "D_Selvagem_Grande_1938";
            StKitts1955.GeographicInfo.Datum.Name = "D_St_Kitts_1955";
            StLucia1955.GeographicInfo.Datum.Name = "D_St_Lucia_1955";
            StVincent1945.GeographicInfo.Datum.Name = "D_St_Vincent_1945";
            ST71Belep.GeographicInfo.Datum.Name = "D_ST71_Belep";
            ST84IledesPins.GeographicInfo.Datum.Name = "D_ST84_Ile_des_Pins";
            ST87Ouvea.GeographicInfo.Datum.Name = "D_ST87_Ouvea";
            Tahaa.GeographicInfo.Datum.Name = "D_Tahaa";
            Tahiti.GeographicInfo.Datum.Name = "D_Tahiti";
            TernIslandAstro1961.GeographicInfo.Datum.Name = "D_Tern_Island_1961";
            TristanAstro1968.GeographicInfo.Datum.Name = "D_Tristan_1968";
            VitiLevu1916.GeographicInfo.Datum.Name = "D_Viti_Levu_1916";
            WakeIslandAstro1952.GeographicInfo.Datum.Name = "D_Wake_Island_1952";
            WakeEniwetok1960.GeographicInfo.Datum.Name = "D_Wake_Eniwetok_1960";
        }

        #endregion
    }
}

#pragma warning restore 1591