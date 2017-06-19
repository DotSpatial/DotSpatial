using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class DialogElement
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        protected System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DialogElement));
            this._groupBox = new GroupBox();
            this._lblStatus = new Label();
            this._groupBox.SuspendLayout();
            this.SuspendLayout();

            // GroupBox1
            this._groupBox.BackgroundImageLayout = ImageLayout.None;
            this._groupBox.Controls.Add(this._lblStatus);
            this._groupBox.Dock = DockStyle.Fill;
            this._groupBox.Location = new Point(0, 0);
            this._groupBox.Name = "_groupBox";
            this._groupBox.Size = new Size(492, 45);
            this._groupBox.TabIndex = 2;
            this._groupBox.TabStop = false;
            this._groupBox.Click += new EventHandler(this.DialogElementClick);

            // _lblStatus
            this._lblStatus.Image = (Image)(resources.GetObject("_lblStatus.Image"));
            this._lblStatus.Location = new Point(12, 20);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new Size(16, 16);
            this._lblStatus.TabIndex = 1;
            this._lblStatus.Click += new EventHandler(this.DialogElementClick);

            // DialogElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            this.AutoSize = true;
            this.Controls.Add(this._groupBox);
            this.Name = "DialogElement";
            this.Size = new Size(492, 45);
            this.Click += new EventHandler(this.DialogElementClick);
            this._groupBox.ResumeLayout(false);
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

        private GroupBox _groupBox;
        private Label _lblStatus;
        private Parameter _param;
        private ToolStatus _status;

    }
}
