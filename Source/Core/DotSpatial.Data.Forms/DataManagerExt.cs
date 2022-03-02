// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
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
        /// This opens a file, but populates the dialog filter with only vector formats.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog, or null if nothing load.</returns>
        public static IFeatureSet OpenVector(this IDataManager self)
        {
            var ofd = new OpenFileDialog { Filter = self.VectorReadFilter };
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler) as IFeatureSet;
        }

        /// <summary>
        /// This uses an open dialog filter with only vector extensions but where multi-select is
        /// enabled, hence allowing multiple vectors to be returned in this list.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>The enumerable or vectors.</returns>
        public static IEnumerable<IFeatureSet> OpenVectors(this IDataManager self)
        {
            var ofd = new OpenFileDialog { Filter = self.VectorReadFilter, Multiselect = true };
            if (ofd.ShowDialog() != DialogResult.OK) yield break;
            foreach (var name in ofd.FileNames)
            {
                var ds = self.OpenVector(name, self.LoadInRam, self.ProgressHandler);
                if (ds != null) yield return ds;
            }
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>for now an IDataSet.</returns>
        public static IImageData OpenImage(this IDataManager self)
        {
            var ofd = new OpenFileDialog { Filter = self.ImageReadFilter };
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler) as IImageData;
        }

        /// <summary>
        /// This uses an open dialog filter with only image extensions for supported image formats,
        /// but where multi-select is enabled, and so allowing multiple images to be returned at once.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>An Enumerable with the opened IImageData objects.</returns>
        public static IEnumerable<IImageData> OpenImages(this IDataManager self)
        {
            var ofd = new OpenFileDialog { Filter = self.ImageReadFilter, Multiselect = true };
            if (ofd.ShowDialog() != DialogResult.OK) yield break;
            foreach (var name in ofd.FileNames)
            {
                var id = self.OpenImage(name, self.ProgressHandler);
                if (id != null) yield return id;
            }
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>An IDataSet with the data from the file specified in an open file dialog.</returns>
        public static IDataSet OpenFile(this IDataManager self)
        {
            var ofd = new OpenFileDialog { Filter = self.DialogReadFilter };
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler);
        }

        /// <summary>
        /// This launches an open file dialog that allows loading of several files at once
        /// and returns the datasets in a list.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>An enumerable of all the files that were opened.</returns>
        public static IEnumerable<IDataSet> OpenFiles(this IDataManager self)
        {
            using (var ofd = new OpenFileDialog { Multiselect = true, Filter = self.DialogReadFilter })
            {
                if (ofd.ShowDialog() != DialogResult.OK) yield break;

                var filterparts = ofd.Filter.Split('|');
                var pos = (ofd.FilterIndex - 1) * 2;
                int index = filterparts[pos].IndexOf(" - ", StringComparison.Ordinal);
                var filterName = index > 0 ? filterparts[pos].Remove(index) : string.Empty; // provider entries contain a -, entries without - aren't specific providers but lists that contain endings more than one provider can open

                foreach (var name in ofd.FileNames)
                {
                    var ds = self.OpenFile(name, self.LoadInRam, self.ProgressHandler, filterName);
                    if (ds != null) yield return ds;
                }
            }
        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>An IRaster with the data from the file specified in an open file dialog.</returns>
        public static IRaster OpenRaster(this IDataManager self)
        {
            var ofd = new OpenFileDialog { Filter = self.RasterReadFilter };
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return self.OpenFile(ofd.FileName, self.LoadInRam, self.ProgressHandler) as IRaster;
        }

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>An enumerable or rasters.</returns>
        public static IEnumerable<IRaster> OpenRasters(this IDataManager self)
        {
            var ofd = new OpenFileDialog { Filter = self.RasterReadFilter, Multiselect = true };
            if (ofd.ShowDialog() != DialogResult.OK) yield break;
            foreach (var name in ofd.FileNames)
            {
                var ds = self.OpenRaster(name, self.LoadInRam, self.ProgressHandler);
                if (ds != null) yield return ds;
            }
        }
    }
}