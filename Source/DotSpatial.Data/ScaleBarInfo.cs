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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/15/2009 4:32:44 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// ScaleBarInfo
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class ScaleBarInfo
    {
        #region Private Variables

        private Color _backColor;
        private Color _foreColor;
        private ScaleBarPosition _position;
        private string _unit;
        private bool _visible;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the backcolor
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets the forecolor
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        /// <summary>
        /// Gets or sets what position that the scalebar should load in
        /// </summary>
        public ScaleBarPosition Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the unit for the scalebar.
        /// </summary>
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        /// <summary>
        /// Gets or sets whether to show the scale bar.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        #endregion
    }
}