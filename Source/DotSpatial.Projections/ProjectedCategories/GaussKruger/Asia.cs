// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:36:17 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.GaussKruger
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Asia.
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Hanoi1972GKZone18;
        public readonly ProjectionInfo Hanoi1972GKZone19;
        public readonly ProjectionInfo SouthYemenGKZone8;
        public readonly ProjectionInfo SouthYemenGKZone9;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia.
        /// </summary>
        public Asia()
        {
            Hanoi1972GKZone18 = ProjectionInfo.FromEpsgCode(2044).SetNames("Hanoi_1972_GK_Zone_18", "GCS_Hanoi_1972", "D_Hanoi_1972");
            Hanoi1972GKZone19 = ProjectionInfo.FromEpsgCode(2045).SetNames("Hanoi_1972_GK_Zone_19", "GCS_Hanoi_1972", "D_Hanoi_1972");
            SouthYemenGKZone8 = ProjectionInfo.FromEpsgCode(2395).SetNames("South_Yemen_GK_Zone_8", "GCS_South_Yemen", "D_South_Yemen");
            SouthYemenGKZone9 = ProjectionInfo.FromEpsgCode(2396).SetNames("South_Yemen_GK_Zone_9", "GCS_South_Yemen", "D_South_Yemen");
        }

        #endregion
    }
}

#pragma warning restore 1591