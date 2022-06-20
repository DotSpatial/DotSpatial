// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// FileItem.
    /// </summary>
    internal class FileItem : DirectoryItem
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileItem"/> class.
        /// </summary>
        public FileItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileItem"/> class with the specified path.
        /// </summary>
        /// <param name="path">Gets or sets a string path.</param>
        public FileItem(string path)
            : base(path)
        {
            IDataManager defDm = DataManager.DefaultDataManager;
            DataFormat df = defDm.GetFileFormat(path);
            if (df == DataFormat.Vector)
            {
                FeatureType ft = defDm.GetFeatureType(path);
                ItemType = ft switch
                {
                    FeatureType.Polygon => ItemType.Polygon,
                    FeatureType.Line => ItemType.Line,
                    FeatureType.Point => ItemType.Point,
                    FeatureType.MultiPoint => ItemType.Point,
                    _ => ItemType.Custom,
                };
            }

            if (df == DataFormat.Raster)
            {
                ItemType = ItemType.Raster;
            }

            if (df == DataFormat.Image)
            {
                ItemType = ItemType.Image;
            }

            if (df == DataFormat.Custom)
            {
                ItemType = ItemType.Custom;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FileInfo.
        /// </summary>
        public FileInfo Info { get; set; }

        #endregion
    }
}