using DotSpatial.Topology.CoordinateSystems;
using DotSpatial.Topology.CoordinateSystems.Transformations;

namespace DotSpatial.Topology
{
    /// <summary>
    /// Interface for classes that provide access to coordinate system and tranformation facilities.
    /// </summary>
    public interface ICoordinateSystemServices
    {
        #region Methods

        /// <summary>
        /// Method to create a coordinate tranformation between two spatial reference systems, defined by their identifiers
        /// </summary>
        /// <remarks>This is a convenience function for <see cref="CreateTransformation(ICoordinateSystem,ICoordinateSystem)"/>.</remarks>
        /// <param name="sourceSrid">The identifier for the source spatial reference system.</param>
        /// <param name="targetSrid">The identifier for the target spatial reference system.</param>
        /// <returns>A coordinate transformation, <value>null</value> if no transformation could be created.</returns>
        ICoordinateTransformation CreateTransformation(int sourceSrid, int targetSrid);

        /// <summary>
        /// Method to create a coordinate tranformation between two spatial reference systems
        /// </summary>
        /// <param name="source">The source spatial reference system.</param>
        /// <param name="target">The target spatial reference system.</param>
        /// <returns>A coordinate transformation, <value>null</value> if no transformation could be created.</returns>
        ICoordinateTransformation CreateTransformation(ICoordinateSystem source, ICoordinateSystem target);

        /// <summary>
        /// Returns the coordinate system by <paramref name="srid"/> identifier
        /// </summary>
        /// <param name="srid">The initialization for the coordinate system</param>
        /// <returns>The coordinate system.</returns>
        ICoordinateSystem GetCoordinateSystem(int srid);

        /// <summary>
        /// Returns the coordinate system by <paramref name="authority"/> and <paramref name="code"/>.
        /// </summary>
        /// <param name="authority">The authority for the coordinate system</param>
        /// <param name="code">The code assigned to the coordinate system by <paramref name="authority"/>.</param>
        /// <returns>The coordinate system.</returns>
        ICoordinateSystem GetCoordinateSystem(string authority, long code);

        /// <summary>
        /// Method to get the identifier, by which this coordinate system can be accessed.
        /// </summary>
        /// <param name="authority">The authority name</param>
        /// <param name="authorityCode">The code assigned by <paramref name="authority"/></param>
        /// <returns>The identifier or <value>null</value></returns>
        int? GetSRID(string authority, long authorityCode);

        #endregion
    }
}