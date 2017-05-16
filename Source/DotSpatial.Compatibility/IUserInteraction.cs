// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// UserInteraction
    /// </summary>
    public interface IUserInteraction
    {
        #region Methods

        /// <summary>
        ///  Retrieve a color ramp, defined by a start and end color, from the user.
        /// </summary>
        /// <param name="suggestedStart">The start color to initialize the dialog with.</param>
        /// <param name="suggestedEnd">The end color to initialize the dialog with.</param>
        /// <param name="selectedStart">The start color that the user selected.</param>
        /// <param name="selectedEnd">The end color that the user selected.</param>
        /// <returns>Boolean, true if the effort was successful.</returns>
        bool GetColorRamp(Color suggestedStart, Color suggestedEnd, out Color selectedStart, out Color selectedEnd);

        /// <summary>
        ///  Prompt the user to select a projection, and return the PROJ4 representation of this
        ///  projection. Specify the dialog caption and an optional default projection ("" for none).
        /// </summary>
        /// <param name="dialogCaption">The text to be displayed on the dialog, e.g. "Please select a projection."</param>
        /// <param name="defaultProjection">The PROJ4 projection string of the projection to default to, "" for none.</param>
        /// <returns>The PROJ4 string of the selected projection</returns>
        string GetProjectionFromUser(string dialogCaption, string defaultProjection);

        #endregion
    }
}