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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:53:34 PM
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
    /// NationalGridsNorway
    /// </summary>
    public class NationalGridsNorway : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NGO1948BaerumKommune;
        public readonly ProjectionInfo NGO1948Bergenhalvoen;
        public readonly ProjectionInfo NGO1948NorwayZone1;
        public readonly ProjectionInfo NGO1948NorwayZone2;
        public readonly ProjectionInfo NGO1948NorwayZone3;
        public readonly ProjectionInfo NGO1948NorwayZone4;
        public readonly ProjectionInfo NGO1948NorwayZone5;
        public readonly ProjectionInfo NGO1948NorwayZone6;
        public readonly ProjectionInfo NGO1948NorwayZone7;
        public readonly ProjectionInfo NGO1948NorwayZone8;
        public readonly ProjectionInfo NGO1948OsloKommune;
        public readonly ProjectionInfo NGO1948OsloNorwayZone1;
        public readonly ProjectionInfo NGO1948OsloNorwayZone2;
        public readonly ProjectionInfo NGO1948OsloNorwayZone3;
        public readonly ProjectionInfo NGO1948OsloNorwayZone4;
        public readonly ProjectionInfo NGO1948OsloNorwayZone5;
        public readonly ProjectionInfo NGO1948OsloNorwayZone6;
        public readonly ProjectionInfo NGO1948OsloNorwayZone7;
        public readonly ProjectionInfo NGO1948OsloNorwayZone8;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsNorway
        /// </summary>
        public NationalGridsNorway()
        {
            NGO1948BaerumKommune = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=10.72291666666667 +k=1.000000 +x_0=19999.32 +y_0=-202977.79 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948Bergenhalvoen = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=6.05625 +k=1.000000 +x_0=100000 +y_0=-200000 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone1 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=6.056249999999999 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone2 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=8.389583333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=10.72291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=13.22291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=16.88958333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone6 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=20.88958333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone7 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=24.88958333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=29.05625 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948OsloKommune = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=10.72291666666667 +k=1.000000 +x_0=0 +y_0=-212979.18 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948OsloNorwayZone1 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=-15.38958333333334 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone2 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=-13.05625 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=-10.72291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=-8.22291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=-4.556250000000003 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone6 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=-0.5562500000000004 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone7 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=3.44375 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=58 +lon_0=7.610416666666659 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");

            NGO1948BaerumKommune.Name = "NGO_1948_Baerum_Kommune";
            NGO1948Bergenhalvoen.Name = "NGO_1948_Bergenhalvoen";
            NGO1948NorwayZone1.Name = "NGO_1948_Norway_Zone_1";
            NGO1948NorwayZone2.Name = "NGO_1948_Norway_Zone_2";
            NGO1948NorwayZone3.Name = "NGO_1948_Norway_Zone_3";
            NGO1948NorwayZone4.Name = "NGO_1948_Norway_Zone_4";
            NGO1948NorwayZone5.Name = "NGO_1948_Norway_Zone_5";
            NGO1948NorwayZone6.Name = "NGO_1948_Norway_Zone_6";
            NGO1948NorwayZone7.Name = "NGO_1948_Norway_Zone_7";
            NGO1948NorwayZone8.Name = "NGO_1948_Norway_Zone_8";
            NGO1948OsloKommune.Name = "NGO_1948_Oslo_Kommune";
            NGO1948OsloNorwayZone1.Name = "NGO_1948_Oslo_Norway_Zone_1";
            NGO1948OsloNorwayZone2.Name = "NGO_1948_Oslo_Norway_Zone_2";
            NGO1948OsloNorwayZone3.Name = "NGO_1948_Oslo_Norway_Zone_3";
            NGO1948OsloNorwayZone4.Name = "NGO_1948_Oslo_Norway_Zone_4";
            NGO1948OsloNorwayZone5.Name = "NGO_1948_Oslo_Norway_Zone_5";
            NGO1948OsloNorwayZone6.Name = "NGO_1948_Oslo_Norway_Zone_6";
            NGO1948OsloNorwayZone7.Name = "NGO_1948_Oslo_Norway_Zone_7";
            NGO1948OsloNorwayZone8.Name = "NGO_1948_Oslo_Norway_Zone_8";

            NGO1948BaerumKommune.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948Bergenhalvoen.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone1.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone2.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone3.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone4.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone5.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone6.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone7.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948NorwayZone8.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948OsloKommune.GeographicInfo.Name = "GCS_NGO_1948";
            NGO1948OsloNorwayZone1.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NGO1948OsloNorwayZone2.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NGO1948OsloNorwayZone3.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NGO1948OsloNorwayZone4.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NGO1948OsloNorwayZone5.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NGO1948OsloNorwayZone6.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NGO1948OsloNorwayZone7.GeographicInfo.Name = "GCS_NGO_1948_Oslo";
            NGO1948OsloNorwayZone8.GeographicInfo.Name = "GCS_NGO_1948_Oslo";

            NGO1948BaerumKommune.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948Bergenhalvoen.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone1.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone2.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone3.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone4.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone5.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone6.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone7.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948NorwayZone8.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloKommune.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone1.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone2.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone3.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone4.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone5.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone6.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone7.GeographicInfo.Datum.Name = "D_NGO_1948";
            NGO1948OsloNorwayZone8.GeographicInfo.Datum.Name = "D_NGO_1948";
        }

        #endregion
    }
}

#pragma warning restore 1591