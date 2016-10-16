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
    /// FeatureSchemeCategoryCollection
    /// </summary>
    public class FeatureCategoryCollection : ChangeEventList<IFeatureCategory>
    {
        private IFeatureScheme _scheme;

        /// <summary>
        /// Creates a new instance of the FeatureSchemeCategoryCollection
        /// </summary>
        public FeatureCategoryCollection()
        {
            Add(new FeatureCategory()); // default grouping
        }

        /// <summary>
        /// Optionally allows the scheme to identify itself for future reference.
        /// </summary>
        public IFeatureScheme Scheme
        {
            get { return _scheme; }
            set { _scheme = value; }
        }

        /// <summary>
        /// Occurs when including legend items
        /// </summary>
        /// <param name="item"></param>
        protected override void OnInclude(IFeatureCategory item)
        {
            item.SetParentItem(_scheme.AppearsInLegend ? _scheme : _scheme.GetParentItem());
            base.OnInclude(item);
        }

        /// <summary>
        /// Occurs when excluding legend items
        /// </summary>
        /// <param name="item"></param>
        protected override void OnExclude(IFeatureCategory item)
        {
            item.SetParentItem(null);
        }
    }
}