// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Kyle Ellison. Created 06/24/2011.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************
namespace DotSpatial.Data
{
    ///<summary>
    /// Callback specified when calling IFeatureSource.SearchAndModifyAttributes
    ///</summary>
    ///<param name="e"></param>
    public delegate bool FeatureSourceRowEditEvent(FeatureSourceRowEditEventArgs e);

    /// <summary>
    /// FeatureSourceRowEditEvent arguments
    /// </summary>
    public class FeatureSourceRowEditEventArgs
    {
        /// <summary>
        /// RowEditEvent arguments
        /// </summary>
        public RowEditEventArgs RowEditEventArgs;

        /// <summary>
        /// Shape geometry associated with the row
        /// </summary>
        public Shape Shape;

        /// <summary>
        /// Construct FeatureSourceRowEditEventArgs
        /// </summary>
        /// <param name="rowEditEventArgs"></param>
        /// <param name="shape"></param>
        public FeatureSourceRowEditEventArgs(RowEditEventArgs rowEditEventArgs, Shape shape)
        {
            RowEditEventArgs = rowEditEventArgs;
            Shape = shape;
        }
    }
}