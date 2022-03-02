// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2009 2:56:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    public enum Proj4Datum
    {
        /// <summary>
        /// World Geodetic System 1984
        /// </summary>
        WGS84,
        /// <summary>
        /// Greek Geodetic Reference system 1987
        /// </summary>
        GGRS87,
        /// <summary>
        /// North American Datum 1983
        /// </summary>
        NAD83,
        /// <summary>
        /// North American Datum 1927
        /// </summary>
        NAD27,
        /// <summary>
        /// Potsdam Rauenburg 1950 DHDN
        /// </summary>
        Potsdam,
        /// <summary>
        /// Carthage 1934 Tunisia
        /// </summary>
        Carthage,
        /// <summary>
        /// Hermannskogel
        /// </summary>
        Hermannskogel,
        /// <summary>
        /// Ireland 1965
        /// </summary>
        Ire65,
        /// <summary>
        /// New Zealand Grid
        /// </summary>
        Nzgd49,
        /// <summary>
        /// Airy 1830
        /// </summary>
        OSGB36
    }
}