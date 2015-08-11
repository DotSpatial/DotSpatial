using System;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Algorithm.Locate
{
    ///<summary>
    /// An interface for classes which determine the <see cref="Location"/> of
    /// points in areal geometries.
    /// </summary>
    /// <author>Martin Davis</author>
    public interface IPointOnGeometryLocator
    {
        #region Methods

        ///<summary>
        /// Determines the <see cref="Location"/> of a point in an areal <see cref="IGeometry"/>.
        ///</summary>
        ///<param name="p">The point to test</param>
        ///<returns>The location of the point in the geometry</returns>
        LocationType Locate(Coordinate p);

        #endregion
    }

    /// <summary>
    /// Static methods for <see cref="IPointOnGeometryLocator"/> classes
    /// </summary>
    public static class PointOnGeometryLocatorExtensions
    {
        #region Methods

        /// <summary> 
        /// Convenience method to test a point for intersection with a geometry
        /// <para/>
        /// The geometry is wrapped in a <see cref="IPointOnGeometryLocator"/> class.
        /// </summary>
        /// <param name="locator">The locator to use.</param>
        /// <param name="coordinate">The coordinate to test.</param>
        /// <returns><c>true</c> if the point is in the interior or boundary of the geometry.</returns>
        public static bool Intersects(IPointOnGeometryLocator locator, Coordinate coordinate)
        {
            if (locator == null)
                throw new ArgumentNullException("locator");
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");

            switch (locator.Locate(coordinate))
            {
                case LocationType.Boundary:
                case LocationType.Interior:
                    return true;

                case LocationType.Exterior:
                    return false;

                default:
                    throw new InvalidOperationException("IPointOnGeometryLocator.Locate should never return anything other than Boundary, Interior, or Exterior.");
            }
        }

        #endregion
    }
}