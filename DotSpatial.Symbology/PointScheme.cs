// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 3:52:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointScheme
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Serializable]
    public class PointScheme : FeatureScheme, IPointScheme
    {
        #region Private Variables

        private PointCategoryCollection _categories;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PointScheme with no categories added to the list yet.
        /// </summary>
        public PointScheme()
        {
            Configure();
            PointCategory def = new PointCategory();
            Categories.Add(def);
        }

        /// <summary>
        /// creates a new instance of the PointScheme, but assigns the specified symbolizer
        /// as the symbolizer to use on a single default category.
        /// </summary>
        /// <param name="extent">The geographic point size for the default will be 1/100th the specified extent</param>
        public PointScheme(IRectangle extent)
        {
            Configure();
            PointCategory def = new PointCategory(extent);
            Categories.Add(def);
        }

        private void Configure()
        {
            _categories = new PointCategoryCollection();
            OnIncludeCategories(_categories);
        }

        private void CategoriesItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the regular symbolizer for the specified cateogry to the specified graphics
        /// surface in the specified bounding rectangle.
        /// </summary>
        /// <param name="index">The integer index of the feature to draw.</param>
        /// <param name="g">The Graphics object to draw to</param>
        /// <param name="bounds">The rectangular bounds to draw in</param>
        public override void DrawCategory(int index, Graphics g, Rectangle bounds)
        {
            Categories[index].Symbolizer.Draw(g, bounds);
        }

        /// <summary>
        /// Adds a new scheme, assuming that the new scheme is the correct type.
        /// </summary>
        /// <param name="category">The category to add</param>
        public override void AddCategory(ICategory category)
        {
            IPointCategory pc = category as IPointCategory;
            if (pc != null) _categories.Add(pc);
        }

        /// <summary>
        /// Reduces the index value of the specified category by 1 by
        /// exchaning it with the category before it.  If there is no
        /// category before it, then this does nothing.
        /// </summary>
        /// <param name="category">The category to decrease the index of</param>
        public override bool DecreaseCategoryIndex(ICategory category)
        {
            IPointCategory pc = category as IPointCategory;
            return pc != null && Categories.DecreaseIndex(pc);
        }

        /// <summary>
        /// Re-orders the specified member by attempting to exchange it with the next higher
        /// index category.  If there is no higher index, this does nothing.
        /// </summary>
        /// <param name="category">The category to increase the index of</param>
        public override bool IncreaseCategoryIndex(ICategory category)
        {
            IPointCategory pc = category as IPointCategory;
            return pc != null && Categories.IncreaseIndex(pc);
        }

        /// <summary>
        /// Removes the specified category
        /// </summary>
        /// <param name="category">The category to insert</param>
        public override void RemoveCategory(ICategory category)
        {
            IPointCategory pc = category as IPointCategory;
            if (pc != null) _categories.Remove(pc);
        }

        /// <summary>
        /// Inserts the category at the specified index
        /// </summary>
        /// <param name="index">The integer index where the category should be inserted</param>
        /// <param name="category">The category to insert</param>
        public override void InsertCategory(int index, ICategory category)
        {
            IPointCategory pc = category as IPointCategory;
            if (pc != null) _categories.Insert(index, pc);
        }

        /// <summary>
        /// Suspends the category events
        /// </summary>
        public override void SuspendEvents()
        {
            _categories.SuspendEvents();
        }

        /// <summary>
        /// Resumes the category events
        /// </summary>
        public override void ResumeEvents()
        {
            _categories.ResumeEvents();
        }

        /// <summary>
        /// Clears the categories
        /// </summary>
        public override void ClearCategories()
        {
            _categories.Clear();
        }

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new IFeatureCategory</returns>
        public override IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            PointCategory result = new PointCategory();
            Color fillColor = CreateRandomColor();
            result.Symbolizer = new PointSymbolizer(fillColor, PointShape.Ellipse, 10);
            result.FilterExpression = filterExpression;
            result.LegendText = filterExpression;
            return result;
        }

        /// <summary>
        /// If possible, use the template to control the colors.  Otherwise, just use the default
        /// settings for creating "unbounded" colors.
        /// </summary>
        /// <param name="count">The integer count.</param>
        /// <returns>The List of colors</returns>
        protected override List<Color> GetDefaultColors(int count)
        {
            if (EditorSettings != null)
            {
                IPointSymbolizer ps = EditorSettings.TemplateSymbolizer as IPointSymbolizer;
                if (ps != null)
                {
                    List<Color> result = new List<Color>();
                    Color c = ps.GetFillColor();
                    for (int i = 0; i < count; i++)
                    {
                        result.Add(c);
                    }
                    return result;
                }
            }
            return base.GetDefaultColors(count);
        }

        /// <summary>
        /// Calculates the unique colors as a scheme
        /// </summary>
        /// <param name="fs">The featureset with the data Table definition</param>
        /// <param name="uniqueField">The unique field</param>
        public Hashtable GenerateUniqueColors(IFeatureSet fs, string uniqueField)
        {
            return GenerateUniqueColors(fs, uniqueField, color => new PointCategory(color, PointShape.Rectangle, 10));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolic categories as a valid IPointSchemeCategoryCollection.
        /// </summary>
        /// <remarks>
        /// [TypeConverter(typeof(CategoryCollectionConverter))]
        /// [Editor(typeof(PointCategoryCollectionEditor), typeof(UITypeEditor))]
        /// </remarks>
        [Description("Gets the list of categories.")]
        [Serialize("Categories")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointCategoryCollection Categories
        {
            get { return _categories; }
            set
            {
                OnExcludeCategories(_categories);
                _categories = value;
                OnIncludeCategories(_categories);
            }
        }

        /// <summary>
        /// Gets the point categories cast as FeatureCategories.  This is enumerable,
        /// but should be thought of as a copy of the original, not the original itself.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IFeatureCategory> GetCategories()
        {
            // Leave the cast so 3.5 can use this code.
            return _categories.Cast<IFeatureCategory>();
        }

        /// <summary>
        /// Creates the category using a random fill color
        /// </summary>
        /// <param name="fillColor">The base color to use for creating the category</param>
        /// <param name="size">The double size of the larger dimension of the point.</param>
        /// <returns>A new polygon category</returns>
        public override ICategory CreateNewCategory(Color fillColor, double size)
        {
            IPointSymbolizer ps = EditorSettings.TemplateSymbolizer.Copy() as IPointSymbolizer ??
                                  new PointSymbolizer(fillColor, PointShape.Ellipse, size);
            ps.SetFillColor(fillColor);
            Size2D oSize = ps.GetSize();
            double rat = size / Math.Max(oSize.Width, oSize.Height);
            ps.SetSize(new Size2D(rat * oSize.Width, rat * oSize.Height));
            return new PointCategory(ps);
        }

        /// <summary>
        /// Gets the number of categories in this scheme
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int NumCategories
        {
            get
            {
                if (_categories != null)
                {
                    return _categories.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Handle the event un-wiring and scheme update for the old categories
        /// </summary>
        /// <param name="categories">The category collection to update.</param>
        protected virtual void OnExcludeCategories(PointCategoryCollection categories)
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
        /// <param name="categories">The category collection to update</param>
        protected virtual void OnIncludeCategories(PointCategoryCollection categories)
        {
            if (categories == null) return;
            categories.Scheme = this;
            categories.ItemChanged += CategoriesItemChanged;
            categories.SelectFeatures += OnSelectFeatures;
            categories.DeselectFeatures += OnDeselectFeatures;
        }

        #endregion
    }
}