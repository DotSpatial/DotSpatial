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
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation
{
    /// <summary>
    /// The base class for operations that require <see cref="GeometryGraph"/>s.
    /// </summary>
    public class GeometryGraphOperation
    {
        #region Fields

        /// <summary>
        /// The operation args into an array so they can be accessed by index.
        /// </summary>
        protected GeometryGraph[] arg;

        /// <summary>
        /// 
        /// </summary>
        protected IPrecisionModel resultPrecisionModel;

        private LineIntersector _li = new RobustLineIntersector();

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        public GeometryGraphOperation(IGeometry g0, IGeometry g1)
            :this(g0, g1, BoundaryNodeRules.OgcSfsBoundaryRule /*BoundaryNodeRules.EndpointBoundaryRule*/)
        {}

        public GeometryGraphOperation(IGeometry g0, IGeometry g1, IBoundaryNodeRule boundaryNodeRule)
        {
            // use the most precise model for the result
            if (g0.PrecisionModel.CompareTo(g1.PrecisionModel) >= 0)
                 ComputationPrecision = g0.PrecisionModel;
            else ComputationPrecision = g1.PrecisionModel;

            arg = new GeometryGraph[2];
            arg[0] = new GeometryGraph(0, g0, boundaryNodeRule);
            arg[1] = new GeometryGraph(1, g1, boundaryNodeRule);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g0"></param>
        public GeometryGraphOperation(IGeometry g0) 
        {
            ComputationPrecision = g0.PrecisionModel;

            arg = new GeometryGraph[1];
            arg[0] = new GeometryGraph(0, g0);;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected IPrecisionModel ComputationPrecision
        {
            get
            {
                return resultPrecisionModel;
            }
            set
            {
                resultPrecisionModel = value;
                lineIntersector.PrecisionModel = resultPrecisionModel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected LineIntersector lineIntersector
        {
            get
            {
                return _li;
            }
            set
            {
                _li = value;
            }

        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IGeometry GetArgGeometry(int i)
        {
            return arg[i].Geometry; 
        }

        #endregion
    }
}