// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.NationalGrids
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for SouthAfrica.
    /// </summary>
    public class SouthAfrica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo CapeLo15;
        public readonly ProjectionInfo CapeLo17;
        public readonly ProjectionInfo CapeLo19;
        public readonly ProjectionInfo CapeLo21;
        public readonly ProjectionInfo CapeLo23;
        public readonly ProjectionInfo CapeLo25;
        public readonly ProjectionInfo CapeLo27;
        public readonly ProjectionInfo CapeLo29;
        public readonly ProjectionInfo CapeLo31;
        public readonly ProjectionInfo CapeLo33;
        public readonly ProjectionInfo Hartebeesthoek94Lo15;
        public readonly ProjectionInfo Hartebeesthoek94Lo17;
        public readonly ProjectionInfo Hartebeesthoek94Lo19;
        public readonly ProjectionInfo Hartebeesthoek94Lo21;
        public readonly ProjectionInfo Hartebeesthoek94Lo23;
        public readonly ProjectionInfo Hartebeesthoek94Lo25;
        public readonly ProjectionInfo Hartebeesthoek94Lo27;
        public readonly ProjectionInfo Hartebeesthoek94Lo29;
        public readonly ProjectionInfo Hartebeesthoek94Lo31;
        public readonly ProjectionInfo Hartebeesthoek94Lo33;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAfrica.
        /// </summary>
        public SouthAfrica()
        {
            CapeLo15 = ProjectionInfo.FromAuthorityCode("ESRI", 102470).SetNames("Cape_Lo15", "GCS_Cape", "D_Cape"); // missing
            CapeLo17 = ProjectionInfo.FromAuthorityCode("ESRI", 102471).SetNames("Cape_Lo17", "GCS_Cape", "D_Cape"); // missing
            CapeLo19 = ProjectionInfo.FromAuthorityCode("ESRI", 102472).SetNames("Cape_Lo19", "GCS_Cape", "D_Cape"); // missing
            CapeLo21 = ProjectionInfo.FromAuthorityCode("ESRI", 102473).SetNames("Cape_Lo21", "GCS_Cape", "D_Cape"); // missing
            CapeLo23 = ProjectionInfo.FromAuthorityCode("ESRI", 102474).SetNames("Cape_Lo23", "GCS_Cape", "D_Cape"); // missing
            CapeLo25 = ProjectionInfo.FromAuthorityCode("ESRI", 102475).SetNames("Cape_Lo25", "GCS_Cape", "D_Cape"); // missing
            CapeLo27 = ProjectionInfo.FromAuthorityCode("ESRI", 102476).SetNames("Cape_Lo27", "GCS_Cape", "D_Cape"); // missing
            CapeLo29 = ProjectionInfo.FromAuthorityCode("ESRI", 102477).SetNames("Cape_Lo29", "GCS_Cape", "D_Cape"); // missing
            CapeLo31 = ProjectionInfo.FromAuthorityCode("ESRI", 102478).SetNames("Cape_Lo31", "GCS_Cape", "D_Cape"); // missing
            CapeLo33 = ProjectionInfo.FromAuthorityCode("ESRI", 102479).SetNames("Cape_Lo33", "GCS_Cape", "D_Cape"); // missing
            Hartebeesthoek94Lo15 = ProjectionInfo.FromAuthorityCode("ESRI", 102480).SetNames("Hartebeesthoek94_Lo15", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo17 = ProjectionInfo.FromAuthorityCode("ESRI", 102481).SetNames("Hartebeesthoek94_Lo17", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo19 = ProjectionInfo.FromAuthorityCode("ESRI", 102482).SetNames("Hartebeesthoek94_Lo19", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo21 = ProjectionInfo.FromAuthorityCode("ESRI", 102483).SetNames("Hartebeesthoek94_Lo21", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo23 = ProjectionInfo.FromAuthorityCode("ESRI", 102484).SetNames("Hartebeesthoek94_Lo23", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo25 = ProjectionInfo.FromAuthorityCode("ESRI", 102485).SetNames("Hartebeesthoek94_Lo25", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo27 = ProjectionInfo.FromAuthorityCode("ESRI", 102486).SetNames("Hartebeesthoek94_Lo27", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo29 = ProjectionInfo.FromAuthorityCode("ESRI", 102487).SetNames("Hartebeesthoek94_Lo29", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo31 = ProjectionInfo.FromAuthorityCode("ESRI", 102488).SetNames("Hartebeesthoek94_Lo31", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
            Hartebeesthoek94Lo33 = ProjectionInfo.FromAuthorityCode("ESRI", 102489).SetNames("Hartebeesthoek94_Lo33", "GCS_Hartebeesthoek_1994", "D_Hartebeesthoek_1994"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591