// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 3:18:04 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// StrokeStyle
    /// </summary>
    public enum StrokeStyle
    {
        /// <summary>
        /// The most complex form, containing a linear pattern that can have a hash as well as decorations
        /// </summary>
        Catographic,

        /// <summary>
        /// This is not directly supported by DotSpatial, but is in fact, some new type that
        /// will have to be returned.
        /// </summary>
        Custom,

        /// <summary>
        /// Draws only the marker symbols where the line occurs, and uses the dash pattern to control placement.
        /// </summary>
        Marker,

        /// <summary>
        /// The simplest line, offering the easiest interface to use
        /// </summary>
        Simple,

        /// <summary>
        /// A hash line
        /// </summary>
        Hash,

        /// <summary>
        /// Uses a picture to generate a texture
        /// </summary>
        Picture,
    }
}