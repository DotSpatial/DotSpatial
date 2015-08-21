using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Overlay.Validate
{
    ///<summary>
    /// Finds the most likely <see cref="Location"/> of a point relative to
    /// the polygonal components of a geometry, using a tolerance value.
    ///</summary> 
    ///<remarks>
    /// If a point is not clearly in the Interior or Exterior,
    /// it is considered to be on the Boundary.
    /// In other words, if the point is within the tolerance of the Boundary,
    /// it is considered to be on the Boundary; otherwise,
    /// whether it is Interior or Exterior is determined directly.
    ///</remarks>
    /// <author>Martin Davis</author>
    public class FuzzyPointLocator
    {
        #region Fields

        private readonly double _boundaryDistanceTolerance;
        private readonly IGeometry _g;
        private readonly IMultiLineString _linework;
        private readonly PointLocator _ptLocator = new PointLocator();
        private readonly LineSegment _seg = new LineSegment();

        #endregion

        #region Constructors

        public FuzzyPointLocator(IGeometry g, double boundaryDistanceTolerance)
        {
            _g = g;
            _boundaryDistanceTolerance = boundaryDistanceTolerance;
            _linework = ExtractLinework(g);
        }

        #endregion

        #region Methods

        ///<summary>
        /// Extracts linework for polygonal components.
        ///</summary>
        ///<param name="g">The geometry from which to extract</param>
        ///<returns>A lineal geometry containing the extracted linework</returns>
        private static IMultiLineString ExtractLinework(IGeometry g)
        {
            PolygonalLineworkExtracter extracter = new PolygonalLineworkExtracter();
            g.Apply(extracter);
            List<ILineString> linework = extracter.Linework;
            return g.Factory.CreateMultiLineString(linework.ToArray());
        }

        public Location GetLocation(Coordinate pt)
        {
            if (IsWithinToleranceOfBoundary(pt))
                return Location.Boundary;
            /*
            double dist = linework.distance(point);

            // if point is close to boundary, it is considered to be on the boundary
            if (dist < tolerance)
              return Location.BOUNDARY;
             */

            // now we know point must be clearly inside or outside geometry, so return actual location value
            return _ptLocator.Locate(pt, _g);
        }

        private bool IsWithinToleranceOfBoundary(Coordinate pt)
        {
            for (int i = 0; i < _linework.NumGeometries; i++)
            {
                ILineString line = (ILineString)_linework.GetGeometryN(i);
                ICoordinateSequence seq = line.CoordinateSequence;
                for (int j = 0; j < seq.Count - 1; j++)
                {
                    seq.GetCoordinate(j, _seg.P0);
                    seq.GetCoordinate(j + 1, _seg.P1);
                    double dist = _seg.Distance(pt);
                    if (dist <= _boundaryDistanceTolerance)
                        return true;
                }
            }
            return false;
        }

        #endregion
    }

    ///<summary>
    /// Extracts the LineStrings in the boundaries of all the polygonal elements in the target <see cref="IGeometry"/>.
    ///</summary>
    ///<author>Martin Davis</author>
    class PolygonalLineworkExtracter : IGeometryFilter
    {
        #region Fields

        private readonly List<ILineString> _linework;

        #endregion

        #region Constructors

        public PolygonalLineworkExtracter()
        {
            _linework = new List<ILineString>();
        }

        #endregion

        #region Properties

        ///<summary>
        /// Gets the list of polygonal linework.
        ///</summary>
        public List<ILineString> Linework
        {
            get { return _linework; }
        }

        #endregion

        #region Methods

        ///<summary>
        /// Filters out all linework for polygonal elements
        /// </summary>
        public void Filter(IGeometry g)
        {
            if (g is IPolygon)
            {
                IPolygon poly = (IPolygon)g;
                _linework.Add(poly.ExteriorRing);
                for (int i = 0; i < poly.NumInteriorRings; i++)
                {
                    _linework.Add(poly.InteriorRings[i]);
                }
            }
        }

        #endregion
    }
}