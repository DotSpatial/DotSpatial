using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DoubleBox));
            lblCaption = new Label();
            txtValue = new TextBox();
            ttHelp = new ToolTip(components);
            SuspendLayout();

            // lblCaption
            resources.ApplyResources(lblCaption, "lblCaption");
            lblCaption.Name = "lblCaption";
            ttHelp.SetToolTip(lblCaption, resources.GetString("lblCaption.ToolTip"));

            // txtValue
            resources.ApplyResources(txtValue, "txtValue");
            txtValue.Name = "txtValue";
            ttHelp.SetToolTip(txtValue, resources.GetString("txtValue.ToolTip"));
            txtValue.TextChanged += new EventHandler(TxtValueTextChanged);

            // DoubleBox
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtValue);
            Controls.Add(lblCaption);
            Name = "DoubleBox";
            ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCaption;
        private ToolTip ttHelp;
        private TextBox txtValue;
    }
}