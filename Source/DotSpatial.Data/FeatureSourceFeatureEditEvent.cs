// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
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