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
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

#if !PocketPC
#endif

using System.Reflection;

namespace DotSpatial.Positioning.Forms
{
#if !PocketPC || DesignTime
#if Framework20

    /// <summary>
    /// Represents a base class for round controls painted using polar
    /// coordinates.
    /// </summary>
    [Obfuscation(Feature = "renaming", Exclude = false, ApplyToMembers = true)]
    [Obfuscation(Feature = "controlflow", Exclude = true, ApplyToMembers = true)]
    [Obfuscation(Feature = "stringencryption", Exclude = false, ApplyToMembers = true)]
#endif
    [Serializable]
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
        private bool _isInterpolationActive;
        [NonSerialized]
        private readonly Thread _interpolationThread;
        [NonSerialized]
        private readonly ManualResetEvent _interpolationWaitHandle = new ManualResetEvent(false);
        private readonly Interpolator _rotationInterpolator = new Interpolator(0, 0, 15, InterpolationMethod.CubicEaseOut);
        private int _interpolationIndex = 14;
        private readonly Object _interpolationSyncRoot = new object();
#endif

        private Angle _pRotation = Angle.Empty;
        private Azimuth _pOrigin = Azimuth.East;
        private PolarCoordinateOrientation _pOrientation = PolarCoordinateOrientation.Counterclockwise;
        private float _pMaximumR = 100.0f;
        private float _pCenterR;
#if !PocketPC
        private PolarControlEffect _effect = PolarControlEffect.None;
#endif

        #region Glass shadow

#if !PocketPC
        // Get a value which is darker or lighter
        Color _circleEdge;
        Color _circleBack;
        Color _circleCenter;
        Color _circleBright;
        readonly ColorBlend _glassShadowColorBlend = new ColorBlend();
        readonly ColorBlend _glassReflectionColorBlend = new ColorBlend();
        readonly GraphicsPath _glassShadowPath = new GraphicsPath();
        PathGradientBrush _glassShadowBrush;

        /// <summary>
        /// Glass reflection
        /// </summary>
        public LinearGradientBrush GlassReflectionBrush;
        /// <summary>
        /// Glass reflection rectangle
        /// </summary>
        public RectangleF GlassReflectionRectangle;
#endif

        #endregion

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected PolarControl()
            : this("DotSpatial.Positioning Polar Control (http://dotspatial.codeplex.com)")
        { }

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
            DoBackColorChanged(EventArgs.Empty);

            // Start the rotation interpolation thread
            _interpolationThread = new Thread(InterpolationLoop)
                                       {
                                           IsBackground = true,
                                           Name =
                                               "DotSpatial.Positioning Rotation Interpolation Thread (http://dotspatial.codeplex.com)"
                                       };

#if !Framework20
			InterpolationThread.Priority = ThreadPriority.Normal;
#endif

            // Start threading
            _isInterpolationActive = true;
            _interpolationThread.Start();
            // Let it start up
            Thread.Sleep(0);
#endif
        }

#if !PocketPC

        /// <summary>Cleans up any unmanaged GDI+ resources used during painting.</summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                // Get the interpolation thread out of a loop
                _isInterpolationActive = false;

                if (_interpolationThread != null)
                {
#if Framework20 && !PocketPC
                    if (!_interpolationWaitHandle.SafeWaitHandle.IsInvalid)
#else
					if (InterpolationWaitHandle.Handle != new IntPtr(-1))
#endif
                        _interpolationWaitHandle.Set();

                    // Abort the painting thread
                    //if (!InterpolationThread.Join(500))
                    {
                        try
                        {
                            _interpolationThread.Abort();
                        }
                        catch
                        {
                        }
                    }
                }

#if Framework20 && !PocketPC
                if (!_interpolationWaitHandle.SafeWaitHandle.IsInvalid)
#else
				if (InterpolationWaitHandle.Handle != new IntPtr(-1))
#endif
                {
                    try
                    {
                        _interpolationWaitHandle.Close();
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
                if (_glassShadowBrush != null)
                {
                    try
                    {
                        _glassShadowBrush.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
                if (_glassShadowPath != null)
                {
                    try
                    {
                        _glassShadowPath.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
#endif
            }
            finally
            {
                base.Dispose(disposing);
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
                return _effect;
            }
            set
            {
                if (_effect.Equals(value)) return;
                _effect = value;
                OnEffectChanged(_effect);
                InvokeRepaint();
            }
        }

#if !PocketPC

        private void DoBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            Color backgroundColor;
            if (BackColor.Equals(Color.Transparent))
            {
                backgroundColor = Parent != null ? Parent.BackColor : Color.White;
            }
            else
            {
                backgroundColor = BackColor;
            }

            // Get the hue of the background color
            float hue = backgroundColor.GetHue();
            float saturation = backgroundColor.GetSaturation();
            float bright = backgroundColor.GetBrightness();

            // Get a value which is darker or lighter
            _circleEdge = ColorFromAhsb(220, hue, saturation, bright * 0.2f);
            _circleBack = ColorFromAhsb(190, hue, saturation, bright * 0.4f);
            _circleCenter = ColorFromAhsb(210, hue, saturation, bright * 0.7f);
            _circleBright = ColorFromAhsb(250, hue, saturation, bright * 1.0f);

            #region Draw the faded edge of the circle to give spherical depth

            _glassShadowColorBlend.Colors = new[] { _circleEdge, _circleBack, _circleCenter, _circleCenter };
            _glassShadowColorBlend.Positions = new[] { 0.00F, 0.25f, 0.7f, 1.0f };

            #endregion

            #region Set the colors of the reflection

            // If the background color is black, make the class reflection white
            if (backgroundColor.Equals(Color.Black))
            {
                float whiteHue = Color.White.GetHue();
                float whiteSaturation = Color.White.GetSaturation();
                float whiteBright = Color.White.GetBrightness();
                _circleCenter = ColorFromAhsb(210, whiteHue, whiteSaturation, whiteBright * 0.7f);
                _circleBright = ColorFromAhsb(250, whiteHue, whiteSaturation, whiteBright * 1.0f);
                _glassReflectionColorBlend.Colors = new[] { _circleBright, Color.FromArgb(200, _circleCenter), Color.FromArgb(10, _circleCenter), Color.Transparent };
            }
            else
            {
                Color nearWhite = ColorFromAhsb(255, hue, saturation, 1.0f);

                _glassReflectionColorBlend.Colors = new[] { nearWhite, Color.FromArgb(200, _circleCenter), Color.FromArgb(10, _circleCenter), Color.Transparent };
            }

            _glassReflectionColorBlend.Positions = new[] { 0F, 0.15f, 0.35F, 1.0f };

            MakeBrushes();

            #endregion
        }

        /// <inheritdocs/>
        protected override void OnBackColorChanged(EventArgs e)
        {
            DoBackColorChanged(e);
        }

        /// <summary>
        /// Make Brushes
        /// </summary>
        public void MakeBrushes()
        {
            #region Glass shadow

            // If the rectangle is empty, skip
            if (ClientRectangle.IsEmpty)
                return;

            // Create a path which encapsulates the control.
            _glassShadowPath.Reset();
            _glassShadowPath.AddEllipse(ClientRectangle);

            // Dispose of any old brush
            if (_glassShadowBrush != null)
                _glassShadowBrush.Dispose();

            // Create a new brush
            _glassShadowBrush = new PathGradientBrush(_glassShadowPath);

            // And set its gradient colors
            if (_glassShadowColorBlend.Colors.Length > 1)
                _glassShadowBrush.InterpolationColors = _glassShadowColorBlend;

            #endregion

            #region Glass reflection

            // Dispose of any old brush
            if (GlassReflectionBrush != null)
                GlassReflectionBrush.Dispose();

            // Create the linear gradient
            GlassReflectionBrush = new LinearGradientBrush(ClientRectangle, Color.White, Color.White, 85, false);
            if (_glassReflectionColorBlend.Colors.Length > 1)
                GlassReflectionBrush.InterpolationColors = _glassReflectionColorBlend;

            // Set the rectangle for drawing the reflection
            GlassReflectionRectangle = new RectangleF(Convert.ToSingle(Width * 0.15), Convert.ToSingle(Height * 0.02),
                                Convert.ToSingle(Width * 0.7), Convert.ToSingle(Height * 0.58));

            #endregion
        }

#endif

        /// <summary>Occurs when the control's effect has changed.</summary>
        protected virtual void OnEffectChanged(PolarControlEffect effect)
        {
            switch (effect)
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
            if (RotationChanged != null)
                RotationChanged(this, new AngleEventArgs(rotation));
        }

        /// <summary>Occurs when the compass direction associated with 0° has changed.</summary>
        protected virtual void OnOriginChanged(Azimuth origin)
        {
            if (OriginChanged != null)
                OriginChanged(this, new AzimuthEventArgs(origin));
        }

        /// <summary>Occurs when the control's orientation has changed.</summary>
        protected virtual void OnOrientationChanged(PolarCoordinateOrientation orientation)
        {
            if (OrientationChanged != null)
                OrientationChanged(this, new PolarCoordinateOrientationEventArgs(orientation));
        }

        /// <summary>
        /// Create Graphics
        /// </summary>
        /// <returns></returns>
        public PolarGraphics CreatePolarGraphics()
        {
            return CreatePolarGraphics(OffScreenGraphics);
        }

        /// <summary>
        /// Create Polar Graphics
        /// </summary>
        /// <param name="graphics"></param>
        /// <returns></returns>
        public PolarGraphics CreatePolarGraphics(Graphics graphics)
        {
#if PocketPC
				return new PolarGraphics(graphics, new Rectangle(0, 0, Width, Height),
											  pRotation, pOrigin, pOrientation, pCenterR, pMaximumR);
#else
            return new PolarGraphics(graphics, RotationInterpolated, _pOrigin, _pOrientation, _pCenterR, _pMaximumR);
#endif
        }

#if !PocketPC

        /// <summary>Occurs when the control's background is painted.</summary>
        protected override void OnPaintOffScreenBackground(PaintEventArgs e)
        {
            // Call the base (which clears the screen)
            base.OnPaintOffScreenBackground(e);

            // And draw effects
            if (_effect == PolarControlEffect.Glass)
            {
                // Draw the faded edge
                e.Graphics.FillEllipse(_glassShadowBrush, ClientRectangle);
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
            if (_effect == PolarControlEffect.Glass)
            {
                // Draw a white reflection on the top
                e.Graphics.FillEllipse(GlassReflectionBrush, GlassReflectionRectangle);
            }
        }

#endif

#if !PocketPC || DesignTime

        /// <summary>Controls the amount of rotation applied to the entire control.</summary>
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
                return _pRotation;
            }
            set
            {
                if (_pRotation.Equals(value))
                    return;

                // Set the new rotation value
                _pRotation = value;

                // Exit if we're done
                if (IsDisposed)
                    return;
#if !PocketPC
                lock (_interpolationSyncRoot)
                {
                    // Get the current interpolated position
                    double current = _rotationInterpolator[_interpolationIndex];

                    // What is the winding direction?
                    bool isClockwise = (current > _rotationInterpolator.Minimum)
                        || (_rotationInterpolator.Minimum == 0
                        && _rotationInterpolator.Maximum == 0);

                    // Get a value 180 less than and 180 greater than the current value
                    double lowOpposite = current - 180.0;
                    double highOpposite = current + 180.0;

                    // Get the target value relative to the current value
                    double targetValue = value.Normalize().DecimalDegrees;

                    // Is the target value crossing a 0/360 degree boundary?
                    bool isNormalizationNeeded = false;
                    if (targetValue < lowOpposite)
                    {
                        targetValue += 360.0;
                        isNormalizationNeeded = true;
                    }
                    if (targetValue > highOpposite)
                    {
                        targetValue -= 360.0;
                        isNormalizationNeeded = true;
                    }

                    //Console.WriteLine("Current = {0}, Target = {1}", Current, TargetValue);

                    // Set final values
                    double finalTarget = targetValue;
                    double finalCurrent = current;

                    if (isNormalizationNeeded)
                    {
                        if (targetValue < 0)
                        {
                            finalCurrent += 360.0;
                            finalTarget += 360.0;
                        }
                        else if (targetValue > 360)
                        {
                            finalCurrent -= 360.0;
                            finalTarget -= 360.0;
                        }
                    }

                    // Is the new value within 180° clockwise of the current value?
                    if (value.DecimalDegrees <= current + 180.0)
                    {
                        // Yes. Is the interpolator already clockwise?
                        if (isClockwise)
                        {
                            // Yes.  Just set the new maximum
                            _rotationInterpolator.Maximum = finalTarget;
                        }
                        else
                        {
                            // No, we're changing direction.  Swap the values
                            _rotationInterpolator.Minimum = finalCurrent;
                            _rotationInterpolator.Maximum = finalTarget;
                            _interpolationIndex = 0;
                        }
                    }
                    else if (value.DecimalDegrees >= current - 180.0)
                    {
                        // Is the value within 180° counter-clockwise of the current value?
                        // Yes. Is the interpolator already counter-clockwise?
                        if (!isClockwise)
                        {
                            // Yes.  Just set the new minimum
                            _rotationInterpolator.Minimum = finalCurrent;
                            _rotationInterpolator.Maximum = finalTarget;
                        }
                        else
                        {
                            // Nope, we're changing direction
                            _rotationInterpolator.Minimum = finalCurrent;
                            _rotationInterpolator.Maximum = finalTarget;
                            _interpolationIndex = 0;
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
                _interpolationWaitHandle.Set();
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
                return new Angle(_rotationInterpolator[_interpolationIndex]);
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
                return _rotationInterpolator.InterpolationMethod;
            }
            set
            {
                _rotationInterpolator.InterpolationMethod = value;
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
            _rotationInterpolator.Count = framesPerSecond;
            // Adjust the index if it's outside of bounds
            if (_interpolationIndex > _rotationInterpolator.Count - 1)
                _interpolationIndex = _rotationInterpolator.Count - 1;
        }

        // Handles all interpolation
        private void InterpolationLoop()
        {
            // Flag that the thread is now alive
            //InterpolationThreadWaitHandle.Set();
            // Loop until tinterpolation is disabled
            while (_isInterpolationActive)
            {
                try
                {
                    // Wait for interpolation to actually be needed
                    _interpolationWaitHandle.WaitOne();
                    // If we're shutting down, just exit
                    if (!_isInterpolationActive)
                        break;
                    // Get the number of iterations
                    int count = _rotationInterpolator.Count;
                    // And loop through them all
                    while (_isInterpolationActive && _interpolationIndex < count - 1)
                    {
                        // Render the next value
                        InvokeRepaint();
                        // Bump up the counter
                        _interpolationIndex++;
                        // Wait for the next frame
                        Thread.Sleep(1000 / count);
                        Thread.Sleep(0);
                    }
                    // Reset interpolation figures
                    _rotationInterpolator.Minimum = _pRotation.DecimalDegrees;
                    _rotationInterpolator.Maximum = _pRotation.DecimalDegrees;
                    _interpolationIndex = 0;
                    // Reset the flag which started this iteration
                    _interpolationWaitHandle.Reset();
                }
                catch (ThreadAbortException)
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

#if !PocketPC || DesignTime

        /// <summary>
        /// Returns the compass direction which matches zero degrees.
        /// </summary>
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
                return _pOrigin;
            }
            set
            {
                if (_pOrigin.Equals(value))
                    return;

                _pOrigin = value;

                // Mark that rotation has changed
                OnOriginChanged(_pOrigin);

                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>Returns the radius corresponding to the edge of the control.</summary>
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
                return _pMaximumR;
            }
            set
            {
                if (_pMaximumR == value)
                    return;
                // Update the value
                _pMaximumR = value;
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
                InvokeRepaint();
            }
        }

#endif

#if !PocketPC || DesignTime

        /// <summary>Returns the radius corresponding to the center of the control.</summary>
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
                return _pCenterR;
            }
            set
            {
                if (_pCenterR == value) return;
                // Update the value
                _pCenterR = value;
                // And redraw everything
                InvokeRepaint();
            }
        }

#if !PocketPC || DesignTime

        /// <summary>
        /// Returns whether positive values are applied in a clockwise or counter-clockwise direction.
        /// </summary>
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
                return _pOrientation;
            }
            set
            {
                if (_pOrientation.Equals(value))
                    return;

                _pOrientation = value;

                // Mark that rotation has changed
                OnOrientationChanged(_pOrientation);

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