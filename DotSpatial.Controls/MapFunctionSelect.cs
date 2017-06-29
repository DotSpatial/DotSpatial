// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 3:21:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using SelectionMode = DotSpatial.Symbology.SelectionMode;

namespace DotSpatial.Controls
{
    /// <summary>
    /// SelectTool
    /// </summary>
    public class MapFunctionSelect : MapFunction
    {
        #region Private Variables

        private readonly Pen _selectionPen;
        private Point _currentPoint;
        private Coordinate _geoStartPoint;
        private bool _isDragging;
        private Point _startPoint;
        private ILayer former;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectTool
        /// </summary>
        public MapFunctionSelect(IMap inMap)
            : base(inMap)
        {
            _selectionPen = new Pen(Color.Black) { DashStyle = DashStyle.Dash };
            YieldStyle = YieldStyles.LeftButton | YieldStyles.Keyboard;
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDraw(MapDrawArgs e)
        {
            if (_isDragging)
            {
                // don't draw anything unless we need to draw a select rectangle
                Rectangle r = Opp.RectangleFromPoints(_startPoint, _currentPoint);
                r.Width -= 1;
                r.Height -= 1;
                e.Graphics.DrawRectangle(Pens.White, r);
                e.Graphics.DrawRectangle(_selectionPen, r);
            }
            base.OnDraw(e);
        }

        /// <summary>
        /// Handles the MouseDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = e.Location;
				_currentPoint = _startPoint;
                _geoStartPoint = e.GeographicLocation;
                _isDragging = true;
                Map.IsBusy = true;
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles pressing the delete key to remove features from the specified layer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool changed = false;
            if (e.KeyCode == Keys.Delete)
            {
                bool selectable = false;
                foreach (ILayer layer in Map.MapFrame.GetAllLayers())
                {
                    IFeatureLayer fl = layer as IFeatureLayer;
                    if (fl == null) continue;

                    if (fl.IsWithinLegendSelection())
                    {
                        selectable = true;
                        fl.RemoveSelectedFeatures();
                        changed = true;
                        break;
                    }
                }
                if (selectable == false)
                {
                    MessageBox.Show(MessageStrings.MapFunctionSelect_OnKeyDown_No_Deletable_Layers);
                }
            }
            base.OnKeyDown(e);
            if (changed && Map != null) Map.MapFrame.Invalidate();
        }

        /// <summary>
        /// Handles MouseMove
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            int x = Math.Min(Math.Min(_startPoint.X, _currentPoint.X), e.X);
            int y = Math.Min(Math.Min(_startPoint.Y, _currentPoint.Y), e.Y);
            int mx = Math.Max(Math.Max(_startPoint.X, _currentPoint.X), e.X);
            int my = Math.Max(Math.Max(_startPoint.Y, _currentPoint.Y), e.Y);
            _currentPoint = e.Location;
            if (_isDragging)
            {
                Map.Invalidate(new Rectangle(x, y, mx - x, my - y));
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (Map == null) Map = e.Map;
            if (_isDragging == false) return;
#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            _currentPoint = e.Location;
            _isDragging = false;
            //Map.Invalidate(); // Get rid of the selection box
            //Application.DoEvents();
            IEnvelope env = new Envelope(_geoStartPoint.X, e.GeographicLocation.X,
                _geoStartPoint.Y, e.GeographicLocation.Y);
            IEnvelope tolerant = env;

            if (_startPoint.X == e.X && _startPoint.Y == e.Y)
            {
                // click selection doesn't work quite right without some tiny tolerance.
                double tol = Map.MapFrame.ViewExtents.Width / 10000;
                env.ExpandBy(tol);
            }

            if (Math.Abs(_startPoint.X - e.X) < 8 && Math.Abs(_startPoint.Y - e.Y) < 8)
            {
                Coordinate c1 = e.Map.PixelToProj(new Point(e.X - 4, e.Y - 4));
                Coordinate c2 = e.Map.PixelToProj(new Point(e.X + 4, e.Y + 4));
                tolerant = new Envelope(c1, c2);
            }

            former = null;
            foreach (var l in Map.MapFrame.GetAllLayers())
            {
                if (l.IsSelected)
                {
                    former = l;
                    l.IsSelected = false;
                }
            }
            if (former == null && Map.MapFrame.IsSelected)
            {
                former = Map.MapFrame;
            }
            Map.MapFrame.IsSelected = true;
            Map.MapFrame.SuspendEvents();
            HandleSelection(tolerant, env);
            Map.MapFrame.IsSelected = false;
            if (former != null)
            {
                former.IsSelected = true;
            }
            Map.MapFrame.ResumeEvents();
            // Force an invalidate to clear the dotted lines, even if we haven't changed anything.
            e.Map.Invalidate();
            //e.Map.MapFrame.Initialize();
#if DEBUG
            sw.Stop();
            Debug.WriteLine("Initialize: " + sw.ElapsedMilliseconds);
#endif
            base.OnMouseUp(e);
            Map.IsBusy = false;
        }

        private void HandleSelection(IEnvelope tolerant, IEnvelope strict)
        {
            Keys key = Control.ModifierKeys;
            if ((((key & Keys.Shift) == Keys.Shift) == false)
                && (((key & Keys.Control) == Keys.Control) == false))
            {
                // If they are not pressing shift, then first clear the selection before adding new members to it.
                IEnvelope region;
#if DEBUG
                var sw = new Stopwatch();
                sw.Start();
#endif
                Map.ClearSelection(out region);
#if DEBUG
                sw.Stop();
                Debug.WriteLine("Clear: " + sw.ElapsedMilliseconds);
#endif
            }

            if ((key & Keys.Control) == Keys.Control)
            {
                IEnvelope region;
                Map.InvertSelection(tolerant, strict, SelectionMode.Intersects, out region);
            }
            else
            {
                bool selectable = false;
                foreach (ILayer layer in Map.MapFrame.GetAllLayers())
                {
                    if ((layer.IsVisible && layer.IsWithinLegendSelection()) || layer.IsSelected)
                    {
                        selectable = true;
                        break;
                    }
                }
                if (selectable == false)
                {
                    MessageBox.Show(MessageStrings.MapFunctionSelect_NoSelectableLayer);
                }
                IEnvelope region;
                Map.Select(tolerant, strict, SelectionMode.Intersects, out region);
            }
        }
    }
}