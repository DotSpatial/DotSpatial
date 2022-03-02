using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
#if !PocketPC || DesignTime || Framework20
using System.ComponentModel;
#endif
using DotSpatial.Positioning.Drawing;
using DotSpatial.Positioning.Gps.IO;

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Gps.Controls
{
	/// <summary>Represents a user control used to display altitude.</summary>
	/// <remarks>
	/// Altimeters are used to visually represent some form of altitude, typically the
	/// user's current altitude above sea level. Altitude is measured using three needles which
	/// represent (from longest to shortest) hundreds, thousands and tens-of-thousands. The
	/// display of the Altimeter is controlled via the <strong>Value</strong> property.
	/// </remarks>
#if !PocketPC || DesignTime
	[ToolboxBitmap(typeof(Altimeter))]
	[DefaultProperty("Value")]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
#endif
    public sealed class Altimeter : PolarControl
    {
#if PocketPC
        private Font _AltitudeLabelFont = new Font("Tahoma", 8.0f, FontStyle.Bold);
#if Framework20
		private Pen _MajorTickPen = new Pen(Color.Black, 2.0f);
#else
		private Pen _MajorTickPen = new Pen(Color.Black);
#endif
#else
        private Font _AltitudeLabelFont = new Font("Tahoma", 12.0f, FontStyle.Regular);
		private Thread _InterpolationThread;
		private ManualResetEvent _AnimationWaitHandle = new ManualResetEvent(false);
		private Interpolator _ValueInterpolator = new Interpolator(15, InterpolationMethod.CubicEaseInOut);
        private int _InterpolationIndex;
		private bool _IsInterpolationActive;
        private SolidBrush _NeedleShadowBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        private Size _NeedleShadowSize = new Size(5, 5);
		private Pen _MajorTickPen = new Pen(Color.Black, 2.0f);
#endif
		private Pen _CenterPen = new Pen(Color.Gray);
		private Pen _MinorTickPen = new Pen(Color.Black);
		private Pen _TensOfThousandsPen = new Pen(Color.Black);
		private Pen _ThousandsPen = new Pen(Color.Black);
		private Pen _HundredsPen = new Pen(Color.Black);
		private SolidBrush _AltitudeLabelBrush = new SolidBrush(Color.Black);
		private Distance _Altitude = Distance.Empty;
        private Font _ValueFont = new Font("Tahoma", 9.0f, FontStyle.Regular);
        private SolidBrush _TensOfThousandsBrush = new SolidBrush(Color.Red);
		private SolidBrush _ThousandsBrush = new SolidBrush(Color.Red);
		private SolidBrush _HundredsBrush = new SolidBrush(Color.Red);
		private SolidBrush _ValueBrush = new SolidBrush(Color.Black);
		private string _ValueFormat = "v uu";
        private bool _IsUsingRealTimeData = false;


        #region Needles

        private static PolarCoordinate[] _TensOfThousandsNeedle = new PolarCoordinate[]
			{
				new PolarCoordinate(6, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(25, 358, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(30, 0, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(25, 2, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(6, 165, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(6, 195, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(6, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise)
			};
        private static PolarCoordinate[] _HundredsNeedle = new PolarCoordinate[]
			{
				new PolarCoordinate(10, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(85, 358, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(90, 0, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(85, 2, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(10, 165, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(10, 195, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(10, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise)
			};
        private static PolarCoordinate[] _ThousandsNeedle = new PolarCoordinate[]
			{
				new PolarCoordinate(8, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(40, 350, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(65, 0, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(40, 10, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, 165, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, 195, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(8, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise)
			};

        #endregion

        #region Constants

        private const double ConversionFactor = 3.6;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Value property has changed.
        /// </summary>
        public event EventHandler<DistanceEventArgs> ValueChanged;

        #endregion

        #region Constructors

        /// <summary>Creates a new instance.</summary>
		public Altimeter()
			: base("DotSpatial.Positioning Multithreaded Altimeter Control (http://dotspatial.codeplex.com)")
		{
#if !PocketPC
			// Start the interpolation thread
			_InterpolationThread = new Thread(new ThreadStart(InterpolationLoop));
			_InterpolationThread.IsBackground = true;
			_InterpolationThread.Name = "DotSpatial.Positioning Altimeter Needle Animation Thread (http://dotspatial.codeplex.com)";
			_IsInterpolationActive = true;
			_InterpolationThread.Start();

#endif
			Origin = Azimuth.North;
			Orientation = PolarCoordinateOrientation.Clockwise;

//#if PocketPC && !Framework20 && !DesignTime
//            DotSpatial.Positioning.Gps.IO.Devices.AltitudeChanged += new EventHandler<DistanceEventArgs>(Devices_CurrentAltitudeChanged);
//#endif
        }

        #endregion


        protected override void Dispose(bool disposing)
        {
            if (_IsUsingRealTimeData)
            {
                try
                {
                    // Only work with events if we're not in design mode
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        // Disconnect from the altitude changed event
                        Devices.AltitudeChanged -= new EventHandler<DistanceEventArgs>(Devices_CurrentAltitudeChanged);
                    }
                }
                catch
                {
                }
            }
            
#if !PocketPC
            // Get the interpolation thread out of a loop 
            _IsInterpolationActive = false;

            if (_InterpolationThread != null)
            {

                if (_AnimationWaitHandle != null)
                {
                    try { _AnimationWaitHandle.Set(); }
                    catch { }
                }

                try { _InterpolationThread.Abort(); }
                catch { }

                _InterpolationThread = null;
            }

			if (_AnimationWaitHandle != null)
			{
				try { _AnimationWaitHandle.Close(); }
				catch {}
				finally { _AnimationWaitHandle = null; }
			}
#endif

            if (_CenterPen != null) 
            {
                _CenterPen.Dispose();
                _CenterPen = null;
            }
            if (_MinorTickPen != null)
            {
                _MinorTickPen.Dispose();
                _MinorTickPen = null;
            }
            if (_AltitudeLabelBrush != null)
            {
                _AltitudeLabelBrush.Dispose();
                _AltitudeLabelBrush = null;
            }
            if (_MajorTickPen != null)
            {
                _MajorTickPen.Dispose();
                _MajorTickPen = null;
            }

            if (_AltitudeLabelFont != null)
            {
                _AltitudeLabelFont.Dispose();
                _AltitudeLabelFont = null;
            }
#if !PocketPC

            if (_NeedleShadowBrush != null)
            {
                _NeedleShadowBrush.Dispose();
                _NeedleShadowBrush = null;
            }
#endif
            if (_TensOfThousandsBrush != null)
            {
                _TensOfThousandsBrush.Dispose();
                _TensOfThousandsBrush = null;
            }
            if (_ThousandsBrush != null)
            {
                _ThousandsBrush.Dispose();
                _ThousandsBrush = null;
            }
            if (_HundredsBrush != null)
            {
                _HundredsBrush.Dispose();
                _HundredsBrush = null;
            }
            if (_TensOfThousandsPen != null)
            {
                _TensOfThousandsPen.Dispose();
                _TensOfThousandsPen = null;
            }
            if (_ThousandsPen != null)
            {
                _ThousandsPen.Dispose();
                _ThousandsPen = null;
            }
            if (_HundredsPen != null)
            {
                _HundredsPen.Dispose();
                _HundredsPen = null;
            }
            if (_ValueFont != null)
            {
                _ValueFont.Dispose();
                _ValueFont = null;
            }
            if (_ValueBrush != null)
            {
                _ValueBrush.Dispose();
                _ValueBrush = null;
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

      

		/// <summary>
		/// Indicates if the control should automatically display the current
		/// altitude.
		/// </summary>
		/// <value>
		/// A <strong>Boolean</strong>, <strong>True</strong> if the control automatically
		/// displays the current altitude.
		/// </value>
		/// <remarks>
		/// When this property is enabled, the control will examine the
		/// <strong>CurrentAltitude</strong> property of the
		/// <strong>DotSpatial.Positioning.Gps.Devices</strong> class and update itself when the property
		/// changes. When disabled, the <strong>Value</strong> property must be set manually to
		/// change the control.
		/// </remarks>
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
				return _IsUsingRealTimeData;
			}
			set
			{
				// Has anything changed?  If not, exit
				if(_IsUsingRealTimeData == value) 
                    return;

                // Set the new value
				_IsUsingRealTimeData = value;

				// Set the control to the last known altitude (if any)
                if (_IsUsingRealTimeData)
                {
                    try
                    {
                        // Hook into the AltitudeChanged event\
                        // When in design mode, do not hook into events
                        if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                        {
                            // Hook into the altitude changed event
                            Devices.AltitudeChanged += new EventHandler<DistanceEventArgs>(Devices_CurrentAltitudeChanged);
                        }

                        // Set the value of the control
#if !PocketPC
						if(DesignMode)
							Value = Distance.Empty;
						else
#endif
                            Value = Devices.Altitude;

                    }
                    catch
                    {
                        Value = Distance.Empty;
                    }
                }
                else
                {
                    // When in design mode, do not hook into events
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        // Hook into the altitude changed event
                        Devices.AltitudeChanged -= new EventHandler<DistanceEventArgs>(Devices_CurrentAltitudeChanged);
                    }

                    // Set teh control to no value
                    Value = Distance.Empty;
                }

                // Tell the control to repaint
				InvokeRepaint();
			}
		}


		/// <summary>Controls the font used for displaying altitude text.</summary>
		/// <remarks>
		/// This property controls the font used to display altitude text on the control. To
		/// control the font of numbers drawn around the edge of the control, see the
		/// <strong>AltitudeLabelFont</strong> property.
		/// </remarks>
		/// <seealso cref="AltitudeLabelFont">AltitudeLabelFont Property</seealso>
#if !PocketPC || DesignTime
		[Category("Altitude Label")]
		[DefaultValue(typeof(Font), "Tahoma, 9pt")]
		[Description("Controls the font of the label displaying the current altitude.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Font ValueFont
		{
			get
			{
				return _ValueFont;
			}
			set
			{
				if(_ValueFont.Equals(value)) return;
				_ValueFont = value;
				InvokeRepaint();
			}
		}

		/// <summary>Controls the color of altitude text.</summary>
		/// <remarks>
		/// This property controls the color of the altitude text on the control. To change
		/// the color of numbers drawn around the edge of the control, see the
		/// <strong>AltitudeLabelColor</strong> property.
		/// </remarks>
		/// <seealso cref="AltitudeLabelColor">AltitudeLabelColor Property</seealso>
#if !PocketPC || DesignTime
		[Category("Altitude Label")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color of the label displaying the current altitude.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color ValueColor 
		{
			get
			{
				return _ValueBrush.Color;
			}
			set
			{
				if(_ValueBrush.Color.Equals(value)) return;
				_ValueBrush.Color = value;
				InvokeRepaint();
			}
		}

        /// <summary>
        /// Controls how the control smoothly transitions from one value to another.
        /// </summary>
#if !PocketPC
		[Category("Behavior")]
		[DefaultValue(typeof(InterpolationMethod), "CubicEaseInOut")]
		[Description("Controls how the control smoothly transitions from one value to another.")]
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

		/// <summary>Controls the format of altitude values.</summary>
		/// <value>
		/// A <strong>String</strong> which is compatible with the <strong>ToString</strong>
		/// method of the <strong>Distance</strong> class.
		/// </value>
		/// <remarks>
		/// This property controls how text is output on the control. By default, the format
		/// is "<strong>v uu</strong>" where <strong>v</strong> represents the numeric portion of
		/// the altitude, and <strong>uu</strong> represents units.
		/// </remarks>
#if !PocketPC || DesignTime
		[Category("Altitude Label")]
		[DefaultValue(typeof(string), "v uu")]
		[Description("Controls the format of the label displaying the current altitude.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public string ValueFormat
		{
			get
			{
				return _ValueFormat;
			}
			set
			{
				if(_ValueFormat.Equals(value)) return;
				_ValueFormat = value;
				InvokeRepaint();
			}
		}

		/// <summary>Controls the altitude to display on the control.</summary>
		/// <value>A <strong>Distance</strong> structure measuring the altitude to display.</value>
		/// <remarks>
		/// Changing this property causes the needles on the control to move so that they
		/// represent the value.
		/// </remarks>
#if !PocketPC || DesignTime
		[Category("Appearance")]
		[DefaultValue(typeof(Distance), "0 m")]
		[Description("Controls the amount of distance above sea level being displayed in the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		////[CLSCompliant(false)]
		public Distance Value
		{
			get
			{
				return _Altitude;
			}
			set
			{
				if(_Altitude.Equals(value)) 
                    return;
				_Altitude = value;
				// Force either feet or meters
				if(_Altitude.IsMetric)
					_Altitude = _Altitude.ToMeters();
				else
					_Altitude = _Altitude.ToFeet();
#if PocketPC
                InvokeRepaint();
#else
				if(IsDisposed)
					return;

				lock(_ValueInterpolator)
				{
					// Are we changing direction?
					if(_Altitude.Value >= _ValueInterpolator.Minimum
						&& _Altitude.Value > _ValueInterpolator[_InterpolationIndex])
					{
						// No.  Just set the new maximum
						_ValueInterpolator.Maximum = _Altitude.Value;
					}					
					else if(_Altitude.Value < _ValueInterpolator.Minimum)
					{
						// We're changing directions, so stop then accellerate again
						_ValueInterpolator.Minimum = _ValueInterpolator[_InterpolationIndex];
						_ValueInterpolator.Maximum = _Altitude.Value;
						_InterpolationIndex = 0;
					}
					else if(_Altitude.Value > _ValueInterpolator.Minimum
						&& _Altitude.Value < _ValueInterpolator[_InterpolationIndex])
					{
						// We're changing directions, so stop then accellerate again
						_ValueInterpolator.Minimum = _ValueInterpolator[_InterpolationIndex];
						_ValueInterpolator.Maximum = _Altitude.Value;
						_InterpolationIndex = 0;
					}
					else if(_Altitude.Value > _ValueInterpolator.Maximum)
					{
						// No.  Just set the new maximum
						_ValueInterpolator.Maximum = _Altitude.Value;
					}
				}
				// And activate the interpolation thread
//				if((InterpolationThread.ThreadState & ThreadState.Suspended) != 0)
//					InterpolationThread.Resume();
				_AnimationWaitHandle.Set();
#endif

                // Signal that the value has changed
                OnValueChanged(new DistanceEventArgs(_Altitude));
			}
		}

        private void OnValueChanged(DistanceEventArgs e)
        {
            if (ValueChanged != null)
				ValueChanged(this, e);
        }

		/// <summary>Controls the color of numbers around the edge of the control.</summary>
		/// <remarks>
		/// This property controls the color of numbers around the edge of the control. To
		/// control the color of altitude text, see the <strong>ValueColor</strong>
		/// property.
		/// </remarks>
		/// <seealso cref="ValueColor">ValueColor Property</seealso>
#if !PocketPC || DesignTime
		[Category("Altitude Label")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color of altitude digits drawn around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color AltitudeLabelColor 
		{
			get
			{
				return _AltitudeLabelBrush.Color;
			}
			set
			{
				if(_AltitudeLabelBrush.Color.Equals(value)) return;
				_AltitudeLabelBrush.Color = value;
				InvokeRepaint();
			}
		}

		/// <summary>Controls the font of numbers drawn around the edge of the control.</summary>
		/// <remarks>
		/// This property controls the font used to draw numbers around the edge of the
		/// control. To control the font used to draw altitude text, see the
		/// <strong>ValueFont</strong> property.
		/// </remarks>
		/// <seealso cref="ValueFont">ValueFont Property</seealso>
#if !PocketPC || DesignTime
		[Category("Altitude Label")]
#if PocketPC
		[DefaultValue(typeof(Font), "Tahoma, 8pt, style=Bold")]		
#else
		[DefaultValue(typeof(Font), "Tahoma, 12pt")]
#endif
		[Description("Controls the font of altitude digits drawn around the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Font AltitudeLabelFont 
		{
			get
			{
				return _AltitudeLabelFont;
			}
			set
			{
				if(_AltitudeLabelFont.Equals(value)) return;
				_AltitudeLabelFont = value;
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
				return _MinorTickPen.Color;
			}
			set
			{
				_MinorTickPen.Color = value;
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
				return _MajorTickPen.Color;
			}
			set
			{
				_MajorTickPen.Color = value;
				InvokeRepaint();
			}
		}
		/// <summary>Controls the interior color of the tens-of-thousands needle.</summary>
		/// <remarks>
		/// The tens-of-thousands needle is the smallest needle of the control. The interior
		/// can be made invisible by setting this property to <strong>Transparent</strong>.
		/// </remarks>
#if !PocketPC || DesignTime
		[Category("Needle Colors")]
		[DefaultValue(typeof(Color), "Red")]
		[Description("Controls the color of the interior of the tens-of-thousands needle.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
		public Color TensOfThousandsNeedleFillColor
		{
			get
			{
				return _TensOfThousandsBrush.Color;
			}
			set
			{
				if(_TensOfThousandsBrush.Color.Equals(value)) return;
				_TensOfThousandsBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Needle Colors")]
		[DefaultValue(typeof(Color), "Red")]
		[Description("Controls the color of the interior of the thousands needle.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color ThousandsNeedleFillColor
		{
			get
			{
				return _ThousandsBrush.Color;
			}
			set
			{
				if(_ThousandsBrush.Color.Equals(value)) return;
				_ThousandsBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Needle Colors")]
		[DefaultValue(typeof(Color), "Red")]
		[Description("Controls the color of the interior of the hundreds needle.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color HundredsNeedleFillColor
		{
			get
			{
				return _HundredsBrush.Color;
			}
			set
			{
				if(_HundredsBrush.Color.Equals(value)) return;
				_HundredsBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Needle Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color of the edge of the tens-of-thousands needle.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color TensOfThousandsNeedleOutlineColor
		{
			get
			{
				return _TensOfThousandsPen.Color;
			}
			set
			{
				if(_TensOfThousandsPen.Color.Equals(value)) return;
				_TensOfThousandsPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
		[Category("Needle Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color of the edge of the thousands needle.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color ThousandsNeedleOutlineColor
		{
			get
			{
				return _ThousandsPen.Color;
			}
			set
			{
				if(_ThousandsPen.Color.Equals(value)) return;
				_ThousandsPen.Color = value;
				InvokeRepaint();
			}
		}

		
#if !PocketPC || DesignTime
		[Category("Needle Colors")]
		[DefaultValue(typeof(Color), "Black")]
		[Description("Controls the color of the edge of the hundreds needle.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color HundredsNeedleOutlineColor
		{
			get
			{
				return _HundredsPen.Color;
			}
			set
			{
				if(_HundredsPen.Color.Equals(value)) return;
				_HundredsPen.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "128, 0, 0, 0")]
		[Description("Controls the color of the shadow cast by the altitude needles.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
        public Color NeedleShadowColor
		{
			get
			{
				return _NeedleShadowBrush.Color;
			}
			set
			{
				if(_NeedleShadowBrush.Color.Equals(value)) return;
				_NeedleShadowBrush.Color = value;
				InvokeRepaint();
			}
		}
#endif

#if !PocketPC || Framework20
        ///// <summary>
        ///// Occurs when the GDI handle for the control is initialized.
        ///// </summary>
        //protected override void OnHandleCreated(EventArgs e)
        //{
        //    // Subscribe to events
        //    try
        //    {
        //        base.OnHandleCreated(e);
        //    }
        //    catch
        //    {
        //    }
        //}

        ///// <summary>
        ///// Occurs when the GDI handle for the control is destroyed.
        ///// </summary>
        //protected override void OnHandleDestroyed(EventArgs e)
        //{
        //    try
        //    {
        //        // When in design mode, do not hook into events
        //        if (License.Context.UsageMode == LicenseUsageMode.Runtime)
        //        {
        //            // Hook into the altitude changed event
        //            DotSpatial.Positioning.Gps.IO.Devices.AltitudeChanged -= new EventHandler<DistanceEventArgs>(Devices_CurrentAltitudeChanged);
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

#if !PocketPC
		[Category("Appearance")]
		[DefaultValue(typeof(Size), "5, 5")]
		[Description("Controls the size of the shadow cast by the altitude needles.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
        public Size NeedleShadowSize
		{
			get
			{
				return _NeedleShadowSize;
			}
			set
			{
				_NeedleShadowSize = value;
				InvokeRepaint();
			}
		}
#endif
        protected override void OnPaintOffScreen(PaintEventArgs e)
		{
            PolarGraphics f = base.CreatePolarGraphics(e.Graphics);
			
			// What altitude are we drawing?
#if PocketPC
            Distance AltitudeToRender = _Altitude;
#else
			Distance AltitudeToRender = new Distance(_ValueInterpolator[_InterpolationIndex], _Altitude.Units);
#endif

				// There are 100 tick marks in 360 degrees.   3.6° per tick mark
				double ConversionFactor = 3.6;
				// Draw tick marks
				if(_MinorTickPen != null)
				{
					for(double alt = 0; alt < 100; alt += 1)
					{
						// Convert the speed to an angle
						Angle angle = new Angle(alt * ConversionFactor);
						// Get the coordinate of the line's start
						PolarCoordinate start = new PolarCoordinate(95, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
						PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
						// And draw a line
						f.DrawLine(_MinorTickPen, start, end);
					}
				}
				// Draw tick marks
				if(_MajorTickPen != null)
				{
					for(double alt = 0; alt < 100; alt += 10)
					{
						// Convert the speed to an angle
						Angle angle = new Angle(alt * ConversionFactor);
						// Get the coordinate of the line's start
						PolarCoordinate start = new PolarCoordinate(94, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
						PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
						// And draw a line
						f.DrawLine(_MajorTickPen, start, end);
						// And also a string
						string s = Convert.ToString(alt * 0.1, CultureInfo.CurrentCulture);
						f.DrawCenteredString(s, _AltitudeLabelFont, _AltitudeLabelBrush,
							new PolarCoordinate(85, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise));
					}
				}
				// Calculate all needle values
				double pTensOfThousandsValue = AltitudeToRender.Value / 1000.0;
				double pThousandsValue = AltitudeToRender.Value / 100.0;
				double pValue = AltitudeToRender.Value / 10.0;			

				// Now draw the tens-of-thousands needle
				// Rotate the needle to the right place
				PolarCoordinate[] Needle1 = new PolarCoordinate[_TensOfThousandsNeedle.Length];
				for(int index = 0; index < Needle1.Length; index++)
					Needle1[index] = _TensOfThousandsNeedle[index].Rotate(pTensOfThousandsValue * ConversionFactor);

				// Now draw the tens-of-thousands needle
				// Rotate the needle to the right place
				PolarCoordinate[] Needle2 = new PolarCoordinate[_ThousandsNeedle.Length];
				for(int index = 0; index < Needle2.Length; index++)
				{
					Needle2[index] = _ThousandsNeedle[index].Rotate(pThousandsValue * ConversionFactor);
				}

				// Now draw the tens-of-Hundreds needle
				// Rotate the needle to the right place
				PolarCoordinate[] Needle3 = new PolarCoordinate[_HundredsNeedle.Length];
				for(int index = 0; index < Needle3.Length; index++)
				{
					Needle3[index] = _HundredsNeedle[index].Rotate(pValue * ConversionFactor);
				}

				string AltitudeString = AltitudeToRender.ToString(_ValueFormat, CultureInfo.CurrentCulture);
				//SizeF FontSize = f.Graphics.MeasureString(AltitudeString, pValueFont);	
		
#if PocketPC
            f.DrawCenteredString(AltitudeString, _ValueFont,
                _ValueBrush, new PolarCoordinate(45.0f, 0.0, Azimuth.North, PolarCoordinateOrientation.Clockwise));
#else
				f.DrawRotatedString(AltitudeString, _ValueFont,
					_ValueBrush, new PolarCoordinate(45.0f, Angle.Empty, Azimuth.North, PolarCoordinateOrientation.Clockwise));
#endif
				// Draw an ellipse at the center
				f.DrawEllipse(_CenterPen, PolarCoordinate.Empty, 10);

#if !PocketPC
				f.Graphics.TranslateTransform(_NeedleShadowSize.Width, _NeedleShadowSize.Height, MatrixOrder.Append);

				f.FillPolygon(_NeedleShadowBrush, Needle1);
				f.FillPolygon(_NeedleShadowBrush, Needle2);
				f.FillPolygon(_NeedleShadowBrush, Needle3);

				f.Graphics.ResetTransform();
#endif

				f.FillPolygon(_TensOfThousandsBrush, Needle1);
				f.DrawPolygon(_TensOfThousandsPen, Needle1);
				f.FillPolygon(_ThousandsBrush, Needle2);
				f.DrawPolygon(_ThousandsPen, Needle2);
				f.FillPolygon(_HundredsBrush, Needle3);
				f.DrawPolygon(_HundredsPen, Needle3);

				// Draw an ellipse at the center
				f.FillEllipse(_HundredsBrush, PolarCoordinate.Empty, 7);
				f.DrawEllipse(_HundredsPen, PolarCoordinate.Empty, 7);
			
		}

		private void Devices_CurrentAltitudeChanged(object sender, DistanceEventArgs e)
		{
			if(_IsUsingRealTimeData)
				Value = e.Distance;
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
//            try
//            {
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                base.OnHandleCreated(e);
//            }
//        }

//        protected override void OnHandleDestroyed(EventArgs e)
//        {
//            try
//            {
//#if !PocketPC
//                // Get the interpolation thread out of a loop 
//                IsInterpolationActive = false;
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
//#endif
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                base.OnHandleDestroyed(e);
//            }
//        }

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
