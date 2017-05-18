using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class SizeControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.btnEdit.Click += new EventHandler(this.BtnEditClick);
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

        private Size2DDialog _editDialog;
        private Button btnEdit;
        private GroupBox grpSize;
        private SymbolSizeChooser scSizes;
    }
}