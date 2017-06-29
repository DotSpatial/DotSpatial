// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/21/2009 3:51:16 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// UserInteraction
    /// </summary>
    public class UserInteraction : IUserInteraction
    {
        #region Private Variables

        #endregion

        #region Methods

        /// <summary>
        ///  Prompt the user to select a projection, and return the PROJ4 representation of this
        ///  projection. Specify the dialog caption and an optional default projection ("" for none).
        /// </summary>
        /// <param name="dialogCaption">The text to be displayed on the dialog, e.g. "Please select a projection."</param>
        /// <param name="defaultProjection">The PROJ4 projection string of the projection to default to, "" for none.</param>
        /// <returns></returns>
        public string GetProjectionFromUser(string dialogCaption, string defaultProjection)
        {
            //         ProjectionSelectDialog dlg = new ProjectionSelectDialog();
            //         return dlg.ShowDialog() != DialogResult.OK ? null : dlg.SelectedCoordinateSystem.ToProj4String();
            return string.Empty;
        }

        /// <summary>
        ///  Retrieve a color ramp, defined by a start and end color, from the user.
        /// </summary>
        /// <param name="suggestedStart">The start color to initialize the dialog with.</param>
        /// <param name="suggestedEnd">The end color to initialize the dialog with.</param>
        /// <param name="selectedStart">The start color that the user selected.</param>
        /// <param name="selectedEnd">The end color that the user selected.</param>
        /// <returns>Boolean, true if the method was successful.</returns>
        public bool GetColorRamp(Color suggestedStart, Color suggestedEnd, out Color selectedStart, out Color selectedEnd)
        {
            //bool TO_DO_SHOW_COLOR_RAMP_DIALOG = true;
            throw new NotImplementedException();
        }

        #endregion
    }
}