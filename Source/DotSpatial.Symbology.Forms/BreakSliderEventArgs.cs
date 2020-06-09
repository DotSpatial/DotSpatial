// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// BreakSliderEventArgs.
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
        /// Gets or sets the protected break slider.
        /// </summary>
        public BreakSlider Slider { get; protected set; }

        #endregion
    }
}