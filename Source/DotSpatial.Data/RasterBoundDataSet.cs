// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 11/25/2010 9:20 AM
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using DotSpatial.Projections;

namespace DotSpatial.Data
{
    /// <summary>
    /// This is an abstract base class that represents a datasets that has a RasterBounds on it, and reprojects
    /// by using the RasterBounds. This works for Image and Raster implementations.
    /// </summary>
    public abstract class RasterBoundDataSet : DataSet
    {
        #region Fields

        private IRasterBounds _bounds;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterBoundDataSet"/> class.
        /// </summary>
        protected RasterBoundDataSet()
        {
            _bounds = new RasterBounds(0, 0, new double[] { 0, 1, 0, 0, 0, -1 });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the image bounds being used to define the georeferencing of the image
        /// </summary>
        public IRasterBounds Bounds
        {
            get
            {
                return _bounds;
            }

            set
            {
                _bounds = value;

                OnBoundsChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets the Bounds.Envelope through an Extents property.
        /// </summary>
        public override Extent Extent
        {
            get
            {
                return Bounds?.Extent;
            }

            set
            {
                if (Bounds != null) Bounds.Extent = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// RasterBounds datasets offer a limited sort of reproject on the fly.
        /// This tries to update the bounds by reprojecting the top left and bottom
        /// left and top right coordinates and updating the "affine" transform.
        /// This should not be used if CanReproject is false.
        /// Greater accuracy can be accomplished using the Projective transform,
        /// which is planned to be implemented as a toolbox function.
        /// </summary>
        /// <param name="targetProjection">The projectionInfo to project to.</param>
        public override void Reproject(ProjectionInfo targetProjection)
        {
            if (CanReproject == false) return;

            double[] aff = Projections.Reproject.ReprojectAffine(Bounds.AffineCoefficients, Bounds.NumRows, Bounds.NumColumns, Projection, targetProjection);
            Bounds.AffineCoefficients = aff;
        }

        /// <summary>
        /// Disposes the managed memory objects in the ImageData class, and then forwards
        /// the Dispose operation to the internal dataset in the base class, if any.
        /// </summary>
        /// <param name="disposeManagedResources">Boolean, true if both managed and unmanaged resources should be finalized.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                Bounds = null;
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Occurs when the raster bounds of this data class have changed.
        /// </summary>
        /// <param name="bounds">The bounds that changed.</param>
        protected virtual void OnBoundsChanged(IRasterBounds bounds)
        {
        }

        #endregion
    }
}