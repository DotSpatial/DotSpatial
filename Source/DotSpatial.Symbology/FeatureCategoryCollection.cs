// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 3:56:16 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureCategoryCollection
    /// </summary>
    public class FeatureCategoryCollection : ChangeEventList<IFeatureCategory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureCategoryCollection"/> class.
        /// </summary>
        public FeatureCategoryCollection()
        {
            Add(new FeatureCategory()); // default grouping
        }

        /// <summary>
        /// Gets or sets the scheme to identify itself for future reference.
        /// </summary>
        public IFeatureScheme Scheme { get; set; }

        /// <summary>
        /// Occurs when including legend items.
        /// </summary>
        /// <param name="item">Item that gets included.</param>
        protected override void OnInclude(IFeatureCategory item)
        {
            item.SetParentItem(Scheme.AppearsInLegend ? Scheme : Scheme.GetParentItem());
            base.OnInclude(item);
        }

        /// <summary>
        /// Occurs when excluding legend items.
        /// </summary>
        /// <param name="item">Item that gets excluded.</param>
        protected override void OnExclude(IFeatureCategory item)
        {
            item.SetParentItem(null);
        }
    }
}