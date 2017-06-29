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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 1:54:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A Scheme category does not reference individual members or indices, but simply describes a symbolic representation that
    /// can be used by an actual category.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Serializable]
    public class FeatureCategory : Category, IFeatureCategory
    {
        #region IFeatureCategory Members

        /// <summary>
        /// Occurs when the select features context menu is clicked.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> SelectFeatures;

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        public event EventHandler<ExpressionEventArgs> DeselectFeatures;

        #endregion

        #region Private Variables

        private IFeatureSymbolizer _featureSymbolizer;
        private string _filterExpression;
        private SymbologyMenuItem _mnuRemoveMe;
        private SymbologyMenuItem _mnuSelectFeatures;
        private SymbologyMenuItem _mnuDeselectFeatures;
        private IFeatureSymbolizer _selectionSymbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureSchemeCategory
        /// </summary>
        public FeatureCategory()
        {
            base.LegendSymbolMode = SymbolMode.Symbol;
            LegendType = LegendType.Symbol;
            CreateContextMenuItems();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the minimum and maximum in order to create the filter expression.  This will also
        /// count the members that match the specified criteria.
        /// </summary>
        public override void ApplyMinMax(EditorSettings settings)
        {
            base.ApplyMinMax(settings);
            FeatureEditorSettings fs = settings as FeatureEditorSettings;
            if (fs == null) return;
            string field = "[" + fs.FieldName.ToUpper() + "]";
            if (!string.IsNullOrEmpty(fs.NormField)) field += "/[" + fs.NormField.ToUpper() + "]";
            IntervalSnapMethod method = settings.IntervalSnapMethod;
            int digits = settings.IntervalRoundingDigits;
            LegendText = Range.ToString(method, digits);
            _filterExpression = Range.ToExpression(field);
        }

        /// <summary>
        /// Controls what happens in the legend when this item is instructed to draw a symbol.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            _featureSymbolizer.Draw(g, box);
        }

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        public bool IsWithinLegendSelection()
        {
            if (IsSelected) return true;
            ILayer lyr = GetParentItem() as ILayer;
            while (lyr != null)
            {
                if (lyr.IsSelected) return true;
                lyr = lyr.GetParentItem() as ILayer;
            }
            return false;
        }

        /// <summary>
        /// This gets a single color that attempts to represent the specified
        /// category.  For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern.  If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        public virtual Color GetColor()
        {
            return Color.Gray;
        }

        /// <summary>
        /// This applies the color to the top symbol stroke or pattern.
        /// </summary>
        /// <param name="color">The Color to apply</param>
        public virtual void SetColor(Color color)
        {
        }

        /// <summary>
        /// In some cases, it is useful to simply be able to show an approximation of the actual expression.
        /// This also removes brackets from the field names to make it slightly cleaner.
        /// </summary>
        public void DisplayExpression()
        {
            string exp = _filterExpression;
            if (exp != null)
            {
                exp = exp.Replace("[", string.Empty);
                exp = exp.Replace("]", string.Empty);
            }
            LegendText = exp;
        }

        /// <summary>
        /// Makes it so that if there are any pre-existing listeners to the SelectFeatuers
        /// event when creating a clone of this object, those listeners are removed.
        /// They should be added correctly when the cloned item is added to the collection,
        /// after being cloned.
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            //todo: do the same for DeselectFeatures event...
            FeatureCategory cat = copy as FeatureCategory;
            if (cat != null && cat.SelectFeatures != null)
            {
                foreach (var handler in cat.SelectFeatures.GetInvocationList())
                {
                    cat.SelectFeatures -= (EventHandler<ExpressionEventArgs>)handler;
                }
            }
            if (cat != null)
            {
                cat.CreateContextMenuItems();
            }
            base.OnCopy(copy);
        }

        /// <summary>
        /// Forces the creation of an entirely new context menu list.  That way, we are not
        /// pointing to an event handler in the previous parent.
        /// </summary>
        public void CreateContextMenuItems()
        {
            base.ContextMenuItems = new List<SymbologyMenuItem>();
            _mnuRemoveMe = new SymbologyMenuItem("Remove Category", RemoveCategoryClicked);
            _mnuSelectFeatures = new SymbologyMenuItem("Select Features", SelectFeaturesClicked);
            _mnuDeselectFeatures = new SymbologyMenuItem("Deselect Features", DeselectFeaturesClicked);
            base.ContextMenuItems.Add(_mnuRemoveMe);
            base.ContextMenuItems.Add(_mnuSelectFeatures);
            base.ContextMenuItems.Add(_mnuDeselectFeatures);
        }

        private void RemoveCategoryClicked(object sender, EventArgs e)
        {
            OnRemoveItem();
        }

        private void SelectFeaturesClicked(object sender, EventArgs e)
        {
            OnSelectFeatures();
        }
        private void DeselectFeaturesClicked(object sender, EventArgs e)
        {
            OnDeselectFeatures();
        }
        /// <summary>
        /// Fires the SelectFeatures event
        /// </summary>
        protected virtual void OnSelectFeatures()
        {
            if (SelectFeatures != null)
            {
                SelectFeatures(this, new ExpressionEventArgs(_filterExpression));
            }
        }
        #region OnDeselectFeatures
        /// <summary>
        /// Triggers the DeselectFeatures event.
        /// </summary>
        public virtual void OnDeselectFeatures()
        {
            if (DeselectFeatures != null)
                DeselectFeatures(this, new ExpressionEventArgs(_filterExpression));
        }
        #endregion
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer used for this category.
        /// </summary>
        [Serialize("Symbolizer")]
        public IFeatureSymbolizer Symbolizer
        {
            get { return _featureSymbolizer; }
            set
            {
                if (_featureSymbolizer != null) _featureSymbolizer.ItemChanged -= SymbolizerItemChanged;
                if (_featureSymbolizer != value && value != null)
                {
                    value.ItemChanged += SymbolizerItemChanged;
                    value.SetParentItem(this);
                }
                _featureSymbolizer = value;
                OnItemChanged();
            }
        }

        /// <summary>
        /// Gets or sets the symbolizer used for this category
        /// </summary>
        [Serialize("SelectionSymbolizer")]
        public IFeatureSymbolizer SelectionSymbolizer
        {
            get { return _selectionSymbolizer; }
            set
            {
                if (_selectionSymbolizer != null) _selectionSymbolizer.ItemChanged -= SymbolizerItemChanged;
                if (_selectionSymbolizer != value && value != null)
                {
                    value.ItemChanged += SymbolizerItemChanged;
                    value.SetParentItem(this);
                }
                _selectionSymbolizer = value;
                OnItemChanged();
            }
        }

        /// <summary>
        /// Gets or set the filter expression that is used to add members to generate a category based on this scheme.
        /// </summary>
        /// <remarks>[Editor(typeof(ExpressionEditor), typeof(UITypeEditor))]</remarks>
        [Description("Gets or set the filter expression that is used to add members to generate a category based on this scheme.")]
        [Serialize("FilterExpression")]
        public string FilterExpression
        {
            get
            {
                return _filterExpression;
            }
            set { _filterExpression = value; }
        }

        private void SymbolizerItemChanged(object sender, EventArgs e)
        {
            OnItemChanged();
        }

        #endregion
    }
}