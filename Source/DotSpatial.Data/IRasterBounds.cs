// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2008 12:30:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************



namespace DotSpatial.Data
{
    public interface IRasterBounds
    {
        #region Methods

        /// <summary>
        /// Returns a deep copy of this raster bounds object.
        /// </summary>
        /// <returns>An IRasterBounds interface.</returns>
        IRasterBounds Copy();

        /// <summary>
        /// This is the overridable open method that should be used.
        /// The OpenWorldFile method is called by our RasterBounds class,
        /// but this allows custom files to be loaded and saved.
        /// </summary>
        void Open(string fileName);

        /// <summary>
        /// This is the overridable save method that should be used.
        /// The OpenWorldFile method is called by our RasterBounds class,
        /// but this allows custom files to be loaded and saved.
        /// </summary>
        void Save();

        #endregion


        /// <summary>
        /// Gets or sets the difference between the maximum and minimum y values.
        /// Setting this will change only the minimum Y value, leaving the Top alone
        /// </summary>
        /// <returns>max y - min y, or 0 if this is a null <c>Envelope</c>.</returns>
        double Height { get; set; }

        /// <summary>
        /// Gets or Sets the difference between the maximum and minimum x values.
        /// Setting this will change only the Maximum X value, and leave the minimum X alone
        /// </summary>
        /// <returns>max x - min x, or 0 if this is a null <c>Envelope</c>.</returns>
        double Width { get; set; }

        /// <summary>
        /// In the precedent set by controls, envelopes support an X value and a width.
        /// The X value represents the Minimum.X coordinate for this envelope.
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// In the precedent set by controls, envelopes support a Y value and a height.
        /// the Y value represents the Top of the envelope, (Maximum.Y) and the Height
        /// represents the span between Maximum and Minimum Y.
        /// </summary>
        double Y { get; set; }

        #region Properties

        /// <summary>
        /// Gets or sets the double affine coefficients that control the world-file
        /// positioning of this image. X' and Y' are real world coords.
        /// X' = [0] + [1] * Column + [2] * Row
        /// Y' = [3] + [4] * Column + [5] * Row
        /// </summary>
        double[] AffineCoefficients
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the desired width per cell. This will keep the skew the same, but
        /// will adjust both the column based and row based width coefficients in order
        /// to match the specified cell width. This can be thought of as the width
        /// of a bounding box that contains an entire grid cell, no matter if it is skewed.
        /// </summary>
        double CellWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the desired height per cell. This will keep the skew the same, but
        /// will adjust both the column based and row based height coefficients in order
        /// to match the specified cell height. This can be thought of as the height
        /// of a bounding box that contains an entire grid cell, no matter if it is skewed.
        /// </summary>
        double CellHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rectangular confines for this envelope. The skew will remain
        /// the same when setting this, but the image will be translated and stretched
        /// to fit in the specified envelope.
        /// </summary>
        Extent Extent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of rows in the raster.
        /// </summary>
        int NumRows
        {
            get;
        }

        /// <summary>
        /// Gets the number of columns in the raster.
        /// </summary>
        int NumColumns
        {
            get;
        }

        /// <summary>
        /// Gets or sets the world file name. This won't do anything until the "Load" or "Save" methods are called.
        /// </summary>
        string WorldFile
        {
            get;
            set;
        }

        #endregion
    }
}