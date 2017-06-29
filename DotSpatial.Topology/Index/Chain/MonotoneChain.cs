// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Topology.Index.Chain
{
    /// <summary>
    /// MonotoneChains are a way of partitioning the segments of a linestring to
    /// allow for fast searching of intersections.
    /// They have the following properties:
    /// the segments within a monotone chain will never intersect each other
    /// the envelope of any contiguous subset of the segments in a monotone chain
    /// is equal to the envelope of the endpoints of the subset.
    /// Property 1 means that there is no need to test pairs of segments from within
    /// the same monotone chain for intersection.
    /// Property 2 allows
    /// binary search to be used to find the intersection points of two monotone chains.
    /// For many types of real-world data, these properties eliminate a large number of
    /// segment comparisons, producing substantial speed gains.
    /// One of the goals of this implementation of MonotoneChains is to be
    /// as space and time efficient as possible. One design choice that aids this
    /// is that a MonotoneChain is based on a subarray of a list of points.
    /// This means that new arrays of points (potentially very large) do not
    /// have to be allocated.
    /// MonotoneChains support the following kinds of queries:
    /// Envelope select: determine all the segments in the chain which
    /// intersect a given envelope.
    /// Overlap: determine all the pairs of segments in two chains whose
    /// envelopes overlap.
    /// This implementation of MonotoneChains uses the concept of internal iterators
    /// to return the resultsets for the above queries.
    /// This has time and space advantages, since it
    /// is not necessary to build lists of instantiated objects to represent the segments
    /// returned by the query.
    /// However, it does mean that the queries are not thread-safe.
    /// </summary>
    public class MonotoneChain
    {
        private readonly object _context;  // user-defined information
        private readonly int _end;
        private readonly IList<Coordinate> _pts;
        private readonly int _start;
        private Envelope _env;
        private int _id;                 // useful for optimizing chain comparisons

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="context"></param>
        public MonotoneChain(IList<Coordinate> pts, int start, int end, object context)
        {
            _pts = pts;
            _start = start;
            _end = end;
            _context = context;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual object Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Envelope Envelope
        {
            get
            {
                if (_env == null)
                {
                    Coordinate p0 = _pts[_start];
                    Coordinate p1 = _pts[_end];
                    _env = new Envelope(p0, p1);
                }
                return _env;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int StartIndex
        {
            get
            {
                return _start;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int EndIndex
        {
            get
            {
                return _end;
            }
        }

        /// <summary>
        /// Return the subsequence of coordinates forming this chain.
        /// Allocates a new array to hold the Coordinates.
        /// </summary>
        public virtual Coordinate[] Coordinates
        {
            get
            {
                Coordinate[] coord = new Coordinate[_end - _start + 1];
                int index = 0;
                for (int i = _start; i <= _end; i++)
                    coord[index++] = _pts[i];
                return coord;
            }
        }

        /// <summary>
        /// Gets a copy of the line segment located at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public virtual LineSegment GetLineSegment(int index)
        {
            return new LineSegment(_pts[index], _pts[index + 1]);
        }

        /// <summary>
        /// Determine all the line segments in the chain whose envelopes overlap
        /// the searchEnvelope, and process them.
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="mcs"></param>
        public virtual void Select(IEnvelope searchEnv, MonotoneChainSelectAction mcs)
        {
            ComputeSelect(searchEnv, _start, _end, mcs);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="start0"></param>
        /// <param name="end0"></param>
        /// <param name="mcs"></param>
        private void ComputeSelect(IEnvelope searchEnv, int start0, int end0, MonotoneChainSelectAction mcs)
        {
            Coordinate p0 = _pts[start0];
            Coordinate p1 = _pts[end0];
            mcs.TempEnv1.Init(p0, p1);

            // terminating condition for the recursion
            if (end0 - start0 == 1)
            {
                mcs.Select(this, start0);
                return;
            }
            // nothing to do if the envelopes don't overlap
            if (!searchEnv.Intersects(mcs.TempEnv1))
                return;

            // the chains overlap, so split each in half and iterate  (binary search)
            int mid = (start0 + end0) / 2;

            // Assert: mid != start or end (since we checked above for end - start <= 1)
            // check terminating conditions before recursing
            if (start0 < mid)
                ComputeSelect(searchEnv, start0, mid, mcs);
            if (mid < end0)
                ComputeSelect(searchEnv, mid, end0, mcs);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="mco"></param>
        public virtual void ComputeOverlaps(MonotoneChain mc, MonotoneChainOverlapAction mco)
        {
            ComputeOverlaps(_start, _end, mc, mc._start, mc._end, mco);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="start0"></param>
        /// <param name="end0"></param>
        /// <param name="mc"></param>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        /// <param name="mco"></param>
        private void ComputeOverlaps(int start0, int end0, MonotoneChain mc, int start1, int end1, MonotoneChainOverlapAction mco)
        {
            Coordinate p00 = _pts[start0];
            Coordinate p01 = _pts[end0];
            Coordinate p10 = mc._pts[start1];
            Coordinate p11 = mc._pts[end1];

            // terminating condition for the recursion
            if (end0 - start0 == 1 && end1 - start1 == 1)
            {
                mco.Overlap(this, start0, mc, start1);
                return;
            }
            // nothing to do if the envelopes of these chains don't overlap
            mco.TempEnv1.Init(p00, p01);
            mco.TempEnv2.Init(p10, p11);
            if (!mco.TempEnv1.Intersects(mco.TempEnv2))
                return;

            // the chains overlap, so split each in half and iterate  (binary search)
            int mid0 = (start0 + end0) / 2;
            int mid1 = (start1 + end1) / 2;

            // Assert: mid != start or end (since we checked above for end - start <= 1)
            // check terminating conditions before recursing
            if (start0 < mid0)
            {
                if (start1 < mid1)
                    ComputeOverlaps(start0, mid0, mc, start1, mid1, mco);
                if (mid1 < end1)
                    ComputeOverlaps(start0, mid0, mc, mid1, end1, mco);
            }
            if (mid0 < end0)
            {
                if (start1 < mid1)
                    ComputeOverlaps(mid0, end0, mc, start1, mid1, mco);
                if (mid1 < end1)
                    ComputeOverlaps(mid0, end0, mc, mid1, end1, mco);
            }
        }
    }
}