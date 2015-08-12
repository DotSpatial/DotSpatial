using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// A <c>CoordinateFilter</c> that creates an array containing every coordinate in a <c>Geometry</c>.
    /// </summary>
    public class CoordinateArrayFilter : ICoordinateFilter 
    {
        #region Fields

        readonly Coordinate[] _pts;
        int _n;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a <c>CoordinateArrayFilter</c>.
        /// </summary>
        /// <param name="size">The number of points that the <c>CoordinateArrayFilter</c> will collect.</param>
        public CoordinateArrayFilter(int size) 
        {
            _pts = new Coordinate[size];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the <c>Coordinate</c>s collected by this <c>CoordinateArrayFilter</c>.
        /// </summary>
        public Coordinate[] Coordinates
        {
            get
            {
                return _pts;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        public void Filter(Coordinate coord) 
        {
            _pts[_n++] = coord;
        }

        #endregion
    }
}
