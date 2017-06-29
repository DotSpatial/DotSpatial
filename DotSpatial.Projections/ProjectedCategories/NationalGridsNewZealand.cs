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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:52:35 PM
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
    /// NationalGridsNewZealand
    /// </summary>
    public class NationalGridsNewZealand : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo ChathamIslands1979MapGrid;
        public readonly ProjectionInfo NZGD1949AmuriCircuit;
        public readonly ProjectionInfo NZGD1949BayofPlentyCircuit;
        public readonly ProjectionInfo NZGD1949BluffCircuit;
        public readonly ProjectionInfo NZGD1949BullerCircuit;
        public readonly ProjectionInfo NZGD1949CollingwoodCircuit;
        public readonly ProjectionInfo NZGD1949GawlerCircuit;
        public readonly ProjectionInfo NZGD1949GreyCircuit;
        public readonly ProjectionInfo NZGD1949HawkesBayCircuit;
        public readonly ProjectionInfo NZGD1949HokitikaCircuit;
        public readonly ProjectionInfo NZGD1949JacksonsBayCircuit;
        public readonly ProjectionInfo NZGD1949KarameaCircuit;
        public readonly ProjectionInfo NZGD1949LindisPeakCircuit;
        public readonly ProjectionInfo NZGD1949MarlboroughCircuit;
        public readonly ProjectionInfo NZGD1949MountEdenCircuit;
        public readonly ProjectionInfo NZGD1949MountNicholasCircuit;
        public readonly ProjectionInfo NZGD1949MountPleasantCircuit;
        public readonly ProjectionInfo NZGD1949MountYorkCircuit;
        public readonly ProjectionInfo NZGD1949NelsonCircuit;
        public readonly ProjectionInfo NZGD1949NorthTaieriCircuit;
        public readonly ProjectionInfo NZGD1949ObservationPointCircuit;
        public readonly ProjectionInfo NZGD1949OkaritoCircuit;
        public readonly ProjectionInfo NZGD1949PovertyBayCircuit;
        public readonly ProjectionInfo NZGD1949TaranakiCircuit;
        public readonly ProjectionInfo NZGD1949TimaruCircuit;
        public readonly ProjectionInfo NZGD1949TuhirangiCircuit;
        public readonly ProjectionInfo NZGD1949UTMZone58S;
        public readonly ProjectionInfo NZGD1949UTMZone59S;
        public readonly ProjectionInfo NZGD1949UTMZone60S;
        public readonly ProjectionInfo NZGD1949WairarapaCircuit;
        public readonly ProjectionInfo NZGD1949WanganuiCircuit;
        public readonly ProjectionInfo NZGD1949WellingtonCircuit;
        public readonly ProjectionInfo NZGD2000AmuriCircuit;
        public readonly ProjectionInfo NZGD2000BayofPlentyCircuit;
        public readonly ProjectionInfo NZGD2000BluffCircuit;
        public readonly ProjectionInfo NZGD2000BullerCircuit;
        public readonly ProjectionInfo NZGD2000ChathamIslandCircuit;
        public readonly ProjectionInfo NZGD2000CollingwoodCircuit;
        public readonly ProjectionInfo NZGD2000GawlerCircuit;
        public readonly ProjectionInfo NZGD2000GreyCircuit;
        public readonly ProjectionInfo NZGD2000HawkesBayCircuit;
        public readonly ProjectionInfo NZGD2000HokitikaCircuit;
        public readonly ProjectionInfo NZGD2000JacksonsBayCircuit;
        public readonly ProjectionInfo NZGD2000KarameaCircuit;
        public readonly ProjectionInfo NZGD2000LindisPeakCircuit;
        public readonly ProjectionInfo NZGD2000MarlboroughCircuit;
        public readonly ProjectionInfo NZGD2000MountEdenCircuit;
        public readonly ProjectionInfo NZGD2000MountNicholasCircuit;
        public readonly ProjectionInfo NZGD2000MountPleasantCircuit;
        public readonly ProjectionInfo NZGD2000MountYorkCircuit;
        public readonly ProjectionInfo NZGD2000NelsonCircuit;
        public readonly ProjectionInfo NZGD2000NewZealandTransverseMercator;
        public readonly ProjectionInfo NZGD2000NorthTaieriCircuit;
        public readonly ProjectionInfo NZGD2000ObservationPointCircuit;
        public readonly ProjectionInfo NZGD2000OkaritoCircuit;
        public readonly ProjectionInfo NZGD2000PovertyBayCircuit;
        public readonly ProjectionInfo NZGD2000TaranakiCircuit;
        public readonly ProjectionInfo NZGD2000TimaruCircuit;
        public readonly ProjectionInfo NZGD2000TuhirangiCircuit;
        public readonly ProjectionInfo NZGD2000UTMZone58S;
        public readonly ProjectionInfo NZGD2000UTMZone59S;
        public readonly ProjectionInfo NZGD2000UTMZone60S;
        public readonly ProjectionInfo NZGD2000WairarapaCircuit;
        public readonly ProjectionInfo NZGD2000WanganuiCircuit;
        public readonly ProjectionInfo NZGD2000WellingtonCircuit;
        public readonly ProjectionInfo NewZealandMapGrid;
        public readonly ProjectionInfo NewZealandNorthIsland;
        public readonly ProjectionInfo NewZealandSouthIsland;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsNewZealand
        /// </summary>
        public NationalGridsNewZealand()
        {
            ChathamIslands1979MapGrid = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-44 +lon_0=-176.5 +k=0.999600 +x_0=350000 +y_0=650000 +ellps=intl +units=m +no_defs ");
            NewZealandMapGrid = ProjectionInfo.FromProj4String("+proj=nzmg +lat_0=-41 +lon_0=173 +x_0=2510000 +y_0=6023150 +ellps=intl +datum=nzgd49 +units=m +no_defs ");
            NewZealandNorthIsland = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-39 +lon_0=175.5 +k=1.000000 +x_0=274319.5243848086 +y_0=365759.3658464114 +ellps=intl +to_meter=0.9143984146160287 +no_defs ");
            NewZealandSouthIsland = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-44 +lon_0=171.5 +k=1.000000 +x_0=457199.2073080143 +y_0=457199.2073080143 +ellps=intl +to_meter=0.9143984146160287 +no_defs ");
            NZGD1949AmuriCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-42.68911658333333 +lon_0=173.0101333888889 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949BayofPlentyCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-37.76124980555556 +lon_0=176.46619725 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949BluffCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-46.60000961111111 +lon_0=168.342872 +k=1.000000 +x_0=300002.66 +y_0=699999.58 +ellps=intl +units=m +no_defs ");
            NZGD1949BullerCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.81080286111111 +lon_0=171.5812600555556 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949CollingwoodCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-40.71475905555556 +lon_0=172.6720465 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949GawlerCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.74871155555556 +lon_0=171.3607484722222 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949GreyCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-42.33369427777778 +lon_0=171.5497713055556 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949HawkesBayCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-39.65092930555556 +lon_0=176.6736805277778 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949HokitikaCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-42.88632236111111 +lon_0=170.9799935 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949JacksonsBayCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.97780288888889 +lon_0=168.606267 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949KarameaCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.28991152777778 +lon_0=172.1090281944444 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949LindisPeakCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-44.73526797222222 +lon_0=169.4677550833333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MarlboroughCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.54448666666666 +lon_0=173.8020741111111 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountEdenCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-36.87986527777778 +lon_0=174.7643393611111 +k=0.999900 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountNicholasCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.13290258333333 +lon_0=168.3986411944444 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountPleasantCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.59063758333333 +lon_0=172.7271935833333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountYorkCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.56372616666666 +lon_0=167.7388617777778 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949NelsonCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.27454472222222 +lon_0=173.2993168055555 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949NorthTaieriCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.86151336111111 +lon_0=170.2825891111111 +k=0.999960 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949ObservationPointCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.81619661111111 +lon_0=170.6285951666667 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949OkaritoCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.11012813888889 +lon_0=170.2609258333333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949PovertyBayCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-38.62470277777778 +lon_0=177.8856362777778 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949TaranakiCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-39.13575830555556 +lon_0=174.22801175 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949TimaruCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-44.40222036111111 +lon_0=171.0572508333333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949TuhirangiCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-39.51247038888889 +lon_0=175.6400368055556 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone58S = ProjectionInfo.FromProj4String("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone59S = ProjectionInfo.FromProj4String("+proj=utm +zone=59 +south +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone60S = ProjectionInfo.FromProj4String("+proj=utm +zone=60 +south +ellps=intl +units=m +no_defs ");
            NZGD1949WairarapaCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-40.92553263888889 +lon_0=175.6473496666667 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949WanganuiCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-40.24194713888889 +lon_0=175.4880996111111 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949WellingtonCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.30131963888888 +lon_0=174.7766231111111 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD2000AmuriCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-42.68888888888888 +lon_0=173.01 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000BayofPlentyCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-37.76111111111111 +lon_0=176.4661111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000BluffCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-46.6 +lon_0=168.3427777777778 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000BullerCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.81055555555555 +lon_0=171.5811111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000ChathamIslandCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-44 +lon_0=-176.5 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000CollingwoodCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-40.71472222222223 +lon_0=172.6719444444444 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000GawlerCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.74861111111111 +lon_0=171.3605555555555 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000GreyCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-42.33361111111111 +lon_0=171.5497222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000HawkesBayCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-39.65083333333333 +lon_0=176.6736111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000HokitikaCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-42.88611111111111 +lon_0=170.9797222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000JacksonsBayCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.97777777777778 +lon_0=168.6061111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000KarameaCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.28972222222222 +lon_0=172.1088888888889 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000LindisPeakCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-44.735 +lon_0=169.4675 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MarlboroughCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.54444444444444 +lon_0=173.8019444444444 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountEdenCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-36.87972222222222 +lon_0=174.7641666666667 +k=0.999900 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountNicholasCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.13277777777778 +lon_0=168.3986111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountPleasantCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.59055555555556 +lon_0=172.7269444444445 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountYorkCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.56361111111111 +lon_0=167.7386111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000NelsonCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.27444444444444 +lon_0=173.2991666666667 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000NewZealandTransverseMercator = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=173 +k=0.999600 +x_0=1600000 +y_0=10000000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000NorthTaieriCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.86138888888889 +lon_0=170.2825 +k=0.999960 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000ObservationPointCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-45.81611111111111 +lon_0=170.6283333333333 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000OkaritoCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-43.11 +lon_0=170.2608333333333 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000PovertyBayCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-38.62444444444444 +lon_0=177.8855555555556 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000TaranakiCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-39.13555555555556 +lon_0=174.2277777777778 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000TimaruCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-44.40194444444445 +lon_0=171.0572222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000TuhirangiCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-39.51222222222222 +lon_0=175.64 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone58S = ProjectionInfo.FromProj4String("+proj=utm +zone=58 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone59S = ProjectionInfo.FromProj4String("+proj=utm +zone=59 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone60S = ProjectionInfo.FromProj4String("+proj=utm +zone=60 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000WairarapaCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-40.92527777777777 +lon_0=175.6472222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000WanganuiCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-40.24194444444444 +lon_0=175.4880555555555 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000WellingtonCircuit = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=-41.3011111111111 +lon_0=174.7763888888889 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");

            ChathamIslands1979MapGrid.Name = "Chatham_Islands_1979_Map_Grid";
            NewZealandMapGrid.Name = "GD_1949_New_Zealand_Map_Grid";
            NewZealandNorthIsland.Name = "New_Zealand_North_Island";
            NewZealandSouthIsland.Name = "New_Zealand_South_Island";
            NZGD1949AmuriCircuit.Name = "NZGD_1949_Amuri_Circuit";
            NZGD1949BayofPlentyCircuit.Name = "NZGD_1949_Bay_of_Plenty_Circuit";
            NZGD1949BluffCircuit.Name = "NZGD_1949_Bluff_Circuit";
            NZGD1949BullerCircuit.Name = "NZGD_1949_Buller_Circuit";
            NZGD1949CollingwoodCircuit.Name = "NZGD_1949_Collingwood_Circuit";
            NZGD1949GawlerCircuit.Name = "NZGD_1949_Gawler_Circuit";
            NZGD1949GreyCircuit.Name = "NZGD_1949_Grey_Circuit";
            NZGD1949HawkesBayCircuit.Name = "NZGD_1949_Hawkes_Bay_Circuit";
            NZGD1949HokitikaCircuit.Name = "NZGD_1949_Hokitika_Circuit";
            NZGD1949JacksonsBayCircuit.Name = "NZGD_1949_Jacksons_Bay_Circuit";
            NZGD1949KarameaCircuit.Name = "NZGD_1949_Karamea_Circuit";
            NZGD1949LindisPeakCircuit.Name = "NZGD_1949_Lindis_Peak_Circuit";
            NZGD1949MarlboroughCircuit.Name = "NZGD_1949_Marlborough_Circuit";
            NZGD1949MountEdenCircuit.Name = "NZGD_1949_Mount_Eden_Circuit";
            NZGD1949MountNicholasCircuit.Name = "NZGD_1949_Mount_Nicholas_Circuit";
            NZGD1949MountPleasantCircuit.Name = "NZGD_1949_Mount_Pleasant_Circuit";
            NZGD1949MountYorkCircuit.Name = "NZGD_1949_Mount_York_Circuit";
            NZGD1949NelsonCircuit.Name = "NZGD_1949_Nelson_Circuit";
            NZGD1949NorthTaieriCircuit.Name = "NZGD_1949_North_Taieri_Circuit";
            NZGD1949ObservationPointCircuit.Name = "NZGD_1949_Observation_Point_Circuit";
            NZGD1949OkaritoCircuit.Name = "NZGD_1949_Okarito_Circuit";
            NZGD1949PovertyBayCircuit.Name = "NZGD_1949_Poverty_Bay_Circuit";
            NZGD1949TaranakiCircuit.Name = "NZGD_1949_Taranaki_Circuit";
            NZGD1949TimaruCircuit.Name = "NZGD_1949_Timaru_Circuit";
            NZGD1949TuhirangiCircuit.Name = "NZGD_1949_Tuhirangi_Circuit";
            NZGD1949UTMZone58S.Name = "NZGD_1949_UTM_Zone_58S";
            NZGD1949UTMZone59S.Name = "NZGD_1949_UTM_Zone_59S";
            NZGD1949UTMZone60S.Name = "NZGD_1949_UTM_Zone_60S";
            NZGD1949WairarapaCircuit.Name = "NZGD_1949_Wairarapa_Circuit";
            NZGD1949WanganuiCircuit.Name = "NZGD_1949_Wanganui_Circuit";
            NZGD1949WellingtonCircuit.Name = "NZGD_1949_Wellington_Circuit";
            NZGD2000AmuriCircuit.Name = "NZGD_2000_Amuri_Circuit";
            NZGD2000BayofPlentyCircuit.Name = "NZGD_2000_Bay_of_Plenty_Circuit";
            NZGD2000BluffCircuit.Name = "NZGD_2000_Bluff_Circuit";
            NZGD2000BullerCircuit.Name = "NZGD_2000_Buller_Circuit";
            NZGD2000ChathamIslandCircuit.Name = "NZGD_2000_Chatham_Island_Circuit";
            NZGD2000CollingwoodCircuit.Name = "NZGD_2000_Collingwood_Circuit";
            NZGD2000GawlerCircuit.Name = "NZGD_2000_Gawler_Circuit";
            NZGD2000GreyCircuit.Name = "NZGD_2000_Grey_Circuit";
            NZGD2000HawkesBayCircuit.Name = "NZGD_2000_Hawkes_Bay_Circuit";
            NZGD2000HokitikaCircuit.Name = "NZGD_2000_Hokitika_Circuit";
            NZGD2000JacksonsBayCircuit.Name = "NZGD_2000_Jacksons_Bay_Circuit";
            NZGD2000KarameaCircuit.Name = "NZGD_2000_Karamea_Circuit";
            NZGD2000LindisPeakCircuit.Name = "NZGD_2000_Lindis_Peak_Circuit";
            NZGD2000MarlboroughCircuit.Name = "NZGD_2000_Marlborough_Circuit";
            NZGD2000MountEdenCircuit.Name = "NZGD_2000_Mount_Eden_Circuit";
            NZGD2000MountNicholasCircuit.Name = "NZGD_2000_Mount_Nicholas_Circuit";
            NZGD2000MountPleasantCircuit.Name = "NZGD_2000_Mount_Pleasant_Circuit";
            NZGD2000MountYorkCircuit.Name = "NZGD_2000_Mount_York_Circuit";
            NZGD2000NelsonCircuit.Name = "NZGD_2000_Nelson_Circuit";
            NZGD2000NewZealandTransverseMercator.Name = "NZGD_2000_New_Zealand_Transverse_Mercator";
            NZGD2000NorthTaieriCircuit.Name = "NZGD_2000_North_Taieri_Circuit";
            NZGD2000ObservationPointCircuit.Name = "NZGD_2000_Observation_Point_Circuit";
            NZGD2000OkaritoCircuit.Name = "NZGD_2000_Okarito_Circuit";
            NZGD2000PovertyBayCircuit.Name = "NZGD_2000_Poverty_Bay_Circuit";
            NZGD2000TaranakiCircuit.Name = "NZGD_2000_Taranaki_Circuit";
            NZGD2000TimaruCircuit.Name = "NZGD_2000_Timaru_Circuit";
            NZGD2000TuhirangiCircuit.Name = "NZGD_2000_Tuhirangi_Circuit";
            NZGD2000UTMZone58S.Name = "NZGD_2000_UTM_Zone_58S";
            NZGD2000UTMZone59S.Name = "NZGD_2000_UTM_Zone_59S";
            NZGD2000UTMZone60S.Name = "NZGD_2000_UTM_Zone_60S";
            NZGD2000WairarapaCircuit.Name = "NZGD_2000_Wairarapa_Circuit";
            NZGD2000WanganuiCircuit.Name = "NZGD_2000_Wanganui_Circuit";
            NZGD2000WellingtonCircuit.Name = "NZGD_2000_Wellington_Circuit";
            ChathamIslands1979MapGrid.GeographicInfo.Name = "GCS_Chatham_Islands_1979";
            NewZealandMapGrid.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NewZealandNorthIsland.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NewZealandSouthIsland.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949AmuriCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949BayofPlentyCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949BluffCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949BullerCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949CollingwoodCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949GawlerCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949GreyCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949HawkesBayCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949HokitikaCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949JacksonsBayCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949KarameaCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949LindisPeakCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949MarlboroughCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949MountEdenCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949MountNicholasCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949MountPleasantCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949MountYorkCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949NelsonCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949NorthTaieriCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949ObservationPointCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949OkaritoCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949PovertyBayCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949TaranakiCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949TimaruCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949TuhirangiCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949UTMZone58S.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949UTMZone59S.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949UTMZone60S.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949WairarapaCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949WanganuiCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD1949WellingtonCircuit.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD2000AmuriCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000BayofPlentyCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000BluffCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000BullerCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000ChathamIslandCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000CollingwoodCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000GawlerCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000GreyCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000HawkesBayCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000HokitikaCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000JacksonsBayCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000KarameaCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000LindisPeakCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000MarlboroughCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000MountEdenCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000MountNicholasCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000MountPleasantCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000MountYorkCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000NelsonCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000NewZealandTransverseMercator.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000NorthTaieriCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000ObservationPointCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000OkaritoCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000PovertyBayCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000TaranakiCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000TimaruCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000TuhirangiCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000UTMZone58S.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000UTMZone59S.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000UTMZone60S.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000WairarapaCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000WanganuiCircuit.GeographicInfo.Name = "GCS_NZGD_2000";
            NZGD2000WellingtonCircuit.GeographicInfo.Name = "GCS_NZGD_2000";

            ChathamIslands1979MapGrid.GeographicInfo.Datum.Name = "D_Chatham_Islands_1979";
            NewZealandMapGrid.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NewZealandNorthIsland.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NewZealandSouthIsland.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949AmuriCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949BayofPlentyCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949BluffCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949BullerCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949CollingwoodCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949GawlerCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949GreyCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949HawkesBayCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949HokitikaCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949JacksonsBayCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949KarameaCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949LindisPeakCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949MarlboroughCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949MountEdenCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949MountNicholasCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949MountPleasantCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949MountYorkCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949NelsonCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949NorthTaieriCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949ObservationPointCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949OkaritoCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949PovertyBayCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949TaranakiCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949TimaruCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949TuhirangiCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949UTMZone58S.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949UTMZone59S.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949UTMZone60S.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949WairarapaCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949WanganuiCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD1949WellingtonCircuit.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD2000AmuriCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000BayofPlentyCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000BluffCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000BullerCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000ChathamIslandCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000CollingwoodCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000GawlerCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000GreyCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000HawkesBayCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000HokitikaCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000JacksonsBayCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000KarameaCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000LindisPeakCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000MarlboroughCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000MountEdenCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000MountNicholasCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000MountPleasantCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000MountYorkCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000NelsonCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000NewZealandTransverseMercator.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000NorthTaieriCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000ObservationPointCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000OkaritoCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000PovertyBayCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000TaranakiCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000TimaruCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000TuhirangiCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000UTMZone58S.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000UTMZone59S.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000UTMZone60S.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000WairarapaCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000WanganuiCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
            NZGD2000WellingtonCircuit.GeographicInfo.Datum.Name = "D_NZGD_2000";
        }

        #endregion
    }
}

#pragma warning restore 1591