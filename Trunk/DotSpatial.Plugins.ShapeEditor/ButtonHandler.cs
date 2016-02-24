// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.ShapeEditor.dll
// Description:  The data access libraries for the DotSpatial project.
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
        private IHeaderControl _Header;
        private IFeatureLayer _activeLayer;
        private SimpleActionItem _addShape;
        private AddShapeFunction _addShapeFunction;
        private bool _disposed;

        private IMap _geoMap;
        private MoveVertexFunction _moveVertexFunction;

        private bool _doSnapping = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonHandler"/> class.
        /// </summary>
        /// <param name="manager">The app manager.</param>
        public ButtonHandler(AppManager manager)
        {
            if (manager.HeaderControl == null)
                throw new ArgumentNullException("manager", "A HeaderControl must be available through the AppManager.");

            _Header = manager.HeaderControl;
            AddButtons();
        }

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
                if (_geoMap != null && _geoMap.Layers != null && _geoMap.Layers.SelectedLayer != null)
                {
                    SetActiveLayer(_geoMap.Layers.SelectedLayer);
                }
                if (_geoMap != null && _geoMap.Layers != null)
                {
                    _geoMap.Layers.LayerSelected += Layers_LayerSelected;
                    _geoMap.MapFrame.LayerSelected += MapFrame_LayerSelected;
                    _geoMap.MapFrame.LayerRemoved += MapFrameOnLayerRemoved;
                }
            }
        }

        private void MapFrameOnLayerRemoved(object sender, LayerEventArgs e)
        {
            if (e.Layer == _activeLayer) _activeLayer = null;
        }

        /// <summary>
        /// Gets a value indicating whether the "dispose" method has been called.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        #region IDisposable Members

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

        #endregion

        /// <summary>
        /// Sets up the handler to respond to buttons pressed on a ribbon or toolbar.
        /// </summary>
        private void AddButtons()
        {
            const string shapeEditorMenuKey = "kShapeEditor";

            //_Header.Add(new RootItem(ShapeEditorMenuKey, "Shape Editing"));
            _Header.Add(new SimpleActionItem(shapeEditorMenuKey, ShapeEditorResources.New, NewButton_Click) { GroupCaption = "Shape Editor", SmallImage = ShapeEditorResources.NewShapefile.ToBitmap(), RootKey = HeaderControl.HomeRootItemKey });
            _addShape = new SimpleActionItem(shapeEditorMenuKey, ShapeEditorResources.Add_Shape, AddShapeButton_Click) { GroupCaption = "Shape Editor", SmallImage = ShapeEditorResources.NewShape.ToBitmap(), RootKey = HeaderControl.HomeRootItemKey };
            _Header.Add(_addShape);
            _Header.Add(new SimpleActionItem(shapeEditorMenuKey, ShapeEditorResources.Move_Vertex, MoveVertexButton_Click) { GroupCaption = "Shape Editor", SmallImage = ShapeEditorResources.move, RootKey = HeaderControl.HomeRootItemKey });
            _Header.Add(new SimpleActionItem(shapeEditorMenuKey, ShapeEditorResources.Snapping, SnappingButton_Click) { GroupCaption = "Shape Editor", SmallImage = ShapeEditorResources.SnappingIcon.ToBitmap(), RootKey = HeaderControl.HomeRootItemKey });
        }

        private void SnappingButton_Click(object sender, EventArgs e)
        {
            SnapSettingsDialog dlg = new SnapSettingsDialog { DoSnapping = _doSnapping };
            dlg.ShowDialog();
            _doSnapping = dlg.DoSnapping;
            if (_moveVertexFunction != null) _moveVertexFunction.DoSnapping = _doSnapping; // changed by jany_ (2016-02-24) update the snap settings of the functions without having to stop editing
            if (_addShapeFunction != null) _addShapeFunction.DoSnapping = _doSnapping;
        }

        private void MoveVertexButton_Click(object sender, EventArgs e)
        {
            if (_geoMap == null) { return; }
            if (_activeLayer == null) { return; }
            if (_moveVertexFunction == null)
            {
                _moveVertexFunction = new MoveVertexFunction(_geoMap) { Name = "MoveVertex" };
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

        private void MapFrame_LayerSelected(object sender, LayerSelectedEventArgs e)
        {
            if (!e.IsSelected && e.Layer == _activeLayer)
            {
                _activeLayer = null;
                if (_moveVertexFunction != null)
                {
                    _moveVertexFunction.ClearSelection(); // changed by jany_ (2016-02-24) make sure highlighted features are reset too to prevent exception
                }
                return;
            }

            _activeLayer = e.Layer as IFeatureLayer;
            if (_activeLayer == null) { return; }

            if (_moveVertexFunction != null)
                UpdateMoveVertexFunctionLayer();
            if (_addShapeFunction != null) // changed by jany_ (2016-02-24) update both because moveFeature might not be the active function just because it is not null
                UpdateAddShapeFunctionLayer();
        }

        private void UpdateMoveVertexFunctionLayer()
        {
            _moveVertexFunction.ClearSelection(); // changed by jany_ (2016-02-24) make sure highlighted features are reset too to prevent exception
            _moveVertexFunction.Layer = _activeLayer;
            SetSnapLayers(_moveVertexFunction);
        }

        private void UpdateAddShapeFunctionLayer()
        {
            _addShapeFunction.Layer = _activeLayer;
            SetSnapLayers(_addShapeFunction);
        }

        private void AddShapeButton_Click(object sender, EventArgs e)
        {
            if (_geoMap == null) { return; }

            if (_geoMap.Layers.SelectedLayer != null)
            {
                _activeLayer = _geoMap.Layers.SelectedLayer as IFeatureLayer;
            }
            if (_activeLayer == null) { return; }

            if (_addShapeFunction == null)
            {
                _addShapeFunction = new AddShapeFunction(_geoMap) { Name = "AddShape" };
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

        private void NewButton_Click(object sender, EventArgs e)
        {
            FeatureTypeDialog dlg = new FeatureTypeDialog();
            if (dlg.ShowDialog() != DialogResult.OK) { return; }
            FeatureSet fs = new FeatureSet(dlg.FeatureType);
            if (_geoMap.Projection != null) { fs.Projection = _geoMap.Projection; }
            fs.CoordinateType = dlg.CoordinateType;
            fs.IndexMode = false;
            IMapFeatureLayer layer;
            if (!String.IsNullOrWhiteSpace(dlg.Filename))
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
            layer.LegendText = !String.IsNullOrEmpty(layer.DataSet.Name) ? layer.DataSet.Name : _geoMap.Layers.UnusedName("New Layer");
        }

        private void Layers_LayerSelected(object sender, LayerSelectedEventArgs e)
        {
            SetActiveLayer(e.Layer);
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

        /// <summary>
        /// Finalizes an instance of the ButtonHandler class.
        /// </summary>
        ~ButtonHandler()
        {
            Dispose(false);
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
    }
}