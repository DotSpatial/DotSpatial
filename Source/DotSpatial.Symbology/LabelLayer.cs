// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Layer used for labeling features.
    /// </summary>
    public class LabelLayer : Layer, ILabelLayer
    {
        #region Fields

        private IFeatureLayer _featureLayer;

        [Serialize("Symbology")]
        private ILabelScheme _symbology;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelLayer"/> class.
        /// </summary>
        public LabelLayer()
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">FeatureSet whose features get labeled by this label layer.</param>
        public LabelLayer(IFeatureSet inFeatureSet)
        {
            FeatureSet = inFeatureSet;
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelLayer"/> class.
        /// </summary>
        /// <param name="inFeatureLayer">Layer whose features get labeled by this label layer.</param>
        public LabelLayer(IFeatureLayer inFeatureLayer)
        {
            FeatureSet = inFeatureLayer.DataSet;
            _featureLayer = inFeatureLayer;
            Configure();
        }

        #endregion

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
        /// Gets or sets an optional layer to link this layer to. If this is specified, then drawing will
        /// be associated with this layer. This also updates the FeatureSet property.
        /// </summary>
        [ShallowCopy]
        public IFeatureLayer FeatureLayer
        {
            get
            {
                return _featureLayer;
            }

            set
            {
                _featureLayer = value;
                FeatureSet = _featureLayer.DataSet;
            }
        }

        /// <summary>
        /// Gets or sets the featureSet that defines the text for the labels on this layer.
        /// </summary>
        [ShallowCopy]
        public IFeatureSet FeatureSet { get; set; }

        /// <summary>
        /// Gets or sets the selection symbolizer from the first TextSymbol group.
        /// </summary>
        [ShallowCopy]
        public ILabelSymbolizer SelectionSymbolizer
        {
            get
            {
                if (_symbology?.Categories == null || _symbology.Categories.Count == 0) return null;
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
                if (_symbology?.Categories == null || _symbology.Categories.Count == 0) return null;
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

        /// <summary>
        /// Gets or sets the labeling scheme as a collection of categories.
        /// </summary>
        public ILabelScheme Symbology
        {
            get
            {
                return _symbology;
            }

            set
            {
                _symbology = value;
                CreateLabels(); // update the drawn state with the new categories
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the current selection, reverting the geometries back to their normal colors.
        /// </summary>
        public void ClearSelection()
        {
        }

        /// <summary>
        /// This builds the _drawnStates based on the current label scheme.
        /// </summary>
        public virtual void CreateLabels()
        {
            if (FeatureSet != null && FeatureSet.IndexMode)
            {
                CreateIndexedLabels();
                return;
            }

            DrawnStates = new Dictionary<IFeature, LabelDrawState>();
            if (FeatureSet == null || Symbology == null) return;

            foreach (ILabelCategory category in Symbology.Categories)
            {
                List<IFeature> features = !string.IsNullOrWhiteSpace(category.FilterExpression) ? FeatureSet.SelectByAttribute(category.FilterExpression) : FeatureSet.Features.ToList();

                foreach (IFeature feature in features)
                {
                    if (DrawnStates.ContainsKey(feature)) DrawnStates[feature] = new LabelDrawState(category);
                    else DrawnStates.Add(feature, new LabelDrawState(category));
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
        /// Removes the features in the given region.
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
        /// Fires the selection cleared event
        /// </summary>
        /// <param name="args">The arguments.</param>
        protected virtual void OnSelectionCleared(FeatureChangeArgs args)
        {
            SelectionCleared?.Invoke(this, args);
        }

        /// <summary>
        /// Fires the selection extended event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        protected virtual void OnSelectionExtended(FeatureChangeEnvelopeArgs args)
        {
            SelectionExtended?.Invoke(this, args);
        }

        private void Configure()
        {
            if (FeatureSet != null) MyExtent = FeatureSet.Extent.Copy();
            _symbology = new LabelScheme();
        }

        #endregion
    }
}