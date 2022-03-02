// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is simply an alias to make things a tad (though not much) more understandable.
    /// </summary>
    public class PolygonCategoryCollection : ChangeEventList<IPolygonCategory>
    {
        #region Fields

        private IPolygonScheme _scheme;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCategoryCollection"/> class.
        /// </summary>
        public PolygonCategoryCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCategoryCollection"/> class with the supplied scheme.
        /// </summary>
        /// <param name="scheme">The scheme to use ofr this collection.</param>
        public PolygonCategoryCollection(IPolygonScheme scheme)
        {
            _scheme = scheme;
        }

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
        /// Gets or sets the parent scheme for this collection.
        /// </summary>
        [Serialize("Scheme", ConstructorArgumentIndex = 0)]
        [ShallowCopy]
        public IPolygonScheme Scheme
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
        /// Overrides the copy behavior to remove the now unnecessary SelectFeatures event handler.
        /// </summary>
        /// <param name="copy">The copy.</param>
        protected override void OnCopy(CopyList<IPolygonCategory> copy)
        {
            PolygonCategoryCollection pcc = copy as PolygonCategoryCollection;
            if (pcc?.SelectFeatures != null)
            {
                foreach (var handler in pcc.SelectFeatures.GetInvocationList())
                {
                    pcc.SelectFeatures -= (EventHandler<ExpressionEventArgs>)handler;
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
        /// Changes the parent item of the specified category.
        /// </summary>
        /// <param name="item">The item that gets excluded.</param>
        protected override void OnExclude(IPolygonCategory item)
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
        protected override void OnInclude(IPolygonCategory item)
        {
            if (item != null)
            {
                item.SelectFeatures += OnSelectFeatures;
                item.DeselectFeatures += OnDeselectFeatures;
                if (_scheme != null) item.SetParentItem(_scheme.AppearsInLegend ? _scheme : _scheme.GetParentItem());
            }

            base.OnInclude(item);
        }

        /// <summary>
        /// Instructs the parent layer to select features matching the specified expression.
        /// </summary>
        /// <param name="sender">The object sender where features were selected.</param>
        /// <param name="e">The event args describing the expression used for selection.</param>
        protected virtual void OnSelectFeatures(object sender, ExpressionEventArgs e)
        {
            SelectFeatures?.Invoke(sender, e);
        }

        private void UpdateItemParentPointers()
        {
            foreach (IPolygonCategory item in InnerList)
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