// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A class that specifically controls the drawing for Polygons
    /// </summary>
    public class PolygonSymbolizerOld : FeatureSymbolizerOld, IPolygonSymbolizerOld
    {
        #region Variables

        private bool _borderIsVisible;
        private ILineSymbolizer _borderSymbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public PolygonSymbolizerOld()
        {
            _borderSymbolizer = new LineSymbolizer();
            Configure();
        }

        /// <summary>
        /// Gets or sets the polygon symbolizer
        /// </summary>
        /// <param name="selected">Boolean, true if this should use a standard selection symbology of light cyan coloring</param>
        public PolygonSymbolizerOld(bool selected)
        {
            _borderSymbolizer = new LineSymbolizer(selected);
            Configure();
        }

        /// <summary>
        /// Creates a new polygon symbolizer based on the specified parameters.
        /// </summary>
        /// <param name="env">The IEnvelope representing the base geometric size of the layer.  This helps to estimate a useful geographic line width</param>
        /// <param name="selected">Boolean, true if this should use a standard selection symbology of light cyan coloring</param>
        public PolygonSymbolizerOld(IEnvelope env, bool selected)
        {
            _borderSymbolizer = new LineSymbolizer(env, selected);
            Configure();
        }

        private void Configure()
        {
            base.ScaleMode = ScaleMode.Simple;
            _borderIsVisible = true;
            base.FillColor = SymbologyGlobal.RandomLightColor(1);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Replaces the drawing code so that the polygon characteristics are more evident.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="target"></param>
        public override void Draw(Graphics g, Rectangle target)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(target);
            g.FillPath(FillBrush, gp);

            if (_borderIsVisible)
            {
                g.SmoothingMode = _borderSymbolizer.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                const double width = 1;
                if (_borderSymbolizer.ScaleMode == ScaleMode.Geographic)
                {
                    // TO DO: Geographic Scaling
                }
                foreach (IStroke stroke in _borderSymbolizer.Strokes)
                {
                    stroke.DrawPath(g, gp, width);
                }
            }

            gp.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the border symbolizer
        /// </summary>
        public virtual ILineSymbolizer BorderSymbolizer
        {
            get { return _borderSymbolizer; }
            set { _borderSymbolizer = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that determines whether or not the polygon border should be drawn.
        /// </summary>
        public virtual bool BorderIsVisible
        {
            get { return _borderIsVisible; }
            set { _borderIsVisible = value; }
        }

        #endregion
    }
}