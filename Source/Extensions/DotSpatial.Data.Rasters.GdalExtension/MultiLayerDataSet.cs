// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotSpatial.Controls;
using DotSpatial.Projections;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// A dataset that contains data for more more than one layer.
    /// </summary>
    public class MultiLayerDataSet : DataSet, ISelfLoadSet
    {
        #region Fields

        private string _fileName;

        /// <summary>
        /// The name that should be shown as the LegendText of the MapSelfLoadGroup returned by GetLayer.
        /// </summary>
        private string _groupName;

        /// <summary>
        /// This contains the featuresets and their corresponding style lists that are contained in this MultiLayerDataSet.
        /// </summary>
        private IDictionary<IFeatureSet, IList<string>> _dataSets;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLayerDataSet"/> class.
        /// </summary>
        public MultiLayerDataSet()
        {
            _dataSets = new Dictionary<IFeatureSet, IList<string>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLayerDataSet"/> class that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the MultiLayerDataSet to load.</param>
        public MultiLayerDataSet(string fileName)
            : this()
        {
            Open(fileName);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public new void Dispose()
        {
            foreach (var dataset in _dataSets)
            {
                dataset.Key.Dispose();
            }

            _dataSets.Clear();
            base.Dispose();
        }

        /// <summary>
        /// Gets the IMapSelfLoadLayer that contains all the DataSets as Layers and can be added to the map.
        /// </summary>
        /// <returns>Null, if there are no DataSets, a MapSelfLoadLayer if there is only 1 DataSet otherwise a MapSelfLoadGroup with layers for the existing DataSets.</returns>
        public IMapSelfLoadLayer GetLayer()
        {
            return GetLayer(null);
        }

        /// <summary>
        /// Gets the IMapSelfLoadLayer that contains all the DataSets as Layers and can be added to the map.
        /// </summary>
        /// <param name="layerNames">The names of the layers that should be loaded. If this is null all layers get loaded.</param>
        /// <returns>Null, if there are no DataSets, a MapSelfLoadLayer if there is only 1 DataSet otherwise a MapSelfLoadGroup with layers for the existing DataSets.</returns>
        public IMapSelfLoadLayer GetLayer(string[] layerNames)
        {
            if (_dataSets.Count < 1) return null;

            if (_dataSets.Count > 1)
            {
                // return a group with all the layers
                var gr = new MapSelfLoadGroup
                {
                    LegendText = _groupName,
                    DataSet = this,
                    FilePath = _fileName
                };

                foreach (var ds in _dataSets)
                {
                    if (layerNames == null || layerNames.Contains(ds.Key.Name))
                    {
                        IMapFeatureLayer l = gr.Layers.Add(ds.Key);
                        OgrDataReader.TranslateStyles(l, ds.Value);
                        l.IsExpanded = false;
                    }
                }

                return gr;
            }

            // return the single existing layer
            var fs = _dataSets.Keys.FirstOrDefault();
            if (fs == null || (layerNames != null && !layerNames.Contains(fs.Name))) return null;

            IMapSelfLoadLayer layer;

            switch (fs.FeatureType)
            {
                case FeatureType.MultiPoint:
                case FeatureType.Point:
                    layer = new MapSelfLoadPointLayer(fs);
                    break;
                case FeatureType.Line:
                    layer = new MapSelfLoadLineLayer(fs);
                    break;
                case FeatureType.Polygon:
                    layer = new MapSelfLoadPolygonLayer(fs);
                    break;
                default:
                    return null;
            }

            layer.LegendText = _groupName;
            OgrDataReader.TranslateStyles((IMapFeatureLayer)layer, _dataSets[fs]);
            layer.IsExpanded = false;
            return layer;
        }

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">Name of the file that should be opened.</param>
        public void Open(string fileName)
        {
            Dispose();

            _fileName = fileName;
            _groupName = Path.GetFileNameWithoutExtension(fileName);

            using var reader = new OgrDataReader(fileName);
            _dataSets = reader.GetLayers();
        }

        /// <inheritdoc />
        public new void Reproject(ProjectionInfo targetProjection)
        {
            foreach (var ds in _dataSets)
            {
                ds.Key.Reproject(targetProjection);
            }
        }

        #endregion
    }
}