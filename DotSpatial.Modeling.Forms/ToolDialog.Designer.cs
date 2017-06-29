using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace DotSpatial.Modeling.Forms
{
    public partial class ToolDialog
    {
        #region Windows Form Designer generated code
        private SplitContainer splitContainer1;
        private Panel panelOkCancel;
        private Button btnCancel;
        private Button btnOK;
        private Panel panelElementContainer;
        private Panel panelHelp;
        private Panel panelToolIcon;
        private LinkLabel helpHyperlink;
        private RichTextBox rtbHelp;
        private Panel pnlHelpImage;

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelElementContainer = new System.Windows.Forms.Panel();
            this.panelOkCancel = new System.Windows.Forms.Panel();
            this.helpHyperlink = new System.Windows.Forms.LinkLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelHelp = new System.Windows.Forms.Panel();
            this.panelToolIcon = new System.Windows.Forms.Panel();
            this.pnlHelpImage = new System.Windows.Forms.Panel();
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.panelPadding = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelOkCancel.SuspendLayout();
            this.panelHelp.SuspendLayout();
            this.panelPadding.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelElementContainer);
            this.splitContainer1.Panel1.Controls.Add(this.panelOkCancel);
            this.splitContainer1.Panel1MinSize = 450;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.AutoScrollMinSize = new System.Drawing.Size(85, 0);
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.panelHelp);
            this.splitContainer1.Size = new System.Drawing.Size(792, 476);
            this.splitContainer1.SplitterDistance = 527;
            this.splitContainer1.TabIndex = 2;
            // 
            // panelElementContainer
            // 
            this.panelElementContainer.AutoScroll = true;
            this.panelElementContainer.AutoScrollMinSize = new System.Drawing.Size(250, 0);
            this.panelElementContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelElementContainer.Location = new System.Drawing.Point(0, 0);
            this.panelElementContainer.Name = "panelElementContainer";
            this.panelElementContainer.Size = new System.Drawing.Size(527, 442);
            this.panelElementContainer.TabIndex = 2;
            this.panelElementContainer.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // panelOkCancel
            // 
            this.panelOkCancel.AutoSize = true;
            this.panelOkCancel.Controls.Add(this.helpHyperlink);
            this.panelOkCancel.Controls.Add(this.btnCancel);
            this.panelOkCancel.Controls.Add(this.btnOK);
            this.panelOkCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOkCancel.Location = new System.Drawing.Point(0, 442);
            this.panelOkCancel.Name = "panelOkCancel";
            this.panelOkCancel.Size = new System.Drawing.Size(527, 34);
            this.panelOkCancel.TabIndex = 1;
            this.panelOkCancel.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // helpHyperlink
            // 
            this.helpHyperlink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpHyperlink.AutoSize = true;
            this.helpHyperlink.Location = new System.Drawing.Point(459, 13);
            this.helpHyperlink.Name = "helpHyperlink";
            this.helpHyperlink.Size = new System.Drawing.Size(53, 13);
            this.helpHyperlink.TabIndex = 2;
            // this.helpHyperlink.TabStop = true;
            this.helpHyperlink.Text = "Tool Help";
            this.helpHyperlink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpHyperlink_LinkClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(83, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(7, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelHelp
            // 
            this.panelHelp.AutoScroll = true;
            this.panelHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHelp.Controls.Add(this.panelPadding);
            this.panelHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHelp.Location = new System.Drawing.Point(0, 0);
            this.panelHelp.Name = "panelHelp";
            this.panelHelp.Size = new System.Drawing.Size(261, 476);
            this.panelHelp.TabIndex = 0;
            this.panelHelp.SizeChanged += new System.EventHandler(this.panelHelp_SizeChanged);
            // 
            // panelToolIcon
            // 
            this.panelToolIcon.BackgroundImage = global::DotSpatial.Modeling.Forms.Images.Hammer;
            this.panelToolIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelToolIcon.Location = new System.Drawing.Point(217, 3);
            this.panelToolIcon.Name = "panelToolIcon";
            this.panelToolIcon.Size = new System.Drawing.Size(32, 32);
            this.panelToolIcon.TabIndex = 2;
            this.panelToolIcon.Click += new System.EventHandler(this.otherElement_Click);
            // 
            // pnlHelpImage
            // 
            this.pnlHelpImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlHelpImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHelpImage.Location = new System.Drawing.Point(10, 260);
            this.pnlHelpImage.Name = "pnlHelpImage";
            this.pnlHelpImage.Size = new System.Drawing.Size(239, 161);
            this.pnlHelpImage.TabIndex = 1;
            // 
            // rtbHelp
            // 
            this.rtbHelp.BackColor = System.Drawing.SystemColors.Window;
            this.rtbHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtbHelp.Location = new System.Drawing.Point(10, 10);
            this.rtbHelp.Margin = new System.Windows.Forms.Padding(5);
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.ReadOnly = true;
            this.rtbHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbHelp.Size = new System.Drawing.Size(239, 250);
            this.rtbHelp.TabIndex = 0;
            this.rtbHelp.Text = "";
            // 
            // panelPadding
            // 
            this.panelPadding.Controls.Add(this.panelToolIcon);
            this.panelPadding.Controls.Add(this.pnlHelpImage);
            this.panelPadding.Controls.Add(this.rtbHelp);
            this.panelPadding.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPadding.Location = new System.Drawing.Point(0, 0);
            this.panelPadding.Name = "panelPadding";
            this.panelPadding.Padding = new System.Windows.Forms.Padding(10);
            this.panelPadding.Size = new System.Drawing.Size(259, 441);
            this.panelPadding.TabIndex = 3;
            // 
            // ToolDialog
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(792, 476);
            this.Controls.Add(this.splitContainer1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "ToolDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ToolDialog";
            this.Click += new System.EventHandler(this.otherElement_Click);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelOkCancel.ResumeLayout(false);
            this.panelOkCancel.PerformLayout();
            this.panelHelp.ResumeLayout(false);
            this.panelPadding.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Panel panelPadding;
    }
}
