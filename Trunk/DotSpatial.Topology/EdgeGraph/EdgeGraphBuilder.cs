using System.Collections.Generic;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.EdgeGraph
{
    /// <summary>
    /// Builds an edge graph from geometries containing edges.
    /// </summary>
    public class EdgeGraphBuilder
    {
        #region Fields

        private readonly EdgeGraph graph = new EdgeGraph();

        #endregion

        #region Constructors

        public EdgeGraphBuilder() { }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the edges of a Geometry to the graph. 
        /// May be called multiple times.
        /// Any dimension of Geometry may be added; the constituent edges are extracted.
        /// </summary>
        /// <param name="geometry">geometry to be added</param>
        public void Add(IGeometry geometry)
        {
            geometry.Apply(new GeometryComponentFilter(c =>
            {
                if (c is ILineString)
                    Add(c as ILineString);
            }));
        }

        /// <summary>
        ///  Adds the edges in a collection of <see cref="IGeometry"/>s to the graph. 
        /// May be called multiple times.
        /// Any dimension of <see cref="IGeometry"/> may be added.
        /// </summary>
        /// <param name="geometries">the geometries to be added</param>
        public void Add(IEnumerable<IGeometry> geometries)
        {
            foreach (IGeometry geometry in geometries)
                Add(geometry);
        }

        private void Add(ILineString lineString)
        {
            ICoordinateSequence seq = lineString.CoordinateSequence;
            for (int i = 1; i < seq.Count; i++)
            {
                Coordinate prev = seq.GetCoordinate(i - 1);
                Coordinate curr = seq.GetCoordinate(i);
                graph.AddEdge(prev, curr);
            }
        }

        public static EdgeGraph Build(IEnumerable<IGeometry> geoms)
        {
            EdgeGraphBuilder builder = new EdgeGraphBuilder();
            builder.Add(geoms);
            return builder.GetGraph();
        }

        public EdgeGraph GetGraph()
        {
            return graph;
        }

        #endregion
    }
}
