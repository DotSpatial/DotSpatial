// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Serialization;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Symbolizer for line features.
    /// </summary>
    [Serializable]
    public class LineSymbolizer : FeatureSymbolizer, ILineSymbolizer
    {
        #region Fields

        private IList<IStroke> _strokes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizer"/> class.
        /// </summary>
        public LineSymbolizer()
        {
            _strokes = new CopyList<IStroke>
                       {
                           new SimpleStroke()
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizer"/> class.
        /// This creates a new set of cartographic lines that together form a line with a border. Since a compound
        /// pen is used, it is possible to use this to create a transparent line with just two border parts.
        /// </summary>
        /// <param name="fillColor">The fill color for the line</param>
        /// <param name="borderColor">The border color of the line</param>
        /// <param name="width">The width of the entire line</param>
        /// <param name="dash">The dash pattern to use</param>
        /// <param name="caps">The style of the start and end caps</param>
        public LineSymbolizer(Color fillColor, Color borderColor, double width, DashStyle dash, LineCap caps)
            : this(fillColor, borderColor, width, dash, caps, caps)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizer"/> class.
        /// This creates a new set of cartographic lines that together form a line with a border. Since a compound
        /// pen is used, it is possible to use this to create a transparent line with just two border parts.
        /// </summary>
        /// <param name="fillColor">The fill color for the line</param>
        /// <param name="borderColor">The border color of the line</param>
        /// <param name="width">The width of the entire line</param>
        /// <param name="dash">The dash pattern to use</param>
        /// <param name="startCap">The style of the start cap</param>
        /// <param name="endCap">The style of the end cap</param>
        public LineSymbolizer(Color fillColor, Color borderColor, double width, DashStyle dash, LineCap startCap, LineCap endCap)
        {
            _strokes = new CopyList<IStroke>();
            ICartographicStroke bs = new CartographicStroke(borderColor)
            {
                Width = width,
                StartCap = startCap,
                EndCap = endCap,
                DashStyle = dash
            };
            _strokes.Add(bs);

            ICartographicStroke cs = new CartographicStroke(fillColor)
            {
                StartCap = startCap,
                EndCap = endCap,
                DashStyle = dash,
                Width = width - 2
            };
            _strokes.Add(cs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizer"/> class using the various strokes to form a
        /// composit symbol.
        /// </summary>
        /// <param name="strokes">Strokes used to form a composit symbol.</param>
        public LineSymbolizer(IEnumerable<IStroke> strokes)
        {
            _strokes = new CopyList<IStroke>();
            foreach (var stroke in strokes)
            {
                _strokes.Add(stroke);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizer"/> class for handling selections.
        /// </summary>
        /// <param name="selected">Boolean, true if this should be symbolized like a selected line.</param>
        public LineSymbolizer(bool selected)
        {
            _strokes = new CopyList<IStroke>
                       {
                           selected ? new CartographicStroke(Color.Cyan) : new CartographicStroke()
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizer"/> class with the specified color and width.
        /// </summary>
        /// <param name="color">The color</param>
        /// <param name="width">The line width</param>
        public LineSymbolizer(Color color, double width)
        {
            _strokes = new CopyList<IStroke>
                       {
                           new SimpleStroke(width, color)
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolizer"/> class.
        /// Creates a line symbolizer that has a width that is scaled in proportion to the specified envelope as 1/100th of the
        /// width of the envelope.
        /// </summary>
        /// <param name="env">not used</param>
        /// <param name="selected">Boolean, true if this should be symbolized like a selected line.</param>
        public LineSymbolizer(Envelope env, bool selected)
        {
            _strokes = new CopyList<IStroke>();
            ICartographicStroke myStroke = new CartographicStroke();
            if (selected) myStroke.Color = Color.Cyan;
            myStroke.Width = 1;
            _strokes.Add(myStroke);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of strokes, which define how the drawing pen should behave.
        /// </summary>
        /// <remarks>
        /// [Editor(typeof(StrokesEditor), typeof(UITypeEditor))]
        /// [TypeConverter(typeof(GeneralTypeConverter))]
        /// </remarks>
        [Description("Controls multiple layers of pens, drawn on top of each other. From object.")]
        [Serialize("Strokes")]
        public IList<IStroke> Strokes
        {
            get
            {
                return _strokes;
            }

            set
            {
                _strokes = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws a line instead of a rectangle.
        /// </summary>
        /// <param name="g">The graphics device to draw to.</param>
        /// <param name="target">The rectangle that is used to calculate the lines position and size.</param>
        public override void Draw(Graphics g, Rectangle target)
        {
            foreach (var stroke in _strokes)
            {
                using (var p = stroke.ToPen(1)) g.DrawLine(p, new Point(target.X, target.Y + (target.Height / 2)), new Point(target.Right, target.Y + (target.Height / 2)));
            }
        }

        /// <summary>
        /// Draws the line that is needed to show this lines legend symbol.
        /// </summary>
        /// <param name="g">The graphics device to draw to.</param>
        /// <param name="target">The rectangle that is used to calculate the lines position and size.</param>
        public void DrawLegendSymbol(Graphics g, Rectangle target)
        {
            foreach (IStroke stroke in _strokes)
            {
                GraphicsPath p = new GraphicsPath();
                p.AddLine(new Point(target.X, target.Y + (target.Height / 2)), new Point(target.Right, target.Y + (target.Height / 2)));

                var cs = stroke as ICartographicStroke;
                if (cs != null)
                {
                    cs.DrawLegendPath(g, p, 1);
                }
                else
                {
                    stroke.DrawPath(g, p, 1);
                }
            }
        }

        /// <summary>
        /// Sequentially draws all of the strokes using the specified graphics path.
        /// </summary>
        /// <param name="g">The graphics device to draw to.</param>
        /// <param name="gp">The graphics path that describes the pathway to draw</param>
        /// <param name="scaleWidth">The double scale width that when multiplied by the width gives a measure in pixels</param>
        public virtual void DrawPath(Graphics g, GraphicsPath gp, double scaleWidth)
        {
            foreach (var stroke in _strokes)
            {
                using (var p = stroke.ToPen(scaleWidth)) g.DrawPath(p, gp);
            }
        }

        /// <summary>
        /// Gets  the color of the top-most stroke.
        /// </summary>
        /// <returns>The fill color.</returns>
        public Color GetFillColor()
        {
            if (_strokes == null) return Color.Empty;
            if (_strokes.Count == 0) return Color.Empty;

            var ss = _strokes[_strokes.Count - 1] as ISimpleStroke;
            return ss?.Color ?? Color.Empty;
        }

        /// <summary>
        /// Gets the Size that is needed to show this line in legend with max. 2 decorations.
        /// </summary>
        /// <returns>The legend symbol size.</returns>
        public override Size GetLegendSymbolSize()
        {
            Size size = new Size(16, 16); // default size for smaller lines
            if (_strokes == null) return size;

            foreach (var stroke in _strokes.OfType<ISimpleStroke>())
            {
                if (stroke.Width > size.Height) size.Height = (int)stroke.Width;
            }

            foreach (var stroke in _strokes.OfType<ICartographicStroke>())
            {
                Size s = stroke.GetLegendSymbolSize();
                if (s.Width > size.Width) size.Width = s.Width;
                if (s.Height > size.Height) size.Height = s.Height;
            }

            return size;
        }

        /// <summary>
        /// This gets the largest width of all the strokes.
        /// Setting this will change the width of all the strokes to the specified width, and is not recommended
        /// if you are using thin lines drawn over thicker lines.
        /// </summary>
        /// <returns>The width.</returns>
        public double GetWidth()
        {
            double w = 0;
            if (_strokes == null) return 1;

            foreach (var stroke in _strokes.OfType<ISimpleStroke>())
            {
                if (stroke.Width > w) w = stroke.Width;
            }

            return w;
        }

        /// <summary>
        /// Sets the fill color fo the top-most stroke, and forces the top-most stroke
        /// to be a type of stroke that can accept a fill color if necessary.
        /// </summary>
        /// <param name="fillColor">The new fill color.</param>
        public void SetFillColor(Color fillColor)
        {
            if (_strokes == null) return;
            if (_strokes.Count == 0) return;

            var ss = _strokes[_strokes.Count - 1] as ISimpleStroke;
            if (ss != null)
            {
                ss.Color = fillColor;
            }
        }

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        public override void SetOutline(Color outlineColor, double width)
        {
            var w = GetWidth();
            _strokes.Insert(0, new SimpleStroke(w + (2 * width), outlineColor));
            base.SetOutline(outlineColor, width);
        }

        /// <summary>
        /// This keeps the ratio of the widths the same, but scales the width up for
        /// all the strokes.
        /// </summary>
        /// <param name="width">The new width.</param>
        public void SetWidth(double width)
        {
            if (_strokes == null) return;
            if (_strokes.Count == 0) return;

            var w = GetWidth();
            if (w == 0) return;

            var ratio = width / w;

            foreach (var stroke in _strokes.OfType<ISimpleStroke>())
            {
                stroke.Width *= ratio;
            }
        }

        #endregion
    }
}