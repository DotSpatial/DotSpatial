// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/15/2009 4:56:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// StatusBarInfo
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class StatusBarInfo
    {
        #region Private Variables

        string _alternate;
        bool _getFromProjection;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the alternate string to show in the event that GetFromProjection is false.
        /// </summary>
        public string Alternate
        {
            get { return _alternate; }
            set { _alternate = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not status bar coordinates should display the unites derived from the projection string.
        /// </summary>
        public bool GetFromProjection
        {
            get { return _getFromProjection; }
            set { _getFromProjection = value; }
        }

        #endregion
    }
}