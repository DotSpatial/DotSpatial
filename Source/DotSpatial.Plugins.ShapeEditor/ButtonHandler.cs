// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// Organizes how the buttons will be displayed and what happens when pressing different buttons.
    /// </summary>
    public class ButtonHandler : IDisposable
    {
        #region Fields

        private readonly IHeaderControl _header;
        private IFeatureLayer _activeLayer;
        private SimpleActionItem _newShapeFile;
        private SimpleActionItem _addShape;
        private SimpleActionItem _moveVertex;
        private AddShapeFunction _addShapeFunction;
        private SimpleActionItem _snapping;
        private bool _disposed;

        private bool _doSnapping = true;
        private bool _doShapeDragging = false;
        private bool _doShowVerticesIndex = true;

        private IMap _geoMap;
        private CheckBoxActionItem _snappingActivCheck;
        private MoveVertexFunction _moveVertexFunction;
        private SimpleActionItem _shapeDraggingActivButton;
        private CheckBoxActionItem _showVerticesCheck;
        private AppManager _manager;
        private CultureInfo _handlerCulture;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonHandler"/> class.
        /// </summary>
        /// <param name="manager">The app manager.</param>
        public ButtonHandler(AppManager manager)
        {
            if (manager == null || manager.HeaderControl == null)
                throw new ArgumentNullException(nameof(manager), MessageStrings.HeaderControlMustBeAvailable);

            _manager = manager;
            _header = manager.HeaderControl;
            AddButtons();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ButtonHandler"/> class.
        /// </summary>
        ~ButtonHandler()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the "dispose" method has been called.
        /// </summary>
        public bool IsDisposed => _disposed;

        /// <summary>
        /// Gets or sets the 2D Geographic map to use with this feature editing toolkit.
        /// </summary>
        public IMap Map
        {
            get
            {
                return _geoMap;
            }

            set
            {
                _geoMap = value;
                if (_geoMap?.Layers?.SelectedLayer != null)
                {
                    SetActiveLayer(_geoMap.Layers.SelectedLayer);
                }

                if (_geoMap?.Layers != null)
                {
                    _geoMap.Layers.LayerSelected += LayersLayerSelected;
                    _geoMap.MapFrame.LayerSelected += MapFrameLayerSelected;
                    _geoMap.MapFrame.LayerRemoved += MapFrameOnLayerRemoved;
                }
            }
        }

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo HandlerCulture
        {
            set
            {
                if (_handlerCulture == value) return;

                _handlerCulture = value;

                if (_handlerCulture == null) _handlerCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _handlerCulture;
                Thread.CurrentThread.CurrentUICulture = _handlerCulture;

                if (_addShapeFunction != null) _addShapeFunction.AddShapeCulture = _handlerCulture;
                if (_moveVertexFunction != null) _moveVertexFunction.MoveVertexCulture = _handlerCulture;

                UpdateHandlerItems();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Actually, this creates disposable items but doesn't own them.
        /// When the ribbon disposes it will remove the items.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // This exists to prevent FX Cop from complaining.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Act to activate a button on the hanlder.
        /// </summary>
        /// <param name="button_name"> Button to activate.</param>
        public void Buttons_Activate(string button_name)
        {
            switch (button_name)
            {
                case "Shape_Dragging":
                    _doShapeDragging = true;
                    UpdateShapeDraggingActivButton();
                    break;

                case "Feature_Snapping":
                    _doSnapping = false;
                    UpdateSnappingActivCheck();
                    break;
            }
        }

        /// <summary>
        /// Act to deactivate a button on the hanlder.
        /// </summary>
        /// <param name="button_name"> Button to dactivate.</param>
        public void Buttons_Deactivate(string button_name)
        {
            switch (button_name)
            {
                case "Shape_Dragging":
                    _doShapeDragging = false;
                    UpdateShapeDraggingActivButton();
                    break;

                case "Feature_Snapping":
                    _doSnapping = false;
                    UpdateSnappingActivCheck();
                    break;
            }
        }

        /// <summary>
        /// Act to disable a button on the hanlder.
        /// </summary>
        /// <param name="button_name"> Button to dactivate.</param>
        public void Buttons_Disable(string button_name)
        {
            switch (button_name)
            {
                case "Shape_Dragging":
                    _doShapeDragging = false;
                    _shapeDraggingActivButton.Enabled = false;
                    UpdateShapeDraggingActivButton();
                    break;
            }
        }

        /// <summary>
        /// Disposes this handler, removing any buttons that it is responsible for adding.
        /// </summary>
        /// <param name="disposeManagedResources">Disposes of the resources.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (!_disposed)
            {
                // One option would be to leave the non-working tools,
                // but if this gets disposed we should clean up after
                // ourselves and remove any added controls.
                RemoveControls();

                if (disposeManagedResources)
                {
                    if (_addShapeFunction != null && !_addShapeFunction.IsDisposed)
                    {
                        _addShapeFunction.Dispose();
                    }

                    _geoMap = null;
                    _activeLayer = null;
                    _moveVertexFunction = null;
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Sets up the handler to respond to buttons pressed on a ribbon or toolbar.
        /// </summary>
        private void AddButtons()
        {
            const string ShapeEditorMenuKey = "kShapeEditor";

            SeparatorItem separatorMove = new SeparatorItem(ShapeEditorMenuKey, "Shape Editor");
            SeparatorItem separatorSnap = new SeparatorItem(ShapeEditorMenuKey, "Shape Editor");

            _header.Add(new RootItem(ShapeEditorMenuKey, "Shape Editing"));
            _newShapeFile = new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.New, NewButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.NewShapefile.ToBitmap(),
                RootKey = HeaderControl.HomeRootItemKey
            };
            _header.Add(_newShapeFile);

            _addShape = new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.Add_Shape, AddShapeButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.NewShape.ToBitmap(),
                RootKey = HeaderControl.HomeRootItemKey
            };
            _header.Add(_addShape);

            _header.Add(separatorMove);
            _moveVertex = new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.Move_Vertex, MoveVertexButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.move,
                RootKey = HeaderControl.HomeRootItemKey
            };
            _header.Add(_moveVertex);

            _shapeDraggingActivButton = new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.ShapeDragging_InActive, ShapeDraggingActivButtonClick)
            {
                GroupCaption = "Shape Editor",
                Enabled = false,
                SmallImage = System.Drawing.SystemIcons.Asterisk.ToBitmap()
            };
            _header.Add(_shapeDraggingActivButton);

            _showVerticesCheck = new CheckBoxActionItem(ShapeEditorMenuKey, ShapeEditorResources.ShowVerticesIndex_Label, ShowVerticesActivCheckChanged)
            {
                GroupCaption = "Shape Editor",
                Caption = ShapeEditorResources.ShowVerticesIndex_Label,
                Checked = false,
                ToolTipText = ShapeEditorResources.ShowVerticesIndex_InActive,
                RootKey = HeaderControl.HomeRootItemKey
            };
            _showVerticesCheck.CheckedChanged += ShowVerticesActivCheckChanged;

            _header.Add(_showVerticesCheck);

            _header.Add(separatorSnap);
            _snapping = new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.Snapping_Settings, SnappingButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.SnappingIcon.ToBitmap(),
                RootKey = HeaderControl.HomeRootItemKey
            };
            _header.Add(_snapping);

            _snappingActivCheck = new CheckBoxActionItem(ShapeEditorMenuKey, ShapeEditorResources.Snapping_Label, SnappingActivCheckChanged)
            {
                GroupCaption = "Shape Editor",
                Caption = ShapeEditorResources.Snapping_Label,
                Checked = true,
                ToolTipText = ShapeEditorResources.Snapping_Active,
                RootKey = HeaderControl.HomeRootItemKey
            };
            _snappingActivCheck.CheckedChanged += SnappingActivCheckChanged;
            _header.Add(_snappingActivCheck);
        }

        private void AddShapeButtonClick(object sender, EventArgs e)
        {
            if (_geoMap == null)
            {
                return;
            }

            if (_geoMap.Layers.SelectedLayer != null)
            {
                _activeLayer = _geoMap.Layers.SelectedLayer as IFeatureLayer;
            }

            if (_activeLayer == null)
            {
                return;
            }

            if (_addShapeFunction == null)
            {
                _addShapeFunction = new AddShapeFunction(_geoMap, this)
                {
                    Name = "AddShape"
                };
                _addShapeFunction.AddShapeCulture = _handlerCulture;
            }

            if (_geoMap.MapFunctions.Contains(_addShapeFunction) == false)
            {
                _geoMap.MapFunctions.Add(_addShapeFunction);
            }

            _geoMap.FunctionMode = FunctionMode.None;
            _geoMap.Cursor = Cursors.Hand;
            UpdateAddShapeFunctionLayer();
            _addShapeFunction.Activate();
        }

        private void LayersLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            SetActiveLayer(e.Layer);
        }

        private void MapFrameLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            if (!e.IsSelected && e.Layer == _activeLayer)
            {
                _activeLayer = null;
                _moveVertexFunction?.ClearSelection(); // changed by jany_ (2016-02-24) make sure highlighted features are reset too to prevent exception

                return;
            }

            _activeLayer = e.Layer as IFeatureLayer;
            if (_activeLayer == null)
            {
                if (_moveVertexFunction != null)
                    _moveVertexFunction.Deactivate();
                if (_addShapeFunction != null)
                    _addShapeFunction.Deactivate();
                _geoMap.FunctionMode = FunctionMode.Pan;
                _geoMap.Cursor = Cursors.Hand;
                return;
            }

            if (_moveVertexFunction != null)
                UpdateMoveVertexFunctionLayer();
            if (_addShapeFunction != null) // changed by jany_ (2016-02-24) update both because moveFeature might not be the active function just because it is not null
                UpdateAddShapeFunctionLayer();
        }

        private void MapFrameOnLayerRemoved(object sender, LayerEventArgs e)
        {
            if (e.Layer == _activeLayer)
            {
                // _activeLayer = null;
                SetActiveLayer(null);
                RemoveSnapLayer(e.Layer);
            }
        }

        private void MoveVertexButtonClick(object sender, EventArgs e)
        {
            if (_geoMap == null)
            {
                return;
            }

            if (_activeLayer == null)
            {
                return;
            }

            if (_moveVertexFunction == null)
            {
                _moveVertexFunction = new MoveVertexFunction(_geoMap, this)
                {
                    Name = "MoveVertex"
                };
                _moveVertexFunction.MoveVertexCulture = _handlerCulture;
            }

            if (_geoMap.MapFunctions.Contains(_moveVertexFunction) == false)
            {
                _geoMap.MapFunctions.Add(_moveVertexFunction);
            }

            _geoMap.FunctionMode = FunctionMode.None;
            _geoMap.Cursor = Cursors.Cross;
            _shapeDraggingActivButton.Enabled = true;
            _moveVertexFunction.Layer = _activeLayer;
            UpdateMoveVertexFunctionLayer();
            _moveVertexFunction.Activate();
            UpdateShowVerticesActivCheck();
        }

        private void NewButtonClick(object sender, EventArgs e)
        {
            FeatureTypeDialog dlg = new FeatureTypeDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            FeatureSet fs = new FeatureSet(dlg.FeatureType);
            if (_geoMap.Projection != null)
            {
                fs.Projection = _geoMap.Projection;
            }

            fs.CoordinateType = dlg.CoordinateType;
            fs.IndexMode = false;
            IMapFeatureLayer layer;
            if (!string.IsNullOrWhiteSpace(dlg.Filename))
            {
                fs.SaveAs(dlg.Filename, true);
                layer = (IMapFeatureLayer)_geoMap.Layers.Add(dlg.Filename);
            }
            else
            {
                layer = _geoMap.Layers.Add(fs);
            }

            layer.EditMode = true;
            _geoMap.Layers.SelectedLayer = layer;
            layer.LegendText = !string.IsNullOrEmpty(layer.DataSet.Name) ? layer.DataSet.Name : _geoMap.Layers.UnusedName("New Layer");
        }

        /// <summary>
        /// Any controls that were added to display members will be removed.
        /// </summary>
        private void RemoveControls()
        {
            // Todo: restore cursor if necessary.
            if (_addShapeFunction != null && _geoMap != null)
            {
                if (_geoMap.MapFunctions.Contains(_addShapeFunction))
                {
                    _geoMap.MapFunctions.Remove(_addShapeFunction);
                }
            }

            if (_moveVertexFunction != null && _geoMap != null)
            {
                if (_geoMap.MapFunctions.Contains(_moveVertexFunction))
                {
                    _geoMap.MapFunctions.Remove(_moveVertexFunction);
                }
            }
        }

        private void SetActiveLayer(ILayer layer)
        {
            IFeatureLayer fl = layer as IFeatureLayer;
            if (fl == null)
            {
                _activeLayer = null;
                _addShape.Enabled = false;
                if (_moveVertexFunction != null)
                {
                    _moveVertexFunction.ClearSelection(); // To prevent nullReference Exception when occrus MouseMove.
                    UpdateMoveVertexFunctionLayer();
                }

                if (_addShapeFunction != null)
                {
                    _addShapeFunction.DeleteShape(null, null); // To clean the map while there is pending shape editing.
                    UpdateAddShapeFunctionLayer();
                }
            }
            else
            {
                _activeLayer = fl;
                _addShape.Enabled = true;
            }
        }

        private void SetSnapLayers(SnappableMapFunction func)
        {
            func.DoSnapping = _doSnapping;
            if (!_doSnapping)
                return;

            foreach (var fl in _geoMap.GetFeatureLayers())
            {
                func.AddLayerToSnap(fl); // changed by jany_ (2016-02-24) allow all layers to be snapped because there seems to be no reason to exclude any of them
            }
        }

        private void RemoveSnapLayer(ILayer fl)
        {
            IFeatureLayer flRemoved = fl as IFeatureLayer;
            if (flRemoved == null) return;

            if (_addShapeFunction != null) _addShapeFunction.RemoveLayerFromSnap(flRemoved);
            if (_moveVertexFunction != null) _moveVertexFunction.RemoveLayerFromSnap(flRemoved);
        }

        private void ShowVerticesActivCheckChanged(object sender, EventArgs e)
        {
            _doShowVerticesIndex = _showVerticesCheck.Checked;

            UpdateShowVerticesActivCheck();
        }

        private void UpdateShowVerticesActivCheck()
        {
            _showVerticesCheck.Caption = ShapeEditorResources.ShowVerticesIndex_Label;

            switch (_doShowVerticesIndex)
            {
                case true:
                    _showVerticesCheck.ToolTipText = ShapeEditorResources.ShowVerticesIndex_Active;
                    break;
                case false:
                    _showVerticesCheck.ToolTipText = ShapeEditorResources.ShowVerticesIndex_InActive;
                    break;
            }

            if (_moveVertexFunction != null)
            {
                _moveVertexFunction.DoShowVerticesIndex = _doShowVerticesIndex;
                _moveVertexFunction.Map.Invalidate();
            }
        }

        private void SnappingActivCheckChanged(object sender, EventArgs e)
        {
            _doSnapping = _snappingActivCheck.Checked;

            UpdateSnappingActivCheck();
        }

        private void UpdateSnappingActivCheck()
        {
            _snappingActivCheck.Caption = ShapeEditorResources.Snapping_Label;

            switch (_doSnapping)
            {
                case true:
                    _snappingActivCheck.ToolTipText = ShapeEditorResources.Snapping_Active;
                    break;
                case false:
                    _snappingActivCheck.ToolTipText = ShapeEditorResources.Snapping_InActive;
                    break;
            }

            if (_moveVertexFunction != null)
            {
                _moveVertexFunction.DoSnapping = _doSnapping; // changed by jany_ (2016-02-24) update the snap settings of the functions without having to stop editing
            }
        }

        private void ShapeDraggingActivButtonClick(object sender, EventArgs e)
        {
            if (_moveVertexFunction == null || !_moveVertexFunction.Enabled) return;

            _doShapeDragging = !_doShapeDragging;
            _moveVertexFunction.DoShapeDragging = _doShapeDragging;

            UpdateShapeDraggingActivButton();
        }

        private void UpdateShapeDraggingActivButton()
        {
            switch (_doShapeDragging)
            {
                case true:
                    _shapeDraggingActivButton.Caption = ShapeEditorResources.ShapeDragging_Active;
                    _shapeDraggingActivButton.ToolTipText = ShapeEditorResources.ShapeDragging_Active;
                    _shapeDraggingActivButton.SmallImage = System.Drawing.SystemIcons.Hand.ToBitmap();
                    Map.Cursor = Cursors.UpArrow;
                    break;
                case false:
                    _shapeDraggingActivButton.Caption = ShapeEditorResources.ShapeDragging_InActive;
                    _shapeDraggingActivButton.ToolTipText = ShapeEditorResources.ShapeDragging_InActive;
                    _shapeDraggingActivButton.SmallImage = System.Drawing.SystemIcons.Asterisk.ToBitmap();
                    if (_moveVertexFunction != null && _moveVertexFunction.Enabled) Map.Cursor = Cursors.Cross;
                    break;
            }
        }

        private void SnappingButtonClick(object sender, EventArgs e)
        {
            using (SnapSettingsDialog dlg = new SnapSettingsDialog(_geoMap)
            {
                DoSnapping = _doSnapping,
                SnappCulture = _handlerCulture
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _doSnapping = dlg.DoSnapping;
                    _snappingActivCheck.Checked = _doSnapping;
                    UpdateSnappingActivCheck();
                }
            }
        }

        private void UpdateAddShapeFunctionLayer()
        {
            // _addShapeFunction.DoEdgeSnapping = _doEdgeSnapping;
            _addShapeFunction.Layer = _activeLayer;
            SetSnapLayers(_addShapeFunction);
        }

        private void UpdateMoveVertexFunctionLayer()
        {
            if (_moveVertexFunction != null && !_moveVertexFunction.Enabled) _doShapeDragging = false;

            if (_moveVertexFunction.Enabled) _geoMap.Cursor = Cursors.Cross;
            _moveVertexFunction.DoShapeDragging = _doShapeDragging;
            UpdateShapeDraggingActivButton();
            _moveVertexFunction.ClearSelection(); // changed by jany_ (2016-02-24) make sure highlighted features are reset too to prevent exception
            _moveVertexFunction.Layer = _activeLayer;
            SetSnapLayers(_moveVertexFunction);
        }

        private void UpdateHandlerItems()
        {
            _newShapeFile.Caption = ShapeEditorResources.New;
            _newShapeFile.ToolTipText = ShapeEditorResources.New;

            _addShape.Caption = ShapeEditorResources.Add_Shape;
            _addShape.ToolTipText = ShapeEditorResources.Add_Shape;

            _moveVertex.Caption = ShapeEditorResources.Move_Vertex;
            _moveVertex.ToolTipText = ShapeEditorResources.Move_Vertex;

            _snapping.Caption = ShapeEditorResources.Snapping_Settings;
            _snapping.ToolTipText = ShapeEditorResources.Snapping_Settings;

            UpdateSnappingActivCheck();
            UpdateShapeDraggingActivButton();
            UpdateShowVerticesActivCheck();
        }

        #endregion
    }
}