// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2009 4:59:03 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DynamicVisibilityControl
    /// </summary>
    public partial class DynamicVisibilityControl : UserControl
    {
        #region Fields

        private readonly IWindowsFormsEditorService _dialogProvider;
        private readonly ILayer _layer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVisibilityControl"/> class.
        /// Note, this default constructor won't be able to grab the extents
        /// from a layer, but instead will use the "grab extents".
        /// </summary>
        public DynamicVisibilityControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVisibilityControl"/> class.
        /// </summary>
        /// <param name="dialogProvider">Service that may have launched this control</param>
        /// <param name="layer">the layer that this property is being adjusted on</param>
        public DynamicVisibilityControl(IWindowsFormsEditorService dialogProvider, ILayer layer)
        {
            _dialogProvider = dialogProvider;
            UseDynamicVisibility = layer.UseDynamicVisibility;
            DynamicVisibilityWidth = layer.DynamicVisibilityWidth;
            _layer = layer;
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic width where the layer content becomes visible again.
        /// </summary>
        public double DynamicVisibilityWidth { get; set; }

        /// <summary>
        /// Gets or sets the grab extents. If a layer is not provided, the DynamicVisibilityExtents
        /// will be set to the grab extents instead.
        /// </summary>
        public Envelope GrabExtents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating  whether dynamic visibility should be used.
        /// </summary>
        public bool UseDynamicVisibility { get; set; }

        #endregion

        #region Methods

        private void BtnGrabExtentsClick(object sender, EventArgs e)
        {
            if (_layer != null)
            {
                DynamicVisibilityWidth = _layer.MapFrame.ViewExtents.Width;
                _layer.DynamicVisibilityWidth = DynamicVisibilityWidth;
                _layer.UseDynamicVisibility = true;
            }
            else
            {
                DynamicVisibilityWidth = GrabExtents.Width;
            }

            _dialogProvider?.CloseDropDown();
            Hide();
        }

        private void ChkUseDynamicVisibilityCheckedChanged(object sender, EventArgs e)
        {
            UseDynamicVisibility = chkUseDynamicVisibility.Checked;
            if (_layer != null) _layer.UseDynamicVisibility = chkUseDynamicVisibility.Checked;
        }

        #endregion
    }
}