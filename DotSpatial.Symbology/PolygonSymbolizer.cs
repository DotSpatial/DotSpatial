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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 1:14:36 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PolygonSymbolizer
    /// </summary>
    public class PolygonSymbolizer : FeatureSymbolizer, IPolygonSymbolizer
    {
        #region Private Variables

        private IList<IPattern> _patterns;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PolygonSymbolizer
        /// </summary>
        public PolygonSymbolizer()
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new SimplePattern());
        }

        /// <summary>
        /// Creates a new instance of a polygon symbolizer
        /// </summary>
        /// <param name="fillColor">The fill color to use for the polygons</param>
        /// <param name="outlineColor">The border color to use for the polygons</param>
        public PolygonSymbolizer(Color fillColor, Color outlineColor)
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new SimplePattern(fillColor));
            OutlineSymbolizer = new LineSymbolizer(outlineColor, 1);
        }

        /// <summary>
        /// Creates a new instance of a solid colored polygon symbolizer
        /// </summary>
        /// <param name="fillColor">The fill color to use for the polygons</param>
        /// <param name="outlineColor">The border color to use for the polygons</param>
        /// <param name="outlineWidth">The width of the outline to use fo</param>
        public PolygonSymbolizer(Color fillColor, Color outlineColor, double outlineWidth)
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new SimplePattern(fillColor));
            OutlineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
        }

        /// <summary>
        /// Creates a new instance of a Gradient Pattern using the specified colors and angle
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        /// <param name="style">Controls how the gradient is drawn</param>
        public PolygonSymbolizer(Color startColor, Color endColor, double angle, GradientType style)
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new GradientPattern(startColor, endColor, angle, style));
        }

        /// <summary>
        /// Creates a new instance of a Gradient Pattern using the specified colors and angle
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        /// <param name="style">The type of gradient to use</param>
        /// <param name="outlineColor">The color to use for the border symbolizer</param>
        /// <param name="outlineWidth">The width of the line to use for the border symbolizer</param>
        public PolygonSymbolizer(Color startColor, Color endColor, double angle, GradientType style, Color outlineColor, double outlineWidth)
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new GradientPattern(startColor, endColor, angle, style));
            OutlineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
        }

        /// <summary>
        /// Creates a new PicturePattern with the specified image
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        public PolygonSymbolizer(Image picture, WrapMode wrap, double angle)
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new PicturePattern(picture, wrap, angle));
        }

        /// <summary>
        /// Creates a new PicturePattern with the specified image
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        /// <param name="outlineColor">The color to use for the border symbolizer</param>
        /// <param name="outlineWidth">The width of the line to use for the border symbolizer</param>
        public PolygonSymbolizer(Image picture, WrapMode wrap, double angle, Color outlineColor, double outlineWidth)
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new PicturePattern(picture, wrap, angle));
            OutlineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
        }

        /// <summary>
        /// Creates a new symbolizer, using the patterns specified by the list or array of patterns.
        /// </summary>
        /// <param name="patterns">The patterns to add to this symbolizer.</param>
        public PolygonSymbolizer(IEnumerable<IPattern> patterns)
        {
            _patterns = new CopyList<IPattern>();
            foreach (IPattern pattern in patterns)
            {
                _patterns.Add(pattern);
            }
        }

        /// <summary>
        /// Specifies a polygon symbolizer with a specific fill color.
        /// </summary>
        /// <param name="fillColor">The color to use as a fill color.</param>
        public PolygonSymbolizer(Color fillColor)
        {
            _patterns = new CopyList<IPattern>();
            _patterns.Add(new SimplePattern(fillColor));
        }

        /// <summary>
        /// Creates a new instance of PolygonSymbolizer
        /// </summary>
        /// <param name="selected">Boolean, true if this should use selection symbology</param>
        public PolygonSymbolizer(bool selected)
        {
            _patterns = new CopyList<IPattern>();
            if (selected)
            {
                _patterns.Add(new SimplePattern(Color.Cyan));
                OutlineSymbolizer = new LineSymbolizer(Color.DarkCyan, 1);
            }
            else
            {
                _patterns.Add(new SimplePattern());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        public override void SetOutline(Color outlineColor, double width)
        {
            if (_patterns == null) return;
            if (_patterns.Count == 0) return;
            foreach (IPattern pattern in _patterns)
            {
                pattern.Outline.SetFillColor(outlineColor);
                pattern.Outline.SetWidth(width);
                pattern.UseOutline = true;
            }
            base.SetOutline(outlineColor, width);
        }

        /// <summary>
        /// This gets the largest width of all the strokes of the outlines of all the patterns.  Setting this will
        /// forceably adjust the width of all the strokes of the outlines of all the patterns.
        /// </summary>
        public double GetOutlineWidth()
        {
            if (_patterns == null) return 0;
            if (_patterns.Count == 0) return 0;
            double w = 0;
            foreach (IPattern pattern in _patterns)
            {
                double tempWidth = pattern.Outline.GetWidth();
                if (tempWidth > w) w = tempWidth;
            }
            return w;
        }

        /// <summary>
        /// Forces the specified width to be the width of every stroke outlining every pattern.
        /// </summary>
        /// <param name="width">The width to force as the outline width</param>
        public void SetOutlineWidth(double width)
        {
            if (_patterns == null) return;
            if (_patterns.Count == 0) return;
            foreach (IPattern pattern in _patterns)
            {
                pattern.Outline.SetWidth(width);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Symbolizer for the borders of this polygon as they appear on the top-most pattern.
        /// </summary>
        [ShallowCopy, Serialize("OutlineSymbolizer")]
        public ILineSymbolizer OutlineSymbolizer
        {
            get
            {
                if (_patterns == null) return null;
                if (_patterns.Count == 0) return null;
                return _patterns[_patterns.Count - 1].Outline;
            }
            set
            {
                if (_patterns == null) return;
                if (_patterns.Count == 0) return;
                _patterns[_patterns.Count - 1].Outline = value;
            }
        }

        /// <summary>
        /// gets or sets the list of patterns to use for filling polygons.
        /// </summary>
        [Serialize("Patterns")]
        public IList<IPattern> Patterns
        {
            get { return _patterns; }
            set
            {
                //if (_patterns != null) OnIgnorePatternEvents();
                _patterns = value;
                //if (_patterns != null) OnHandlePatternEvents();
            }
        }

        /// <summary>
        /// Gets the fill color of the top-most pattern.
        /// </summary>
        /// <returns></returns>
        public Color GetFillColor()
        {
            if (_patterns == null) return Color.Empty;
            if (_patterns.Count == 0) return Color.Empty;
            return _patterns[_patterns.Count - 1].GetFillColor();
        }

        /// <summary>
        /// Sets the fill color of the top-most pattern.
        /// If the pattern is not a simple pattern, a simple pattern will be forced.
        /// </summary>
        /// <param name="color">The Color structure</param>
        public void SetFillColor(Color color)
        {
            if (_patterns == null) return;
            if (_patterns.Count == 0) return;
            ISimplePattern sp = _patterns[_patterns.Count - 1] as ISimplePattern;
            if (sp == null)
            {
                sp = new SimplePattern();
                _patterns[_patterns.Count - 1] = sp;
            }
            sp.FillColor = color;
        }

        #endregion

        #region Event Handlers

        private void PatternsItemChanged(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Draws the polygon symbology
        /// </summary>
        /// <param name="g">The graphics device to draw to</param>
        /// <param name="target">The target rectangle to draw symbology content to</param>
        public override void Draw(Graphics g, Rectangle target)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(target);
            foreach (IPattern pattern in _patterns)
            {
                pattern.Bounds = new RectangleF(target.X, target.Y, target.Width, target.Height);
                pattern.FillPath(g, gp);
            }
            foreach (IPattern pattern in _patterns)
            {
                if (pattern.Outline != null)
                {
                    pattern.Outline.DrawPath(g, gp, 1);
                }
            }
            gp.Dispose();
        }

        /// <summary>
        /// Occurs after the pattern list is set so that we can listen for when
        /// the outline symbolizer gets updated.
        /// </summary>
        protected virtual void OnHandlePatternEvents()
        {
            IChangeEventList<IPattern> patterns = _patterns as IChangeEventList<IPattern>;
            if (patterns != null)
            {
                patterns.ItemChanged += PatternsItemChanged;
            }
        }

        /// <summary>
        /// Occurs before the pattern list is set so that we can stop listening
        /// for messages from the old outline.
        /// </summary>
        protected virtual void OnIgnorePatternEvents()
        {
            IChangeEventList<IPattern> patterns = _patterns as IChangeEventList<IPattern>;
            if (patterns != null)
            {
                patterns.ItemChanged -= PatternsItemChanged;
            }
        }

        #endregion
    }
}