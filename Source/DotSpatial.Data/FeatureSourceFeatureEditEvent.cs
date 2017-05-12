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
    /// <summary>
    /// Callback specified when calling IFeatureSource.SearchAndModifyAttributes
    /// </summary>
    /// <param name="e">The event args.</param>
    /// <returns>Boolean</returns>
    public delegate bool FeatureSourceRowEditEvent(FeatureSourceRowEditEventArgs e);
}