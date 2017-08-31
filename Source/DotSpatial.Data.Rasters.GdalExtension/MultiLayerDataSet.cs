// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLayerDataSet"/> class.
        /// </summary>
        public MultiLayerDataSet()
        {
            DataSets = new List<IDataSet>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLayerDataSet"/> class that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public MultiLayerDataSet(string fileName)
            : this()
        {
            Open(fileName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of the DataSets that are contained in this MultiLayerDataSet.
        /// </summary>
        public IList<IDataSet> DataSets { get; }

        /// <summary>
        /// Gets or sets the name that should be shown as the LegendText of the MapSelfLoadGroup returned by GetLayer.
        /// </summary>
        public string GroupName { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public new void Dispose()
        {
            foreach (var dataset in DataSets)
            {
                dataset.Dispose();
            }

            DataSets.Clear();
            base.Dispose();
        }

        /// <summary>
        /// Gets the MapSelfLoadGroup that contains all the DataSets as Layers and can be added to the map.
        /// </summary>
        /// <returns>Null, if there a no DataSets otherwise the MapSelfLoadGroup with the DataSets.</returns>
        public IMapSelfLoadLayer GetLayer()
        {
            if (DataSets.Count < 1) return null;

            if (DataSets.Count > 1)
            {
                // return a group with all the layers
                var gr = new MapSelfLoadGroup
                {
                    LegendText = GroupName,
                    DataSet = this,
                    FilePath = _fileName
                };

                foreach (var ds in DataSets)
                {
                    gr.Layers.Add(ds);
                }

                return gr;
            }

            var fs = DataSets[0] as IFeatureSet;
            if (fs == null) return null;

            switch (fs.FeatureType)
            {
                case FeatureType.MultiPoint:
                case FeatureType.Point:
                    return new MapSelfLoadPointLayer(fs);

                case FeatureType.Line:
                    return new MapSelfLoadLineLayer(fs);

                case FeatureType.Polygon:
                    return new MapSelfLoadPolygonLayer(fs);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">Name of the file that should be opened.</param>
        public void Open(string fileName)
        {
            Dispose();

            _fileName = fileName;
            GroupName = Path.GetFileNameWithoutExtension(fileName);

            using (var reader = new OgrDataReader(fileName))
            {
                foreach (var layer in reader.GetLayers())
                {
                    DataSets.Add(layer);
                }
            }
        }

        /// <inheritdoc />
        public new void Reproject(ProjectionInfo targetProjection)
        {
            foreach (var ds in DataSets)
            {
                ds.Reproject(targetProjection);
            }
        }

        #endregion
    }
}