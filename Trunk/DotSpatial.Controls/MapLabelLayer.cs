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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 10:20:46 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Kyle Ellison 01/07/2010 Changed Draw*Feature from private to public to expose label functionality
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// GeoLabelLayer
    /// </summary>
    public class MapLabelLayer : LabelLayer, IMapLabelLayer
    {
        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Private Variables

        /// <summary>
        /// The existing labels, accessed for all map label layers, not just this instance
        /// </summary>
        internal static List<RectangleF> ExistingLabels = new List<RectangleF>(); // for collision prevention, tracks existing labels.

        private Image _backBuffer; // draw to the back buffer, and swap to the stencil when done.
        private IEnvelope _bufferExtent; // the geographic extent of the current buffer.
        private Rectangle _bufferRectangle;
        private int _chunkSize;
        private bool _isInitialized;
        private Image _stencil; // draw features to the stencil
        private static readonly Caches _caches = new Caches();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoLabelLayer
        /// </summary>
        public MapLabelLayer()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new label layer based on the specified featureset
        /// </summary>
        /// <param name="inFeatureSet"></param>
        public MapLabelLayer(IFeatureSet inFeatureSet)
            : base(inFeatureSet)
        {
            Configure();
        }

        /// <summary>
        /// Creates a new label layer based on the specified feature layer
        /// </summary>
        /// <param name="inFeatureLayer">The feature layer to build layers from</param>
        public MapLabelLayer(IFeatureLayer inFeatureLayer)
            : base(inFeatureLayer)
        {
            Configure();
        }

        private void Configure()
        {
            _chunkSize = 10000;
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
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
            if (FeatureSet == null) return;
            var sw = new Stopwatch();
            sw.Start();

            if (FeatureSet.IndexMode)
            {
                // First determine the number of features we are talking about based on region.
                List<int> drawIndices = new List<int>();
                foreach (Extent region in regions)
                {
                    if (region != null)
                    {
                        // We need to consider labels that go off the screen.  figure a region
                        // that is larger.
                        Extent sur = region.Copy();
                        sur.ExpandBy(region.Width, region.Height);
                        // Use union to prevent duplicates.  No sense in drawing more than we have to.
                        drawIndices = drawIndices.Union(FeatureSet.SelectIndices(sur)).ToList();
                    }
                }
                List<Rectangle> clips = args.ProjToPixel(regions);
                DrawFeatures(args, drawIndices, clips, true);
            }
            else
            {
                // First determine the number of features we are talking about based on region.
                List<IFeature> drawList = new List<IFeature>();
                foreach (Extent region in regions)
                {
                    if (region != null)
                    {
                        // We need to consider labels that go off the screen.  figure a region
                        // that is larger.
                        Extent r = region.Copy();
                        r.ExpandBy(region.Width, region.Height);
                        // Use union to prevent duplicates.  No sense in drawing more than we have to.
                        drawList = drawList.Union(FeatureSet.Select(r)).ToList();
                    }
                }
                List<Rectangle> clipRects = args.ProjToPixel(regions);
                DrawFeatures(args, drawList, clipRects, true);
            }

            sw.Stop();
            Debug.WriteLine("MapLabelLayer {0} DrawRegions: {1} ms", FeatureSet.Name, sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing.  Specifying transparent
        /// will replace content with transparent pixels.</param>
        public void Clear(List<Rectangle> rectangles, Color color)
        {
            if (_backBuffer == null) return;
            Graphics g = Graphics.FromImage(_backBuffer);
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
        /// If useChunks is true, then this method
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="features">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
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
                    //  FinishDrawing();
                    OnBufferChanged(clipRectangles);
                    Application.DoEvents();
                    //this.StartDrawing();
                }
            }
        }

        /// <summary>
        /// If useChunks is true, then this method
        /// </summary>
        /// <param name="args">The GeoArgs that control how these features should be drawn.</param>
        /// <param name="features">The features that should be drawn.</param>
        /// <param name="clipRectangles">If an entire chunk is drawn and an update is specified, this clarifies the changed rectangles.</param>
        /// <param name="useChunks">Boolean, if true, this will refresh the buffer in chunks.</param>
        public virtual void DrawFeatures(MapArgs args, List<int> features, List<Rectangle> clipRectangles, bool useChunks)
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
                    //  FinishDrawing();
                    OnBufferChanged(clipRectangles);
                    Application.DoEvents();
                    //this.StartDrawing();
                }
            }
        }

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, IEnumerable<int> features)
        {
            // Check that exists at least one category with Expression
            if (Symbology.Categories.All(_ => string.IsNullOrEmpty(_.Expression))) return;

            Graphics g = e.Device ?? Graphics.FromImage(_backBuffer);

            // Only draw features that are currently visible.

            if (FastDrawnStates == null)
            {
                CreateIndexedLabels();
            }
            FastLabelDrawnState[] drawStates = FastDrawnStates;
            if (drawStates == null) return;
            //Sets the graphics objects smoothing modes
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Action<int, IFeature> drawFeature;
            switch (FeatureSet.FeatureType)
            {
                case FeatureType.Polygon:
                    drawFeature = (fid, feature) => DrawPolygonFeature(e, g, feature, drawStates[fid].Category, drawStates[fid].Selected, ExistingLabels);
                    break;
                case FeatureType.Line:
                    drawFeature = (fid, feature) => DrawLineFeature(e, g, feature, drawStates[fid].Category, drawStates[fid].Selected,ExistingLabels);
                    break;
                case FeatureType.Point:
                    drawFeature = (fid, feature) => DrawPointFeature(e, g, feature, drawStates[fid].Category, drawStates[fid].Selected, ExistingLabels);
                    break;
                default:
                   return; // Can't draw something else
            }

            foreach (var category in Symbology.Categories)
            {
                var catFeatures = new List<int>();
                foreach (int fid in features)
                {
                    if (drawStates[fid] == null || drawStates[fid].Category == null) continue;
                    if (drawStates[fid].Category == category)
                    {
                        catFeatures.Add(fid);
                    }
                }
                // Now that we are restricted to a certain category, we can look at
                // priority
                if (category.Symbolizer.PriorityField != "FID")
                {
                    Feature.ComparisonField = category.Symbolizer.PriorityField;
                    catFeatures.Sort();
                    // When preventing collisions, we want to do high priority first.
                    // otherwise, do high priority last.
                    if (category.Symbolizer.PreventCollisions)
                    {
                        if (!category.Symbolizer.PrioritizeLowValues)
                        {
                            catFeatures.Reverse();
                        }
                    }
                    else
                    {
                        if (category.Symbolizer.PrioritizeLowValues)
                        {
                            catFeatures.Reverse();
                        }
                    }
                }

                foreach (var fid in catFeatures)
                {
                    if (!FeatureLayer.DrawnStates[fid].Visible) continue;
                    var feature = FeatureSet.GetFeature(fid);
                    drawFeature(fid, feature);
                }
            }

            if (e.Device == null) g.Dispose();
        }

        // This draws the individual line features
        private void DrawFeatures(MapArgs e, IEnumerable<IFeature> features)
        {
            // Check that exists at least one category with Expression
            if (Symbology.Categories.All(_ => string.IsNullOrEmpty(_.Expression))) return;

            Graphics g = e.Device ?? Graphics.FromImage(_backBuffer);

            // Only draw features that are currently visible.
            if (DrawnStates == null || !DrawnStates.ContainsKey(features.First()))
            {
                CreateLabels();
            }
            Dictionary<IFeature, LabelDrawState> drawStates = DrawnStates;
            if (drawStates == null) return;
            //Sets the graphics objects smoothing modes
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Action<IFeature> drawFeature;
            switch (features.First().FeatureType)
            {
                case FeatureType.Polygon:
                    drawFeature = (f) => DrawPolygonFeature(e, g, f, drawStates[f].Category, drawStates[f].Selected, ExistingLabels);
                    break;
                case FeatureType.Line:
                    drawFeature = (f) => DrawLineFeature(e, g, f, drawStates[f].Category, drawStates[f].Selected, ExistingLabels);
                    break;
                case FeatureType.Point:
                    drawFeature = (f) => DrawPointFeature(e, g, f, drawStates[f].Category, drawStates[f].Selected, ExistingLabels);
                    break;
                default:
                    return; // Can't draw something else
            }


            foreach (ILabelCategory category in Symbology.Categories)
            {
                var cat = category; // prevent access to unmodified closure problems
                List<IFeature> catFeatures = new List<IFeature>();
                foreach (IFeature f in features)
                {
                    if (drawStates.ContainsKey(f))
                    {
                        if (drawStates[f].Category == cat)
                        {
                            catFeatures.Add(f);
                        }
                    }
                }
                // Now that we are restricted to a certain category, we can look at
                // priority
                if (category.Symbolizer.PriorityField != "FID")
                {
                    Feature.ComparisonField = cat.Symbolizer.PriorityField;
                    catFeatures.Sort();
                    // When preventing collisions, we want to do high priority first.
                    // otherwise, do high priority last.
                    if (cat.Symbolizer.PreventCollisions)
                    {
                        if (!cat.Symbolizer.PrioritizeLowValues)
                        {
                            catFeatures.Reverse();
                        }
                    }
                    else
                    {
                        if (cat.Symbolizer.PrioritizeLowValues)
                        {
                            catFeatures.Reverse();
                        }
                    }
                }
                for (int i = 0; i < catFeatures.Count; i++)
                {
                    if (!FeatureLayer.DrawnStates[i].Visible) continue;
                    drawFeature(catFeatures[i]);
                }
            }

            if (e.Device == null) g.Dispose();
        }

        private static bool Collides(RectangleF rectangle, IEnumerable<RectangleF> drawnRectangles)
        {
            return drawnRectangles.Any(rectangle.IntersectsWith);
        }

        /// <summary>
        /// Draws a label on a polygon with various different methods
        /// </summary>
        public static void DrawPolygonFeature(MapArgs e, Graphics g, IFeature f, ILabelCategory category, bool selected, List<RectangleF> existingLabels)
        {
            ILabelSymbolizer symb = category.Symbolizer;
            if (selected) symb = category.SelectionSymbolizer;

            //Gets the features text and calculate the label size
            string txt = GetLabelText(f, category);
            if (txt == null) return;
            Func<SizeF> labelSize = () => g.MeasureString(txt, _caches.GetFont(symb));

            if (f.NumGeometries == 1)
            {
                RectangleF labelBounds = PlacePolygonLabel(f.BasicGeometry, e, labelSize, symb);
                CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
            }
            else
            {
                if (symb.PartsLabelingMethod == PartLabelingMethod.LabelAllParts)
                {
                    for (int n = 0; n < f.NumGeometries; n++)
                    {
                        RectangleF labelBounds = PlacePolygonLabel(f.GetBasicGeometryN(n), e, labelSize, symb);
                        CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
                    }
                }
                else
                {
                    double largestArea = 0;
                    IPolygon largest = null;
                    for (int n = 0; n < f.NumGeometries; n++)
                    {
                        IPolygon pg = Geometry.FromBasicGeometry(f.GetBasicGeometryN(n)) as IPolygon;
                        if (pg == null) continue;
                        double tempArea = pg.Area;
                        if (largestArea < tempArea)
                        {
                            largestArea = tempArea;
                            largest = pg;
                        }
                    }
                    RectangleF labelBounds = PlacePolygonLabel(largest, e, labelSize, symb);
                    CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
                }
            }
        }

        private static void CollisionDraw(string txt, Graphics g, ILabelSymbolizer symb, IFeature f, MapArgs e, RectangleF labelBounds, List<RectangleF> existingLabels)
        {
            if (labelBounds.IsEmpty || !e.ImageRectangle.IntersectsWith(labelBounds)) return;
            if (symb.PreventCollisions)
            {
                if (!Collides(labelBounds, existingLabels))
                {
                    DrawLabel(g, txt, labelBounds, symb, f);
                    existingLabels.Add(labelBounds);
                }
            }
            else
            {
                DrawLabel(g, txt, labelBounds, symb, f);
            }
        }

        private static RectangleF PlacePolygonLabel(IBasicGeometry geom, MapArgs e, Func<SizeF> labelSize, ILabelSymbolizer symb)
        {
            IPolygon pg = Geometry.FromBasicGeometry(geom) as IPolygon;
            if (pg == null) return RectangleF.Empty;
            Coordinate c;
            switch (symb.LabelPlacementMethod)
            {
                case LabelPlacementMethod.Centroid:
                    c = pg.Centroid.Coordinates[0];
                    break;
                case LabelPlacementMethod.InteriorPoint:
                    c = pg.InteriorPoint.Coordinate;
                    break;
                default:
                    c = geom.Envelope.Center();
                    break;
            }
            if (e.GeographicExtents.Intersects(c) == false) return RectangleF.Empty;
            var lz = labelSize();
            PointF adjustment = Position(symb, lz);
            float x = Convert.ToSingle((c.X - e.MinX) * e.Dx) + adjustment.X;
            float y = Convert.ToSingle((e.MaxY - c.Y) * e.Dy) + adjustment.Y;
            return new RectangleF(x, y, lz.Width, lz.Height);
        }

        /// <summary>
        /// Draws a label on a point with various different methods
        /// </summary>
        /// <param name="e"></param>
        /// <param name="g"></param>
        /// <param name="f"></param>
        /// <param name="category"></param>
        /// <param name="selected"></param>
        /// <param name="existingLabels"></param>
        public static void DrawPointFeature(MapArgs e, Graphics g, IFeature f, ILabelCategory category, bool selected, List<RectangleF> existingLabels)
        {
            ILabelSymbolizer symb = category.Symbolizer;
            if (selected) symb = category.SelectionSymbolizer;

            //Gets the features text and calculate the label size
            string txt = GetLabelText(f, category);
            if (txt == null) return;

            Func<SizeF> labelSize = () => g.MeasureString(txt, _caches.GetFont(symb));

            //Depending on the labeling strategy we do diff things
            if (symb.PartsLabelingMethod == PartLabelingMethod.LabelAllParts)
            {
                for (int n = 0; n < f.NumGeometries; n++)
                {
                    RectangleF labelBounds = PlacePointLabel(f, e, labelSize, symb);
                    CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
                }
            }
            else
            {
                RectangleF labelBounds = PlacePointLabel(f, e, labelSize, symb);
                CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
            }
        }

        private static RectangleF PlacePointLabel(IBasicGeometry f, MapArgs e, Func<SizeF> labelSize, ILabelSymbolizer symb)
        {
            Coordinate c = f.GetBasicGeometryN(1).Coordinates[0];
            if (e.GeographicExtents.Intersects(c) == false) return RectangleF.Empty;
            var lz = labelSize();
            PointF adjustment = Position(symb, lz);
            float x = Convert.ToSingle((c.X - e.MinX) * e.Dx) + adjustment.X;
            float y = Convert.ToSingle((e.MaxY - c.Y) * e.Dy) + adjustment.Y;
            return new RectangleF(x, y, lz.Width, lz.Height);
        }

        /// <summary>
        /// Draws a label on a line with various different methods
        /// </summary>
        public static void DrawLineFeature(MapArgs e, Graphics g, IFeature f, ILabelCategory category, bool selected, List<RectangleF> existingLabels)
        {
            if (f == null) return;
            ILabelSymbolizer symb = category.Symbolizer;
            if (selected) symb = category.SelectionSymbolizer;

            //Gets the features text and calculate the label size
            string txt = GetLabelText(f, category);
            if (txt == null) return;
            Func<SizeF> labelSize = () => g.MeasureString(txt, _caches.GetFont(symb));

            if (f.NumGeometries == 1)
            {
                RectangleF labelBounds = PlaceLineLabel(f.BasicGeometry, labelSize, e, symb);
                CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
            }
            else
            {
                //Depending on the labeling strategy we do diff things
                if (symb.PartsLabelingMethod == PartLabelingMethod.LabelAllParts)
                {
                    for (int n = 0; n < f.NumGeometries; n++)
                    {
                        RectangleF labelBounds = PlaceLineLabel(f.GetBasicGeometryN(n), labelSize, e, symb);
                        CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
                    }
                }
                else
                {
                    double longestLine = 0;
                    int longestIndex = 0;
                    for (int n = 0; n < f.NumGeometries; n++)
                    {
                        ILineString ls = f.GetBasicGeometryN(n) as ILineString;
                        double tempLength = 0;
                        if (ls != null) tempLength = ls.Length;
                        if (longestLine < tempLength)
                        {
                            longestLine = tempLength;
                            longestIndex = n;
                        }
                    }
                    RectangleF labelBounds = PlaceLineLabel(f.GetBasicGeometryN(longestIndex), labelSize, e, symb);
                    CollisionDraw(txt, g, symb, f, e, labelBounds, existingLabels);
                }
            }
        }

        private static RectangleF PlaceLineLabel(IBasicGeometry lineString, Func<SizeF> labelSize, MapArgs e, ILabelSymbolizer symb)
        {
            ILineString ls = Geometry.FromBasicGeometry(lineString) as ILineString;
            if (ls == null) return RectangleF.Empty;
            Coordinate c;
            if (symb.LabelPlacementMethod == LabelPlacementMethod.Centroid)
                c = ls.Centroid.Coordinate;
            else if (symb.LabelPlacementMethod == LabelPlacementMethod.InteriorPoint)
                c = ls.InteriorPoint.Coordinate;
            else
                c = ls.Envelope.Center();

            var lz = labelSize();
            PointF adjustment = Position(symb, lz);
            float x = Convert.ToSingle((c.X - e.MinX) * e.Dx) + adjustment.X;
            float y = Convert.ToSingle((e.MaxY - c.Y) * e.Dy) + adjustment.Y;
            return new RectangleF(x, y, lz.Width, lz.Height);
        }

        private static PointF Position(ILabelSymbolizer symb, SizeF size)
        {
            ContentAlignment orientation = symb.Orientation;
            float x = symb.OffsetX;
            float y = -symb.OffsetY;
            switch (orientation)
            {
                case ContentAlignment.TopLeft:
                    return new PointF(-size.Width + x, -size.Height + y);
                case ContentAlignment.TopCenter:
                    return new PointF(-size.Width / 2 + x, -size.Height + y);
                case ContentAlignment.TopRight:
                    return new PointF(0 + x, -size.Height + y);
                case ContentAlignment.MiddleLeft:
                    return new PointF(-size.Width + x, -size.Height / 2 + y);
                case ContentAlignment.MiddleCenter:
                    return new PointF(-size.Width / 2 + x, -size.Height / 2 + y);
                case ContentAlignment.MiddleRight:
                    return new PointF(0 + x, -size.Height / 2 + y);
                case ContentAlignment.BottomLeft:
                    return new PointF(-size.Width + x, 0 + y);
                case ContentAlignment.BottomCenter:
                    return new PointF(-size.Width / 2 + x, 0 + y);
                case ContentAlignment.BottomRight:
                    return new PointF(0 + x, 0 + y);
            }
            return new PointF(0, 0);
        }

        /// <summary>
        /// Draws labels in a specified rectangle
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="labelText">The label text to draw</param>
        /// <param name="labelBounds">The rectangle of the label</param>
        /// <param name="symb">the Label Symbolizer to use when drawing the label</param>
        /// <param name="feature">Feature to draw</param>
        private static void DrawLabel(Graphics g, string labelText, RectangleF labelBounds, ILabelSymbolizer symb, IFeature feature)
        {
            //Sets up the brushes and such for the labeling
            Font textFont = _caches.GetFont(symb);
            var format = new StringFormat { Alignment = symb.Alignment, };

            //Text graphics path
            var gp = new GraphicsPath();
            gp.AddString(labelText, textFont.FontFamily, (int)textFont.Style, textFont.SizeInPoints * 96F / 72F, labelBounds, format);

            // Rotate text
            var angleToRotate = GetAngleToRotate(symb, feature);
            RotateAt(g, labelBounds.X, labelBounds.Y, angleToRotate);

            // Draws the text outline
            if (symb.BackColorEnabled && symb.BackColor != Color.Transparent)
            {
                var backBrush = _caches.GetSolidBrush(symb.BackColor);
                if (symb.FontColor == Color.Transparent)
                {
                    using (var backgroundGP = new GraphicsPath())
                    {
                        backgroundGP.AddRectangle(labelBounds);
                        backgroundGP.FillMode = FillMode.Alternate;
                        backgroundGP.AddPath(gp, true);
                        g.FillPath(backBrush, backgroundGP);
                    }
                }
                else
                {
                    g.FillRectangle(backBrush, labelBounds);
                }
            }

            // Draws the border if its enabled
            if (symb.BorderVisible && symb.BorderColor != Color.Transparent)
            {
                var borderPen = _caches.GetPen(symb.BorderColor);
                g.DrawRectangle(borderPen, labelBounds.X, labelBounds.Y, labelBounds.Width, labelBounds.Height);
            }

            // Draws the drop shadow
            if (symb.DropShadowEnabled && symb.DropShadowColor != Color.Transparent)
            {
                var shadowBrush = _caches.GetSolidBrush(symb.DropShadowColor);
                var gpTrans = new Matrix();
                gpTrans.Translate(symb.DropShadowPixelOffset.X, symb.DropShadowPixelOffset.Y);
                gp.Transform(gpTrans);
                g.FillPath(shadowBrush, gp);
                gpTrans = new Matrix();
                gpTrans.Translate(-symb.DropShadowPixelOffset.X, -symb.DropShadowPixelOffset.Y);
                gp.Transform(gpTrans);
                gpTrans.Dispose();
            }

            // Draws the text halo
            if (symb.HaloEnabled && symb.HaloColor != Color.Transparent)
            {
                using(var haloPen = new Pen(symb.HaloColor) {Width = 2, Alignment = PenAlignment.Outset})
                    g.DrawPath(haloPen, gp);
            }

            // Draws the text if its not transparent
            if (symb.FontColor != Color.Transparent)
            {
                var foreBrush = _caches.GetSolidBrush(symb.FontColor);
                g.FillPath(foreBrush, gp);
            }
            gp.Dispose();
        }

        private static float GetAngleToRotate(ILabelSymbolizer symb, IFeature feature)
        {
            if (symb.UseAngle)
            {
                try
                {
                    return Convert.ToSingle(symb.Angle);
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            if (symb.UseLabelAngleField)
            {
                var angleField = symb.LabelAngleField;
                if (String.IsNullOrEmpty(angleField))
                    return 0;

                try
                {
                    return Convert.ToSingle(feature.DataRow[angleField]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            return 0;
        }

        private static void RotateAt(Graphics gr, float cx, float cy, float angle)
        {
            gr.ResetTransform();
            gr.TranslateTransform(-cx, -cy, MatrixOrder.Append);
            gr.RotateTransform(angle, MatrixOrder.Append);
            gr.TranslateTransform(cx, cy, MatrixOrder.Append);
        }

        private static string GetLabelText(IFeature feature, ILabelCategory category)
        {
            string result = category.Expression;
            if (!string.IsNullOrEmpty(result) && feature != null)
            {
                foreach (DataColumn dc in feature.DataRow.Table.Columns)
                {
                    if (String.IsNullOrEmpty(result)) break; // Result already is empty, no need to iterate over columns
                    result = result.Replace("[" + dc.ColumnName + "]", feature.DataRow[dc.ColumnName].ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        public void FinishDrawing()
        {
            OnFinishDrawing();
            if (_stencil != null && _stencil != _backBuffer) _stencil.Dispose();
            _stencil = _backBuffer;
            FeatureLayer.Invalidate();
        }

        ///// <summary>
        ///// This begins the process of drawing features in the given geographic regions
        ///// to the buffer, where the transform is specified by the GeoArgs.  This also
        ///// configures the size of the buffer and the geographic extents based on
        ///// the input args.
        ///// </summary>
        //public virtual void Initialize(MapArgs args, List<IEnvelope> regions)
        //{
        //    BufferEnvelope = args.GeographicExtents.Copy();
        //    BufferRectangle = args.ImageRectangle;
        //    Buffer = new Bitmap(args.ImageRectangle.Width, args.ImageRectangle.Height);
        //    StartDrawing(true); // set-up the back buffer, preserving what we can
        //   // this.Clear(regions); // clear the regions to draw in
        //    DrawRegions(args, regions); // actually do the drawing.
        //    FinishDrawing();
        //    List<Rectangle> clipRects = args.ProjToPixel(regions);
        //    OnBufferChanged(clipRects);
        //}

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
        public Image BackBuffer
        {
            get { return _backBuffer; }
            set { _backBuffer = value; }
        }

        /// <summary>
        /// Gets the current buffer.
        /// </summary>
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Buffer
        {
            get { return _stencil; }
            set { _stencil = value; }
        }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnvelope BufferEnvelope
        {
            get { return _bufferExtent; }
            set { _bufferExtent = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle
        {
            get { return _bufferRectangle; }
            set { _bufferRectangle = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of labels that will be rendered before
        /// refreshing the screen.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ChunkSize
        {
            get { return _chunkSize; }
            set { _chunkSize = value; }
        }

        /// <summary>
        /// Gets or sets the MapFeatureLayer that this label layer is attached to.
        /// </summary>
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IMapFeatureLayer FeatureLayer
        {
            get { return base.FeatureLayer as IMapFeatureLayer; }
            set { base.FeatureLayer = value; }
        }

        /// <summary>
        /// Gets or sets whether or not this layer has been initialized.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        #endregion

        /// <summary>
        /// Fires the OnBufferChanged event
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            var h = BufferChanged;
            if (h != null)
            {
                h(this, new ClipArgs(clipRectangles));
            }
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

        private class Caches
        {
            private readonly Dictionary<string, Font> _symbFonts = new Dictionary<string, Font>();
            private readonly Dictionary<Color, Brush> _solidBrushes = new Dictionary<Color, Brush>();
            private readonly Dictionary<Color, Pen> _pens = new Dictionary<Color, Pen>();

            public Font GetFont(ILabelSymbolizer symb)
            {
                var fontDesc = string.Format("{0};{1};{2}", symb.FontFamily, symb.FontSize, symb.FontStyle);
                return _symbFonts.GetOrAdd(fontDesc, _ => symb.GetFont());
            }

            public Brush GetSolidBrush(Color color)
            {
                return _solidBrushes.GetOrAdd(color, _ => new SolidBrush(color));
            }

            public Pen GetPen(Color color)
            {
                return _pens.GetOrAdd(color, _ => new Pen(color));
            }
        }
    }

    internal static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                value = valueFactory(key);
                dic.Add(key, value);
            }
            return value;
        }
    }
}