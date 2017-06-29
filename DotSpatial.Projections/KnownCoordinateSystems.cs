// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 11:58:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// KnownCoordinateSystems
    /// </summary>
    public static class KnownCoordinateSystems
    {
        /// <summary>
        /// Geographic systems operate in angular units, but can use different
        /// spheroid definitions or angular offsets.
        /// </summary>
        private static GeographicSystems _geographic;

        /// <summary>
        /// Projected systems are systems that use linear units like meters or feet
        /// rather than angular units like degrees or radians
        /// </summary>
        private static ProjectedSystems _projected;

        /// <summary>
        /// Projected systems are systems that use linear units like meters or feet
        /// rather than angular units like degrees or radians
        /// </summary>
        public static ProjectedSystems Projected
        {
            get { return _projected ?? (_projected = new ProjectedSystems()); }
        }

        /// <summary>
        /// Geographic systems operate in angular units, but can use different
        /// spheroid definitions or angular offsets.
        /// </summary>
        public static GeographicSystems Geographic
        {
            get { return _geographic ?? (_geographic = new GeographicSystems()); }
        }
    }
}