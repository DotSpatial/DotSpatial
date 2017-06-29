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
using System.Windows.Forms;
using System.Threading;

#if !PocketPC || DesignTime || Framework20

using System.ComponentModel;

#endif

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Forms
{
    /// <summary>
    /// Indicates which time is displayed by the clock control.
    /// </summary>
    public enum ClockDisplayMode
    {
        /// <summary>
        /// GPS satellite signals are used to display the current time.
        /// </summary>
        SatelliteDerivedTime,
        /// <summary>
        /// The computer's system clock is used to display the current time.
        /// </summary>
        LocalMachineTime,
        /// <summary>
        /// A custom time will be displayed by setting the Value property manually.
        /// </summary>
        Manual
    }

#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a user control which displays the local or satellite-derived time.
    /// </summary>
    [ToolboxBitmap(typeof(Clock))]
    [DefaultProperty("Value")]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
#endif
    public sealed class Clock : PolarControl
    {
        #region Private Variables

#if PocketPC && !Framework20
		// Controls the pen used for small tick marks around the edge
		private Pen pMinorTickPen = new Pen(Color.Black);
		// Controls the pen used for larger tick marks around the edge
		private Pen pMajorTickPen = new Pen(Color.Black);
		// Controls the pen used for the hours hand
		private Pen pHoursPen = new Pen(Color.Black);
		// Controls the pen used for the minutes hand
		private Pen pMinutesPen = new Pen(Color.Black);
		// Controls the pen used for the seconds hand
		private Pen pSecondsPen = new Pen(Color.Red);
#else
        // Controls the pen used for small tick marks around the edge
        private readonly Pen _pMinorTickPen = new Pen(Color.Black, 1.0f);
        // Controls the pen used for larger tick marks around the edge
        private readonly Pen _pMajorTickPen = new Pen(Color.Black, 2.0f);
        // Controls the pen used for the hours hand
        private readonly Pen _pHoursPen = new Pen(Color.Black, 1.0f);
        // Controls the pen used for the minutes hand
        private readonly Pen _pMinutesPen = new Pen(Color.Black, 1.0f);
        // Controls the pen used for the seconds hand
        private readonly Pen _pSecondsPen = new Pen(Color.Red, 1.0f);
#endif
        // Controls the brush used for labeling numbers
        private readonly SolidBrush _pValueLabelBrush = new SolidBrush(Color.Black);
        // Controls the time drawn by the control
        private DateTime _pValue = DateTime.Now;
        // Controls whether time is updated automatically
        private ClockDisplayMode _pDisplayMode = ClockDisplayMode.Manual;
#if PocketPC
        // Controls the font for the clock
        private Font pHoursFont = new Font("Tahoma", 8.0f, FontStyle.Regular);
#else
        // Controls the font for the clock
        private Font _pHoursFont = new Font("Tahoma", 9.0f, FontStyle.Regular);
#endif
        // Updates the clock to the current time
        private Thread _clockThread;
#if PocketPC
        private TimeSpan pUpdateInterval = TimeSpan.FromMilliseconds(500);
#else
        private TimeSpan _pUpdateInterval = TimeSpan.FromMilliseconds(100);
#endif
        //#if PocketPC && !DesignTime
        //        private bool pIsClockThreadAlive;
        //#endif
#if !PocketPC || DesignTime
        private const int MAXIMUM_GRACEFUL_SHUTDOWN_TIME = 500;
#elif PocketPC && Framework20
		private const int MaximumGracefulShutdownTime = 500;
#endif

        #endregion

        /// <summary>
        /// Occurs when the value changes
        /// </summary>
        public event EventHandler<DateTimeEventArgs> ValueChanged;

        /// <summary>
        /// The constructor provides a name for the control.  This name is used as the name of the thread if multithreading is enabled.
        /// </summary>
        public Clock()
            : base("DotSpatial.Positioning Multithreaded Clock Control (http://dotspatial.codeplex.com)")
        {
            // Set the behavior of angles.  "Origin" means that 0° points up,
            // and "Orientation" states that larger angles go clockwise around the control.
            Origin = Azimuth.North;
            Orientation = PolarCoordinateOrientation.Clockwise;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // Unhook from events (but only at run-time.  Doing so at
            // design-time can cause WF errors.)
            if (_pDisplayMode == ClockDisplayMode.SatelliteDerivedTime
                && LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                Devices.UtcDateTimeChanged -= Devices_CurrentUtcDateTimeChanged;
            }

            // Free unmanaged GDI resources (very important!)
            _pHoursFont.Dispose();
            _pMinorTickPen.Dispose();
            _pMajorTickPen.Dispose();
            _pValueLabelBrush.Dispose();
            _pHoursPen.Dispose();
            _pMinutesPen.Dispose();
            _pSecondsPen.Dispose();

            // Shut down the clock thread
            DisplayMode = ClockDisplayMode.Manual;

            // Move on down the line
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is called whenever the control must be rendered.  All rendering takes
        /// place off-screen, which prevents flickering.  The "PolarControl" base class automatically
        /// handles tasks such as resizing and smoothing.  All you have to be concerned with is
        /// calculating the polar coordinates to draw.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintOffScreen(PaintEventArgs e)
        {
            /* The DotSpatial.Positioning comes with a special class named "PolarGraphics," which works
                * almost identically to the "Graphics" class, except that coordinates are given
                * in polar form instead of (X, Y).
                *
                * Polar coordinates are given as an angle and a radius.  The angle is between 0°
                * and 360° clockwise from the top of the control.  The radius is zero (0) at the
                * center of the control, and 100 at the outer edge.  So, a polar coordinate of 90°
                * with a radius of 50 would mark a point straight to the right, half way to the
                * edge.  Mastering polar coordinates gives you the opportunity to create your
                * own controls.
                */

            // Make a PolarGraphics class for easier drawing using polar coordinates
            PolarGraphics f = CreatePolarGraphics(e.Graphics);

            #region Drawing of Tick Marks

            // Draw sixty small tick marks around the control
            for (double value = 0; value < 60; value += 1)
            {
                // There are 120 tick marks in 360°
                Angle angle = new Angle(value * (360 / 60));

                // Get the coordinate of the line's start and end
                PolarCoordinate start = new PolarCoordinate(96, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);

                // And draw a line
                f.DrawLine(_pMinorTickPen, start, end);
            }

            // Draw twelve tick marks
            for (double value = 1; value <= 12; value += 1)
            {
                // Convert the value to an angle
                Angle angle = new Angle(value * (360 / 12));

                // Get the coordinate of the line's start
                PolarCoordinate start = new PolarCoordinate(93, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);

                // And draw the tick mark
                f.DrawLine(_pMajorTickPen, start, end);

                // Label the clock position around the circle
                if (_pHoursFont != null)
                {
                    string s = Convert.ToString(value, CultureInfo.CurrentUICulture);
                    f.DrawCenteredString(s, _pHoursFont, _pValueLabelBrush,
                        new PolarCoordinate(85, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise));
                }
            }

            #endregion

            /* In order to make the control more appealing, we'll make the hours, minutes, and
				* seconds hand as accurate as possible.  For example, if the time is 5:30, the hours
				* hand should be halfway between 5 and 6, or 5.5.  To get these values, we'll
				* calculate "fractional" hours, minutes and seconds using the TimeSpan structure.
				*
				* when you look at the control, you'll see that the minutes hand moves slowly but
				* smoothly as time progresses.  With smoothing enabled, you almost can't tell it's
				* actually moving.
				*/

            // Calculate the time elapsed since midnight
            DateTime midnight = new DateTime(_pValue.Year, _pValue.Month, _pValue.Day);
            TimeSpan timeSinceMidnight = _pValue.Subtract(midnight);

            #region Drawing of Hours

            // There are twelve hours in 360° of a circle
            Angle hourAngle = new Angle(timeSinceMidnight.TotalHours * (360 / 12));

            // Draw the hour "needle"
            f.DrawLine(_pHoursPen, PolarCoordinate.Center,
                new PolarCoordinate(30, hourAngle, Azimuth.North, PolarCoordinateOrientation.Clockwise));

            #endregion

            #region Drawing of Minutes

            // There are sixty minutes in 360° of a circle
            Angle minuteAngle = new Angle(timeSinceMidnight.TotalMinutes * (360 / 60));

            // Draw the Minute "needle"
            f.DrawLine(_pMinutesPen, PolarCoordinate.Center,
                new PolarCoordinate(80, minuteAngle, Azimuth.North, PolarCoordinateOrientation.Clockwise));

            #endregion

            #region Drawing of Seconds

            // There are sixty seconds in 360° of a circle
            Angle secondAngle = new Angle(timeSinceMidnight.TotalSeconds * (360 / 60));

            // Draw the Seconds "needle"
            f.DrawLine(_pSecondsPen, PolarCoordinate.Center,
                new PolarCoordinate(90, secondAngle, Azimuth.North, PolarCoordinateOrientation.Clockwise));

            #endregion
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of smaller tick marks drawn around the edge of the
        /// control.
        /// </summary>
        /// <remarks>
        /// Minor tick marks are drawn between major tick marks around the control. These
        /// tick marks can be made invisible by changing the color to
        /// <strong>Transparent</strong>.
        /// </remarks>
        [Category("Tick Marks")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of smaller tick marks drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color MinorTickColor
        {
            get
            {
                return _pMinorTickPen.Color;
            }
            set
            {
                _pMinorTickPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <remarks>
        /// There are ten major tick marks in an altimeter, drawn next to numbers on the
        /// control. These tick marks can be made invisible by changing the color to
        /// <strong>Transparent</strong>.
        /// </remarks>
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
                return _pMajorTickPen.Color;
            }
            set
            {
                _pMajorTickPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the font used to draw the hour labels around the edge of the clock.
        /// </summary>
        [Category("Needles")]
        [DefaultValue(typeof(Font), "Tahoma, 9pt, style=Regular")]
        [Description("Controls the font used to draw the hour labels around the edge of the clock.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Font HoursFont
        {
            get
            {
                return _pHoursFont;
            }
            set
            {
                _pHoursFont = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the shortest hand on the clock, representing hours.
        /// </summary>
        [Category("Needles")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of the shortest hand on the clock, representing hours.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color HoursColor
        {
            get
            {
                return _pHoursPen.Color;
            }
            set
            {
                _pHoursPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the minutes hand on the clock.
        /// </summary>
        [Category("Needles")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of the minutes hand on the clock.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color MinutesColor
        {
            get
            {
                return _pMinutesPen.Color;
            }
            set
            {
                _pMinutesPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the seconds hand on the clock.
        /// </summary>
        [Category("Needles")]
        [DefaultValue(typeof(Color), "Red")]
        [Description("Controls the color of the seconds hand on the clock.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color SecondsColor
        {
            get
            {
                return _pSecondsPen.Color;
            }
            set
            {
                _pSecondsPen.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the technique used to display the current time.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(ClockDisplayMode), "LocalMachineTime")]
        [Description("Controls whether the clock displays GPS time, local time, or a custom value.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public ClockDisplayMode DisplayMode
        {
            get
            {
                return _pDisplayMode;
            }
            set
            {
                // Has anything changed?  If not, exit
                if (_pDisplayMode == value)
                    return;

                // If the display mode was lock time, stop the clock thread
                if (_pDisplayMode == ClockDisplayMode.LocalMachineTime)
                {
#if !PocketPC || DesignTime || (PocketPC && Framework20)
                    /* Compact Framework 2.0 supports the ability to
					 * forcefully abort a thread, but there is no Join
					 * method.
					 */

                    try
                    {
                        if (_clockThread != null && !_clockThread.Join(MAXIMUM_GRACEFUL_SHUTDOWN_TIME))
                            _clockThread.Abort();
                    }
                    catch
                    {
                    }
#else
					/* Compact Framework 1.0 does not support the ability to
					 * forcefully shut down a thread, so we must let it exit
					 * on its own.
					 */
#endif
                }
                else if (_pDisplayMode == ClockDisplayMode.SatelliteDerivedTime
                    && LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                {
                    // If the display mode was GPS-derived time, unhook from the events
                    // (but only at run-time.  Doing so at design-time can cause WF errors.)
                    Devices.UtcDateTimeChanged -= Devices_CurrentUtcDateTimeChanged;
                }

                // Remember the new value
                _pDisplayMode = value;

                // Set the control to the last known altitude (if any)
                switch (_pDisplayMode)
                {
                    case ClockDisplayMode.LocalMachineTime:
                        Value = DateTime.Now;
                        // Start the clock thread
                        _clockThread = new Thread(ClockThreadProc)
                                          {
                                              Name = "DotSpatial.Positioning Clock Update Thread (http://dotspatial.codeplex.com)",
                                              IsBackground = true
                                          };
#if !PocketPC || Framework20
#endif
                        _clockThread.Start();

                        // Let it start up
                        Thread.Sleep(0);
                        break;
                    case ClockDisplayMode.SatelliteDerivedTime:
                        // Hook into events (but only at run-time.  Doing so at
                        // design-time can cause WF errors.)
                        if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                        {
                            Devices.UtcDateTimeChanged += Devices_CurrentUtcDateTimeChanged;
                        }

                        // Set the clock to GPS-derived time
                        Value = Devices.UtcDateTime.ToLocalTime();
                        break;
                    case ClockDisplayMode.Manual:
                        // No change
                        break;
                }

                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the amount of time allowed to elapse before the clock is refreshed with the latest time report.  This property works only when the control is set to display the local machine's time.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(TimeSpan), "00:00:00.500")]
        [Description("Controls the amount of time allowed to elapse before the clock is refreshed with the latest time report.  This property works only when the control is set to display the local machine's time.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public TimeSpan UpdateInterval
        {
            get
            {
                return _pUpdateInterval;
            }
            set
            {
                _pUpdateInterval = value.TotalMilliseconds < 10 ? TimeSpan.FromMilliseconds(10) : value;
            }
        }

        private void ClockThreadProc()
        {
            try
            {
                // Loop until the mode changes, or until the control is disposed
                while (!IsDisposed && _pDisplayMode == ClockDisplayMode.LocalMachineTime)
                {
                    // Set the current time
                    Value = DateTime.Now;
                    // And sleep half a second
#if PocketPC
					Thread.Sleep((int)pUpdateInterval.TotalMilliseconds);
#else
                    Thread.Sleep(_pUpdateInterval);
#endif
                    // Let other threads breathe
                    Thread.Sleep(10);
                }
            }
            catch
            {
                // Prevent errors from being raised directly to the UI
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the time being displayed by the device.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(DateTime), "12:00:00 AM")]
        [Description("Controls the time displayed by the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public DateTime Value
        {
            get
            {
                return _pValue;
            }
            set
            {
                if (_pValue.Equals(value)) return;
                _pValue = value;

                // If the control's disposed, skip redrawing
                if (IsDisposed)
                    return;

                // Redraw the control
                InvokeRepaint();

                // Signal that the value has changed
                OnValueChanged(new DateTimeEventArgs(_pValue));
            }
        }

        /// <summary>
        /// Controls the color used to paint numeric labels for hours.
        /// </summary>
        public Color ValueColor
        {
            get
            {
                return _pValueLabelBrush.Color;
            }
            set
            {
                _pValueLabelBrush.Color = value;
                InvokeRepaint();
            }
        }

        private void OnValueChanged(DateTimeEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }

        private void Devices_CurrentUtcDateTimeChanged(object sender, DateTimeEventArgs e)
        {
            if (_pDisplayMode == ClockDisplayMode.SatelliteDerivedTime)
                Value = e.DateTime.ToLocalTime();
        }
    }
}