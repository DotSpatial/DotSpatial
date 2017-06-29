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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/6/2009 9:02:07 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ColorRangeEventArgs
    /// </summary>
    public class ColorRangeEventArgs : EventArgs
    {
        #region Private Variables

        private Color _endColor;
        private bool _hsl;
        private int _hueShift;
        private Color _startColor;
        private bool _useColorRange;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorRangeEventArgs
        /// </summary>
        public ColorRangeEventArgs(Color startColor, Color endColor, int hueShift, bool hsl, bool useRange)
        {
            _startColor = startColor;
            _endColor = endColor;
            _hueShift = hueShift;
            _hsl = hsl;
            _useColorRange = useRange;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the start color
        /// </summary>
        public Color StartColor
        {
            get { return _startColor; }
            protected set { _startColor = value; }
        }

        /// <summary>
        /// Gets the end color
        /// </summary>
        public Color EndColor
        {
            get { return _endColor; }
            protected set { _endColor = value; }
        }

        /// <summary>
        /// Gets or sets the hue shift
        /// </summary>
        public int HueShift
        {
            get { return _hueShift; }
            protected set { _hueShift = value; }
        }

        /// <summary>
        /// Gets a boolean.  If true, the ramp of colors should
        /// be built using the HSL characteristics of the start and
        /// end colors rather than the RGB characteristics
        /// </summary>
        public bool HSL
        {
            get { return _hsl; }
            protected set { _hsl = value; }
        }

        /// <summary>
        /// Gets a boolean, true if this color range should be used.
        /// </summary>
        public bool UseColorRange
        {
            get { return _useColorRange; }
            set { _useColorRange = value; }
        }

        #endregion
    }
}