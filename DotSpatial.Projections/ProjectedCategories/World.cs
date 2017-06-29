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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:12:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// Jiri Kadlec         | 11/20/2010 |  Updated the proj4 string definition of Web Mercator Auxiliary Sphere
// ********************************************************************************************************

#pragma warning disable 1591

using DotSpatial.Projections.Transforms;

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// World
    /// </summary>
    public class World : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Aitoffworld;
        public readonly ProjectionInfo Behrmannworld;
        public readonly ProjectionInfo Bonneworld;
        public readonly ProjectionInfo CrasterParabolicworld;
        public readonly ProjectionInfo Cubeworld;
        public readonly ProjectionInfo CylindricalEqualAreaworld;
        public readonly ProjectionInfo EckertIIIworld;
        public readonly ProjectionInfo EckertIIworld;
        public readonly ProjectionInfo EckertIVworld;
        public readonly ProjectionInfo EckertIworld;
        public readonly ProjectionInfo EckertVIworld;
        public readonly ProjectionInfo EckertVworld;
        public readonly ProjectionInfo EquidistantConicworld;
        public readonly ProjectionInfo EquidistantCylindricalworld;
        public readonly ProjectionInfo FlatPolarQuarticworld;
        public readonly ProjectionInfo Fullerworld;
        public readonly ProjectionInfo GallStereographicworld;
        public readonly ProjectionInfo HammerAitoffworld;
        public readonly ProjectionInfo Loximuthalworld;
        public readonly ProjectionInfo Mercatorworld;

        public readonly ProjectionInfo MillerCylindricalworld;
        public readonly ProjectionInfo Mollweideworld;
        public readonly ProjectionInfo PlateCarreeworld;
        public readonly ProjectionInfo Polyconicworld;
        public readonly ProjectionInfo QuarticAuthalicworld;
        public readonly ProjectionInfo Robinsonworld;
        public readonly ProjectionInfo Sinusoidalworld;
        public readonly ProjectionInfo TheWorldfromSpace;
        public readonly ProjectionInfo Timesworld;
        public readonly ProjectionInfo VanderGrintenIworld;
        public readonly ProjectionInfo VerticalPerspectiveworld;
        public readonly ProjectionInfo WebMercator;
        public readonly ProjectionInfo WinkelIIworld;
        public readonly ProjectionInfo WinkelIworld;
        public readonly ProjectionInfo WinkelTripelNGSworld;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of World
        /// </summary>
        public World()
        {
            Aitoffworld = ProjectionInfo.FromProj4String("+proj=aitoff +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Behrmannworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Bonneworld = ProjectionInfo.FromProj4String("+proj=bonne +lon_0=0 +lat_1=60 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            CrasterParabolicworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Cubeworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            CylindricalEqualAreaworld = ProjectionInfo.FromProj4String("+proj=cea +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIIIworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIIworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIVworld = ProjectionInfo.FromProj4String("+proj=eck4 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertVIworld = ProjectionInfo.FromProj4String("+proj=eck6 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertVworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EquidistantConicworld = ProjectionInfo.FromProj4String("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=60 +lat_2=60 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EquidistantCylindricalworld = ProjectionInfo.FromProj4String("+proj=eqc +lat_ts=0 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            FlatPolarQuarticworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Fullerworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            GallStereographicworld = ProjectionInfo.FromProj4String("+proj=gall +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            HammerAitoffworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Loximuthalworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Mercatorworld = ProjectionInfo.FromProj4String("+proj=merc +lat_ts=0 +lon_0=0 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            MillerCylindricalworld = ProjectionInfo.FromProj4String("+proj=mill +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +R_A +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Mollweideworld = ProjectionInfo.FromProj4String("+proj=moll +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            PlateCarreeworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Polyconicworld = ProjectionInfo.FromProj4String("+proj=poly +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            QuarticAuthalicworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Robinsonworld = ProjectionInfo.FromProj4String("+proj=robin +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Sinusoidalworld = ProjectionInfo.FromProj4String("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            TheWorldfromSpace = ProjectionInfo.FromProj4String("+proj=ortho +lat_0=42.5333333333 +lon_0=-72.5333333334 +x_0=0 +y_0=0 +a=6370997 +b=6370997 +units=m +no_defs ");
            Timesworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            VanderGrintenIworld = ProjectionInfo.FromProj4String("+proj=vandg +lon_0=0 +x_0=0 +y_0=0 +R_A +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            VerticalPerspectiveworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

            //JIRI -- Changed the web mercator projection definition
            WebMercator = ProjectionInfo.FromProj4String("+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0.0 +k=1.0 +units=m +nadgrids=@null +no_defs ");
            WebMercator.Transform = new MercatorAuxiliarySphere();
            WebMercator.ScaleFactor = 1;
            WebMercator.AuxiliarySphereType = AuxiliarySphereType.SemimajorAxis;
            WebMercator.GeographicInfo.Datum.Spheroid = new Spheroid(WebMercator.GeographicInfo.Datum.Spheroid.EquatorialRadius);
            WebMercator.Transform.Init(WebMercator);
            //ITransform originalTransform = WebMercator.Transform;
            //WebMercator.GeographicInfo.Datum.DatumType = DatumType.WGS84; //web mercator has a WGS84 datum type
            //WebMercator.Transform = originalTransform; //reset the transform

            WinkelIIworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WinkelIworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WinkelTripelNGSworld = ProjectionInfo.FromProj4String("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

            Aitoffworld.Name = "World_Aitoff";
            Behrmannworld.Name = "World_Behrmann";
            Bonneworld.Name = "World_Bonne";
            CrasterParabolicworld.Name = "World_Craster_Parabolic";
            Cubeworld.Name = "World_Cube";
            CylindricalEqualAreaworld.Name = "World_Cylindrical_Equal_Area";
            EckertIIIworld.Name = "World_Eckert_III";
            EckertIIworld.Name = "World_Eckert_II";
            EckertIVworld.Name = "World_Eckert_IV";
            EckertIworld.Name = "World_Eckert_I";
            EckertVIworld.Name = "World_Eckert_VI";
            EckertVworld.Name = "World_Eckert_V";
            EquidistantConicworld.Name = "World_Equidistant_Conic";
            EquidistantCylindricalworld.Name = "World_Equidistant_Cylindrical";
            FlatPolarQuarticworld.Name = "World_Flat_Polar_Quartic";
            Fullerworld.Name = "World_Fuller";
            GallStereographicworld.Name = "World_Gall_Stereographic";
            HammerAitoffworld.Name = "World_Hammer_Aitoff";
            Loximuthalworld.Name = "World_Loximuthal";
            Mercatorworld.Name = "World_Mercator";
            MillerCylindricalworld.Name = "World_Miller_Cylindrical";
            Mollweideworld.Name = "World_Mollweide";
            PlateCarreeworld.Name = "World_Plate_Carree";
            Polyconicworld.Name = "World_Polyconic";
            QuarticAuthalicworld.Name = "World_Quartic_Authalic";
            Robinsonworld.Name = "World_Robinson";
            Sinusoidalworld.Name = "World_Sinusoidal";
            TheWorldfromSpace.Name = "The_World_From_Space";
            Timesworld.Name = "World_Times";
            VanderGrintenIworld.Name = "World_Van_der_Grinten_I";
            VerticalPerspectiveworld.Name = "World_Vertical_Perspective";
            //name changed by JK to match the esri string
            //WebMercator.Name = "WGS_1984_Web_Mercator";
            WebMercator.Name = "WGS_1984_Web_Mercator_Auxiliary_Sphere";
            WinkelIworld.Name = "World_Winkel_II";
            WinkelTripelNGSworld.Name = "World_Winkel_Tripel_NGS";

            Aitoffworld.GeographicInfo.Name = "GCS_WGS_1984";
            Behrmannworld.GeographicInfo.Name = "GCS_WGS_1984";
            Bonneworld.GeographicInfo.Name = "GCS_WGS_1984";
            CrasterParabolicworld.GeographicInfo.Name = "GCS_WGS_1984";
            Cubeworld.GeographicInfo.Name = "GCS_WGS_1984";
            CylindricalEqualAreaworld.GeographicInfo.Name = "GCS_WGS_1984";
            EckertIIIworld.GeographicInfo.Name = "GCS_WGS_1984";
            EckertIIworld.GeographicInfo.Name = "GCS_WGS_1984";
            EckertIVworld.GeographicInfo.Name = "GCS_WGS_1984";
            EckertIworld.GeographicInfo.Name = "GCS_WGS_1984";
            EckertVIworld.GeographicInfo.Name = "GCS_WGS_1984";
            EckertVworld.GeographicInfo.Name = "GCS_WGS_1984";
            EquidistantConicworld.GeographicInfo.Name = "GCS_WGS_1984";
            EquidistantCylindricalworld.GeographicInfo.Name = "GCS_WGS_1984";
            FlatPolarQuarticworld.GeographicInfo.Name = "GCS_WGS_1984";
            Fullerworld.GeographicInfo.Name = "GCS_WGS_1984";
            GallStereographicworld.GeographicInfo.Name = "GCS_WGS_1984";
            HammerAitoffworld.GeographicInfo.Name = "GCS_WGS_1984";
            Loximuthalworld.GeographicInfo.Name = "GCS_WGS_1984";
            Mercatorworld.GeographicInfo.Name = "GCS_WGS_1984";
            MillerCylindricalworld.GeographicInfo.Name = "GCS_WGS_1984";
            Mollweideworld.GeographicInfo.Name = "GCS_WGS_1984";
            PlateCarreeworld.GeographicInfo.Name = "GCS_WGS_1984";
            Polyconicworld.GeographicInfo.Name = "GCS_WGS_1984";
            QuarticAuthalicworld.GeographicInfo.Name = "GCS_WGS_1984";
            Robinsonworld.GeographicInfo.Name = "GCS_WGS_1984";
            Sinusoidalworld.GeographicInfo.Name = "GCS_WGS_1984";
            TheWorldfromSpace.GeographicInfo.Name = "GCS_Sphere_ARC_INFO";
            Timesworld.GeographicInfo.Name = "GCS_WGS_1984";
            VanderGrintenIworld.GeographicInfo.Name = "GCS_WGS_1984";
            VerticalPerspectiveworld.GeographicInfo.Name = "GCS_WGS_1984";
            WinkelIworld.GeographicInfo.Name = "GCS_WGS_1984";
            WinkelTripelNGSworld.GeographicInfo.Name = "GCS_WGS_1984";
            //Jiri Kadlec - changed the 'web mercator' geographic info name
            //WebMercator.GeographicInfo.Name = "GCS_WGS_1984_Major_Auxiliary_Sphere";
            WebMercator.GeographicInfo.Name = "GCS_WGS_1984";

            Aitoffworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Behrmannworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Bonneworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            CrasterParabolicworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Cubeworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            CylindricalEqualAreaworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EckertIIIworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EckertIIworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EckertIVworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EckertIworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EckertVIworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EckertVworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EquidistantConicworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            EquidistantCylindricalworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            FlatPolarQuarticworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Fullerworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            GallStereographicworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            HammerAitoffworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Loximuthalworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Mercatorworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            MillerCylindricalworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Mollweideworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            PlateCarreeworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Polyconicworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            QuarticAuthalicworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Robinsonworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            Sinusoidalworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            TheWorldfromSpace.GeographicInfo.Datum.Name = "D_Sphere_ARC_INFO";
            Timesworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            VanderGrintenIworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            VerticalPerspectiveworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            WinkelIworld.GeographicInfo.Datum.Name = "D_WGS_1984";
            WinkelTripelNGSworld.GeographicInfo.Datum.Name = "D_WGS_1984";

            //Jiri Kadlec - Changed the WebMercator datum name to match the EsriString
            //WebMercator.GeographicInfo.Datum.Name = "D_WGS_1984_Major_Auxiliary_Sphere";
            WebMercator.GeographicInfo.Datum.Name = "D_WGS_1984";
        }

        #endregion
    }
}

#pragma warning restore 1591