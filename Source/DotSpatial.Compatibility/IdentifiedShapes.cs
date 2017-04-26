// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 2:01:53 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// This interface is used to access the list of shapes that were found during an Identify function call.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IdentifiedShapes
    {
        #region Properties

        /// <summary>
        /// Gets the number of shapes that were identified.
        /// </summary>
        int Count { get; }

        #endregion

        /// <summary>
        /// Gets the shape index of an identified that is stored at the position
        /// specified by the Index parameter.
        /// </summary>
        /// <param name="index">Index of the element to get.</param>
        int this[int index] { get; }
    }
}