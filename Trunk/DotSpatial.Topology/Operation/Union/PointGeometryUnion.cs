using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Utilities;
using Wintellect.PowerCollections;

namespace DotSpatial.Topology.Operation.Union
{
    public class PointGeometryUnion
    {
        #region Fields

        private readonly IGeometryFactory _geomFact;
        private readonly IGeometry _otherGeom;
        private readonly IGeometry _pointGeom;

        #endregion

        #region Constructors

        public PointGeometryUnion(IPuntal pointGeom, IGeometry otherGeom)
        {
            _pointGeom = (IGeometry)pointGeom;
            _otherGeom = otherGeom;
            _geomFact = otherGeom.Factory;
        }

        #endregion

        #region Methods

        ///<summary>
        /// Computes the union of a <see cref="IPoint"/> geometry with 
        /// another arbitrary <see cref="IGeometry"/>.
        /// Does not copy any component geometries.
        ///</summary>
        ///<param name="pointGeom"></param>
        ///<param name="otherGeom"></param>
        ///<returns></returns>
        public static IGeometry Union(IPuntal pointGeom, IGeometry otherGeom)
        {
            var unioner = new PointGeometryUnion(pointGeom, otherGeom);
            return unioner.Union();
        }

        public IGeometry Union()
        {
            PointLocator locater = new PointLocator();
            // use a set to eliminate duplicates, as required for union
            var exteriorCoords = new OrderedSet<Coordinate>();

            foreach (IPoint point in PointExtracter.GetPoints(_pointGeom))
            {
                Coordinate coord = point.Coordinate;
                Location loc = locater.Locate(coord, _otherGeom);

                if (loc == Location.Exterior)
                {
                    exteriorCoords.Add(coord);
                }
            }

            // if no points are in exterior, return the other geom
            if (exteriorCoords.Count == 0)
            {
                return _otherGeom;
            }

            // make a puntal geometry of appropriate size
            ICoordinateSequence coords = _geomFact.CoordinateSequenceFactory.Create(exteriorCoords.ToArray());
            IGeometry ptComp = coords.Count == 1 ? (IGeometry)_geomFact.CreatePoint(coords.GetCoordinate(0)) : _geomFact.CreateMultiPoint(coords);

            // add point component to the other geometry
            return GeometryCombiner.Combine(ptComp, _otherGeom);
        }

        #endregion
    }
}
