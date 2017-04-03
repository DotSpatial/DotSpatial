// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Specifies the OGC type of connection that can occur between two segments
    /// </summary>
    public enum LineJoinType
    {
        /// <summary>
        /// Flat?
        /// </summary>
        Mitre,

        /// <summary>
        /// Rounded
        /// </summary>
        Round,

        /// <summary>
        /// Beveled
        /// </summary>
        Bevel
    }
}