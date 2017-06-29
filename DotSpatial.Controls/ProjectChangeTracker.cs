using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        //the main map where changes are tracked
        private IMap map;

        public ProjectChangeTracker(IMap mainMap)
        {
            Contract.Requires(mainMap != null);

            this.Map = mainMap;
        }

        /// <summary>
        /// Gets or sets the map where changes are tracked
        /// </summary>
        public IMap Map
        {
            get { return map; }
            set
            {
                if (map != null)
                {
                    //remove previous event handlers
                    map.MapFrame.ViewExtentsChanged -= MapFrame_ViewExtentsChanged;
                    map.LayerAdded -= map_LayerAdded;
                    map.MapFrame.LayerRemoved -= MapFrame_LayerRemoved;
                    map.MapFrame.LayerSelected -= MapFrame_LayerSelected;
                    map.MapFrame.Invalidated -= MapFrame_Invalidated;
                }

                //attach new event handlers
                map = value;

                if (map != null)
                {
                    map.MapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
                    map.LayerAdded += map_LayerAdded;
                    map.MapFrame.LayerRemoved += MapFrame_LayerRemoved;
                    map.MapFrame.LayerSelected += MapFrame_LayerSelected;
                    map.MapFrame.Invalidated += MapFrame_Invalidated;
                }
            }
        }

        private void MapFrame_Invalidated(object sender, EventArgs e)
        {
            Debug.WriteLine("Map Frame Invalidated");
            OnMapPropertyChanged();
        }

        private void MapFrame_LayerSelected(object sender, LayerSelectedEventArgs e)
        {
            Debug.WriteLine("Layer Selected");
            OnMapPropertyChanged();
        }

        private void MapFrame_LayerRemoved(object sender, LayerEventArgs e)
        {
            Debug.WriteLine("Layer Removed");
            OnMapPropertyChanged();
        }

        private void map_LayerAdded(object sender, LayerEventArgs e)
        {
            Debug.WriteLine("Layer Added");
            OnMapPropertyChanged();
        }

        private void MapFrame_ViewExtentsChanged(object sender, ExtentArgs e)
        {
            Debug.WriteLine("View Extents Changed");
            OnMapPropertyChanged();
        }

        private void OnMapPropertyChanged()
        {
            if (MapPropertyChanged != null)
                MapPropertyChanged(this, null);
        }

        /// <summary>
        /// This event fires if some visible properties of the map such as view extent or
        /// map layer appearance is changed.
        /// </summary>
        public event EventHandler MapPropertyChanged;
    }
}