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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:24:27 PM
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
    /// Europe
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo EMEP150KilometerGrid;
        public readonly ProjectionInfo EMEP50KilometerGrid;
        public readonly ProjectionInfo ETRS1989LAEA;
        public readonly ProjectionInfo ETRS1989LCC;
        public readonly ProjectionInfo EuropeAlbersEqualAreaConic;
        public readonly ProjectionInfo EuropeEquidistantConic;
        public readonly ProjectionInfo EuropeLambertConformalConic;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe
        /// </summary>
        public Europe()
        {
            EMEP150KilometerGrid = new ProjectionInfo();
            EMEP150KilometerGrid.ParseEsriString("PROJCS[\"EMEP_150_Kilometer_Grid\", GEOGCS[\"GCS_Sphere_EMEP\", DATUM[\"D_Sphere_EMEP\", SPHEROID[\"Sphere_EMEP\", 6370000.0, 0.0]], PRIMEM[\"Greenwich\", 0.0], UNIT[\"Degree\", 0.0174532925199433]], PROJECTION[\"Stereographic_North_Pole\"], PARAMETER[\"False_Easting\", 3.0], PARAMETER[\"False_Northing\", 37.0], PARAMETER[\"Central_Meridian\", -32.0], PARAMETER[\"Standard_Parallel_1\", 60.0], UNIT[\"150_Kilometers\", 150000.0]]");
            EMEP50KilometerGrid = new ProjectionInfo();
            EMEP50KilometerGrid.ParseEsriString("PROJCS[\"EMEP_50_Kilometer_Grid\", GEOGCS[\"GCS_Sphere_EMEP\", DATUM[\"D_Sphere_EMEP\", SPHEROID[\"Sphere_EMEP\", 6370000.0, 0.0]], PRIMEM[\"Greenwich\", 0.0], UNIT[\"Degree\", 0.0174532925199433]], PROJECTION[\"Stereographic_North_Pole\"], PARAMETER[\"False_Easting\", 8.0], PARAMETER[\"False_Northing\", 110.0], PARAMETER[\"Central_Meridian\", -32.0], PARAMETER[\"Standard_Parallel_1\", 60.0], UNIT[\"50_Kilometers\", 50000.0]]");
            ETRS1989LAEA = ProjectionInfo.FromProj4String("+proj=laea +lat_0=52 +lon_0=10 +x_0=4321000 +y_0=3210000 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989LCC = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=35 +lat_2=65 +lat_0=52 +lon_0=10 +x_0=4000000 +y_0=2800000 +ellps=GRS80 +units=m +no_defs ");
            EuropeAlbersEqualAreaConic = ProjectionInfo.FromProj4String("+proj=aea +lat_1=43 +lat_2=62 +lat_0=30 +lon_0=10 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");
            EuropeEquidistantConic = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=43 +lat_2=62 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");
            EuropeLambertConformalConic = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=43 +lat_2=62 +lat_0=30 +lon_0=10 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");

            EMEP50KilometerGrid.Name = "EMEP_50_Kilometer_Grid";
            ETRS1989LAEA.Name = "ETRS_1989_LAEA";
            ETRS1989LCC.Name = "ETRS_1989_LCC";
            EuropeAlbersEqualAreaConic.Name = "Europe_Albers_Equal_Area_Conic";
            EuropeEquidistantConic.Name = "Europe_Equidistant_Conic";
            EuropeLambertConformalConic.Name = "Europe_Lambert_Conformal_Conic";

            EMEP150KilometerGrid.GeographicInfo.Name = "GCS_Sphere_EMEP";
            EMEP50KilometerGrid.GeographicInfo.Name = "GCS_Sphere_EMEP";
            ETRS1989LAEA.GeographicInfo.Name = "GCS_ETRS_1989";
            ETRS1989LCC.GeographicInfo.Name = "GCS_ETRS_1989";
            EuropeAlbersEqualAreaConic.GeographicInfo.Name = "GCS_European_1950";
            EuropeEquidistantConic.GeographicInfo.Name = "GCS_European_1950";
            EuropeLambertConformalConic.GeographicInfo.Name = "GCS_European_1950";

            EMEP150KilometerGrid.GeographicInfo.Datum.Name = "D_Sphere_EMEP";
            EMEP50KilometerGrid.GeographicInfo.Datum.Name = "D_Sphere_EMEP";
            ETRS1989LAEA.GeographicInfo.Datum.Name = "D_ETRS_1989";
            ETRS1989LCC.GeographicInfo.Datum.Name = "D_ETRS_1989";
            EuropeAlbersEqualAreaConic.GeographicInfo.Datum.Name = "D_European_1950";
            EuropeEquidistantConic.GeographicInfo.Datum.Name = "D_European_1950";
            EuropeLambertConformalConic.GeographicInfo.Datum.Name = "D_European_1950";
        }

        #endregion
    }
}

#pragma warning restore 1591