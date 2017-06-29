namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// A thin, typically vertical separation bar.
    /// </summary>
    public class SeparatorItem : GroupedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeparatorItem"/> class.
        /// </summary>
        public SeparatorItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SeparatorItem class.
        /// </summary>
        public SeparatorItem(string rootKey, string groupCaption)
            : base(rootKey, groupCaption)
        {
        }
    }
}