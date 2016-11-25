// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 11:58:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    public static class KnownCoordinateSystems
    {
        /// <summary>
        /// Geographic systems operate in angular units, but can use different
        /// spheroid definitions or angular offsets.
        /// </summary>
        private static GeographicSystems _geographic;

        /// <summary>
        /// Projected systems are systems that use linear units like meters or feet
        /// rather than angular units like degrees or radians
        /// </summary>
        private static ProjectedSystems _projected;

        /// <summary>
        /// Projected systems are systems that use linear units like meters or feet
        /// rather than angular units like degrees or radians
        /// </summary>
        public static ProjectedSystems Projected
        {
            get { return _projected ?? (_projected = new ProjectedSystems()); }
        }

        /// <summary>
        /// Geographic systems operate in angular units, but can use different
        /// spheroid definitions or angular offsets.
        /// </summary>
        public static GeographicSystems Geographic
        {
            get { return _geographic ?? (_geographic = new GeographicSystems()); }
        }
    }
}