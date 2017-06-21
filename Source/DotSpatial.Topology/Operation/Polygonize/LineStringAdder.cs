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

namespace DotSpatial.Topology.Operation.Polygonize
{
    /// <summary>
    /// Add every linear element in a point into the polygonizer graph.
    /// </summary>
    public class LineStringAdder : IGeometryComponentFilter
    {
        #region Private Variables

        private readonly Polygonizer _container;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LineStringAdder
        /// </summary>
        public LineStringAdder(Polygonizer container)
        {
            _container = container;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the Filter to the specified geometry
        /// </summary>
        /// <param name="g"></param>
        public virtual void Filter(IGeometry g)
        {
            ILineString l = g as ILineString;
            if (l != null)
            {
                _container.Add(l);
            }
        }

        #endregion
    }
}