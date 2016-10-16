// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 11/25/2010 10:23 AM
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************
namespace DotSpatial.Data
{
    /// <summary>
    /// This interface specifically insists on an IRasterBounds as the Bounds property.
    /// </summary>
    public interface IRasterBoundDataSet : IDataSet, IContainRasterBounds
    {
    }
}