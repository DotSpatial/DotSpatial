// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
// The Original Code is DotSpatial.dll for the DotSpatial project
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Topology;
using Msg = DotSpatial.Symbology.SymbologyMessageStrings;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is should not be instantiated because it cannot in itself perform the necessary functions.
    /// Instead, most of the specified functionality must be implemented in the more specific classes.
    /// This is also why there is no direct constructor for this class.  You can use the static
    /// "FromFile" or "FromFeatureLayer" to create FeatureLayers from a file.
    /// </summary>
    public abstract class FeatureLayer : Layer, IFeatureLayer
    {
        #region Events

        /// <summary>
        /// Occurs after a new symbolic scheme has been applied to the layer.
        /// </summary>
        public event EventHandler SchemeApplied;

        /// <summary>
        /// Occurs after a snapshot is taken, and contains an event argument with the bitmap
        /// to be displayed.
        /// </summary>
        public event EventHandler<SnapShotEventArgs> SnapShotTaken;

        /// <summary>
        /// Occurs before the attribute Table is displayed, also allowing this event to be handled.
        /// </summary>
        public event HandledEventHandler ViewAttributes;

        /// <summary>
        /// Occurs before the label setup dialog is displayed, allowing the event to be handled.
        /// </summary>
        public event HandledEventHandler LabelSetup;

        #endregion

        #region Variables

        /// <summary>
        /// The _category extents.
        /// </summary>
        private IDictionary<IFeatureCategory, Extent> _categoryExtents;

        /// <summary>
        /// The _chunk size.
        /// </summary>
        private int _chunkSize;

        /// <summary>
        /// The _drawing bounds.
        /// </summary>
        private Rectangle _drawingBounds;

        /// <summary>
        /// The _drawing filter.
        /// </summary>
        private IDrawingFilter _drawingFilter;

        /// <summary>
        /// The _drawn states.
        /// </summary>
        private FastDrawnState[] _drawnStates;

        /// <summary>
        /// The _drawn states needed.
        /// </summary>
        private bool _drawnStatesNeeded;

        /// <summary>
        /// The _edit mode.
        /// </summary>
        private bool _editMode;

        /// <summary>
        /// The _feature symbolizer.
        /// </summary>
        private IFeatureSymbolizer _featureSymbolizer;

        /// <summary>
        /// The _label layer.
        /// </summary>
        private ILabelLayer _labelLayer;

        /// <summary>
        /// The _name.
        /// </summary>
        private string _name;

        /// <summary>
        /// The _scheme.
        /// </summary>
        private IFeatureScheme _scheme;

        /// <summary>
        /// The _selection.
        /// </summary>
        private ISelection _selection;

        /// <summary>
        /// The _selection feature symbolizer.
        /// </summary>
        private IFeatureSymbolizer _selectionFeatureSymbolizer;

        /// <summary>
        /// The _show labels.
        /// </summary>
        private bool _showLabels;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureLayer"/> class.
        /// Constructor
        /// </summary>
        /// <param name="featureSet">
        /// The data bearing layer to apply new drawing characteristics to
        /// </param>
        protected FeatureLayer(IFeatureSet featureSet)
        {
            Configure(featureSet);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureLayer"/> class.
        /// Constructor
        /// </summary>
        /// <param name="featureSet">
        /// The data bearing layer to apply new drawing characteristics to
        /// </param>
        /// <param name="container">
        /// The container this layer should be added to
        /// </param>
        protected FeatureLayer(IFeatureSet featureSet, ICollection<ILayer> container)
            : base(container)
        {
            Configure(featureSet);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureLayer"/> class.
        /// Constructor
        /// </summary>
        /// <param name="featureSet">
        /// The data bearing layer to apply new drawing characteristics to
        /// </param>
        /// <param name="container">
        /// The container this layer should be added to
        /// </param>
        /// <param name="progressHandler">
        /// A progress handler for receiving status messages
        /// </param>
        protected FeatureLayer(IFeatureSet featureSet, ICollection<ILayer> container, IProgressHandler progressHandler)
            : base(container, progressHandler)
        {
            Configure(featureSet);
        }

        /// <summary>
        /// Gets or sets the Boolean flag that controls whether the DrawnStates are needed.  If nothing is selected,
        /// and there is only one category, and there is no filter expression on that category, then this should be false.
        /// </summary>
        public bool DrawnStatesNeeded
        {
            get { return _drawnStatesNeeded; }
            set
            {
                if (_drawnStatesNeeded == false && value)
                {
                    AssignFastDrawnStates();
                }

                _drawnStatesNeeded = value;
            }
        }

        /// <summary>
        /// Zooms to the envelope of the selected features.
        /// </summary>
        public void ZoomToSelectedFeatures()
        {
            ZoomToSelectedFeatures(2, 2);
        }

        /// <summary>
        /// Zooms to the envelope of the selected features, adding a border of the size specified.
        /// </summary>
        public void ZoomToSelectedFeatures(double distanceX, double distanceY)
        {
            if (_selection.Count == 0)
            {
                return;
            }

            IEnvelope env = _selection.Envelope;
            if (env.Width == 0 || env.Height == 0)
            {
                env.ExpandBy(distanceX, distanceY);
            }

            OnZoomToLayer(env);
        }

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="featureSet">
        /// The feature set.
        /// </param>
        private void Configure(IFeatureSet featureSet)
        {
            _categoryExtents = new Dictionary<IFeatureCategory, Extent>();
            _drawingBounds = new Rectangle(-32000, -32000, 64000, 64000);
            DataSet = featureSet;
            LegendText = featureSet.Name;
            _name = featureSet.Name;
            SymbologyMenuItem label = new SymbologyMenuItem(Msg.FeatureLayer_Labeling);
            label.MenuItems.Add(new SymbologyMenuItem(Msg.FeatureLayer_Label_Setup, SymbologyImages.Label, LabelSetupClick));
            label.MenuItems.Add(new SymbologyMenuItem(Msg.SetDynamicVisibilityScale, SymbologyImages.ZoomScale,
                                                      LabelExtentsClick));
            ContextMenuItems.Insert(4, label);
            SymbologyMenuItem selection = new SymbologyMenuItem(Msg.FeatureLayer_Selection, SymbologyImages.select, null);
            ContextMenuItems.Insert(5, selection);
            selection.MenuItems.Add(new SymbologyMenuItem(Msg.FeatureLayer_Zoom_To_Selected, SymbologyImages.ZoomInMap,
                                                          SelectionZoomClick));
            selection.MenuItems.Add(new SymbologyMenuItem(Msg.FeatureLayer_Create_Layer_From_Selected_Features, SymbologyImages.Copy,
                                                          SelectionToLayerClick));
            selection.MenuItems.Add(new SymbologyMenuItem(Msg.FeatureLayer_SelectAll, SymbologyImages.select_all, SelectAllClick));
            selection.MenuItems.Add(new SymbologyMenuItem(Msg.FeatureLayer_UnselectAll, SymbologyImages.deselect_16x16, UnselectAllClick));

            ContextMenuItems.Add(new SymbologyMenuItem(Msg.FeatureLayer_Join_Excel_File, SymbologyImages.redbluearrows,
                                                       JoinExcel));
            if (!featureSet.IndexMode)
            {
                _editMode = true;
            }

            // Categories and selections

            // these are like a stored procedure that only care about the selection and no other drawing characteristics.
            if (_editMode)
            {
                _drawingFilter = new DrawingFilter(DataSet.Features, _scheme, 5000);
                _selection = new Selection(featureSet, _drawingFilter);
            }
            else
            {
                _selection = new IndexSelection(this);
            }

            _selection.Changed += SelectedFeaturesChanged;

            _drawnStatesNeeded = false;
        }

        /// <summary>
        /// The data set vertices invalidated.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DataSetVerticesInvalidated(object sender, EventArgs e)
        {
            OnApplyScheme(Symbology);
            Invalidate();
        }

        /// <summary>
        /// The join excel.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void JoinExcel(object sender, EventArgs e)
        {
            // This one works slightly differently.  A new result featureset
            // is created, but only some of the time.  If the new result is
            // different from the original "EditCopy" then we should add the
            // result to the layer.  Otherwise, do nothing.
            FeatureSetEventArgs args = new FeatureSetEventArgs(DataSet);
            FeatureLayerEventSender.Instance.Raise_ExcelJoin(this, args);
        }

        /// <summary>
        /// The label extents click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LabelExtentsClick(object sender, EventArgs e)
        {
            if (_labelLayer == null)
                return;

            var args = new DynamicVisibilityEventArgs(_labelLayer);
            args.Item.DynamicVisibilityWidth = _labelLayer.FeatureLayer.MapFrame.ViewExtents.Width;
            FeatureLayerEventSender.Instance.Raise_LabelExtents(this, args);
        }

        /// <summary>
        /// The selected features changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SelectedFeaturesChanged(object sender, EventArgs e)
        {
            if (!_drawnStatesNeeded && !_editMode)
            {
                AssignFastDrawnStates();
            }

            OnItemChanged();
            OnSelectionChanged();
        }

        /// <summary>
        /// The selection to layer click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SelectionToLayerClick(object sender, EventArgs e)
        {
            IFeatureLayer newLayer;
            if (CreateLayerFromSelectedFeatures(out newLayer))
            {
                IGroup grp = GetParentItem() as IGroup;
                if (grp != null)
                {
                    int index = grp.IndexOf(this);
                    grp.Insert(index + 1, newLayer);
                }

                newLayer.LegendText = LegendText + " selection";
            }
        }

        /// <summary>
        /// The selection zoom click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SelectionZoomClick(object sender, EventArgs e)
        {
            ZoomToSelectedFeatures();
        }

        private void SelectAllClick(object sender, EventArgs e)
        {
            this.SelectAll();
        }

        private void UnselectAllClick(object sender, EventArgs e)
        {
            this.UnSelectAll();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the specified scheme to this layer, applying the filter constraints in the scheme.
        /// It is at this time when features are grouped as a one-time operation into their symbol
        /// categories, so that this doesn't have to happen independently each drawing cycle.
        /// </summary>
        /// <param name="inScheme">
        /// The scheme to be applied to this layer.
        /// </param>
        public void ApplyScheme(IFeatureScheme inScheme)
        {
            OnApplyScheme(inScheme);
        }

        /// <summary>
        /// Clears the current selection, reverting the geometries back to their
        /// normal colors.
        /// </summary>
        /// <param name="affectedArea">
        /// An out value that represents the envelope that was modified by the clear selection instruction
        /// </param>
        /// <returns>
        /// The clear selection.
        /// </returns>
        public override bool ClearSelection(out IEnvelope affectedArea)
        {
            affectedArea = _selection.Envelope;
            if (!_drawnStatesNeeded)
            {
                return false;
            }

            bool changed = false;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                if (_selection.Count > 0)
                {
                    changed = true;
                }

                _selection.Clear();
            }
            else
            {
                SuspendChangeEvent();
                foreach (IFeatureCategory category in _scheme.GetCategories())
                {
                    if (!category.IsWithinLegendSelection())
                    {
                        continue;
                    }

                    _selection.RegionCategory = category;
                    _selection.Clear();
                    _selection.RegionCategory = null;
                }

                ResumeChangeEvent();
            }

            return changed;
        }

        /// <summary>
        /// This method actually draws the image to the snapshot using the graphics object.  This should be
        /// overridden in sub-classes because the drawing methods are very different.
        /// </summary>
        /// <param name="g">
        /// A graphics object to draw to
        /// </param>
        /// <param name="p">
        /// A projection handling interface designed to translate
        ///  geographic coordinates to screen coordinates
        /// </param>
        public virtual void DrawSnapShot(Graphics g, IProj p)
        {
            // Overridden in subclasses
        }

        /// <summary>
        /// Saves a featureset with only the selected features to the specified fileName.
        /// </summary>
        /// <param name="fileName">
        /// The string fileName to export features to.
        /// </param>
        public void ExportSelection(string fileName)
        {
            FeatureSet fs = _selection.ToFeatureSet();
            fs.SaveAs(fileName, true);
        }

        /// <summary>
        /// Gets the visible characteristic for an individual feature, regardless of whether
        /// this layer is in edit mode.
        /// </summary>
        /// <param name="index">
        /// </param>
        public IFeatureCategory GetCategory(int index)
        {
            if (_editMode)
            {
                return DrawingFilter.DrawnStates[DataSet.Features[index]].SchemeCategory;
            }

            return DrawnStates[index].Category;
        }

        /// <summary>
        /// Gets the visible characteristic for a given feature, rather than using the index,
        /// regardless of whether this layer is in edit mode.
        /// </summary>
        /// <param name="feature">
        /// </param>
        public IFeatureCategory GetCategory(IFeature feature)
        {
            if (_editMode)
            {
                return DrawingFilter.DrawnStates[feature].SchemeCategory;
            }

            int index = DataSet.Features.IndexOf(feature);
            return DrawnStates[index].Category;
        }

        /// <summary>
        /// Gets the visible characteristic for an individual feature
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// The get visible.
        /// </returns>
        public bool GetVisible(int index)
        {
            return _editMode ? DrawingFilter.DrawnStates[DataSet.Features[index]].IsVisible : DrawnStates[index].Visible;
        }

        /// <summary>
        /// Gets the visible characteristic for a given feature, rather than using the index.
        /// </summary>
        /// <param name="feature">
        /// </param>
        /// <returns>
        /// The get visible.
        /// </returns>
        public bool GetVisible(IFeature feature)
        {
            if (_editMode)
            {
                return DrawingFilter.DrawnStates[feature].IsVisible;
            }

            int index = DataSet.Features.IndexOf(feature);
            return DrawnStates[index].Visible;
        }

        /// <summary>
        /// This method inverts the selection for the specified region.  Members already a part of the selection
        /// will be removed from the selection, while members that are not a part of the selection will be added
        /// to the selection.
        /// </summary>
        /// <param name="tolerant">
        /// The region specifying where features should be added or removed from the
        /// selection.
        /// </param>
        /// <param name="strict">
        /// With polygon selection it is better not to allow any tolerance since the
        ///  polygons already contain it.
        /// </param>
        /// <param name="selectionMode">
        /// The SelectionModes enumeration that clarifies how the features should
        /// interact with the region.
        /// </param>
        /// <param name="affectedArea">
        /// The geographic region that will be impacted by the changes.
        /// </param>
        /// <returns>
        /// The invert selection.
        /// </returns>
        public override bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionMode selectionMode,
                                             out IEnvelope affectedArea)
        {
            if (!_drawnStatesNeeded && !_editMode)
            {
                AssignFastDrawnStates();
            }

            IEnvelope region = tolerant;
            if (DataSet.FeatureType == FeatureType.Polygon)
            {
                region = strict;
            }

            affectedArea = new Envelope();
            bool changed = false;

            _selection.SelectionMode = selectionMode;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                changed = _selection.InvertSelection(region, out affectedArea);
            }
            else
            {
                if (!_drawnStatesNeeded)
                {
                    AssignFastDrawnStates();
                }

                SuspendChangeEvent();
                _selection.SuspendChanges();
                List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
                foreach (IFeatureCategory category in categories)
                {
                    if (!category.IsWithinLegendSelection())
                    {
                        continue;
                    }

                    _selection.RegionCategory = category;
                    if (_selection.AddRegion(region, out affectedArea))
                    {
                        changed = true;
                    }

                    _selection.RegionCategory = null;
                }

                _selection.ResumeChanges();
                ResumeChangeEvent();
            }

            return changed;
        }

        /// <summary>
        /// Highlights the values in the specified region, and returns the affected area from the selection,
        /// which should allow for slightly faster drawing in cases where only a small area is changed.
        /// This will also specify the method by which members should be selected.
        /// </summary>
        /// <param name="tolerant">
        /// The envelope to change.
        /// </param>
        /// <param name="strict">
        /// The envelope to use in cases like polygons where the geometry has no tolerance.
        /// </param>
        /// <param name="selectionMode">
        /// The selection mode that clarifies the rules to use for selection.
        /// </param>
        /// <param name="affectedArea">
        /// The geographic envelope of the region impacted by the selection.
        /// </param>
        /// <returns>
        /// Boolean, true if items were selected.
        /// </returns>
        public override bool Select(IEnvelope tolerant, IEnvelope strict, SelectionMode selectionMode,
                                    out IEnvelope affectedArea)
        {
            if (!_drawnStatesNeeded && !_editMode)
            {
                AssignFastDrawnStates();
            }

            IEnvelope region = tolerant;
            if (DataSet.FeatureType == FeatureType.Polygon)
            {
                region = strict;
            }

            affectedArea = _selection.Envelope;

            bool changed = false;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                _selection.SelectionMode = selectionMode;
                changed = _selection.AddRegion(region, out affectedArea);
            }
            else
            {
                if (!_drawnStatesNeeded)
                {
                    AssignFastDrawnStates();
                }

                SuspendChangeEvent();
                _selection.ProgressHandler = ProgressHandler;
                _selection.SuspendChanges();
                List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
                foreach (IFeatureCategory category in categories)
                {
                    if (!category.IsSelected)
                    {
                        continue;
                    }

                    _selection.RegionCategory = category;
                    _selection.AddRegion(region, out affectedArea);
                    _selection.RegionCategory = null;
                }

                _selection.ResumeChanges();
                ResumeChangeEvent();
            }

            return changed;
        }

        /// <summary>
        /// Selects the specified list of features.  If the specified feature is already selected,
        /// this method will not alter it.  This will only fire a single SelectionExtended event,
        /// rather than firing it for each member selected.
        /// </summary>
        /// <param name="featureIndices">
        /// A List of integers representing the zero-based feature index values
        /// </param>
        public void Select(IEnumerable<int> featureIndices)
        {
            if (_editMode)
            {
                List<IFeature> features = featureIndices.Select(fid => DataSet.Features[fid]).ToList();
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null)
                {
                    sel.AddRange(features);
                }
            }
            else
            {
                if (!_drawnStatesNeeded)
                {
                    AssignFastDrawnStates();
                }

                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null)
                {
                    sel.AddRange(featureIndices);
                }
            }
        }

        /// <summary>
        /// Selects a single feature specified by the integer index in the Features list.
        /// </summary>
        /// <param name="featureIndex">
        /// The zero-based integer index of the feature.
        /// </param>
        public void Select(int featureIndex)
        {
            if (_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null)
                {
                    sel.Add(DataSet.Features[featureIndex]);
                }
            }
            else
            {
                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null)
                {
                    sel.Add(featureIndex);
                }
            }
        }

        /// <summary>
        /// Selects the specified feature
        /// </summary>
        /// <param name="feature">
        /// The feature to select
        /// </param>
        public void Select(IFeature feature)
        {
            if (_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null)
                {
                    sel.Add(feature);
                }
            }
            else
            {
                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null)
                {
                    sel.Add(DataSet.Features.IndexOf(feature));
                }
            }
        }

        /// <summary>
        /// Cycles through all the features and selects them
        /// </summary>
        public virtual void SelectAll()
        {
            if (!_drawnStatesNeeded && !_editMode)
            {
                AssignFastDrawnStates();
            }

            IEnvelope ignoreme;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                _selection.AddRegion(Extent.ToEnvelope(), out ignoreme);
            }
            else
            {
                SuspendChangeEvent();
                foreach (IFeatureCategory category in _scheme.GetCategories())
                {
                    if (!category.IsWithinLegendSelection())
                    {
                        continue;
                    }

                    _selection.RegionCategory = category;
                    _selection.AddRegion(Extent.ToEnvelope(), out ignoreme);
                    _selection.RegionCategory = null;
                }

                ResumeChangeEvent();
            }
        }

        /// <summary>
        /// Selects all the features in this layer that are associated
        /// with the specified attribute.  This automatically replaces the existing selection.
        /// </summary>
        /// <param name="filterExpression">
        /// The string expression to
        /// identify based on attributes the features to select.
        /// </param>
        public void SelectByAttribute(string filterExpression)
        {
            SelectByAttribute(filterExpression, ModifySelectionMode.Replace);
        }

        /// <summary>
        /// Unselects the features by attribute.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        public void UnselectByAttribute(string filterExpression)
        {
            SelectByAttribute(filterExpression, ModifySelectionMode.Subtract);
        }

        /// <summary>
        /// Modifies the features with a new selection based on the modifyMode.
        /// </summary>
        /// <param name="filterExpression">
        /// The string filter expression to use
        /// </param>
        /// <param name="modifyMode">
        /// Determines how the newly chosen features should interact with the existing
        ///  selection
        /// </param>
        public void SelectByAttribute(string filterExpression, ModifySelectionMode modifyMode)
        {
            if (!_drawnStatesNeeded && !_editMode)
            {
                AssignFastDrawnStates();
            }

            List<int> newSelection = DataSet.SelectIndexByAttribute(filterExpression);
            _selection.SuspendChanges();
            if (modifyMode == ModifySelectionMode.Replace)
            {
                _selection.Clear();
                Select(newSelection);
            }

            if (modifyMode == ModifySelectionMode.Append)
            {
                Select(newSelection);
            }

            if (modifyMode == ModifySelectionMode.SelectFrom)
            {
                List<int> cond = new List<int>();
                if (_editMode)
                {
                    IFeatureSelection fs = _selection as IFeatureSelection;
                    if (fs != null)
                    {
                        cond.AddRange(fs.Select(feature => DataSet.Features.IndexOf(feature)));
                    }
                }
                else
                {
                    IIndexSelection sel = _selection as IIndexSelection;
                    if (sel != null)
                    {
                        cond = sel.ToList();
                    }
                }

                IEnumerable<int> result = cond.Intersect(newSelection);
                _selection.Clear();
                Select(result);
            }

            if (modifyMode == ModifySelectionMode.Subtract)
            {
                UnSelect(newSelection);
            }

            _selection.ResumeChanges();
            OnItemChanged();
        }

        /// <inheritdoc />
        public void SetCategory(int index, IFeatureCategory category)
        {
            if (_editMode)
            {
                DrawingFilter.DrawnStates[DataSet.Features[index]].SchemeCategory = category;
            }
            else
            {
                DrawnStates[index].Category = category;
            }

            if (!_scheme.GetCategories().Contains(category))
            {
                _scheme.InsertCategory(0, category);
            }
        }

        /// <inheritdoc />
        public void SetCategory(IFeature feature, IFeatureCategory category)
        {
            if (_editMode)
            {
                DrawingFilter.DrawnStates[feature].SchemeCategory = category;
            }
            else
            {
                int index = DataSet.Features.IndexOf(feature);
                DrawnStates[index].Category = category;
            }

            if (!_scheme.GetCategories().Contains(category))
            {
                _scheme.InsertCategory(0, category);
            }
        }

        /// <summary>
        /// This forces the creation of a category for the specified symbolizer, if it doesn't exist.
        /// This will add the specified feature to the category.  Be sure that the symbolizer type
        /// matches the feature type.
        /// </summary>
        /// <param name="index">
        /// The integer index of the shape to control.
        /// </param>
        /// <param name="symbolizer">
        /// The symbolizer to assign.
        /// </param>
        public void SetShapeSymbolizer(int index, IFeatureSymbolizer symbolizer)
        {
            foreach (IFeatureCategory category in Symbology.GetCategories())
            {
                if (category.Symbolizer != symbolizer)
                {
                    continue;
                }

                if (DataSet.IndexMode)
                {
                    _drawnStates[index].Category = category;
                    _drawnStates[index].Visible = true;
                }
                else
                {
                    IFeature f = DataSet.Features[index];
                    DrawingFilter.DrawnStates[f].SchemeCategory = category;
                }

                Invalidate();
                return;
            }

            IFeatureCategory cat = Symbology.CreateNewCategory(Color.Blue, 1) as IFeatureCategory;
            if (cat != null)
            {
                cat.Symbolizer = symbolizer;
                Symbology.AddCategory(cat);
                if (DataSet.IndexMode)
                {
                    _drawnStates[index].Category = cat;
                    _drawnStates[index].Visible = true;
                }
                else
                {
                    IFeature f = DataSet.Features[index];
                    DrawingFilter.DrawnStates[f].SchemeCategory = cat;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Sets the visible characteristic for an individual feature regardless of
        /// whether this layer is in edit mode.
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <param name="visible">
        /// </param>
        public void SetVisible(int index, bool visible)
        {
            if (_editMode)
            {
                DrawingFilter.DrawnStates[DataSet.Features[index]].IsVisible = visible;
            }
            else
            {
                DrawnStates[index].Visible = visible;
            }
        }

        /// <summary>
        /// Sets the visible characteristic for a given feature, rather than using the index
        /// regardless of whether this layer is in edit mode.
        /// </summary>
        /// <param name="feature">
        /// </param>
        /// <param name="visible">
        /// </param>
        public void SetVisible(IFeature feature, bool visible)
        {
            if (_editMode)
            {
                DrawingFilter.DrawnStates[feature].IsVisible = visible;
            }
            else
            {
                int index = DataSet.Features.IndexOf(feature);
                DrawnStates[index].Visible = visible;
            }
        }

        /// <summary>
        /// Displays a form with the attributes for this shapefile.
        /// </summary>
        public void ShowAttributes()
        {
            // Allow derived classes to prevent this
            HandledEventArgs result = new HandledEventArgs(false);
            OnViewAttributes(result);
            if (result.Handled)
            {
                return;
            }

            FeatureLayerEventSender.Instance.Raise_ShowAttributes(this, new FeatureLayerEventArgs(this));
        }

        /// <summary>
        /// Creates a bitmap of the requested size that covers the specified geographic extent using
        /// the current symbolizer for this layer.  This does not have any drawing optimizations,
        /// or techniques to speed up performance and should only be used in special cases like
        /// draping of vector content onto a texture.  It also doesn't worry about selections.
        /// </summary>
        /// <param name="geographicExtent">
        /// The extent to use when computing the snapshot.
        /// </param>
        /// <param name="width">
        /// The integer height of the bitmap.  The height is calculated based on
        /// the aspect ratio of the specified geographic extent.
        /// </param>
        /// <returns>
        /// A Bitmap object
        /// </returns>
        public Bitmap SnapShot(Extent geographicExtent, int width)
        {
            int height = Convert.ToInt32((geographicExtent.Height / geographicExtent.Width) * width);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            ImageProjection p = new ImageProjection(geographicExtent, new Rectangle(0, 0, width, height));
            DrawSnapShot(g, p);
            g.Dispose();
            OnSnapShotTaken(bmp);
            return bmp;
        }

        /// <summary>
        /// Unselects the specified features.  If any features already unselected, they are ignored.
        /// This will only fire a single Selection
        /// </summary>
        /// <param name="featureIndices">
        /// </param>
        public void UnSelect(IEnumerable<int> featureIndices)
        {
            if (_editMode)
            {
                List<IFeature> features = featureIndices.Select(fid => DataSet.Features[fid]).ToList();
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null)
                {
                    sel.RemoveRange(features);
                }
            }
            else
            {
                if (!_drawnStatesNeeded)
                {
                    AssignFastDrawnStates();
                }

                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null)
                {
                    sel.RemoveRange(featureIndices);
                }
            }
        }

        /// <summary>
        /// Unselects the specified feature.
        /// </summary>
        /// <param name="featureIndex">
        /// The integer representing the feature to unselect.
        /// </param>
        public void UnSelect(int featureIndex)
        {
            if (_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null)
                {
                    sel.Remove(DataSet.Features[featureIndex]);
                }
            }
            else
            {
                if (!_drawnStatesNeeded)
                {
                    AssignFastDrawnStates();
                }

                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null)
                {
                    sel.Remove(featureIndex);
                }
            }
        }

        /// <summary>
        /// Removes the specified feature from the selection.
        /// </summary>
        /// <param name="feature">
        /// The feature to unselect.
        /// </param>
        public void UnSelect(IFeature feature)
        {
            if (_editMode)
            {
                IFeatureSelection sel = _selection as IFeatureSelection;
                if (sel != null)
                {
                    sel.Remove(feature);
                }
            }
            else
            {
                if (!_drawnStatesNeeded)
                {
                    AssignFastDrawnStates();
                }

                IIndexSelection sel = _selection as IIndexSelection;
                if (sel != null)
                {
                    sel.Remove(DataSet.Features.IndexOf(feature));
                }
            }
        }

        /// <summary>
        /// Un-highlights or returns the features from the specified region.  The specified selectionMode
        /// will be used to determine how to choose features.
        /// </summary>
        /// <param name="tolerant">
        /// The geographic envelope in cases like clicking near points where tolerance
        ///  is allowed.
        /// </param>
        /// <param name="strict">
        /// The geographic region when working with absolutes, without a tolerance.
        /// </param>
        /// <param name="selectionMode">
        /// The selection mode that controls how to choose members relative to the
        /// region.
        /// </param>
        /// <param name="affectedArea">
        /// The geographic envelope that will be visibly impacted by the change.
        /// </param>
        /// <returns>
        /// Boolean, true if members were removed from the selection.
        /// </returns>
        public override bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionMode selectionMode,
                                      out IEnvelope affectedArea)
        {
            if (!_drawnStatesNeeded && !_editMode)
            {
                AssignFastDrawnStates();
            }

            IEnvelope region = tolerant;
            if (DataSet.FeatureType == FeatureType.Polygon)
            {
                region = strict;
            }

            affectedArea = new Envelope();

            bool changed = false;
            if (IsWithinLegendSelection() || _scheme.IsWithinLegendSelection())
            {
                if (_editMode)
                {
                    _selection.SelectionMode = selectionMode;
                    changed = _selection.RemoveRegion(region, out affectedArea);
                }
                else
                {
                    _selection.SelectionMode = selectionMode;
                    changed = _selection.RemoveRegion(region, out affectedArea);
                }
            }
            else
            {
                SuspendChangeEvent();
                _selection.SuspendChanges();
                List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
                foreach (IFeatureCategory category in categories)
                {
                    if (!category.IsSelected)
                    {
                        continue;
                    }

                    _selection.RegionCategory = category;
                    _selection.RemoveRegion(region, out affectedArea);
                    _selection.RegionCategory = null;
                }

                _selection.ResumeChanges();
                ResumeChangeEvent();
            }

            return changed;
        }

        /// <summary>
        /// Unselects all the features that are currently selected
        /// </summary>
        public virtual void UnSelectAll()
        {
            _selection.Clear();
        }

        /// <summary>
        /// This is testing the idea of using an input parameter type that is marked as out
        /// instead of a return type.
        /// </summary>
        /// <param name="result">
        /// The result of the creation
        /// </param>
        /// <returns>
        /// Boolean, true if a layer can be created
        /// </returns>
        public virtual bool CreateLayerFromSelectedFeatures(out IFeatureLayer result)
        {
            // This needs to be overridden at the higher levels where drawing function
            // and point types exist, but is available here so that you don't need
            // to know what kind of feature layer you have to create an output layer.
            result = null;
            return false;
        }

        /// <summary>
        /// Occurs when the properties should be shown, and launches a layer dialog.
        /// </summary>
        /// <param name="e">
        /// </param>
        protected override void OnShowProperties(HandledEventArgs e)
        {
            FeatureLayerEventArgs args = new FeatureLayerEventArgs(this);
            FeatureLayerEventSender.Instance.Raise_ShowProperties(this, args);
            e.Handled = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dictionary of extents that is calculated from the categories.  This is calculated one time,
        /// when the scheme is applied, and then the cached value is used to help drawing symbols that
        /// are modified by the categorical boundaries.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDictionary<IFeatureCategory, Extent> CategoryExtents
        {
            get { return _categoryExtents; }
        }

        /// <summary>
        /// Gets or sets a rectangle that gives the maximal extent for 2D GDI+ drawing in pixels.
        /// Coordinates outside this range will cause overflow exceptions to occur.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle DrawingBounds
        {
            get { return _drawingBounds; }
            set { _drawingBounds = value; }
        }

        /// <summary>
        /// Gets or sets the drawing filter that can be used to narrow the list of features and then
        /// cycle through those features.  Using a for-each expression on the filter will automatically
        /// apply the constraints specified by the characteristics.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public IDrawingFilter DrawingFilter
        {
            get { return _drawingFilter; }
            set { _drawingFilter = value; }
        }

        /// <summary>
        ///  Gets or sets a string name for this layer.  This is not necessarily the same as the legend text.
        /// </summary>
        [Category("General"),
         Description("Gets or sets a string name for this layer.  This is not necessarily the same as the legend text."
             )]
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the chunk size on the drawing filter.  This should be controlled
        /// by drawing layers.
        /// </summary>
        protected int ChunkSize
        {
            get { return _chunkSize; }
            set { _chunkSize = value; }
        }

        /// <summary>
        /// Gets or sets the underlying dataset for this layer, specifically as an IFeatureSet
        /// </summary>
        [Serialize("DataSet", ConstructorArgumentIndex = 0)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IFeatureSet DataSet
        {
            get { return base.DataSet as IFeatureSet; }
            set
            {
                if (DataSet != null)
                {
                    OnIgnoreFeaturesetEvents(DataSet);
                }

                base.DataSet = value;
                OnHandleFeaturesetEvents(DataSet);
            }
        }

        /// <summary>
        /// Controls the drawn states according to a feature index.  This is used if the EditMode is
        /// false.  When EditMode is true, then drawn states are tied to the features instead.
        /// </summary>
        public FastDrawnState[] DrawnStates
        {
            get
            {
                if (_drawnStates == null)
                {
                    DrawnStatesNeeded = true;
                }

                return _drawnStates;
            }

            set { _drawnStates = value; }
        }

        /// <summary>
        /// Gets or sets a boolean.  If edit mode is true, feature index is ignored, and features
        /// are assumed to be entirely loaded into ram.  If edit mode is false, then index
        /// is used instead and features are not assumed to be loaded into ram.
        /// </summary>
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                if (_editMode)
                {
                    _drawingFilter = new DrawingFilter(DataSet.Features, _scheme, 5000);
                    _selection = new Selection(DataSet, _drawingFilter);
                }
                else
                {
                    _selection = new IndexSelection(this);
                }
            }
        }

        /// <summary>
        /// Gets the envelope of the DataSet supporting this FeatureLayer
        /// </summary>
        [Category("General"), Description("Gets the envelope of the DataSet supporting this FeatureLayer")]
        public override Extent Extent
        {
            get
            {
                if (DataSet == null || DataSet.Extent == null || DataSet.Extent.IsEmpty())
                {
                    return new Extent();
                }

                return DataSet.Extent;
            }
        }

        /// <summary>
        /// Gets or sets the label layer
        /// </summary>
        [Category("General"), Description("Gets or sets the label layer associated with this feature layer.")]
        [ShallowCopy]
        [Serialize("LabelLayer")]
        public virtual ILabelLayer LabelLayer
        {
            get { return _labelLayer; }
            set
            {
                _labelLayer = value;
                _labelLayer.FeatureLayer = this;
                _labelLayer.CreateLabels();
                OnItemChanged();
            }
        }

        /// <summary>
        /// Restructures the LegendItems based on whether or not the symbology makes use
        /// of schemes.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                if (Symbology.AppearsInLegend)
                {
                    List<ILegendItem> list = new List<ILegendItem> { Symbology };
                    return list;
                }

                // Leave this cast in place for compatibility with 3.5.
                return Symbology.GetCategories().Cast<ILegendItem>();
            }
        }

        /// <summary>
        /// Gets a Selection class that is allows the user to cycle through all the selected features with
        /// a foreach method.  This can be null.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISelection Selection
        {
            get { return _selection; }
        }

        /// <summary>
        /// Gets or sets the shared characteristics to use with the selected features.
        /// </summary>
        [Category("Symbology"), Description("Gets or sets a collection of feature characteristics for this layer."),
         ShallowCopy]
        public virtual IFeatureSymbolizer SelectionSymbolizer
        {
            get
            {
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {
                            return categories.First().SelectionSymbolizer;
                        }
                    }
                }

                return _selectionFeatureSymbolizer;
            }

            set
            {
                value.SetParentItem(this);
                bool defaultExisted = false;
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {
                            categories.First().SelectionSymbolizer = value;
                            defaultExisted = true;
                        }
                    }
                }

                if (defaultExisted == false)
                {
                    _selectionFeatureSymbolizer = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether labels should be drawn.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether labels should be drawn."), Serialize("ShowLabels")]
        public virtual bool ShowLabels
        {
            get { return _showLabels; }
            set
            {
                _showLabels = value;
                OnItemChanged();
            }
        }

        /// <summary>
        /// Gets or sets and interface for the shared symbol characteristics between point, line and polygon features
        /// </summary>
        [Category("Appearance"), Description("Gets or sets a collection of feature characteristics for this layer."),
         ShallowCopy]
        public virtual IFeatureSymbolizer Symbolizer
        {
            get
            {
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {
                            return categories.First().Symbolizer;
                        }
                    }
                }

                return _featureSymbolizer;
            }

            set
            {
                value.SetParentItem(this);
                bool defaultExisted = false;
                if (_scheme != null)
                {
                    IEnumerable<IFeatureCategory> categories = _scheme.GetCategories();
                    if (categories != null)
                    {
                        if (categories.Count() > 0)
                        {
                            categories.First().Symbolizer = value;
                            defaultExisted = true;
                        }
                    }
                }

                if (defaultExisted == false)
                {
                    _featureSymbolizer = value;
                }
            }
        }

        /// <summary>
        /// Gets the current feature scheme, but to change it ApplyScheme should be called, so that
        /// feature categories are updated as well.
        /// </summary>
        [ShallowCopy]
        public IFeatureScheme Symbology
        {
            get { return _scheme; }
            set
            {
                if (value == _scheme)
                {
                    return;
                }

                OnExcludeScheme(_scheme);
                _scheme = value;
                OnIncludeScheme(value);
                ApplyScheme(value);
            }
        }

        /// <summary>
        /// Occurs when setting the symbology to a new scheme and allows removing event handlers
        /// </summary>
        /// <param name="scheme">
        /// </param>
        protected virtual void OnExcludeScheme(IFeatureScheme scheme)
        {
            if (scheme == null)
            {
                return;
            }

            scheme.ItemChanged -= SchemeItemChanged;
            scheme.SetParentItem(null);
            scheme.SelectFeatures -= OnSelectFeatures;
            scheme.DeselectFeatures -= OnDeselectFeatures;
        }

        /// <summary>
        /// Occurs as a new featureset is being assigned to this layer
        /// </summary>
        /// <param name="featureSet">
        /// The feature Set.
        /// </param>
        protected virtual void OnHandleFeaturesetEvents(IFeatureSet featureSet)
        {
            if (featureSet == null)
            {
                return;
            }

            DataSet.VerticesInvalidated += DataSetVerticesInvalidated;
            DataSet.FeatureAdded += DataSetFeatureAdded;
            DataSet.FeatureRemoved += DataSetFeatureRemoved;
        }

        /// <summary>
        /// Unwires event handlers for the specified featureset.
        /// </summary>
        /// <param name="featureSet">
        /// </param>
        protected virtual void OnIgnoreFeaturesetEvents(IFeatureSet featureSet)
        {
            if (featureSet == null)
            {
                return;
            }

            DataSet.VerticesInvalidated -= DataSetVerticesInvalidated;
            DataSet.FeatureAdded -= DataSetFeatureAdded;
            DataSet.FeatureRemoved -= DataSetFeatureRemoved;
        }

        /// <summary>
        /// Occurs when setting symbology to a new scheme and allows adding event handlers
        /// </summary>
        /// <param name="scheme">
        /// </param>
        protected virtual void OnIncludeScheme(IFeatureScheme scheme)
        {
            if (scheme == null)
            {
                return;
            }

            scheme.ItemChanged += SchemeItemChanged;
            scheme.SetParentItem(this);
            scheme.SelectFeatures += OnSelectFeatures;
            scheme.DeselectFeatures += OnDeselectFeatures;
        }

        /// <summary>
        /// Occurs when selecting features and fires the SelectByAttribute event with
        /// the expression used as the filter expression
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected virtual void OnSelectFeatures(object sender, ExpressionEventArgs e)
        {
            SelectByAttribute(e.Expression);
        }

        /// <summary>
        /// Occurs when selecting features and fires the SelectByAttribute event with
        /// the expression used as the filter expression
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected virtual void OnDeselectFeatures(object sender, ExpressionEventArgs e)
        {
            UnselectByAttribute(e.Expression);
        }
        /// <summary>
        /// The data set feature added.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DataSetFeatureAdded(object sender, FeatureEventArgs e)
        {
            if (_drawingFilter == null)
            {
                return;
            }

            if (_drawingFilter.DrawnStates == null)
            {
                return;
            }

            _drawingFilter.DrawnStates.Add(e.Feature, new DrawnState(Symbology.GetCategories().First(), false, 0, true));
        }

        /// <summary>
        /// The data set feature removed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DataSetFeatureRemoved(object sender, FeatureEventArgs e)
        {
            if (_drawingFilter == null)
            {
                return;
            }

            _drawingFilter.DrawnStates.Remove(e.Feature);
        }

        /// <summary>
        /// Echoes the ItemChanged event
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void SchemeItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Assigns the fast drawn states
        /// </summary>
        public void AssignFastDrawnStates()
        {
            _drawnStatesNeeded = true;
            _drawnStates = new FastDrawnState[DataSet.ShapeIndices.Count];
            _selection.Changed -= SelectedFeaturesChanged;
            _selection = new IndexSelection(this); // update the new drawn-states;
            _selection.Changed += SelectedFeaturesChanged;

            // Fastest when no categories are used because we don't need DataTable at all
            List<IFeatureCategory> categories = _scheme.GetCategories().ToList();
            IFeatureCategory deflt = null;
            if (categories.Count > 0 && categories[0].FilterExpression == null)
            {
                deflt = categories[0];
            }

            for (int i = 0; i < DataSet.ShapeIndices.Count; i++)
            {
                _drawnStates[i] = new FastDrawnState { Category = deflt };
            }

            if (categories.Count == 1 && categories[0].FilterExpression == null)
            {
                return;
            }

            bool containsFid = false;
            if (!DataSet.AttributesPopulated)
            {
                // We don't want to read the table multiple times for each category.  Just
                // read a block and then do all the categories we can for that block.
                List<string> names = new List<string>();
                foreach (var category in categories)
                {
                    var current = DistinctFieldsInExpression(category.FilterExpression);
                    foreach (string name in current.Where(name => !names.Contains(name)))
                    {
                        names.Add(name);
                    }
                }

                if (names.Count() == 0)
                {
                    for (int i = 0; i < DataSet.NumRows(); i++)
                    {
                        _drawnStates[i].Category = categories[categories.Count - 1];
                    }

                    return;
                }

                int rowCount = DataSet.NumRows();
                const int size = 50000;
                int numPages = (int)Math.Ceiling((double)rowCount / size);
                for (int page = 0; page < numPages; page++)
                {
                    int count = (page == numPages - 1) ? rowCount - page * size : size;
                    DataTable expressionTable = DataSet.GetAttributes(page * size, count, names);
                    foreach (var category in categories)
                    {
                        DataRow[] res = expressionTable.Select(category.FilterExpression);
                        foreach (DataRow r in res)
                        {
                            _drawnStates[(int)r["FID"]].Category = category;
                        }
                    }
                }
            }
            else
            {
                DataTable table = DataSet.DataTable;
                foreach (var category in categories)
                {
                    if (category.FilterExpression != null && category.FilterExpression.Contains("[FID]"))
                    {
                        containsFid = true;
                    }
                }

                if (containsFid && table.Columns.Contains("FID") == false)
                {
                    table.Columns.Add("FID");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        table.Rows[i]["FID"] = i;
                    }
                }

                foreach (var category in categories)
                {
                    DataRow[] result = table.Select(category.FilterExpression);
                    foreach (DataRow row in result)
                    {
                        _drawnStates[table.Rows.IndexOf(row)].Category = category;
                    }
                }

                if (containsFid)
                {
                    table.Columns.Remove("FID");
                }
            }
        }

        /// <summary>
        /// This method will remove the in ram features from the underlying dataset.
        /// This will not affect the data source.
        /// </summary>
        /// <param name="indexValues">The list or array of integer index values.</param>
        public void RemoveFeaturesAt(IEnumerable<int> indexValues)
        {
            DataSet.RemoveShapesAt(indexValues);
            AssignFastDrawnStates();
        }

        /// <summary>
        /// This forces the removal of all the selected features.
        /// </summary>
        public void RemoveSelectedFeatures()
        {
            // Only work with selections with some features.
            if (Selection.Count == 0) return;

            IFeatureSet fs = DataSet;
            if (fs.IndexMode)
            {
                // Use Index selection to remove by index
                IndexSelection indexSel = Selection as IndexSelection;

                // In case we have an invalid cast for some reason.
                if (indexSel == null) return;

                // Create a list of index values to remove.
                List<int> orderedIndex = new List<int>();
                foreach (int index in indexSel)
                {
                    orderedIndex.Add(index);
                }

                RemoveFeaturesAt(orderedIndex);
                // Since the indexing has changed, we need to update the drawn states too.
            }
            else
            {
                // This case tracks by IFeature, so we don't need to do a lot else.
                List<IFeature> features = Selection.ToFeatureList();
                foreach (IFeature feature in features)
                {
                    DataSet.Features.Remove(feature);
                }
            }
        }

        /// <summary>
        /// This does more than remove an index key.  This also shifts down the category
        /// and selection state for every drawn state with a higher index than the
        /// given index value.  Call this only in index mode, and only if the shape
        /// is being removed from the FeatureSet.  in practice, this should really
        /// only be called internally, but is here just in case.
        /// </summary>
        /// <param name="indexValues">
        /// The list or array of index values to remove.
        /// </param>
        private void RemoveDrawnStates(IEnumerable<int> indexValues)
        {
            Dictionary<int, FastDrawnState> set = new Dictionary<int, FastDrawnState>();
            int ind = 0;
            List<int> remaining = new List<int>();
            foreach (FastDrawnState state in _drawnStates)
            {
                set.Add(ind, state);
                remaining.Add(ind);
                ind++;
            }

            foreach (int index in indexValues)
            {
                set.Remove(index);
                remaining.Remove(index);
            }

            _drawnStates = new FastDrawnState[set.Count];
            int i = 0;
            foreach (int r in remaining)
            {
                _drawnStates[i] = set[r];
                i++;
            }

            _selection.Changed -= SelectedFeaturesChanged;
            // If the drawnStates changes, the selection must also change.
            _selection = new IndexSelection(this);
            _selection.Changed += SelectedFeaturesChanged;
        }

        /// <summary>
        /// This calculates the extent for the category and caches it in the extents collection
        /// </summary>
        /// <param name="category">
        /// </param>
        protected virtual Extent CalculateCategoryExtent(IFeatureCategory category)
        {
            Extent ext = new Extent(new[] { double.MaxValue, double.MaxValue, double.MinValue, double.MinValue });
            if (_editMode)
            {
                IDictionary<IFeature, IDrawnState> features = _drawingFilter.DrawnStates;

                foreach (IFeature f in DataSet.Features)
                {
                    if (category == features[f].SchemeCategory)
                    {
                        ext.ExpandToInclude(new Extent(f.Envelope));
                    }
                }

                if (_categoryExtents.Keys.Contains(category))
                {
                    _categoryExtents[category] = ext.Copy();
                }
                else
                {
                    _categoryExtents.Add(category, ext.Copy());
                }
            }
            else
            {
                FastDrawnState[] states = DrawnStates;
                List<ShapeRange> ranges = DataSet.ShapeIndices;
                for (int shp = 0; shp < DrawnStates.Length; shp++)
                {
                    if (states[shp].Category != null)
                    {
                        if (!_categoryExtents.ContainsKey(states[shp].Category))
                        {
                            _categoryExtents.Add(states[shp].Category, ranges[shp].Extent.Copy());
                        }
                        else
                        {
                            _categoryExtents[states[shp].Category].ExpandToInclude(ranges[shp].Extent);
                        }
                    }
                }
            }

            return ext;
        }

        /// <summary>
        /// This method cycles through all the Categories in the scheme and creates a new
        /// category.
        /// </summary>
        /// <param name="scheme">
        /// The scheme to apply
        /// </param>
        protected virtual void OnApplyScheme(IFeatureScheme scheme)
        {
            // _drawingFilter.ApplyScheme(scheme);
            if (_editMode)
            {
                _drawingFilter.ApplyScheme(scheme);
            }
            else
            {
                List<IFeatureCategory> categories = scheme.GetCategories().ToList();
                if (_drawnStatesNeeded || (_selection != null && _selection.Count > 0) || categories.Count > 1)
                {
                    AssignFastDrawnStates();
                }
                else if (categories.Count == 1)
                {
                    if (categories[0].FilterExpression != null)
                    {
                        AssignFastDrawnStates();
                    }
                }
            }

            _categoryExtents.Clear();
            OnSchemeApplied();
            OnItemChanged(this);
        }

        /// <summary>
        /// Occurs during a copy operation and handles removing surplus event handlers
        /// </summary>
        /// <param name="copy">
        /// </param>
        protected override void OnCopy(Descriptor copy)
        {
            // Remove event handlers from the copy (since Memberwise clone also clones handlers.)
            FeatureLayer flCopy = copy as FeatureLayer;
            if (flCopy == null)
            {
                return;
            }

            if (flCopy.ViewAttributes != null)
            {
                foreach (var handler in flCopy.ViewAttributes.GetInvocationList())
                {
                    flCopy.ViewAttributes -= (HandledEventHandler)handler;
                }
            }

            if (flCopy.LabelSetup != null)
            {
                foreach (var handler in flCopy.LabelSetup.GetInvocationList())
                {
                    flCopy.LabelSetup -= (HandledEventHandler)handler;
                }
            }

            if (flCopy.SnapShotTaken != null)
            {
                foreach (var handler in flCopy.SnapShotTaken.GetInvocationList())
                {
                    flCopy.SnapShotTaken -= (EventHandler<SnapShotEventArgs>)handler;
                }
            }

            if (flCopy.SchemeApplied != null)
            {
                foreach (var handler in flCopy.SchemeApplied.GetInvocationList())
                {
                    flCopy.SchemeApplied -= (EventHandler)handler;
                }
            }

            base.OnCopy(copy);
        }

        /// <summary>
        /// A default method to generate a label layer.
        /// </summary>
        protected virtual void OnCreateLabels()
        {
            _labelLayer = new LabelLayer();
        }

        /// <summary>
        /// Handles the situation for exporting the layer as a new source.
        /// </summary>
        protected override void OnExportData()
        {
            // In this case the feature layer won't be edited, but rather,
            // it needs to be passed so the process can work with the selection
            // and possibly add the finished content back into the map.
            FeatureLayerEventArgs args = new FeatureLayerEventArgs(this);
            FeatureLayerEventSender.Instance.Raise_ExportData(this, args);
        }

        /// <summary>
        /// Occurs before the label setup dialog is shown.  If handled is set to true,
        /// then the dialog will not be shown.
        /// </summary>
        /// <param name="e">
        /// A HandledEventArgs parameter
        /// </param>
        protected virtual void OnLabelSetup(HandledEventArgs e)
        {
            if (LabelSetup != null)
            {
                LabelSetup(this, e);
            }
        }

        /// <summary>
        /// This fires the scheme applied event after the scheme has been altered
        /// </summary>
        protected virtual void OnSchemeApplied()
        {
            if (SchemeApplied != null)
            {
                SchemeApplied(this, new EventArgs());
            }
        }

        /// <summary>
        /// Fires the SnapShotTaken event.  This can be overridden in order to modify the bitmap returned etc.
        /// </summary>
        /// <param name="e">
        /// </param>
        protected virtual void OnSnapShotTaken(Bitmap e)
        {
            if (SnapShotTaken != null)
            {
                SnapShotTaken(this, new SnapShotEventArgs(e));
            }
        }

        /// <summary>
        /// Occurs before attributes are about to be viewed.  Overriding this
        /// allows that to be handled, and if e.Handled is true, this class
        /// won't show the attributes.
        /// </summary>
        /// <param name="e">
        /// A HandledEventArgs
        /// </param>
        protected virtual void OnViewAttributes(HandledEventArgs e)
        {
            if (ViewAttributes != null)
            {
                ViewAttributes(this, e);
            }
        }

        /// <summary>
        /// The distinct fields in expression.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// </returns>
        private static IEnumerable<string> DistinctFieldsInExpression(string expression)
        {
            // Fields should be indicated by [ ] characters.
            List<string> allNames = new List<string>();
            if (expression == null)
            {
                return allNames;
            }
            bool isField = false; // I don't think nesting is possible
            List<char> currentName = new List<char>();
            foreach (char current in expression)
            {
                if (isField)
                {
                    if (current == ']')
                    {
                        isField = false;
                        allNames.Add(new string(currentName.ToArray()));
                        currentName = new List<char>();
                        continue;
                    }

                    currentName.Add(current);
                }

                if (current == '[')
                {
                    isField = true;
                    continue;
                }
            }

            return allNames.Distinct();
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// The label setup click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LabelSetupClick(object sender, EventArgs e)
        {
            HandledEventArgs result = new HandledEventArgs(false);
            OnLabelSetup(result);
            if (result.Handled)
            {
                return;
            }

            if (_labelLayer == null)
            {
                OnCreateLabels();
                ShowLabels = true;
            }

            var args = new LabelLayerEventArgs(_labelLayer);
            FeatureLayerEventSender.Instance.Raise_LabelSetup(this, args);
        }

        #endregion

        /// <summary>
        /// Disposes memory objects
        /// </summary>
        /// <param name="disposeManagedResources">
        /// </param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                ViewAttributes = null;
                LabelSetup = null;
                SnapShotTaken = null;
                SchemeApplied = null;
                _name = null;
                _scheme = null;
                _selection = null;
                _drawingFilter = null;
                _featureSymbolizer = null;
                _selectionFeatureSymbolizer = null;
                _categoryExtents = null;
                _drawnStates = null;
            }

            if (_labelLayer != null)
            {
                _labelLayer.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
    }
}