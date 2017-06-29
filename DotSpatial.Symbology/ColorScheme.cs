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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 11:43:53 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    [Serializable]
    public class ColorScheme : Scheme, IColorScheme
    {
        #region Private Variables

        private ColorCategoryCollection _categories;
        private float _opacity;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorScheme
        /// </summary>
        public ColorScheme()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new instance of a color scheme using a predefined color scheme and the minimum and maximum specified
        /// from the raster itself
        /// </summary>
        /// <param name="schemeType">The predefined scheme to use</param>
        /// <param name="raster">The raster to obtain the minimum and maximum settings from</param>
        public ColorScheme(ColorSchemeType schemeType, IRaster raster)
        {
            Configure();
            ApplyScheme(schemeType, raster);
        }

        /// <summary>
        /// This creates a new scheme, applying the specified color scheme, and using the minimum and maximum values indicated.
        /// </summary>
        /// <param name="schemeType">The predefined color scheme</param>
        /// <param name="min">The minimum</param>
        /// <param name="max">The maximum</param>
        public ColorScheme(ColorSchemeType schemeType, double min, double max)
        {
            Configure();
            ApplyScheme(schemeType, min, max);
        }

        private void Configure()
        {
            _categories = new ColorCategoryCollection(this);
            _opacity = 1;
            EditorSettings = new RasterEditorSettings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the specified color scheme and uses the specified raster to define the
        /// minimum and maximum to use for the scheme.
        /// </summary>
        /// <param name="schemeType"></param>
        /// <param name="raster"></param>
        public void ApplyScheme(ColorSchemeType schemeType, IRaster raster)
        {
            double min, max;
            if (!raster.IsInRam)
            {
                GetValues(raster);
                min = Statistics.Minimum;
                max = Statistics.Maximum;
            }
            else
            {
                min = raster.Minimum;
                max = raster.Maximum;   
            }

            ApplyScheme(schemeType, min, max);
        }

        /// <summary>
        /// Creates the category using a random fill color
        /// </summary>
        /// <param name="fillColor">The base color to use for creating the category</param>
        /// <param name="size">For points this is the larger dimension, for lines this is the largest width</param>
        /// <returns>A new IFeatureCategory that matches the type of this scheme</returns>
        public override ICategory CreateNewCategory(Color fillColor, double size)
        {
            return new ColorCategory(null, null, fillColor, fillColor);
        }

        /// <summary>
        /// Creates the categories for this scheme based on statistics and values
        /// sampled from the specified raster.
        /// </summary>
        /// <param name="raster">The raster to use when creating categories</param>
        public void CreateCategories(IRaster raster)
        {
            GetValues(raster);
            CreateBreakCategories();
            OnItemChanged(this);
        }

        /// <summary>
        /// Gets the values from the raster.  If MaxSampleCount is less than the
        /// number of cells, then it randomly samples the raster with MaxSampleCount
        /// values.  Otherwise it gets all the values in the raster.
        /// </summary>
        /// <param name="raster">The raster to sample</param>
        public void GetValues(IRaster raster)
        {
            Values = raster.GetRandomValues(EditorSettings.MaxSampleCount);
            var keepers = Values.Where(val => val != raster.NoDataValue).ToList();
            Values = keepers;
            Statistics.Calculate(Values, raster.Minimum, raster.Maximum);
        }

        /// <summary>
        /// Applies the specified color scheme and uses the specified raster to define the
        /// minimum and maximum to use for the scheme.
        /// </summary>
        /// <param name="schemeType">ColorSchemeType</param>
        /// <param name="min">THe minimum value to use for the scheme</param>
        /// <param name="max">THe maximum value to use for the scheme</param>
        public void ApplyScheme(ColorSchemeType schemeType, double min, double max)
        {
            if (Categories == null)
            {
                Categories = new ColorCategoryCollection(this);
            }
            else
            {
                Categories.Clear();    
            }

            IColorCategory eqCat = null, low = null, high = null;
            if (min == max)
            {
                // Create one category
                eqCat = new ColorCategory(min, max) {Range = {MaxIsInclusive = true, MinIsInclusive = true}};
                eqCat.ApplyMinMax(EditorSettings);
                Categories.Add(eqCat);
            }
            else
            {
                // Create two categories
                low = new ColorCategory(min, (min + max) / 2) {Range = {MaxIsInclusive = true}};
                high = new ColorCategory((min + max) / 2, max) {Range = {MaxIsInclusive = true}};
                low.ApplyMinMax(EditorSettings);
                high.ApplyMinMax(EditorSettings);
                Categories.Add(low);
                Categories.Add(high);    
            }

            Color lowColor, midColor, highColor;
            int alpha = ByteRange(Convert.ToInt32(_opacity * 255F));
            switch (schemeType)
            {
                case ColorSchemeType.Summer_Mountains:
                    lowColor = Color.FromArgb(alpha, 10, 100, 10);
                    midColor = Color.FromArgb(alpha, 153, 125, 25);
                    highColor = Color.FromArgb(alpha, 255, 255, 255);
                    break;
                case ColorSchemeType.FallLeaves:
                    lowColor = Color.FromArgb(alpha, 10, 100, 10);
                    midColor = Color.FromArgb(alpha, 199, 130, 61);
                    highColor = Color.FromArgb(alpha, 241, 220, 133);
                    break;
                case ColorSchemeType.Desert:
                    lowColor = Color.FromArgb(alpha, 211, 206, 97);
                    midColor = Color.FromArgb(alpha, 139, 120, 112);
                    highColor = Color.FromArgb(alpha, 255, 255, 255);
                    break;
                case ColorSchemeType.Glaciers:
                    lowColor = Color.FromArgb(alpha, 105, 171, 224);
                    midColor = Color.FromArgb(alpha, 162, 234, 240);
                    highColor = Color.FromArgb(alpha, 255, 255, 255);
                    break;
                case ColorSchemeType.Meadow:
                    lowColor = Color.FromArgb(alpha, 68, 128, 71);
                    midColor = Color.FromArgb(alpha, 43, 91, 30);
                    highColor = Color.FromArgb(alpha, 167, 220, 168);
                    break;
                case ColorSchemeType.Valley_Fires:
                    lowColor = Color.FromArgb(alpha, 164, 0, 0);
                    midColor = Color.FromArgb(alpha, 255, 128, 64);
                    highColor = Color.FromArgb(alpha, 255, 255, 191);
                    break;
                case ColorSchemeType.DeadSea:
                    lowColor = Color.FromArgb(alpha, 51, 137, 208);
                    midColor = Color.FromArgb(alpha, 226, 227, 166);
                    highColor = Color.FromArgb(alpha, 151, 146, 117);
                    break;
                case ColorSchemeType.Highway:
                    lowColor = Color.FromArgb(alpha, 51, 137, 208);
                    midColor = Color.FromArgb(alpha, 214, 207, 124);
                    highColor = Color.FromArgb(alpha, 54, 152, 69);
                    break;
                default:
                    lowColor = midColor = highColor = Color.Transparent;
                    break;
            }

            if (eqCat != null)
            {
                eqCat.LowColor = eqCat.HighColor = lowColor;
            }
            else
            {
                Debug.Assert(low != null);
                Debug.Assert(high != null);

                low.LowColor = lowColor;
                low.HighColor = midColor;
                high.LowColor = midColor;
                high.HighColor = highColor;
            }

            OnItemChanged(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the floating point value for the opacity
        /// </summary>
        [Serialize("Opacity")]
        public float Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        /// <summary>
        /// Gets or sets the raster categories
        /// </summary>
        [Serialize("Categories")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorCategoryCollection Categories
        {
            get { return _categories; }
            set
            {
                if (_categories != null) _categories.Scheme = null;
                _categories = value;
                if (_categories != null) _categories.Scheme = this;
            }
        }

        /// <summary>
        /// Gets or sets the raster editor settings associated with this scheme.
        /// </summary>
        [Serialize("EditorSettings")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new RasterEditorSettings EditorSettings
        {
            get { return base.EditorSettings as RasterEditorSettings; }
            set { base.EditorSettings = value; }
        }

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new IFeatureCategory</returns>
        public override ICategory CreateRandomCategory()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return CreateNewCategory(CreateRandomColor(rnd), 20);
        }

        /// <summary>
        /// Occurs when setting the parent item and updates the parent item pointers
        /// </summary>
        /// <param name="value"></param>
        protected override void OnSetParentItem(ILegendItem value)
        {
            base.OnSetParentItem(value);
            _categories.UpdateItemParentPointers();
        }

        #endregion

        #region IColorScheme Members

        /// <summary>
        /// Draws the category in the specified location.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        public override void DrawCategory(int index, Graphics g, Rectangle bounds)
        {
            _categories[index].LegendSymbol_Painted(g, bounds);
        }

        /// <summary>
        /// Adds the specified category
        /// </summary>
        /// <param name="category"></param>
        public override void AddCategory(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            if (cc != null) _categories.Add(cc);
        }

        /// <summary>
        /// Attempts to decrease the index value of the specified category, and returns
        /// true if the move was successful.
        /// </summary>
        /// <param name="category">The category to decrease the index of</param>
        /// <returns></returns>
        public override bool DecreaseCategoryIndex(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            return cc != null && _categories.DecreaseIndex(cc);
        }

        /// <summary>
        /// Removes the specified category
        /// </summary>
        /// <param name="category"></param>
        public override void RemoveCategory(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            if (cc != null) _categories.Remove(cc);
        }

        /// <summary>
        /// Inserts the item at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="category"></param>
        public override void InsertCategory(int index, ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            if (cc != null) _categories.Insert(index, cc);
        }

        /// <summary>
        /// Attempts to increase the position of the specified category, and returns true
        /// if the index increase was successful.
        /// </summary>
        /// <param name="category">The category to increase the position of</param>
        /// <returns>Boolean, true if the item's position was increased</returns>
        public override bool IncreaseCategoryIndex(ICategory category)
        {
            IColorCategory cc = category as IColorCategory;
            return cc != null && _categories.IncreaseIndex(cc);
        }

        /// <summary>
        /// Suspends the change item event from firing as the list is being changed
        /// </summary>
        public override void SuspendEvents()
        {
            _categories.SuspendEvents();
        }

        /// <summary>
        /// Allows the ChangeItem event to get passed on when changes are made
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

        #endregion
    }
}