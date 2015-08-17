using DotSpatial.Topology.EdgeGraph;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Dissolve
{
    /// <summary>
    /// A HalfEdge which carries information
    /// required to support <see cref="LineDissolver"/>.
    /// </summary>
    public class DissolveHalfEdge : MarkHalfEdge
    {
        #region Fields

        private bool _isStart;

        #endregion

        #region Constructors

        public DissolveHalfEdge(Coordinate orig)
            : base(orig) { }

        #endregion

        #region Properties

        /// <summary>
        /// Tests whether this edge is the starting segment
        /// in a LineString being dissolved.
        /// </summary>
        /// <returns><c>true</c> if this edge is a start segment</returns>        
        public bool IsStart
        {
            get { return _isStart; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets this edge to be the start segment of an input LineString.
        /// </summary>
        public void SetStart()
        {
            _isStart = true;
        }

        #endregion
    }
}