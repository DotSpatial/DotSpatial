// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.IO;
using DotSpatial.Controls;
using DotSpatial.Serialization;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// This MapLineLayer is able to load itself on deserialization even if the contained DataSet doesn't know how to load the file.
    /// </summary>
    public class MapSelfLoadLineLayer : MapLineLayer, IMapSelfLoadLayer
    {
        #region Fields

        private string _fileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelfLoadLineLayer"/> class.
        /// </summary>
        /// <param name="featureSet">The FeatureSet that contains the layers data.</param>
        public MapSelfLoadLineLayer(IFeatureSet featureSet)
            : base(featureSet)
        {
            Filename = FeatureSet.Filename;
            DataSet.Filename = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the DataSet.
        /// </summary>
        public new IFeatureSet DataSet
        {
            get
            {
                return base.DataSet;
            }

            set
            {
                base.DataSet = value;
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
        /// Gets a value indicating whether the serializer should use the static open method or the constructor on deserializing the FeatureSet.
        /// This should only be true if a FeatureSet was added to the Map. Shapefiles can be loaded via constructor. Do not remove this. This is used to write the relevant informations to the dspx.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        [Serialize("UseStaticOpenMethod", UseStaticConstructor = true, StaticConstructorMethodName = "Open")]
        private bool UseStaticOpenMethod => true;

        // ReSharper restore UnusedMember.Local
        #endregion

        #region Methods

        /// <summary>
        /// This static method is only meant to be used by the deserializer.
        /// </summary>
        /// <param name="filePath">Path of the file that contains the layer that gets loaded.</param>
        /// <returns>The opened layer.</returns>
        public static MapSelfLoadLineLayer Open(string filePath)
        {
            var dataset = DataManager.DefaultDataManager.OpenFile(filePath) as ISelfLoadSet;
            var layer = dataset?.GetLayer() as MapSelfLoadLineLayer;
            return layer;
        }

        #endregion
    }
}