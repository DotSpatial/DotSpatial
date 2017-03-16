// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2008 10:26:54 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Represents collection of map layers.
    /// </summary>
    public class MapLayerCollection : LayerCollection, IMapLayerCollection
    {
        #region Events

        /// <summary>
        /// Occurs when the stencil for one of the layers has been updated in a specific region.
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Private Variables

        private IProgressHandler _progressHandler;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new blank instance of a MapLayer collection.  This is especially useful
        /// for tracking layers that can draw themselves.  This does not concern itself with
        /// view extents like a dataframe, but rather is a grouping of layers that is itself
        /// also an IMapLayer.
        /// </summary>
        public MapLayerCollection(IMapFrame containerFrame, IProgressHandler progressHandler)
        {
            base.MapFrame = containerFrame;
            base.ParentGroup = containerFrame;
            _progressHandler = progressHandler;
        }

        /// <summary>
        /// Creates the Collection in the situation where the map frame is not the immediate parent,
        /// but rather the group is the immediate parent, while frame is the ultimate map frame that
        /// contains this geo layer collection.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="group"></param>
        /// <param name="progressHandler"></param>
        public MapLayerCollection(IMapFrame frame, IMapGroup group, IProgressHandler progressHandler)
        {
            base.MapFrame = frame;
            ParentGroup = group;
            _progressHandler = progressHandler;
        }

        /// <summary>
        /// Creates a new blank instance of a MapLayer collection.  This is especially useful
        /// for tracking layers that can draw themselves.  This does not concern itself with
        /// view extents like a dataframe, but rather is a grouping of layers that is itself
        /// also an IMapLayer.
        /// </summary>
        public MapLayerCollection(IMapFrame containerFrame)
        {
            base.MapFrame = containerFrame;
            ParentGroup = containerFrame;
        }

        /// <summary>
        /// Creates a new layer collection that is free-floating.  This will not be contained in a map frame.
        /// </summary>
        public MapLayerCollection()
        {
        }

        /// <summary>
        /// Gets the map frame of this layer collection
        /// </summary>
        public new IMapFrame MapFrame
        {
            get
            {
                return base.MapFrame as IMapFrame;
            }
            set
            {
                base.MapFrame = value;
            }
        }

        #endregion

        #region Methods

        #region Add

        /// <summary>
        /// This overload automatically constructs a new MapLayer from the specified
        /// feature layer with the default drawing characteristics and returns a valid
        /// IMapLayer which can be further cast into a PointLayer, MapLineLayer or
        /// a PolygonLayer, depending on the data that is passed in.
        /// </summary>
        /// <param name="layer">A pre-existing FeatureLayer that has already been created from a featureSet</param>
        public void Add(IMapLayer layer)
        {
            base.Add(layer);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.List&lt;T&gt;
        /// </summary>
        /// <param name="collection">collection: The collection whose elements should be added to the end of the
        /// System.Collections.Generic.List&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null,
        /// if type T is a reference type.</param>
        /// <exception cref="System.ApplicationException">Unable to add while the ReadOnly property is set to true.</exception>
        public void AddRange(IEnumerable<IMapLayer> collection)
        {
            foreach (IMapLayer layer in collection)
            {
                base.Add(layer);
            }
        }

        /// <summary>
        /// Adds the specified fileName to the map as a new layer.
        /// </summary>
        /// <param name="fileName">The string fileName to add as a layer.</param>
        /// <returns>An IMapLayer that is the layer handle for the specified file.</returns>
        public virtual IMapLayer Add(string fileName)
        {
            IDataSet dataSet = DataManager.DefaultDataManager.OpenFile(fileName);
            return Add(dataSet);
        }

        /// <summary>
        /// Adds the dataset specified to the file.  Depending on whether this is a featureSet,
        /// Raster, or ImageData, this will return the appropriate layer for the map.
        /// </summary>
        /// <param name="dataSet">A dataset</param>
        /// <returns>The IMapLayer to add</returns>
        public virtual IMapLayer Add(IDataSet dataSet)
        {
            var fs = dataSet as IFeatureSet;
            if (fs != null) return Add(fs);
            var r = dataSet as IRaster;
            if (r != null) return Add(r);
            var id = dataSet as IImageData;
            if (id != null) return Add(id);
            return null;
        }

        /// <summary>
        /// This overload automatically constructs a new MapLayer from the specified
        /// feature layer with the default drawing characteristics and returns a valid
        /// IMapLayer which can be further cast into a PointLayer, MapLineLayer or
        /// a PolygonLayer, depending on the data that is passed in.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics</param>
        /// <returns>A newly created valid implementation of FeatureLayer which at least gives a few more common
        /// drawing related methods and can also be cast into the appropriate Point, Line or Polygon layer.</returns>
        public virtual IMapFeatureLayer Add(IFeatureSet featureSet)
        {
            if (featureSet == null) return null;

            featureSet.ProgressHandler = ProgressHandler;
            IMapFeatureLayer res = null;
            if (featureSet.FeatureType == FeatureType.Point || featureSet.FeatureType == FeatureType.MultiPoint)
            {
                res = new MapPointLayer(featureSet);
            }
            else if (featureSet.FeatureType == FeatureType.Line)
            {
                res = new MapLineLayer(featureSet);
            }
            else if (featureSet.FeatureType == FeatureType.Polygon)
            {
                res = new MapPolygonLayer(featureSet);
            }
            if (res != null)
            {
                base.Add(res);
                res.ProgressHandler = ProgressHandler;
            }

            return res;
        }


        /// <summary>
        /// Adds the raster to layer collection
        /// </summary>
        /// <param name="raster"></param>
        /// <returns></returns>
        public virtual IMapRasterLayer Add(IRaster raster)
        {
            if (raster == null) return null;

            raster.ProgressHandler = ProgressHandler;
            var gr = new MapRasterLayer(raster);
            Add(gr);
            return gr;
        }

        /// <summary>
        /// Adds the specified image data as a new layer to the map.
        /// </summary>
        /// <param name="image">The image to add as a layer</param>
        /// <returns>the IMapImageLayer interface for the layer that was added to the map.</returns>
        public IMapImageLayer Add(IImageData image)
        {
            if (image == null) return null;

            if (image.Height == 0 || image.Width == 0) return null;
            var il = new MapImageLayer(image);
            base.Add(il);
            return il;
        }

        #endregion

        /// <summary>
        /// This copies the members of this collection to the specified array index, but
        /// only if they match the IGeoLayer interface.  (Other kinds of layers can be
        /// added to this collection by casting it to a LayerCollection)
        /// </summary>
        /// <param name="inArray">The array of IGeoLayer interfaces to copy values to</param>
        /// <param name="arrayIndex">The zero-based integer index in the output array to start copying values to</param>
        public void CopyTo(IMapLayer[] inArray, int arrayIndex)
        {
            int index = arrayIndex;
            foreach (IMapLayer layer in this)
            {
                if (layer != null)
                {
                    inArray[index] = layer;
                    index++;
                }
            }
        }

        /// <summary>
        /// Tests to see if the specified IGeoLayer exists in the current collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(IMapLayer item)
        {
            return Contains(item as ILayer);
        }

        /// <summary>
        /// Gets the zero-based integer index of the specified IGeoLayer in this collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(IMapLayer item)
        {
            return IndexOf(item as ILayer);
        }

        /// <summary>
        /// Inserts an element into the System.Collections.Generic.List&lt;T&gt; at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-index is greater than System.Collections.Generic.List&lt;T&gt;.Count.</exception>
        /// <exception cref="System.ApplicationException">Unable to insert while the ReadOnly property is set to true.</exception>
        public void Insert(int index, IMapLayer item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// Moves the given layer to the given position.
        /// </summary>
        /// <param name="layer">Layer that gets moved.</param>
        /// <param name="newPosition">Position, the layer is moved to.</param>
        public void Move(IMapLayer layer, int newPosition)
        {
            base.Move(layer, newPosition);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.List&lt;T&gt;.
        /// </summary>
        /// <param name="item">The object to remove from the System.Collections.Generic.List&lt;T&gt;. The value can be null for reference types.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not
        /// found in the System.Collections.Generic.List&lt;T&gt;.</returns>
        /// <exception cref="System.ApplicationException">Unable to remove while the ReadOnly property is set to true.</exception>
        public bool Remove(IMapLayer item)
        {
            return base.Remove(item);
        }

        /// <summary>
        /// The default, indexed value of type T
        /// </summary>
        /// <param name="index">The numeric index</param>
        /// <returns>An object of type T corresponding to the index value specified</returns>
        public new IMapLayer this[int index]
        {
            get
            {
                return base[index] as IMapLayer;
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Inserts the elements of a collection into the EventList&lt;T&gt; at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the EventList&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0.-or-index is greater than EventList&lt;T&gt;.Count.</exception>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception>
        /// <exception cref="System.ApplicationException">Unable to insert while the ReadOnly property is set to true.</exception>
        public void InsertRange(int index, IEnumerable<IMapLayer> collection)
        {
            int i = index;
            foreach (IMapLayer gl in collection)
            {
                Insert(i, gl as ILayer);
                i++;
            }
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public new bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public new IMapGroup ParentGroup
        {
            get
            {
                return base.ParentGroup as IMapGroup;
            }
            set
            {
                base.ParentGroup = value;
            }
        }

        /// <summary>
        /// Gets or sets the progress handler to report progress for time consuming actions.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get
            {
                return _progressHandler;
            }
            set
            {
                _progressHandler = value;
            }
        }

        /// <inheritdoc />
        public new IMapLayer SelectedLayer
        {
            get
            {
                return base.SelectedLayer as IMapLayer;
            }
            set
            {
                base.SelectedLayer = value;
            }
        }

        #endregion

        #region IMapLayerCollection Members

        /// <inheritdoc />
        public new IEnumerator<IMapLayer> GetEnumerator()
        {
            return new MapLayerEnumerator(base.GetEnumerator());
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This simply forwards the call from a layer to the container
        /// of this collection (like a MapFrame).
        /// </summary>
        /// <param name="sender">The layer that actually changed</param>
        /// <param name="e"></param>
        protected virtual void OnBufferChanged(object sender, ClipArgs e)
        {
            if (BufferChanged != null) BufferChanged(sender, e);
        }

        #endregion

        #region Nested classes

        private class MapLayerEnumerator : IEnumerator<IMapLayer>
        {
            readonly IEnumerator<ILayer> _internalEnumerator;

            /// <summary>
            /// Creates a new instance of LayerEnumerator
            /// </summary>
            public MapLayerEnumerator(IEnumerator<ILayer> source)
            {
                _internalEnumerator = source;
            }

            #region IEnumerator<IMapLayer> Members

            /// <summary>
            /// Retrieves the current member as an ILegendItem
            /// </summary>
            public IMapLayer Current
            {
                get
                {
                    return _internalEnumerator.Current as IMapLayer;
                }
            }

            object IEnumerator.Current
            {
                get { return _internalEnumerator.Current; }
            }

            /// <summary>
            /// Calls the Dispose method
            /// </summary>
            public void Dispose()
            {
                _internalEnumerator.Dispose();
            }

            /// <summary>
            /// Moves to the next member
            /// </summary>
            /// <returns>boolean, true if the enumerator was able to advance</returns>
            public bool MoveNext()
            {
                while (_internalEnumerator.MoveNext())
                {
                    var result = _internalEnumerator.Current as IMapLayer;
                    if (result != null) return true;
                }
                return false;
            }

            /// <summary>
            /// Resets to before the first member
            /// </summary>
            public void Reset()
            {
                _internalEnumerator.Reset();
            }

            #endregion
        }

        #endregion
    }
}