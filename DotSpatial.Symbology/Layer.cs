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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 4:44:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Layer
    /// </summary>
    [ToolboxItem(false)]
    public class Layer : RenderableLegendItem, ILayer
    {
        #region Events

        /// <summary>
        /// Occurs if this layer was selected
        /// </summary>
        public event EventHandler<LayerSelectedEventArgs> LayerSelected;

        /// <summary>
        /// Occurs if the maps should zoom to this layer.
        /// </summary>
        public event EventHandler<EnvelopeArgs> ZoomToLayer;

        /// <summary>
        /// Occurs before the properties are actually shown, also allowing the event to be handled.
        /// </summary>
        public event HandledEventHandler ShowProperties;

        /// <summary>
        /// Occurs when all aspects of the layer finish loading.
        /// </summary>
        public event EventHandler FinishedLoading;

        /// <summary>
        /// Occurs after the selection is changed
        /// </summary>
        public event EventHandler SelectionChanged;

        #endregion Events

        #region Private Variables

        private IDataSet _dataSet;
        private int _disposeLockCount;
        private DynamicVisibilityMode _dynamicVisibilityMode;
        private double _dynamicVisibilityWidth;
        private Layer _editCopy;
        private Extent _invalidatedRegion; // When a region is invalidated instead of the whole layer.
        private bool _isDisposed;
        private IFrame _mapFrame;
        private IProgressHandler _progressHandler;
        private ProgressMeter _progressMeter;
        private ProjectionInfo _projection;
        private string _projectionString;
        private IPropertyDialogProvider _propertyDialogProvider;
        private bool _selectionEnabled;
        private bool _useDynamicVisibility;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Creates a new Layer, but this should be done from the derived classes
        /// </summary>
        protected Layer()
        {
            // InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new Layer, but this should be done from the derived classes
        /// </summary>
        /// <param name="container">The container this layer should be a member of</param>
        protected Layer(ICollection<ILayer> container)
        {
            if (container != null) container.Add(this);
            Configure();
        }

        /// <summary>
        /// Creates a new layer with only a progress handler
        /// </summary>
        /// <param name="progressHandler"></param>
        protected Layer(IProgressHandler progressHandler)
        {
            _progressHandler = progressHandler;
            Configure();
        }

        /// <summary>
        /// Creates a new Layer, but this should be done from the derived classes
        /// </summary>
        /// <param name="container">The container this layer should be a member of</param>
        /// <param name="progressHandler">A progress handler for handling progress messages</param>
        protected Layer(ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            _progressHandler = progressHandler;
            if (container != null) container.Add(this);
            Configure();
        }

        private void Configure()
        {
            _selectionEnabled = true;
            base.ContextMenuItems = new List<SymbologyMenuItem>
                                        {
                                            new SymbologyMenuItem(SymbologyMessageStrings.RemoveLayer, SymbologyImages.mnuLayerClear,
                                                                  RemoveLayerClick),
                                            new SymbologyMenuItem(SymbologyMessageStrings.ZoomToLayer, SymbologyImages.ZoomInMap,
                                                                  ZoomToLayerClick),
                                            new SymbologyMenuItem(SymbologyMessageStrings.SetDynamicVisibilityScale, SymbologyImages.ZoomScale,
                                                                  SetDynamicVisibility)
                                        };
            SymbologyMenuItem mnuData = new SymbologyMenuItem("Data");
            mnuData.MenuItems.Add(new SymbologyMenuItem(SymbologyMessageStrings.ExportData, SymbologyImages.save, ExportDataClick));
            base.ContextMenuItems.Add(mnuData);
            base.ContextMenuItems.Add(new SymbologyMenuItem(SymbologyMessageStrings.Properties, SymbologyImages.color_scheme, ShowPropertiesClick));
            base.LegendSymbolMode = SymbolMode.Checkbox;
            LegendType = LegendType.Layer;
            base.IsExpanded = true;
        }

        private void PropertyDialogProviderChangesApplied(object sender, ChangedObjectEventArgs e)
        {
            CopyProperties(e.Item as ILegendItem);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Given a geographic extent, this tests the "IsVisible", "UseDynamicVisibility",
        ///  "DynamicVisibilityMode" and "DynamicVisibilityWidth"
        /// In order to determine if this layer is visible for the specified scale.
        /// </summary>
        /// <param name="geographicExtent">The geographic extent, where the width will be tested.</param>
        /// <returns>Boolean, true if this layer should be visible for this extent.</returns>
        public bool VisibleAtExtent(Extent geographicExtent)
        {
            if (!IsVisible) return false;
            if (UseDynamicVisibility)
            {
                if (DynamicVisibilityMode == DynamicVisibilityMode.ZoomedIn)
                {
                    if (geographicExtent.Width > DynamicVisibilityWidth)
                    {
                        return false;  // skip the geoLayer if we are zoomed out too far.
                    }
                }
                else
                {
                    if (geographicExtent.Width < DynamicVisibilityWidth)
                    {
                        return false;  // skip the geoLayer if we are zoomed in too far.
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Tests the specified legend item.  If the item is another layer or a group or a map-frame, then this
        /// will return false.  Furthermore, if the parent of the item is not also this object, then it will
        /// also return false.  The idea is that layers can have sub-nodes move around, but not transport from
        /// place to place.
        /// </summary>
        /// <param name="item">the legend item to test</param>
        /// <returns>Boolean that if true means that it is ok to insert the specified item into this layer.</returns>
        public override bool CanReceiveItem(ILegendItem item)
        {
            if (item.GetParentItem() != this) return false;
            ILayer lyr = item as ILayer;
            if (lyr != null) return false;
            IFrame mf = item as IFrame;
            if (mf != null) return false;
            IGroup gr = item as IGroup;
            if (gr != null) return false;
            return true;
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
        /// Notifies the layer that the next time an area that intersects with this region
        /// is specified, it must first re-draw content to the image buffer.
        /// </summary>
        /// <param name="region">The envelope where content has become invalidated.</param>
        public virtual void Invalidate(Extent region)
        {
            if (_invalidatedRegion != null)
            {
                // This is set to null when we do the redrawing, so we would rather expand
                // the redraw region than forget to update a modified area.
                _invalidatedRegion.ExpandToInclude(region);
            }
            else
            {
                _invalidatedRegion = region;
            }
        }

        /// <summary>
        /// Notifies parent layer that this item is invalid and should be redrawn.
        /// </summary>
        public override void Invalidate()
        {
            OnItemChanged(this);
        }

        private void SetDynamicVisibility(object sender, EventArgs e)
        {
            var la = LayerActions;
            if (la != null)
            {
                la.DynamicVisibility(this, MapFrame);
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets custom actions for Layer
        /// </summary>
        [Browsable(false)]
        public ILayerActions LayerActions { get; set; }

        /// <summary>
        /// Gets or sets the internal data set.  This can be null, as in the cases of groups or map-frames.
        /// Copying a layer should not create a duplicate of the dataset, but rather it should point to the
        /// original dataset.  The ShallowCopy attribute is used so even though the DataSet itself may be cloneable,
        /// cloning a layer will treat the dataset like a shallow copy.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public IDataSet DataSet
        {
            get { return _dataSet; }
            set
            {
                if (_dataSet == value) return;
                if (_dataSet != null)
                {
                    _dataSet.UnlockDispose();
                    if (!_dataSet.IsDisposeLocked)
                    {
                        _dataSet.Dispose();
                    }
                }
                _dataSet = value;
                _dataSet.LockDispose();
            }
        }

        /// <summary>
        /// Dynamic visibility represents layers that only appear when you zoom in close enough.
        /// This value represents the geographic width where that happens.
        /// </summary>
        [Serialize("DynamicVisibilityWidth")]
        [Category("Behavior"), Description("Dynamic visibility represents layers that only appear when the zoom scale is closer (or further) from a set scale.  This value represents the geographic width where the change takes place.")]
        public double DynamicVisibilityWidth
        {
            get { return _dynamicVisibilityWidth; }
            set { _dynamicVisibilityWidth = value; }
        }

        /// <summary>
        /// This controls whether the layer is visible when zoomed in closer than the dynamic
        /// visibility width or only when further away from the dynamic visibility width
        /// </summary>
        [Serialize("DynamicVisibilityMode")]
        [Category("Behavior"), Description("This controls whether the layer is visible when zoomed in closer than the dynamic visiblity width or only when further away from the dynamic visibility width")]
        public DynamicVisibilityMode DynamicVisibilityMode
        {
            get { return _dynamicVisibilityMode; }
            set { _dynamicVisibilityMode = value; }
        }

        /// <summary>
        /// Gets the currently invalidated region.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Extent InvalidRegion
        {
            get { return _invalidatedRegion; }
            protected set { _invalidatedRegion = value; }
        }

        /// <summary>
        /// Gets the map frame of the parent LayerCollection.
        /// </summary>
        [Browsable(false), ShallowCopy]
        public virtual IFrame MapFrame
        {
            get
            {
                return _mapFrame;
            }
            set
            {
                _mapFrame = value;
            }
        }

        /// <summary>
        /// Gets or sets the ProgressHandler for this layer.  Setting this overrides the default
        /// behavior which is to use the
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IProgressHandler ProgressHandler
        {
            get
            {
                if (_progressHandler != null) return _progressHandler;
                return DataManager.DefaultDataManager.ProgressHandler;
            }
            set
            {
                _progressHandler = value;
                if (_progressMeter == null) _progressMeter = new ProgressMeter(_progressHandler);
                _progressMeter.ProgressHandler = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether to allow the dynamic visibility
        /// envelope to control visibility.
        /// </summary>
        [Serialize("UseDynamicVisibility")]
        [Category("Behavior"), Description("Gets or sets a boolean indicating whether to allow the dynamic visibility envelope to control visibility.")]
        public bool UseDynamicVisibility
        {
            get { return _useDynamicVisibility; }
            set { _useDynamicVisibility = value; }
        }

        /// <inheritdoc />
        [Category("Behavior"), Description("Gets or sets a boolean indicating whether this layer is selected in the legend.")]
        public override bool IsSelected
        {
            get
            {
                return base.IsSelected;
            }
            set
            {
                if (base.IsSelected != value)
                {
                    OnLayerSelected(this, value);
                    base.IsSelected = value;
                }
            }
        }

        /// <inheritdoc />
        [Serialize("IsVisible")]
        [Category("Behavior"), Description("Gets or sets a boolean indicating whether this layer is visible in the map.")]
        public override bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                base.IsVisible = value;
            }
        }

        #endregion Properties

        #region Protected Methods

        /// <summary>
        /// Fires the zoom to layer event
        /// </summary>
        protected virtual void OnZoomToLayer()
        {
            if (ZoomToLayer == null) return;
            IEnvelope env = Extent.ToEnvelope();
            ZoomToLayer(this, new EnvelopeArgs(env));
        }

        /// <summary>
        /// Fires the zoom to layer event, but specifies the extent.
        /// </summary>
        /// <param name="env">IEnvelope env</param>
        protected virtual void OnZoomToLayer(IEnvelope env)
        {
            if (ZoomToLayer != null)
            {
                ZoomToLayer(this, new EnvelopeArgs(env));
            }
        }

        #endregion Protected Methods

        #region EventHandlers

        private void ExportDataClick(object sender, EventArgs e)
        {
            OnExportData();
        }

        private void ShowPropertiesClick(object sender, EventArgs e)
        {
            // Allow subclasses to prevent this class from showing the default dialog
            HandledEventArgs result = new HandledEventArgs(false);
            OnShowProperties(result);
            if (result.Handled) return;

            if (_propertyDialogProvider == null) return;
            _editCopy = this.Copy();
            CopyProperties(_editCopy); // for some reason we are getting blank layers during edits, this tries to fix that

            _propertyDialogProvider.ShowDialog(_editCopy);
            LayerManager.DefaultLayerManager.ActiveProjectLayers = new List<ILayer>();
        }

        #endregion EventHandlers

        #region Protected Methods

        /// <summary>
        /// Layers launch a "Property Grid" by default.  However, this can be overridden with a different UIEditor by this
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IPropertyDialogProvider PropertyDialogProvider
        {
            get
            {
                return _propertyDialogProvider;
            }
            protected set
            {
                if (_propertyDialogProvider != null)
                {
                    _propertyDialogProvider.ChangesApplied -= PropertyDialogProviderChangesApplied;
                }
                _propertyDialogProvider = value;
                if (_propertyDialogProvider != null)
                {
                    base.ContextMenuItems.Add(new SymbologyMenuItem(SymbologyMessageStrings.Layer_Properties, ShowPropertiesClick));
                    _propertyDialogProvider.ChangesApplied += PropertyDialogProviderChangesApplied;
                }
            }
        }

        /// <summary>
        /// Occurs before showing the properties dialog.  If the handled member
        /// was set to true, then this class will not show the event args.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnShowProperties(HandledEventArgs e)
        {
            if (ShowProperties != null) ShowProperties(this, e);
        }

        /// <summary>
        /// This should be overridden to copy the symbolizer properties from editCopy
        /// </summary>
        /// <param name="editCopy">The version that went into the property dialog</param>
        protected override void OnCopyProperties(object editCopy)
        {
            ILayer layer = editCopy as ILayer;
            if (layer != null)
            {
                SuspendChangeEvent();
                base.OnCopyProperties(editCopy);
                ResumeChangeEvent();
            }
        }

        /// <summary>
        /// Zooms to the specific layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomToLayerClick(object sender, EventArgs e)
        {
            OnZoomToLayer();
        }

        /// <summary>
        /// Removes this layer from its parent list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLayerClick(object sender, EventArgs e)
        {
            OnRemoveItem();
        }

        /// <summary>
        /// Fires the LayerSelected event
        /// </summary>
        protected virtual void OnLayerSelected(ILayer sender, bool selected)
        {
            if (LayerSelected == null) return;
            LayerSelected(this, new LayerSelectedEventArgs(sender, selected));
        }

        /// <summary>
        /// Fires the OnFinishedLoading event.
        /// </summary>
        protected void OnFinishedLoading()
        {
            if (FinishedLoading != null) FinishedLoading(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when instructions are being sent for this layer to export data.
        /// </summary>
        protected virtual void OnExportData()
        {
        }

        /// <summary>
        /// special treatment for event handlers during a copy event
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Descriptor copy)
        {
            // Remove event handlers from the copy. (They will be set again when adding to a new map.)
            Layer copyLayer = copy as Layer;
            if (copyLayer == null) return;
            if (copyLayer.LayerSelected != null)
            {
                foreach (var handler in copyLayer.LayerSelected.GetInvocationList())
                {
                    copyLayer.LayerSelected -= (EventHandler<LayerSelectedEventArgs>)handler;
                }
            }
            if (copyLayer.ZoomToLayer != null)
            {
                foreach (var handler in copyLayer.ZoomToLayer.GetInvocationList())
                {
                    copyLayer.ZoomToLayer -= (EventHandler<EnvelopeArgs>)handler;
                }
            }
            if (copyLayer.ShowProperties != null)
            {
                foreach (var handler in copyLayer.ShowProperties.GetInvocationList())
                {
                    copyLayer.ShowProperties -= (HandledEventHandler)handler;
                }
            }
            if (copyLayer.FinishedLoading != null)
            {
                foreach (var handler in copyLayer.FinishedLoading.GetInvocationList())
                {
                    copyLayer.FinishedLoading -= (EventHandler)handler;
                }
            }
            if (copyLayer.SelectionChanged != null)
            {
                foreach (var handler in copyLayer.SelectionChanged.GetInvocationList())
                {
                    copyLayer.SelectionChanged -= (EventHandler)handler;
                }
            }
            base.OnCopy(copy);
        }

        #endregion Protected Methods

        #region Protected Properties

        /// <summary>
        /// Gets or sets the progress meter being used internally by layer classes.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected ProgressMeter ProgressMeter
        {
            get { return _progressMeter ?? (_progressMeter = new ProgressMeter(ProgressHandler)); }
            set { _progressMeter = value; }
        }

        #endregion Protected Properties

        #region Static Methods

        /// <summary>
        /// Opens a fileName using the default layer provider and returns a new layer.  The layer will not automatically have a container or be added to a map.
        /// </summary>
        /// <param name="fileName">The string fileName of the layer to open</param>
        /// <returns>An ILayer interface</returns>
        public static ILayer OpenFile(string fileName)
        {
            if (File.Exists(fileName) == false) return null;
            return LayerManager.DefaultLayerManager.OpenLayer(fileName);
        }

        /// <summary>
        /// Opens a fileName using the default layer provider and returns a new layer.  The layer will not automatically have a container or be added to a map.
        /// </summary>
        /// <param name="fileName">The string fileName of the layer to open</param>
        /// <param name="progressHandler">An IProgresshandler that overrides the Default Layer Manager's progress handler</param>
        /// <returns>An ILayer interface with the new layer.</returns>
        public static ILayer OpenFile(string fileName, IProgressHandler progressHandler)
        {
            return File.Exists(fileName) == false ? null : LayerManager.DefaultLayerManager.OpenLayer(fileName, progressHandler);
        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the specified container.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="container">The container (usually a LayerCollection) to add to</param>
        /// <returns>The layer after it has been created and added to the container</returns>
        public static ILayer OpenFile(string fileName, ICollection<ILayer> container)
        {
            if (File.Exists(fileName) == false) return null;
            ILayerManager dm = LayerManager.DefaultLayerManager;
            return dm.OpenLayer(fileName, container);
        }

        /// <summary>
        /// Attempts to call the open fileName method for any ILayerProvider plugin
        /// that matches the extension on the string.
        /// </summary>
        /// <param name="fileName">A String fileName to attempt to open.</param>
        /// <param name="inRam">A boolean value that if true will attempt to force a load of the data into memory.  This value overrides the property on this LayerManager.</param>
        /// <param name="container">A container to open this layer in</param>
        /// <param name="progressHandler">Specifies the progressHandler to receive progress messages.  This value overrides the property on this LayerManager.</param>
        /// <returns>An ILayer</returns>
        public virtual ILayer OpenLayer(string fileName, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            if (File.Exists(fileName) == false) return null;
            ILayerManager dm = LayerManager.DefaultLayerManager;
            return dm.OpenLayer(fileName, inRam, container, progressHandler);
        }

        #endregion Static Methods

        /// <summary>
        /// Gets or sets a boolean indicating whether the memory objects have already been disposed of.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposed
        {
            get { return _isDisposed; }
            set { _isDisposed = value; }
        }

        #region ILayer Members

        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool ClearSelection(out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool Select(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// This is overriden in sub-classes
        /// </summary>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode"></param>
        /// <param name="affectedArea"></param>
        /// <returns></returns>
        public virtual bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = null;
            return false;
        }

        /// <summary>
        /// Gets or sets the boolean that controls whether or not items from the layer can be selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SelectionEnabled
        {
            get
            {
                return _selectionEnabled;
            }
            set
            {
                _selectionEnabled = value;
            }
        }

        /// <summary>
        /// Disposes the memory objects in this layer.
        /// </summary>
        public void Dispose()
        {
            // This is a new feature, so may be buggy if someone is calling dispose without checking the lock.
            // This will help find those spots in debug mode but will cross fingers and hope for the best
            // in release mode rather than throwing an exception.
            Debug.Assert(IsDisposeLocked == false);
            Dispose(true);
        }

        /// <summary>
        /// Locks dispose.  This typically adds one instance of an internal reference counter.
        /// </summary>
        public void LockDispose()
        {
            _disposeLockCount++;
        }

        /// <summary>
        /// Gets a value indicating whether an existing reference is requesting that the object is not disposed of.
        /// Automatic disposal, as is the case when a layer is removed from the map, will not take place until
        /// all the locks on dispose have been removed.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposeLocked
        {
            get { return _disposeLockCount > 0; }
        }

        /// <summary>
        /// Unlocks dispose.  This typically removes one instance of an internal reference counter.
        /// </summary>
        public void UnlockDispose()
        {
            _disposeLockCount--;
        }

        /// <summary>
        /// Gets a Boolean indicating if this layer can be reprojected.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanReproject
        {
            get
            {
                if (DataSet != null) return DataSet.CanReproject;
                return false;
            }
        }

        /// <summary>
        /// Gets the or sets the projection information for the dataset of this layer.
        /// This only defines the projection information and does not reproject the dataset or the layer.
        /// </summary>
        [Category("DataSet Properties"), Description("The geographic projection that this raster is using.")]
        public ProjectionInfo Projection
        {
            get
            {
                if (DataSet != null) return DataSet.Projection;
                return _projection;
            }
            set
            {
                if (DataSet != null)
                {
                    DataSet.Projection = value;
                }
                _projection = value;
            }
        }

        /// <summary>
        /// Gets or sets the unmodified projection string that can be used regardless of whether the
        /// DotSpatial.Projection module is available. This string can be in the Proj4string or in the
        /// EsriString format. Setting the Projection string only defines projection information.
        /// Call the Reproject() method to actually reproject the dataset and layer.
        /// </summary>
        [Category("DataSet Properties"), Description("The geographic projection that this raster is using.")]
        public string ProjectionString
        {
            get
            {
                if (DataSet != null) return DataSet.ProjectionString;
                return _projectionString;
            }
            set
            {
                if (DataSet != null)
                {
                    DataSet.ProjectionString = value;
                    return;
                }
                _projectionString = value;
                if (Data.DataSet.ProjectionSupported())
                {
                    ProjectionInfo test = ProjectionInfo.FromProj4String(value);
                    if (!test.IsValid)
                    {
                        test.TryParseEsriString(value);
                    }
                    if (test.IsValid) Projection = test;
                }
            }
        }

        /// <summary>
        /// Reprojects the dataset for this layer.
        /// </summary>
        /// <param name="targetProjection">The target projection to use.</param>
        public virtual void Reproject(ProjectionInfo targetProjection)
        {
            if (DataSet != null)
            {
                if (ProgressHandler != null) ProgressHandler.Progress(String.Empty, 0, "Reprojecting Layer " + LegendText);
                DataSet.Reproject(targetProjection);
                if (ProgressHandler != null) ProgressHandler.Progress(String.Empty, 0, String.Empty);
            }
        }

        #endregion

        /// <summary>
        /// Fires the SelectionChanged event
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
        }

        /// <summary>
        /// Finalizes an instance of the Layer class.
        /// </summary>
        ~Layer()
        {
            Dispose(false);
        }

        /// <summary>
        /// This allows overriding layers to handle any memory cleanup.
        /// </summary>
        /// <param name="disposeManagedResources">True if managed resources should be set to null.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (_isDisposed)
            {
                return;
            }
            if (disposeManagedResources)
            {
                LayerSelected = null;
                ZoomToLayer = null;
                ShowProperties = null;
                FinishedLoading = null;
                SelectionChanged = null;
                base.ContextMenuItems = null;
                MyExtent = null;
                base.LegendText = null;
                _progressHandler = null;
                _progressMeter = null;
                _invalidatedRegion = null;
                _mapFrame = null;
                _propertyDialogProvider = null;
            }
            // Since the InnerDataset likely contains unmanaged memory constructs, dispose of it here.
            if (_dataSet != null)
            {
                _dataSet.UnlockDispose();
                if (!_dataSet.IsDisposeLocked)
                {
                    _dataSet.Dispose();
                }
                _dataSet = null;
            }
            if (_editCopy != null)
            {
                _editCopy.Dispose();
                _editCopy = null;
            }
            _isDisposed = true;
        }
    }
}