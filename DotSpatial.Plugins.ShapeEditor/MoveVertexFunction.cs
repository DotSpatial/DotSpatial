// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.ShapeEditor.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/27/2010 11:28:32 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using System;

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
        #region Private Variables

        private IFeatureCategory _activeCategory;
        private IFeature _activeFeature; // not yet selected
        private Coordinate _activeVertex;
        private Coordinate _closedCircleCoord;
        private Coordinate _dragCoord;
        private bool _dragging;
        private IFeatureSet _featureSet;
        private Rectangle _imageRect;
        private IFeatureLayer _layer;
        private Point _mousePosition;
        private Coordinate _nextPoint;
        private IFeatureCategory _oldCategory;
        private Coordinate _previousPoint;
        private IFeatureCategory _selectedCategory;
        private IFeature _selectedFeature;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MoveVertexFunction class.
        /// </summary>
        public MoveVertexFunction()
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the MoveVertexFunction class where the Map will be defined.
        /// </summary>
        /// <param name="map">The map control that implements the IMap interface.</param>
        public MoveVertexFunction(IMap map)
            : base(map)
        {
            Configure();
        }

        private void Configure()
        {
            YieldStyle = (YieldStyles.LeftButton | YieldStyles.RightButton);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnDraw(MapDrawArgs e)
        {
            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            if (_selectedFeature != null)
            {
                foreach (Coordinate c in _selectedFeature.Coordinates)
                {
                    Point pt = e.GeoGraphics.ProjToPixel(c);
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
                    Point b = _mousePosition;
                    Pen p = new Pen(Color.Blue) { DashStyle = DashStyle.Dash };
                    if (_previousPoint != null)
                    {
                        Point a = e.GeoGraphics.ProjToPixel(_previousPoint);
                        e.Graphics.DrawLine(p, a, b);
                    }
                    if (_nextPoint != null)
                    {
                        Point c = e.GeoGraphics.ProjToPixel(_nextPoint);
                        e.Graphics.DrawLine(p, b, c);
                    }
                    p.Dispose();
                }
            }
        }

        private void UpdateDragCoordiante(Coordinate loc)
        {
            // Cannot change selected feature at this time because we are dragging a vertex
            _dragCoord.X = loc.X;
            _dragCoord.Y = loc.Y;
            if (_closedCircleCoord != null)
            {
                _closedCircleCoord.X = loc.X;
                _closedCircleCoord.Y = loc.Y;
            }
            Map.Invalidate();
        }

        private void VertexHighlight()
        {
            // The feature is selected so color vertex that can be moved
            // but don't highlight other shapes.
            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            Extent ext = Map.PixelToProj(mouseRect);
            if (!(_activeVertex == null))
            {
                if (!ext.Contains(_activeVertex))
                {
                    _activeVertex = null;
                    Map.Invalidate();
                }
            }
            foreach (Coordinate c in _selectedFeature.Coordinates)
            {
                if (ext.Contains(c))
                {
                    _activeVertex = c;
                    Map.Invalidate();
                }
            }
        }

        /// <summary>
        /// Highlighting shapes with a mouse over is something that also needs to be undone when the
        /// mouse leaves.  This test handles changing the colors back to normal when the mouse leaves a shape.
        /// </summary>
        /// <param name="e">The GeoMouseArgs parameter contains information about the mouse location and geographic coordinates.</param>
        /// <returns>Boolean, true if mapframe initialize (or visual change) is necessary.</returns>
        private bool ShapeRemoveHighlight(GeoMouseArgs e)
        {
            // If no shapes have ever been highlighted, this is meaningless.
            if (_oldCategory == null) { return false; }
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
                if (!rect.Contains(Map.ProjToPixel(_activeFeature.Coordinates[0])))
                {
                    mpl.SetCategory(_activeFeature, _oldCategory);
                    _activeFeature = null;
                    requiresInvalidate = true;
                }
            }
            else
            {
                if (!_activeFeature.Intersects(env))
                {
                    _layer.SetCategory(_activeFeature, _oldCategory);
                    _activeFeature = null;
                    requiresInvalidate = true;
                }
            }
            return requiresInvalidate;
        }

        /// <summary>
        /// Before a shape is selected, moving the mouse over a shape will highlight that shape by changing
        /// its appearance.  This tests features to determine the first feature to qualify as the highlight.
        /// </summary>
        /// <param name="e">The GeoMouseArgs parameter contains information about the mouse location
        /// and geographic coordinates.</param>
        /// <returns>A value indicating whether the shape was successfully highlighted.</returns>
        private bool ShapeHighlight(GeoMouseArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e", "e is null.");

            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            Extent ext = Map.PixelToProj(mouseRect);
            IPolygon env = ext.ToEnvelope().ToPolygon();
            bool requiresInvalidate = false;
            foreach (IFeature feature in _featureSet.Features)
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
                        if (_imageRect.Contains(Map.ProjToPixel(feature.Coordinates[0])))
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
                    if (feature.Intersects(env))
                    {
                        _activeFeature = feature;
                        _oldCategory = _layer.GetCategory(_activeFeature);

                        if (_featureSet.FeatureType == FeatureType.Polygon)
                        {
                            IPolygonCategory pc = _activeCategory as IPolygonCategory;
                            if (pc == null)
                            {
                                _activeCategory = new PolygonCategory(Color.FromArgb(55, 255, 0, 0), Color.Red, 1) { LegendItemVisible = false };
                            }
                        }
                        if (_featureSet.FeatureType == FeatureType.Line)
                        {
                            ILineCategory pc = _activeCategory as ILineCategory;
                            if (pc == null)
                            {
                                _activeCategory = new LineCategory(Color.Red, 3) { LegendItemVisible = false };
                            }
                        }
                        _layer.SetCategory(_activeFeature, _activeCategory);
                        requiresInvalidate = true;
                    }
                }
            }
            return requiresInvalidate;
        }

        /// <inheritdoc />
        protected override void OnMouseMove(GeoMouseArgs e)
        {
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

                UpdateDragCoordiante(snappedCoord); // Snapping changes
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
                        if (ShapeRemoveHighlight(e)) { requiresInvalidate = true; }
                    }
                    if (_activeFeature == null)
                    {
                        if (ShapeHighlight(e)) { requiresInvalidate = true; }
                    }

                    if (requiresInvalidate)
                    {
                        Map.MapFrame.Initialize();
                        Map.Invalidate();
                    }
                }

                // check to see if the coordinates intersect with a shape in our current featureset.
            }
        }

        /// <summary>
        /// This function checks to see if the current mouse location is over a vertex.
        /// </summary>
        /// <param name="e">The GeoMouseArgs parameter contains information about the mouse
        /// location and geographic coordinates.</param>
        private bool CheckForVertexDrag(GeoMouseArgs e)
        {
            Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);
            IEnvelope env = Map.PixelToProj(mouseRect).ToEnvelope();
            if (e.Button == MouseButtons.Left)
            {
                if (_layer.DataSet.FeatureType == FeatureType.Polygon)
                {
                    for (int prt = 0; prt < _selectedFeature.NumGeometries; prt++)
                    {
                        IBasicGeometry g = _selectedFeature.GetBasicGeometryN(prt);
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
                                Map.Invalidate();
                                return true;
                            }
                        }
                    }
                }
                else if (_layer.DataSet.FeatureType == FeatureType.Line)
                {
                    for (int prt = 0; prt < _selectedFeature.NumGeometries; prt++)
                    {
                        IBasicGeometry g = _selectedFeature.GetBasicGeometryN(prt);
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
                                Map.Invalidate();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <inheritdoc />
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            _mousePosition = e.Location;
            if (_dragging)
            {
                if (e.Button == MouseButtons.Right)
                {
                    _dragging = false;
                    Map.Invalidate();
                    Map.IsBusy = false;
                }
            }
            else
            {
                if (_selectedFeature != null)
                {
                    Rectangle mouseRect = new Rectangle(_mousePosition.X - 3, _mousePosition.Y - 3, 6, 6);

                    IEnvelope env = Map.PixelToProj(mouseRect).ToEnvelope();

                    if (CheckForVertexDrag(e)) { return; }

                    // No vertex selection has occured.
                    if (!_selectedFeature.Intersects(env.ToPolygon()))
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
                            _selectedCategory = new LineCategory(Color.Cyan, 1) { LegendItemVisible = false };
                        }
                        _layer.SetCategory(_selectedFeature, _selectedCategory);
                    }
                    else
                    {
                        _dragging = true;
                        Map.IsBusy = true;
                        _dragCoord = _activeFeature.Coordinates[0];
                        MapPointLayer mpl = _layer as MapPointLayer;
                        if (mpl != null)
                        {
                            mpl.SetVisible(_activeFeature, false);
                        }
                        IPointCategory sc = _selectedCategory as IPointCategory;
                        if (sc == null)
                        {
                            IPointSymbolizer ps =
                                _layer.GetCategory(_activeFeature).Symbolizer.Copy() as IPointSymbolizer;
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
            DeselectFeature();
            RemoveHighlightFromFeature();
            _oldCategory = null;
            base.OnDeactivate();
        }

        /// <inheritdoc />
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button == MouseButtons.Left && _dragging)
            {
                _dragging = false;
                Map.IsBusy = false;
                _featureSet.InvalidateVertices();

                if (_featureSet.FeatureType == FeatureType.Point || _featureSet.FeatureType == FeatureType.MultiPoint)
                {
                    if (_activeFeature == null) { return; }
                    if (_layer.GetCategory(_activeFeature) != _selectedCategory)
                    {
                        _layer.SetCategory(_activeFeature, _selectedCategory);
                        _layer.SetVisible(_activeFeature, true);
                    }
                }
                else
                {
                    if (_selectedFeature == null) { return; }
                    if (_layer.GetCategory(_selectedFeature) != _selectedCategory)
                    {
                        _layer.SetCategory(_selectedFeature, _selectedCategory);
                    }
                }
            }
            Map.MapFrame.Initialize();
        }

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
    }
}