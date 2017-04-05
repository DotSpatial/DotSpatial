// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:08:55 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// Bart Adriaanse      | 30/11/2013 |  Added Amersfoort definition, the proper name for dutch datum
//                     |            |  Note: please look for DutchRD in Pojected.NationalGrids
// ************************************************************************************************* 

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Europe.
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Albanian1987;
        public readonly ProjectionInfo Amersfoort;
        public readonly ProjectionInfo ATFParis;
        public readonly ProjectionInfo Belge1950Brussels;
        public readonly ProjectionInfo Belge1972;
        public readonly ProjectionInfo Bern1898;
        public readonly ProjectionInfo Bern1898Bern;
        public readonly ProjectionInfo Bern1938;
        public readonly ProjectionInfo CGRS1993;
        public readonly ProjectionInfo CH1903;
        public readonly ProjectionInfo CH1903Plus;
        public readonly ProjectionInfo D48;
        public readonly ProjectionInfo Datum73;
        public readonly ProjectionInfo DatumLisboaBessel;
        public readonly ProjectionInfo DatumLisboaHayford;
        public readonly ProjectionInfo DealulPiscului1933Romania;
        public readonly ProjectionInfo DealulPiscului1970Romania;
        public readonly ProjectionInfo DeutscheHauptdreiecksnetz;
        public readonly ProjectionInfo Estonia1937;
        public readonly ProjectionInfo Estonia1992;
        public readonly ProjectionInfo Estonia1997;
        public readonly ProjectionInfo ETRF1989;
        public readonly ProjectionInfo ETRS1989;
        public readonly ProjectionInfo EUREFFIN;
        public readonly ProjectionInfo European1979;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EuropeanDatum1987;
        public readonly ProjectionInfo FD1954;
        public readonly ProjectionInfo fk89;
        public readonly ProjectionInfo GGRS1987;
        public readonly ProjectionInfo Greek;
        public readonly ProjectionInfo GreekAthens;
        public readonly ProjectionInfo HD1909;
        public readonly ProjectionInfo Hermannskogel;
        public readonly ProjectionInfo Hjorsey1955;
        public readonly ProjectionInfo HTRS96;
        public readonly ProjectionInfo HungarianDatum1972;
        public readonly ProjectionInfo IRENET95;
        public readonly ProjectionInfo ISN1993;
        public readonly ProjectionInfo ISN2004;
        public readonly ProjectionInfo Kartastokoordinaattijarjestelma;
        public readonly ProjectionInfo Lisbon;
        public readonly ProjectionInfo Lisbon1890;
        public readonly ProjectionInfo Lisbon1890Lisbon;
        public readonly ProjectionInfo LisbonLisbon;
        public readonly ProjectionInfo LKS1992;
        public readonly ProjectionInfo LKS1994;
        public readonly ProjectionInfo Luxembourg1930;
        public readonly ProjectionInfo Madrid1870Madrid;
        public readonly ProjectionInfo MGI1901;
        public readonly ProjectionInfo MGIFerro;
        public readonly ProjectionInfo MilitarGeographischeInstitut;
        public readonly ProjectionInfo MOLDREF99;
        public readonly ProjectionInfo MonteMario;
        public readonly ProjectionInfo MonteMarioRome;
        public readonly ProjectionInfo NGO1948;
        public readonly ProjectionInfo NGO1948Oslo;
        public readonly ProjectionInfo NorddeGuerreParis;
        public readonly ProjectionInfo NouvelleTriangulationFrancaise;
        public readonly ProjectionInfo NTFParis;
        public readonly ProjectionInfo OSGB1936;
        public readonly ProjectionInfo OSGB1970SN;
        public readonly ProjectionInfo OSNI1952;
        public readonly ProjectionInfo OSSN1980;
        public readonly ProjectionInfo PD83;
        public readonly ProjectionInfo Pulkovo1942;
        public readonly ProjectionInfo Pulkovo1942Adj1958;
        public readonly ProjectionInfo Pulkovo1942Adj1983;
        public readonly ProjectionInfo Pulkovo1995;
        public readonly ProjectionInfo PZ1990;
        public readonly ProjectionInfo RD83;
        public readonly ProjectionInfo ReseauNationalBelge1950;
        public readonly ProjectionInfo Reykjavik1900;
        public readonly ProjectionInfo RGF1993;
        public readonly ProjectionInfo Roma1940;
        public readonly ProjectionInfo RT1990;
        public readonly ProjectionInfo RT38;
        public readonly ProjectionInfo RT38Stockholm;
        public readonly ProjectionInfo S42Hungary;
        public readonly ProjectionInfo SJTSK;
        public readonly ProjectionInfo SJTSKFerro;
        public readonly ProjectionInfo Slovenia1996;
        public readonly ProjectionInfo SREF98;
        public readonly ProjectionInfo SWEREF99;
        public readonly ProjectionInfo SwissTRF1995;
        public readonly ProjectionInfo TM65;
        public readonly ProjectionInfo TM75;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe.
        /// </summary>
        public Europe()
        {
            Albanian1987 = ProjectionInfo.FromEpsgCode(4191).SetNames("", "GCS_Albanian_1987", "D_Albanian_1987");
            Amersfoort = ProjectionInfo.FromEpsgCode(4289).SetNames("", "GCS_Amersfoort", "D_Amersfoort");
            ATFParis = ProjectionInfo.FromEpsgCode(4901).SetNames("", "GCS_ATF_Paris", "D_ATF");
            Belge1950Brussels = ProjectionInfo.FromEpsgCode(4809).SetNames("", "GCS_Belge_1950_Brussels", "D_Belge_1950");
            Belge1972 = ProjectionInfo.FromEpsgCode(4313).SetNames("", "GCS_Belge_1972", "D_Belge_1972");
            Bern1898 = ProjectionInfo.FromEpsgCode(4217).SetNames("NAD_1983_BLM_Zone_59N", "GCS_North_American_1983", "D_North_American_1983");
            Bern1898Bern = ProjectionInfo.FromEpsgCode(4801).SetNames("", "GCS_Bern_1898_Bern", "D_Bern_1898");
            Bern1938 = ProjectionInfo.FromEpsgCode(4306).SetNames("", "GCS_Bern_1938", "D_Bern_1938");
            CGRS1993 = ProjectionInfo.FromAuthorityCode("ESRI", 104141).SetNames("", "GCS_CGRS_1993", "D_Cyprus_Geodetic_Reference_System_1993"); // missing
            CH1903 = ProjectionInfo.FromEpsgCode(4149).SetNames("", "GCS_CH1903", "D_CH1903");
            CH1903Plus = ProjectionInfo.FromEpsgCode(4150).SetNames("", "GCS_CH1903+", "D_CH1903+");
            D48 = ProjectionInfo.FromAuthorityCode("ESRI", 104131).SetNames("", "GCS_D48", "D_D48"); // missing
            Datum73 = ProjectionInfo.FromEpsgCode(4274).SetNames("", "GCS_Datum_73", "D_Datum_73");
            DatumLisboaBessel = ProjectionInfo.FromAuthorityCode("ESRI", 104105).SetNames("", "GCS_Datum_Lisboa_Bessel", "D_Datum_Lisboa_Bessel");
            DatumLisboaHayford = ProjectionInfo.FromAuthorityCode("ESRI", 104106).SetNames("", "GCS_Datum_Lisboa_Hayford", "D_Datum_Lisboa_Hayford");
            DealulPiscului1933Romania = ProjectionInfo.FromEpsgCode(4316).SetNames("", "GCS_Dealul_Piscului_1933", "D_Dealul_Piscului_1933");
            DealulPiscului1970Romania = ProjectionInfo.FromEpsgCode(4317).SetNames("", "GCS_Dealul_Piscului_1970", "D_Dealul_Piscului_1970");
            DeutscheHauptdreiecksnetz = ProjectionInfo.FromEpsgCode(4314).SetNames("", "GCS_Deutsches_Hauptdreiecksnetz", "D_Deutsches_Hauptdreiecksnetz");
            Estonia1937 = ProjectionInfo.FromAuthorityCode("ESRI", 104101).SetNames("", "GCS_Estonia_1937", "D_Estonia_1937");
            Estonia1992 = ProjectionInfo.FromEpsgCode(4133).SetNames("", "GCS_Estonia_1992", "D_Estonia_1992");
            Estonia1997 = ProjectionInfo.FromEpsgCode(4180).SetNames("", "GCS_Estonia_1997", "D_Estonia_1997");
            ETRF1989 = ProjectionInfo.FromAuthorityCode("ESRI", 104258).SetNames("", "GCS_ETRF_1989", "D_ETRF_1989"); // missing
            ETRS1989 = ProjectionInfo.FromEpsgCode(4258).SetNames("", "GCS_ETRS_1989", "D_ETRS_1989");
            EUREFFIN = ProjectionInfo.FromAuthorityCode("ESRI", 104129).SetNames("", "GCS_EUREF_FIN", "D_ETRS_1989"); // missing
            European1979 = ProjectionInfo.FromEpsgCode(4668).SetNames("", "GCS_European_1979", "D_European_1979");
            EuropeanDatum1950 = ProjectionInfo.FromEpsgCode(4230).SetNames("", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1987 = ProjectionInfo.FromEpsgCode(4231).SetNames("", "GCS_European_1987", "D_European_1987");
            FD1954 = ProjectionInfo.FromEpsgCode(4741).SetNames("", "GCS_FD_1954", "D_Faroe_Datum_1954");
            fk89 = ProjectionInfo.FromEpsgCode(4753).SetNames("", "GCS_fk89", "D_fk89");
            GGRS1987 = ProjectionInfo.FromEpsgCode(4121).SetNames("", "GCS_GGRS_1987", "D_GGRS_1987");
            Greek = ProjectionInfo.FromEpsgCode(4120).SetNames("", "GCS_Greek", "D_Greek");
            GreekAthens = ProjectionInfo.FromEpsgCode(4815).SetNames("", "GCS_Greek_Athens", "D_Greek");
            HD1909 = ProjectionInfo.FromAuthorityCode("EPSG", 104990).SetNames("", "GCS_HD1909", "D_Hungarian_Datum_1909"); // missing
            Hermannskogel = ProjectionInfo.FromAuthorityCode("ESRI", 104102).SetNames("", "GCS_Hermannskogel", "D_Hermannskogel");
            Hjorsey1955 = ProjectionInfo.FromEpsgCode(4658).SetNames("", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            HTRS96 = ProjectionInfo.FromEpsgCode(4761).SetNames("", "GCS_HTRS96", "D_Croatian_Terrestrial_Reference_System");
            HungarianDatum1972 = ProjectionInfo.FromEpsgCode(4237).SetNames("", "GCS_Hungarian_1972", "D_Hungarian_1972");
            IRENET95 = ProjectionInfo.FromEpsgCode(4173).SetNames("", "GCS_IRENET95", "D_IRENET95");
            ISN1993 = ProjectionInfo.FromEpsgCode(4659).SetNames("", "GCS_ISN_1993", "D_Islands_Network_1993");
            ISN2004 = ProjectionInfo.FromAuthorityCode("EPSG", 104144).SetNames("", "GCS_ISN_2004", "D_Islands_Network_2004"); // missing
            Kartastokoordinaattijarjestelma = ProjectionInfo.FromEpsgCode(4123).SetNames("", "GCS_KKJ", "D_KKJ");
            Lisbon = ProjectionInfo.FromEpsgCode(4207).SetNames("", "GCS_Lisbon", "D_Lisbon");
            Lisbon1890 = ProjectionInfo.FromEpsgCode(4666).SetNames("", "GCS_Lisbon_1890", "D_Lisbon_1890");
            Lisbon1890Lisbon = ProjectionInfo.FromEpsgCode(4904).SetNames("", "GCS_Lisbon_1890_Lisbon", "D_Lisbon_1890");
            LisbonLisbon = ProjectionInfo.FromEpsgCode(4803).SetNames("", "GCS_Lisbon_Lisbon", "D_Lisbon");
            LKS1992 = ProjectionInfo.FromEpsgCode(4661).SetNames("", "GCS_LKS_1992", "D_Latvia_1992");
            LKS1994 = ProjectionInfo.FromEpsgCode(4669).SetNames("", "GCS_LKS_1994", "D_Lithuania_1994");
            Luxembourg1930 = ProjectionInfo.FromEpsgCode(4181).SetNames("", "GCS_Luxembourg_1930", "D_Luxembourg_1930");
            Madrid1870Madrid = ProjectionInfo.FromEpsgCode(4903).SetNames("", "GCS_Madrid_1870_Madrid", "D_Madrid_1870");
            MGI1901 = ProjectionInfo.FromAuthorityCode("EPSG", 104992).SetNames("", "GCS_MGI_1901", "D_MGI_1901"); // missing
            MGIFerro = ProjectionInfo.FromEpsgCode(4805).SetNames("", "GCS_MGI_Ferro", "D_MGI");
            MilitarGeographischeInstitut = ProjectionInfo.FromEpsgCode(4312).SetNames("", "GCS_MGI", "D_MGI");
            MOLDREF99 = ProjectionInfo.FromEpsgCode(4023).SetNames("", "GCS_MOLDREF99", "D_MOLDREF99");
            MonteMario = ProjectionInfo.FromEpsgCode(4265).SetNames("", "GCS_Monte_Mario", "D_Monte_Mario");
            MonteMarioRome = ProjectionInfo.FromEpsgCode(4806).SetNames("", "GCS_Monte_Mario_Rome", "D_Monte_Mario");
            NGO1948 = ProjectionInfo.FromEpsgCode(4273).SetNames("", "GCS_NGO_1948", "D_NGO_1948");
            NGO1948Oslo = ProjectionInfo.FromEpsgCode(4817).SetNames("", "GCS_NGO_1948_Oslo", "D_NGO_1948");
            NorddeGuerreParis = ProjectionInfo.FromEpsgCode(4902).SetNames("", "GCS_Nord_de_Guerre_Paris", "D_Nord_de_Guerre");
            NouvelleTriangulationFrancaise = ProjectionInfo.FromEpsgCode(4275).SetNames("", "GCS_NTF", "D_NTF");
            NTFParis = ProjectionInfo.FromEpsgCode(4807).SetNames("", "GCS_NTF_Paris", "D_NTF");
            OSGB1936 = ProjectionInfo.FromEpsgCode(4277).SetNames("", "GCS_OSGB_1936", "D_OSGB_1936");
            OSGB1970SN = ProjectionInfo.FromEpsgCode(4278).SetNames("", "GCS_OSGB_1970_SN", "D_OSGB_1970_SN");
            OSNI1952 = ProjectionInfo.FromEpsgCode(4188).SetNames("", "GCS_OSNI_1952", "D_OSNI_1952");
            OSSN1980 = ProjectionInfo.FromEpsgCode(4279).SetNames("", "GCS_OS_SN_1980", "D_OS_SN_1980");
            PD83 = ProjectionInfo.FromEpsgCode(4746).SetNames("", "GCS_PD/83", "D_Potsdam_1983");
            Pulkovo1942 = ProjectionInfo.FromEpsgCode(4284).SetNames("", "GCS_Pulkovo_1942", "D_Pulkovo_1942");
            Pulkovo1942Adj1958 = ProjectionInfo.FromEpsgCode(4179).SetNames("", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1983 = ProjectionInfo.FromEpsgCode(4178).SetNames("", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            Pulkovo1995 = ProjectionInfo.FromEpsgCode(4200).SetNames("", "GCS_Pulkovo_1995", "D_Pulkovo_1995");
            PZ1990 = ProjectionInfo.FromEpsgCode(4740).SetNames("", "GCS_PZ_1990", "D_Parametrop_Zemp_1990");
            RD83 = ProjectionInfo.FromEpsgCode(4745).SetNames("", "GCS_RD/83", "D_Rauenberg_1983");
            ReseauNationalBelge1950 = ProjectionInfo.FromEpsgCode(4215).SetNames("", "GCS_Belge_1950", "D_Belge_1950");
            Reykjavik1900 = ProjectionInfo.FromEpsgCode(4657).SetNames("", "GCS_Reykjavik_1900", "D_Reykjavik_1900");
            RGF1993 = ProjectionInfo.FromEpsgCode(4171).SetNames("", "GCS_RGF_1993", "D_RGF_1993");
            Roma1940 = ProjectionInfo.FromAuthorityCode("ESRI", 104127).SetNames("", "GCS_Roma_1940", "D_Roma_1940"); // missing
            RT1990 = ProjectionInfo.FromEpsgCode(4124).SetNames("", "GCS_RT_1990", "D_RT_1990");
            RT38 = ProjectionInfo.FromEpsgCode(4308).SetNames("", "GCS_RT38", "D_Stockholm_1938");
            RT38Stockholm = ProjectionInfo.FromEpsgCode(4814).SetNames("", "GCS_RT38_Stockholm", "D_Stockholm_1938");
            S42Hungary = ProjectionInfo.FromAuthorityCode("ESRI", 37257).SetNames("", "GCS_S42_Hungary", "D_S42_Hungary");
            SJTSK = ProjectionInfo.FromEpsgCode(4156).SetNames("", "GCS_S_JTSK", "D_S_JTSK");
            SJTSKFerro = ProjectionInfo.FromEpsgCode(4818).SetNames("", "GCS_S_JTSK_Ferro", "D_S_JTSK");
            Slovenia1996 = ProjectionInfo.FromEpsgCode(4765).SetNames("", "GCS_Slovenia_1996", "D_Slovenia_Geodetic_Datum_1996");
            SREF98 = ProjectionInfo.FromEpsgCode(4075).SetNames("", "GCS_SREF98", "D_Serbian_Reference_Network_1998");
            SWEREF99 = ProjectionInfo.FromEpsgCode(4619).SetNames("", "GCS_SWEREF99", "D_SWEREF99");
            SwissTRF1995 = ProjectionInfo.FromEpsgCode(4151).SetNames("", "GCS_Swiss_TRF_1995", "D_Swiss_TRF_1995");
            TM65 = ProjectionInfo.FromEpsgCode(4299).SetNames("", "GCS_TM65", "D_TM65");
            TM75 = ProjectionInfo.FromEpsgCode(4300).SetNames("", "GCS_TM75", "D_TM75");
        }

        #endregion
    }
}

#pragma warning restore 1591