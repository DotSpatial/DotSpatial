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
    /// Extension methods for <see cref="FeatureSet"/>
    /// </summary>
    public static class FeatureSetExt
    {
        /// <summary>
        /// Displays a dialog, allowing the users to open a raster.
        /// </summary>
        public static void Open(this FeatureSet self)
        {
            string filter = DataManager.DefaultDataManager.RasterReadFilter;
            OpenFileDialog ofd = new OpenFileDialog { Filter = filter };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FeatureSet.Open(ofd.FileName);
        }
    }
}