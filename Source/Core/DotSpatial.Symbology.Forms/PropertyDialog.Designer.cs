using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class PropertyDialog
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
            this.cmdOK.Click += new EventHandler(this.CmdOkClick);
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
            this.cmdCancel.Click += new EventHandler(this.CmdCancelClick);
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
            this.cmdApply.Click += new EventHandler(this.CmdApplyClick);
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

        private Button cmdApply;
        private Button cmdCancel;
        private Button cmdOK;
        private PropertyGrid propertyGrid1;
    }
}