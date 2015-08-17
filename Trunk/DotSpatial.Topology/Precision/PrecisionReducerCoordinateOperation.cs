using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Utilities;

namespace DotSpatial.Topology.Precision
{
    public class PrecisionReducerCoordinateOperation : GeometryEditor.CoordinateOperation
    {
        #region Fields

        private readonly bool _removeCollapsed = true;
        private readonly IPrecisionModel _targetPrecModel;

        #endregion

        #region Constructors

        public PrecisionReducerCoordinateOperation(IPrecisionModel targetPrecModel, bool removeCollapsed)
        {
            _targetPrecModel = targetPrecModel;
            _removeCollapsed = removeCollapsed;
        }

        #endregion

        #region Methods

        public override IList<Coordinate> Edit(IList<Coordinate> coordinates, IGeometry geom)
        {
            if (coordinates.Count == 0)
                return null;

            var reducedCoords = new Coordinate[coordinates.Count];
            // copy coordinates and reduce
            for (int i = 0; i < coordinates.Count; i++)
            {
                var coord = new Coordinate(coordinates[i]);
                _targetPrecModel.MakePrecise(coord);
                reducedCoords[i] = coord;
            }
            // remove repeated points, to simplify returned geometry as much as possible
            var noRepeatedCoordList = new CoordinateList(reducedCoords,
                    false);
            var noRepeatedCoords = noRepeatedCoordList.ToCoordinateArray();

            /**
             * Check to see if the removal of repeated points collapsed the coordinate
             * List to an invalid length for the type of the parent geometry. It is not
             * necessary to check for Point collapses, since the coordinate list can
             * never collapse to less than one point. If the length is invalid, return
             * the full-length coordinate array first computed, or null if collapses are
             * being removed. (This may create an invalid geometry - the client must
             * handle this.)
             */
            int minLength = 0;
            if (geom is ILineString)
                minLength = 2;
            if (geom is ILinearRing)
                minLength = LinearRing.MinimumValidSize;

            Coordinate[] collapsedCoords = reducedCoords;
            if (_removeCollapsed)
                collapsedCoords = null;

            // return null or orginal length coordinate array
            if (noRepeatedCoords.Length < minLength)
            {
                return collapsedCoords;
            }

            // ok to return shorter coordinate array
            return noRepeatedCoords;
        }

        #endregion
    }
}