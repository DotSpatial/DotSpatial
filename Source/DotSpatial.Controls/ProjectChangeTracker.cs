using System;
using System.Diagnostics;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The ProjectChangeTracker class is responsible for tracking changes in the current project.
    /// A change occurs if layers are added or removed, map view extent is changed, symbology is changed,
    /// if layers are selected or unselected or if layer visibility is changed.
    /// </summary>
    internal class ProjectChangeTracker
    {
        #region Fields

        // the main map where changes are tracked
        private IMap _map;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectChangeTracker"/> class.
        /// </summary>
        /// <param name="mainMap">Map used for tracking.</param>
        public ProjectChangeTracker(IMap mainMap)
        {
            Map = mainMap;
        }

        #endregion

        #region Events

        /// <summary>
        /// This event fires if some visible properties of the map such as view extent or
        /// map layer appearance is changed.
        /// </summary>
        public event EventHandler MapPropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map where changes are tracked
        /// </summary>
        public IMap Map
        {
            get
            {
                return _map;
            }

            set
            {
                if (_map != null)
                {
                    // remove previous event handlers
                    _map.MapFrame.ViewExtentsChanged -= MapFrameViewExtentsChanged;
                    _map.LayerAdded -= MapLayerAdded;
                    _map.MapFrame.LayerRemoved -= MapFrameLayerRemoved;
                    _map.MapFrame.LayerSelected -= MapFrameLayerSelected;
                    _map.MapFrame.Invalidated -= MapFrameInvalidated;
                }

                // attach new event handlers
                _map = value;

                if (_map != null)
                {
                    _map.MapFrame.ViewExtentsChanged += MapFrameViewExtentsChanged;
                    _map.LayerAdded += MapLayerAdded;
                    _map.MapFrame.LayerRemoved += MapFrameLayerRemoved;
                    _map.MapFrame.LayerSelected += MapFrameLayerSelected;
                    _map.MapFrame.Invalidated += MapFrameInvalidated;
                }
            }
        }

        #endregion

        #region Methods

        private void MapLayerAdded(object sender, LayerEventArgs e)
        {
            Debug.WriteLine("Layer Added");
            OnMapPropertyChanged();
        }

        private void MapFrameInvalidated(object sender, EventArgs e)
        {
            Debug.WriteLine("Map Frame Invalidated");
            OnMapPropertyChanged();
        }

        private void MapFrameLayerRemoved(object sender, LayerEventArgs e)
        {
            Debug.WriteLine("Layer Removed");
            OnMapPropertyChanged();
        }

        private void MapFrameLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            Debug.WriteLine("Layer Selected");
            OnMapPropertyChanged();
        }

        private void MapFrameViewExtentsChanged(object sender, ExtentArgs e)
        {
            Debug.WriteLine("View Extents Changed");
            OnMapPropertyChanged();
        }

        private void OnMapPropertyChanged()
        {
            MapPropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}