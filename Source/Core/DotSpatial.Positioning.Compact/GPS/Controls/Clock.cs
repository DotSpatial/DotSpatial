using System;
using System.Globalization;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using DotSpatial.Positioning.Drawing;
using DotSpatial.Positioning.Gps.IO;
using DotSpatial.Positioning;
#if !PocketPC || DesignTime || Framework20
using System.ComponentModel;
#endif

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Gps.Controls
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

	/// <summary>
	/// Represents a user control which displays the local or satellite-derived time.
	/// </summary>
#if !PocketPC || DesignTime
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
		private Pen pMinorTickPen = new Pen(Color.Black, 1.0f);
		// Controls the pen used for larger tick marks around the edge
		private Pen pMajorTickPen = new Pen(Color.Black, 2.0f);
		// Controls the pen used for the hours hand
		private Pen pHoursPen = new Pen(Color.Black, 1.0f);
		// Controls the pen used for the minutes hand
		private Pen pMinutesPen = new Pen(Color.Black, 1.0f);
		// Controls the pen used for the seconds hand
		private Pen pSecondsPen = new Pen(Color.Red, 1.0f);
#endif
		// Controls the brush used for labeling numbers 
		private SolidBrush pValueLabelBrush = new SolidBrush(Color.Black);
		// Controls the time drawn by the control
		private DateTime pValue = DateTime.Now;
		// Controls whether time is updated automatically
		private ClockDisplayMode pDisplayMode = ClockDisplayMode.Manual;
#if PocketPC
        // Controls the font for the clock
        private Font pHoursFont = new Font("Tahoma", 8.0f, FontStyle.Regular);
#else
        // Controls the font for the clock
        private Font pHoursFont = new Font("Tahoma", 9.0f, FontStyle.Regular);
#endif
        // Updates the clock to the current time
		private Thread ClockThread;
#if PocketPC
        private TimeSpan pUpdateInterval = TimeSpan.FromMilliseconds(500);
#else
        private TimeSpan pUpdateInterval = TimeSpan.FromMilliseconds(100);
#endif
//#if PocketPC && !DesignTime
//        private bool pIsClockThreadAlive;
//#endif
#if !PocketPC || DesignTime
		private const int MaximumGracefulShutdownTime = 500;
#elif PocketPC && Framework20
		private const int MaximumGracefulShutdownTime = 500;
#endif


		#endregion

        public event EventHandler<DateTimeEventArgs> ValueChanged;


		/* The constructor provides a name for the control.  This name is used as the
			* name of the thread if multithreading is enabled. 
			*/
		public Clock() 
			: base("DotSpatial.Positioning Multithreaded Clock Control (http://dotspatial.codeplex.com)")
		{		
			// Set the behavior of angles.  "Origin" means that 0° points up,
			// and "Orientation" states that larger angles go clockwise around the control.
			Origin = Azimuth.North;
			Orientation = PolarCoordinateOrientation.Clockwise;
			
//#if PocketPC && !Framework20 && !DesignTime
//            // Hook into the altitude changed event
//            DotSpatial.Positioning.Gps.IO.Devices.UtcDateTimeChanged += new EventHandler<DateTimeEventArgs>(Devices_CurrentUtcDateTimeChanged);
//#endif
		}

		protected override void Dispose(bool disposing)
		{
            // Unhook from events (but only at run-time.  Doing so at
            // design-time can cause WF errors.)
            if (pDisplayMode == ClockDisplayMode.SatelliteDerivedTime
                && LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                Devices.UtcDateTimeChanged -= new EventHandler<DateTimeEventArgs>(Devices_CurrentUtcDateTimeChanged);
            }
            

//#if PocketPC && !Framework20 && !DesignTime
//            try
//            {
//                // Hook into the altitude changed event
//                DotSpatial.Positioning.Gps.IO.Devices.UtcDateTimeChanged -= new EventHandler<DateTimeEventArgs>(Devices_CurrentUtcDateTimeChanged);
//            }
//            catch
//            {
//            }
//#endif

			// Free unmanaged GDI resources (very important!)
            pHoursFont.Dispose();
			pMinorTickPen.Dispose();
			pMajorTickPen.Dispose();
			pValueLabelBrush.Dispose();
			pHoursPen.Dispose();
			pMinutesPen.Dispose();
			pSecondsPen.Dispose();

			// Shut down the clock thread
			DisplayMode = ClockDisplayMode.Manual;

			// Move on down the line
			try
			{
				base.Dispose(disposing);
			}
			catch
			{
			}
		}

//#if !PocketPC || Framework20
//        protected override void OnHandleCreated(EventArgs e)
//        {

//            // Subscribe to events
//            try
//            {
//                base.OnHandleCreated(e);

//                // Hook into the date/time changed event
//#if !PocketPC
//                if(DesignMode)
//                    return;
//#endif
//                DotSpatial.Positioning.Gps.IO.Devices.UtcDateTimeChanged += new EventHandler<DateTimeEventArgs>(Devices_CurrentUtcDateTimeChanged);
//            }
//            catch
//            {
//            }

//        }

//        protected override void OnHandleDestroyed(EventArgs e)
//        {
//            try
//            {
//#if !PocketPC
//                if(DesignMode)
//                    return;
//#endif
//                // Hook into the date/time changed event
//                DotSpatial.Positioning.Gps.IO.Devices.UtcDateTimeChanged -= new EventHandler<DateTimeEventArgs>(Devices_CurrentUtcDateTimeChanged);
//            }
//            catch
//            {
//            }
//            finally
//            {
//                base.OnHandleDestroyed(e);
//            }
//        }
//#endif

		/* This method is called whenever the control must be rendered.  All rendering takes
			* place off-screen, which prevents flickering.  The "PolarControl" base class automatically
			* handles tasks such as resizing and smoothing.  All you have to be concerned with is
			* calculating the polar coordinates to draw.
			*/
		////[CLSCompliant(false)]
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
			for(double value = 0; value < 60; value += 1)
			{
				// There are 120 tick marks in 360°
				Angle angle = new Angle(value * (360 / 60));
			
				// Get the coordinate of the line's start and end
				PolarCoordinate start = new PolarCoordinate(96, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
				PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
			
				// And draw a line
				f.DrawLine(pMinorTickPen, start, end);
			}

			// Draw twelve tick marks
			for(double value = 1; value <= 12; value += 1)
			{
				// Convert the value to an angle
				Angle angle = new Angle(value * (360 / 12));

				// Get the coordinate of the line's start
				PolarCoordinate start = new PolarCoordinate(93, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
				PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
			
				// And draw the tick mark
				f.DrawLine(pMajorTickPen, start, end);
			
				// Label the clock position around the circle
                if (pHoursFont != null)
                {
                    string s = Convert.ToString(value,CultureInfo.CurrentUICulture);
                    f.DrawCenteredString(s, pHoursFont, pValueLabelBrush,
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
			DateTime Midnight = new DateTime(pValue.Year, pValue.Month, pValue.Day);
			TimeSpan TimeSinceMidnight = pValue.Subtract(Midnight);

			#region Drawing of Hours

			// There are twelve hours in 360° of a circle
			Angle HourAngle = new Angle(TimeSinceMidnight.TotalHours * (360 / 12));

			// Draw the hour "needle"
			f.DrawLine(pHoursPen, PolarCoordinate.Center, 
				new PolarCoordinate(30, HourAngle, Azimuth.North, PolarCoordinateOrientation.Clockwise));

			#endregion

			#region Drawing of Minutes

			// There are sixty minutes in 360° of a circle
			Angle MinuteAngle = new Angle(TimeSinceMidnight.TotalMinutes * (360 / 60));

			// Draw the Minute "needle"
			f.DrawLine(pMinutesPen, PolarCoordinate.Center, 
				new PolarCoordinate(80, MinuteAngle, Azimuth.North, PolarCoordinateOrientation.Clockwise));

			#endregion
	
			#region Drawing of Seconds

			// There are sixty seconds in 360° of a circle
			Angle SecondAngle = new Angle(TimeSinceMidnight.TotalSeconds * (360 / 60));

			// Draw the Seconds "needle"
			f.DrawLine(pSecondsPen, PolarCoordinate.Center, 
				new PolarCoordinate(90, SecondAngle, Azimuth.North, PolarCoordinateOrientation.Clockwise));

			#endregion

		}

		/// <summary>
		/// Controls the color of smaller tick marks drawn around the edge of the
		/// control.
		/// </summary>
		/// <remarks>
		/// Minor tick marks are drawn between major tick marks around the control. These
		/// tick marks can be made invisible by changing the color to
		/// <strong>Transparent</strong>.
		/// </remarks>
#if !PocketPC || DesignTime
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
				return pMinorTickPen.Color;
			}
			set
			{
				pMinorTickPen.Color = value;
				InvokeRepaint();
			}
		}

		/// <remarks>
		/// There are ten major tick marks in an altimeter, drawn next to numbers on the
		/// control. These tick marks can be made invisible by changing the color to
		/// <strong>Transparent</strong>.
		/// </remarks>
#if !PocketPC || DesignTime
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
				return pMajorTickPen.Color;
			}
			set
			{
				pMajorTickPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
                return pHoursFont;
            }
            set
            {
                pHoursFont = value;
                InvokeRepaint();
            }
        }

		
#if !PocketPC || DesignTime
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
				return pHoursPen.Color;
			}
			set
			{
				pHoursPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pMinutesPen.Color;
			}
			set
			{
				pMinutesPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pSecondsPen.Color;
			}
			set
			{
				pSecondsPen.Color = value;
				InvokeRepaint();
			}
		}

		/// <summary>
		/// Controls the technique used to display the current time.
		/// </summary>
#if !PocketPC || DesignTime
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
				return pDisplayMode;
			}
			set
			{
				// Has anything changed?  If not, exit
				if(pDisplayMode == value) 
                    return;

				// If the display mode was lock time, stop the clock thread
				if(pDisplayMode == ClockDisplayMode.LocalMachineTime)
				{
#if !PocketPC || DesignTime || (PocketPC && Framework20)
					/* Compact Framework 2.0 supports the ability to
					 * forcefully abort a thread, but there is no Join
					 * method.
					 */

                    try
                    {
                        if(ClockThread != null && !ClockThread.Join(MaximumGracefulShutdownTime))
                            ClockThread.Abort();
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

                // If the display mode was GPS-derived time, unhook from the events
                // (but only at run-time.  Doing so at design-time can cause WF errors.)
                else if (pDisplayMode == ClockDisplayMode.SatelliteDerivedTime
                    && LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                {
                    Devices.UtcDateTimeChanged -= new EventHandler<DateTimeEventArgs>(Devices_CurrentUtcDateTimeChanged);
                }

                // Remember the new value
				pDisplayMode = value;

				// Set the control to the last known altitude (if any)
				switch(pDisplayMode)
				{
					case ClockDisplayMode.LocalMachineTime:
						Value = DateTime.Now;
						// Start the clock thread
						ClockThread = new Thread(new ThreadStart(ClockThreadProc));
#if !PocketPC || Framework20
						ClockThread.Name = "DotSpatial.Positioning Clock Update Thread (http://dotspatial.codeplex.com)";
						ClockThread.IsBackground = true;
#endif
						ClockThread.Start();

						// Let it start up
						Thread.Sleep(0);
						break;
					case ClockDisplayMode.SatelliteDerivedTime:
                        // Hook into events (but only at run-time.  Doing so at
                        // design-time can cause WF errors.)
                        if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                        {
                            Devices.UtcDateTimeChanged += new EventHandler<DateTimeEventArgs>(Devices_CurrentUtcDateTimeChanged);
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
                return pUpdateInterval;
            }
            set
            {
                if (value.TotalMilliseconds < 10)
                    pUpdateInterval = TimeSpan.FromMilliseconds(10);
                else
                    pUpdateInterval = value;
            }
        }

		private void ClockThreadProc()
		{
			try
			{
//#if PocketPC && !DesignTime
//                // Signal that we're done
//                pIsClockThreadAlive = true;
//#endif

				// Loop until the mode changes, or until the control is disposed
				while(!IsDisposed && pDisplayMode == ClockDisplayMode.LocalMachineTime)
				{
					// Set the current time
					Value = DateTime.Now;
					// And sleep half a second
#if PocketPC
					Thread.Sleep((int)pUpdateInterval.TotalMilliseconds);
#else
                    Thread.Sleep(pUpdateInterval);
#endif
                    // Let other threads breathe
                    Thread.Sleep(10);
				}
			}
			catch
			{
				// Prevent errors from being raised directly to the UI
			}
//#if PocketPC && !DesignTime
//            finally
//            {
//                // Signal that we're done
//                pIsClockThreadAlive = false;
//            }
//#endif
		}

		/// <summary>
		/// Controls the time being displayed by the device.
		/// </summary>
#if !PocketPC || DesignTime
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
				return pValue;
			}
			set
			{
				if(pValue.Equals(value)) return;
				pValue = value;

				// If the control's disposed, skip redrawing
				if(IsDisposed)
					return;

				// Redraw the control
                InvokeRepaint();

                // Signal that the value has changed
                OnValueChanged(new DateTimeEventArgs(pValue));
			}
		}


        /// <summary>
        /// Controls the color used to paint numeric labels for hours.
        /// </summary>
        public Color ValueColor
        {
            get
            {
                return pValueLabelBrush.Color;
            }
            set
            {
                pValueLabelBrush.Color = value;
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
			if(pDisplayMode == ClockDisplayMode.SatelliteDerivedTime)
				Value = e.DateTime.ToLocalTime();
		}

	}
}
