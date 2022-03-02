// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// A feature set for SpatiaLite data.
    /// </summary>
    public class SpatiaLiteFeatureSet : FeatureSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpatiaLiteFeatureSet"/> class.
        /// </summary>
        /// <param name="fType">The feature type of the contained features.</param>
        public SpatiaLiteFeatureSet(FeatureType fType)
            : base(fType)
        {
        }

        /// <summary>
        /// Gets or sets the name of the layer that is contained in the FeatureSet.
        /// </summary>
        [Serialize("LayerName", ConstructorArgumentIndex = 1, UseCase = SerializeAttribute.UseCases.StaticMethodOnly)]
        public string LayerName { get; set; }

        /// <summary>
        /// Gets a value indicating whether the serializer should use the static open method or the constructor on deserializing the FeatureSet.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        [Serialize("UseStaticOpenMethod", UseStaticConstructor = true, StaticConstructorMethodName = "OpenLayer")]
        public override bool UseStaticOpenMethod => true;

        /// <summary>
        /// This static method is only meant to be used by the deserializer.
        /// </summary>
        /// <param name="filePath">Path of the file that contains the layer that gets loaded.</param>
        /// <param name="layerName">Name of the layer that should be loaded.</param>
        /// <returns>The opened layer.</returns>
        public static IFeatureSet OpenLayer(string filePath, string layerName)
        {
            var connectionString = SqLiteHelper.GetSqLiteConnectionString(filePath);

            var file = SpatiaLiteHelper.Open(connectionString, out string error);

            if (file == null) throw new FileLoadException(string.Format(Resources.DatabaseNotValid, filePath, error));

            var fs = file.ReadFeatureSet(layerName);
            return fs;
        }

        /// <summary>
        /// Gets the feature at the given index.
        /// </summary>
        /// <param name="index">Index of the feature that should be returned.</param>
        /// <returns>The feature belonging to the given index.</returns>
        public override IFeature GetFeature(int index)
        {
            var res = base.GetFeature(index);
            if (res.DataRow == null)
            {
                res.DataRow = DataTable.Rows[index];
            }

            return res;
        }
    }
}