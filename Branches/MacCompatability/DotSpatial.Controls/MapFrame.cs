// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2008 9:37:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Kyle Ellison 11/02/2010 Draw map background using map control BackColor
// Kyle Ellison 01/07/2010 Made Initialize and InitializeLabels both public virtual to enable labels
//                         in custom Map Layers
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Projections.Forms;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A MapFrame accomplishes two things.  Firstly, it organizes the layers to be drawn, and establishes the geographic
    /// extents.  Secondly, it hosts the back-buffer image that can be larger than the component that
    /// this map frame would normally be drawn to.  When it receives instructions to paint itself, the client rectangle
    /// will automatically end up behaving like a clip rectangle.
    /// </summary>
    [Serializable]
    public class MapFrame : LayerFrame, IMapFrame
    {
        #region Events

        /// <summary>
        /// Occurs after changes have been made to the back buffer that affect the viewing area of the screen,
        /// thereby requiring an invalidation.
        /// </summary>
        public event EventHandler ScreenUpdated;

        /// <summary>
        /// Occurs after every one of the zones, chunks and stages has finished rendering to a stencil.
        /// </summary>
        public event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs when the buffer content has been altered and any containing maps should quick-draw
        /// from the buffer, followed by the tool drawing.
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        /// <summary>
        /// Occurs when View changed
        /// </summary>
        public event EventHandler<ViewChangedEventArgs> ViewChanged;

        #endregion Events

        #region Private Variables

        private Image _backBuffer;
        private Rectangle _backView;
        private Image _buffer;
        private ProjectionInfo _chosenProjection;
        private List<Rectangle> _clipRegions;
        private int _currentChunk;
        private bool _extendBuffer;
        private int _height;
        private bool _isPanning;
        private IMapLayerCollection _layers;
        private Rectangle _originalView;
        private IMap _parent;
        private ActionMode _projectionModeDefine;
        private ActionMode _projectionModeReproject;
        private bool _resizing;
        private Rectangle _view;
        private int _width;
        private ILayer former;

        private bool _isZoomingNextOrPrevious;
        private readonly LimitedStack<Extent> _previousExtents = new LimitedStack<Extent>();
        private LimitedStack<Extent> _nextExtents = new LimitedStack<Extent>();
        private Extent _lastExtent;
        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Creates the default map frame, allowing the control that it belongs to to be set later.
        /// </summary>
        public MapFrame()
        {
            base.ViewExtents = new Extent(-180, 180, -90, 90);
            if (Data.DataSet.ProjectionSupported())
            {
                Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            }
            _backBuffer = CreateBuffer();
            Layers = new MapLayerCollection(this);
           
            base.IsSelected = true;  // by default allow the map frame to be selected

            //add properties context menu item
            base.ContextMenuItems.Add(new SymbologyMenuItem(MessageStrings.MapFrame_Projection, Projection_Click));
        }

        /// <summary>
        /// Creates a new instance of a MapFrame without specifying the extents.  The
        /// geographic extents of the world will be used.
        /// </summary>
        /// <param name="inParent">The parent control that should own this map frame.</param>
        public MapFrame(IMap inParent)
            : this()
        {
            _parent = inParent;
        }

        /// <summary>
        /// Creates a new instance of MapFrame
        /// </summary>
        public MapFrame(IMap inParent, Extent inExtent)
            : this()
        {
            _parent = inParent;
            base.ViewExtents = inExtent;
        }

        /// <inheritdoc />
        public override int IndexOf(ILayer item)
        {
            IMapLayer ml = item as IMapLayer;
            if (ml != null)
            {
                return _layers.IndexOf(ml);
            }
            return -1;
        }

        /// <inheritdoc />
        public override void Add(ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            if (ml != null)
            {
                _layers.Add(ml);
            }
        }

        /// <inheritdoc />
        public override ILayer this[int index]
        {
            get
            {
                return _layers[index];
            }
            set
            {
                IMapLayer ml = value as IMapLayer;
                _layers[index] = ml;
            }
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _layers.Clear();
        }

        /// <inheritdoc />
        public override IEnumerator<ILayer> GetEnumerator()
        {
            return new MapLayerEnumerator(_layers.GetEnumerator());
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        /// <inheritdoc />
        public override bool Contains(ILayer item)
        {
            IMapLayer ml = item as IMapLayer;
            return ml != null && _layers.Contains(ml);
        }

        /// <inheritdoc />
        public override bool Remove(ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            return ml != null && _layers.Remove(ml);
        }

        /// <inheritdoc />
        public override void RemoveAt(int index)
        {
            _layers.RemoveAt(index);
        }

        /// <inheritdoc />
        public override void Insert(int index, ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            if (ml != null)
            {
                _layers.Insert(index, ml);
            }
        }

        /// <inheritdoc />
        public override void SuspendEvents()
        {
            _layers.SuspendEvents();
        }

        /// <inheritdoc />
        public override void ResumeEvents()
        {
            _layers.ResumeEvents();
        }

        /// <inheritdoc />
        public override bool EventsSuspended
        {
            get { return _layers.EventsSuspended; }
        }

        public override bool ClearSelection(out IEnvelope affectedAreas)
        {
            former = null;
            foreach (var l in this.GetAllLayers())
            {
                if (l.IsSelected)
                {
                    former = l;
                    l.IsSelected = false;
                }
           } 
            if (former == null && IsSelected)
            {
                former = this;
            }
            IsSelected = true;
            bool cleared = base.ClearSelection(out affectedAreas);
            IsSelected = false;
            if (former != null)
            {
                former.IsSelected = true;
            }
            return cleared;
        }

        /// <summary>
        /// This is not called during a resize, but rather after panning or zooming where the
        /// view is used as a guide to update the extents.  This will also call ResetBuffer.
        /// </summary>
        public virtual void ResetExtents()
        {
            // Find the geographic extents that would be centered on the current view
            Extent env = BufferToProj(View);
            ViewExtents = env;
        }

        /// <summary>
        /// Re-creates the buffer based on the size of the control without changing
        /// the geographic extents.  This is used after a resize operation.
        /// </summary>
        public virtual void ResetBuffer()
        {
            _backBuffer = null;
            // reset the view rectangle to represent the same region
            //_view = _extendBuffer ? new Rectangle(_width / 3, _height / 3, _width / 3, _height / 3) : new Rectangle(0, 0, _width, _height);

            Initialize();
        }

        /// <summary>
        /// Instructs the map frame to draw content from the specified regions to the buffer..
        /// </summary>
        /// <param name="regions">The regions to initialize.</param>
        public virtual void Initialize(List<Extent> regions)
        {
            bool setView = false;
            if (_backBuffer == null)
            {
                _backBuffer = CreateBuffer();

                //set the view
                setView = true;
            }

            Graphics bufferDevice = Graphics.FromImage(_backBuffer);
            MapArgs args = new MapArgs(ClientRectangle, ViewExtents, bufferDevice);
            GraphicsPath gp = new GraphicsPath();
            foreach (Extent region in regions)
            {
                if (region == null) continue;
                Rectangle rect = args.ProjToPixel(region);
                gp.StartFigure();
                gp.AddRectangle(rect);
            }
            bufferDevice.Clip = new Region(gp);

            // Draw the background color
            if (null != _parent) bufferDevice.Clear(_parent.BackColor);
            else bufferDevice.Clear(Color.White);

            // First draw all the vector content
            foreach (IMapLayer layer in Layers)
            {
                if (layer.VisibleAtExtent(ViewExtents)) layer.DrawRegions(args, regions);
            }
            // Then labels
            MapLabelLayer.ClearAllExistingLabels();
            foreach (var layer in Layers)
            {
                InitializeLabels(regions, args, layer);
            }

            // First draw all the vector content
            foreach (var layer in DrawingLayers.OfType<IMapLayer>())
            {
                if (layer.VisibleAtExtent(ViewExtents)) layer.DrawRegions(args, regions);
            }

            if (_buffer != null && _buffer != _backBuffer) _buffer.Dispose();
            _buffer = _backBuffer;
            if(setView)
                _view = _backView;
            bufferDevice.Clip = new Region(ImageRectangle);
            gp.Dispose();
            List<Rectangle> rects = args.ProjToPixel(regions);
            OnBufferChanged(this, new ClipArgs(rects));
        }

        /// <summary>
        /// Uses the current buffer and envelope to force each of the contained layers
        /// to re-draw their content.  This is useful after a zoom or size change.
        /// </summary>
        public virtual void Initialize()
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0) return;
            try
            {
                Initialize(new List<Extent> { ViewExtents });
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Pans the image for this map frame.  Instead of drawing entirely new content, from all 5 zones,
        /// just the slivers of newly revealed area need to be re-drawn.
        /// </summary>
        /// <param name="shift">A Point showing the amount to shift in pixels</param>
        public virtual void Pan(Point shift)
        {
            _view = new Rectangle(_view.X + shift.X, _view.Y + shift.Y, _view.Width, _view.Height);
            ResetExtents();
        }

        /// <summary>
        /// When content in a geographic region needs to be invalidated
        /// </summary>
        /// <param name="region"></param>
        public override void Invalidate(Extent region)
        {
            foreach (IMapLayer layer in Layers)
            {
                layer.Invalidate(region);
            }
        }

        /// <summary>
        /// This is a different view of the layers cast as legend items.  This allows
        /// easier cycling in recursive legend code.
        /// </summary>
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                // leave cast for 3.5 compatibility
                return _layers.Cast<ILegendItem>();
            }
        }

        /// <summary>
        /// Converts the internal list of layers into a list of ILayer interfaces
        /// </summary>
        /// <returns></returns>
        public override IList<ILayer> GetLayers()
        {
            if (_layers == null) return null;
            return _layers.Cast<ILayer>().ToList();
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="targetRectangle"></param>
        public void Print(Graphics device, Rectangle targetRectangle)
        {
            Print(device, targetRectangle, ViewExtents);
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="targetRectangle"></param>
        /// <param name="targetEnvelope">the extents to draw to the target rectangle</param>
        public virtual void Print(Graphics device, Rectangle targetRectangle, Extent targetEnvelope)
        {
            MapArgs args = new MapArgs(targetRectangle, targetEnvelope, device);
            Matrix oldMatrix = device.Transform;
            try
            {
                device.TranslateTransform(targetRectangle.X, targetRectangle.Y);

                foreach (IMapLayer ml in Layers)
                {
                    PrintLayer(ml, args);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                device.Transform = oldMatrix;
            }
        }

        /// <summary>
        /// Forces this MapFrame to copy the buffers for its layers to the back-buffer.
        /// </summary>
        public override void Invalidate()
        {
            Initialize();
        }

        private Bitmap CreateBuffer()
        {
            if (_parent != null)
            {
                _width = _parent.ClientRectangle.Width;
                _height = _parent.ClientRectangle.Height;
            }
            else
            {
                _width = 1000;
                _height = 800;
            }
            if (_height < 5) _height = 5;
            if (_width < 5) _width = 5;

            _backView = new Rectangle(0, 0, _width, _height);
            if (_extendBuffer)
            {
                _backView.X = _width;
                _backView.Y = _height;
                _width = _width * 3;
                _height = _height * 3;
            }

            return new Bitmap(_width, _height);
        }

        /// <summary>
        /// Overrides the group creation to make sure that the new group will cast its layers
        /// to the appropriate Layer type.
        /// </summary>
        protected override void OnCreateGroup()
        {
            new MapGroup(Layers, this, ProgressHandler) { LegendText = "New Group" };
        }

        /// <summary>
        /// Draw label content for a Map Layer
        /// </summary>
        protected virtual void InitializeLabels(List<Extent> regions, MapArgs args, IRenderable layer)
        {
            if (!layer.IsVisible) return;

            var grp = layer as IMapGroup;
            if (grp != null)
            {
                foreach (IMapLayer lyr in grp.Layers)
                {
                    InitializeLabels(regions, args, lyr);
                }
                return;
            }

            var mfl = layer as IMapFeatureLayer;
            if (mfl != null)
            {
                if (mfl.ShowLabels && mfl.LabelLayer != null)
                {
                    if (mfl.LabelLayer.VisibleAtExtent(args.GeographicExtents))
                    {
                        mfl.LabelLayer.DrawRegions(args, regions);
                    }
                }
            }
        }

        private void PrintLayer(IMapLayer layer, MapArgs args)
        {
            MapLabelLayer.ClearAllExistingLabels();
            IMapGroup group = layer as IMapGroup;
            if (group != null)
            {
                foreach (IMapLayer subLayer in group.Layers)
                {
                    PrintLayer(subLayer, args);
                }
            }

            IMapLayer geoLayer = layer;
            if (geoLayer != null)
            {
                if (geoLayer.UseDynamicVisibility)
                {
                    if (ViewExtents.Width > geoLayer.DynamicVisibilityWidth)
                    {
                        return;  // skip the geoLayer if we are zoomed out too far.
                    }
                }

                if (geoLayer.IsVisible == false) return;

                geoLayer.DrawRegions(args, new List<Extent> { args.GeographicExtents });

                IMapFeatureLayer mfl = geoLayer as IMapFeatureLayer;
                if (mfl != null)
                {
                    if (mfl.UseDynamicVisibility)
                    {
                        if (ViewExtents.Width > mfl.DynamicVisibilityWidth) return;
                    }
                    if (mfl.ShowLabels && mfl.LabelLayer != null)
                    {
                        if (mfl.LabelLayer.UseDynamicVisibility)
                        {
                            if (ViewExtents.Width > mfl.LabelLayer.DynamicVisibilityWidth) return;
                        }
                        mfl.LabelLayer.DrawRegions(args, new List<Extent> { args.GeographicExtents });
                    }
                }
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Obtains a rectangle relative to the background image by comparing
        /// the current View rectangle with the parent control's size.
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public Rectangle ParentToView(Rectangle clip)
        {
            Rectangle result = new Rectangle
                                   {
                                       X = View.X + (clip.X * View.Width) / _parent.ClientRectangle.Width,
                                       Y = View.Y + (clip.Y * View.Height) / _parent.ClientRectangle.Height,
                                       Width = clip.Width * View.Width / _parent.ClientRectangle.Width,
                                       Height = clip.Height * View.Height / _parent.ClientRectangle.Height
                                   };
            return result;
        }

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        public void ZoomIn()
        {
            Rectangle r = View;
            r.Inflate(-r.Width / 4, -r.Height / 4);
            View = r;
            ResetExtents();
        }

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        public void ZoomOut()
        {
            Rectangle r = View;
            r.Inflate(r.Width / 2, r.Height / 2);
            View = r;
            ResetExtents();
        }

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        /// <param name="ext"></param>
        protected override void OnExtentsChanged(Extent ext)
        {
            if (ext.X == -180 && ext.Y == 90) { return; }
            if (_isZoomingNextOrPrevious)
            {
                // reset the flag for the next extents change
                _isZoomingNextOrPrevious = false;
            }
            else
            {
                // Add the last extent to the stack
                // We shouldn't really need to peek and compare, but found that the method
                // Might be called too freqently in some case.
                if (ViewExtents != _previousExtents.Peek())
                {
                    if (_lastExtent != null)
                        _previousExtents.Push(_lastExtent);

                    _lastExtent = ext;

                    // clear the forward history.
                    _nextExtents = new LimitedStack<Extent>();
                }
            }
            base.OnExtentsChanged(ext);
        }

        /// <summary>
        /// Determines whether this instance [can zoom to next]. Should not be called inside of
        /// MapFrame_ViewExtentsChanged event.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can zoom to next]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanZoomToNext()
        {
            return _nextExtents.Count > 0;
        }

        /// <summary>
        /// Determines whether this instance [can zoom to previous]. Should not be called inside of
        /// MapFrame_ViewExtentsChanged event.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can zoom to previous]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanZoomToPrevious()
        {
            return _previousExtents.Count > 0;
        }

        /// <summary>
        /// Zooms to the next extent
        /// </summary>
        public void ZoomToNext()
        {
            if (_nextExtents.Count > 0)
            {
                _isZoomingNextOrPrevious = true;
                Extent extent = _nextExtents.Pop();
                _previousExtents.Push(ViewExtents);
                base.ViewExtents = extent;
                ResetBuffer();
            }
        }

        /// <summary>
        /// Zooms to the previous extent
        /// </summary>
        public void ZoomToPrevious()
        {
            if (_previousExtents.Count > 0)
            {
                _isZoomingNextOrPrevious = true;
                Extent extent = _previousExtents.Pop();
                _nextExtents.Push(ViewExtents);
                base.ViewExtents = extent;
                ResetBuffer();
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// This gets or sets the first clipRegion.  If more than one region needs to be set,
        /// the ClipRegions list should be used instead.
        /// </summary>
        public Rectangle ClipRectangle
        {
            get
            {
                if (_clipRegions == null)
                {
                    _clipRegions = new List<Rectangle> { ClientRectangle };
                }
                return _clipRegions[0];
            }
            set
            {
                if (_clipRegions == null)
                {
                    _clipRegions = new List<Rectangle>();
                }
                _clipRegions.Clear();
                _clipRegions.Add(value);
            }
        }

        /// <summary>
        /// Gets or sets a set of rectangles that should be drawn during the next update
        /// </summary>
        public List<Rectangle> ClipRegions
        {
            get { return _clipRegions; }
            set { _clipRegions = value; }
        }

        /// <summary>
        /// Gets or sets the geographic extents to be drawn to a buffer.
        /// If "ExtendBuffer" is true, then these extents are larger
        /// than the geographic extents of the parent client,
        /// and care should be taken when using PixelToProj,
        /// as it will work differently.
        /// </summary>
        public Extent BufferExtents
        {
            get
            {
                if (_extendBuffer)
                {
                    Extent ext = ViewExtents.Copy();
                    ext.ExpandBy(ext.Width, ext.Height);
                    return ext;
                }
                return ViewExtents;
            }
        }

        /// <summary>
        /// Gets or sets the height
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the width in pixels for this map frame.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the integer that specifies the chunk that is actively being drawn
        /// </summary>
        public int CurrentChunk
        {
            get { return _currentChunk; }
            set
            {
                _currentChunk = value;
            }
        }

        /// <inheritdoc/>
        public override Extent ViewExtents
        {
            get
            {
                return base.ViewExtents;
            }
            set
            {
                if (value == null) return;
                Extent ext = value.Copy();
                ResetAspectRatio(ext);
                // reset buffer initializes with correct buffer.  Don't allow initialization yet.
                SuspendExtentChanged();
                base.ViewExtents = ext;
                ResetBuffer();
                ResumeExtentChanged(); // fires the needed event.
            }
        }

        /// <summary>
        /// Gets the geographic extents
        /// </summary>
        public Extent GeographicExtents
        {
            get { return BufferToProj(View); }
        }

        /// <summary>
        /// Gets or sets whether this map frame should define its buffer
        /// region to be the same size as the client, or three times larger.
        /// </summary>
        [Serialize("ExtendBuffer")]
        public bool ExtendBuffer
        {
            get { return _extendBuffer; }
            set { _extendBuffer = value; }
        }

        /// <summary>
        /// Gets or sets whether this map frame is currently in the process of redrawing the
        /// stencils after a pan operation.  Drawing should not take place if this is true.
        /// </summary>
        public bool IsPanning
        {
            get { return _isPanning; }
            set { _isPanning = value; }
        }

        /// <summary>
        /// When the control is being resized, the view needs to change in order to preserve the aspect ratio,
        /// even though we want to use the exact same extents.
        /// </summary>
        public void ParentResize()
        {
            if (_resizing == false)
            {
                _originalView = _view;
                _resizing = true;
            }
            double w = Parent.ClientRectangle.Width;
            double h = Parent.ClientRectangle.Height;
            double vW = _originalView.Width;
            double vH = _originalView.Height;
            if (h == 0 || w == 0 || vW == 0 || vH == 0) return;
            double controlAspect = w / h;
            double viewAspect = vW / vH;
            if (controlAspect > viewAspect)
            {
                int dW = Convert.ToInt32(vH * controlAspect - vW);
                _view.X = _originalView.X - dW / 2;
                _view.Width = _originalView.Width + dW;
            }
            else
            {
                int dH = Convert.ToInt32(vW / controlAspect - vH);
                _view.Y = _originalView.Y - dH / 2;
                _view.Height = _originalView.Height + dH;
            }
        }

        /// <summary>
        /// Gets or sets the layers
        /// </summary>
        [Serialize("Layers")]
        public new IMapLayerCollection Layers
        {
            get { return _layers; }
            set
            {
                if (Layers != null)
                {
                    Ignore_Layer_Events(_layers);
                }
                _layers = value;
                _layers.ProgressHandler = ProgressHandler;
                if (_layers != null)
                {
                    Handle_Layer_Events(value);
                    _layers.MapFrame = this;
                    _layers.ParentGroup = this;
                }
            }
        }

        /// <summary>
        /// gets or sets the ReprojectMode that determines if new layers are reprojected when added to the map.
        /// </summary>
        public ActionMode ProjectionModeReproject
        {
            get { return _projectionModeReproject; }
            set { _projectionModeReproject = value; }
        }

        /// <summary>
        /// Gets or sets the PromptMode that determines how to warn users when attempting to add a layer without
        /// a projection to a map that has a projection.
        /// </summary>
        public ActionMode ProjectionModeDefine
        {
            get { return _projectionModeDefine; }
            set
            {
                _projectionModeDefine = value;
            }
        }

        /// <summary>
        /// gets or sets the rectangle in pixel coordinates that will be drawn to the entire screen.
        /// </summary>
        public Rectangle View
        {
            get { return _view; }
            set
            {
                if (_view == value) return;

                var h = ViewChanged;
                if (h != null)
                {
                    var old_view = _view;
                    _view = value;
                    var args = new ViewChangedEventArgs {OldView = old_view, NewView = _view};
                    h(this, args);
                }
                else
                {
                    _view = value;
                }
            }
        }

        /// <summary>
        /// If a BackBuffer.Extents exists, this will enlarge those extents to match the aspect ratio
        /// of the pixel view.  If one doesn't exist, the _mapFrame.Extents will be used instead.
        /// </summary>
        /// <param name="newEnv">The envelope to adjust</param>
        protected void ResetAspectRatio(Extent newEnv)
        {
            // ---------- Aspect Ratio Handling
            if (newEnv == null) return;
            double h = Parent.Height;
            double w = Parent.Width;
            // It isn't exactly an exception, but rather just an indication not to do anything here.
            if (h == 0 || w == 0) return;

            double controlAspect = w / h;
            double envelopeAspect = newEnv.Width / newEnv.Height;
            Coordinate center = newEnv.ToEnvelope().Center();

            if (controlAspect > envelopeAspect)
            {
                // The Control is proportionally wider than the envelope to display.
                // If the envelope is proportionately wider than the control, "reveal" more width without
                // changing height If the envelope is proportionately taller than the control,
                // "hide" width without changing height
                newEnv.SetCenter(center, newEnv.Height * controlAspect, newEnv.Height);
            }
            else
            {
                // The control is proportionally taller than the content is
                // If the envelope is proportionately wider than the control,
                // "hide" the extra height without changing width
                // If the envelope is proportionately taller than the control, "reveal" more height without changing width
                newEnv.SetCenter(center, newEnv.Width, newEnv.Width / controlAspect);
            }
            _resizing = false;
        }

        #endregion Properties
         
        #region Protected Methods

        /// <summary>
        /// The right in client rectangle coordinates
        /// </summary>
        public int Right
        {
            get { return ClientRectangle.Right; }
        }

        /// <summary>
        /// The bottom (or height) of this client rectangle
        /// </summary>
        public int Bottom
        {
            get { return ClientRectangle.Bottom; }
        }

        /// <summary>
        /// This will create a new layer from the featureset and add it.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics</param>
        public override void Add(IFeatureSet featureSet)
        {
            Layers.Add(featureSet);
        }

        /// <summary>
        /// Draws from the buffer.
        /// </summary>
        /// <param name="pe"></param>
        public void Draw(PaintEventArgs pe)
        {
            if (_buffer == null) return;

            Rectangle clip = pe.ClipRectangle;
            if (clip.IsEmpty) clip = _parent.ClientRectangle;
            Rectangle clipView = ParentToView(clip);
            if (clip.Width == 0 || clip.Height == 0) return;
            if (clipView.Width == 0 || clipView.Height == 0) return;
            try
            {
                if(_buffer.Size != clip.Size && clip.Location.IsEmpty)
                    pe.Graphics.DrawImageUnscaledAndClipped(_buffer, clip);
                else
                    pe.Graphics.DrawImage(_buffer, clip, clipView, GraphicsUnit.Pixel);
            }
            catch
            {
                // There was an exception (probably because of sizing issues) so don't bother with the chunk timer.
            }

            //base.OnPaint(pe);
        }

        /// <summary>
        /// Unlike PixelToProj, which works relative to the client control,
        /// BufferToProj takes a pixel coordinate on the buffer and
        /// converts it to geographic coordinates.
        /// </summary>
        /// <param name="position">A Point describing the pixel position on the back buffer</param>
        /// <returns>An ICoordinate describing the geographic position</returns>
        public Coordinate BufferToProj(Point position)
        {
            IEnvelope view = BufferExtents.ToEnvelope();
            if (base.ViewExtents == null) return new Coordinate(0, 0);
            double x = Convert.ToDouble(position.X);
            double y = Convert.ToDouble(position.Y);
            x = x * view.Width / _width + view.Minimum.X;
            y = view.Maximum.Y - y * view.Height / _height;

            return new Coordinate(x, y, 0.0);
        }

        /// <summary>
        /// This projects a rectangle relative to the buffer into and IEnvelope in geographic coordinates.
        /// </summary>
        /// <param name="rect">A Rectangle</param>
        /// <returns>An IEnvelope interface</returns>
        public Extent BufferToProj(Rectangle rect)
        {
            Point tl = new Point(rect.X, rect.Y);
            Point br = new Point(rect.Right, rect.Bottom);
            Coordinate topLeft = BufferToProj(tl);
            Coordinate bottomRight = BufferToProj(br);
            return new Extent(topLeft.X, bottomRight.Y, bottomRight.X, topLeft.Y);
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A Point with the new location.</returns>
        public Point ProjToBuffer(Coordinate location)
        {
            IEnvelope view = BufferExtents.ToEnvelope();
            if (_width == 0 || _height == 0) return new Point(0, 0);
            int x = Convert.ToInt32((location.X - view.Minimum.X) * (_width / view.Width));
            int y = Convert.ToInt32((view.Maximum.Y - location.Y) * (_height / view.Height));
            return new Point(x, y);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A Rectangle</returns>
        public Rectangle ProjToBuffer(Extent env)
        {
            Coordinate tl = new Coordinate(env.MinX, env.MaxY);
            Coordinate br = new Coordinate(env.MaxX, env.MinY);
            Point topLeft = ProjToBuffer(tl);
            Point bottomRight = ProjToBuffer(br);
            return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }

        /// <summary>
        /// This will cause an invalidation for each layer.  The actual rectangle to re-draw is not specified
        /// here, but rather this simply indicates that some re-calculation is necessary.
        /// </summary>
        public void InvalidateLayers()
        {
            if (Layers == null) return;
            foreach (ILayer layer in base.Layers)
            {
                layer.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the buffered image.  Mess with this at your own risk.
        /// </summary>
        public Image BufferImage
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        /// <summary>
        /// Gets or sets the client rectangle
        /// </summary>
        public Rectangle ClientRectangle
        {
            get { return new Rectangle(0, 0, _width, _height); }
        }

        /// <summary>
        /// Gets or sets the ImageRectangle for drawing the buffer
        /// </summary>
        public Rectangle ImageRectangle
        {
            get
            {
                if (_extendBuffer) return new Rectangle(_width / 3, _height / 3, _width / 3, _height / 3);
                return new Rectangle(0, 0, _width, _height);
            }
        }

        /// <summary>
        /// Gets or sets the progress handler to use.  Setting this will set the progress handler for
        /// each of the layers in this map frame.
        /// </summary>
        public override IProgressHandler ProgressHandler
        {
            get { return base.ProgressHandler; }
            set
            {
                base.ProgressHandler = value;
                _layers.ProgressHandler = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent control for this map frame.
        /// </summary>
        public new IMap Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                _backBuffer = CreateBuffer();
            }
        }

        /// <summary>
        /// Fires the ScreenUpdated event
        /// </summary>
        protected virtual void OnScreenUpdated()
        {
            if (ScreenUpdated == null) return;
            ScreenUpdated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the FinsihedRefresh event
        /// </summary>
        protected virtual void OnFinishedRefresh()
        {
            if (FinishedRefresh != null)
            {
                FinishedRefresh(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Wires each of the layer events that the MapFrame should be listening to.
        /// </summary>
        /// <param name="collection"></param>
        protected override void Handle_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            IMapLayerCollection glc = collection as IMapLayerCollection;
            if (glc == null) return;
            glc.BufferChanged += GeoLayerBufferChanged;
            glc.LayerAdded += LayerColection_LayerAdded;
            glc.LayerVisibleChanged += LayerCollection_LayerVisibleChanged;
            glc.ItemChanged += LayerCollection_MembersChanged;
            base.Handle_Layer_Events(collection);
        }

        /// <summary>
        /// Modifies the ZoomToLayer behavior to account for the possibility of an expanded
        /// MapFrame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Layers_ZoomToLayer(object sender, EnvelopeArgs e)
        {
            ZoomToLayerEnvelope(e.Envelope);
        }

        private void ZoomToLayerEnvelope(IEnvelope layerEnvelope)
        {
            if (_extendBuffer)
            {
                layerEnvelope.ExpandBy(layerEnvelope.Width, layerEnvelope.Height);
            }

            const double eps = 1e-7;
            if (layerEnvelope.Width > eps && layerEnvelope.Height > eps)
            {
                layerEnvelope.ExpandBy(layerEnvelope.Width / 10, layerEnvelope.Height / 10); // work item #84
            }
            else
            {
                double zoomInFactor = 0.05; //fixed zoom-in by 10% - 5% on each side
                double newExtentWidth = ViewExtents.Width * zoomInFactor;
                double newExtentHeight = ViewExtents.Height * zoomInFactor;
                layerEnvelope.ExpandBy(newExtentWidth, newExtentHeight);
            }

            ViewExtents = layerEnvelope.ToExtent();
        }

        private void LayerCollection_LayerVisibleChanged(object sender, EventArgs e)
        {
            Initialize();
            OnUpdateMap();
        }

        private void LayerCollection_MembersChanged(object sender, EventArgs e)
        {
            Initialize();
            OnUpdateMap();
        }

        private void LayerColection_LayerAdded(object sender, LayerEventArgs e)
        {
            IMapLayer layer = e.Layer as IMapLayer;
            if (layer == null) return;
            if (!ExtentsInitialized || ViewExtents == null || ViewExtents.IsEmpty() ||
                ViewExtents.Width == 0 || View.Height == 0)
            {
                ExtentsInitialized = true;
                Extent desired = e.Layer.Extent;
                if (desired != null && desired.IsEmpty() == false)
                {
                    if (desired.Width == 0 && desired.Height == 0)
                    {
                        // 1 point, or all points are the same point.
                        double x = desired.X;
                        double y = desired.Y;
                        ViewExtents = new Extent(x - x / 10, y - y / 10, x + x / 10, y + y / 10);
                    }
                    else if (desired.Width > 0 && desired.Width < 1E300)
                    {
                        if (desired.Height > 0 && desired.Height < 1E300)
                        {
                            Extent env = desired.Copy();
                            env.ExpandBy(env.Width / 10, env.Height / 10); // Work item #84
                            ViewExtents = env;
                        }
                    }
                }
            }

            ReprojectOnTheFly(layer);
            Initialize();
        }

        private void ReprojectOnTheFly(IMapLayer layer)
        {
            if (layer.DataSet == null) return;
            if (!Data.DataSet.ProjectionSupported()) return;

            var preventReproject = DefineProjection(layer);
            if ((Projection == null || Layers.Count == 1))
            {
                Projection = layer.DataSet.Projection;
                return;
            }

            if (preventReproject) return;
            if (Projection.Equals(layer.DataSet.Projection)) return;
            var bReproject = false;
            if (ProjectionModeReproject == ActionMode.Prompt || ProjectionModeReproject == ActionMode.PromptOnce)
            {
                string message = MessageStrings.MapFrame_GlcLayerAdded_ProjectionMismatch;
                if (ProjectionModeReproject == ActionMode.PromptOnce)
                {
                    message =
                        "The newly added layer has a coordinate system, but that coordinate system does not match the other layers in the map.  Do you want to reproject new layers on the fly so that they are drawn in the same coordinate system as the other layers?";
                }
                DialogResult result = MessageBox.Show(
                    message,
                    MessageStrings.MapFrame_GlcLayerAdded_Projection_Mismatch,
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    bReproject = true;
                }
                if (ProjectionModeReproject == ActionMode.PromptOnce)
                {
                    ProjectionModeReproject = result == DialogResult.Yes
                        ? ActionMode.Always
                        : ActionMode.Never;
                }
            }
            if (bReproject || ProjectionModeReproject == ActionMode.Always)
            {
                layer.Reproject(Projection);
            }
        }

        private bool DefineProjection(IMapLayer layer)
        {
            if (layer.DataSet.Projection == null || layer.DataSet.Projection.Transform == null)
            {
                if (ProjectionModeDefine == ActionMode.Always && _chosenProjection == null)
                {
                    _chosenProjection = Projection;
                }
                ProjectionInfo result = _chosenProjection;
                if (ProjectionModeDefine == ActionMode.Prompt || ProjectionModeDefine == ActionMode.PromptOnce)
                {
                    var dlg = new UndefinedProjectionDialog
                    {
                        OriginalString = layer.DataSet.ProjectionString,
                        MapProjection = Projection,
                        LayerName = layer.DataSet.Name,
                    };

                    if (_chosenProjection != null) dlg.SelectedCoordinateSystem = _chosenProjection;
                    dlg.AlwaysUse = (ProjectionModeDefine == ActionMode.PromptOnce);
                    dlg.ShowDialog ();

                    if (dlg.AlwaysUse)
                    {
                        _chosenProjection = dlg.Result;
                        ProjectionModeDefine = ActionMode.Always;
                    }
                    result = dlg.Result;
                }
                if (result != null)
                {
                    layer.DataSet.Projection = result;
                }
            }
            return layer.DataSet.Projection == null;
        }

        /// <summary>
        /// Unwires events from the layer collection
        /// </summary>
        /// <param name="collection"></param>
        protected override void Ignore_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            IMapLayerCollection glc = collection as IMapLayerCollection;
            if (glc == null) return;
            glc.BufferChanged -= GeoLayerBufferChanged;
            glc.LayerAdded -= LayerColection_LayerAdded;
            glc.LayerVisibleChanged -= LayerCollection_LayerVisibleChanged;
            base.Ignore_Layer_Events(collection);
        }

        /// <summary>
        /// When any region for the stencil of any layers is changed, we should update the
        /// image that we have in that region.  This activity will be suspended in the
        /// case of a large scale update for all the layers until they have all updated.
        /// </summary>
        private void GeoLayerBufferChanged(object sender, ClipArgs e)
        {
            OnBufferChanged(this, e);
        }

        /// <summary>
        /// Fires the BufferChanged event.  This is fired even if the new content is not currently
        /// in the view rectangle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBufferChanged(object sender, ClipArgs e)
        {
            if (BufferChanged != null) BufferChanged(this, e);
        }

        /// <summary>
        /// When the Projection context menu is clicked
        /// </summary>
        private void Projection_Click(object sender, EventArgs e)
        {
            //Launches a MapFrameProjectionDialog
            using (var dialog = new MapFrameProjectionDialog(this))
            {
                dialog.ShowDialog();
            }
        }

        #endregion Protected Methods

        #region IMapFrame Members

        /// <inheritdoc />
        public IMapFrame ParentMapFrame
        {
            get { return this; }
        }

        /// <inheritdoc />
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
        }

        #endregion IMapFrame Members
    }

    internal class LimitedStack<T>
    {
        public readonly int Limit;
        private readonly List<T> _stack;

        public LimitedStack(int limit = 32)
        {
            Limit = limit;
            _stack = new List<T>(limit);
        }

        public void Push(T item)
        {
            if (_stack.Count == Limit) _stack.RemoveAt(0);
            _stack.Add(item);
        }

        public T Peek()
        {
            if (_stack.Count == 0) return default(T);
            return _stack[_stack.Count - 1];
        }

        public T Pop()
        {
            var item = _stack[_stack.Count - 1];
            _stack.RemoveAt(_stack.Count - 1);
            return item;
        }

        public int Count
        {
            get { return _stack.Count; }
        }
    }
}