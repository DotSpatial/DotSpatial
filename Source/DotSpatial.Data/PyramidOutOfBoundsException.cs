// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 11:51:17 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    public class PyramidOutOfBoundsException : PyramidException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of PyramidOutOfBoundsException
        /// </summary>
        public PyramidOutOfBoundsException()
            : base(DataStrings.PyramidOutOfBoundsException)
        {
        }

        #endregion
    }
}