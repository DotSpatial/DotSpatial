// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 12:04:41 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// PointTypes
    /// </summary>
    public enum PointType
    {
        /// <summary>
        /// Circular points
        /// </summary>
        Circle,

        /// <summary>
        /// Diamond
        /// </summary>
        Diamond,

        /// <summary>
        /// Square
        /// </summary>
        Square,

        /// <summary>
        /// Triangle pointed down
        /// </summary>
        TriangleDown,

        /// <summary>
        /// Triangle pointed left
        /// </summary>
        TriangleLeft,

        /// <summary>
        /// Triangle pointed right
        /// </summary>
        TriangleRight,

        /// <summary>
        /// Triangle pointed up
        /// </summary>
        TriangleUp,

        /// <summary>
        /// User defined
        /// </summary>
        UserDefined,
    }
}