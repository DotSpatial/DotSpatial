// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can zoom into the map using left mouse clicks or rectangle dragging. It zooms out on right mouse clicks.
    /// </summary>
    public class MapFunctionClickZoom : MapFunction
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
        /// Initializes a new instance of the <see cref="MapFunctionClickZoom"/> class.
        /// </summary>
        /// <param name="inMap">The map the tool should work on.</param>
        public MapFunctionClickZoom(IMap inMap)
            : base(inMap)
        {
            _selectionPen = new Pen(Color.Black)
                            {
                                DashStyle = DashStyle.Dash
                            };
            YieldStyle = YieldStyles.LeftButton | YieldStyles.RightButton;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnDraw(MapDrawArgs e)
        {
            if (_isDragging)
            {
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
        /// <param name="e">The event args.</param>
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
        /// Handles MouseMove
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            if (_isDragging)
            {
                int x = Math.Min(Math.Min(_startPoint.X, _currentPoint.X), e.X);
                int y = Math.Min(Math.Min(_startPoint.Y, _currentPoint.Y), e.Y);
                int mx = Math.Max(Math.Max(_startPoint.X, _currentPoint.X), e.X);
                int my = Math.Max(Math.Max(_startPoint.Y, _currentPoint.Y), e.Y);
                _currentPoint = e.Location;
                Map.Invalidate(new Rectangle(x, y, mx - x, my - y));
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up situation
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (!(e.Map.IsZoomedToMaxExtent && e.Button == MouseButtons.Right))
            {
                e.Map.IsZoomedToMaxExtent = false;
                bool handled = false;
                _currentPoint = e.Location;

                Map.Invalidate();
                if (_isDragging)
                {
                    if (_geoStartPoint != null && _startPoint != e.Location)
                    {
                        Envelope env = new Envelope(_geoStartPoint.X, e.GeographicLocation.X, _geoStartPoint.Y, e.GeographicLocation.Y);
                        if (Math.Abs(e.X - _startPoint.X) > 1 && Math.Abs(e.Y - _startPoint.Y) > 1)
                        {
                            e.Map.ViewExtents = env.ToExtent();
                            handled = true;
                        }
                    }
                }

                _isDragging = false;

                if (handled == false)
                {
                    Rectangle r = e.Map.MapFrame.View;
                    int w = r.Width;
                    int h = r.Height;
                    if (e.Button == MouseButtons.Left)
                    {
                        r.Inflate(-r.Width / 4, -r.Height / 4);

                        // The mouse cursor should anchor the geographic location during zoom.
                        r.X += (e.X / 2) - (w / 4);
                        r.Y += (e.Y / 2) - (h / 4);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        r.Inflate(r.Width / 2, r.Height / 2);
                        r.X += (w / 2) - e.X;
                        r.Y += (h / 2) - e.Y;
                    }

                    e.Map.MapFrame.View = r;
                    e.Map.MapFrame.ResetExtents();
                }
            }

            base.OnMouseUp(e);
            Map.IsBusy = false;
        }

        #endregion
    }
}