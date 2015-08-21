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

using System;
using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Geometries.Utilities;

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// Simplifies a <see cref="IGeometry"/> using the Douglas-Peucker algorithm.
    /// </summary>
    /// <remarks>
    /// Ensures that any polygonal geometries returned are valid.
    /// Simple lines are not guaranteed to remain simple after simplification.
    /// All geometry types are handled. 
    /// Empty and point geometries are returned unchanged.
    /// Empty geometry components are deleted.
    /// <para/>
    /// Note that in general D-P does not preserve topology -
    /// e.g. polygons can be split, collapse to lines or disappear
    /// holes can be created or disappear,
    /// and lines can cross.
    /// To simplify point while preserving topology use TopologySafeSimplifier.
    /// (However, using D-P is significantly faster).
    /// <para/>
    /// KNOWN BUGS:
    /// In some cases the approach used to clean invalid simplified polygons
    /// can distort the output geometry severely.  
    /// </remarks>
    /// <seealso cref="TopologyPreservingSimplifier"/>
    public class DouglasPeuckerSimplifier
    {
        #region Fields

        private readonly IGeometry _inputGeom;
        private double _distanceTolerance;
        private bool _isEnsureValidTopology = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a simplifier for a given geometry.
        /// </summary>
        /// <param name="inputGeom">The geometry to simplify.</param>
        public DouglasPeuckerSimplifier(IGeometry inputGeom)
        {
            _inputGeom = inputGeom;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The distance tolerance for the simplification.
        /// </summary>
        /// <remarks>
        /// All vertices in the simplified geometry will be within this
        /// distance of the original geometry.
        /// The tolerance value must be non-negative. 
        /// </remarks>
        public double DistanceTolerance
        {
            get { return _distanceTolerance; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "value must be non-negative");
                _distanceTolerance = value;
            }
        }

        /// <summary>
        /// Controls whether simplified polygons will be "fixed"
        /// to have valid topology.
        /// </summary>
        /// <remarks>
        /// The caller may choose to disable this because:
        /// <list type="Bullet">
        /// <item>valid topology is not required</item>
        /// <item>fixing topology is a relative expensive operation</item>
        /// <item>in some pathological cases the topology fixing operation may either fail or run for too long</item>
        /// </list>
        /// The default is to fix polygon topology.
        /// </remarks>
        public bool EnsureValidTopology
        {
            get { return _isEnsureValidTopology; }
            set { _isEnsureValidTopology = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the simplified geometry.
        /// </summary>
        /// <returns>The simplified geometry.</returns>
        public IGeometry GetResultGeometry()
        {
            // empty input produces an empty result
            if (_inputGeom.IsEmpty)
                return (IGeometry)_inputGeom.Clone();

            DPTransformer transformer = new DPTransformer(this, EnsureValidTopology);
            return transformer.Transform(_inputGeom);
        }

        /// <summary>
        /// Simplifies a geometry using a given tolerance.
        /// </summary>
        /// <param name="geom">The geometry to simplify.</param>
        /// <param name="distanceTolerance">The tolerance to use.</param>
        /// <returns>A simplified version of the geometry.</returns>
        public static IGeometry Simplify(IGeometry geom, double distanceTolerance)
        {
            DouglasPeuckerSimplifier tss = new DouglasPeuckerSimplifier(geom);
            tss.DistanceTolerance = distanceTolerance;
            return tss.GetResultGeometry();
        }

        #endregion

        #region Classes

        /// <summary>
        ///
        /// </summary>
        private class DPTransformer : GeometryTransformer
        {
            #region Fields

            private readonly DouglasPeuckerSimplifier _container;
            private readonly bool _ensureValidTopology;

            #endregion

            #region Constructors

            /// <summary>
            ///
            /// </summary>
            /// <param name="container"></param>
            /// <param name="ensureValidTopology"></param>
            public DPTransformer(DouglasPeuckerSimplifier container, bool ensureValidTopology)
            {
                _container = container;
                _ensureValidTopology = ensureValidTopology;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Creates a valid area point from one that possibly has
            /// bad topology (i.e. self-intersections).
            /// Since buffer can handle invalid topology, but always returns
            /// valid point, constructing a 0-width buffer "corrects" the
            /// topology.
            /// Note this only works for area geometries, since buffer always returns
            /// areas.  This also may return empty geometries, if the input
            /// has no actual area.
            /// </summary>
            /// <param name="rawAreaGeom">An area point possibly containing self-intersections.</param>
            /// <returns>A valid area point.</returns>
            private IGeometry CreateValidArea(IGeometry rawAreaGeom)
            {
                if (_ensureValidTopology)
                    return rawAreaGeom.Buffer(0.0);
                return rawAreaGeom;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="coords"></param>
            /// <param name="parent"></param>
            /// <returns></returns>
            protected override ICoordinateSequence TransformCoordinates(ICoordinateSequence coords, IGeometry parent)
            {
                Coordinate[] inputPts = coords.ToCoordinateArray();
                IList<Coordinate> newPts = inputPts.Length == 0
                     ? new Coordinate[0]
                     : DouglasPeuckerLineSimplifier.Simplify(inputPts, _container.DistanceTolerance);

                return Factory.CoordinateSequenceFactory.Create(newPts);
            }

            ///<summary>
            /// Simplifies a LinearRing.  If the simplification results in a degenerate ring, remove the component.
            ///</summary>
            /// <returns>null if the simplification results in a degenerate ring</returns>
            protected override IGeometry TransformLinearRing(ILinearRing geom, IGeometry parent)
            {
                Boolean removeDegenerateRings = parent is IPolygon;
                IGeometry simpResult = base.TransformLinearRing(geom, parent);

                if (removeDegenerateRings && !(simpResult is ILinearRing))
                    return null;
                return simpResult;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="geom"></param>
            /// <param name="parent"></param>
            /// <returns></returns>
            protected override IGeometry TransformMultiPolygon(IMultiPolygon geom, IGeometry parent)
            {
                IGeometry roughGeom = base.TransformMultiPolygon(geom, parent);
                return CreateValidArea(roughGeom);
            }

            /// <summary>
            /// Simplifies a polygon, fixing it if required.
            /// </summary>
            /// <param name="geom"></param>
            /// <param name="parent"></param>
            /// <returns></returns>
            protected override IGeometry TransformPolygon(IPolygon geom, IGeometry parent)
            {
                // empty geometries are simply removed
                if (geom.IsEmpty)
                    return null;

                IGeometry rawGeom = base.TransformPolygon(geom, parent);
                // don't try and correct if the parent is going to do this
                if (parent is IMultiPolygon)
                    return rawGeom;
                return CreateValidArea(rawGeom);
            }

            #endregion
        }

        #endregion
    }
}