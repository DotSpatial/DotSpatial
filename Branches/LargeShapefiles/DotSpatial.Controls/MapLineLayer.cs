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
    public class MapLineLayer : LineLayer, IMapLineLayer, ISupportChunksDrawing
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
        /// Creates an empty line layer with a Line FeatureSet that has no members.
        /// </summary>
        public MapLineLayer()
        {
            Configure();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <exception cref="LineFeatureTypeException">Thrown if a non-line featureSet is supplied</exception>
        public MapLineLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure();
        }

        /// <summary>
        /// Constructor that also shows progress
        /// </summary>
        /// <param name="featureSet">A featureset that contains lines</param>
        /// <param name="container">An IContainer that the line layer should be created in</param>
        /// <exception cref="LineFeatureTypeException">Thrown if a non-line featureSet is supplied</exception>
        public MapLineLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(featureSet, container, null)
        {
            Configure();
        }

        /// <summary>
        /// Creates a GeoLineLayer constructor, but passes the boolean notFinished variable to indicate
        /// whether or not this layer should fire the FinishedLoading event.
        /// </summary>
        /// <exception cref="LineFeatureTypeException">Thrown if a non-line featureSet is supplied</exception>
        public MapLineLayer(IFeatureSet featureSet, ICollection<ILayer> container, bool notFinished)
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
            MapLineLayer temp;
            bool resultOk = CreateLayerFromSelectedFeatures(out temp);
            result = temp;
            return resultOk;
        }

        /// <summary>
        /// This is the strong typed version of the same process that is specific to geo point layers.
        /// </summary>
        /// <param name="result">The new GeoPointLayer to be created</param>
        /// <returns>Boolean, true if there were any values in the selection</returns>
        public virtual bool CreateLayerFromSelectedFeatures(out MapLineLayer result)
        {
            result = null;
            if (Selection == null || Selection.Count == 0) return false;
            FeatureSet fs = Selection.ToFeatureSet();
            result = new MapLineLayer(fs);
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
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        #region Private Methods

        private void DrawFeatures(MapArgs e, List<int> indices)
        {
            var g = e.Device ?? Graphics.FromImage(BackBuffer);

            var states = DrawnStates;
            foreach (var category in Symbology.Categories)
            {
                Tuple<ILineSymbolizer, double, Rectangle> normalData = null, selectionData = null;
                if (category.Symbolizer != null)
                {
                    normalData = new Tuple<ILineSymbolizer, double, Rectangle>(
                        category.Symbolizer,
                        category.Symbolizer.GetScale(e),
                        ComputeClippingRectangle(e, category.Symbolizer));
                }
                if (category.SelectionSymbolizer != null)
                {
                    selectionData = new Tuple<ILineSymbolizer, double, Rectangle>(
                        category.SelectionSymbolizer,
                        category.SelectionSymbolizer.GetScale(e),
                        ComputeClippingRectangle(e, category.SelectionSymbolizer));
                }

                foreach (var index in indices)
                {
                    var state = states[index];
                    if (!state.Visible) continue;
                    var pc = state.Category as ILineCategory;
                    if (pc == null) continue;
                    if (pc != category) continue;

                    var nd = state.Selected ? selectionData : normalData;
                    if (nd == null) continue;

                    var shape = DataSet.GetShape(index, false);
                    using (var graphPath = new GraphicsPath())
                    {
                        BuildLineString(graphPath, shape.Vertices, shape.Range, e, nd.Item3);
                        foreach (var stroke in nd.Item1.Strokes)
                        {
                            stroke.DrawPath(g, graphPath, nd.Item2);
                        }
                    }
                }
            }
           
            if (e.Device == null) g.Dispose();
        }
   
        private static Rectangle ComputeClippingRectangle(IProj args, ILineSymbolizer ls)
        {
            // Compute a clipping rectangle that accounts for symbology
            int maxLineWidth = 2 * (int)Math.Ceiling(ls.GetWidth());
            var clipRect = new Rectangle(args.ImageRectangle.Location.X, args.ImageRectangle.Location.Y, args.ImageRectangle.Width, args.ImageRectangle.Height);
            clipRect.Inflate(maxLineWidth, maxLineWidth);
            return clipRect;
        }

        private static void BuildLineString(GraphicsPath path, double[] vertices, ShapeRange shpx, MapArgs args, Rectangle clipRect)
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

                List<List<double[]>> multiLinestrings;
                if (!shpx.Extent.Within(args.GeographicExtents))
                {
                    multiLinestrings = CohenSutherland.ClipLinestring(points, clipRect.Left, clipRect.Top,
                        clipRect.Right, clipRect.Bottom);
                }
                else
                {
                    multiLinestrings = new List<List<double[]>> { points };
                }

                foreach (var linestring in multiLinestrings)
                {
                    var intPoints = DuplicationPreventer.Clean(linestring).ToArray();
                    if (intPoints.Length < 2) continue;

                    path.StartFigure();
                    path.AddLines(intPoints);
                }
            }
        }

        #endregion

        #region ISupportChunksDrawing implementation

        int ISupportChunksDrawing.ChunkSize { get { return ChunkSize; } }
        void ISupportChunksDrawing.OnBufferChanged(List<Rectangle> clipRectangles) { OnBufferChanged(clipRectangles); }

        #endregion
    }
}