// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 8:35:34 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    public class LabelLayer : Layer, ILabelLayer
    {
        #region Events

        /// <summary>
        /// Occurs after the selection has been cleared
        /// </summary>
        public event EventHandler<FeatureChangeArgs> SelectionCleared;

        /// <summary>
        /// Occurs after the selection is updated by the addition of new members
        /// </summary>
        public event EventHandler<FeatureChangeEnvelopeArgs> SelectionExtended;

        #endregion

        #region Private Variables

        private IFeatureLayer _featureLayer;

        [Serialize("Symbology")]
        private ILabelScheme _symbology;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LabelLayer
        /// </summary>
        public LabelLayer()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new layer that uses the attributes from the given featureSet
        /// </summary>
        /// <param name="inFeatureSet"></param>
        public LabelLayer(IFeatureSet inFeatureSet)
        {
            FeatureSet = inFeatureSet;
            Configure();
        }

        /// <summary>
        /// Creates a new label layer based on the features in the
        /// </summary>
        /// <param name="inFeatureLayer"></param>
        public LabelLayer(IFeatureLayer inFeatureLayer)
        {
            FeatureSet = inFeatureLayer.DataSet;
            _featureLayer = inFeatureLayer;
            Configure();
        }

        private void Configure()
        {
            if (FeatureSet != null) MyExtent = FeatureSet.Extent.Copy();
            _symbology = new LabelScheme();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the current selection, reverting the geometries back to their
        /// normal colors.
        /// </summary>
        public void ClearSelection()
        {
        }

        /// <summary>
        /// This builds the _drawnStates based on the current label scheme.
        /// </summary>
        public virtual void CreateLabels()
        {
            if (FeatureSet != null)
            {
                if (FeatureSet.IndexMode)
                {
                    CreateIndexedLabels();
                    return;
                }
            }
            DrawnStates = new Dictionary<IFeature, LabelDrawState>();
            if (FeatureSet == null) return;
            // DataTable dt = _featureSet.DataTable; // if working correctly, this should auto-populate
            if (Symbology == null) return;

            foreach (ILabelCategory category in Symbology.Categories)
            {
                List<IFeature> features;
                if (!string.IsNullOrWhiteSpace(category.FilterExpression))
                    features = FeatureSet.SelectByAttribute(category.FilterExpression);
                else
                    features = FeatureSet.Features.ToList();

                foreach (IFeature feature in features)
                {
                    if (DrawnStates.ContainsKey(feature))
                        DrawnStates[feature] = new LabelDrawState(category);
                    else
                        DrawnStates.Add(feature, new LabelDrawState(category));
                }
            }
        }

        /// <summary>
        /// Highlights the values from a specified region. This will not unselect any members,
        /// so if you want to select a new region instead of an old one, first use ClearSelection.
        /// This is the default selection that only tests the anchor point, not the entire label.
        /// </summary>
        /// <param name="region">An Envelope showing a 3D selection box for intersection testing.</param>
        /// <returns>True if any members were added to the current selection.</returns>
        public bool Select(Extent region)
        {
            List<IFeature> features = FeatureSet.Select(region);
            if (features.Count == 0) return false;
            foreach (IFeature feature in features)
            {
                DrawnStates[feature].Selected = true;
            }
            return true;
        }

        /// <summary>
        /// This builds the _drawnStates based on the current label scheme.
        /// </summary>
        protected void CreateIndexedLabels()
        {
            if (FeatureSet == null) return;
            FastDrawnStates = new FastLabelDrawnState[FeatureSet.ShapeIndices.Count];

            // DataTable dt = _featureSet.DataTable; // if working correctly, this should auto-populate
            if (Symbology == null) return;

            foreach (ILabelCategory category in Symbology.Categories)
            {
                if (category.FilterExpression != null)
                {
                    List<int> features = FeatureSet.SelectIndexByAttribute(category.FilterExpression);
                    foreach (int feature in features)
                    {
                        FastDrawnStates[feature] = new FastLabelDrawnState(category);
                    }
                }
                else
                {
                    for (int i = 0; i < FastDrawnStates.Length; i++)
                    {
                        FastDrawnStates[i] = new FastLabelDrawnState(category);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the features in the given region
        /// </summary>
        /// <param name="region">the geographic region to remove the feature from the selection on this layer</param>
        /// <returns>Boolean true if any features were removed from the selection.</returns>
        public bool UnSelect(Extent region)
        {
            List<IFeature> features = FeatureSet.Select(region);
            if (features.Count == 0) return false;
            foreach (IFeature feature in features)
            {
                DrawnStates[feature].Selected = false;
            }
            return true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dictionary that quickly identifies the category for
        /// each label.
        /// </summary>
        [ShallowCopy]
        public Dictionary<IFeature, LabelDrawState> DrawnStates { get; set; }

        /// <summary>
        /// Gets or sets the indexed collection of drawn states
        /// </summary>
        public FastLabelDrawnState[] FastDrawnStates { get; set; }

        /// <summary>
        /// Gets or sets the featureSet that defines the text for the labels on this layer.
        /// </summary>
        [ShallowCopy]
        public IFeatureSet FeatureSet { get; set; }

        /// <summary>
        /// Gets or sets an optional layer to link this layer to. If this is specified, then drawing will
        /// be associated with this layer. This also updates the FeatureSet property.
        /// </summary>
        [ShallowCopy]
        public IFeatureLayer FeatureLayer
        {
            get { return _featureLayer; }
            set
            {
                _featureLayer = value;
                FeatureSet = _featureLayer.DataSet;
            }
        }

        /// <summary>
        /// Gets or sets the labeling scheme as a collection of categories.
        /// </summary>
        public ILabelScheme Symbology
        {
            get { return _symbology; }
            set
            {
                _symbology = value;
                CreateLabels(); // update the drawn state with the new categories
            }
        }

        /// <summary>
        /// Gets or sets the selection symbolizer from the first TextSymbol group.
        /// </summary>
        [ShallowCopy]
        public ILabelSymbolizer SelectionSymbolizer
        {
            get
            {
                if (_symbology == null) return null;
                if (_symbology.Categories == null) return null;
                if (_symbology.Categories.Count == 0) return null;
                return _symbology.Categories[0].SelectionSymbolizer;
            }
            set
            {
                if (_symbology == null) _symbology = new LabelScheme();
                if (_symbology.Categories == null) _symbology.Categories = new BaseList<ILabelCategory>();
                if (_symbology.Categories.Count == 0) _symbology.Categories.Add(new LabelCategory());
                _symbology.Categories[0].SelectionSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the regular symbolizer from the first TextSymbol group.
        /// </summary>
        [ShallowCopy]
        public ILabelSymbolizer Symbolizer
        {
            get
            {
                if (_symbology == null) return null;
                if (_symbology.Categories == null) return null;
                if (_symbology.Categories.Count == 0) return null;
                return _symbology.Categories[0].Symbolizer;
            }
            set
            {
                if (_symbology == null) _symbology = new LabelScheme();
                if (_symbology.Categories == null) _symbology.Categories = new BaseList<ILabelCategory>();
                if (_symbology.Categories.Count == 0) _symbology.Categories.Add(new LabelCategory());
                _symbology.Categories[0].Symbolizer = value;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the selection cleared event
        /// </summary>
        protected virtual void OnSelectionCleared(FeatureChangeArgs args)
        {
            if (SelectionCleared != null) SelectionCleared(this, args);
        }

        /// <summary>
        /// Fires the selection extended event
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnSelectionExtended(FeatureChangeEnvelopeArgs args)
        {
            if (SelectionExtended != null) SelectionExtended(this, args);
        }

        #endregion
    }
}