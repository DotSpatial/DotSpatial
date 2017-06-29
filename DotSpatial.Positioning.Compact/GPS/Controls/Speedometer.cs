using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
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
	/// Represents a user control used to measure speed graphically.
	/// </summary>
#if !PocketPC || DesignTime
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
		private System.Threading.Thread InterpolationThread;
		private bool IsInterpolationActive;
		//private ManualResetEvent InterpolationThreadWaitHandle = new ManualResetEvent(false);
		private ManualResetEvent AnimationWaitHandle = new ManualResetEvent(false);
		private Interpolator ValueInterpolator = new Interpolator(15, InterpolationMethod.CubicEaseInOut);
		private int InterpolationIndex;
        private SolidBrush pNeedleShadowBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
		private Size pNeedleShadowSize = new Size(5, 5);
#endif
        private Speed pMaximumSpeed = new Speed(120, SpeedUnit.KilometersPerHour);
        private Speed pSpeedLabelInterval = new Speed(10, SpeedUnit.KilometersPerHour);
		private Speed pMinorTickInterval = new Speed(5, SpeedUnit.KilometersPerHour);
		private Speed pMajorTickInterval = new Speed(10, SpeedUnit.KilometersPerHour);
        private Pen pCenterPen = new Pen(Color.Gray);
		private Pen pMinorTickPen = new Pen(Color.Black);
		private Pen pMajorTickPen = new Pen(Color.Black);
		private string pSpeedLabelFormat = "v";
#if PocketPC
        private Font pSpeedLabelFont = new Font("Tahoma", 7.0f, FontStyle.Regular);
#else
        private Font pSpeedLabelFont = new Font("Tahoma", 11.0f, FontStyle.Regular);
#endif
        private SolidBrush pSpeedLabelBrush = new SolidBrush(Color.Black);
		private Speed pSpeed = new Speed(0, SpeedUnit.MetersPerSecond);
		private SolidBrush pNeedleFillBrush = new SolidBrush(Color.Red);
		private Pen pNeedleOutlinePen = new Pen(Color.Black);
		private Angle pMinimumAngle = new Angle(40);
		private Angle pMaximumAngle = new Angle(320);
		private static PolarCoordinate[] SpeedometerNeedle = new PolarCoordinate[]
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
		private bool pIsUsingRealTimeData = false;
        private bool pIsUnitLabelVisible = true;
		private double ConversionFactor;

#if (PocketPC && Framework20)
		private const int MaximumGracefulShutdownTime = 2000;
#elif !PocketPC
		private const int MaximumGracefulShutdownTime = 500;
#endif

        public event EventHandler<SpeedEventArgs> ValueChanged;

		public Speedometer()
			: base("DotSpatial.Positioning Multithreaded Speedometer Control (http://dotspatial.codeplex.com)")
		{
#if !PocketPC
			// Start the interpolation thread
			InterpolationThread = new Thread(new ThreadStart(InterpolationLoop));
			InterpolationThread.IsBackground = true;
			InterpolationThread.Name = "DotSpatial.Positioning Speedometer Needle Animation Thread (http://dotspatial.codeplex.com)";
			IsInterpolationActive = true;
			InterpolationThread.Start();
#endif
			// Set the control to display clockwise from north
			Orientation = PolarCoordinateOrientation.Clockwise;
			Origin = Azimuth.South;
			// The center is zero and edge is 100
			CenterR = 0;
			MaximumR = 100;

			// Use the speed depending on the local culture
			if(RegionInfo.CurrentRegion.IsMetric)
			{
				pMaximumSpeed = new Speed(120, SpeedUnit.KilometersPerHour);
				pSpeedLabelInterval = new Speed(10, SpeedUnit.KilometersPerHour);
#if PocketPC
				pMinorTickInterval = new Speed(5, SpeedUnit.KilometersPerHour); 
#else
				pMinorTickInterval = new Speed(1, SpeedUnit.KilometersPerHour); 
#endif
                pMajorTickInterval = new Speed(10, SpeedUnit.KilometersPerHour); 
			}
			else
			{
				pMaximumSpeed = new Speed(120, SpeedUnit.StatuteMilesPerHour);
				pSpeedLabelInterval = new Speed(10, SpeedUnit.StatuteMilesPerHour);
#if PocketPC
				pMinorTickInterval = new Speed(5, SpeedUnit.StatuteMilesPerHour); 
#else
				pMinorTickInterval = new Speed(1, SpeedUnit.StatuteMilesPerHour); 
#endif			
                pMajorTickInterval = new Speed(10, SpeedUnit.StatuteMilesPerHour); 
			}

			// Calculate the factor for converting speed into an angle
			ConversionFactor = (pMaximumAngle.DecimalDegrees - pMinimumAngle.DecimalDegrees) / pMaximumSpeed.Value;

//#if PocketPC && !Framework20 && !DesignTime
//            // Bind the global event for when speed changes
//            DotSpatial.Positioning.Gps.IO.Devices.SpeedChanged += new EventHandler<SpeedEventArgs>(Devices_CurrentSpeedChanged);
//#endif

        }

//#if !PocketPC || Framework20
//        protected override void OnHandleCreated(EventArgs e)
//        {
//            base.OnHandleCreated(e);

//            // Subscribe to events
//            try
//            {
//                // Only hook into events if we're at run-time.  Hooking events
//                // at design-time can actually cause errors in the WF Designer.
//                if (License.Context.UsageMode == LicenseUsageMode.Runtime)
//                {
//                    DotSpatial.Positioning.Gps.IO.Devices.SpeedChanged += new EventHandler<SpeedEventArgs>(Devices_CurrentSpeedChanged);
//                }
//            }
//            catch
//            {
//            }

//        }

//        protected override void OnHandleDestroyed(EventArgs e)
//        {
//            try
//            {
//                // Only hook into events if we're at run-time.  Hooking events
//                // at design-time can actually cause errors in the WF Designer.
//                if (License.Context.UsageMode == LicenseUsageMode.Runtime)
//                {
//                    DotSpatial.Positioning.Gps.IO.Devices.SpeedChanged -= new EventHandler<SpeedEventArgs>(Devices_CurrentSpeedChanged);
//                }
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

        protected override void Dispose(bool disposing)
        {
            try
            {
                // Only hook into events if we're at run-time.  Hooking events
                // at design-time can actually cause errors in the WF Designer.
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime
                    && pIsUsingRealTimeData)
                {
                    Devices.SpeedChanged -= new EventHandler<SpeedEventArgs>(Devices_CurrentSpeedChanged);
                }
            }
            catch
            {
            }

#if !PocketPC
            // Get the interpolation thread out of a loop 
            IsInterpolationActive = false;

            if (InterpolationThread != null)
            {

                if (AnimationWaitHandle != null)
                {
                    try
                    {
                        AnimationWaitHandle.Set();
                    }
                    catch
                    {
                    }
                }

                if (!InterpolationThread.Join(MaximumGracefulShutdownTime))
                {
                    try
                    {
                        InterpolationThread.Abort();
                    }
                    catch
                    {
                    }
                }

            }

			if (AnimationWaitHandle != null)
			{
				try
				{
					AnimationWaitHandle.Close();
				}
				catch
				{
				}
				finally
				{
					AnimationWaitHandle = null;
				}
			}
#endif

            if (pCenterPen != null)
            {
                try
                {
                    pCenterPen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pCenterPen = null;
                }
            }
            if (pMinorTickPen != null)
            {
                try
                {
                    pMinorTickPen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pMinorTickPen = null;
                }
            }
            if (pMajorTickPen != null)
            {
                try
                {
                    pMajorTickPen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pMajorTickPen = null;
                }
            }
            if (pSpeedLabelFont != null)
            {
                try
                {
                    pSpeedLabelFont.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pSpeedLabelFont = null;
                }
            }
            if (pSpeedLabelBrush != null)
            {
                try
                {
                    pSpeedLabelBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pSpeedLabelBrush = null;
                }
            }
            if (pNeedleFillBrush != null)
            {
                try
                {
                    pNeedleFillBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pNeedleFillBrush = null;
                }
            }
            if (pNeedleOutlinePen != null)
            {
                try
                {
                    pNeedleOutlinePen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pNeedleOutlinePen = null;
                }
            }
#if !PocketPC
            if (pNeedleShadowBrush != null)
            {
                try
                {
                    pNeedleShadowBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pNeedleShadowBrush = null;
                }
            }
#endif

            try
            {
                base.Dispose(disposing);
            }
            catch
            {
            }
        }

      

#if Framework20 && !PocketPC
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
        
        protected override void OnPaintOffScreen(PaintEventArgs e)
		{
			PolarGraphics f = base.CreatePolarGraphics(e.Graphics);
			

			// What altitude are we drawing?
#if PocketPC
            Speed SpeedToRender = pSpeed;
#else
			Speed SpeedToRender = new Speed(ValueInterpolator[InterpolationIndex], pSpeed.Units);
#endif

            // Cache drawing intervals and such to prevent a race condition
            double MinorInterval = pMinorTickInterval.Value;
            double MajorInterval = pMajorTickInterval.Value;
            double CachedSpeedLabelInterval = pSpeedLabelInterval.ToUnitType(pMaximumSpeed.Units).Value;
            Speed MaxSpeed = pMaximumSpeed.Clone();
            double MinorStep = pMinorTickInterval.ToUnitType(pMaximumSpeed.Units).Value;
            double MajorStep = pMajorTickInterval.ToUnitType(pMaximumSpeed.Units).Value;
            
			// Draw tick marks
            double angle;
            PolarCoordinate start;
            PolarCoordinate end;

            #region Draw minor tick marks

            if (MinorInterval > 0)
            {
                for (double speed = 0; speed < MaxSpeed.Value; speed += MinorStep)
                {
                    // Convert the speed to an angle
                    angle = speed * ConversionFactor + pMinimumAngle.DecimalDegrees;
                    // Get the coordinate of the line's start
                    start = new PolarCoordinate(95, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    end = new PolarCoordinate(100, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    // And draw a line
                    f.DrawLine(pMinorTickPen, start, end);
                }
            }

            #endregion

            #region Draw major tick marks

            if (MajorInterval > 0)
            {
                for (double speed = 0; speed < MaxSpeed.Value; speed += MajorStep)
                {
                    // Convert the speed to an angle
                    angle = speed * ConversionFactor + pMinimumAngle.DecimalDegrees;
                    // Get the coordinate of the line's start
                    start = new PolarCoordinate(90, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    end = new PolarCoordinate(100, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                    // And draw a line
                    f.DrawLine(pMajorTickPen, start, end);
                }

                #region Draw a major tick mark at the maximum speed

                // Convert the speed to an angle
                angle = MaxSpeed.Value * ConversionFactor + pMinimumAngle.DecimalDegrees;
                // Get the coordinate of the line's start
                start = new PolarCoordinate(90, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                end = new PolarCoordinate(100, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise);
                // And draw a line
                f.DrawLine(pMajorTickPen, start, end);

                #endregion

            }

            #endregion

            if (CachedSpeedLabelInterval > 0)
            {
                for (double speed = 0; speed < MaxSpeed.Value; speed += CachedSpeedLabelInterval)
                {
                    // Convert the speed to an angle
                    angle = speed * ConversionFactor + pMinimumAngle.DecimalDegrees;
                    // And draw a line
                    f.DrawCenteredString(new Speed(speed, MaxSpeed.Units).ToString(pSpeedLabelFormat, CultureInfo.CurrentCulture), pSpeedLabelFont, pSpeedLabelBrush,
                        new PolarCoordinate(75, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise));
                }

                // Convert the speed to an angle
                angle = MaxSpeed.Value * ConversionFactor + pMinimumAngle.DecimalDegrees;
                
                // And draw the speed label
                f.DrawCenteredString(MaxSpeed.ToString(pSpeedLabelFormat, CultureInfo.CurrentCulture), pSpeedLabelFont, pSpeedLabelBrush,
                    new PolarCoordinate(75, angle, Azimuth.South, PolarCoordinateOrientation.Clockwise));
            }

				// Draw the units for the speedometer
            if (pIsUnitLabelVisible)
            {
                f.DrawCenteredString(pMaximumSpeed.ToString("u", CultureInfo.CurrentCulture), pSpeedLabelFont, pSpeedLabelBrush,
                    new PolarCoordinate(90, Angle.Empty, Azimuth.South, PolarCoordinateOrientation.Clockwise));
            }
			
				PolarCoordinate[] Needle = new PolarCoordinate[SpeedometerNeedle.Length];
				for(int index = 0; index < Needle.Length; index++)
				{
					Needle[index] = SpeedometerNeedle[index].Rotate((SpeedToRender.ToUnitType(pMaximumSpeed.Units).Value * this.ConversionFactor) + pMinimumAngle.DecimalDegrees);
				}

				// Draw an ellipse at the center
				f.DrawEllipse(pCenterPen, PolarCoordinate.Empty, 10);

#if !PocketPC
				// Now draw a shadow
				f.Graphics.TranslateTransform(pNeedleShadowSize.Width, pNeedleShadowSize.Height, MatrixOrder.Append);
				f.FillPolygon(pNeedleShadowBrush, Needle);
				f.Graphics.ResetTransform();
#endif

				// Then draw the actual needle
				f.FillPolygon(pNeedleFillBrush, Needle);
				f.DrawPolygon(pNeedleOutlinePen, Needle);

		}

#if !PocketPC || DesignTime
		[Category("Speedometer Needle")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color of the edge of the needle.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color NeedleOutlineColor
		{
			get
			{
				return pNeedleOutlinePen.Color;
			}
			set
			{
				if(pNeedleOutlinePen.Color.Equals(value)) return;
				pNeedleOutlinePen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pNeedleFillBrush.Color;
			}
			set
			{
				if(pNeedleFillBrush.Color.Equals(value)) return;
				pNeedleFillBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "128, 0, 0, 0")]
		[Description("Controls the color of the shadow cast by the needle.")]
        public Color NeedleShadowColor
		{
			get
			{
				return pNeedleShadowBrush.Color;
			}
			set
			{
				pNeedleShadowBrush.Color = value;
				InvokeRepaint();
			}
		}
#endif

#if !PocketPC
		[Category("Appearance")]
		[DefaultValue(typeof(Size), "5, 5")]
		[Description("Controls the size of the shadow cast by the needle.")]
        public Size NeedleShadowSize
		{
			get
			{
				return pNeedleShadowSize;
			}
			set
			{
				pNeedleShadowSize = value;
				InvokeRepaint();
			}
		}
#endif

#if !PocketPC || DesignTime
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
				return pSpeed;
			}
			set
			{
				if(pSpeed.Equals(value)) return;
				pSpeed = value.ToUnitType(pMaximumSpeed.Units);
#if PocketPC
                InvokeRepaint();
#else
				if(IsDisposed)
					return;

				lock(ValueInterpolator)
				{
					// Are we changing direction?
					if(pSpeed.Value >= ValueInterpolator.Minimum
						&& pSpeed.Value > ValueInterpolator[InterpolationIndex])
					{
						// No.  Just set the new maximum
						ValueInterpolator.Maximum = pSpeed.Value;
					}					
					else if(pSpeed.Value < ValueInterpolator.Minimum)
					{
						// We're changing directions, so stop then accellerate again
						ValueInterpolator.Minimum = ValueInterpolator[InterpolationIndex];
						ValueInterpolator.Maximum = pSpeed.Value;
						InterpolationIndex = 0;
					}
					else if(pSpeed.Value > ValueInterpolator.Minimum
						&& pSpeed.Value < ValueInterpolator[InterpolationIndex])
					{
						// We're changing directions, so stop then accellerate again
						ValueInterpolator.Minimum = ValueInterpolator[InterpolationIndex];
						ValueInterpolator.Maximum = pSpeed.Value;
						InterpolationIndex = 0;
					}
					else if(pSpeed.Value > ValueInterpolator.Maximum)
					{
						// No.  Just set the new maximum
						ValueInterpolator.Maximum = pSpeed.Value;
					}
				}
				// And activate the interpolation thread
				AnimationWaitHandle.Set();
#endif

                OnValueChanged(new SpeedEventArgs(pSpeed));
			}
		}

        private void OnValueChanged(SpeedEventArgs e)
        {
            if (ValueChanged != null)
				ValueChanged(this, e);
        }

#if !PocketPC
		[Category("Behavior")]
		[DefaultValue(typeof(InterpolationMethod), "CubicEaseInOut")]
		[Description("Controls how the control smoothly transitions from one value to another.")]
		////[CLSCompliant(false)]
        public InterpolationMethod ValueInterpolationMethod
		{
			get
			{
				return ValueInterpolator.InterpolationMethod;
			}
			set
			{
				ValueInterpolator.InterpolationMethod = value;
			}
		}
#endif

#if !PocketPC || DesignTime
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
				return pMaximumSpeed;
			}
			set
			{
				if(pMaximumSpeed.Equals(value)) return;
				pMaximumSpeed = value;
				// Calculate the factor for converting speed into an angle
				ConversionFactor = (pMaximumAngle.DecimalDegrees - pMinimumAngle.DecimalDegrees) / pMaximumSpeed.Value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
        [Category("Speed Label")]
		[DefaultValue(typeof(Speed), "10 km/h")]
		[Description("Controls the amount of speed in between each label around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
        public Speed SpeedLabelInterval
		{
			get
			{
				return pSpeedLabelInterval;
			}
			set
			{
				if(pSpeedLabelInterval.Equals(value)) return;
				pSpeedLabelInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
        [Category("Tick Marks")]
		[DefaultValue(typeof(Speed), "5 km/h")]
		[Description("Controls the number of degrees in between each smaller tick mark around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
        public Speed MinorTickInterval
        {
			get
			{
				return pMinorTickInterval;
			}
			set
			{
				if(pMinorTickInterval.Equals(value)) return;
				pMinorTickInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Tick Marks")]
		[DefaultValue(typeof(Speed), "10 km/h")]
		[Description("Controls the number of degrees in between each larger tick mark around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
        public Speed MajorTickInterval
        {
			get
			{
				return pMajorTickInterval;
			}
			set
			{
				if(pMajorTickInterval.Equals(value)) return;
				pMajorTickInterval = value;
				InvokeRepaint();
			}
		}

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
				if(pMinorTickPen.Color.Equals(value)) return;
				pMinorTickPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
                return pIsUnitLabelVisible;
            }
            set
            {
                if (pIsUnitLabelVisible.Equals(value))
                    return;

                pIsUnitLabelVisible = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime
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
				return pIsUsingRealTimeData;
			}
			set
			{
                // Has nothing changed?
				if(pIsUsingRealTimeData == value)
                    return;

                // Store the new value
				pIsUsingRealTimeData = value;

                if (pIsUsingRealTimeData)
                {
                    // Only hook into events if we're at run-time.  Hooking events
                    // at design-time can actually cause errors in the WF Designer.
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        Devices.SpeedChanged += new EventHandler<SpeedEventArgs>(Devices_CurrentSpeedChanged);
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
                        Devices.SpeedChanged -= new EventHandler<SpeedEventArgs>(Devices_CurrentSpeedChanged);
                    }

                    // Reset the value to zero
                    Value = Speed.AtRest;
                }

				InvokeRepaint();
			}
		}

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
				if(pMajorTickPen.Color.Equals(value)) return;
				pMajorTickPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pSpeedLabelFormat;
			}
			set
			{
				if(pSpeedLabelFormat.Equals(value)) return;
				pSpeedLabelFormat = value;
                InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
        public Font SpeedLabelFont
		{
			get
			{
				return pSpeedLabelFont;
			}
			set
			{
				if(pSpeedLabelFont.Equals(value)) return;
				pSpeedLabelFont = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pSpeedLabelBrush.Color;
			}
			set
			{
				if(pSpeedLabelBrush.Color.Equals(value)) return;
				pSpeedLabelBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
        [Category("Behavior")]
        [DefaultValue(typeof(Angle), "40")]
        [Description("Controls the angle associated with the smallest possible speed.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
        public Angle MinimumAngle
		{
			get
			{
				return pMinimumAngle;
			}
			set
			{
				if(pMinimumAngle.Equals(value)) return;
				pMinimumAngle = value;
				// Calculate the factor for converting speed into an angle
				ConversionFactor = (pMaximumAngle.DecimalDegrees - pMinimumAngle.DecimalDegrees) / pMaximumSpeed.Value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
        [Category("Behavior")]
        [DefaultValue(typeof(Angle), "320")]
        [Description("Controls the angle associated with the largest possible speed.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
        public Angle MaximumAngle
        {
			get
			{
				return pMaximumAngle;
			}
			set
			{
				if(pMaximumAngle.Equals(value)) return;
				pMaximumAngle = value;
				// Calculate the factor for converting speed into an angle
				ConversionFactor = (pMaximumAngle.DecimalDegrees - pMinimumAngle.DecimalDegrees) / pMaximumSpeed.Value;
				InvokeRepaint();
			}
		}

		private void Devices_CurrentSpeedChanged(object sender, SpeedEventArgs e)
		{
			if(pIsUsingRealTimeData)
				Value = e.Speed;
		}

#if !PocketPC
		protected override void OnTargetFrameRateChanged(int framesPerSecond)
		{
			base.OnTargetFrameRateChanged(framesPerSecond);
			// Recalculate our things
			ValueInterpolator.Count = framesPerSecond;
			// Adjust the index if it's outside of bounds
			if(InterpolationIndex > ValueInterpolator.Count - 1)
				InterpolationIndex = ValueInterpolator.Count - 1;
		}

		private void InterpolationLoop()
		{
			// Flag that we're alive
			//InterpolationThreadWaitHandle.Set();
			// Are we at the end?
			while(IsInterpolationActive)
			{
				try
				{
					// Wait for interpolation to actually be needed
					//InterpolationThread.Suspend();
					AnimationWaitHandle.WaitOne();
					// If we're shutting down, just exit
					if (!IsInterpolationActive)
						break;
					// Keep updating interpolation until we're done
					while (IsInterpolationActive && InterpolationIndex < ValueInterpolator.Count)
					{
						// Render the next value
						InvokeRepaint();
						InterpolationIndex++;
						// Wait for the next frame
						Thread.Sleep(1000 / ValueInterpolator.Count);
					}
					// Reset interpolation
					ValueInterpolator.Minimum = ValueInterpolator.Maximum;
					InterpolationIndex = 0;
					AnimationWaitHandle.Reset();
				}
				catch(ThreadAbortException)
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
