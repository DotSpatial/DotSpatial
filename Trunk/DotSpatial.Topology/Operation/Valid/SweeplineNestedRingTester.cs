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
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.Index.Sweepline;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Operation.Valid
{
    /// <summary>
    /// Tests whether any of a set of <c>LinearRing</c>s are
    /// nested inside another ring in the set, using a <c>SweepLineIndex</c>
    /// index to speed up the comparisons.
    /// </summary>
    public class SweeplineNestedRingTester
    {
        #region Fields

        private readonly GeometryGraph _graph;  // used to find non-node vertices
        private readonly IList _rings = new ArrayList();
        private Coordinate _nestedPt;
        private SweepLineIndex _sweepLine;
        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        public SweeplineNestedRingTester(GeometryGraph graph)
        {
            _graph = graph;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Coordinate NestedPoint 
        {
            get
            {
                return _nestedPt; 
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ring"></param>
        public void Add(ILinearRing ring)
        {
            _rings.Add(ring);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildIndex()
        {
            _sweepLine = new SweepLineIndex();
            for (int i = 0; i < _rings.Count; i++) 
            {
                ILinearRing ring = (ILinearRing) _rings[i];
                IEnvelope env = ring.EnvelopeInternal;
                SweepLineInterval sweepInt = new SweepLineInterval(env.Minimum.X, env.Maximum.X, ring);
                _sweepLine.Add(sweepInt);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerRing"></param>
        /// <param name="searchRing"></param>
        /// <returns></returns>
        private bool IsInside(ILinearRing innerRing, ILinearRing searchRing)
        {
            IList<Coordinate> innerRingPts = innerRing.Coordinates;
            IList<Coordinate> searchRingPts = searchRing.Coordinates;
            if (!innerRing.EnvelopeInternal.Intersects(searchRing.EnvelopeInternal))
                return false;
            Coordinate innerRingPt = IsValidOp.FindPointNotNode(innerRingPts, searchRing, _graph);
            Assert.IsTrue(innerRingPt != null, "Unable to find a ring point not a node of the search ring");
            bool isInside = CGAlgorithms.IsPointInRing(innerRingPt, searchRingPts);
            if (isInside) 
            {
                _nestedPt = innerRingPt;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsNonNested()
        {
            BuildIndex();
            OverlapAction action = new OverlapAction(this);
            _sweepLine.ComputeOverlaps(action);
            return action.IsNonNested;
        }

        #endregion

        #region Classes

        /// <summary>
        /// 
        /// </summary>
        public class OverlapAction : ISweepLineOverlapAction
        {
            #region Fields

            private readonly SweeplineNestedRingTester _container;
            bool _isNonNested = true;

            #endregion

            #region Constructors

            /// <summary>
            /// 
            /// </summary>
            /// <param name="container"></param>
            public OverlapAction(SweeplineNestedRingTester container)
            {
                _container = container;
            }

            #endregion

            #region Properties

            /// <summary>
            /// 
            /// </summary>
            public bool IsNonNested
            {
                get 
                { 
                    return _isNonNested; 
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// 
            /// </summary>
            /// <param name="s0"></param>
            /// <param name="s1"></param>
            public void Overlap(SweepLineInterval s0, SweepLineInterval s1)
            {
                ILinearRing innerRing = (ILinearRing) s0.Item;
                ILinearRing searchRing = (ILinearRing) s1.Item;
                if (innerRing == searchRing) 
                    return;
                if (_container.IsInside(innerRing, searchRing))
                    _isNonNested = false;
            }

            #endregion
        }

        #endregion
    }
}