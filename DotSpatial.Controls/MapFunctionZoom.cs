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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/11/2008 3:54:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that can zoom the map using the scroll wheel.
    /// </summary>
    public class MapFunctionZoom : MapFunction
    {
        #region Private Variables

        private Rectangle _client;
        private int _direction;
        private IMapFrame _mapFrame;
        private double _sensitivity;
        private int _timerInterval;
        private Timer _zoomTimer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MapFunctionZoom class.
        /// </summary>
        public MapFunctionZoom(IMap inMap)
            : base(inMap)
        {
            Configure();
        }

        private void Configure()
        {
            YieldStyle = YieldStyles.Scroll;
            _timerInterval = 100;
            _zoomTimer = new Timer { Interval = _timerInterval };
            _zoomTimer.Tick += ZoomTimerTick;
            _client = Rectangle.Empty;
            Sensitivity = .30;
            ForwardZoomsIn = true;
            Name = "ScrollZoom";
        }

        private void ZoomTimerTick(object sender, EventArgs e)
        {
            _zoomTimer.Stop();
            if (_mapFrame == null)
            {
                return;
            }
            _client = Rectangle.Empty;
            _mapFrame.ResetExtents();
            Map.IsBusy = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the wheel zoom sensitivity.  Increasing makes it more sensitive.  Maximum is 0.5, Minimum is 0.01
        /// </summary>
        public double Sensitivity
        {
            get { return 1.0 / _sensitivity; }
            set
            {
                if (value > 0.5)
                    value = 0.5;
                else if (value < 0.01)
                    value = 0.01;
                _sensitivity = 1.0 / value;
            }
        }

        /// <summary>
        /// Controls the sense (direction) of zoom (in or out) as you roll the mouse wheel
        /// </summary>
        public bool ForwardZoomsIn
        {
            get { return _direction > 0; }
            set { _direction = value ? 1 : -1; }
        }

        /// <summary>
        /// Gets or sets the full refresh timeout value in milliseconds
        /// </summary>
        public int TimerInterval
        {
            get { return _timerInterval; }
            set
            {
                _timerInterval = value;
                _zoomTimer.Interval = _timerInterval;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Mouse Wheel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(GeoMouseArgs e) //Fix this
        {
            _zoomTimer.Stop(); // if the timer was already started, stop it.

            Extent MaxExtent = e.Map.GetMaxExtent();
         
            if ((e.Map.IsZoomedToMaxExtent == true) && (_direction * e.Delta < 0))
            {}
            else
            {
                e.Map.IsZoomedToMaxExtent = false;
                Rectangle r = e.Map.MapFrame.View;

                // For multiple zoom steps before redrawing, we actually
                // want the x coordinate relative to the screen, not
                // the x coordinate relative to the previously modified view.
                if (_client == Rectangle.Empty)
                {
                    _client = r;
                }
                int cw = _client.Width;
                int ch = _client.Height;

                double w = r.Width;
                double h = r.Height;

                if (_direction * e.Delta > 0)
                {

                    double inFactor = 2.0 * _sensitivity;
                    r.Inflate(Convert.ToInt32(-w / inFactor), Convert.ToInt32(-h / inFactor));
                    // try to keep the mouse cursor in the same geographic position
                    r.X += Convert.ToInt32((e.X * w / (_sensitivity * cw)) - w / inFactor);
                    r.Y += Convert.ToInt32((e.Y * h / (_sensitivity * ch)) - h / inFactor);

                }
                else
                {
                    double outFactor = 0.5 * _sensitivity;
                    r.Inflate(Convert.ToInt32(w / _sensitivity), Convert.ToInt32(h / _sensitivity));
                    r.X += Convert.ToInt32(w / _sensitivity - (e.X * w / (outFactor * cw)));
                    r.Y += Convert.ToInt32(h / _sensitivity - (e.Y * h / (outFactor * ch)));
                }
                int mapHeight = e.Map.MapFrame.View.Height;
                int mapWidth = e.Map.MapFrame.View.Width;


                e.Map.MapFrame.View = r;
                e.Map.Invalidate();
                _zoomTimer.Start();
                _mapFrame = e.Map.MapFrame;
                Map.IsBusy = true;
                base.OnMouseWheel(e);

            }

        }

        #endregion
    }
}