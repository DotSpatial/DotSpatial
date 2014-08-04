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
using DotSpatial.Data;
using DotSpatial.Symbology;
using Point = System.Drawing.Point;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles point drawing
    /// </summary>
    public class MapPointLayer : PointLayer, IMapPointLayer, ISupportChunksDrawing
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
            using (var g = Graphics.FromImage(BackBuffer))
            {
                foreach (var r in rectangles)
                {
                    if (r.IsEmpty == false)
                    {
                        g.Clip = new Region(r);
                        g.Clear(color);
                    }
                }
            }
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
            var resultOk = CreateLayerFromSelectedFeatures(out temp);
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
            var fs = Selection.ToFeatureSet();
            result = new MapPointLayer(fs);
            return true;
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
            var backBuffer = new Bitmap(BufferRectangle.Width, BufferRectangle.Height);
            if (Buffer != null)
            {
                if (Buffer.Width == backBuffer.Width && Buffer.Height == backBuffer.Height)
                {
                    if (preserve)
                    {
                        var g = Graphics.FromImage(backBuffer);
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
            var h = BufferChanged;
            if (h != null) h(this, new ClipArgs(clipRectangles));
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

        private void DrawFeatures(MapArgs e, List<int> indices)
        {
            var g = e.Device ?? Graphics.FromImage(BackBuffer);
            var origTransform = g.Transform;

            var minX = e.MinX;
            var maxY = e.MaxY;
            var dx = e.Dx;
            var dy = e.Dy;

            var states = DrawnStates;
            foreach (var category in Symbology.Categories)
            {
                var normalSymbol = GetSymbolizerBitmap(category.Symbolizer, e);
                var selectedSymbol = GetSymbolizerBitmap(category.SelectionSymbolizer, e);
                if (normalSymbol == null || selectedSymbol == null) continue;

                foreach (var index in indices)
                {
                    var state = states[index];
                    if (!state.Visible) continue;
                    var pc = state.Category as IPointCategory;
                    if (pc == null) continue;
                    if (pc != category) continue;

                    var bmp = state.Selected? selectedSymbol : normalSymbol;
                    var shape = DataSet.GetShape(index, false);
                    foreach (var part in shape.Range.Parts)
                    {
                        foreach (var vertex in part)
                        {
                            var pt = new Point
                            {
                                X = Convert.ToInt32((vertex.X - minX)*dx),
                                Y = Convert.ToInt32((maxY - vertex.Y)*dy)
                            };

                            var shift = origTransform.Clone();
                            shift.Translate(pt.X, pt.Y);
                            g.Transform = shift;

                            g.DrawImageUnscaled(bmp, -bmp.Width/2, -bmp.Height/2);
                        }
                    }
                }
            }


            if (e.Device == null) g.Dispose();
            else g.Transform = origTransform;
        }

        private static Bitmap GetSymbolizerBitmap(IPointSymbolizer symbolizer, IProj e)
        {
            if (symbolizer == null) return null;
            double scaleSize = 1;
            if (symbolizer.ScaleMode == ScaleMode.Geographic)
            {
                scaleSize = e.ImageRectangle.Width / e.GeographicExtents.Width;
            }
            var size = symbolizer.GetSize();
            if (size.Width * scaleSize < 1 || size.Height * scaleSize < 1) return null;

            var bitmap = new Bitmap((int)(size.Width * scaleSize) + 1, (int)(size.Height * scaleSize) + 1);
            var bg = Graphics.FromImage(bitmap);
            bg.SmoothingMode = symbolizer.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
            var trans = bg.Transform;

            // keenedge:
            // added ' * scaleSize ' to fix a problme when ploted using ScaleMode=Geographic.   however, it still
            // appeared to be shifted up and left by 1 pixel so I also added the one pixel shift to the NW.
            trans.Translate(((float)(size.Width * scaleSize) / 2 - 1), (float)(size.Height * scaleSize) / 2 - 1);
            bg.Transform = trans;
            symbolizer.Draw(bg, 1);

            return bitmap;
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

        /// <summary>
        /// Attempts to create a new GeoPointLayer using the specified file.  If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="fileName">A string fileName to create a point layer for.</param>
        /// <param name="progressHandler">Any valid implementation of IProgressHandler for receiving progress messages</param>
        /// <returns>A GeoPointLayer created from the specified fileName.</returns>
        public static new MapPointLayer OpenFile(string fileName, IProgressHandler progressHandler)
        {
            var fl = LayerManager.DefaultLayerManager.OpenLayer(fileName, progressHandler);
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
            var fl = LayerManager.DefaultLayerManager.OpenVectorLayer(fileName);
            return fl as MapPointLayer;
        }

        #region ISupportChunksDrawing implementation

        int ISupportChunksDrawing.ChunkSize { get { return ChunkSize; }}
        void ISupportChunksDrawing.OnBufferChanged(List<Rectangle> clipRectangles) { OnBufferChanged(clipRectangles); }

        #endregion
    }
}