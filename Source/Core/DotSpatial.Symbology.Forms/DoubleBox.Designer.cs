using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class DoubleBox
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
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DoubleBox));
            this.lblCaption = new Label();
            this.txtValue = new TextBox();
            this.ttHelp = new ToolTip(this.components);
            this.SuspendLayout();
            //
            // lblCaption
            //
            resources.ApplyResources(this.lblCaption, "lblCaption");
            this.lblCaption.Name = "lblCaption";
            //
            // txtValue
            //
            resources.ApplyResources(this.txtValue, "txtValue");
            this.txtValue.Name = "txtValue";
            this.txtValue.TextChanged += new EventHandler(this.TxtValueTextChanged);
            //
            // DoubleBox
            //
            resources.ApplyResources(this, "$this");

            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblCaption);
            this.Name = "DoubleBox";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblCaption;
        private ToolTip ttHelp;
        private TextBox txtValue;
    }
}