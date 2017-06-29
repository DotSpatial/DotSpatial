// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/27/2009 9:32:38 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// StatisticalEventArgs
    /// </summary>
    public class StatisticalEventArgs : EventArgs
    {
        #region Private Variables

        private Statistics _stats;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatisticalEventArgs
        /// </summary>
        public StatisticalEventArgs(Statistics statistics)
        {
            _stats = statistics;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the set of statistics related to this event.
        /// </summary>
        public Statistics Statistics
        {
            get { return _stats; }
            protected set { _stats = value; }
        }

        #endregion
    }
}