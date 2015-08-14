using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Algorithm
{
    ///<summary>
    /// An interface for classes which determine the <see cref="Location"/> of points in a <see cref="IGeometry"/>
    ///</summary>
    ///<author>Martin Davis</author>
    public interface IPointInAreaLocator
    {
        #region Methods

        ///<summary>
        /// Determines the  <see cref="Location"/> of a point in the <see cref="IGeometry"/>.
        /// </summary>
        /// <param name="p">The point to test</param>
        /// <returns>the location of the point in the geometry</returns>
        LocationType Locate(Coordinate p);

        #endregion
    }
}