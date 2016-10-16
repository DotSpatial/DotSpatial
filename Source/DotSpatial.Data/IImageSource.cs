// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/12/2010 10:45:38 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// IImageSource
    /// </summary>
    public interface IImageSource
    {
        #region Properties

        /// <summary>
        /// Gets or sets the bounds
        /// </summary>
        RasterBounds Bounds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of rows
        /// </summary>
        int NumRows
        {
            get;
        }

        /// <summary>
        /// Gets the total number of columns
        /// </summary>
        int NumColumns
        {
            get;
        }

        /// <summary>
        /// Gets the number of overviews, not counting the original image
        /// </summary>
        int NumOverviews
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Returns the data from the file in the form of ARGB bytes.
        /// </summary>
        /// <param name="startRow">The zero based integer index of the first row (Y)</param>
        /// <param name="startColumn">The zero based integer index of the first column (X)</param>
        /// <param name="numRows">The number of rows to read</param>
        /// <param name="numColumns">The number of columns to read</param>
        /// <param name="overview">The integer overview.  0 for the original image.  Each successive index divides the length and height in half.  </param>
        /// <returns>A Byte of values in ARGB order and in row-major raster-scan sequence</returns>
        byte[] ReadWindow(int startRow, int startColumn, int numRows, int numColumns, int overview);

        /// <summary>
        /// This returns the window of data as a bitmap.
        /// </summary>
        /// <param name="startRow">The zero based integer index of the first row (Y)</param>
        /// <param name="startColumn">The zero based integer index of the first column (X)</param>
        /// <param name="numRows">The number of rows to read</param>
        /// <param name="numColumns">The number of columns to read</param>
        /// <param name="overview">The integer overview.  0 for the original image.  Each successive index divides the length and height in half.  </param>
        /// <returns></returns>
        Bitmap GetBitmap(int startRow, int startColumn, int numRows, int numColumns, int overview);

        #endregion
    }
}