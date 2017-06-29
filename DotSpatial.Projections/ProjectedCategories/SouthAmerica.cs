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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:26:29 PM
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
    /// SouthAmerica
    /// </summary>
    public class SouthAmerica : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo SouthAmericaAlbersEqualAreaConic;
        public readonly ProjectionInfo SouthAmericaEquidistantConic;
        public readonly ProjectionInfo SouthAmericaLambertConformalConic;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAmerica
        /// </summary>
        public SouthAmerica()
        {
            SouthAmericaAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=-5 +lat_2=-42 +lat_0=-32 +lon_0=-60 +x_0=0 +y_0=0 +ellps=aust_SA +units=m +no_defs ");
            SouthAmericaEquidistantConic = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=-5 +lat_2=-42 +x_0=0 +y_0=0 +ellps=aust_SA +units=m +no_defs ");
            SouthAmericaLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=-5 +lat_2=-42 +lat_0=-32 +lon_0=-60 +x_0=0 +y_0=0 +ellps=aust_SA +units=m +no_defs ");

            SouthAmericaAlbersEqualAreaConic.Name = "South_America_Albers_Equal_Area_Conic";
            SouthAmericaEquidistantConic.Name = "South_America_Equidistant_Conic";
            SouthAmericaLambertConformalConic.Name = "South_America_Lambert_Conformal_Conic";

            SouthAmericaAlbersEqualAreaConic.GeographicInfo.Name = "GCS_South_American_1969";
            SouthAmericaEquidistantConic.GeographicInfo.Name = "GCS_South_American_1969";
            SouthAmericaLambertConformalConic.GeographicInfo.Name = "GCS_South_American_1969";

            SouthAmericaAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_South_American_1969";
            SouthAmericaEquidistantConic.GeographicInfo.Datum.Name = "D_South_American_1969";
            SouthAmericaLambertConformalConic.GeographicInfo.Datum.Name = "D_South_American_1969";
        }

        #endregion
    }
}

#pragma warning restore 1591