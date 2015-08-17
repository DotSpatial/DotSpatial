using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Buffer.Validate
{
    ///<summary>
    /// Finds the approximate maximum distance from a buffer curve to
    /// the originating geometry.
    ///</summary>
    /// <remarks><para>The approximate maximum distance is determined by testing
    /// all vertices in the buffer curve, as well
    /// as midpoints of the curve segments.
    /// Due to the way buffer curves are constructed, this
    /// should be a very close approximation.</para>
    /// <para>This is similar to the Discrete Oriented Hausdorff distance
    /// from the buffer curve to the input.</para>
    /// </remarks>
    /// <author>mbdavis</author>
    public class BufferCurveMaximumDistanceFinder
    {
        #region Fields

        private readonly IGeometry _inputGeom;
        private readonly PointPairDistance _maxPtDist = new PointPairDistance();

        #endregion

        #region Constructors

        public BufferCurveMaximumDistanceFinder(IGeometry inputGeom)
        {
            _inputGeom = inputGeom;
        }

        #endregion

        #region Properties

        public PointPairDistance DistancePoints
        {
            get { return _maxPtDist; }
        }

        #endregion

        #region Methods

        private void computeMaxMidpointDistance(IGeometry curve)
        {
            MaxMidpointDistanceFilter distFilter = new MaxMidpointDistanceFilter(_inputGeom);
            curve.Apply(distFilter);
            _maxPtDist.SetMaximum(distFilter.MaxPointDistance);
        }

        private void ComputeMaxVertexDistance(IGeometry curve)
        {
            MaxPointDistanceFilter distFilter = new MaxPointDistanceFilter(_inputGeom);
            curve.Apply(distFilter);
            _maxPtDist.SetMaximum(distFilter.MaxPointDistance);
        }

        public double FindDistance(IGeometry bufferCurve)
        {
            ComputeMaxVertexDistance(bufferCurve);
            computeMaxMidpointDistance(bufferCurve);
            return _maxPtDist.Distance;
        }

        #endregion

        #region Classes

        public class MaxMidpointDistanceFilter
          : ICoordinateSequenceFilter
        {
            #region Fields

            private readonly IGeometry geom;
            private readonly PointPairDistance maxPtDist = new PointPairDistance();
            private readonly PointPairDistance minPtDist = new PointPairDistance();

            #endregion

            #region Constructors

            public MaxMidpointDistanceFilter(IGeometry geom)
            {
                this.geom = geom;
            }

            #endregion

            #region Properties

            public bool Done
            {
                get { return false; }
            }

            public bool GeometryChanged
            {
                get { return false; }
            }

            public PointPairDistance MaxPointDistance
            {
                get { return maxPtDist; }
            }

            #endregion

            #region Methods

            public void Filter(ICoordinateSequence seq, int index)
            {
                if (index == 0)
                    return;

                var p0 = seq.GetCoordinate(index - 1);
                var p1 = seq.GetCoordinate(index);
                var midPt = new Coordinate(
                        (p0.X + p1.X) / 2,
                        (p0.Y + p1.Y) / 2);

                minPtDist.Initialize();
                DistanceToPointFinder.ComputeDistance(geom, midPt, minPtDist);
                maxPtDist.SetMaximum(minPtDist);
            }

            #endregion
        }

        public class MaxPointDistanceFilter : ICoordinateFilter
        {
            #region Fields

            private readonly IGeometry geom;
            private readonly PointPairDistance maxPtDist = new PointPairDistance();
            private readonly PointPairDistance minPtDist = new PointPairDistance();

            #endregion

            #region Constructors

            public MaxPointDistanceFilter(IGeometry geom)
            {
                this.geom = geom;
            }

            #endregion

            #region Properties

            public PointPairDistance MaxPointDistance
            {
                get { return maxPtDist; }
            }

            #endregion

            #region Methods

            public void Filter(Coordinate pt)
            {
                minPtDist.Initialize();
                DistanceToPointFinder.ComputeDistance(geom, pt, minPtDist);
                maxPtDist.SetMaximum(minPtDist);
            }

            #endregion
        }

        #endregion
    }
}