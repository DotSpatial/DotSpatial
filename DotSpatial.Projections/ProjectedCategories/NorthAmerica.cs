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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:25:11 PM
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
    /// NorthAmerica
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AlaskaAlbersEqualAreaConic;
        public readonly ProjectionInfo CanadaAlbersEqualAreaConic;
        public readonly ProjectionInfo CanadaLambertConformalConic;
        public readonly ProjectionInfo HawaiiAlbersEqualAreaConic;
        public readonly ProjectionInfo NorthAmericaAlbersEqualAreaConic;
        public readonly ProjectionInfo NorthAmericaEquidistantConic;
        public readonly ProjectionInfo NorthAmericaLambertConformalConic;
        public readonly ProjectionInfo USAContiguousAlbersEqualAreaConic;
        public readonly ProjectionInfo USAContiguousAlbersEqualAreaConicUSGS;
        public readonly ProjectionInfo USAContiguousEquidistantConic;
        public readonly ProjectionInfo USAContiguousLambertConformalConic;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica
        /// </summary>
        public NorthAmerica()
        {
            AlaskaAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=55 +lat_2=65 +lat_0=50 +lon_0=-154 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            CanadaAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=50 +lat_2=70 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            CanadaLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=50 +lat_2=70 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            HawaiiAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=8 +lat_2=18 +lat_0=13 +lon_0=-157 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NorthAmericaAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=20 +lat_2=60 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NorthAmericaEquidistantConic = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=20 +lat_2=60 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NorthAmericaLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=20 +lat_2=60 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=37.5 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousAlbersEqualAreaConicUSGS = ProjectionInfo.FromProj4String("+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousEquidistantConic = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=33 +lat_2=45 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=33 +lat_2=45 +lat_0=39 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");

            AlaskaAlbersEqualAreaConic.Name = "Alaska_Albers_Equal_Area_Conic";
            CanadaAlbersEqualAreaConic.Name = "Canada_Albers_Equal_Area_Conic";
            CanadaLambertConformalConic.Name = "Canada_Lambert_Conformal_Conic";
            HawaiiAlbersEqualAreaConic.Name = "Hawaii_Albers_Equal_Area_Conic";
            NorthAmericaAlbersEqualAreaConic.Name = "North_America_Albers_Equal_Area_Conic";
            NorthAmericaEquidistantConic.Name = "North_America_Equidistant_Conic";
            NorthAmericaLambertConformalConic.Name = "North_America_Lambert_Conformal_Conic";
            USAContiguousAlbersEqualAreaConic.Name = "USA_Contiguous_Albers_Equal_Area_Conic";
            USAContiguousAlbersEqualAreaConicUSGS.Name = "USA_Contiguous_Albers_Equal_Area_Conic_USGS_version";
            USAContiguousEquidistantConic.Name = "USA_Contiguous_Equidistant_Conic";
            USAContiguousLambertConformalConic.Name = "USA_Contiguous_Lambert_Conformal_Conic";

            AlaskaAlbersEqualAreaConic.GeographicInfo.Name = "GCS_North_American_1983";
            CanadaAlbersEqualAreaConic.GeographicInfo.Name = "GCS_North_American_1983";
            CanadaLambertConformalConic.GeographicInfo.Name = "GCS_North_American_1983";
            HawaiiAlbersEqualAreaConic.GeographicInfo.Name = "GCS_North_American_1983";
            NorthAmericaAlbersEqualAreaConic.GeographicInfo.Name = "GCS_North_American_1983";
            NorthAmericaEquidistantConic.GeographicInfo.Name = "GCS_North_American_1983";
            NorthAmericaLambertConformalConic.GeographicInfo.Name = "GCS_North_American_1983";
            USAContiguousAlbersEqualAreaConic.GeographicInfo.Name = "GCS_North_American_1983";
            USAContiguousAlbersEqualAreaConicUSGS.GeographicInfo.Name = "GCS_North_American_1983";
            USAContiguousEquidistantConic.GeographicInfo.Name = "GCS_North_American_1983";
            USAContiguousLambertConformalConic.GeographicInfo.Name = "GCS_North_American_1983";

            AlaskaAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            CanadaAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            CanadaLambertConformalConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            HawaiiAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            NorthAmericaAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            NorthAmericaEquidistantConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            NorthAmericaLambertConformalConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            USAContiguousAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            USAContiguousAlbersEqualAreaConicUSGS.GeographicInfo.Datum.Name = "D_North_American_1983";
            USAContiguousEquidistantConic.GeographicInfo.Datum.Name = "D_North_American_1983";
            USAContiguousLambertConformalConic.GeographicInfo.Datum.Name = "D_North_American_1983";
        }

        #endregion
    }
}

#pragma warning restore 1591