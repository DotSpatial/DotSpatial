// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Extension methods for <see cref="Raster"/>.
    /// </summary>
    public static class RasterExt
    {
        /// <summary>
        /// Displays a dialog, allowing the users to open a raster.
        /// </summary>
        /// <param name="self">this.</param>
        public static void Open(this Raster self)
        {
            if (self.Filename == null)
            {
                string filter = DataManager.DefaultDataManager.RasterReadFilter;
                OpenFileDialog ofd = new OpenFileDialog { Filter = filter };
                if (ofd.ShowDialog() != DialogResult.OK) return;
                self.Filename = ofd.FileName;
            }

            Raster.Open(self.Filename);
        }
    }
}