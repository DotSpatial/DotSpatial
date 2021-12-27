using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// This contains methods for creating Coordinates of different types from the same input.
    /// </summary>
    public static class CoordinateExt
    {
        /// <summary>
        /// Creates an empty Coordinate. Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM.
        /// </summary>
        /// <param name="withZ">Indicates whether the coordinate should contain Z.</param>
        /// <param name="withM">Indicates whether the coordinate should contain M.</param>
        /// <returns>Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM.</returns>
        public static Coordinate CreateEmptyCoordinate(bool withZ, bool withM)
        {
            if (withZ && withM)
            {
                return new CoordinateZM();
            }
            else if (withM)
            {
                return new CoordinateM();
            }
            else if (withZ)
            {
                return new CoordinateZ();
            }
            else
            {
                return new Coordinate();
            }
        }

        /// <summary>
        /// Creates a Coordinate. Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM.
        /// </summary>
        /// <param name="x">The x value to use.</param>
        /// <param name="y">The y value to use.</param>
        /// <param name="withZ">Indicates whether the coordinate should contain Z.</param>
        /// <param name="z">The z value to use. This will be ignored if withZ is false.</param>
        /// <param name="withM">Indicates whether the coordinate should contain M.</param>
        /// <param name="m">The m value to use. This will be ignored if withM is false.</param>
        /// <returns>Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM.</returns>
        public static Coordinate CreateCoordinate(double x, double y, bool withZ, double z, bool withM, double m)
        {
            if (withZ && withM)
            {
                return new CoordinateZM(x, y, z, m);
            }
            else if (withM)
            {
                return new CoordinateM(x, y, m);
            }
            else if (withZ)
            {
                return new CoordinateZ(x, y, z);
            }
            else
            {
                return new Coordinate(x, y);
            }
        }

        /// <summary>
        /// Creates an empty point with the coordinate type resulting from the given input. Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM for the point.
        /// </summary>
        /// <param name="withZ">Indicates whether the coordinate should contain Z.</param>
        /// <param name="withM">Indicates whether the coordinate should contain M.</param>
        /// <returns>Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM for the point.</returns>
        public static Point CreateEmptyPoint(bool withZ, bool withM)
        {
            return new Point(CreateEmptyCoordinate(withZ, withM));
        }

        /// <summary>
        /// Creates a Point with the coordinate type resulting from the given input. Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM for the point.
        /// </summary>
        /// <param name="x">The x value to use.</param>
        /// <param name="y">The y value to use.</param>
        /// <param name="withZ">Indicates whether the coordinate should contain Z.</param>
        /// <param name="z">The z value to use. This will be ignored if withZ is false.</param>
        /// <param name="withM">Indicates whether the coordinate should contain M.</param>
        /// <param name="m">The m value to use. This will be ignored if withM is false.</param>
        /// <returns>Based on the given values for withZ/withM this will create a Coordinate/CoordinateZ/CoordinateM or CoordinateZM for the point.</returns>
        public static Point CreatePoint(double x, double y, bool withZ, double z, bool withM, double m)
        {
            return new Point(CreateCoordinate(x, y, withZ, z, withM, m));
        }
    }
}
