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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:55:21 PM
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
    /// Polar
    /// </summary>
    public class Polar : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NorthPoleAzimuthalEquidistant;
        public readonly ProjectionInfo NorthPoleGnomonic;
        public readonly ProjectionInfo NorthPoleLambertAzimuthalEqualArea;
        public readonly ProjectionInfo NorthPoleOrthographic;
        public readonly ProjectionInfo NorthPoleStereographic;
        public readonly ProjectionInfo Perroud1950TerreAdeliePolarStereographic;
        public readonly ProjectionInfo Petrels1972TerreAdeliePolarStereographic;
        public readonly ProjectionInfo SouthPoleAzimuthalEquidistant;
        public readonly ProjectionInfo SouthPoleGnomonic;
        public readonly ProjectionInfo SouthPoleLambertAzimuthalEqualArea;
        public readonly ProjectionInfo SouthPoleOrthographic;
        public readonly ProjectionInfo SouthPoleStereographic;
        public readonly ProjectionInfo UPSNorth;
        public readonly ProjectionInfo UPSSouth;
        public readonly ProjectionInfo WGS1984AntarcticPolarStereographic;
        public readonly ProjectionInfo WGS1984AustralianAntarcticLambert;
        public readonly ProjectionInfo WGS1984AustralianAntarcticPolarStereographic;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Polar
        /// </summary>
        public Polar()
        {
            NorthPoleAzimuthalEquidistant = ProjectionInfo.FromProj4String("+proj=aeqd +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleGnomonic = ProjectionInfo.FromProj4String("+proj=gnom +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleLambertAzimuthalEqualArea = ProjectionInfo.FromProj4String("+proj=laea +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleOrthographic = ProjectionInfo.FromProj4String("+proj=ortho +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleStereographic = ProjectionInfo.FromProj4String("+proj=stere +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Perroud1950TerreAdeliePolarStereographic = ProjectionInfo.FromProj4String("+ellps=intl +units=m +no_defs ");
            Petrels1972TerreAdeliePolarStereographic = ProjectionInfo.FromProj4String("+ellps=intl +units=m +no_defs ");
            SouthPoleAzimuthalEquidistant = ProjectionInfo.FromProj4String("+proj=aeqd +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleGnomonic = ProjectionInfo.FromProj4String("+proj=gnom +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleLambertAzimuthalEqualArea = ProjectionInfo.FromProj4String("+proj=laea +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleOrthographic = ProjectionInfo.FromProj4String("+proj=ortho +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleStereographic = ProjectionInfo.FromProj4String("+proj=stere +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            UPSNorth = ProjectionInfo.FromProj4String("+proj=stere +lat_0=90 +lon_0=0 +k=.994 +x_0=2000000 +y_0=2000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            UPSSouth = ProjectionInfo.FromProj4String("+proj=stere +lat_0=-90 +lon_0=0 +k=.994 +x_0=2000000 +y_0=2000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984AntarcticPolarStereographic = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984AustralianAntarcticLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=-68.5 +lat_2=-74.5 +lat_0=-50 +lon_0=70 +x_0=6000000 +y_0=6000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984AustralianAntarcticPolarStereographic = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

            NorthPoleAzimuthalEquidistant.Name = "North_Pole_Azimuthal_Equidistant";
            NorthPoleGnomonic.Name = "North_Pole_Gnomonic";
            NorthPoleLambertAzimuthalEqualArea.Name = "North_Pole_Lambert_Azimuthal_Equal_Area";
            NorthPoleOrthographic.Name = "North_Pole_Orthographic";
            NorthPoleStereographic.Name = "North_Pole_Stereographic";
            Perroud1950TerreAdeliePolarStereographic.Name = "Perroud_1950_Terre_Adelie_Polar_Stereographic";
            Petrels1972TerreAdeliePolarStereographic.Name = "Petrels_1972_Terre_Adelie_Polar_Stereographic";
            SouthPoleAzimuthalEquidistant.Name = "South_Pole_Azimuthal_Equidistant";
            SouthPoleGnomonic.Name = "South_Pole_Gnomonic";
            SouthPoleLambertAzimuthalEqualArea.Name = "South_Pole_Lambert_Azimuthal_Equal_Area";
            SouthPoleOrthographic.Name = "South_Pole_Orthographic";
            SouthPoleStereographic.Name = "South_Pole_Stereographic";
            UPSNorth.Name = "UPS_North";
            UPSSouth.Name = "UPS_South";
            WGS1984AntarcticPolarStereographic.Name = "WGS_1984_Antarctic_Polar_Stereographic";
            WGS1984AustralianAntarcticLambert.Name = "WGS_1984_Australian_Antarctic_Lambert";
            WGS1984AustralianAntarcticPolarStereographic.Name = "WGS_1984_Australian_Antarctic_Polar_Stereographic";

            NorthPoleAzimuthalEquidistant.GeographicInfo.Name = "GCS_WGS_1984";
            NorthPoleGnomonic.GeographicInfo.Name = "GCS_WGS_1984";
            NorthPoleLambertAzimuthalEqualArea.GeographicInfo.Name = "GCS_WGS_1984";
            NorthPoleOrthographic.GeographicInfo.Name = "GCS_WGS_1984";
            NorthPoleStereographic.GeographicInfo.Name = "GCS_WGS_1984";
            Perroud1950TerreAdeliePolarStereographic.GeographicInfo.Name = "GCS_Pointe_Geologie_Perroud_1950";
            Petrels1972TerreAdeliePolarStereographic.GeographicInfo.Name = "GCS_Petrels_1972";
            SouthPoleAzimuthalEquidistant.GeographicInfo.Name = "GCS_WGS_1984";
            SouthPoleGnomonic.GeographicInfo.Name = "GCS_WGS_1984";
            SouthPoleLambertAzimuthalEqualArea.GeographicInfo.Name = "GCS_WGS_1984";
            SouthPoleOrthographic.GeographicInfo.Name = "GCS_WGS_1984";
            SouthPoleStereographic.GeographicInfo.Name = "GCS_WGS_1984";
            UPSNorth.GeographicInfo.Name = "GCS_WGS_1984";
            UPSSouth.GeographicInfo.Name = "GCS_WGS_1984";
            WGS1984AntarcticPolarStereographic.GeographicInfo.Name = "GCS_WGS_1984";
            WGS1984AustralianAntarcticLambert.GeographicInfo.Name = "GCS_WGS_1984";
            WGS1984AustralianAntarcticPolarStereographic.GeographicInfo.Name = "GCS_WGS_1984";

            NorthPoleAzimuthalEquidistant.GeographicInfo.Datum.Name = "D_WGS_1984";
            NorthPoleGnomonic.GeographicInfo.Datum.Name = "D_WGS_1984";
            NorthPoleLambertAzimuthalEqualArea.GeographicInfo.Datum.Name = "D_WGS_1984";
            NorthPoleOrthographic.GeographicInfo.Datum.Name = "D_WGS_1984";
            NorthPoleStereographic.GeographicInfo.Datum.Name = "D_WGS_1984";
            Perroud1950TerreAdeliePolarStereographic.GeographicInfo.Datum.Name = "D_Pointe_Geologie_Perroud_1950";
            Petrels1972TerreAdeliePolarStereographic.GeographicInfo.Datum.Name = "D_Petrels_1972";
            SouthPoleAzimuthalEquidistant.GeographicInfo.Datum.Name = "D_WGS_1984";
            SouthPoleGnomonic.GeographicInfo.Datum.Name = "D_WGS_1984";
            SouthPoleLambertAzimuthalEqualArea.GeographicInfo.Datum.Name = "D_WGS_1984";
            SouthPoleOrthographic.GeographicInfo.Datum.Name = "D_WGS_1984";
            SouthPoleStereographic.GeographicInfo.Datum.Name = "D_WGS_1984";
            UPSNorth.GeographicInfo.Datum.Name = "D_WGS_1984";
            UPSSouth.GeographicInfo.Datum.Name = "D_WGS_1984";
            WGS1984AntarcticPolarStereographic.GeographicInfo.Datum.Name = "D_WGS_1984";
            WGS1984AustralianAntarcticLambert.GeographicInfo.Datum.Name = "D_WGS_1984";
            WGS1984AustralianAntarcticPolarStereographic.GeographicInfo.Datum.Name = "D_WGS_1984";
        }

        #endregion
    }
}

#pragma warning restore 1591