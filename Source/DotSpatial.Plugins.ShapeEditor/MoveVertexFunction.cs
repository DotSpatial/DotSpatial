// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
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
        private IGeometry _draggingGeom_Old;
        private bool _dragging;
        private bool _draggingShape;
        private IFeatureSet _featureSet;
        private Rectangle _imageRect;
        private IFeatureLayer _layer;
        private System.Drawing.Point _mousePosition;
        private System.Drawing.Point _mousePosition_Old;
        private Coordinate _nextPoint;
        private IFeatureCategory _oldCategory;
        private Coordinate _previousPoint;
        private IFeatureCategory _selectedCategory;
        private IFeature _selectedFeature;
        private ContextMenu _insertContext;
        private ContextMenu _deleteContext;
        private MenuItem _insertVertex;
        private MenuItem _deleteVertex;
        private MenuItem _continueFromStartPoint;
        private MenuItem _continueFromEndPoint;
        private ButtonHandler _myHandler;
        private bool _featContinue;
        private bool _featContinue_Start;
        private bool _featContinue_End;
        private List<Coordinate> _coords_Old;
        private List<Coordinate> _coordsContinue = new List<Coordinate>();
        private CultureInfo _moveVertexCulture;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveVertexFunction"/> class where the Map will be defined.
        /// </summary>
        /// <param name="map">The map control that implements the IMap interface.</param>
        /// <param name="handler"> The handler</param>
        public MoveVertexFunction(IMap map, ButtonHandler handler)
            : base(map)
        {
            _myHandler = handler;
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
                _featureSet = _layer?.DataSet;
                InitializeSnapLayers();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShapeDragging  is performed or not.
        /// </summary>
        public bool DoShapeDragging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vertices will be showned or not.
        /// </summary>
        public bool DoShowVerticesIndex { get; set; }

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo MoveVertexCulture
        {
            set
            {
                if (_moveVertexCulture == value) return;

                _moveVertexCulture = value;

                if (_moveVertexCulture == null) _moveVertexCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _moveVertexCulture;
                Thread.CurrentThread.CurrentUICulture = _moveVertexCulture;

                UpdateMoveResources();
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
            if (_selectedFeature == null) return;
            if (SnappedFeature != _selectedFeature) return;
            if (SnappingType != SnappingTypeVertex) return;
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

                    LinearRing newLinearRing = new LinearRing(newCoords.ToArray());

                    _selectedFeature.Geometry = new Polygon(newLinearRing);

                    break;
            }

            _featureSet.InitializeVertices();
            _selectedFeature.UpdateEnvelope();
            OnVertexMoved(new VertexMovedEventArgs(_selectedFeature));

            Map.Refresh();
        }

        /// <summary>
        /// Insert a vertex at the snapped coordinate.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">An empty EventArgs class.</param>
        public void Vertex_Insert(object sender, EventArgs e)
        {
            if (_selectedFeature == null) return;
            if (SnappedFeature != _selectedFeature) return;
            if (SnappingType != SnappingTypeEdge) return;
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
            OnVertexMoved(new VertexMovedEventArgs(_selectedFeature));

            Map.Invalidate();
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
                _layer.Symbology.RemoveCategory(_activeCategory); // To leave the Symbology cleaned
                _layer.Symbology.RemoveCategory(_selectedCategory); // To leave the Symbology cleaned
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

            if (_myHandler != null)
            {
                _myHandler.Buttons_Disable("Shape_Dragging");
            }
        }

        /// <inheritdoc />
        protected override void OnDraw(MapDrawArgs e)
        {
            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            Rectangle mouseRectVertex = new Rectangle(_mousePosition.X - 6, _mousePosition.Y - 6, 12, 12);
            Rectangle mouseRectEdge = new Rectangle(_mousePosition.X - 2, _mousePosition.Y - 2, 4, 4);

            if (_selectedFeature != null)
            {
                int coordCounter = 0;

                foreach (Coordinate c in _selectedFeature.Geometry.Coordinates)
                {
                    System.Drawing.Point pt = e.GeoGraphics.ProjToPixel(c);
                    if (e.GeoGraphics.ImageRectangle.Contains(pt))
                    {
                        e.Graphics.FillRectangle(Brushes.Blue, pt.X - 2, pt.Y - 2, 4, 4);

                        if (DoShowVerticesIndex)
                        {
                            e.Graphics.DrawString(coordCounter.ToString(), new Font(new FontFamily("Arial"), 10, GraphicsUnit.Pixel), Brushes.Red, pt.X, pt.Y);
                        }
                    }

                    // && mouseRect.Contains(pt))
                    if (SnappingType == SnappingTypeVertex)
                    {
                        if (SnappedFeature.Equals(_selectedFeature))
                        {
                            e.Graphics.FillEllipse(Brushes.Red, mouseRect);
                        }
                        else
                        {
                            e.Graphics.DrawEllipse(Pens.Red, mouseRectVertex);
                        }
                    }

                    if (SnappingType == SnappingTypeEdge && SnappedCoordIndex > 0)
                    {
                        e.Graphics.FillEllipse(Brushes.Red, mouseRect);
                    }

                    coordCounter++;
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
            _mousePosition = e.Location;

            if (e.Button == MouseButtons.Right)
            {
                if (_dragging)
                {
                    Cancel_Dragging();
                }

                if (_featContinue)
                {
                    UpdateContinueShape_GoBack();
                }
            }

            // if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            if (e.Button == MouseButtons.Left)
            {
                if (_selectedFeature != null)
                {
                    Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);

                    Envelope env = Map.PixelToProj(mouseRect).ToEnvelope();

                    Coordinate snappedCoord = e.GeographicLocation;
                    ComputeSnappedLocation(e, ref snappedCoord);

                    if (DoShapeDragging)
                    {
                        if (CheckForShapeDrag(e))
                        {
                            Map.MapFrame.SuspendEvents();
                        }

                        return;
                    }
                    else if (_featContinue)
                    {
                        if (_featContinue_End)
                        {
                            _coords_Old.Add(snappedCoord);
                        }
                        else
                        {
                            _coords_Old.Insert(0, snappedCoord);
                        }

                        return;
                    }
                    else if (CheckForVertexDrag(e))
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

            base.OnMouseDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            SnappingType = string.Empty;
            SnappedCoordIndex = 0;

            _mousePosition = e.Location;
            Coordinate snappedCoord = e.GeographicLocation;
            bool snapOccured = false;

            if (_dragging || _draggingShape || DoShapeDragging || _featContinue)
            {
                snapOccured = ComputeSnappedLocation(e, ref snappedCoord);

                // Begin snapping changes
                if (snapOccured)
                {
                    _mousePosition = Map.ProjToPixel(snappedCoord);
                }
            }

            if (_dragging)
            {

                // End snapping changes
                UpdateDragCoordinate(snappedCoord); // Snapping changes
            }
            else if (_draggingShape)
            {

                // End snapping changes
                UpdateDragShapeCoordinate(snappedCoord); // Snapping changes
            }
            else if (_featContinue)
            {

                // End snapping changes
                UpdateContinueShape(snappedCoord); // Snapping changes
            }
            else
            {
                if (_selectedFeature != null)
                {
                    snapOccured = ComputeSnappedLocation(e, ref snappedCoord);

                    if (snapOccured)
                    {
                        _mousePosition = Map.ProjToPixel(snappedCoord);
                    }

                    Map.Invalidate();
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
            Rectangle mouseRect = new Rectangle(e.X - SnapTol, e.Y - SnapTol, SnapTol * 2, SnapTol * 2);

            if (e.Button == MouseButtons.Right)
            {
                if (_featContinue) return;

                Coordinate snappedCoord = e.GeographicLocation;
                ComputeSnappedLocation(e, ref snappedCoord);

                if (_featureSet.FeatureType != FeatureType.Point && _featureSet.FeatureType != FeatureType.MultiPoint)
                {
                    if (_selectedFeature != null)
                    {
                        if (ComputeSnappedLocation_ForSelectedFeature(mouseRect, ref _selectedFeature, Layer, ref snappedCoord))
                        {
                            _mousePosition = Map.ProjToPixel(snappedCoord);

                            if (SnappingType == SnappingTypeVertex && _selectedFeature.Equals(SnappedFeature))
                            {
                                // CheckSnappedCoord_On_SelectedFeature(ref _selectedFeature, _mousePosition);
                                Continue_PrepareMenu("Delete");
                                _deleteContext.Show((Control)Map, e.Location);
                            }

                            if (SnappingType == SnappingTypeEdge && _selectedFeature.Equals(SnappedFeature) && SnappedCoordIndex > 0)
                            {
                                // CheckSnappedCoord_On_SelectedFeature(ref _selectedFeature, _mousePosition);
                                Continue_PrepareMenu("Insert");
                                _insertContext.Show((Control)Map, e.Location);
                            }
                        }
                    }
                }
            }

            if (e.Button == MouseButtons.Left && (_dragging || _draggingShape))
            {
                _dragging = false;
                _draggingShape = false;
                Map.IsBusy = false;
                Map.MapFrame.ResumeEvents();

                _featureSet.InvalidateVertices();
                _featureSet.InitializeVertices();
                _featureSet.UpdateExtent();

                if (_featureSet.FeatureType == FeatureType.Point || _featureSet.FeatureType == FeatureType.MultiPoint)
                {
                    if (_activeFeature == null)
                    {
                        return;
                    }

                    OnVertexMoved(new VertexMovedEventArgs(_selectedFeature));

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
        /// Capture the key Press event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 8: // BackSpace Key
                    if (_featContinue)
                    {
                        // UpdateContinueShape_GoBack();
                    }

                    break;

                case 13: // Enter Key
                    if (_featContinue)
                    {
                        UpdateContinueShape_Close();
                    }

                    break;

                case 27:
                    if (_dragging || _draggingShape)
                    {
                        Cancel_Dragging();
                        return;
                    }

                    if (DoShapeDragging)
                    {
                        DoShapeDragging = false;
                        Map.Cursor = Cursors.Cross;

                        if (_myHandler != null)
                        {
                            _myHandler.Buttons_Deactivate("Shape_Dragging");
                            return;
                        }
                    }

                    if (_featContinue)
                    {
                        Cancel_Dragging();
                        return;
                    }

                    if (_selectedFeature != null && e.KeyValue == 27)
                    {
                        DeselectFeature();
                        return;
                    }

                    break;
            }
        }

        /// <summary>
        /// Capture the key Press event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
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

        /// <summary>
        /// This function checks to see if the current mouse location is over a vertex.
        /// </summary>
        /// <param name="e">The GeoMouseArgs parameter contains information about the mouse location and geographic coordinates.</param>
        /// <returns>True, if the current mouse location is over a vertex.</returns>
        private bool CheckForShapeDrag(GeoMouseArgs e)
        {
            Rectangle mouseRect = new Rectangle(_mousePosition.X - SnapTol, _mousePosition.Y - SnapTol, 2 * SnapTol, 2 * SnapTol);
            Envelope env = Map.PixelToProj(mouseRect).ToEnvelope();
            if (e.Button == MouseButtons.Left && _selectedFeature != null)
            {
                _dragCoord = Map.PixelToProj(e.Location);
                _mousePosition_Old = e.Location;

                IGeometry featGeom = _selectedFeature.Geometry;

                if (env.ToPolygon().Intersects(featGeom))
                {
                    if (ComputeSnappedLocation(e, ref _dragCoord))
                    {
                        _mousePosition = Map.ProjToPixel(_dragCoord);
                        _mousePosition_Old = Map.ProjToPixel(_dragCoord);
                    }
                }

                _draggingShape = true;
                _dragCoord_Old = _dragCoord.Copy();
                _draggingGeom_Old = featGeom.Copy();

                Map.Invalidate();
                return true;
            }

            return false;
        }

        private void Configure()
        {
            YieldStyle = YieldStyles.LeftButton | YieldStyles.RightButton;

            if (_continueFromStartPoint == null)
            {
                _continueFromStartPoint = new MenuItem(string.Empty, Continue_From_Start);
            }

            if (_continueFromEndPoint == null)
            {
                _continueFromEndPoint = new MenuItem(string.Empty, Continue_From_End);
            }

            if (_deleteVertex == null)
            {
                _deleteVertex = new MenuItem(string.Empty, Vertex_Delete);
            }

            if (_deleteContext == null)
            {
                _deleteContext = new ContextMenu();
            }

            if (!_deleteContext.MenuItems.Contains(_continueFromStartPoint))
            {
                _deleteContext.MenuItems.Add(_continueFromStartPoint);
            }

            if (!_deleteContext.MenuItems.Contains(_deleteVertex))
            {
                _deleteContext.MenuItems.Add(_deleteVertex);
            }

            if (!_deleteContext.MenuItems.Contains(_continueFromEndPoint))
            {
                _deleteContext.MenuItems.Add(_continueFromEndPoint);
            }

            // _
            if (_insertVertex == null)
            {
                _insertVertex = new MenuItem(string.Empty, Vertex_Insert);
            }

            if (_insertContext == null)
            {
                _insertContext = new ContextMenu();
            }

            if (!_insertContext.MenuItems.Contains(_insertVertex))
            {
                _insertContext.MenuItems.Add(_insertVertex);
            }

            if (!_insertContext.MenuItems.Contains(_continueFromEndPoint))
            {
                _insertContext.MenuItems.Add(_continueFromEndPoint);
            }

            MoveVertexCulture = new CultureInfo(string.Empty);
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

            if (_layer == null) return false;
            if (_featureSet == null) return false;

            if (_activeCategory != null) _layer.Symbology.RemoveCategory(_activeCategory);
            if (_selectedCategory != null) _layer.Symbology.RemoveCategory(_selectedCategory);

            Rectangle mouseRect = new Rectangle(_mousePosition.X - SnapTol, _mousePosition.Y - SnapTol, 2 * SnapTol, 2 * SnapTol);
            Extent ext = Map.PixelToProj(mouseRect);
            IPolygon env = ext.ToEnvelope().ToPolygon();

            foreach (IFeature feature in _featureSet.Features)
            {
                if (Map.ViewExtents.Intersects(feature.Geometry.EnvelopeInternal))
                {
                    if (_featureSet.FeatureType == FeatureType.Point || _featureSet.FeatureType == FeatureType.MultiPoint)
                    {
                        MapPointLayer mpl = _layer as MapPointLayer;
                        if (mpl != null)
                        {
                            int w = SnapTol;
                            int h = SnapTol;

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

                            // _imageRect = new Rectangle(e.Location.X - (w / 2), e.Location.Y - (h / 2), w, h);
                            _imageRect = new Rectangle(e.Location.X - SnapTol, e.Location.Y - SnapTol, 2 * SnapTol, 2 * SnapTol); // Points side to side react anormaly
                            if (_imageRect.Contains(Map.ProjToPixel(feature.Geometry.Coordinates[0])))
                            {
                                _activeFeature = feature;
                                _oldCategory = mpl.GetCategory(feature);

                                _selectedCategory = _oldCategory.Copy();
                                _selectedCategory.SetColor(Color.Red);
                                _selectedCategory.LegendItemVisible = false;

                                mpl.SetCategory(_activeFeature, _selectedCategory);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (feature.Geometry.Intersects(env))
                        {
                            _activeFeature = feature;
                            _oldCategory = _layer.GetCategory(_activeFeature);

                            if (_featureSet.FeatureType == FeatureType.Polygon)
                            {
                                IPolygonCategory polc = _activeCategory as IPolygonCategory;
                                if (polc == null)
                                {
                                    _activeCategory = new PolygonCategory(Color.FromArgb(55, 255, 0, 0), Color.Red, 1)
                                    {
                                        LegendItemVisible = false
                                    };
                                }
                            }

                            if (_featureSet.FeatureType == FeatureType.Line)
                            {
                                ILineCategory lc = _activeCategory as ILineCategory;
                                if (lc == null)
                                {
                                    _activeCategory = new LineCategory(Color.Red, 3)
                                    {
                                        LegendItemVisible = false
                                    };
                                }
                            }

                            _layer.SetCategory(_activeFeature, _activeCategory);
                            return true;
                        }
                    }
                }
            }

            return false;
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

            Rectangle mouseRect = new Rectangle(_mousePosition.X - SnapTol, _mousePosition.Y - SnapTol, 2 * SnapTol, 2 * SnapTol);
            Extent ext = Map.PixelToProj(mouseRect);
            MapPointLayer mpl = _layer as MapPointLayer;

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

                // Rectangle rect = new Rectangle(e.Location.X - (w / 2), e.Location.Y - (h / 2), w * 2, h * 2);
                Rectangle rect = new Rectangle(e.Location.X - SnapTol, e.Location.Y - SnapTol, SnapTol * 2, SnapTol * 2);
                if (!rect.Contains(Map.ProjToPixel(_activeFeature.Geometry.Coordinates[0])))
                {
                    mpl.SetCategory(_activeFeature, _oldCategory);
                    _activeFeature = null;

                    _layer.Symbology.RemoveCategory(_selectedCategory); // To keep the Symbology cleaned
                    _selectedCategory = null;
                    return true; // Why continue when an active feature is crossed;
                }
            }
            else
            {
                if (!_activeFeature.Geometry.Intersects(env))
                {
                    _layer.SetCategory(_activeFeature, _oldCategory);
                    _activeFeature = null;

                    _layer.Symbology.RemoveCategory(_activeCategory); // To keep the Symbology cleaned
                    _layer.Symbology.RemoveCategory(_selectedCategory);
                    _activeCategory = null;
                    _selectedCategory = null;
                    return true; // Why continue when an active feature is crossed;
                }
            }

            return false;
        }

        private void UpdateDragCoordinate(Coordinate loc)
        {
            if (!_dragging) return;

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
            {
                _selectedFeature.UpdateEnvelope();
            }

            _featureSet.ShapeIndices = null;

            Map.Refresh();
        }

        private void UpdateDragShapeCoordinate(Coordinate loc)
        {
            if (!_draggingShape) return;

            // Cannot change selected feature at this time because we are dragging a .
            IGeometry featGeom = _selectedFeature.Geometry.Copy();

            for (int ic = 0; ic < _selectedFeature.Geometry.Coordinates.Length; ic++)
            {
                Coordinate c = _selectedFeature.Geometry.Coordinates[ic];

                c.X = _draggingGeom_Old.Coordinates[ic].X + (loc.X - _dragCoord_Old.X);
                c.Y = _draggingGeom_Old.Coordinates[ic].Y + (loc.Y - _dragCoord_Old.Y);
            }

            if (!_selectedFeature.Geometry.IsValid)
            {
                _selectedFeature.Geometry = featGeom;
            }

            _featureSet.InitializeVertices();

            if (_featureSet.FeatureType != FeatureType.Point && _featureSet.FeatureType != FeatureType.MultiPoint)
            { _selectedFeature.UpdateEnvelope(); }

            _featureSet.ShapeIndices = null;
            _featureSet.UpdateExtent();

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

            bool doCoord_Snap;
            int coordCounter = 0;
            foreach (Coordinate c in _selectedFeature.Geometry.Coordinates)
            {
                doCoord_Snap = true;

                if (Layer.SnapVertices)
                {
                    if (coordCounter == 0 && !Layer.SnapStartPoint) doCoord_Snap = false;
                    if (coordCounter == (_selectedFeature.Geometry.Coordinates.Length - 1) && !Layer.SnapEndPoint) doCoord_Snap = false;

                    if (doCoord_Snap)
                    {
                        if (ext.Contains(c))
                        {
                            _activeVertex = c;
                            Map.Invalidate();
                        }
                    }
                }

                if (coordCounter > 0 && Layer.SnapEdges && _selectedFeature.FeatureType != FeatureType.Point && _selectedFeature.FeatureType != FeatureType.MultiPoint)
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

                            // _activeVertex = indexedEedge.ExtractPoint(proj_tIndex);
                            _mousePosition = Map.ProjToPixel(indexedEedge.ExtractPoint(proj_tIndex));
                            SnappingType = SnappingTypeEdge;
                            DoMouseMoveForSnapDrawing(false, _mousePosition);
                        }
                    }
                }

                coordCounter++;
            }
        }

        private void Continue_PrepareMenu(string menuType)
        {
            if (_featureSet.FeatureType == FeatureType.Line && !DoShapeDragging)
            {
                switch (menuType)
                {
                    case "Insert":

                        if (!_insertContext.MenuItems.Contains(_continueFromStartPoint))
                        {
                            _insertContext.MenuItems.Add(0, _continueFromStartPoint);
                        }

                        if (!_insertContext.MenuItems.Contains(_continueFromEndPoint))
                        {
                            _insertContext.MenuItems.Add(_continueFromEndPoint);
                        }

                        break;

                    case "Delete":

                        if (!_deleteContext.MenuItems.Contains(_continueFromStartPoint))
                        {
                            _deleteContext.MenuItems.Add(0, _continueFromStartPoint);
                        }

                        if (!_deleteContext.MenuItems.Contains(_continueFromEndPoint))
                        {
                            _deleteContext.MenuItems.Add(_continueFromEndPoint);
                        }

                        break;
                }
            }
            else
            {
                if (_insertContext.MenuItems.Contains(_continueFromStartPoint))
                {
                    _insertContext.MenuItems.Remove(_continueFromStartPoint);
                }

                if (_insertContext.MenuItems.Contains(_continueFromEndPoint))
                {
                    _insertContext.MenuItems.Remove(_continueFromEndPoint);
                }

                if (_deleteContext.MenuItems.Contains(_continueFromStartPoint))
                {
                    _deleteContext.MenuItems.Remove(_continueFromStartPoint);
                }

                if (_deleteContext.MenuItems.Contains(_continueFromEndPoint))
                {
                    _deleteContext.MenuItems.Remove(_continueFromEndPoint);
                }
            }
        }

        private void Cancel_Dragging()
        {
           if (_dragging)
            {
                _dragging = false;
                _dragCoord.X = _dragCoord_Old.X;
                _dragCoord.Y = _dragCoord_Old.Y;
            }

           if (_draggingShape)
            {
                _draggingShape = false;
                _selectedFeature.Geometry = _draggingGeom_Old;
            }

           if (_featContinue)
            {
                _featContinue = false;
                _featContinue_Start = false;
                _featContinue_End = false;

                _selectedFeature.Geometry = _draggingGeom_Old;
            }

           if (_featureSet.FeatureType != FeatureType.Point && _featureSet.FeatureType != FeatureType.MultiPoint)
            {
                _selectedFeature.UpdateEnvelope();
            }
           else
            {
                _featureSet.InvalidateVertices();
            }

           _featureSet.InitializeVertices();
           _featureSet.UpdateExtent();

           Map.Refresh();
           Map.IsBusy = false;
           Map.MapFrame.ResumeEvents();
        }

        /// <summary>
        /// Continue a feature from it's StartPoint.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">An empty EventArgs class.</param>
        private void Continue_From_Start(object sender, EventArgs e)
        {
            if (_selectedFeature == null) return;
            if (SnappedFeature != _selectedFeature) return;
            if (SnappingType != SnappingTypeEdge && SnappingType != SnappingTypeVertex) return;

            // Coordinate[] coords = _selectedFeature.Geometry.Coordinates;
            IList<Coordinate> coords = _selectedFeature.Geometry.Coordinates;

            if (!Map.ViewExtents.Contains(coords[0]))
            {
                Map.ViewExtents.Center.X = coords[0].X;
                Map.ViewExtents.Center.Y = coords[0].Y;

                Map.MapFrame.ViewExtents = new Extent(coords[0].X - (Map.ViewExtents.Width / 2), coords[0].Y - (Map.ViewExtents.Height / 2), coords[0].X + (Map.ViewExtents.Width / 2), coords[0].Y + (Map.ViewExtents.Height / 2));
            }

            _mousePosition = Map.ProjToPixel(coords[0]);

            _coordsContinue.Clear();
            _draggingGeom_Old = _selectedFeature.Geometry.Copy();
            _coords_Old = coords.CloneList();

            _featContinue = true;
            _featContinue_Start = true;
        }

        /// <summary>
        /// Continue a feature from it's EndPoint.
        /// </summary>
        /// <param name="sender">The object sender</param>
        /// <param name="e">An empty EventArgs class.</param>
        private void Continue_From_End(object sender, EventArgs e)
        {
            if (_selectedFeature == null) return;
            if (SnappedFeature != _selectedFeature) return;
            if (SnappingType != SnappingTypeEdge && SnappingType != SnappingTypeVertex) return;

            // Coordinate[] coords = _selectedFeature.Geometry.Coordinates;
            IList<Coordinate> coords = _selectedFeature.Geometry.Coordinates;

            if (!Map.ViewExtents.Contains(coords[coords.Count - 1]))
            {
                Map.ViewExtents.Center.X = coords[coords.Count - 1].X;
                Map.ViewExtents.Center.Y = coords[coords.Count - 1].Y;

                Map.MapFrame.ViewExtents = new Extent(coords[coords.Count - 1].X - (Map.ViewExtents.Width / 2), coords[coords.Count - 1].Y - (Map.ViewExtents.Height / 2), coords[coords.Count - 1].X + (Map.ViewExtents.Width / 2), coords[coords.Count - 1].Y + (Map.ViewExtents.Height / 2));
            }

            _mousePosition = Map.ProjToPixel(coords[coords.Count - 1]);

            _coordsContinue.Clear();
            _draggingGeom_Old = _selectedFeature.Geometry.Copy();
            _coords_Old = coords.CloneList();

            _featContinue = true;
            _featContinue_End = true;
        }

        private void UpdateContinueShape(Coordinate loc)
        {
            if (_featureSet.FeatureType != FeatureType.Line) return;
            if (!_featContinue) return;

            // Cannot change selected feature at this time because we are dragging a .
            IGeometry featGeom = _selectedFeature.Geometry.Copy();
            LineString newLine;

            if (_featContinue_Start)
            {
                _coordsContinue = _coords_Old.CloneList();
                _coordsContinue.Insert(0, loc);

                newLine = new LineString(_coordsContinue.ToArray());
                _selectedFeature.Geometry = newLine;
            }

            if (_featContinue_End)
            {
                _coordsContinue = _coords_Old.CloneList();
                _coordsContinue.Add(loc);

                newLine = new LineString(_coordsContinue.ToArray());
                _selectedFeature.Geometry = newLine;
            }

            if (!_selectedFeature.Geometry.IsValid)
            {
                _selectedFeature.Geometry = featGeom;
            }

            _featureSet.InitializeVertices();
            _selectedFeature.UpdateEnvelope();

            _featureSet.ShapeIndices = null;
            _featureSet.UpdateExtent();

            Map.Refresh();
        }

        private void UpdateContinueShape_GoBack()
        {
            if (_featureSet.FeatureType != FeatureType.Line) return;
            if (!_featContinue) return;

            // Cannot change selected feature at this time because we are dragging a .
            if (_coords_Old.Count > 0)
            {
                if (_featContinue_End)
                {
                    _coords_Old.RemoveAt(_coords_Old.Count - 1);
                }
                else
                {
                    _coords_Old.RemoveAt(0);
                }

                LineString newLine = new LineString(_coords_Old.ToArray());
                _selectedFeature.Geometry = newLine;

                _featureSet.InitializeVertices();
                _selectedFeature.UpdateEnvelope();

                _featureSet.ShapeIndices = null;
                _featureSet.UpdateExtent();

                Map.Refresh();

                UpdateContinueShape(Map.PixelToProj(_mousePosition));
            }
        }

        private void UpdateContinueShape_Close()
        {
            if (_featureSet.FeatureType != FeatureType.Line) return;
            if (!_featContinue) return;

            // Cannot change selected feature at this time because we are dragging a .
            LineString newLine = new LineString(_coords_Old.ToArray());
            _selectedFeature.Geometry = newLine;

            _featureSet.InitializeVertices();
            _selectedFeature.UpdateEnvelope();

            _featureSet.ShapeIndices = null;
            _featureSet.UpdateExtent();

            _featContinue = false;
            _featContinue_Start = false;
            _featContinue_End = false;

            OnVertexMoved(new VertexMovedEventArgs(_selectedFeature));
            Map.Refresh();
        }

        /// <summary>
        /// updates the objects by using the appropriate values according to a sepecific language.
        /// </summary>
        private void UpdateMoveResources()
        {
            if (_insertVertex != null) _insertVertex.Text = ShapeEditorResources.Move_Ctx_InsertVertex;
            if (_deleteVertex != null) _deleteVertex.Text = ShapeEditorResources.Move_Ctx_DeteleVertex;

            if (_continueFromStartPoint != null) _continueFromStartPoint.Text = ShapeEditorResources.Move_Ctx_ContinueStart;
            if (_continueFromEndPoint != null) _continueFromEndPoint.Text = ShapeEditorResources.Move_Ctx_ContinueEnd;
         }
    }

    #endregion
}
