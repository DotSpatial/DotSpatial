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

using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// Simplifies a TaggedLineString, preserving topology
    /// (in the sense that no new intersections are introduced).
    /// Uses the recursive D-P algorithm.
    /// </summary>
    public class TaggedLineStringSimplifier
    {
        // notice: modified for "safe" assembly in Sql 2005
        // Added readonly!
        private static readonly LineIntersector Li = new RobustLineIntersector();

        private readonly LineSegmentIndex _inputIndex = new LineSegmentIndex();
        private readonly LineSegmentIndex _outputIndex = new LineSegmentIndex();
        private double _distanceTolerance;
        private TaggedLineString _line;
        private IList<Coordinate> _linePts;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputIndex"></param>
        /// <param name="outputIndex"></param>
        public TaggedLineStringSimplifier(LineSegmentIndex inputIndex, LineSegmentIndex outputIndex)
        {
            _inputIndex = inputIndex;
            _outputIndex = outputIndex;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double DistanceTolerance
        {
            get
            {
                return _distanceTolerance;
            }
            set
            {
                _distanceTolerance = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        public virtual void Simplify(TaggedLineString line)
        {
            _line = line;
            _linePts = line.ParentCoordinates;
            SimplifySection(0, _linePts.Count - 1, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="depth"></param>
        private void SimplifySection(int i, int j, int depth)
        {
            depth += 1;
            int[] sectionIndex = new int[2];
            if ((i + 1) == j)
            {
                LineSegment newSeg = _line.GetSegment(i);
                _line.AddToResult(newSeg);
                // leave this segment in the input index, for efficiency
                return;
            }

            double[] distance = new double[1];
            int furthestPtIndex = FindFurthestPoint(_linePts, i, j, distance);
            bool isValidToFlatten = true;

            // must have enough points in the output line
            if (_line.ResultSize < _line.MinimumSize && depth < 2) isValidToFlatten = false;
            // flattening must be less than distanceTolerance
            if (distance[0] > DistanceTolerance) isValidToFlatten = false;
            // test if flattened section would cause intersection
            LineSegment candidateSeg = new LineSegment();
            candidateSeg.P0 = _linePts[i];
            candidateSeg.P1 = _linePts[j];
            sectionIndex[0] = i;
            sectionIndex[1] = j;
            if (HasBadIntersection(_line, sectionIndex, candidateSeg)) isValidToFlatten = false;

            if (isValidToFlatten)
            {
                LineSegment newSeg = Flatten(i, j);
                _line.AddToResult(newSeg);
                return;
            }
            SimplifySection(i, furthestPtIndex, depth);
            SimplifySection(furthestPtIndex, j, depth);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        private static int FindFurthestPoint(IList<Coordinate> pts, int i, int j, double[] maxDistance)
        {
            LineSegment seg = new LineSegment();
            seg.P0 = pts[i];
            seg.P1 = pts[j];
            double maxDist = -1.0;
            int maxIndex = i;
            for (int k = i + 1; k < j; k++)
            {
                Coordinate midPt = pts[k];
                double distance = seg.Distance(midPt);
                if (distance > maxDist)
                {
                    maxDist = distance;
                    maxIndex = k;
                }
            }
            maxDistance[0] = maxDist;
            return maxIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private LineSegment Flatten(int start, int end)
        {
            // make a new segment for the simplified point
            Coordinate p0 = _linePts[start];
            Coordinate p1 = _linePts[end];
            LineSegment newSeg = new LineSegment(p0, p1);
            // update the indexes
            Remove(_line, start, end);
            _outputIndex.Add(newSeg);
            return newSeg;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentLine"></param>
        /// <param name="sectionIndex"></param>
        /// <param name="candidateSeg"></param>
        /// <returns></returns>
        private bool HasBadIntersection(TaggedLineString parentLine, int[] sectionIndex, LineSegment candidateSeg)
        {
            if (HasBadOutputIntersection(candidateSeg))
                return true;
            if (HasBadInputIntersection(parentLine, sectionIndex, candidateSeg))
                return true;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="candidateSeg"></param>
        /// <returns></returns>
        private bool HasBadOutputIntersection(LineSegment candidateSeg)
        {
            IList querySegs = _outputIndex.Query(candidateSeg);
            for (IEnumerator i = querySegs.GetEnumerator(); i.MoveNext(); )
            {
                LineSegment querySeg = (LineSegment)i.Current;
                if (HasInteriorIntersection(querySeg, candidateSeg))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentLine"></param>
        /// <param name="sectionIndex"></param>
        /// <param name="candidateSeg"></param>
        /// <returns></returns>
        private bool HasBadInputIntersection(TaggedLineString parentLine, int[] sectionIndex, LineSegment candidateSeg)
        {
            IList querySegs = _inputIndex.Query(candidateSeg);
            for (IEnumerator i = querySegs.GetEnumerator(); i.MoveNext(); )
            {
                TaggedLineSegment querySeg = (TaggedLineSegment)i.Current;
                if (HasInteriorIntersection(querySeg, candidateSeg))
                {
                    if (IsInLineSection(parentLine, sectionIndex, querySeg))
                        continue;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tests whether a segment is in a section of a TaggedLineString-
        /// </summary>
        /// <param name="line"></param>
        /// <param name="sectionIndex"></param>
        /// <param name="seg"></param>
        /// <returns></returns>
        private static bool IsInLineSection(TaggedLineString line, int[] sectionIndex, TaggedLineSegment seg)
        {
            // not in this line
            if (seg.Parent != line.Parent) return false;
            int segIndex = seg.Index;
            if (segIndex >= sectionIndex[0] && segIndex < sectionIndex[1])
                return true;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seg0"></param>
        /// <param name="seg1"></param>
        /// <returns></returns>
        private static bool HasInteriorIntersection(ILineSegmentBase seg0, ILineSegmentBase seg1)
        {
            Li.ComputeIntersection(seg0.P0, seg0.P1, seg1.P0, seg1.P1);
            return Li.IsInteriorIntersection();
        }

        /// <summary>
        /// Remove the segs in the section of the line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void Remove(TaggedLineString line, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                TaggedLineSegment seg = line.GetSegment(i);
                _inputIndex.Remove(seg);
            }
        }
    }
}