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

using System.ComponentModel;
using System.Drawing;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// TextSymbolizer
    /// </summary>
    public class LabelSymbolizer : Descriptor, ILabelSymbolizer
    {
        #region Private Variables

        private StringAlignment _alignment;
        private double _angle;
        private Color _backColor;
        private bool _backColorEnabled;
        private Color _borderColor;
        private bool _borderEnabled;
        private Color _dropShadowColor;
        private bool _dropShadowEnabled;
        private Coordinate _dropShadowGeographicOffset;
        private PointF _dropShadowPixelOffset;
        private Color _fontColor;
        private string _fontFamily;
        private float _fontSize;
        private FontStyle _fontStyle;
        private Color _haloColor;
        private bool _haloEnabled;
        private LabelPlacementMethod _labelPlacementMethod;
        private float _offsetX;
        private float _offsetY;
        private ContentAlignment _orientation;
        private PartLabelingMethod _partsLabelingMethod;
        private bool _preventCollisions;
        private bool _prioritizeLowValues;
        private string _priorityField;
        private ScaleMode _scaleMode;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TextSymbolizer
        /// </summary>
        public LabelSymbolizer()
        {
            _angle = 0;
            UseAngle = false;
            LabelAngleField = null;
            UseLabelAngleField = false;
            _borderEnabled = false;
            _borderColor = Color.Black;
            _backColor = Color.AntiqueWhite;
            _backColorEnabled = false;
            _dropShadowColor = Color.FromArgb(20, 0, 0, 0);
            _dropShadowEnabled = false;
            _dropShadowGeographicOffset = new Coordinate(0, 0);
            _dropShadowPixelOffset = new PointF(2F, 2F);
            _fontSize = 10F;
            _fontFamily = "Arial Unicode MS";
            _fontStyle = FontStyle.Regular;
            _fontColor = Color.Black;
            _haloColor = Color.White;
            _haloEnabled = false;
            _scaleMode = ScaleMode.Symbolic;
            _labelPlacementMethod = LabelPlacementMethod.Centroid;
            _partsLabelingMethod = PartLabelingMethod.LabelLargestPart;
            _preventCollisions = true;
            _priorityField = "FID";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the properties defined on this symbolizer to return a font.
        /// </summary>
        /// <returns>A new font</returns>
        public Font GetFont()
        {
            return new Font(_fontFamily, _fontSize, _fontStyle);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        [Category("General"), Description("Gets or sets the background color of a rectangle around the label")]
        [Serialize("BackColorOpacity")]
        public float BackColorOpacity
        {
            get { return _backColor.GetOpacity(); }
            set { _backColor = _backColor.ToTransparent(value); }
        }

        /// <summary>
        /// Gets or sets the border color
        /// </summary>
        [Category("Border"), Description("Gets or sets the border color opacity")]
        [Serialize("BorderColorOpacity")]
        public float BorderColorOpacity
        {
            get { return _borderColor.GetOpacity(); }
            set { _borderColor = _borderColor.ToTransparent(value); }
        }

        /// <summary>
        /// Gets or set the color that the font should be drawn in.
        /// </summary>
        [Serialize("FontColorOpacity")]
        public float FontColorOpacity
        {
            get { return _fontColor.GetOpacity(); }
            set { _fontColor = _fontColor.ToTransparent(value); }
        }

        /// <summary>
        /// Gets or sets the multi-line text alignment in the box. I.e., Control the positioning of the text within the rectangular bounds.
        /// </summary>
        [Serialize("Alignment")]
        public StringAlignment Alignment
        {
            get { return _alignment; }
            set { _alignment = value; }
        }

        /// <summary>
        /// Gets or set the angle that the font should be drawn in
        /// </summary>
        [Serialize("Angle")]
        public double Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        /// <summary>
        /// Gets or set a boolean indicating whether or not <see cref="Angle"/> should be used
        /// </summary>
        [Serialize("UseAngle")]
        public bool UseAngle { get; set; }

        /// <summary>
        /// Gets or set the field with angle to draw label
        /// </summary>
        [Serialize("LabelAngleField")]
        public string LabelAngleField { get; set; }

        /// <summary>
        /// Gets or set a boolean indicating whether or not <see cref="LabelAngleField"/> should be used
        /// </summary>
        [Serialize("UseLabelAngleField")]
        public bool UseLabelAngleField { get; set; }

        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        [Category("General"), Description("Gets or sets the background color of a rectangle around the label")]
        [Serialize("BackColor")]
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not a background color should be used
        /// </summary>
        [Category("General"), Description("Gets or sets a boolean indicating whether or not a background color should be used")]
        [Serialize("BackColorEnabled")]
        public bool BackColorEnabled
        {
            get { return _backColorEnabled; }
            set { _backColorEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the border color
        /// </summary>
        [Category("Border"), Description("Gets or sets the border color")]
        [Serialize("BorderColor")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not a border should be drawn around the label.
        /// </summary>
        [Category("Border"), Description("Gets or sets a boolean indicating whether or not a border should be drawn around the label.")]
        [Serialize("BorderVisible")]
        public bool BorderVisible
        {
            get { return _borderEnabled; }
            set { _borderEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that will force a shadow to be drawn if this is true.
        /// </summary>
        [Serialize("DropShadowEnabled")]
        public bool DropShadowEnabled
        {
            get { return _dropShadowEnabled; }
            set { _dropShadowEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the color of the actual shadow.  Use the alpha channel to specify opacity.
        /// </summary>
        [Serialize("DropShadowColor")]
        public Color DropShadowColor
        {
            get { return _dropShadowColor; }
            set { _dropShadowColor = value; }
        }

        /// <summary>
        /// Gets or sets an X and Y geographic offset that is only used if ScaleMode is set to Geographic
        /// </summary>
        [Serialize("DropShadowGeographicOffset")]
        public Coordinate DropShadowGeographicOffset
        {
            get { return _dropShadowGeographicOffset; }
            set { _dropShadowGeographicOffset = value; }
        }

        /// <summary>
        /// Gets or sets an X and Y pixel offset that is used if the ScaleMode is set to Symbolic or Simple.
        /// </summary>
        [Serialize("DropShadowPixelOffset")]
        public PointF DropShadowPixelOffset
        {
            get { return _dropShadowPixelOffset; }
            set { _dropShadowPixelOffset = value; }
        }

        /// <summary>
        /// Gets or sets the string font family name
        /// </summary>
        [Serialize("FontFamily")]
        public string FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }

        /// <summary>
        /// gets or sets the font size
        /// </summary>
        [Serialize("FontSize")]
        public float FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        [Serialize("FontStyle")]
        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }

        /// <summary>
        /// Gets or sets the orientation relative to the placement point. I.e., Controls the position of the label relative to the feature.
        /// </summary>
        [Serialize("Orientation")]
        public ContentAlignment Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        /// <summary>
        /// Gets or set the color that the font should be drawn in.
        /// </summary>
        [Serialize("FontColor")]
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that governs whether or not to draw a halo.
        /// </summary>
        [Serialize("HaloEnabled")]
        public bool HaloEnabled
        {
            get { return _haloEnabled; }
            set { _haloEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the color of the halo that surrounds the text.
        /// </summary>
        [Serialize("HaloColor")]
        public Color HaloColor
        {
            get { return _haloColor; }
            set { _haloColor = value; }
        }

        /// <summary>
        /// Gets or sets the labeling method
        /// </summary>
        [Serialize("LabelMethod")]
        public LabelPlacementMethod LabelPlacementMethod
        {
            get { return _labelPlacementMethod; }
            set { _labelPlacementMethod = value; }
        }

        /// <summary>
        /// Gets or sets the way features with multiple parts are labeled
        /// </summary>
        [Serialize("LabelParts")]
        public PartLabelingMethod PartsLabelingMethod
        {
            get { return _partsLabelingMethod; }
            set { _partsLabelingMethod = value; }
        }

        /// <summary>
        /// Gets or sets the X offset in pixels from the center of each feature.
        /// </summary>
        [Serialize("OffsetX")]
        public float OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        /// <summary>
        /// Gets or sets the Y offset in pixels from the center of each feature.
        /// </summary>
        [Serialize("OffsetY")]
        public float OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        /// <summary>
        /// Gets or sets a boolean.  If true, as high priority labels are placed, they
        /// take up space and will not allow low priority labels that conflict for the
        /// space to be placed.
        /// </summary>
        [Serialize("PreventCollisions")]
        public bool PreventCollisions
        {
            get { return _preventCollisions; }
            set { _preventCollisions = value; }
        }

        /// <summary>
        /// Gets or sets the string field name for the field that controls which labels
        /// get placed first.  If collision detection is on, a higher priority means
        /// will get placed first.  If it is off, higher priority will be labeled
        /// on top of lower priority.
        /// </summary>
        [Serialize("PriorityField")]
        public string PriorityField
        {
            get { return _priorityField; }
            set { _priorityField = value; }
        }

        /// <summary>
        /// Gets or sets a boolean.  Normally high values from the field are given
        /// a higher priority.  If this is true, low values are given priority instead.
        /// </summary>
        [Serialize("PrioritizeLowValues")]
        public bool PrioritizeLowValues
        {
            get { return _prioritizeLowValues; }
            set { _prioritizeLowValues = value; }
        }

        /// <summary>
        /// Gets or sets the scaling behavior for the text
        /// </summary>
        [Serialize("ScaleMode")]
        public ScaleMode ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        #endregion
    }
}