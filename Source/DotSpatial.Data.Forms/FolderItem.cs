// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// FolderItem
    /// </summary>
    internal class FolderItem : DirectoryItem
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderItem"/> class.
        /// </summary>
        public FolderItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderItem"/> class that points to the specified path.
        /// </summary>
        /// <param name="path">The string path that this FolderItem should be identified with</param>
        public FolderItem(string path)
            : base(path)
        {
            ItemType = ItemType.Folder;
            Info = new DirectoryInfo(path);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the directory info
        /// </summary>
        public DirectoryInfo Info { get; set; }

        #endregion
    }
}