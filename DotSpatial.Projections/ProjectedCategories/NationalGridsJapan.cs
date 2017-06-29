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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:51:42 PM
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
    /// NationalGridsJapan
    /// </summary>
    public class NationalGridsJapan : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo JGD2000JapanZone1;
        public readonly ProjectionInfo JGD2000JapanZone10;
        public readonly ProjectionInfo JGD2000JapanZone11;
        public readonly ProjectionInfo JGD2000JapanZone12;
        public readonly ProjectionInfo JGD2000JapanZone13;
        public readonly ProjectionInfo JGD2000JapanZone14;
        public readonly ProjectionInfo JGD2000JapanZone15;
        public readonly ProjectionInfo JGD2000JapanZone16;
        public readonly ProjectionInfo JGD2000JapanZone17;
        public readonly ProjectionInfo JGD2000JapanZone18;
        public readonly ProjectionInfo JGD2000JapanZone19;
        public readonly ProjectionInfo JGD2000JapanZone2;
        public readonly ProjectionInfo JGD2000JapanZone3;
        public readonly ProjectionInfo JGD2000JapanZone4;
        public readonly ProjectionInfo JGD2000JapanZone5;
        public readonly ProjectionInfo JGD2000JapanZone6;
        public readonly ProjectionInfo JGD2000JapanZone7;
        public readonly ProjectionInfo JGD2000JapanZone8;
        public readonly ProjectionInfo JGD2000JapanZone9;
        public readonly ProjectionInfo JapanZone1;
        public readonly ProjectionInfo JapanZone10;
        public readonly ProjectionInfo JapanZone11;
        public readonly ProjectionInfo JapanZone12;
        public readonly ProjectionInfo JapanZone13;
        public readonly ProjectionInfo JapanZone14;
        public readonly ProjectionInfo JapanZone15;
        public readonly ProjectionInfo JapanZone16;
        public readonly ProjectionInfo JapanZone17;
        public readonly ProjectionInfo JapanZone18;
        public readonly ProjectionInfo JapanZone19;
        public readonly ProjectionInfo JapanZone2;
        public readonly ProjectionInfo JapanZone3;
        public readonly ProjectionInfo JapanZone4;
        public readonly ProjectionInfo JapanZone5;
        public readonly ProjectionInfo JapanZone6;
        public readonly ProjectionInfo JapanZone7;
        public readonly ProjectionInfo JapanZone8;
        public readonly ProjectionInfo JapanZone9;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsJapan
        /// </summary>
        public NationalGridsJapan()
        {
            JapanZone1 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=33 +lon_0=129.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone10 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40 +lon_0=140.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone11 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=44 +lon_0=140.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone12 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=44 +lon_0=142.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone13 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=44 +lon_0=144.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone14 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=142 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone15 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=127.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone16 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=124 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone17 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone18 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=20 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone19 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=154 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone2 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=33 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=132.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=33 +lon_0=133.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=134.3333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone6 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone7 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=137.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=138.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone9 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=139.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JGD2000JapanZone1 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=33 +lon_0=129.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone10 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=40 +lon_0=140.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone11 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=44 +lon_0=140.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone12 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=44 +lon_0=142.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone13 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=44 +lon_0=144.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone14 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=142 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone15 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=127.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone16 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=124 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone17 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone18 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=20 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone19 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=26 +lon_0=154 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone2 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=33 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone3 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=132.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone4 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=33 +lon_0=133.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone5 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=134.3333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone6 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone7 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=137.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone8 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=138.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone9 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=36 +lon_0=139.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");

            JapanZone1.Name = "Japan_Zone_1";
            JapanZone10.Name = "Japan_Zone_10";
            JapanZone11.Name = "Japan_Zone_11";
            JapanZone12.Name = "Japan_Zone_12";
            JapanZone13.Name = "Japan_Zone_13";
            JapanZone14.Name = "Japan_Zone_14";
            JapanZone15.Name = "Japan_Zone_15";
            JapanZone16.Name = "Japan_Zone_16";
            JapanZone17.Name = "Japan_Zone_17";
            JapanZone18.Name = "Japan_Zone_18";
            JapanZone19.Name = "Japan_Zone_19";
            JapanZone2.Name = "Japan_Zone_2";
            JapanZone3.Name = "Japan_Zone_3";
            JapanZone4.Name = "Japan_Zone_4";
            JapanZone5.Name = "Japan_Zone_5";
            JapanZone6.Name = "Japan_Zone_6";
            JapanZone7.Name = "Japan_Zone_7";
            JapanZone8.Name = "Japan_Zone_8";
            JapanZone9.Name = "Japan_Zone_9";
            JGD2000JapanZone1.Name = "JGD_2000_Japan_Zone_1";
            JGD2000JapanZone10.Name = "JGD_2000_Japan_Zone_10";
            JGD2000JapanZone11.Name = "JGD_2000_Japan_Zone_11";
            JGD2000JapanZone12.Name = "JGD_2000_Japan_Zone_12";
            JGD2000JapanZone13.Name = "JGD_2000_Japan_Zone_13";
            JGD2000JapanZone14.Name = "JGD_2000_Japan_Zone_14";
            JGD2000JapanZone15.Name = "JGD_2000_Japan_Zone_15";
            JGD2000JapanZone16.Name = "JGD_2000_Japan_Zone_16";
            JGD2000JapanZone17.Name = "JGD_2000_Japan_Zone_17";
            JGD2000JapanZone18.Name = "JGD_2000_Japan_Zone_18";
            JGD2000JapanZone19.Name = "JGD_2000_Japan_Zone_19";
            JGD2000JapanZone2.Name = "JGD_2000_Japan_Zone_2";
            JGD2000JapanZone3.Name = "JGD_2000_Japan_Zone_3";
            JGD2000JapanZone4.Name = "JGD_2000_Japan_Zone_4";
            JGD2000JapanZone5.Name = "JGD_2000_Japan_Zone_5";
            JGD2000JapanZone6.Name = "JGD_2000_Japan_Zone_6";
            JGD2000JapanZone7.Name = "JGD_2000_Japan_Zone_7";
            JGD2000JapanZone8.Name = "JGD_2000_Japan_Zone_8";
            JGD2000JapanZone9.Name = "JGD_2000_Japan_Zone_9";

            JapanZone1.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone10.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone11.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone12.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone13.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone14.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone15.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone16.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone17.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone18.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone19.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone2.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone3.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone4.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone5.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone6.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone7.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone8.GeographicInfo.Name = "GCS_Tokyo";
            JapanZone9.GeographicInfo.Name = "GCS_Tokyo";
            JGD2000JapanZone1.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone10.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone11.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone12.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone13.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone14.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone15.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone16.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone17.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone18.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone19.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone2.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone3.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone4.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone5.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone6.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone7.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone8.GeographicInfo.Name = "GCS_JGD_2000";
            JGD2000JapanZone9.GeographicInfo.Name = "GCS_JGD_2000";

            JapanZone1.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone10.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone11.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone12.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone13.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone14.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone15.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone16.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone17.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone18.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone19.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone2.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone3.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone4.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone5.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone6.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone7.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone8.GeographicInfo.Datum.Name = "D_Tokyo";
            JapanZone9.GeographicInfo.Datum.Name = "D_Tokyo";
            JGD2000JapanZone1.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone10.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone11.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone12.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone13.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone14.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone15.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone16.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone17.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone18.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone19.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone2.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone3.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone4.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone5.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone6.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone7.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone8.GeographicInfo.Datum.Name = "D_JGD_2000";
            JGD2000JapanZone9.GeographicInfo.Datum.Name = "D_JGD_2000";
        }

        #endregion
    }
}

#pragma warning restore 1591