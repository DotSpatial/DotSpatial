using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Operation.Overlay.Snap;

namespace DotSpatial.Topology.Operation.Overlay.Validate
{
    ///<summary>
    /// Validates that the result of an overlay operation is geometrically correct within a determined tolerance.
    /// Uses fuzzy point location to find points which are
    /// definitely in either the interior or exterior of the result
    /// geometry, and compares these results with the expected ones.
    ///</summary>
    /// <remarks>
    /// This algorithm is only useful where the inputs are polygonal.
    /// This is a heuristic test, and may return false positive results
    /// (I.e. it may fail to detect an invalid result.)
    /// It should never return a false negative result, however
    /// (I.e. it should never report a valid result as invalid.)
    /// </remarks>
    /// <author>Martin Davis</author>
    /// <see cref="OverlayOp"/>
    public class OverlayResultValidator
    {
        #region Constant Fields

        private const double Tolerance = 0.000001;

        #endregion

        #region Fields

        private readonly double _boundaryDistanceTolerance = Tolerance;
        private readonly IGeometry[] _geom;
        private readonly LocationType[] _location = new LocationType[3];
        private readonly FuzzyPointLocator[] _locFinder;
        private readonly List<Coordinate> _testCoords = new List<Coordinate>();
        private Coordinate _invalidLocation;

        #endregion

        #region Constructors

        public OverlayResultValidator(IGeometry a, IGeometry b, IGeometry result)
        {
            /*
             * The tolerance to use needs to depend on the size of the geometries.
             * It should not be more precise than double-precision can support. 
             */
            _boundaryDistanceTolerance = ComputeBoundaryDistanceTolerance(a, b);
            _geom = new[] {a, b, result};
            _locFinder = new[]
                             {
                                 new FuzzyPointLocator(_geom[0], _boundaryDistanceTolerance),
                                 new FuzzyPointLocator(_geom[1], _boundaryDistanceTolerance),
                                 new FuzzyPointLocator(_geom[2], _boundaryDistanceTolerance)
                             };
        }

        #endregion

        #region Properties

        public Coordinate InvalidLocation
        {
            get { return _invalidLocation; }
        }

        #endregion

        #region Methods

        private void AddTestPts(IGeometry g)
        {
            OffsetPointGenerator ptGen = new OffsetPointGenerator(g);
            _testCoords.AddRange(ptGen.GetPoints(5 * _boundaryDistanceTolerance));
        }

        private bool CheckValid(SpatialFunction overlayOp)
        {
            for (int i = 0; i < _testCoords.Count; i++)
            {
                Coordinate pt = _testCoords[i];
                if (!CheckValid(overlayOp, pt))
                {
                    _invalidLocation = pt;
                    return false;
                }
            }
            return true;
        }

        private bool CheckValid(SpatialFunction overlayOp, Coordinate pt)
        {
            _location[0] = _locFinder[0].GetLocation(pt);
            _location[1] = _locFinder[1].GetLocation(pt);
            _location[2] = _locFinder[2].GetLocation(pt);

            /*
             * If any location is on the Boundary, can't deduce anything, so just return true
             */
            if (HasLocation(_location, LocationType.Boundary))
                return true;

            return IsValidResult(overlayOp, _location);
        }

        private static double ComputeBoundaryDistanceTolerance(IGeometry g0, IGeometry g1)
        {
            return Math.Min(GeometrySnapper.ComputeSizeBasedSnapTolerance(g0),
                    GeometrySnapper.ComputeSizeBasedSnapTolerance(g1));
        }

        private static bool HasLocation(LocationType[] location, LocationType loc)
        {
            for (int i = 0; i < 3; i++)
            {
                if (location[i] == loc)
                    return true;
            }
            return false;
        }

        public static bool IsValid(IGeometry a, IGeometry b, SpatialFunction overlayOp, IGeometry result)
        {
            OverlayResultValidator validator = new OverlayResultValidator(a, b, result);
            return validator.IsValid(overlayOp);
        }

        public bool IsValid(SpatialFunction overlayOp)
        {
            AddTestPts(_geom[0]);
            AddTestPts(_geom[1]);
            bool isValid = CheckValid(overlayOp);

            /*
            System.out.println("OverlayResultValidator: " + isValid);
            System.out.println("G0");
            System.out.println(geom[0]);
            System.out.println("G1");
            System.out.println(geom[1]);
            System.out.println("Result");
            System.out.println(geom[2]);
            */

            return isValid;
        }

        private static bool IsValidResult(SpatialFunction overlayOp, LocationType[] location)
        {
            bool expectedInterior = OverlayOp.IsResultOfOp(location[0], location[1], overlayOp);

            bool resultInInterior = (location[2] == LocationType.Interior);
            // MD use simpler: boolean isValid = (expectedInterior == resultInInterior);
            bool isValid = !(expectedInterior ^ resultInInterior);

            if (!isValid) ReportResult(overlayOp, location, expectedInterior);

            return isValid;
        }

        private static void ReportResult(SpatialFunction overlayOp, LocationType[] location, bool expectedInterior)
        {
#if !PCL
// ReSharper disable RedundantStringFormatCall
            // String.Format needed to build 2.0 release!
            Debug.WriteLine(String.Format("{0}:" + " A:{1} B:{2} expected:{3} actual:{4}", 
                overlayOp,
                Location.ToLocationSymbol(location[0]),
                Location.ToLocationSymbol(location[1]), expectedInterior ? 'i' : 'e',
                Location.ToLocationSymbol(location[2])));
// ReSharper restore RedundantStringFormatCall
#endif
        }

        #endregion
    }
}
