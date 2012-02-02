// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/21/10 8:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Extend the data manager with some convenient dialog spawning options.
    /// </summary>
    public static class DataManagerExt
    {
        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog</returns>
        public static IFeatureSet OpenVector(this IDataManager self)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = self.VectorReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler) as IFeatureSet;
        }

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <returns>The list or rasters</returns>
        public static List<IFeatureSet> OpenVectors(this IDataManager self)
        {
            List<IFeatureSet> result = new List<IFeatureSet>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = self.VectorReadFilter;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IFeatureSet ds = self.OpenVector(name, self.LoadInRam, self.ProgressHandler);
                if (ds != null) result.Add(ds);
            }
            return result;
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>for now an IDataSet</returns>
        public static IImageData OpenImage(this IDataManager self)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = self.ImageReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler) as IImageData;
        }

        /// <summary>
        /// This uses an open dialog filter with only image extensions for supported image formats,
        /// but where multi-select is enabled, and so allowing multiple images to be returned at once.
        /// </summary>
        /// <returns></returns>
        public static List<IImageData> OpenImages(this IDataManager self)
        {
            List<IImageData> result = new List<IImageData>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = self.ImageReadFilter;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IImageData id = self.OpenImage(name, self.ProgressHandler);
                if (id != null) result.Add(id);
            }
            return result;
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        public static IDataSet OpenFile(this IDataManager self)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = self.DialogReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler);
        }

        /// <summary>
        /// This launches an open file dialog that allows loading of several files at once
        /// and returns the datasets in a list.
        /// </summary>
        /// <returns>A list of all the files that were opened</returns>
        public static List<IDataSet> OpenFiles(this IDataManager self)
        {
            List<IDataSet> result = new List<IDataSet>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = self.DialogReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IDataSet ds = self.OpenFile(name, self.LoadInRam, self.ProgressHandler);
                if (ds != null) result.Add(ds);
            }
            return result;
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        public static IRaster OpenRaster(this IDataManager self)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = self.RasterReadFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler) as IRaster;
        }

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <returns>The list or rasters</returns>
        public static List<IRaster> OpenRasters(this IDataManager self)
        {
            List<IRaster> result = new List<IRaster>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = self.RasterReadFilter;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            foreach (string name in ofd.FileNames)
            {
                IRaster ds = self.OpenRaster(name, self.LoadInRam, self.ProgressHandler);
                if (ds != null) result.Add(ds);
            }
            return result;
        }
    }
}