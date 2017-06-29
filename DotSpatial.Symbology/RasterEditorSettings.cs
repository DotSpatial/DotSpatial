// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/14/2009 8:50:58 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    [Serializable]
    public class RasterEditorSettings : EditorSettings
    {
        #region Private Variables

        private double _max;
        private double _min;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of VectorEditorSettings
        /// </summary>
        public RasterEditorSettings()
        {
            NumBreaks = 2;
            MaxSampleCount = 10000;
            _min = -100000;
            _max = 1000000;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the minimum value that will contribute to statistics
        /// </summary>
        [Serialize("Min")]
        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value that will contribute to statistics.
        /// </summary>
        [Serialize("Max")]
        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        #endregion
    }
}