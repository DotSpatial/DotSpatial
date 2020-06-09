// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Extension method for <see cref="Shapefile"/>.
    /// </summary>
    public static class ShapefileExt
    {
        /// <summary>
        /// This will use this object to open a shapefile, but launches an open file dialog for the user to select the file to open.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>A new Shapefile created from the file chosen by the open file dialog.</returns>
        public static Shapefile OpenFile(this Shapefile self)
        {
            using (var ofd = new OpenFileDialog { Filter = @"Shapefiles (*.shp) |*.shp|All Files|*.*" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return null;
                return Shapefile.OpenFile(ofd.FileName);
            }
        }
    }
}