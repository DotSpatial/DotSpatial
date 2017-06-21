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
// *********************************************************************************************************

namespace DotSpatial.Topology
{
    /// <summary>
    /// IMatrix4
    /// </summary>
    public interface IMatrixD : IMatrix
    {
        #region Methods

        /// <summary>
        /// Multiplies every value in the specified n x m matrix by the specified double inScalar.
        /// </summary>
        /// <param name="inScalar">The double precision floating point to multiply all the members against</param>
        /// <returns>A new n x m matrix</returns>
        IMatrixD Multiply(double inScalar);

        /// <summary>
        /// This replaces the underlying general multiplication with a more specific type.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        IMatrixD Multiply(IMatrixD matrix);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the values for this matrix of double precision coordinates
        /// </summary>
        double[,] Values
        {
            get;
            set;
        }

        #endregion
    }
}