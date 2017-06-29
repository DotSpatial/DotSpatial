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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:06:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// Australia
    /// </summary>
    public class Australia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AustralianGeodeticDatum1966;
        public readonly ProjectionInfo AustralianGeodeticDatum1984;
        public readonly ProjectionInfo ChathamIslands1979;
        public readonly ProjectionInfo GeocentricDatumofAustralia1994;
        public readonly ProjectionInfo NZGD2000;
        public readonly ProjectionInfo NewZealandGeodeticDatum1949;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Australia
        /// </summary>
        public Australia()
        {
            AustralianGeodeticDatum1966 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=aust_SA +no_defs ");
            AustralianGeodeticDatum1984 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=aust_SA +no_defs ");
            ChathamIslands1979 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            GeocentricDatumofAustralia1994 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            NewZealandGeodeticDatum1949 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            NZGD2000 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");

            AustralianGeodeticDatum1966.GeographicInfo.Name = "GCS_Australian_1966";
            AustralianGeodeticDatum1984.GeographicInfo.Name = "GCS_Australian_1984";
            ChathamIslands1979.GeographicInfo.Name = "GCS_Chatham_Islands_1979";
            GeocentricDatumofAustralia1994.GeographicInfo.Name = "GCS_GDA_1994";
            NewZealandGeodeticDatum1949.GeographicInfo.Name = "GCS_New_Zealand_1949";
            NZGD2000.GeographicInfo.Name = "GCS_NZGD_2000";

            AustralianGeodeticDatum1966.GeographicInfo.Datum.Name = "D_Australian_1966";
            AustralianGeodeticDatum1984.GeographicInfo.Datum.Name = "D_Australian_1984";
            ChathamIslands1979.GeographicInfo.Datum.Name = "D_Chatham_Islands_1979";
            GeocentricDatumofAustralia1994.GeographicInfo.Datum.Name = "D_GDA_1994";
            NewZealandGeodeticDatum1949.GeographicInfo.Datum.Name = "D_New_Zealand_1949";
            NZGD2000.GeographicInfo.Datum.Name = "D_NZGD_2000";
        }

        #endregion
    }
}

#pragma warning restore 1591