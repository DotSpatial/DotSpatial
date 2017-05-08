// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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
    /// Symbolizer for polygon features.
    /// </summary>
    public class PolygonSymbolizer : FeatureSymbolizer, IPolygonSymbolizer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class.
        /// </summary>
        public PolygonSymbolizer()
        {
            Patterns = new CopyList<IPattern>
                       {
                           new SimplePattern()
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class.
        /// </summary>
        /// <param name="fillColor">The color to use as a fill color.</param>
        public PolygonSymbolizer(Color fillColor)
        {
            Patterns = new CopyList<IPattern>
                       {
                           new SimplePattern(fillColor)
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class.
        /// </summary>
        /// <param name="fillColor">The fill color to use for the polygons</param>
        /// <param name="outlineColor">The border color to use for the polygons</param>
        public PolygonSymbolizer(Color fillColor, Color outlineColor)
            : this(fillColor, outlineColor, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class using a solid fill pattern.
        /// </summary>
        /// <param name="fillColor">The fill color to use for the polygons</param>
        /// <param name="outlineColor">The border color to use for the polygons</param>
        /// <param name="outlineWidth">The width of the outline to use fo</param>
        public PolygonSymbolizer(Color fillColor, Color outlineColor, double outlineWidth)
        {
            Patterns = new CopyList<IPattern>
                       {
                           new SimplePattern(fillColor)
                       };
            OutlineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class using a Gradient Pattern with the specified colors and angle.
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        /// <param name="style">Controls how the gradient is drawn</param>
        public PolygonSymbolizer(Color startColor, Color endColor, double angle, GradientType style)
        {
            Patterns = new CopyList<IPattern>
                       {
                           new GradientPattern(startColor, endColor, angle, style)
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class using a Gradient Pattern with the specified colors and angle.
        /// </summary>
        /// <param name="startColor">The start color</param>
        /// <param name="endColor">The end color</param>
        /// <param name="angle">The direction of the gradient, measured in degrees clockwise from the x-axis</param>
        /// <param name="style">The type of gradient to use</param>
        /// <param name="outlineColor">The color to use for the border symbolizer</param>
        /// <param name="outlineWidth">The width of the line to use for the border symbolizer</param>
        public PolygonSymbolizer(Color startColor, Color endColor, double angle, GradientType style, Color outlineColor, double outlineWidth)
            : this(startColor, endColor, angle, style)
        {
            OutlineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class using PicturePattern with the specified image.
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        public PolygonSymbolizer(Image picture, WrapMode wrap, double angle)
        {
            Patterns = new CopyList<IPattern>
                       {
                           new PicturePattern(picture, wrap, angle)
                       };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class using PicturePattern with the specified image.
        /// </summary>
        /// <param name="picture">The picture to draw</param>
        /// <param name="wrap">The way to wrap the picture</param>
        /// <param name="angle">The angle to rotate the image</param>
        /// <param name="outlineColor">The color to use for the border symbolizer</param>
        /// <param name="outlineWidth">The width of the line to use for the border symbolizer</param>
        public PolygonSymbolizer(Image picture, WrapMode wrap, double angle, Color outlineColor, double outlineWidth)
            : this(picture, wrap, angle)
        {
            OutlineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class using the patterns specified by the list or array of patterns.
        /// </summary>
        /// <param name="patterns">The patterns to add to this symbolizer.</param>
        public PolygonSymbolizer(IEnumerable<IPattern> patterns)
        {
            Patterns = new CopyList<IPattern>();
            foreach (IPattern pattern in patterns)
            {
                Patterns.Add(pattern);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizer"/> class.
        /// </summary>
        /// <param name="selected">Boolean, true if this should use selection symbology</param>
        public PolygonSymbolizer(bool selected)
        {
            Patterns = new CopyList<IPattern>();
            if (selected)
            {
                Patterns.Add(new SimplePattern(Color.Cyan));
                OutlineSymbolizer = new LineSymbolizer(Color.DarkCyan, 1);
            }
            else
            {
                Patterns.Add(new SimplePattern());
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Symbolizer for the borders of this polygon as they appear on the top-most pattern.
        /// </summary>
        [ShallowCopy]
        [Serialize("OutlineSymbolizer")]
        public ILineSymbolizer OutlineSymbolizer
        {
            get
            {
                if (Patterns == null) return null;
                if (Patterns.Count == 0) return null;

                return Patterns[Patterns.Count - 1].Outline;
            }

            set
            {
                if (Patterns == null) return;
                if (Patterns.Count == 0) return;

                Patterns[Patterns.Count - 1].Outline = value;
            }
        }

        /// <summary>
        /// gets or sets the list of patterns to use for filling polygons.
        /// </summary>
        [Serialize("Patterns")]
        public IList<IPattern> Patterns { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the polygon symbology
        /// </summary>
        /// <param name="g">The graphics device to draw to</param>
        /// <param name="target">The target rectangle to draw symbology content to</param>
        public override void Draw(Graphics g, Rectangle target)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(target);
            foreach (IPattern pattern in Patterns)
            {
                pattern.Bounds = new RectangleF(target.X, target.Y, target.Width, target.Height);
                pattern.FillPath(g, gp);
            }

            foreach (IPattern pattern in Patterns)
            {
                pattern.Outline?.DrawPath(g, gp, 1);
            }

            gp.Dispose();
        }

        /// <summary>
        /// Gets the fill color of the top-most pattern.
        /// </summary>
        /// <returns>The fill color.</returns>
        public Color GetFillColor()
        {
            if (Patterns == null) return Color.Empty;
            if (Patterns.Count == 0) return Color.Empty;

            return Patterns[Patterns.Count - 1].GetFillColor();
        }

        /// <summary>
        /// This gets the largest width of all the strokes of the outlines of all the patterns. Setting this will
        /// forceably adjust the width of all the strokes of the outlines of all the patterns.
        /// </summary>
        /// <returns>The outline width.</returns>
        public double GetOutlineWidth()
        {
            if (Patterns == null) return 0;
            if (Patterns.Count == 0) return 0;

            double w = 0;
            foreach (IPattern pattern in Patterns)
            {
                double tempWidth = pattern.Outline.GetWidth();
                if (tempWidth > w) w = tempWidth;
            }

            return w;
        }

        /// <summary>
        /// Sets the fill color of the top-most pattern.
        /// If the pattern is not a simple pattern, a simple pattern will be forced.
        /// </summary>
        /// <param name="color">The Color structure</param>
        public void SetFillColor(Color color)
        {
            if (Patterns == null) return;
            if (Patterns.Count == 0) return;

            ISimplePattern sp = Patterns[Patterns.Count - 1] as ISimplePattern;
            if (sp == null)
            {
                sp = new SimplePattern();
                Patterns[Patterns.Count - 1] = sp;
            }

            sp.FillColor = color;
        }

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        public override void SetOutline(Color outlineColor, double width)
        {
            if (Patterns == null) return;
            if (Patterns.Count == 0) return;

            foreach (IPattern pattern in Patterns)
            {
                pattern.Outline.SetFillColor(outlineColor);
                pattern.Outline.SetWidth(width);
                pattern.UseOutline = true;
            }

            base.SetOutline(outlineColor, width);
        }

        /// <summary>
        /// Forces the specified width to be the width of every stroke outlining every pattern.
        /// </summary>
        /// <param name="width">The width to force as the outline width</param>
        public void SetOutlineWidth(double width)
        {
            if (Patterns == null || Patterns.Count == 0) return;

            foreach (IPattern pattern in Patterns)
            {
                pattern.Outline.SetWidth(width);
            }
        }

        /// <summary>
        /// Occurs after the pattern list is set so that we can listen for when
        /// the outline symbolizer gets updated.
        /// </summary>
        protected virtual void OnHandlePatternEvents()
        {
            IChangeEventList<IPattern> patterns = Patterns as IChangeEventList<IPattern>;
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
            IChangeEventList<IPattern> patterns = Patterns as IChangeEventList<IPattern>;
            if (patterns != null)
            {
                patterns.ItemChanged -= PatternsItemChanged;
            }
        }

        private void PatternsItemChanged(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        #endregion
    }
}