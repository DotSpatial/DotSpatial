// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles polygon drawing.
    /// </summary>
    public class MapPolygonLayer : PolygonLayer, IMapPolygonLayer
    {
        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Private variables

        const int SELECTED = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new empty MapPolygonLayer with an empty FeatureSet of FeatureType Polygon
        /// </summary>
        public MapPolygonLayer()
            : base(new FeatureSet(FeatureType.Polygon))
        {
            Configure();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inFeatureSet"></param>
        public MapPolygonLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="featureSet">A featureset that contains lines</param>
        /// <param name="container">An IContainer that the line layer should be created in</param>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(featureSet, container, null)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="featureSet">A featureset that contains lines</param>
        /// <param name="container">An IContainer that the line layer should be created in</param>
        /// <param name="notFinished"></param>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container, bool notFinished)
            : base(featureSet, container, null)
        {
            Configure();
            if (notFinished == false) OnFinishedLoading();
        }

        private void Configure()
        {
            ChunkSize = 25000;
            ProgressReportingEnabled = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This will draw any features that intersect this region.  To specify the features
        /// directly, use OnDrawFeatures.  This will not clear existing buffer content.
        /// For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space</param>
        /// <param name="regions">The geographic regions to draw</param>
        public virtual void DrawRegions(MapArgs args, List<Extent> regions)
        {
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            if (EditMode)
            {
                List<IFeature> drawList = new List<IFeature>();
                drawList = regions.Where(region => region != null).Aggregate(drawList, (current, region) => current.Union(DataSet.Select(region)).ToList());
                DrawFeatures(args, drawList, clipRects, true);
            }
            else
            {
                List<int> drawList = new List<int>();
                List<ShapeRange> shapes = DataSet.ShapeIndices;
                for (int shp = 0; shp < shapes.Count; shp++)
                {
                    foreach (Extent region in regions)
                    {
                        if (!shapes[shp].Extent.Intersects(region)) continue;
                        drawList.Add(shp);
                        break;
                    }
                }
                DrawFeatures(args, drawList, clipRects, true);
            }
        }

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing.  Specifying transparent
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
        /// If useChunks is true, then this method
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="features">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified,
        ///  this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        public virtual void DrawFeatures(MapArgs args, List<IFeature> features, List<Rectangle> clipRectangles, bool useChunks)
        {
            if (useChunks == false)
            {
                DrawFeatures(args, features);
                return;
            }

            int count = features.Count;
            int numChunks = (int)Math.Ceiling(count / (double)ChunkSize);

            for (int chunk = 0; chunk < numChunks; chunk++)
            {
                int numFeatures = ChunkSize;
                if (chunk == numChunks - 1) numFeatures = features.Count - (chunk * ChunkSize);
                DrawFeatures(args, features.GetRange(chunk * ChunkSize, numFeatures));

                if (numChunks > 0 && chunk < numChunks - 1)
                {
                    FinishDrawing();
                    Application.DoEvents();
                    OnBufferChanged(clipRectangles);
                    //this.StartDrawing();
                }
            }
        }

        /// <summary>
        /// If useChunks is true, then this method
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="indices">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        public virtual void DrawFeatures(MapArgs args, List<int> indices, List<Rectangle> clipRectangles, bool useChunks)
        {
            if (useChunks == false)
            {
                DrawFeatures(args, indices);
                return;
            }

            int count = indices.Count;
            int numChunks = (int)Math.Ceiling(count / (double)ChunkSize);

            for (int chunk = 0; chunk < numChunks; chunk++)
            {
                int numFeatures = ChunkSize;
                if (chunk == numChunks - 1) numFeatures = indices.Count - (chunk * ChunkSize);
                DrawFeatures(args, indices.GetRange(chunk * ChunkSize, numFeatures));

                if (numChunks > 0 && chunk < numChunks - 1)
                {
                    // FinishDrawing();
                    Application.DoEvents();
                    OnBufferChanged(clipRectangles);
                    // this.StartDrawing();
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
        /// back buffer (instead of the fore-buffer).  Calling draw methods without
        /// calling this may cause exceptions.
        /// </summary>
        /// <param name="preserve">Boolean, true if the front buffer content should be copied to the back buffer
        /// where drawing will be taking place.</param>
        public void StartDrawing(bool preserve)
        {
            Bitmap backBuffer = new Bitmap(BufferRectangle.Width, BufferRectangle.Height);
            if (Buffer != null)
            {
                if (Buffer.Width == backBuffer.Width && Buffer.Height == backBuffer.Height)
                {
                    if (preserve)
                    {
                        Graphics g = Graphics.FromImage(backBuffer);
                        g.DrawImageUnscaled(Buffer, 0, 0);
                    }
                }
            }
            if (BackBuffer != null && BackBuffer != Buffer) BackBuffer.Dispose();
            BackBuffer = backBuffer;
            OnStartDrawing();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BackBuffer { get; set; }

        /// <summary>
        /// Gets the current buffer.
        /// </summary>
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Buffer { get; set; }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Envelope BufferEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle { get; set; }

        /// <summary>
        /// true if the layer component reports progress messages, false otherwise
        /// </summary>
        [ShallowCopy,
         Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ProgressReportingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the label layer that is associated with this polygon layer.
        /// </summary>
        [ShallowCopy,
         Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IMapLabelLayer LabelLayer
        {
            get { return base.LabelLayer as IMapLabelLayer; }
            set { base.LabelLayer = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the OnBufferChanged event
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            var h = BufferChanged;
            if (h == null) return;
            h(this, new ClipArgs(clipRectangles));
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

        #endregion

        #region Private Functions

        // This draws the individual polygon features
        private void DrawFeatures(MapArgs e, IEnumerable<IFeature> features)
        {
            List<GraphicsPath> paths;
            // First, use the coordinates to build the drawing paths
            BuildPaths(e, features, out paths);

            // Next draw all the paths using the various category symbols.
            DrawPaths(e, paths);

            foreach (var path in paths)
            {
                path.Dispose();
            }
        }

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, IEnumerable<int> indices)
        {
            if (DataSet.ShapeIndices == null) return;
            List<GraphicsPath> paths;

            // First, use the coordinates to build the drawing paths
            BuildPaths(e, indices, out paths);

            // Next draw all the paths using the various category symbols.
            DrawPaths(e, paths);

            foreach (var path in paths)
            {
                path.Dispose();
            }
        }

        /// <summary>
        /// Draws the GraphicsPaths.  Before we were effectively "re-creating" the same geometric
        /// </summary>
        /// <param name="e"></param>
        /// <param name="paths"></param>
        private void DrawPaths(MapArgs e, IList<GraphicsPath> paths)
        {
            Graphics g = e.Device ?? Graphics.FromImage(BackBuffer);
            int numCategories = Symbology.Categories.Count;

            if (!DrawnStatesNeeded && !EditMode)
            {
                IPolygonSymbolizer ps = Symbolizer;

                g.SmoothingMode = ps.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                Extent catBounds = DataSet.Extent;
                var bounds = new RectangleF
                {
                    X = Convert.ToSingle((catBounds.MinX - e.MinX) * e.Dx),
                    Y = Convert.ToSingle((e.MaxY - catBounds.MaxY) * e.Dy)
                };
                float r = Convert.ToSingle((catBounds.MaxX - e.MinX) * e.Dx);
                bounds.Width = r - bounds.X;
                float b = Convert.ToSingle((e.MaxY - catBounds.MinY) * e.Dy);
                bounds.Height = b - bounds.Y;

                foreach (IPattern pattern in ps.Patterns)
                {
                    IGradientPattern gp = pattern as IGradientPattern;
                    if (gp != null)
                    {
                        gp.Bounds = bounds;
                    }

                    pattern.FillPath(g, paths[0]);
                }

                double scale = 1;
                if (ps.ScaleMode == ScaleMode.Geographic)
                {
                    scale = e.ImageRectangle.Width / e.GeographicExtents.Width;
                }
                foreach (IPattern pattern in ps.Patterns)
                {
                    if (pattern.UseOutline)
                    {
                        pattern.DrawPath(g, paths[0], scale);
                    }
                }
            }
            else
            {
                for (int selectState = 0; selectState < 2; selectState++)
                {
                    int iCategory = 0;
                    foreach (IPolygonCategory category in Symbology.Categories)
                    {
                        Extent catBounds;
                        catBounds = CategoryExtents.Keys.Contains(category) ? CategoryExtents[category] : CalculateCategoryExtent(category);
                        if (catBounds == null) catBounds = Extent;
                        var bounds = new RectangleF
                        {
                            X = Convert.ToSingle((catBounds.MinX - e.MinX) * e.Dx),
                            Y = Convert.ToSingle((e.MaxY - catBounds.MaxY) * e.Dy)
                        };
                        float r = Convert.ToSingle((catBounds.MaxX - e.MinX) * e.Dx);
                        bounds.Width = r - bounds.X;
                        float b = Convert.ToSingle((e.MaxY - catBounds.MinY) * e.Dy);
                        bounds.Height = b - bounds.Y;

                        int index = selectState * numCategories + iCategory;
                        // Define the symbology based on the category and selection state
                        IPolygonSymbolizer ps = category.Symbolizer;
                        if (selectState == SELECTED) ps = category.SelectionSymbolizer;

                        g.SmoothingMode = ps.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;

                        foreach (IPattern pattern in ps.Patterns)
                        {
                            IGradientPattern gp = pattern as IGradientPattern;
                            if (gp != null)
                            {
                                gp.Bounds = bounds;
                            }
                            if (paths[index] != null)
                            {
                                paths[index].FillMode = FillMode.Winding;
                                pattern.FillPath(g, paths[index]);
                            }
                        }

                        double scale = 1;
                        if (ps.ScaleMode == ScaleMode.Geographic)
                        {
                            scale = e.ImageRectangle.Width / e.GeographicExtents.Width;
                        }
                        foreach (IPattern pattern in ps.Patterns)
                        {
                            if (pattern.UseOutline)
                            {
                                pattern.DrawPath(g, paths[index], scale);
                            }
                        }
                        iCategory++;
                    } // category
                } // selectState
            }

            if (e.Device == null) g.Dispose();
        }

        private void BuildPaths(MapArgs e, IEnumerable<int> indices, out List<GraphicsPath> paths)
        {
            paths = new List<GraphicsPath>();
            Extent drawExtents = e.GeographicExtents;
            Rectangle clipRect = e.ProjToPixel(e.GeographicExtents);
            SoutherlandHodgman shClip = new SoutherlandHodgman(clipRect);

            List<GraphicsPath> graphPaths = new List<GraphicsPath>();
            Dictionary<FastDrawnState, GraphicsPath> borders = new Dictionary<FastDrawnState, GraphicsPath>();
            for (int selectState = 0; selectState < 2; selectState++)
            {
                foreach (IPolygonCategory category in Symbology.Categories)
                {
                    FastDrawnState state = new FastDrawnState(selectState == 1, category);

                    GraphicsPath border = new GraphicsPath();
                    borders.Add(state, border);
                    graphPaths.Add(border);
                }
            }

            paths.AddRange(graphPaths);

            List<ShapeRange> shapes = DataSet.ShapeIndices;
            double[] vertices = DataSet.Vertex;

            if (ProgressReportingEnabled)
            {
                ProgressMeter = new ProgressMeter(ProgressHandler, "Building Paths", indices.Count());
            }

            if (!DrawnStatesNeeded)
            {
                FastDrawnState state = new FastDrawnState(false, Symbology.Categories[0]);

                foreach (int shp in indices)
                {
                    if (ProgressReportingEnabled) ProgressMeter.Next();
                    ShapeRange shape = shapes[shp];
                    if (!shape.Extent.Intersects(e.GeographicExtents)) return;
                    if (shp >= shapes.Count) return;
                    if (!borders.ContainsKey(state)) return;

                    BuildPolygon(vertices, shapes[shp], borders[state], e, drawExtents.Contains(shape.Extent) ? null : shClip);
                }
            }
            else
            {
                FastDrawnState[] states = DrawnStates;
                foreach (GraphicsPath borderPath in borders.Values)
                {
                    if (borderPath != null)
                    {
                        borderPath.FillMode = FillMode.Winding;
                    }
                }

                foreach (int shp in indices)
                {
                    if (ProgressReportingEnabled) ProgressMeter.Next();
                    if (shp >= shapes.Count) return;
                    if (shp >= states.Length)
                    {
                        AssignFastDrawnStates();
                        states = DrawnStates;
                    }
                    if (states[shp].Visible == false) continue;
                    ShapeRange shape = shapes[shp];
                    if (!shape.Extent.Intersects(e.GeographicExtents)) continue;
                    if (drawExtents.Contains(shape.Extent))
                    {
                        FastDrawnState state = states[shp];
                        if (!borders.ContainsKey(state)) continue;
                        BuildPolygon(vertices, shapes[shp], borders[state], e, null);
                    }
                    else
                    {
                        FastDrawnState state = states[shp];
                        if (!borders.ContainsKey(state)) continue;
                        BuildPolygon(vertices, shapes[shp], borders[state], e, shClip);
                    }
                }
            }
            if (ProgressReportingEnabled) ProgressMeter.Reset();
        }

        private void BuildPaths(MapArgs e, IEnumerable<IFeature> features, out List<GraphicsPath> borderPaths)
        {
            borderPaths = new List<GraphicsPath>();
            Rectangle clipRect = ComputeClippingRectangle(e);
            Extent drawExtents = e.PixelToProj(clipRect);
            SoutherlandHodgman shClip = new SoutherlandHodgman(clipRect);

            for (int selectState = 0; selectState < 2; selectState++)
            {
                foreach (IPolygonCategory category in Symbology.Categories)
                {
                    // Determine the subset of the specified features that are visible and match the category
                    IPolygonCategory polygonCategory = category;
                    int i = selectState;
                    Func<IDrawnState, bool> isMember = state =>
                                                       state.SchemeCategory == polygonCategory &&
                                                       state.IsVisible &&
                                                       state.IsSelected == (i == 1);

                    var drawnFeatures = from feature in features
                                        where isMember(DrawingFilter[feature])
                                        select feature;

                    GraphicsPath borderPath = new GraphicsPath();
                    foreach (IFeature f in drawnFeatures)
                    {
                        BuildPolygon(DataSet.Vertex, f.ShapeIndex, borderPath, e,
                                     drawExtents.Contains(f.Geometry.EnvelopeInternal) ? null : shClip);
                    }
                    borderPaths.Add(borderPath);
                }
            }
        }

        private static Rectangle ComputeClippingRectangle(MapArgs args)
        {
            const int maxSymbologyFuzz = 50;
            var clipRect = new Rectangle(args.ImageRectangle.Location.X, args.ImageRectangle.Location.Y, args.ImageRectangle.Width, args.ImageRectangle.Height);
            clipRect.Inflate(maxSymbologyFuzz, maxSymbologyFuzz);
            return clipRect;
        }

        /// <summary>
        /// Appends the specified polygon to the graphics path.
        /// </summary>
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
                    var pt = new[]
                    {
                        (vertices[i*2] - minX)*dx,
                        (maxY - vertices[i*2 + 1])*dy
                    };
                    points.Add(pt);
                }
                if (null != shClip)
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

        #endregion
		
    }
}