using System;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.IntervalRTree;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Algorithm.Locate
{
    ///<summary>
    /// Determines the location of <see cref="Coordinate"/>s relative to
    /// a <see cref="IPolygonal"/> geometry, using indexing for efficiency.
    /// This algorithm is suitable for use in cases where
    /// many points will be tested against a given area.
    /// <para/>
    /// Thread-safe and immutable.
    ///</summary>
    /// <author>Martin Davis</author>
    public class IndexedPointInAreaLocator : IPointOnGeometryLocator
    {
        #region Fields

        private readonly IntervalIndexedGeometry _index;

        #endregion

        #region Constructors

        ///<summary>
        /// Creates a new locator for a given <see cref="IGeometry"/>.
        ///</summary>
        /// <param name="g">the Geometry to locate in</param>
        public IndexedPointInAreaLocator(IGeometry g)
        {
            if (!(g is IPolygonal))
                throw new ArgumentException("Argument must be Polygonal");
            _index = new IntervalIndexedGeometry(g);
        }

        #endregion

        #region Methods

        ///<summary>
        /// Determines the <see cref="Location"/> of a point in an areal <see cref="IGeometry"/>.
        ///</summary>
        /// <param name="p">The point to test</param>
        /// <returns>The location of the point in the geometry
        /// </returns>
        public LocationType Locate(Coordinate p)
        {
            RayCrossingCounter rcc = new RayCrossingCounter(p);


            SegmentVisitor visitor = new SegmentVisitor(rcc);
            _index.Query(p.Y, p.Y, visitor);

            /*
             // MD - slightly slower alternative
            List segs = index.query(p.y, p.y);
            countSegs(rcc, segs);
            */

            return rcc.Location;
        }

        #endregion

        #region Classes

        private class IntervalIndexedGeometry
        {
            #region Fields

            private readonly SortedPackedIntervalRTree<LineSegment> _index = new SortedPackedIntervalRTree<LineSegment>();

            #endregion

            #region Constructors

            public IntervalIndexedGeometry(IGeometry geom)
            {
                Init(geom);
            }

            #endregion

            #region Methods

            private void AddLine(Coordinate[] pts)
            {
                for (int i = 1; i < pts.Length; i++)
                {
                    LineSegment seg = new LineSegment(pts[i - 1], pts[i]);
                    double min = Math.Min(seg.P0.Y, seg.P1.Y);
                    double max = Math.Max(seg.P0.Y, seg.P1.Y);
                    _index.Insert(min, max, seg);
                }
            }

            private void Init(IGeometry geom)
            {
                var lines = LinearComponentExtracter.GetLines(geom);
                foreach (ILineString line in lines)
                {
                    Coordinate[] pts = line.Coordinates;
                    AddLine(pts);
                }
            }

            /*
            public IList Query(double min, double max)
            {
                ArrayListVisitor visitor = new ArrayListVisitor();
                index.Query(min, max, visitor);
                return visitor.Items;
            }
             */

            public void Query(double min, double max, IItemVisitor<LineSegment> visitor)
            {
                _index.Query(min, max, visitor);
            }

            #endregion
        }

        /*
        private void countSegs(RayCrossingCounter rcc, List segs)
        {
          for (Iterator i = segs.iterator(); i.hasNext(); ) {
            LineSegment seg = (LineSegment) i.next();
            rcc.countSegment(seg.getCoordinate(0), seg.getCoordinate(1));
      
            // short-circuit if possible
            if (rcc.isOnSegment()) return;
          }
        }
        */

        private class SegmentVisitor : IItemVisitor<LineSegment>
        {
            #region Fields

            private readonly RayCrossingCounter _counter;

            #endregion

            #region Constructors

            public SegmentVisitor(RayCrossingCounter counter)
            {
                _counter = counter;
            }

            #endregion

            #region Methods

            public void VisitItem(LineSegment seg)
            {
                _counter.CountSegment(seg.GetCoordinate(0), seg.GetCoordinate(1));
            }

            #endregion
        }

        #endregion
    }
}