// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
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