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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#if !PocketPC || DesignTime || Framework20

using System.ComponentModel;

#endif

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Forms
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

#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a user control used to display the location and signal strength of GPS satellites.
    /// </summary>
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
        private Azimuth _bearing = Azimuth.North;
#if PocketPC
        private Angle _MinorTickInterval = new Angle(5);
#if Framework20
        private Pen _MajorTickPen = new Pen(Color.Black, 2.0f);
#else
		private Pen _MajorTickPen = new Pen(Color.Black);
#endif
#else
        private Angle _minorTickInterval = new Angle(2);
        private Pen _majorTickPen = new Pen(Color.Black, 2.0f);
#endif
        private Angle _majorTickInterval = new Angle(15);
        private Pen _minorTickPen = new Pen(Color.Black);
        private Angle _directionLabelInterval = new Angle(45);
        private string _directionLabelFormat = "c";
        private SolidBrush _directionLabelBrush = new SolidBrush(Color.Black);
        private Pen _halfwayUpPen = new Pen(Color.Gray);
#if PocketPC
        private Font _DirectionLabelFont = new Font("Tahoma", 7.0f, FontStyle.Regular);
        private Font _PseudorandomNumberFont = new Font("Tahoma", 7.0f, FontStyle.Bold);
        private SolidBrush _SatelliteFixBrush = new SolidBrush(Color.LimeGreen);
#else
        private Font _directionLabelFont = new Font("Tahoma", 12.0f, FontStyle.Bold);
        private Font _pseudorandomNumberFont = new Font("Tahoma", 9.0f, FontStyle.Regular);
#endif
        private SolidBrush _pseudorandomNumberBrush = new SolidBrush(Color.Black);
        private bool _isUsingRealTimeData = true;
#if !PocketPC
        private SolidBrush _satelliteShadowBrush = new SolidBrush(Color.FromArgb(32, 0, 0, 0));
        private Color _satelliteFixColor = Color.LightGreen;
#endif

        private Color _satelliteNoSignalFillColor = Color.Transparent;
        private Color _satellitePoorSignalFillColor = Color.Red;
        private Color _satelliteModerateSignalFillColor = Color.Orange;
        private Color _satelliteGoodSignalFillColor = Color.Green;
        private Color _satelliteExcellentSignalFillColor = Color.LightGreen;

        private Color _satelliteNoSignalOutlineColor = Color.Transparent;
        private Color _satellitePoorSignalOutlineColor = Color.Black;
        private Color _satelliteModerateSignalOutlineColor = Color.Black;
        private Color _satelliteGoodSignalOutlineColor = Color.Black;
        private Color _satelliteExcellentSignalOutlineColor = Color.Black;

        private readonly ColorInterpolator _fillNone = new ColorInterpolator(Color.Transparent, Color.Red, 10);
        private readonly ColorInterpolator _fillPoor = new ColorInterpolator(Color.Red, Color.Orange, 10);
        private readonly ColorInterpolator _pFillModerate = new ColorInterpolator(Color.Orange, Color.Green, 10);
        private readonly ColorInterpolator _pFillGood = new ColorInterpolator(Color.Green, Color.LightGreen, 10);
        private readonly ColorInterpolator _pFillExcellent = new ColorInterpolator(Color.LightGreen, Color.White, 10);
        private readonly ColorInterpolator _pOutlineNone = new ColorInterpolator(Color.Transparent, Color.Gray, 10);
        private readonly ColorInterpolator _pOutlinePoor = new ColorInterpolator(Color.Gray, Color.Gray, 10);
        private readonly ColorInterpolator _pOutlineModerate = new ColorInterpolator(Color.Gray, Color.Gray, 10);
        private readonly ColorInterpolator _pOutlineGood = new ColorInterpolator(Color.Gray, Color.Gray, 10);
        private readonly ColorInterpolator _pOutlineExcellent = new ColorInterpolator(Color.Gray, Color.LightGreen, 10);

        private List<Satellite> _satellites;
        private RotationOrientation _rotationOrientation = RotationOrientation.TrackUp;

        //private object RenderSyncLock = new object();

        private static readonly PointD[] _icon = new[]
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
        private static PointD _iconCenter = new PointD(13, 5);

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public SatelliteViewer()
            : base("DotSpatial.Positioning Multithreaded Satellite Viewer Control (http://dotspatial.codeplex.com)")
        {
            //MessageBox.Show("SatelliteViewer Initialization started.");

            _satellites = new List<Satellite>();
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
            _halfwayUpPen.DashStyle = DashStyle.DashDotDot;
#endif

#if PocketPC && !Framework20 && !DesignTime
            // Bind global events when GPS data changes
            DotSpatial.Positioning.Gps.IO.Devices.SatellitesChanged += new SatelliteCollectionEventHandler(Devices_CurrentSatellitesChanged);
            DotSpatial.Positioning.Gps.IO.Devices.BearingChanged += new EventHandler<AzimuthEventArgs>(Devices_CurrentBearingChanged);
#endif
        }

#if !PocketPC || Framework20

        /// <inheritdocs/>
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
                    Devices.SatellitesChanged += Devices_CurrentSatellitesChanged;
                    Devices.BearingChanged += Devices_CurrentBearingChanged;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <inheritdocs/>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            try
            {
                // Only hook into events if we're at run-time.  Hooking events
                // at design-time can actually cause errors in the WF Designer.
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                {
                    Devices.SatellitesChanged -= Devices_CurrentSatellitesChanged;
                    Devices.BearingChanged -= Devices_CurrentBearingChanged;
                }
            }
            finally
            {
                base.OnHandleDestroyed(e);
            }
        }

#endif

        /// <inheritdocs/>
        protected override void OnInitialize()
        {
            //MessageBox.Show("SatelliteViewer OnInitialize.");

            base.OnInitialize();

            // Set the collection if it's design mode
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                // TODO: How to Randomize satellites?
                _satellites = new List<Satellite>(); // SatelliteCollection.Random(45);
            }
            else
            {
                if (_isUsingRealTimeData)
                {
                    // Merge it with live satellite data
                    _satellites = Devices.Satellites;
                }
            }

            //MessageBox.Show("SatelliteViewer OnInitialize completed.");
        }

        /// <inheritdocs/>
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
#if !PocketPC
            if (_satelliteShadowBrush != null)
            {
                _satelliteShadowBrush.Dispose();
                _satelliteShadowBrush = null;
            }
#endif
            if (_pseudorandomNumberFont != null)
            {
                try
                {
                    _pseudorandomNumberFont.Dispose();
                }
                finally
                {
                    _pseudorandomNumberFont = null;
                }
            }
            if (_pseudorandomNumberBrush != null)
            {
                try
                {
                    _pseudorandomNumberBrush.Dispose();
                }
                finally
                {
                    _pseudorandomNumberBrush = null;
                }
            }
            if (_halfwayUpPen != null)
            {
                try
                {
                    _halfwayUpPen.Dispose();
                }
                finally
                {
                    _halfwayUpPen = null;
                }
            }

            base.Dispose(disposing);
        }

        private Color GetFillColor(SignalToNoiseRatio signal)
        {
            if (signal.Value < 10)
                return _fillNone[signal.Value];
            if (signal.Value < 20)
                return _fillPoor[signal.Value - 10];
            if (signal.Value < 30)
                return _pFillModerate[signal.Value - 20];
            if (signal.Value < 40)
                return _pFillGood[signal.Value - 30];
            if (signal.Value < 50)
                return _pFillExcellent[signal.Value - 40];
            return _pFillExcellent[9];
        }

        private Color GetOutlineColor(SignalToNoiseRatio signal)
        {
            if (signal.Value < 10)
                return _pOutlineNone[signal.Value];
            if (signal.Value < 20)
                return _pOutlinePoor[signal.Value - 10];
            if (signal.Value < 30)
                return _pOutlineModerate[signal.Value - 20];
            if (signal.Value < 40)
                return _pOutlineGood[signal.Value - 30];
            if (signal.Value < 50)
                return _pOutlineExcellent[signal.Value - 40];
            return _pOutlineExcellent[9];
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the amount of rotation applied to the entire control to indicate the current direction of travel.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(Azimuth), "0")]
        [Description("Controls the amount of rotation applied to the entire control to indicate the current direction of travel.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Azimuth Bearing
        {
            get
            {
                return _bearing;
            }
            set
            {
                if (_bearing.Equals(value)) return;
                _bearing = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls whether the Satellites property is set manually, or automatically read from any available GPS device.
        /// </summary>
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
                return _isUsingRealTimeData;
            }
            set
            {
                if (_isUsingRealTimeData == value)
                    return;

                //MessageBox.Show("IsUsingRealTimeData started.");

                _isUsingRealTimeData = value;

#if !DesignTime
                if (_isUsingRealTimeData)
                {
                    // Use current satellite information
                    _satellites = Devices.Satellites;

                    // Also set the bearing
                    if (_rotationOrientation == RotationOrientation.TrackUp)
                        Rotation = new Angle(-Devices.Bearing.DecimalDegrees);
                }
                else
                {
                    _satellites.Clear();
                    // Clear the rotation
                    if (_rotationOrientation == RotationOrientation.TrackUp)
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

        /// <summary>
        /// Controls the number of degrees in between each smaller tick mark around the control.
        /// </summary>
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
                return _minorTickInterval;
            }
            set
            {
                if (_minorTickInterval.Equals(value)) return;
                _minorTickInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the format of compass directions drawn around the control.
        /// </summary>
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
                return _directionLabelFormat;
            }
            set
            {
                if (_directionLabelFormat == value) return;
                _directionLabelFormat = value;
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
        public Angle MajorTickInterval
        {
            get
            {
                return _majorTickInterval;
            }
            set
            {
                if (_majorTickInterval.Equals(value)) return;
                _majorTickInterval = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color used to draw smaller tick marks around the control.
        /// </summary>
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
                return _minorTickPen.Color;
            }
            set
            {
                lock (_minorTickPen)
                {
                    if (_minorTickPen.Color.Equals(value)) return;
                    _minorTickPen.Color = value;
                }
                Thread.Sleep(0);
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color used to draw larger tick marks around the control.
        /// </summary>
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
                return _majorTickPen.Color;
            }
            set
            {
                lock (_majorTickPen)
                {
                    if (_majorTickPen.Color.Equals(value)) return;
                    _majorTickPen.Color = value;
                }
                Thread.Sleep(0);
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the number of degrees in between each compass label around the control.
        /// </summary>
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
        /// Controls the color used to display compass direction letters around the control.
        /// </summary>
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
                return _directionLabelBrush.Color;
            }
            set
            {
                lock (_directionLabelBrush)
                {
                    if (_directionLabelBrush.Color.Equals(value)) return;
                    _directionLabelBrush.Color = value;
                }
                Thread.Sleep(0);
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the font used to draw compass labels around the control.
        /// </summary>
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
        /// Controls the color inside of satellite icons with no signal.
        /// </summary>
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
                return _satelliteNoSignalFillColor;
            }
            set
            {
                if (_satelliteNoSignalFillColor.Equals(value)) return;
                _satelliteNoSignalFillColor = value;
                _fillNone.EndColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color inside of satellite icons with a weak signal.
        /// </summary>
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
                return _satellitePoorSignalFillColor;
            }
            set
            {
                if (_satellitePoorSignalFillColor.Equals(value)) return;
                _satellitePoorSignalFillColor = value;
                _fillNone.EndColor = value;
                _fillPoor.StartColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color inside of satellite icons with a moderate signal.
        /// </summary>
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
                return _satelliteModerateSignalFillColor;
            }
            set
            {
                if (_satelliteModerateSignalFillColor.Equals(value)) return;
                _satelliteModerateSignalFillColor = value;
                _pFillModerate.StartColor = value;
                _fillPoor.EndColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color inside of satellite icons with a strong signal.
        /// </summary>
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
                return _satelliteGoodSignalFillColor;
            }
            set
            {
                if (_satelliteGoodSignalFillColor.Equals(value)) return;
                _satelliteGoodSignalFillColor = value;
                _pFillGood.StartColor = value;
                _pFillModerate.EndColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color inside of satellite icons with a very strong signal.
        /// </summary>
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
                return _satelliteExcellentSignalFillColor;
            }
            set
            {
                if (_satelliteExcellentSignalFillColor.Equals(value)) return;
                _satelliteExcellentSignalFillColor = value;
                _pFillGood.EndColor = value;
                _pFillExcellent.StartColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color around satellite icons with no signal.
        /// </summary>
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
                return _satelliteNoSignalOutlineColor;
            }
            set
            {
                if (_satelliteNoSignalOutlineColor.Equals(value)) return;
                _satelliteNoSignalOutlineColor = value;
                _pOutlineNone.EndColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color around satellite icons with a weak signal.
        /// </summary>
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
                return _satellitePoorSignalOutlineColor;
            }
            set
            {
                if (_satellitePoorSignalOutlineColor.Equals(value)) return;
                _satellitePoorSignalOutlineColor = value;
                _pOutlineNone.EndColor = value;
                _pOutlinePoor.StartColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color around satellite icons with a moderate signal.
        /// </summary>
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
                return _satelliteModerateSignalOutlineColor;
            }
            set
            {
                if (_satelliteModerateSignalOutlineColor.Equals(value)) return;
                _satelliteModerateSignalOutlineColor = value;
                _pOutlinePoor.EndColor = value;
                _pOutlineModerate.StartColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color around satellite icons with a strong signal.
        /// </summary>
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
                return _satelliteGoodSignalOutlineColor;
            }
            set
            {
                if (_satelliteGoodSignalOutlineColor.Equals(value)) return;
                _satelliteGoodSignalOutlineColor = value;
                _pOutlineModerate.EndColor = value;
                _pOutlineGood.StartColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color around satellite icons with a very strong signal.
        /// </summary>
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
                return _satelliteExcellentSignalOutlineColor;
            }
            set
            {
                if (_satelliteExcellentSignalOutlineColor.Equals(value)) return;
                _satelliteExcellentSignalOutlineColor = value;
                _pOutlineGood.EndColor = value;
                _pOutlineExcellent.StartColor = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color of the ellipse drawn around fixed satellites.
        /// </summary>
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
                return _satelliteFixColor;
#endif
            }
            set
            {
#if PocketPC
				if (_SatelliteFixBrush.Color.Equals(value))
					return;
				_SatelliteFixBrush.Color = value;
#else
                if (_satelliteFixColor.Equals(value)) return;
                _satelliteFixColor = value;
#endif
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls which bearing points straight up on the screen.
        /// </summary>
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
                return _rotationOrientation;
            }
            set
            {
                if (_rotationOrientation == value)
                    return;
                _rotationOrientation = value;

                // If this becomes active, set the current bearing
                Rotation = _rotationOrientation == RotationOrientation.TrackUp ? new Angle(-Devices.Bearing.DecimalDegrees) : Angle.Empty;

                //InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Contains the list of satellites drawn inside of the control.
        /// </summary>
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
                return _satellites;
            }
            set
            {
                _satellites = value;

                // Redraw the control
                if (_satellites != null)
                    InvokeRepaint();
            }
        }

#if Framework20 && !PocketPC

        /// <summary>
        /// The Origin Azimuth angle
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

            // Cache drawing intervals and such to prevent a race condition
            double minorInterval = _minorTickInterval.DecimalDegrees;
            double majorInterval = _majorTickInterval.DecimalDegrees;
            double directionInterval = _directionLabelInterval.DecimalDegrees;

            // Draw tick marks
            if (minorInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += minorInterval)
                {
                    // And draw a line
                    f.DrawLine(_minorTickPen, new PolarCoordinate(88.0f, angle),
                        new PolarCoordinate(90, angle));
                }
            }

            // Draw tick marks
            if (majorInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += majorInterval)
                {
                    // And draw a line
                    f.DrawLine(_majorTickPen, new PolarCoordinate(85.0f, angle), new PolarCoordinate(90, angle));
                }
            }

            if (directionInterval > 0)
            {
                for (double angle = 0; angle < 360; angle += directionInterval)
                {
                    // Get the coordinate of the line's start
                    PolarCoordinate start = new PolarCoordinate(70, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
#if PocketPC
					f.DrawCenteredString(((Azimuth)angle).ToString(_DirectionLabelFormat, CultureInfo.CurrentCulture), _DirectionLabelFont, _DirectionLabelBrush, start);
#else
                    f.DrawRotatedString(((Azimuth)angle).ToString(_directionLabelFormat, CultureInfo.CurrentCulture), _directionLabelFont, _directionLabelBrush, start);
#endif
                }
            }

            // Draw an ellipse at the center
            f.DrawEllipse(_halfwayUpPen, PolarCoordinate.Empty, 45);

            // Now draw each satellite
            int satelliteCount = 0;

            if (Satellites != null)
            {
                satelliteCount = Satellites.Count;
#if !PocketPC

                for (int index = 0; index < satelliteCount; index++)
                {
                    Satellite satellite = _satellites[index];

                    // Don't draw if the satellite is stale
                    if (!satellite.IsActive && !DesignMode)
                        continue;

                    // Is the satellite transparent?
                    if (GetFillColor(satellite.SignalToNoiseRatio).A < _satelliteShadowBrush.Color.A)
                        continue;

                    // Get the coordinate for this satellite
                    PolarCoordinate center = new PolarCoordinate(Convert.ToSingle(90.0f - satellite.Elevation.DecimalDegrees),
                        satellite.Azimuth.DecimalDegrees, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    PointD centerPoint = f.ToPointD(center);

                    // Each icon is 30x30, so we'll translate it by half the distance
                    double pShadowSize = Math.Sin(Radian.FromDegrees(satellite.Elevation.DecimalDegrees).Value) * 7;

                    f.Graphics.TranslateTransform((float)(centerPoint.X - _iconCenter.X + pShadowSize), (float)(centerPoint.Y - _iconCenter.Y + pShadowSize));

                    // Draw each satellite
                    PointF[] satelliteIcon = new PointF[_icon.Length];
                    for (int iconIndex = 0; iconIndex < _icon.Length; iconIndex++)
                    {
                        satelliteIcon[iconIndex] = new PointF((float)_icon[iconIndex].X, (float)_icon[iconIndex].Y);
                    }

                    using (Matrix y = new Matrix())
                    {
                        y.RotateAt(Convert.ToSingle(satellite.Azimuth.DecimalDegrees - f.Rotation.DecimalDegrees + Origin.DecimalDegrees),
                            new PointF((float)_iconCenter.X, (float)_iconCenter.Y), MatrixOrder.Append);
                        y.TransformPoints(satelliteIcon);
                    }

                    f.Graphics.FillPolygon(_satelliteShadowBrush, satelliteIcon);
                    f.Graphics.ResetTransform();
                }
#endif
            }

            // Now draw each satellite
            if (_satellites != null)
            {
                List<Satellite> fixedSatellites = Satellite.GetFixedSatellites(_satellites);
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
                    PolarCoordinate center = new PolarCoordinate(Convert.ToSingle(90.0 - satellite.Elevation.DecimalDegrees),
                        satellite.Azimuth.DecimalDegrees, Azimuth.North, PolarCoordinateOrientation.Clockwise);

#if PocketPC
						f.FillEllipse(_SatelliteFixBrush, Center, 16);
#else
                    using (SolidBrush fixBrush = new SolidBrush(Color.FromArgb(Math.Min(255, fixedSatellites.Count * 20), _satelliteFixColor)))
                    {
                        f.FillEllipse(fixBrush, center, 16);
                    }
#endif
                }

                // Now draw each satellite
                for (int index = 0; index < satelliteCount; index++)
                {
                    Satellite satellite = _satellites[index];
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
                    PolarCoordinate center = new PolarCoordinate(Convert.ToSingle(90.0 - satellite.Elevation.DecimalDegrees),
                        satellite.Azimuth.DecimalDegrees, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    PointD centerPoint = f.ToPointD(center);

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

                    f.Graphics.TranslateTransform((float)(centerPoint.X - _iconCenter.X - pShadowSize * 0.1), (float)(centerPoint.Y - _iconCenter.Y - pShadowSize * 0.1));

                    // Draw each satellite
                    PointF[] satelliteIcon = new PointF[_icon.Length];
                    for (int iconIndex = 0; iconIndex < _icon.Length; iconIndex++)
                    {
                        satelliteIcon[iconIndex] = new PointF((float)_icon[iconIndex].X, (float)_icon[iconIndex].Y);
                    }

                    Matrix y = new Matrix();
                    y.RotateAt(Convert.ToSingle(satellite.Azimuth.DecimalDegrees - f.Rotation.DecimalDegrees + Origin.DecimalDegrees),
                        new PointF((float)_iconCenter.X, (float)_iconCenter.Y), MatrixOrder.Append);
                    y.TransformPoints(satelliteIcon);
                    y.Dispose();

                    SolidBrush fillBrush = new SolidBrush(GetFillColor(satellite.SignalToNoiseRatio));
                    f.Graphics.FillPolygon(fillBrush, satelliteIcon);
                    fillBrush.Dispose();

                    Pen fillPen = new Pen(GetOutlineColor(satellite.SignalToNoiseRatio), 1.0f);
                    f.Graphics.DrawPolygon(fillPen, satelliteIcon);
                    fillPen.Dispose();

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

                    f.DrawRotatedString(satellite.PseudorandomNumber.ToString(CultureInfo.CurrentCulture), _pseudorandomNumberFont,
                        _pseudorandomNumberBrush, new PolarCoordinate(center.R - 11, center.Theta,
                        Azimuth.North, PolarCoordinateOrientation.Clockwise));
#endif
                }
            }
        }

#if !PocketPC

        /// <summary>
        /// Controls the color of the shadow cast by satellite icons.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "32, 0, 0, 0")]
        [Description("Controls the color of the shadow cast by satellite icons.")]
        public Color ShadowColor
        {
            get
            {
                return _satelliteShadowBrush.Color;
            }
            set
            {
                lock (_satelliteShadowBrush)
                {
                    if (_satelliteShadowBrush.Color.Equals(value)) return;
                    _satelliteShadowBrush.Color = value;
                }
                InvokeRepaint();
            }
        }

#endif

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the font used to display the ID of each satellite.
        /// </summary>
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
                return _pseudorandomNumberFont;
            }
            set
            {
                if (_pseudorandomNumberFont.Equals(value)) return;
                _pseudorandomNumberFont = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color used to display the ID of each satellite.
        /// </summary>
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
                return _pseudorandomNumberBrush.Color;
            }
            set
            {
                lock (_pseudorandomNumberBrush)
                {
                    if (_pseudorandomNumberBrush.Color.Equals(value)) return;
                    _pseudorandomNumberBrush.Color = value;
                }
                Thread.Sleep(0);
                InvokeRepaint();
            }
        }

        private void Devices_CurrentSatellitesChanged(object sender, SatelliteListEventArgs e)
        {
            //TODO should this be done here or in a user defined event handler?
            if (_isUsingRealTimeData) Satellites = (List<Satellite>)e.Satellites;
            InvokeRepaint();
        }

        private void Devices_CurrentBearingChanged(object sender, AzimuthEventArgs e)
        {
            if (_isUsingRealTimeData && _rotationOrientation == RotationOrientation.TrackUp)
                Rotation = new Angle(e.Azimuth.DecimalDegrees);
        }
    }
}