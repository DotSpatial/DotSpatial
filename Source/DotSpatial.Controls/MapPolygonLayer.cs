// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles polygon drawing.
    /// </summary>
    public class MapPolygonLayer : PolygonLayer, IMapPolygonLayer
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPolygonLayer"/> class that is empty with an empty FeatureSet of FeatureType Polygon.
        /// </summary>
        public MapPolygonLayer()
            : base(new FeatureSet(FeatureType.Polygon))
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPolygonLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">A polygon feature set.</param>
        public MapPolygonLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPolygonLayer"/> class.
        /// </summary>
        /// <param name="featureSet">A featureset that contains polygons</param>
        /// <param name="container">An IContainer that the polygon layer should be created in</param>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(featureSet, container, null)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPolygonLayer"/> class.
        /// </summary>
        /// <param name="featureSet">A featureset that contains polygons</param>
        /// <param name="container">An IContainer that the polygon layer should be created in.</param>
        /// <param name="notFinished">Indicates whether the OnFinishedLoading event should be suppressed after loading finished.</param>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container, bool notFinished)
            : base(featureSet, container, null)
        {
            Configure();
            if (!notFinished) OnFinishedLoading();
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        [ShallowCopy]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BackBuffer { get; set; }

        /// <summary>
        /// Gets or sets the current buffer.
        /// </summary>
        [ShallowCopy]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Buffer { get; set; }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Envelope BufferEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle { get; set; }

        /// <summary>
        /// Gets or sets the label layer that is associated with this polygon layer.
        /// </summary>
        [ShallowCopy]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IMapLabelLayer LabelLayer
        {
            get
            {
                return base.LabelLayer as IMapLabelLayer;
            }

            set
            {
                base.LabelLayer = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the layer component reports progress messages.
        /// </summary>
        [ShallowCopy]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ProgressReportingEnabled { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing. Specifying transparent
        /// will replace content with transparent pixels.</param>
        public void Clear(List<Rectangle> rectangles, Color color)
        {
            if (BackBuffer == null) return;
            Graphics g = Graphics.FromImage(BackBuffer);
            foreach (Rectangle r in rectangles)
            {
                if (r.IsEmpty == false)
                {
                    g.Clip = new Region(r);
                    g.Clear(color);
                }
            }

            g.Dispose();
        }

        /// <summary>
        /// This is testing the idea of using an input parameter type that is marked as out
        /// instead of a return type.
        /// </summary>
        /// <param name="result">The result of the creation</param>
        /// <returns>Boolean, true if a layer can be created</returns>
        public override bool CreateLayerFromSelectedFeatures(out IFeatureLayer result)
        {
            MapPolygonLayer temp;
            bool resultOk = CreateLayerFromSelectedFeatures(out temp);
            result = temp;
            return resultOk;
        }

        /// <summary>
        /// This is the strong typed version of the same process that is specific to geo point layers.
        /// </summary>
        /// <param name="result">The new GeoPointLayer to be created</param>
        /// <returns>Boolean, true if there were any values in the selection</returns>
        public virtual bool CreateLayerFromSelectedFeatures(out MapPolygonLayer result)
        {
            result = null;
            if (Selection == null || Selection.Count == 0) return false;
            FeatureSet fs = Selection.ToFeatureSet();
            result = new MapPolygonLayer(fs);

            return true;
        }

        /// <summary>
        /// If EditMode is true, then this method is used for feature drawing.
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="features">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        /// <param name="selected">Indicates whether to draw the normal colored features or the selection colored features.</param>
        public virtual void DrawFeatures(MapArgs args, List<IFeature> features, List<Rectangle> clipRectangles, bool useChunks, bool selected)
        {
            if (!useChunks)
            {
                DrawFeatures(args, features, selected);
                return;
            }

            int count = features.Count;
            int numChunks = (int)Math.Ceiling(count / (double)ChunkSize);

            for (int chunk = 0; chunk < numChunks; chunk++)
            {
                int numFeatures = ChunkSize;
                if (chunk == numChunks - 1) numFeatures = features.Count - (chunk * ChunkSize);
                DrawFeatures(args, features.GetRange(chunk * ChunkSize, numFeatures), selected);

                if (numChunks > 0 && chunk < numChunks - 1)
                {
                    FinishDrawing();
                    Application.DoEvents();
                    OnBufferChanged(clipRectangles);
                }
            }
        }

        /// <summary>
        /// If EditMode is false, then this method is used for feature drawing.
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="indices">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        /// <param name="selected">Indicates whether to draw the normal colored features or the selection colored features.</param>
        public virtual void DrawFeatures(MapArgs args, List<int> indices, List<Rectangle> clipRectangles, bool useChunks, bool selected)
        {
            if (!useChunks)
            {
                DrawFeatures(args, indices, selected);
                return;
            }

            int count = indices.Count;
            int numChunks = (int)Math.Ceiling(count / (double)ChunkSize);

            for (int chunk = 0; chunk < numChunks; chunk++)
            {
                int numFeatures = ChunkSize;
                if (chunk == numChunks - 1) numFeatures = indices.Count - (chunk * ChunkSize);
                DrawFeatures(args, indices.GetRange(chunk * ChunkSize, numFeatures), selected);

                if (numChunks > 0 && chunk < numChunks - 1)
                {
                    Application.DoEvents();
                    OnBufferChanged(clipRectangles);
                }
            }
        }

        /// <summary>
        /// This will draw any features that intersect this region. To specify the features
        /// directly, use OnDrawFeatures. This will not clear existing buffer content.
        /// For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space</param>
        /// <param name="regions">The geographic regions to draw</param>
        /// <param name="selected">Indicates whether to draw the normal colored features or the selection colored features.</param>
        public virtual void DrawRegions(MapArgs args, List<Extent> regions, bool selected)
        {
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            if (EditMode)
            {
                List<IFeature> drawList = new List<IFeature>();
                drawList = regions.Where(region => region != null).Aggregate(drawList, (current, region) => current.Union(DataSet.Select(region)).ToList());
                DrawFeatures(args, drawList, clipRects, true, selected);
            }
            else
            {
                List<int> drawList = new List<int>();
                List<ShapeRange> shapes = DataSet.ShapeIndices;

                // CGX
                if (shapes != null)
                {
                    for (int shp = 0; shp < shapes.Count; shp++)
                    {
                        foreach (Extent region in regions)
                        {
                            if (!shapes[shp].Extent.Intersects(region)) continue;
                            drawList.Add(shp);
                            break;
                        }
                    }

                    DrawFeatures(args, drawList, clipRects, true, selected);
                }
            }
        }

        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        public void FinishDrawing()
        {
            if (Buffer != null && Buffer != BackBuffer) Buffer.Dispose();
            Buffer = BackBuffer;
        }

        /// <summary>
        /// Copies any current content to the back buffer so that drawing should occur on the
        /// back buffer (instead of the fore-buffer). Calling draw methods without calling this may cause exceptions.
        /// </summary>
        /// <param name="preserve">Boolean, true if the front buffer content should be copied to the back buffer
        /// where drawing will be taking place.</param>
        public void StartDrawing(bool preserve)
        {
            Bitmap backBuffer = new Bitmap(BufferRectangle.Width, BufferRectangle.Height);
            if (Buffer?.Width == backBuffer.Width && Buffer.Height == backBuffer.Height && preserve)
            {
                Graphics g = Graphics.FromImage(backBuffer);
                g.DrawImageUnscaled(Buffer, 0, 0);
            }

            if (BackBuffer != null && BackBuffer != Buffer) BackBuffer.Dispose();
            BackBuffer = backBuffer;
            OnStartDrawing();
        }

        /// <summary>
        /// Fires the OnBufferChanged event
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            BufferChanged?.Invoke(this, new ClipArgs(clipRectangles));
        }

        /// <summary>
        /// A default method to generate a label layer.
        /// </summary>
        protected override void OnCreateLabels()
        {
            LabelLayer = new MapLabelLayer(this);
        }

        /// <summary>
        /// Occurs when a new drawing is started, but after the BackBuffer has been established.
        /// </summary>
        protected virtual void OnStartDrawing()
        {
        }

        /// <summary>
        /// Appends the specified polygon to the graphics path.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="shpx">The shape range.</param>
        /// <param name="borderPath">The border path.</param>
        /// <param name="args">The map arguments.</param>
        /// <param name="shClip">The southerland hodgmen polygon clipper.</param>
        private static void BuildPolygon(double[] vertices, ShapeRange shpx, GraphicsPath borderPath, MapArgs args, SoutherlandHodgman shClip)
        {
            double minX = args.MinX;
            double maxY = args.MaxY;
            double dx = args.Dx;
            double dy = args.Dy;

            for (int prt = 0; prt < shpx.Parts.Count; prt++)
            {
                PartRange prtx = shpx.Parts[prt];
                int start = prtx.StartIndex;
                int end = prtx.EndIndex;
                var points = new List<double[]>(end - start + 1);

                for (int i = start; i <= end; i++)
                {
                    var pt = new[] { (vertices[i * 2] - minX) * dx, (maxY - vertices[(i * 2) + 1]) * dy };
                    points.Add(pt);
                }

                if (shClip != null)
                {
                    points = shClip.Clip(points);
                }

                var intPoints = DuplicationPreventer.Clean(points).ToArray();
                if (intPoints.Length < 2)
                {
                    continue;
                }

                borderPath.StartFigure();
                borderPath.AddLines(intPoints);
            }
        }

        private static Rectangle ComputeClippingRectangle(MapArgs args)
        {
            const int MaxSymbologyFuzz = 50;
            var clipRect = new Rectangle(args.ImageRectangle.Location.X, args.ImageRectangle.Location.Y, args.ImageRectangle.Width, args.ImageRectangle.Height);
            clipRect.Inflate(MaxSymbologyFuzz, MaxSymbologyFuzz);
            return clipRect;
        }

        private void BuildPaths(MapArgs e, IEnumerable<int> indices, out Dictionary<FastDrawnState, GraphicsPath> paths, bool selected)
        {
            paths = new Dictionary<FastDrawnState, GraphicsPath>();

            var indiceList = indices as IList<int> ?? indices.ToList();
            FastDrawnState[] states = DrawnStatesNeeded ? DrawnStates : new FastDrawnState[0];
            if (DrawnStatesNeeded && indiceList.Max() >= states.Length)
            {
                AssignFastDrawnStates();
                states = DrawnStates;
            }

            if (selected && (!DrawnStatesNeeded || !DrawnStates.Any(_ => _.Selected))) return;

            if (ProgressReportingEnabled)
            {
                ProgressMeter = new ProgressMeter(ProgressHandler, "Building Paths", indiceList.Count);
            }

            FastDrawnState state = new FastDrawnState(selected, Symbology.Categories[0]);
            Extent drawExtents = e.GeographicExtents;
            Rectangle clipRect = new Rectangle(0, 0, e.ImageRectangle.Width, e.ImageRectangle.Height);
            SoutherlandHodgman shClip = new SoutherlandHodgman(clipRect);

            List<ShapeRange> shapes = DataSet.ShapeIndices;
            double[] vertices = DataSet.Vertex;

            foreach (int shp in indiceList)
            {
                if (ProgressReportingEnabled) ProgressMeter.Next();
                if (shp >= shapes.Count) return;
                ShapeRange shape = shapes[shp];
                if (!shape.Extent.Intersects(e.GeographicExtents)) continue;

                if (DrawnStatesNeeded)
                {
                    if (!states[shp].Visible || (selected && !states[shp].Selected)) continue;

                    state = new FastDrawnState(selected, states[shp].Category);
                }

                // CGX
                if (Visibility != null && Visibility.Length > shp)
                {
                    bool Visi = Visibility[shp].Visible;
                    if (!Visi) continue;
                } // FIN CGX
                if (!paths.ContainsKey(state))
                {
                    paths.Add(state, new GraphicsPath(FillMode.Winding));
                }

                BuildPolygon(vertices, shapes[shp], paths[state], e, drawExtents.Contains(shape.Extent) ? null : shClip);
            }

            if (ProgressReportingEnabled) ProgressMeter.Reset();
        }

        private void BuildPaths(MapArgs e, IEnumerable<IFeature> features, out Dictionary<FastDrawnState, GraphicsPath> borderPaths, bool selected)
        {
            borderPaths = new Dictionary<FastDrawnState, GraphicsPath>();

            if (selected && !DrawingFilter.DrawnStates.Any(_ => _.Value.IsSelected)) return;

            Rectangle clipRect = ComputeClippingRectangle(e);
            Extent drawExtents = e.PixelToProj(clipRect);
            SoutherlandHodgman shClip = new SoutherlandHodgman(clipRect);

            var featureList = features as IList<IFeature> ?? features.ToList();
            foreach (var category in Symbology.Categories)
            {
                // Determine the subset of the specified features that are visible and match the category
                IFeatureCategory polygonCategory = category;
                Func<IDrawnState, bool> isMember;

                if (selected)
                {
                    // get only selected features
                    isMember = state => state.SchemeCategory == polygonCategory && state.IsVisible && state.IsSelected;
                }
                else
                {
                    // get all features
                    isMember = state => state.SchemeCategory == polygonCategory && state.IsVisible;
                }

                var drawnFeatures = (from feature in featureList where isMember(DrawingFilter[feature]) select feature).ToList();

                if (drawnFeatures.Count > 0)
                {
                    GraphicsPath borderPath = new GraphicsPath();
                    foreach (IFeature f in drawnFeatures)
                    {
                        BuildPolygon(DataSet.Vertex, f.ShapeIndex, borderPath, e, drawExtents.Contains(f.Geometry.EnvelopeInternal) ? null : shClip);
                    }

                    borderPaths.Add(new FastDrawnState(selected, category), borderPath);
                }
            }
        }

        private void Configure()
        {
            ChunkSize = 25000;
            ProgressReportingEnabled = false; // CGX true -> false
        }

        // This draws the individual polygon features
        private void DrawFeatures(MapArgs e, IEnumerable<IFeature> features, bool selected)
        {
            Dictionary<FastDrawnState, GraphicsPath> paths;

            // First, use the coordinates to build the drawing paths
            BuildPaths(e, features, out paths, selected);

            // Next draw all the paths using the various category symbols.
            DrawPaths(e, paths, selected);

            foreach (var path in paths.Values)
            {
                path.Dispose();
            }
        }

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, IEnumerable<int> indices, bool selected)
        {
            if (DataSet.ShapeIndices == null) return;
            Dictionary<FastDrawnState, GraphicsPath> paths;

            // First, use the coordinates to build the drawing paths
            BuildPaths(e, indices, out paths, selected);

            // Next draw all the paths using the various category symbols.
            DrawPaths(e, paths, selected);

            foreach (var path in paths.Values)
            {
                path.Dispose();
            }
        }

        /// <summary>
        /// Draws the GraphicsPaths. Before we were effectively "re-creating" the same geometric.
        /// </summary>
        /// <param name="e">The map arguments.</param>
        /// <param name="paths">The graphics path.</param>
        /// <param name="selected">Indicates whether to draw the normal colored features or the selection colored features.</param>
        private void DrawPaths(MapArgs e, Dictionary<FastDrawnState, GraphicsPath> paths, bool selected)
        {
            Graphics g = e.Device ?? Graphics.FromImage(BackBuffer);

            foreach (var kvp in paths)
            {
                var category = kvp.Key.Category;

                if (kvp.Key.Category == null)
                    continue;

                Extent catBounds = (CategoryExtents.Keys.Contains(category) ? CategoryExtents[category] : CalculateCategoryExtent(category)) ?? Extent;
                var bounds = new RectangleF
                {
                    X = Convert.ToSingle((catBounds.MinX - e.MinX) * e.Dx),
                    Y = Convert.ToSingle((e.MaxY - catBounds.MaxY) * e.Dy)
                };
                float r = Convert.ToSingle((catBounds.MaxX - e.MinX) * e.Dx);
                bounds.Width = r - bounds.X;
                float b = Convert.ToSingle((e.MaxY - catBounds.MinY) * e.Dy);
                bounds.Height = b - bounds.Y;

                var ps = (selected && kvp.Key.Selected ? category.SelectionSymbolizer : category.Symbolizer) as PolygonSymbolizer;
                if (ps == null) continue;

                g.SmoothingMode = ps.GetSmoothingMode();

                foreach (IPattern pattern in ps.Patterns)
                {
                    IGradientPattern gp = pattern as IGradientPattern;
                    if (gp != null)
                    {
                        gp.Bounds = bounds;
                    }

                    pattern.FillPath(g, kvp.Value);
                }

                double scale = ps.GetScale(e);
                // CGX
                if (MapFrame != null && (MapFrame as IMapFrame).ReferenceScale > 1.0 && (MapFrame as IMapFrame).CurrentScale > 0.0)
                {
                    double dReferenceScale = (MapFrame as IMapFrame).ReferenceScale;
                    double dCurrentScale = (MapFrame as IMapFrame).CurrentScale;
                    scale = dReferenceScale / dCurrentScale;
                } // Fin CGX

                foreach (IPattern pattern in ps.Patterns)
                {
                    if (pattern.UseOutline)
                    {
                        pattern.DrawPath(g, kvp.Value, scale);
                    }
                }
            }

            if (e.Device == null) g.Dispose();
        }

        #endregion

        #region CGX
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inFeatureSet"></param>
        public MapPolygonLayer(IFeatureSet inFeatureSet, FastDrawnState[] inVisibility)
            : base(inFeatureSet)
        {
            Configure();
            OnFinishedLoading();

            Visibility = inVisibility;

        }
        FastDrawnState[] _Visibility = null;
        [Serialize("FastDrawnState", ConstructorArgumentIndex = 1)]
        public FastDrawnState[] Visibility
        {
            get
            {



                return _Visibility;
            }

            set
            {

                _Visibility = value;

            }
        }
        public void StoreVisibility()
        {
            Visibility = DrawnStates;
        }

        public void SetVisibility()
        {
            DrawnStatesNeeded = true;
            if (_Visibility != null && DrawnStates != null
                && _Visibility.Length == DrawnStates.Length)
            {
                for (int i = 0; i < _Visibility.Length; i++)
                {
                    DrawnStates[i].Visible = _Visibility[i].Visible;
                }
            }
        }
        #endregion

    }
}