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
    /// This class contains predefined CoordinateSystems for Europe.
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo BelgeLambert1950;
        public readonly ProjectionInfo BelgeLambert1972;
        public readonly ProjectionInfo BelgeLambert2005;
        public readonly ProjectionInfo BelgeLambert2008;
        public readonly ProjectionInfo Bern1898BernLV03C;
        public readonly ProjectionInfo BritishNationalGrid;
        public readonly ProjectionInfo CGRS1993LTM;
        public readonly ProjectionInfo CH1903LV03;
        public readonly ProjectionInfo CH1903LV03CG;
        public readonly ProjectionInfo CH1903LV95;
        public readonly ProjectionInfo D48SloveniaTM;
        public readonly ProjectionInfo Datum73HayfordGaussIGeoE;
        public readonly ProjectionInfo Datum73HayfordGaussIPCC;
        public readonly ProjectionInfo Datum73ModifiedPortugueseGrid;
        public readonly ProjectionInfo ED1950TM0N;
        public readonly ProjectionInfo ED1950TM27;
        public readonly ProjectionInfo ED1950TM30;
        public readonly ProjectionInfo ED1950TM33;
        public readonly ProjectionInfo ED1950TM36;
        public readonly ProjectionInfo ED1950TM39;
        public readonly ProjectionInfo ED1950TM42;
        public readonly ProjectionInfo ED1950TM45;
        public readonly ProjectionInfo ED1950TM5NE;
        public readonly ProjectionInfo ED1950Turkey10;
        public readonly ProjectionInfo ED1950Turkey11;
        public readonly ProjectionInfo ED1950Turkey12;
        public readonly ProjectionInfo ED1950Turkey13;
        public readonly ProjectionInfo ED1950Turkey14;
        public readonly ProjectionInfo ED1950Turkey15;
        public readonly ProjectionInfo ED1950Turkey9;
        public readonly ProjectionInfo Estonia1997EstoniaNationalGrid;
        public readonly ProjectionInfo EstonianCoordinateSystemof1992;
        public readonly ProjectionInfo ETRS1989DKTM1;
        public readonly ProjectionInfo ETRS1989DKTM2;
        public readonly ProjectionInfo ETRS1989DKTM3;
        public readonly ProjectionInfo ETRS1989DKTM4;
        public readonly ProjectionInfo ETRS1989GuernseyGrid;
        public readonly ProjectionInfo ETRS1989JerseyTM;
        public readonly ProjectionInfo ETRS1989KosovoGrid;
        public readonly ProjectionInfo ETRS1989Kp2000Bornholm;
        public readonly ProjectionInfo ETRS1989Kp2000Jutland;
        public readonly ProjectionInfo ETRS1989Kp2000Zealand;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone5;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone6;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone7;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone8;
        public readonly ProjectionInfo ETRS1989PolandCS92;
        public readonly ProjectionInfo ETRS1989PortugalTM06;
        public readonly ProjectionInfo ETRS1989SloveniaTM;
        public readonly ProjectionInfo ETRS1989TM30NE;
        public readonly ProjectionInfo ETRS1989TMBaltic1993;
        public readonly ProjectionInfo ETRS1989UWPP1992;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS5;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS6;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS7;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS8;
        public readonly ProjectionInfo EUREFFINTM35FIN;
        public readonly ProjectionInfo FinlandZone1;
        public readonly ProjectionInfo FinlandZone2;
        public readonly ProjectionInfo FinlandZone3;
        public readonly ProjectionInfo FinlandZone4;
        public readonly ProjectionInfo GreekGrid;
        public readonly ProjectionInfo GuernseyGrid;
        public readonly ProjectionInfo HD1972EgysegesOrszagosVetuleti;
        public readonly ProjectionInfo Helle1954JanMayenGrid;
        public readonly ProjectionInfo HTRS96CroatiaLCC;
        public readonly ProjectionInfo HTRS96CroatiaTM;
        public readonly ProjectionInfo IRENET95IrishTranverseMercator;
        public readonly ProjectionInfo IrishNationalGrid;
        public readonly ProjectionInfo ISN1993Lambert1993;
        public readonly ProjectionInfo ISN2004Lambert2004;
        public readonly ProjectionInfo KKJFinlandZone0;
        public readonly ProjectionInfo KKJFinlandZone5;
        public readonly ProjectionInfo LisboaBesselBonne;
        public readonly ProjectionInfo LisboaHayfordGaussIGeoE;
        public readonly ProjectionInfo LisboaHayfordGaussIPCC;
        public readonly ProjectionInfo LKS1992LatviaTM;
        public readonly ProjectionInfo LKS1992LatviaTMFN0;
        public readonly ProjectionInfo LKS1994LithuaniaTM;
        public readonly ProjectionInfo Luxembourg1930Gauss;
        public readonly ProjectionInfo Madrid1870MadridSpain;
        public readonly ProjectionInfo MGI1901SloveneNationalGrid;
        public readonly ProjectionInfo MGI3DegreeGaussZone5;
        public readonly ProjectionInfo MGI3DegreeGaussZone6;
        public readonly ProjectionInfo MGI3DegreeGaussZone7;
        public readonly ProjectionInfo MGI3DegreeGaussZone8;
        public readonly ProjectionInfo MGIBalkans5;
        public readonly ProjectionInfo MGIBalkans6;
        public readonly ProjectionInfo MGIBalkans7;
        public readonly ProjectionInfo MGIBalkans8;
        public readonly ProjectionInfo MGISloveniaGrid;
        public readonly ProjectionInfo MonteMarioItaly1;
        public readonly ProjectionInfo MonteMarioItaly2;
        public readonly ProjectionInfo MonteMarioRomeItaly1;
        public readonly ProjectionInfo MonteMarioRomeItaly2;
        public readonly ProjectionInfo OSNI1952IrishNationalGrid;
        public readonly ProjectionInfo PortugueseNationalGrid;
        public readonly ProjectionInfo Pulkovo194258GUGiK80;
        public readonly ProjectionInfo Pulkovo1942Adj1958PolandZoneI;
        public readonly ProjectionInfo Pulkovo1942Adj1958PolandZoneII;
        public readonly ProjectionInfo Pulkovo1942Adj1958PolandZoneIII;
        public readonly ProjectionInfo Pulkovo1942Adj1958PolandZoneIV;
        public readonly ProjectionInfo Pulkovo1942Adj1958PolandZoneV;
        public readonly ProjectionInfo RDNew;
        public readonly ProjectionInfo RDOld;
        public readonly ProjectionInfo Roma1940GaussBoagaEst;
        public readonly ProjectionInfo Roma1940GaussBoagaOvest;
        public readonly ProjectionInfo RT9025gonWest;
        public readonly ProjectionInfo SJTSKFerroKrovak;
        public readonly ProjectionInfo SJTSKFerroKrovakEastNorth;
        public readonly ProjectionInfo SJTSKKrovak;
        public readonly ProjectionInfo SJTSKKrovakEastNorth;
        public readonly ProjectionInfo Slovenia1996SloveneNationalGrid;
        public readonly ProjectionInfo Stereo1933;
        public readonly ProjectionInfo Stereo1970;
        public readonly ProjectionInfo SwedishNationalGrid;
        public readonly ProjectionInfo TM75IrishGrid;
        public readonly ProjectionInfo UWPP1992;
        public readonly ProjectionInfo UWPP2000pas5;
        public readonly ProjectionInfo UWPP2000pas6;
        public readonly ProjectionInfo UWPP2000pas7;
        public readonly ProjectionInfo UWPP2000pas8;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe.
        /// </summary>
        public Europe()
        {
            BelgeLambert1950 = ProjectionInfo.FromEpsgCode(21500).SetNames("Belge_Lambert_1950", "GCS_Belge_1950_Brussels", "D_Belge_1950");
            BelgeLambert1972 = ProjectionInfo.FromEpsgCode(31370).SetNames("Belge_Lambert_1972", "GCS_Belge_1972", "D_Belge_1972");
            BelgeLambert2005 = ProjectionInfo.FromEpsgCode(3447).SetNames("Belge_Lambert_2005", "GCS_ETRS_1989", "D_ETRS_1989");
            BelgeLambert2008 = ProjectionInfo.FromEpsgCode(3812).SetNames("Belge_Lambert_2008", "GCS_ETRS_1989", "D_ETRS_1989");
            Bern1898BernLV03C = ProjectionInfo.FromEpsgCode(21780).SetNames("Bern_1898_Bern_LV03C", "GCS_Bern_1898_Bern", "D_Bern_1898");
            BritishNationalGrid = ProjectionInfo.FromEpsgCode(27700).SetNames("British_National_Grid", "GCS_OSGB_1936", "D_OSGB_1936");
            CGRS1993LTM = ProjectionInfo.FromAuthorityCode("ESRI", 102319).SetNames("CGRS_1993_LTM", "GCS_CGRS_1993", "D_Cyprus_Geodetic_Reference_System_1993"); // missing
            CH1903LV03 = ProjectionInfo.FromEpsgCode(21781).SetNames("CH1903_LV03", "GCS_CH1903", "D_CH1903");
            CH1903LV03CG = ProjectionInfo.FromEpsgCode(21782).SetNames("CH1903_LV03C-G", "GCS_CH1903", "D_CH1903");
            CH1903LV95 = ProjectionInfo.FromEpsgCode(2056).SetNames("CH1903+_LV95", "GCS_CH1903+", "D_CH1903+");
            D48SloveniaTM = ProjectionInfo.FromAuthorityCode("ESRI", 102060).SetNames("D48_Slovenia_TM", "GCS_D48", "D_D48"); // missing
            Datum73HayfordGaussIGeoE = ProjectionInfo.FromAuthorityCode("ESRI", 102160).SetNames("Datum_73_Hayford_Gauss_IGeoE", "GCS_Datum_73", "D_Datum_73");
            Datum73HayfordGaussIPCC = ProjectionInfo.FromAuthorityCode("ESRI", 102161).SetNames("Datum_73_Hayford_Gauss_IPCC", "GCS_Datum_73", "D_Datum_73");
            Datum73ModifiedPortugueseGrid = ProjectionInfo.FromEpsgCode(27493).SetNames("Datum_73_Modified_Portuguese_Grid", "GCS_Datum_73", "D_Datum_73");
            ED1950TM0N = ProjectionInfo.FromEpsgCode(23090).SetNames("ED_1950_TM_0_N", "GCS_European_1950", "D_European_1950");
            ED1950TM27 = ProjectionInfo.FromEpsgCode(2319).SetNames("ED_1950_TM27", "GCS_European_1950", "D_European_1950");
            ED1950TM30 = ProjectionInfo.FromEpsgCode(2320).SetNames("ED_1950_TM30", "GCS_European_1950", "D_European_1950");
            ED1950TM33 = ProjectionInfo.FromEpsgCode(2321).SetNames("ED_1950_TM33", "GCS_European_1950", "D_European_1950");
            ED1950TM36 = ProjectionInfo.FromEpsgCode(2322).SetNames("ED_1950_TM36", "GCS_European_1950", "D_European_1950");
            ED1950TM39 = ProjectionInfo.FromEpsgCode(2323).SetNames("ED_1950_TM39", "GCS_European_1950", "D_European_1950");
            ED1950TM42 = ProjectionInfo.FromEpsgCode(2324).SetNames("ED_1950_TM42", "GCS_European_1950", "D_European_1950");
            ED1950TM45 = ProjectionInfo.FromEpsgCode(2325).SetNames("ED_1950_TM45", "GCS_European_1950", "D_European_1950");
            ED1950TM5NE = ProjectionInfo.FromEpsgCode(23095).SetNames("ED_1950_TM_5_NE", "GCS_European_1950", "D_European_1950");
            ED1950Turkey10 = ProjectionInfo.FromAuthorityCode("ESRI", 2182).SetNames("ED_1950_Turkey_10", "GCS_European_1950", "D_European_1950"); // missing
            ED1950Turkey11 = ProjectionInfo.FromAuthorityCode("ESRI", 2183).SetNames("ED_1950_Turkey_11", "GCS_European_1950", "D_European_1950"); // missing
            ED1950Turkey12 = ProjectionInfo.FromAuthorityCode("ESRI", 2184).SetNames("ED_1950_Turkey_12", "GCS_European_1950", "D_European_1950"); // missing
            ED1950Turkey13 = ProjectionInfo.FromAuthorityCode("ESRI", 2185).SetNames("ED_1950_Turkey_13", "GCS_European_1950", "D_European_1950"); // missing
            ED1950Turkey14 = ProjectionInfo.FromAuthorityCode("ESRI", 2186).SetNames("ED_1950_Turkey_14", "GCS_European_1950", "D_European_1950"); // missing
            ED1950Turkey15 = ProjectionInfo.FromAuthorityCode("ESRI", 2187).SetNames("ED_1950_Turkey_15", "GCS_European_1950", "D_European_1950"); // missing
            ED1950Turkey9 = ProjectionInfo.FromAuthorityCode("ESRI", 2181).SetNames("ED_1950_Turkey_9", "GCS_European_1950", "D_European_1950"); // missing
            Estonia1997EstoniaNationalGrid = ProjectionInfo.FromEpsgCode(3301).SetNames("Estonia_1997_Estonia_National_Grid", "GCS_Estonia_1997", "D_Estonia_1997");
            EstonianCoordinateSystemof1992 = ProjectionInfo.FromEpsgCode(3300).SetNames("Estonian_Coordinate_System_of_1992", "GCS_Estonia_1992", "D_Estonia_1992");
            ETRS1989DKTM1 = ProjectionInfo.FromAuthorityCode("EPSG", 103216).SetNames("ETRS_1989_DKTM1", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989DKTM2 = ProjectionInfo.FromAuthorityCode("EPSG", 103217).SetNames("ETRS_1989_DKTM2", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989DKTM3 = ProjectionInfo.FromAuthorityCode("EPSG", 103218).SetNames("ETRS_1989_DKTM3", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989DKTM4 = ProjectionInfo.FromAuthorityCode("EPSG", 103219).SetNames("ETRS_1989_DKTM4", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989GuernseyGrid = ProjectionInfo.FromEpsgCode(3108).SetNames("ETRS_1989_Guernsey_Grid", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989JerseyTM = ProjectionInfo.FromEpsgCode(3109).SetNames("ETRS_1989_Jersey_Transverse_Mercator", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989KosovoGrid = ProjectionInfo.FromAuthorityCode("ESRI", 102157).SetNames("ETRS_1989_Kosovo_Grid", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989Kp2000Bornholm = ProjectionInfo.FromEpsgCode(2198).SetNames("ETRS_1989_Kp2000_Bornholm", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989Kp2000Jutland = ProjectionInfo.FromEpsgCode(2196).SetNames("ETRS_1989_Kp2000_Jutland", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989Kp2000Zealand = ProjectionInfo.FromEpsgCode(2197).SetNames("ETRS_1989_Kp2000_Zealand", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989PolandCS2000Zone5 = ProjectionInfo.FromEpsgCode(2176).SetNames("ETRS_1989_Poland_CS2000_Zone_5", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989PolandCS2000Zone6 = ProjectionInfo.FromEpsgCode(2177).SetNames("ETRS_1989_Poland_CS2000_Zone_6", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989PolandCS2000Zone7 = ProjectionInfo.FromEpsgCode(2178).SetNames("ETRS_1989_Poland_CS2000_Zone_7", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989PolandCS2000Zone8 = ProjectionInfo.FromEpsgCode(2179).SetNames("ETRS_1989_Poland_CS2000_Zone_8", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989PolandCS92 = ProjectionInfo.FromEpsgCode(2180).SetNames("ETRS_1989_Poland_CS92", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989PortugalTM06 = ProjectionInfo.FromEpsgCode(3763).SetNames("ETRS_1989_Portugal_TM06", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989SloveniaTM = ProjectionInfo.FromAuthorityCode("ESRI", 102109).SetNames("ETRS_1989_Slovenia_TM", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989TM30NE = ProjectionInfo.FromEpsgCode(2213).SetNames("ETRS_1989_TM_30_NE", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989TMBaltic1993 = ProjectionInfo.FromEpsgCode(25884).SetNames("ETRS_1989_TM_Baltic_1993", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UWPP1992 = ProjectionInfo.FromAuthorityCode("ESRI", 102173).SetNames("ETRS_1989_UWPP_1992", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989UWPP2000PAS5 = ProjectionInfo.FromAuthorityCode("ESRI", 102174).SetNames("ETRS_1989_UWPP_2000_PAS_5", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989UWPP2000PAS6 = ProjectionInfo.FromAuthorityCode("ESRI", 102175).SetNames("ETRS_1989_UWPP_2000_PAS_6", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989UWPP2000PAS7 = ProjectionInfo.FromAuthorityCode("ESRI", 102176).SetNames("ETRS_1989_UWPP_2000_PAS_7", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989UWPP2000PAS8 = ProjectionInfo.FromAuthorityCode("ESRI", 102177).SetNames("ETRS_1989_UWPP_2000_PAS_8", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            EUREFFINTM35FIN = ProjectionInfo.FromEpsgCode(3067).SetNames("EUREF_FIN_TM35FIN", "GCS_EUREF_FIN", "D_ETRS_1989");
            FinlandZone1 = ProjectionInfo.FromEpsgCode(2391).SetNames("Finland_Zone_1", "GCS_KKJ", "D_KKJ");
            FinlandZone2 = ProjectionInfo.FromEpsgCode(2392).SetNames("Finland_Zone_2", "GCS_KKJ", "D_KKJ");
            FinlandZone3 = ProjectionInfo.FromEpsgCode(2393).SetNames("Finland_Zone_3", "GCS_KKJ", "D_KKJ");
            FinlandZone4 = ProjectionInfo.FromEpsgCode(2394).SetNames("Finland_Zone_4", "GCS_KKJ", "D_KKJ");
            GreekGrid = ProjectionInfo.FromEpsgCode(2100).SetNames("Greek_Grid", "GCS_GGRS_1987", "D_GGRS_1987");
            GuernseyGrid = ProjectionInfo.FromAuthorityCode("ESRI", 102070).SetNames("Guernsey_Grid", "GCS_WGS_1984", "D_WGS_1984"); // missing
            HD1972EgysegesOrszagosVetuleti = ProjectionInfo.FromEpsgCode(23700).SetNames("Hungarian_1972_Egyseges_Orszagos_Vetuleti", "GCS_Hungarian_1972", "D_Hungarian_1972");
            Helle1954JanMayenGrid = ProjectionInfo.FromEpsgCode(3058).SetNames("Helle_1954_Jan_Mayen_Grid", "GCS_Helle_1954", "D_Helle_1954");
            HTRS96CroatiaLCC = ProjectionInfo.FromEpsgCode(3766).SetNames("HTRS96_Croatia_LCC", "GCS_HTRS96", "D_Croatian_Terrestrial_Reference_System");
            HTRS96CroatiaTM = ProjectionInfo.FromEpsgCode(3765).SetNames("HTRS96_Croatia_TM", "GCS_HTRS96", "D_Croatian_Terrestrial_Reference_System");
            IRENET95IrishTranverseMercator = ProjectionInfo.FromEpsgCode(2157).SetNames("IRENET95_Irish_Transverse_Mercator", "GCS_IRENET95", "D_IRENET95");
            IrishNationalGrid = ProjectionInfo.FromEpsgCode(29902).SetNames("TM65_Irish_Grid", "GCS_TM65", "D_TM65");
            ISN1993Lambert1993 = ProjectionInfo.FromEpsgCode(3057).SetNames("ISN_1993_Lambert_1993", "GCS_ISN_1993", "D_Islands_Network_1993");
            ISN2004Lambert2004 = ProjectionInfo.FromAuthorityCode("EPSG", 102420).SetNames("ISN_2004_Lambert_2004", "GCS_ISN_2004", "D_Islands_Network_2004"); // missing
            KKJFinlandZone0 = ProjectionInfo.FromEpsgCode(3386).SetNames("KKJ_Finland_Zone_0", "GCS_KKJ", "D_KKJ");
            KKJFinlandZone5 = ProjectionInfo.FromEpsgCode(3387).SetNames("KKJ_Finland_Zone_5", "GCS_KKJ", "D_KKJ");
            LisboaBesselBonne = ProjectionInfo.FromAuthorityCode("ESRI", 102163).SetNames("Lisboa_Bessel_Bonne", "GCS_Datum_Lisboa_Bessel", "D_Datum_Lisboa_Bessel");
            LisboaHayfordGaussIGeoE = ProjectionInfo.FromAuthorityCode("ESRI", 102164).SetNames("Lisboa_Hayford_Gauss_IGeoE", "GCS_Datum_Lisboa_Hayford", "D_Datum_Lisboa_Hayford");
            LisboaHayfordGaussIPCC = ProjectionInfo.FromAuthorityCode("ESRI", 102165).SetNames("Lisboa_Hayford_Gauss_IPCC", "GCS_Datum_Lisboa_Hayford", "D_Datum_Lisboa_Hayford");
            LKS1992LatviaTM = ProjectionInfo.FromEpsgCode(3059).SetNames("LKS_1992_Latvia_TM", "GCS_LKS_1992", "D_Latvia_1992");
            LKS1992LatviaTMFN0 = ProjectionInfo.FromAuthorityCode("ESRI", 102440).SetNames("LKS_1992_Latvia_TM_0", "GCS_LKS_1992", "D_Latvia_1992"); // missing
            LKS1994LithuaniaTM = ProjectionInfo.FromEpsgCode(3346).SetNames("LKS_1994_Lithuania_TM", "GCS_LKS_1994", "D_Lithuania_1994");
            Luxembourg1930Gauss = ProjectionInfo.FromEpsgCode(2169).SetNames("Luxembourg_1930_Gauss", "GCS_Luxembourg_1930", "D_Luxembourg_1930");
            Madrid1870MadridSpain = ProjectionInfo.FromEpsgCode(2062).SetNames("Madrid_1870_Madrid_Spain", "GCS_Madrid_1870_Madrid", "D_Madrid_1870");
            MGI1901SloveneNationalGrid = ProjectionInfo.FromEpsgCode(3912).SetNames("MGI_1901_Slovene_National_Grid", "GCS_MGI_1901", "D_MGI_1901");
            MGI3DegreeGaussZone5 = ProjectionInfo.FromEpsgCode(31265).SetNames("MGI_3_Degree_Gauss_Zone_5", "GCS_MGI", "D_MGI");
            MGI3DegreeGaussZone6 = ProjectionInfo.FromEpsgCode(31266).SetNames("MGI_3_Degree_Gauss_Zone_6", "GCS_MGI", "D_MGI");
            MGI3DegreeGaussZone7 = ProjectionInfo.FromEpsgCode(31267).SetNames("MGI_3_Degree_Gauss_Zone_7", "GCS_MGI", "D_MGI");
            MGI3DegreeGaussZone8 = ProjectionInfo.FromEpsgCode(31268).SetNames("MGI_3_Degree_Gauss_Zone_8", "GCS_MGI", "D_MGI");
            MGIBalkans5 = ProjectionInfo.FromEpsgCode(31275).SetNames("MGI_Balkans_5", "GCS_MGI", "D_MGI");
            MGIBalkans6 = ProjectionInfo.FromEpsgCode(31276).SetNames("MGI_Balkans_6", "GCS_MGI", "D_MGI");
            MGIBalkans7 = ProjectionInfo.FromEpsgCode(31277).SetNames("MGI_Balkans_7", "GCS_MGI", "D_MGI");
            MGIBalkans8 = ProjectionInfo.FromEpsgCode(31279).SetNames("MGI_Balkans_8", "GCS_MGI", "D_MGI");
            MGISloveniaGrid = ProjectionInfo.FromEpsgCode(2170).SetNames("MGI_Slovenia_Grid", "GCS_MGI", "D_MGI");
            MonteMarioItaly1 = ProjectionInfo.FromEpsgCode(3003).SetNames("Monte_Mario_Italy_1", "GCS_Monte_Mario", "D_Monte_Mario");
            MonteMarioItaly2 = ProjectionInfo.FromEpsgCode(3004).SetNames("Monte_Mario_Italy_2", "GCS_Monte_Mario", "D_Monte_Mario");
            MonteMarioRomeItaly1 = ProjectionInfo.FromEpsgCode(26591).SetNames("Monte_Mario_Rome_Italy_1", "GCS_Monte_Mario_Rome", "D_Monte_Mario");
            MonteMarioRomeItaly2 = ProjectionInfo.FromEpsgCode(26592).SetNames("Monte_Mario_Rome_Italy_2", "GCS_Monte_Mario_Rome", "D_Monte_Mario");
            OSNI1952IrishNationalGrid = ProjectionInfo.FromEpsgCode(29901).SetNames("OSNI_1952_Irish_National_Grid", "GCS_OSNI_1952", "D_OSNI_1952");
            PortugueseNationalGrid = ProjectionInfo.FromEpsgCode(20790).SetNames("Portuguese_National_Grid", "GCS_Lisbon_Lisbon", "D_Lisbon");
            Pulkovo194258GUGiK80 = ProjectionInfo.FromEpsgCode(3328).SetNames("Pulkovo_1942_Adj_1958_GUGiK-80", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958PolandZoneI = ProjectionInfo.FromEpsgCode(3120).SetNames("Pulkovo_1942_Adj_1958_Poland_Zone_I", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958PolandZoneII = ProjectionInfo.FromEpsgCode(2172).SetNames("Pulkovo_1942_Adj_1958_Poland_Zone_II", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958PolandZoneIII = ProjectionInfo.FromEpsgCode(2173).SetNames("Pulkovo_1942_Adj_1958_Poland_Zone_III", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958PolandZoneIV = ProjectionInfo.FromEpsgCode(2174).SetNames("Pulkovo_1942_Adj_1958_Poland_Zone_IV", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958PolandZoneV = ProjectionInfo.FromEpsgCode(2175).SetNames("Pulkovo_1942_Adj_1958_Poland_Zone_V", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            RDNew = ProjectionInfo.FromEpsgCode(28992).SetNames("RD_New", "GCS_Amersfoort", "D_Amersfoort");
            RDOld = ProjectionInfo.FromEpsgCode(28991).SetNames("RD_Old", "GCS_Amersfoort", "D_Amersfoort");
            Roma1940GaussBoagaEst = ProjectionInfo.FromAuthorityCode("ESRI", 102093).SetNames("Roma_1940_Gauss_Boaga_Est", "GCS_Roma_1940", "D_Roma_1940"); // missing
            Roma1940GaussBoagaOvest = ProjectionInfo.FromAuthorityCode("ESRI", 102094).SetNames("Roma_1940_Gauss_Boaga_Ovest", "GCS_Roma_1940", "D_Roma_1940"); // missing
            RT9025gonWest = ProjectionInfo.FromEpsgCode(2400).SetNames("RT90_25_gon_W", "GCS_RT_1990", "D_RT_1990");
            SJTSKFerroKrovak = ProjectionInfo.FromEpsgCode(2065).SetNames("S-JTSK_Ferro_Krovak", "GCS_S_JTSK_Ferro", "D_S_JTSK");
            SJTSKFerroKrovakEastNorth = ProjectionInfo.FromAuthorityCode("ESRI", 102066).SetNames("S-JTSK_Ferro_Krovak_East_North", "GCS_S_JTSK_Ferro", "D_S_JTSK"); // missing
            SJTSKKrovak = ProjectionInfo.FromAuthorityCode("ESRI", 102065).SetNames("S-JTSK_Krovak", "GCS_S_JTSK", "D_S_JTSK"); // missing
            SJTSKKrovakEastNorth = ProjectionInfo.FromAuthorityCode("ESRI", 102067).SetNames("S-JTSK_Krovak_East_North", "GCS_S_JTSK", "D_S_JTSK"); // missing
            Slovenia1996SloveneNationalGrid = ProjectionInfo.FromEpsgCode(3794).SetNames("Slovenia_1996_Slovene_National_Grid", "GCS_Slovenia_1996", "D_Slovenia_Geodetic_Datum_1996");
            Stereo1933 = ProjectionInfo.FromEpsgCode(31600).SetNames("Stereo_33", "GCS_Dealul_Piscului_1933", "D_Dealul_Piscului_1933");
            Stereo1970 = ProjectionInfo.FromEpsgCode(31700).SetNames("Stereo_70", "GCS_Dealul_Piscului_1970", "D_Dealul_Piscului_1970");
            SwedishNationalGrid = ProjectionInfo.FromEpsgCode(30800).SetNames("Swedish_National_Grid", "GCS_RT38_Stockholm", "D_Stockholm_1938");
            TM75IrishGrid = ProjectionInfo.FromEpsgCode(29903).SetNames("TM75_Irish_Grid", "GCS_TM75", "D_TM75");
            UWPP1992 = ProjectionInfo.FromAuthorityCode("ESRI", 102194).SetNames("UWPP_1992", "GCS_ETRF_1989", "D_ETRF_1989"); // missing
            UWPP2000pas5 = ProjectionInfo.FromAuthorityCode("ESRI", 102195).SetNames("UWPP_2000_PAS_5", "GCS_ETRF_1989", "D_ETRF_1989"); // missing
            UWPP2000pas6 = ProjectionInfo.FromAuthorityCode("ESRI", 102196).SetNames("UWPP_2000_PAS_6", "GCS_ETRF_1989", "D_ETRF_1989"); // missing
            UWPP2000pas7 = ProjectionInfo.FromAuthorityCode("ESRI", 102197).SetNames("UWPP_2000_PAS_7", "GCS_ETRF_1989", "D_ETRF_1989"); // missing
            UWPP2000pas8 = ProjectionInfo.FromAuthorityCode("ESRI", 102198).SetNames("UWPP_2000_PAS_8", "GCS_ETRF_1989", "D_ETRF_1989"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591