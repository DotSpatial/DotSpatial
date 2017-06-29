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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 12:24:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureSymbolizer
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Serializable]
    public class FeatureSymbolizer : LegendItem, IFeatureSymbolizer
    {
        #region Private Variables

        private bool _isVisible;
        private ScaleMode _scaleMode;
        private bool _smoothing;
        private GraphicsUnit _unit;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureSymbolizer
        /// </summary>
        protected FeatureSymbolizer()
        {
            _scaleMode = ScaleMode.Simple;
            _smoothing = true;
            _isVisible = true;
            _unit = GraphicsUnit.Pixel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        public virtual void SetOutline(Color outlineColor, double width)
        {
            OnItemChanged(this);
        }

        /// <summary>
        /// Draws a simple rectangle in the specified location.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="target"></param>
        public virtual void Draw(Graphics g, Rectangle target)
        {
            g.DrawRectangle(Pens.Black, target);
        }

        /// <summary>
        /// Occurs during the copy properties method, when copying properties from the source symbolizer
        /// to this symbolizer.
        /// </summary>
        /// <param name="source">The source symbolizer to copy properties from.</param>
        protected override void OnCopyProperties(object source)
        {
            base.OnCopyProperties(source);
            OnItemChanged();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether or not this specific feature should be drawn.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets a boolean indicating whether or not this should be drawn.")]
        [Serialize("IsVisible")]
        public virtual bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }

        /// <summary>
        /// Gets or Sets a ScaleModes enumeration that determines whether non-coordinate drawing
        /// properties like width or size use pixels or world coordinates.  If pixels are
        /// specified, a back transform is used to approximate pixel sizes.
        /// </summary>
        [Serialize("ScaleModes")]
        public virtual ScaleMode ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        /// <summary>
        /// Gets or sets the smoothing mode to use that controls advanced features like
        /// anti-aliasing.  By default this is set to antialias.
        /// </summary>
        [Serialize("Smoothing")]
        public virtual bool Smoothing
        {
            get { return _smoothing; }
            set { _smoothing = value; }
        }

        /// <summary>
        /// Gets or sets the graphics unit to work with.
        /// </summary>
        [Serialize("Units")]
        public GraphicsUnit Units
        {
            get { return _unit; }
            set { _unit = value; }
        }

        #endregion
    }
}