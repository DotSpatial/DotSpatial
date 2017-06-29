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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/20/2010 2:08:26 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// AuxiliarySphereTypes
    /// </summary>
    public enum AuxiliarySphereType
    {
        /// <summary>
        /// Use semimajor axis or radius of the geographic coordinate system
        /// </summary>
        SemimajorAxis = 0,
        /// <summary>
        /// Use semiminor axis or radius
        /// </summary>
        SemiminorAxis = 1,
        /// <summary>
        /// Calculate and use authalic radius
        /// </summary>
        Authalic = 2,
        /// <summary>
        /// Use authalic radius and convert geodetic latitudes to authalic latitudes
        /// </summary>
        AuthalicWithConvertedLatitudes = 3,
        /// <summary>
        /// This indicates that this parameter should not appear in the projection string.
        /// </summary>
        NotSpecified = 4
    }
}