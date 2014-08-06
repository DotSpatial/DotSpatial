using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The Map Control for 2D applications.
    /// </summary>
    partial class MapCore : NSView
    {
        /// <summary>
        /// Public event advertising the mouse down
        /// </summary>
        public event EventHandler<GeoMouseArgs> GeoMouseDown;

        /// <summary>
        /// Public event advertising the mouse up
        /// </summary>
        public event EventHandler<GeoMouseArgs> GeoMouseUp;

        /// <summary>
        /// Public event advertising the mouse up
        /// </summary>
        public event EventHandler<GeoMouseArgs> GeoScrollWheel;

        /// <summary>
        /// Public event advertising the mouse down
        /// </summary>
        public event EventHandler<KeyEventArgs> GeoKeyDown;

        /// <summary>
        /// Public event advertising the mouse up
        /// </summary>
        public event EventHandler<KeyEventArgs> GeoKeyUp;

        private NSCursor macCursor = NSCursor.ArrowCursor;
        private Color backColor = Color.White;

        public void Invalidate()
        {
            SetNeedsDisplayInRect(((NSView)this).Bounds);
        }

        public void Invalidate(Rectangle clipRectangle)
        {
            SetNeedsDisplayInRect(new RectangleF(clipRectangle.X, 
                Height - (clipRectangle.Height + clipRectangle.Y),
                clipRectangle.Width, clipRectangle.Height));
        }

        public Cursor Cursor {
            get;
            set;
        }

        public Point PointToScreen(Point position)
        {
            return Point.Empty;
        }

        public Point PointToClient(Point position)
        {
            return Point.Empty;
        }

        public int Left
        {
            get{
                return 0;
            }
        }

        public int Top
        {
            get{
                return 0;
            }
        }

        public Color BackColor
        {
            set{
                backColor = value;
            }
            get{
                return backColor;
            }
        }

        public Rectangle Bounds
        {
            get{
                return new Rectangle ((int)Frame.X, (int)(this.Superview.Bounds.Height - Frame.Y),
                    (int)Frame.Width, (int)Frame.Height);
            }
        }

        public Rectangle ClientRectangle
        {
            get{
                return new Rectangle (0, 0, Width, Height);
            }
        }

        public int Height {
            get{
                return (int)((NSView)this).Bounds.Height;
            }
        }

        public int Width {
            get{
                return (int)((NSView)this).Bounds.Width;
            }
        }

        /// <summary>
        /// Cursor hiding from designer
        /// </summary>
        public NSCursor MacCursor {
            get{return macCursor;}
            set{
                if (macCursor != value) {
                    macCursor = value;
                    if(Window != null)
                        Window.InvalidateCursorRectsForView (this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current tool mode.  This rapidly enables or disables specific tools to give
        /// a combination of functionality.  Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        public FunctionMode FunctionMode
        {
            get { return _functionMode; }
            set
            {
                _functionMode = value;
                switch (_functionMode)
                {
                case FunctionMode.ZoomIn:
                    try
                    {
                        MacCursor = new NSCursor(ToNSImage(Images.cursorZoomIn), new PointF(6, 6));
                    }
                    catch
                    {
                        MacCursor = NSCursor.ArrowCursor;
                    }
                    break;
                case FunctionMode.ZoomOut:
                    try
                    {
                        MacCursor = new NSCursor(ToNSImage(Images.cursorZoomOut), new PointF(16, 15));
                    }
                    catch
                    {
                        MacCursor = NSCursor.ArrowCursor;
                    }
                    break;
                case FunctionMode.Info:
                    MacCursor = NSCursor.CrosshairCursor;
                    break;
                case FunctionMode.Label:
                    MacCursor = NSCursor.IBeamCursor;
                    break;
                case FunctionMode.None:
                    MacCursor = NSCursor.ArrowCursor;
                    break;
                case FunctionMode.Pan:
                    MacCursor = NSCursor.ClosedHandCursor;
                    break;
                case FunctionMode.Select:
                    MacCursor = NSCursor.PointingHandCursor;
                    break;
                }

                // Turn off functions that are not "Always on"
                if (_functionMode == FunctionMode.None)
                {
                    foreach (var f in MapFunctions)
                    {
                        if ((f.YieldStyle & YieldStyles.AlwaysOn) != YieldStyles.AlwaysOn) f.Deactivate();
                    }
                }
                else
                {
                    IMapFunction newMode = _functionLookup[_functionMode];
                    ActivateMapFunction(newMode);
                    // Except for function mode "none" allow scrolling
                    IMapFunction scroll = MapFunctions.Find(f => f.GetType() == typeof(MapFunctionZoom));
                    ActivateMapFunction(scroll);
                }

                //function mode changed event
                OnFunctionModeChanged(this, EventArgs.Empty);
            }
        }

        public Dictionary<FunctionMode, IMapFunction> FunctionLookup
        {
            get
            {
                return _functionLookup;
            }
            set
            {
                _functionLookup = value;
            }
        }

        public static NSImage ToNSImage(byte[] b)
        {
            NSData imageData = NSData.FromArray(b);
            return new NSImage(imageData);
        }

        /// <summary>
        /// This causes all of the data layers to re-draw themselves to the buffer, rather than just drawing
        /// the buffer itself like what happens during "Invalidate"
        /// </summary>
        public void Refresh()
        {
            MapFrame.Initialize();
            Invalidate();
        }

        public override void KeyUp(NSEvent theEvent)
        {
            KeyEventArgs e = new KeyEventArgs(ToKeys(theEvent.KeyCode));

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoKeyUp(e);
                }
            }

            var handler = GeoKeyUp;
            if (handler != null)
            {
                handler(this, e);
            }
            base.KeyDown (theEvent);
        }

        private Keys ToKeys(ushort KeyCode)
        {
            Keys key;
            switch (KeyCode)
            {
            case (ushort)NSKey.UpArrow:
                key = Keys.Up;
                break;
            case (ushort)NSKey.DownArrow:
                key = Keys.Down;
                break;
            case (ushort)NSKey.LeftArrow:
                key = Keys.Left;
                break;
            case (ushort)NSKey.RightArrow:
                key = Keys.Right;
                break;
            case 126:
                key = Keys.Up;
                break;
            case 125:
                key = Keys.Down;
                break;
            case 123:
                key = Keys.Left;
                break;
            case 124:
                key = Keys.Right;
                break;
            case (ushort)NSKey.PageUp:
                key = Keys.PageUp;
                break;
            case (ushort)NSKey.PageDown:
                key = Keys.PageDown;
                break;
            case (ushort)NSKey.Home:
                key = Keys.Home;
                break;
            case (ushort)NSKey.End:
                key = Keys.End;
                break;
            case 116:
                key = Keys.PageUp;
                break;
            case 121:
                key = Keys.PageDown;
                break;
            case 115:
                key = Keys.Home;
                break;
            case 119:
                key = Keys.End;
                break;
            case (ushort)NSKey.Space:
                key = Keys.Space;
                break;
            default:
                key = Keys.None;
                break;
            }
            return key;
        }

        public override void ResetCursorRects()
        {
            base.ResetCursorRects();
            AddCursorRect(((NSView)this).Bounds, macCursor);
        }

        public override bool AcceptsFirstResponder ()
        {
            return true;
        }

        public override void KeyDown(NSEvent theEvent)
        {
            KeyEventArgs e = new KeyEventArgs(ToKeys(theEvent.KeyCode));

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoKeyDown(e);
                }
            }

            var handler = GeoKeyDown;
            if (handler != null)
            {
                handler(this, e);
            }
            base.KeyDown (theEvent);
        }

        /// <summary>
        /// Fires the OnMouseUp event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        public override void MouseUp(NSEvent theEvent)
        {
            var LocationInView = ConvertPointFromView (theEvent.LocationInWindow, null);
            GeoMouseArgs e = new GeoMouseArgs(new MouseEventArgs (MouseButtons.Left, theEvent.ClickCount,
                (int)LocationInView.X, (int)(Height - LocationInView.Y), 0), this);

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseUp(e);
                    if (e.Handled) break;
                }
            }

            var handler = GeoMouseUp;
            if (handler != null)
            {
                handler(this, e);
            }
            base.MouseUp(theEvent);
        }

        /// <summary>
        /// Fires the OnMouseDown event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        public override void MouseDown(NSEvent theEvent)
        {
            var LocationInView = ConvertPointFromView (theEvent.LocationInWindow, null);
            GeoMouseArgs e = new GeoMouseArgs(new MouseEventArgs (MouseButtons.Left, theEvent.ClickCount,
                (int)LocationInView.X, (int)(Height - LocationInView.Y), 0), this);

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseDown(e);
                    if (e.Handled) break;
                }
            }

            var handler = GeoMouseDown;
            if (handler != null)
            {
                handler(this, e);
            }
            base.MouseDown(theEvent);
        }

        /// <summary>
        /// Fires the OnMouseMove event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        public override void MouseDragged(NSEvent theEvent)
        {
            var LocationInView = ConvertPointFromView (theEvent.LocationInWindow, null);
            GeoMouseArgs args = new GeoMouseArgs(new MouseEventArgs (MouseButtons.None, 0,
                (int)LocationInView.X, (int)(Height - LocationInView.Y), 0), this);

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseMove(args);
                    if (args.Handled) break;
                }
            }

            OnMouseMove(args);

            base.MouseDragged(theEvent);
        }

        /// <summary>
        /// Fires the OnMouseWheel event for the active tools
        /// </summary>
        /// <param name="e"></param>
        public override void ScrollWheel(NSEvent theEvent) 
        {
            var LocationInView = ConvertPointFromView (theEvent.LocationInWindow, null);
            GeoMouseArgs e = new GeoMouseArgs(new MouseEventArgs(MouseButtons.None, 0, 
                (int)LocationInView.X, (int)(Height - LocationInView.Y),
                (int)(theEvent.DeltaY*10)), this);

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseWheel (e);
                    if (e.Handled) break;
                }
            }

            var handler = GeoScrollWheel;
            if (handler != null)
            {
                handler(this, e);
            }
            base.ScrollWheel(theEvent);        
        }

        /// <summary>
        /// Perform custom drawing
        /// </summary>
        /// <param name="e"></param>
        public override void DrawRect(RectangleF dirtyRect)
        {
            if (MapFrame.IsPanning) return;

            var context = NSGraphicsContext.CurrentContext.GraphicsPort;
            Rectangle clip = new Rectangle((int)dirtyRect.X, 
                (int)(Height - (dirtyRect.Height + dirtyRect.Y)), 
                (int)dirtyRect.Width, (int)dirtyRect.Height);

            if (dirtyRect.IsEmpty)
                clip = ClientRectangle;

            // if the area to paint is too small, there's nothing to paint.
            // Added to fix http://dotspatial.codeplex.com/workitem/320
            if (clip.Width < 1 || clip.Height < 1) return;

            Bitmap stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(stencil);
            g.CompositingMode = CompositingMode.SourceCopy;

            Brush b = new SolidBrush(Color.White);
            g.FillRectangle(b, clip);
            b.Dispose();
            Matrix m = new Matrix();
            m.Translate(-clip.X, -clip.Y);
            g.Transform = m;

            PaintEventArgs e = new PaintEventArgs (g, clip);
            Draw(g, e);

            MapDrawArgs args = new MapDrawArgs(g, clip, MapFrame);
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled) tool.Draw(args);
            }

            g.Dispose();

            if(dirtyRect.IsEmpty)
                context.DrawImage(((NSView)this).Bounds, ToCGImage(stencil));
            else
                context.DrawImage(dirtyRect, ToCGImage(stencil));

            stencil.Dispose();
        }

        public static CGImage ToCGImage(Image img) 
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            img.Save(s, System.Drawing.Imaging.ImageFormat.Png);
            byte[] b = s.ToArray();
            CGDataProvider dp = new CGDataProvider(b,0,(int)s.Length);
            s.Flush();
            s.Close();
            CGImage img2 = CGImage.FromPNG(dp,null,false,CGColorRenderingIntent.Default);
            return img2;
        }

        public override void ViewDidEndLiveResize()
        {
            OnSizeChanged (this, new EventArgs());
            base.ViewDidEndLiveResize ();
        }
    }
}