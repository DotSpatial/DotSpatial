using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using SelectionMode = DotSpatial.Symbology.SelectionMode;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;

namespace DotSpatial.Controls.MonoMac
{
    /// <summary>
    /// The Map Control for 2D applications.
    /// </summary>
    public class Map : NSView, IMap
    {
        #region MonoMac Code

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

        private NSCursor _nsCursor = NSCursor.ArrowCursor;
        private Color _backColor = Color.White;
        private Rectangle _bounds = Rectangle.Empty;

        public Map()
        {
            _mapCore = new MapCore(this);
            Configure();
            MapFunctions.Add(new MapFunctionZoom (this));
            OnSizeChanged (this, new EventArgs());
        }

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
                _backColor = value;
            }
            get{
                return _backColor;
            }
        }

        public Rectangle Bounds
        {
            get{
                _bounds.Height = (int)Frame.Height;
                _bounds.Width = (int)Frame.Width;
                _bounds.X = (int)Frame.X;
                _bounds.Y = (int)(this.Superview.Bounds.Height - Frame.Y);
                return _bounds;
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
        public NSCursor nsCursor {
            get{return _nsCursor;}
            set{
                if (_nsCursor != value) {
                    _nsCursor = value;
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
                        nsCursor = new NSCursor(ToNSImage(Images.cursorZoomIn), new PointF(6, 6));
                    }
                    catch
                    {
                        nsCursor = NSCursor.ArrowCursor;
                    }
                    break;
                case FunctionMode.ZoomOut:
                    try
                    {
                        nsCursor = new NSCursor(ToNSImage(Images.cursorZoomOut), new PointF(16, 15));
                    }
                    catch
                    {
                        nsCursor = NSCursor.ArrowCursor;
                    }
                    break;
                case FunctionMode.Info:
                    nsCursor = NSCursor.CrosshairCursor;
                    break;
                case FunctionMode.Label:
                    nsCursor = NSCursor.IBeamCursor;
                    break;
                case FunctionMode.None:
                    nsCursor = NSCursor.ArrowCursor;
                    break;
                case FunctionMode.Pan:
                    nsCursor = NSCursor.ClosedHandCursor;
                    break;
                case FunctionMode.Select:
                    nsCursor = NSCursor.PointingHandCursor;
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
                    IMapFunction newMode = FunctionLookup[_functionMode];
                    this.ActivateMapFunction(newMode);
                    // Except for function mode "none" allow scrolling
                    IMapFunction scroll = MapFunctions.Find(f => f.GetType() == typeof(MapFunctionZoom));
                    this.ActivateMapFunction(scroll);
                }

                //function mode changed event
                OnFunctionModeChanged(this, EventArgs.Empty);
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
            AddCursorRect(((NSView)this).Bounds, nsCursor);
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
            if(!InLiveResize && _mapCore.oldSize != Bounds.Size)
                _mapCore.oldSize = Bounds.Size;

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

        #endregion

        #region Protected Variables

        private IMapFrame _geoMapFrame;
        private int _isBusyIndex;
        private ILegend _legend;
        private IProgressHandler _progressHandler;
        private FunctionMode _functionMode;
        private MapCore _mapCore;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not
        /// the drawing layers should cache off-screen data to
        /// the buffer.  Panning will be much more elegant,
        /// but zooming, selecting and resizing will take a
        /// performance penalty.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets a boolean that indicates whether or not zooming will also retrieve content immediately around the map so that panning pulls new content onto the screen.")]
        public bool ExtendBuffer
        {
            get { return MapFrame.ExtendBuffer; }
            set { MapFrame.ExtendBuffer = value; }
        }

        /// <summary>
        /// Gets or sets the Projection Esri string of the map. This property is used for serializing
        /// the projection string to the project file.
        /// </summary>
        [Serialize("ProjectionEsriString")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string ProjectionEsriString
        {
            get
            {
                return Projection != null ? Projection.ToEsriString() : null;
            }
            set
            {
                if (Projection != null)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        Projection = null;
                    }
                    else
                    {
                        if (Projection.ToEsriString() != value)
                        {
                            Projection = ProjectionInfo.FromEsriString(value);
                            OnProjectionChanged();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a PromptMode enumeration that controls how users are prompted before adding layers
        /// that have a coordinate system that is different from the map.
        /// </summary>
        public ActionMode ProjectionModeReproject
        {
            get
            {
                if (_geoMapFrame != null) return MapFrame.ProjectionModeReproject;
                return ActionMode.Never;
            }
            set
            {
                if (_geoMapFrame != null) _geoMapFrame.ProjectionModeReproject = value;
            }
        }

        /// <summary>
        /// Gets or sets a PromptMode enumeration that controls how users are prompted before adding layers
        /// that have a coordinate system that is different from the map.
        /// </summary>
        public ActionMode ProjectionModeDefine
        {
            get
            {
                if (_geoMapFrame != null) return MapFrame.ProjectionModeDefine;
                return ActionMode.Never;
            }
            set
            {
                if (_geoMapFrame != null) _geoMapFrame.ProjectionModeDefine = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether layers should draw during the actual resize itself.  The
        /// normal behavior is to draw the existing image buffer in the new size and position which is much
        /// faster for large datasets, but is not as visually appealing if you only work with small datasets.
        /// </summary>
        public bool RedrawLayersWhileResizing { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not the sel
        /// </summary>
        public bool SelectionEnabled
        {
            get
            {
                if (MapFrame == null) return false;
                return MapFrame.SelectionEnabled;
            }
            set
            {
                if (MapFrame == null) return;
                MapFrame.SelectionEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether the Garbage collector should collect after drawing.
        /// This can be disabled for fast-action panning, but should normally be enabled.
        /// </summary>
        public bool CollectAfterDraw { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<FunctionMode, IMapFunction> FunctionLookup { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of tools built into this project
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<IMapFunction> MapFunctions { get; set; }

        /// <summary>
        /// Gets or sets a boolean that indicates whether a map-function is currently interacting with the map.
        /// If this is true, then any tool-tip like popups or other mechanisms that require lots of re-drawing
        /// should suspend themselves to prevent conflict.  Setting this actually increments an internal integer,
        /// so when that integer is 0, the map is "Not" busy, but multiple busy processess can work independantly.
        /// </summary>
        public bool IsBusy
        {
            get { return (_isBusyIndex > 0); }
            set
            {
                if (value) _isBusyIndex++;
                else _isBusyIndex--;
                if (_isBusyIndex <= 0)
                {
                    _isBusyIndex = 0;
                }
            }
        }

        /// <summary>
        /// If true then the map is zoomed to its full extents
        /// Added by Eric Hullinger 1/3/2013
        /// </summary>
        public bool IsZoomedToMaxExtent { get; set; }

        /// <summary>
        /// Gets or sets the back buffer.  The back buffer should be in Format32bbpArgb bitmap.
        /// If it is not, then the image on the back buffer will be copied from the supplied image.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BufferedImage
        {
            get { return _geoMapFrame.BufferImage; }
            set { _geoMapFrame.BufferImage = value; }
        }

        /// <summary>
        /// If this is true, then point layers in the map will only draw points that are
        /// more than 50% revealed.  This should increase drawing speed for layers that have
        /// a large number of points.
        /// </summary>
        public bool CollisionDetection { get; set; }

        /// <summary>
        /// Gets the geographic bounds of all of the different data layers currently visible on the map.
        /// </summary>
        [Category("Bounds"),
            Description("Gets the geographic bounds of all of the different data layers currently visible on the map."),
            Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Extent Extent
        {
            get { return _geoMapFrame.Extent; }
        }

        /// <summary>
        /// Gets or sets the geographic extents to show in the view.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Extent ViewExtents
        {
            get { return _geoMapFrame.ViewExtents; }
            set
            {
                _geoMapFrame.ViewExtents = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the MapFrame that should be displayed in this map.
        /// </summary>
        [Serialize("MapFrame"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapFrame MapFrame
        {
            get { return _geoMapFrame; }
            set
            {
                var proj = Projection;
                OnExcludeMapFrame(_geoMapFrame);
                _geoMapFrame = value;
                _geoMapFrame.ProgressHandler = ProgressHandler;
                OnIncludeMapFrame(_geoMapFrame);
                if (proj != null)
                    Projection = proj;
                MapFrame.ResetBuffer();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the collection of layers
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapLayerCollection Layers
        {
            get
            {
                if (_geoMapFrame != null) return _geoMapFrame.Layers;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ProjectionInfo Projection
        {
            get
            {
                return _geoMapFrame != null ? _geoMapFrame.Projection : null;
            }
            set
            {
                if (_geoMapFrame != null)
                {
                    _geoMapFrame.Projection = value;
                    OnProjectionChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the legend to use when showing the layers from this map
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
                if (_legend != null)
                {
                    _legend.AddMapFrame(_geoMapFrame);
                }
            }
        }

        IFrame IBasicMap.MapFrame
        {
            get { return _geoMapFrame; }
        }

        /// <summary>
        /// Gets or sets the progress handler for this component.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set
            {
                _progressHandler = value;
                _geoMapFrame.ProgressHandler = value;
            }
        }

        protected void MapFrame_ViewExtentsChanged(object sender, ExtentArgs args)
        {
            OnViewExtentsChanged(sender, args);
        }

        /// <summary>
        /// Handles removing event handlers for the map frame
        /// </summary>
        /// <param name="mapFrame"></param>
        protected virtual void OnExcludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null) return;
            mapFrame.UpdateMap -= MapFrameUpdateMap;
            mapFrame.ScreenUpdated -= MapFrameScreenUpdated;
            mapFrame.ItemChanged -= MapFrameItemChanged;
            mapFrame.BufferChanged -= MapFrame_BufferChanged;
            mapFrame.SelectionChanged -= MapFrame_SelectionChanged;
            mapFrame.LayerAdded -= MapFrame_LayerAdded;
            mapFrame.ViewExtentsChanged -= MapFrame_ViewExtentsChanged;
            if (Legend != null)
            {
                Legend.RemoveMapFrame(mapFrame, true);
            }
        }

        /// <summary>
        /// Handles adding new event handlers to the map frame
        /// </summary>
        /// <param name="mapFrame"></param>
        protected virtual void OnIncludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null)
            {
                if (Legend == null) return;
                Legend.RefreshNodes();
                return;
            }
            mapFrame.Parent = this;
            mapFrame.UpdateMap += MapFrameUpdateMap;
            mapFrame.ScreenUpdated += MapFrameScreenUpdated;
            mapFrame.ItemChanged += MapFrameItemChanged;
            mapFrame.BufferChanged += MapFrame_BufferChanged;
            mapFrame.SelectionChanged += MapFrame_SelectionChanged;
            mapFrame.LayerAdded += MapFrame_LayerAdded;
            mapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
            if (Legend == null) return;
            Legend.AddMapFrame(mapFrame);
        }

        protected void MapFrame_LayerAdded(object sender, LayerEventArgs e)
        {
            OnLayerAdded(sender, e);
        }

        /// <summary>
        /// Fires the LayerAdded event
        /// </summary>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            var h = LayerAdded;
            if (h != null) h(sender, e);
        }

        protected void MapFrame_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        /// <summary>
        /// Occurs after the selection is updated on all the layers
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            var h = SelectionChanged;
            if (h != null) h(this, EventArgs.Empty);
        }

        protected void MapFrame_BufferChanged(object sender, ClipArgs e)
        {
            Rectangle view = MapFrame.View;
            foreach (Rectangle clip in e.ClipRectangles)
            {
                if (clip.IsEmpty == false)
                {
                    var mapClip = new Rectangle(clip.X - view.X, clip.Y - view.Y, clip.Width, clip.Height);
                    Invalidate(mapClip);
                }
            }

        }

        #endregion

        #region Map Core

        /// <summary>
        /// Occurs after the map refreshes the image
        /// </summary>
        public event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs when the map function mode has changed
        /// </summary>
        /// <remarks>Example of changing the function mode
        /// is changing from zoom mode to select mode.</remarks>
        public event EventHandler FunctionModeChanged;

        /// <summary>
        /// Occurs after a resize event
        /// </summary>
        public event EventHandler Resized;

        /// <summary>
        /// Occurs after the selection has changed for all the layers
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs after a layer has been added to the mapframe, or any of the child groups of that mapframe.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Public event advertising the mouse movement
        /// </summary>
        public event EventHandler<GeoMouseArgs> GeoMouseMove;

        /// <summary>
        /// Fires after the view extents have been altered and the map has redrawn to the new extents.
        /// This is an echo of the MapFrame.ViewExtentsChanged, so you only want one handler.
        /// </summary>
        public event EventHandler<ExtentArgs> ViewExtentsChanged;

        /// <summary>
        /// Occurs after the projection of the map has been changed
        /// </summary>
        public event EventHandler ProjectionChanged;

        private void Configure()
        {
            _mapCore.Configure();
        }

        /// <summary>
        /// Removes any members from existing in the selected state
        /// </summary>
        public bool ClearSelection(out IEnvelope affectedArea)
        {
            return _mapCore.ClearSelection(out affectedArea);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public bool Select(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            return _mapCore.Select(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode determining how to test for intersection.</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members.</param>
        /// <returns>boolean, true if members were changed by the selection process.</returns>
        public bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            return _mapCore.InvertSelection(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            return _mapCore.UnSelect(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Allows the user to add a new layer to the map using an open file dialog to choose a layer file.
        /// Multi-select is an option, so this return a list with all the layers.
        /// </summary>
        public virtual List<IMapLayer> AddLayers()
        {
            return _mapCore.AddLayers();
        }

        /// <summary>
        /// Adds the fileName as a new layer to the map, returning the new layer.
        /// </summary>
        /// <param name="fileName">The string fileName of the layer to add</param>
        /// <returns>The IMapLayer added to the file.</returns>
        public virtual IMapLayer AddLayer(string fileName)
        {
            return _mapCore.AddLayer(fileName);
        }

        /// <summary>
        /// This is so that if you have a basic map interface you can still prompt
        /// to add a layer, you just won't get an IMapLayer back.
        /// </summary>
        ILayer IBasicMap.AddLayer()
        {
            return AddLayer();
        }

        /// <summary>
        /// Uses the file dialog to allow selection of a fileName for opening the
        /// new layer, but does not allow multiple files to be added at once.
        /// </summary>
        /// <returns>The newly opened IMapLayer</returns>
        public virtual IMapLayer AddLayer()
        {
            return _mapCore.AddLayer();
        }

        /// <summary>
        /// Allows a multi-select file dialog to add raster layers, applying a
        /// filter so that only supported raster formats will appear.
        /// </summary>
        /// <returns>A list of the IMapRasterLayers that were opened.</returns>
        public virtual List<IMapRasterLayer> AddRasterLayers()
        {
            return _mapCore.AddRasterLayers();
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster to the map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapRasterLayer that was added, or null.</returns>
        public virtual IMapRasterLayer AddRasterLayer()
        {
            return _mapCore.AddRasterLayer();
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported vector extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapFeatureLayers</returns>
        public virtual List<IMapFeatureLayer> AddFeatureLayers()
        {
            return _mapCore.AddFeatureLayers();
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster tot he map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapFeatureLayer that was added, or null.</returns>
        public virtual IMapFeatureLayer AddFeatureLayer()
        {
            return _mapCore.AddFeatureLayer();
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported image extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapImageLayers</returns>
        public virtual List<IMapImageLayer> AddImageLayers()
        {
            return _mapCore.AddImageLayers();
        }

        /// <summary>
        /// Allows an open dialog without multi-select to specify a single fileName
        /// to be added to the map as a new layer and returns the newly added layer.
        /// </summary>
        /// <returns>The layer that was added to the map, or null.</returns>
        public virtual IMapImageLayer AddImageLayer()
        {
            return _mapCore.AddImageLayer();
        }

        /// <summary>
        /// This can be called any time, and is currently being used to capture
        /// the end of a resize event when the actual data should be updated.
        /// </summary>
        public virtual void ResetBuffer()
        {
            _mapCore.ResetBuffer();
        }

        /// <summary>
        /// Saves the dataset belonging to the layer.
        /// </summary>
        public virtual void SaveLayer()
        {
            _mapCore.SaveLayer();
        }

        protected void OnMouseMove(GeoMouseArgs args)
        {
            var h = GeoMouseMove;
            if (h != null) h(this, args);
        }

        protected virtual void OnFinishedRefresh(EventArgs e)
        {
            var h = FinishedRefresh;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Fires the ProjectionChanged event.
        /// </summary>
        protected virtual void OnProjectionChanged()
        {
            var h = ProjectionChanged;
            if (h != null) h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the ViewExtentsChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnViewExtentsChanged(object sender, ExtentArgs args)
        {
            var h = ViewExtentsChanged;
            if (h != null) h(sender, args);

            _mapCore.OnViewExtentsChanged();
        }

        /// <summary>
        /// Fires the FunctionModeChanged event.
        /// </summary>
        protected virtual void OnFunctionModeChanged(object sender, EventArgs e)
        {
            var h = FunctionModeChanged;
            if (h != null) h(this, e);
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            _mapCore.OnSizeChanged();
            OnResized();
        }

        /// <summary>
        /// Occurs after this object has been resized.
        /// </summary>
        protected virtual void OnResized()
        {
            var handler = Resized;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void Draw(Graphics g, PaintEventArgs e)
        {
            _mapCore.Draw(g, e);
        }

        protected void MapFrameItemChanged(object sender, EventArgs e)
        {
            _mapCore.MapFrameItemChanged(sender, e);
        }

        protected void MapFrameUpdateMap(object sender, EventArgs e)
        {
            _mapCore.MapFrameUpdateMap(sender, e);
        }

        protected void MapFrameScreenUpdated(object sender, EventArgs e)
        {
            _mapCore.MapFrameScreenUpdated(sender, e);
        }

        #endregion
    }
}