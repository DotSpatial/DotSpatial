// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is simply an alias to make things a tad (though not much) more understandable.
    /// </summary>
    public class LineCategoryCollection : ChangeEventList<ILineCategory>
    {
        #region Fields

        private ILineScheme _scheme;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> DeselectFeatures;

        /// <summary>
        /// Occurs when a category indicates that its filter expression should be used
        /// to select its members.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> SelectFeatures;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent line scheme.
        /// </summary>
        public ILineScheme Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                _scheme = value;
                UpdateItemParentPointers();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the default OnCopy behavior to remove the duplicte SelectFeatures event handler.
        /// </summary>
        /// <param name="copy">The copy.</param>
        protected override void OnCopy(CopyList<ILineCategory> copy)
        {
            LineCategoryCollection lcc = copy as LineCategoryCollection;
            if (lcc?.SelectFeatures != null)
            {
                foreach (var handler in lcc.SelectFeatures.GetInvocationList())
                {
                    lcc.SelectFeatures -= (EventHandler<ExpressionEventArgs>)handler;
                }
            }

            base.OnCopy(copy);
        }

        /// <summary>
        /// Instructs the parent layer to select features matching the specified expression.
        /// </summary>
        /// <param name="sender">The object sender where features were selected.</param>
        /// <param name="e">The event args describing which expression was used.</param>
        protected virtual void OnDeselectFeatures(object sender, ExpressionEventArgs e)
        {
            DeselectFeatures?.Invoke(sender, e);
        }

        /// <summary>
        /// Handles the exclude event. This removes the event handlers from the given item.
        /// </summary>
        /// <param name="item">Item that gets excluded.</param>
        protected override void OnExclude(ILineCategory item)
        {
            if (item == null) return;

            item.SelectFeatures -= OnSelectFeatures;
            item.DeselectFeatures -= OnDeselectFeatures;
            item.SetParentItem(null);
            base.OnExclude(item);
        }

        /// <summary>
        /// Ensures that newly added categories can navigate to higher legend items.
        /// </summary>
        /// <param name="item">The newly added legend item.</param>
        protected override void OnInclude(ILineCategory item)
        {
            if (_scheme == null) return;

            item.SelectFeatures += OnSelectFeatures;
            item.DeselectFeatures += OnDeselectFeatures;
            item.SetParentItem(_scheme.AppearsInLegend ? _scheme : _scheme.GetParentItem());
            base.OnInclude(item);
        }

        /// <summary>
        /// Instructs the parent layer to select features matching the specified expression.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnSelectFeatures(object sender, ExpressionEventArgs e)
        {
            SelectFeatures?.Invoke(sender, e);
        }

        /// <summary>
        /// Cycles through all the categories and resets the parent item.
        /// </summary>
        private void UpdateItemParentPointers()
        {
            foreach (var item in InnerList)
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

        #endregion
    }
}