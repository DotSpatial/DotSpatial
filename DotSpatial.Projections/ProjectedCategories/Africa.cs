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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:00 PM
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
    /// Africa
    /// </summary>
    public class Africa : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AfricaAlbersEqualAreaConic;
        public readonly ProjectionInfo AfricaEquidistantConic;
        public readonly ProjectionInfo AfricaLambertConformalConic;
        public readonly ProjectionInfo AfricaSinusoidal;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Africa
        /// </summary>
        public Africa()
        {
            AfricaAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=20 +lat_2=-23 +lat_0=0 +lon_0=25 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AfricaEquidistantConic = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=20 +lat_2=-23 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AfricaLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=20 +lat_2=-23 +lat_0=0 +lon_0=25 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AfricaSinusoidal = ProjectionInfo.FromProj4String("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

            AfricaAlbersEqualAreaConic.Name = "Africa_Albers_Equal_Area_Conic";
            AfricaEquidistantConic.Name = "Africa_Equidistant_Conic";
            AfricaLambertConformalConic.Name = "Africa_Lambert_Conformal_Conic";
            AfricaSinusoidal.Name = "Africa_Sinusoidal";

            AfricaAlbersEqualAreaConic.GeographicInfo.Name = "GCS_WGS_1984";
            AfricaEquidistantConic.GeographicInfo.Name = "GCS_WGS_1984";
            AfricaLambertConformalConic.GeographicInfo.Name = "GCS_WGS_1984";
            AfricaSinusoidal.GeographicInfo.Name = "GCS_WGS_1984";

            AfricaAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AfricaEquidistantConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AfricaLambertConformalConic.GeographicInfo.Datum.Name = "D_WGS_1984";
            AfricaSinusoidal.GeographicInfo.Datum.Name = "D_WGS_1984";
        }

        #endregion
    }
}

#pragma warning restore 1591