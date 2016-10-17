// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/11/2009 4:50:16 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    public interface IProjectionCategory
    {
        #region Properties

        /// <summary>
        /// Gets or sets the main category for this projection
        /// </summary>
        string MainCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the category for this projection
        /// </summary>
        string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the proj4 string that defines this projection
        /// </summary>
        string Proj4String
        {
            get;
            set;
        }

        #endregion
    }
}