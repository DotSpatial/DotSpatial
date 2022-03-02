// ********************************************************************************************************
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Christoph Perger    | 20/10/2017 |  Created Plate Carree as an alias of EquidistantCylindrical
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// PlateCarree
    /// </summary>
    public class PlateCarree : EquidistantCylindrical
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of PlateCarree as an alias of EquidistantCylindrical
        /// </summary>
        public PlateCarree()
        {
            Proj4Name = "eqc";
            Name = "Plate_Carree";
        }

        #endregion
    }
}