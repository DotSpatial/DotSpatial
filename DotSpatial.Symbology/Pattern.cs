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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/19/2009 10:51:27 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The pattern can act as both the base class for specific types of pattern  as well as a wrapper class that allows
    /// for an enumerated constructor that makes it easier to figure out what kinds of patterns can be created.
    /// </summary>
    public class Pattern : Descriptor, IPattern
    {
        #region IPattern Members

        /// <summary>
        /// Fires the item changed event
        /// </summary>
        public event EventHandler ItemChanged;

        /// <summary>
        /// Not Used
        /// </summary>
        public event EventHandler RemoveItem;

        #endregion

        #region Private Variables

        private RectangleF _bounds;
        private IPattern _innerPattern;
        private ILineSymbolizer _outline;
        private PatternType _patternType;
        private bool _useOutline;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Pattern
        /// </summary>
        public Pattern()
        {
            _outline = new LineSymbolizer();
            _outline.ItemChanged += OutlineItemChanged;
            _useOutline = true;
        }

        /// <summary>
        /// Creates a new pattern with the specified type
        /// </summary>
        /// <param name="type">The subclass of pattern to use as the internal pattern</param>
        public Pattern(PatternType type)
        {
            SetType(type);
        }

        private void OutlineItemChanged(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the rectangular bounds.  This controls how the gradient is drawn, and
        /// should be set to the envelope of the entire layer being drawn
        /// </summary>
        public RectangleF Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets a color that can be used to represent this pattern.  In some cases, a color is not
        /// possible, in which case, this returns Gray.
        /// </summary>
        /// <returns>A single System.Color that can be used to represent this pattern.</returns>
        public virtual Color GetFillColor()
        {
            return Color.Gray;
        }

        /// <summary>
        /// Sets the color that will attempt to be applied to the top pattern.  If the pattern is
        /// not colorable, this does nothing.
        /// </summary>
        public virtual void SetFillColor(Color color)
        {
            // Overridden in child classes
        }

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
        /// Fills the specified graphics path with the pattern specified by this object
        /// </summary>
        /// <param name="g">The Graphics device to draw to</param>
        /// <param name="gp">The GraphicsPath that describes the closed shape to fill</param>
        public virtual void FillPath(Graphics g, GraphicsPath gp)
        {
            if (_innerPattern != null)
            {
                _innerPattern.FillPath(g, gp);
            }
            // Does nothing by default, and must be handled in sub-classes
        }

        /// <summary>
        /// Draws the borders for this graphics path by sequentially drawing all
        /// the strokes in the border symbolizer
        /// </summary>
        /// <param name="g">The Graphics device to draw to </param>
        /// <param name="gp">The GraphicsPath that describes the outline to draw</param>
        /// <param name="scaleWidth">The scaleWidth to use for scaling the line width </param>
        public virtual void DrawPath(Graphics g, GraphicsPath gp, double scaleWidth)
        {
            if (_innerPattern != null)
            {
                _innerPattern.DrawPath(g, gp, scaleWidth);
                return;
            }
            if (_useOutline && _outline != null)
            {
                _outline.DrawPath(g, gp, scaleWidth);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ILineSymbolizer that describes the outline symbology for this pattern.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// [Editor(typeof(LineSymbolizerEditor), typeof(UITypeEditor))]
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
        /// Gets or sets the pattern type.  Setting this
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
        /// Gets or sets a boolean indicating whether or not the pattern should use the outline symbolizer.
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

        #region Protected Methods

        /// <summary>
        /// Occurs when the item is changed
        /// </summary>
        protected virtual void OnItemChanged()
        {
            if (ItemChanged != null) ItemChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// This is not currently used, but technically should cause the list of patterns to remove this pattern.
        /// </summary>
        protected virtual void OnRemoveItem()
        {
            if (RemoveItem != null) RemoveItem(this, EventArgs.Empty);
        }

        #endregion

        #region Private Functions

        private void SetType(PatternType type)
        {
            _patternType = type;
            IPattern result = null;
            switch (type)
            {
                case PatternType.Gradient:
                    result = new GradientPattern();
                    break;
                case PatternType.Line:
                    break;
                case PatternType.Marker:
                    break;
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