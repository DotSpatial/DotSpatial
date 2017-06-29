using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.ComponentModel;
#if !PocketPC
using System.Drawing.Text;
#endif
#if PocketPC
using DotSpatial.Positioning.Licensing;
#endif

namespace DotSpatial.Positioning.Drawing
{
    /// <summary>
    /// Represents a base class for designing flicker-free, multithreaded user
    /// controls.
    /// </summary>
    /// <remarks>
    /// 	<para>This powerful and versatile class provides a framework upon which to create
    ///     high-performance owner-drawn controls. Common rendering challenges such as
    ///     multithreading, thread synchronization, double-buffering, performance tuning,
    ///     platform tuning and animation are all handled by this class.</para>
    /// 	<para>Controls which inherit from this class perform all paint operations by
    ///     overriding the <strong>OnPaintOffScreen</strong> method, and optionally the
    ///     <strong>OnPaintOffScreenBackground</strong> and
    ///     <strong>OnPaintOffScreenAdornments</strong> methods. Controls which demand higher
    ///     performance should perform all rendering on a separate thread. This is done by
    ///     setting the <strong>IsPaintingOnSeparateThread</strong> property to
    ///     <strong>True</strong>. Since the actual painting of the control is handled by this
    ///     class, the <strong>OnPaint</strong> method should not be overridden.</para>
    /// 	<para>When all off-screen paint operations have completed, this class will copy the
    ///     contents of the off-screen bitmap to the on-screen bitmap which is visibly
    ///     displayed on the control. By deferring all rendering operations to another thread,
    ///     the user interface remains very responsive even during time-consuming paint
    ///     operations.</para>
    /// 	<para>Performance tuning is another major feature of this class. The
    ///     <strong>OptimizationMode</strong> property gives developers the ability to tune
    ///     rendering performance for animation speed, rendering quality, low CPU usage, or a
    ///     balance between all three.</para>
    /// 	<para>While thread synchronization has been implemented wherever possible in this
    ///     class, and the class is almost entirely thread-safe, some care should be taken when
    ///     accessing base <strong>Control</strong> properties from a separate thread. Even
    ///     basic properties like <strong>Visible</strong> can fail, especially on the Compact
    ///     Framework. For minimal threading issues, avoid reading control properties during
    ///     paint events.</para>
    /// 	<para>This class has been tuned to deliver the best performance on all versions of
    ///     the .NET Framework (1.0, 1.1 and 2.0) as well as the .NET Compact Framework (1.0
    ///     and 2.0). Compatibility is also managed internally, which simplifies the process of
    ///     porting controls to the Compact Framework.</para>
    /// </remarks>
    /// <example>
    ///     This example demonstrates how little code is required to use features like
    ///     double-buffering and multithreading. <strong>IsPaintingOnSeparateThread</strong>
    ///     enables multithreading, and all paint operations take place in the
    ///     <strong>OnPaintOffScreen</strong> method instead of <strong>OnPaint</strong>. To
    ///     prevent memory leaks, all GDI objects are disposed of during the
    ///     <strong>Dispose</strong> method.
    ///     <code lang="VB" title="[New Example]">
    /// Public Class MyControl 
    ///     Inherits DoubleBufferedControl
    ///     Dim MyBrush As New SolidBrush(Color.Blue)
    ///     
    ///     Sub New()
    ///         IsPaintingOnSeparateThread = True
    ///     End Sub
    ///     
    ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
    ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50))
    ///     End Sub
    ///     
    ///     Public Overrides Sub Dispose(ByVal disposing As Boolean)
    ///         MyBrush.Dispose()
    ///     End Sub
    /// End Class
    ///     </code>
    /// 	<code lang="CS" title="[New Example]">
    /// public class MyControl : DoubleBufferedControl
    /// {
    ///     SolidBrush MyBrush = new SolidBrush(Color.Blue);
    ///     
    ///     MyControl()
    ///     {
    ///         IsPaintingOnSeparateThread = true;
    ///     }
    ///  
    ///     protected override void OnPaintOffScreen(PaintEventArgs e)
    ///     {
    ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50));
    ///     }
    ///     
    ///     public override void Dispose(bool disposing)
    ///     {
    ///         MyBrush.Dispose();
    ///     }
    /// }
    ///     </code>
    /// </example>
#if !PocketPC || DesignTime
	[RefreshProperties(RefreshProperties.All)]
#endif    
#if PocketPC     
    #if DesignTime
	[ToolboxItemFilter("System.CF.Windows.Forms", ToolboxItemFilterType.Custom)]
	[ToolboxItemFilter("DotSpatial.Positioning.Controls", ToolboxItemFilterType.Allow)]
#endif
#if Framework20
#if !PocketPC
    [ToolboxItem(true)]
#endif
#endif
#endif
    public abstract class DoubleBufferedControl : Control
    {
        private Bitmap _OffScreenBitmap = new Bitmap(1, 1);
        internal Graphics _OffScreenGraphics;
        private Size _OffScreenBitmapSize = new Size(1, 1);
        private Bitmap _OnScreenBitmap = new Bitmap(1, 1);
		private object OffScreenSyncRoot = new object();
		private object OnScreenSyncRoot = new object();
		private bool _IsExceptionTextAllowed = true;
#if !PocketPC
        // Default to the highest quality
        private GraphicsSettings _GraphicsSettings = GraphicsSettings.Balanced; //.HighPerformance; //.HighQuality;
        private int _TargetFramesPerSecond;
#endif
#if PocketPC
        private Color _BackgroundColor = SystemColors.Window;
#elif Framework20
        private Color _BackgroundColor = Color.Transparent;
#else
		private Color _BackgroundColor = SystemColors.Control;
#endif
        internal int _Width;
        internal int _Height;
        private bool _IsTransparent;
        private bool _IsDisposed;
        private bool _IsPaintingOnSeparateThread;
        private AutoResetEvent RenderRequestWaitHandle = new AutoResetEvent(false);
        private ManualResetEvent PausedWaitHandle = new ManualResetEvent(true);
        private Thread PaintingThread;
		private bool _NeedNewBitmaps = true;
		private string _ThreadName;
		private ThreadPriority _ThreadPriority = ThreadPriority.Normal;

#if PocketPC
		private bool _IsUpperLeftCornerAdjusted;
		private bool _IsPaintingThreadAlive;
        private bool _IsHandleCreated;
#endif



        /// <summary>Occurs when an exception is thrown during off-screen painting operations.</summary>
        /// <remarks>
        /// 	<para>When the control is rendering on a separate thread, exceptions cannot be
        ///     caught by a regular <strong>Try..Catch</strong> statement. Exceptions are instead
        ///     channeled through this event. The control will also attempt to display exception
        ///     information on-screen to inform developers of the code which failed.</para>
        /// 	<para>It is important to capture this event or override the
        ///     <strong>OnPaintException</strong> method in order to be properly notified of
        ///     problems. Without doing this, the control could fail to paint properly yet give no
        ///     indication that there is a problem.</para>
        /// </remarks>
        /// <example>
        ///     This example hooks into the <strong>ExceptionOccurred</strong> event of a control
        ///     in order to handle painting exceptions. 
        ///     <code lang="VB" title="[New Example]">
        /// Public Class MyControl
        ///     Inherits DoubleBufferedControl
        ///     
        ///     Sub New()
        ///         ' Receive notifications of paint problems
        ///         AddHandler ExceptionOccurred, AddressOf HandleExceptions
        ///     End Sub
        ///     
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         ' Try to paint with a null Pen
        ///         e.Graphics.DrawRectangle(Nothing, Rectangle.Empty)
        ///     End Sub
        ///     
        ///     Private Sub HandleExceptions(ByVal sender As Object, ByVal e As ExceptionEventArgs)
        ///         ' Write the error to the Debug window
        ///         Debug.WriteLine(e.Exception.ToString())
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        /// {   
        ///     MyControl()
        ///     {
        ///         // Receive notifications of paint problems
        ///         ExceptionOccurred += new ExceptionEventHandler(HandleExceptions);
        ///     }
        ///     
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         // Try to paint with a null Pen. 
        ///         e.Graphics.DrawRectangle(null, Rectangle.Empty);
        ///     }
        ///     
        ///     private sub HandleExceptions(object sender, ExceptionEventArgs e)
        ///     {
        ///         // Write the error to the Console
        ///         Console.WriteLine(e.Exception.ToString());
        ///     }
        /// }
        ///     </code>
        /// </example>
        public event EventHandler<ExceptionEventArgs> ExceptionOccurred;
		/// <summary>
		/// Occurs when the control is to be redrawn in the background.
		/// </summary>
		/// <remarks>In the DoubleBufferedControl class, all painting occurs off-screen.  Then, the off-screen bitmap is quickly
		/// drawn on-screen when painting completes.  This event is called immediately after the PaintOffScreenBackground event.</remarks>
		public event EventHandler<PaintEventArgs> PaintOffScreen;
		/// <summary>
		/// Occurs when the control's background is to be redrawn before the main graphics of the control.
		/// </summary>
		/// <remarks>In the DoubleBufferedControl class, all painting occurs off-screen.  Then, the off-screen bitmap is quickly
		/// drawn on-screen when painting completes.   This event is called to paint any background graphics before the main elements of the
		/// control are drawn.  Some painting effects, such as glass or plastic, use this event along with PaintOffScreenAdornments
		/// to add annotations to the control.  This event is called immediately before the PaintOffScreen event.</remarks>
        public event EventHandler<PaintEventArgs> PaintOffScreenBackground;
		/// <summary>
		/// Occurs when additional graphics or annotations must be added to the control.
		/// </summary>
		/// <remarks>In the DoubleBufferedControl class, all painting occurs off-screen.  Then, the off-screen bitmap is quickly
		/// drawn on-screen when painting completes.   This event is called to paint any additional graphics after the main elements of the
		/// control are drawn.  Some painting effects, such as glass, adding text or logos, use this event to draw on the control.  This event is called immediately after the PaintOffScreen event.</remarks>
        public event EventHandler<PaintEventArgs> PaintOffScreenAdornments;

        /// <summary>Creates a new instance.</summary>        
        protected DoubleBufferedControl()
            : this("DotSpatial.Positioning Multithreaded Control (http://dotspatial.codeplex.com)")
        {}     

        /// <summary>Creates a new instance using the specified thread name.</summary>
        /// <param name="threadName">
        /// The name associated with the rendering thread when rendering is multithreaded.
        /// This name appears in the Output window when the thread exits and can be useful during
        /// debugging. The name should also contain a company or support URL.
        /// </param>
        protected DoubleBufferedControl(string threadName)
        {
            // Set the name of the thread to help with debugging
            _ThreadName = threadName;

			// Finally, move painting to its own thread
			IsPaintingOnSeparateThread = true;

#if !PocketPC
            // Set control rendering options for best performance
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.Selectable
                | ControlStyles.ResizeRedraw
                | ControlStyles.CacheText, true);
            SetStyle(ControlStyles.Opaque, false);

            // Use a high frames-per-second setting
			TargetFramesPerSecond = 90;

#if Framework20
            // Use "optimized" double-buffering on DF2 platform
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
#else
            // Use "regular" double-buffering on DF1.1 and DF1.0
			SetStyle(ControlStyles.DoubleBuffer, true);
#endif
            // Default to a transparent background
			BackColor = Color.Transparent;

            // Default to a square control, 200x200px
			Size = new Size(200, 200);
#else
            // Default to a smaller control, 100x100px
            Size = new Size(100, 100);

            // Is there a parent control?
            if (Parent != null)
            {
                // Yes.  Inherit the parent's background color
                BackColor = Parent.BackColor;
            }
            else
            {
                // Set the background color depending on the platform
                switch (Platform.HostPlatformID)
                {
                    case HostPlatformID.Desktop:
                        
                        // Use the "Window" system color
                        BackColor = SystemColors.Window;

                        // We don't need to adjust the upper-left corner of controls
                        _IsUpperLeftCornerAdjusted = false;
                        break;

                    case HostPlatformID.WindowsCE:

                        // Use the grey color
                        BackColor = Color.Silver;

                        // We need to adjust the upper-left corner of controls at run-time
                        _IsUpperLeftCornerAdjusted = !IsDesignMode;
                        break;

                    case HostPlatformID.PocketPC:
                    
                        // Use the "Window" system color
                        BackColor = SystemColors.Window;

                        // We need to adjust the upper-left corner of controls at run-time
                        _IsUpperLeftCornerAdjusted = !IsDesignMode;
                        break;

                    case HostPlatformID.Smartphone:
                        
                        // Use the "Window" system color
                        BackColor = SystemColors.Window;

                        // We don't need to adjust the upper-left corner of controls
                        _IsUpperLeftCornerAdjusted = false;
                        break;
                }
            }          
#endif

#if !PocketPC
            /* The following event sink is very important.  It appears that .NET
			 * has trouble trying to resolve assemblies, especially when TypeConverters
			 * are involved.  This event sink assists .NET with locating the proper
			 * assembly.
			 * 
			 * Without this event, strange "Invalid cast." and other design-time errors
			 * will happen!  So please leave this in.
			 */
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
#endif
        }

        /// <summary>
        /// Indicates whether the control is currently running inside of a Windows Forms designer.
        /// </summary>
        public bool IsDesignMode
        {
            get
            {
                return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }

#if (!PocketPC || DesignTime)
        /// <summary>Indicates if the control and its resources have been disposed.</summary>
        public new bool IsDisposed
        {
            get
            {
                return _IsDisposed;
            }
        }
#elif PocketPC 
        /// <summary>Indicates if the control and its resources have been disposed.</summary>
        public 
#if !Framework20
            new
#endif
            bool IsDisposed
        {
            get
            {
                return _IsDisposed;
            }
        }
#endif

        /// <summary>
        /// Allows the control to be repainted after a call to SuspendPainting.
        /// </summary>
        public void ResumePainting()
        {
            PausedWaitHandle.Set();

            // Lastly, redraw the control
            InvokeRepaint();
        }

        /// <summary>
        /// Temporarily pauses all painting operations.
        /// </summary>
        public void SuspendPainting()
        {
            PausedWaitHandle.Reset();
        }

        /// <summary>
        /// Indicates if all off-screen rendering takes place on a separate thread.
        /// </summary>
        /// <remarks>
        /// 	<para>This powerful property controls whether or not rendering operations are
        ///     multithreaded. When set to <strong>True</strong>, a new thread is launched and all
        ///     subsequent calls to <strong>OnPaintOffScreen</strong>,
        ///     <strong>OnPaintOffScreenBackground</strong> and
        ///     <strong>OnPaintOffScreenAdornments</strong> occur on that thread. Thread
        ///     synchronization features are enabled so that painting operations never interfere
        ///     with rendering operations. The priority of the rendering thread is controlled via
        ///     the <strong>ThreadPriority</strong> property.</para>
        /// 	<para>When this property is <strong>False</strong>, the rendering thread is torn
        ///     down and all rendering occurs on the owner's thread. Controls which perform
        ///     significant painting operations should enable this property to allow the user
        ///     interface to be more responsive. As a general rule, any intense processing should
        ///     be moved away from the user interface thread.</para>
        /// </remarks>
        /// <example>
        ///     This example instructs the control to perform all rendering on a separate thread.
        ///     Note that all thread management is handled automatically -- the only operation
        ///     required is enabling the property. 
        ///     <code lang="VB" title="[New Example]">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///     
        ///     Sub New()
        ///         ' Enable multithreading
        ///         IsPaintingOnSeparateThread = True
        ///     End Sub
        ///     
        ///     ' This method is now called from another thread
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         Dim MyBrush As New SolidBrush(Color.Blue)
        ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50))
        ///         MyBrush.Dispose()
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        /// {    
        ///     MyControl()
        ///     {
        ///         // Enable multithreading
        ///         IsPaintingOnSeparateThread = true;
        ///     }
        ///     
        ///     // This method is now called from another thread
        ///     protected overrides void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         SolidBrush MyBrush = new SolidBrush(Color.Blue);
        ///         e.Graphics.FillRectangle(MyBrush, new Rectangle(50, 50, 50, 50));
        ///         MyBrush.Dispose();
        ///     }
        /// }
        ///     </code>
        /// </example>
#if !PocketPC || DesignTime
        [Category("Performance")]
        [Description("Controls whether off-screen rendering takes place on a separate thread.  Good for demanding controls.")]
        [DefaultValue(typeof(bool), "False")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public bool IsPaintingOnSeparateThread
        {
            get
            {
                return _IsPaintingOnSeparateThread;
            }
            set
            {
                // Has the value actually changed?
                if (_IsPaintingOnSeparateThread == value) 
                    return;

                // If we were using a separate thread, tear it down
                if (!value)
                {
                    // We're no longer painting on another thread
                    _IsPaintingOnSeparateThread = false;

                    // Stop painting immediately
                    if (
                        // If we don't have a thread, skip
                        PaintingThread != null  
                        // If the thread is dead, skip
#if PocketPC
                        && _IsPaintingThreadAlive
#else
                        && PaintingThread.IsAlive
#endif
                        )
                    {
                        // Abort the thread
                        PaintingThread.Abort();
                    }
                }
                else
                {
                    // We're painting on another thread
                    _IsPaintingOnSeparateThread = true;

                    // Otherwise, start it up
                    PaintingThread = new Thread(new ThreadStart(PaintingThreadProc));

                    // Set the thread priority
                    PaintingThread.Priority = _ThreadPriority;

                    // Set the control name to assist with debugging
                    PaintingThread.Name = _ThreadName;

                    // The thread should immediately exit when the app exits
                    PaintingThread.IsBackground = true;

                    // Start the rendering thread
                    PaintingThread.Start();
                }
            }
        }

        /// <summary>
        /// Controls the upper-left portion of the off-screen bitmap to paint
        /// on-screen.
        /// </summary>
        /// <value>
        /// A <strong>Point</strong> structure indicating the corner of the off-screen bitmap
        /// to draw on-screen. Default is <strong>Empty</strong>.
        /// </value>
        /// <remarks>
        /// 	<para>If the size of the off-screen bitmap is different than the on-screen bitmap,
        ///     a control may need to draw different portions of the off-screen bitmap. For
        ///     example, if an off-screen bitmap is 200x200 pixels but the visible portion of the
        ///     control is only 50x50 pixels, an offset of (10,10) instructs the control to paint
        ///     the off-screen bitmap from (10,10)-(60,60).</para>
        /// 	<para>for most controls, this property does not need to be overridden. Controls
        ///     which override this property also override the <strong>OffScreenBitmapSize</strong>
        ///     property to specify a size defferent than the visible portion of the
        ///     control.</para>
        /// </remarks>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Controls the upper-left portion of the off-screen bitmap to paint on-screen.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        protected virtual Point OffScreenBitmapOffset
        {
            get
            {
                return Point.Empty;
            }
        }

        /// <summary>Returns the bitmap used for off-screen painting operations.</summary>
        /// <value>A <strong>Bitmap</strong> containing off-screen painted data.</value>
        /// <remarks>
        /// 	<para>This control maintains two separate bitmaps: an "off-screen" bitmap, where
        ///     all painting operations take place, and an "on-screen" bitmap which is displayed
        ///     visually to the user. When an off-screen painting operation completes successfully,
        ///     the off-screen bitmap is copies to the on-screen bitmap, then painted on the
        ///     display. This property returns the off-screen bitmap created during the most recent
        ///     paint iteration.</para>
        /// </remarks>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Returns the bitmap used for off-screen painting operations.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public Bitmap OffScreenBitmap
        {
            get
            {
                return _OffScreenBitmap;
            }
        }

        /// <summary>Controls the relative priority of multithreaded painting operations.</summary>
        /// <value>
        /// A <strong>ThreadPriority</strong> value. Default is
        /// <strong>Normal</strong>.
        /// </value>
        /// <remarks>
        /// Painting operations may require more CPU time if they represent the majority of
        /// a user interface, or if painting operations are more complicated. Performance can be
        /// improved by increasing the priority of the rendering thread. Likewise, if a control is
        /// of minor importance to an application, a lower thread priority can improve performance
        /// in more important areas of the application.
        /// </remarks>
        /// <example>
        ///     This example enables multithreaded painting then changes the priority of the
        ///     rendering thread to <strong>Lowest</strong> to give the rest of the application
        ///     more CPU time. 
        ///     <code lang="VB" title="[New Example]">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///     
        ///     Sub New()
        ///         ' Enable multithreading
        ///         IsPaintingOnSeparateThread = True
        ///         ' Set a low thread priority
        ///         ThreadPriority = ThreadPriority.Lowest
        ///     End Sub
        ///     
        ///     ' This method is now called from another thread
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///        ' ...etc.
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        /// {
        ///     MyControl()
        ///     {
        ///         // Enable multithreading
        ///         IsPaintingOnSeparateThread = true;
        ///         // Set a low thread priority
        ///         ThreadPriority = ThreadPriority.Lowest;
        ///     }
        ///     
        ///     // This method is now called from another thread
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///        // ...etc.
        ///     }
        /// }
        ///     </code>
        /// </example>
#if !PocketPC || DesignTime
		[Category("Performance")]
		[Description("Controls the relative priority of multithreaded painting operations.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(ThreadPriority.Normal)]        
#endif        
#if !PocketPC && Framework20
		[Obsolete("Changing thread priorty on Desktop Framework 2.0 can cause instability.  A setting of Normal is recommended for all threads.")]
#endif
		public ThreadPriority ThreadPriority
        {
            get
            {
                return _ThreadPriority;
            }
            set
            {
                if (_ThreadPriority == value) 
                    return;
                _ThreadPriority = value;
            }
        }

        //public new bool Visible
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}

        //        public new int Width
        //        {
        //            get
        //            {
        //                return ControlWidth;
        //            }
        //			set
        //			{
        //				base.Width = value;
        //				ControlWidth = value;
        //			}
        //        }
        //
        //#if !PocketPC
        //        [Category("Appearance")]
        //        [Description("Controls the width and height of the control.")]
        //        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //#endif
        //		public new Size Size
        //		{
        //			get
        //			{
        //				return new Size(ControlWidth, ControlHeight);
        //			}
        //			set
        //			{
        //				ControlWidth = value.Width;
        //				ControlHeight = value.Height;
        //				base.Size = value;
        //			}
        //		}
        //
        //        public new Rectangle ClientRectangle
        //        {
        //            get
        //            {
        //                return new Rectangle(0, 0, Width, Height);
        //            }
        //        }
        //
        //        public new Size ClientSize
        //        {
        //            get
        //            {
        //                return new Size(Width, Height);
        //            }
        //        }
        //
        //        public new int Height
        //        {
        //            get
        //            {
        //                return ControlHeight;
        //            }
        //			set
        //			{
        //				base.Height = value;
        //				ControlHeight = value;
        //			}
        //        }

        /// <summary>Controls the background color of the control.</summary>
        /// <value>A <strong>Color</strong> structure representing the background color.</value>
        /// <remarks>
        /// The default <strong>BackColor</strong> property of the <strong>Control</strong>
        /// class cannot be accessed from a thread other than the UI thread. As a result, this
        /// property was shadowed in order to make it thread-safe.
        /// </remarks>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Controls the color of any pixel not included in other rendering operations.")]
		#if PocketPC
				[DefaultValue(typeof(Color), "Window")]
		#elif Framework20
				[DefaultValue(typeof(Color), "Transparent")]
		#else
				[DefaultValue(typeof(Color), "Control")]
		#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
#endif
        public override Color BackColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                if (_BackgroundColor.Equals(value)) 
                    return;

                _BackgroundColor = value;
                _IsTransparent = _BackgroundColor.Equals(Color.Transparent);
                
#if !PocketPC
                // Notify of the change
                OnBackColorChanged(EventArgs.Empty);
#endif                
                // Repaint the control
                InvokeRepaint();
            }
        }

        /// <summary>
        /// Repaints the control either directly or via the painting thread.
        /// </summary>
        public void InvokeRepaint()
        {
            // Are we painting on a separate thread?
            if (_IsPaintingOnSeparateThread)
                // Yes.  Signal the thread
                RenderRequestWaitHandle.Set();
            else
                // No.  Repaint immediately
                Repaint();
        }

        /// <summary>
        /// Redraws the control off-screen.
        /// </summary>
        /// <returns></returns>
		public void Repaint()
		{
			try
			{
				if (
                    // If there's no handle, exit
                    !IsHandleCreated 
                    // If the control is disposed, exit
                    || IsDisposed 
                    // If the width is zero, exit
                    || _Width <= 0 
                    // If the height is zero, exit
                    || _Height <= 0 
                    // If the control is invisible, exit
                    || !Visible)
                {
					return;
                }

                // Prevent other threads from accessing the bitmap
                lock (OffScreenSyncRoot)
                {
                    // Do we need a new bitmap?
                    if (_NeedNewBitmaps)
                    {
                        // Does one exist now?
                        if (_OffScreenBitmap != null)
                        {
                            // Yes.  Dispose of it
                            if (_OffScreenGraphics != null)
                                _OffScreenGraphics.Dispose();

                            _OffScreenBitmap.Dispose();
                        }

                        // Create a new bitmap
#if PocketPC
					    _OffScreenBitmap = new Bitmap(_Width, _Height);
#else
                        _OffScreenBitmap = new Bitmap(_Width, _Height); //, PixelFormat.Format32bppPArgb);
#endif

                        // Create new graphics for the bitmap
                        _OffScreenGraphics = Graphics.FromImage(_OffScreenBitmap);

#if !PocketPC
                        // Apply graphics quality settings
                        _GraphicsSettings.Apply(_OffScreenGraphics);
#endif
                    }

                    // Make paint event arguments
                    PaintEventArgs f = new PaintEventArgs(_OffScreenGraphics, ClientRectangle);

                    // No.  Render the background
                    OnPaintOffScreenBackground(f);

                    // No. Render the main contents
                    OnPaintOffScreen(f);

                    // No, render adornments
                    OnPaintOffScreenAdornments(f);

                    // Swap the buffer
                    lock (OnScreenSyncRoot)
                    {
                        _OnScreenBitmap = _OffScreenBitmap.Clone() as Bitmap;
                    }
                }

				// And update the control
                if (InvokeRequired)
                    Invalidate();
                else
                    Refresh();
			}
#if !PocketPC
			catch (ThreadAbortException)
			{
				// Painting has been aborted. Shut down
				return;
			}
#endif
			catch (ObjectDisposedException)
			{
				// Painting is cancelled.  Shut down.
				return;
			}
			catch (Exception ex)
			{
				OnPaintException(ex);
				return;
			}
		}

        /// <summary>
        /// Controls whether exception messages are displayed on the control.
        /// </summary>
        /// <remarks>In some situations, an exception can occur during a paint operation.  To notify
        /// developers of the problem, the error text is written directly to the control.  This behavior
        /// may not be suitable for some developers, however, who want to trap errors more gracefully.
        /// Setting this property to False causes error messages to never be drawn on the control.  The
        /// ExceptionOccurred event should instead be used to gracefully handle errors when they occur.</remarks>
#if !PocketPC || DesignTime
		[Category("Performance")]
		[Description("Controls whether exception messages are drawn in the control, or suppressed.")]
		[DefaultValue(typeof(bool), "True")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
		public bool IsExceptionTextAllowed
        {
            get
            {
                return _IsExceptionTextAllowed;
            }
            set
            {
                _IsExceptionTextAllowed = value;
            }
        }

        /// <summary>Occurs when an exception occurs during a multithreaded paint operation.</summary>
        /// <remarks>
        /// 	<para>Exceptions caught during rendering iterations are channeled through this
        ///     method instead of being re-thrown. This allows developers the ability to silently
        ///     handle rendering problems without letting them interrupt the user interface. This
        ///     method invokes the <strong>ExceptionOccurred</strong> event.</para>
        /// </remarks>
        /// <example>
        ///     This example demonstrates how to be notified of rendering failures using the
        ///     <strong>OnPaintException</strong> method. 
        ///     <code lang="VB" title="[New Example]">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///     
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         ' Cause an error by using a null pen
        ///         e.Graphics.DrawRectangle(Nothing, Rectangle.Empty)
        ///     End Sub
        ///     
        ///     Protected Overrides Sub OnPaintException(ByVal e As ExceptionEventArgs)
        ///         Throw e.Exception
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        /// {
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         // Cause an error by using a null pen
        ///         e.Graphics.DrawRectangle(null, Rectangle.Empty);
        ///     }
        ///     
        ///     protected override void OnPaintException(ExceptionEventArgs e)
        ///     {
        ///         throw e.Exception;
        ///     }
        /// }
        ///     </code>
        /// </example>
        /// <param name="exception">An <strong>Exception</strong> object describing the rendering error.</param>
        protected virtual void OnPaintException(Exception exception)
        {
            try
            {
                // No.  If we have an existing one, release it
                if (_IsDisposed)
                    return;

                // Are we allowed to render the error?
                if (_IsExceptionTextAllowed)
                {
                    // No.  Keep going
                    lock (OffScreenSyncRoot)
                    {
                        _OffScreenBitmap.Dispose();
#if PocketPC
                        _OffScreenBitmap = new Bitmap(_Width, _Height);
#else
                        _OffScreenBitmap = new Bitmap(_Width, _Height, PixelFormat.Format32bppPArgb);
#endif
                        _OffScreenGraphics = Graphics.FromImage(_OffScreenBitmap);


                        // Set the rendering quality
                        _OffScreenGraphics.Clear(SystemColors.Control);
#if PocketPC
                            SolidBrush ControlBrush = new SolidBrush(SystemColors.ControlText);
                            using (Font ErrorFont = new Font("Tahoma", 7.0f, FontStyle.Regular))
                            {
                                _OffScreenGraphics.DrawString(exception.ToString(), ErrorFont, ControlBrush,
                                    new Rectangle(0, 0, _Width, _Height));
                            }
                            ControlBrush.Dispose();
#else
                        _OffScreenGraphics.DrawString(exception.ToString(), Font, SystemBrushes.ControlText, ClientRectangle);
#endif


                        lock (OnScreenSyncRoot)
                        {
                            // Release the old bitmap
                            _OnScreenBitmap.Dispose();

                            // And commit the new one
                            _OnScreenBitmap = _OffScreenBitmap.Clone() as Bitmap;
                        }
                    }
                }

                // And update the control
                Invalidate();

                // Notify of the exception
                if (ExceptionOccurred != null)
    				ExceptionOccurred(this, new ExceptionEventArgs(exception));
            }
            catch
            {
                // We must ignore subsequent errors to maintain stability
            }
        }

        /// <summary>Returns the width of the control in pixels.</summary>
        /// <value>An <strong>Integer</strong> indicating the width of the control in pixels.</value>
        /// <remarks>
        /// The default <strong>Width</strong> property of the <strong>Control</strong> class
        /// cannot be accessed from a thread other than the UI thread. As a result, this property
        /// was shadowed in order to make it thread-safe.
        /// </remarks>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Returns the horizontal length of the control in pixels.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public new int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (_Width.Equals(value)) return;
                _Width = value;
                base.Width = value;
            }
        }

#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Returns the horizontal and vertical length of the control in pixels.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]    
#endif
        public new Size Size
        {
            get
            {
                return new Size(_Width, _Height);
            }
            set
            {
                if (_Width.Equals(value.Width) && _Height.Equals(value.Height))
                    return;

                _Width = value.Width;
                _Height = value.Height;

                base.Size = value;
            }
        }

        /// <remarks>
        /// The default <strong>Height</strong> property of the <strong>Control</strong>
        /// class cannot be accessed from a thread other than the UI thread. As a result, this
        /// property was shadowed in order to make it thread-safe.
        /// </remarks>
        /// <summary>Returns the height of the control in pixels.</summary>
        /// <value>An <strong>Integer</strong> indicating the width of the control in pixels.</value>
#if !PocketPC || DesignTime
        [Category("Appearance")]
        [Description("Returns the vertical length of the control in pixels.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public new int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (_Height.Equals(value)) return;
                _Height = value;
                base.Height = value;
            }
        }

        private void PaintingThreadProc()
        {
#if PocketPC 
            try
            {
                // Signal that we're alive now
                _IsPaintingThreadAlive = true;
#endif

                // Keep rendering until we're stopping
                while (_IsPaintingOnSeparateThread)
                {
                    try
                    {
#if PocketPC || !Framework20
                        // Update the thread priority
                        if (PaintingThread.Priority != _ThreadPriority)
                            PaintingThread.Priority = _ThreadPriority;
#endif

                        // Wait for rendering
                        RenderRequestWaitHandle.WaitOne();

                        // If we've stopped rendering, exit
                        PausedWaitHandle.WaitOne();

                        // Redraw the control
                        Repaint();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Exit!
                        break;
                    }
#if !PocketPC || Framework20
                    catch (ThreadAbortException)
                    {
                        // Just exit!
                        break;
                    }
#endif
                    catch (Exception ex)
                    {
                        OnPaintException(ex);
                    }
                }
#if PocketPC
            }
            catch
            {
                // Ignore since we're on a separate thread
            }
            finally
            {             
                // Signal that we're alive now
                _IsPaintingThreadAlive = false;
            }
#endif
        }

#if PocketPC
        public bool IsHandleCreated
        {
            get { return _IsHandleCreated; }
        }
#endif

        protected override void OnHandleCreated(EventArgs e)
        {
            try
            {
                // Initialize the control
                base.OnHandleCreated(e);

#if PocketPC
                _IsHandleCreated = true;
#endif

                // Are we initialized?                
                OnInitialize();

                // And render
                Repaint();
            }
            catch (ThreadAbortException)
            {
                // Just exit!
            }
            catch (Exception ex)
            {
                OnPaintException(ex);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            try
            {
                // Shut down painting
                if (PaintingThread != null
#if PocketPC
                    && _IsPaintingThreadAlive 
#else
                    && PaintingThread.IsAlive
#endif
                    )
                {
                    PaintingThread.Abort();
                }
            }
            catch (ThreadAbortException)
            {
                // Just exit!
            }
            catch (Exception ex)
            {
                OnPaintException(ex);
            }
            finally
            {
                base.OnHandleDestroyed(e);
            }
        }

        /// <summary>Returns the bitmap used to paint the visible portion of the control.</summary>
        /// <value>A <strong>Bitmap</strong> containing data to be painted on the display.</value>
        /// <remarks>
        /// 	<para>This control maintains two separate bitmaps: an "off-screen" bitmap, where
        ///     all painting operations take place, and an "on-screen" bitmap which is displayed
        ///     visually to the user. When an off-screen painting operation completes successfully,
        ///     the off-screen bitmap is copies to the on-screen bitmap, then painted on the
        ///     display. This property returns the on-screen bitmap matching what is actually seen
        ///     in the control.</para>
        /// </remarks>
#if !PocketPC || DesignTime
		[Category("Appearance")]
        [Description("Returns the bitmap used to paint the visible area of the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
#endif
        public Bitmap OnScreenBitmap
        {
            get
            {
                return _OnScreenBitmap;
            }
        }

#if !PocketPC
        /// <summary>Controls the desired frame rate for rendering operations.</summary>
        /// <value>
        /// An <strong>Integer</strong> indicating the desired frame rate. Default is
        /// <strong>30</strong>.
        /// </value>
        /// <remarks>
        /// 	<para>This property is used by controls which make use of animation effects.
        ///     Typically, this property tells a control how long to delay between repaint
        ///     operations, or how many frames to allocate for a certain time period. Controls
        ///     which do not do any custom animation effects ignore this property and it has no
        ///     effect. When this property changes, the <strong>OnTargetFrameRateChanged</strong>
        ///     virtual method is called.</para>
        /// 	<para>If a control is able to repaint itself very quickly, smooth animation effects
        ///     can be achieved. A refresh rate of 30 frames per second or above is necessary to
        ///     give the human eye the illusion of motion. Refresh rates of 60 or even 120 are
        ///     possible for extremely fast controls and can result in very nice, smooth animation
        ///     effects.</para>
        /// 	<para>This property can be a bit confusing for developers because it does not
        ///     control the rate at which the control is actually repainted. (The control is only
        ///     repainted when necessary, whenever that occurs.) This property is used only to tune
        ///     custom effects implemented by controls inheriting from this class.</para>
        /// </remarks>
        /// <example>
        ///     This example animates a green rectangle. The default target frame rate is set to 60
        ///     frames per second, which causes the interval of a timer to change. The timer
        ///     changes the position of the rectangle. 
        ///     <code lang="VB" title="[New Example]">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///     Dim AnimatedRectangle As Rectangle
        ///     
        ///     Sub New()
        ///         ' Set a 60 frames per second target
        ///         TargetFramesPerSecond = 60
        ///     End Sub
        ///     
        ///     Protected Overrides Sub OnTargetFrameRateChanged()
        ///         ' Change the timer to fire 60 times per second
        ///         MyTimer.Interval = 1000 / TargetFramesPerSecond
        ///     End Sub
        ///     
        ///     Private Sub MyTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        ///         ' Change the location of an animated thing
        ///         AnimatedRectangle = New Rectangle((AnimatedRectangle.X + 1) Mod Width, 
        ///                                           (AnimatedRectangle.Y + 1) Mod Height, 
        ///                                           50, 50)
        ///         ' And cause the control to repaint
        ///         InvokeRepaint()
        ///     End Sub
        ///  
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         Dim MyBrush As New SolidBrush(Color.Green)
        ///         e.Graphics.FillRectangle(MyBrush, AnimatedRectangle)
        ///         MyBrush.Dispose()
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        ///     Rectangle AnimatedRectangle;
        ///     
        ///     MyControl()
        ///     {
        ///         // Set a 60 frames per second target
        ///         TargetFramesPerSecond = 60;
        ///     }
        ///     
        ///     protected override void OnTargetFrameRateChanged()
        ///     {
        ///         // Change the timer to fire 60 times per second
        ///         MyTimer.Interval = 1000 / TargetFramesPerSecond
        ///     }
        ///     
        ///     private void MyTimer_Tick(object sender, EventArgs e)
        ///     {
        ///         // Change the location of an animated thing
        ///         AnimatedRectangle = New Rectangle((AnimatedRectangle.X + 1) ^ Width, 
        ///                                           (AnimatedRectangle.Y + 1) ^ Height, 
        ///                                           50, 50);
        ///         // And cause the control to repaint
        ///         InvokeRepaint();
        ///     }
        ///  
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         SolidBrush MyBrush = new SolidBrush(Color.Green);
        ///         e.Graphics.FillRectangle(MyBrush, AnimatedRectangle);
        ///         MyBrush.Dispose();
        ///     }
        /// }
        ///     </code>
        /// </example>
        [Category("Performance")]
        [DefaultValue(typeof(int), "90")]
        [Description("Controls the preferred frames per second refresh rate for the control.  Animations at 30FPS or above will appear more fluid.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[NotifyParentProperty(true)]
        public int TargetFramesPerSecond
        {
            get
            {
                return _TargetFramesPerSecond;
            }
            set
            {
                if (_TargetFramesPerSecond == value) return;
                if (value <= 0)
                    throw new ArgumentException("The target frames per second must be above zero.");

                _TargetFramesPerSecond = value;
                OnTargetFrameRateChanged(_TargetFramesPerSecond);

                // Redraw the control
                Repaint();
            }
        }

        /// <summary>Occurs when the desired frame rate has changed.</summary>
        /// <remarks>
        /// This virtual method is called whenever the <strong>TargetFrameRate</strong>
        /// property has changed. This method gives controls the opportunity to adjust animation
        /// effects to achieve the frame rate as closely as possible.
        /// </remarks>
        /// <param name="framesPerSecond">
        /// An <strong>Integer</strong> specifying the ideal or desired frame rate for the
        /// control.
        /// </param>
        protected virtual void OnTargetFrameRateChanged(int framesPerSecond)
        {}

        /// <summary>
        /// Controls the graphics quality settings for the control.
        /// </summary>
        [Category("Quality")]
        [DefaultValue(typeof(GraphicsSettings), "HighQuality")]
        [Description("Controls the graphics quality settings for the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [NotifyParentProperty(true)]
        public GraphicsSettings GraphicsSettings
        {
            get { return _GraphicsSettings; }
            set
            {
                _GraphicsSettings = value;

                lock (OffScreenSyncRoot)
                {
                    _GraphicsSettings.Apply(_OffScreenGraphics);
                }
            }
        }

#endif

        /// <summary>Performs all major painting operations for the control.</summary>
        /// <remarks>
        /// 	<para>This method must be overridden. All painting operations for the control take
        ///     place during this method. After this method completes, the
        ///     <strong>OnPaintOffScreenAdornments</strong> method is called. When all painting
        ///     operations have completed, the task of repainting the control visually takes place
        ///     automatically.</para>
        /// 	<para>Developers seeking to upgrade their existing controls to use this control
        ///     must move all code from <strong>OnPaint</strong> into
        ///     <strong>OnPaintOffScreen</strong>. For best performance, the
        ///     <strong>OnPaint</strong> method would not be overridden at all.</para>
        /// </remarks>
        /// <param name="e">
        /// A <strong>CancelablePaintEventArgs</strong> object used for all painting
        /// operations, as well as to signal when a rendering iteration has been aborted.
        /// </param>
        protected virtual void OnPaintOffScreen(PaintEventArgs e)
        {
			if (PaintOffScreen != null)
				PaintOffScreen(this, e);
        }

        /// <summary>
        /// Optional. Performs painting operations which draw the control's
        /// background.
        /// </summary>
        /// <remarks>
        /// 	<para>This method provides the ability to prepare the background of the control
        ///     before major painting operations take place. By default, this method calls the
        ///     <strong>Graphics.Clear</strong> method to erase the control to the background
        ///     color.</para>
        /// 	<para>This method can be overridden to perform certain visual effects. For example,
        ///     the <strong>PolarControl</strong> class, which inherits from this class, uses this
        ///     method to apply a fading circular gradient to create a 3D illusion. Then, the
        ///     <strong>OnPaintOffScreenAdornments</strong> method is also overridden to apply a
        ///     second effect which gives the appearance of glass over the control. For more
        ///     information on glass effects, see the <strong>Effect</strong> property of the
        ///     <strong>PolarControl</strong> class.</para>
        /// </remarks>
        /// <param name="e">
        /// A <strong>CancelablePaintEventArgs</strong> object used for all painting
        /// operations, as well as to signal when a rendering iteration has been aborted.
        /// </param>
        protected virtual void OnPaintOffScreenBackground(PaintEventArgs e)
        {
            // Clear its background
            e.Graphics.Clear(_BackgroundColor);

			if (PaintOffScreenBackground != null)
				PaintOffScreenBackground(this, e);
        }

        /// <summary>
        /// Optional. Performs any additional painting operations after the main portions of
        /// the control are painted.
        /// </summary>
        /// <remarks>
        /// By default, this method does nothing. This method can be overridden to apply
        /// certain details, however, such as a watermark, company logo, or glass. For example, the
        /// <strong>PolarControl</strong> class overrides this method to apply a glass effect on
        /// top of anything that has already been painted. For more information on glass effects,
        /// see the <strong>Effect</strong> property of the <strong>PolarControl</strong>
        /// class.
        /// </remarks>
        /// <param name="e">
        /// A <strong>CancelablePaintEventArgs</strong> object used for all painting
        /// operations, as well as to signal when a rendering iteration has been aborted.
        /// </param>
        protected virtual void OnPaintOffScreenAdornments(PaintEventArgs e)
        {
			if (PaintOffScreenAdornments != null)
				PaintOffScreenAdornments(this, e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Update the control width (used for rendering on a separate thread)
            // For some reason, .NET barfs when trying to access Width or Height from a seaprate
            // thread.  Using these variables makes the property thread-safe.
            _Width = base.Width;
            _Height = base.Height;
            _NeedNewBitmaps = true;

            // Refresh the control
            InvokeRepaint();
        }

#if !PocketPC
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            // Suspend or resume rendering.  There's no need to keep
            // rendering if the control is invisible anyway
            if (!Visible)
                SuspendLayout();
            else
                ResumeLayout();
        }
#endif


#if PocketPC
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do nothing
        }
#endif

        /// <summary>
        /// Occurs when the control is about to be rendered for the first time.
        /// </summary>
        /// <remarks>
        /// 	<para>This virtual method provides the ability to prepare any local variables
        ///     before the control is painted for the first time. This method is typically used to
        ///     create GDI objects such as <strong>Pen</strong>, <strong>Brush</strong>, and
        ///     <strong>Font</strong> immediately before they are needed. It is recommended that
        ///     this event be used to create any such GDI objects. Additionally, it is also
        ///     recommended that GDI objects get created outside of the
        ///     <strong>OnPaintOffScreen</strong> method if they are used repeatedly.</para>
        /// 	<para>For desktop framework applications, this method is called when the control's
        ///     handle is created. For Compact Framework 1.0 applications, there is no handle
        ///     creation event, so this method is called when the first call to
        ///     <strong>OnPaint</strong> occurs.</para>
        /// </remarks>
        protected virtual void OnInitialize()
        {

        }

        /// <summary>Occurs when the control is to be redrawn on-screen.</summary>
        /// <remarks>
        /// 	<para>In the <strong>DoubleBufferedControl</strong> class, the process of painting
        ///     the control on-screen is handled automatically. As a result, this method does not
        ///     have to be overloaded to paint the control. In fact, this method should not be
        ///     overridden to perform any painting operations because it would defeat the purpose
        ///     of the control and re-introduce flickering problems.</para>
        /// 	<para>Ideally, well-written controls will move any and all painting operations from
        ///     the <strong>OnPaint</strong> method into the <strong>OnPaintOffScreen</strong>
        ///     method, and avoid overriding <strong>OnPaint</strong> entirely. By keeping the
        ///     <strong>OnPaint</strong> method as lightweight as possible, user interfaces remain
        ///     responsive and free from flickering problems.</para>
        /// </remarks>
        /// <example>
        /// 	<para>This example demonstrates how a user control is upgraded from Control to the
        ///     <strong>DoubleBufferedControl</strong> class. Upgrading is straightforward: All
        ///     painting operations are moved from <strong>OnPaint</strong> to
        ///     <strong>OnPaintOffScreen</strong> and <strong>OnPaint</strong> is no longer
        ///     overridden.</para>
        /// 	<code lang="VB" title="[New Example]" description="Before: A control's paint operations before being upgraded to use the DoubleBufferedControl class.">
        /// Public Class MyControl 
        ///     Inherits Control
        ///     Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        ///         Dim MyBrush As New SolidBrush(Color.Blue)
        ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50))
        ///         MyBrush.Dispose();
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]" description="Before: A control's paint operations before being upgraded to use the DoubleBufferedControl class.">
        /// public class MyControl : Control
        /// {
        ///     protected override void OnPaint(PaintEventArgs e)
        ///     {
        ///         SolidBrush MyBrush = new SolidBrush(Color.Blue);
        ///         e.Graphics.FillRectangle(MyBrush, new Rectangle(50, 50, 50, 50));
        ///         MyBrush.Dispose();
        ///     }
        /// }
        ///     </code>
        /// 	<code lang="VB" title="[New Example]" description="After: A control's paint operations after upgrading to DoubleBufferedControl.">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         Dim MyBrush As New SolidBrush(Color.Blue)
        ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50))
        ///         MyBrush.Dispose();
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        /// {
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         SolidBrush MyBrush = new SolidBrush(Color.Blue);
        ///         e.Graphics.FillRectangle(MyBrush, new Rectangle(50, 50, 50, 50));
        ///         MyBrush.Dispose();
        ///     }
        /// }
        ///     </code>
        /// </example>
        protected override void OnPaint(PaintEventArgs e)
        {
#if !PocketPC
            // Apply graphics settings
            GraphicsSettings.HighPerformance.Apply(e.Graphics);
#endif

            // Draw the control
            lock (OnScreenSyncRoot)
            {
#if PocketPC
                e.Graphics.DrawImage(_OnScreenBitmap, 0, 0);
#else
                e.Graphics.DrawImage(_OnScreenBitmap, Point.Empty);
#endif
            }
        }

#if PocketPC
		/// <summary>
		/// Controls whether bitmaps rendered on-screen are shifted slightly to correct display problems.
		/// </summary>
		/// <remarks>When Compact Framework 1.0 applications are run on newer PocketPC devices which
		/// support higher resolutions, a rendering bug can occur.  This bug, caused by the Compact Framework,
		/// causes bitmaps to be drawn in the wrong location, inaccurate by one pixel.  The end result is a 
		/// black "corner" in the upper-left of the control.  This property, when enabled, will correct
		/// the problem by scaling the bitmap one pixel larger during rendering.</remarks>
#if !PocketPC || DesignTime
		[Category("Layout")]
		[Description("Controls whether bitmaps rendered on-screen are shifted slightly to correct display problems on newer devices.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		#if PocketPC
			#if Smartphone
							[DefaultValue(typeof(bool), "False")]
			#elif Framework20
								[DefaultValue(typeof(bool), "False")]
			#else
								[DefaultValue(typeof(bool), "True")]
			#endif
		#endif
#endif
        public bool IsUpperLeftCornerAdjusted
		{
			get
			{
				return _IsUpperLeftCornerAdjusted;
			}
			set
			{
				if(_IsUpperLeftCornerAdjusted == value)
					return;
				_IsUpperLeftCornerAdjusted = value;

                InvokeRepaint();
			}
		}
#endif

        /// <summary>
        /// Indicates the point at the center of the control.
        /// </summary>
        /// <remarks>This property is typically used for centering items in the control.  This property
        /// is updated automatically as the control is resized.</remarks>
        /// <value>
        /// A <strong>Point</strong> structure representing the pixel at the center of the
        /// control.
        /// </value>
        /// <example>
        ///     This example uses the <strong>Center</strong> property to center a rectangle in the
        ///     middle of the control. 
        ///     <code lang="VB" title="[New Example]">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///  
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         ' Center a rectangle in the middle of the control
        ///         Dim MyShape As New Rectangle(Center.X - 25, Center.Y - 25, 50, 50)
        ///         ' Now paint it        
        ///         Dim MyBrush As New SolidBrush(Color.Green)
        ///         e.Graphics.FillRectangle(MyBrush, MyShape)
        ///         MyBrush.Dispose()
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        /// {
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         // Center a rectangle in the middle of the control
        ///         Rectangle MyShape = new Rectangle(Center.X - 25, Center.Y - 25, 50, 50);
        ///         // Now paint it        
        ///         SolidBrush MyBrush = new SolidBrush(Color.Green);
        ///         e.Graphics.FillRectangle(MyBrush, MyShape);
        ///         MyBrush.Dispose();
        ///     }
        /// }
        ///     </code>
        /// </example>
#if !PocketPC || DesignTime
		[Category("Behavior")]
		[Description("Returns the pixel coordinate located at the center of the control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
#endif
        public Point Center
        {
            get
            {
                return new Point((int)(_Width * 0.5), (int)(_Height * 0.5));
            }
        }

		~DoubleBufferedControl()
		{
			Dispose(true);
		}

        /// <summary>Releases memory and GDI resources created by the control.</summary>
        /// <remarks>
        /// 	<para>This method is very important to implement properly when working with
        ///     controls. Any <strong>Brush</strong>, <strong>Pen</strong>, <strong>Font</strong>,
        ///     <strong>Matrix</strong>, <strong>GraphicsPath</strong>, <strong>Region</strong> or
        ///     other GDI+ object containing a <strong>Dispose</strong> method must be disposed of
        ///     before they are destructed by the garbage collector.</para>
        /// 	<para>Well-written controls will create unmanaged GDI+ objects outside of the
        ///     <strong>OnPaintOffScreen</strong> method, then dispose of them during the
        ///     <strong>Dispose</strong> method. This practice improves performance by creating as
        ///     new new objects and resources as possible while minimizing problems which may occur
        ///     due to resources which are not properly released.</para>
        /// 	<para>Failure to call the <strong>Dispose</strong> method on GDI+ objects will
        ///     cause memory leaks which will slowly eat up available memory until the application
        ///     can no longer function. Use the "GDI Objects" column of the Windows Task Manager to
        ///     monitor the number of GDI objects allocated. Memory leaks will cause the GDI Object
        ///     count to increase continuously, whereas well-written controls will experience a GDI
        ///     Object count that remains fairly constant over a longer period of time.</para>
        /// 	<para>To view the GDI Objects column in the Windows Task Manager:</para>
        /// 	<para class="xmldocnumberedlist"></para>
        /// 	<list type="bullet">
        /// 		<item>Open the Windows Task Manager</item>
        /// 		<item>Select the "Processes" tab.</item>
        /// 		<item>Select "Choose Columns..." from the View menu.</item>
        /// 		<item>Check the "GDI Objects" box and click OK.</item>
        /// 	</list>
        /// </remarks>
        /// <example>
        ///     This example demonstrates how a control might create subtle problems when the
        ///     <strong>Dispose</strong> method is not used on every GDI+ object. 
        ///     <code lang="VB" title="[New Example]" description="An example of a poorly written control: A new SolidBrush is created on every paint iteration when it only needs to be created once. The brush is never disposed of, creating a memory leak.">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         Dim MyBrush As New SolidBrush(Color.Blue)
        ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50))
        ///     End Sub
        ///     
        ///     ' NOTE: MyBrush is never disposed.  A memory leak
        ///     ' will occur!
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]" description="An example of a poorly written control: A new SolidBrush is created on every paint iteration when it only needs to be created once. The brush is never disposed of, creating a memory leak.">
        /// public class MyControl : DoubleBufferedControl
        /// {
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         SolidBrush MyBrush = new SolidBrush(Color.Blue);
        ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50));
        ///     }
        ///     
        ///     // NOTE: MyBrush is never disposed.  A memory leak
        ///     // will occur!
        /// }
        ///     </code>
        /// 	<code lang="VB" title="[New Example]" description="Problems are solved by properly implementing the Dispose method. Performance is improved by moving the SolidBrush declaration out of OnPaintOffScreen.">
        /// Public Class MyControl 
        ///     Inherits DoubleBufferedControl
        ///     ' 1. GDI objects are created outside of the OnPaintOffScreen
        ///     '    methods whenever possible.
        ///     Dim MyBrush As New SolidBrush(Color.Blue)
        ///     
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         ' 2. The paint method is as lightweight as possible,
        ///         '    improving rendering performance
        ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50))
        ///     End Sub
        ///     
        ///     Public Overrides Sub Dispose(ByVal disposing As Boolean)
        ///         ' 3. Any GDI+ objects are disposed of properly    
        ///         MyBrush.Dispose()
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]" description="Problems are solved by properly implementing the Dispose method. Performance is improved by moving the SolidBrush declaration out of OnPaintOffScreen.">
        /// public class MyControl : DoubleBufferedControl
        /// {
        ///     // 1. GDI objects are created outside of the OnPaintOffScreen
        ///     //    methods whenever possible.
        ///     SolidBrush MyBrush = new SolidBrush(Color.Blue);
        ///     
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         // 2. The paint method is as lightweight as possible,
        ///         //    improving rendering performance
        ///         e.Graphics.FillRectangle(MyBrush, New Rectangle(50, 50, 50, 50));
        ///     }
        ///     
        ///     public override void Dispose(bool disposing)
        ///     {
        ///         // 3. Any GDI+ objects are disposed of properly    
        ///         MyBrush.Dispose();
        ///     }
        /// }
        ///     </code>
        /// </example>
        protected override void Dispose(bool disposing)
        {
            if (_IsDisposed)
                return;

            _IsDisposed = true;
            
			// No longer need to finalize
			GC.SuppressFinalize(this);

#if !PocketPC 
            // Unhook the event
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
#endif


            try
            {
                // If we're rendering, close down the thread
                IsPaintingOnSeparateThread = false;
            }
            catch
            {
            }          

            if (_OffScreenBitmap != null)
            {
                try
                {
                    _OffScreenBitmap.Dispose();
                }
                catch
                {
                    // Ignore
                }
            }

			if (_OnScreenBitmap != null)
			{
				try
				{
					_OnScreenBitmap.Dispose();
				}
				catch
				{
					// Ignore
				}
			}

#if PocketPC || !Framework20
			if (RenderRequestWaitHandle.Handle != new IntPtr(-1))
			{
				try
				{
					RenderRequestWaitHandle.Close();
				}
				catch (ObjectDisposedException)
				{
					// Ignore
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

#if !PocketPC
        /// <summary>
        /// Converts a color from HSB (hue, saturation, brightness) to RGB.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {

            //if (0 > a || 255 < a)
            //{
            //    return Color.Magenta;
            //    throw new ArgumentOutOfRangeException("a", a,
            //      Resources.InvalidAlpha);
            //}
            //if (0f > h || 360f < h)
            //{
            //    throw new ArgumentOutOfRangeException("h", h,
            //      Resources.InvalidHue);
            //}
            //if (0f > s || 1f < s)
            //{
            //    throw new ArgumentOutOfRangeException("s", s,
            //      Resources.InvalidSaturation);
            //}
            //if (0f > b || 1f < b)
            //{
            //    throw new ArgumentOutOfRangeException("b", b,
            //      Resources.InvalidBrightness);
            //}

            if (0 == s)
            {
                return Color.FromArgb(a, Convert.ToInt32(b * 255),
                  Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }
#endif

#if !PocketPC 
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AppDomain domain = (AppDomain)sender;

            foreach (Assembly asm in domain.GetAssemblies())
            {
                if (asm.FullName == args.Name)
                    return asm;
            }
            return null;
        }
#endif
	}

    /// <summary>Represents information about a cancelable paint iteration.</summary>
    /// <remarks>
    /// This class is used primarily by the <strong>OnPaintOffScreen</strong> method of
    /// the <strong>DoubleBufferedControl</strong> class when paint operations need to be
    /// performed. This class behaves the same as <strong>PaintEventArgs</strong>, but includes
    /// an extra <strong>IsCanceled</strong> property to indicate when a rendering iteration
    /// should be aborted.
    /// </remarks>
    //[CLSCompliant(false)]
    public sealed class CancelablePaintEventArgs : PaintEventArgs
    {
        private bool pCanceled;

        /// <summary>
        /// Creates a new instance using the specified <strong>Graphics</strong> object and
        /// clipping rectangle.
        /// </summary>
        /// <param name="graphics">
        /// A <strong>Graphics</strong> object used for all painting within the
        /// control.
        /// </param>
        /// <param name="clipRectangle">
        /// A <strong>Rectangle</strong> that defines the area that should be painted.
        /// Typically the size of the entire control.
        /// </param>
        public CancelablePaintEventArgs(Graphics graphics, Rectangle clipRectangle)
            : base(graphics, clipRectangle)
        {
        }

        /// <summary>Indicates if the painting operation should be completely aborted.</summary>
        /// <value>
        /// A <strong>Boolean</strong>, <strong>True</strong> if painting was aborted.
        /// Default is <strong>False</strong>.
        /// </value>
        /// <remarks>
        /// 	<para>This property is used by controls which allow their paint operations to be
        ///     cancelled. When set to True, the entire painting iteration is stopped and
        ///     restarted. This property is useful if a control always needs to display the very
        ///     latest information.</para>
        /// 	<para>Setting this property to <strong>True</strong> can have some undesirable
        ///     affects. For example, if a paint iteration is cancelled repeatedly, the control
        ///     will never get far enough in a paint operation to paint on-screen. Some care should
        ///     be taken when using this property.</para>
        /// </remarks>
        /// <example>
        ///     This example demonstrates how to write a cancelable paint operation. It's typically
        ///     a good idea to check for conditions which should cause a paint to cancel before
        ///     beginning a time-consuming painting task. In this case, the
        ///     <strong>IsPaintingAborted</strong> property is examined before entering a large
        ///     loop. <strong>IsPaintingAborted</strong> becomes <strong>True</strong> when a new
        ///     request to paint the control is made after starting the current paint iteration.
        ///     <code lang="VB" title="[New Example]">
        /// Public Class MyControl
        ///     Inherits DoubleBufferedControl
        ///     
        ///     Sub New()
        ///         IsPaintingOnSeparateThread = True
        ///     End Sub
        ///     
        ///     Protected Overrides Sub OnPaintOffScreen(ByVal e As CancelablePaintEventArgs)
        ///         ' Should painting be cancelled?
        ///         If IsPaintingAborted
        ///             ' Yes.  Abort all painting
        ///             e.IsCanceled = True
        ///             Exit Sub
        ///         End If
        ///             
        ///         ' Otherwise, A big paint operation begins
        ///         Dim Count As Integer
        ///         For Count = 1 To 20000
        ///             Dim MyBrush As New SolidBrush(Color.Green)
        ///             e.Graphics.DrawRectangle(MyBrush, New Rectangle(Count, Count, 5, 5))
        ///             MyBrush.Dispose()
        ///         Next Count
        ///     End Sub
        /// End Class
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// public class MyControl : DoubleBufferedControl
        /// {
        ///     MyControl()
        ///     {
        ///         IsPaintingOnSeparateThread = true;
        ///     }
        ///     
        ///     protected override void OnPaintOffScreen(PaintEventArgs e)
        ///     {
        ///         // Should painting be cancelled?
        ///         if(IsPaintingAborted)
        ///         {
        ///             // Yes.  Abort all painting
        ///             e.IsCanceled = true;
        ///             return;
        ///         }
        ///             
        ///         // Otherwise, A big paint operation begins
        ///         for(int Count = 1; Count &lt;= 20000; Count++)
        ///         {
        ///             SolidBrush MyBrush = new SolidBrush(Color.Green);
        ///             e.Graphics.DrawRectangle(MyBrush, new Rectangle(Count, Count, 5, 5));
        ///             MyBrush.Dispose();
        ///         }
        ///     }
        /// }
        ///     </code>
        /// </example>
        public bool Canceled
        {
            get
            {
                return pCanceled;
            }
            set
            {
                pCanceled = value;
            }
        }
    }
}
