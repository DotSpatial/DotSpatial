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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:54:34 PM
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
    /// NationalGridsSweden
    /// </summary>
    public class NationalGridsSweden : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo RT380gon;
        public readonly ProjectionInfo RT3825gonO;
        public readonly ProjectionInfo RT3825gonV;
        public readonly ProjectionInfo RT385gonO;
        public readonly ProjectionInfo RT385gonV;
        public readonly ProjectionInfo RT3875gonV;
        public readonly ProjectionInfo RT900gon;
        public readonly ProjectionInfo RT9025gonO;
        public readonly ProjectionInfo RT9025gonV;
        public readonly ProjectionInfo RT905gonO;
        public readonly ProjectionInfo RT905gonV;
        public readonly ProjectionInfo RT9075gonV;
        public readonly ProjectionInfo SWEREF991200;
        public readonly ProjectionInfo SWEREF991330;
        public readonly ProjectionInfo SWEREF991415;
        public readonly ProjectionInfo SWEREF991500;
        public readonly ProjectionInfo SWEREF991545;
        public readonly ProjectionInfo SWEREF991630;
        public readonly ProjectionInfo SWEREF991715;
        public readonly ProjectionInfo SWEREF991800;
        public readonly ProjectionInfo SWEREF991845;
        public readonly ProjectionInfo SWEREF992015;
        public readonly ProjectionInfo SWEREF992145;
        public readonly ProjectionInfo SWEREF992315;
        public readonly ProjectionInfo SWEREF99TM;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsSweden
        /// </summary>
        public NationalGridsSweden()
        {
            RT380gon = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=18.05827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT3825gonO = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=20.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT3825gonV = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=15.80827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT385gonO = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=22.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT385gonV = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=13.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT3875gonV = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=11.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT900gon = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=18.05827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT9025gonO = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=20.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT9025gonV = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=15.80827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT905gonO = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=22.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT905gonV = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=13.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT9075gonV = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=11.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            SWEREF991200 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=12 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991330 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=13.5 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991415 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=14.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991500 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991545 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=15.75 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991630 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=16.5 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991715 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=17.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991800 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=18 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991845 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=18.75 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF992015 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=20.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF992145 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=21.75 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF992315 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=23.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF99TM = ProjectionInfo.FromProj4String("+proj=utm +zone=33 +ellps=GRS80 +units=m +no_defs ");
            RT380gon.Name = "RT38_0_gon";
            RT3825gonO.Name = "RT38_25_gon_O";
            RT3825gonV.Name = "RT38_25_gon_V";
            RT385gonO.Name = "RT38_5_gon_O";
            RT385gonV.Name = "RT38_5_gon_V";
            RT3875gonV.Name = "RT38_75_gon_V";
            RT900gon.Name = "RT90_0_gon";
            RT9025gonO.Name = "RT90_25_gon_O";
            RT9025gonV.Name = "RT90_25_gon_V";
            RT905gonO.Name = "RT90_5_gon_O";
            RT905gonV.Name = "RT90_5_gon_V";
            RT9075gonV.Name = "RT90_75_gon_V";
            SWEREF991200.Name = "SWEREF99_12_00";
            SWEREF991330.Name = "SWEREF99_13_30";
            SWEREF991415.Name = "SWEREF99_14_15";
            SWEREF991500.Name = "SWEREF99_15_00";
            SWEREF991545.Name = "SWEREF99_15_45";
            SWEREF991630.Name = "SWEREF99_16_30";
            SWEREF991715.Name = "SWEREF99_17_15";
            SWEREF991800.Name = "SWEREF99_18_00";
            SWEREF991845.Name = "SWEREF99_18_45";
            SWEREF992015.Name = "SWEREF99_20_15";
            SWEREF992145.Name = "SWEREF99_21_45";
            SWEREF992315.Name = "SWEREF99_23_15";
            SWEREF99TM.Name = "SWEREF99_TM";
            RT380gon.GeographicInfo.Name = "GCS_RT38";
            RT3825gonO.GeographicInfo.Name = "GCS_RT38";
            RT3825gonV.GeographicInfo.Name = "GCS_RT38";
            RT385gonO.GeographicInfo.Name = "GCS_RT38";
            RT385gonV.GeographicInfo.Name = "GCS_RT38";
            RT3875gonV.GeographicInfo.Name = "GCS_RT38";
            RT900gon.GeographicInfo.Name = "GCS_RT_1990";
            RT9025gonO.GeographicInfo.Name = "GCS_RT_1990";
            RT9025gonV.GeographicInfo.Name = "GCS_RT_1990";
            RT905gonO.GeographicInfo.Name = "GCS_RT_1990";
            RT905gonV.GeographicInfo.Name = "GCS_RT_1990";
            RT9075gonV.GeographicInfo.Name = "GCS_RT_1990";
            SWEREF991200.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991330.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991415.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991500.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991545.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991630.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991715.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991800.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF991845.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF992015.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF992145.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF992315.GeographicInfo.Name = "GCS_SWEREF99";
            SWEREF99TM.GeographicInfo.Name = "GCS_SWEREF99";
            RT380gon.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            RT3825gonO.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            RT3825gonV.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            RT385gonO.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            RT385gonV.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            RT3875gonV.GeographicInfo.Datum.Name = "D_Stockholm_1938";
            RT900gon.GeographicInfo.Datum.Name = "D_RT_1990";
            RT9025gonO.GeographicInfo.Datum.Name = "D_RT_1990";
            RT9025gonV.GeographicInfo.Datum.Name = "D_RT_1990";
            RT905gonO.GeographicInfo.Datum.Name = "D_RT_1990";
            RT905gonV.GeographicInfo.Datum.Name = "D_RT_1990";
            RT9075gonV.GeographicInfo.Datum.Name = "D_RT_1990";
            SWEREF991200.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991330.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991415.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991500.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991545.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991630.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991715.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991800.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF991845.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF992015.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF992145.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF992315.GeographicInfo.Datum.Name = "D_SWEREF99";
            SWEREF99TM.GeographicInfo.Datum.Name = "D_SWEREF99";
        }

        #endregion
    }
}

#pragma warning restore 1591