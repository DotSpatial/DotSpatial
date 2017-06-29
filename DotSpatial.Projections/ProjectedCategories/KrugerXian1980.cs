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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:41:10 PM
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
    /// KrugerZian1980
    /// </summary>
    public class KrugerXian1980 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Xian19803DegreeGKCM102E;
        public readonly ProjectionInfo Xian19803DegreeGKCM105E;
        public readonly ProjectionInfo Xian19803DegreeGKCM108E;
        public readonly ProjectionInfo Xian19803DegreeGKCM111E;
        public readonly ProjectionInfo Xian19803DegreeGKCM114E;
        public readonly ProjectionInfo Xian19803DegreeGKCM117E;
        public readonly ProjectionInfo Xian19803DegreeGKCM120E;
        public readonly ProjectionInfo Xian19803DegreeGKCM123E;
        public readonly ProjectionInfo Xian19803DegreeGKCM126E;
        public readonly ProjectionInfo Xian19803DegreeGKCM129E;
        public readonly ProjectionInfo Xian19803DegreeGKCM132E;
        public readonly ProjectionInfo Xian19803DegreeGKCM135E;
        public readonly ProjectionInfo Xian19803DegreeGKCM75E;
        public readonly ProjectionInfo Xian19803DegreeGKCM78E;
        public readonly ProjectionInfo Xian19803DegreeGKCM81E;
        public readonly ProjectionInfo Xian19803DegreeGKCM84E;
        public readonly ProjectionInfo Xian19803DegreeGKCM87E;
        public readonly ProjectionInfo Xian19803DegreeGKCM90E;
        public readonly ProjectionInfo Xian19803DegreeGKCM93E;
        public readonly ProjectionInfo Xian19803DegreeGKCM96E;
        public readonly ProjectionInfo Xian19803DegreeGKCM99E;
        public readonly ProjectionInfo Xian19803DegreeGKZone25;
        public readonly ProjectionInfo Xian19803DegreeGKZone26;
        public readonly ProjectionInfo Xian19803DegreeGKZone27;
        public readonly ProjectionInfo Xian19803DegreeGKZone28;
        public readonly ProjectionInfo Xian19803DegreeGKZone29;
        public readonly ProjectionInfo Xian19803DegreeGKZone30;
        public readonly ProjectionInfo Xian19803DegreeGKZone31;
        public readonly ProjectionInfo Xian19803DegreeGKZone32;
        public readonly ProjectionInfo Xian19803DegreeGKZone33;
        public readonly ProjectionInfo Xian19803DegreeGKZone34;
        public readonly ProjectionInfo Xian19803DegreeGKZone35;
        public readonly ProjectionInfo Xian19803DegreeGKZone36;
        public readonly ProjectionInfo Xian19803DegreeGKZone37;
        public readonly ProjectionInfo Xian19803DegreeGKZone38;
        public readonly ProjectionInfo Xian19803DegreeGKZone39;
        public readonly ProjectionInfo Xian19803DegreeGKZone40;
        public readonly ProjectionInfo Xian19803DegreeGKZone41;
        public readonly ProjectionInfo Xian19803DegreeGKZone42;
        public readonly ProjectionInfo Xian19803DegreeGKZone43;
        public readonly ProjectionInfo Xian19803DegreeGKZone44;
        public readonly ProjectionInfo Xian19803DegreeGKZone45;
        public readonly ProjectionInfo Xian1980GKCM105E;
        public readonly ProjectionInfo Xian1980GKCM111E;
        public readonly ProjectionInfo Xian1980GKCM117E;
        public readonly ProjectionInfo Xian1980GKCM123E;
        public readonly ProjectionInfo Xian1980GKCM129E;
        public readonly ProjectionInfo Xian1980GKCM135E;
        public readonly ProjectionInfo Xian1980GKCM75E;
        public readonly ProjectionInfo Xian1980GKCM81E;
        public readonly ProjectionInfo Xian1980GKCM87E;
        public readonly ProjectionInfo Xian1980GKCM93E;
        public readonly ProjectionInfo Xian1980GKCM99E;
        public readonly ProjectionInfo Xian1980GKZone13;
        public readonly ProjectionInfo Xian1980GKZone14;
        public readonly ProjectionInfo Xian1980GKZone15;
        public readonly ProjectionInfo Xian1980GKZone16;
        public readonly ProjectionInfo Xian1980GKZone17;
        public readonly ProjectionInfo Xian1980GKZone18;
        public readonly ProjectionInfo Xian1980GKZone19;
        public readonly ProjectionInfo Xian1980GKZone20;
        public readonly ProjectionInfo Xian1980GKZone21;
        public readonly ProjectionInfo Xian1980GKZone22;
        public readonly ProjectionInfo Xian1980GKZone23;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of KrugerZian1980
        /// </summary>
        public KrugerXian1980()
        {
            Xian19803DegreeGKCM102E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=102 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM105E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM108E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=108 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM111E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM114E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=114 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM117E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM120E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=120 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM123E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM126E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=126 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM129E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM132E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=132 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM135E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM75E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM78E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=78 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM81E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM84E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=84 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM87E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM90E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=90 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM93E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM96E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=96 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKCM99E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone25 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=25500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone26 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=78 +k=1.000000 +x_0=26500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone27 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=27500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone28 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=84 +k=1.000000 +x_0=28500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone29 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=29500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone30 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=90 +k=1.000000 +x_0=30500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone31 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=31500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone32 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=96 +k=1.000000 +x_0=32500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone33 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=33500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone34 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=102 +k=1.000000 +x_0=34500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone35 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=35500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone36 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=108 +k=1.000000 +x_0=36500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone37 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=37500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone38 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=114 +k=1.000000 +x_0=38500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone39 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=39500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone40 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=120 +k=1.000000 +x_0=40500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone41 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=41500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone42 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=126 +k=1.000000 +x_0=42500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone43 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=43500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone44 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=132 +k=1.000000 +x_0=44500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian19803DegreeGKZone45 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=45500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM105E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM111E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM117E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM123E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM129E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM135E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM75E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM81E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM87E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM93E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKCM99E = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone13 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=13500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone14 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=14500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone15 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=15500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone16 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=16500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone17 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=17500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone18 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=18500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone19 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=19500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone20 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=20500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone21 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=21500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone22 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=22500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");
            Xian1980GKZone23 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=23500000 +y_0=0 +a=6378140 +b=6356755.288157528 +units=m +no_defs ");

            Xian19803DegreeGKCM102E.Name = "Xian_1980_3_Degree_GK_CM_102E";
            Xian19803DegreeGKCM105E.Name = "Xian_1980_3_Degree_GK_CM_105E";
            Xian19803DegreeGKCM108E.Name = "Xian_1980_3_Degree_GK_CM_108E";
            Xian19803DegreeGKCM111E.Name = "Xian_1980_3_Degree_GK_CM_111E";
            Xian19803DegreeGKCM114E.Name = "Xian_1980_3_Degree_GK_CM_114E";
            Xian19803DegreeGKCM117E.Name = "Xian_1980_3_Degree_GK_CM_117E";
            Xian19803DegreeGKCM120E.Name = "Xian_1980_3_Degree_GK_CM_120E";
            Xian19803DegreeGKCM123E.Name = "Xian_1980_3_Degree_GK_CM_123E";
            Xian19803DegreeGKCM126E.Name = "Xian_1980_3_Degree_GK_CM_126E";
            Xian19803DegreeGKCM129E.Name = "Xian_1980_3_Degree_GK_CM_129E";
            Xian19803DegreeGKCM132E.Name = "Xian_1980_3_Degree_GK_CM_132E";
            Xian19803DegreeGKCM135E.Name = "Xian_1980_3_Degree_GK_CM_135E";
            Xian19803DegreeGKCM75E.Name = "Xian_1980_3_Degree_GK_CM_75E";
            Xian19803DegreeGKCM78E.Name = "Xian_1980_3_Degree_GK_CM_78E";
            Xian19803DegreeGKCM81E.Name = "Xian_1980_3_Degree_GK_CM_81E";
            Xian19803DegreeGKCM84E.Name = "Xian_1980_3_Degree_GK_CM_84E";
            Xian19803DegreeGKCM87E.Name = "Xian_1980_3_Degree_GK_CM_87E";
            Xian19803DegreeGKCM90E.Name = "Xian_1980_3_Degree_GK_CM_90E";
            Xian19803DegreeGKCM93E.Name = "Xian_1980_3_Degree_GK_CM_93E";
            Xian19803DegreeGKCM96E.Name = "Xian_1980_3_Degree_GK_CM_96E";
            Xian19803DegreeGKCM99E.Name = "Xian_1980_3_Degree_GK_CM_99E";
            Xian19803DegreeGKZone25.Name = "Xian_1980_3_Degree_GK_Zone_25";
            Xian19803DegreeGKZone26.Name = "Xian_1980_3_Degree_GK_Zone_26";
            Xian19803DegreeGKZone27.Name = "Xian_1980_3_Degree_GK_Zone_27";
            Xian19803DegreeGKZone28.Name = "Xian_1980_3_Degree_GK_Zone_28";
            Xian19803DegreeGKZone29.Name = "Xian_1980_3_Degree_GK_Zone_29";
            Xian19803DegreeGKZone30.Name = "Xian_1980_3_Degree_GK_Zone_30";
            Xian19803DegreeGKZone31.Name = "Xian_1980_3_Degree_GK_Zone_31";
            Xian19803DegreeGKZone32.Name = "Xian_1980_3_Degree_GK_Zone_32";
            Xian19803DegreeGKZone33.Name = "Xian_1980_3_Degree_GK_Zone_33";
            Xian19803DegreeGKZone34.Name = "Xian_1980_3_Degree_GK_Zone_34";
            Xian19803DegreeGKZone35.Name = "Xian_1980_3_Degree_GK_Zone_35";
            Xian19803DegreeGKZone36.Name = "Xian_1980_3_Degree_GK_Zone_36";
            Xian19803DegreeGKZone37.Name = "Xian_1980_3_Degree_GK_Zone_37";
            Xian19803DegreeGKZone38.Name = "Xian_1980_3_Degree_GK_Zone_38";
            Xian19803DegreeGKZone39.Name = "Xian_1980_3_Degree_GK_Zone_39";
            Xian19803DegreeGKZone40.Name = "Xian_1980_3_Degree_GK_Zone_40";
            Xian19803DegreeGKZone41.Name = "Xian_1980_3_Degree_GK_Zone_41";
            Xian19803DegreeGKZone42.Name = "Xian_1980_3_Degree_GK_Zone_42";
            Xian19803DegreeGKZone43.Name = "Xian_1980_3_Degree_GK_Zone_43";
            Xian19803DegreeGKZone44.Name = "Xian_1980_3_Degree_GK_Zone_44";
            Xian19803DegreeGKZone45.Name = "Xian_1980_3_Degree_GK_Zone_45";
            Xian1980GKCM105E.Name = "Xian_1980_GK_CM_105E";
            Xian1980GKCM111E.Name = "Xian_1980_GK_CM_111E";
            Xian1980GKCM117E.Name = "Xian_1980_GK_CM_117E";
            Xian1980GKCM123E.Name = "Xian_1980_GK_CM_123E";
            Xian1980GKCM129E.Name = "Xian_1980_GK_CM_129E";
            Xian1980GKCM135E.Name = "Xian_1980_GK_CM_135E";
            Xian1980GKCM75E.Name = "Xian_1980_GK_CM_75E";
            Xian1980GKCM81E.Name = "Xian_1980_GK_CM_81E";
            Xian1980GKCM87E.Name = "Xian_1980_GK_CM_87E";
            Xian1980GKCM93E.Name = "Xian_1980_GK_CM_93E";
            Xian1980GKCM99E.Name = "Xian_1980_GK_CM_99E";
            Xian1980GKZone13.Name = "Xian_1980_GK_Zone_13";
            Xian1980GKZone14.Name = "Xian_1980_GK_Zone_14";
            Xian1980GKZone15.Name = "Xian_1980_GK_Zone_15";
            Xian1980GKZone16.Name = "Xian_1980_GK_Zone_16";
            Xian1980GKZone17.Name = "Xian_1980_GK_Zone_17";
            Xian1980GKZone18.Name = "Xian_1980_GK_Zone_18";
            Xian1980GKZone19.Name = "Xian_1980_GK_Zone_19";
            Xian1980GKZone20.Name = "Xian_1980_GK_Zone_20";
            Xian1980GKZone21.Name = "Xian_1980_GK_Zone_21";
            Xian1980GKZone22.Name = "Xian_1980_GK_Zone_22";
            Xian1980GKZone23.Name = "Xian_1980_GK_Zone_23";

            Xian19803DegreeGKCM102E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM105E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM108E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM111E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM114E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM117E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM120E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM123E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM126E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM129E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM132E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM135E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM75E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM78E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM81E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM84E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM87E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM90E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM93E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM96E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKCM99E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone25.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone26.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone27.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone28.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone29.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone30.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone31.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone32.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone33.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone34.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone35.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone36.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone37.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone38.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone39.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone40.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone41.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone42.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone43.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone44.GeographicInfo.Name = "GCS_Xian_1980";
            Xian19803DegreeGKZone45.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM105E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM111E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM117E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM123E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM129E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM135E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM75E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM81E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM87E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM93E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKCM99E.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone13.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone14.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone15.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone16.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone17.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone18.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone19.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone20.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone21.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone22.GeographicInfo.Name = "GCS_Xian_1980";
            Xian1980GKZone23.GeographicInfo.Name = "GCS_Xian_1980";

            Xian19803DegreeGKCM102E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM105E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM108E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM111E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM114E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM117E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM120E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM123E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM126E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM129E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM132E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM135E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM75E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM78E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM81E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM84E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM87E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM90E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM93E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM96E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKCM99E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone25.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone26.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone27.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone28.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone29.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone30.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone31.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone32.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone33.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone34.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone35.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone36.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone37.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone38.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone39.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone40.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone41.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone42.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone43.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone44.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian19803DegreeGKZone45.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM105E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM111E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM117E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM123E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM129E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM135E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM75E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM81E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM87E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM93E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKCM99E.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone13.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone14.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone15.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone16.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone17.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone18.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone19.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone20.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone21.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone22.GeographicInfo.Datum.Name = "D_Xian_1980";
            Xian1980GKZone23.GeographicInfo.Datum.Name = "D_Xian_1980";
        }

        #endregion
    }
}

#pragma warning restore 1591