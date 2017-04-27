// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
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

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that zooms the map by scrolling the scroll wheel and pans the map by pressing the mouse wheel and moving the mouse.
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
        private bool _isDragging;
        private bool _preventDrag;
        private Rectangle _source;
        private Point _dragStart;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MapFunctionZoom class.
        /// </summary>
        public MapFunctionZoom(IMap inMap)
            : base(inMap)
        {
            Configure();
            BusySet = false;
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
            if (_mapFrame == null) return;
            _client = Rectangle.Empty;
            _mapFrame.ResetExtents();
            Map.IsBusy = false;
            BusySet = false;
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
        protected override void OnMouseWheel(GeoMouseArgs e) // Fix this
        {
            _zoomTimer.Stop(); // if the timer was already started, stop it.
            if (!(e.Map.IsZoomedToMaxExtent && (_direction * e.Delta < 0)))
            {
                e.Map.IsZoomedToMaxExtent = false;
                Rectangle r = e.Map.MapFrame.View;

                // For multiple zoom steps before redrawing, we actually
                // want the x coordinate relative to the screen, not
                // the x coordinate relative to the previously modified view.
                if (_client == Rectangle.Empty) _client = r;
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

                e.Map.MapFrame.View = r;
                e.Map.Invalidate();
                _zoomTimer.Start();
                _mapFrame = e.Map.MapFrame;
                if (!BusySet)
                {
                    Map.IsBusy = true;
                    BusySet = true;
                }
                base.OnMouseWheel(e);
            }

        }

        /// <summary>
        /// Handles the actions that the tool controls during the OnMouseDown event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Middle && !_preventDrag)
            {
                _dragStart = e.Location;
                _source = e.Map.MapFrame.View;
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the mouse move event, changing the viewing extents to match the movements
        /// of the mouse if the left mouse button is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            if (_dragStart != Point.Empty && !_preventDrag)
            {
                if (!BusySet)
                {
                    Map.IsBusy = true;
                    BusySet = true;
                }

                _isDragging = true;
                Point diff = new Point { X = _dragStart.X - e.X, Y = _dragStart.Y - e.Y };
                e.Map.MapFrame.View = new Rectangle(_source.X + diff.X, _source.Y + diff.Y, _source.Width, _source.Height);
                Map.Invalidate();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Mouse Up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Middle && _isDragging)
            {
                _isDragging = false;
                _preventDrag = true;
                e.Map.MapFrame.ResetExtents();
                _preventDrag = false;
                Map.IsBusy = false;
                BusySet = false;
            }
            _dragStart = Point.Empty;

            base.OnMouseUp(e);
        }

        #endregion

        public bool BusySet { get; set; }
    }
}