namespace DotSpatial.Topology.Triangulate.QuadEdge
{
    /// <summary>
    /// Locates <see cref="QuadEdge"/>s in a <see cref="QuadEdgeSubdivision"/>,
    /// optimizing the search by starting in the
    /// locality of the last edge found.
    /// </summary>
    /// <author>Martin Davis</author>
    public class LastFoundQuadEdgeLocator : IQuadEdgeLocator
    {
        #region Fields

        private readonly QuadEdgeSubdivision _subdiv;
        private QuadEdge _lastEdge;

        #endregion

        #region Constructors

        public LastFoundQuadEdgeLocator(QuadEdgeSubdivision subdiv)
        {
            _subdiv = subdiv;
            Init();
        }

        #endregion

        #region Methods

        private QuadEdge FindEdge()
        {
            var edges = _subdiv.GetEdges();
            // assume there is an edge - otherwise will get an exception
            var enumerator = edges.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            else
            {
                throw new System.IndexOutOfRangeException();
            }
        }

        private void Init()
        {
            _lastEdge = FindEdge();
        }

        /// <summary>
        /// Locates an edge e, such that either v is on e, or e is an edge of a triangle containing v.
        /// The search starts from the last located edge amd proceeds on the general direction of v.
        /// </summary>
        public QuadEdge Locate(Vertex v)
        {
            if (! _lastEdge.IsLive)
            {
                Init();
            }

            QuadEdge e = _subdiv.LocateFromEdge(v, _lastEdge);
            _lastEdge = e;
            return e;
        }

        #endregion
    }
}