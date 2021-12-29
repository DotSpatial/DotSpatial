// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Extension methods for <see cref="FeatureSet"/>.
    /// </summary>
    public static class FeatureSetExt
    {
        /// <summary>
        /// Displays a dialog, allowing the users to open a raster.
        /// </summary>
        /// <param name="self">this.</param>
        public static void Open(this FeatureSet self)
        {
            string filter = DataManager.DefaultDataManager.RasterReadFilter;
            using (var ofd = new OpenFileDialog { Filter = filter })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                FeatureSet.Open(ofd.FileName);
            }
        }
    }
}