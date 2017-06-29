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
	/// Represents a user control used to display the current direction of travel.
	/// </summary>
#if !PocketPC || DesignTime
	[ToolboxBitmap(typeof(Compass))]
	[DefaultProperty("Value")]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
    //[DesignTimeVisible(true)]
#endif
	//[CLSCompliant(false)]
	public sealed class Compass : PolarControl
	{
#if !PocketPC
		private System.Threading.Thread _InterpolationThread;
		private bool _IsInterpolationActive;
		private ManualResetEvent _AnimationWaitHandle = new ManualResetEvent(false);
		private Interpolator _ValueInterpolator = new Interpolator(15, InterpolationMethod.CubicEaseOut);
		private int _InterpolationIndex;
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
		private Font _DirectionLabelFont = new Font("Tahoma", 12.0f, FontStyle.Bold); 
		private Angle _DirectionLabelInterval = new Angle(45);
		private Angle _AngleLabelInterval = new Angle(30);
		private Font _AngleLabelFont = new Font("Tahoma", 8.0f, FontStyle.Regular); 
		private Angle _MinorTickInterval = new Angle(2);
		private Pen _DirectionTickPen = new Pen(Color.Black, 3.0f);
#endif

		private Azimuth _Bearing = Azimuth.North;
		private Angle _MajorTickInterval = new Angle(15);
		private Pen _CenterPen = new Pen(Color.Gray);
		private Pen _MinorTickPen = new Pen(Color.Black);
		private Pen _MajorTickPen = new Pen(Color.Black);
		private SolidBrush _DirectionLabelBrush = new SolidBrush(Color.Black);
		private SolidBrush _AngleLabelBrush = new SolidBrush(Color.Black);
		private string _AngleLabelFormat = "h°";
		
        private static PolarCoordinate[] NeedlePointsNorth = new PolarCoordinate[]
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
		private static PolarCoordinate[] NeedlePointsSouth = new PolarCoordinate[]
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
		private SolidBrush pNorthNeedleBrush = new SolidBrush(Color.Red);
		private Pen pNorthNeedlePen = new Pen(Color.Black);
		private SolidBrush pSouthNeedleBrush = new SolidBrush(Color.White);
		private Pen pSouthNeedlePen = new Pen(Color.Black);
		private bool pIsUsingRealTimeData = false;
#if !PocketPC
		private SolidBrush pNeedleShadowBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
		private Size pNeedleShadowSize = new Size(5, 5);
#endif
#if (PocketPC && Framework20)
		private const int MaximumGracefulShutdownTime = 2000;
#elif !PocketPC
		private const int MaximumGracefulShutdownTime = 500;
#endif

		////[CLSCompliant(false)]
        public event EventHandler<AzimuthEventArgs> ValueChanged;



		public Compass()
			: base("DotSpatial.Positioning Multithreaded Compass Control (http://dotspatial.codeplex.com)")
		{
#if !PocketPC
			// Start the interpolation thread
			_InterpolationThread = new Thread(new ThreadStart(InterpolationLoop));
			_InterpolationThread.IsBackground = true;
			_InterpolationThread.Name = "DotSpatial.Positioning Compass Needle Animation Thread (http://dotspatial.codeplex.com)";
			_IsInterpolationActive = true;
			_InterpolationThread.Start();
			//InterpolationThreadWaitHandle.WaitOne();
#endif
            Orientation = PolarCoordinateOrientation.Clockwise;
			Origin = Azimuth.North;
			// The center is zero and edge is 100
			CenterR = 0;
			MaximumR = 100;

			// Now mirror the points to create the southern needle
			NeedlePointsSouth = new PolarCoordinate[38];
			for (int index = 0; index < NeedlePointsNorth.Length; index++)
			{
				NeedlePointsSouth[index] = NeedlePointsNorth[index].Rotate(180);
			}

//#if PocketPC && !Framework20 && !DesignTime
//            // Bind the global event for when the current bearing changes
//            DotSpatial.Positioning.Gps.IO.Devices.BearingChanged += new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
//#endif
        }

#if !PocketPC || Framework20
        //protected override void OnHandleCreated(EventArgs e)
        //{

        //    // Subscribe to events
        //    try
        //    {
        //        base.OnHandleCreated(e);

        //        // Only hook into events if we're at run-time.  Hooking events
        //        // at design-time can actually cause errors in the WF Designer.
        //        if (License.Context.UsageMode == LicenseUsageMode.Runtime)
        //        {
        //            DotSpatial.Positioning.Gps.IO.Devices.BearingChanged += new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
        //        }
        //    }
        //    catch
        //    {
        //    }

        //}

        //protected override void OnHandleDestroyed(EventArgs e)
        //{
        //    try
        //    {
        //        // Only hook into events if we're at run-time.  Hooking events
        //        // at design-time can actually cause errors in the WF Designer.
        //        if (License.Context.UsageMode == LicenseUsageMode.Runtime)
        //        {
        //            DotSpatial.Positioning.Gps.IO.Devices.BearingChanged -= new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        base.OnHandleDestroyed(e);
        //    }
        //}
#endif

        protected override void Dispose(bool disposing)
        {
            try
            {
                // Only hook into events if we're at run-time.  Hooking events
                // at design-time can actually cause errors in the WF Designer.
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime
                    && pIsUsingRealTimeData)
                {
                    Devices.BearingChanged -= new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
                }
            }
            catch
            {
            }

#if !PocketPC
			// Get the interpolation thread out of a loop 
			_IsInterpolationActive = false;

            if (_InterpolationThread != null)
            {

                if (_AnimationWaitHandle != null)
                {
                    try
                    {
                        _AnimationWaitHandle.Set();
                    }
                    catch
                    {
                    }
                }

                if (!_InterpolationThread.Join(MaximumGracefulShutdownTime))
                {
                    try
                    {
                        _InterpolationThread.Abort();
                    }
                    catch
                    {
                    }
                }

            }

			if (_AnimationWaitHandle != null)
			{
				try
				{
					_AnimationWaitHandle.Close();
				}
				catch
				{
				}
				finally
				{
					_AnimationWaitHandle = null;
				}
			}
#endif

            if (_CenterPen != null)
            {
                try
                {
                    _CenterPen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _CenterPen = null;
                }
            }
            if (_MinorTickPen != null)
            {
                try
                {
                    _MinorTickPen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _MinorTickPen = null;
                }
            }
            if (_MajorTickPen != null)
            {
                try
                {
                    _MajorTickPen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _MajorTickPen = null;
                }
            }
            if (_DirectionLabelFont != null)
            {
                try
                {
                    _DirectionLabelFont.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _DirectionLabelFont = null;
                }
            }
            if (_DirectionTickPen != null)
            {
                try
                {
                    _DirectionTickPen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _DirectionTickPen = null;
                }
            }
            if (_AngleLabelFont != null)
            {
                try
                {
                    _AngleLabelFont.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _AngleLabelFont = null;
                }
            }
#if !PocketPC
			if (pNeedleShadowBrush != null)
			{
				pNeedleShadowBrush.Dispose();
				pNeedleShadowBrush = null;
			}
#endif
            if (pNorthNeedleBrush != null)
            {
                try
                {
                    pNorthNeedleBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pNorthNeedleBrush = null;
                }
            }
            if (pNorthNeedlePen != null)
            {
                try
                {
                    pNorthNeedlePen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pNorthNeedlePen = null;
                }
            }
            if (pSouthNeedleBrush != null)
            {
                try
                {
                    pSouthNeedleBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pSouthNeedleBrush = null;
                }
            }
            if (pSouthNeedlePen != null)
            {
                try
                {
                    pSouthNeedlePen.Dispose();
                }
                catch
                {
                }
                finally
                {
                    pSouthNeedlePen = null;
                }
            }
            if (_DirectionLabelBrush != null)
            {
                try
                {
                    _DirectionLabelBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _DirectionLabelBrush = null;
                }
            }
            if (_AngleLabelBrush != null)
            {
                try
                {
                    _AngleLabelBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _AngleLabelBrush = null;
                }
            }

            // Move on down the line
            try
            {
                base.Dispose(disposing);
            }
            catch
            {
            }
        }

       

#if !PocketPC || DesignTime
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
				return _Bearing;
			}
			set
			{
				if(_Bearing.Equals(value)) return;
				_Bearing = value;
#if PocketPC
                InvokeRepaint();
#else
				if(IsDisposed)
					return;

				lock(_ValueInterpolator)
				{
					// Are we changing direction?
					if(_Bearing.DecimalDegrees >= _ValueInterpolator.Minimum
						&& _Bearing.DecimalDegrees > _ValueInterpolator[_InterpolationIndex])
					{
						// No.  Just set the new maximum
						_ValueInterpolator.Maximum = _Bearing.DecimalDegrees;
					}					
					else if(_Bearing.DecimalDegrees < _ValueInterpolator.Minimum)
					{
						// We're changing directions, so stop then accellerate again
						_ValueInterpolator.Minimum = _ValueInterpolator[_InterpolationIndex];
						_ValueInterpolator.Maximum = _Bearing.DecimalDegrees;
						_InterpolationIndex = 0;
					}
					else if(_Bearing.DecimalDegrees > _ValueInterpolator.Minimum
						&& _Bearing.DecimalDegrees < _ValueInterpolator[_InterpolationIndex])
					{
						// We're changing directions, so stop then accellerate again
						_ValueInterpolator.Minimum = _ValueInterpolator[_InterpolationIndex];
						_ValueInterpolator.Maximum = _Bearing.DecimalDegrees;
						_InterpolationIndex = 0;
					}
					else if(_Bearing.DecimalDegrees > _ValueInterpolator.Maximum)
					{
						// No.  Just set the new maximum
						_ValueInterpolator.Maximum = _Bearing.DecimalDegrees;
					}

					// If the difference is > 180°, adjust so that it moves the right direction
					if(_ValueInterpolator.Maximum - _ValueInterpolator.Minimum > 180)
						_ValueInterpolator.Minimum = _ValueInterpolator.Minimum % 360.0 + 360.0;				
					else if(_ValueInterpolator.Minimum - _ValueInterpolator.Maximum > 180)
						_ValueInterpolator.Maximum = _ValueInterpolator.Maximum % 360.0 + 360.0;				
				}
				// And activate the interpolation thread
//				if((InterpolationThread.ThreadState & ThreadState.Suspended) != 0)
//					InterpolationThread.Resume();
				_AnimationWaitHandle.Set();

#endif

                OnValueChanged(new AzimuthEventArgs(_Bearing));
			}
		}

        private void OnValueChanged(AzimuthEventArgs e)
        {
            if (ValueChanged != null)
				ValueChanged(this, e);
        }

#if !PocketPC
		[Category("Behavior")]
		[DefaultValue(typeof(InterpolationMethod), "CubicEaseOut")]
		[Description("Controls how the control smoothly transitions from one value to another.")]
		////[CLSCompliant(false)]
		public InterpolationMethod ValueInterpolationMethod
		{
			get
			{
				return _ValueInterpolator.InterpolationMethod;
			}
			set
			{
				_ValueInterpolator.InterpolationMethod = value;
			}
		}
#endif

#if !PocketPC || DesignTime
#if PocketPC
		[DefaultValue(typeof(Angle), "0")]
#else
		[DefaultValue(typeof(Angle), "30.0")]
#endif
		[Category("Angle Labels")]
		[Description("Controls the number of degrees in between each label around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
		public Angle AngleLabelInterval
		{
			get
			{
				return _AngleLabelInterval;
			}
			set
			{
				if(_AngleLabelInterval.Equals(value)) return;
				_AngleLabelInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _AngleLabelBrush.Color;
			}
			set
			{
				if(_AngleLabelBrush.Color.Equals(value)) return;
				_AngleLabelBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
#if PocketPC
		[DefaultValue(typeof(Font), "Tahoma, 6pt")]
#else
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
				return _AngleLabelFont;
			}
			set
			{
				if(_AngleLabelFont.Equals(value)) return;
				_AngleLabelFont = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Direction Labels")]
		[DefaultValue(typeof(Angle), "45")]
		[Description("Controls the number of degrees in between each compass direction (i.e. \"N\", \"NW\") around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
			////[CLSCompliant(false)]
		public Angle DirectionLabelInterval
		{
			get
			{
				return _DirectionLabelInterval;
			}
			set
			{
				if(_DirectionLabelInterval.Equals(value)) return;
				_DirectionLabelInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _DirectionLabelBrush.Color;
			}
			set
			{
				if(_DirectionLabelBrush.Color.Equals(value)) return;
				_DirectionLabelBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _DirectionTickPen.Color;
			}
			set
			{
				if(_DirectionTickPen.Color.Equals(value)) return;
				_DirectionTickPen.Color = value;
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
                // Anything to do?
				if(pIsUsingRealTimeData == value) 
                    return;

                // Record the new value
				pIsUsingRealTimeData = value;

				// Set the control to the last known bearing (if any)
                if (pIsUsingRealTimeData)
                {
                    // Hook into real-time events.
                    // Only hook into events if we're at run-time.  Hooking events
                    // at design-time can actually cause errors in the WF Designer.
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        Devices.BearingChanged += new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
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
                        Devices.BearingChanged -= new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
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
				return _DirectionLabelFont;
			}
			set
			{
				if(_DirectionLabelFont.Equals(value)) return;
				_DirectionLabelFont = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _AngleLabelFormat;
			}
			set
			{
				if(_AngleLabelFormat.Equals(value)) return;
				_AngleLabelFormat = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
		////[CLSCompliant(false)]
		public Angle MinorTickInterval
		{
			get
			{
				return _MinorTickInterval;
			}
			set
			{
				if(_MinorTickInterval == value) return;
				_MinorTickInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _MinorTickPen.Color;
			}
			set
			{
				_MinorTickPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _MajorTickInterval;
			}
			set
			{
				if(_MajorTickInterval == value) return;
				_MajorTickInterval = value;
				InvokeRepaint();
			}
		}

//#if Framework20 && !PocketPC
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        public override Azimuth Origin
//        {
//            get
//            {
//                return base.Origin;
//            }
//            set
//            {
//                base.Origin = value;
//            }
//        }        
//
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        public override Angle Rotation
//        {
//            get
//            {
//                return base.Rotation;
//            }
//            set
//            {
//                base.Rotation = value;
//            }
//        }
//#endif

#if !PocketPC || DesignTime
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
				return _MajorTickPen.Color;
			}
			set
			{
				_MajorTickPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pNorthNeedleBrush.Color;
			}
			set
			{
				if(pNorthNeedleBrush.Color.Equals(value)) return;
				pNorthNeedleBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pNorthNeedlePen.Color;
			}
			set
			{
				if(pNorthNeedlePen.Color.Equals(value)) return;
				pNorthNeedlePen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pSouthNeedleBrush.Color;
			}
			set
			{
				if(pSouthNeedleBrush.Color.Equals(value)) return;
				pSouthNeedleBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return pSouthNeedlePen.Color;
			}
			set
			{
				if(pSouthNeedlePen.Color.Equals(value)) return;
				pSouthNeedlePen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC
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
				return pNeedleShadowBrush.Color;
			}
			set
			{
				if(pNeedleShadowBrush.Color.Equals(value)) return;
				pNeedleShadowBrush.Color = value;
				InvokeRepaint();
			}
		}
#endif

#if !PocketPC
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
				return pNeedleShadowSize;
			}
			set
			{
				pNeedleShadowSize = value;
				InvokeRepaint();
			}
		}
#endif

		protected override void OnPaintOffScreen(PaintEventArgs e)
		{
			PolarGraphics f = CreatePolarGraphics(e.Graphics);
			
			// What bearing are we drawing?
#if PocketPC
            Azimuth BearingToRender = _Bearing;
#else
			Azimuth BearingToRender;
			try
			{
				BearingToRender = new Azimuth(_ValueInterpolator[_InterpolationIndex]);
			}
			catch
			{
				throw;
			}
#endif

            // Cache drawing options in order to prevent race conditions during
            // drawing! 
            double MinorInterval = _MinorTickInterval.DecimalDegrees;
            double MajorInterval = _MajorTickInterval.DecimalDegrees;
            double DirectionInterval = _DirectionLabelInterval.DecimalDegrees;
            double AngleInterval = _AngleLabelInterval.DecimalDegrees;


			// Draw tick marks
            if (MinorInterval > 0)
			{
                for (double angle = 0; angle < 360; angle += MinorInterval)
				{
					// And draw a line
					f.DrawLine(_MinorTickPen, new PolarCoordinate(98, angle), new PolarCoordinate(100, angle));
				}
			}
			// Draw tick marks
            if (MajorInterval > 0)
			{
                for (double angle = 0; angle < 360; angle += MajorInterval)
				{
					// And draw a line
					f.DrawLine(_MajorTickPen, new PolarCoordinate(95, angle), new PolarCoordinate(100, angle));
				}
			}
            if (DirectionInterval > 0)
			{
                for (double angle = 0; angle < 360; angle += DirectionInterval)
				{
					// And draw a line
					f.DrawLine(_DirectionTickPen, new PolarCoordinate(92, angle), new PolarCoordinate(100, angle));
				}
			}
            if (AngleInterval > 0)
			{
                for (double angle = 0; angle < 360; angle += AngleInterval)
				{
					// Get the coordinate of the line's start
					PolarCoordinate start = new PolarCoordinate(60, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
#if PocketPC
					f.DrawCenteredString(((Angle)angle).ToString(_AngleLabelFormat, CultureInfo.CurrentCulture), _AngleLabelFont, _AngleLabelBrush, start);
#else
                    f.DrawRotatedString(((Angle)angle).ToString(_AngleLabelFormat, CultureInfo.CurrentCulture), _AngleLabelFont, _AngleLabelBrush, start);
#endif
				}
			}
            if (DirectionInterval > 0)
			{
                for (double angle = 0; angle < 360; angle += DirectionInterval)
				{
					// Get the coordinate of the line's start
					PolarCoordinate start = new PolarCoordinate(80, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
#if PocketPC
                    f.DrawCenteredString(((Azimuth)angle).ToString("c", CultureInfo.CurrentCulture), _DirectionLabelFont, _DirectionLabelBrush, start);
#else
                    f.DrawRotatedString(((Azimuth)angle).ToString("c", CultureInfo.CurrentCulture), _DirectionLabelFont, _DirectionLabelBrush, start);
#endif
				}
			}

			// Draw an ellipse at the center
			f.DrawEllipse(_CenterPen, PolarCoordinate.Empty, 10);

			// Now draw the needle shadow			
			PolarCoordinate[] NeedleNorth = NeedlePointsNorth.Clone() as PolarCoordinate[];
			PolarCoordinate[] NeedleSouth = NeedlePointsSouth.Clone() as PolarCoordinate[];

			// Adjust the needle to the current bearing
			for(int index = 0; index < NeedleNorth.Length; index++)
			{
				NeedleNorth[index] = NeedleNorth[index].Rotate(BearingToRender.DecimalDegrees);
				NeedleSouth[index] = NeedleSouth[index].Rotate(BearingToRender.DecimalDegrees);
			}

#if !PocketPC
			// Now draw a shadow
			f.Graphics.TranslateTransform(pNeedleShadowSize.Width, pNeedleShadowSize.Height, MatrixOrder.Append);

			f.FillPolygon(pNeedleShadowBrush, NeedleNorth);
			f.FillPolygon(pNeedleShadowBrush, NeedleSouth);

			f.Graphics.ResetTransform();
#endif

			f.FillPolygon(pNorthNeedleBrush, NeedleNorth);
			f.DrawPolygon(pNorthNeedlePen, NeedleNorth);
			f.FillPolygon(pSouthNeedleBrush, NeedleSouth);
			f.DrawPolygon(pSouthNeedlePen, NeedleSouth);

		}

		private void Devices_CurrentBearingChanged(object sender, AzimuthEventArgs e)
		{
			if(pIsUsingRealTimeData)
				Value = e.Azimuth;
		}

#if !PocketPC
		protected override void OnTargetFrameRateChanged(int framesPerSecond)
		{
			base.OnTargetFrameRateChanged(framesPerSecond);
			// Recalculate our things
			_ValueInterpolator.Count = framesPerSecond;
			// Adjust the index if it's outside of bounds
			if(_InterpolationIndex > _ValueInterpolator.Count - 1)
				_InterpolationIndex = _ValueInterpolator.Count - 1;
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
//				if(InterpolationThread != null)
//				{
//					if((InterpolationThread.ThreadState & ThreadState.Suspended) != 0)
//						InterpolationThread.Resume();
//					while(!InterpolationThread.Join(1000))
//					{
//						if((InterpolationThread.ThreadState & ThreadState.AbortRequested) == 0)
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
			while(_IsInterpolationActive)
			{
				try
				{
					// Wait for interpolation to actually be needed
					_AnimationWaitHandle.WaitOne();
					//InterpolationThread.Suspend();
					// If we're shutting down, just exit
					if (!_IsInterpolationActive)
						break;
					// Keep updating interpolation until we're done
					while (_IsInterpolationActive && _InterpolationIndex < _ValueInterpolator.Count)
					{
						// Render the next value
						InvokeRepaint();
						_InterpolationIndex++;
						// Wait for the next frame
						Thread.Sleep(1000 / _ValueInterpolator.Count);
					}
					// Reset interpolation
					_ValueInterpolator.Minimum = _ValueInterpolator.Maximum;
					_InterpolationIndex = 0;
					_AnimationWaitHandle.Reset();
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
