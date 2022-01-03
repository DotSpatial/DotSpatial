// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Plugins.ExtensionManager.Properties;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// An AppFunction.
    /// </summary>
    public class AppFunction : MapFunctionGlyph
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Manager that the dialog uses to control Apps.
        /// </summary>
        public AppManager Manager { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected override void OnDrawGlyph(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Resources.plugin_disabled_16x16, 4, 4);
        }

        /// <inheritdoc/>
        protected override void OnDrawGlyphLit(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Resources.plugin_16x16, 4, 4);
        }

        /// <inheritdoc/>
        protected override void OnGlpyhClick(GeoMouseArgs e)
        {
            using (var form = new ExtensionManagerForm())
            {
                form.App = Manager;
                form.ShowDialog();
            }
        }

        #endregion
    }
}