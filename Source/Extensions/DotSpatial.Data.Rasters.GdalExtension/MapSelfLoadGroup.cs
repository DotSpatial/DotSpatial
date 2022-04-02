// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.IO;
using System.Linq;
using DotSpatial.Controls;
using DotSpatial.Serialization;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// This layer can be used to add groups to the map that load themselves on deserialization.
    /// </summary>
    public class MapSelfLoadGroup : MapGroup, IMapGroup, IMapSelfLoadLayer
    {
        #region Fields

        private ISelfLoadSet _dataSet;

        private string _fileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelfLoadGroup"/> class.
        /// </summary>
        public MapSelfLoadGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelfLoadGroup"/> class.
        /// </summary>
        /// <param name="filePath">The path of the file that containes the layer data.</param>
        public MapSelfLoadGroup(string filePath)
        {
            _dataSet = DataManager.DefaultDataManager.OpenFile(filePath) as ISelfLoadSet;

            if (_dataSet?.GetLayer() is MapSelfLoadGroup layer)
            {
                Layers = layer.Layers;
                FilePath = filePath;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the internal data set. This can be null, as in the cases of groups or map-frames.
        /// Copying a layer should not create a duplicate of the dataset, but rather it should point to the
        /// original dataset. The ShallowCopy attribute is used so even though the DataSet itself may be cloneable,
        /// cloning a layer will treat the dataset like a shallow copy.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ISelfLoadSet DataSet
        {
            get
            {
                return _dataSet;
            }

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
                _dataSet?.LockDispose();
            }
        }

        /// <summary>
        /// Gets or sets the file name of a file based data set. The file name should be the absolute path including
        /// the file extension. For data sets coming from a database or a web service, the Filename property is NULL.
        /// </summary>
        public string Filename
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = string.IsNullOrEmpty(value) ? null : Path.GetFullPath(value);
            }
        }

        /// <summary>
        /// Gets or sets the current file path. This is the relative path relative to the current project folder.
        /// For data sets coming from a database or a web service, the FilePath property is NULL.
        /// This property is used when saving source file information to a DSPX project.
        /// </summary>
        [Serialize("FilePath", ConstructorArgumentIndex = 0, UseCase = SerializeAttribute.UseCases.StaticMethodOnly)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FilePath
        {
            get
            {
                // do not construct FilePath for DataSets without a Filename
                return string.IsNullOrEmpty(Filename) ? null : FilePathUtils.RelativePathTo(Filename);
            }

            set
            {
                Filename = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of layers.
        /// </summary>
        public new IMapLayerCollection Layers
        {
            get
            {
                return base.Layers;
            }

            set
            {
                if (base.Layers != null)
                {
                    IgnoreLayerEvents(base.Layers);
                }

                HandleLayerEvents(value);
                base.Layers = value;

                // set the MapFrame property
                if (ParentMapFrame != null)
                {
                    base.Layers.MapFrame = ParentMapFrame;
                }
            }
        }

        /// <summary>
        /// Gets a list that contains the names of the layers in this group. This is used to load only those layers when deserializing a dspx file.
        /// </summary>
        [Serialize("LayerNames", ConstructorArgumentIndex = 1, UseCase = SerializeAttribute.UseCases.StaticMethodOnly)]
        public string[] LayerNames
        {
            get
            {
                return Layers.Select(_ => _.DataSet.Name).ToArray();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the serializer should use the static open method or the constructor on deserializing the MapSelfLoadGroup.
        /// Do not remove this. This is used to write the relevant informations to the dspx.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        [Serialize("UseStaticOpenMethod", UseStaticConstructor = true, StaticConstructorMethodName = "Open")]
        private bool UseStaticOpenMethod => true;

        // ReSharper restore UnusedMember.Local
        #endregion

        #region Methods

        /// <summary>
        /// This static method is only meant to be used by the deserializer. It allows the deserializer to reload the group layer with the layers that were contained on serializing.
        /// </summary>
        /// <param name="filePath">Path of the file that contains the group that gets loaded.</param>
        /// <param name="layerNames">The names of the layers that should be loaded.</param>
        /// <returns>The opened group.</returns>
        public static MapSelfLoadGroup Open(string filePath, string[] layerNames)
        {
            var dataset = DataManager.DefaultDataManager.OpenFile(filePath) as ISelfLoadSet;
            var layer = dataset?.GetLayer(layerNames) as MapSelfLoadGroup;
            return layer;
        }

        #endregion
    }
}