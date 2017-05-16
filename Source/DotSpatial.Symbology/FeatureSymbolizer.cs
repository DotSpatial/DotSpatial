// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Common symbolizer for features.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class FeatureSymbolizer : LegendItem, IFeatureSymbolizer
    {
        #region Fields

        private bool _isVisible;
        private ScaleMode _scaleMode;
        private bool _smoothing;
        private GraphicsUnit _unit;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSymbolizer"/> class.
        /// </summary>
        protected FeatureSymbolizer()
        {
            _scaleMode = ScaleMode.Simple;
            _smoothing = true;
            _isVisible = true;
            _unit = GraphicsUnit.Pixel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not this specific feature should be drawn.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets a boolean indicating whether or not this should be drawn.")]
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
        /// properties like width or size use pixels or world coordinates. If pixels are
        /// specified, a back transform is used to approximate pixel sizes.
        /// </summary>
        [Serialize("ScaleModes")]
        public virtual ScaleMode ScaleMode
        {
            get
            {
                return _scaleMode;
            }

            set
            {
                _scaleMode = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether things should be anti-aliased. By default this is set to antialias.
        /// </summary>
        [Serialize("Smoothing")]
        public virtual bool Smoothing
        {
            get
            {
                return _smoothing;
            }

            set
            {
                _smoothing = value;
            }
        }

        /// <summary>
        /// Gets or sets the graphics unit to work with.
        /// </summary>
        [Serialize("Units")]
        public GraphicsUnit Units
        {
            get
            {
                return _unit;
            }

            set
            {
                _unit = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws a simple rectangle in the specified location.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="target">The rectangle that gets drawn.</param>
        public virtual void Draw(Graphics g, Rectangle target)
        {
            g.DrawRectangle(Pens.Black, target);
        }

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
    }
}