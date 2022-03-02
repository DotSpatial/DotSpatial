// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureCategoryCollection.
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