// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/4/2009 11:52:50 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public class MapGroup : Group, IMapGroup
    {
        #region Fields

        private IMapLayerCollection _layers;

        #endregion

        #region  Constructors

        /// <summary>
        /// Creates a new instance of GeoGroup
        /// </summary>
        public MapGroup()
        {
            Layers = new MapLayerCollection();
        }

        /// <summary>
        /// Creates a new group for the specified map.  This will place the group at the root level on the MapFrame.
        /// </summary>
        /// <param name="map">The map to add this group to.</param>
        /// <param name="name">The name to appear in the legend text.</param>
        public MapGroup(IMap map, string name)
            : base(map.MapFrame, map.ProgressHandler)
        {
            Layers = new MapLayerCollection(map.MapFrame, this, map.ProgressHandler);
            LegendText = name;
            map.Layers.Add(this);
        }

        /// <summary>
        /// Creates a group that sits in a layer list and uses the specified progress handler
        /// </summary>
        /// <param name="container">the layer list</param>
        /// <param name="frame"></param>
        /// <param name="progressHandler">the progress handler</param>
        public MapGroup(ICollection<IMapLayer> container, IMapFrame frame, IProgressHandler progressHandler)
            : base(frame, progressHandler)
        {
            Layers = new MapLayerCollection(frame, this, progressHandler);
            container.Add(this);
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public override int Count
        {
            get
            {
                return _layers.Count;
            }
        }

        /// <inheritdoc />
        public override bool EventsSuspended
        {
            get
            {
                return _layers.EventsSuspended;
            }
        }

        /// <inheritdoc />
        public override bool IsReadOnly
        {
            get
            {
                return _layers.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets the collection of Geographic drawing layers.
        /// </summary>
        /// <summary>
        /// Gets or sets the layers
        /// </summary>
        [Serialize("Layers")]
        public new IMapLayerCollection Layers
        {
            get
            {
                return _layers;
            }

            set
            {
                if (Layers != null)
                {
                    Ignore_Layer_Events(_layers);
                }

                Handle_Layer_Events(value);
                _layers = value;

                // set the MapFrame property
                if (ParentMapFrame != null)
                {
                    _layers.MapFrame = ParentMapFrame;
                }
            }
        }

        /// <summary>
        /// This is a different view of the layers cast as legend items.  This allows
        /// easier cycling in recursive legend code.
        /// </summary>
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                // Keep cast for 3.5 framework
                return _layers.Cast<ILegendItem>();
            }
        }

        /// <inheritdoc />
        public override IFrame MapFrame
        {
            get
            {
                return base.MapFrame;
            }

            set
            {
                base.MapFrame = value;
                if (_layers != null)
                {
                    IMapFrame newValue = value as IMapFrame;
                    if (newValue != null && _layers.MapFrame == null)
                    {
                        _layers.MapFrame = newValue;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the MapFrame that this group ultimately belongs to.  This may not
        /// be the immediate parent of this group.
        /// </summary>
        public IMapFrame ParentMapFrame
        {
            get
            {
                return MapFrame as IMapFrame;
            }

            set
            {
                MapFrame = value;
            }
        }

        #endregion

        #region Indexers

        /// <inheritdoc />
        public override ILayer this[int index]
        {
            get
            {
                return _layers[index];
            }

            set
            {
                IMapLayer ml = value as IMapLayer;
                _layers[index] = ml;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Add(ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            if (ml != null)
            {
                _layers.Add(ml);
            }
        }

        /// <inheritdoc />
        public override void Clear()
        {
            _layers.Clear();
        }

        /// <inheritdoc />
        public override bool Contains(ILayer item)
        {
            IMapLayer ml = item as IMapLayer;
            return ml != null && _layers.Contains(ml);
        }

        /// <inheritdoc />
        public override void CopyTo(ILayer[] array, int arrayIndex)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                array[i + arrayIndex] = _layers[i];
            }
        }

        /// <summary>
        /// This draws content from the specified geographic regions onto the specified graphics
        /// object specified by MapArgs.
        /// </summary>
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
            if (Layers == null) return;
            foreach (IMapLayer layer in Layers)
            {
                if (!layer.IsVisible) continue;
                if (layer.UseDynamicVisibility)
                {
                    if (layer.DynamicVisibilityMode == DynamicVisibilityMode.ZoomedIn)
                    {
                        if (MapFrame.ViewExtents.Width > layer.DynamicVisibilityWidth)
                        {
                            continue; // skip the geoLayer if we are zoomed out too far.
                        }
                    }
                    else
                    {
                        if (MapFrame.ViewExtents.Width < layer.DynamicVisibilityWidth)
                        {
                            continue; // skip the geoLayer if we are zoomed in too far.
                        }
                    }
                }

                layer.DrawRegions(args, regions);
            }
        }

        /// <inheritdoc />
        public override IEnumerator<ILayer> GetEnumerator()
        {
            return new MapLayerEnumerator(_layers.GetEnumerator());
        }

        /// <summary>
        /// Gets the layers cast as ILayer without any information about the actual drawing methods.
        /// This is useful for handling methods that my come from various types of maps.
        /// </summary>
        /// <returns>An enumerable collection of ILayer</returns>
        public override IList<ILayer> GetLayers()
        {
            return _layers.Cast<ILayer>().ToList();
        }

        /// <inheritdoc />
        public override int IndexOf(ILayer item)
        {
            IMapLayer ml = item as IMapLayer;
            if (ml != null)
            {
                return _layers.IndexOf(ml);
            }

            return -1;
        }

        /// <inheritdoc />
        public override void Insert(int index, ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            if (ml != null)
            {
                _layers.Insert(index, ml);
            }
        }

        /// <inheritdoc />
        public override bool Remove(ILayer layer)
        {
            IMapLayer ml = layer as IMapLayer;
            return ml != null && _layers.Remove(ml);
        }

        /// <inheritdoc />
        public override void RemoveAt(int index)
        {
            _layers.RemoveAt(index);
        }

        /// <inheritdoc />
        public override void ResumeEvents()
        {
            _layers.ResumeEvents();
        }

        /// <inheritdoc />
        public override void SuspendEvents()
        {
            _layers.SuspendEvents();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        /// <summary>
        /// Overrides the base CreateGroup method to ensure that new groups are GeoGroups.
        /// </summary>
        protected override void OnCreateGroup()
        {
            new MapGroup(Layers, ParentMapFrame, ProgressHandler)
            {
                LegendText = "New Group"
            };
        }

        #endregion

        #region Classes

        /// <summary>
        /// Transforms an IMapLayer enumerator into an ILayer Enumerator
        /// </summary>
        private class MapLayerEnumerator : IEnumerator<ILayer>
        {
            #region Fields

            private readonly IEnumerator<IMapLayer> _enumerator;

            #endregion

            #region  Constructors

            /// <summary>
            /// Creates a new instance of the MapLayerEnumerator
            /// </summary>
            /// <param name="subEnumerator"></param>
            public MapLayerEnumerator(IEnumerator<IMapLayer> subEnumerator)
            {
                _enumerator = subEnumerator;
            }

            #endregion

            #region Properties

            /// <inheritdoc />
            public ILayer Current
            {
                get
                {
                    return _enumerator.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return _enumerator.Current;
                }
            }

            #endregion

            #region Methods

            /// <inheritdoc />
            public void Dispose()
            {
                _enumerator.Dispose();
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            /// <inheritdoc />
            public void Reset()
            {
                _enumerator.Reset();
            }

            #endregion
        }

        #endregion
    }
}