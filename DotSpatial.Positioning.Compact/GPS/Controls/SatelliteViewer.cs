using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Positioning.Drawing;
using DotSpatial.Positioning.Gps.IO;
using DotSpatial.Positioning.Gps;
#if !PocketPC || DesignTime || Framework20
using System.ComponentModel;
#endif

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif


namespace DotSpatial.Positioning.Gps.Controls
{
	/// <summary>
	/// Controls whether controls are rotated to show the current bearing straight up.
	/// </summary>
	public enum RotationOrientation
	{
		/// <summary>
		/// The control will be rotated so that North always points to the top of the screen.
		/// </summary>
		NorthUp = 0,
		/// <summary>
		/// The control will be rotated so the the current bearing points to the top of the screen.
		/// </summary>
		TrackUp = 1
	}
	
	/// <summary>
	/// Represents a user control used to display the location and signal strength of GPS satellites.
	/// </summary>
#if !PocketPC || DesignTime
	[ToolboxBitmap(typeof(SatelliteViewer))]
	[DefaultProperty("Satellites")]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
#endif
	//[CLSCompliant(false)]
	public sealed class SatelliteViewer : PolarControl
	{
		private Azimuth _Bearing = Azimuth.North;
#if PocketPC
        private Angle _MinorTickInterval = new Angle(5);
#if Framework20
        private Pen _MajorTickPen = new Pen(Color.Black, 2.0f);
#else
		private Pen _MajorTickPen = new Pen(Color.Black);
#endif
#else
		private Angle _MinorTickInterval = new Angle(2);
		private Pen _MajorTickPen = new Pen(Color.Black, 2.0f);
#endif
		private Angle _MajorTickInterval = new Angle(15);
		private Pen _MinorTickPen = new Pen(Color.Black);
		private Angle _DirectionLabelInterval = new Angle(45);
		private string _DirectionLabelFormat = "c";
		private SolidBrush _DirectionLabelBrush = new SolidBrush(Color.Black);
		private Pen _HalfwayUpPen = new Pen(Color.Gray);
#if PocketPC
        private Font _DirectionLabelFont = new Font("Tahoma", 7.0f, FontStyle.Regular);
        private Font _PseudorandomNumberFont = new Font("Tahoma", 7.0f, FontStyle.Bold);
        private SolidBrush _SatelliteFixBrush = new SolidBrush(Color.LimeGreen);
#else
		private Font _DirectionLabelFont = new Font("Tahoma", 12.0f, FontStyle.Bold);
		private Font _PseudorandomNumberFont = new Font("Tahoma", 9.0f, FontStyle.Regular);
#endif
		private SolidBrush _PseudorandomNumberBrush = new SolidBrush(Color.Black);
		private bool _IsUsingRealTimeData = true;
#if !PocketPC
		private SolidBrush _SatelliteShadowBrush = new SolidBrush(Color.FromArgb(32, 0, 0, 0));
		private Color _SatelliteFixColor = Color.LightGreen;
#endif

		private Color _SatelliteNoSignalFillColor = Color.Transparent;
		private Color _SatellitePoorSignalFillColor = Color.Red;
		private Color _SatelliteModerateSignalFillColor = Color.Orange;
		private Color _SatelliteGoodSignalFillColor = Color.Green;
		private Color _SatelliteExcellentSignalFillColor = Color.LightGreen;

		private Color _SatelliteNoSignalOutlineColor = Color.Transparent;
		private Color _SatellitePoorSignalOutlineColor = Color.Black;
		private Color _SatelliteModerateSignalOutlineColor = Color.Black;
		private Color _SatelliteGoodSignalOutlineColor = Color.Black;
		private Color _SatelliteExcellentSignalOutlineColor = Color.Black;

		private ColorInterpolator _FillNone = new ColorInterpolator(Color.Transparent, Color.Red, 10);
		private ColorInterpolator _FillPoor = new ColorInterpolator(Color.Red, Color.Orange, 10);
		private ColorInterpolator pFillModerate = new ColorInterpolator(Color.Orange, Color.Green, 10);
		private ColorInterpolator pFillGood = new ColorInterpolator(Color.Green, Color.LightGreen, 10);
		private ColorInterpolator pFillExcellent = new ColorInterpolator(Color.LightGreen, Color.White, 10);
		private ColorInterpolator pOutlineNone = new ColorInterpolator(Color.Transparent, Color.Gray, 10);
		private ColorInterpolator pOutlinePoor = new ColorInterpolator(Color.Gray, Color.Gray, 10);
		private ColorInterpolator pOutlineModerate = new ColorInterpolator(Color.Gray, Color.Gray, 10);
		private ColorInterpolator pOutlineGood = new ColorInterpolator(Color.Gray, Color.Gray, 10);
		private ColorInterpolator pOutlineExcellent = new ColorInterpolator(Color.Gray, Color.LightGreen, 10);

        private List<Satellite> _Satellites;
        private List<Satellite> _FixedSatellites;
        private RotationOrientation _RotationOrientation = RotationOrientation.TrackUp;

		//private object RenderSyncLock = new object();

		private static PointD[] Icon = new PointD[]
			{
				new PointD(0, 0),
				new PointD(0, 10),
				new PointD(3, 10),
				new PointD(3, 5),
				new PointD(4, 5),
				new PointD(4, 10),
				new PointD(8, 10),
				new PointD(8, 5),
				new PointD(10, 5),
				new PointD(10, 6),
				new PointD(12, 8),
				new PointD(14, 8),
				new PointD(16, 6),
				new PointD(16, 5),
				new PointD(18, 5),
				new PointD(18, 10),
				new PointD(22, 10),
				new PointD(22, 5),
				new PointD(23, 5),
				new PointD(23, 10),
				new PointD(26, 10),
				new PointD(26, 0),
				new PointD(23, 0),
				new PointD(23, 5),
				new PointD(22, 5),
				new PointD(22, 0),
				new PointD(18, 0),
				new PointD(18, 5),
				new PointD(16, 5),
				new PointD(16, 4),
				new PointD(14, 2),
				new PointD(12, 2),
				new PointD(10, 4),
				new PointD(10, 5),
				new PointD(8, 5),
				new PointD(8, 0),
				new PointD(4, 0),
				new PointD(4, 5),
				new PointD(3, 5),
				new PointD(3, 0),
				new PointD(0, 0)
			};
		private static PointD IconCenter = new PointD(13, 5);

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public SatelliteViewer()
			: base("DotSpatial.Positioning Multithreaded Satellite Viewer Control (http://dotspatial.codeplex.com)")
		{
            //MessageBox.Show("SatelliteViewer Initialization started.");

            _Satellites = new List<Satellite>();
			Orientation = PolarCoordinateOrientation.Clockwise;
			Origin = Azimuth.North;
			// Set the max and min
			CenterR = 0;
			MaximumR = 90;            

#if PocketPC
#if Framework20
            _HalfwayUpPen.DashStyle = DashStyle.Dash;
#endif
#else
			_HalfwayUpPen.DashStyle = DashStyle.DashDotDot;
#endif

#if PocketPC && !Framework20 && !DesignTime
            // Bind global events when GPS data changes
            DotSpatial.Positioning.Gps.IO.Devices.SatellitesChanged += new SatelliteCollectionEventHandler(Devices_CurrentSatellitesChanged);
            DotSpatial.Positioning.Gps.IO.Devices.BearingChanged += new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
#endif
		}

#if !PocketPC || Framework20
        protected override void OnHandleCreated(EventArgs e)
        {
            // Subscribe to events
            try
            {
                base.OnHandleCreated(e);

                // Only hook into events if we're at run-time.  Hooking events
                // at design-time can actually cause errors in the WF Designer.
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                {
                    Devices.SatellitesChanged += new EventHandler<SatelliteListEventArgs>(Devices_CurrentSatellitesChanged);
                    Devices.BearingChanged += new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
                }
            }
            catch
            {
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            try
            {
                // Only hook into events if we're at run-time.  Hooking events
                // at design-time can actually cause errors in the WF Designer.
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                {
                    Devices.SatellitesChanged -= new EventHandler<SatelliteListEventArgs>(Devices_CurrentSatellitesChanged);
                    Devices.BearingChanged -= new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
                }
            }
            catch
            {
            }
            finally
            {
                base.OnHandleDestroyed(e);
            }
        }
#endif

		protected override void OnInitialize()
		{

            //MessageBox.Show("SatelliteViewer OnInitialize.");

            base.OnInitialize();

			// Set the collection if it's design mode
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                // TODO: How to Randomize satellites?
                _Satellites = new List<Satellite>(); // SatelliteCollection.Random(45);
            }
            else
            {
                if (_IsUsingRealTimeData)
                {
                    // Merge it with live satellite data
                    _Satellites = Devices.Satellites;
                }
            }

            //MessageBox.Show("SatelliteViewer OnInitialize completed.");

		}



		protected override void Dispose(bool disposing)
		{
#if PocketPC && !Framework20 && !DesignTime

            //MessageBox.Show("SatelliteViewer Dispose.");

			// Bind global events when GPS data changes
			try
			{
				DotSpatial.Positioning.Gps.IO.Devices.SatellitesChanged -= new SatelliteCollectionEventHandler(Devices_CurrentSatellitesChanged);
			}
			catch
			{
			}
			try
			{
				DotSpatial.Positioning.Gps.IO.Devices.BearingChanged -= new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
			}
			catch
			{
			}
#endif

#if PocketPC
			if (_SatelliteFixBrush != null)
			{
				try
				{
					_SatelliteFixBrush.Dispose();
				}
				catch
				{
				}
				finally
				{
					_SatelliteFixBrush = null;
				}
			}
#endif

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
#if !PocketPC
			if (_SatelliteShadowBrush != null)
			{
				_SatelliteShadowBrush.Dispose();
				_SatelliteShadowBrush = null;
			}
#endif
			if (_PseudorandomNumberFont != null)
			{
				try
				{
					_PseudorandomNumberFont.Dispose();
				}
				catch
				{
				}
				finally
				{
					_PseudorandomNumberFont = null;
				}
			}
			if (_PseudorandomNumberBrush != null)
			{
				try
				{
					_PseudorandomNumberBrush.Dispose();
				}
				catch
				{
				}
				finally
				{
					_PseudorandomNumberBrush = null;
				}
			}
			if (_HalfwayUpPen != null)
			{
				try
				{
					_HalfwayUpPen.Dispose();
				}
				catch
				{
				}
				finally
				{
					_HalfwayUpPen = null;
				}
			}

			try
			{
				base.Dispose(disposing);
			}
			catch
			{
			}
		}

		

		private Color GetFillColor(SignalToNoiseRatio signal)
		{
			if (signal.Value < 10)
				return _FillNone[signal.Value];
			else if (signal.Value < 20)
				return _FillPoor[signal.Value - 10];
			else if (signal.Value < 30)
				return pFillModerate[signal.Value - 20];
			else if (signal.Value < 40)
				return pFillGood[signal.Value - 30];
			else if (signal.Value < 50)
				return pFillExcellent[signal.Value - 40];
			else
				return pFillExcellent[9];
		}

		private Color GetOutlineColor(SignalToNoiseRatio signal)
		{
			if (signal.Value < 10)
				return pOutlineNone[signal.Value];
			else if (signal.Value < 20)
				return pOutlinePoor[signal.Value - 10];
			else if (signal.Value < 30)
				return pOutlineModerate[signal.Value - 20];
			else if (signal.Value < 40)
				return pOutlineGood[signal.Value - 30];
			else if (signal.Value < 50)
				return pOutlineExcellent[signal.Value - 40];
			else
				return pOutlineExcellent[9];
		}


#if !PocketPC || DesignTime
		[Category("Behavior")]
		[DefaultValue(typeof(Azimuth), "0")]
		[Description("Controls the amount of rotation applied to the entire control to indicate the current direction of travel.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
		public Azimuth Bearing
		{
			get
			{
				return _Bearing;
			}
			set
			{
				if (_Bearing.Equals(value)) return;
				_Bearing = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Behavior")]
		[DefaultValue(typeof(bool), "True")]
		[Description("Controls whether the Satellites property is set manually, or automatically read from any available GPS device.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public bool IsUsingRealTimeData
		{
			get
			{
				return _IsUsingRealTimeData;
			}
			set
			{
				if (_IsUsingRealTimeData == value) 
                    return;

                //MessageBox.Show("IsUsingRealTimeData started.");


				_IsUsingRealTimeData = value;

#if !DesignTime
				if (_IsUsingRealTimeData)
				{
					// Use current satellite information
					_Satellites = Devices.Satellites;

					// Also set the bearing
					if(this._RotationOrientation == RotationOrientation.TrackUp)
						Rotation = new Angle(-Devices.Bearing.DecimalDegrees);
				}
				else
				{
					_Satellites.Clear();
					// Clear the rotation
					if(this._RotationOrientation == RotationOrientation.TrackUp)
						Rotation = Angle.Empty;
				}
#endif
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		#if PocketPC
			[DefaultValue(typeof(Angle), "5")]
		#else
			[DefaultValue(typeof(Angle), "2")]
		#endif
		[Category("Tick Marks")]
		[Description("Controls the number of degrees in between each smaller tick mark around the control.")]
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
				if (_MinorTickInterval.Equals(value)) return;
				_MinorTickInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Direction Labels")]
		[DefaultValue(typeof(string), "c")]
		[Description("Controls the format of compass directions drawn around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public string DirectionLabelFormat
		{
			get
			{
				return _DirectionLabelFormat;
			}
			set
			{
				if (_DirectionLabelFormat == value) return;
				_DirectionLabelFormat = value;
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
				if (_MajorTickInterval.Equals(value)) return;
				_MajorTickInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Tick Marks")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color used to draw smaller tick marks around the control.")]
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
				lock (_MinorTickPen)
				{
					if (_MinorTickPen.Color.Equals(value)) return;
					_MinorTickPen.Color = value;
				}
				Thread.Sleep(0);
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Tick Marks")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color used to draw larger tick marks around the control.")]
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
				lock (_MajorTickPen)
				{
					if (_MajorTickPen.Color.Equals(value)) return;
					_MajorTickPen.Color = value;
				}
				Thread.Sleep(0);
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Direction Labels")]
		[DefaultValue(typeof(Angle), "45")]
		[Description("Controls the number of degrees in between each compass label around the control.")]
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
				if (_DirectionLabelInterval.Equals(value)) return;
				_DirectionLabelInterval = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Direction Labels")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color used to display compass direction letters around the control.")]
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
				lock (_DirectionLabelBrush)
				{
					if (_DirectionLabelBrush.Color.Equals(value)) return;
					_DirectionLabelBrush.Color = value;
				}
				Thread.Sleep(0);
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Direction Labels")]
		#if PocketPC
			[DefaultValue(typeof(Font), "Tahoma, 7pt")]
		#else
			[DefaultValue(typeof(Font), "Tahoma, 12pt, style=Bold")]
		#endif
		[Description("Controls the font used to draw compass labels around the control.")]
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
				if (_DirectionLabelFont.Equals(value)) return;
				_DirectionLabelFont = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Transparent")]
		[Description("Controls the color inside of satellite icons with no signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color NoSignalFillColor
		{
			get
			{
				return _SatelliteNoSignalFillColor;
			}
			set
			{
				if (_SatelliteNoSignalFillColor.Equals(value)) return;
				_SatelliteNoSignalFillColor = value;
				_FillNone.EndColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Red")]
		[Description("Controls the color inside of satellite icons with a weak signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color PoorSignalFillColor
		{
			get
			{
				return _SatellitePoorSignalFillColor;
			}
			set
			{
				if (_SatellitePoorSignalFillColor.Equals(value)) return;
				_SatellitePoorSignalFillColor = value;
				_FillNone.EndColor = value;
				_FillPoor.StartColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Orange")]
		[Description("Controls the color inside of satellite icons with a moderate signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color ModerateSignalFillColor
		{
			get
			{
				return _SatelliteModerateSignalFillColor;
			}
			set
			{
				if (_SatelliteModerateSignalFillColor.Equals(value)) return;
				_SatelliteModerateSignalFillColor = value;
				pFillModerate.StartColor = value;
				_FillPoor.EndColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Green")]
		[Description("Controls the color inside of satellite icons with a strong signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color GoodSignalFillColor
		{
			get
			{
				return _SatelliteGoodSignalFillColor;
			}
			set
			{
				if (_SatelliteGoodSignalFillColor.Equals(value)) return;
				_SatelliteGoodSignalFillColor = value;
				pFillGood.StartColor = value;
				pFillModerate.EndColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "LightGreen")]
		[Description("Controls the color inside of satellite icons with a very strong signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color ExcellentSignalFillColor
		{
			get
			{
				return _SatelliteExcellentSignalFillColor;
			}
			set
			{
				if (_SatelliteExcellentSignalFillColor.Equals(value)) return;
				_SatelliteExcellentSignalFillColor = value;
				pFillGood.EndColor = value;
				pFillExcellent.StartColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Transparent")]
		[Description("Controls the color around satellite icons with no signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color NoSignalOutlineColor
		{
			get
			{
				return _SatelliteNoSignalOutlineColor;
			}
			set
			{
				if (_SatelliteNoSignalOutlineColor.Equals(value)) return;
				_SatelliteNoSignalOutlineColor = value;
				pOutlineNone.EndColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color around satellite icons with a weak signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color PoorSignalOutlineColor
		{
			get
			{
				return _SatellitePoorSignalOutlineColor;
			}
			set
			{
				if (_SatellitePoorSignalOutlineColor.Equals(value)) return;
				_SatellitePoorSignalOutlineColor = value;
				pOutlineNone.EndColor = value;
				pOutlinePoor.StartColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color around satellite icons with a moderate signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color ModerateSignalOutlineColor
		{
			get
			{
				return _SatelliteModerateSignalOutlineColor;
			}
			set
			{
				if (_SatelliteModerateSignalOutlineColor.Equals(value)) return;
				_SatelliteModerateSignalOutlineColor = value;
				pOutlinePoor.EndColor = value;
				pOutlineModerate.StartColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color around satellite icons with a strong signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color GoodSignalOutlineColor
		{
			get
			{
				return _SatelliteGoodSignalOutlineColor;
			}
			set
			{
				if (_SatelliteGoodSignalOutlineColor.Equals(value)) return;
				_SatelliteGoodSignalOutlineColor = value;
				pOutlineModerate.EndColor = value;
				pOutlineGood.StartColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color around satellite icons with a very strong signal.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color ExcellentSignalOutlineColor
		{
			get
			{
				return _SatelliteExcellentSignalOutlineColor;
			}
			set
			{
				if (_SatelliteExcellentSignalOutlineColor.Equals(value)) return;
				_SatelliteExcellentSignalOutlineColor = value;
				pOutlineGood.EndColor = value;
				pOutlineExcellent.StartColor = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "LimeGreen")]
		[Description("Controls the color of the ellipse drawn around fixed satellites.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color FixColor
		{
			get
			{
#if PocketPC
				return _SatelliteFixBrush.Color;
#else
				return _SatelliteFixColor;
#endif
			}
			set
			{
#if PocketPC
				if(_SatelliteFixBrush.Color.Equals(value))
					return;
				_SatelliteFixBrush.Color = value;
#else
				if (_SatelliteFixColor.Equals(value)) return;
				_SatelliteFixColor = value;
#endif
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Behavior")]
		[DefaultValue(typeof(RotationOrientation), "TrackUp")]
		[Description("Controls which bearing points straight up on the screen.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public RotationOrientation RotationOrientation
		{
			get
			{
				return _RotationOrientation;
			}
			set
			{
				if(_RotationOrientation == value)
					return;
				_RotationOrientation = value;

				// If this becomes active, set the current bearing
				if(_RotationOrientation == RotationOrientation.TrackUp)
					Rotation = new Angle(-Devices.Bearing.DecimalDegrees);
				else
					Rotation = Angle.Empty;

				//InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellites")]
		[Description("Contains the list of satellites drawn inside of the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public List<Satellite> Satellites
		{
			get
			{
				return _Satellites;
			}
			set
			{
			    _Satellites = value;
                
                // Extract the fixed satellites
                if (value == null)
                    _FixedSatellites = null;
                else
                    _FixedSatellites = Satellite.GetFixedSatellites(_Satellites);

                // Redraw the control
				if (_Satellites != null)
					InvokeRepaint();
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
            PolarGraphics f = CreatePolarGraphics(e.Graphics);

            // Cache drawing intervals and such to prevent a race condition
            double MinorInterval = _MinorTickInterval.DecimalDegrees;
            double MajorInterval = _MajorTickInterval.DecimalDegrees;
            double DirectionInterval = _DirectionLabelInterval.DecimalDegrees;

            // Draw tick marks
            if (MinorInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += MinorInterval)
                {
                    // And draw a line
                    f.DrawLine(_MinorTickPen, new PolarCoordinate(88.0f, angle),
                        new PolarCoordinate(90, angle));
                }
            }

            // Draw tick marks
            if (MajorInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += MajorInterval)
                {
                    // And draw a line
                    f.DrawLine(_MajorTickPen, new PolarCoordinate(85.0f, angle), new PolarCoordinate(90, angle));
                }
            }

            if (DirectionInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += DirectionInterval)
                {
                    // Get the coordinate of the line's start
                    PolarCoordinate start = new PolarCoordinate(70, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
#if PocketPC
					f.DrawCenteredString(((Azimuth)angle).ToString(_DirectionLabelFormat, CultureInfo.CurrentCulture), _DirectionLabelFont, _DirectionLabelBrush, start);
#else
                    f.DrawRotatedString(((Azimuth)angle).ToString(_DirectionLabelFormat, CultureInfo.CurrentCulture), _DirectionLabelFont, _DirectionLabelBrush, start);
#endif
                }
            }

            // Draw an ellipse at the center
            f.DrawEllipse(_HalfwayUpPen, PolarCoordinate.Empty, 45);

            // Now draw each satellite
            int satelliteCount = 0;

            if (Satellites != null)
            {
                satelliteCount = Satellites.Count;
#if !PocketPC

                for (int index = 0; index < satelliteCount; index++)
                {
                    Satellite satellite = _Satellites[index];

                    // Don't draw if the satellite is stale
                    if (!satellite.IsActive && !DesignMode)
                        continue;

                    // Is the satellite transparent?
                    if (GetFillColor(satellite.SignalToNoiseRatio).A < _SatelliteShadowBrush.Color.A)
                        continue;

                    // Get the coordinate for this satellite
                    PolarCoordinate Center = new PolarCoordinate(Convert.ToSingle(90.0f - satellite.Elevation.DecimalDegrees),
                        satellite.Azimuth.DecimalDegrees, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    PointD CenterPoint = f.ToPointD(Center);

                    // Each icon is 30x30, so we'll translate it by half the distance
                    double pShadowSize = Math.Sin(Radian.FromDegrees(satellite.Elevation.DecimalDegrees).Value) * 7;

                    f.Graphics.TranslateTransform((float)(CenterPoint.X - IconCenter.X + pShadowSize), (float)(CenterPoint.Y - IconCenter.Y + pShadowSize));

                    // Draw each satellite
                    PointF[] SatelliteIcon = new PointF[Icon.Length];
                    for (int iconIndex = 0; iconIndex < Icon.Length; iconIndex++)
                    {
                        SatelliteIcon[iconIndex] = new PointF((float)Icon[iconIndex].X, (float)Icon[iconIndex].Y);
                    }

                    using (Matrix y = new Matrix())
                    {
                        y.RotateAt(Convert.ToSingle(satellite.Azimuth.DecimalDegrees - f.Rotation.DecimalDegrees + Origin.DecimalDegrees),
                            new PointF((float)IconCenter.X, (float)IconCenter.Y), MatrixOrder.Append);
                        y.TransformPoints(SatelliteIcon);
                    }

                    f.Graphics.FillPolygon(_SatelliteShadowBrush, SatelliteIcon);
                    f.Graphics.ResetTransform();
                }
#endif
            }

            // Now draw each satellite				
            if (_Satellites != null)
            {
                List<Satellite> fixedSatellites = Satellite.GetFixedSatellites(_Satellites);
                int fixedSatelliteCount = fixedSatellites.Count;

                for (int index = 0; index < fixedSatelliteCount; index++)
                {
                    Satellite satellite = fixedSatellites[index];
                    if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                    {
#if PocketPC


                        // Don't draw if the satellite is stale
                        if (!satellite.IsActive)
                            continue;
#else
                        // Don't draw if the satellite is stale
                        if (!satellite.IsActive && !DesignMode)
                            continue;
#endif
                    }

                    // Get the coordinate for this satellite
                    PolarCoordinate Center = new PolarCoordinate(Convert.ToSingle(90.0 - satellite.Elevation.DecimalDegrees),
                        satellite.Azimuth.DecimalDegrees, Azimuth.North, PolarCoordinateOrientation.Clockwise);

#if PocketPC
						f.FillEllipse(_SatelliteFixBrush, Center, 16);
#else
                    using (SolidBrush FixBrush = new SolidBrush(Color.FromArgb(Math.Min(255, fixedSatellites.Count * 20), _SatelliteFixColor)))
                    {
                        f.FillEllipse(FixBrush, Center, 16);
                    }
#endif
                }


                // Now draw each satellite
                for (int index = 0; index < satelliteCount; index++)
                {
                    Satellite satellite = _Satellites[index];
                    if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                    {
#if PocketPC
                        // Don't draw if the satellite is stale
                        if (!satellite.IsActive)
                            continue;
#else
                        // Don't draw if the satellite is stale
                        if (!satellite.IsActive && !DesignMode)
                            continue;
#endif
                    }

                    // Get the coordinate for this satellite
                    PolarCoordinate Center = new PolarCoordinate(Convert.ToSingle(90.0 - satellite.Elevation.DecimalDegrees),
                        satellite.Azimuth.DecimalDegrees, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    PointD CenterPoint = f.ToPointD(Center);

#if PocketPC
					// Manually rotate each point of the icon
					Point[] SatelliteIcon = new Point[Icon.Length];
					for (int index2 = 0; index2 < Icon.Length; index2++)
					{
						 PointD point = Icon[index2]
							.RotateAt(satellite.Azimuth.DecimalDegrees - Rotation.DecimalDegrees + Origin.DecimalDegrees, IconCenter)
							.Add(CenterPoint.X - IconCenter.X, CenterPoint.Y - IconCenter.Y);

                         SatelliteIcon[index2] = new Point((int)point.X, (int)point.Y);
					}

					Color SatelliteColor = GetFillColor(satellite.SignalToNoiseRatio);
					SolidBrush FillBrush = new SolidBrush(SatelliteColor);
					f.Graphics.FillPolygon(FillBrush, SatelliteIcon);
					FillBrush.Dispose();

#else
                    // Each icon is 30x30, so we'll translate it by half the distance
                    double pShadowSize = Math.Sin(Radian.FromDegrees(satellite.Elevation.DecimalDegrees).Value) * 7;

                    f.Graphics.TranslateTransform((float)(CenterPoint.X - IconCenter.X - pShadowSize * 0.1), (float)(CenterPoint.Y - IconCenter.Y - pShadowSize * 0.1));

                    // Draw each satellite
                    PointF[] SatelliteIcon = new PointF[Icon.Length];
                    for (int iconIndex = 0; iconIndex < Icon.Length; iconIndex++)
                    {
                        SatelliteIcon[iconIndex] = new PointF((float)Icon[iconIndex].X, (float)Icon[iconIndex].Y);
                    }

                    Matrix y = new Matrix();
                    y.RotateAt(Convert.ToSingle(satellite.Azimuth.DecimalDegrees - f.Rotation.DecimalDegrees + Origin.DecimalDegrees),
                        new PointF((float)IconCenter.X, (float)IconCenter.Y), MatrixOrder.Append);
                    y.TransformPoints(SatelliteIcon);
                    y.Dispose();

                    SolidBrush FillBrush = new SolidBrush(GetFillColor(satellite.SignalToNoiseRatio));
                    f.Graphics.FillPolygon(FillBrush, SatelliteIcon);
                    FillBrush.Dispose();

                    Pen FillPen = new Pen(GetOutlineColor(satellite.SignalToNoiseRatio), 1.0f);
                    f.Graphics.DrawPolygon(FillPen, SatelliteIcon);
                    FillPen.Dispose();

                    f.Graphics.ResetTransform();
#endif

#if PocketPC
					if (!SatelliteColor.Equals(Color.Transparent))
					{
						f.DrawCenteredString(satellite.PseudorandomNumber.ToString(), _PseudorandomNumberFont,
							_PseudorandomNumberBrush, new PolarCoordinate(Center.R - 11, Center.Theta.DecimalDegrees,
							Azimuth.North, PolarCoordinateOrientation.Clockwise));
					}
#else

                    f.DrawRotatedString(satellite.PseudorandomNumber.ToString(CultureInfo.CurrentCulture), _PseudorandomNumberFont,
                        _PseudorandomNumberBrush, new PolarCoordinate((float)(Center.R - 11), Center.Theta,
                        Azimuth.North, PolarCoordinateOrientation.Clockwise));
#endif
                }
            }
        }

#if !PocketPC
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "32, 0, 0, 0")]
		[Description("Controls the color of the shadow cast by satellite icons.")]
		public Color ShadowColor
		{
			get
			{
				return _SatelliteShadowBrush.Color;
			}
			set
			{
				lock (_SatelliteShadowBrush)
				{
					if (_SatelliteShadowBrush.Color.Equals(value)) return;
					_SatelliteShadowBrush.Color = value;
				}
				InvokeRepaint();
			}
		}
#endif

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		#if PocketPC
			[DefaultValue(typeof(Font), "Tahoma, 7pt, style=Bold")]
		#else
			[DefaultValue(typeof(Font), "Tahoma, 9pt")]
		#endif
		[Description("Controls the font used to display the ID of each satellite.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Font PseudorandomNumberFont
		{
			get
			{
				return _PseudorandomNumberFont;
			}
			set
			{
				if (_PseudorandomNumberFont.Equals(value)) return;
				_PseudorandomNumberFont = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Satellite Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color used to display the ID of each satellite.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color PseudorandomNumberColor
		{
			get
			{
				return _PseudorandomNumberBrush.Color;
			}
			set
			{
				lock (_PseudorandomNumberBrush)
				{
					if (_PseudorandomNumberBrush.Color.Equals(value)) return;
					_PseudorandomNumberBrush.Color = value;
				}
				Thread.Sleep(0);
				InvokeRepaint();
			}
		}

		private void SatelliteChanged(object sender, SatelliteEventArgs e)
		{
			InvokeRepaint();
		}

		private void Devices_CurrentSatellitesChanged(object sender, SatelliteListEventArgs e)
		{
			//TODO should this be done here or in a user defined event handler?
            if (_IsUsingRealTimeData) Satellites = (List<Satellite>)e.Satellites;
            InvokeRepaint();
		}

		private void Devices_CurrentBearingChanged(object sender, AzimuthEventArgs e)
		{
			if (_IsUsingRealTimeData && _RotationOrientation == RotationOrientation.TrackUp)
				Rotation = new Angle(e.Azimuth.DecimalDegrees);
		}
	}
}
