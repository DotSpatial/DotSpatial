// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/27/2009 8:41:43 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// BreakSliderEventArgs
    /// </summary>
    public class BreakSliderEventArgs : EventArgs
    {
        #region Private Variables

        private BreakSlider _slider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of BreakSliderEventArgs
        /// </summary>
        public BreakSliderEventArgs(BreakSlider slider)
        {
            _slider = slider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the protected break slider
        /// </summary>
        public BreakSlider Slider
        {
            get { return _slider; }
            protected set { _slider = value; }
        }

        #endregion
    }
}