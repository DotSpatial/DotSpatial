// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
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
// The Original Code is from DotSpatial.Symbology.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/18/2010 1:30:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// When a menu item is clicked, it usually shows a dialog, but the dialog might be
    /// System.Windows.Forms, which is not known in DotSpatial.Symbology.  Instead,
    /// the event is tracked to this class, which can be tracked.
    /// </summary>
    public sealed class RasterLayerEventSender
    {
        #region Singleton Pattern

        /// <summary>
        /// As part of the singleton pattern the fact that this is static
        /// allows all the various event throwing options to work through this.
        /// </summary>
        static readonly RasterLayerEventSender _instance = new RasterLayerEventSender();

        /// <summary>
        /// This is private as part of the Singleton pattern so that only using the
        /// "DefaultManager" appraoch
        /// </summary>
        private RasterLayerEventSender()
        {
        }

        /// <summary>
        /// This instance is static member of the singleton.
        /// SymbologyEvents bob = SymbologyEvents.Instance;
        /// </summary>
        public static RasterLayerEventSender Instance
        {
            get { return _instance; }
        }

        #endregion

        #region Feature Layers

        /// <summary>
        /// Occurs when the "Properties" menu item is clicked in a feature layer
        /// </summary>
        public event EventHandler<RasterLayerEventArgs> ShowPropertiesClicked;

        /// <summary>
        /// Raises the shared ShowPropertiesClicked event
        /// </summary>
        public void Raise_ShowProperties(object sender, RasterLayerEventArgs e)
        {
            if (ShowPropertiesClicked != null) ShowPropertiesClicked(sender, e);
        }

        /// <summary>
        /// Occurs when the export data menu item has been clicked
        /// </summary>
        public event EventHandler<RasterEventArgs> ExportDataClicked;

        /// <summary>
        /// Raises the shared ExportDataclicked event
        /// </summary>
        public void Raise_ExportData(object sender, RasterEventArgs e)
        {
            if (ExportDataClicked != null) ExportDataClicked(sender, e);
        }

        #endregion
    }
}