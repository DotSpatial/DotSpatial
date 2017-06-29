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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:47:45 PM
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
    /// NationalGridsCanada
    /// </summary>
    public class NationalGridsCanada : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo ATS1977MTM4NovaScotia;
        public readonly ProjectionInfo ATS1977MTM5NovaScotia;
        public readonly ProjectionInfo ATS1977NewBrunswickStereographic;
        public readonly ProjectionInfo NAD192710TMAEPForest;
        public readonly ProjectionInfo NAD192710TMAEPResource;
        public readonly ProjectionInfo NAD19273TM111;
        public readonly ProjectionInfo NAD19273TM114;
        public readonly ProjectionInfo NAD19273TM117;
        public readonly ProjectionInfo NAD19273TM120;
        public readonly ProjectionInfo NAD1927CGQ77MTM10SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM2SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM3SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM4SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM5SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM6SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM7SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM8SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM9SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77QuebecLambert;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone17N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone18N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone19N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone20N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone21N;
        public readonly ProjectionInfo NAD1927DEF1976MTM10;
        public readonly ProjectionInfo NAD1927DEF1976MTM11;
        public readonly ProjectionInfo NAD1927DEF1976MTM12;
        public readonly ProjectionInfo NAD1927DEF1976MTM13;
        public readonly ProjectionInfo NAD1927DEF1976MTM14;
        public readonly ProjectionInfo NAD1927DEF1976MTM15;
        public readonly ProjectionInfo NAD1927DEF1976MTM16;
        public readonly ProjectionInfo NAD1927DEF1976MTM17;
        public readonly ProjectionInfo NAD1927DEF1976MTM8;
        public readonly ProjectionInfo NAD1927DEF1976MTM9;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone15N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone16N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone17N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone18N;
        public readonly ProjectionInfo NAD1927MTM1;
        public readonly ProjectionInfo NAD1927MTM2;
        public readonly ProjectionInfo NAD1927MTM3;
        public readonly ProjectionInfo NAD1927MTM4;
        public readonly ProjectionInfo NAD1927MTM5;
        public readonly ProjectionInfo NAD1927MTM6;
        public readonly ProjectionInfo NAD1927QuebecLambert;
        public readonly ProjectionInfo NAD198310TMAEPForest;
        public readonly ProjectionInfo NAD198310TMAEPResource;
        public readonly ProjectionInfo NAD19833TM111;
        public readonly ProjectionInfo NAD19833TM114;
        public readonly ProjectionInfo NAD19833TM117;
        public readonly ProjectionInfo NAD19833TM120;
        public readonly ProjectionInfo NAD1983BCEnvironmentAlbers;
        public readonly ProjectionInfo NAD1983CSRS98MTM10;
        public readonly ProjectionInfo NAD1983CSRS98MTM2SCoPQ;
        public readonly ProjectionInfo NAD1983CSRS98MTM3;
        public readonly ProjectionInfo NAD1983CSRS98MTM4;
        public readonly ProjectionInfo NAD1983CSRS98MTM5;
        public readonly ProjectionInfo NAD1983CSRS98MTM6;
        public readonly ProjectionInfo NAD1983CSRS98MTM7;
        public readonly ProjectionInfo NAD1983CSRS98MTM8;
        public readonly ProjectionInfo NAD1983CSRS98MTM9;
        public readonly ProjectionInfo NAD1983CSRS98NewBrunswickStereographic;
        public readonly ProjectionInfo NAD1983CSRS98PrinceEdwardIsland;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone11N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone12N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone13N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone17N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone18N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone19N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone20N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone21N;
        public readonly ProjectionInfo NAD1983MTM1;
        public readonly ProjectionInfo NAD1983MTM10;
        public readonly ProjectionInfo NAD1983MTM11;
        public readonly ProjectionInfo NAD1983MTM12;
        public readonly ProjectionInfo NAD1983MTM13;
        public readonly ProjectionInfo NAD1983MTM14;
        public readonly ProjectionInfo NAD1983MTM15;
        public readonly ProjectionInfo NAD1983MTM16;
        public readonly ProjectionInfo NAD1983MTM17;
        public readonly ProjectionInfo NAD1983MTM2;
        public readonly ProjectionInfo NAD1983MTM2SCoPQ;
        public readonly ProjectionInfo NAD1983MTM3;
        public readonly ProjectionInfo NAD1983MTM4;
        public readonly ProjectionInfo NAD1983MTM5;
        public readonly ProjectionInfo NAD1983MTM6;
        public readonly ProjectionInfo NAD1983MTM7;
        public readonly ProjectionInfo NAD1983MTM8;
        public readonly ProjectionInfo NAD1983MTM9;
        public readonly ProjectionInfo NAD1983QuebecLambert;
        public readonly ProjectionInfo PrinceEdwardIslandStereographic;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsCanada
        /// </summary>
        public NationalGridsCanada()
        {
            ATS1977MTM4NovaScotia = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=4500000 +y_0=0 +a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            ATS1977MTM5NovaScotia = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=5500000 +y_0=0 +a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            ATS1977NewBrunswickStereographic = ProjectionInfo.FromProj4String("+proj=sterea +lat_0=46.5 +lon_0=-66.5 +k=0.999912 +x_0=300000 +y_0=800000 +a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            NAD192710TMAEPForest = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=500000 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD192710TMAEPResource = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM111 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-111 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM114 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-114 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM117 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-117 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM120 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-120 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927CGQ77MTM10SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM2SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-55.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM3SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM4SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM5SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM6SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM7SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-70.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM8SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM9SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77QuebecLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=46 +lat_2=60 +lat_0=44 +lon_0=-68.5 +x_0=0 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone17N = ProjectionInfo.FromProj4String("+proj=utm +zone=17 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone18N = ProjectionInfo.FromProj4String("+proj=utm +zone=18 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone19N = ProjectionInfo.FromProj4String("+proj=utm +zone=19 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone20N = ProjectionInfo.FromProj4String("+proj=utm +zone=20 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone21N = ProjectionInfo.FromProj4String("+proj=utm +zone=21 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM10 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM11 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-82.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM12 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-81 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM13 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-84 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM14 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-87 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM15 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-90 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM16 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-93 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM17 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-96 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM9 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone15N = ProjectionInfo.FromProj4String("+proj=utm +zone=15 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone16N = ProjectionInfo.FromProj4String("+proj=utm +zone=16 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone17N = ProjectionInfo.FromProj4String("+proj=utm +zone=17 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone18N = ProjectionInfo.FromProj4String("+proj=utm +zone=18 +ellps=clrk66 +units=m +no_defs ");
            NAD1927MTM1 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-53 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM2 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-56 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM6 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927QuebecLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=60 +lat_2=46 +lat_0=44 +lon_0=-68.5 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD198310TMAEPForest = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD198310TMAEPResource = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM111 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-111 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM114 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-114 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM117 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-117 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM120 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-120 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983BCEnvironmentAlbers = ProjectionInfo.FromProj4String("+proj=aea +lat_1=50 +lat_2=58.5 +lat_0=45 +lon_0=-126 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983CSRS98MTM10 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM2SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-55.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM6 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM7 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-70.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM9 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98NewBrunswickStereographic = ProjectionInfo.FromProj4String("+proj=sterea +lat_0=46.5 +lon_0=-66.5 +k=0.999912 +x_0=2500000 +y_0=7500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98PrinceEdwardIsland = ProjectionInfo.FromProj4String("+proj=sterea +lat_0=47.25 +lon_0=-63 +k=0.999912 +x_0=400000 +y_0=800000 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs ");
            NAD1983CSRS98UTMZone11N = ProjectionInfo.FromProj4String("+proj=utm +zone=11 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone12N = ProjectionInfo.FromProj4String("+proj=utm +zone=12 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone13N = ProjectionInfo.FromProj4String("+proj=utm +zone=13 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone17N = ProjectionInfo.FromProj4String("+proj=utm +zone=17 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone18N = ProjectionInfo.FromProj4String("+proj=utm +zone=18 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone19N = ProjectionInfo.FromProj4String("+proj=utm +zone=19 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone20N = ProjectionInfo.FromProj4String("+proj=utm +zone=20 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone21N = ProjectionInfo.FromProj4String("+proj=utm +zone=21 +ellps=GRS80 +units=m +no_defs ");
            NAD1983MTM1 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-53 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM10 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM11 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-82.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM12 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-81 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM13 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-84 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM14 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-87 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM15 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-90 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM16 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-93 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM17 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-96 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM2 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-56 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM2SCoPQ = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-55.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM6 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM7 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-70.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM9 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983QuebecLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=46 +lat_2=60 +lat_0=44 +lon_0=-68.5 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            PrinceEdwardIslandStereographic = ProjectionInfo.FromProj4String("+proj=sterea +lat_0=47.25 +lon_0=-63 +k=0.999912 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");

            ATS1977MTM4NovaScotia.Name = "ATS_1977_MTM_4_Nova_Scotia";
            ATS1977MTM5NovaScotia.Name = "ATS_1977_MTM_5_Nova_Scotia";
            ATS1977NewBrunswickStereographic.Name = "ATS_1977_New_Brunswick_Stereographic";
            NAD192710TMAEPForest.Name = "NAD_1927_10TM_AEP_Forest";
            NAD192710TMAEPResource.Name = "NAD_1927_10TM_AEP_Resource";
            NAD19273TM111.Name = "NAD_1927_3TM_111";
            NAD19273TM114.Name = "NAD_1927_3TM_114";
            NAD19273TM117.Name = "NAD_1927_3TM_117";
            NAD19273TM120.Name = "NAD_1927_3TM_120";
            NAD1927CGQ77MTM10SCoPQ.Name = "NAD_1927_CGQ77_MTM_10_SCoPQ";
            NAD1927CGQ77MTM2SCoPQ.Name = "NAD_1927_CGQ77_MTM_2_SCoPQ";
            NAD1927CGQ77MTM3SCoPQ.Name = "NAD_1927_CGQ77_MTM_3_SCoPQ";
            NAD1927CGQ77MTM4SCoPQ.Name = "NAD_1927_CGQ77_MTM_4_SCoPQ";
            NAD1927CGQ77MTM5SCoPQ.Name = "NAD_1927_CGQ77_MTM_5_SCoPQ";
            NAD1927CGQ77MTM6SCoPQ.Name = "NAD_1927_CGQ77_MTM_6_SCoPQ";
            NAD1927CGQ77MTM7SCoPQ.Name = "NAD_1927_CGQ77_MTM_7_SCoPQ";
            NAD1927CGQ77MTM8SCoPQ.Name = "NAD_1927_CGQ77_MTM_8_SCoPQ";
            NAD1927CGQ77MTM9SCoPQ.Name = "NAD_1927_CGQ77_MTM_9_SCoPQ";
            NAD1927CGQ77QuebecLambert.Name = "NAD_1927_CGQ77_Quebec_Lambert";
            NAD1927CGQ77UTMZone17N.Name = "NAD_1927_CGQ77_UTM_Zone_17N";
            NAD1927CGQ77UTMZone18N.Name = "NAD_1927_CGQ77_UTM_Zone_18N";
            NAD1927CGQ77UTMZone19N.Name = "NAD_1927_CGQ77_UTM_Zone_19N";
            NAD1927CGQ77UTMZone20N.Name = "NAD_1927_CGQ77_UTM_Zone_20N";
            NAD1927CGQ77UTMZone21N.Name = "NAD_1927_CGQ77_UTM_Zone_21N";
            NAD1927DEF1976MTM10.Name = "NAD_1927_DEF_1976_MTM_10";
            NAD1927DEF1976MTM11.Name = "NAD_1927_DEF_1976_MTM_11";
            NAD1927DEF1976MTM12.Name = "NAD_1927_DEF_1976_MTM_12";
            NAD1927DEF1976MTM13.Name = "NAD_1927_DEF_1976_MTM_13";
            NAD1927DEF1976MTM14.Name = "NAD_1927_DEF_1976_MTM_14";
            NAD1927DEF1976MTM15.Name = "NAD_1927_DEF_1976_MTM_15";
            NAD1927DEF1976MTM16.Name = "NAD_1927_DEF_1976_MTM_16";
            NAD1927DEF1976MTM17.Name = "NAD_1927_DEF_1976_MTM_17";
            NAD1927DEF1976MTM8.Name = "NAD_1927_DEF_1976_MTM_8";
            NAD1927DEF1976MTM9.Name = "NAD_1927_DEF_1976_MTM_9";
            NAD1927DEF1976UTMZone15N.Name = "NAD_1927_DEF_1976_UTM_Zone_15N";
            NAD1927DEF1976UTMZone16N.Name = "NAD_1927_DEF_1976_UTM_Zone_16N";
            NAD1927DEF1976UTMZone17N.Name = "NAD_1927_DEF_1976_UTM_Zone_17N";
            NAD1927DEF1976UTMZone18N.Name = "NAD_1927_DEF_1976_UTM_Zone_18N";
            NAD1927MTM1.Name = "NAD_1927_MTM_1";
            NAD1927MTM2.Name = "NAD_1927_MTM_2";
            NAD1927MTM3.Name = "NAD_1927_MTM_3";
            NAD1927MTM4.Name = "NAD_1927_MTM_4";
            NAD1927MTM5.Name = "NAD_1927_MTM_5";
            NAD1927MTM6.Name = "NAD_1927_MTM_6";
            NAD1927QuebecLambert.Name = "NAD_1927_Quebec_Lambert";
            NAD198310TMAEPForest.Name = "NAD_1983_10TM_AEP_Forest";
            NAD198310TMAEPResource.Name = "NAD_1983_10TM_AEP_Resource";
            NAD19833TM111.Name = "NAD_1983_3TM_111";
            NAD19833TM114.Name = "NAD_1983_3TM_114";
            NAD19833TM117.Name = "NAD_1983_3TM_117";
            NAD19833TM120.Name = "NAD_1983_3TM_120";
            NAD1983BCEnvironmentAlbers.Name = "NAD_1983_BC_Environment_Albers";
            NAD1983CSRS98MTM10.Name = "NAD_1983_CRS98_MTM_10";
            NAD1983CSRS98MTM2SCoPQ.Name = "NAD_1983_CSRS98_MTM_2_SCoPQ";
            NAD1983CSRS98MTM3.Name = "NAD_1983_CRS98_MTM_3";
            NAD1983CSRS98MTM4.Name = "NAD_1983_CRS98_MTM_4";
            NAD1983CSRS98MTM5.Name = "NAD_1983_CRS98_MTM_5";
            NAD1983CSRS98MTM6.Name = "NAD_1983_CRS98_MTM_6";
            NAD1983CSRS98MTM7.Name = "NAD_1983_CRS98_MTM_7";
            NAD1983CSRS98MTM8.Name = "NAD_1983_CRS98_MTM_8";
            NAD1983CSRS98MTM9.Name = "NAD_1983_CRS98_MTM_9";
            NAD1983CSRS98NewBrunswickStereographic.Name = "NAD_1983_CSRS98_New_Brunswick_Stereographic";
            NAD1983CSRS98PrinceEdwardIsland.Name = "NAD_1983_CSRS98_Prince_Edward_Island";
            NAD1983CSRS98UTMZone11N.Name = "NAD_1983_CSRS98_UTM_Zone_11N";
            NAD1983CSRS98UTMZone12N.Name = "NAD_1983_CSRS98_UTM_Zone_12N";
            NAD1983CSRS98UTMZone13N.Name = "NAD_1983_CSRS98_UTM_Zone_13N";
            NAD1983CSRS98UTMZone17N.Name = "NAD_1983_CSRS98_UTM_Zone_17N";
            NAD1983CSRS98UTMZone18N.Name = "NAD_1983_CSRS98_UTM_Zone_18N";
            NAD1983CSRS98UTMZone19N.Name = "NAD_1983_CSRS98_UTM_Zone_19N";
            NAD1983CSRS98UTMZone20N.Name = "NAD_1983_CSRS98_UTM_Zone_20N";
            NAD1983CSRS98UTMZone21N.Name = "NAD_1983_CSRS98_UTM_Zone_21N";
            NAD1983MTM1.Name = "NAD_1983_MTM_1";
            NAD1983MTM10.Name = "NAD_1983_MTM_10";
            NAD1983MTM11.Name = "NAD_1983_MTM_11";
            NAD1983MTM12.Name = "NAD_1983_MTM_12";
            NAD1983MTM13.Name = "NAD_1983_MTM_13";
            NAD1983MTM14.Name = "NAD_1983_MTM_14";
            NAD1983MTM15.Name = "NAD_1983_MTM_15";
            NAD1983MTM16.Name = "NAD_1983_MTM_16";
            NAD1983MTM17.Name = "NAD_1983_MTM_17";
            NAD1983MTM2.Name = "NAD_1983_MTM_2";
            NAD1983MTM2SCoPQ.Name = "NAD_1983_MTM_2_SCoPQ";
            NAD1983MTM3.Name = "NAD_1983_MTM_3";
            NAD1983MTM4.Name = "NAD_1983_MTM_4";
            NAD1983MTM5.Name = "NAD_1983_MTM_5";
            NAD1983MTM6.Name = "NAD_1983_MTM_6";
            NAD1983MTM7.Name = "NAD_1983_MTM_7";
            NAD1983MTM8.Name = "NAD_1983_MTM_8";
            NAD1983MTM9.Name = "NAD_1983_MTM_9";
            NAD1983QuebecLambert.Name = "NAD_1983_Quebec_Lambert";
            PrinceEdwardIslandStereographic.Name = "Prince_Edward_Island_Stereographic";

            ATS1977MTM4NovaScotia.GeographicInfo.Name = "GCS_ATS_1977";
            ATS1977MTM5NovaScotia.GeographicInfo.Name = "GCS_ATS_1977";
            ATS1977NewBrunswickStereographic.GeographicInfo.Name = "GCS_ATS_1977";
            NAD192710TMAEPForest.GeographicInfo.Name = "GCS_North_American_1927";
            NAD192710TMAEPResource.GeographicInfo.Name = "GCS_North_American_1927";
            NAD19273TM111.GeographicInfo.Name = "GCS_North_American_1927";
            NAD19273TM114.GeographicInfo.Name = "GCS_North_American_1927";
            NAD19273TM117.GeographicInfo.Name = "GCS_North_American_1927";
            NAD19273TM120.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927CGQ77MTM10SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM2SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM3SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM4SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM5SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM6SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM7SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM8SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77MTM9SCoPQ.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77QuebecLambert.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone17N.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone18N.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone19N.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone20N.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone21N.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927DEF1976MTM10.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM11.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM12.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM13.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM14.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM15.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM16.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM17.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM8.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM9.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone15N.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone16N.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone17N.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone18N.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927MTM1.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927MTM2.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927MTM3.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927MTM4.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927MTM5.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927MTM6.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927QuebecLambert.GeographicInfo.Name = "GCS_North_American_1927";
            NAD198310TMAEPForest.GeographicInfo.Name = "GCS_North_American_1983";
            NAD198310TMAEPResource.GeographicInfo.Name = "GCS_North_American_1983";
            NAD19833TM111.GeographicInfo.Name = "GCS_North_American_1983";
            NAD19833TM114.GeographicInfo.Name = "GCS_North_American_1983";
            NAD19833TM117.GeographicInfo.Name = "GCS_North_American_1983";
            NAD19833TM120.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983BCEnvironmentAlbers.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983CSRS98MTM10.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM2SCoPQ.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM3.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM4.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM5.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM6.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM7.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM8.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98MTM9.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98NewBrunswickStereographic.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98PrinceEdwardIsland.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone11N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone12N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone13N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone17N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone18N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone19N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone20N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone21N.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NAD1983MTM1.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM10.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM11.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM12.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM13.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM14.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM15.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM16.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM17.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM2.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM2SCoPQ.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM3.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM4.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM5.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM6.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM7.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM8.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983MTM9.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983QuebecLambert.GeographicInfo.Name = "GCS_North_American_1983";
            PrinceEdwardIslandStereographic.GeographicInfo.Name = "GCS_ATS_1977";

            ATS1977MTM4NovaScotia.GeographicInfo.Datum.Name = "D_ATS_1977";
            ATS1977MTM5NovaScotia.GeographicInfo.Datum.Name = "D_ATS_1977";
            ATS1977NewBrunswickStereographic.GeographicInfo.Datum.Name = "D_ATS_1977";
            NAD192710TMAEPForest.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD192710TMAEPResource.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD19273TM111.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD19273TM114.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD19273TM117.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD19273TM120.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927CGQ77MTM10SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM2SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM3SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM4SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM5SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM6SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM7SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM8SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77MTM9SCoPQ.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77QuebecLambert.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone17N.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone18N.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone19N.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone20N.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927CGQ77UTMZone21N.GeographicInfo.Datum.Name = "D_NAD_1927_CGQ77";
            NAD1927DEF1976MTM10.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM11.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM12.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM13.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM14.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM15.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM16.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM17.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM8.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976MTM9.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone15N.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone16N.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone17N.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927DEF1976UTMZone18N.GeographicInfo.Datum.Name = "D_NAD_1927_Definition_1976";
            NAD1927MTM1.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927MTM2.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927MTM3.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927MTM4.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927MTM5.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927MTM6.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927QuebecLambert.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD198310TMAEPForest.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD198310TMAEPResource.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD19833TM111.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD19833TM114.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD19833TM117.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD19833TM120.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983BCEnvironmentAlbers.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983CSRS98MTM10.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM2SCoPQ.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM3.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM4.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM5.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM6.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM7.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM8.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98MTM9.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98NewBrunswickStereographic.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98PrinceEdwardIsland.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone11N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone12N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone13N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone17N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone18N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone19N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone20N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983CSRS98UTMZone21N.GeographicInfo.Datum.Name = "D_North_American_1983_CSRS98";
            NAD1983MTM1.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM10.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM11.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM12.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM13.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM14.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM15.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM16.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM17.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM2.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM2SCoPQ.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM3.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM4.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM5.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM6.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM7.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM8.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983MTM9.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983QuebecLambert.GeographicInfo.Datum.Name = "D_North_American_1983";
            PrinceEdwardIslandStereographic.GeographicInfo.Datum.Name = "D_ATS_1977";
        }

        #endregion
    }
}

#pragma warning restore 1591