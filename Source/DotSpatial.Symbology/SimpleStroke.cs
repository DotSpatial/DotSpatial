// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents stroke using specified <see cref="Color"/> and <see cref="DashStyle"/>.
    /// </summary>
    [Serializable]
    [XmlRoot("SimpleStroke")]
    public class SimpleStroke : Stroke, ISimpleStroke
    {
        #region Fields

        private Color _color;
        private DashStyle _dashStyle;
        private double _width;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStroke"/> class.
        /// </summary>
        public SimpleStroke()
        {
            _color = SymbologyGlobal.RandomDarkColor(1);
            _width = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStroke"/> class.
        /// </summary>
        /// <param name="width">The double width of the line to set</param>
        public SimpleStroke(double width)
        {
            Width = width;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStroke"/> class.
        /// </summary>
        /// <param name="color">The color to use for the stroke</param>
        public SimpleStroke(Color color)
        {
            Color = color;
            _width = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStroke"/> class.
        /// </summary>
        /// <param name="width">The double width of the line to set</param>
        /// <param name="color">The color to use for the stroke</param>
        public SimpleStroke(double width, Color color)
        {
            Width = width;
            Color = color;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color for this drawing layer
        /// </summary>
        [Serialize("Color")]
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }

        /// <summary>
        /// Gets or sets the dash style
        /// </summary>
        [Serialize("DashStyle")]
        public DashStyle DashStyle
        {
            get
            {
                return _dashStyle;
            }

            set
            {
                _dashStyle = value;
            }
        }

        /// <summary>
        /// gets or sets the opacity of the color. 1 is fully opaque while 0 is fully transparent.
        /// </summary>
        [Serialize("Opacity")]
        public float Opacity
        {
            get
            {
                return _color.A / 255F;
            }

            set
            {
                float val = value;
                if (val > 1) val = 1F;
                if (val < 0) val = 0F;
                _color = Color.FromArgb((int)(val * 255), _color.R, Color.G, Color.B);
            }
        }

        /// <summary>
        /// Gets or sets the width of this line relative to the
        /// width passed in.
        /// </summary>
        [Serialize("Width")]
        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a color to represent this line. If the stroke doesn't work as a color,
        /// then this color will be gray.
        /// </summary>
        /// <returns>The color.</returns>
        public override Color GetColor()
        {
            return _color;
        }

        /// <summary>
        /// Sets the color of this stroke to the specified color if possible.
        /// </summary>
        /// <param name="color">The color to assign to this color.</param>
        public override void SetColor(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Creates a system drawing pen that has all of the symbolic information necessary for this stroke.
        /// </summary>
        /// <param name="width">The base width. In symbolic drawing this is 1, but in geographic drawing,
        /// this will be a number representing the result of a transform from projToPixel width.</param>
        /// <returns>The created pen.</returns>
        public override Pen ToPen(double width)
        {
            float w = (float)(width * Width);
            Pen result = new Pen(Color, w)
            {
                DashStyle = _dashStyle,
                LineJoin = LineJoin.Round,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            if (_dashStyle == DashStyle.Custom)
            {
                result.DashPattern = new[] { 1F };
            }

            return result;
        }

        /// <summary>
        /// Handles randomization of simple stroke content.
        /// </summary>
        /// <param name="generator">The random generator to use for randomizing characteristics.</param>
        protected override void OnRandomize(Random generator)
        {
            _color = generator.NextColor();
            Opacity = generator.NextFloat();
            _width = generator.NextFloat(10);
            _dashStyle = generator.NextEnum<DashStyle>();
            base.OnRandomize(generator);
        }

        #endregion
    }
}