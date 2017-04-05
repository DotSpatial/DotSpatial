// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
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
    /// This class contains predefined CoordinateSystems for Polar.
    /// </summary>
    public class Polar : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NorthPoleAzimuthalEquidistant;
        public readonly ProjectionInfo NorthPoleGnomonic;
        public readonly ProjectionInfo NorthPoleLambertAzimuthalEqualArea;
        public readonly ProjectionInfo NorthPoleOrthographic;
        public readonly ProjectionInfo NorthPoleStereographic;
        public readonly ProjectionInfo NSIDCEASEGridGlobal;
        public readonly ProjectionInfo NSIDCEASEGridNorth;
        public readonly ProjectionInfo NSIDCEASEGridSouth;
        public readonly ProjectionInfo NSIDCSeaIcePolarStereographicNorth;
        public readonly ProjectionInfo NSIDCSeaIcePolarStereographicSouth;
        // public readonly ProjectionInfo Perroud1950TerreAdeliePolarStereographic;
        // public readonly ProjectionInfo Petrels1972TerreAdeliePolarStereographic;
        public readonly ProjectionInfo RSRGD2000DarwinGlacierLC2000;
        public readonly ProjectionInfo SouthPoleAzimuthalEquidistant;
        public readonly ProjectionInfo SouthPoleGnomonic;
        public readonly ProjectionInfo SouthPoleLambertAzimuthalEqualArea;
        public readonly ProjectionInfo SouthPoleOrthographic;
        public readonly ProjectionInfo SouthPoleStereographic;
        public readonly ProjectionInfo UPSNorth;
        public readonly ProjectionInfo UPSSouth;
        public readonly ProjectionInfo WGS1984AntarcticPolarStereographic;
        public readonly ProjectionInfo WGS1984ArcticPolarStereographic;
        public readonly ProjectionInfo WGS1984AustralianAntarcticLambert;
        public readonly ProjectionInfo WGS1984AustralianAntarcticPolarStereographic;
        public readonly ProjectionInfo WGS1984EASEGridNorth;
        public readonly ProjectionInfo WGS1984EASEGridSouth;
        public readonly ProjectionInfo WGS1984IBCAOPolarStereographic;
        public readonly ProjectionInfo WGS1984NorthPoleLAEAAlaska;
        public readonly ProjectionInfo WGS1984NorthPoleLAEAAtlantic;
        public readonly ProjectionInfo WGS1984NorthPoleLAEABeringSea;
        public readonly ProjectionInfo WGS1984NorthPoleLAEACanada;
        public readonly ProjectionInfo WGS1984NorthPoleLAEAEurope;
        public readonly ProjectionInfo WGS1984NorthPoleLAEARussia;
        public readonly ProjectionInfo WGS1984NSIDCSeaIcePolarStereographicNorth;
        public readonly ProjectionInfo WGS1984NSIDCSeaIcePolarStereographicSouth;
        public readonly ProjectionInfo WGS1984USGSTransantarcticMountains;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Polar.
        /// </summary>
        public Polar()
        {
            NorthPoleAzimuthalEquidistant = ProjectionInfo.FromAuthorityCode("ESRI", 102016).SetNames("North_Pole_Azimuthal_Equidistant", "GCS_WGS_1984", "D_WGS_1984");
            NorthPoleGnomonic = ProjectionInfo.FromAuthorityCode("ESRI", 102034).SetNames("North_Pole_Gnomonic", "GCS_WGS_1984", "D_WGS_1984"); // missing
            NorthPoleLambertAzimuthalEqualArea = ProjectionInfo.FromAuthorityCode("ESRI", 102017).SetNames("North_Pole_Lambert_Azimuthal_Equal_Area", "GCS_WGS_1984", "D_WGS_1984");
            NorthPoleOrthographic = ProjectionInfo.FromAuthorityCode("ESRI", 102035).SetNames("North_Pole_Orthographic", "GCS_WGS_1984", "D_WGS_1984"); // missing
            NorthPoleStereographic = ProjectionInfo.FromAuthorityCode("ESRI", 102018).SetNames("North_Pole_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            NSIDCEASEGridGlobal = ProjectionInfo.FromEpsgCode(3410).SetNames("NSIDC_EASE_Grid_Global", "GCS_Sphere_International_1924_Authalic", "D_Sphere_International_1924_Authalic");
            NSIDCEASEGridNorth = ProjectionInfo.FromEpsgCode(3408).SetNames("NSIDC_EASE_Grid_North", "GCS_Sphere_International_1924_Authalic", "D_Sphere_International_1924_Authalic");
            NSIDCEASEGridSouth = ProjectionInfo.FromEpsgCode(3409).SetNames("NSIDC_EASE_Grid_South", "GCS_Sphere_International_1924_Authalic", "D_Sphere_International_1924_Authalic");
            NSIDCSeaIcePolarStereographicNorth = ProjectionInfo.FromEpsgCode(3411).SetNames("NSIDC_Sea_Ice_Polar_Stereographic_North", "GCS_Hughes_1980", "D_Hughes_1980");
            NSIDCSeaIcePolarStereographicSouth = ProjectionInfo.FromEpsgCode(3412).SetNames("NSIDC_Sea_Ice_Polar_Stereographic_South", "GCS_Hughes_1980", "D_Hughes_1980");
            // Perroud1950TerreAdeliePolarStereographic = ProjectionInfo.FromAuthorityCode("EPSG", 2986).SetNames("Perroud_1950_Terre_Adelie_Polar_Stereographic", "GCS_Pointe_Geologie_Perroud_1950", "D_Pointe_Geologie_Perroud_1950"); // projection not found
            // Petrels1972TerreAdeliePolarStereographic = ProjectionInfo.FromAuthorityCode("EPSG", 2985).SetNames("Petrels_1972_Terre_Adelie_Polar_Stereographic", "GCS_Petrels_1972", "D_Petrels_1972"); // projection not found
            RSRGD2000DarwinGlacierLC2000 = ProjectionInfo.FromEpsgCode(3852).SetNames("RSRGD2000_DGLC2000", "GCS_RSRGD2000", "D_Ross_Sea_Region_Geodetic_Datum_2000");
            SouthPoleAzimuthalEquidistant = ProjectionInfo.FromAuthorityCode("ESRI", 102019).SetNames("South_Pole_Azimuthal_Equidistant", "GCS_WGS_1984", "D_WGS_1984");
            SouthPoleGnomonic = ProjectionInfo.FromAuthorityCode("ESRI", 102036).SetNames("South_Pole_Gnomonic", "GCS_WGS_1984", "D_WGS_1984"); // missing
            SouthPoleLambertAzimuthalEqualArea = ProjectionInfo.FromAuthorityCode("ESRI", 102020).SetNames("South_Pole_Lambert_Azimuthal_Equal_Area", "GCS_WGS_1984", "D_WGS_1984");
            SouthPoleOrthographic = ProjectionInfo.FromAuthorityCode("ESRI", 102037).SetNames("South_Pole_Orthographic", "GCS_WGS_1984", "D_WGS_1984"); // missing
            SouthPoleStereographic = ProjectionInfo.FromAuthorityCode("ESRI", 102021).SetNames("South_Pole_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            UPSNorth = ProjectionInfo.FromEpsgCode(32661).SetNames("UPS_North", "GCS_WGS_1984", "D_WGS_1984");
            UPSSouth = ProjectionInfo.FromEpsgCode(32761).SetNames("UPS_South", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984AntarcticPolarStereographic = ProjectionInfo.FromEpsgCode(3031).SetNames("WGS_1984_Antarctic_Polar_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984ArcticPolarStereographic = ProjectionInfo.FromEpsgCode(3995).SetNames("WGS_1984_Arctic_Polar_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984AustralianAntarcticLambert = ProjectionInfo.FromEpsgCode(3033).SetNames("WGS_1984_Australian_Antarctic_Lambert", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984AustralianAntarcticPolarStereographic = ProjectionInfo.FromEpsgCode(3032).SetNames("WGS_1984_Australian_Antarctic_Polar_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984EASEGridNorth = ProjectionInfo.FromEpsgCode(3973).SetNames("WGS_1984_EASE_Grid_North", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984EASEGridSouth = ProjectionInfo.FromEpsgCode(3974).SetNames("WGS_1984_EASE_Grid_South", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984IBCAOPolarStereographic = ProjectionInfo.FromEpsgCode(3996).SetNames("WGS_1984_IBCAO_Polar_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NorthPoleLAEAAlaska = ProjectionInfo.FromEpsgCode(3572).SetNames("WGS_1984_North_Pole_LAEA_Alaska", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NorthPoleLAEAAtlantic = ProjectionInfo.FromEpsgCode(3574).SetNames("WGS_1984_North_Pole_LAEA_Atlantic", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NorthPoleLAEABeringSea = ProjectionInfo.FromEpsgCode(3571).SetNames("WGS_1984_North_Pole_LAEA_Bering_Sea", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NorthPoleLAEACanada = ProjectionInfo.FromEpsgCode(3573).SetNames("WGS_1984_North_Pole_LAEA_Canada", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NorthPoleLAEAEurope = ProjectionInfo.FromEpsgCode(3575).SetNames("WGS_1984_North_Pole_LAEA_Europe", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NorthPoleLAEARussia = ProjectionInfo.FromEpsgCode(3576).SetNames("WGS_1984_North_Pole_LAEA_Russia", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NSIDCSeaIcePolarStereographicNorth = ProjectionInfo.FromEpsgCode(3413).SetNames("WGS_1984_NSIDC_Sea_Ice_Polar_Stereographic_North", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984NSIDCSeaIcePolarStereographicSouth = ProjectionInfo.FromEpsgCode(3976).SetNames("WGS_1984_NSIDC_Sea_Ice_Polar_Stereographic_South", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984USGSTransantarcticMountains = ProjectionInfo.FromEpsgCode(3294).SetNames("WGS_1984_USGS_Transantarctic_Mountains", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591