// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 1:15:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// The OpenFileElement can be added directly to a form and supports all of the
    /// basic dialog options that are important for browsing vector/raster/image
    /// data.
    /// </summary>
    internal class OpenFileElement : UserControl
    {
        private IContainer components = null;
        private ImageList imlImages;

        #region Private Variables

        #endregion

        #region Constructors

        #endregion

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(OpenFileElement));
            this.imlImages = new ImageList(this.components);
            this.SuspendLayout();
            //
            // imlImages
            //
            this.imlImages.ColorDepth = ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imlImages, "imlImages");
            this.imlImages.TransparentColor = Color.Transparent;
            //
            // OpenFileElement
            //
            this.Name = "OpenFileElement";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
        }

        #region Methods

        #endregion

        #region Properties

        #endregion
    }
}