// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A class that specifically controls the drawing for Polygons.
    /// </summary>
    public class PolygonSymbolizerOld : FeatureSymbolizerOld, IPolygonSymbolizerOld
    {
        #region Fields

        private bool _borderIsVisible;
        private ILineSymbolizer _borderSymbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizerOld"/> class.
        /// </summary>
        public PolygonSymbolizerOld()
        {
            _borderSymbolizer = new LineSymbolizer();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizerOld"/> class.
        /// </summary>
        /// <param name="selected">Boolean, true if this should use a standard selection symbology of light cyan coloring</param>
        public PolygonSymbolizerOld(bool selected)
        {
            _borderSymbolizer = new LineSymbolizer(selected);
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonSymbolizerOld"/> class.
        /// </summary>
        /// <param name="env">The Envelope representing the base geometric size of the layer. This helps to estimate a useful geographic line width</param>
        /// <param name="selected">Boolean, true if this should use a standard selection symbology of light cyan coloring</param>
        public PolygonSymbolizerOld(Envelope env, bool selected)
        {
            _borderSymbolizer = new LineSymbolizer(env, selected);
            Configure();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the polygon border should be drawn.
        /// </summary>
        public virtual bool BorderIsVisible
        {
            get
            {
                return _borderIsVisible;
            }

            set
            {
                _borderIsVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the border symbolizer.
        /// </summary>
        public virtual ILineSymbolizer BorderSymbolizer
        {
            get
            {
                return _borderSymbolizer;
            }

            set
            {
                _borderSymbolizer = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Replaces the drawing code so that the polygon characteristics are more evident.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="target">The rectangle that gets drawn.</param>
        public override void Draw(Graphics g, Rectangle target)
        {
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(target);
                g.FillPath(FillBrush, gp);

                if (_borderIsVisible)
                {
                    g.SmoothingMode = _borderSymbolizer.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                    const double Width = 1;
                    if (_borderSymbolizer.ScaleMode == ScaleMode.Geographic)
                    {
                        // TO DO: Geographic Scaling
                    }

                    foreach (IStroke stroke in _borderSymbolizer.Strokes)
                    {
                        stroke.DrawPath(g, gp, Width);
                    }
                }
            }
        }

        private void Configure()
        {
            ScaleMode = ScaleMode.Simple;
            _borderIsVisible = true;
            FillColor = SymbologyGlobal.RandomLightColor(1);
        }

        #endregion
    }
}