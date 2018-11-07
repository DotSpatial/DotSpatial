// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Threading;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This allows the user to switch between stop zooming out on max extent and zooming out farther than max extent.
    /// </summary>
    public partial class OptionsForm : Form
    {
        #region Fields

        private readonly AppManager _appManager;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsForm"/> class.
        /// </summary>
        /// <param name="appManager">The Appmanager of witch elements the options should be applied to.</param>
        public OptionsForm(AppManager appManager)
        {
            InitializeComponent();

            _appManager = appManager;
            if (_appManager != null)
            {
                Thread.CurrentThread.CurrentCulture = appManager.AppCulture;
                Thread.CurrentThread.CurrentUICulture = appManager.AppCulture;
                Refresh();

                if (_appManager.Map != null)
                {
                    chkZoomOutFartherThanMaxExtent.Visible = true;
                    chkZoomOutFartherThanMaxExtent.Checked = _appManager.Map.ZoomOutFartherThanMaxExtent;
                }
                else { chkZoomOutFartherThanMaxExtent.Visible = false; }

                if (_appManager.Legend != null)
                {
                    chkEditLegendBoxes.Visible = true;
                    chkShowLegendMenus.Visible = true;
                    cmbLanguage.Visible = true;
                    chkEditLegendBoxes.Checked = _appManager.Legend.EditLegendItemBoxes;
                    chkShowLegendMenus.Checked = _appManager.Legend.ShowContextMenu;

                    if (string.IsNullOrWhiteSpace(_appManager.AppCulture.Name))
                    { if (cmbLanguage.Items.Count > 0) cmbLanguage.SelectedIndex = 0; }
                    else
                    {
                        switch (_appManager.AppCulture.Name)
                        {
                            case "fr-FR":
                                cmbLanguage.SelectedIndex = 1;
                                break;
                        }
                   }
                }
                else
                {
                    chkEditLegendBoxes.Visible = false;
                    chkShowLegendMenus.Visible = false;
                    cmbLanguage.Visible = false;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves the changed settings.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void BtOkClick(object sender, EventArgs e)
        {
            if (_appManager != null)
            {
                if (_appManager.Map != null)
                {
                    _appManager.Map.ZoomOutFartherThanMaxExtent = chkZoomOutFartherThanMaxExtent.Checked;
                    if (!_appManager.Map.ZoomOutFartherThanMaxExtent)
                    {
                        var maxExt = _appManager.Map.GetMaxExtent();
                        if (_appManager.Map.Extent.Height > maxExt.Height || _appManager.Map.Extent.Width > maxExt.Width)
                            _appManager.Map.ZoomToMaxExtent();
                    }
                }

                if (_appManager.Legend != null)
                {
                    _appManager.Legend.EditLegendItemBoxes = chkEditLegendBoxes.Checked;
                    _appManager.Legend.ShowContextMenu = chkShowLegendMenus.Checked;

                    if (cmbLanguage.SelectedIndex == 0)
                    { _appManager.CultureString = string.Empty; }
                    else
                    {
                        switch (cmbLanguage.SelectedIndex)
                        {
                            case 1:
                                _appManager.CultureString = "fr-FR";
                                break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}