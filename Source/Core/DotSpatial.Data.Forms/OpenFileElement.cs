// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// The OpenFileElement can be added directly to a form and supports all of the
    /// basic dialog options that are important for browsing vector/raster/image data.
    /// </summary>
    internal class OpenFileElement : UserControl
    {
        #region Fields

        private IContainer _components;
        private ImageList _imlImages;

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            _components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(OpenFileElement));
            _imlImages = new ImageList(_components);
            SuspendLayout();

            // imlImages
            _imlImages.ColorDepth = ColorDepth.Depth32Bit;
            resources.ApplyResources(_imlImages, "_imlImages");
            _imlImages.TransparentColor = Color.Transparent;

            // OpenFileElement
            Name = "OpenFileElement";
            resources.ApplyResources(this, "$this");
            ResumeLayout(false);
        }

        #endregion
    }
}