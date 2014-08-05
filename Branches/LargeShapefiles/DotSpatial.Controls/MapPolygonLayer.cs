// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public class MapPolygonLayer : PolygonLayer, IMapPolygonLayer, ISupportChunksDrawing
    {
        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new empty MapPolygonLayer with an empty FeatureSet of FeatureType Polygon
        /// </summary>
        public MapPolygonLayer()
        {
            Configure();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        ///<exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public MapPolygonLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure();
        }

        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="featureSet">A featureset that contains lines</param>
        /// <param name="container">An IContainer that the line layer should be created in</param>
        ///<exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(featureSet, container, null)
        {
            Configure();
        }

        /// <summary>
        /// Creates a new instance of the polygon layer where the container is specified
        /// </summary>
        ///<exception cref="PolygonFeatureTypeException">Thrown if a non-polygon featureset is supplied.</exception>
        public MapPolygonLayer(IFeatureSet featureSet, ICollection<ILayer> container, bool notFinished)
            : base(featureSet, container, null)
        {
            Configure(notFinished);
        }

        private void Configure(bool notFinished = false)
        {
            ChunkSize = 50000;
            if (notFinished == false) OnFinishedLoading();
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
            // First determine the number of features we are talking about based on region.
            var clipRects = args.ProjToPixel(regions);

            var drawList = new List<int>();
            for (var shp = 0; shp < DataSet.Count; shp++)
            {
                var pointExtent = DataSet.GetFeatureExtent(shp);
                if (regions.Any(pointExtent.Intersects))
                {
                    drawList.Add(shp);
                }
            }
            DrawFeatures(args, drawList, clipRects, true);
        }

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing.  Specifying transparent
        /// will replace content with transparent pixels.</param>
        public void Clear(IEnumerable<Rectangle> rectangles, Color color)
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
        /// Draw features
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="indices">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        public virtual void DrawFeatures(MapArgs args, List<int> indices, List<Rectangle> clipRectangles, bool useChunks)
        {
            this.DrawUsingChunks(args, indices, clipRectangles, useChunks, DrawFeatures);
        }

        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        public void FinishDrawing()
        {
            OnFinishDrawing();
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
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of features that will be rendered before
        /// refreshing the screen.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ChunkSize { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the OnBufferChanged event
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            var h = BufferChanged;
            if (h != null) h(this, new ClipArgs(clipRectangles));
        }

        /// <summary>
        /// Occurs when a new drawing is started, but after the BackBuffer has been established.
        /// </summary>
        protected virtual void OnStartDrawing()
        {
        }

        /// <summary>
        /// Indiciates that whatever drawing is going to occur has finished and the contents
        /// are about to be flipped forward to the front buffer.
        /// </summary>
        protected virtual void OnFinishDrawing()
        {
        }


        #endregion

        #region Private Functions

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, List<int> indices)
        {
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
     
        private void DrawPaths(MapArgs e, IList<GraphicsPath> paths)
        {
            var g = e.Device ?? Graphics.FromImage(BackBuffer);

            int numCategories = Symbology.Categories.Count;
            for (int selectState = 0; selectState < 2; selectState++)
            {
                int iCategory = 0;
                foreach (IPolygonCategory category in Symbology.Categories)
                {
                    var catBounds = (CategoryExtents.Keys.Contains(category) ? CategoryExtents[category] : CalculateCategoryExtent(category)) ??
                                    Extent;
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
                    if (selectState == 1) ps = category.SelectionSymbolizer;
                    g.SmoothingMode = ps.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;

                    foreach (var pattern in ps.Patterns)
                    {
                        var gp = pattern as IGradientPattern;
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

                    var scale = ps.GetScale(e);
                    foreach (IPattern pattern in ps.Patterns)
                    {
                        if (pattern.UseOutline)
                        {
                            pattern.DrawPath(g, paths[index], scale);
                        }
                    }
                    iCategory++;
                }
            }
        
            if (e.Device == null) g.Dispose();
        }

        private void BuildPaths(MapArgs e, IEnumerable<int> indices, out List<GraphicsPath> paths)
        {
            paths = new List<GraphicsPath>();
            var clipRect = ComputeClippingRectangle(e);
            var drawExtents = e.PixelToProj(clipRect);
            var shClip = new SoutherlandHodgman(clipRect);

            var graphPaths = new List<GraphicsPath>();
            var borders = new Dictionary<FastDrawnState, GraphicsPath>();
            for (var selectState = 0; selectState < 2; selectState++)
            {
                foreach (var category in Symbology.Categories)
                {
                    var state = new FastDrawnState(selectState == 1, category);
                    var border = new GraphicsPath {FillMode = FillMode.Winding};
                    borders.Add(state, border);
                    graphPaths.Add(border);
                }
            }

            paths.AddRange(graphPaths);

            var states = DrawnStates;
            foreach (var shp in indices)
            {
                var state = states[shp];
                if (!state.Visible) continue;
                if (!borders.ContainsKey(state)) continue;

                var shape = DataSet.GetShape(shp, false);
                if (!shape.Range.Extent.Intersects(e.GeographicExtents)) continue;

                BuildPolygon(shape.Vertices, shape.Range, borders[state], e,
                    drawExtents.Contains(shape.Range.Extent) ? null : shClip);
            }
        }

        private static Rectangle ComputeClippingRectangle(IProj args)
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

            foreach (var prtx in shpx.Parts)
            {
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

        #region ISupportChunksDrawing implementation

        int ISupportChunksDrawing.ChunkSize { get { return ChunkSize; } }
        void ISupportChunksDrawing.OnBufferChanged(List<Rectangle> clipRectangles) { OnBufferChanged(clipRectangles); }

        #endregion
    }
}