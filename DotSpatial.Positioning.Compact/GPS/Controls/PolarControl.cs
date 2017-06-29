using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
#if !PocketPC
using System.Drawing.Text;
#endif
using DotSpatial.Positioning;
using System.Reflection;

namespace DotSpatial.Positioning.Drawing
{
	/// <summary>
	/// Represents a base class for round controls painted using polar
	/// coordinates.
	/// </summary>
#if !PocketPC || DesignTime
#if Framework20
    [Obfuscation(Feature = "renaming", Exclude = false, ApplyToMembers = true)]
    [Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    [Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
	[Serializable()]
	[RefreshProperties(RefreshProperties.All)]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
#endif
	public abstract class PolarControl : DoubleBufferedControl 
	{
#if !PocketPC       
		private bool IsInterpolationActive;
        [NonSerialized]
		private Thread InterpolationThread;
        [NonSerialized]
		private ManualResetEvent InterpolationWaitHandle = new ManualResetEvent(false);
		private Interpolator RotationInterpolator = new Interpolator(0, 0, 15, InterpolationMethod.CubicEaseOut);
		private int InterpolationIndex = 14;
		private Object InterpolationSyncRoot = new object();
#endif

		private Angle pRotation = Angle.Empty;
		private Azimuth pOrigin = Azimuth.East;
		private PolarCoordinateOrientation pOrientation = PolarCoordinateOrientation.Counterclockwise;
		private float pMaximumR = 100.0f;
		private float pCenterR;
#if !PocketPC
		private PolarControlEffect _Effect = PolarControlEffect.None;
#endif

        #region Glass shadow
#if !PocketPC
        // Get a value which is darker or lighter
        Color CircleEdge;
        Color CircleBack;
        Color CircleCenter;
        Color CircleBright;
        ColorBlend GlassShadowColorBlend = new ColorBlend();
        ColorBlend GlassReflectionColorBlend = new ColorBlend();
        GraphicsPath GlassShadowPath = new GraphicsPath();
        PathGradientBrush GlassShadowBrush;

        // Glass reflection
        public LinearGradientBrush GlassReflectionBrush;
        public RectangleF GlassReflectionRectangle;
#endif

        #endregion

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		protected PolarControl() 
            : this("DotSpatial.Positioning Polar Control (http://dotspatial.codeplex.com)")
		{}

        /// <summary>
        /// Creates a new instance using the specified thread name.
        /// </summary>
        /// <param name="threadName">A <strong>String</strong> representing the friendly name of the control.</param>
        /// <remarks>The thread name is dusplayed in the Visual Studio "Threads" debugging window.  Multithreaded debugging
        /// can be simplified when threads are clearly and uniquely named.</remarks>
		protected PolarControl(string threadName) 
            : base(threadName)
        {

#if !PocketPC
            OnBackColorChanged(EventArgs.Empty);
            
            // Start the rotation interpolation thread
			InterpolationThread = new Thread(new ThreadStart(InterpolationLoop));
			InterpolationThread.IsBackground = true;
			InterpolationThread.Name = "DotSpatial.Positioning Rotation Interpolation Thread (http://dotspatial.codeplex.com)";

#if !Framework20
			InterpolationThread.Priority = ThreadPriority.Normal;
#endif

			// Start threading
			IsInterpolationActive = true;
			InterpolationThread.Start();
			// Let it start up
			Thread.Sleep(0);
#endif
		}

#if !PocketPC
		/// <summary>Cleans up any unmanaged GDI+ resources used during painting.</summary>
		protected override void Dispose( bool disposing )
		{

			try
			{
				// Get the interpolation thread out of a loop 
				IsInterpolationActive = false;

				if(InterpolationThread != null)
				{
#if Framework20 && !PocketPC
                    if (!InterpolationWaitHandle.SafeWaitHandle.IsInvalid)
#else
					if(InterpolationWaitHandle.Handle != new IntPtr(-1))
#endif
						InterpolationWaitHandle.Set();

                    // Abort the painting thread
                    //if (!InterpolationThread.Join(500))
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

#if Framework20 && !PocketPC
                if (!InterpolationWaitHandle.SafeWaitHandle.IsInvalid)
#else
				if (InterpolationWaitHandle.Handle != new IntPtr(-1))
#endif
				{
					try
					{
						InterpolationWaitHandle.Close();
					}
					catch (ObjectDisposedException)
					{
					}
                }

#if !PocketPC
                if (GlassReflectionBrush != null)
                {
                    try
                    {
                        GlassReflectionBrush.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
                if (GlassShadowBrush != null)
                {
                    try
                    {
                        GlassShadowBrush.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
                if (GlassShadowPath != null)
                {
                    try
                    {
                        GlassShadowPath.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
#endif

            }
			catch
			{
				throw;
			}
			finally
			{
				base.Dispose( disposing );
			}
		}
#endif

		/// <summary>Occurs when the rotation amount has changed.</summary>
		public event EventHandler<AngleEventArgs> RotationChanged;
		/// <summary>Occurs when the compass direction associated with 0° has changed.</summary>
		public event EventHandler<AzimuthEventArgs> OriginChanged;
		/// <summary>Occurs when the control's coordinate orientation has changed.</summary>
		public event EventHandler<PolarCoordinateOrientationEventArgs> OrientationChanged;
		
#if !PocketPC 
		/// <summary>Controls the special painting effect applied to the control.</summary>
		[Category("Appearance")]
		[Description("Controls the special materials effect used to render the control.")]
        [DefaultValue(typeof(PolarControlEffect), "None")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public PolarControlEffect Effect
		{
			get
			{
				return _Effect;
			}
			set
			{
				if(_Effect.Equals(value)) return;
				_Effect = value;
				OnEffectChanged(_Effect);
				InvokeRepaint();
			}
		}

#if !PocketPC
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            Color BackgroundColor;
            if (BackColor.Equals(Color.Transparent))
            {
                if (Parent != null)
                    BackgroundColor = Parent.BackColor;
                else
                    BackgroundColor = Color.White;
            }
            else
            {
                BackgroundColor = BackColor;
            }

            // Get the hue of the background color
            float hue = BackgroundColor.GetHue();
            float saturation = BackgroundColor.GetSaturation();
            float bright = BackgroundColor.GetBrightness();
            float alpha = BackgroundColor.A;

            // Get a value which is darker or lighter
            CircleEdge = DoubleBufferedControl.ColorFromAhsb(220, hue, saturation, bright * 0.2f);
            CircleBack = DoubleBufferedControl.ColorFromAhsb(190, hue, saturation, bright * 0.4f);
            CircleCenter = DoubleBufferedControl.ColorFromAhsb(210, hue, saturation, bright * 0.7f);
            CircleBright = DoubleBufferedControl.ColorFromAhsb(250, hue, saturation, bright * 1.0f);

            #region Draw the faded edge of the circle to give spherical depth

            GlassShadowColorBlend.Colors = new Color[] { CircleEdge, CircleBack, CircleCenter, CircleCenter };
            GlassShadowColorBlend.Positions = new float[] { 0.00F, 0.25f, 0.7f, 1.0f };

            #endregion

            #region Set the colors of the reflection

            // If the background color is black, make the class reflection white
            if (BackgroundColor.Equals(Color.Black))
            {
                float WhiteHue = Color.White.GetHue();
                float WhiteSaturation = Color.White.GetSaturation();
                float WhiteBright = Color.White.GetBrightness();
                CircleCenter = DoubleBufferedControl.ColorFromAhsb(210, WhiteHue, WhiteSaturation, WhiteBright * 0.7f);
                CircleBright = DoubleBufferedControl.ColorFromAhsb(250, WhiteHue, WhiteSaturation, WhiteBright * 1.0f);
                GlassReflectionColorBlend.Colors = new Color[] { CircleBright, Color.FromArgb(200, CircleCenter), Color.FromArgb(10, CircleCenter), Color.Transparent };
            }
            else
            {
                Color NearWhite = DoubleBufferedControl.ColorFromAhsb(255, hue, saturation, 1.0f);

                GlassReflectionColorBlend.Colors = new Color[] { NearWhite, Color.FromArgb(200, CircleCenter), Color.FromArgb(10, CircleCenter), Color.Transparent };
            }

            GlassReflectionColorBlend.Positions = new float[] { 0F, 0.15f, 0.35F, 1.0f };

            MakeBrushes();

            #endregion
        }

        public void MakeBrushes()
        {

            #region Glass shadow

			// If the rectangle is empty, skip
			if(ClientRectangle.IsEmpty)
				return; 

            // Create a path which encapsulates the control.
            GlassShadowPath.Reset();
            GlassShadowPath.AddEllipse(ClientRectangle);

            // Dispose of any old brush
            if (GlassShadowBrush != null)
                GlassShadowBrush.Dispose();

            // Create a new brush
            GlassShadowBrush = new PathGradientBrush(GlassShadowPath);

            // And set its gradient colors
            if (GlassShadowColorBlend.Colors.Length > 1)
                GlassShadowBrush.InterpolationColors = GlassShadowColorBlend;

            #endregion

            #region Glass reflection

            // Dispose of any old brush
            if (GlassReflectionBrush != null)
                GlassReflectionBrush.Dispose();

            // Create the linear gradient
            GlassReflectionBrush = new LinearGradientBrush(ClientRectangle, Color.White, Color.White, 85, false);
            if (GlassReflectionColorBlend.Colors.Length > 1)
                GlassReflectionBrush.InterpolationColors = GlassReflectionColorBlend;

            // Set the rectangle for drawing the reflection
            GlassReflectionRectangle = new RectangleF(Convert.ToSingle(Width * 0.15), Convert.ToSingle(Height * 0.02),
                                Convert.ToSingle(Width * 0.7), Convert.ToSingle(Height * 0.58));
            #endregion


        }
#endif

		/// <summary>Occurs when the control's effect has changed.</summary>
		protected virtual void OnEffectChanged(PolarControlEffect effect)
		{
			switch(effect)
			{
                case PolarControlEffect.Glass:
                {
                    //BackColor = Color.Transparent;
                    MakeBrushes();
                    break;
                }
            case PolarControlEffect.None:
                {
                    //if (Parent != null)
                    //    BackColor = Parent.BackColor;
                    //else
                    //    BackColor = SystemColors.Control;
                    break;
                }
			}
		}
#endif

#if !PocketPC

		/// <summary>Occurs when the control's size has changed.</summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Remake the glass brushes
            MakeBrushes();

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(
                    new Rectangle(-1, -1, ClientRectangle.Width + 2, ClientRectangle.Height + 2));

                if (Region != null)
                    Region.Dispose();

                Region = new Region(path);
            }

        }
#endif

		/// <summary>Occurs when the control's rotation has changed.</summary>
		protected virtual void OnRotationChanged(Angle rotation)
		{
			if(RotationChanged != null)
				RotationChanged(this, new AngleEventArgs(rotation));
		}

		/// <summary>Occurs when the compass direction associated with 0° has changed.</summary>
		protected virtual void OnOriginChanged(Azimuth origin)
		{
			if(OriginChanged != null)
				OriginChanged(this, new AzimuthEventArgs(origin));
		}

		/// <summary>Occurs when the control's orientation has changed.</summary>
		protected virtual void OnOrientationChanged(PolarCoordinateOrientation orientation)
		{
			if(OrientationChanged != null)
				OrientationChanged(this, new PolarCoordinateOrientationEventArgs(orientation));
		}

		public PolarGraphics CreatePolarGraphics()
		{
			return CreatePolarGraphics(_OffScreenGraphics);
		}

		public PolarGraphics CreatePolarGraphics(Graphics graphics)
		{
			
#if PocketPC
				return new PolarGraphics(graphics, new Rectangle(0, 0, Width, Height), 
											  pRotation, pOrigin, pOrientation, pCenterR, pMaximumR);
#else
				return new PolarGraphics(graphics, RotationInterpolated, pOrigin, pOrientation, pCenterR, pMaximumR);
#endif
        }

#if !PocketPC

		/// <summary>Occurs when the control's background is painted.</summary>
        protected override void OnPaintOffScreenBackground(PaintEventArgs e)
		{
            // Call the base (which clears the screen)
            base.OnPaintOffScreenBackground(e);

            // And draw effects
			if(_Effect == PolarControlEffect.Glass)
			{
                // Draw the faded edge
                e.Graphics.FillEllipse(GlassShadowBrush, ClientRectangle);
            }
		}
#endif

#if !PocketPC
		/// <summary>
		/// Occurs when additional painting is performed on top of the control's main
		/// content.
		/// </summary>
        protected override void OnPaintOffScreenAdornments(PaintEventArgs e)
		{
			if(_Effect == PolarControlEffect.Glass)
			{
                // Draw a white reflection on the top
                e.Graphics.FillEllipse(GlassReflectionBrush, GlassReflectionRectangle);
            }
		}
#endif

		/// <summary>Controls the amount of rotation applied to the entire control.</summary>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Controls the amount of rotation applied to the entire control.")]
        [DefaultValue(typeof(Angle), "0")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public virtual Angle Rotation
        {
			get
			{
				return pRotation;
			}
			set
			{
				if(pRotation.Equals(value)) 
                    return;
				
                // Set the new rotation value
			    pRotation = value;

				// Exit if we're done
				if(IsDisposed)
					return;
#if !PocketPC
                lock (InterpolationSyncRoot)
                {                  
                    // Get the current interpolated position
                    double Current = RotationInterpolator[InterpolationIndex];

                    // What is the winding direction?
                    bool IsClockwise = (Current > RotationInterpolator.Minimum)
                        || (RotationInterpolator.Minimum == 0
                        && RotationInterpolator.Maximum == 0);

                    // Get a value 180 less than and 180 greater than the current value
                    double LowOpposite = Current - 180.0;
                    double HighOpposite = Current + 180.0;

                    // Get the target value relative to the current value
                    double TargetValue = value.Normalize().DecimalDegrees;

                    // Is the target value crossing a 0/360 degree boundary?
                    bool IsNormalizationNeeded = false;
                    if (TargetValue < LowOpposite)
                    {
                        TargetValue += 360.0;
                        IsNormalizationNeeded = true;
                    }
                    if (TargetValue > HighOpposite)
                    {
                        TargetValue -= 360.0;
                        IsNormalizationNeeded = true;
                    }

                    //Console.WriteLine("Current = {0}, Target = {1}", Current, TargetValue);

                    // Set final values
                    double FinalTarget = TargetValue;
                    double FinalCurrent = Current;

                    if (IsNormalizationNeeded)
                    {
                        if (TargetValue < 0)
                        {
                            FinalCurrent += 360.0;
                            FinalTarget += 360.0;
                        }
                        else if (TargetValue > 360)
                        {
                            FinalCurrent -= 360.0;
                            FinalTarget -= 360.0;
                        }
                    }

                    // Is the new value within 180° clockwise of the current value?
                    if (value.DecimalDegrees <= Current + 180.0)
                    {
                        // Yes. Is the interpolator already clockwise?
                        if (IsClockwise)
                        {
                            // Yes.  Just set the new maximum                            
                            RotationInterpolator.Maximum = FinalTarget;
                        }
                        else
                        {
                            // No, we're changing direction.  Swap the values
                            RotationInterpolator.Minimum = FinalCurrent;
                            RotationInterpolator.Maximum = FinalTarget;
                            InterpolationIndex = 0;
                        }
                    }
                    // Is the value within 180° counter-clockwise of the current value?
                    else if (value.DecimalDegrees >= Current - 180.0)
                    {
                        // Yes. Is the interpolator already counter-clockwise?
                        if (!IsClockwise)
                        {
                            // Yes.  Just set the new minimum
                            RotationInterpolator.Minimum = FinalCurrent;
                            RotationInterpolator.Maximum = FinalTarget;
                        }
                        else
                        {
                            // Nope, we're changing direction
                            RotationInterpolator.Minimum = FinalCurrent;
                            RotationInterpolator.Maximum = FinalTarget;
                            InterpolationIndex = 0;
                        }
                    }

                    //// Are we changing direction?
                    //if (pRotation.DecimalDegrees >= RotationInterpolator.Minimum
                    //    && pRotation.DecimalDegrees > RotationInterpolator[InterpolationIndex])
                    //{
                    //    // No.  Just set the new maximum
                    //    RotationInterpolator.Maximum = pRotation.DecimalDegrees;
                    //}
                    //else if (pRotation.DecimalDegrees < RotationInterpolator.Minimum)
                    //{
                    //    // We're changing directions, so stop then accellerate again
                    //    RotationInterpolator.Minimum = RotationInterpolator[InterpolationIndex];
                    //    RotationInterpolator.Maximum = pRotation.DecimalDegrees;
                    //    InterpolationIndex = 0;
                    //}
                    //else if (pRotation.DecimalDegrees > RotationInterpolator.Minimum
                    //    && pRotation.DecimalDegrees < RotationInterpolator[InterpolationIndex])
                    //{
                    //    // We're changing directions, so stop then accellerate again
                    //    RotationInterpolator.Minimum = RotationInterpolator[InterpolationIndex];
                    //    RotationInterpolator.Maximum = pRotation.DecimalDegrees;
                    //    InterpolationIndex = 0;
                    //}
                    //else if (pRotation.DecimalDegrees > RotationInterpolator.Maximum)
                    //{
                    //    // No.  Just set the new maximum
                    //    RotationInterpolator.Maximum = pRotation.DecimalDegrees;
                    //}
                }
            
				// And activate the interpolation timer
				InterpolationWaitHandle.Set();
#endif
				// Mark that rotation has changed
				OnRotationChanged(value);

                InvokeRepaint();
            }
        }

#if !PocketPC
		/// <summary>Returns the current amount of rotation during an animation.</summary>
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected virtual Angle RotationInterpolated
		{
			get
			{
				return new Angle(RotationInterpolator[InterpolationIndex]);
			}
		}

		/// <summary>Controls the acceleration and deceleration technique used during rotation.</summary>
        [Category("Layout")]
        [DefaultValue(typeof(InterpolationMethod), "CubicEaseOut")]
		[Description("Controls how the control smoothly transitions from one rotation to another.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public InterpolationMethod RotationInterpolationMethod
		{
			get
			{
				return RotationInterpolator.InterpolationMethod;
			}
			set
			{
				RotationInterpolator.InterpolationMethod = value;
			}
		}
#endif

		/// <summary>Rotates the entire control to the specified value.</summary>
        public void Rotate(Angle angle)
		{
			Rotation = new Angle(Rotation.DecimalDegrees + angle.DecimalDegrees);
		}

#if !PocketPC

		/// <summary>Occurs when the desired animation frame rate has changed.</summary>
		protected override void OnTargetFrameRateChanged(int framesPerSecond)
		{
			base.OnTargetFrameRateChanged(framesPerSecond);
			// Recalculate our things
			RotationInterpolator.Count = framesPerSecond;
			// Adjust the index if it's outside of bounds
			if(InterpolationIndex > RotationInterpolator.Count - 1)
				InterpolationIndex = RotationInterpolator.Count - 1;
		}

		// Handles all interpolation
		private void InterpolationLoop()
		{
            // Flag that the thread is now alive
            //InterpolationThreadWaitHandle.Set();
            // Loop until tinterpolation is disabled
			while(IsInterpolationActive)
			{
				try
				{
					// Wait for interpolation to actually be needed
					InterpolationWaitHandle.WaitOne();
					// If we're shutting down, just exit
					if (!IsInterpolationActive)
						break;
					// Get the number of iterations
					int Count = RotationInterpolator.Count;
					// And loop through them all
					while (IsInterpolationActive && InterpolationIndex < Count - 1)
					{
						// Render the next value
						InvokeRepaint();
						// Bump up the counter
						InterpolationIndex++;
						// Wait for the next frame
						Thread.Sleep(1000 / Count);
						Thread.Sleep(0);
					}
					// Reset interpolation figures
					RotationInterpolator.Minimum = pRotation.DecimalDegrees;
					RotationInterpolator.Maximum = pRotation.DecimalDegrees;
					InterpolationIndex = 0;
					// Reset the flag which started this iteration
					InterpolationWaitHandle.Reset();
				}
				catch(ThreadAbortException)
				{
					// Just exit!
					break;
				}
				catch
				{
					// Ignore.
				}
			}
			// Reset the flag which started this iteration
			//InterpolationThreadWaitHandle.Set();
		}
#endif

		/// <summary>
		/// Returns the compass direction which matches zero degrees.
		/// </summary>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Controls the compass direction associated with zero degrees.")]
        [DefaultValue(typeof(Azimuth), "0")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public virtual Azimuth Origin
        {
			get
			{
				return pOrigin;
			}
			set
			{
				if(pOrigin.Equals(value))
					return;

				pOrigin = value;

				// Mark that rotation has changed
				OnOriginChanged(pOrigin);

                InvokeRepaint();

			}
		}

		/// <summary>Returns the radius corresponding to the edge of the control.</summary>
#if !PocketPC || DesignTime
		[Category("Layout")]
		[Description("Controls the value of R (radius) at the edge of the control.  Almost always 100.")]
        [DefaultValue(typeof(double), "100.0")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public float MaximumR
        {
			get
			{
				return pMaximumR;
			}
			set
			{
				if(pMaximumR == value) 
					return;
				// Update the value
				pMaximumR = value;
				// And redraw everything
				InvokeRepaint();
			}
		}

#if !PocketPC
		/// <summary>Controls the background color of the control.</summary>
		[Category("Appearance")]
        [Description("Controls the background color of the control, and whether or not it is transparent.")]
        [DefaultValue(typeof(Color), "Transparent")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;

                //// Dispose the existing region
                //if(Region != null)
                //    Region.Dispose();
                //// Make the control transparent
                //if (BackColor.Equals(Color.Transparent))
                //{
                //    using(GraphicsPath path = new GraphicsPath())
                //    {
                //        path.AddEllipse(0, 0, Width, Height);
                //        Region = new Region(path);
                //    }
                //}
                //else
                //{
                //    Region = new Region(new Rectangle(0, 0, Width, Height));
                //}
				InvokeRepaint();
            }
        }
#endif

		/// <summary>Returns the radius corresponding to the center of the control.</summary>
#if !PocketPC || DesignTime
		[Category("Layout")]
		[Description("Controls the value of R (radius) at the center of the control.  Almost always zero.")]
        [DefaultValue(typeof(double), "0")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public float CenterR
		{
			get
			{
				return pCenterR;
			}
			set
			{
				if(pCenterR == value) return;
				// Update the value
				pCenterR = value;
				// And redraw everything
				InvokeRepaint();
			}
		}

		/// <summary>
		/// Returns whether positive values are applied in a clockwise or counter-clockwise direction.
		/// </summary>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Controls the winding direction of positive angles during drawing operations.")]
        [DefaultValue(typeof(PolarCoordinateOrientation), "Clockwise")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public virtual PolarCoordinateOrientation Orientation
		{
			get
			{
				return pOrientation;
			}
			set
			{
				if(pOrientation.Equals(value))
					return;

				pOrientation = value;

				// Mark that rotation has changed
				OnOrientationChanged(pOrientation);

                InvokeRepaint();
			}
		}
	}


	/// <summary>Indicates the special effect applied to polar controls during painting.</summary>
	public enum PolarControlEffect
	{
		/// <summary>No effect is applied.</summary>
		None,
		/// <summary>
		/// Additional painting is performed during
		/// <strong>OnPaintOffScreenBackground</strong> and
		/// <strong>OnPaintOffScreenAdornments</strong> to give the appearance of lighting and
		/// glass.
		/// </summary>
		Glass
	}
}
