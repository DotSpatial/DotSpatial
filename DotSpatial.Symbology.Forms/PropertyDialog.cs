// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in Fall 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Dialog that can be useful for showing properties
    /// </summary>
    public class PropertyDialog : Form
    {
        #region Private Variables

        // Designer variables
        private Button cmdApply;
        private Button cmdCancel;
        private Button cmdOK;
        private PropertyGrid propertyGrid1;

        #endregion

        #region Constructors

        /// <summary>
        /// This creates a new instance of the Dialog
        /// </summary>
        public PropertyDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PropertyDialog));
            this.propertyGrid1 = new PropertyGrid();
            this.cmdOK = new Button();
            this.cmdCancel = new Button();
            this.cmdApply = new Button();
            this.SuspendLayout();
            //
            // propertyGrid1
            //
            this.propertyGrid1.AccessibleDescription = null;
            this.propertyGrid1.AccessibleName = null;
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.BackgroundImage = null;
            this.propertyGrid1.Font = null;
            this.propertyGrid1.Name = "propertyGrid1";
            //
            // cmdOK
            //
            this.cmdOK.AccessibleDescription = null;
            this.cmdOK.AccessibleName = null;
            resources.ApplyResources(this.cmdOK, "cmdOK");
            this.cmdOK.BackgroundImage = null;
            this.cmdOK.Font = null;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new EventHandler(this.cmdOK_click);
            //
            // cmdCancel
            //
            this.cmdCancel.AccessibleDescription = null;
            this.cmdCancel.AccessibleName = null;
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackgroundImage = null;
            this.cmdCancel.Font = null;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new EventHandler(this.cmdCancel_click);
            //
            // cmdApply
            //
            this.cmdApply.AccessibleDescription = null;
            this.cmdApply.AccessibleName = null;
            resources.ApplyResources(this.cmdApply, "cmdApply");
            this.cmdApply.BackgroundImage = null;
            this.cmdApply.Font = null;
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new EventHandler(this.cmdApply_click);
            //
            // PropertyDialog
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.propertyGrid1);
            this.Font = null;
            this.Icon = null;
            this.Name = "PropertyDialog";
            this.ResumeLayout(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// An original object place holder to allow outside handlers, but still track
        /// the object and its copy in a tightly correlated way.
        /// </summary>
        public IDescriptor OriginalObject { get; set; }

        /// <summary>
        /// This provides access to the property grid on this dialog
        /// </summary>
        public PropertyGrid PropertyGrid
        {
            get
            {
                return propertyGrid1;
            }
            set
            {
                propertyGrid1 = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event occurs when someone presses the apply button
        /// </summary>
        public event EventHandler ChangesApplied;

        /// <summary>
        /// Fires the ChangesApplied event.  If an original object IDescriptor has been set,
        /// then this directly handles the update.
        /// </summary>
        protected virtual void OnChangesApplied()
        {
            if (OriginalObject != null) OriginalObject.CopyProperties(PropertyGrid.SelectedObject);

            if (ChangesApplied != null)
            {
                ChangesApplied(this, EventArgs.Empty);
            }
        }

        #endregion

        #region EventHandlers

        private void cmdApply_click(object sender, EventArgs e)
        {
            OnChangesApplied();
        }

        private void cmdOK_click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            OnChangesApplied();
            Close();
        }

        private void cmdCancel_click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion
    }
}