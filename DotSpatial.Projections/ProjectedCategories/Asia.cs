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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:38 PM
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
    /// Asia
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AsiaLambertConformalConic;
        public readonly ProjectionInfo AsiaNorthAlbersEqualAreaConic;
        public readonly ProjectionInfo AsiaNorthEquidistantConic;
        public readonly ProjectionInfo AsiaNorthLambertConformalConic;
        public readonly ProjectionInfo AsiaSouthAlbersEqualAreaConic;
        public readonly ProjectionInfo AsiaSouthEquidistantConic;
        public readonly ProjectionInfo AsiaSouthLambertConformalConic;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia
        /// </summary>
        public Asia()
        {
            AsiaLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=30 +lat_2=62 +lat_0=0 +lon_0=105 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaNorthAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=15 +lat_2=65 +lat_0=30 +lon_0=95 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaNorthEquidistantConic = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=15 +lat_2=65 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaNorthLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=15 +lat_2=65 +lat_0=30 +lon_0=95 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaSouthAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=7 +lat_2=-32 +lat_0=-15 +lon_0=125 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaSouthEquidistantConic = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=7 +lat_2=-32 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaSouthLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=7 +lat_2=-32 +lat_0=-15 +lon_0=125 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

            AsiaLambertConformalConic.Name = "Asia_Lambert_Conformal_Conic";
            AsiaNorthAlbersEqualAreaConic.Name = "Asia_North_Albers_Equal_Area_Conic";
            AsiaNorthEquidistantConic.Name = "Asia_North_Equidistant_Conic";
            AsiaNorthLambertConformalConic.Name = "Asia_North_Lambert_Conformal_Conic";
            AsiaSouthAlbersEqualAreaConic.Name = "Asia_South_Albers_Equal_Area_Conic";
            AsiaSouthEquidistantConic.Name = "Asia_South_Equidistant_Conic";
            AsiaSouthLambertConformalConic.Name = "Asia_South_Lambert_Conformal_Conic";

            AsiaLambertConformalConic.GeographicInfo.Name = "GCS_WGS_1984";
            AsiaNorthAlbersEqualAreaConic.GeographicInfo.Name = "GCS_WGS_1984";
            AsiaNorthEquidistantConic.GeographicInfo.Name = "GCS_WGS_1984";
            AsiaNorthLambertConformalConic.GeographicInfo.Name = "GCS_WGS_1984";
            AsiaSouthAlbersEqualAreaConic.GeographicInfo.Name = "GCS_WGS_1984";
            AsiaSouthEquidistantConic.GeographicInfo.Name = "GCS_WGS_1984";
            AsiaSouthLambertConformalConic.GeographicInfo.Name = "GCS_WGS_1984";

            AsiaLambertConformalConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AsiaNorthAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AsiaNorthEquidistantConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AsiaNorthLambertConformalConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AsiaSouthAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AsiaSouthEquidistantConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AsiaSouthLambertConformalConic.GeographicInfo.Datum.Name = "D_WGS_1984";
        }

        #endregion
    }
}

#pragma warning restore 1591