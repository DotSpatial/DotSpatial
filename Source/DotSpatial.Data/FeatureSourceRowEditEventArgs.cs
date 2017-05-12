namespace DotSpatial.Data
{
    /// <summary>
    /// FeatureSourceRowEditEvent arguments
    /// </summary>
    public class FeatureSourceRowEditEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSourceRowEditEventArgs"/> class.
        /// </summary>
        /// <param name="rowEditEventArgs">The RowEditEventArgs.</param>
        /// <param name="shape">The shape.</param>
        public FeatureSourceRowEditEventArgs(RowEditEventArgs rowEditEventArgs, Shape shape)
        {
            RowEditEventArgs = rowEditEventArgs;
            Shape = shape;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the RowEditEvent arguments.
        /// </summary>
        public RowEditEventArgs RowEditEventArgs { get; set; }

        /// <summary>
        /// Gets or sets the shape geometry associated with the row.
        /// </summary>
        public Shape Shape { get; set; }

        #endregion
    }
}