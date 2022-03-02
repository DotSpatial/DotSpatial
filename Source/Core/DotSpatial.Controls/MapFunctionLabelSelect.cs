// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;
using Point = System.Drawing.Point;
using Timer = System.Windows.Forms.Timer;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that allows labels to be selected.
    /// </summary>
    public class MapFunctionLabelSelect : MapFunction
    {
        #region Fields

        private readonly Pen _selectionPen;

        private readonly Timer _selectTimer;
        private Point _currentPoint;
        private bool _doSelect;
        private Coordinate _geoStartPoint;
        private bool _isDragging;
        private Point _startPoint;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionLabelSelect"/> class.
        /// </summary>
        /// <param name="inMap">The map the tool should work on.</param>
        public MapFunctionLabelSelect(IMap inMap)
            : base(inMap)
        {
            _selectionPen = new Pen(Color.Black)
            {
                DashStyle = DashStyle.Dash
            };
            _doSelect = false;
            _selectTimer = new Timer
            {
                Interval = 10
            };
            _selectTimer.Tick += SelectTimerTick;
            YieldStyle = YieldStyles.LeftButton | YieldStyles.RightButton | YieldStyles.Keyboard;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selection envelope.
        /// </summary>
        public Envelope SelectionEnvelope { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="e">The event args.</param>
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

            if (_doSelect)
            {
                foreach (IMapLayer lyr in Map.MapFrame.Layers)
                {
                    IMapFeatureLayer fl = lyr as IMapFeatureLayer;
                    fl?.LabelLayer?.Invalidate();
                }

                _doSelect = false;
                _selectTimer.Start();
            }

            base.OnDraw(e);
        }

        /// <summary>
        /// Handles the MouseDown.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = e.Location;
                _geoStartPoint = e.GeographicLocation;
                _isDragging = true;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles MouseMove.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            _currentPoint = e.Location;
            Map.Invalidate();
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up situation.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            _currentPoint = e.Location;
            _isDragging = false;
            Map.Invalidate();
            if (_geoStartPoint != null)
            {
                SelectionEnvelope = new Envelope(_geoStartPoint.X, e.GeographicLocation.X, _geoStartPoint.Y, e.GeographicLocation.Y);
            }

            // If they are not pressing shift, then first clear the selection before adding new members to it.
            if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift)
            {
                foreach (IMapLayer lyr in Map.MapFrame.Layers)
                {
                    IMapFeatureLayer fl = lyr as IMapFeatureLayer;
                    fl?.LabelLayer?.ClearSelection();
                }
            }

            _doSelect = true;
            e.Map.MapFrame.ResetBuffer();
            e.Map.Invalidate();
            base.OnMouseUp(e);
        }

        private void SelectTimerTick(object? sender, EventArgs e)
        {
            _selectTimer.Stop();
            Map.ResetBuffer();
        }

        #endregion
    }
}