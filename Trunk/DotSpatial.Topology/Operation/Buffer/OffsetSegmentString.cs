using System.Collections.Generic;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// A dynamic list of the vertices in a constructed offset curve.
    /// Automatically removes adjacent vertices
    /// which are closer than a given tolerance.
    /// </summary>
    /// <author>Martin Davis</author>
    internal class OffsetSegmentString
    {
        #region Fields

        private readonly List<Coordinate> _ptList;

        #endregion

        #region Constructors

        public OffsetSegmentString()
        {
            _ptList = new List<Coordinate>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The distance below which two adjacent points on the curve are considered to be coincident.
        /// This is chosen to be a small fraction of the offset distance.
        /// </summary>
        public double MinimumVertexDistance { get; set; }

        public IPrecisionModel PrecisionModel { get; set; }

        #endregion

        #region Methods

        public void AddPt(Coordinate pt)
        {
            var bufPt = new Coordinate(pt);
            PrecisionModel.MakePrecise(bufPt);
            // don't add duplicate (or near-duplicate) points
            if (IsRedundant(bufPt))
                return;
            _ptList.Add(bufPt);
        }

        public void AddPts(IList<Coordinate> pt, bool isForward)
        {
            if (isForward)
            {
                foreach (Coordinate t in pt)
                    AddPt(t);
            }
            else
            {
                for (int i = pt.Count - 1; i >= 0; i--)
                    AddPt(pt[i]);
            }
        }

        public void CloseRing()
        {
            if (_ptList.Count < 1)
                return;

            var startPt = new Coordinate(_ptList[0]);
            var lastPt = _ptList[_ptList.Count - 1];
           
            if (startPt.Equals(lastPt)) return;
            _ptList.Add(startPt);
        }

        public Coordinate[] GetCoordinates()
        {
            return _ptList.ToArray();
        }

        /// <summary>
        /// Tests whether the given point is redundant relative to the previous point in the list (up to tolerance).
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>true if the point is redundant</returns>
        private bool IsRedundant(Coordinate pt)
        {
            if (_ptList.Count < 1)
                return false;
            var lastPt = _ptList[_ptList.Count - 1];
            double ptDist = pt.Distance(lastPt);
            return ptDist < MinimumVertexDistance;
        }

        public void Reverse()
        {
        }

        public override string ToString()
        {
            var fact = new GeometryFactory();
            var line = fact.CreateLineString(GetCoordinates());
            return line.ToString();
        }

        #endregion
    }
}