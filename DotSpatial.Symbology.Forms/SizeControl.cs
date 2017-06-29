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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/14/2009 11:22:04 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A user control that is specifically designed to control the point sizes
    /// </summary>
    [DefaultEvent("SelectedSizeChanged")]
    public class SizeControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the size changes on this control, ether through applying changes in the dialog or else
        /// by selecting one of the pre-configured sizes.
        /// </summary>
        public event EventHandler SelectedSizeChanged;

        #endregion

        #region private variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly IContainer _components;

        private Size2DDialog _editDialog;
        private Button btnEdit;
        private GroupBox grpSize;
        private SymbolSizeChooser scSizes;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the size control
        /// </summary>
        public SizeControl()
        {
            _components = null;
            InitializeComponent();
            _editDialog = new Size2DDialog();
            _editDialog.ChangesApplied += EditDialogChangesApplied;
            scSizes.SelectedSizeChanged += ScSizesSelectedSizeChanged;
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(SizeControl));
            this.grpSize = new GroupBox();
            this.scSizes = new SymbolSizeChooser();
            this.btnEdit = new Button();
            this.grpSize.SuspendLayout();
            ((ISupportInitialize)(this.scSizes)).BeginInit();
            this.SuspendLayout();
            //
            // grpSize
            //
            this.grpSize.AccessibleDescription = null;
            this.grpSize.AccessibleName = null;
            resources.ApplyResources(this.grpSize, "grpSize");
            this.grpSize.BackgroundImage = null;
            this.grpSize.Controls.Add(this.scSizes);
            this.grpSize.Controls.Add(this.btnEdit);
            this.grpSize.Font = null;
            this.grpSize.Name = "grpSize";
            this.grpSize.TabStop = false;
            //
            // scSizes
            //
            this.scSizes.AccessibleDescription = null;
            this.scSizes.AccessibleName = null;
            resources.ApplyResources(this.scSizes, "scSizes");
            this.scSizes.BackgroundImage = null;
            this.scSizes.BoxBackColor = SystemColors.Control;
            this.scSizes.BoxSelectionColor = SystemColors.Highlight;
            this.scSizes.BoxSize = new Size(36, 36);
            this.scSizes.Font = null;
            this.scSizes.Name = "scSizes";
            this.scSizes.NumBoxes = 4;
            this.scSizes.Orientation = Orientation.Horizontal;
            this.scSizes.RoundingRadius = 6;
            //
            // btnEdit
            //
            this.btnEdit.AccessibleDescription = null;
            this.btnEdit.AccessibleName = null;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackgroundImage = null;
            this.btnEdit.Font = null;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            //
            // SizeControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.grpSize);
            this.Font = null;
            this.Name = "SizeControl";
            this.grpSize.ResumeLayout(false);
            ((ISupportInitialize)(this.scSizes)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbol to use when drawing the various sizes
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISymbol Symbol
        {
            get
            {
                return scSizes.Symbol;
            }
            set
            {
                scSizes.Symbol = value;
                _editDialog.Symbol = value;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Fires the SizeChanged event
        /// </summary>
        protected virtual void OnSelectedSizeChanged()
        {
            if (SelectedSizeChanged != null) SelectedSizeChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Event Handlers

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _editDialog.ShowDialog();
        }

        private void EditDialogChangesApplied(object sender, EventArgs e)
        {
            scSizes.Invalidate();
            OnSelectedSizeChanged();
        }

        private void ScSizesSelectedSizeChanged(object sender, EventArgs e)
        {
            // Even though this is a reference type, and the "symbol" property is
            // up to date on the edit dialog, we need to force it to regenerate
            // the actual text values in the text boxes.
            _editDialog.Symbol = scSizes.Symbol;
            OnSelectedSizeChanged();
        }

        #endregion
    }
}