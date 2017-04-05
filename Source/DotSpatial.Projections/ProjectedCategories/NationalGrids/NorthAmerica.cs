// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
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

namespace DotSpatial.Projections.ProjectedCategories.NationalGrids
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for NorthAmerica.
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo CRTM05;
        public readonly ProjectionInfo MexicanDatum1993UTMZone11N;
        public readonly ProjectionInfo MexicanDatum1993UTMZone12N;
        public readonly ProjectionInfo MexicanDatum1993UTMZone13N;
        public readonly ProjectionInfo MexicanDatum1993UTMZone14N;
        public readonly ProjectionInfo MexicanDatum1993UTMZone15N;
        public readonly ProjectionInfo MexicanDatum1993UTMZone16N;
        public readonly ProjectionInfo NAD1927GuatemalaNorte;
        public readonly ProjectionInfo NAD1927GuatemalaSur;
        public readonly ProjectionInfo Ocotepeque1935CostaRicaLambertNorte;
        public readonly ProjectionInfo Ocotepeque1935CostaRicaLambertSur;
        public readonly ProjectionInfo WGS1984CostaRicaTM90;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica.
        /// </summary>
        public NorthAmerica()
        {
            CRTM05 = ProjectionInfo.FromAuthorityCode("EPSG", 102305).SetNames("CRTM05", "GCS_CR05", "D_Costa_Rica_2005"); // missing
            MexicanDatum1993UTMZone11N = ProjectionInfo.FromAuthorityCode("EPSG", 103794).SetNames("Mexican_Datum_1993_UTM_Zone_11N", "GCS_Mexican_Datum_of_1993", "D_Mexican_Datum_of_1993"); // missing
            MexicanDatum1993UTMZone12N = ProjectionInfo.FromAuthorityCode("EPSG", 103795).SetNames("Mexican_Datum_1993_UTM_Zone_12N", "GCS_Mexican_Datum_of_1993", "D_Mexican_Datum_of_1993"); // missing
            MexicanDatum1993UTMZone13N = ProjectionInfo.FromAuthorityCode("EPSG", 103796).SetNames("Mexican_Datum_1993_UTM_Zone_13N", "GCS_Mexican_Datum_of_1993", "D_Mexican_Datum_of_1993"); // missing
            MexicanDatum1993UTMZone14N = ProjectionInfo.FromAuthorityCode("EPSG", 103797).SetNames("Mexican_Datum_1993_UTM_Zone_14N", "GCS_Mexican_Datum_of_1993", "D_Mexican_Datum_of_1993"); // missing
            MexicanDatum1993UTMZone15N = ProjectionInfo.FromAuthorityCode("EPSG", 103798).SetNames("Mexican_Datum_1993_UTM_Zone_15N", "GCS_Mexican_Datum_of_1993", "D_Mexican_Datum_of_1993"); // missing
            MexicanDatum1993UTMZone16N = ProjectionInfo.FromAuthorityCode("EPSG", 103799).SetNames("Mexican_Datum_1993_UTM_Zone_16N", "GCS_Mexican_Datum_of_1993", "D_Mexican_Datum_of_1993"); // missing
            NAD1927GuatemalaNorte = ProjectionInfo.FromEpsgCode(32061).SetNames("NAD_1927_Guatemala_Norte", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927GuatemalaSur = ProjectionInfo.FromEpsgCode(32062).SetNames("NAD_1927_Guatemala_Sur", "GCS_North_American_1927", "D_North_American_1927");
            Ocotepeque1935CostaRicaLambertNorte = ProjectionInfo.FromAuthorityCode("ESRI", 102221).SetNames("Ocotepeque_1935_Costa_Rica_Lambert_Norte", "GCS_Ocotepeque_1935", "D_Ocotepeque_1935"); // missing
            Ocotepeque1935CostaRicaLambertSur = ProjectionInfo.FromAuthorityCode("ESRI", 102222).SetNames("Ocotepeque_1935_Costa_Rica_Lambert_Sur", "GCS_Ocotepeque_1935", "D_Ocotepeque_1935"); // missing
            WGS1984CostaRicaTM90 = ProjectionInfo.FromAuthorityCode("ESRI", 102223).SetNames("WGS_1984_Costa_Rica_TM_90", "GCS_WGS_1984", "D_WGS_1984"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591