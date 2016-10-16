// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:47:11 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Interface for manipulating the PreviewMap
    /// </summary>
    public interface IPreviewMap
    {
        /// <summary>
        /// Gets/Sets the back color
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets/Sets the Picture to be displayed
        /// </summary>
        Image Picture { get; set; }

        /// <summary>
        /// Gets/Sets the color used for the LocatorBox
        /// </summary>
        Color LocatorBoxColor { get; set; }

        /// <summary>
        /// Tells the PreviewMap to rebuild itself by getting new data from the main view
        /// </summary>
        void GetPictureFromMap();

        /// <summary>
        /// Tells the PreviewMap to rebuild itself by getting new data from the main view (current extents).
        /// </summary>
        void Update();

        /// <summary>
        /// Tells the PreviewMap to rebuild itself by getting new data from the main view.
        /// <param name="updateExtents">Update from full extent or current view?</param>
        /// </summary>
        void Update(PreviewExtentMode updateExtents);

        /// <summary>
        /// Loads a picture into the PreviewMap from a specified file
        /// </summary>
        /// <param name="fileName">The path to the file to load</param>
        /// <returns>true on success, false on failure</returns>
        bool GetPictureFromFile(string fileName);
    }
}