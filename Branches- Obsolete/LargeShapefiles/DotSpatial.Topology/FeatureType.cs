// *******************************************************************************************************
// Product: DotSpatial.Topology.FeatureType.cs
// Description:  An abreviated list for quick classification.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
// *******************************************************************************************************

namespace DotSpatial.Topology
{
    /// <summary>
    /// An abreviated list for quick classification
    /// </summary>
    public enum FeatureType
    {
        /// <summary>
        /// None specified or custom
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Point
        /// </summary>
        Point = 1,

        /// <summary>
        /// Line
        /// </summary>
        Line = 2,

        /// <summary>
        /// Polygon
        /// </summary>
        Polygon = 3,

        /// <summary>
        /// MultiPoint
        /// </summary>
        MultiPoint = 4
    }
}