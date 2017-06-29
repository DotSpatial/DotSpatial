// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    public class FolderItem : DirectoryItem
    {
        #region Private Variables

        private DirectoryInfo _info;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FolderItem
        /// </summary>
        public FolderItem()
        {
        }

        /// <summary>
        /// Creates a new instance of FolderItem, but already pointing to the specified path
        /// </summary>
        /// <param name="path">The string path that this FolderItem should be identified with</param>
        public FolderItem(string path)
            : base(path)
        {
            ItemType = ItemType.Folder;
            _info = new DirectoryInfo(path);
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the directory info
        /// </summary>
        public DirectoryInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        #endregion
    }
}