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
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

#if !PocketPC || DesignTime || Framework20

using System.Drawing.Drawing2D;
using System.ComponentModel;

#endif

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime

    /// <summary>
    /// Represents a control used to display satellite signal strengths.
    /// </summary>
    [ToolboxBitmap(typeof(SatelliteSignalBar))]
    [DefaultProperty("Satellites")]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
    [DesignTimeVisible(true)]
#endif
    //[CLSCompliant(false)]
    public sealed class SatelliteSignalBar : DoubleBufferedControl
    {
        private List<Satellite> _satellites = new List<Satellite>();
        private List<Satellite> _fixedSatellites = new List<Satellite>();
        private int _gapWidth = 4;
        private Font _signalStrengthLabelFont = new Font("Tahoma", 6.0f, FontStyle.Regular);
        private SolidBrush _signalStrengthLabelBrush = new SolidBrush(Color.Black);
        private Font _pseudorandomNumberFont = new Font("Tahoma", 8.0f, FontStyle.Regular);
        private SolidBrush _pseudorandomNumberBrush = new SolidBrush(Color.Black);
        private bool _isUsingRealTimeData = true;
#if PocketPC
		private SolidBrush pSatelliteFixBrush = new SolidBrush(Color.LimeGreen);
#else
        private Color _satelliteFixColor = Color.LimeGreen;
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
        private readonly ColorInterpolator _fillModerate = new ColorInterpolator(Color.Orange, Color.Green, 10);
        private readonly ColorInterpolator _fillGood = new ColorInterpolator(Color.Green, Color.LightGreen, 10);
        private readonly ColorInterpolator _fillExcellent = new ColorInterpolator(Color.LightGreen, Color.White, 10);
        private readonly ColorInterpolator _outlineNone = new ColorInterpolator(Color.Transparent, Color.Gray, 10);
        private readonly ColorInterpolator _outlinePoor = new ColorInterpolator(Color.Gray, Color.Gray, 10);
        private readonly ColorInterpolator _outlineModerate = new ColorInterpolator(Color.Gray, Color.Gray, 10);
        private readonly ColorInterpolator _outlineGood = new ColorInterpolator(Color.Gray, Color.Gray, 10);
        private readonly ColorInterpolator _outlineExcellent = new ColorInterpolator(Color.Gray, Color.LightGreen, 10);

        //// Defines a 5-pointed star shape
        //private PolarCoordinate[] StarShape = new PolarCoordinate[] {
        //    new PolarCoordinate(0, Angle.Empty),

        // private object RenderSyncLock = new object();
        /// <summary>
        /// Satellite Signal Bar
        /// </summary>
        public SatelliteSignalBar()
            : base("DotSpatial.Positioning Multithreaded Satellite Signal Bar Control (http://dotspatial.codeplex.com)")
        {
#if PocketPC
            Size = new Size(100, 50);
#else
            Size = new Size(200, 100);
#endif

#if PocketPC && !Framework20 && !DesignTime
            DotSpatial.Positioning.Gps.IO.Devices.SatellitesChanged += new SatelliteCollectionEventHandler(Devices_CurrentSatellitesChanged);
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
                }
            }
            catch
            {
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

        /// <inheritdocs/>
        protected override void Dispose(bool disposing)
        {
#if PocketPC && !Framework20 && !DesignTime
            try
            {
                // This must be successful in order for the finalizer to fire
                DotSpatial.Positioning.Gps.IO.Devices.SatellitesChanged -= new SatelliteCollectionEventHandler(Devices_CurrentSatellitesChanged);
            }
            catch
            {
            }
#endif

#if PocketPC
			if (pSatelliteFixBrush != null)
			{
				try
				{
					pSatelliteFixBrush.Dispose();
				}
				catch
				{
				}
				finally
				{
					pSatelliteFixBrush = null;
				}
			}
#endif

            if (_signalStrengthLabelFont != null)
            {
                try
                {
                    _signalStrengthLabelFont.Dispose();
                }
                finally
                {
                    _signalStrengthLabelFont = null;
                }
            }
            if (_signalStrengthLabelBrush != null)
            {
                try
                {
                    _signalStrengthLabelBrush.Dispose();
                }
                finally
                {
                    _signalStrengthLabelBrush = null;
                }
            }
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

            // Move on down the line

            base.Dispose(disposing);
        }

#if !PocketPC

        /// <inheritdocs/>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 45);
            }
        }

#endif

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the satellites which are currently being viewed in the control.
        /// </summary>
        [Category("Satellites")]
        [Description("Controls the satellites which are currently being viewed in the control.")]
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

                if (value == null)
                    _fixedSatellites = null;
                else
                {
                    // Get fixed satellites
                    _fixedSatellites = Satellite.GetFixedSatellites(_satellites);

                    // Sort satellites by signal strength
                    _satellites.Sort();
                    _fixedSatellites.Sort();
                }

                // Refresxh the control
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the number of pixels in between vertical satellite signal bars.
        /// </summary>
        [Category("Appearance")]
        [Description("Controls the number of pixels in between vertical satellite signal bars.")]
        [DefaultValue(typeof(int), "4")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public int GapWidth
        {
            get
            {
                return _gapWidth;
            }
            set
            {
                if (_gapWidth == value) return;
                _gapWidth = value;
                InvokeRepaint();
            }
        }

        /// <inheritdocs/>
        protected override void OnInitialize()
        {
            base.OnInitialize();

#if PocketPC
#if DesignTime
			_Satellites = SatelliteCollection.Random(45);
#else
            if (_IsUsingRealTimeData)
                _Satellites = DotSpatial.Positioning.Gps.IO.Devices.Satellites;
#endif
#else
            if (DesignMode)
            {
                // TODO: Random satellites
                //pSatellites = SatelliteCollection.Random(45);
            }
            else if (_isUsingRealTimeData)
                _satellites = Devices.Satellites;
#endif
        }

        /// <inheritdocs/>
        protected override void OnPaintOffScreen(PaintEventArgs e)
        {
            // Sort the satellites by signal
            if (_satellites == null)
                return;

            // Decide which collection to display
            List<Satellite> satellitesToDraw;
            List<Satellite> satellitesToRender;

            try
            {
#if PocketPC
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                {
                    //SatellitesToRender = SatelliteCollection.Random(45);
                }
                else
                {
                    if (_Satellites == null)
                        return;

                    SatellitesToRender = new List<Satellite>();

                foreach (Satellite satellite in _Satellites)
                        {
                            // Don't draw if the satellite is stale
                            if (!satellite.IsActive)
                                continue;
                            // Add it in
                            SatellitesToRender.Add(satellite);
                        }
                    Thread.Sleep(0);
                }
#else

                satellitesToRender = new List<Satellite>();

                for (int index = 0; index < _satellites.Count; index++)
                {
                    Satellite satellite = _satellites[index];

                    // Don't draw if the satellite is stale
                    if (!satellite.IsActive && !DesignMode)
                        continue;
                    // Add it in
                    satellitesToRender.Add(satellite);
                }
#endif

                // Make a fake collection if necessary
                if (satellitesToRender.Count == 0)
                    return;

                // Sort the list by PRN
                satellitesToRender.Sort();

                // Calculate the width of each bar
                float totalWidth = (Width - _gapWidth) / (float)satellitesToRender.Count;
                float barWidth = totalWidth - _gapWidth;

                satellitesToDraw = new List<Satellite>();

                // If if the bars are thin, see if we can exclude 0 dB satellites
                if (barWidth < 15)
                {
                    // Display only the satellites with a > 0 dB signal
                    foreach (Satellite satellite in satellitesToRender)
                    {
                        // Draw if the signal is > 0
                        if (!satellite.SignalToNoiseRatio.IsEmpty)
                            satellitesToDraw.Add(satellite);
                    }
                    // If there's anything left, recalculate
                    if (satellitesToDraw.Count == 0)
                        return;
                    // Recalculate bar/total width
                    totalWidth = (Width - _gapWidth) / (float)satellitesToDraw.Count;
                    barWidth = totalWidth - _gapWidth;
                }
                else
                {
                    // Display only the satellites with a > 0 dB signal
                    foreach (Satellite satellite in satellitesToRender)
                    {
                        // Draw if the signal is > 0
                        satellitesToDraw.Add(satellite);
                    }
                }

                // Anything to do?
                if (satellitesToDraw.Count == 0)
                    return;

                // Now draw each one
                float startX = _gapWidth;
                float startY = Height - e.Graphics.MeasureString("10", _pseudorandomNumberFont).Height;
                float scaleFactor = (startY
                                     - e.Graphics.MeasureString("10", _signalStrengthLabelFont).Height) / 50.0f;

                foreach (Satellite satellite in satellitesToDraw)
                {
                    // Each icon is 30x30, so we'll translate it by half the distance
                    if (satellite.IsFixed)
                    {
                        SizeF prnSize = e.Graphics.MeasureString(satellite.PseudorandomNumber.ToString(CultureInfo.CurrentCulture), _pseudorandomNumberFont);

#if PocketPC
                        e.Graphics.FillEllipse(pSatelliteFixBrush, (int)(StartX + (BarWidth * 0.5) - (PrnSize.Width * 0.5) - 4.0), (int)(StartY - 4), (int)(PrnSize.Width + 8), (int)(PrnSize.Height + 8));
#else
                        using (SolidBrush fixBrush = new SolidBrush(Color.FromArgb(Math.Min(255, _fixedSatellites.Count * 20), _satelliteFixColor)))
                        {
                            e.Graphics.FillEllipse(fixBrush, (float)(startX + (barWidth * 0.5) - (prnSize.Width * 0.5) - 4.0), startY - 4, prnSize.Width + 8, prnSize.Height + 8);
                        }
#endif
                    }

                    startX += _gapWidth;
                    startX += barWidth;
                }

                startX = _gapWidth;

                foreach (Satellite satellite in satellitesToDraw)
                {
                    // If the signal is 0dB, skip it
                    if (satellite.SignalToNoiseRatio.Value == 0)
                        continue;

                    // Keep drawing the satellite
                    float satelliteY = startY - (Math.Min(satellite.SignalToNoiseRatio.Value, 50) * scaleFactor);

#if PocketPC
                    // Draw a rectangle for each satellite
                    SolidBrush FillBrush = new SolidBrush(GetFillColor(satellite.SignalToNoiseRatio));
                    e.Graphics.FillRectangle(FillBrush, (int)StartX, (int)SatelliteY, (int)BarWidth, (int)(StartY - SatelliteY));
                    FillBrush.Dispose();

                    Pen FillPen = new Pen(GetOutlineColor(satellite.SignalToNoiseRatio));
                    e.Graphics.DrawRectangle(FillPen, (int)StartX, (int)SatelliteY, (int)BarWidth, (int)(StartY - SatelliteY));
                    FillPen.Dispose();
#else
                    // Get the fill color
                    Color barColor = GetFillColor(satellite.SignalToNoiseRatio);
                    float barHue = barColor.GetHue();

                    // Create gradients for a glass effect
                    Color topTopColor = ColorFromAhsb(255, barHue, 0.2958f, 0.7292f);
                    Color topBottomColor = ColorFromAhsb(255, barHue, 0.5875f, 0.35f);
                    Color bottomTopColor = ColorFromAhsb(255, barHue, 0.7458f, 0.2f);
                    Color bottomBottomColor = ColorFromAhsb(255, barHue, 0.6f, 0.4042f);

                    // Draw a rectangle for each satellite
                    RectangleF topRect = new RectangleF(startX, satelliteY, barWidth, Convert.ToSingle((startY - satelliteY) * 0.5));
                    using (Brush topFillBrush = new LinearGradientBrush(topRect, topTopColor, topBottomColor, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(topFillBrush, topRect);
                    }
                    // Draw a rectangle for each satellite
                    RectangleF bottomRect = new RectangleF(startX, satelliteY + topRect.Height, barWidth, topRect.Height);
                    using (Brush bottomFillBrush = new LinearGradientBrush(bottomRect, bottomTopColor, bottomBottomColor, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(bottomFillBrush, bottomRect);
                    }

                    using (Pen fillPen = new Pen(GetOutlineColor(satellite.SignalToNoiseRatio), 1.0f))
                    {
                        e.Graphics.DrawRectangle(fillPen, startX, satelliteY, barWidth, startY - satelliteY);
                    }
#endif
                    string prnString = satellite.PseudorandomNumber.ToString(CultureInfo.CurrentCulture);
                    SizeF prnSize = e.Graphics.MeasureString(prnString, _pseudorandomNumberFont);
                    e.Graphics.DrawString(prnString, _pseudorandomNumberFont,
                        _pseudorandomNumberBrush, (float)(startX + (barWidth * 0.5) - (prnSize.Width * 0.5)), startY);

                    string renderString = satellite.SignalToNoiseRatio.ToString("0 dB", CultureInfo.CurrentCulture);
                    SizeF signalSize = e.Graphics.MeasureString(renderString, _signalStrengthLabelFont);
                    if (signalSize.Width > barWidth)
                    {
                        renderString = satellite.SignalToNoiseRatio.ToString("0 dB", CultureInfo.CurrentCulture).Replace(" dB", string.Empty);
                        signalSize = e.Graphics.MeasureString(renderString, _signalStrengthLabelFont);
                    }
                    e.Graphics.DrawString(renderString, _signalStrengthLabelFont,
                        _signalStrengthLabelBrush, (float)(startX + (barWidth * 0.5) - (signalSize.Width * 0.5)), satelliteY - signalSize.Height);

                    startX += _gapWidth;
                    startX += barWidth;
                }
            }
            catch
            {
            }
        }

        private Color GetFillColor(SignalToNoiseRatio signal)
        {
            if (signal.Value < 10)
                return _fillNone[signal.Value];
            if (signal.Value < 20)
                return _fillPoor[signal.Value - 10];
            if (signal.Value < 30)
                return _fillModerate[signal.Value - 20];
            if (signal.Value < 40)
                return _fillGood[signal.Value - 30];
            if (signal.Value < 50)
                return _fillExcellent[signal.Value - 40];
            return _fillExcellent[9];
        }

        private Color GetOutlineColor(SignalToNoiseRatio signal)
        {
            if (signal.Value < 10)
                return _outlineNone[signal.Value];
            if (signal.Value < 20)
                return _outlinePoor[signal.Value - 10];
            if (signal.Value < 30)
                return _outlineModerate[signal.Value - 20];
            if (signal.Value < 40)
                return _outlineGood[signal.Value - 30];
            if (signal.Value < 50)
                return _outlineExcellent[signal.Value - 40];
            return _outlineExcellent[9];
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
                _fillNone.StartColor = value;
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
                _fillModerate.StartColor = value;
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
                _fillGood.StartColor = value;
                _fillModerate.EndColor = value;
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
                _fillExcellent.EndColor = value;
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
                _outlineNone.StartColor = value;
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
                _outlineNone.EndColor = value;
                _outlinePoor.StartColor = value;
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
                _outlinePoor.EndColor = value;
                _outlineModerate.StartColor = value;
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
                _outlineModerate.EndColor = value;
                _outlineGood.StartColor = value;
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
                _outlineExcellent.StartColor = value;
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
				return pSatelliteFixBrush.Color;
#else
                return _satelliteFixColor;
#endif
            }
            set
            {
#if PocketPC
				if (pSatelliteFixBrush.Color.Equals(value))
					return;
				pSatelliteFixBrush.Color = value;
#else
                if (_satelliteFixColor.Equals(value)) return;
                _satelliteFixColor = value;
#endif
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
                // Has anything actually changed?
                if (_isUsingRealTimeData == value)
                    return;

                _isUsingRealTimeData = value;
#if !DesignTime
                if (_isUsingRealTimeData)
                    _satellites = Devices.Satellites;
#endif
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the font used to draw the strength of each satellite.
        /// </summary>
        [Category("Satellite Colors")]
        [DefaultValue(typeof(Font), "Tahoma, 6pt")]
        [Description("Controls the font used to draw the strength of each satellite.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Font SignalStrengthLabelFont
        {
            get
            {
                return _signalStrengthLabelFont;
            }
            set
            {
                if (_signalStrengthLabelFont.Equals(value)) return;
                _signalStrengthLabelFont = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the color used to draw the strength of each satellite.
        /// </summary>
        [Category("Satellite Colors")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color used to draw the strength of each satellite.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Color SignalStrengthLabelColor
        {
            get
            {
                return _signalStrengthLabelBrush.Color;
            }
            set
            {
                if (_signalStrengthLabelBrush.Color.Equals(value)) return;
                _signalStrengthLabelBrush.Color = value;
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Controls the font used to draw the ID of each satellite.
        /// </summary>
        [Category("Satellite Colors")]
        [DefaultValue(typeof(Font), "Tahoma, 8pt")]
        [Description("Controls the font used to draw the ID of each satellite.")]
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
        /// Controls the color used to draw the ID of each satellite.
        /// </summary>
        [Category("Satellite Colors")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Controls the color used to draw the ID of each satellite.")]
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
                if (_pseudorandomNumberBrush.Color.Equals(value)) return;
                _pseudorandomNumberBrush.Color = value;
                InvokeRepaint();
            }
        }

        private void Devices_CurrentSatellitesChanged(object sender, SatelliteListEventArgs e)
        {
            if (_isUsingRealTimeData)
            {
                //TODO should this be done here or assigned from a user defined handler for the event?
                Satellites = (List<Satellite>)e.Satellites;
                InvokeRepaint();
            }
        }
    }
}