// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 2:01:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// IdentifiedLayers is used to access the list of layers that contained any
    /// information gathered during an Identify function call.
    /// </summary>
    public interface IIdentifiedLayers
    {
        #region Properties

        /// <summary>
        /// Gets the number of layers that had information from the Identify function call.
        /// </summary>
        int Count { get; }

        #endregion

        /// <summary>
        /// Returns an <c>IdentifiedShapes</c> object containing inforamtion about shapes that were
        /// identified during the Identify function call.
        /// </summary>
        /// <param name="layerHandle">Handle of the layer.</param>
        IdentifiedShapes this[int layerHandle] { get; }
    }
}