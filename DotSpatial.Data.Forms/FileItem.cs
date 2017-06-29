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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/14/2008 2:22:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.IO;
using DotSpatial.Topology;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// FileItem
    /// </summary>
    public class FileItem : DirectoryItem
    {
        #region Private Variables

        private FileInfo _info;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FileItem
        /// </summary>
        public FileItem()
        {
        }

        /// <summary>
        /// Creates a new insteance of a FileItem associated with the specified path.
        /// </summary>
        /// <param name="path">Gets or sets a string path</param>
        public FileItem(string path)
            : base(path)
        {
            IDataManager defDM = DataManager.DefaultDataManager;
            DataFormat df = defDM.GetFileFormat(path);
            if (df == DataFormat.Vector)
            {
                FeatureType ft = defDM.GetFeatureType(path);
                switch (ft)
                {
                    case FeatureType.Polygon: ItemType = ItemType.Polygon; break;
                    case FeatureType.Line: ItemType = ItemType.Line; break;
                    case FeatureType.Point: ItemType = ItemType.Point; break;
                    case FeatureType.MultiPoint: ItemType = ItemType.Point; break;
                    default: ItemType = ItemType.Custom; break;
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

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FileInfo
        /// </summary>
        public FileInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        #endregion
    }
}