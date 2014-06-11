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
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Modified to do 3D in January 2008 by Ted Dunsford
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
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles point drawing
    /// </summary>
    public class MapPointLayer : PointLayer, IMapPointLayer
    {
        #region Events

        /// <summary>
        /// Occurs when drawing content has changed on the buffer for this layer
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// This creates a blank MapPointLayer with the DataSet set to an empty new featureset of the Point featuretype.
        /// </summary>
        public MapPointLayer()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new instance of a GeoPointLayer without sending any status messages
        /// </summary>
        /// <param name="featureSet">The IFeatureLayer of data values to turn into a graphical GeoPointLayer</param>
        public MapPointLayer(IFeatureSet featureSet)
            : base(featureSet)
        {
            // this simply handles the default case where no status messages are requested
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Creates a new instance of the point layer where the container is specified
        /// </summary>
        /// <param name="featureSet"></param>
        /// <param name="container"></param>
        public MapPointLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(featureSet, container, null)
        {
            Configure();
            OnFinishedLoading();
        }

        /// <summary>
        /// Creates a new instance of the point layer where the container is specified
        /// </summary>
        /// <param name="featureSet"></param>
        /// <param name="container"></param>
        /// <param name="notFinished"></param>
        public MapPointLayer(IFeatureSet featureSet, ICollection<ILayer> container, bool notFinished)
            : base(featureSet, container, null)
        {
            Configure();
            if (notFinished == false) OnFinishedLoading();
        }

        private void Configure()
        {
            ChunkSize = 50000;
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
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            if (EditMode)
            {
                List<IFeature> drawList = new List<IFeature>();
                foreach (Extent region in regions)
                {
                    if (region != null)
                    {
                        // Use union to prevent duplicates.  No sense in drawing more than we have to.
                        drawList = drawList.Union(DataSet.Select(region)).ToList();
                    }
                }
                DrawFeatures(args, drawList, clipRects, true);
            }
            else
            {
                List<int> drawList = new List<int>();
                double[] verts = DataSet.Vertex;

                if (DataSet.FeatureType == FeatureType.Point)
                {
                    for (int shp = 0; shp < verts.Length / 2; shp++)
                    {
                        foreach (Extent extent in regions)
                        {
                            if (extent.Intersects(verts[shp * 2], verts[shp * 2 + 1]))
                            {
                                drawList.Add(shp);
                            }
                        }
                    }
                }
                else
                {
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
            MapPointLayer temp;
            bool resultOk = CreateLayerFromSelectedFeatures(out temp);
            result = temp;
            return resultOk;
        }

        /// <summary>
        /// This is the strong typed version of the same process that is specific to geo point layers.
        /// </summary>
        /// <param name="result">The new GeoPointLayer to be created</param>
        /// <returns>Boolean, true if there were any values in the selection</returns>
        public virtual bool CreateLayerFromSelectedFeatures(out MapPointLayer result)
        {
            result = null;
            if (Selection == null || Selection.Count == 0) return false;
            FeatureSet fs = Selection.ToFeatureSet();
            result = new MapPointLayer(fs);
            return true;
        }

        /// <summary>
        /// If useChunks is true, then this method
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="features">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        public virtual void DrawFeatures(MapArgs args, List<IFeature> features, List<Rectangle> clipRectangles, bool useChunks)
        {
            if (useChunks == false || features.Count < ChunkSize)
            {
                DrawFeatures(args, features);
                return;
            }

            int count = features.Count;
            int numChunks = (int)Math.Ceiling(count / (double)ChunkSize);
            for (int chunk = 0; chunk < numChunks; chunk++)
            {
                int groupSize = ChunkSize;
                if (chunk == numChunks - 1) groupSize = count - chunk * ChunkSize;
                List<IFeature> subset = features.GetRange(chunk * ChunkSize, groupSize);
                DrawFeatures(args, subset);
                if (numChunks <= 0 || chunk >= numChunks - 1) continue;
                FinishDrawing();
                OnBufferChanged(clipRectangles);
                Application.DoEvents();
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
                    OnBufferChanged(clipRectangles);
                    Application.DoEvents();
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

        #region Protected Methods

        /// <summary>
        /// Fires the OnBufferChanged event
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels</param>
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

        #endregion

        #region Private  Methods

        // This draws the individual point features
        private void DrawFeatures(MapArgs e, IEnumerable<int> indices)
        {
            Graphics g = e.Device ?? Graphics.FromImage(BackBuffer);
            Matrix origTransform = g.Transform;
            FeatureType featureType = DataSet.FeatureType;

            double minX = e.MinX;
            double maxY = e.MaxY;
            double dx = e.Dx;
            double dy = e.Dy;

            if (!DrawnStatesNeeded)
            {
                if (Symbology == null || Symbology.Categories.Count == 0) return;
                FastDrawnState state = new FastDrawnState(false, Symbology.Categories[0]);
                IPointCategory pc = state.Category as IPointCategory;
                IPointSymbolizer ps = null;
                if (pc != null && pc.Symbolizer != null) ps = pc.Symbolizer;
                if (ps == null) return;
                g.SmoothingMode = ps.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                double[] vertices = DataSet.Vertex;
                foreach (int index in indices)
                {
                    if (DrawnStates != null && DrawnStates.Length > index)
                    {
                        if (!DrawnStates[index].Visible) continue;
                    }
                    if (featureType == FeatureType.Point)
                    {
                        var pt = new Point
                        {
                            X = Convert.ToInt32((vertices[index*2] - minX)*dx),
                            Y = Convert.ToInt32((maxY - vertices[index*2 + 1])*dy)
                        };
                        double scaleSize = 1;
                        if (ps.ScaleMode == ScaleMode.Geographic)
                        {
                            scaleSize = e.ImageRectangle.Width / e.GeographicExtents.Width;
                        }
                        Matrix shift = origTransform.Clone();
                        shift.Translate(pt.X, pt.Y);
                        g.Transform = shift;
                        ps.Draw(g, scaleSize);
                    }
                    else
                    {
                        // multi-point
                        ShapeRange range = DataSet.ShapeIndices[index];
                        for (int i = range.StartIndex; i <= range.EndIndex(); i++)
                        {
                            var pt = new Point
                            {
                                X = Convert.ToInt32((vertices[i*2] - minX)*dx),
                                Y = Convert.ToInt32((maxY - vertices[i*2 + 1])*dy)
                            };
                            double scaleSize = 1;
                            if (ps.ScaleMode == ScaleMode.Geographic)
                            {
                                scaleSize = e.ImageRectangle.Width / e.GeographicExtents.Width;
                            }
                            Matrix shift = origTransform.Clone();
                            shift.Translate(pt.X, pt.Y);
                            g.Transform = shift;
                            ps.Draw(g, scaleSize);
                        }
                    }
                }
            }
            else
            {
                FastDrawnState[] states = DrawnStates;
                double[] vertices = DataSet.Vertex;
                foreach (IPointCategory category in Symbology.Categories)
                {
                    if (category.Symbolizer == null) continue;

                    double scaleSize = 1;
                    if (category.Symbolizer.ScaleMode == ScaleMode.Geographic)
                    {
                        scaleSize = e.ImageRectangle.Width / e.GeographicExtents.Width;
                    }
                    Size2D size = category.Symbolizer.GetSize();
                    if (size.Width * scaleSize < 1 || size.Height * scaleSize < 1) continue;

                    Bitmap normalSymbol = new Bitmap((int)(size.Width * scaleSize) + 1, (int)(size.Height * scaleSize) + 1);
                    Graphics bg = Graphics.FromImage(normalSymbol);
                    bg.SmoothingMode = category.Symbolizer.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                    Matrix trans = bg.Transform;

                    // keenedge:
                    // added ' * scaleSize ' to fix a problme when ploted using ScaleMode=Geographic.   however, it still
                    // appeared to be shifted up and left by 1 pixel so I also added the one pixel shift to the NW.
                    // trans.Translate((float)size.Width / 2, (float)size.Height / 2);
                    trans.Translate(((float)(size.Width * scaleSize) / 2 - 1), (float)(size.Height * scaleSize) / 2 - 1);
                    bg.Transform = trans;
                    category.Symbolizer.Draw(bg, 1);

                    Size2D selSize = category.SelectionSymbolizer.GetSize();
                    if (selSize.Width * scaleSize < 1 || selSize.Height * scaleSize < 1) continue;

                    Bitmap selectedSymbol = new Bitmap((int)(selSize.Width * scaleSize + 1), (int)(selSize.Height * scaleSize + 1));
                    Graphics sg = Graphics.FromImage(selectedSymbol);
                    sg.SmoothingMode = category.SelectionSymbolizer.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                    Matrix trans2 = sg.Transform;
                    trans2.Translate((float)selSize.Width / 2, (float)selSize.Height / 2);
                    sg.Transform = trans2;
                    category.SelectionSymbolizer.Draw(sg, 1);

                    foreach (int index in indices)
                    {
                        FastDrawnState state = states[index];
                        if (!state.Visible) continue;
                        if (state.Category == null) continue;
                        IPointCategory pc = state.Category as IPointCategory;
                        if (pc == null) continue;
                        if (pc != category) continue;
                        Bitmap bmp = normalSymbol;
                        if (state.Selected)
                        {
                            bmp = selectedSymbol;
                        }
                        if (featureType == FeatureType.Point)
                        {
                            var pt = new Point
                            {
                                X = Convert.ToInt32((vertices[index*2] - minX)*dx),
                                Y = Convert.ToInt32((maxY - vertices[index*2 + 1])*dy)
                            };

                            Matrix shift = origTransform.Clone();
                            shift.Translate(pt.X, pt.Y);
                            g.Transform = shift;

                            g.DrawImageUnscaled(bmp, -bmp.Width / 2, -bmp.Height / 2);
                        }
                        else
                        {
                            ShapeRange range = DataSet.ShapeIndices[index];
                            for (int i = range.StartIndex; i <= range.EndIndex(); i++)
                            {
                                var pt = new Point
                                {
                                    X = Convert.ToInt32((vertices[i*2] - minX)*dx),
                                    Y = Convert.ToInt32((maxY - vertices[i*2 + 1])*dy)
                                };

                                Matrix shift = origTransform.Clone();
                                shift.Translate(pt.X, pt.Y);
                                g.Transform = shift;
                                g.DrawImageUnscaled(bmp, -bmp.Width / 2, -bmp.Height / 2);
                            }
                        }
                    }
                }
            }

            if (e.Device == null) g.Dispose();
            else g.Transform = origTransform;
        }

        // This draws the individual point features
        private void DrawFeatures(MapArgs e, IEnumerable<IFeature> features)
        {
            Graphics g = e.Device ?? Graphics.FromImage(BackBuffer);
            Matrix origTransform = g.Transform;
            double minX = e.MinX;
            double maxY = e.MaxY;
            double dx = e.Dx;
            double dy = e.Dy;
            IDictionary<IFeature, IDrawnState> states = DrawingFilter.DrawnStates;
            if (states == null) return;
            foreach (IPointCategory category in Symbology.Categories)
            {
                foreach (IFeature feature in features)
                {
                    if (states.ContainsKey(feature) == false) continue;
                    IDrawnState ds = states[feature];
                    if (ds == null) continue;
                    if (!ds.IsVisible) continue;
                    if (ds.SchemeCategory == null) continue;
                    IPointCategory pc = ds.SchemeCategory as IPointCategory;
                    if (pc == null) continue;
                    if (pc != category) continue;
                    IPointSymbolizer ps = pc.Symbolizer;
                    if (ds.IsSelected)
                    {
                        ps = pc.SelectionSymbolizer;
                    }
                    if (ps == null) continue;
                    g.SmoothingMode = ps.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
                    foreach (Coordinate c in feature.Coordinates)
                    {
                        Point pt = new Point
                                       {
                                           X = Convert.ToInt32((c.X - minX) * dx),
                                           Y = Convert.ToInt32((maxY - c.Y) * dy)
                                       };
                        double scaleSize = 1;
                        if (ps.ScaleMode == ScaleMode.Geographic)
                        {
                            scaleSize = e.ImageRectangle.Width / e.GeographicExtents.Width;
                        }
                        Matrix shift = origTransform.Clone();
                        shift.Translate(pt.X, pt.Y);
                        g.Transform = shift;
                        ps.Draw(g, scaleSize);
                    }
                }
            }

            if (e.Device == null)
            {
                g.Dispose();
            }
            else
            {
                g.Transform = origTransform;
            }
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
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnvelope BufferEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle { get; set; }

        /// <summary>
        /// Gets or sets the label layer that is associated with this point layer.
        /// </summary>
        [ShallowCopy]
        public new IMapLabelLayer LabelLayer
        {
            get { return base.LabelLayer as IMapLabelLayer; }
            set { base.LabelLayer = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Attempts to create a new GeoPointLayer using the specified file.  If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="fileName">A string fileName to create a point layer for.</param>
        /// <param name="progressHandler">Any valid implementation of IProgressHandler for receiving progress messages</param>
        /// <returns>A GeoPointLayer created from the specified fileName.</returns>
        public static new MapPointLayer OpenFile(string fileName, IProgressHandler progressHandler)
        {
            ILayer fl = LayerManager.DefaultLayerManager.OpenLayer(fileName, progressHandler);
            return fl as MapPointLayer;
        }

        /// <summary>
        /// Attempts to create a new GeoPointLayer using the specified file.  If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="fileName">A string fileName to create a point layer for.</param>
        /// <returns>A GeoPointLayer created from the specified fileName.</returns>
        public static new MapPointLayer OpenFile(string fileName)
        {
            IFeatureLayer fl = LayerManager.DefaultLayerManager.OpenVectorLayer(fileName);
            return fl as MapPointLayer;
        }

        #endregion
    }
}