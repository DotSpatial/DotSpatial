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

using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// Extension method for shapefile
    /// </summary>
    public static class ShapefileExt
    {
        /// <summary>
        /// This will use this object to open a shapefile, but launches an open file dialog for the user
        /// to select the file to open.
        /// </summary>
        /// <returns>A new Shapefile created from the file chosen by the open file dialog.</returns>
        public static Shapefile OpenFile(this Shapefile self)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Shapefiles (*.shp) |*.shp|All Files|*.*";
            if (ofd.ShowDialog() != DialogResult.OK) return null;
            return Shapefile.OpenFile(ofd.FileName);
        }
    }
}