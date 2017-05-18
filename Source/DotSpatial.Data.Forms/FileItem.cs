// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// FileItem
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
        /// <param name="path">Gets or sets a string path</param>
        public FileItem(string path)
            : base(path)
        {
            IDataManager defDm = DataManager.DefaultDataManager;
            DataFormat df = defDm.GetFileFormat(path);
            if (df == DataFormat.Vector)
            {
                FeatureType ft = defDm.GetFeatureType(path);
                switch (ft)
                {
                    case FeatureType.Polygon:
                        ItemType = ItemType.Polygon;
                        break;
                    case FeatureType.Line:
                        ItemType = ItemType.Line;
                        break;
                    case FeatureType.Point:
                        ItemType = ItemType.Point;
                        break;
                    case FeatureType.MultiPoint:
                        ItemType = ItemType.Point;
                        break;
                    default:
                        ItemType = ItemType.Custom;
                        break;
                }
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
        /// Gets or sets the FileInfo
        /// </summary>
        public FileInfo Info { get; set; }

        #endregion
    }
}