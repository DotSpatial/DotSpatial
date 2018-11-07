using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    public partial class ToolDialog
    {
        #region Windows Form Designer generated code
        private readonly IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolDialog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelElementContainer = new System.Windows.Forms.Panel();
            this.panelOkCancel = new System.Windows.Forms.Panel();
            this.helpHyperlink = new System.Windows.Forms.LinkLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelHelp = new System.Windows.Forms.Panel();
            this.panelPadding = new System.Windows.Forms.Panel();
            this.panelToolIcon = new System.Windows.Forms.Panel();
            this.pnlHelpImage = new System.Windows.Forms.Panel();
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
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
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelElementContainer);
            this.splitContainer1.Panel1.Controls.Add(this.panelOkCancel);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.panelHelp);
            // 
            // panelElementContainer
            // 
            resources.ApplyResources(this.panelElementContainer, "panelElementContainer");
            this.panelElementContainer.Name = "panelElementContainer";
            this.panelElementContainer.Click += new System.EventHandler(this.OtherElementClick);
            // 
            // panelOkCancel
            // 
            resources.ApplyResources(this.panelOkCancel, "panelOkCancel");
            this.panelOkCancel.Controls.Add(this.helpHyperlink);
            this.panelOkCancel.Controls.Add(this.btnCancel);
            this.panelOkCancel.Controls.Add(this.btnOK);
            this.panelOkCancel.Name = "panelOkCancel";
            this.panelOkCancel.Click += new System.EventHandler(this.OtherElementClick);
            // 
            // helpHyperlink
            // 
            resources.ApplyResources(this.helpHyperlink, "helpHyperlink");
            this.helpHyperlink.Name = "helpHyperlink";
            this.helpHyperlink.TabStop = true;
            this.helpHyperlink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HelpHyperlinkLinkClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // panelHelp
            // 
            resources.ApplyResources(this.panelHelp, "panelHelp");
            this.panelHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHelp.Controls.Add(this.panelPadding);
            this.panelHelp.Name = "panelHelp";
            this.panelHelp.SizeChanged += new System.EventHandler(this.PanelHelpSizeChanged);
            // 
            // panelPadding
            // 
            this.panelPadding.Controls.Add(this.panelToolIcon);
            this.panelPadding.Controls.Add(this.pnlHelpImage);
            this.panelPadding.Controls.Add(this.rtbHelp);
            resources.ApplyResources(this.panelPadding, "panelPadding");
            this.panelPadding.Name = "panelPadding";
            // 
            // panelToolIcon
            // 
            this.panelToolIcon.BackgroundImage = global::DotSpatial.Modeling.Forms.Images.Hammer;
            resources.ApplyResources(this.panelToolIcon, "panelToolIcon");
            this.panelToolIcon.Name = "panelToolIcon";
            this.panelToolIcon.Click += new System.EventHandler(this.OtherElementClick);
            // 
            // pnlHelpImage
            // 
            resources.ApplyResources(this.pnlHelpImage, "pnlHelpImage");
            this.pnlHelpImage.Name = "pnlHelpImage";
            // 
            // rtbHelp
            // 
            this.rtbHelp.BackColor = System.Drawing.SystemColors.Window;
            this.rtbHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.rtbHelp, "rtbHelp");
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.ReadOnly = true;
            // 
            // ToolDialog
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.MinimizeBox = false;
            this.Name = "ToolDialog";
            this.Click += new System.EventHandler(this.OtherElementClick);
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
