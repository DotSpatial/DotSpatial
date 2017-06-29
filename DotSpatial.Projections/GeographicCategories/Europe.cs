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
    /// Europe
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo ATFParis;
        public readonly ProjectionInfo Amersfoort;
        public readonly ProjectionInfo Albanian1987;
        public readonly ProjectionInfo Belge1950Brussels;
        public readonly ProjectionInfo Belge1972;
        public readonly ProjectionInfo Bern1898;
        public readonly ProjectionInfo Bern1898Bern;
        public readonly ProjectionInfo Bern1938;
        public readonly ProjectionInfo CH1903;
        public readonly ProjectionInfo Datum73;
        public readonly ProjectionInfo DatumLisboaBessel;
        public readonly ProjectionInfo DatumLisboaHayford;
        public readonly ProjectionInfo DealulPiscului1933Romania;
        public readonly ProjectionInfo DealulPiscului1970Romania;
        public readonly ProjectionInfo DeutscheHauptdreiecksnetz;
        public readonly ProjectionInfo DutchRD;
        public readonly ProjectionInfo ETRF1989;
        public readonly ProjectionInfo ETRS1989;
        public readonly ProjectionInfo EUREFFIN;
        public readonly ProjectionInfo Estonia1937;
        public readonly ProjectionInfo Estonia1992;
        public readonly ProjectionInfo Estonia1997;
        public readonly ProjectionInfo European1979;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EuropeanDatum1987;
        public readonly ProjectionInfo Greek;
        public readonly ProjectionInfo GreekAthens;
        public readonly ProjectionInfo GreekGeodeticRefSystem1987;
        public readonly ProjectionInfo Hermannskogel;
        public readonly ProjectionInfo Hjorsey1955;
        public readonly ProjectionInfo HungarianDatum1972;
        public readonly ProjectionInfo IRENET95;
        public readonly ProjectionInfo ISN1993;
        public readonly ProjectionInfo Kartastokoordinaattijarjestelma;
        public readonly ProjectionInfo LKS1992;
        public readonly ProjectionInfo LKS1994;
        public readonly ProjectionInfo Lisbon;
        public readonly ProjectionInfo Lisbon1890;
        public readonly ProjectionInfo Lisbon1890Lisbon;
        public readonly ProjectionInfo LisbonLisbon;
        public readonly ProjectionInfo Luxembourg1930;
        public readonly ProjectionInfo MGIFerro;
        public readonly ProjectionInfo Madrid1870Madrid;
        public readonly ProjectionInfo MilitarGeographischeInstitut;
        public readonly ProjectionInfo MonteMario;
        public readonly ProjectionInfo MonteMarioRome;
        public readonly ProjectionInfo NGO1948;
        public readonly ProjectionInfo NGO1948Oslo;
        public readonly ProjectionInfo NTFParis;
        public readonly ProjectionInfo NorddeGuerreParis;
        public readonly ProjectionInfo NouvelleTriangulationFrancaise;
        public readonly ProjectionInfo OSGB1936;
        public readonly ProjectionInfo OSGB1970SN;
        public readonly ProjectionInfo OSNI1952;
        public readonly ProjectionInfo OSSN1980;
        public readonly ProjectionInfo Pulkovo1942;
        public readonly ProjectionInfo Pulkovo1942Adj1958;
        public readonly ProjectionInfo Pulkovo1942Adj1983;
        public readonly ProjectionInfo Pulkovo1995;
        public readonly ProjectionInfo Qornoq;
        public readonly ProjectionInfo RGF1993;
        public readonly ProjectionInfo RT1990;
        public readonly ProjectionInfo RT38;
        public readonly ProjectionInfo RT38Stockholm;
        public readonly ProjectionInfo ReseauNationalBelge1950;
        public readonly ProjectionInfo ReseauNationalBelge1972;
        public readonly ProjectionInfo Reykjavik1900;
        public readonly ProjectionInfo Roma1940;
        public readonly ProjectionInfo S42Hungary;
        public readonly ProjectionInfo SJTSK;
        public readonly ProjectionInfo SWEREF99;
        public readonly ProjectionInfo SwissTRF1995;
        public readonly ProjectionInfo TM65;
        public readonly ProjectionInfo TM75;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe
        /// </summary>
        public Europe()
        {
            Albanian1987 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Amersfoort = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +towgs84=565.2369,50.0087,465.658,-0.406857330322398,0.350732676542563,-1.8703473836068,4.0812 +no_defs "); 
            ATFParis = ProjectionInfo.FromProj4String("+proj=longlat +a=6376523 +b=6355862.933255573 +pm=2.337229166666667 +no_defs ");
            Belge1950Brussels = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +pm=4.367975 +no_defs ");
            Belge1972 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Bern1898 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Bern1898Bern = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=7.439583333333333 +no_defs ");
            Bern1938 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            CH1903 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Datum73 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            DatumLisboaBessel = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            DatumLisboaHayford = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            DealulPiscului1933Romania = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            DealulPiscului1970Romania = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            DeutscheHauptdreiecksnetz = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            DutchRD = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +towgs84=565.2369,50.0087,465.658,-0.406857330322398,0.350732676542563,-1.8703473836068,4.0812 +no_defs "); 
            Estonia1937 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Estonia1992 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Estonia1997 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ETRF1989 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            ETRS1989 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            EUREFFIN = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            European1979 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1950 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1987 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Greek = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            GreekAthens = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=23.7163375 +no_defs ");
            GreekGeodeticRefSystem1987 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Hermannskogel = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Hjorsey1955 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            HungarianDatum1972 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS67 +no_defs ");
            IRENET95 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ISN1993 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Kartastokoordinaattijarjestelma = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Lisbon = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            LisbonLisbon = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +pm=-9.131906111111112 +no_defs ");
            Lisbon1890 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Lisbon1890Lisbon = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=-9.131906111111112 +no_defs ");
            LKS1992 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            LKS1994 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Luxembourg1930 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Madrid1870Madrid = ProjectionInfo.FromProj4String("+proj=longlat +a=6378298.3 +b=6356657.142669561 +pm=-3.687938888888889 +no_defs ");
            MGIFerro = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=-17.66666666666667 +no_defs ");
            MilitarGeographischeInstitut = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            MonteMario = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            MonteMarioRome = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +pm=12.45233333333333 +no_defs ");
            NGO1948 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377492.018 +b=6356173.508712696 +no_defs ");
            NGO1948Oslo = ProjectionInfo.FromProj4String("+proj=longlat +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +no_defs ");
            NorddeGuerreParis = ProjectionInfo.FromProj4String("+proj=longlat +a=6376523 +b=6355862.933255573 +pm=2.337229166666667 +no_defs ");
            NouvelleTriangulationFrancaise = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            NTFParis = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +no_defs ");
            OSSN1980 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=airy +no_defs ");
            OSGB1936 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=airy +no_defs ");
            OSGB1970SN = ProjectionInfo.FromProj4String("+proj=longlat +ellps=airy +no_defs ");
            OSNI1952 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=airy +no_defs ");
            Pulkovo1942 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1942Adj1958 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1942Adj1983 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            Qornoq = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ReseauNationalBelge1950 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ReseauNationalBelge1972 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Reykjavik1900 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377019.27 +b=6355762.5391 +no_defs ");
            RGF1993 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Roma1940 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            RT1990 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            RT38 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            RT38Stockholm = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +pm=18.05827777777778 +no_defs ");
            S42Hungary = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            SJTSK = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            SWEREF99 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            SwissTRF1995 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            TM65 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377340.189 +b=6356034.447938534 +no_defs ");
            TM75 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377340.189 +b=6356034.447938534 +no_defs ");

            Albanian1987.GeographicInfo.Name = "GCS_Albanian_1987";
            ATFParis.GeographicInfo.Name = "GCS_ATF_Paris";
            Belge1950Brussels.GeographicInfo.Name = "GCS_Belge_1950_Brussels";
            Belge1972.GeographicInfo.Name = "GCS_Belge_1972";
            Bern1898.GeographicInfo.Name = "GCS_Bern_1898";
            Bern1898Bern.GeographicInfo.Name = "GCS_Bern_1898_Bern";
            Bern1938.GeographicInfo.Name = "GCS_Bern_1938";
            CH1903.GeographicInfo.Name = "GCS_CH1903";
            Datum73.GeographicInfo.Name = "GCS_Datum_73";
            DatumLisboaBessel.GeographicInfo.Name = "GCS_Datum_Lisboa_Bessel";
            DatumLisboaHayford.GeographicInfo.Name = "GCS_Datum_Lisboa_Hayford";
            DealulPiscului1933Romania.GeographicInfo.Name = "GCS_Dealul_Piscului_1933";
            DealulPiscului1970Romania.GeographicInfo.Name = "GCS_Dealul_Piscului_1970";
            DeutscheHauptdreiecksnetz.GeographicInfo.Name = "GCS_Deutsches_Hauptdreiecksnetz";
            Estonia1937.GeographicInfo.Name = "GCS_Estonia_1937";
            Estonia1992.GeographicInfo.Name = "GCS_Estonia_1992";
            Estonia1997.GeographicInfo.Name = "GCS_Estonia_1997";
            ETRF1989.GeographicInfo.Name = "GCS_ETRF_1989";
            ETRS1989.GeographicInfo.Name = "GCS_ETRS_1989";
            EUREFFIN.GeographicInfo.Name = "GCS_EUREF_FIN";
            European1979.GeographicInfo.Name = "GCS_European_1979";
            EuropeanDatum1950.GeographicInfo.Name = "GCS_European_1950";
            EuropeanDatum1987.GeographicInfo.Name = "GCS_European_1987";
            Greek.GeographicInfo.Name = "GCS_Greek";
            GreekAthens.GeographicInfo.Name = "GCS_Greek_Athens";
            GreekGeodeticRefSystem1987.GeographicInfo.Name = "GCS_GGRS_1987";
            Hermannskogel.GeographicInfo.Name = "GCS_Hermannskogel";
            Hjorsey1955.GeographicInfo.Name = "GCS_Hjorsey_1955";
            HungarianDatum1972.GeographicInfo.Name = "GCS_Hungarian_1972";
            IRENET95.GeographicInfo.Name = "GCS_IRENET95";
            ISN1993.GeographicInfo.Name = "GCS_ISN_1993";
            Kartastokoordinaattijarjestelma.GeographicInfo.Name = "GCS_KKJ";
            Lisbon.GeographicInfo.Name = "GCS_Lisbon";
            LisbonLisbon.GeographicInfo.Name = "GCS_Lisbon_Lisbon";
            Lisbon1890.GeographicInfo.Name = "GCS_Lisbon_1890";
            Lisbon1890Lisbon.GeographicInfo.Name = "GCS_Lisbon_1890_Lisbon";
            LKS1992.GeographicInfo.Name = "GCS_LKS_1992";
            LKS1994.GeographicInfo.Name = "GCS_LKS_1994";
            Luxembourg1930.GeographicInfo.Name = "GCS_Luxembourg_1930";
            Madrid1870Madrid.GeographicInfo.Name = "GCS_Madrid_1870_Madrid";
            MGIFerro.GeographicInfo.Name = "GCS_MGI_Ferro";
            MilitarGeographischeInstitut.GeographicInfo.Name = "GCS_MGI";
            MonteMario.GeographicInfo.Name = "GCS_Monte_Mario";
            MonteMarioRome.GeographicInfo.Name = "GCS_Monte_Mario_Rome";
            NGO1948.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948Oslo.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NorddeGuerreParis.GeographicInfo.Name = "GCS_Nord_de_Guerre_Paris";
            NouvelleTriangulationFrancaise.GeographicInfo.Name = "GCS_NTF";
            NTFParis.GeographicInfo.Name = "GCS_NTF_Paris";
            OSSN1980.GeographicInfo.Name = "GCS_OS_SN_1980";
            OSGB1936.GeographicInfo.Name = "GCS_OSGB_1936";
            OSGB1970SN.GeographicInfo.Name = "GCS_OSGB_1970_SN";
            OSNI1952.GeographicInfo.Name = "GCS_OSNI_1952";
            Pulkovo1942.GeographicInfo.Name = "GCS_Pulkovo_1942";
            Pulkovo1942Adj1958.GeographicInfo.Name = "GCS_Pulkovo_1942_Adj_1958";
            Pulkovo1942Adj1983.GeographicInfo.Name = "GCS_Pulkovo_1942_Adj_1983";
            Pulkovo1995.GeographicInfo.Name = "GCS_Pulkovo_1995";
            Qornoq.GeographicInfo.Name = "GCS_Qornoq";
            ReseauNationalBelge1950.GeographicInfo.Name = "GCS_Belge_1950";
            ReseauNationalBelge1972.GeographicInfo.Name = "GCS_Belge_1972";
            Reykjavik1900.GeographicInfo.Name = "GCS_Reykjavik_1900";
            RGF1993.GeographicInfo.Name = "GCS_RGF_1993";
            Roma1940.GeographicInfo.Name = "GCS_Roma_1940";
            RT1990.GeographicInfo.Name = "GCS_RT_1990";
            RT38.GeographicInfo.Name = "GCS_RT38";
            RT38Stockholm.GeographicInfo.Name = "GCS_RT38_Stockholm";
            S42Hungary.GeographicInfo.Name = "GCS_S42_Hungary";
            SJTSK.GeographicInfo.Name = "GCS_S_JTSK";
            SWEREF99.GeographicInfo.Name = "GCS_SWEREF99";
            SwissTRF1995.GeographicInfo.Name = "GCS_Swiss_TRF_1995";
            TM65.GeographicInfo.Name = "GCS_TM65";
            TM75.GeographicInfo.Name = "GCS_TM75";

            Albanian1987.GeographicInfo.Datum.Name = "D_Albanian_1987";
            ATFParis.GeographicInfo.Datum.Name = "D_ATF";
            Belge1950Brussels.GeographicInfo.Datum.Name = "D_Belge_1950";
            Belge1972.GeographicInfo.Datum.Name = "D_Belge_1972";
            Bern1898.GeographicInfo.Datum.Name = "D_Bern_1898";
            Bern1898Bern.GeographicInfo.Datum.Name = "D_Bern_1898";
            Bern1938.GeographicInfo.Datum.Name = "D_Bern_1938";
            CH1903.GeographicInfo.Datum.Name = "D_CH1903";
            Datum73.GeographicInfo.Datum.Name = "D_Datum_73";
            DatumLisboaBessel.GeographicInfo.Datum.Name = "D_Datum_Lisboa_Bessel";
            DatumLisboaHayford.GeographicInfo.Datum.Name = "D_Datum_Lisboa_Hayford";
            DealulPiscului1933Romania.GeographicInfo.Datum.Name = "D_Dealul_Piscului_1933";
            DealulPiscului1970Romania.GeographicInfo.Datum.Name = "D_Dealul_Piscului_1970";
            DeutscheHauptdreiecksnetz.GeographicInfo.Datum.Name = "D_Deutsches_Hauptdreiecksnetz";
            Estonia1937.GeographicInfo.Datum.Name = "D_Estonia_1937";
            Estonia1992.GeographicInfo.Datum.Name = "D_Estonia_1992";
            Estonia1997.GeographicInfo.Datum.Name = "D_Estonia_1997";
            ETRF1989.GeographicInfo.Datum.Name = "D_ETRF_1989";
            ETRS1989.GeographicInfo.Datum.Name = "D_ETRS_1989";
            EUREFFIN.GeographicInfo.Datum.Name = "D_ETRS_1989";
            European1979.GeographicInfo.Datum.Name = "D_European_1979";
            EuropeanDatum1950.GeographicInfo.Datum.Name = "D_European_1950";
            EuropeanDatum1987.GeographicInfo.Datum.Name = "D_European_1987";
            Greek.GeographicInfo.Datum.Name = "D_Greek";
            GreekAthens.GeographicInfo.Datum.Name = "D_Greek";
            GreekGeodeticRefSystem1987.GeographicInfo.Datum.Name = "D_GGRS_1987";
            Hermannskogel.GeographicInfo.Datum.Name = "D_Hermannskogel";
            Hjorsey1955.GeographicInfo.Datum.Name = "D_Hjorsey_1955";
            HungarianDatum1972.GeographicInfo.Datum.Name = "D_Hungarian_1972";
            IRENET95.GeographicInfo.Datum.Name = "D_IRENET95";
            ISN1993.GeographicInfo.Datum.Name = "D_Islands_Network_1993";
            Kartastokoordinaattijarjestelma.GeographicInfo.Datum.Name = "D_KKJ";
            Lisbon.GeographicInfo.Datum.Name = "D_Lisbon";
            LisbonLisbon.GeographicInfo.Datum.Name = "D_Lisbon";
            Lisbon1890.GeographicInfo.Datum.Name = "D_Lisbon_1890";
            Lisbon1890Lisbon.GeographicInfo.Datum.Name = "D_Lisbon_1890";
            LKS1992.GeographicInfo.Datum.Name = "D_Latvia_1992";
            LKS1994.GeographicInfo.Datum.Name = "D_Lithuania_1994";
            Luxembourg1930.GeographicInfo.Datum.Name = "D_Luxembourg_1930";
            Madrid1870Madrid.GeographicInfo.Datum.Name = "D_Madrid_1870";
            MGIFerro.GeographicInfo.Datum.Name = "D_MGI";
            MilitarGeographischeInstitut.GeographicInfo.Datum.Name = "D_MGI";
            MonteMario.GeographicInfo.Datum.Name = "D_Monte_Mario";
            MonteMarioRome.GeographicInfo.Datum.Name = "D_Monte_Mario";
            NGO1948.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948Oslo.GeographicInfo.Datum.Name = "D_NGO_1948";
            NorddeGuerreParis.GeographicInfo.Datum.Name = "D_Nord_de_Guerre";
            NouvelleTriangulationFrancaise.GeographicInfo.Datum.Name = "D_NTF";
            NTFParis.GeographicInfo.Datum.Name = "D_NTF";
            OSSN1980.GeographicInfo.Datum.Name = "D_OS_SN_1980";
            OSGB1936.GeographicInfo.Datum.Name = "D_OSGB_1936";
            OSGB1970SN.GeographicInfo.Datum.Name = "D_OSGB_1970_SN";
            OSNI1952.GeographicInfo.Datum.Name = "D_OSNI_1952";
            Pulkovo1942.GeographicInfo.Datum.Name = "D_Pulkovo_1942";
            Pulkovo1942Adj1958.GeographicInfo.Datum.Name = "D_Pulkovo_1942_Adj_1958";
            Pulkovo1942Adj1983.GeographicInfo.Datum.Name = "D_Pulkovo_1942_Adj_1983";
            Pulkovo1995.GeographicInfo.Datum.Name = "D_Pulkovo_1995";
            Qornoq.GeographicInfo.Datum.Name = "D_Qornoq";
            ReseauNationalBelge1950.GeographicInfo.Datum.Name = "D_Belge_1950";
            ReseauNationalBelge1972.GeographicInfo.Datum.Name = "D_Belge_1972";
            Reykjavik1900.GeographicInfo.Datum.Name = "D_Reykjavik_1900";
            RGF1993.GeographicInfo.Datum.Name = "D_RGF_1993";
            Roma1940.GeographicInfo.Datum.Name = "D_Roma_1940";
            RT1990.GeographicInfo.Datum.Name = "D_RT_1990";
            RT38.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            RT38Stockholm.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            S42Hungary.GeographicInfo.Datum.Name = "D_S42_Hungary";
            SJTSK.GeographicInfo.Datum.Name = "D_S_JTSK";
            SWEREF99.GeographicInfo.Datum.Name = "D_SWEREF99";
            SwissTRF1995.GeographicInfo.Datum.Name = "D_Swiss_TRF_1995";
            TM65.GeographicInfo.Datum.Name = "D_TM65";
            TM75.GeographicInfo.Datum.Name = "D_TM75";
        }

        #endregion
    }
}

#pragma warning restore 1591