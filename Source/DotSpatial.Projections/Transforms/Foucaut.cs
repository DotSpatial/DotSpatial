// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The original content was ported from the C language from the 4.6 version of Proj4 libraries.
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:47:41 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// Foucaut is a tweaked version of QuarticAuthalic that uses tan mode.
    /// </summary>
    public class Foucaut : QuarticAuthalic
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of Foucaut
        /// </summary>
        public Foucaut()
        {
            Proj4Name = "fouc";
            Name = "Foucaut";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Setup(2, 2, true);
        }

        #endregion
    }
}