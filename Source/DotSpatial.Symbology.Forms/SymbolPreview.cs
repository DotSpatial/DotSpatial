// ********************************************************************************************************
// Product Name: PredefinedSymbols.dll Alpha
// Description:  The basic module for PredefinedSymbols version 6.0
// ********************************************************************************************************
//
// The Original Code is from PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/19/2009 9:56:08 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This component displays the preview of a feature symbolizer.
    /// </summary>
    [ToolboxItem(false)]
    public class SymbolPreview : Control
    {
        #region Fields

        private IFeatureSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolPreview"/> class.
        /// </summary>
        public SymbolPreview()
        {
            _symbolizer = new PointSymbolizer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature symbolizer displayed in the preview.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFeatureSymbolizer Symbolizer
        {
            get
            {
                return _symbolizer;
            }

            set
            {
                _symbolizer = value;
                UpdatePreview(_symbolizer);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the preview display using the specified symbolizer.
        /// </summary>
        /// <param name="symbolizer">The symbolizer displayed in the preview</param>
        public void UpdatePreview(IFeatureSymbolizer symbolizer)
        {
            if (symbolizer == null) return;

            _symbolizer = symbolizer;
            using (Graphics g = CreateGraphics())
            {
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                var pointSymbolizer = symbolizer as PointSymbolizer;
                if (pointSymbolizer != null)
                {
                    DrawPointSymbolizer(pointSymbolizer, g, rect);
                    return;
                }

                var lineSymbolizer = symbolizer as LineSymbolizer;
                if (lineSymbolizer != null)
                {
                    DrawLineSymbolizer(lineSymbolizer, g, rect);
                    return;
                }

                var polygonSymbolizer = symbolizer as PolygonSymbolizer;
                if (polygonSymbolizer != null)
                {
                    DrawPolygonSymbolizer(polygonSymbolizer, g, rect);
                }
                else
                {
                    symbolizer.Draw(g, rect);
                }
            }
        }

        /// <summary>
        /// Handles the paint event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            UpdatePreview(_symbolizer);
        }

        /// <summary>
        /// Cancels the on paint background event to prevent flicker.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void DrawLineSymbolizer(LineSymbolizer sym, Graphics g, Rectangle rect)
        {
            if (sym != null)
            {
                g.FillRectangle(Brushes.White, rect);
                GraphicsPath gp = new GraphicsPath();
                gp.AddLine(10, rect.Height / 2, rect.Width - 20, rect.Height / 2);
                foreach (IStroke stroke in sym.Strokes)
                {
                    stroke.DrawPath(g, gp, 1);
                }

                gp.Dispose();
            }
        }

        private void DrawPointSymbolizer(PointSymbolizer sym, Graphics g, Rectangle rect)
        {
            if (sym != null)
            {
                g.Clear(Color.White);
                Matrix shift = g.Transform;
                shift.Translate(rect.Width / 2, rect.Height / 2);
                g.Transform = shift;
                double scale = 1;
                if (sym.ScaleMode == ScaleMode.Geographic || sym.GetSize().Height > (rect.Height - 6))
                {
                    scale = (rect.Height - 6) / sym.GetSize().Height;
                }

                sym.Draw(g, scale);
            }
        }

        private void DrawPolygonSymbolizer(PolygonSymbolizer sym, Graphics g, Rectangle rect)
        {
            if (sym != null)
            {
                g.Clear(Color.White);
                Rectangle rect2 = new Rectangle(5, 5, rect.Width - 10, rect.Height - 10);
                sym.Draw(g, rect2);
            }
        }

        #endregion
    }
}