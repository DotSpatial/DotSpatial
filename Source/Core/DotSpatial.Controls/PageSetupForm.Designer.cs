using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class PageSetupForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PageSetupForm));
            this.cancelButton = new Button();
            this.okButton = new Button();
            this.groupBox1 = new GroupBox();
            this.txtBoxBottom = new TextBox();
            this.txtBoxTop = new TextBox();
            this.txtBoxLeft = new TextBox();
            this.label5 = new Label();
            this.txtBoxRight = new TextBox();
            this.label3 = new Label();
            this.label6 = new Label();
            this.label4 = new Label();
            this.lblPaperDimension = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.comboPaperSizes = new ComboBox();
            this._groupBox2 = new GroupBox();
            this._rdbLandscape = new RadioButton();
            this._rdbPortrait = new RadioButton();
            this.groupBox1.SuspendLayout();
            this._groupBox2.SuspendLayout();
            this.SuspendLayout();

            // Cancel_Button
            this.cancelButton.DialogResult = DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new EventHandler(this.CancelButtonClick);

            // OK_Button
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new EventHandler(this.OkButtonClick);

            // GroupBox1
            this.groupBox1.Controls.Add(this.txtBoxBottom);
            this.groupBox1.Controls.Add(this.txtBoxTop);
            this.groupBox1.Controls.Add(this.txtBoxLeft);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBoxRight);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;

            // txtBoxBottom
            resources.ApplyResources(this.txtBoxBottom, "txtBoxBottom");
            this.txtBoxBottom.Name = "txtBoxBottom";
            this.txtBoxBottom.Leave += new EventHandler(this.TxtBoxBottomLeave);

            // txtBoxTop
            resources.ApplyResources(this.txtBoxTop, "txtBoxTop");
            this.txtBoxTop.Name = "txtBoxTop";
            this.txtBoxTop.Leave += new EventHandler(this.TxtBoxTopLeave);

            // txtBoxLeft
            resources.ApplyResources(this.txtBoxLeft, "txtBoxLeft");
            this.txtBoxLeft.Name = "txtBoxLeft";
            this.txtBoxLeft.Leave += new EventHandler(this.TxtBoxLeftLeave);

            // Label5
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";

            // txtBoxRight
            resources.ApplyResources(this.txtBoxRight, "txtBoxRight");
            this.txtBoxRight.Name = "txtBoxRight";
            this.txtBoxRight.Leave += new EventHandler(this.TxtBoxRightLeave);

            // Label3
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";

            // Label6
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";

            // Label4
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";

            // lblPaperDimension
            resources.ApplyResources(this.lblPaperDimension, "lblPaperDimension");
            this.lblPaperDimension.Name = "lblPaperDimension";

            // Label2
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";

            // Label1
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";

            // ComboPaperSizes
            this.comboPaperSizes.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboPaperSizes.FormattingEnabled = true;
            resources.ApplyResources(this.comboPaperSizes, "comboPaperSizes");
            this.comboPaperSizes.Name = "comboPaperSizes";
            this.comboPaperSizes.SelectedIndexChanged += new EventHandler(this.ComboPaperSizesSelectedIndexChanged);

            // _groupBox2
            this._groupBox2.Controls.Add(this._rdbLandscape);
            this._groupBox2.Controls.Add(this._rdbPortrait);
            resources.ApplyResources(this._groupBox2, "_groupBox2");
            this._groupBox2.Name = "_groupBox2";
            this._groupBox2.TabStop = false;

            // _rdbLandscape
            resources.ApplyResources(this._rdbLandscape, "_rdbLandscape");
            this._rdbLandscape.Name = "_rdbLandscape";
            this._rdbLandscape.TabStop = true;
            this._rdbLandscape.UseVisualStyleBackColor = true;
            this._rdbLandscape.CheckedChanged += new EventHandler(this.RdbLandscapeCheckedChanged);

            // _rdbPortrait
            resources.ApplyResources(this._rdbPortrait, "_rdbPortrait");
            this._rdbPortrait.Name = "_rdbPortrait";
            this._rdbPortrait.TabStop = true;
            this._rdbPortrait.UseVisualStyleBackColor = true;
            this._rdbPortrait.CheckedChanged += new EventHandler(this.RdbPortraitCheckedChanged);

            // PageSetupForm
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._groupBox2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblPaperDimension);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboPaperSizes);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PageSetupForm";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._groupBox2.ResumeLayout(false);
            this._groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        internal Button cancelButton;
        internal ComboBox comboPaperSizes;
        internal GroupBox groupBox1;
        internal Label label1;
        internal Label label2;
        internal Label label3;
        internal Label label4;
        internal Label label5;
        internal Label label6;

        internal Label lblPaperDimension;
        internal Button okButton;
        internal TextBox txtBoxBottom;
        internal TextBox txtBoxLeft;
        internal TextBox txtBoxRight;
        internal TextBox txtBoxTop;

    }
}