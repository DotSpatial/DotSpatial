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

using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A GraphComponent is the parent class for the objects'
    /// that form a graph.  Each GraphComponent can carry a
    /// Label.
    /// </summary>
    public abstract class GraphComponent
    {
        #region Private Variables

        private bool _isCovered;
        private bool _isCoveredSet;
        private bool _isInResult;
        private bool _isVisited;
        private Label _label;

        #endregion

        /// <summary>
        ///
        /// </summary>
        protected GraphComponent() { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inLabel"></param>
        protected GraphComponent(Label inLabel)
        {
            _label = inLabel;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Label Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsInResult
        {
            get
            {
                return _isInResult;
            }
            set
            {
                _isInResult = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsCovered
        {
            get
            {
                return _isCovered;
            }
            set
            {
                _isCovered = value;
                _isCoveredSet = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsCoveredSet
        {
            get
            {
                return _isCoveredSet;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsVisited
        {
            get
            {
                return _isVisited;
            }
            set
            {
                _isVisited = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>
        /// A coordinate in this component (or null, if there are none).
        /// </returns>
        public abstract Coordinate Coordinate { get; set; }

        /// <summary>
        /// An isolated component is one that does not intersect or touch any other
        /// component.  This is the case if the label has valid locations for
        /// only a single Geometry.
        /// </summary>
        /// <returns><c>true</c> if this component is isolated.</returns>
        public abstract bool IsIsolated { get; }

        /// <summary>
        /// Compute the contribution to an IM for this component.
        /// </summary>
        public abstract void ComputeIm(IntersectionMatrix im);

        /// <summary>
        /// Update the IM with the contribution for this component.
        /// A component only contributes if it has a labelling for both parent geometries.
        /// </summary>
        /// <param name="im"></param>
        public virtual void UpdateIm(IntersectionMatrix im)
        {
            Assert.IsTrue(Label.GeometryCount >= 2, "found partial label");
            ComputeIm(im);
        }
    }
}