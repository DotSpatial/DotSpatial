// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/14/2008 2:23:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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