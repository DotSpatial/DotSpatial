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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:57:57 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// StatePlaneNad1983
    /// </summary>
    public class StatePlaneNad1983 : CoordinateSystemCategory
    {
        #region Private Variables

        ///<summary>
        /// Michigan Geo Ref 2008
        ///</summary>
        public readonly ProjectionInfo MichiganGeoRef2008;

        ///<summary>
        ///</summary>
        public readonly ProjectionInfo NAD1983Maine2000CentralZone;

        public readonly ProjectionInfo NAD1983Maine2000EastZone;
        public readonly ProjectionInfo NAD1983Maine2000WestZone;
        public readonly ProjectionInfo NAD1983StatePlaneAlabamaEastFIPS0101;
        public readonly ProjectionInfo NAD1983StatePlaneAlabamaWestFIPS0102;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska10FIPS5010;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska1FIPS5001;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska2FIPS5002;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska3FIPS5003;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska4FIPS5004;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska5FIPS5005;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska6FIPS5006;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska7FIPS5007;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska8FIPS5008;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska9FIPS5009;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaCentralFIPS0202;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaEastFIPS0201;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaWestFIPS0203;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasNorthFIPS0301;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasSouthFIPS0302;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIFIPS0401;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIFIPS0402;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIIFIPS0403;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIVFIPS0404;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVFIPS0405;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVIFIPS0406;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoCentralFIPS0502;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoNorthFIPS0501;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoSouthFIPS0503;
        public readonly ProjectionInfo NAD1983StatePlaneConnecticutFIPS0600;
        public readonly ProjectionInfo NAD1983StatePlaneDelawareFIPS0700;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaEastFIPS0901;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaNorthFIPS0903;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaWestFIPS0902;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaEastFIPS1001;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaWestFIPS1002;
        public readonly ProjectionInfo NAD1983StatePlaneGuamFIPS5400;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii1FIPS5101;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii2FIPS5102;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii3FIPS5103;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii4FIPS5104;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii5FIPS5105;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoCentralFIPS1102;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoEastFIPS1101;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoWestFIPS1103;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisEastFIPS1201;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisWestFIPS1202;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaEastFIPS1301;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaWestFIPS1302;
        public readonly ProjectionInfo NAD1983StatePlaneIowaNorthFIPS1401;
        public readonly ProjectionInfo NAD1983StatePlaneIowaSouthFIPS1402;
        public readonly ProjectionInfo NAD1983StatePlaneKansasNorthFIPS1501;
        public readonly ProjectionInfo NAD1983StatePlaneKansasSouthFIPS1502;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyFIPS1600;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyNorthFIPS1601;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckySouthFIPS1602;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaNorthFIPS1701;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaSouthFIPS1702;
        public readonly ProjectionInfo NAD1983StatePlaneMaineEastFIPS1801;
        public readonly ProjectionInfo NAD1983StatePlaneMaineWestFIPS1802;
        public readonly ProjectionInfo NAD1983StatePlaneMarylandFIPS1900;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsIslandFIPS2002;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsMainlandFIPS2001;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganCentralFIPS2112;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganNorthFIPS2111;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganSouthFIPS2113;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaCentralFIPS2202;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaNorthFIPS2201;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaSouthFIPS2203;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiEastFIPS2301;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiWestFIPS2302;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriCentralFIPS2402;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriEastFIPS2401;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriWestFIPS2403;
        public readonly ProjectionInfo NAD1983StatePlaneMontanaFIPS2500;
        public readonly ProjectionInfo NAD1983StatePlaneNebraskaFIPS2600;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaCentralFIPS2702;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaEastFIPS2701;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaWestFIPS2703;
        public readonly ProjectionInfo NAD1983StatePlaneNewHampshireFIPS2800;
        public readonly ProjectionInfo NAD1983StatePlaneNewJerseyFIPS2900;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoCentralFIPS3002;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoEastFIPS3001;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoWestFIPS3003;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkCentralFIPS3102;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkEastFIPS3101;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkLongIslandFIPS3104;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkWestFIPS3103;
        public readonly ProjectionInfo NAD1983StatePlaneNorthCarolinaFIPS3200;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaNorthFIPS3301;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaSouthFIPS3302;
        public readonly ProjectionInfo NAD1983StatePlaneOhioNorthFIPS3401;
        public readonly ProjectionInfo NAD1983StatePlaneOhioSouthFIPS3402;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaNorthFIPS3501;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaSouthFIPS3502;
        public readonly ProjectionInfo NAD1983StatePlaneOregonNorthFIPS3601;
        public readonly ProjectionInfo NAD1983StatePlaneOregonSouthFIPS3602;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaNorthFIPS3701;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaSouthFIPS3702;
        public readonly ProjectionInfo NAD1983StatePlanePuertoRicoVirginIslandsFIPS5200;
        public readonly ProjectionInfo NAD1983StatePlaneRhodeIslandFIPS3800;
        public readonly ProjectionInfo NAD1983StatePlaneSouthCarolinaFIPS3900;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaNorthFIPS4001;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaSouthFIPS4002;
        public readonly ProjectionInfo NAD1983StatePlaneTennesseeFIPS4100;
        public readonly ProjectionInfo NAD1983StatePlaneTexasCentralFIPS4203;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNorthCentralFIPS4202;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNorthFIPS4201;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSouthCentralFIPS4204;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSouthFIPS4205;
        public readonly ProjectionInfo NAD1983StatePlaneUtahCentralFIPS4302;
        public readonly ProjectionInfo NAD1983StatePlaneUtahNorthFIPS4301;
        public readonly ProjectionInfo NAD1983StatePlaneUtahSouthFIPS4303;
        public readonly ProjectionInfo NAD1983StatePlaneVermontFIPS4400;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaNorthFIPS4501;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaSouthFIPS4502;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonNorthFIPS4601;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonSouthFIPS4602;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaNorthFIPS4701;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaSouthFIPS4702;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinCentralFIPS4802;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinNorthFIPS4801;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinSouthFIPS4803;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingEastCentralFIPS4902;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingEastFIPS4901;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWestCentralFIPS4903;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWestFIPS4904;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneNad1983
        /// </summary>
        public StatePlaneNad1983()
        {
            MichiganGeoRef2008 =
                ProjectionInfo.FromProj4String("+proj=omerc +lat_0=45.30916666666666 +lonc=-86 +alpha=337.25556 +k=0.9996 +x_0=2546731.496 +y_0=-4354009.816 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983Maine2000CentralZone =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=43.5 +lon_0=-69.125 +k=0.999980 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983Maine2000EastZone =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=43.83333333333334 +lon_0=-67.875 +k=0.999980 +x_0=700000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983Maine2000WestZone =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=42.83333333333334 +lon_0=-70.375 +k=0.999980 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlabamaEastFIPS0101 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=30.5 +lon_0=-85.83333333333333 +k=0.999960 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlabamaWestFIPS0102 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=30 +lon_0=-87.5 +k=0.999933 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska10FIPS5010 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=51.83333333333334 +lat_2=53.83333333333334 +lat_0=51 +lon_0=-176 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska1FIPS5001 =
                ProjectionInfo.FromProj4String("+proj=omerc +lat_0=57 +lonc=-133.6666666666667 +alpha=-36.86989764583333 +k=0.9999 +x_0=5000000 +y_0=-5000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska2FIPS5002 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-142 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska3FIPS5003 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-146 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska4FIPS5004 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-150 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska5FIPS5005 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-154 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska6FIPS5006 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-158 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska7FIPS5007 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-162 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska8FIPS5008 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-166 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneAlaska9FIPS5009 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=54 +lon_0=-170 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneArizonaCentralFIPS0202 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-111.9166666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneArizonaEastFIPS0201 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-110.1666666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneArizonaWestFIPS0203 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-113.75 +k=0.999933 +x_0=213360 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneArkansasNorthFIPS0301 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=34.93333333333333 +lat_2=36.23333333333333 +lat_0=34.33333333333334 +lon_0=-92 +x_0=400000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneArkansasSouthFIPS0302 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=33.3 +lat_2=34.76666666666667 +lat_0=32.66666666666666 +lon_0=-92 +x_0=400000 +y_0=400000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneCaliforniaIFIPS0401 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40 +lat_2=41.66666666666666 +lat_0=39.33333333333334 +lon_0=-122 +x_0=2000000 +y_0=500000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneCaliforniaIIFIPS0402 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.33333333333334 +lat_2=39.83333333333334 +lat_0=37.66666666666666 +lon_0=-122 +x_0=2000000 +y_0=500000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneCaliforniaIIIFIPS0403 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.06666666666667 +lat_2=38.43333333333333 +lat_0=36.5 +lon_0=-120.5 +x_0=2000000 +y_0=500000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneCaliforniaIVFIPS0404 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=36 +lat_2=37.25 +lat_0=35.33333333333334 +lon_0=-119 +x_0=2000000 +y_0=500000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneCaliforniaVFIPS0405 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=34.03333333333333 +lat_2=35.46666666666667 +lat_0=33.5 +lon_0=-118 +x_0=2000000 +y_0=500000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneCaliforniaVIFIPS0406 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.78333333333333 +lat_2=33.88333333333333 +lat_0=32.16666666666666 +lon_0=-116.25 +x_0=2000000 +y_0=500000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneColoradoCentralFIPS0502 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.45 +lat_2=39.75 +lat_0=37.83333333333334 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneColoradoNorthFIPS0501 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=39.71666666666667 +lat_2=40.78333333333333 +lat_0=39.33333333333334 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneColoradoSouthFIPS0503 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.23333333333333 +lat_2=38.43333333333333 +lat_0=36.66666666666666 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneConnecticutFIPS0600 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=41.2 +lat_2=41.86666666666667 +lat_0=40.83333333333334 +lon_0=-72.75 +x_0=304800.6096 +y_0=152400.3048 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneDelawareFIPS0700 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=38 +lon_0=-75.41666666666667 +k=0.999995 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneFloridaEastFIPS0901 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-81 +k=0.999941 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneFloridaNorthFIPS0903 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=29.58333333333333 +lat_2=30.75 +lat_0=29 +lon_0=-84.5 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneFloridaWestFIPS0902 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-82 +k=0.999941 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneGeorgiaEastFIPS1001 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=30 +lon_0=-82.16666666666667 +k=0.999900 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneGeorgiaWestFIPS1002 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=30 +lon_0=-84.16666666666667 +k=0.999900 +x_0=700000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneGuamFIPS5400 =
                ProjectionInfo.FromProj4String("+proj=poly +lat_0=13.47246635277778 +lon_0=144.7487507055556 +x_0=50000 +y_0=50000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneHawaii1FIPS5101 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=18.83333333333333 +lon_0=-155.5 +k=0.999967 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneHawaii2FIPS5102 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=20.33333333333333 +lon_0=-156.6666666666667 +k=0.999967 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneHawaii3FIPS5103 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.16666666666667 +lon_0=-158 +k=0.999990 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneHawaii4FIPS5104 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.83333333333333 +lon_0=-159.5 +k=0.999990 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneHawaii5FIPS5105 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.66666666666667 +lon_0=-160.1666666666667 +k=1.000000 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIdahoCentralFIPS1102 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-114 +k=0.999947 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIdahoEastFIPS1101 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-112.1666666666667 +k=0.999947 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIdahoWestFIPS1103 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-115.75 +k=0.999933 +x_0=800000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIllinoisEastFIPS1201 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-88.33333333333333 +k=0.999975 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIllinoisWestFIPS1202 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-90.16666666666667 +k=0.999941 +x_0=700000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIndianaEastFIPS1301 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=37.5 +lon_0=-85.66666666666667 +k=0.999967 +x_0=100000 +y_0=250000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIndianaWestFIPS1302 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=37.5 +lon_0=-87.08333333333333 +k=0.999967 +x_0=900000 +y_0=250000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIowaNorthFIPS1401 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.06666666666667 +lat_2=43.26666666666667 +lat_0=41.5 +lon_0=-93.5 +x_0=1500000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneIowaSouthFIPS1402 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40.61666666666667 +lat_2=41.78333333333333 +lat_0=40 +lon_0=-93.5 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneKansasNorthFIPS1501 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.71666666666667 +lat_2=39.78333333333333 +lat_0=38.33333333333334 +lon_0=-98 +x_0=400000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneKansasSouthFIPS1502 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.26666666666667 +lat_2=38.56666666666667 +lat_0=36.66666666666666 +lon_0=-98.5 +x_0=400000 +y_0=400000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneKentuckyFIPS1600 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.08333333333334 +lat_2=38.66666666666666 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=1500000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneKentuckyNorthFIPS1601 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.96666666666667 +lat_2=38.96666666666667 +lat_0=37.5 +lon_0=-84.25 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneKentuckySouthFIPS1602 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=36.73333333333333 +lat_2=37.93333333333333 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=500000 +y_0=500000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneLouisianaNorthFIPS1701 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=31.16666666666667 +lat_2=32.66666666666666 +lat_0=30.5 +lon_0=-92.5 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneLouisianaSouthFIPS1702 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=29.3 +lat_2=30.7 +lat_0=28.5 +lon_0=-91.33333333333333 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMaineEastFIPS1801 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=43.66666666666666 +lon_0=-68.5 +k=0.999900 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMaineWestFIPS1802 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=42.83333333333334 +lon_0=-70.16666666666667 +k=0.999967 +x_0=900000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMarylandFIPS1900 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.3 +lat_2=39.45 +lat_0=37.66666666666666 +lon_0=-77 +x_0=400000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMassachusettsIslandFIPS2002 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=41.28333333333333 +lat_2=41.48333333333333 +lat_0=41 +lon_0=-70.5 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMassachusettsMainlandFIPS2001 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=41.71666666666667 +lat_2=42.68333333333333 +lat_0=41 +lon_0=-71.5 +x_0=200000 +y_0=750000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMichiganCentralFIPS2112 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.36666666666666 +x_0=6000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMichiganNorthFIPS2111 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=8000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMichiganSouthFIPS2113 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.36666666666666 +x_0=4000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMinnesotaCentralFIPS2202 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.61666666666667 +lat_2=47.05 +lat_0=45 +lon_0=-94.25 +x_0=800000 +y_0=100000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMinnesotaNorthFIPS2201 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=47.03333333333333 +lat_2=48.63333333333333 +lat_0=46.5 +lon_0=-93.09999999999999 +x_0=800000 +y_0=100000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMinnesotaSouthFIPS2203 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=43.78333333333333 +lat_2=45.21666666666667 +lat_0=43 +lon_0=-94 +x_0=800000 +y_0=100000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMississippiEastFIPS2301 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=29.5 +lon_0=-88.83333333333333 +k=0.999950 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMississippiWestFIPS2302 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=29.5 +lon_0=-90.33333333333333 +k=0.999950 +x_0=700000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMissouriCentralFIPS2402 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-92.5 +k=0.999933 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMissouriEastFIPS2401 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-90.5 +k=0.999933 +x_0=250000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMissouriWestFIPS2403 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36.16666666666666 +lon_0=-94.5 +k=0.999941 +x_0=850000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneMontanaFIPS2500 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45 +lat_2=49 +lat_0=44.25 +lon_0=-109.5 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNebraskaFIPS2600 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40 +lat_2=43 +lat_0=39.83333333333334 +lon_0=-100 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNevadaCentralFIPS2702 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=34.75 +lon_0=-116.6666666666667 +k=0.999900 +x_0=500000 +y_0=6000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNevadaEastFIPS2701 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=34.75 +lon_0=-115.5833333333333 +k=0.999900 +x_0=200000 +y_0=8000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNevadaWestFIPS2703 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=34.75 +lon_0=-118.5833333333333 +k=0.999900 +x_0=800000 +y_0=4000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewHampshireFIPS2800 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=42.5 +lon_0=-71.66666666666667 +k=0.999967 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewJerseyFIPS2900 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewMexicoCentralFIPS3002 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-106.25 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewMexicoEastFIPS3001 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-104.3333333333333 +k=0.999909 +x_0=165000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewMexicoWestFIPS3003 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-107.8333333333333 +k=0.999917 +x_0=830000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewYorkCentralFIPS3102 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40 +lon_0=-76.58333333333333 +k=0.999938 +x_0=250000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewYorkEastFIPS3101 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewYorkLongIslandFIPS3104 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40.66666666666666 +lat_2=41.03333333333333 +lat_0=40.16666666666666 +lon_0=-74 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNewYorkWestFIPS3103 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40 +lon_0=-78.58333333333333 +k=0.999938 +x_0=350000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNorthCarolinaFIPS3200 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=34.33333333333334 +lat_2=36.16666666666666 +lat_0=33.75 +lon_0=-79 +x_0=609601.22 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNorthDakotaNorthFIPS3301 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=47.43333333333333 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneNorthDakotaSouthFIPS3302 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=46.18333333333333 +lat_2=47.48333333333333 +lat_0=45.66666666666666 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneOhioNorthFIPS3401 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40.43333333333333 +lat_2=41.7 +lat_0=39.66666666666666 +lon_0=-82.5 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneOhioSouthFIPS3402 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.73333333333333 +lat_2=40.03333333333333 +lat_0=38 +lon_0=-82.5 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneOklahomaNorthFIPS3501 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=35.56666666666667 +lat_2=36.76666666666667 +lat_0=35 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneOklahomaSouthFIPS3502 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=33.93333333333333 +lat_2=35.23333333333333 +lat_0=33.33333333333334 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneOregonNorthFIPS3601 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.33333333333334 +lat_2=46 +lat_0=43.66666666666666 +lon_0=-120.5 +x_0=2500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneOregonSouthFIPS3602 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.33333333333334 +lat_2=44 +lat_0=41.66666666666666 +lon_0=-120.5 +x_0=1500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlanePennsylvaniaNorthFIPS3701 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40.88333333333333 +lat_2=41.95 +lat_0=40.16666666666666 +lon_0=-77.75 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlanePennsylvaniaSouthFIPS3702 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=39.93333333333333 +lat_2=40.96666666666667 +lat_0=39.33333333333334 +lon_0=-77.75 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlanePuertoRicoVirginIslandsFIPS5200 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=200000 +y_0=200000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneRhodeIslandFIPS3800 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.08333333333334 +lon_0=-71.5 +k=0.999994 +x_0=100000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneSouthCarolinaFIPS3900 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.5 +lat_2=34.83333333333334 +lat_0=31.83333333333333 +lon_0=-81 +x_0=609600 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneSouthDakotaNorthFIPS4001 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.41666666666666 +lat_2=45.68333333333333 +lat_0=43.83333333333334 +lon_0=-100 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneSouthDakotaSouthFIPS4002 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.83333333333334 +lat_2=44.4 +lat_0=42.33333333333334 +lon_0=-100.3333333333333 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneTennesseeFIPS4100 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=35.25 +lat_2=36.41666666666666 +lat_0=34.33333333333334 +lon_0=-86 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneTexasCentralFIPS4203 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=30.11666666666667 +lat_2=31.88333333333333 +lat_0=29.66666666666667 +lon_0=-100.3333333333333 +x_0=700000 +y_0=3000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneTexasNorthCentralFIPS4202 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.13333333333333 +lat_2=33.96666666666667 +lat_0=31.66666666666667 +lon_0=-98.5 +x_0=600000 +y_0=2000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneTexasNorthFIPS4201 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=34.65 +lat_2=36.18333333333333 +lat_0=34 +lon_0=-101.5 +x_0=200000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneTexasSouthCentralFIPS4204 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=28.38333333333333 +lat_2=30.28333333333333 +lat_0=27.83333333333333 +lon_0=-99 +x_0=600000 +y_0=4000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneTexasSouthFIPS4205 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26.16666666666667 +lat_2=27.83333333333333 +lat_0=25.66666666666667 +lon_0=-98.5 +x_0=300000 +y_0=5000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneUtahCentralFIPS4302 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=39.01666666666667 +lat_2=40.65 +lat_0=38.33333333333334 +lon_0=-111.5 +x_0=500000 +y_0=2000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneUtahNorthFIPS4301 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40.71666666666667 +lat_2=41.78333333333333 +lat_0=40.33333333333334 +lon_0=-111.5 +x_0=500000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneUtahSouthFIPS4303 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.21666666666667 +lat_2=38.35 +lat_0=36.66666666666666 +lon_0=-111.5 +x_0=500000 +y_0=3000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneVermontFIPS4400 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=42.5 +lon_0=-72.5 +k=0.999964 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneVirginiaNorthFIPS4501 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.03333333333333 +lat_2=39.2 +lat_0=37.66666666666666 +lon_0=-78.5 +x_0=3500000 +y_0=2000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneVirginiaSouthFIPS4502 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=36.76666666666667 +lat_2=37.96666666666667 +lat_0=36.33333333333334 +lon_0=-78.5 +x_0=3500000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWashingtonNorthFIPS4601 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=47.5 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-120.8333333333333 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWashingtonSouthFIPS4602 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.83333333333334 +lat_2=47.33333333333334 +lat_0=45.33333333333334 +lon_0=-120.5 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWestVirginiaNorthFIPS4701 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=39 +lat_2=40.25 +lat_0=38.5 +lon_0=-79.5 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWestVirginiaSouthFIPS4702 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.48333333333333 +lat_2=38.88333333333333 +lat_0=37 +lon_0=-81 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWisconsinCentralFIPS4802 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.25 +lat_2=45.5 +lat_0=43.83333333333334 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWisconsinNorthFIPS4801 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.56666666666667 +lat_2=46.76666666666667 +lat_0=45.16666666666666 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWisconsinSouthFIPS4803 =
                ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.73333333333333 +lat_2=44.06666666666667 +lat_0=42 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWyomingEastCentralFIPS4902 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40.5 +lon_0=-107.3333333333333 +k=0.999938 +x_0=400000 +y_0=100000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWyomingEastFIPS4901 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40.5 +lon_0=-105.1666666666667 +k=0.999938 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWyomingWestCentralFIPS4903 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40.5 +lon_0=-108.75 +k=0.999938 +x_0=600000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983StatePlaneWyomingWestFIPS4904 =
                ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40.5 +lon_0=-110.0833333333333 +k=0.999938 +x_0=800000 +y_0=100000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");

            NAD1983Maine2000CentralZone.Name = "NAD_1983_Maine_2000_Central_Zone";
            NAD1983Maine2000EastZone.Name = "NAD_1983_Maine_2000_East_Zone";
            NAD1983Maine2000WestZone.Name = "NAD_1983_Maine_2000_West_Zone";
            NAD1983StatePlaneAlabamaEastFIPS0101.Name = "NAD_1983_StatePlane_Alabama_East_FIPS_0101";
            NAD1983StatePlaneAlabamaWestFIPS0102.Name = "NAD_1983_StatePlane_Alabama_West_FIPS_0102";
            NAD1983StatePlaneAlaska10FIPS5010.Name = "NAD_1983_StatePlane_Alaska_10_FIPS_5010";
            NAD1983StatePlaneAlaska1FIPS5001.Name = "NAD_1983_StatePlane_Alaska_1_FIPS_5001";
            NAD1983StatePlaneAlaska2FIPS5002.Name = "NAD_1983_StatePlane_Alaska_2_FIPS_5002";
            NAD1983StatePlaneAlaska3FIPS5003.Name = "NAD_1983_StatePlane_Alaska_3_FIPS_5003";
            NAD1983StatePlaneAlaska4FIPS5004.Name = "NAD_1983_StatePlane_Alaska_4_FIPS_5004";
            NAD1983StatePlaneAlaska5FIPS5005.Name = "NAD_1983_StatePlane_Alaska_5_FIPS_5005";
            NAD1983StatePlaneAlaska6FIPS5006.Name = "NAD_1983_StatePlane_Alaska_6_FIPS_5006";
            NAD1983StatePlaneAlaska7FIPS5007.Name = "NAD_1983_StatePlane_Alaska_7_FIPS_5007";
            NAD1983StatePlaneAlaska8FIPS5008.Name = "NAD_1983_StatePlane_Alaska_8_FIPS_5008";
            NAD1983StatePlaneAlaska9FIPS5009.Name = "NAD_1983_StatePlane_Alaska_9_FIPS_5009";
            NAD1983StatePlaneArizonaCentralFIPS0202.Name = "NAD_1983_StatePlane_Arizona_Central_FIPS_0202";
            NAD1983StatePlaneArizonaEastFIPS0201.Name = "NAD_1983_StatePlane_Arizona_East_FIPS_0201";
            NAD1983StatePlaneArizonaWestFIPS0203.Name = "NAD_1983_StatePlane_Arizona_West_FIPS_0203";
            NAD1983StatePlaneArkansasNorthFIPS0301.Name = "NAD_1983_StatePlane_Arkansas_North_FIPS_0301";
            NAD1983StatePlaneArkansasSouthFIPS0302.Name = "NAD_1983_StatePlane_Arkansas_South_FIPS_0302";
            NAD1983StatePlaneCaliforniaIFIPS0401.Name = "NAD_1983_StatePlane_California_I_FIPS_0401";
            NAD1983StatePlaneCaliforniaIIFIPS0402.Name = "NAD_1983_StatePlane_California_II_FIPS_0402";
            NAD1983StatePlaneCaliforniaIIIFIPS0403.Name = "NAD_1983_StatePlane_California_III_FIPS_0403";
            NAD1983StatePlaneCaliforniaIVFIPS0404.Name = "NAD_1983_StatePlane_California_IV_FIPS_0404";
            NAD1983StatePlaneCaliforniaVFIPS0405.Name = "NAD_1983_StatePlane_California_V_FIPS_0405";
            NAD1983StatePlaneCaliforniaVIFIPS0406.Name = "NAD_1983_StatePlane_California_VI_FIPS_0406";
            NAD1983StatePlaneColoradoCentralFIPS0502.Name = "NAD_1983_StatePlane_Colorado_Central_FIPS_0502";
            NAD1983StatePlaneColoradoNorthFIPS0501.Name = "NAD_1983_StatePlane_Colorado_North_FIPS_0501";
            NAD1983StatePlaneColoradoSouthFIPS0503.Name = "NAD_1983_StatePlane_Colorado_South_FIPS_0503";
            NAD1983StatePlaneConnecticutFIPS0600.Name = "NAD_1983_StatePlane_Connecticut_FIPS_0600";
            NAD1983StatePlaneDelawareFIPS0700.Name = "NAD_1983_StatePlane_Delaware_FIPS_0700";
            NAD1983StatePlaneFloridaEastFIPS0901.Name = "NAD_1983_StatePlane_Florida_East_FIPS_0901";
            NAD1983StatePlaneFloridaNorthFIPS0903.Name = "NAD_1983_StatePlane_Florida_North_FIPS_0903";
            NAD1983StatePlaneFloridaWestFIPS0902.Name = "NAD_1983_StatePlane_Florida_West_FIPS_0902";
            NAD1983StatePlaneGeorgiaEastFIPS1001.Name = "NAD_1983_StatePlane_Georgia_East_FIPS_1001";
            NAD1983StatePlaneGeorgiaWestFIPS1002.Name = "NAD_1983_StatePlane_Georgia_West_FIPS_1002";
            NAD1983StatePlaneGuamFIPS5400.Name = "NAD_1983_StatePlane_Guam_FIPS_5400";
            NAD1983StatePlaneHawaii1FIPS5101.Name = "NAD_1983_StatePlane_Hawaii_1_FIPS_5101";
            NAD1983StatePlaneHawaii2FIPS5102.Name = "NAD_1983_StatePlane_Hawaii_2_FIPS_5102";
            NAD1983StatePlaneHawaii3FIPS5103.Name = "NAD_1983_StatePlane_Hawaii_3_FIPS_5103";
            NAD1983StatePlaneHawaii4FIPS5104.Name = "NAD_1983_StatePlane_Hawaii_4_FIPS_5104";
            NAD1983StatePlaneHawaii5FIPS5105.Name = "NAD_1983_StatePlane_Hawaii_5_FIPS_5105";
            NAD1983StatePlaneIdahoCentralFIPS1102.Name = "NAD_1983_StatePlane_Idaho_Central_FIPS_1102";
            NAD1983StatePlaneIdahoEastFIPS1101.Name = "NAD_1983_StatePlane_Idaho_East_FIPS_1101";
            NAD1983StatePlaneIdahoWestFIPS1103.Name = "NAD_1983_StatePlane_Idaho_West_FIPS_1103";
            NAD1983StatePlaneIllinoisEastFIPS1201.Name = "NAD_1983_StatePlane_Illinois_East_FIPS_1201";
            NAD1983StatePlaneIllinoisWestFIPS1202.Name = "NAD_1983_StatePlane_Illinois_West_FIPS_1202";
            NAD1983StatePlaneIndianaEastFIPS1301.Name = "NAD_1983_StatePlane_Indiana_East_FIPS_1301";
            NAD1983StatePlaneIndianaWestFIPS1302.Name = "NAD_1983_StatePlane_Indiana_West_FIPS_1302";
            NAD1983StatePlaneIowaNorthFIPS1401.Name = "NAD_1983_StatePlane_Iowa_North_FIPS_1401";
            NAD1983StatePlaneIowaSouthFIPS1402.Name = "NAD_1983_StatePlane_Iowa_South_FIPS_1402";
            NAD1983StatePlaneKansasNorthFIPS1501.Name = "NAD_1983_StatePlane_Kansas_North_FIPS_1501";
            NAD1983StatePlaneKansasSouthFIPS1502.Name = "NAD_1983_StatePlane_Kansas_South_FIPS_1502";
            NAD1983StatePlaneKentuckyFIPS1600.Name = "NAD_1983_StatePlane_Kentucky_FIPS_1600";
            NAD1983StatePlaneKentuckyNorthFIPS1601.Name = "NAD_1983_StatePlane_Kentucky_North_FIPS_1601";
            NAD1983StatePlaneKentuckySouthFIPS1602.Name = "NAD_1983_StatePlane_Kentucky_South_FIPS_1602";
            NAD1983StatePlaneLouisianaNorthFIPS1701.Name = "NAD_1983_StatePlane_Louisiana_North_FIPS_1701";
            NAD1983StatePlaneLouisianaSouthFIPS1702.Name = "NAD_1983_StatePlane_Louisiana_South_FIPS_1702";
            NAD1983StatePlaneMaineEastFIPS1801.Name = "NAD_1983_StatePlane_Maine_East_FIPS_1801";
            NAD1983StatePlaneMaineWestFIPS1802.Name = "NAD_1983_StatePlane_Maine_West_FIPS_1802";
            NAD1983StatePlaneMarylandFIPS1900.Name = "NAD_1983_StatePlane_Maryland_FIPS_1900";
            NAD1983StatePlaneMassachusettsIslandFIPS2002.Name = "NAD_1983_StatePlane_Massachusetts_Island_FIPS_2002";
            NAD1983StatePlaneMassachusettsMainlandFIPS2001.Name = "NAD_1983_StatePlane_Massachusetts_Mainland_FIPS_2001";
            NAD1983StatePlaneMichiganCentralFIPS2112.Name = "NAD_1983_StatePlane_Michigan_Central_FIPS_2112";
            NAD1983StatePlaneMichiganNorthFIPS2111.Name = "NAD_1983_StatePlane_Michigan_North_FIPS_2111";
            NAD1983StatePlaneMichiganSouthFIPS2113.Name = "NAD_1983_StatePlane_Michigan_South_FIPS_2113";
            NAD1983StatePlaneMinnesotaCentralFIPS2202.Name = "NAD_1983_StatePlane_Minnesota_Central_FIPS_2202";
            NAD1983StatePlaneMinnesotaNorthFIPS2201.Name = "NAD_1983_StatePlane_Minnesota_North_FIPS_2201";
            NAD1983StatePlaneMinnesotaSouthFIPS2203.Name = "NAD_1983_StatePlane_Minnesota_South_FIPS_2203";
            NAD1983StatePlaneMississippiEastFIPS2301.Name = "NAD_1983_StatePlane_Mississippi_East_FIPS_2301";
            NAD1983StatePlaneMississippiWestFIPS2302.Name = "NAD_1983_StatePlane_Mississippi_West_FIPS_2302";
            NAD1983StatePlaneMissouriCentralFIPS2402.Name = "NAD_1983_StatePlane_Missouri_Central_FIPS_2402";
            NAD1983StatePlaneMissouriEastFIPS2401.Name = "NAD_1983_StatePlane_Missouri_East_FIPS_2401";
            NAD1983StatePlaneMissouriWestFIPS2403.Name = "NAD_1983_StatePlane_Missouri_West_FIPS_2403";
            NAD1983StatePlaneMontanaFIPS2500.Name = "NAD_1983_StatePlane_Montana_FIPS_2500";
            NAD1983StatePlaneNebraskaFIPS2600.Name = "NAD_1983_StatePlane_Nebraska_FIPS_2600";
            NAD1983StatePlaneNevadaCentralFIPS2702.Name = "NAD_1983_StatePlane_Nevada_Central_FIPS_2702";
            NAD1983StatePlaneNevadaEastFIPS2701.Name = "NAD_1983_StatePlane_Nevada_East_FIPS_2701";
            NAD1983StatePlaneNevadaWestFIPS2703.Name = "NAD_1983_StatePlane_Nevada_West_FIPS_2703";
            NAD1983StatePlaneNewHampshireFIPS2800.Name = "NAD_1983_StatePlane_New_Hampshire_FIPS_2800";
            NAD1983StatePlaneNewJerseyFIPS2900.Name = "NAD_1983_StatePlane_New_Jersey_FIPS_2900";
            NAD1983StatePlaneNewMexicoCentralFIPS3002.Name = "NAD_1983_StatePlane_New_Mexico_Central_FIPS_3002";
            NAD1983StatePlaneNewMexicoEastFIPS3001.Name = "NAD_1983_StatePlane_New_Mexico_East_FIPS_3001";
            NAD1983StatePlaneNewMexicoWestFIPS3003.Name = "NAD_1983_StatePlane_New_Mexico_West_FIPS_3003";
            NAD1983StatePlaneNewYorkCentralFIPS3102.Name = "NAD_1983_StatePlane_New_York_Central_FIPS_3102";
            NAD1983StatePlaneNewYorkEastFIPS3101.Name = "NAD_1983_StatePlane_New_York_East_FIPS_3101";
            NAD1983StatePlaneNewYorkLongIslandFIPS3104.Name = "NAD_1983_StatePlane_New_York_Long_Island_FIPS_3104";
            NAD1983StatePlaneNewYorkWestFIPS3103.Name = "NAD_1983_StatePlane_New_York_West_FIPS_3103";
            NAD1983StatePlaneNorthCarolinaFIPS3200.Name = "NAD_1983_StatePlane_North_Carolina_FIPS_3200";
            NAD1983StatePlaneNorthDakotaNorthFIPS3301.Name = "NAD_1983_StatePlane_North_Dakota_North_FIPS_3301";
            NAD1983StatePlaneNorthDakotaSouthFIPS3302.Name = "NAD_1983_StatePlane_North_Dakota_South_FIPS_3302";
            NAD1983StatePlaneOhioNorthFIPS3401.Name = "NAD_1983_StatePlane_Ohio_North_FIPS_3401";
            NAD1983StatePlaneOhioSouthFIPS3402.Name = "NAD_1983_StatePlane_Ohio_South_FIPS_3402";
            NAD1983StatePlaneOklahomaNorthFIPS3501.Name = "NAD_1983_StatePlane_Oklahoma_North_FIPS_3501";
            NAD1983StatePlaneOklahomaSouthFIPS3502.Name = "NAD_1983_StatePlane_Oklahoma_South_FIPS_3502";
            NAD1983StatePlaneOregonNorthFIPS3601.Name = "NAD_1983_StatePlane_Oregon_North_FIPS_3601";
            NAD1983StatePlaneOregonSouthFIPS3602.Name = "NAD_1983_StatePlane_Oregon_South_FIPS_3602";
            NAD1983StatePlanePennsylvaniaNorthFIPS3701.Name = "NAD_1983_StatePlane_Pennsylvania_North_FIPS_3701";
            NAD1983StatePlanePennsylvaniaSouthFIPS3702.Name = "NAD_1983_StatePlane_Pennsylvania_South_FIPS_3702";
            NAD1983StatePlanePuertoRicoVirginIslandsFIPS5200.Name = "NAD_1983_StatePlane_Puerto_Rico_Virgin_Islands_FIPS_5200";
            NAD1983StatePlaneRhodeIslandFIPS3800.Name = "NAD_1983_StatePlane_Rhode_Island_FIPS_3800";
            NAD1983StatePlaneSouthCarolinaFIPS3900.Name = "NAD_1983_StatePlane_South_Carolina_FIPS_3900";
            NAD1983StatePlaneSouthDakotaNorthFIPS4001.Name = "NAD_1983_StatePlane_South_Dakota_North_FIPS_4001";
            NAD1983StatePlaneSouthDakotaSouthFIPS4002.Name = "NAD_1983_StatePlane_South_Dakota_South_FIPS_4002";
            NAD1983StatePlaneTennesseeFIPS4100.Name = "NAD_1983_StatePlane_Tennessee_FIPS_4100";
            NAD1983StatePlaneTexasCentralFIPS4203.Name = "NAD_1983_StatePlane_Texas_Central_FIPS_4203";
            NAD1983StatePlaneTexasNorthCentralFIPS4202.Name = "NAD_1983_StatePlane_Texas_North_Central_FIPS_4202";
            NAD1983StatePlaneTexasNorthFIPS4201.Name = "NAD_1983_StatePlane_Texas_North_FIPS_4201";
            NAD1983StatePlaneTexasSouthCentralFIPS4204.Name = "NAD_1983_StatePlane_Texas_South_Central_FIPS_4204";
            NAD1983StatePlaneTexasSouthFIPS4205.Name = "NAD_1983_StatePlane_Texas_South_FIPS_4205";
            NAD1983StatePlaneUtahCentralFIPS4302.Name = "NAD_1983_StatePlane_Utah_Central_FIPS_4302";
            NAD1983StatePlaneUtahNorthFIPS4301.Name = "NAD_1983_StatePlane_Utah_North_FIPS_4301";
            NAD1983StatePlaneUtahSouthFIPS4303.Name = "NAD_1983_StatePlane_Utah_South_FIPS_4303";
            NAD1983StatePlaneVermontFIPS4400.Name = "NAD_1983_StatePlane_Vermont_FIPS_4400";
            NAD1983StatePlaneVirginiaNorthFIPS4501.Name = "NAD_1983_StatePlane_Virginia_North_FIPS_4501";
            NAD1983StatePlaneVirginiaSouthFIPS4502.Name = "NAD_1983_StatePlane_Virginia_South_FIPS_4502";
            NAD1983StatePlaneWashingtonNorthFIPS4601.Name = "NAD_1983_StatePlane_Washington_North_FIPS_4601";
            NAD1983StatePlaneWashingtonSouthFIPS4602.Name = "NAD_1983_StatePlane_Washington_South_FIPS_4602";
            NAD1983StatePlaneWestVirginiaNorthFIPS4701.Name = "NAD_1983_StatePlane_West_Virginia_North_FIPS_4701";
            NAD1983StatePlaneWestVirginiaSouthFIPS4702.Name = "NAD_1983_StatePlane_West_Virginia_South_FIPS_4702";
            NAD1983StatePlaneWisconsinCentralFIPS4802.Name = "NAD_1983_StatePlane_Wisconsin_Central_FIPS_4802";
            NAD1983StatePlaneWisconsinNorthFIPS4801.Name = "NAD_1983_StatePlane_Wisconsin_North_FIPS_4801";
            NAD1983StatePlaneWisconsinSouthFIPS4803.Name = "NAD_1983_StatePlane_Wisconsin_South_FIPS_4803";
            NAD1983StatePlaneWyomingEastCentralFIPS4902.Name = "NAD_1983_StatePlane_Wyoming_East_Central_FIPS_4902";
            NAD1983StatePlaneWyomingEastFIPS4901.Name = "NAD_1983_StatePlane_Wyoming_East_FIPS_4901";
            NAD1983StatePlaneWyomingWestCentralFIPS4903.Name = "NAD_1983_StatePlane_Wyoming_West_Central_FIPS_4903";
            NAD1983StatePlaneWyomingWestFIPS4904.Name = "NAD_1983_StatePlane_Wyoming_West_FIPS_4904";

            NAD1983Maine2000CentralZone.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983Maine2000EastZone.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983Maine2000WestZone.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlabamaEastFIPS0101.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlabamaWestFIPS0102.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska10FIPS5010.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska1FIPS5001.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska2FIPS5002.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska3FIPS5003.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska4FIPS5004.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska5FIPS5005.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska6FIPS5006.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska7FIPS5007.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska8FIPS5008.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneAlaska9FIPS5009.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneArizonaCentralFIPS0202.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneArizonaEastFIPS0201.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneArizonaWestFIPS0203.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneArkansasNorthFIPS0301.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneArkansasSouthFIPS0302.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneCaliforniaIFIPS0401.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneCaliforniaIIFIPS0402.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneCaliforniaIIIFIPS0403.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneCaliforniaIVFIPS0404.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneCaliforniaVFIPS0405.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneCaliforniaVIFIPS0406.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneColoradoCentralFIPS0502.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneColoradoNorthFIPS0501.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneColoradoSouthFIPS0503.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneConnecticutFIPS0600.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneDelawareFIPS0700.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneFloridaEastFIPS0901.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneFloridaNorthFIPS0903.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneFloridaWestFIPS0902.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneGeorgiaEastFIPS1001.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneGeorgiaWestFIPS1002.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneGuamFIPS5400.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneHawaii1FIPS5101.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneHawaii2FIPS5102.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneHawaii3FIPS5103.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneHawaii4FIPS5104.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneHawaii5FIPS5105.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIdahoCentralFIPS1102.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIdahoEastFIPS1101.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIdahoWestFIPS1103.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIllinoisEastFIPS1201.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIllinoisWestFIPS1202.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIndianaEastFIPS1301.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIndianaWestFIPS1302.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIowaNorthFIPS1401.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneIowaSouthFIPS1402.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneKansasNorthFIPS1501.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneKansasSouthFIPS1502.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneKentuckyFIPS1600.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneKentuckyNorthFIPS1601.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneKentuckySouthFIPS1602.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneLouisianaNorthFIPS1701.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneLouisianaSouthFIPS1702.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMaineEastFIPS1801.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMaineWestFIPS1802.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMarylandFIPS1900.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMassachusettsIslandFIPS2002.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMassachusettsMainlandFIPS2001.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMichiganCentralFIPS2112.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMichiganNorthFIPS2111.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMichiganSouthFIPS2113.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMinnesotaCentralFIPS2202.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMinnesotaNorthFIPS2201.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMinnesotaSouthFIPS2203.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMississippiEastFIPS2301.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMississippiWestFIPS2302.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMissouriCentralFIPS2402.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMissouriEastFIPS2401.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMissouriWestFIPS2403.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneMontanaFIPS2500.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNebraskaFIPS2600.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNevadaCentralFIPS2702.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNevadaEastFIPS2701.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNevadaWestFIPS2703.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewHampshireFIPS2800.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewJerseyFIPS2900.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewMexicoCentralFIPS3002.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewMexicoEastFIPS3001.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewMexicoWestFIPS3003.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewYorkCentralFIPS3102.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewYorkEastFIPS3101.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewYorkLongIslandFIPS3104.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNewYorkWestFIPS3103.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNorthCarolinaFIPS3200.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNorthDakotaNorthFIPS3301.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneNorthDakotaSouthFIPS3302.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneOhioNorthFIPS3401.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneOhioSouthFIPS3402.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneOklahomaNorthFIPS3501.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneOklahomaSouthFIPS3502.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneOregonNorthFIPS3601.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneOregonSouthFIPS3602.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlanePennsylvaniaNorthFIPS3701.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlanePennsylvaniaSouthFIPS3702.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlanePuertoRicoVirginIslandsFIPS5200.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneRhodeIslandFIPS3800.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneSouthCarolinaFIPS3900.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneSouthDakotaNorthFIPS4001.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneSouthDakotaSouthFIPS4002.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneTennesseeFIPS4100.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneTexasCentralFIPS4203.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneTexasNorthCentralFIPS4202.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneTexasNorthFIPS4201.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneTexasSouthCentralFIPS4204.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneTexasSouthFIPS4205.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneUtahCentralFIPS4302.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneUtahNorthFIPS4301.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneUtahSouthFIPS4303.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneVermontFIPS4400.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneVirginiaNorthFIPS4501.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneVirginiaSouthFIPS4502.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWashingtonNorthFIPS4601.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWashingtonSouthFIPS4602.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWestVirginiaNorthFIPS4701.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWestVirginiaSouthFIPS4702.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWisconsinCentralFIPS4802.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWisconsinNorthFIPS4801.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWisconsinSouthFIPS4803.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWyomingEastCentralFIPS4902.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWyomingEastFIPS4901.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWyomingWestCentralFIPS4903.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983StatePlaneWyomingWestFIPS4904.GeographicInfo.Name = "GCS_North_American_1983";

            NAD1983Maine2000CentralZone.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983Maine2000EastZone.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983Maine2000WestZone.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlabamaEastFIPS0101.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlabamaWestFIPS0102.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska10FIPS5010.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska1FIPS5001.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska2FIPS5002.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska3FIPS5003.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska4FIPS5004.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska5FIPS5005.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska6FIPS5006.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska7FIPS5007.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska8FIPS5008.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneAlaska9FIPS5009.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneArizonaCentralFIPS0202.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneArizonaEastFIPS0201.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneArizonaWestFIPS0203.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneArkansasNorthFIPS0301.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneArkansasSouthFIPS0302.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneCaliforniaIFIPS0401.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneCaliforniaIIFIPS0402.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneCaliforniaIIIFIPS0403.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneCaliforniaIVFIPS0404.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneCaliforniaVFIPS0405.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneCaliforniaVIFIPS0406.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneColoradoCentralFIPS0502.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneColoradoNorthFIPS0501.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneColoradoSouthFIPS0503.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneConnecticutFIPS0600.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneDelawareFIPS0700.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneFloridaEastFIPS0901.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneFloridaNorthFIPS0903.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneFloridaWestFIPS0902.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneGeorgiaEastFIPS1001.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneGeorgiaWestFIPS1002.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneGuamFIPS5400.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneHawaii1FIPS5101.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneHawaii2FIPS5102.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneHawaii3FIPS5103.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneHawaii4FIPS5104.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneHawaii5FIPS5105.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIdahoCentralFIPS1102.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIdahoEastFIPS1101.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIdahoWestFIPS1103.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIllinoisEastFIPS1201.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIllinoisWestFIPS1202.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIndianaEastFIPS1301.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIndianaWestFIPS1302.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIowaNorthFIPS1401.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneIowaSouthFIPS1402.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneKansasNorthFIPS1501.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneKansasSouthFIPS1502.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneKentuckyFIPS1600.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneKentuckyNorthFIPS1601.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneKentuckySouthFIPS1602.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneLouisianaNorthFIPS1701.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneLouisianaSouthFIPS1702.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMaineEastFIPS1801.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMaineWestFIPS1802.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMarylandFIPS1900.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMassachusettsIslandFIPS2002.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMassachusettsMainlandFIPS2001.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMichiganCentralFIPS2112.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMichiganNorthFIPS2111.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMichiganSouthFIPS2113.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMinnesotaCentralFIPS2202.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMinnesotaNorthFIPS2201.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMinnesotaSouthFIPS2203.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMississippiEastFIPS2301.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMississippiWestFIPS2302.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMissouriCentralFIPS2402.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMissouriEastFIPS2401.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMissouriWestFIPS2403.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneMontanaFIPS2500.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNebraskaFIPS2600.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNevadaCentralFIPS2702.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNevadaEastFIPS2701.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNevadaWestFIPS2703.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewHampshireFIPS2800.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewJerseyFIPS2900.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewMexicoCentralFIPS3002.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewMexicoEastFIPS3001.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewMexicoWestFIPS3003.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewYorkCentralFIPS3102.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewYorkEastFIPS3101.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewYorkLongIslandFIPS3104.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNewYorkWestFIPS3103.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNorthCarolinaFIPS3200.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNorthDakotaNorthFIPS3301.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneNorthDakotaSouthFIPS3302.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneOhioNorthFIPS3401.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneOhioSouthFIPS3402.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneOklahomaNorthFIPS3501.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneOklahomaSouthFIPS3502.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneOregonNorthFIPS3601.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneOregonSouthFIPS3602.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlanePennsylvaniaNorthFIPS3701.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlanePennsylvaniaSouthFIPS3702.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlanePuertoRicoVirginIslandsFIPS5200.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneRhodeIslandFIPS3800.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneSouthCarolinaFIPS3900.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneSouthDakotaNorthFIPS4001.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneSouthDakotaSouthFIPS4002.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneTennesseeFIPS4100.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneTexasCentralFIPS4203.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneTexasNorthCentralFIPS4202.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneTexasNorthFIPS4201.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneTexasSouthCentralFIPS4204.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneTexasSouthFIPS4205.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneUtahCentralFIPS4302.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneUtahNorthFIPS4301.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneUtahSouthFIPS4303.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneVermontFIPS4400.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneVirginiaNorthFIPS4501.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneVirginiaSouthFIPS4502.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWashingtonNorthFIPS4601.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWashingtonSouthFIPS4602.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWestVirginiaNorthFIPS4701.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWestVirginiaSouthFIPS4702.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWisconsinCentralFIPS4802.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWisconsinNorthFIPS4801.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWisconsinSouthFIPS4803.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWyomingEastCentralFIPS4902.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWyomingEastFIPS4901.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWyomingWestCentralFIPS4903.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983StatePlaneWyomingWestFIPS4904.GeographicInfo.Datum.Name = "D_North_American_1983";
        }

        #endregion
    }
}

#pragma warning restore 1591