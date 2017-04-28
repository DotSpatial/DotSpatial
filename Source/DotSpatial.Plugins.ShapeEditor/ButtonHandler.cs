// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.ShapeEditor.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/11/2009 11:03:39 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
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
        private SimpleActionItem _addShape;
        private AddShapeFunction _addShapeFunction;
        private bool _disposed;

        private bool _doSnapping = true;

        private IMap _geoMap;
        private MoveVertexFunction _moveVertexFunction;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonHandler"/> class.
        /// </summary>
        /// <param name="manager">The app manager.</param>
        public ButtonHandler(AppManager manager)
        {
            if (manager.HeaderControl == null)
                throw new ArgumentNullException(nameof(manager), MessageStrings.HeaderControlMustBeAvailable);

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

            // _Header.Add(new RootItem(ShapeEditorMenuKey, "Shape Editing"));
            _header.Add(new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.New, NewButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.NewShapefile.ToBitmap(),
                RootKey = HeaderControl.HomeRootItemKey
            });
            _addShape = new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.Add_Shape, AddShapeButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.NewShape.ToBitmap(),
                RootKey = HeaderControl.HomeRootItemKey
            };
            _header.Add(_addShape);
            _header.Add(new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.Move_Vertex, MoveVertexButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.move,
                RootKey = HeaderControl.HomeRootItemKey
            });
            _header.Add(new SimpleActionItem(ShapeEditorMenuKey, ShapeEditorResources.Snapping, SnappingButtonClick)
            {
                GroupCaption = "Shape Editor",
                SmallImage = ShapeEditorResources.SnappingIcon.ToBitmap(),
                RootKey = HeaderControl.HomeRootItemKey
            });
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
                _addShapeFunction = new AddShapeFunction(_geoMap)
                {
                    Name = "AddShape"
                };
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
                return;
            }

            if (_moveVertexFunction != null)
                UpdateMoveVertexFunctionLayer();
            if (_addShapeFunction != null) // changed by jany_ (2016-02-24) update both because moveFeature might not be the active function just because it is not null
                UpdateAddShapeFunctionLayer();
        }

        private void MapFrameOnLayerRemoved(object sender, LayerEventArgs e)
        {
            if (e.Layer == _activeLayer) _activeLayer = null;
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
                _moveVertexFunction = new MoveVertexFunction(_geoMap)
                {
                    Name = "MoveVertex"
                };
            }

            if (_geoMap.MapFunctions.Contains(_moveVertexFunction) == false)
            {
                _geoMap.MapFunctions.Add(_moveVertexFunction);
            }

            _geoMap.FunctionMode = FunctionMode.None;
            _geoMap.Cursor = Cursors.Cross;
            _moveVertexFunction.Layer = _activeLayer;
            UpdateMoveVertexFunctionLayer();
            _moveVertexFunction.Activate();
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
                _addShape.Enabled = false;
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

        private void SnappingButtonClick(object sender, EventArgs e)
        {
            SnapSettingsDialog dlg = new SnapSettingsDialog
            {
                DoSnapping = _doSnapping
            };
            dlg.ShowDialog();
            _doSnapping = dlg.DoSnapping;
            if (_moveVertexFunction != null) _moveVertexFunction.DoSnapping = _doSnapping; // changed by jany_ (2016-02-24) update the snap settings of the functions without having to stop editing
            if (_addShapeFunction != null) _addShapeFunction.DoSnapping = _doSnapping;
        }

        private void UpdateAddShapeFunctionLayer()
        {
            _addShapeFunction.Layer = _activeLayer;
            SetSnapLayers(_addShapeFunction);
        }

        private void UpdateMoveVertexFunctionLayer()
        {
            _moveVertexFunction.ClearSelection(); // changed by jany_ (2016-02-24) make sure highlighted features are reset too to prevent exception
            _moveVertexFunction.Layer = _activeLayer;
            SetSnapLayers(_moveVertexFunction);
        }

        #endregion
    }
}