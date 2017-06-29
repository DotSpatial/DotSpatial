// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.Forms.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;

#if !PocketPC || DesignTime || Framework20

using System.ComponentModel;

#endif

#if PocketPC
#endif

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a user control used to display the current direction of travel.
    /// </summary>
    [ToolboxBitmap(typeof(Compass))]
    [DefaultProperty("Value")]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
#endif
    public sealed class Compass : PolarControl
    {
#if !PocketPC
        private readonly Thread _interpolationThread;
        private bool _isInterpolationActive;
        private ManualResetEvent _animationWaitHandle = new ManualResetEvent(false);
        private readonly Interpolator _valueInterpolator = new Interpolator(15, InterpolationMethod.CubicEaseOut);
        private int _interpolationIndex;
#endif
#if PocketPC
        private Font _DirectionLabelFont = new Font("Tahoma", 8.0f, FontStyle.Bold);
        private Angle _DirectionLabelInterval = new Angle(45);
        private Angle _AngleLabelInterval = new Angle(0);
        private Font _AngleLabelFont = new Font("Tahoma", 6.0f, FontStyle.Regular);
        private Angle _MinorTickInterval = new Angle(5);
#if Framework20
                private Pen _DirectionTickPen = new Pen(Color.Black, 3.0f);
#else
        private Pen pDirectionTickPen = new Pen(Color.Black);
#endif

#else
        private Font _directionLabelFont = new Font("Tahoma", 12.0f, FontStyle.Bold);
        private Angle _directionLabelInterval = new Angle(45);
        private Angle _angleLabelInterval = new Angle(30);
        private Font _angleLabelFont = new Font("Tahoma", 8.0f, FontStyle.Regular);
        private Angle _minorTickInterval = new Angle(2);
        private Pen _directionTickPen = new Pen(Color.Black, 3.0f);
#endif

        private Azimuth _bearing = Azimuth.North;
        private Angle _majorTickInterval = new Angle(15);
        private Pen _centerPen = new Pen(Color.Gray);
        private Pen _minorTickPen = new Pen(Color.Black);
        private Pen _majorTickPen = new Pen(Color.Black);
        private SolidBrush _directionLabelBrush = new SolidBrush(Color.Black);
        private SolidBrush _angleLabelBrush = new SolidBrush(Color.Black);
        private string _angleLabelFormat = "h°";

        private static readonly PolarCoordinate[] _needlePointsNorth = new[]
            {
                new PolarCoordinate(5, new Angle(270), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(270), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(275), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(280), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(285), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(290), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(295), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(300), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(305), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(310), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(315), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(320), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(325), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(330), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(335), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(55, new Angle(0), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(25), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(30), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(35), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(40), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(45), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(50), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(55), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(60), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(65), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(70), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(75), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(80), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(85), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(90), Azimuth.North, PolarCoordinateOrientation.Clockwise)
            };
        private static PolarCoordinate[] _needlePointsSouth = new[]
            {
                new PolarCoordinate(5, new Angle(270), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(270), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(275), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(280), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(285), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(290), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(295), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(300), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(305), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(310), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(315), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(320), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(325), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(330), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(335), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(355), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(55, new Angle(0), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(50, new Angle(5), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(25), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(30), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(35), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(40), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(45), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(50), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(55), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(60), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(65), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(70), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(75), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(80), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(85), Azimuth.North, PolarCoordinateOrientation.Clockwise),
                new PolarCoordinate(8, new Angle(90), Azimuth.North, PolarCoordinateOrientation.Clockwise)
            };
        private SolidBrush _pNorthNeedleBrush = new SolidBrush(Color.Red);
        private Pen _pNorthNeedlePen = new Pen(Color.Black);
        private SolidBrush _pSouthNeedleBrush = new SolidBrush(Color.White);
        private Pen _pSouthNeedlePen = new Pen(Color.Black);
        private bool _pIsUsingRealTimeData;
#if !PocketPC
        private SolidBrush _pNeedleShadowBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        private Size _pNeedleShadowSize = new Size(5, 5);
#endif
#if (PocketPC && Framework20)
        private const int MaximumGracefulShutdownTime = 2000;
#elif !PocketPC
        private const int MAXIMUM_GRACEFUL_SHUTDOWN_TIME = 500;
#endif

        /// <summary>
        /// Occurs when the value changes
        /// </summary>
        public event EventHandler<AzimuthEventArgs> ValueChanged;

        /// <summary>
        /// A compass control
        /// </summary>
        public Compass()
            : base("DotSpatial.Positioning Multithreaded Compass Control (http://dotspatial.codeplex.com)")
        {
#if !PocketPC
            // Start the interpolation thread
            _interpolationThread = new Thread(InterpolationLoop)
                                       {
                                           IsBackground = true,
                                           Name =
                                               "DotSpatial.Positioning Compass Needle Animation Thread (http://dotspatial.codeplex.com)"
                                       };
            _isInterpolationActive = true;
            _interpolationThread.Start();
#endif
            Orientation = PolarCoordinateOrientation.Clockwise;
            Origin = Azimuth.North;
            // The center is zero and edge is 100
            CenterR = 0;
            MaximumR = 100;

            // Now mirror the points to create the southern needle
            _needlePointsSouth = new PolarCoordinate[38];
            for (int index = 0; index < _needlePointsNorth.Length; index++)
            {
                _needlePointsSouth[index] = _needlePointsNorth[index].Rotate(180);
            }
        }

        /// <inheritdocs/>
        protected override void Dispose(bool disposing)
        {
            try
            {
                // Only hook into events if we're at run-time.  Hooking events
                // at design-time can actually cause errors in the WF Designer.
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime
                    && _pIsUsingRealTimeData)
                {
                    Devices.BearingChanged -= Devices_CurrentBearingChanged;
                }
            }
            catch
            {
            }

#if !PocketPC
            // Get the interpolation thread out of a loop
            _isInterpolationActive = false;

            if (_interpolationThread != null)
            {
                if (_animationWaitHandle != null)
                {
                    try
                    {
                        _animationWaitHandle.Set();
                    }
                    catch
                    {
                    }
                }

                if (!_interpolationThread.Join(MAXIMUM_GRACEFUL_SHUTDOWN_TIME))
                {
                    try
                    {
                        _interpolationThread.Abort();
                    }
                    catch
                    {
                    }
                }
            }

            if (_animationWaitHandle != null)
            {
                try
                {
                    _animationWaitHandle.Close();
                }
                finally
                {
                    _animationWaitHandle = null;
                }
            }
#endif

            if (_centerPen != null)
            {
                try
                {
                    _centerPen.Dispose();
                }
                finally
                {
                    _centerPen = null;
                }
            }
            if (_minorTickPen != null)
            {
                try
                {
                    _minorTickPen.Dispose();
                }
                finally
                {
                    _minorTickPen = null;
                }
            }
            if (_majorTickPen != null)
            {
                try
                {
                    _majorTickPen.Dispose();
                }
                finally
                {
                    _majorTickPen = null;
                }
            }
            if (_directionLabelFont != null)
            {
                try
                {
                    _directionLabelFont.Dispose();
                }
                finally
                {
                    _directionLabelFont = null;
                }
            }
            if (_directionTickPen != null)
            {
                try
                {
                    _directionTickPen.Dispose();
                }
                finally
                {
                    _directionTickPen = null;
                }
            }
            if (_angleLabelFont != null)
            {
                try
                {
                    _angleLabelFont.Dispose();
                }
                finally
                {
                    _angleLabelFont = null;
                }
            }
#if !PocketPC
            if (_pNeedleShadowBrush != null)
            {
                _pNeedleShadowBrush.Dispose();
                _pNeedleShadowBrush = null;
            }
#endif
            if (_pNorthNeedleBrush != null)
            {
                try
                {
                    _pNorthNeedleBrush.Dispose();
                }
                finally
                {
                    _pNorthNeedleBrush = null;
                }
            }
            if (_pNorthNeedlePen != null)
            {
                try
                {
                    _pNorthNeedlePen.Dispose();
                }
                finally
                {
                    _pNorthNeedlePen = null;
                }
            }
            if (_pSouthNeedleBrush != null)
            {
                try
                {
                    _pSouthNeedleBrush.Dispose();
                }
                finally
                {
                    _pSouthNeedleBrush = null;
                }
            }
            if (_pSouthNeedlePen != null)
            {
                try
                {
                    _pSouthNeedlePen.Dispose();
                }
                finally
                {
                    _pSouthNeedlePen = null;
                }
            }
            if (_directionLabelBrush != null)
            {
                try
                {
                    _directionLabelBrush.Dispose();
                }
                finally
                {
                    _directionLabelBrush = null;
                }
            }
            if (_angleLabelBrush != null)
            {
                try
                {
                    _angleLabelBrush.Dispose();
                }
                finally
                {
                    _angleLabelBrush = null;
                }
            }

            // Move on down the line
            base.Dispose(disposing);
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the direction that the needle points to.
        /// </summary>
        [DefaultValue(typeof(Azimuth), "0")]
        [Category("Appearance")]
        [Description("Controls the direction that the needle points to.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Azimuth Value
        {
            get
            {
                return _bearing;
            }
            set
            {
                if (_bearing.Equals(value)) return;
                _bearing = value;
#if PocketPC
                InvokeRepaint();
#else
                if (IsDisposed)
                    return;

                lock (_valueInterpolator)
                {
                    // Are we changing direction?
                    if (_bearing.DecimalDegrees >= _valueInterpolator.Minimum
                        && _bearing.DecimalDegrees > _valueInterpolator[_interpolationIndex])
                    {
                        // No.  Just set the new maximum
                        _valueInterpolator.Maximum = _bearing.DecimalDegrees;
                    }
                    else if (_bearing.DecimalDegrees < _valueInterpolator.Minimum)
                    {
                        // We're changing directions, so stop then accellerate again
                        _valueInterpolator.Minimum = _valueInterpolator[_interpolationIndex];
                        _valueInterpolator.Maximum = _bearing.DecimalDegrees;
                        _interpolationIndex = 0;
                    }
                    else if (_bearing.DecimalDegrees > _valueInterpolator.Minimum
                        && _bearing.DecimalDegrees < _valueInterpolator[_interpolationIndex])
                    {
                        // We're changing directions, so stop then accellerate again
                        _valueInterpolator.Minimum = _valueInterpolator[_interpolationIndex];
                        _valueInterpolator.Maximum = _bearing.DecimalDegrees;
                        _interpolationIndex = 0;
                    }
                    else if (_bearing.DecimalDegrees > _valueInterpolator.Maximum)
                    {
                        // No.  Just set the new maximum
                        _valueInterpolator.Maximum = _bearing.DecimalDegrees;
                    }

                    // If the difference is > 180°, adjust so that it moves the right direction
                    if (_valueInterpolator.Maximum - _valueInterpolator.Minimum > 180)
                        _valueInterpolator.Minimum = _valueInterpolator.Minimum % 360.0 + 360.0;
                    else if (_valueInterpolator.Minimum - _valueInterpolator.Maximum > 180)
                        _valueInterpolator.Maximum = _valueInterpolator.Maximum % 360.0 + 360.0;
                }
                // And activate the interpolation thread
                //				if ((InterpolationThread.ThreadState & ThreadState.Suspended) != 0)
                //					InterpolationThread.Resume();
                _animationWaitHandle.Set();

#endif

                OnValueChanged(new AzimuthEventArgs(_bearing));
            }
        }

        private void OnValueChanged(AzimuthEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }

#if !PocketPC

        /// <summary>
        /// Controls how the control smoothly transitions from one value to another.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(InterpolationMethod), "CubicEaseOut")]
        [Description("Controls how the control smoothly transitions from one value to another.")]
        public InterpolationMethod ValueInterpolationMethod
        {
            get
            {
                return _valueInterpolator.InterpolationMethod;
            }
            set
            {
                _valueInterpolator.InterpolationMethod = value;
            }
        }

#endif

#if !PocketPC || DesignTime
#if PocketPC
        [DefaultValue(typeof(Angle), "0")]
#else

        /// <summary>
        /// Controls the number of degrees in between each label around the control.
        /// </summary>
        [DefaultValue(typeof(Angle), "30.0")]
#endif
        [Category("Angle Labels")]
        [Description("Controls the number of degrees in between each label around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Angle AngleLabelInterval
        {
            get
            {
                return _angleLabelInterval;
            }
            set
            {
                if (_angleLabelInterval.Equals(value)) return;
                _angleLabelInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of degree labels drawn around the control.
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Angle Labels")]
        [Description("Controls the color of degree labels drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color AngleLabelColor
        {
            get
            {
                return _angleLabelBrush.Color;
            }
            set
            {
                if (_angleLabelBrush.Color.Equals(value)) return;
                _angleLabelBrush.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime
#if PocketPC
        [DefaultValue(typeof(Font), "Tahoma, 6pt")]
#else

        /// <summary>
        /// Controls the font of degree labels drawn around the control.
        /// </summary>
        [DefaultValue(typeof(Font), "Tahoma, 8pt")]
#endif
        [Category("Angle Labels")]
        [Description("Controls the font of degree labels drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Font AngleLabelFont
        {
            get
            {
                return _angleLabelFont;
            }
            set
            {
                if (_angleLabelFont.Equals(value)) return;
                _angleLabelFont = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the number of degrees in between each compass direction (i.e. \"N\", \"NW\") around the control.
        /// </summary>
        [Category("Direction Labels")]
        [DefaultValue(typeof(Angle), "45")]
        [Description("Controls the number of degrees in between each compass direction (i.e. \"N\", \"NW\") around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Angle DirectionLabelInterval
        {
            get
            {
                return _directionLabelInterval;
            }
            set
            {
                if (_directionLabelInterval.Equals(value)) return;
                _directionLabelInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of compass labels on the control.
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Direction Labels")]
        [Description("Controls the color of compass labels on the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color DirectionLabelColor
        {
            get
            {
                return _directionLabelBrush.Color;
            }
            set
            {
                if (_directionLabelBrush.Color.Equals(value)) return;
                _directionLabelBrush.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of tick marks next to each direction label on the control.
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Direction Labels")]
        [Description("Controls the color of tick marks next to each direction label on the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color DirectionTickColor
        {
            get
            {
                return _directionTickPen.Color;
            }
            set
            {
                if (_directionTickPen.Color.Equals(value)) return;
                _directionTickPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls whether the Value property is set manually, or automatically read from any available GPS device.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(bool), "False")]
        [Description("Controls whether the Value property is set manually, or automatically read from any available GPS device.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public bool IsUsingRealTimeData
        {
            get
            {
                return _pIsUsingRealTimeData;
            }
            set
            {
                // Anything to do?
                if (_pIsUsingRealTimeData == value)
                    return;

                // Record the new value
                _pIsUsingRealTimeData = value;

                // Set the control to the last known bearing (if any)
                if (_pIsUsingRealTimeData)
                {
                    // Hook into real-time events.
                    // Only hook into events if we're at run-time.  Hooking events
                    // at design-time can actually cause errors in the WF Designer.
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        Devices.BearingChanged += Devices_CurrentBearingChanged;
                    }

                    // Set the current bearing
                    Value = Devices.Bearing;
                }
                else
                {
                    // Only hook into events if we're at run-time.  Hooking events
                    // at design-time can actually cause errors in the WF Designer.
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        Devices.BearingChanged -= Devices_CurrentBearingChanged;
                    }

                    // Set a bearing of zero
                    Value = Azimuth.North;
                }

                // Tell the control to repaint
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime
#if PocketPC
        [DefaultValue(typeof(Font), "Tahoma, 8pt, style=Bold")]
#else

        /// <summary>
        /// Controls the font used to draw direction labels on the control.
        /// </summary>
        [DefaultValue(typeof(Font), "Tahoma, 12pt, style=Bold")]
#endif
        [Category("Direction Labels")]
        [Description("Controls the font used to draw direction labels on the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Font DirectionLabelFont
        {
            get
            {
                return _directionLabelFont;
            }
            set
            {
                if (_directionLabelFont.Equals(value)) return;
                _directionLabelFont = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the output format of labels drawn around the control. (i.e.  h°m's\")"
        /// </summary>
        [DefaultValue(typeof(string), "h°")]
        [Category("Angle Labels")]
        [Description("Controls the output format of labels drawn around the control. (i.e.  h°m's\")")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public string AngleLabelFormat
        {
            get
            {
                return _angleLabelFormat;
            }
            set
            {
                if (_angleLabelFormat.Equals(value)) return;
                _angleLabelFormat = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the number of degrees in between each small tick mark around the control.
        /// </summary>
        [Category("Tick Marks")]
        [Description("Controls the number of degrees in between each small tick mark around the control.")]
#if PocketPC
        [DefaultValue(typeof(Angle), "5")]
#else
        [DefaultValue(typeof(Angle), "2")]
#endif
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Angle MinorTickInterval
        {
            get
            {
                return _minorTickInterval;
            }
            set
            {
                if (_minorTickInterval == value) return;
                _minorTickInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of smaller tick marks drawn around the control.
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Tick Marks")]
        [Description("Controls the color of smaller tick marks drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color MinorTickColor
        {
            get
            {
                return _minorTickPen.Color;
            }
            set
            {
                _minorTickPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the number of degrees in between each larger tick mark around the control.
        /// </summary>
        [DefaultValue(typeof(Angle), "15")]
        [Category("Tick Marks")]
        [Description("Controls the number of degrees in between each larger tick mark around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        ////[CLSCompliant(false)]
        public Angle MajorTickInterval
        {
            get
            {
                return _majorTickInterval;
            }
            set
            {
                if (_majorTickInterval == value) return;
                _majorTickInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of larger tick marks drawn around the control.
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Tick Marks")]
        [Description("Controls the color of larger tick marks drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color MajorTickColor
        {
            get
            {
                return _majorTickPen.Color;
            }
            set
            {
                _majorTickPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the interior of the needle which points North.
        /// </summary>
        [DefaultValue(typeof(Color), "Red")]
        [Category("Compass Needle")]
        [Description("Controls the color of the interior of the needle which points North.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color NorthNeedleFillColor
        {
            get
            {
                return _pNorthNeedleBrush.Color;
            }
            set
            {
                if (_pNorthNeedleBrush.Color.Equals(value)) return;
                _pNorthNeedleBrush.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the edge of the needle which points North.
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Compass Needle")]
        [Description("Controls the color of the edge of the needle which points North.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color NorthNeedleOutlineColor
        {
            get
            {
                return _pNorthNeedlePen.Color;
            }
            set
            {
                if (_pNorthNeedlePen.Color.Equals(value)) return;
                _pNorthNeedlePen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the interior of the needle which points South.
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Category("Compass Needle")]
        [Description("Controls the color of the interior of the needle which points South.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color SouthNeedleFillColor
        {
            get
            {
                return _pSouthNeedleBrush.Color;
            }
            set
            {
                if (_pSouthNeedleBrush.Color.Equals(value)) return;
                _pSouthNeedleBrush.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the edge of the needle which points South.
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Category("Compass Needle")]
        [Description("Controls the color of the edge of the needle which points South.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color SouthNeedleOutlineColor
        {
            get
            {
                return _pSouthNeedlePen.Color;
            }
            set
            {
                if (_pSouthNeedlePen.Color.Equals(value)) return;
                _pSouthNeedlePen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC

        /// <summary>
        /// Controls the color of the shadow cast by the compass needle.
        /// </summary>
        [DefaultValue(typeof(Color), "128, 0, 0, 0")]
        [Category("Appearance")]
        [Description("Controls the color of the shadow cast by the compass needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color NeedleShadowColor
        {
            get
            {
                return _pNeedleShadowBrush.Color;
            }
            set
            {
                if (_pNeedleShadowBrush.Color.Equals(value)) return;
                _pNeedleShadowBrush.Color = value;
                InvokeRepaint();
            }
        }

#endif

#if !PocketPC

        /// <summary>
        /// Controls the size of the shadow cast by the compass needle.
        /// </summary>
        [DefaultValue(typeof(Size), "5, 5")]
        [Category("Appearance")]
        [Description("Controls the size of the shadow cast by the compass needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Size NeedleShadowSize
        {
            get
            {
                return _pNeedleShadowSize;
            }
            set
            {
                _pNeedleShadowSize = value;
                InvokeRepaint();
            }
        }

#endif

        /// <inheritdocs/>
        protected override void OnPaintOffScreen(PaintEventArgs e)
        {
            PolarGraphics f = CreatePolarGraphics(e.Graphics);

            // What bearing are we drawing?
#if PocketPC
            Azimuth BearingToRender = _Bearing;
#else
            Azimuth bearingToRender = new Azimuth(_valueInterpolator[_interpolationIndex]);
#endif

            // Cache drawing options in order to prevent race conditions during
            // drawing!
            double minorInterval = _minorTickInterval.DecimalDegrees;
            double majorInterval = _majorTickInterval.DecimalDegrees;
            double directionInterval = _directionLabelInterval.DecimalDegrees;
            double angleInterval = _angleLabelInterval.DecimalDegrees;

            // Draw tick marks
            if (minorInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += minorInterval)
                {
                    // And draw a line
                    f.DrawLine(_minorTickPen, new PolarCoordinate(98, angle), new PolarCoordinate(100, angle));
                }
            }
            // Draw tick marks
            if (majorInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += majorInterval)
                {
                    // And draw a line
                    f.DrawLine(_majorTickPen, new PolarCoordinate(95, angle), new PolarCoordinate(100, angle));
                }
            }
            if (directionInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += directionInterval)
                {
                    // And draw a line
                    f.DrawLine(_directionTickPen, new PolarCoordinate(92, angle), new PolarCoordinate(100, angle));
                }
            }
            if (angleInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += angleInterval)
                {
                    // Get the coordinate of the line's start
                    PolarCoordinate start = new PolarCoordinate(60, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
#if PocketPC
                    f.DrawCenteredString(((Angle)angle).ToString(_AngleLabelFormat, CultureInfo.CurrentCulture), _AngleLabelFont, _AngleLabelBrush, start);
#else
                    f.DrawRotatedString(((Angle)angle).ToString(_angleLabelFormat, CultureInfo.CurrentCulture), _angleLabelFont, _angleLabelBrush, start);
#endif
                }
            }
            if (directionInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += directionInterval)
                {
                    // Get the coordinate of the line's start
                    PolarCoordinate start = new PolarCoordinate(80, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
#if PocketPC
                    f.DrawCenteredString(((Azimuth)angle).ToString("c", CultureInfo.CurrentCulture), _DirectionLabelFont, _DirectionLabelBrush, start);
#else
                    f.DrawRotatedString(((Azimuth)angle).ToString("c", CultureInfo.CurrentCulture), _directionLabelFont, _directionLabelBrush, start);
#endif
                }
            }

            // Draw an ellipse at the center
            f.DrawEllipse(_centerPen, PolarCoordinate.Empty, 10);

            // Now draw the needle shadow
            PolarCoordinate[] needleNorth = _needlePointsNorth.Clone() as PolarCoordinate[];
            PolarCoordinate[] needleSouth = _needlePointsSouth.Clone() as PolarCoordinate[];

            // Adjust the needle to the current bearing
            if (needleNorth != null)
                for (int index = 0; index < needleNorth.Length; index++)
                {
                    needleNorth[index] = needleNorth[index].Rotate(bearingToRender.DecimalDegrees);
                    if (needleSouth != null) needleSouth[index] = needleSouth[index].Rotate(bearingToRender.DecimalDegrees);
                }

#if !PocketPC
            // Now draw a shadow
            f.Graphics.TranslateTransform(_pNeedleShadowSize.Width, _pNeedleShadowSize.Height, MatrixOrder.Append);

            f.FillPolygon(_pNeedleShadowBrush, needleNorth);
            f.FillPolygon(_pNeedleShadowBrush, needleSouth);

            f.Graphics.ResetTransform();
#endif

            f.FillPolygon(_pNorthNeedleBrush, needleNorth);
            f.DrawPolygon(_pNorthNeedlePen, needleNorth);
            f.FillPolygon(_pSouthNeedleBrush, needleSouth);
            f.DrawPolygon(_pSouthNeedlePen, needleSouth);
        }

        private void Devices_CurrentBearingChanged(object sender, AzimuthEventArgs e)
        {
            if (_pIsUsingRealTimeData)
                Value = e.Azimuth;
        }

#if !PocketPC

        /// <inheritdocs/>
        protected override void OnTargetFrameRateChanged(int framesPerSecond)
        {
            base.OnTargetFrameRateChanged(framesPerSecond);
            // Recalculate our things
            _valueInterpolator.Count = framesPerSecond;
            // Adjust the index if it's outside of bounds
            if (_interpolationIndex > _valueInterpolator.Count - 1)
                _interpolationIndex = _valueInterpolator.Count - 1;
        }

        //
        //		protected override void OnHandleCreated(EventArgs e)
        //		{
        //			try
        //			{
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //			finally
        //			{
        //				base.OnHandleCreated(e);
        //			}
        //
        //		}
        //
        //		protected override void OnHandleDestroyed(EventArgs e)
        //		{
        //			try
        //			{
        //				// Get the interpolation thread out of a loop
        //				IsInterpolationActive = false;
        //
        //				if (InterpolationThread != null)
        //				{
        //					if ((InterpolationThread.ThreadState & ThreadState.Suspended) != 0)
        //						InterpolationThread.Resume();
        //					while (!InterpolationThread.Join(1000))
        //					{
        //						if ((InterpolationThread.ThreadState & ThreadState.AbortRequested) == 0)
        //							InterpolationThread.Abort();
        //					}
        //				}
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //			finally
        //			{
        //				base.OnHandleDestroyed(e);
        //			}
        //		}

        private void InterpolationLoop()
        {
            // Flag that we're alive
            //InterpolationThreadWaitHandle.Set();
            // Are we at the end?
            while (_isInterpolationActive)
            {
                try
                {
                    // Wait for interpolation to actually be needed
                    _animationWaitHandle.WaitOne();
                    //InterpolationThread.Suspend();
                    // If we're shutting down, just exit
                    if (!_isInterpolationActive)
                        break;
                    // Keep updating interpolation until we're done
                    while (_isInterpolationActive && _interpolationIndex < _valueInterpolator.Count)
                    {
                        // Render the next value
                        InvokeRepaint();
                        _interpolationIndex++;
                        // Wait for the next frame
                        Thread.Sleep(1000 / _valueInterpolator.Count);
                    }
                    // Reset interpolation
                    _valueInterpolator.Minimum = _valueInterpolator.Maximum;
                    _interpolationIndex = 0;
                    _animationWaitHandle.Reset();
                }
                catch (ThreadAbortException)
                {
                    // Just exit!
                    break;
                }
                catch
                {
                }
            }
            // Flag that we're alive
            //InterpolationThreadWaitHandle.Set();
        }

#endif
    }
}