// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
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
        private SimpleActionItem _findButton;
        private SqlExpressionDialog _qd;
        private string _qdExpression;
        private string _oldFeaturelayer;

        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            _findButton = new SimpleActionItem(HeaderControl.HomeRootItemKey, Resources.FindButton, FindToolClick)
            {
                GroupCaption = "Map Tool",
                SmallImage = Resources.page_white_find_16x16,
                LargeImage = Resources.page_white_find,
            };
            App.HeaderControl.Add(_findButton);

            App.AppCultureChanged += OnAppCultureChanged;

            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.AppCultureChanged -= OnAppCultureChanged;

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

            if (fl.DataSet.Name != _oldFeaturelayer)
            {
                _qdExpression = string.Empty;
                _oldFeaturelayer = fl.DataSet.Name;
            }

            using (_qd = new SqlExpressionDialog())
            {
                _qd.DialogCulture = ExtensionCulture;

                fl.DataSet.FillAttributes();
                if (fl.DataSet.AttributesPopulated)
                    _qd.Table = fl.DataSet.DataTable;
                else
                    _qd.AttributeSource = fl.DataSet;

                _qd.Expression = _qdExpression;

                // Note: User must click ok button to see anything.
                if (_qd.ShowDialog() == DialogResult.Cancel)
                {
                    _qdExpression = _qd.Expression;
                    return;
                }

                _qdExpression = _qd.Expression;

                if (!string.IsNullOrWhiteSpace(_qd.Expression))
                {
                    try
                    {
                        fl.SelectByAttribute(_qd.Expression);
                    }
                    catch (SyntaxErrorException ex)
                    {
                        MessageBox.Show(string.Format(Resources.IncorrectSyntax, ex.Message));
                    }
                }
            }
        }

        private void OnAppCultureChanged(object sender, CultureInfo appCulture)
        {
            ExtensionCulture = appCulture;
            UpdatePlugInItems();
        }

        private void UpdatePlugInItems()
        {
            _findButton.Caption = Resources.FindButton;
            _findButton.ToolTipText = Resources.FindButton;
        }

        #endregion
    }
}