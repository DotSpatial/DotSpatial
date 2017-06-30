// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// World or pixel coordinates
    /// </summary>
    [Obsolete("Do not use it. This enum is not used in DotSpatial anymore.")] // Marked in 1.7
    public enum GeoReferenceType
    {
        /// <summary>
        /// No referencing was specified so the default will be used
        /// </summary>
        Empty = -1,

        /// <summary>
        /// The coordinates are given relative to the geographic locations, rather than pixel coordinates
        /// </summary>
        GeoReferenced = 0,

        /// <summary>
        /// The coordinates are given in pixels
        /// </summary>
        Pixels = 1
    }

    /// <summary>
    /// An enumeration showing whetehr this item should use a picture or an image
    /// </summary>
    [Obsolete("Do not use it. This enum is not used in DotSpatial anymore.")] // Marked in 1.7
    public enum PictureType
    {
        /// <summary>
        /// No specification was set, so use the default
        /// </summary>
        Empty = -1,

        /// <summary>
        /// Specifies to use an icon
        /// </summary>
        Icon = 0,

        /// <summary>
        /// Specifies to use an image
        /// </summary>
        Image = 1
    }

    /// <summary>
    /// Gives an enumeration for several different line styles
    /// </summary>
    [Obsolete("Do not use it. This enum is not used in DotSpatial anymore.")] // Marked in 1.7
    public enum LineStyle
    {
        /// <summary>
        /// Specifies that none of the normal options were chosen so a default should be used.
        /// </summary>
        Empty = -1,
        /// <summary>
        /// This will draw the specified line so that the curve joins itself
        /// at the ends.
        /// </summary>
        ClosedCurve = 2,
        /// <summary>
        /// Draws a cardinal spline through the points of this feature
        /// </summary>
        Curve = 2,

        /// <summary>
        /// Draws a set of line segments through the points of this feature
        /// </summary>
        Lines = 0
    }

    /// <summary>
    /// This is like a boolean, but with a "default" of empty.  Empty signifies
    /// that the value is not specified.
    /// </summary>
    [Obsolete("Do not use it. This enum is not used in DotSpatial anymore.")] // Marked in 1.7
    public enum TrueFalse
    {
        /// <summary>
        /// Not specified
        /// </summary>
        Empty = 0,

        /// <summary>
        /// Specified as true
        /// </summary>
        True = 1,

        /// <summary>
        /// Specified as false
        /// </summary>
        False = -1
    }

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

    /// <summary>
    /// Specifies the OGC treatment to give the line at the end-points
    /// </summary>
    [Obsolete("Do not use it. This enum is not used in DotSpatial anymore.")] // Marked in 1.7
    public enum LineCapType
    {
        /// <summary>
        /// Flat?
        /// </summary>
        Butt,

        /// <summary>
        /// Round
        /// </summary>
        Round,

        /// <summary>
        /// flat but a little outwards
        /// </summary>
        Square
    }
}