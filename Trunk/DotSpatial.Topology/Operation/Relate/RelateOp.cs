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

using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Relate
{
    /// <summary>
    /// Implements the SFS <c>relate()</c>  generalized spatial predicate on two <see cref="IGeometry"/>s.
    /// <br/>
    /// The class supports specifying a custom <see cref="IBoundaryNodeRule"/>
    /// to be used during the relate computation.
    /// </summary>
    /// <remarks>
    /// If named spatial predicates are used on the result <see cref="IntersectionMatrix"/>
    /// of the RelateOp, the result may or not be affected by the 
    /// choice of <tt>BoundaryNodeRule</tt>, depending on the exact nature of the pattern.
    /// For instance, <see cref="Geometries.IntersectionMatrix.IsIntersects"/> is insensitive 
    /// to the choice of <tt>BoundaryNodeRule</tt>, 
    /// whereas <see cref="Geometries.IntersectionMatrix.IsTouches"/> is affected by the rule chosen.
    /// <para/> 
    /// <b>Note:</b> custom Boundary Node Rules do not (currently)
    /// affect the results of other <see cref="IGeometry"/> methods (such
    /// as <see cref="IGeometry.Boundary"/>.  The results of
    /// these methods may not be consistent with the relationship computed by
    /// a custom Boundary Node Rule.
    /// </remarks>
    public class RelateOp : GeometryGraphOperation
    {
        #region Fields

        private readonly RelateComputer _relate;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Relate operation, using the default (OGC SFS) Boundary Node Rule.
        /// </summary>
        /// <param name="g0">a Geometry to relate</param>
        /// <param name="g1">another Geometry to relate</param>
        public RelateOp(IGeometry g0, IGeometry g1) : base(g0, g1)
        {            
            _relate = new RelateComputer(arg);
        }

        /// <summary>
        /// Creates a new Relate operation, using the default (OGC SFS) Boundary Node Rule.
        /// </summary>
        /// <param name="g0">a Geometry to relate</param>
        /// <param name="g1">another Geometry to relate</param>
        /// <param name="boundaryNodeRule">The Boundary Node Rule to use</param>
        public RelateOp(IGeometry g0, IGeometry g1, IBoundaryNodeRule boundaryNodeRule)
            : base(g0, g1, boundaryNodeRule)
        {
            _relate = new RelateComputer(arg);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the IntersectionMatrix for the spatial relationship
        /// between the input geometries.
        /// </summary>
        public IntersectionMatrix IntersectionMatrix
        {
            get
            {
                return _relate.ComputeIM();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the <see cref="IntersectionMatrix"/> for the spatial relationship
        ///  between two <see cref="IGeometry"/>s, using the default (OGC SFS) Boundary Node Rule
        /// </summary>
        /// <param name="a">A geometry to test</param>
        /// <param name="b">A geometry to test</param>
        /// <returns>The IntersectonMatrix for the spatial relationship between the geometries</returns>
        public static IntersectionMatrix Relate(IGeometry a, IGeometry b)
        {
            RelateOp relOp = new RelateOp(a, b);
            IntersectionMatrix im = relOp.IntersectionMatrix;
            return im;
        }

        /// <summary>
        /// Computes the <see cref="IntersectionMatrix"/> for the spatial relationship
        ///  between two <see cref="IGeometry"/>s, using the specified Boundary Node Rule
        /// </summary>
        /// <param name="a">A geometry to test</param>
        /// <param name="b">A geometry to test</param>
        /// <param name="boundaryNodeRule">The Boundary Node Rule to use</param>
        /// <returns>The IntersectonMatrix for the spatial relationship between the geometries</returns>
        public static IntersectionMatrix Relate(IGeometry a, IGeometry b, IBoundaryNodeRule boundaryNodeRule)
        {
            RelateOp relOp = new RelateOp(a, b, boundaryNodeRule);
            IntersectionMatrix im = relOp.IntersectionMatrix;
            return im;
        }

        #endregion
    }
}