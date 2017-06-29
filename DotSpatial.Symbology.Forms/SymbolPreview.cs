// ********************************************************************************************************
// Product Name: PredefinedSymbols.dll Alpha
// Description:  The basic module for PredefinedSymbols version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// This component displays the preview of a feature symbolizer
    /// </summary>
    [ToolboxItem(false)]
    public class SymbolPreview : Control
    {
        #region Events

        #endregion

        #region Private Variables

        private IFeatureSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SymbolPreview control
        /// </summary>
        public SymbolPreview()
        {
            _symbolizer = new PointSymbolizer();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the preview display using the specified symbolizer
        /// </summary>
        /// <param name="symbolizer">The symbolizer displayed in the preview</param>
        public void UpdatePreview(IFeatureSymbolizer symbolizer)
        {
            if (symbolizer == null) return;

            _symbolizer = symbolizer;
            Graphics g = CreateGraphics();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            if (symbolizer is PointSymbolizer)
            {
                DrawPointSymbolizer(symbolizer as PointSymbolizer, g, rect);
            }
            else if (symbolizer is LineSymbolizer)
            {
                DrawLineSymbolizer(symbolizer as LineSymbolizer, g, rect);
            }
            else if (symbolizer is PolygonSymbolizer)
            {
                DrawPolygonSymbolizer(symbolizer as PolygonSymbolizer, g, rect);
            }
            else
            {
                symbolizer.Draw(g, rect);
            }
            g.Dispose();
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

        /// <summary>
        /// Cancels the on paint background event to prevent flicker
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
            //OnPaint(pevent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            UpdatePreview(_symbolizer);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The feature symbolizer displayed in the preview
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

        #region Event Handlers

        #endregion
    }
}