// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.FindFeature.Properties;
using DotSpatial.Symbology.Forms;

namespace DotSpatial.Plugins.FindFeature
{
    /// <summary>
    /// Allows a user to select polygons that match a query.
    /// </summary>
    public class FindFeaturePlugin : Extension
    {
        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.HomeRootItemKey, "Find", FindToolClick)
            {
                GroupCaption = "Map Tool",
                SmallImage = Resources.page_white_find_16x16,
                LargeImage = Resources.page_white_find,
            });

            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        /// <summary>
        /// Find a feature by query expression.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void FindToolClick(object sender, EventArgs e)
        {
            var fl = App.Map.GetFeatureLayers().FirstOrDefault(_ => _.IsSelected);
            if (fl == null)
            {
                MessageBox.Show(Resources.PleaseSelectAFeatureLayer);
                return;
            }

            using (SqlExpressionDialog qd = new SqlExpressionDialog())
            {
                if (fl.DataSet.AttributesPopulated)
                    qd.Table = fl.DataSet.DataTable;
                else
                    qd.AttributeSource = fl.DataSet;

                // Note: User must click ok button to see anything.
                if (qd.ShowDialog() == DialogResult.Cancel)
                    return;

                if (!string.IsNullOrWhiteSpace(qd.Expression))
                {
                    try
                    {
                        fl.SelectByAttribute(qd.Expression);
                    }
                    catch (SyntaxErrorException ex)
                    {
                        MessageBox.Show(string.Format(Resources.IncorrectSyntax, ex.Message));
                    }
                }
            }
        }

        #endregion
    }
}