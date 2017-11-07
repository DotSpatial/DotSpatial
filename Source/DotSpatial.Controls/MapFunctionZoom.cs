// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFunction that zooms the map by scrolling the scroll wheel and pans the map by pressing the mouse wheel and moving the mouse.
    /// </summary>
    public class MapFunctionZoom : MapFunction
    {
        #region Fields

        private Rectangle _client;
        private int _direction;
        private Point _dragStart;
        private bool _isDragging;
        private IMapFrame _mapFrame;
        private bool _preventDrag;
        private double _sensitivity;
        private Rectangle _source;
        private int _timerInterval;
        private Timer _zoomTimer;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionZoom"/> class.
        /// </summary>
        /// <param name="inMap">The map the tool should work on.</param>
        public MapFunctionZoom(IMap inMap)
            : base(inMap)
        {
            Configure();
            BusySet = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the map function is currently interacting with the map.
        /// </summary>
        public bool BusySet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether forward zooms in. This controls the sense (direction) of zoom (in or out) as you roll the mouse wheel.
        /// </summary>
        public bool ForwardZoomsIn
        {
            get
            {
                return _direction > 0;
            }

            set
            {
                _direction = value ? 1 : -1;
            }
        }

        /// <summary>
        /// Gets or sets the wheel zoom sensitivity. Increasing makes it more sensitive. Maximum is 0.5, Minimum is 0.01
        /// </summary>
        public double Sensitivity
        {
            get
            {
                return 1.0 / _sensitivity;
            }

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
        /// Gets or sets the full refresh timeout value in milliseconds
        /// </summary>
        public int TimerInterval
        {
            get
            {
                return _timerInterval;
            }

            set
            {
                _timerInterval = value;
                _zoomTimer.Interval = _timerInterval;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the actions that the tool controls during the OnMouseDown event
        /// </summary>
        /// <param name="e">The event args.</param>
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
        /// <param name="e">The event args.</param>
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
                Point diff = new Point
                             {
                                 X = _dragStart.X - e.X,
                                 Y = _dragStart.Y - e.Y
                             };
                e.Map.MapFrame.View = new Rectangle(_source.X + diff.X, _source.Y + diff.Y, _source.Width, _source.Height);
                Map.Invalidate();
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Mouse Up
        /// </summary>
        /// <param name="e">The event args.</param>
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

        private static int[] scaleArray = {
            500000000,400000000,300000000,250000000,200000000,150000000,125000000,100000000,80000000,60000000,40000000,30000000,25000000,20000000,15000000,12500000,10000000,
            8000000,6000000,5000000,4000000,3000000,2500000,2000000,1500000,1250000,1000000,800000,700000,600000,500000,400000,300000,250000,200000,150000,125000,100000,80000,70000,60000,50000,
            40000,30000,25000,20000,15000,12500,10000,8000,6000,5000,4000,3000,2500,2000,1500,1250,1000,800,600,500,400,300,250,200,150,125,100,80,60,50,40,30,25,20,15,10,5,4,3,2,1,0
        };

        /// <summary>
        /// Mouse Wheel
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseWheel(GeoMouseArgs e)
        {

            _zoomTimer.Stop(); // if the timer was already started, stop it.
            if (!(e.Map.IsZoomedToMaxExtent && (_direction * e.Delta < 0)))
            {
                e.Map.IsZoomedToMaxExtent = false;

                double dCurrentScale = e.Map.MapFrame.CurrentScale;
                int iCurrentScale = (int)dCurrentScale;
                int iScalesCount = scaleArray.Length;
                int iClosestCurrentIndex = 0;
                for (int iIndex = scaleArray[iScalesCount / 2] > iCurrentScale ? iScalesCount / 2 : 0; iIndex < iScalesCount; iIndex++)
                {
                    if (scaleArray[iIndex] <= iCurrentScale)
                    {
                        iClosestCurrentIndex = iIndex;
                        break;
                    }
                }
                if (iClosestCurrentIndex > 0)
                {
                    double dDiffLowerFromCurrent = dCurrentScale - (double)scaleArray[iClosestCurrentIndex];
                    double dDiffUpperFromCurrent = (double)scaleArray[iClosestCurrentIndex - 1] - dCurrentScale;
                    if (dDiffUpperFromCurrent < dDiffLowerFromCurrent) iClosestCurrentIndex--;
                }
                int iIndexNow = iClosestCurrentIndex + (e.Delta * _direction) / SystemInformation.MouseWheelScrollDelta;
                if (iIndexNow < 0) iIndexNow = 0;
                else if (iIndexNow > iScalesCount - 2) iIndexNow = iScalesCount - 2; // last of array is 'zero'


                if (!BusySet)
                {
                    Map.IsBusy = true;
                    BusySet = true;
                }
                e.Map.MapFrame.ComputeExtentFromScale((double)scaleArray[iIndexNow], e.Location);
                base.OnMouseWheel(e);
            }
        }

        private void Configure()
        {
            YieldStyle = YieldStyles.Scroll;
            _timerInterval = 100;
            _zoomTimer = new Timer
                         {
                             Interval = _timerInterval
                         };
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
    }
}