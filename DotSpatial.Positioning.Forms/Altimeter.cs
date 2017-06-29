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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace DotSpatial.Positioning.Forms
{
    /// <summary>Represents a user control used to display altitude.</summary>
    /// <remarks>
    /// Altimeters are used to visually represent some form of altitude, typically the
    /// user's current altitude above sea level. Altitude is measured using three needles which
    /// represent (from longest to shortest) hundreds, thousands and tens-of-thousands. The
    /// display of the Altimeter is controlled via the <strong>Value</strong> property.
    /// </remarks>
    [ToolboxBitmap(typeof(Altimeter))]
    [DefaultProperty("Value")]
    [ToolboxItem(true)]
    public sealed class Altimeter : PolarControl
    {
        private Font _altitudeLabelFont = new Font("Tahoma", 12.0f, FontStyle.Regular);
        private Thread _interpolationThread;
        private ManualResetEvent _animationWaitHandle = new ManualResetEvent(false);
        private readonly Interpolator _valueInterpolator = new Interpolator(15, InterpolationMethod.CubicEaseInOut);
        private int _interpolationIndex;
        private bool _isInterpolationActive;
        private SolidBrush _needleShadowBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        private Size _needleShadowSize = new Size(5, 5);
        private Pen _majorTickPen = new Pen(Color.Black, 2.0f);
        private Pen _centerPen = new Pen(Color.Gray);
        private Pen _minorTickPen = new Pen(Color.Black);
        private Pen _tensOfThousandsPen = new Pen(Color.Black);
        private Pen _thousandsPen = new Pen(Color.Black);
        private Pen _hundredsPen = new Pen(Color.Black);
        private SolidBrush _altitudeLabelBrush = new SolidBrush(Color.Black);
        private Distance _altitude = Distance.Empty;
        private Font _valueFont = new Font("Tahoma", 9.0f, FontStyle.Regular);
        private SolidBrush _tensOfThousandsBrush = new SolidBrush(Color.Red);
        private SolidBrush _thousandsBrush = new SolidBrush(Color.Red);
        private SolidBrush _hundredsBrush = new SolidBrush(Color.Red);
        private SolidBrush _valueBrush = new SolidBrush(Color.Black);
        private string _valueFormat = "v uu";
        private bool _isUsingRealTimeData;

        #region Needles

        private static readonly PolarCoordinate[] _tensOfThousandsNeedle = new[]
			{
				new PolarCoordinate(6, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(25, 358, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(30, 0, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(25, 2, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(6, 165, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(6, 195, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(6, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise)
			};
        private static readonly PolarCoordinate[] _hundredsNeedle = new[]
			{
				new PolarCoordinate(10, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(85, 358, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(90, 0, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(85, 2, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(10, 165, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(10, 195, Azimuth.North, PolarCoordinateOrientation.Clockwise),
				new PolarCoordinate(10, 345, Azimuth.North, PolarCoordinateOrientation.Clockwise)
			};
        private static readonly PolarCoordinate[] _thousandsNeedle = new[]
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
            // Start the interpolation thread
            _interpolationThread = new Thread(InterpolationLoop)
                                       {
                                           IsBackground = true,
                                           Name =
                                               "DotSpatial.Positioning Altimeter Needle Animation Thread (http://dotspatial.codeplex.com)"
                                       };
            _isInterpolationActive = true;
            _interpolationThread.Start();

            Origin = Azimuth.North;
            Orientation = PolarCoordinateOrientation.Clockwise;
        }

        #endregion

        /// <inheritdocs/>
        protected override void Dispose(bool disposing)
        {
            if (_isUsingRealTimeData)
            {
                try
                {
                    // Only work with events if we're not in design mode
                    if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    {
                        // Disconnect from the altitude changed event
                        Devices.AltitudeChanged -= Devices_CurrentAltitudeChanged;
                    }
                }
                catch
                {
                }
            }

            // Get the interpolation thread out of a loop
            _isInterpolationActive = false;

            if (_interpolationThread != null)
            {
                if (_animationWaitHandle != null)
                {
                    try { _animationWaitHandle.Set(); }
                    catch { }
                }

                try { _interpolationThread.Abort(); }
                finally { _interpolationThread = null; }
            }

            if (_animationWaitHandle != null)
            {
                try { _animationWaitHandle.Close(); }
                finally { _animationWaitHandle = null; }
            }

            if (_centerPen != null)
            {
                _centerPen.Dispose();
                _centerPen = null;
            }
            if (_minorTickPen != null)
            {
                _minorTickPen.Dispose();
                _minorTickPen = null;
            }
            if (_altitudeLabelBrush != null)
            {
                _altitudeLabelBrush.Dispose();
                _altitudeLabelBrush = null;
            }
            if (_majorTickPen != null)
            {
                _majorTickPen.Dispose();
                _majorTickPen = null;
            }

            if (_altitudeLabelFont != null)
            {
                _altitudeLabelFont.Dispose();
                _altitudeLabelFont = null;
            }

            if (_needleShadowBrush != null)
            {
                _needleShadowBrush.Dispose();
                _needleShadowBrush = null;
            }
            if (_tensOfThousandsBrush != null)
            {
                _tensOfThousandsBrush.Dispose();
                _tensOfThousandsBrush = null;
            }
            if (_thousandsBrush != null)
            {
                _thousandsBrush.Dispose();
                _thousandsBrush = null;
            }
            if (_hundredsBrush != null)
            {
                _hundredsBrush.Dispose();
                _hundredsBrush = null;
            }
            if (_tensOfThousandsPen != null)
            {
                _tensOfThousandsPen.Dispose();
                _tensOfThousandsPen = null;
            }
            if (_thousandsPen != null)
            {
                _thousandsPen.Dispose();
                _thousandsPen = null;
            }
            if (_hundredsPen != null)
            {
                _hundredsPen.Dispose();
                _hundredsPen = null;
            }
            if (_valueFont != null)
            {
                _valueFont.Dispose();
                _valueFont = null;
            }
            if (_valueBrush != null)
            {
                _valueBrush.Dispose();
                _valueBrush = null;
            }

            // Move on down the line
            base.Dispose(disposing);
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
        [Category("Behavior")]
        [DefaultValue(typeof(bool), "False")]
        [Description("Controls whether the Value property is set manually, or automatically read from any available GPS device.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsUsingRealTimeData
        {
            get
            {
                return _isUsingRealTimeData;
            }
            set
            {
                // Has anything changed?  If not, exit
                if (_isUsingRealTimeData == value)
                    return;

                // Set the new value
                _isUsingRealTimeData = value;

                // Set the control to the last known altitude (if any)
                if (_isUsingRealTimeData)
                {
                    try
                    {
                        // Hook into the AltitudeChanged event\
                        // When in design mode, do not hook into events
                        if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                        {
                            // Hook into the altitude changed event
                            Devices.AltitudeChanged += Devices_CurrentAltitudeChanged;
                        }

                        // Set the value of the control
                        Value = DesignMode ? Distance.Empty : Devices.Altitude;
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
                        Devices.AltitudeChanged -= Devices_CurrentAltitudeChanged;
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
        [Category("Altitude Label")]
        [DefaultValue(typeof(Font), "Tahoma, 9pt")]
        [Description("Controls the font of the label displaying the current altitude.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Font ValueFont
        {
            get
            {
                return _valueFont;
            }
            set
            {
                if (_valueFont.Equals(value)) return;
                _valueFont = value;
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
        [Category("Altitude Label")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of the label displaying the current altitude.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color ValueColor
        {
            get
            {
                return _valueBrush.Color;
            }
            set
            {
                if (_valueBrush.Color.Equals(value)) return;
                _valueBrush.Color = value;
                InvokeRepaint();
            }
        }

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
        [Category("Altitude Label")]
        [DefaultValue(typeof(string), "v uu")]
        [Description("Controls the format of the label displaying the current altitude.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string ValueFormat
        {
            get
            {
                return _valueFormat;
            }
            set
            {
                if (_valueFormat.Equals(value)) return;
                _valueFormat = value;
                InvokeRepaint();
            }
        }

        /// <summary>Controls the altitude to display on the control.</summary>
        /// <value>A <strong>Distance</strong> structure measuring the altitude to display.</value>
        /// <remarks>
        /// Changing this property causes the needles on the control to move so that they
        /// represent the value.
        /// </remarks>
        [Category("Appearance")]
        [DefaultValue(typeof(Distance), "0 m")]
        [Description("Controls the amount of distance above sea level being displayed in the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        ////[CLSCompliant(false)]
        public Distance Value
        {
            get
            {
                return _altitude;
            }
            set
            {
                if (_altitude.Equals(value))
                    return;
                _altitude = value;
                // Force either feet or meters
                _altitude = _altitude.IsMetric ? _altitude.ToMeters() : _altitude.ToFeet();

                if (IsDisposed)
                    return;

                lock (_valueInterpolator)
                {
                    // Are we changing direction?
                    if (_altitude.Value >= _valueInterpolator.Minimum
                        && _altitude.Value > _valueInterpolator[_interpolationIndex])
                    {
                        // No.  Just set the new maximum
                        _valueInterpolator.Maximum = _altitude.Value;
                    }
                    else if (_altitude.Value < _valueInterpolator.Minimum)
                    {
                        // We're changing directions, so stop then accellerate again
                        _valueInterpolator.Minimum = _valueInterpolator[_interpolationIndex];
                        _valueInterpolator.Maximum = _altitude.Value;
                        _interpolationIndex = 0;
                    }
                    else if (_altitude.Value > _valueInterpolator.Minimum
                        && _altitude.Value < _valueInterpolator[_interpolationIndex])
                    {
                        // We're changing directions, so stop then accellerate again
                        _valueInterpolator.Minimum = _valueInterpolator[_interpolationIndex];
                        _valueInterpolator.Maximum = _altitude.Value;
                        _interpolationIndex = 0;
                    }
                    else if (_altitude.Value > _valueInterpolator.Maximum)
                    {
                        // No.  Just set the new maximum
                        _valueInterpolator.Maximum = _altitude.Value;
                    }
                }

                _animationWaitHandle.Set();

                // Signal that the value has changed
                OnValueChanged(new DistanceEventArgs(_altitude));
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
        [Category("Altitude Label")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of altitude digits drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]

        public Color AltitudeLabelColor
        {
            get
            {
                return _altitudeLabelBrush.Color;
            }
            set
            {
                if (_altitudeLabelBrush.Color.Equals(value)) return;
                _altitudeLabelBrush.Color = value;
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
        [Category("Altitude Label")]
        [DefaultValue(typeof(Font), "Tahoma, 12pt")]
        [Description("Controls the font of altitude digits drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Font AltitudeLabelFont
        {
            get
            {
                return _altitudeLabelFont;
            }
            set
            {
                if (_altitudeLabelFont.Equals(value)) return;
                _altitudeLabelFont = value;
                InvokeRepaint();
            }
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
        [Category("Tick Marks")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of smaller tick marks drawn around the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
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

        /// <summary>Controls the interior color of the tens-of-thousands needle.</summary>
        /// <remarks>
        /// The tens-of-thousands needle is the smallest needle of the control. The interior
        /// can be made invisible by setting this property to <strong>Transparent</strong>.
        /// </remarks>
        [Category("Needle Colors")]
        [DefaultValue(typeof(Color), "Red")]
        [Description("Controls the color of the interior of the tens-of-thousands needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color TensOfThousandsNeedleFillColor
        {
            get
            {
                return _tensOfThousandsBrush.Color;
            }
            set
            {
                if (_tensOfThousandsBrush.Color.Equals(value)) return;
                _tensOfThousandsBrush.Color = value;
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Controls the color of the interior of the thousands needle.
        /// </summary>
        [Category("Needle Colors")]
        [DefaultValue(typeof(Color), "Red")]
        [Description("Controls the color of the interior of the thousands needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color ThousandsNeedleFillColor
        {
            get
            {
                return _thousandsBrush.Color;
            }
            set
            {
                if (_thousandsBrush.Color.Equals(value)) return;
                _thousandsBrush.Color = value;
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Controls the color of the interior of the hundreds needle.
        /// </summary>
        [Category("Needle Colors")]
        [DefaultValue(typeof(Color), "Red")]
        [Description("Controls the color of the interior of the hundreds needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HundredsNeedleFillColor
        {
            get
            {
                return _hundredsBrush.Color;
            }
            set
            {
                if (_hundredsBrush.Color.Equals(value)) return;
                _hundredsBrush.Color = value;
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Controls the color of the edge of the tens-of-thousands needle.
        /// </summary>
        [Category("Needle Colors")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of the edge of the tens-of-thousands needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color TensOfThousandsNeedleOutlineColor
        {
            get
            {
                return _tensOfThousandsPen.Color;
            }
            set
            {
                if (_tensOfThousandsPen.Color.Equals(value)) return;
                _tensOfThousandsPen.Color = value;
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Controls the color of the edge of the thousands needle.
        /// </summary>
        [Category("Needle Colors")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of the edge of the thousands needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color ThousandsNeedleOutlineColor
        {
            get
            {
                return _thousandsPen.Color;
            }
            set
            {
                if (_thousandsPen.Color.Equals(value)) return;
                _thousandsPen.Color = value;
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Controls the color of the edge of the hundreds needle.
        /// </summary>
        [Category("Needle Colors")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color of the edge of the hundreds needle.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HundredsNeedleOutlineColor
        {
            get
            {
                return _hundredsPen.Color;
            }
            set
            {
                if (_hundredsPen.Color.Equals(value)) return;
                _hundredsPen.Color = value;
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Controls the color of the shadow cast by the altitude needles.
        /// </summary>
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
                return _needleShadowBrush.Color;
            }
            set
            {
                if (_needleShadowBrush.Color.Equals(value)) return;
                _needleShadowBrush.Color = value;
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Controls the size of the shadow cast by the altitude needles.
        /// </summary>
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
                return _needleShadowSize;
            }
            set
            {
                _needleShadowSize = value;
                InvokeRepaint();
            }
        }

        /// <inheritdocs/>
        protected override void OnPaintOffScreen(PaintEventArgs e)
        {
            PolarGraphics f = CreatePolarGraphics(e.Graphics);

            // What altitude are we drawing?

            Distance altitudeToRender = new Distance(_valueInterpolator[_interpolationIndex], _altitude.Units);

            // There are 100 tick marks in 360 degrees.   3.6° per tick mark
            const double conversionFactor = 3.6;
            // Draw tick marks
            if (_minorTickPen != null)
            {
                for (double alt = 0; alt < 100; alt += 1)
                {
                    // Convert the speed to an angle
                    Angle angle = new Angle(alt * conversionFactor);
                    // Get the coordinate of the line's start
                    PolarCoordinate start = new PolarCoordinate(95, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    // And draw a line
                    f.DrawLine(_minorTickPen, start, end);
                }
            }
            // Draw tick marks
            if (_majorTickPen != null)
            {
                for (double alt = 0; alt < 100; alt += 10)
                {
                    // Convert the speed to an angle
                    Angle angle = new Angle(alt * conversionFactor);
                    // Get the coordinate of the line's start
                    PolarCoordinate start = new PolarCoordinate(94, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    PolarCoordinate end = new PolarCoordinate(100, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise);
                    // And draw a line
                    f.DrawLine(_majorTickPen, start, end);
                    // And also a string
                    string s = Convert.ToString(alt * 0.1, CultureInfo.CurrentCulture);
                    f.DrawCenteredString(s, _altitudeLabelFont, _altitudeLabelBrush,
                        new PolarCoordinate(85, angle, Azimuth.North, PolarCoordinateOrientation.Clockwise));
                }
            }
            // Calculate all needle values
            double pTensOfThousandsValue = altitudeToRender.Value / 1000.0;
            double pThousandsValue = altitudeToRender.Value / 100.0;
            double pValue = altitudeToRender.Value / 10.0;

            // Now draw the tens-of-thousands needle
            // Rotate the needle to the right place
            PolarCoordinate[] needle1 = new PolarCoordinate[_tensOfThousandsNeedle.Length];
            for (int index = 0; index < needle1.Length; index++)
                needle1[index] = _tensOfThousandsNeedle[index].Rotate(pTensOfThousandsValue * conversionFactor);

            // Now draw the tens-of-thousands needle
            // Rotate the needle to the right place
            PolarCoordinate[] needle2 = new PolarCoordinate[_thousandsNeedle.Length];
            for (int index = 0; index < needle2.Length; index++)
            {
                needle2[index] = _thousandsNeedle[index].Rotate(pThousandsValue * conversionFactor);
            }

            // Now draw the tens-of-Hundreds needle
            // Rotate the needle to the right place
            PolarCoordinate[] needle3 = new PolarCoordinate[_hundredsNeedle.Length];
            for (int index = 0; index < needle3.Length; index++)
            {
                needle3[index] = _hundredsNeedle[index].Rotate(pValue * conversionFactor);
            }

            string altitudeString = altitudeToRender.ToString(_valueFormat, CultureInfo.CurrentCulture);
            //SizeF FontSize = f.Graphics.MeasureString(AltitudeString, pValueFont);

            f.DrawRotatedString(altitudeString, _valueFont,
                _valueBrush, new PolarCoordinate(45.0f, Angle.Empty, Azimuth.North, PolarCoordinateOrientation.Clockwise));

            // Draw an ellipse at the center
            f.DrawEllipse(_centerPen, PolarCoordinate.Empty, 10);

            f.Graphics.TranslateTransform(_needleShadowSize.Width, _needleShadowSize.Height, MatrixOrder.Append);

            f.FillPolygon(_needleShadowBrush, needle1);
            f.FillPolygon(_needleShadowBrush, needle2);
            f.FillPolygon(_needleShadowBrush, needle3);

            f.Graphics.ResetTransform();

            f.FillPolygon(_tensOfThousandsBrush, needle1);
            f.DrawPolygon(_tensOfThousandsPen, needle1);
            f.FillPolygon(_thousandsBrush, needle2);
            f.DrawPolygon(_thousandsPen, needle2);
            f.FillPolygon(_hundredsBrush, needle3);
            f.DrawPolygon(_hundredsPen, needle3);

            // Draw an ellipse at the center
            f.FillEllipse(_hundredsBrush, PolarCoordinate.Empty, 7);
            f.DrawEllipse(_hundredsPen, PolarCoordinate.Empty, 7);
        }

        private void Devices_CurrentAltitudeChanged(object sender, DistanceEventArgs e)
        {
            if (_isUsingRealTimeData)
                Value = e.Distance;
        }

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
            }
        }
    }
}