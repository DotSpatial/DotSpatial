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
    /// Extension method for raster
    /// </summary>
    public static class RasterExt
    {
        /// <summary>
        /// Displays a dialog, allowing the users to open a raster.
        /// </summary>
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