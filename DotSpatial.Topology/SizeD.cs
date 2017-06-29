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
using System.ComponentModel;

namespace DotSpatial.Topology
{
    /// <summary>
    /// SizeD is just like a Size class except that it has double valued measures,
    /// and expresses sizes in three dimensions.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SizeD : ISize
    {
        #region Private Variables

        private double _xSize;
        private double _ySize;
        private double _zSize;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SizeD
        /// </summary>
        public SizeD()
        {
        }

        /// <summary>
        /// Creates a new SizeD structure.
        /// </summary>
        /// <param name="xSize">X or longitude size</param>
        /// <param name="ySize">Y or latitude size</param>
        /// <param name="zSize">Z or altitude size</param>
        public SizeD(double xSize, double ySize, double zSize)
        {
            _xSize = xSize;
            _ySize = ySize;
            _zSize = zSize;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the size in the x direction or longitude
        /// </summary>
        public virtual double XSize
        {
            get { return _xSize; }
            set { _xSize = value; }
        }

        /// <summary>
        /// Gets or sets the size in the y direction or latitude
        /// </summary>
        public virtual double YSize
        {
            get { return _ySize; }
            set { _ySize = value; }
        }

        /// <summary>
        /// Gets or sets the size in the z direction or altitude
        /// </summary>
        public virtual double ZSize
        {
            get { return _zSize; }
            set { _zSize = value; }
        }

        #endregion
    }
}