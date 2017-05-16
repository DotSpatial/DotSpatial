// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Interface for manipulating the PreviewMap
    /// </summary>
    public interface IPreviewMap
    {
        #region Properties

        /// <summary>
        /// Gets or sets the back color
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the color used for the LocatorBox
        /// </summary>
        Color LocatorBoxColor { get; set; }

        /// <summary>
        /// Gets or sets the Picture to be displayed
        /// </summary>
        Image Picture { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a picture into the PreviewMap from a specified file
        /// </summary>
        /// <param name="fileName">The path to the file to load</param>
        /// <returns>true on success, false on failure</returns>
        bool GetPictureFromFile(string fileName);

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
        /// </summary>
        /// <param name="updateExtents">Update from full extent or current view?</param>
        void Update(PreviewExtentMode updateExtents);

        #endregion
    }
}