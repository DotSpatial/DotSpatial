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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 8:41:13 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Serialization;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    public class LabelSymbolizer : Descriptor, ILabelSymbolizer
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of TextSymbolizer
        /// </summary>
        public LabelSymbolizer()
        {
            Angle = 0;
            UseAngle = false;
            LabelAngleField = null;
            UseLabelAngleField = false;
            BorderVisible = false;
            BorderColor = Color.Black;
            BackColor = Color.AntiqueWhite;
            BackColorEnabled = false;
            DropShadowColor = Color.FromArgb(20, 0, 0, 0);
            DropShadowEnabled = false;
            DropShadowGeographicOffset = new Coordinate(0, 0);
            DropShadowPixelOffset = new PointF(2F, 2F);
            FontSize = 10F;
            FontFamily = "Arial Unicode MS";
            FontStyle = FontStyle.Regular;
            FontColor = Color.Black;
            HaloColor = Color.White;
            HaloEnabled = false;
            ScaleMode = ScaleMode.Symbolic;
            LabelPlacementMethod = LabelPlacementMethod.Centroid;
            PartsLabelingMethod = PartLabelingMethod.LabelLargestPart;
            PreventCollisions = true;
            PriorityField = "FID";
            Orientation = ContentAlignment.MiddleCenter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the multi-line text alignment in the box. I.e., Control the positioning of the text within the rectangular bounds.
        /// </summary>
        [Serialize("Alignment")]
        public StringAlignment Alignment { get; set; }

        /// <summary>
        /// Gets or set the angle that the font should be drawn in
        /// </summary>
        [Serialize("Angle")]
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        [Category("General"), Description("Gets or sets the background color of a rectangle around the label"),
         Serialize("BackColor")]
        public Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not a background color should be used
        /// </summary>
        [Category("General"),
         Description("Gets or sets a boolean indicating whether or not a background color should be used"),
         Serialize("BackColorEnabled")]
        public bool BackColorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        [Category("General"), Description("Gets or sets the background color of a rectangle around the label")]
        [Serialize("BackColorOpacity")]
        [Obsolete("Use BackColor.GetOpacity() instead")] // Marked in 1.7
        public float BackColorOpacity
        {
            get { return BackColor.GetOpacity(); }
            set { BackColor = BackColor.ToTransparent(value); }
        }

        /// <summary>
        /// Gets or sets the border color
        /// </summary>
        [Category("Border"), Description("Gets or sets the border color"), Serialize("BorderColor")]
        public Color BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the border color
        /// </summary>
        [Category("Border"), Description("Gets or sets the border color opacity")]
        [Serialize("BorderColorOpacity")]
        [Obsolete("Use BorderColor.GetOpacity() instead")] // Marked in 1.7
        public float BorderColorOpacity
        {
            get { return BorderColor.GetOpacity(); }
            set { BorderColor = BorderColor.ToTransparent(value); }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not a border should be drawn around the label.
        /// </summary>
        [Category("Border"),
         Description("Gets or sets a boolean indicating whether or not a border should be drawn around the label."),
         Serialize("BorderVisible")]
        public bool BorderVisible { get; set; }

        /// <summary>
        /// Gets or sets the color of the actual shadow.  Use the alpha channel to specify opacity.
        /// </summary>
        [Serialize("DropShadowColor")]
        public Color DropShadowColor { get; set; }

        /// <summary>
        /// Gets or sets a boolean that will force a shadow to be drawn if this is true.
        /// </summary>
        [Serialize("DropShadowEnabled")]
        public bool DropShadowEnabled { get; set; }

        /// <summary>
        /// Gets or sets an X and Y geographic offset that is only used if ScaleMode is set to Geographic
        /// </summary>
        [Serialize("DropShadowGeographicOffset")]
        public Coordinate DropShadowGeographicOffset { get; set; }

        /// <summary>
        /// Gets or sets an X and Y pixel offset that is used if the ScaleMode is set to Symbolic or Simple.
        /// </summary>
        [Serialize("DropShadowPixelOffset")]
        public PointF DropShadowPixelOffset { get; set; }

        /// <summary>
        /// Gets or sets format string used to draw float fields. E.g.:
        /// #.##, 0.000. If empty - then format not used.
        /// </summary>
        [Serialize("FloatingFormat")]
        public string FloatingFormat { get; set; }

        /// <summary>
        /// Gets or set the color that the font should be drawn in.
        /// </summary>
        [Serialize("FontColor")]
        public Color FontColor { get; set; }

        /// <summary>
        /// Gets or set the color that the font should be drawn in.
        /// </summary>
        [Serialize("FontColorOpacity")]
        [Obsolete("Use FontColor.GetOpacity() instead")] // Marked in 1.7
        public float FontColorOpacity
        {
            get { return FontColor.GetOpacity(); }
            set { FontColor = FontColor.ToTransparent(value); }
        }

        /// <summary>
        /// Gets or sets the string font family name
        /// </summary>
        [Serialize("FontFamily")]
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font size
        /// </summary>
        [Serialize("FontSize")]
        public float FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        [Serialize("FontStyle")]
        public FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the color of the halo that surrounds the text.
        /// </summary>
        [Serialize("HaloColor")]
        public Color HaloColor { get; set; }

        /// <summary>
        /// Gets or sets a boolean that governs whether or not to draw a halo.
        /// </summary>
        [Serialize("HaloEnabled")]
        public bool HaloEnabled { get; set; }

        /// <summary>
        /// Gets or set the field with angle to draw label
        /// </summary>
        [Serialize("LabelAngleField")]
        public string LabelAngleField { get; set; }

        /// <summary>
        /// Gets or sets the labeling method
        /// </summary>
        [Serialize("LabelMethod")]
        public LabelPlacementMethod LabelPlacementMethod { get; set; }

        /// <summary>
        /// Gets or sets the labeling method for line labels.
        /// </summary>
        [Serialize("LineLabelPlacementMethod")]
        public LineLabelPlacementMethod LineLabelPlacementMethod { get; set; }

        /// <summary>
        /// Gets or sets the orientation of line labels.
        /// </summary>
        [Serialize("LineOrientation")]
        public LineOrientation LineOrientation { get; set; }

        /// <summary>
        /// Gets or sets the X offset in pixels from the center of each feature.
        /// </summary>
        [Serialize("OffsetX")]
        public float OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the Y offset in pixels from the center of each feature.
        /// </summary>
        [Serialize("OffsetY")]
        public float OffsetY { get; set; }

        /// <summary>
        /// Gets or sets the orientation relative to the placement point. I.e., Controls the position of the label relative to the feature.
        /// </summary>
        [Serialize("Orientation")]
        public ContentAlignment Orientation { get; set; }

        /// <summary>
        /// Gets or sets the way features with multiple parts are labeled.
        /// </summary>
        [Serialize("LabelParts")]
        public PartLabelingMethod PartsLabelingMethod { get; set; }

        /// <summary>
        /// Gets or sets a boolean.  If true, as high priority labels are placed, they
        /// take up space and will not allow low priority labels that conflict for the
        /// space to be placed.
        /// </summary>
        [Serialize("PreventCollisions")]
        public bool PreventCollisions { get; set; }

        /// <summary>
        /// Gets or sets a boolean.  Normally high values from the field are given
        /// a higher priority.  If this is true, low values are given priority instead.
        /// </summary>
        [Serialize("PrioritizeLowValues")]
        public bool PrioritizeLowValues { get; set; }

        /// <summary>
        /// Gets or sets the string field name for the field that controls which labels
        /// get placed first.  If collision detection is on, a higher priority means
        /// will get placed first.  If it is off, higher priority will be labeled
        /// on top of lower priority.
        /// </summary>
        [Serialize("PriorityField")]
        public string PriorityField { get; set; }

        /// <summary>
        /// Gets or sets the scaling behavior for the text
        /// </summary>
        [Serialize("ScaleMode")]
        public ScaleMode ScaleMode { get; set; }

        /// <summary>
        /// Gets or set a boolean indicating whether or not <see cref="Angle"/> should be used
        /// </summary>
        [Serialize("UseAngle")]
        public bool UseAngle { get; set; }

        /// <summary>
        /// Gets or set a boolean indicating whether or not <see cref="LabelAngleField"/> should be used
        /// </summary>
        [Serialize("UseLabelAngleField")]
        public bool UseLabelAngleField { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not the LineOrientation gets used.
        /// </summary>
        [Serialize("UseLineOrientation")]
        public bool UseLineOrientation { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the properties defined on this symbolizer to return a font.
        /// </summary>
        /// <returns>A new font</returns>
        public Font GetFont()
        {
            return new Font(FontFamily, FontSize, FontStyle);
        }

        #endregion
    }
}