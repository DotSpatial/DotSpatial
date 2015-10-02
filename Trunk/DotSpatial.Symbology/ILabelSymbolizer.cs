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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 8:57:29 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ITextSymbolizer
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface ILabelSymbolizer : ICloneable
    {
        #region Methods

        /// <summary>
        /// Uses the properties defined on this symbolizer to return a font.
        /// </summary>
        /// <returns>A new font</returns>
        Font GetFont();

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the orientation of line labels.
        /// </summary>
        [Category("General"), Description("Gets or sets the orientation of line labels.")]
        LineOrientation LineOrientation { get; set; }


        /// <summary>
        /// Gets or sets a boolean indicating whether or not the LineOrientation gets used.
        /// </summary>
         [Category("General"), Description("Gets or sets a boolean indicating whether or not LineOrientation should be used.")]
        bool UseLineOrientation { get; set; }

        /// <summary>
        /// Gets or sets the labeling method
        /// </summary>
        [Category("General"), Description("Gets or sets the labeling method.")]
        LabelPlacementMethod LabelPlacementMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the labeling method
        /// </summary>
        [Category("General"), Description("Gets or sets the labeling method for line labels.")]
        LineLabelPlacementMethod LineLabelPlacementMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the way features with multiple parts are labeled
        /// </summary>
        [Category("General"), Description("Gets or sets the way features with multiple parts are labeled.")]
        PartLabelingMethod PartsLabelingMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the the multi-line text alignment in the box.
        /// </summary>
        [Category("General"), Description("Gets or sets the horizontal relationship of the text to the anchorpoint.")]
        StringAlignment Alignment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or set the angle that the font should be drawn in
        /// </summary>
        [Category("General"), Description("Gets or sets the angle that the font should be drawn in")]
        double Angle { get; set; }

        /// <summary>
        /// Gets or set a boolean indicating whether or not <see cref="Angle"/> should be used
        /// </summary>
        [Category("General"), Description("Gets or set a boolean indicating whether or not Angle should be used")]
        bool UseAngle { get; set; }

        /// <summary>
        /// Gets or set the field with angle to draw label
        /// </summary>
        [Category("General"), Description("Gets or set the field with angle to draw label")]
        string LabelAngleField { get; set; }

        /// <summary>
        /// Gets or set a boolean indicating whether or not <see cref="LabelAngleField"/> should be used
        /// </summary>
        [Category("General"), Description("Gets or set a boolean indicating whether or not LabelAngleField should be used")]
        bool UseLabelAngleField { get; set; }

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        [Category("General"), Description("Gets or sets the background color of a rectangle around the label")]
        Color BackColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not a background color should be used
        /// </summary>
        [Category("General"), Description("Gets or sets a boolean indicating whether or not a background color should be used")]
        bool BackColorEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the border color
        /// </summary>
        [Category("Border"), Description("Gets or sets the border color")]
        Color BorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not a border should be drawn around the label.
        /// </summary>
        [Category("Border"), Description("Gets or sets a boolean indicating whether or not a border should be drawn around the label.")]
        bool BorderVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that will force a shadow to be drawn if this is true.
        /// </summary>
        [Category("Shadow"), Description("Gets or sets a boolean that will force a shadow to be drawn if this is true.")]
        bool DropShadowEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the actual shadow.  Use the alpha channel to specify opacity.
        /// </summary>
        [Category("Shadow"), Description("Gets or sets the color of the actual shadow.  Use the alpha channel to specify opacity.")]
        Color DropShadowColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an X and Y geographic offset that is only used if ScaleMode is set to Geographic
        /// </summary>
        [Category("Shadow"), Description("ets or sets an X and Y geographic offset that is only used if ScaleMode is set to Geographic")]
        Coordinate DropShadowGeographicOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an X and Y pixel offset that is used if the ScaleMode is set to Symbolic or Simple.
        /// </summary>
        [Category("Shadow"), Description("Gets or sets an X and Y pixel offset that is used if the ScaleMode is set to Symbolic or Simple.")]
        PointF DropShadowPixelOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string font family name
        /// </summary>
        string FontFamily
        {
            get;
            set;
        }

        /// <summary>
        /// gets or sets the font size
        /// </summary>
        float FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        FontStyle FontStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or set the color that the font should be drawn in.
        /// </summary>
        [Category("General"), Description("Gets or sets the color that the font should be drawn in.")]
        Color FontColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean that governs whether or not to draw a halo.
        /// </summary>
        [Category("Halo"), Description("Gets or sets a boolean that governs whether or not to draw a halo.")]
        bool HaloEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the halo that surrounds the text.
        /// </summary>
        [Category("Halo"), Description("Gets or sets the color of the halo that surrounds the text.")]
        Color HaloColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the X offset in pixels from the center of each feature.
        /// </summary>
        [Category("General"), Description("Gets or sets the X offset in pixels from the center of each feature.")]
        float OffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Y offset in pixels from the center of each feature.
        /// </summary>
        [Category("General"), Description("Gets or sets the Y offset in pixels from the center of each feature.")]
        float OffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean.  If true, as high priority labels are placed, they
        /// take up space and will not allow low priority labels that conflict for the
        /// space to be placed.
        /// </summary>
        bool PreventCollisions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string field name for the field that controls which labels
        /// get placed first.  If collision detection is on, a higher priority means
        /// will get placed first.  If it is off, higher priority will be labeled
        /// on top of lower priority.
        /// </summary>
        string PriorityField
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean.  Normally high values from the field are given
        /// a higher priority.  If this is true, low values are given priority instead.
        /// </summary>
        bool PrioritizeLowValues
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of the label relative to the placement point
        /// </summary>
        [Category("General"), Description("Gets or sets the position of the label relative to the placement point")]
        ContentAlignment Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scaling behavior for the text
        /// </summary>
        [Category("General"), Description(" Gets or sets the scaling behavior for the text")]
        ScaleMode ScaleMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets format string used to draw float fields. E.g.:
        /// #.##, 0.000. If empty - then format not used.
        /// </summary>
        string FloatingFormat { get; set; }

        #endregion
    }
}