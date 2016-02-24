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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/11/2009 11:03:39 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// This function allows interacting with the map through mouse clicks to create a new shape.
    /// </summary>
    public class AddShapeFunction : SnappableMapFunction, IDisposable
    {
        #region private variables

        private ContextMenu _context;
        private CoordinateDialog _coordinateDialog;
        private List<Coordinate> _coordinates;
        private bool _disposed;
        private IFeatureSet _featureSet;
        private MenuItem _finishPart;
        private Point _mousePosition;
        private List<List<Coordinate>> _parts;
        private bool _standBy;
        private IMapLineLayer _tempLayer;
        private IFeatureLayer _layer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AddShapeFunction class.  This specifies the Map that this function should be applied to.
        /// </summary>
        /// <param name="map">The map control that implements the IMap interface that this function uses.</param>
        public AddShapeFunction(IMap map)
            : base(map)
        {
            Configure();
        }

        private void Configure()
        {
            YieldStyle = (YieldStyles.LeftButton | YieldStyles.RightButton);
            _context = new ContextMenu();
            _context.MenuItems.Add("Delete", DeleteShape);
            _finishPart = new MenuItem("Finish Part", FinishPart);
            _context.MenuItems.Add(_finishPart);
            _context.MenuItems.Add("Finish Shape", FinishShape);
            _parts = new List<List<Coordinate>>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Forces this function to begin collecting points for building a new shape.
        /// </summary>
        protected override void OnActivate()
        {
            if (_coordinateDialog == null) _coordinateDialog = new CoordinateDialog();

            _coordinateDialog.ShowZValues = _featureSet.CoordinateType == CoordinateType.Z;
            _coordinateDialog.ShowMValues = _featureSet.CoordinateType == CoordinateType.M || _featureSet.CoordinateType == CoordinateType.Z;

            if (_featureSet.FeatureType == FeatureType.Point || _featureSet.FeatureType == FeatureType.MultiPoint)
            {
                if (_context.MenuItems.Contains(_finishPart)) _context.MenuItems.Remove(_finishPart);
            }
            else if (!_context.MenuItems.Contains(_finishPart))
                _context.MenuItems.Add(1, _finishPart);

            _coordinateDialog.Show();
            _coordinateDialog.FormClosing += CoordinateDialogFormClosing;
            if (!_standBy) _coordinates = new List<Coordinate>();
            if (_tempLayer != null)
            {
                Map.MapFrame.DrawingLayers.Remove(_tempLayer);
                Map.MapFrame.Invalidate();
                Map.Invalidate();
                _tempLayer = null;
            }
            _standBy = false;
            base.OnActivate();
        }

        /// <summary>
        /// Allows for new behavior during deactivation.
        /// </summary>
        protected override void OnDeactivate()
        {
            if (_standBy) { return; }

            // Don't completely deactivate, but rather go into standby mode
            // where we draw only the content that we have actually locked in.
            _standBy = true;
            if (_coordinateDialog != null) { _coordinateDialog.Hide(); }
            if (_coordinates != null && _coordinates.Count > 1)
            {
                LineString ls = new LineString(_coordinates);
                FeatureSet fs = new FeatureSet(FeatureType.Line);
                fs.Features.Add(new Feature(ls));
                MapLineLayer gll = new MapLineLayer(fs)
                                       {
                                           Symbolizer =
                                               {
                                                   ScaleMode = ScaleMode.Symbolic,
                                                   Smoothing = true
                                               },
                                           MapFrame = Map.MapFrame
                                       };
                _tempLayer = gll;
                Map.MapFrame.DrawingLayers.Add(gll);
                Map.MapFrame.Invalidate();
                Map.Invalidate();
            }

            base.Deactivate();
        }

        /// <summary>
        /// Handles drawing of editing features.
        /// </summary>
        /// <param name="e">The drawing args for the draw method.</param>
        protected override void OnDraw(MapDrawArgs e)
        {
            if (_standBy) { return; }

            // Begin snapping changes
            DoSnapDrawing(e.Graphics, _mousePosition);
            // End snapping changes

            if (_featureSet.FeatureType == FeatureType.Point) { return; }

            // Draw any completed parts first so that they are behind my active drawing content.
            if (_parts != null)
            {
                GraphicsPath gp = new GraphicsPath();

                List<Point> partPoints = new List<Point>();
                foreach (List<Coordinate> part in _parts)
                {
                    partPoints.AddRange(part.Select(c => Map.ProjToPixel(c)));
                    if (_featureSet.FeatureType == FeatureType.Line)
                    {
                        gp.AddLines(partPoints.ToArray());
                    }
                    if (_featureSet.FeatureType == FeatureType.Polygon)
                    {
                        gp.AddPolygon(partPoints.ToArray());
                    }
                    partPoints.Clear();
                }
                e.Graphics.DrawPath(Pens.Blue, gp);
                if (_featureSet.FeatureType == FeatureType.Polygon)
                {
                    Brush fill = new SolidBrush(Color.FromArgb(70, Color.LightCyan));
                    e.Graphics.FillPath(fill, gp);
                    fill.Dispose();
                }
            }

            Pen bluePen = new Pen(Color.Blue, 2F);
            Pen redPen = new Pen(Color.Red, 3F);
            Brush redBrush = new SolidBrush(Color.Red);
            List<Point> points = new List<Point>();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (_coordinates != null)
            {
                points.AddRange(_coordinates.Select(coord => Map.ProjToPixel(coord)));
                foreach (Point pt in points)
                {
                    e.Graphics.FillRectangle(redBrush, new Rectangle(pt.X - 2, pt.Y - 2, 4, 4));
                }
                if (points.Count > 1)
                {
                    if (_featureSet.FeatureType != FeatureType.MultiPoint)
                    {
                        e.Graphics.DrawLines(bluePen, points.ToArray());
                    }
                }
                if (points.Count > 0 && _standBy == false)
                {
                    if (_featureSet.FeatureType != FeatureType.MultiPoint)
                    {
                        e.Graphics.DrawLine(redPen, points[points.Count - 1], _mousePosition);
                    }
                }
            }

            bluePen.Dispose();
            redPen.Dispose();
            redBrush.Dispose();
            base.OnDraw(e);
        }

        /// <summary>
        /// This method occurs as the mouse moves.
        /// </summary>
        /// <param name="e">The GeoMouseArcs class describes the mouse condition along with geographic coordinates.</param>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            if (_standBy) { return; }
            // Begin snapping changes
            Coordinate snappedCoord = e.GeographicLocation;
            bool prevWasSnapped = this.isSnapped;
            this.isSnapped = ComputeSnappedLocation(e, ref snappedCoord);
            _coordinateDialog.X = snappedCoord.X;
            _coordinateDialog.Y = snappedCoord.Y;
            // End snapping changes

            if (_coordinates != null && _coordinates.Count > 0)
            {
                List<Point> points = _coordinates.Select(coord => Map.ProjToPixel(coord)).ToList();
                Rectangle oldRect = SymbologyGlobal.GetRectangle(_mousePosition, points[points.Count - 1]);
                Rectangle newRect = SymbologyGlobal.GetRectangle(e.Location, points[points.Count - 1]);
                Rectangle invalid = Rectangle.Union(newRect, oldRect);
                invalid.Inflate(20, 20);
                Map.Invalidate(invalid);
            }

            // Begin snapping changes
            _mousePosition = this.isSnapped ? Map.ProjToPixel(snappedCoord) : e.Location;
            DoMouseMoveForSnapDrawing(prevWasSnapped, _mousePosition);
            // End snapping changes

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse-Up situation.
        /// </summary>
        /// <param name="e">The GeoMouseArcs class describes the mouse condition along with geographic coordinates.</param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (_standBy) { return; }
            if (_featureSet == null || _featureSet.IsDisposed) { return; }

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                // Add the current point to the featureset
                if (_featureSet.FeatureType == FeatureType.Point)
                {
                    // Begin snapping changes
                    Coordinate snappedCoord = _coordinateDialog.Coordinate;
                    ComputeSnappedLocation(e, ref snappedCoord);
                    // End snapping changes

                    Topology.Point pt = new Topology.Point(snappedCoord); // Snapping changes
                    Feature f = new Feature(pt);
                    _featureSet.Features.Add(f);
                    _featureSet.ShapeIndices = null; // Reset shape indices
                    _featureSet.UpdateExtent();
                    _layer.AssignFastDrawnStates();
                    _featureSet.InvalidateVertices();
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    _context.Show((Control)Map, e.Location);
                }
                else
                {
                    if (_coordinates == null) { _coordinates = new List<Coordinate>(); }

                    // Begin snapping changes
                    Coordinate snappedCoord = e.GeographicLocation;
                    ComputeSnappedLocation(e, ref snappedCoord);
                    // End snapping changes

                    _coordinates.Add(snappedCoord); // Snapping changes
                    if (_coordinates.Count > 1)
                    {
                        Point p1 = Map.ProjToPixel(_coordinates[_coordinates.Count - 1]);
                        Point p2 = Map.ProjToPixel(_coordinates[_coordinates.Count - 2]);
                        Rectangle invalid = SymbologyGlobal.GetRectangle(p1, p2);
                        invalid.Inflate(20, 20);
                        Map.Invalidate(invalid);
                    }
                }
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Delete the shape currently being edited.
        /// </summary>
        /// <param name="sender">The sender of the DeleteShape event.</param>
        /// <param name="e">An empty EventArgument.</param>
        public void DeleteShape(object sender, EventArgs e)
        {
            _coordinates = new List<Coordinate>();
            _parts = new List<List<Coordinate>>();
            Map.Invalidate();
        }

        /// <summary>
        /// Finish the shape.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">An empty EventArgs class.</param>
        public void FinishShape(object sender, EventArgs e)
        {
            if (_featureSet != null && !_featureSet.IsDisposed)
            {
                Feature f = null;
                if (_featureSet.FeatureType == FeatureType.MultiPoint)
                {
                    f = new Feature(new MultiPoint(_coordinates));
                }
                if (_featureSet.FeatureType == FeatureType.Line || _featureSet.FeatureType == FeatureType.Polygon)
                {
                    FinishPart(sender, e);
                    Shape shp = new Shape(_featureSet.FeatureType);
                    foreach (List<Coordinate> part in _parts)
                    {
                        if (part.Count >= 2)
                        {
                            shp.AddPart(part, _featureSet.CoordinateType);
                        }
                    }
                    f = new Feature(shp);
                }
                if (f != null)
                {
                    _featureSet.Features.Add(f);
                }
                _featureSet.ShapeIndices = null; // Reset shape indices
                _featureSet.UpdateExtent();
                _layer.AssignFastDrawnStates();
                _featureSet.InvalidateVertices();
            }

            _coordinates = new List<Coordinate>();
            _parts = new List<List<Coordinate>>();
        }

        /// <summary>
        /// Finish the part of the shape being edited.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">An empty EventArgs class.</param>
        public void FinishPart(object sender, EventArgs e)
        {
            _parts.Add(_coordinates);
            _coordinates = new List<Coordinate>();
            Map.Invalidate();
        }

        /// <summary>
        /// Occurs when this function is removed.
        /// </summary>
        protected override void OnUnload()
        {
            if (Enabled)
            {
                _coordinates = null;
                _coordinateDialog.Hide();
            }
            if (_tempLayer != null)
            {
                Map.MapFrame.DrawingLayers.Remove(_tempLayer);
                Map.MapFrame.Invalidate();

                _tempLayer = null;
            }
            Map.Invalidate();
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether the "dispose" method has been called.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        public IFeatureLayer Layer
        {
            get { return _layer; }
            set
            {
                if (_layer == value) return;
                _layer = value;
                _featureSet = _layer != null ? _layer.DataSet : null;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Actually, this creates disposable items but doesn't own them.
        /// When the ribbon disposes it will remove the items.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // This exists to prevent FX Cop from complaining.
            GC.SuppressFinalize(this);
        }

        #endregion

        private void CoordinateDialogFormClosing(object sender, FormClosingEventArgs e)
        {
            // This signals that we are done with editing, and should therefore close up shop
            Enabled = false;
        }

        /// <summary>
        /// Finalizes an instance of the AddShapeFunction class.
        /// </summary>
        ~AddShapeFunction()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes this handler, removing any buttons that it is responsible for adding.
        /// </summary>
        /// <param name="disposeManagedResources">Disposes of the resources.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (!_disposed)
            {
                // One option would be to leave the non-working tools,
                // but if this gets disposed we should clean up after
                // ourselves and remove any added controls.
                if (disposeManagedResources)
                {
                    if (!_coordinateDialog.IsDisposed) { _coordinateDialog.Dispose(); }
                    if (_context != null) { _context.Dispose(); }
                    if (_finishPart != null) { _finishPart.Dispose(); }

                    _featureSet = null;
                    _coordinates = null;
                    _coordinateDialog = null;
                    _tempLayer = null;
                    _context = null;
                    _finishPart = null;
                    _parts = null;
                    _layer = null;
                }
                _disposed = true;
            }
        }
    }
}