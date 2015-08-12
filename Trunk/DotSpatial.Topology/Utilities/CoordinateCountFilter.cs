using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// A <c>CoordinateFilter</c> that counts the total number of coordinates
    /// in a <c>Geometry</c>.
    /// </summary>
    public class CoordinateCountFilter : ICoordinateFilter 
    {
        #region Fields

        private int _n;

        #endregion

        #region Properties

        /*
        /// <summary>
        /// 
        /// </summary>
        public CoordinateCountFilter() { }
        */

        /// <summary>
        /// Returns the result of the filtering.
        /// </summary>
        public int Count 
        {
            get
            {
                return _n;
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
            _n++;
        }

        #endregion
    }
}
