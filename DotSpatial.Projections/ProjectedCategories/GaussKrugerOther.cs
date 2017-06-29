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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:36:17 PM
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
    /// GaussKrugerOther
    /// </summary>
    public class GaussKrugerOther : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Albanian1987GKZone4;
        public readonly ProjectionInfo ED19503DegreeGKZone10;
        public readonly ProjectionInfo ED19503DegreeGKZone11;
        public readonly ProjectionInfo ED19503DegreeGKZone12;
        public readonly ProjectionInfo ED19503DegreeGKZone13;
        public readonly ProjectionInfo ED19503DegreeGKZone14;
        public readonly ProjectionInfo ED19503DegreeGKZone15;
        public readonly ProjectionInfo ED19503DegreeGKZone9;
        public readonly ProjectionInfo Hanoi1972GKZone18;
        public readonly ProjectionInfo Hanoi1972GKZone19;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone3;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone4;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone5;
        public readonly ProjectionInfo SouthYemenGKZone8;
        public readonly ProjectionInfo SouthYemenGKZone9;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GaussKrugerOther
        /// </summary>
        public GaussKrugerOther()
        {
            Albanian1987GKZone4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            ED19503DegreeGKZone10 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=10500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone11 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=11500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone12 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=36 +k=1.000000 +x_0=12500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone13 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=39 +k=1.000000 +x_0=13500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone14 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=42 +k=1.000000 +x_0=14500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone15 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=15500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone9 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=9500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            Hanoi1972GKZone18 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=18500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Hanoi1972GKZone19 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=19500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1942Adj19833DegreeGKZone3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=9 +k=1.000000 +x_0=3500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1942Adj19833DegreeGKZone4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=12 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1942Adj19833DegreeGKZone5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=5500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            SouthYemenGKZone8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=8500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            SouthYemenGKZone9 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=51 +k=1.000000 +x_0=9500000 +y_0=0 +ellps=krass +units=m +no_defs ");

            Albanian1987GKZone4.Name = "Albanian_1987_GK_Zone_4";
            ED19503DegreeGKZone10.Name = "ED_1950_3_Degree_GK_Zone_10";
            ED19503DegreeGKZone11.Name = "ED_1950_3_Degree_GK_Zone_11";
            ED19503DegreeGKZone12.Name = "ED_1950_3_Degree_GK_Zone_12";
            ED19503DegreeGKZone13.Name = "ED_1950_3_Degree_GK_Zone_13";
            ED19503DegreeGKZone14.Name = "ED_1950_3_Degree_GK_Zone_14";
            ED19503DegreeGKZone15.Name = "ED_1950_3_Degree_GK_Zone_15";
            ED19503DegreeGKZone9.Name = "ED_1950_3_Degree_GK_Zone_9";
            Hanoi1972GKZone18.Name = "Hanoi_1972_GK_Zone_18";
            Hanoi1972GKZone19.Name = "Hanoi_1972_GK_Zone_19";
            Pulkovo1942Adj19833DegreeGKZone3.Name = "Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_3";
            Pulkovo1942Adj19833DegreeGKZone4.Name = "Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_4";
            Pulkovo1942Adj19833DegreeGKZone5.Name = "Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_5";
            SouthYemenGKZone8.Name = "South_Yemen_GK_Zone_8";
            SouthYemenGKZone9.Name = "South_Yemen_GK_Zone_9";

            Albanian1987GKZone4.GeographicInfo.Name = "GCS_Albanian_1987";
            ED19503DegreeGKZone10.GeographicInfo.Name = "GCS_European_1950";
            ED19503DegreeGKZone11.GeographicInfo.Name = "GCS_European_1950";
            ED19503DegreeGKZone12.GeographicInfo.Name = "GCS_European_1950";
            ED19503DegreeGKZone13.GeographicInfo.Name = "GCS_European_1950";
            ED19503DegreeGKZone14.GeographicInfo.Name = "GCS_European_1950";
            ED19503DegreeGKZone15.GeographicInfo.Name = "GCS_European_1950";
            ED19503DegreeGKZone9.GeographicInfo.Name = "GCS_European_1950";
            Hanoi1972GKZone18.GeographicInfo.Name = "GCS_Hanoi_1972";
            Hanoi1972GKZone19.GeographicInfo.Name = "GCS_Hanoi_1972";
            Pulkovo1942Adj19833DegreeGKZone3.GeographicInfo.Name = "GCS_Pulkovo_1942_Adj_1983";
            Pulkovo1942Adj19833DegreeGKZone4.GeographicInfo.Name = "GCS_Pulkovo_1942_Adj_1983";
            Pulkovo1942Adj19833DegreeGKZone5.GeographicInfo.Name = "GCS_Pulkovo_1942_Adj_1983";
            SouthYemenGKZone8.GeographicInfo.Name = "GCS_South_Yemen";
            SouthYemenGKZone9.GeographicInfo.Name = "GCS_South_Yemen";

            Albanian1987GKZone4.GeographicInfo.Datum.Name = "D_Albanian_1987";
            ED19503DegreeGKZone10.GeographicInfo.Datum.Name = "D_European_1950";
            ED19503DegreeGKZone11.GeographicInfo.Datum.Name = "D_European_1950";
            ED19503DegreeGKZone12.GeographicInfo.Datum.Name = "D_European_1950";
            ED19503DegreeGKZone13.GeographicInfo.Datum.Name = "D_European_1950";
            ED19503DegreeGKZone14.GeographicInfo.Datum.Name = "D_European_1950";
            ED19503DegreeGKZone15.GeographicInfo.Datum.Name = "D_European_1950";
            ED19503DegreeGKZone9.GeographicInfo.Datum.Name = "D_European_1950";
            Hanoi1972GKZone18.GeographicInfo.Datum.Name = "D_Hanoi_1972";
            Hanoi1972GKZone19.GeographicInfo.Datum.Name = "D_Hanoi_1972";
            Pulkovo1942Adj19833DegreeGKZone3.GeographicInfo.Datum.Name = "D_Pulkovo_1942_Adj_1983";
            Pulkovo1942Adj19833DegreeGKZone4.GeographicInfo.Datum.Name = "D_Pulkovo_1942_Adj_1983";
            Pulkovo1942Adj19833DegreeGKZone5.GeographicInfo.Datum.Name = "D_Pulkovo_1942_Adj_1983";
            SouthYemenGKZone8.GeographicInfo.Datum.Name = "D_South_Yemen";
            SouthYemenGKZone9.GeographicInfo.Datum.Name = "D_South_Yemen";
        }

        #endregion
    }
}

#pragma warning restore 1591