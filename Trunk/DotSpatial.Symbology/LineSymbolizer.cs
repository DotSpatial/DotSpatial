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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 2:18:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Topology.Geometries;
using Point = System.Drawing.Point;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineSymbolizer
    /// </summary>
    [Serializable]
    public class LineSymbolizer : FeatureSymbolizer, ILineSymbolizer
    {
        #region Private Variables

        private IList<IStroke> _strokes;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LineSymbolizer
        /// </summary>
        public LineSymbolizer()
        {
            _strokes = new CopyList<IStroke> { new SimpleStroke() };
        }

        /// <summary>
        /// Creates a new set of cartographic lines that together form a line with a border.  Since a compound
        /// pen is used, it is possible to use this to create a transparent line with just two border parts.
        /// </summary>
        /// <param name="fillColor">The fill color for the line</param>
        /// <param name="borderColor">The border color of the line</param>
        /// <param name="width">The width of the entire line</param>
        /// <param name="dash">The dash pattern to use</param>
        /// <param name="caps">The style of the start and end caps</param>
        public LineSymbolizer(Color fillColor, Color borderColor, double width, DashStyle dash, LineCap caps)
            : this(fillColor, borderColor, width, dash, caps, caps) { }

        /// <summary>
        /// Creates a new set of cartographic lines that together form a line with a border.  Since a compound
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
                                             DashStyle = dash,
                                         };
            _strokes.Add(bs);

            ICartographicStroke cs = new CartographicStroke(fillColor)
                                         {
                                             StartCap = startCap,
                                             EndCap = endCap,
                                             DashStyle = dash,
                                             Width = width - 2,
                                         };
            _strokes.Add(cs);
        }

        /// <summary>
        /// Creates a new instance of a LineSymbolizer using the various strokes to form a
        /// composit symbol.
        /// </summary>
        /// <param name="strokes"></param>
        public LineSymbolizer(IEnumerable<IStroke> strokes)
        {
            _strokes = new CopyList<IStroke>();
            foreach (var stroke in strokes)
            {
                _strokes.Add(stroke);
            }
        }

        /// <summary>
        /// Creates a new instance of LineSymbolizer for handling selections.
        /// </summary>
        /// <param name="selected">Boolean, true if this should be symbolized like a selected line.</param>
        public LineSymbolizer(bool selected)
        {
            _strokes = new CopyList<IStroke> { selected ? new CartographicStroke(Color.Cyan) : new CartographicStroke() };
        }

        /// <summary>
        /// Creates a new LineSymbolizer with a single layer with the specified color and width.
        /// </summary>
        /// <param name="color">The color</param>
        /// <param name="width">The line width</param>
        public LineSymbolizer(Color color, double width)
        {
            _strokes = new CopyList<IStroke> { new SimpleStroke(width, color) };
        }

        /// <summary>
        /// Creates a line symbolizer that has a width that is scaled in proportion to the specified envelope as 1/100th of the
        /// width of the envelope.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="selected"></param>
        public LineSymbolizer(IEnvelope env, bool selected)
        {
            _strokes = new CopyList<IStroke>();
            ICartographicStroke myStroke = new CartographicStroke();
            if (selected) myStroke.Color = Color.Cyan;
            myStroke.Width = 1;
            _strokes.Add(myStroke);
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
                using (var p = stroke.ToPen(1))
                    g.DrawLine(p, new Point(target.X, target.Y + target.Height / 2), new Point(target.Right, target.Y + target.Height / 2));
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
                p.AddLine(new Point(target.X, target.Y + target.Height / 2), new Point(target.Right, target.Y + target.Height / 2));

                var cs = stroke as ICartographicStroke;
                if (cs != null)
                {
                    cs.DrawLegendPath(g, p, 1);
                }
                else { stroke.DrawPath(g, p, 1); }
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
                using (var p = stroke.ToPen(scaleWidth))
                    g.DrawPath(p, gp);
            }
        }

        /// <summary>
        /// Gets  the color of the top-most stroke.
        /// </summary>
        public Color GetFillColor()
        {
            if (_strokes == null) return Color.Empty;
            if (_strokes.Count == 0) return Color.Empty;
            var ss = _strokes[_strokes.Count - 1] as ISimpleStroke;
            return ss != null ? ss.Color : Color.Empty;
        }

        /// <summary>
        /// Sets the fill color fo the top-most stroke, and forces the top-most stroke
        /// to be a type of stroke that can accept a fill color if necessary.
        /// </summary>
        /// <param name="fillColor"></param>
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
        /// Gets the Size that is needed to show this line in legend with max. 2 decorations.
        /// </summary>
        public override Size GetLegendSymbolSize()
        {
            Size size = new Size(16, 16); //default size for smaller lines
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
        /// This keeps the ratio of the widths the same, but scales the width up for
        /// all the strokes.
        /// </summary>
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

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        public override void SetOutline(Color outlineColor, double width)
        {
            var w = GetWidth();
            _strokes.Insert(0, new SimpleStroke(w + 2 * width, outlineColor));
            base.SetOutline(outlineColor, width);
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
        [Description("Controls multiple layers of pens, drawn on top of each other.  From object.")]
        [Serialize("Strokes")]
        public IList<IStroke> Strokes
        {
            get { return _strokes; }
            set { _strokes = value; }
        }

        #endregion
    }
}