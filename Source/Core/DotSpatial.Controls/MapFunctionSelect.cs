// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;
using Point = System.Drawing.Point;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A map function that can be used to select features.
    /// </summary>
    public class MapFunctionSelect : MapFunction
    {
        #region Fields

        private readonly Pen _selectionPen;
        private Point _currentPoint;
        private Coordinate _geoStartPoint;
        private bool _isDragging;
        private Point _startPoint;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionSelect"/> class.
        /// </summary>
        /// <param name="inMap">The map the tool should work on.</param>
        public MapFunctionSelect(IMap inMap)
            : base(inMap)
        {
            _selectionPen = new Pen(Color.Black)
            {
                DashStyle = DashStyle.Dash
            };
            YieldStyle = YieldStyles.LeftButton | YieldStyles.Keyboard;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the selection rectangle.
        /// </summary>
        /// <param name="e">The map draw args.</param>
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
        /// Handles pressing the delete key to remove features from the specified layer.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool changed = false;
            if (e.KeyCode == Keys.Delete)
            {
                foreach (var fl in Map.MapFrame.GetAllFeatureLayers().Where(_ => _.SelectionEnabled && _.IsVisible && _.Selection.Count > 0))
                {
                    fl.RemoveSelectedFeatures();
                    changed = true;
                }

                if (!changed)
                {
                    MessageBox.Show(MessageStrings.MapFunctionSelect_OnKeyDown_No_Deletable_Layers);
                }
            }

            base.OnKeyDown(e);
            if (changed) Map?.MapFrame.Invalidate();
        }

        /// <summary>
        /// Handles the MouseDown.
        /// </summary>
        /// <param name="e">The eventargs.</param>
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
        /// Handles MouseMove.
        /// </summary>
        /// <param name="e">The event args.</param>
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
        /// Handles the Mouse Up situation.
        /// </summary>
        /// <param name="e">The event args.</param>
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

            Envelope env = new(_geoStartPoint.X, e.GeographicLocation.X, _geoStartPoint.Y, e.GeographicLocation.Y);
            Envelope tolerant = env;

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

            HandleSelection(tolerant, env);

            Map.MapFrame.ResumeEvents();

            // Force an invalidate to clear the dotted lines, even if we haven't changed anything.
            e.Map.Invalidate();

#if DEBUG
            sw.Stop();
            Debug.WriteLine("Initialize: " + sw.ElapsedMilliseconds);
#endif
            base.OnMouseUp(e);
            Map.IsBusy = false;
        }

        private void HandleSelection(Envelope tolerant, Envelope strict)
        {
            Keys key = Control.ModifierKeys;

            if (!Map.MapFrame.GetAllLayers().Any(_ => _.SelectionEnabled && _.IsVisible))
            {
                MessageBox.Show(MessageStrings.MapFunctionSelect_NoSelectableLayer);
            }
            else if ((key & Keys.Control) == Keys.Control)
            {
                Map.InvertSelection(tolerant, strict);
            }
            else
            {
                Map.Select(tolerant, strict, (key & Keys.Shift) != Keys.Shift ? ClearStates.Force : ClearStates.False);
            }
        }

        #endregion
    }
}