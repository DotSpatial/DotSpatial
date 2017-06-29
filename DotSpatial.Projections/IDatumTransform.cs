// ********************************************************************************************************
// Product Name: DotSpatial.Projections
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
//
// The Initial Developer of this Original Code is Steve Riddell. Created 5/27/2011 1:04:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// ********************************************************************************************************
namespace DotSpatial.Projections
{
    /// <summary>
    /// IDatumTransform
    /// </summary>
    public interface IDatumTransform
    {
        /// <summary>
        /// Transform function
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="xy"></param>
        /// <param name="z"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        void Transform(ProjectionInfo source, ProjectionInfo dest, double[] xy, double[] z, int startIndex, int numPoints);
    }
}