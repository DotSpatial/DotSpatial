// *******************************************************************************************************
// Product: DotSpatial.Analysis.Voroni.cs
// Description: Creates a delaunay tesselation where each point is effectively converted into triangles.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  8/26/2009         |  Initially written.  
//--------------------|--------------------|--------------------------------------------------------------
// Ted Dunsford       |  6/30/2010         |  Moved to DotSpatial.  
//--------------------|--------------------|--------------------------------------------------------------
// Dan Ames           |  3/2013            |  Updated and standarded licence and header info.  
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Topology;
using DotSpatial.Topology.Voronoi;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// This class provides an application programming interface to access the Voronoi calculations that are wrapped by a tool.
    /// </summary>
    public static class Voronoi
    {
        /// <summary>
        /// The Voronoi Graph calculation creates a delaunay tesselation where
        /// each point is effectively converted into triangles.
        /// </summary>
        /// <param name="points">The points to use for creating the tesselation.</param>
        /// <returns>The generated line featureset.</returns>
        public static IFeatureSet DelaunayLines(IFeatureSet points)
        {
            double[] vertices = points.Vertex;
            VoronoiGraph gp = Fortune.ComputeVoronoiGraph(vertices);
            FeatureSet result = new FeatureSet();
            foreach (VoronoiEdge edge in gp.Edges)
            {
                Coordinate c1 = edge.RightData.ToCoordinate();
                Coordinate c2 = edge.LeftData.ToCoordinate();
                LineString ls = new LineString(new List<Coordinate> { c1, c2 });
                Feature f = new Feature(ls);
                result.AddFeature(f);
            }
            return result;
        }

        /// <summary>
        /// The Voronoi Graph calculation creates the lines that form a voronoi diagram.
        /// </summary>
        /// <param name="points">The points to use for creating the tesselation.</param>
        /// <returns>An IFeatureSet that is the resulting set of lines in the diagram.</returns>
        public static IFeatureSet VoronoiLines(IFeatureSet points)
        {
            double[] vertices = points.Vertex;
            VoronoiGraph gp = Fortune.ComputeVoronoiGraph(vertices);

            HandleBoundaries(gp, points.Extent.ToEnvelope());

            FeatureSet result = new FeatureSet();
            foreach (VoronoiEdge edge in gp.Edges)
            {
                Coordinate c1 = edge.VVertexA.ToCoordinate();
                Coordinate c2 = edge.VVertexB.ToCoordinate();
                LineString ls = new LineString(new List<Coordinate> { c1, c2 });
                Feature f = new Feature(ls);
                result.AddFeature(f);
            }
            return result;
        }

        /// <summary>
        /// The Voronoi Graph calculation creates the lines that form a voronoi diagram.
        /// </summary>
        /// <param name="points">The points to use for creating the tesselation.</param>
        /// <param name="cropToExtent">The normal polygons have sharp angles that extend like stars.
        /// Cropping will ensure that the original featureset extent plus a small extra buffer amount
        /// is the outer extent of the polygons.  Errors seem to occur if the exact extent is used.</param>
        /// <returns>The IFeatureSet containing the lines that were formed in the diagram.</returns>
        public static IFeatureSet VoronoiPolygons(IFeatureSet points, bool cropToExtent)
        {
            IFeatureSet fs = new FeatureSet();
            VoronoiPolygons(points, fs, cropToExtent);
            return fs;
        }

        /// <summary>
        /// The Voronoi Graph calculation creates the lines that form a voronoi diagram.
        /// </summary>
        /// <param name="points">The points to use for creating the tesselation.</param>
        /// <param name="result">The output featureset.</param>
        /// <param name="cropToExtent">The normal polygons have sharp angles that extend like stars.
        /// Cropping will ensure that the original featureset extent plus a small extra buffer amount
        /// is the outer extent of the polygons.  Errors seem to occur if the exact extent is used.</param>
        public static void VoronoiPolygons(IFeatureSet points, IFeatureSet result, bool cropToExtent)
        {
            double[] vertices = points.Vertex;
            VoronoiGraph gp = Fortune.ComputeVoronoiGraph(vertices);

            Extent ext = points.Extent;
            ext.ExpandBy(ext.Width / 100, ext.Height / 100);
            IEnvelope env = ext.ToEnvelope();
            IPolygon bounds = env.ToPolygon();

            // Convert undefined coordinates to a defined coordinate.
            HandleBoundaries(gp, env);
            for (int i = 0; i < vertices.Length / 2; i++)
            {
                List<VoronoiEdge> myEdges = new List<VoronoiEdge>();
                Vector2 v = new Vector2(vertices, i * 2);
                foreach (VoronoiEdge edge in gp.Edges)
                {
                    if (!v.Equals(edge.RightData) && !v.Equals(edge.LeftData))
                    {
                        continue;
                    }
                    myEdges.Add(edge);
                }
                List<Coordinate> coords = new List<Coordinate>();
                VoronoiEdge firstEdge = myEdges[0];
                coords.Add(firstEdge.VVertexA.ToCoordinate());
                coords.Add(firstEdge.VVertexB.ToCoordinate());
                Vector2 previous = firstEdge.VVertexB;
                myEdges.Remove(myEdges[0]);
                Vector2 start = firstEdge.VVertexA;
                while (myEdges.Count > 0)
                {
                    for (int j = 0; j < myEdges.Count; j++)
                    {
                        VoronoiEdge edge = myEdges[j];
                        if (edge.VVertexA.Equals(previous))
                        {
                            previous = edge.VVertexB;
                            Coordinate c = previous.ToCoordinate();
                            coords.Add(c);
                            myEdges.Remove(edge);
                            break;
                        }

                        // couldn't match by adding to the end, so try adding to the beginning
                        if (edge.VVertexB.Equals(start))
                        {
                            start = edge.VVertexA;
                            coords.Insert(0, start.ToCoordinate());
                            myEdges.Remove(edge);
                            break;
                        }

                        // I don't like the reverse situation, but it seems necessary.
                        if (edge.VVertexB.Equals(previous))
                        {
                            previous = edge.VVertexA;
                            Coordinate c = previous.ToCoordinate();
                            coords.Add(c);
                            myEdges.Remove(edge);
                            break;
                        }

                        if (edge.VVertexA.Equals(start))
                        {
                            start = edge.VVertexB;
                            coords.Insert(0, start.ToCoordinate());
                            myEdges.Remove(edge);
                            break;
                        }
                    }
                }
                for (int j = 0; j < coords.Count; j++)
                {
                    Coordinate cA = coords[j];

                    // Remove NAN values
                    if (double.IsNaN(cA.X) || double.IsNaN(cA.Y))
                    {
                        coords.Remove(cA);
                    }

                    // Remove duplicate coordinates
                    for (int k = j + 1; k < coords.Count; k++)
                    {
                        Coordinate cB = coords[k];
                        if (cA.Equals2D(cB))
                        {
                            coords.Remove(cB);
                        }
                    }
                }
                foreach (Coordinate coord in coords)
                {
                    if (double.IsNaN(coord.X) || double.IsNaN(coord.Y))
                    {
                        coords.Remove(coord);
                    }
                }
                if (coords.Count <= 2)
                {
                    continue;
                }

                Polygon pg = new Polygon(coords);

                if (cropToExtent)
                {
                    try
                    {
                        IGeometry g = pg.Intersection(bounds);
                        IPolygon p = g as IPolygon;
                        if (p != null)
                        {
                            Feature f = new Feature(p, result);
                            f.CopyAttributes(points.Features[i]);
                        }
                    }
                    catch (Exception)
                    {
                        Feature f = new Feature(pg, result);
                        f.CopyAttributes(points.Features[i]);
                    }
                }
                else
                {
                    Feature f = new Feature(pg, result);
                    f.CopyAttributes(points.Features[i]);
                }
            }
            return;
        }

        /// <summary>
        /// The original algorithm simply allows edges that have one defined point and
        /// another "NAN" point.  Simply excluding the not a number coordinates fails
        /// to preserve the known direction of the ray.  We only need to extend this
        /// long enough to encounter the bounding box, not infinity.
        /// </summary>
        /// <param name="graph">The VoronoiGraph with the edge list.</param>
        /// <param name="bounds">The polygon bounding the datapoints.</param>
        private static void HandleBoundaries(VoronoiGraph graph, IEnvelope bounds)
        {
            List<ILineString> boundSegments = new List<ILineString>();
            List<VoronoiEdge> unboundEdges = new List<VoronoiEdge>();

            // Identify bound edges for intersection testing
            foreach (VoronoiEdge edge in graph.Edges)
            {
                if (edge.VVertexA.ContainsNan() || edge.VVertexB.ContainsNan())
                {
                    unboundEdges.Add(edge);
                    continue;
                }

                boundSegments.Add(
                    new LineString(new List<Coordinate> { edge.VVertexA.ToCoordinate(), edge.VVertexB.ToCoordinate() }));
            }

            // calculate a length to extend a ray to look for intersections
            IEnvelope env = bounds;
            double h = env.Height;
            double w = env.Width;
            double len = Math.Sqrt((w * w) + (h * h));
            // len is now long enough to pass entirely through the dataset no matter where it starts

            foreach (VoronoiEdge edge in unboundEdges)
            {
                // the unbound line passes thorugh start
                Coordinate start = (edge.VVertexB.ContainsNan())
                                       ? edge.VVertexA.ToCoordinate()
                                       : edge.VVertexB.ToCoordinate();

                // the unbound line should have a direction normal to the line joining the left and right source points
                double dx = edge.LeftData.X - edge.RightData.X;
                double dy = edge.LeftData.Y - edge.RightData.Y;
                double l = Math.Sqrt((dx * dx) + (dy * dy));

                // the slope of the bisector between left and right
                double sx = -dy / l;
                double sy = dx / l;

                Coordinate center = bounds.Center();
                if ((start.X > center.X && start.Y > center.Y) || (start.X < center.X && start.Y < center.Y))
                {
                    sx = dy / l;
                    sy = -dx / l;
                }

                Coordinate end1 = new Coordinate(start.X + (len * sx), start.Y + (len * sy));
                Coordinate end2 = new Coordinate(start.X - (sx * len), start.Y - (sy * len));
                Coordinate end = (end1.Distance(center) < end2.Distance(center)) ? end2 : end1;
                if (bounds.Contains(end))
                {
                    end = new Coordinate(start.X - (sx * len), start.Y - (sy * len));
                }

                if (edge.VVertexA.ContainsNan())
                {
                    edge.VVertexA = new Vector2(end.ToArray());
                }
                else
                {
                    edge.VVertexB = new Vector2(end.ToArray());
                }
            }
        }
    }
}