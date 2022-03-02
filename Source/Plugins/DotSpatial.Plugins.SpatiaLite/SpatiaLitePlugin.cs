// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// This adds the possibility to use SpatiaLite data as layers.
    /// </summary>
    public class SpatiaLitePlugin : Extension
    {
        #region Methods

        /// <summary>
        /// When the plugin is activated.
        /// </summary>
        public override void Activate()
        {
            // try setting environment variables..
            SpatiaLiteHelper.SetEnvironmentVars();

            string spatiaLiteGroup = "SpatiaLite";

            var bOpenLayer = new SimpleActionItem("Open Layer", ButtonClick)
            {
                LargeImage = Resources.spatialite_open_32,
                SmallImage = Resources.spatialite_open_16,
                ToolTipText = "Add Layer from SpatiaLite",
                GroupCaption = spatiaLiteGroup
            };
            App.HeaderControl.Add(bOpenLayer);

            // query
            var bQuery = new SimpleActionItem("SpatiaLite Query", BQueryClick)
            {
                LargeImage = Resources.spatialite_query_32,
                SmallImage = Resources.spatialite_query_16,
                ToolTipText = "Run SpatiaLite Query",
                GroupCaption = spatiaLiteGroup
            };
            App.HeaderControl.Add(bQuery);

            // save layer (not implemented yet)
            var bSaveLayer = new SimpleActionItem("Save Layer", BSaveLayerClick)
            {
                LargeImage = Resources.spatialite_save_32,
                SmallImage = Resources.spatialite_save_16,
                ToolTipText = "Save Layer to SpatiaLite Database",
                GroupCaption = spatiaLiteGroup
            };
            App.HeaderControl.Add(bSaveLayer);
            base.Activate();
        }

        /// <summary>
        /// Shows the add layer window.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void ButtonClick(object? sender, EventArgs e)
        {
            // check if it's a valid SpatiaLite layer
            using (OpenFileDialog fd = new()
            {
                Title = Resources.OpenSpatialiteDatabase,
                Filter = Resources.SpatialiteDatabaseFilter
            })
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    string cs = SqLiteHelper.GetSqLiteConnectionString(fd.FileName);
                    var slh = SpatiaLiteHelper.Open(cs, out string error);

                    if (slh == null)
                    {
                        MessageBox.Show(string.Format(Resources.DatabaseNotValid, fd.FileName, error));
                        return;
                    }

                    using (FrmAddLayer frm = new(slh, App.Map))
                    {
                        frm.ShowDialog();
                    }
                }
            }
        }

        /// <summary>
        /// When the plugin is deactivated.
        /// </summary>
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        private void BQueryClick(object? sender, EventArgs e)
        {
            // check if it's a valid SpatiaLite layer
            using (OpenFileDialog fd = new()
            {
                Title = Resources.OpenSpatialiteDatabase,
                Filter = Resources.SpatialiteDatabaseFilter
            })
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    string cs = SqLiteHelper.GetSqLiteConnectionString(fd.FileName);
                    var slh = SpatiaLiteHelper.Open(cs, out string error);

                    if (slh == null)
                    {
                        MessageBox.Show(string.Format(Resources.DatabaseNotValid, fd.FileName, error));
                        return;
                    }

                    using (var frm = new FrmQuery(slh, App.Map))
                    {
                        frm.ShowDialog();
                    }
                }
            }
        }

        private void BSaveLayerClick(object? sender, EventArgs e)
        {
            MessageBox.Show(@"This operation is not implemented yet");
        }

        #endregion
    }
}