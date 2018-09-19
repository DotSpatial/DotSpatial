// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.NTSExtension;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// MoveVertexFunction works only with the actively selected layer in the legend.
    /// MoveVertex requires clicking on a shape in order to first select the shape to work with.
    /// Moving the mouse should highlight potential shapes for editing when not in edit mode.
    /// Clicking on the shape establishes "edit mode" for that shape.
    /// It should display all the vertices of the selected polygon in blue.
    /// The mouse down on a vertex starts dragging.
    /// but previous polygon location should be ok as well.
    /// A right click during drag should cancel the movement if dragging.
    /// A further right click will de-select the shape to edit.
    /// </summary>
    public class MoveVertexFunction : SnappableMapFunction
    {
        #region Fields

        private IFeatureCategory _activeCategory;
        private IFeature _activeFeature; // not yet selected
        private Coordinate _activeVertex;
        private Coordinate _closedCircleCoord;
        private Coordinate _dragCoord;
        private Coordinate _dragCoord_Old;
        private bool _dragging;
        private IFeatureSet _featureSet;
        private Rectangle _imageRect;
        private IFeatureLayer _layer;
        private System.Drawing.Point _mousePosition;
        private Coordinate _nextPoint;
        private IFeatureCategory _oldCategory;
        private Coordinate _previousPoint;
        private IFeatureCategory _selectedCategory;
        private IFeature _selectedFeature;
        private ContextMenu _insertContext;
        private ContextMenu _deleteContext;
        private MenuItem _insertVertex;
        private MenuItem _deleteVertex;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveVertexFunction"/> class where the Map will be defined.
        /// </summary>
        /// <param name="map">The map control that implements the IMap interface.</param>
        public MoveVertexFunction(IMap map)
            : base(map)
        {
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Is raised after a vertex was moved.
        /// </summary>
        public event EventHandler<VertexMovedEventArgs> VertextMoved;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        public IFeatureLayer Layer
        {
            get
            {
                return _layer;
            }

            set
            {
                _layer = value;
                _featureSet = _layer.DataSet;
                InitializeSnapLayers();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deselects the selected feature and removes the highlight from any highlighted feature.
        /// </summary>
        public void ClearSelection()
        {
            DeselectFeature();
            RemoveHighlightFromFeature();
            _oldCategory = null;
        }

        /// <summary>
        /// Delete the snapped vertex.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">An empty EventArgs class.</param>
        public void Vertex_Delete(object sender, EventArgs e)
        {
            if (SnappingType != "v") return;
            if (_selectedFeature == null) return;
            if (SnappedFeature != _selectedFeature) return;
            Feature selFeature = _selectedFeature as Feature;

            // Coordinate[] coords = _selectedFeature.Geometry.Coordinates;
            IList<Coordinate> coords = _selectedFeature.Geometry.Coordinates;
            List<Coordinate> newCoords = coords.CloneList();
            int numCoords = coords.Count;
            if (SnappedCoordIndex >= numCoords) return;

            switch (_selectedFeature.FeatureType)
            {
                case FeatureType.Line:
                    if (numCoords < 3) return;
                    newCoords.RemoveAt(SnappedCoordIndex);
                    LineString newLine = new LineString(newCoords.ToArray());
                    _selectedFeature.Geometry = newLine;

                    break;

                case FeatureType.Polygon:

                    if (numCoords < 4) return;

                    if (SnappedCoordIndex == 0) newCoords[numCoords - 1] = newCoords[1];
                    if (SnappedCoordIndex == numCoords) newCoords[0] = newCoords[numCoords - 2];
                    newCoords.RemoveAt(SnappedCoordIndex);

                    LinearRing newLinearRing= new LinearRing(newCoords.ToArray());

                    _selectedFeature.Geometry = new Polygon(newLinearRing);

                    break;
            }

            _featureSet.InitializeVertices();
            _selectedFeature.UpdateEnvelope();
            Map.Refresh();
        }

        /// <summary>
        /// Insert a vertex at the snapped coordinate.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">An empty EventArgs class.</param>
        public void Vertex_Insert(object sender, EventArgs e)
        {
            if (SnappingType != "e") return;
            if (_selectedFeature == null) return;
            if (SnappedFeature != _selectedFeature) return;
            Feature selFeature = _selectedFeature as Feature;

            // Coordinate[] coords = _selectedFeature.Geometry.Coordinates;
            IList<Coordinate> coords = _selectedFeature.Geometry.Coordinates;
            List<Coordinate> newCoords = coords.CloneList();
            int numCoords = coords.Count;
            if (SnappedCoordIndex <= 0 || SnappedCoordIndex >= numCoords) return;

            newCoords.Insert(SnappedCoordIndex, SnappedCoordKeeped);
            switch (_selectedFeature.FeatureType)
            {
                case FeatureType.Line:

                    LineString newLine = new LineString(newCoords.ToArray());
                    _selectedFeature.Geometry = newLine;

                    break;

                case FeatureType.Polygon:

                    LinearRing newLinearRing = new LinearRing(newCoords.ToArray());
                    _selectedFeature.Geometry = new Polygon(newLinearRing);

                    break;
            }

            _featureSet.InitializeVertices();
            _selectedFeature.UpdateEnvelope();
            Map.Refresh();
        }

        /// <summary>
        /// This should be called if for some reason the layer gets un-selected or whatever so that the selection
        /// should clear.
        /// </summary>
        public void DeselectFeature()
        {
            if (_selectedFeature != null)
            {
                _layer.SetCategory(_selectedFeature, _oldCategory);
            }

            _selectedFeature = null;
            Map.MapFrame.Initialize();
            Map.Invalidate();
        }

        /// <summary>
        /// Removes the highlighting from the actively highlighted feature.
        /// </summary>
        public void RemoveHighlightFromFeature()
        {
            if (_activeFeature != null)
            {
                _layer.SetCategory(_activeFeature, _oldCategory);
            }

            _activeFeature = null;
        }

        /// <inheritdoc />
        protected override void OnDeactivate()
        {
            ClearSelection();
            base.OnDeactivate();
        }

        /// <inheritdoc />
        protected override void OnDraw(MapDrawArgs e)
        {
            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            if (_selectedFeature != null)
            {
                foreach (Coordinate c in _selectedFeature.Geometry.Coordinates)
                {
                    System.Drawing.Point pt = e.GeoGraphics.ProjToPixel(c);
                    if (e.GeoGraphics.ImageRectangle.Contains(pt))
                    {
                        e.Graphics.FillRectangle(Brushes.Blue, pt.X - 2, pt.Y - 2, 4, 4);
                    }

                    if (mouseRect.Contains(pt))
                    {
                        e.Graphics.FillRectangle(Brushes.Red, mouseRect);
                    }
                }
            }

            if (_dragging)
            {
                if (_featureSet.FeatureType == FeatureType.Point || _featureSet.FeatureType == FeatureType.MultiPoint)
                {
                    Rectangle r = new Rectangle(_mousePosition.X - (_imageRect.Width / 2), _mousePosition.Y - (_imageRect.Height / 2), _imageRect.Width, _imageRect.Height);
                    _selectedCategory.Symbolizer.Draw(e.Graphics, r);
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.Red, _mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
                    System.Drawing.Point b = _mousePosition;
                    Pen p = new Pen(Color.Blue)
                    {
                        DashStyle = DashStyle.Dash
                    };
                    if (_previousPoint != null)
                    {
                        System.Drawing.Point a = e.GeoGraphics.ProjToPixel(_previousPoint);
                        e.Graphics.DrawLine(p, a, b);
                    }

                    if (_nextPoint != null)
                    {
                        System.Drawing.Point c = e.GeoGraphics.ProjToPixel(_nextPoint);
                        e.Graphics.DrawLine(p, b, c);
                    }

                    p.Dispose();
                }
            }
        }

        /// <inheritdoc />
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                _mousePosition = e.Location;
                if (_dragging)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        _dragging = false;
                        _dragCoord.X = _dragCoord_Old.X;
                        _dragCoord.Y = _dragCoord_Old.Y;

                        Map.Invalidate();
                        Map.IsBusy = false;
                    }
                }
                else
                {
                    if (_selectedFeature != null)
                    {
                        Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);

                        Envelope env = Map.PixelToProj(mouseRect).ToEnvelope();

                        if (CheckForVertexDrag(e))
                        {
                            return;
                        }

                        // No vertex selection has occured.
                        if (!_selectedFeature.Geometry.Intersects(env.ToPolygon()))
                        {
                            // We are clicking down outside of the given polygon, so clear our selected feature
                            DeselectFeature();
                            return;
                        }
                    }

                    if (_activeFeature != null)
                    {
                        // Don't start dragging a vertices right away for polygons and lines.
                        // First you select the polygon, which displays the vertices, then they can be moved.
                        if (_featureSet.FeatureType == FeatureType.Polygon)
                        {
                            _selectedFeature = _activeFeature;
                            _activeFeature = null;
                            IPolygonCategory sc = _selectedCategory as IPolygonCategory;
                            if (sc == null)
                            {
                                _selectedCategory = new PolygonCategory(Color.FromArgb(55, 0, 255, 255), Color.Blue, 1)
                                {
                                    LegendItemVisible = false
                                };
                            }

                            _layer.SetCategory(_selectedFeature, _selectedCategory);
                        }
                        else if (_featureSet.FeatureType == FeatureType.Line)
                        {
                            _selectedFeature = _activeFeature;
                            _activeFeature = null;
                            ILineCategory sc = _selectedCategory as ILineCategory;
                            if (sc == null)
                            {
                                _selectedCategory = new LineCategory(Color.Cyan, 1)
                                {
                                    LegendItemVisible = false
                                };
                            }

                            _layer.SetCategory(_selectedFeature, _selectedCategory);
                        }
                        else
                        {
                            _dragging = true;
                            Map.IsBusy = true;
                            _dragCoord = _activeFeature.Geometry.Coordinates[0];
                            _dragCoord_Old = _dragCoord.Copy();
                            MapPointLayer mpl = _layer as MapPointLayer;
                            mpl?.SetVisible(_activeFeature, false);

                            IPointCategory sc = _selectedCategory as IPointCategory;
                            if (sc == null)
                            {
                                IPointSymbolizer ps = _layer.GetCategory(_activeFeature).Symbolizer.Copy() as IPointSymbolizer;
                                if (ps != null)
                                {
                                    ps.SetFillColor(Color.Cyan);
                                    _selectedCategory = new PointCategory(ps);
                                }
                            }
                        }
                    }

                    Map.MapFrame.Initialize();
                    Map.Invalidate();
                }
            }

            base.OnMouseDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            // SnappingType = string.Empty;
            _mousePosition = e.Location;
            if (_dragging)
            {
                // Begin snapping changes
                Coordinate snappedCoord = e.GeographicLocation;
                if (ComputeSnappedLocation(e, ref snappedCoord))
                {
                    _mousePosition = Map.ProjToPixel(snappedCoord);
                }

                // End snapping changes
                UpdateDragCoordinate(snappedCoord); // Snapping changes
            }
            else
            {
                if (_selectedFeature != null)
                {
                    VertexHighlight();
                }
                else
                {
                    // Before a shape is selected it should be possible to highlight shapes to indicate which one
                    // will be selected.
                    bool requiresInvalidate = false;
                    if (_activeFeature != null)
                    {
                        if (ShapeRemoveHighlight(e))
                        {
                            requiresInvalidate = true;
                        }
                    }

                    if (_activeFeature == null)
                    {
                        if (ShapeHighlight(e))
                        {
                            requiresInvalidate = true;
                        }
                    }

                    if (requiresInvalidate)
                    {
                        Map.MapFrame.Initialize();
                        Map.Invalidate();
                    }
                }

                // check to see if the coordinates intersect with a shape in our current featureset.
            }

            base.OnMouseMove(e);
        }

        /// <inheritdoc />
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Coordinate snappedCoord = e.GeographicLocation;
                ComputeSnappedLocation(e, ref snappedCoord);

                if (_featureSet.FeatureType != FeatureType.Point && _featureSet.FeatureType != FeatureType.MultiPoint)
                {
                    if (_selectedFeature != null)
                    {
                        // Console.WriteLine("Mouse Up" + SnappingType);
                        if (SnappingType == "v")
                        {
                            _deleteContext.Show((Control)Map, e.Location);
                        }

                        if (SnappingType == "e" && SnappedCoordIndex > 0)
                        {
                            _insertContext.Show((Control)Map, e.Location);
                        }
                    }
                }
            }

            if (e.Button == MouseButtons.Left && _dragging)
            {
                _dragging = false;
                Map.IsBusy = false;
                _featureSet.InvalidateVertices();

                if (_featureSet.FeatureType == FeatureType.Point || _featureSet.FeatureType == FeatureType.MultiPoint)
                {
                    if (_activeFeature == null)
                    {
                        return;
                    }

                    OnVertexMoved(new VertexMovedEventArgs(_activeFeature));
                    if (_layer.GetCategory(_activeFeature) != _selectedCategory)
                    {
                        _layer.SetCategory(_activeFeature, _selectedCategory);
                        _layer.SetVisible(_activeFeature, true);
                    }
                }
                else
                {
                    if (_selectedFeature == null)
                    {
                        return;
                    }

                    OnVertexMoved(new VertexMovedEventArgs(_selectedFeature));
                    if (_layer.GetCategory(_selectedFeature) != _selectedCategory)
                    {
                        _layer.SetCategory(_selectedFeature, _selectedCategory);
                    }
                }
            }

            Map.MapFrame.Initialize();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// This function checks to see if the current mouse location is over a vertex.
        /// </summary>
        /// <param name="e">The GeoMouseArgs parameter contains information about the mouse location and geographic coordinates.</param>
        /// <returns>True, if the current mouse location is over a vertex.</returns>
        private bool CheckForVertexDrag(GeoMouseArgs e)
        {
            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            Envelope env = Map.PixelToProj(mouseRect).ToEnvelope();
            if (e.Button == MouseButtons.Left)
            {
                if (_layer.DataSet.FeatureType == FeatureType.Polygon)
                {
                    for (int prt = 0; prt < _selectedFeature.Geometry.NumGeometries; prt++)
                    {
                        IGeometry g = _selectedFeature.Geometry.GetGeometryN(prt);
                        IList<Coordinate> coords = g.Coordinates;
                        for (int ic = 0; ic < coords.Count; ic++)
                        {
                            Coordinate c = coords[ic];
                            if (env.Contains(c))
                            {
                                _dragging = true;
                                _dragCoord = c;
                                if (ic == 0)
                                {
                                    _closedCircleCoord = coords[coords.Count - 1];
                                    _previousPoint = coords[coords.Count - 2];
                                    _nextPoint = coords[1];
                                }
                                else if (ic == coords.Count - 1)
                                {
                                    _closedCircleCoord = coords[0];
                                    _previousPoint = coords[coords.Count - 2];
                                    _nextPoint = coords[1];
                                }
                                else
                                {
                                    _previousPoint = coords[ic - 1];
                                    _nextPoint = coords[ic + 1];
                                    _closedCircleCoord = null;
                                }

                                _dragCoord_Old = _dragCoord.Copy();

                                Map.Invalidate();
                                return true;
                            }
                        }
                    }
                }
                else if (_layer.DataSet.FeatureType == FeatureType.Line)
                {
                    for (int prt = 0; prt < _selectedFeature.Geometry.NumGeometries; prt++)
                    {
                        IGeometry g = _selectedFeature.Geometry.GetGeometryN(prt);
                        IList<Coordinate> coords = g.Coordinates;
                        for (int ic = 0; ic < coords.Count; ic++)
                        {
                            Coordinate c = coords[ic];
                            if (env.Contains(c))
                            {
                                _dragging = true;
                                _dragCoord = c;

                                if (ic == 0)
                                {
                                    _previousPoint = null;
                                    _nextPoint = coords[1];
                                }
                                else if (ic == coords.Count - 1)
                                {
                                    _previousPoint = coords[coords.Count - 2];
                                    _nextPoint = null;
                                }
                                else
                                {
                                    _previousPoint = coords[ic - 1];
                                    _nextPoint = coords[ic + 1];
                                }

                                _dragCoord_Old = _dragCoord.Copy();

                                Map.Invalidate();
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void Configure()
        {
            YieldStyle = YieldStyles.LeftButton | YieldStyles.RightButton;

            if (_deleteVertex == null)
            {
                _deleteVertex = new MenuItem("Delete vertex", Vertex_Delete);
            }

            if (_deleteContext == null)
            {
                _deleteContext = new ContextMenu();
            }

            if (!_deleteContext.MenuItems.Contains(_deleteVertex))
            {
                _deleteContext.MenuItems.Add(_deleteVertex);
            }

            // _
            if (_insertVertex == null)
            {
                _insertVertex = new MenuItem("Insert vertex", Vertex_Insert);
            }

            if (_insertContext == null)
            {
                _insertContext = new ContextMenu();
            }

            if (!_insertContext.MenuItems.Contains(_insertVertex))
            {
                _insertContext.MenuItems.Add(_insertVertex);
            }
        }

        /// <summary>
        /// Fires the VertexMoved event.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void OnVertexMoved(VertexMovedEventArgs e)
        {
            VertextMoved?.Invoke(this, e);
        }

        /// <summary>
        /// Before a shape is selected, moving the mouse over a shape will highlight that shape by changing
        /// its appearance. This tests features to determine the first feature to qualify as the highlight.
        /// </summary>
        /// <param name="e">The GeoMouseArgs parameter contains information about the mouse location and geographic coordinates.</param>
        /// <returns>A value indicating whether the shape was successfully highlighted.</returns>
        private bool ShapeHighlight(GeoMouseArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e), "e is null.");

            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            Extent ext = Map.PixelToProj(mouseRect);
            IPolygon env = ext.ToEnvelope().ToPolygon();
            bool requiresInvalidate = false;
            foreach (IFeature feature in _featureSet.Features)
            {
                if (Map.ViewExtents.Intersects(feature.Geometry.EnvelopeInternal))
                {
                    if (_featureSet.FeatureType == FeatureType.Point || _featureSet.FeatureType == FeatureType.MultiPoint)
                    {
                        MapPointLayer mpl = _layer as MapPointLayer;
                        if (mpl != null)
                        {
                            int w = 3;
                            int h = 3;
                            PointCategory pc = mpl.GetCategory(feature) as PointCategory;
                            if (pc != null)
                            {
                                if (pc.Symbolizer.ScaleMode != ScaleMode.Geographic)
                                {
                                    Size2D size = pc.Symbolizer.GetSize();
                                    w = (int)size.Width;
                                    h = (int)size.Height;
                                }
                            }

                            _imageRect = new Rectangle(e.Location.X - (w / 2), e.Location.Y - (h / 2), w, h);
                            if (_imageRect.Contains(Map.ProjToPixel(feature.Geometry.Coordinates[0])))
                            {
                                _activeFeature = feature;
                                _oldCategory = mpl.GetCategory(feature);
                                if (_selectedCategory == null)
                                {
                                    _selectedCategory = _oldCategory.Copy();
                                    _selectedCategory.SetColor(Color.Red);
                                    _selectedCategory.LegendItemVisible = false;
                                }

                                mpl.SetCategory(_activeFeature, _selectedCategory);
                            }
                        }

                        requiresInvalidate = true;
                    }
                    else
                    {
                        if (feature.Geometry.Intersects(env))
                        {
                            _activeFeature = feature;
                            _oldCategory = _layer.GetCategory(_activeFeature);

                            if (_featureSet.FeatureType == FeatureType.Polygon)
                            {
                                IPolygonCategory pc = _activeCategory as IPolygonCategory;
                                if (pc == null)
                                {
                                    _activeCategory = new PolygonCategory(Color.FromArgb(55, 255, 0, 0), Color.Red, 1)
                                    {
                                        LegendItemVisible = false
                                    };
                                }
                            }

                            if (_featureSet.FeatureType == FeatureType.Line)
                            {
                                ILineCategory pc = _activeCategory as ILineCategory;
                                if (pc == null)
                                {
                                    _activeCategory = new LineCategory(Color.Red, 3)
                                    {
                                        LegendItemVisible = false
                                    };
                                }
                            }

                            _layer.SetCategory(_activeFeature, _activeCategory);
                            requiresInvalidate = true;
                        }
                    }
                }
            }

            return requiresInvalidate;
        }

        /// <summary>
        /// Highlighting shapes with a mouse over is something that also needs to be undone when the
        /// mouse leaves. This test handles changing the colors back to normal when the mouse leaves a shape.
        /// </summary>
        /// <param name="e">The GeoMouseArgs parameter contains information about the mouse location and geographic coordinates.</param>
        /// <returns>Boolean, true if mapframe initialize (or visual change) is necessary.</returns>
        private bool ShapeRemoveHighlight(GeoMouseArgs e)
        {
            // If no shapes have ever been highlighted, this is meaningless.
            if (_oldCategory == null)
            {
                return false;
            }

            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            Extent ext = Map.PixelToProj(mouseRect);
            MapPointLayer mpl = _layer as MapPointLayer;
            bool requiresInvalidate = false;
            IPolygon env = ext.ToEnvelope().ToPolygon();
            if (mpl != null)
            {
                int w = 3;
                int h = 3;
                PointCategory pc = mpl.GetCategory(_activeFeature) as PointCategory;
                if (pc != null)
                {
                    if (pc.Symbolizer.ScaleMode != ScaleMode.Geographic)
                    {
                        w = (int)pc.Symbolizer.GetSize().Width;
                        h = (int)pc.Symbolizer.GetSize().Height;
                    }
                }

                Rectangle rect = new Rectangle(e.Location.X - (w / 2), e.Location.Y - (h / 2), w * 2, h * 2);
                if (!rect.Contains(Map.ProjToPixel(_activeFeature.Geometry.Coordinates[0])))
                {
                    mpl.SetCategory(_activeFeature, _oldCategory);
                    _activeFeature = null;
                    requiresInvalidate = true;
                }
            }
            else
            {
                if (!_activeFeature.Geometry.Intersects(env))
                {
                    _layer.SetCategory(_activeFeature, _oldCategory);
                    _activeFeature = null;
                    requiresInvalidate = true;
                }
            }

            return requiresInvalidate;
        }

        private void UpdateDragCoordinate(Coordinate loc)
        {
            // Cannot change selected feature at this time because we are dragging a vertex
            _dragCoord.X = loc.X;
            _dragCoord.Y = loc.Y;
            if (_closedCircleCoord != null)
            {
                _closedCircleCoord.X = loc.X;
                _closedCircleCoord.Y = loc.Y;
            }

            _featureSet.InitializeVertices();
            if (_featureSet.FeatureType != FeatureType.Point && _featureSet.FeatureType != FeatureType.MultiPoint)
            { _selectedFeature.UpdateEnvelope(); }

            Map.Refresh();
        }

        private void VertexHighlight()
        {
            // The feature is selected so color vertex that can be moved but don't highlight other shapes.
            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            Extent ext = Map.PixelToProj(mouseRect);
            Envelope env = ext.ToEnvelope();

            NetTopologySuite.Geometries.Point mouse_onMap = new NetTopologySuite.Geometries.Point(Map.PixelToProj(_mousePosition));
            IGeometry featGeom = _selectedFeature.Geometry;

            if (_activeVertex != null && !ext.Contains(_activeVertex))
            {
                _activeVertex = null;
                Map.Invalidate();
            }

            int coordCounter = 0;
            foreach (Coordinate c in _selectedFeature.Geometry.Coordinates)
            {
                if (ext.Contains(c))
                {
                    _activeVertex = c;
                    Map.Invalidate();
                }
                else
                {
                    if (DoEdgeSnapping)
                    {
                        if (_selectedFeature.FeatureType != FeatureType.Point && _selectedFeature.FeatureType != FeatureType.MultiPoint)
                        {
                            if (coordCounter > 0)
                            {
                                double edge_Distance = 0;
                                if (Layer.DataSet.CoordinateType.Equals(CoordinateType.Z))
                                {
                                    edge_Distance = _selectedFeature.Geometry.Coordinates[coordCounter - 1].Distance3D(c);
                                }
                                else
                                {
                                    edge_Distance = _selectedFeature.Geometry.Coordinates[coordCounter - 1].Distance(c);
                                }

                                if (edge_Distance > 0)
                                {
                                    List<Coordinate> edgeCoords = new List<Coordinate>();
                                    edgeCoords.Add(_selectedFeature.Geometry.Coordinates[coordCounter - 1]);
                                    edgeCoords.Add(c);

                                    LineString edge = new LineString(edgeCoords.ToArray());

                                    if (mouse_onMap.Distance(edge) < (env.Width / 2))
                                    {
                                        NetTopologySuite.LinearReferencing.LengthIndexedLine indexedEedge = new NetTopologySuite.LinearReferencing.LengthIndexedLine(edge);
                                        double proj_tIndex = indexedEedge.Project(mouse_onMap.Coordinate);

                                        _activeVertex = indexedEedge.ExtractPoint(proj_tIndex);
                                        _mousePosition = Map.ProjToPixel(_activeVertex);
                                        SnappingType = "e";
                                        DoMouseMoveForSnapDrawing(false, _mousePosition);
                                    }
                                }
                            }
                        }
                    }
                }

                coordCounter++;
            }
        }

        #endregion
    }
}