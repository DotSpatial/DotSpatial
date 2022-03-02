// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/26/2009 2:50:35 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Derek Morris        |  9/24/2010 |  Added to handle Gauss_Kruger Esri projections
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// Gauss Kruger is basically transverse mercator
    /// </summary>
    public class GaussKruger : TransverseMercator
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of GaussKruger
        /// </summary>
        public GaussKruger()
        {
            Name = "Gauss_Kruger";
            Proj4Name = "tmerc";
        }

        #endregion
    }
}