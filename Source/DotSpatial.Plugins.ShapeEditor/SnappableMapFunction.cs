// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// This is an abtract class that provides functionality for snapping objects.
    /// </summary>
    public abstract class SnappableMapFunction : MapFunctionZoom
    {
        #region Constants

        /// <summary>
        /// Indicates the vertex's snapping type
        /// </summary>
        protected const string SnappingTypeVertex = "v";

        /// <summary>
        /// Indicates the edge's snapping type
        /// </summary>
        protected const string SnappingTypeEdge = "e";
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnappableMapFunction"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        protected SnappableMapFunction(IMap map)
            : base(map)
        {
            DoSnapping = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether snapping is performed or not.
        /// </summary>
        public bool DoSnapping { get; set; }

        /// <summary>
        /// Gets the feature the computed snappedCoord belongs to.
        /// </summary>
        public IFeature SnappedFeature { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current mouse position has been snapped.
        /// </summary>
        protected bool IsSnapped { get; set; } = false;

        /// <summary>
        /// Gets or sets a list of layers that will be snapped to.
        /// </summary>
        protected List<IFeatureLayer> SnapLayers { get; set; }

        /// <summary>
        /// Gets or sets the pen that will be used to draw the snapping circle.
        /// </summary>
        protected Pen SnapPen { get; set; } = new Pen(Color.HotPink, 2F);

        /// <summary>
        /// Gets or sets the snap tolerance. +/- N pixels around the mouse point.
        /// </summary>
        protected int SnapTol { get; set; } = 3;

        /// <summary>
        /// Gets or sets a value indicating whether the current snapped coordinate is a vertex or an edge.
        /// </summary>
        protected string SnappingType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the snapped coordinate.
        /// </summary>
        protected Coordinate SnappedCoordKeeped { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the snapped coordinate Index.
        /// </summary>
        protected int SnappedCoordIndex { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Add the given layer to the snap list. This list determines which layers the current layer will be snapped to.
        /// </summary>
        /// <param name="layer">The layer that gets added to the list of layers that can be used for snapping.</param>
        public void AddLayerToSnap(IFeatureLayer layer)
        {
            if (SnapLayers == null)
                InitializeSnapLayers();
            if (!SnapLayers.Contains(layer)) SnapLayers.Add(layer);
        }

        /// <summary>
        /// Remove the given layer from the snap list. This list determines which layers the current layer will be snapped to.
        /// </summary>
        /// <param name="layer">The layer that gets removed from the list of layers that can be used for snapping.</param>
        public void RemoveLayerFromSnap(IFeatureLayer layer)
        {
            if (SnapLayers == null)
            {
                InitializeSnapLayers();
                return;
            }

            if (SnapLayers.Contains(layer)) SnapLayers.Remove(layer);
        }

        /// <summary>
        /// Computes a snapped coordinate.  If the mouse is near a snappable object, the output
        /// location of the mouse will be the coordinates of the object rather than the actual
        /// mouse coords.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <param name="snappedCoord">set if a coordinate is found</param>
        /// <returns>true if snap found</returns>
        protected bool ComputeSnappedLocation(GeoMouseArgs e, ref Coordinate snappedCoord)
        {
            SnappingType = string.Empty;
            SnappedCoordIndex = 0;

            if (SnapLayers == null || e == null || Map == null) return false;

            if (!DoSnapping) return false;

            Rectangle mouseRect = new Rectangle(e.X - SnapTol, e.Y - SnapTol, SnapTol * 2, SnapTol * 2);

            Extent pix = Map.PixelToProj(mouseRect);
            if (pix == null)
                return false;

            Envelope mouseEnv = pix.ToEnvelope();

            NetTopologySuite.Geometries.Point mouse_onEarth = new NetTopologySuite.Geometries.Point(snappedCoord);

            foreach (IFeatureLayer layer in SnapLayers.Where(_ => _.Snappable && _.IsVisible))
            {
                foreach (IFeature feat in layer.DataSet.Features)
                {
                    IGeometry featGeom = feat.Geometry;

                    // If the feature is partially or totaly visible in the view.
                    if (Map.ViewExtents.Intersects(featGeom.EnvelopeInternal))
                    {
                        bool doCoord_Snap;

                        // System.Console.WriteLine(feat.Fid);
                        int coordCounter = 0;
                        foreach (Coordinate c in feat.Geometry.Coordinates)
                        {
                            doCoord_Snap = true;

                            if (layer.SnapVertices)
                            {
                                if (coordCounter == 0 && !layer.SnapStartPoint) doCoord_Snap = false;
                                if (coordCounter == (feat.Geometry.Coordinates.Length - 1) && !layer.SnapEndPoint) doCoord_Snap = false;

                                if (doCoord_Snap)
                                {
                                    // If the mouse envelope contains the current coordinate, we found a snap location.
                                    if (mouseEnv.Contains(c))
                                    {
                                        snappedCoord = c;
                                        SnappedCoordIndex = coordCounter;
                                        SnappedFeature = feat;
                                        SnappingType = SnappingTypeVertex;
                                        return true;
                                    }
                                }
                            }

                            if (coordCounter > 0 && layer.SnapEdges && feat.FeatureType != FeatureType.Point && feat.FeatureType != FeatureType.MultiPoint)
                            {
                                double edge_Distance = 0;
                                if (layer.DataSet.CoordinateType.Equals(CoordinateType.Z))
                                {
                                    edge_Distance = feat.Geometry.Coordinates[coordCounter - 1].Distance3D(c);
                                }
                                else
                                {
                                    edge_Distance = feat.Geometry.Coordinates[coordCounter - 1].Distance(c);
                                }

                                if (edge_Distance > 0)
                                {
                                    List<Coordinate> edgeCoords = new List<Coordinate>();
                                    edgeCoords.Add(feat.Geometry.Coordinates[coordCounter - 1]);
                                    edgeCoords.Add(c);

                                    LineString edge = new LineString(edgeCoords.ToArray());

                                    if (mouse_onEarth.Distance(edge) <= (mouseEnv.Width / 2))
                                    {
                                        NetTopologySuite.LinearReferencing.LengthIndexedLine indexedEedge = new NetTopologySuite.LinearReferencing.LengthIndexedLine(edge);
                                        double proj_Index = indexedEedge.Project(mouse_onEarth.Coordinate);

                                        if (proj_Index > (mouseEnv.Width / 2) && proj_Index < (edge.Length - (mouseEnv.Width / 2)))
                                        {
                                            snappedCoord = indexedEedge.ExtractPoint(proj_Index);
                                            SnappedCoordKeeped = snappedCoord;
                                            SnappedCoordIndex = coordCounter;
                                            SnappedFeature = feat;
                                            SnappingType = SnappingTypeEdge;
                                            return true;
                                        }
                                    }
                                }
                            }

                            coordCounter++;
                        }
                    }
                }
            }

            SnappedFeature = null;
            return false;
        }

        /// <summary>
        /// Computes a snapped coordinate.  If the mouse is near the selected feature, the output
        /// location of the mouse will be the coordinates on the feature rather than the actual
        /// mouse coords.
        /// </summary>
        /// <param name="mouseRect">The event args.</param>
        /// <param name="feat">set if a coordinate is found</param>
        /// <param name="layer">the feature's layer</param>
        /// <param name="snappedCoord">the coordinate of the mouse</param>
        /// <returns>true if snap found</returns>
        protected bool ComputeSnappedLocation_ForSelectedFeature(Rectangle mouseRect, ref IFeature feat, IFeatureLayer layer, ref Coordinate snappedCoord)
        {
            SnappingType = string.Empty;
            SnappedCoordIndex = 0;

            if (!DoSnapping) return false;
            if (mouseRect == null || feat == null || layer == null || snappedCoord == null) return false;

            Extent pix = Map.PixelToProj(mouseRect);
            if (pix == null)
                return false;

            Envelope mouseEnv = pix.ToEnvelope();

            NetTopologySuite.Geometries.Point mouse_onEarth = new NetTopologySuite.Geometries.Point(snappedCoord);
            IGeometry featGeom = feat.Geometry;

            // If the feature is partially or totaly visible in the view.
            if (Map.ViewExtents.Intersects(featGeom.EnvelopeInternal))
            {
                bool doCoord_Snap;

                // System.Console.WriteLine(feat.Fid);
                int coordCounter = 0;
                foreach (Coordinate c in feat.Geometry.Coordinates)
                {
                    doCoord_Snap = true;

                    if (layer.SnapVertices)
                    {
                        if (coordCounter == 0 && !layer.SnapStartPoint) doCoord_Snap = false;
                        if (coordCounter == (feat.Geometry.Coordinates.Length - 1) && !layer.SnapEndPoint) doCoord_Snap = false;

                        if (doCoord_Snap)
                        {
                            // If the mouse envelope contains the current coordinate, we found a snap location.
                            if (mouseEnv.Contains(c))
                            {
                                snappedCoord = c;
                                SnappedCoordIndex = coordCounter;
                                SnappedFeature = feat;
                                SnappingType = SnappingTypeVertex;
                                return true;
                            }
                        }
                    }

                    if (coordCounter > 0 && layer.SnapEdges && feat.FeatureType != FeatureType.Point && feat.FeatureType != FeatureType.MultiPoint)
                    {
                        double edge_Distance = 0;
                        if (layer.DataSet.CoordinateType.Equals(CoordinateType.Z))
                        {
                            edge_Distance = feat.Geometry.Coordinates[coordCounter - 1].Distance3D(c);
                        }
                        else
                        {
                            edge_Distance = feat.Geometry.Coordinates[coordCounter - 1].Distance(c);
                        }

                        if (edge_Distance > 0)
                        {
                            List<Coordinate> edgeCoords = new List<Coordinate>();
                            edgeCoords.Add(feat.Geometry.Coordinates[coordCounter - 1]);
                            edgeCoords.Add(c);

                            LineString edge = new LineString(edgeCoords.ToArray());

                            if (mouse_onEarth.Distance(edge) <= (mouseEnv.Width / 2))
                            {
                                NetTopologySuite.LinearReferencing.LengthIndexedLine indexedEedge = new NetTopologySuite.LinearReferencing.LengthIndexedLine(edge);
                                double proj_Index = indexedEedge.Project(mouse_onEarth.Coordinate);

                                if (proj_Index > (mouseEnv.Width / 2) && proj_Index < (edge.Length - (mouseEnv.Width / 2)))
                                {
                                    snappedCoord = indexedEedge.ExtractPoint(proj_Index);
                                    SnappedCoordKeeped = snappedCoord; /* c.Clone() as Coordinate;*/
                                    SnappedCoordIndex = coordCounter;
                                    SnappedFeature = feat;
                                    SnappingType = SnappingTypeEdge;
                                    return true;
                                }
                            }
                        }
                    }

                    coordCounter++;
                }
            }

            SnappedFeature = null;
            return false;
        }

        /// <summary>
        /// Check if the snapped coordinate intersect the _selectedFeature, as snapped feautre
        /// can be different from the selected one.
        /// </summary>
        /// <param name="selectedFeature">The event args.</param>
        /// <param name="mousePosition">set if a mouse is pointing the selected feature</param>
        protected void CheckSnappedCoord_On_SelectedFeature(ref IFeature selectedFeature, System.Drawing.Point mousePosition)
        {
            if (selectedFeature == null || mousePosition == null) return;

            // Console.WriteLine("Testing  Snapped sur Selected");
            Rectangle rect = new Rectangle(mousePosition.X - SnapTol, mousePosition.Y - SnapTol, 2 * SnapTol, 2 * SnapTol);
            Extent ext = Map.PixelToProj(rect);
            Envelope env = ext.ToEnvelope();

            if (selectedFeature.Intersects(env))
            {
                // Console.WriteLine("Snapped sur Selected");
                SnappedFeature = selectedFeature;
            }
        }

        /// <summary>
        /// Perform any actions in the OnMouseMove event that are necessary for snap drawing.
        /// </summary>
        /// <param name="prevWasSnapped">Indicates whether the mouse was snapped to a point before.</param>
        /// <param name="pos">Current position.</param>
        protected void DoMouseMoveForSnapDrawing(bool prevWasSnapped, System.Drawing.Point pos)
        {
            // Invalidate the region around the mouse so that the previous snap colors are erased.
            if ((prevWasSnapped || IsSnapped) && DoSnapping)
            {
                Rectangle invalid = new Rectangle(pos.X - 30, pos.Y - 30, 60, 60);
                Map.Invalidate(invalid);
            }
        }

        /// <summary>
        /// Perform any drawing necessary for snapping (e.g. draw a circle around snapped location).
        /// </summary>
        /// <param name="graphics">graphics to draw on</param>
        /// <param name="pos">point where the circles center will be</param>
        protected void DoSnapDrawing(Graphics graphics, System.Drawing.Point pos)
        {
            if (IsSnapped)
            {
                switch (SnappingType)
                {
                    case SnappingTypeVertex:
                        graphics.DrawEllipse(SnapPen, pos.X - 5, pos.Y - 5, 10, 10);
                        break;
                    case SnappingTypeEdge:
                        graphics.DrawEllipse(SnapPen, pos.X - 2, pos.Y - 2, 4, 4);
                        break;
                }
            }
        }

        /// <summary>
        /// Initialize/Reinitialize the list of snap layers (i.e. when a layer has
        /// been selected or reselected).
        /// </summary>
        protected void InitializeSnapLayers()
        {
            SnapLayers = new List<IFeatureLayer>();
        }

        #endregion
    }
}
