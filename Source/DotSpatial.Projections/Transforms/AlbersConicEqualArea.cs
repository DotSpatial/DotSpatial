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
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    public class AlbersConicEqualArea : AlbersEqualArea
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of AlbersConicEqualArea
        /// </summary>
        public AlbersConicEqualArea()
        {
            Name = "Albers_Conic_Equal_Area";
            Proj4Name = "aea";
        }

        #endregion
    }
}