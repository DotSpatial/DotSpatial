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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:13:38 PM
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
    /// WorldSpheroid
    /// </summary>
    public class WorldSpheroid : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Aitoffsphere;
        public readonly ProjectionInfo Behrmannsphere;
        public readonly ProjectionInfo Bonnesphere;
        public readonly ProjectionInfo CrasterParabolicsphere;
        public readonly ProjectionInfo CylindricalEqualAreasphere;
        public readonly ProjectionInfo EckertIIIsphere;
        public readonly ProjectionInfo EckertIIsphere;
        public readonly ProjectionInfo EckertIVsphere;
        public readonly ProjectionInfo EckertIsphere;
        public readonly ProjectionInfo EckertVIsphere;
        public readonly ProjectionInfo EckertVsphere;
        public readonly ProjectionInfo EquidistantConicsphere;
        public readonly ProjectionInfo EquidistantCylindricalsphere;
        public readonly ProjectionInfo FlatPolarQuarticsphere;
        public readonly ProjectionInfo GallStereographicsphere;
        public readonly ProjectionInfo HammerAitoffsphere;
        public readonly ProjectionInfo Loximuthalsphere;
        public readonly ProjectionInfo Mercatorsphere;
        public readonly ProjectionInfo MillerCylindricalsphere;
        public readonly ProjectionInfo Mollweidesphere;
        public readonly ProjectionInfo PlateCarreesphere;
        public readonly ProjectionInfo Polyconicsphere;
        public readonly ProjectionInfo QuarticAuthalicsphere;
        public readonly ProjectionInfo Robinsonsphere;
        public readonly ProjectionInfo Sinusoidalsphere;
        public readonly ProjectionInfo Timessphere;
        public readonly ProjectionInfo VanderGrintenIsphere;
        public readonly ProjectionInfo VerticalPerspectivesphere;
        public readonly ProjectionInfo WinkelIIsphere;
        public readonly ProjectionInfo WinkelIsphere;
        public readonly ProjectionInfo WinkelTripelNGSsphere;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of WorldSpheroid
        /// </summary>
        public WorldSpheroid()
        {
            Aitoffsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            Behrmannsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            Bonnesphere = ProjectionInfo.FromProj4String("+proj=bonne +lon_0=0 +lat_1=60 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            CrasterParabolicsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            CylindricalEqualAreasphere = ProjectionInfo.FromProj4String("+proj=cea +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIIIsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIIsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIVsphere = ProjectionInfo.FromProj4String("+proj=eck4 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EckertVIsphere = ProjectionInfo.FromProj4String("+proj=eck6 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EckertVsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            EquidistantConicsphere = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=60 +lat_2=60 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EquidistantCylindricalsphere = ProjectionInfo.FromProj4String("+proj=eqc +lat_ts=0 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            FlatPolarQuarticsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            GallStereographicsphere = ProjectionInfo.FromProj4String("+proj=gall +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            HammerAitoffsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            Loximuthalsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            Mercatorsphere = ProjectionInfo.FromProj4String("+proj=merc +lat_ts=0 +lon_0=0 +k=1.000000 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            MillerCylindricalsphere = ProjectionInfo.FromProj4String("+proj=mill +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +R_A +a=6371000 +b=6371000 +units=m +no_defs ");
            Mollweidesphere = ProjectionInfo.FromProj4String("+proj=moll +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            PlateCarreesphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            Polyconicsphere = ProjectionInfo.FromProj4String("+proj=poly +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            QuarticAuthalicsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            Robinsonsphere = ProjectionInfo.FromProj4String("+proj=robin +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            Sinusoidalsphere = ProjectionInfo.FromProj4String("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            Timessphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            VanderGrintenIsphere = ProjectionInfo.FromProj4String("+proj=vandg +lon_0=0 +x_0=0 +y_0=0 +R_A +a=6371000 +b=6371000 +units=m +no_defs ");
            VerticalPerspectivesphere = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WinkelIIsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            WinkelIsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");
            WinkelTripelNGSsphere = ProjectionInfo.FromProj4String("+a=6371000 +b=6371000 +units=m +no_defs ");

            Aitoffsphere.Name = "Sphere_Aitoff";
            Behrmannsphere.Name = "Sphere_Behrmann";
            Bonnesphere.Name = "Sphere_Bonne";
            CrasterParabolicsphere.Name = "Sphere_Craster_Parabolic";
            CylindricalEqualAreasphere.Name = "Sphere_Cylindrical_Equal_Area";
            EckertIsphere.Name = "Sphere_Eckert_I";
            EckertIIsphere.Name = "Sphere_Eckert_II";
            EckertIIIsphere.Name = "Sphere_Eckert_III";
            EckertIVsphere.Name = "Sphere_Eckert_IV";
            EckertVsphere.Name = "Sphere_Eckert_V";
            EckertVIsphere.Name = "Sphere_Eckert_VI";
            EquidistantConicsphere.Name = "Sphere_Equidistant_Conic";
            EquidistantCylindricalsphere.Name = "Sphere_Equidistant_Cylindrical";
            FlatPolarQuarticsphere.Name = "Sphere_Flat_Polar_Quartic";
            GallStereographicsphere.Name = "Sphere_Gall_Stereographic";
            HammerAitoffsphere.Name = "Sphere_Hammer_Aitoff";
            Loximuthalsphere.Name = "Sphere_Loximuthal";
            Mercatorsphere.Name = "Sphere_Mercator";
            MillerCylindricalsphere.Name = "Sphere_Miller_Cylindrical";
            Mollweidesphere.Name = "Sphere_Mollweide";
            PlateCarreesphere.Name = "Sphere_Plate_Carree";
            Polyconicsphere.Name = "Sphere_Polyconic";
            QuarticAuthalicsphere.Name = "Sphere_Quartic_Authalic";
            Robinsonsphere.Name = "Sphere_Robinson";
            Sinusoidalsphere.Name = "Sphere_Sinusoidal";
            Timessphere.Name = "Sphere_Times";
            VanderGrintenIsphere.Name = "Sphere_Van_der_Grinten_I";
            VerticalPerspectivesphere.Name = "Sphere_Vertical_Perspective";
            WinkelIsphere.Name = "Sphere_Winkel_II";
            WinkelTripelNGSsphere.Name = "Sphere_Winkel_Tripel_NGS";

            Aitoffsphere.GeographicInfo.Name = "GCS_Sphere";
            Behrmannsphere.GeographicInfo.Name = "GCS_Sphere";
            Bonnesphere.GeographicInfo.Name = "GCS_Sphere";
            CrasterParabolicsphere.GeographicInfo.Name = "GCS_Sphere";
            CylindricalEqualAreasphere.GeographicInfo.Name = "GCS_Sphere";
            EckertIsphere.GeographicInfo.Name = "GCS_Sphere";
            EckertIIsphere.GeographicInfo.Name = "GCS_Sphere";
            EckertIIIsphere.GeographicInfo.Name = "GCS_Sphere";
            EckertIVsphere.GeographicInfo.Name = "GCS_Sphere";
            EckertVsphere.GeographicInfo.Name = "GCS_Sphere";
            EckertVIsphere.GeographicInfo.Name = "GCS_Sphere";
            EquidistantConicsphere.GeographicInfo.Name = "GCS_Sphere";
            EquidistantCylindricalsphere.GeographicInfo.Name = "GCS_Sphere";
            FlatPolarQuarticsphere.GeographicInfo.Name = "GCS_Sphere";
            GallStereographicsphere.GeographicInfo.Name = "GCS_Sphere";
            HammerAitoffsphere.GeographicInfo.Name = "GCS_Sphere";
            Loximuthalsphere.GeographicInfo.Name = "GCS_Sphere";
            Mercatorsphere.GeographicInfo.Name = "GCS_Sphere";
            MillerCylindricalsphere.GeographicInfo.Name = "GCS_Sphere";
            Mollweidesphere.GeographicInfo.Name = "GCS_Sphere";
            PlateCarreesphere.GeographicInfo.Name = "GCS_Sphere";
            Polyconicsphere.GeographicInfo.Name = "GCS_Sphere";
            QuarticAuthalicsphere.GeographicInfo.Name = "GCS_Sphere";
            Robinsonsphere.GeographicInfo.Name = "GCS_Sphere";
            Sinusoidalsphere.GeographicInfo.Name = "GCS_Sphere";
            Timessphere.GeographicInfo.Name = "GCS_Sphere";
            VanderGrintenIsphere.GeographicInfo.Name = "GCS_Sphere";
            VerticalPerspectivesphere.GeographicInfo.Name = "GCS_WGS_1984";
            WinkelIsphere.GeographicInfo.Name = "GCS_Sphere";
            WinkelTripelNGSsphere.GeographicInfo.Name = "GCS_Sphere";

            Aitoffsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Behrmannsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Bonnesphere.GeographicInfo.Datum.Name = "D_Sphere";
            CrasterParabolicsphere.GeographicInfo.Datum.Name = "D_Sphere";
            CylindricalEqualAreasphere.GeographicInfo.Datum.Name = "D_Sphere";
            EckertIsphere.GeographicInfo.Datum.Name = "D_Sphere";
            EckertIIsphere.GeographicInfo.Datum.Name = "D_Sphere";
            EckertIIIsphere.GeographicInfo.Datum.Name = "D_Sphere";
            EckertIVsphere.GeographicInfo.Datum.Name = "D_Sphere";
            EckertVsphere.GeographicInfo.Datum.Name = "D_Sphere";
            EckertVIsphere.GeographicInfo.Datum.Name = "D_Sphere";
            EquidistantConicsphere.GeographicInfo.Datum.Name = "D_Sphere";
            EquidistantCylindricalsphere.GeographicInfo.Datum.Name = "D_Sphere";
            FlatPolarQuarticsphere.GeographicInfo.Datum.Name = "D_Sphere";
            GallStereographicsphere.GeographicInfo.Datum.Name = "D_Sphere";
            HammerAitoffsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Loximuthalsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Mercatorsphere.GeographicInfo.Datum.Name = "D_Sphere";
            MillerCylindricalsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Mollweidesphere.GeographicInfo.Datum.Name = "D_Sphere";
            PlateCarreesphere.GeographicInfo.Datum.Name = "D_Sphere";
            Polyconicsphere.GeographicInfo.Datum.Name = "D_Sphere";
            QuarticAuthalicsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Robinsonsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Sinusoidalsphere.GeographicInfo.Datum.Name = "D_Sphere";
            Timessphere.GeographicInfo.Datum.Name = "D_Sphere";
            VanderGrintenIsphere.GeographicInfo.Datum.Name = "D_Sphere";
            VerticalPerspectivesphere.GeographicInfo.Datum.Name = "D_WGS_1984";
            WinkelIsphere.GeographicInfo.Datum.Name = "D_Sphere";
            WinkelTripelNGSsphere.GeographicInfo.Datum.Name = "D_Sphere";
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        #endregion
    }
}

#pragma warning restore 1591