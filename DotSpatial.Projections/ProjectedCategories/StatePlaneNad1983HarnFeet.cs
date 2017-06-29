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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:03:19 PM
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
    /// StatePlaneNad1983HarnFeet
    /// </summary>
    public class StatePlaneNad1983HarnFeet : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneConnecticutFIPS0600Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneDelawareFIPS0700Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaEastFIPS0901Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaWestFIPS0902Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii1FIPS5101Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii2FIPS5102Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii3FIPS5103Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii4FIPS5104Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii5FIPS5105Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoEastFIPS1101Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoWestFIPS1103Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaEastFIPS1301Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaWestFIPS1302Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMarylandFIPS1900Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiEastFIPS2301Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiWestFIPS2302Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTennesseeFIPS4100Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasCentralFIPS4203Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthFIPS4201Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthFIPS4205Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneNad1983HarnFeet
        /// </summary>
        public StatePlaneNad1983HarnFeet()
        {
            NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-111.9166666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-110.1666666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-113.75 +k=0.999933 +x_0=213360 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40 +lat_2=41.66666666666666 +lat_0=39.33333333333334 +lon_0=-122 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.33333333333334 +lat_2=39.83333333333334 +lat_0=37.66666666666666 +lon_0=-122 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.06666666666667 +lat_2=38.43333333333333 +lat_0=36.5 +lon_0=-120.5 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=36 +lat_2=37.25 +lat_0=35.33333333333334 +lon_0=-119 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=34.03333333333333 +lat_2=35.46666666666667 +lat_0=33.5 +lon_0=-118 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.78333333333333 +lat_2=33.88333333333333 +lat_0=32.16666666666666 +lon_0=-116.25 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.45 +lat_2=39.75 +lat_0=37.83333333333334 +lon_0=-105.5 +x_0=914401.8288999999 +y_0=304800.6096 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=39.71666666666667 +lat_2=40.78333333333333 +lat_0=39.33333333333334 +lon_0=-105.5 +x_0=914401.8288999999 +y_0=304800.6096 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.23333333333333 +lat_2=38.43333333333333 +lat_0=36.66666666666666 +lon_0=-105.5 +x_0=914401.8288999999 +y_0=304800.6096 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneConnecticutFIPS0600Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=41.2 +lat_2=41.86666666666667 +lat_0=40.83333333333334 +lon_0=-72.75 +x_0=304800.6096 +y_0=152400.3048 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneDelawareFIPS0700Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=38 +lon_0=-75.41666666666667 +k=0.999995 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneFloridaEastFIPS0901Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-81 +k=0.999941 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=29.58333333333333 +lat_2=30.75 +lat_0=29 +lon_0=-84.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneFloridaWestFIPS0902Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-82 +k=0.999941 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=30 +lon_0=-82.16666666666667 +k=0.999900 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=30 +lon_0=-84.16666666666667 +k=0.999900 +x_0=699999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii1FIPS5101Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=18.83333333333333 +lon_0=-155.5 +k=0.999967 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii2FIPS5102Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=20.33333333333333 +lon_0=-156.6666666666667 +k=0.999967 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii3FIPS5103Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.16666666666667 +lon_0=-158 +k=0.999990 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii4FIPS5104Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.83333333333333 +lon_0=-159.5 +k=0.999990 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii5FIPS5105Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.66666666666667 +lon_0=-160.1666666666667 +k=1.000000 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-114 +k=0.999947 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIdahoEastFIPS1101Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-112.1666666666667 +k=0.999947 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIdahoWestFIPS1103Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-115.75 +k=0.999933 +x_0=799999.9999999998 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIndianaEastFIPS1301Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=37.5 +lon_0=-85.66666666666667 +k=0.999967 +x_0=99999.99999999999 +y_0=250000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIndianaWestFIPS1302Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=37.5 +lon_0=-87.08333333333333 +k=0.999967 +x_0=900000 +y_0=250000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.96666666666667 +lat_2=38.96666666666667 +lat_0=37.5 +lon_0=-84.25 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=36.73333333333333 +lat_2=37.93333333333333 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=500000.0000000001 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMarylandFIPS1900Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.3 +lat_2=39.45 +lat_0=37.66666666666666 +lon_0=-77 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=41.28333333333333 +lat_2=41.48333333333333 +lat_0=41 +lon_0=-70.5 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=41.71666666666667 +lat_2=42.68333333333333 +lat_0=41 +lon_0=-71.5 +x_0=199999.9999999999 +y_0=750000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.36666666666666 +x_0=6000000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=7999999.999999998 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.36666666666666 +x_0=3999999.999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneMississippiEastFIPS2301Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=29.5 +lon_0=-88.83333333333333 +k=0.999950 +x_0=300000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMississippiWestFIPS2302Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=29.5 +lon_0=-90.33333333333333 +k=0.999950 +x_0=699999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45 +lat_2=49 +lat_0=44.25 +lon_0=-109.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-106.25 +k=0.999900 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-104.3333333333333 +k=0.999909 +x_0=165000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=31 +lon_0=-107.8333333333333 +k=0.999917 +x_0=829999.9999999998 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40 +lon_0=-76.58333333333333 +k=0.999938 +x_0=250000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40.66666666666666 +lat_2=41.03333333333333 +lat_0=40.16666666666666 +lon_0=-74 +x_0=300000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40 +lon_0=-78.58333333333333 +k=0.999938 +x_0=350000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=47.43333333333333 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=46.18333333333333 +lat_2=47.48333333333333 +lat_0=45.66666666666666 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=35.56666666666667 +lat_2=36.76666666666667 +lat_0=35 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=33.93333333333333 +lat_2=35.23333333333333 +lat_0=33.33333333333334 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.33333333333334 +lat_2=46 +lat_0=43.66666666666666 +lon_0=-120.5 +x_0=2500000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.33333333333334 +lat_2=44 +lat_0=41.66666666666666 +lon_0=-120.5 +x_0=1500000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneTennesseeFIPS4100Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=35.25 +lat_2=36.41666666666666 +lat_0=34.33333333333334 +lon_0=-86 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasCentralFIPS4203Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=30.11666666666667 +lat_2=31.88333333333333 +lat_0=29.66666666666667 +lon_0=-100.3333333333333 +x_0=699999.9999999999 +y_0=3000000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.13333333333333 +lat_2=33.96666666666667 +lat_0=31.66666666666667 +lon_0=-98.5 +x_0=600000 +y_0=2000000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasNorthFIPS4201Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=34.65 +lat_2=36.18333333333333 +lat_0=34 +lon_0=-101.5 +x_0=199999.9999999999 +y_0=999999.9999999999 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=28.38333333333333 +lat_2=30.28333333333333 +lat_0=27.83333333333333 +lon_0=-99 +x_0=600000 +y_0=3999999.999999999 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasSouthFIPS4205Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26.16666666666667 +lat_2=27.83333333333333 +lat_0=25.66666666666667 +lon_0=-98.5 +x_0=300000 +y_0=4999999.999999998 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=39.01666666666667 +lat_2=40.65 +lat_0=38.33333333333334 +lon_0=-111.5 +x_0=499999.9999999998 +y_0=2000000 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=40.71666666666667 +lat_2=41.78333333333333 +lat_0=40.33333333333334 +lon_0=-111.5 +x_0=499999.9999999998 +y_0=999999.9999999999 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=37.21666666666667 +lat_2=38.35 +lat_0=36.66666666666666 +lon_0=-111.5 +x_0=499999.9999999998 +y_0=3000000 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=38.03333333333333 +lat_2=39.2 +lat_0=37.66666666666666 +lon_0=-78.5 +x_0=3499999.999999998 +y_0=2000000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=36.76666666666667 +lat_2=37.96666666666667 +lat_0=36.33333333333334 +lon_0=-78.5 +x_0=3499999.999999998 +y_0=999999.9999999999 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=47.5 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-120.8333333333333 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.83333333333334 +lat_2=47.33333333333334 +lat_0=45.33333333333334 +lon_0=-120.5 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.25 +lat_2=45.5 +lat_0=43.83333333333334 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.56666666666667 +lat_2=46.76666666666667 +lat_0=45.16666666666666 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.73333333333333 +lat_2=44.06666666666667 +lat_0=42 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");

            NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl.Name = "NAD_1983_HARN_StatePlane_Arizona_Central_FIPS_0202_Feet_Intl";
            NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl.Name = "NAD_1983_HARN_StatePlane_Arizona_East_FIPS_0201_Feet_Intl";
            NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl.Name = "NAD_1983_HARN_StatePlane_Arizona_West_FIPS_0203_Feet_Intl";
            NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet.Name = "NAD_1983_HARN_StatePlane_California_I_FIPS_0401_Feet";
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet.Name = "NAD_1983_HARN_StatePlane_California_II_FIPS_0402_Feet";
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet.Name = "NAD_1983_HARN_StatePlane_California_III_FIPS_0403_Feet";
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet.Name = "NAD_1983_HARN_StatePlane_California_IV_FIPS_0404_Feet";
            NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet.Name = "NAD_1983_HARN_StatePlane_California_V_FIPS_0405_Feet";
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet.Name = "NAD_1983_HARN_StatePlane_California_VI_FIPS_0406_Feet";
            NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet.Name = "NAD_1983_HARN_StatePlane_Colorado_Central_FIPS_0502_Feet";
            NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet.Name = "NAD_1983_HARN_StatePlane_Colorado_North_FIPS_0501_Feet";
            NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet.Name = "NAD_1983_HARN_StatePlane_Colorado_South_FIPS_0503_Feet";
            NAD1983HARNStatePlaneConnecticutFIPS0600Feet.Name = "NAD_1983_HARN_StatePlane_Connecticut_FIPS_0600_Feet";
            NAD1983HARNStatePlaneDelawareFIPS0700Feet.Name = "NAD_1983_HARN_StatePlane_Delaware_FIPS_0700_Feet";
            NAD1983HARNStatePlaneFloridaEastFIPS0901Feet.Name = "NAD_1983_HARN_StatePlane_Florida_East_FIPS_0901_Feet";
            NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet.Name = "NAD_1983_HARN_StatePlane_Florida_North_FIPS_0903_Feet";
            NAD1983HARNStatePlaneFloridaWestFIPS0902Feet.Name = "NAD_1983_HARN_StatePlane_Florida_West_FIPS_0902_Feet";
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet.Name = "NAD_1983_HARN_StatePlane_Georgia_East_FIPS_1001_Feet";
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet.Name = "NAD_1983_HARN_StatePlane_Georgia_West_FIPS_1002_Feet";
            NAD1983HARNStatePlaneHawaii1FIPS5101Feet.Name = "NAD_1983_HARN_StatePlane_Hawaii_1_FIPS_5101_Feet";
            NAD1983HARNStatePlaneHawaii2FIPS5102Feet.Name = "NAD_1983_HARN_StatePlane_Hawaii_2_FIPS_5102_Feet";
            NAD1983HARNStatePlaneHawaii3FIPS5103Feet.Name = "NAD_1983_HARN_StatePlane_Hawaii_3_FIPS_5103_Feet";
            NAD1983HARNStatePlaneHawaii4FIPS5104Feet.Name = "NAD_1983_HARN_StatePlane_Hawaii_4_FIPS_5104_Feet";
            NAD1983HARNStatePlaneHawaii5FIPS5105Feet.Name = "NAD_1983_HARN_StatePlane_Hawaii_5_FIPS_5105_Feet";
            NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet.Name = "NAD_1983_HARN_StatePlane_Idaho_Central_FIPS_1102_Feet";
            NAD1983HARNStatePlaneIdahoEastFIPS1101Feet.Name = "NAD_1983_HARN_StatePlane_Idaho_East_FIPS_1101_Feet";
            NAD1983HARNStatePlaneIdahoWestFIPS1103Feet.Name = "NAD_1983_HARN_StatePlane_Idaho_West_FIPS_1103_Feet";
            NAD1983HARNStatePlaneIndianaEastFIPS1301Feet.Name = "NAD_1983_HARN_StatePlane_Indiana_East_FIPS_1301_Feet";
            NAD1983HARNStatePlaneIndianaWestFIPS1302Feet.Name = "NAD_1983_HARN_StatePlane_Indiana_West_FIPS_1302_Feet";
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet.Name = "NAD_1983_HARN_StatePlane_Kentucky_North_FIPS_1601_Feet";
            NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet.Name = "NAD_1983_HARN_StatePlane_Kentucky_South_FIPS_1602_Feet";
            NAD1983HARNStatePlaneMarylandFIPS1900Feet.Name = "NAD_1983_HARN_StatePlane_Maryland_FIPS_1900_Feet";
            NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet.Name = "NAD_1983_HARN_StatePlane_Massachusetts_Island_FIPS_2002_Feet";
            NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet.Name = "NAD_1983_HARN_StatePlane_Massachusetts_Mainland_FIPS_2001_Feet";
            NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl.Name = "NAD_1983_HARN_StatePlane_Michigan_Central_FIPS_2112_Feet_Intl";
            NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl.Name = "NAD_1983_HARN_StatePlane_Michigan_North_FIPS_2111_Feet_Intl";
            NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl.Name = "NAD_1983_HARN_StatePlane_Michigan_South_FIPS_2113_Feet_Intl";
            NAD1983HARNStatePlaneMississippiEastFIPS2301Feet.Name = "NAD_1983_HARN_StatePlane_Mississippi_East_FIPS_2301_Feet";
            NAD1983HARNStatePlaneMississippiWestFIPS2302Feet.Name = "NAD_1983_HARN_StatePlane_Mississippi_West_FIPS_2302_Feet";
            NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl.Name = "NAD_1983_HARN_StatePlane_Montana_FIPS_2500_Feet_Intl";
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet.Name = "NAD_1983_HARN_StatePlane_New_Mexico_Central_FIPS_3002_Feet";
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet.Name = "NAD_1983_HARN_StatePlane_New_Mexico_East_FIPS_3001_Feet";
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet.Name = "NAD_1983_HARN_StatePlane_New_Mexico_West_FIPS_3003_Feet";
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet.Name = "NAD_1983_HARN_StatePlane_New_York_Central_FIPS_3102_Feet";
            NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet.Name = "NAD_1983_HARN_StatePlane_New_York_East_FIPS_3101_Feet";
            NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet.Name = "NAD_1983_HARN_StatePlane_New_York_Long_Island_FIPS_3104_Feet";
            NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet.Name = "NAD_1983_HARN_StatePlane_New_York_West_FIPS_3103_Feet";
            NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl.Name = "NAD_1983_HARN_StatePlane_North_Dakota_North_FIPS_3301_Feet_Intl";
            NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl.Name = "NAD_1983_HARN_StatePlane_North_Dakota_South_FIPS_3302_Feet_Intl";
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet.Name = "NAD_1983_HARN_StatePlane_Oklahoma_North_FIPS_3501_Feet";
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet.Name = "NAD_1983_HARN_StatePlane_Oklahoma_South_FIPS_3502_Feet";
            NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl.Name = "NAD_1983_HARN_StatePlane_Oregon_North_FIPS_3601_Feet_Intl";
            NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl.Name = "NAD_1983_HARN_StatePlane_Oregon_South_FIPS_3602_Feet_Intl";
            NAD1983HARNStatePlaneTennesseeFIPS4100Feet.Name = "NAD_1983_HARN_StatePlane_Tennessee_FIPS_4100_Feet";
            NAD1983HARNStatePlaneTexasCentralFIPS4203Feet.Name = "NAD_1983_HARN_StatePlane_Texas_Central_FIPS_4203_Feet";
            NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet.Name = "NAD_1983_HARN_StatePlane_Texas_North_Central_FIPS_4202_Feet";
            NAD1983HARNStatePlaneTexasNorthFIPS4201Feet.Name = "NAD_1983_HARN_StatePlane_Texas_North_FIPS_4201_Feet";
            NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet.Name = "NAD_1983_HARN_StatePlane_Texas_South_Central_FIPS_4204_Feet";
            NAD1983HARNStatePlaneTexasSouthFIPS4205Feet.Name = "NAD_1983_HARN_StatePlane_Texas_South_FIPS_4205_Feet";
            NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl.Name = "NAD_1983_HARN_StatePlane_Utah_Central_FIPS_4302_Feet_Intl";
            NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl.Name = "NAD_1983_HARN_StatePlane_Utah_North_FIPS_4301_Feet_Intl";
            NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl.Name = "NAD_1983_HARN_StatePlane_Utah_South_FIPS_4303_Feet_Intl";
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet.Name = "NAD_1983_HARN_StatePlane_Virginia_North_FIPS_4501_Feet";
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet.Name = "NAD_1983_HARN_StatePlane_Virginia_South_FIPS_4502_Feet";
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet.Name = "NAD_1983_HARN_StatePlane_Washington_North_FIPS_4601_Feet";
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet.Name = "NAD_1983_HARN_StatePlane_Washington_South_FIPS_4602_Feet";
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet.Name = "NAD_1983_HARN_StatePlane_Wisconsin_Central_FIPS_4802_Feet";
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet.Name = "NAD_1983_HARN_StatePlane_Wisconsin_North_FIPS_4801_Feet";
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet.Name = "NAD_1983_HARN_StatePlane_Wisconsin_South_FIPS_4803_Feet";

            NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneConnecticutFIPS0600Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneDelawareFIPS0700Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneFloridaEastFIPS0901Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneFloridaWestFIPS0902Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii1FIPS5101Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii2FIPS5102Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii3FIPS5103Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii4FIPS5104Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii5FIPS5105Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneIdahoEastFIPS1101Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneIdahoWestFIPS1103Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneIndianaEastFIPS1301Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneIndianaWestFIPS1302Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMarylandFIPS1900Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMississippiEastFIPS2301Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMississippiWestFIPS2302Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneTennesseeFIPS4100Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasCentralFIPS4203Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasNorthFIPS4201Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasSouthFIPS4205Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet.GeographicInfo.Name = "GCS_North_American_1983_HARN";

            NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneConnecticutFIPS0600Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneDelawareFIPS0700Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneFloridaEastFIPS0901Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneFloridaWestFIPS0902Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii1FIPS5101Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii2FIPS5102Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii3FIPS5103Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii4FIPS5104Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneHawaii5FIPS5105Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneIdahoEastFIPS1101Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneIdahoWestFIPS1103Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneIndianaEastFIPS1301Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneIndianaWestFIPS1302Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMarylandFIPS1900Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMississippiEastFIPS2301Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMississippiWestFIPS2302Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneTennesseeFIPS4100Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasCentralFIPS4203Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasNorthFIPS4201Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneTexasSouthFIPS4205Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
        }

        #endregion
    }
}

#pragma warning restore 1591