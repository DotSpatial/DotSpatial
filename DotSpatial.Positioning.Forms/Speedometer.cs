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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

#if !PocketPC || DesignTime || Framework20

using System.ComponentModel;

#endif

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a user control used to measure speed graphically.
    /// </summary>
    [ToolboxBitmap(typeof(Speedometer))]
    [DefaultProperty("Value")]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
#endif
    public sealed class Speedometer : PolarControl
    {
#if !PocketPC
        private readonly Thread _interpolationThread;
        private bool _isInterpolationActive;
        //private ManualResetEvent InterpolationThreadWaitHandle = new ManualResetEvent(false);
        private readonly ManualResetEvent _animationWaitHandle = new ManualResetEvent(false);
        private readonly Interpolator _valueInterpolator = new Interpolator(15, InterpolationMethod.CubicEaseInOut);
        private int _interpolationIndex;

        private Size _pNeedleShadowSize = new Size(5, 5);
#endif
        private Color _minorTickPenColor = Color.Black;
        private Color _majorTickPenColor = Color.Black;
        private Color _speedLabelBrushColor = Color.Black;
        private Color _needleShadowBrushColor = Color.FromArgb(128, 0, 0, 0);
        private Speed _pMaximumSpeed = new Speed(120, SpeedUnit.KilometersPerHour);
        private Speed _pSpeedLabelInterval = new Speed(10, SpeedUnit.KilometersPerHour);
        private Speed _pMinorTickInterval = new Speed(5, SpeedUnit.KilometersPerHour);
        private Speed _pMajorTickInterval = new Speed(10, SpeedUnit.KilometersPerHour);
        private string _pSpeedLabelFormat = "v";

        private Speed _pSpeed = new Speed(0, SpeedUnit.MetersPerSecond);
        private Color _needleFillColor = Color.Red;
        private Color _needleOutlineColor = Color.Black;
        private Angle _pMinimumAngle = new Angle(40);
        private Angle _pMaximumAngle = new Angle(320);
        private static readonly PolarCoordinate[] _speedometerNeedle = new[]
			{
				new PolarCoordinate(70, new Angle(1), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(90), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(95), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(100), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(105), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(110), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(115), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(120), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(125), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(130), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(135), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(140), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(145), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(150), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(155), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(160), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(165), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(170), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(175), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(180), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(185), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(190), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(195), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(200), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(205), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(210), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(215), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(220), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(225), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(230), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(235), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(240), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(245), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(250), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(255), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(260), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(265), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, new Angle(270), Azimuth.South, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(70, new Angle(359), Azimuth.South, PolarCoordinateOrientation.Clockwise)
			};
        private bool _pIsUsingRealTimeData;
        private bool _pIsUnitLabelVisible = true;
        private double _conversionFactor;

#if (PocketPC && Framework20)
		private const int MaximumGracefulShutdownTime = 2000;
#elif !PocketPC
        private const int MAXIMUM_GRACEFUL_SHUTDOWN_TIME = 500;
#endif

        ///<summary>
        /// Occurs when the value changed
        ///</summary>
        public event EventHandler<SpeedEventArgs> ValueChanged;

        /// <summary>
        /// Speedometer
        /// </summary>
        public Speedometer()
            : base("DotSpatial.Positioning Multithreaded Speedometer Control (http://dotspatial.codeplex.com)")
        {
#if !PocketPC
            // Start the interpolation thread
            _interpolationThread = new Thread(InterpolationLoop)
                                       {
                                           IsBackground = true,
                                           Name =
                                               "DotSpatial.Positioning Speedometer Needle Animation Thread (http://dotspatial.codeplex.com)"
                                       };
            _isInterpolationActive = true;
            _interpolationThread.Start();

#endif
            // Set the control to display clockwise from north
            Orientation = PolarCoordinateOrientation.Clockwise;
            Origin = Azimuth.South;
            // The center is zero and edge is 100
            CenterR = 0;
            MaximumR = 100;

            // Use the speed depending on the local culture
            if (RegionInfo.CurrentRegion.IsMetric)
            {
                _pMaximumSpeed = new Speed(120, SpeedUnit.KilometersPerHour);
                _pSpeedLabelInterval = new Speed(10, SpeedUnit.KilometersPerHour);
#if PocketPC
                Font = new Font("Tahoma", 7.0f, FontStyle.Regular);
				_pMinorTickInterval = new Speed(5, SpeedUnit.KilometersPerHour);
#else
                _pMinorTickInterval = new Speed(1, SpeedUnit.KilometersPerHour);
                Font = new Font("Tahoma", 11.0f, FontStyle.Regular);
#endif
                _pMajorTickInterval = new Speed(10, SpeedUnit.KilometersPerHour);
            }
            else
            {
                _pMaximumSpeed = new Speed(120, SpeedUnit.StatuteMilesPerHour);
                _pSpeedLabelInterval = new Speed(10, SpeedUnit.StatuteMilesPerHour);
#if PocketPC
				_pMinorTickInterval = new Speed(5, SpeedUnit.StatuteMilesPerHour);
#else
                _pMinorTickInterval = new Speed(1, SpeedUnit.StatuteMilesPerHour);
#endif
                _pMajorTickInterval = new Speed(10, SpeedUnit.StatuteMilesPerHour);
            }

            // Calculate the factor for converting speed into an angle
            _conversionFactor = (_pMaximumAngle.DecimalDegrees - _pMinimumAngle.DecimalDegrees) / _pMaximumSpeed.Value;
        }

        /// <inheritdocs/>
        protected override void Dispose(bool disposing)
        {
            // Only hook into events if we're at run-time.  Hooking events
            // at design-time can actually cause errors in the WF Designer.
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime
                && _pIsUsingRealTimeData)
            {
                Devices.SpeedChanged -= Devices_CurrentSpeedChanged;
            }

#if !PocketPC
            // Get the interpolation thread out of a loop
            _isInterpolationActive = false;

            if (_interpolationThread != null)
            {
                if (_animationWaitHandle != null) _animationWaitHandle.Set();
                if (!_interpolationThread.Join(MAXIMUM_GRACEFUL_SHUTDOWN_TIME)) _interpolationThread.Abort();
            }

            if (_animationWaitHandle != null) _animationWaitHandle.Close();

#endif

            base.Dispose(disposing);
        }

#if Framework20 && !PocketPC

        /// <summary>
        /// The azimuth angle of hte origin
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Azimuth Origin
        {
            get
            {
                return base.Origin;
            }
            set
            {
                base.Origin = value;
            }
        }

        /// <summary>
        /// The rotation angle
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Angle Rotation
        {
            get
            {
                return base.Rotation;
            }
            set
            {
                base.Rotation = value;
            }
        }

#endif

        /// <inheritdocs/>
        protected override void OnPaintOffScreen(PaintEventArgs e)
        {
            PolarGraphics f = CreatePolarGraphics(e.Graphics);

            // What altitude are we drawing?
#if PocketPC
            Speed SpeedToRender = pSpeed;
#else
            Speed speedToRender = new Speed(_valueInterpolator[_interpolationIndex], _pSpeed.Units);
#endif

            // Cache drawing intervals and such to prevent a race condition
            double minorInterval = _pMinorTickInterval.Value;
            double majorInterval = _pMajorTickInterval.Value;
            double cachedSpeedLabelInterval = _pSpeedLabelInterval.ToUnitType(_pMaximumSpeed.Units).Value;
            Speed maxSpeed = _pMaximumSpeed.Clone();
            double minorStep = _pMinorTickInterval.ToUnitType(_pMaximumSpeed.Units).Value;
            double majorStep = _pMajorTickInterval.ToUnitType(_pMaximumSpeed.Units).Value;

            // Draw tick marks
            double angle;
            PolarCoordinate start;
            PolarCoordinate end;

            #region Draw minor tick marks

            if (minorInterval > 0)
            {
                for (double speed = 0; speed < maxSpeed.Value; speed += minorStep)
                {
                    // Convert the speed to an angle
                    angle = speed * _conversionFactor + _pMinimumAngle.DecimalDegrees;
                    // Get the coordinate of the line's start
                    start = new PolarCoordinate(95, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    end = new PolarCoordinate(100, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    // And draw a line
                    Pen p = new Pen(_minorTickPenColor);
                    f.DrawLine(p, start, end);
                    p.Dispose();
                }
            }

            #endregion

            #region Draw major tick marks

            if (majorInterval > 0)
            {
                using (Pen majorPen = new Pen(_majorTickPenColor))
                {
                    for (double speed = 0; speed < maxSpeed.Value; speed += majorStep)
                    {
                        // Convert the speed to an angle
                        angle = speed * _conversionFactor + _pMinimumAngle.DecimalDegrees;
                        // Get the coordinate of the line's start
                        start = new PolarCoordinate(90, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                        end = new PolarCoordinate(100, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                        // And draw a line

                        f.DrawLine(majorPen, start, end);
                    }

                    #region Draw a major tick mark at the maximum speed

                    // Convert the speed to an angle
                    angle = maxSpeed.Value * _conversionFactor + _pMinimumAngle.DecimalDegrees;
                    // Get the coordinate of the line's start
                    start = new PolarCoordinate(90, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    end = new PolarCoordinate(100, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    // And draw a line
                    f.DrawLine(majorPen, start, end);

                    #endregion
                }
            }

            #endregion

            using (SolidBrush fontBrush = new SolidBrush(_speedLabelBrushColor))
            {
                if (cachedSpeedLabelInterval > 0)
                {
                    for (double speed = 0; speed < maxSpeed.Value; speed += cachedSpeedLabelInterval)
                    {
                        // Convert the speed to an angle
                        angle = speed * _conversionFactor + _pMinimumAngle.DecimalDegrees;
                        // And draw a line
                        f.DrawCenteredString(new Speed(speed, maxSpeed.Units).ToString(_pSpeedLabelFormat, CultureInfo.CurrentCulture), Font, fontBrush,
                            new PolarCoordinate(75, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise));
                    }

                    // Convert the speed to an angle
                    angle = maxSpeed.Value * _conversionFactor + _pMinimumAngle.DecimalDegrees;

                    // And draw the speed label
                    f.DrawCenteredString(maxSpeed.ToString(_pSpeedLabelFormat, CultureInfo.CurrentCulture), Font, fontBrush,
                        new PolarCoordinate(75, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise));
                }

                // Draw the units for the speedometer
                if (_pIsUnitLabelVisible)
                {
                    f.DrawCenteredString(_pMaximumSpeed.ToString("u", CultureInfo.CurrentCulture), Font, fontBrush,
                        new PolarCoordinate(90, Angle.Empty, Azimuth.South, PolarCoordinateOrientation.Clockwise));
                }
            }

            PolarCoordinate[] needle = new PolarCoordinate[_speedometerNeedle.Length];
            for (int index = 0; index < needle.Length; index++)
            {
                needle[index] = _speedometerNeedle[index].Rotate((speedToRender.ToUnitType(_pMaximumSpeed.Units).Value * _conversionFactor) + _pMinimumAngle.DecimalDegrees);
            }

            // Draw an ellipse at the center
            f.DrawEllipse(Pens.Gray, PolarCoordinate.Empty, 10);

#if !PocketPC
            // Now draw a shadow
            f.Graphics.TranslateTransform(_pNeedleShadowSize.Width, _pNeedleShadowSize.Height, MatrixOrder.Append);
            using (Brush b = new SolidBrush(_needleShadowBrushColor)) f.FillPolygon(b, needle);

            f.Graphics.ResetTransform();
#endif

            // Then draw the actual needle
            using (SolidBrush needleFill = new SolidBrush(_needleFillColor)) f.FillPolygon(needleFill, needle);
            using (Pen needlePen = new Pen(_needleOutlineColor)) f.DrawPolygon(needlePen, needle);
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Gets or sets the spedometer needle color of the edge
        /// </summary>
        [Category("Speedometer Needle")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of the edge of the needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color NeedleOutlineColor
        {
            get { return _needleOutlineColor; }
            set
            {
                _needleOutlineColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Gets or sets the needle fill color.
        /// </summary>
        [Category("Speedometer Needle")]
        [DefaultValue(typeof(Color), "Red")]
        [Description("Controls the color of the interior of the needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color NeedleFillColor
        {
            get
            {
                return _needleFillColor;
            }
            set
            {
                _needleFillColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC

        /// <summary>
        /// The Needle Shadow Brush color intially semitransparent black.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "128, 0, 0, 0")]
        [Description("Controls the color of the shadow cast by the needle.")]
        public Color NeedleShadowColor
        {
            get { return _needleShadowBrushColor; }
            set
            {
                _needleShadowBrushColor = value;
                InvokeRepaint();
            }
        }

#endif

#if !PocketPC

        /// <summary>
        ///
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(Size), "5, 5")]
        [Description("Controls the size of the shadow cast by the needle.")]
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

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the amount of speed being displayed in the control
        /// </summary>
        [Category("Behavior")]
        [Description("Controls the amount of speed being displayed in the control.")]
        [DefaultValue(typeof(Speed), "0 m/s")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        ////[CLSCompliant(false)]
        public Speed Value
        {
            get
            {
                return _pSpeed;
            }
            set
            {
                if (_pSpeed.Equals(value)) return;
                _pSpeed = value.ToUnitType(_pMaximumSpeed.Units);
#if PocketPC
                InvokeRepaint();
#else
                if (IsDisposed)
                    return;

                lock (_valueInterpolator)
                {
                    // Are we changing direction?
                    if (_pSpeed.Value >= _valueInterpolator.Minimum
                        && _pSpeed.Value > _valueInterpolator[_interpolationIndex])
                    {
                        // No.  Just set the new maximum
                        _valueInterpolator.Maximum = _pSpeed.Value;
                    }
                    else if (_pSpeed.Value < _valueInterpolator.Minimum)
                    {
                        // We're changing directions, so stop then accellerate again
                        _valueInterpolator.Minimum = _valueInterpolator[_interpolationIndex];
                        _valueInterpolator.Maximum = _pSpeed.Value;
                        _interpolationIndex = 0;
                    }
                    else if (_pSpeed.Value > _valueInterpolator.Minimum
                        && _pSpeed.Value < _valueInterpolator[_interpolationIndex])
                    {
                        // We're changing directions, so stop then accellerate again
                        _valueInterpolator.Minimum = _valueInterpolator[_interpolationIndex];
                        _valueInterpolator.Maximum = _pSpeed.Value;
                        _interpolationIndex = 0;
                    }
                    else if (_pSpeed.Value > _valueInterpolator.Maximum)
                    {
                        // No.  Just set the new maximum
                        _valueInterpolator.Maximum = _pSpeed.Value;
                    }
                }
                // And activate the interpolation thread
                _animationWaitHandle.Set();
#endif

                OnValueChanged(new SpeedEventArgs(_pSpeed));
            }
        }

        private void OnValueChanged(SpeedEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }

#if !PocketPC

        /// <summary>
        /// Controls how the control smoothly transitions from one value to another.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(InterpolationMethod), "CubicEaseInOut")]
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

        /// <summary>
        /// Controls the fastest speed allowed by the control.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(Speed), "120 km/h")]
        [Description("Controls the fastest speed allowed by the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        ////[CLSCompliant(false)]
        public Speed MaximumSpeed
        {
            get
            {
                return _pMaximumSpeed;
            }
            set
            {
                if (_pMaximumSpeed.Equals(value)) return;
                _pMaximumSpeed = value;
                // Calculate the factor for converting speed into an angle
                _conversionFactor = (_pMaximumAngle.DecimalDegrees - _pMinimumAngle.DecimalDegrees) / _pMaximumSpeed.Value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the amount of speed in between each label around the control.
        /// </summary>
        [Category("Speed Label")]
        [DefaultValue(typeof(Speed), "10 km/h")]
        [Description("Controls the amount of speed in between each label around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Speed SpeedLabelInterval
        {
            get
            {
                return _pSpeedLabelInterval;
            }
            set
            {
                if (_pSpeedLabelInterval.Equals(value)) return;
                _pSpeedLabelInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the number of degrees in between each smaller tick mark around the control.
        /// </summary>
        [Category("Tick Marks")]
        [DefaultValue(typeof(Speed), "5 km/h")]
        [Description("Controls the number of degrees in between each smaller tick mark around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Speed MinorTickInterval
        {
            get
            {
                return _pMinorTickInterval;
            }
            set
            {
                if (_pMinorTickInterval.Equals(value)) return;
                _pMinorTickInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the number of degrees in between each larger tick mark around the control.
        /// </summary>
        [Category("Tick Marks")]
        [DefaultValue(typeof(Speed), "10 km/h")]
        [Description("Controls the number of degrees in between each larger tick mark around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Speed MajorTickInterval
        {
            get
            {
                return _pMajorTickInterval;
            }
            set
            {
                if (_pMajorTickInterval.Equals(value)) return;
                _pMajorTickInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// the color of the minor ticks.
        /// </summary>
        [Category("Tick Marks")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of smaller tick marks drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color MinorTickColor
        {
            get { return _minorTickPenColor; }
            set
            {
                _minorTickPenColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls whether the speed label is drawn in the center of the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(bool), "True")]
        [Description("Controls whether the speed label is drawn in the center of the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public bool IsUnitLabelVisible
        {
            get
            {
                return _pIsUnitLabelVisible;
            }
            set
            {
                if (_pIsUnitLabelVisible.Equals(value))
                    return;

                _pIsUnitLabelVisible = value;
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
                // Has nothing changed?
                if (_pIsUsingRealTimeData == value)
                    return;

                // Store the new value
                _pIsUsingRealTimeData = value;

                if (_pIsUsingRealTimeData)
                {
                    // Only hook into events if we're at run-time.  Hooking events
                    // at design-time can actually cause errors in the WF Designer.
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        Devices.SpeedChanged += Devices_CurrentSpeedChanged;
                    }

                    // Set the current real-time speed
                    Value = Devices.Speed;
                }
                else
                {
                    // Only hook into events if we're at run-time.  Hooking events
                    // at design-time can actually cause errors in the WF Designer.
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        Devices.SpeedChanged -= Devices_CurrentSpeedChanged;
                    }

                    // Reset the value to zero
                    Value = Speed.AtRest;
                }

                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Gets or sets the Major Tick Color
        /// </summary>
        [Category("Tick Marks")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of larger tick marks drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color MajorTickColor
        {
            get
            {
                return _majorTickPenColor;
            }
            set
            {
                _majorTickPenColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the display format used for speed labels drawn around the control.
        /// </summary>
        [Category("Speed Label")]
        [DefaultValue(typeof(string), "v")]
        [Description("Controls the display format used for speed labels drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public string SpeedLabelFormat
        {
            get
            {
                return _pSpeedLabelFormat;
            }
            set
            {
                if (_pSpeedLabelFormat.Equals(value)) return;
                _pSpeedLabelFormat = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <inheritdocs/>
        [Category("Speed Label")]
#if PocketPC
		[DefaultValue(typeof(Font), "Tahoma, 7pt")]
#else
        [DefaultValue(typeof(Font), "Tahoma, 11pt")]
#endif
        [Description("Controls the font used for speed labels drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Gets or sets the Speed Label font color
        /// </summary>
        [Category("Speed Label")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of speed labels drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color SpeedLabelColor
        {
            get
            {
                return _speedLabelBrushColor;
            }
            set
            {
                _speedLabelBrushColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the angle associated with the smallest possible speed.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(Angle), "40")]
        [Description("Controls the angle associated with the smallest possible speed.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Angle MinimumAngle
        {
            get
            {
                return _pMinimumAngle;
            }
            set
            {
                if (_pMinimumAngle.Equals(value)) return;
                _pMinimumAngle = value;
                // Calculate the factor for converting speed into an angle
                _conversionFactor = (_pMaximumAngle.DecimalDegrees - _pMinimumAngle.DecimalDegrees) / _pMaximumSpeed.Value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the angle associated with the largest possible speed.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(Angle), "320")]
        [Description("Controls the angle associated with the largest possible speed.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Angle MaximumAngle
        {
            get
            {
                return _pMaximumAngle;
            }
            set
            {
                if (_pMaximumAngle.Equals(value)) return;
                _pMaximumAngle = value;
                // Calculate the factor for converting speed into an angle
                _conversionFactor = (_pMaximumAngle.DecimalDegrees - _pMinimumAngle.DecimalDegrees) / _pMaximumSpeed.Value;
                InvokeRepaint();
            }
        }

        private void Devices_CurrentSpeedChanged(object sender, SpeedEventArgs e)
        {
            if (_pIsUsingRealTimeData)
                Value = e.Speed;
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
                    //InterpolationThread.Suspend();
                    _animationWaitHandle.WaitOne();
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
            }
            // Flag that we're alive
            //InterpolationThreadWaitHandle.Set();
        }

#endif
    }
}