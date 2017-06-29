// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2009 4:59:03 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DotSpatial.Topology;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DynamicVisibilityControl
    /// </summary>
    public class DynamicVisibilityControl : UserControl
    {
        #region Private Variables

        private readonly IWindowsFormsEditorService _dialogProvider;
        private double _dynamicVisibilityWidth;
        private IEnvelope _grabExtents;
        private ILayer _layer;
        private bool _useDynamicVisibility;

        private Button btnGrabExtents;
        private CheckBox chkUseDynamicVisibility;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DynamicVisibilityControl.  Note,
        /// this default constructor won't be able to grab the extents
        /// from a layer, but instead will use the "grab extents"
        /// </summary>
        public DynamicVisibilityControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The true constructor
        /// </summary>
        /// <param name="dialogProvider">Service that may have launched this control</param>
        /// <param name="layer">the layer that this property is being adjusted on</param>
        public DynamicVisibilityControl(IWindowsFormsEditorService dialogProvider, ILayer layer)
        {
            _dialogProvider = dialogProvider;
            _useDynamicVisibility = layer.UseDynamicVisibility;
            _dynamicVisibilityWidth = layer.DynamicVisibilityWidth;
            _layer = layer;
            InitializeComponent();
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicVisibilityControl));
            this.chkUseDynamicVisibility = new System.Windows.Forms.CheckBox();
            this.btnGrabExtents = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkUseDynamicVisibility
            // 
            resources.ApplyResources(this.chkUseDynamicVisibility, "chkUseDynamicVisibility");
            this.chkUseDynamicVisibility.Name = "chkUseDynamicVisibility";
            this.chkUseDynamicVisibility.UseVisualStyleBackColor = true;
            this.chkUseDynamicVisibility.CheckedChanged += new System.EventHandler(this.chkUseDynamicVisibility_CheckedChanged);
            // 
            // btnGrabExtents
            // 
            resources.ApplyResources(this.btnGrabExtents, "btnGrabExtents");
            this.btnGrabExtents.Name = "btnGrabExtents";
            this.btnGrabExtents.UseVisualStyleBackColor = true;
            this.btnGrabExtents.Click += new System.EventHandler(this.btnGrabExtents_Click);
            // 
            // DynamicVisibilityControl
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnGrabExtents);
            this.Controls.Add(this.chkUseDynamicVisibility);
            this.Name = "DynamicVisibilityControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic width where the layer content becomes visible again.
        /// </summary>
        public double DynamicVisibilityWidth
        {
            get { return _dynamicVisibilityWidth; }
            set { _dynamicVisibilityWidth = value; }
        }

        /// <summary>
        /// If a layer is not provided, the DynamicVisibilityExtents
        /// will be set to the grab extents instead.
        /// </summary>
        public IEnvelope GrabExtents
        {
            get { return _grabExtents; }
            set { _grabExtents = value; }
        }

        /// <summary>
        /// Gets or sets a boolean corresponding
        /// </summary>
        public bool UseDynamicVisibility
        {
            get { return _useDynamicVisibility; }
            set { _useDynamicVisibility = value; }
        }

        #endregion

        private void chkUseDynamicVisibility_CheckedChanged(object sender, EventArgs e)
        {
            _useDynamicVisibility = chkUseDynamicVisibility.Checked;
            if (_layer != null) _layer.UseDynamicVisibility = chkUseDynamicVisibility.Checked;
        }

        private void btnGrabExtents_Click(object sender, EventArgs e)
        {
            if (_layer != null)
            {
                _dynamicVisibilityWidth = _layer.MapFrame.ViewExtents.Width;
                _layer.DynamicVisibilityWidth = _dynamicVisibilityWidth;
                _layer.UseDynamicVisibility = true;
            }
            else
            {
                _dynamicVisibilityWidth = _grabExtents.Width;
            }
            if (_dialogProvider != null) _dialogProvider.CloseDropDown();
            Hide();
        }
    }
}