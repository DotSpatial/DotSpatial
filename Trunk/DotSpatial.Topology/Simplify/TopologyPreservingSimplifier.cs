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
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Utilities;

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// Simplifies a point and ensures that
    /// the result is a valid point having the
    /// same dimension and number of components as the input,
    /// and with the components having the same topological 
    /// relationship.
    /// <para/>
    /// If the input is a polygonal geometry
    /// (<see cref="IPolygon"/> or <see cref="IMultiPolygon"/>):
    /// <list type="Bullet">
    /// <item>The result has the same number of shells and holes as the input,
    ///  with the same topological structure</item>
    /// <item>The result rings touch at no more than the number of touching points in the input
    /// (although they may touch at fewer points).
    /// The key implication of this statement is that if the
    /// input is topologically valid, so is the simplified output.</item>
    /// </list>
    /// For linear geometries, if the input does not contain
    /// any intersecting line segments, this property
    /// will be preserved in the output.
    /// <para/>
    /// For all geometry types, the result will contain 
    /// enough vertices to ensure validity.  For polygons
    /// and closed linear geometries, the result will have at
    /// least 4 vertices; for open linestrings the result
    /// will have at least 2 vertices.
    /// <para/>
    /// All geometry types are handled. 
    /// Empty and point geometries are returned unchanged.
    /// Empty geometry components are deleted.
    /// <para/>
    /// The simplification uses a maximum-distance difference algorithm
    /// similar to the Douglas-Peucker algorithm.
    /// </summary>
    /// <remarks>
    /// <h3>KNOWN BUGS</h3>
    /// <list type="Bullet">
    /// <item>May create invalid topology if there are components which are small 
    /// relative to the tolerance value.
    /// In particular, if a small hole is very near an edge, 
    /// it is possible for the edge to be moved by a relatively large tolerance value 
    /// and end up with the hole outside the result shell (or inside another hole).
    /// Similarly, it is possible for a small polygon component to end up inside
    /// a nearby larger polygon.
    /// A workaround is to test for this situation in post-processing and remove
    /// any invalid holes or polygons.</item>
    /// </list>
    /// </remarks>
    /// <seealso cref="DouglasPeuckerSimplifier"/>
    public class TopologyPreservingSimplifier
    {
        #region Fields

        private readonly IGeometry _inputGeom;
        private readonly TaggedLinesSimplifier _lineSimplifier = new TaggedLinesSimplifier();
        private Dictionary<ILineString, TaggedLineString> _lineStringMap;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputGeom"></param>
        public TopologyPreservingSimplifier(IGeometry inputGeom)
        {
            _inputGeom = inputGeom;
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public virtual double DistanceTolerance
        {
            get { return _lineSimplifier.DistanceTolerance; }
            set { _lineSimplifier.DistanceTolerance = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IGeometry GetResultGeometry()
        {
            // empty input produces an empty result
            if (_inputGeom.IsEmpty)
                return (IGeometry)_inputGeom.Clone();

            _lineStringMap = new Dictionary<ILineString, TaggedLineString>();
            LineStringMapBuilderFilter filter = new LineStringMapBuilderFilter(this);
            _inputGeom.Apply(filter);
            _lineSimplifier.Simplify(_lineStringMap.Values);
            LineStringTransformer transformer = new LineStringTransformer(this);
            IGeometry result = transformer.Transform(_inputGeom);
            return result;
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

        #endregion

        #region Classes

        /// <summary>
        /// A filter to add linear geometries to the linestring map 
        /// with the appropriate minimum size constraint.
        /// Closed <see cref="ILineString"/>s (including <see cref="ILinearRing"/>s
        /// have a minimum output size constraint of 4, 
        /// to ensure the output is valid.
        /// For all other linestrings, the minimum size is 2 points.
        /// </summary>
        /// <author>Martin Davis</author>
        private class LineStringMapBuilderFilter : IGeometryComponentFilter
        {
            #region Fields

            private readonly TopologyPreservingSimplifier _container;

            #endregion

            #region Constructors

            public LineStringMapBuilderFilter(TopologyPreservingSimplifier container)
            {
                _container = container;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Filters linear geometries.
            /// </summary>
            /// <param name="geom">A geometry of any type</param>
            public void Filter(IGeometry geom)
            {
                ILineString line = geom as ILineString;
                if (line == null)
                    return;
                if (line.IsEmpty)
                    return;
                int minSize = line.IsClosed ? 4 : 2;
                TaggedLineString taggedLine = new TaggedLineString(line, minSize);
                _container._lineStringMap.Add(line, taggedLine);
            }

            #endregion
        }

        /// <summary>
        ///
        /// </summary>
        private class LineStringTransformer : GeometryTransformer
        {
            #region Fields

            private readonly TopologyPreservingSimplifier _container;

            #endregion

            #region Constructors

            public LineStringTransformer(TopologyPreservingSimplifier container)
            {
                _container = container;
            }

            #endregion

            #region Methods

            /// <summary>
            ///
            /// </summary>
            /// <param name="coords"></param>
            /// <param name="parent"></param>
            /// <returns></returns>
            protected override ICoordinateSequence TransformCoordinates(ICoordinateSequence coords, IGeometry parent)
            {
                // for empty coordinate sequences return null
                if (coords.Count == 0)
                    return null;

                // for linear components (including rings), simplify the linestring
                ILineString s = parent as ILineString;
                if (s != null)
                {
                    TaggedLineString taggedLine = _container._lineStringMap[s];
                    return CreateCoordinateSequence(taggedLine.ResultCoordinates);
                }
                // for anything else (e.g. points) just copy the coordinates
                return base.TransformCoordinates(coords, parent);
            }

            #endregion
        }

        #endregion
    }
}