// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 3:54:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is simply an alias to make things a tad (though not much) more understandable
    /// </summary>
    public class PointCategoryCollection : ChangeEventList<IPointCategory>
    {
        private IPointScheme _scheme;

        /// <summary>
        /// Gets or sets the parent scheme.
        /// </summary>
        public IPointScheme Scheme
        {
            get { return _scheme; }
            set
            {
                _scheme = value;
                UpdateItemParentPointers();
            }
        }

        /// <summary>
        /// Occurs when a category indicates that its filter expression should be used
        /// to select its members.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> SelectFeatures;

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> DeselectFeatures;

        /// <summary>
        /// Instructs the parent layer to select features matching the specified expression.
        /// </summary>
        /// <param name="sender">The object sender where features were selected.</param>
        /// <param name="e">The event args describing which expression was used.</param>
        protected virtual void OnSelectFeatures(object sender, ExpressionEventArgs e)
        {
            if (SelectFeatures != null) SelectFeatures(sender, e);
        }

        /// <summary>
        /// Instructs the parent layer to select features matching the specified expression.
        /// </summary>
        /// <param name="sender">The object sender where features were selected.</param>
        /// <param name="e">The event args describing which expression was used.</param>
        protected virtual void OnDeselectFeatures(object sender, ExpressionEventArgs e)
        {
            if (DeselectFeatures != null) DeselectFeatures(sender, e);
        }

        /// <summary>
        /// Ensures that newly added categories can navigate to higher legend items.
        /// </summary>
        /// <param name="item">The newly added legend item.</param>
        protected override void OnInclude(IPointCategory item)
        {
            if (_scheme == null) return;
            item.SelectFeatures += OnSelectFeatures;
            item.DeselectFeatures += OnDeselectFeatures;
            item.SetParentItem(_scheme.AppearsInLegend ? _scheme : _scheme.GetParentItem());
            base.OnInclude(item);
        }

        /// <summary>
        /// Ensures that items are disconnected from parent items when removed from the collection.
        /// </summary>
        /// <param name="item"></param>
        protected override void OnExclude(IPointCategory item)
        {
            if (item == null) return;
            item.SelectFeatures -= OnSelectFeatures;
            item.DeselectFeatures -= OnDeselectFeatures;
            item.SetParentItem(null);
            base.OnExclude(item);
        }

        /// <summary>
        /// Overrides the OnCopy method to remove the SelectFeatures handler on the copy
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(CopyList<IPointCategory> copy)
        {
            PointCategoryCollection pcc = copy as PointCategoryCollection;
            if (pcc != null && pcc.SelectFeatures != null)
            {
                foreach (var handler in pcc.SelectFeatures.GetInvocationList())
                {
                    pcc.SelectFeatures -= (EventHandler<ExpressionEventArgs>)handler;
                }
            }
            base.OnCopy(copy);
        }

        /// <summary>
        /// Cycles through all the categories and resets the parent item.
        /// </summary>
        private void UpdateItemParentPointers()
        {
            foreach (IPointCategory item in InnerList)
            {
                if (_scheme == null)
                {
                    item.SetParentItem(null);
                }
                else
                {
                    item.SetParentItem(_scheme.AppearsInLegend ? _scheme : _scheme.GetParentItem());
                }
            }
        }
    }
}