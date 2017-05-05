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
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakSliderEventArgs"/> class.
        /// </summary>
        /// <param name="slider">The break slider.</param>
        public BreakSliderEventArgs(BreakSlider slider)
        {
            Slider = slider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the protected break slider
        /// </summary>
        public BreakSlider Slider { get; protected set; }

        #endregion
    }
}