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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:49:00 PM
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
    /// IndianSubcontinent
    /// </summary>
    public class NationalGridsIndia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Kalianpur1880IndiaZone0;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIII;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIV;
        public readonly ProjectionInfo Kalianpur1937IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1937UTMZone45N;
        public readonly ProjectionInfo Kalianpur1937UTMZone46N;
        public readonly ProjectionInfo Kalianpur1962IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1962IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1962UTMZone41N;
        public readonly ProjectionInfo Kalianpur1962UTMZone42N;
        public readonly ProjectionInfo Kalianpur1962UTMZone43N;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIII;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIV;
        public readonly ProjectionInfo Kalianpur1975UTMZone42N;
        public readonly ProjectionInfo Kalianpur1975UTMZone43N;
        public readonly ProjectionInfo Kalianpur1975UTMZone44N;
        public readonly ProjectionInfo Kalianpur1975UTMZone45N;
        public readonly ProjectionInfo Kalianpur1975UTMZone46N;
        public readonly ProjectionInfo Kalianpur1975UTMZone47N;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IndianSubcontinent
        /// </summary>
        public NationalGridsIndia()
        {
            Kalianpur1880IndiaZone0 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=39.5 +lat_0=39.5 +lon_0=68 +k_0=0.99846154 +x_0=2153865.73916853 +y_0=2368292.194628102 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneI = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=68 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIIa = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=74 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIIb = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=90 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIII = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=19 +lat_0=19 +lon_0=80 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIV = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=12 +lat_0=12 +lon_0=80 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1937IndiaZoneIIb = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=90 +k_0=0.99878641 +x_0=2743195.5 +y_0=914398.5 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Kalianpur1937UTMZone45N = ProjectionInfo.FromProj4String("+proj=utm +zone=45 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Kalianpur1937UTMZone46N = ProjectionInfo.FromProj4String("+proj=utm +zone=46 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Kalianpur1962IndiaZoneI = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=68 +k_0=0.99878641 +x_0=2743196.4 +y_0=914398.8000000001 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962IndiaZoneIIa = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=74 +k_0=0.99878641 +x_0=2743196.4 +y_0=914398.8000000001 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962UTMZone41N = ProjectionInfo.FromProj4String("+proj=utm +zone=41 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962UTMZone42N = ProjectionInfo.FromProj4String("+proj=utm +zone=42 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962UTMZone43N = ProjectionInfo.FromProj4String("+proj=utm +zone=43 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1975IndiaZoneI = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=68 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIIa = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=74 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIIb = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=90 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIII = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=19 +lat_0=19 +lon_0=80 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIV = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=12 +lat_0=12 +lon_0=80 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone42N = ProjectionInfo.FromProj4String("+proj=utm +zone=42 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone43N = ProjectionInfo.FromProj4String("+proj=utm +zone=43 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone44N = ProjectionInfo.FromProj4String("+proj=utm +zone=44 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone45N = ProjectionInfo.FromProj4String("+proj=utm +zone=45 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone46N = ProjectionInfo.FromProj4String("+proj=utm +zone=46 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone47N = ProjectionInfo.FromProj4String("+proj=utm +zone=47 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");

            Kalianpur1880IndiaZone0.Name = "Kalianpur_1880_India_Zone_0";
            Kalianpur1880IndiaZoneI.Name = "Kalianpur_1880_India_Zone_I";
            Kalianpur1880IndiaZoneIIa.Name = "Kalianpur_1880_India_Zone_IIa";
            Kalianpur1880IndiaZoneIIb.Name = "Kalianpur_1880_India_Zone_IIb";
            Kalianpur1880IndiaZoneIII.Name = "Kalianpur_1880_India_Zone_III";
            Kalianpur1880IndiaZoneIV.Name = "Kalianpur_1880_India_Zone_IV";
            Kalianpur1937IndiaZoneIIb.Name = "Kalianpur_1937_India_Zone_IIb";
            Kalianpur1937UTMZone45N.Name = "Kalianpur_1937_UTM_Zone_45N";
            Kalianpur1937UTMZone46N.Name = "Kalianpur_1937_UTM_Zone_46N";
            Kalianpur1962IndiaZoneI.Name = "Kalianpur_1962_India_Zone_I";
            Kalianpur1962IndiaZoneIIa.Name = "Kalianpur_1962_India_Zone_IIa";
            Kalianpur1962UTMZone41N.Name = "Kalianpur_1962_UTM_Zone_41N";
            Kalianpur1962UTMZone42N.Name = "Kalianpur_1962_UTM_Zone_42N";
            Kalianpur1962UTMZone43N.Name = "Kalianpur_1962_UTM_Zone_43N";
            Kalianpur1975IndiaZoneI.Name = "Kalianpur_1975_India_Zone_I";
            Kalianpur1975IndiaZoneIIa.Name = "Kalianpur_1975_India_Zone_IIa";
            Kalianpur1975IndiaZoneIIb.Name = "Kalianpur_1975_India_Zone_IIb";
            Kalianpur1975IndiaZoneIII.Name = "Kalianpur_1975_India_Zone_III";
            Kalianpur1975IndiaZoneIV.Name = "Kalianpur_1975_India_Zone_IV";
            Kalianpur1975UTMZone42N.Name = "Kalianpur_1975_UTM_Zone_42N";
            Kalianpur1975UTMZone43N.Name = "Kalianpur_1975_UTM_Zone_43N";
            Kalianpur1975UTMZone44N.Name = "Kalianpur_1975_UTM_Zone_44N";
            Kalianpur1975UTMZone45N.Name = "Kalianpur_1975_UTM_Zone_45N";
            Kalianpur1975UTMZone46N.Name = "Kalianpur_1975_UTM_Zone_46N";
            Kalianpur1975UTMZone47N.Name = "Kalianpur_1975_UTM_Zone_47N";

            Kalianpur1880IndiaZone0.GeographicInfo.Name = "GCS_Kalianpur_1880";
            Kalianpur1880IndiaZoneI.GeographicInfo.Name = "GCS_Kalianpur_1880";
            Kalianpur1880IndiaZoneIIa.GeographicInfo.Name = "GCS_Kalianpur_1880";
            Kalianpur1880IndiaZoneIIb.GeographicInfo.Name = "GCS_Kalianpur_1880";
            Kalianpur1880IndiaZoneIII.GeographicInfo.Name = "GCS_Kalianpur_1880";
            Kalianpur1880IndiaZoneIV.GeographicInfo.Name = "GCS_Kalianpur_1880";
            Kalianpur1937IndiaZoneIIb.GeographicInfo.Name = "GCS_Kalianpur_1937";
            Kalianpur1937UTMZone45N.GeographicInfo.Name = "GCS_Kalianpur_1937";
            Kalianpur1937UTMZone46N.GeographicInfo.Name = "GCS_Kalianpur_1937";
            Kalianpur1962IndiaZoneI.GeographicInfo.Name = "GCS_Kalianpur_1962";
            Kalianpur1962IndiaZoneIIa.GeographicInfo.Name = "GCS_Kalianpur_1962";
            Kalianpur1962UTMZone41N.GeographicInfo.Name = "GCS_Kalianpur_1962";
            Kalianpur1962UTMZone42N.GeographicInfo.Name = "GCS_Kalianpur_1962";
            Kalianpur1962UTMZone43N.GeographicInfo.Name = "GCS_Kalianpur_1962";
            Kalianpur1975IndiaZoneI.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975IndiaZoneIIa.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975IndiaZoneIIb.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975IndiaZoneIII.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975IndiaZoneIV.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975UTMZone42N.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975UTMZone43N.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975UTMZone44N.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975UTMZone45N.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975UTMZone46N.GeographicInfo.Name = "GCS_Kalianpur_1975";
            Kalianpur1975UTMZone47N.GeographicInfo.Name = "GCS_Kalianpur_1975";

            Kalianpur1880IndiaZone0.GeographicInfo.Datum.Name = "D_Kalianpur_1880";
            Kalianpur1880IndiaZoneI.GeographicInfo.Datum.Name = "D_Kalianpur_1880";
            Kalianpur1880IndiaZoneIIa.GeographicInfo.Datum.Name = "D_Kalianpur_1880";
            Kalianpur1880IndiaZoneIIb.GeographicInfo.Datum.Name = "D_Kalianpur_1880";
            Kalianpur1880IndiaZoneIII.GeographicInfo.Datum.Name = "D_Kalianpur_1880";
            Kalianpur1880IndiaZoneIV.GeographicInfo.Datum.Name = "D_Kalianpur_1880";
            Kalianpur1937IndiaZoneIIb.GeographicInfo.Datum.Name = "D_Kalianpur_1937";
            Kalianpur1937UTMZone45N.GeographicInfo.Datum.Name = "D_Kalianpur_1937";
            Kalianpur1937UTMZone46N.GeographicInfo.Datum.Name = "D_Kalianpur_1937";
            Kalianpur1962IndiaZoneI.GeographicInfo.Datum.Name = "D_Kalianpur_1962";
            Kalianpur1962IndiaZoneIIa.GeographicInfo.Datum.Name = "D_Kalianpur_1962";
            Kalianpur1962UTMZone41N.GeographicInfo.Datum.Name = "D_Kalianpur_1962";
            Kalianpur1962UTMZone42N.GeographicInfo.Datum.Name = "D_Kalianpur_1962";
            Kalianpur1962UTMZone43N.GeographicInfo.Datum.Name = "D_Kalianpur_1962";
            Kalianpur1975IndiaZoneI.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975IndiaZoneIIa.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975IndiaZoneIIb.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975IndiaZoneIII.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975IndiaZoneIV.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975UTMZone42N.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975UTMZone43N.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975UTMZone44N.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975UTMZone45N.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975UTMZone46N.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
            Kalianpur1975UTMZone47N.GeographicInfo.Datum.Name = "D_Kalianpur_1975";
        }

        #endregion
    }
}

#pragma warning restore 1591