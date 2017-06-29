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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/21/2009 4:30:25 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// 9/22/09: Chris Wilson--changed order of buttons and removed hotkeys to conform to Windows standards
//          Automatically sets AcceptButton and CancelButton properties of owner form
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DialogButtons
    /// </summary>
    [DefaultEvent("OkClicked"), ToolboxItem(true)]
    public class DialogButtons : UserControl
    {
        #region Events

        /// <summary>
        /// The OK button was clicked
        /// </summary>
        public event EventHandler OkClicked;
        /// <summary>
        /// The Apply button was clicked
        /// </summary>
        public event EventHandler ApplyClicked;
        /// <summary>
        /// The Cancel button was clicked
        /// </summary>
        public event EventHandler CancelClicked;

        #endregion

        private Button btnApply;
        private Button btnCancel;
        private Button btnOK;
        private HelpProvider helpProvider1;

        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DialogButtons
        /// </summary>
        public DialogButtons()
        {
            InitializeComponent();
        }

        #endregion

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DialogButtons));
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnApply = new Button();
            this.helpProvider1 = new HelpProvider();
            this.SuspendLayout();
            //
            // btnOK
            //
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.helpProvider1.SetShowHelp(this.btnOK, ((bool)(resources.GetObject("btnOK.ShowHelp"))));
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += this.btnOK_Click;
            //
            // btnCancel
            //
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.helpProvider1.SetShowHelp(this.btnCancel, ((bool)(resources.GetObject("btnCancel.ShowHelp"))));
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += this.btnCancel_Click;
            //
            // btnApply
            //
            resources.ApplyResources(this.btnApply, "btnApply");
            this.helpProvider1.SetHelpString(this.btnApply, resources.GetString("btnApply.HelpString"));
            this.btnApply.Name = "btnApply";
            this.helpProvider1.SetShowHelp(this.btnApply, ((bool)(resources.GetObject("btnApply.ShowHelp"))));
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += this.btnApply_Click;
            //
            // DialogButtons
            //
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "DialogButtons";
            this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OnOKClicked();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelClicked();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyClicked();
        }

        /// <summary>
        /// Fires the ok clicked event
        /// </summary>
        protected virtual void OnOKClicked()
        {
            if (OkClicked != null) OkClicked(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the Cancel Clicked event
        /// </summary>
        protected virtual void OnCancelClicked()
        {
            if (CancelClicked != null) CancelClicked(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the Apply Clicked event
        /// </summary>
        protected virtual void OnApplyClicked()
        {
            if (ApplyClicked != null) ApplyClicked(this, EventArgs.Empty);
        }
    }
}