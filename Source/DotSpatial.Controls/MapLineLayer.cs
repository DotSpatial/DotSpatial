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
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles line drawing.
    /// </summary>
    public class MapLineLayer : LineLayer, IMapLineLayer
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLineLayer"/> class that is empty with a line feature set that has no members.
        /// </summary>
        public MapLineLayer()
            : base(new FeatureSet(FeatureType.Line))
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLineLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">The line feature set used as data source.</param>
        public MapLineLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLineLayer"/> class.
        /// </summary>
        /// <param name="featureSet">A featureset that contains lines.</param>
        /// <param name="container">An IContainer that the line layer should be created in.</param>
        public MapLineLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(featureSet, container, null)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLineLayer"/> class, but passes the boolean
        /// notFinished variable to indicate whether or not this layer should fire the FinishedLoading event.
        /// </summary>
        /// <param name="featureSet">The line feature set used as data source.</param>
        /// <param name="container">An IContainer that the line layer should be created in.</param>
        /// <param name="notFinished">Indicates whether the OnFinishedLoading event should be suppressed after loading finished.</param>
        public MapLineLayer(IFeatureSet featureSet, ICollection<ILayer> container, bool notFinished)
            : base(featureSet, container, null)
        {
            Configure();
            if (notFinished == false) OnFinishedLoading();
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
        [ShallowCopy]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Envelope BufferEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [ShallowCopy]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle { get; set; }

        /// <summary>
        /// Gets or sets the label layer that is associated with this line layer.
        /// </summary>
        [ShallowCopy]
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
        /// Gets an integer number of chunks for this layer.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NumChunks
        {
            get
            {
                if (DrawingFilter == null) return 0;
                return DrawingFilter.NumChunks;
            }
        }

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
        /// <param name="result">The result of the creation.</param>
        /// <returns>Boolean, true if a layer can be created.</returns>
        public override bool CreateLayerFromSelectedFeatures(out IFeatureLayer result)
        {
            bool resultOk = CreateLayerFromSelectedFeatures(out MapLineLayer temp);
            result = temp;
            return resultOk;
        }

        /// <summary>
        /// This is the strong typed version of the same process that is specific to geo point layers.
        /// </summary>
        /// <param name="result">The new GeoPointLayer to be created.</param>
        /// <returns>Boolean, true if there were any values in the selection.</returns>
        public virtual bool CreateLayerFromSelectedFeatures(out MapLineLayer result)
        {
            result = null;
            if (Selection == null || Selection.Count == 0) return false;
            FeatureSet fs = Selection.ToFeatureSet();
            result = new MapLineLayer(fs);
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
                    OnBufferChanged(clipRectangles);
                    Application.DoEvents();
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
                    OnBufferChanged(clipRectangles);
                    Application.DoEvents();
                }
            }
        }

        /// <summary>
        /// This will draw any features that intersect this region. To specify the features
        /// directly, use OnDrawFeatures. This will not clear existing buffer content.
        /// For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space.</param>
        /// <param name="regions">The geographic regions to draw.</param>
        /// <param name="selected">Indicates whether to draw the normal colored features or the selection colored features.</param>
        public virtual void DrawRegions(MapArgs args, List<Extent> regions, bool selected)
        {
            // First determine the number of features we are talking about based on region.
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            if (EditMode)
            {
                List<IFeature> drawList = new List<IFeature>();
                foreach (Extent region in regions)
                {
                    if (region != null)
                    {
                        // Use union to prevent duplicates. No sense in drawing more than we have to.
                        drawList = drawList.Union(DataSet.Select(region)).ToList();
                    }
                }

                DrawFeatures(args, drawList, clipRects, true, selected);
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

                DrawFeatures(args, drawList, clipRects, true, selected);
            }
        }

        /// <summary>
        /// Builds a linestring into the graphics path, using minX, maxY, dx and dy for the transformations.
        /// </summary>
        /// <param name="path">Graphics path to add the line string to.</param>
        /// <param name="ls">LineString that gets added.</param>
        /// <param name="args">The map arguments.</param>
        /// <param name="clipRect">The clip rectangle.</param>
        internal static void BuildLineString(GraphicsPath path, LineString ls, MapArgs args, Rectangle clipRect)
        {
            double minX = args.MinX;
            double maxY = args.MaxY;
            double dx = args.Dx;
            double dy = args.Dy;

            var points = new List<double[]>();
            double[] previousPoint = null;
            foreach (Coordinate c in ls.Coordinates)
            {
                var pt = new[] { (c.X - minX) * dx, (maxY - c.Y) * dy };
                if (previousPoint == null || previousPoint.Length < 2 || pt[0] != previousPoint[0] || pt[1] != previousPoint[1])
                {
                    points.Add(pt);
                }

                previousPoint = pt;
            }

            AddLineStringToPath(path, args, ls.EnvelopeInternal.ToExtent(), points, clipRect);
        }

        /// <summary>
        /// Adds the line string to the path.
        /// </summary>
        /// <param name="path">Path to add the line string to.</param>
        /// <param name="vertices">Vertices of the line string.</param>
        /// <param name="shpx">Shape range of the line string.</param>
        /// <param name="args">The map arguments.</param>
        /// <param name="clipRect">The clip rectangle.</param>
        internal static void BuildLineString(GraphicsPath path, double[] vertices, ShapeRange shpx, MapArgs args, Rectangle clipRect)
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

                AddLineStringToPath(path, args, shpx.Extent, points, clipRect);
            }
        }

        /// <summary>
        /// Fires the OnBufferChanged event.
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels.</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            if (BufferChanged != null)
            {
                ClipArgs e = new ClipArgs(clipRectangles);
                BufferChanged(this, e);
            }
        }

        /// <summary>
        /// A default method to generate a label layer.
        /// </summary>
        protected override void OnCreateLabels()
        {
            LabelLayer = new MapLabelLayer(this);
        }

        /// <summary>
        /// Indiciates that whatever drawing is going to occur has finished and the contents
        /// are about to be flipped forward to the front buffer.
        /// </summary>
        protected virtual void OnFinishDrawing()
        {
        }

        /// <summary>
        /// Occurs when a new drawing is started, but after the BackBuffer has been established.
        /// </summary>
        protected virtual void OnStartDrawing()
        {
        }

        /// <summary>
        /// Adds the given points to the given path.
        /// </summary>
        /// <param name="path">Path the points get added to.</param>
        /// <param name="args">MapArgs used for clipping.</param>
        /// <param name="extent">Extent of the feature used for clipping.</param>
        /// <param name="points">Points that get added to the path.</param>
        /// <param name="clipRect">The clipping rectangle.</param>
        private static void AddLineStringToPath(GraphicsPath path, MapArgs args, Extent extent, List<double[]> points, Rectangle clipRect)
        {
            List<List<double[]>> multiLinestrings;
            if (!extent.Within(args.GeographicExtents))
            {
                multiLinestrings = CohenSutherland.ClipLinestring(points, clipRect.Left, clipRect.Top, clipRect.Right, clipRect.Bottom);
            }
            else
            {
                multiLinestrings = new List<List<double[]>>
                                   {
                                       points
                                   };
            }

            foreach (List<double[]> linestring in multiLinestrings)
            {
                var intPoints = DuplicationPreventer.Clean(linestring).ToArray();
                if (intPoints.Length < 2)
                {
                    continue;
                }

                path.StartFigure();
                path.AddLines(intPoints);
            }
        }

        private static Rectangle ComputeClippingRectangle(MapArgs args, ILineSymbolizer ls)
        {
            // Compute a clipping rectangle that accounts for symbology
            int maxLineWidth = 2 * (int)Math.Ceiling(ls.GetWidth());
            Rectangle clipRect = args.ProjToPixel(args.GeographicExtents); // use GeographicExtent for clipping because ImageRect clips to much
            clipRect.Inflate(maxLineWidth, maxLineWidth);
            return clipRect;
        }

        /// <summary>
        /// Gets the indices of the features that get drawn.
        /// </summary>
        /// <param name="indices">Indices of all the features that could be drawn.</param>
        /// <param name="states">FastDrawnStates of the features.</param>
        /// <param name="category">Category the features must have to get drawn.</param>
        /// <param name="selected">Indicates whether only the selected features get drawn.</param>
        /// <returns>List of the indices of the features that get drawn.</returns>
        private static List<int> GetFeatures(IList<int> indices, FastDrawnState[] states, ILineCategory category, bool selected)
        {
            List<int> drawnFeatures = new List<int>();

            foreach (int index in indices)
            {
                if (index >= states.Length) break;
                FastDrawnState state = states[index];

                if (selected)
                {
                    if (state.Category == category && state.Visible && state.Selected)
                    {
                        drawnFeatures.Add(index);
                    }
                }
                else
                {
                    if (state.Category == category && state.Visible)
                    {
                        drawnFeatures.Add(index);
                    }
                }
            }

            return drawnFeatures;
        }

        private void Configure()
        {
            BufferRectangle = new Rectangle(0, 0, 3000, 3000);
            ChunkSize = 50000;
        }

        private void DrawFeatures(MapArgs e, IEnumerable<int> indices, bool selected)
        {
            if (selected && !DrawnStatesNeeded) return;

            Graphics g = e.Device ?? Graphics.FromImage(BackBuffer);
            var indiceList = indices as IList<int> ?? indices.ToList();

            Action<GraphicsPath, Rectangle, IEnumerable<int>> drawFeature = (graphPath, clipRect, features) =>
            {
                foreach (int shp in features)
                {
                    ShapeRange shape = DataSet.ShapeIndices[shp];
                    BuildLineString(graphPath, DataSet.Vertex, shape, e, clipRect);
                }
            };

            if (DrawnStatesNeeded)
            {
                FastDrawnState[] states = DrawnStates;

                if (indiceList.Max() >= states.Length)
                {
                    AssignFastDrawnStates();
                    states = DrawnStates;
                }

                if (selected && !states.Any(_ => _.Selected)) return;

                foreach (ILineCategory category in Symbology.Categories)
                {
                    // Define the symbology based on the category and selection state
                    ILineSymbolizer ls = selected ? category.SelectionSymbolizer : category.Symbolizer;
                    var features = GetFeatures(indiceList, states, category, selected);
                    DrawPath(g, ls, e, drawFeature, features);
                }
            }
            else
            {
                // Selection state is disabled and there is only one category
                ILineSymbolizer ls = Symbology.Categories[0].Symbolizer;
                DrawPath(g, ls, e, drawFeature, indiceList);
            }

            if (e.Device == null) g.Dispose();
        }

        /// <summary>
        /// Draws the path that results from the given indices.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the list.</typeparam>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="ls">LineSymbolizer used for drawing.</param>
        /// <param name="e">MapArgs needed for computation.</param>
        /// <param name="action">Action that is used to add the elements to the graphics path that gets drawn.</param>
        /// <param name="list">List that contains the elements that get drawn.</param>
        private void DrawPath<T>(Graphics g, ILineSymbolizer ls, MapArgs e, Action<GraphicsPath, Rectangle, IEnumerable<T>> action, IEnumerable<T> list)
        {
            g.SmoothingMode = ls.GetSmoothingMode();

            Rectangle clipRect = ComputeClippingRectangle(e, ls);

            // Determine the subset of the specified features that are visible and match the category
            using (GraphicsPath graphPath = new GraphicsPath())
            {
                action(graphPath, clipRect, list);

                double scale = ls.GetScale(e);
                foreach (IStroke stroke in ls.Strokes)
                {
                    stroke.DrawPath(g, graphPath, scale);
                }
            }
        }

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, IEnumerable<IFeature> features, bool selected)
        {
            if (selected && !DrawingFilter.DrawnStates.Any(_ => _.Value.IsSelected)) return;

            Graphics g = e.Device ?? Graphics.FromImage(BackBuffer);
            var featureList = features as IList<IFeature> ?? features.ToList();

            Action<GraphicsPath, Rectangle, IEnumerable<IFeature>> drawFeature = (graphPath, clipRect, featList) =>
            {
                foreach (IFeature f in featList)
                {
                    var geo = f.Geometry as LineString;
                    if (geo != null)
                    {
                        BuildLineString(graphPath, geo, e, clipRect);
                    }
                    else
                    {
                        var col = f.Geometry as GeometryCollection;
                        if (col != null)
                        {
                            foreach (var c1 in col.Geometries.OfType<LineString>())
                            {
                                BuildLineString(graphPath, c1, e, clipRect);
                            }
                        }
                    }
                }
            };

            foreach (ILineCategory category in Symbology.Categories)
            {
                // Define the symbology based on the category and selection state
                ILineSymbolizer ls = selected ? category.SelectionSymbolizer : category.Symbolizer;

                // Determine the subset of the specified features that are visible and match the category
                ILineCategory lineCategory = category;
                Func<IDrawnState, bool> isMember;

                if (selected)
                {
                    // get only selected features
                    isMember = state => state.SchemeCategory == lineCategory && state.IsVisible && state.IsSelected;
                }
                else
                {
                    // get all features
                    isMember = state => state.SchemeCategory == lineCategory && state.IsVisible;
                }

                var drawnFeatures = from feature in featureList where isMember(DrawingFilter[feature]) select feature;
                DrawPath(g, ls, e, drawFeature, drawnFeatures);
            }

            if (e.Device == null) g.Dispose();
        }

        #endregion
    }
}