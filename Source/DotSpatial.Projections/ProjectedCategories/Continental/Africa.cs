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

namespace DotSpatial.Projections.ProjectedCategories.Continental
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Africa.
    /// </summary>
    public class Africa : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AfricaAlbersEqualAreaConic;
        public readonly ProjectionInfo AfricaEquidistantConic;
        public readonly ProjectionInfo AfricaLambertConformalConic;
        public readonly ProjectionInfo AfricaSinusoidal;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Africa.
        /// </summary>
        public Africa()
        {
            AfricaAlbersEqualAreaConic = ProjectionInfo.FromAuthorityCode("ESRI", 102022).SetNames("Africa_Albers_Equal_Area_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AfricaEquidistantConic = ProjectionInfo.FromAuthorityCode("ESRI", 102023).SetNames("Africa_Equidistant_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AfricaLambertConformalConic = ProjectionInfo.FromAuthorityCode("ESRI", 102024).SetNames("Africa_Lambert_Conformal_Conic", "GCS_WGS_1984", "D_WGS_1984");
            AfricaSinusoidal = ProjectionInfo.FromAuthorityCode("ESRI", 102011).SetNames("Africa_Sinusoidal", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591