// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 4:01:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Gets or sets the selection method to use.
    /// <list type="bullet">
    /// <item>Inclusion</item>
    /// <item>Intersection</item>
    /// </list>
    /// Inclusion means that the entire shape must be within the selection bounds in order to select
    /// the shape.  Intersection means that only a portion of the shape must be within the selection
    /// bounds in order for the shape to be selected.
    /// </summary>
    public enum SelectMode
    {
        /// <summary>
        /// The entire contents of the potentially selected item must fall withing the specified extents
        /// </summary>
        Inclusion,

        /// <summary>
        /// The item will be selected if any of the contents of the potentially selected item can be found
        /// in the specified extents.
        /// </summary>
        Intersection,
    }
}