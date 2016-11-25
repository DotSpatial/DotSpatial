// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
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