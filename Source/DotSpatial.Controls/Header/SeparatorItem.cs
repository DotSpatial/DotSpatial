namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A thin, typically vertical separation bar.
    /// </summary>
    public class SeparatorItem : GroupedItem
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorItem"/> class.
        /// </summary>
        public SeparatorItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorItem"/> class.
        /// </summary>
        /// <param name="rootKey">The root key.</param>
        /// <param name="groupCaption">The groups caption.</param>
        public SeparatorItem(string rootKey, string groupCaption)
            : base(rootKey, groupCaption)
        {
        }

        #endregion
    }
}