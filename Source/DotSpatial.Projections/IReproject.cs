// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  This Interface defines how reprojection code should be called.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/11/2009 4:46:40 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// This interface defines how reprojection classes should be accessed
    /// </summary>
    public interface IReproject
    {
        /// <summary>
        /// Reprojects the specified points.  The first is the projection info to start from, while the destination
        /// is the projection to end with.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        void ReprojectPoints(double[][] points, ProjectionInfo source, ProjectionInfo dest, int startIndex,
                                   int numPoints);
    }
}