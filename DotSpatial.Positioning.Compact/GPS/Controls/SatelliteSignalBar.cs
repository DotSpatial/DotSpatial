using System;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using DotSpatial.Positioning.Drawing;
using DotSpatial.Positioning.Gps.IO;
using DotSpatial.Positioning.Gps;
#if !PocketPC || DesignTime || Framework20
using System.Drawing.Drawing2D;
using System.ComponentModel;
#endif
using DotSpatial.Positioning;

#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Gps.Controls
{
	/// <summary>
	/// Represents a control used to display satellite signal strengths.
	/// </summary>
#if !PocketPC || DesignTime
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
        private List<Satellite> _Satellites = new List<Satellite>();
        private List<Satellite> _FixedSatellites = new List<Satellite>();
        private int _GapWidth = 4;
		private Font _SignalStrengthLabelFont = new Font("Tahoma", 6.0f, FontStyle.Regular);
		private SolidBrush _SignalStrengthLabelBrush = new SolidBrush(Color.Black);
		private Font _PseudorandomNumberFont = new Font("Tahoma", 8.0f, FontStyle.Regular);
		private SolidBrush _PseudorandomNumberBrush = new SolidBrush(Color.Black);
		private bool _IsUsingRealTimeData = true;
#if PocketPC
		private SolidBrush pSatelliteFixBrush = new SolidBrush(Color.LimeGreen);
#else
		private Color _SatelliteFixColor = Color.LimeGreen;
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
		private ColorInterpolator _FillModerate = new ColorInterpolator(Color.Orange, Color.Green, 10);
		private ColorInterpolator _FillGood = new ColorInterpolator(Color.Green, Color.LightGreen, 10);
		private ColorInterpolator _FillExcellent = new ColorInterpolator(Color.LightGreen, Color.White, 10);
		private ColorInterpolator _OutlineNone = new ColorInterpolator(Color.Transparent, Color.Gray, 10);
		private ColorInterpolator _OutlinePoor = new ColorInterpolator(Color.Gray, Color.Gray, 10);
		private ColorInterpolator _OutlineModerate = new ColorInterpolator(Color.Gray, Color.Gray, 10);
		private ColorInterpolator _OutlineGood = new ColorInterpolator(Color.Gray, Color.Gray, 10);
		private ColorInterpolator _OutlineExcellent = new ColorInterpolator(Color.Gray, Color.LightGreen, 10);

        //// Defines a 5-pointed star shape
        //private PolarCoordinate[] StarShape = new PolarCoordinate[] {
        //    new PolarCoordinate(0, Angle.Empty),


       // private object RenderSyncLock = new object();

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

            if (_SignalStrengthLabelFont != null)
            {
                try
                {
                    _SignalStrengthLabelFont.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _SignalStrengthLabelFont = null;
                }
            }
            if (_SignalStrengthLabelBrush != null)
            {
                try
                {
                    _SignalStrengthLabelBrush.Dispose();
                }
                catch
                {
                }
                finally
                {
                    _SignalStrengthLabelBrush = null;
                }
            }
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

            // Move on down the line
            try
            {
                base.Dispose(disposing);
            }
            catch
            {
            }
        }

      

#if !PocketPC
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 45);
			}
		}
#endif

#if !PocketPC || DesignTime
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
				return _Satellites;
			}
            set
            {
				_Satellites = value;

                if (value == null)
                    _FixedSatellites = null;
                else
                {
                    // Get fixed satellites
                    _FixedSatellites = Satellite.GetFixedSatellites(_Satellites);

                    // Sort satellites by signal strength
                    _Satellites.Sort();
                    _FixedSatellites.Sort();
                }

                // Refresxh the control
                InvokeRepaint();
            }
		}

#if !PocketPC || DesignTime
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
				return _GapWidth;
			}
			set
			{
				if (_GapWidth == value) return;
				_GapWidth = value;
				InvokeRepaint();
			}
		}

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
            else if (_IsUsingRealTimeData)
                _Satellites = Devices.Satellites;
#endif
		}

		////[CLSCompliant(false)]
        protected override void OnPaintOffScreen(PaintEventArgs e)
        {
			// Sort the satellites by signal
            if (_Satellites == null)
                return;

            // Decide which collection to display
            List<Satellite> SatellitesToDraw = null;
            List<Satellite> SatellitesToRender = null;
            float BarWidth;

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
				if(_Satellites == null)
					return;

				SatellitesToRender = new List<Satellite>();
			
				for(int index = 0; index < _Satellites.Count; index++)
                {
                    Satellite satellite = _Satellites[index];

                    // Don't draw if the satellite is stale
					if (!satellite.IsActive && !DesignMode)
						continue;
					// Add it in
					SatellitesToRender.Add(satellite);
				}
#endif

                // Make a fake collection if necessary
                if (SatellitesToRender.Count == 0)
                    return;

                // Sort the list by PRN
                SatellitesToRender.Sort();

                // Calculate the width of each bar
                float TotalWidth = (Width - _GapWidth) / SatellitesToRender.Count;
                BarWidth = TotalWidth - _GapWidth;

                SatellitesToDraw = new List<Satellite>();

                // If if the bars are thin, see if we can exclude 0 dB satellites
                if (BarWidth < 15)
                {
                    // Display only the satellites with a > 0 dB signal
                    foreach (Satellite satellite in SatellitesToRender)
                    {
                        // Draw if the signal is > 0
                        if (!satellite.SignalToNoiseRatio.IsEmpty)
                            SatellitesToDraw.Add(satellite);
                    }
                    // If there's anything left, recalculate
                    if (SatellitesToDraw.Count == 0)
                        return;
                    // Recalculate bar/total width
                    TotalWidth = (Width - _GapWidth) / SatellitesToDraw.Count;
                    BarWidth = TotalWidth - _GapWidth;
                }
                else
                {
                    // Display only the satellites with a > 0 dB signal
                    foreach (Satellite satellite in SatellitesToRender)
                    {
                        // Draw if the signal is > 0
                        SatellitesToDraw.Add(satellite);
                    }
                }

                // Anything to do?
                if (SatellitesToDraw.Count == 0)
                    return;

                // Now draw each one
                float StartX = _GapWidth;
                float StartY = (float)(Height - e.Graphics.MeasureString("10", _PseudorandomNumberFont).Height);
                float ScaleFactor = (float)(StartY
                    - e.Graphics.MeasureString("10", _SignalStrengthLabelFont).Height) / 50.0f;                

                foreach (Satellite satellite in SatellitesToDraw)
                {

                    float SatelliteY = StartY - (satellite.SignalToNoiseRatio.Value * ScaleFactor);

                    // Each icon is 30x30, so we'll translate it by half the distance
                    if (satellite.IsFixed)
                    {
                        SizeF PrnSize = e.Graphics.MeasureString(satellite.PseudorandomNumber.ToString(CultureInfo.CurrentCulture), _PseudorandomNumberFont);

#if PocketPC
                        e.Graphics.FillEllipse(pSatelliteFixBrush, (int)(StartX + (BarWidth * 0.5) - (PrnSize.Width * 0.5) - 4.0), (int)(StartY - 4), (int)(PrnSize.Width + 8), (int)(PrnSize.Height + 8));
#else
						using (SolidBrush FixBrush = new SolidBrush(Color.FromArgb(Math.Min(255, _FixedSatellites.Count * 20), _SatelliteFixColor)))
						{
							e.Graphics.FillEllipse(FixBrush, (float)(StartX + (BarWidth * 0.5) - (PrnSize.Width * 0.5) - 4.0), StartY - 4, PrnSize.Width + 8, PrnSize.Height + 8);
						}
#endif
                    }

                    StartX += _GapWidth;
                    StartX += BarWidth;
                }

                StartX = _GapWidth;

                foreach (Satellite satellite in SatellitesToDraw)
                {
                    // If the signal is 0dB, skip it
                    if (satellite.SignalToNoiseRatio.Value == 0)
                        continue;

                    // Keep drawing the satellite
                    float SatelliteY = StartY - (Math.Min(satellite.SignalToNoiseRatio.Value, 50) * ScaleFactor);

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
                    Color BarColor = GetFillColor(satellite.SignalToNoiseRatio);
                    float BarHue = BarColor.GetHue();

                    // Create gradients for a glass effect
                    Color topTopColor = DoubleBufferedControl.ColorFromAhsb(255, BarHue, 0.2958f, 0.7292f);
                    Color topBottomColor = DoubleBufferedControl.ColorFromAhsb(255, BarHue, 0.5875f, 0.35f);
                    Color bottomTopColor = DoubleBufferedControl.ColorFromAhsb(255, BarHue, 0.7458f, 0.2f);
                    Color bottomBottomColor = DoubleBufferedControl.ColorFromAhsb(255, BarHue, 0.6f, 0.4042f);

					// Draw a rectangle for each satellite
                    RectangleF TopRect = new RectangleF(StartX, SatelliteY, BarWidth, Convert.ToSingle((StartY - SatelliteY) * 0.5));
                    using (Brush TopFillBrush = new LinearGradientBrush(TopRect, topTopColor, topBottomColor, LinearGradientMode.Vertical))
					{
                        e.Graphics.FillRectangle(TopFillBrush, TopRect);
					}
                    // Draw a rectangle for each satellite
                    RectangleF BottomRect = new RectangleF(StartX, SatelliteY + TopRect.Height, BarWidth, TopRect.Height);
                    using (Brush BottomFillBrush = new LinearGradientBrush(BottomRect, bottomTopColor, bottomBottomColor, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(BottomFillBrush, BottomRect);
                    }

					using(Pen FillPen = new Pen(GetOutlineColor(satellite.SignalToNoiseRatio), 1.0f))
					{
						e.Graphics.DrawRectangle(FillPen, StartX, SatelliteY, BarWidth, StartY - SatelliteY);
					}
#endif
                    string PrnString = satellite.PseudorandomNumber.ToString(CultureInfo.CurrentCulture);
                    SizeF PrnSize = e.Graphics.MeasureString(PrnString, _PseudorandomNumberFont);
                    e.Graphics.DrawString(PrnString, _PseudorandomNumberFont,
                        _PseudorandomNumberBrush, (float)(StartX + (BarWidth * 0.5) - (PrnSize.Width * 0.5)), StartY);

                    string RenderString = satellite.SignalToNoiseRatio.ToString("0 dB", CultureInfo.CurrentCulture);
                    SizeF SignalSize = e.Graphics.MeasureString(RenderString, _SignalStrengthLabelFont);
                    if (SignalSize.Width > BarWidth)
                    {
                        RenderString = satellite.SignalToNoiseRatio.ToString("0 dB", CultureInfo.CurrentCulture).Replace(" dB", "");
                        SignalSize = e.Graphics.MeasureString(RenderString, _SignalStrengthLabelFont);
                    }
                    e.Graphics.DrawString(RenderString, _SignalStrengthLabelFont,
                        _SignalStrengthLabelBrush, (float)(StartX + (BarWidth * 0.5) - (SignalSize.Width * 0.5)), SatelliteY - SignalSize.Height);

                    StartX += _GapWidth;
                    StartX += BarWidth;
                }

            }
            catch (NullReferenceException)
            {
                // Don't throw because the control is shutting down
                
            }
            catch
            {
                throw;
            }
//            finally
//            {
//                if (SatellitesToDraw != null)
//                    SatellitesToDraw.Dispose();
//                if (SatellitesToRender != null)
//                    SatellitesToRender.Dispose();
//            }
        }

		private Color GetFillColor(SignalToNoiseRatio signal)
		{
			if (signal.Value < 10)
				return _FillNone[signal.Value];
			else if (signal.Value < 20)
				return _FillPoor[signal.Value - 10];
			else if (signal.Value < 30)
				return _FillModerate[signal.Value - 20];
			else if (signal.Value < 40)
				return _FillGood[signal.Value - 30];
			else if (signal.Value < 50)
				return _FillExcellent[signal.Value - 40];
			else
				return _FillExcellent[9];
		}

		private Color GetOutlineColor(SignalToNoiseRatio signal)
		{
			if (signal.Value < 10)
				return _OutlineNone[signal.Value];
			else if (signal.Value < 20)
				return _OutlinePoor[signal.Value - 10];
			else if (signal.Value < 30)
				return _OutlineModerate[signal.Value - 20];
			else if (signal.Value < 40)
				return _OutlineGood[signal.Value - 30];
			else if (signal.Value < 50)
				return _OutlineExcellent[signal.Value - 40];
			else
				return _OutlineExcellent[9];
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
				_FillNone.StartColor = value;
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
				_FillModerate.StartColor = value;
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
				_FillGood.StartColor = value;
				_FillModerate.EndColor = value;
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
				_FillExcellent.EndColor = value;
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
				_OutlineNone.StartColor = value;
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
				_OutlineNone.EndColor = value;
				_OutlinePoor.StartColor = value;
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
				_OutlinePoor.EndColor = value;
				_OutlineModerate.StartColor = value;
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
				_OutlineModerate.EndColor = value;
				_OutlineGood.StartColor = value;
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
				_OutlineExcellent.StartColor = value;
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
				return pSatelliteFixBrush.Color;
#else
                return _SatelliteFixColor;
#endif
			}
			set
			{
#if PocketPC
				if(pSatelliteFixBrush.Color.Equals(value))
					return;
				pSatelliteFixBrush.Color = value;
#else
                if (_SatelliteFixColor.Equals(value)) return;
                _SatelliteFixColor = value;
#endif
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
                // Has anything actually changed?
				if (_IsUsingRealTimeData == value) 
                    return;

				_IsUsingRealTimeData = value;
#if !DesignTime
				if (_IsUsingRealTimeData)
					_Satellites = Devices.Satellites;
#endif
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _SignalStrengthLabelFont;
			}
			set
			{
				if (_SignalStrengthLabelFont.Equals(value)) return;
				_SignalStrengthLabelFont = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
				return _SignalStrengthLabelBrush.Color;
			}
			set
			{
				if (_SignalStrengthLabelBrush.Color.Equals(value)) return;
				_SignalStrengthLabelBrush.Color = value;
				InvokeRepaint();
			}
		}

#if !PocketPC || DesignTime
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
		[Description("Controls the color used to draw the ID of each satellite.")]
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
				if (_PseudorandomNumberBrush.Color.Equals(value)) return;
				_PseudorandomNumberBrush.Color = value;
				InvokeRepaint();
			}
		}

		private void Devices_CurrentSatellitesChanged(object sender, SatelliteListEventArgs e)
		{
			if (_IsUsingRealTimeData)
			{
				//TODO should this be done here or assigned from a user defined handler for the event?
                Satellites = (List<Satellite>)e.Satellites;
                InvokeRepaint();
			}
		}
	}
}

