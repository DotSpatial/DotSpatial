// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The pattern can act as both the base class for specific types of pattern as well as a wrapper class that allows
    /// for an enumerated constructor that makes it easier to figure out what kinds of patterns can be created.
    /// </summary>
    public class Pattern : Descriptor, IPattern
    {
        #region Fields

        private IPattern _innerPattern;
        private ILineSymbolizer _outline;
        private PatternType _patternType;
        private bool _useOutline;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern"/> class.
        /// </summary>
        public Pattern()
        {
            _outline = new LineSymbolizer();
            _outline.ItemChanged += OutlineItemChanged;
            _useOutline = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern"/> class.
        /// </summary>
        /// <param name="type">The subclass of pattern to use as the internal pattern.</param>
        public Pattern(PatternType type)
        {
            SetType(type);
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires the item changed event.
        /// </summary>
        public event EventHandler ItemChanged;

        /// <summary>
        /// Not Used
        /// </summary>
        public event EventHandler RemoveItem;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rectangular bounds. This controls how the gradient is drawn, and
        /// should be set to the envelope of the entire layer being drawn.
        /// </summary>
        public RectangleF Bounds { get; set; }

        /// <summary>
        /// Gets or sets the ILineSymbolizer that describes the outline symbology for this pattern.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(LineSymbolizerEditor), typeof(UITypeEditor))].
        /// </remarks>
        [Description("Gets or sets the ILineSymbolizer that describes the outline symbology for this pattern.")]
        [Serialize("Outline")]
        public ILineSymbolizer Outline
        {
            get
            {
                return _innerPattern != null ? _innerPattern.Outline : _outline;
            }

            set
            {
                if (_innerPattern != null) _innerPattern.Outline = value;
                _outline = value;
            }
        }

        /// <summary>
        /// Gets or sets the pattern type. Setting this.
        /// </summary>
        [Serialize("PatternType")]
        public PatternType PatternType
        {
            get
            {
                if (_innerPattern != null) return _innerPattern.PatternType;

                return _patternType;
            }

            set
            {
                // Sub-classes will have a null inner pattern that is defined by setting this.
                if (_innerPattern == null)
                {
                    _patternType = value;
                    return;
                }

                // When behaving as a wrapper, the inner pattern should be something other than null.
                SetType(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the pattern should use the outline symbolizer.
        /// </summary>
        [Serialize("UseOutline")]
        public bool UseOutline
        {
            get
            {
                if (_innerPattern != null) return _innerPattern.UseOutline;

                return _useOutline;
            }

            set
            {
                if (_innerPattern != null)
                {
                    _innerPattern.UseOutline = value;
                    return;
                }

                _useOutline = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies the properties defining the outline from the specified source onto this pattern.
        /// </summary>
        /// <param name="source">The source pattern to copy outline properties from.</param>
        public void CopyOutline(IPattern source)
        {
            if (_innerPattern != null)
            {
                _innerPattern.CopyOutline(source);
                return;
            }

            _outline = source.Outline.Copy();
            _useOutline = source.UseOutline;
        }

        /// <summary>
        /// Draws the borders for this graphics path by sequentially drawing all
        /// the strokes in the border symbolizer.
        /// </summary>
        /// <param name="g">The Graphics device to draw to. </param>
        /// <param name="gp">The GraphicsPath that describes the outline to draw.</param>
        /// <param name="scaleWidth">The scaleWidth to use for scaling the line width. </param>
        public virtual void DrawPath(Graphics g, GraphicsPath gp, double scaleWidth)
        {
            if (_innerPattern != null)
            {
                _innerPattern.DrawPath(g, gp, scaleWidth);
                return;
            }

            if (_useOutline)
            {
                _outline?.DrawPath(g, gp, scaleWidth);
            }
        }

        /// <summary>
        /// Fills the specified graphics path with the pattern specified by this object.
        /// </summary>
        /// <param name="g">The Graphics device to draw to.</param>
        /// <param name="gp">The GraphicsPath that describes the closed shape to fill.</param>
        public virtual void FillPath(Graphics g, GraphicsPath gp)
        {
            _innerPattern?.FillPath(g, gp);

            // Does nothing by default, and must be handled in sub-classes
        }

        /// <summary>
        /// Gets a color that can be used to represent this pattern. In some cases, a color is not
        /// possible, in which case, this returns Gray.
        /// </summary>
        /// <returns>A single System.Color that can be used to represent this pattern.</returns>
        public virtual Color GetFillColor()
        {
            return Color.Gray;
        }

        /// <summary>
        /// Sets the color that will attempt to be applied to the top pattern. If the pattern is
        /// not colorable, this does nothing.
        /// </summary>
        /// <param name="color">Color that is set.</param>
        public virtual void SetFillColor(Color color)
        {
            // Overridden in child classes
        }

        /// <summary>
        /// Occurs when the item is changed.
        /// </summary>
        protected virtual void OnItemChanged()
        {
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This is not currently used, but technically should cause the list of patterns to remove this pattern.
        /// </summary>
        protected virtual void OnRemoveItem()
        {
            RemoveItem?.Invoke(this, EventArgs.Empty);
        }

        private void OutlineItemChanged(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        private void SetType(PatternType type)
        {
            _patternType = type;
            IPattern result = null;
            switch (type)
            {
                case PatternType.Gradient:
                    result = new GradientPattern();
                    break;
                case PatternType.Line: break;
                case PatternType.Marker: break;
                case PatternType.Picture:
                    result = new PicturePattern();
                    break;
                case PatternType.Simple:
                    result = new SimplePattern();
                    break;
            }

            if (result != null) result.Outline = _innerPattern.Outline;
            _innerPattern = result;
        }

        #endregion
    }
}