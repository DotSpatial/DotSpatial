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
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// Simplifies a point, ensuring that
    /// the result is a valid point having the
    /// same dimension and number of components as the input.
    /// The simplification uses a maximum distance difference algorithm
    /// similar to the one used in the Douglas-Peucker algorithm.
    /// In particular, if the input is an areal point
    /// ( <c>Polygon</c> or <c>MultiPolygon</c> )
    /// The result has the same number of shells and holes (rings) as the input,
    /// in the same order
    /// The result rings touch at no more than the number of touching point in the input
    /// (although they may touch at fewer points).
    /// </summary>
    public class TopologyPreservingSimplifier
    {
        private readonly IGeometry _inputGeom;
        private readonly TaggedLinesSimplifier _lineSimplifier = new TaggedLinesSimplifier();
        private IDictionary _lineStringMap;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputGeom"></param>
        public TopologyPreservingSimplifier(IGeometry inputGeom)
        {
            _inputGeom = inputGeom;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double DistanceTolerance
        {
            get
            {
                return _lineSimplifier.DistanceTolerance;
            }
            set
            {
                _lineSimplifier.DistanceTolerance = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <param name="distanceTolerance"></param>
        /// <returns></returns>
        public static IGeometry Simplify(IGeometry geom, double distanceTolerance)
        {
            TopologyPreservingSimplifier tss = new TopologyPreservingSimplifier(geom);
            tss.DistanceTolerance = distanceTolerance;
            return tss.GetResultGeometry();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IGeometry GetResultGeometry()
        {
            _lineStringMap = new Hashtable();
            _inputGeom.Apply(new LineStringMapBuilderFilter(this));
            _lineSimplifier.Simplify(new ArrayList(_lineStringMap.Values));
            IGeometry result = (new LineStringTransformer(this)).Transform(_inputGeom);
            return result;
        }

        #region Nested type: LineStringMapBuilderFilter

        /// <summary>
        ///
        /// </summary>
        private class LineStringMapBuilderFilter : IGeometryComponentFilter
        {
            private readonly TopologyPreservingSimplifier _container;

            /// <summary>
            ///
            /// </summary>
            /// <param name="container"></param>
            public LineStringMapBuilderFilter(TopologyPreservingSimplifier container)
            {
                _container = container;
            }

            #region IGeometryComponentFilter Members

            /// <summary>
            ///
            /// </summary>
            /// <param name="geom"></param>
            public void Filter(IGeometry geom)
            {
                if (geom is LinearRing)
                {
                    TaggedLineString taggedLine = new TaggedLineString((LineString)geom, 4);
                    _container._lineStringMap.Add(geom, taggedLine);
                }
                else if (geom is LineString)
                {
                    TaggedLineString taggedLine = new TaggedLineString((LineString)geom, 2);
                    _container._lineStringMap.Add(geom, taggedLine);
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: LineStringTransformer

        /// <summary>
        ///
        /// </summary>
        private class LineStringTransformer : GeometryTransformer
        {
            private readonly TopologyPreservingSimplifier _container;

            /// <summary>
            ///
            /// </summary>
            /// <param name="container"></param>
            public LineStringTransformer(TopologyPreservingSimplifier container)
            {
                _container = container;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="coords"></param>
            /// <param name="parent"></param>
            /// <returns></returns>
            protected override IList<Coordinate> TransformCoordinates(IList<Coordinate> coords, IGeometry parent)
            {
                if (parent is LineString)
                {
                    TaggedLineString taggedLine = (TaggedLineString)_container._lineStringMap[parent];
                    return taggedLine.ResultCoordinates;
                }
                // for anything else (e.g. points) just copy the coordinates
                return base.TransformCoordinates(coords, parent);
            }
        }

        #endregion
    }
}