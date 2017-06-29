namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    ///
    /// </summary>
    public interface ICategoryControl
    {
        /// <summary>
        /// Initializes the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        void Initialize(ILayer layer);

        /// <summary>
        /// Applies the changes.
        /// </summary>
        void ApplyChanges();

        /// <summary>
        /// Cancels changes.
        /// </summary>
        void Cancel();
    }
}