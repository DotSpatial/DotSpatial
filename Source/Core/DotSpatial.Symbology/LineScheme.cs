// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineScheme.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class LineScheme : FeatureScheme, ILineScheme
    {
        #region Fields

        private LineCategoryCollection _categories;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineScheme"/> class.
        /// </summary>
        public LineScheme()
        {
            Configure();
            LineCategory def = new LineCategory();
            Categories.Add(def);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolic categories as a valid LineCategoryCollection.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(CategoryCollectionConverter))].
        /// </remarks>
        [Description("Gets the list of categories.")]
        [Serialize("Categories")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LineCategoryCollection Categories
        {
            get
            {
                return _categories;
            }

            set
            {
                OnExcludeCategories(_categories);
                _categories = value;
                OnIncludeCategories(_categories);
            }
        }

        /// <summary>
        /// Gets the number of categories in this scheme.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int NumCategories => _categories?.Count ?? 0;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new scheme, assuming that the new scheme is the correct type.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public override void AddCategory(ICategory category)
        {
            ILineCategory lc = category as ILineCategory;
            if (lc != null) _categories.Add(lc);
        }

        /// <summary>
        /// Clears the categories.
        /// </summary>
        public override void ClearCategories()
        {
            _categories.Clear();
        }

        /// <summary>
        /// Creates the category using a random fill color.
        /// </summary>
        /// <param name="fillColor">The base color to use for creating the category.</param>
        /// <param name="size">Creates the size.</param>
        /// <returns>A new polygon category.</returns>
        public override ICategory CreateNewCategory(Color fillColor, double size)
        {
            ILineSymbolizer ls = EditorSettings.TemplateSymbolizer.Copy() as ILineSymbolizer;
            if (ls != null)
            {
                ls.SetFillColor(fillColor);
                ls.SetWidth(size);
            }
            else
            {
                ls = new LineSymbolizer(fillColor, size);
            }

            return new LineCategory(ls);
        }

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <param name="filterExpression">Used as filterExpression and LegendText in the resulting category.</param>
        /// <returns>A new IFeatureCategory.</returns>
        public override IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            LineCategory result = new LineCategory();
            Color fillColor = CreateRandomColor();
            result.Symbolizer = new LineSymbolizer(fillColor, 2);
            result.FilterExpression = filterExpression;
            result.LegendText = filterExpression;
            return result;
        }

        /// <summary>
        /// Reduces the index value of the specified category by 1 by
        /// exchaning it with the category before it. If there is no
        /// category before it, then this does nothing.
        /// </summary>
        /// <param name="category">The category to decrease the index of.</param>
        /// <returns>True, if the index was decreased.</returns>
        public override bool DecreaseCategoryIndex(ICategory category)
        {
            ILineCategory pc = category as ILineCategory;
            return pc != null && Categories.DecreaseIndex(pc);
        }

        /// <summary>
        /// Draws the regular symbolizer for the specified cateogry to the specified graphics
        /// surface in the specified bounding rectangle.
        /// </summary>
        /// <param name="index">The integer index of the feature to draw.</param>
        /// <param name="g">The Graphics object to draw to.</param>
        /// <param name="bounds">The rectangular bounds to draw in.</param>
        public override void DrawCategory(int index, Graphics g, Rectangle bounds)
        {
            Categories[index].Symbolizer.Draw(g, bounds);
        }

        /// <summary>
        /// Calculates the unique colors as a scheme.
        /// </summary>
        /// <param name="fs">The featureset with the data Table definition.</param>
        /// <param name="uniqueField">The unique field.</param>
        /// <returns>A hashtable with the generated unique colors.</returns>
        public Hashtable GenerateUniqueColors(IFeatureSet fs, string uniqueField)
        {
            return GenerateUniqueColors(fs, uniqueField, color => new LineCategory(color, 1));
        }

        /// <summary>
        /// Returns the line scheme as a feature categories collection.
        /// </summary>
        /// <returns>The categories.</returns>
        public override IEnumerable<IFeatureCategory> GetCategories()
        {
            // Leave the Cast so that .Net 3.5 can use this code.
            return _categories;
        }

        /// <summary>
        /// Re-orders the specified member by attempting to exchange it with the next higher
        /// index category. If there is no higher index, this does nothing.
        /// </summary>
        /// <param name="category">The category to increase the index of.</param>
        /// <returns>True, if the index was increased.</returns>
        public override bool IncreaseCategoryIndex(ICategory category)
        {
            ILineCategory pc = category as ILineCategory;
            return pc != null && Categories.IncreaseIndex(pc);
        }

        /// <summary>
        /// Inserts the category at the specified index.
        /// </summary>
        /// <param name="index">The integer index where the category should be inserted.</param>
        /// <param name="category">The category to insert.</param>
        public override void InsertCategory(int index, ICategory category)
        {
            ILineCategory lc = category as ILineCategory;
            if (lc != null) _categories.Insert(index, lc);
        }

        /// <summary>
        /// Removes the specified category.
        /// </summary>
        /// <param name="category">The category to insert.</param>
        public override void RemoveCategory(ICategory category)
        {
            ILineCategory lc = category as ILineCategory;
            if (lc != null) _categories.Remove(lc);
        }

        /// <summary>
        /// Resumes the category events.
        /// </summary>
        public override void ResumeEvents()
        {
            _categories.ResumeEvents();
        }

        /// <summary>
        /// Suspends the category events.
        /// </summary>
        public override void SuspendEvents()
        {
            _categories.SuspendEvents();
        }

        /// <summary>
        /// If possible, use the template to control the colors. Otherwise, just use the default
        /// settings for creating "unbounded" colors.
        /// </summary>
        /// <param name="count">The integer count.</param>
        /// <returns>The List of colors.</returns>
        protected override List<Color> GetDefaultColors(int count)
        {
            ILineSymbolizer ls = EditorSettings?.TemplateSymbolizer as ILineSymbolizer;
            if (ls != null)
            {
                List<Color> result = new List<Color>();
                Color c = ls.GetFillColor();
                for (int i = 0; i < count; i++)
                {
                    result.Add(c);
                }

                return result;
            }

            return base.GetDefaultColors(count);
        }

        /// <summary>
        /// Handle the event un-wiring and scheme update for the old categories.
        /// </summary>
        /// <param name="categories">The category collection to update.</param>
        protected virtual void OnExcludeCategories(LineCategoryCollection categories)
        {
            if (categories == null) return;

            categories.Scheme = null;
            categories.ItemChanged -= CategoriesItemChanged;
            categories.SelectFeatures -= OnSelectFeatures;
            categories.DeselectFeatures -= OnDeselectFeatures;
        }

        /// <summary>
        /// Handle the event wiring and scheme update for the new categories.
        /// </summary>
        /// <param name="categories">The category collection to update.</param>
        protected virtual void OnIncludeCategories(LineCategoryCollection categories)
        {
            if (categories == null) return;

            categories.Scheme = this;
            categories.ItemChanged += CategoriesItemChanged;
            categories.SelectFeatures += OnSelectFeatures;
            categories.DeselectFeatures += OnDeselectFeatures;
        }

        private void CategoriesItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        private void Configure()
        {
            _categories = new LineCategoryCollection();
            InitializeCategories();
        }

        private void InitializeCategories()
        {
            _categories.Scheme = this;
            _categories.ItemChanged += CategoriesItemChanged;
        }

        #endregion
    }
}