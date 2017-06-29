// *********************************************************************************************************
// Product Name: DotSpatial.Tools.DialogElement
// Description:  The abstract tool Element class to be inherited by elements of the tool dialog
//
// *********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// *********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A modular component that can be inherited to retrieve parameters for functions.
    /// </summary>
    public class DialogElement : UserControl
    {
        #region Class Variables

        //Status stuff
        private readonly ToolTip _lightTip = new ToolTip();

        /// <summary>
        /// The group box for this element
        /// </summary>
        public GroupBox GroupBox1;

        // The group box that every other component sites in

        // The label that contains the icon
        private Label _lblStatus;
        private Parameter _param;
        private ToolStatus _status;

        /// <summary>
        /// Fires when the inactive areas around the controls are clicked on the element.
        /// </summary>
        public event EventHandler Clicked;

        #endregion

        #region methods

        /// <summary>
        /// Creates a blank dialog element
        /// </summary>
        public DialogElement()
        {
            //Required by the constructor
            InitializeComponent();

            //Sets up the tooltip
            _lightTip.SetToolTip(_lblStatus, string.Empty);
        }

        /// <summary>
        /// Fires whenever the
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void ParamValueChanged(Parameter sender)
        {
            Refresh();
        }

        #endregion

        #region Events

        /// <summary>
        /// Called to fire the click event for this element
        /// </summary>
        /// <param name="e">A mouse event args thingy</param>
        protected new void OnClick(EventArgs e)
        {
            if (Clicked != null)
                Clicked(this, e);
        }

        /// <summary>
        /// Occurs when the dialong element is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DialogElement_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the group box that surrounds the element contents
        /// </summary>
        protected GroupBox GroupBox
        {
            get { return GroupBox1; }
            set { GroupBox1 = value; }
        }

        /// <summary>
        /// Gets or sets the status label
        /// </summary>
        protected Label StatusLabel
        {
            get { return _lblStatus; }
            set { _lblStatus = value; }
        }

        /// <summary>
        /// Gets or sets the tool tip text to display when the mouse hovers over the light status
        /// </summary>
        protected string LightTipText
        {
            get { return _lightTip.GetToolTip(_lblStatus); }
            set { _lightTip.SetToolTip(_lblStatus, value); }
        }

        /// <summary>
        /// Gets the current status the input
        /// </summary>
        public virtual ToolStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                if (_status == ToolStatus.Empty)
                    _lblStatus.Image = Images.Caution;
                else if (_status == ToolStatus.Error)
                    _lblStatus.Image = Images.Error;
                else
                    _lblStatus.Image = Images.valid;
            }
        }

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public Parameter Param
        {
            get { return _param; }
            protected set
            {
                _param = value;
                _param.ValueChanged += ParamValueChanged;
            }
        }

        #endregion

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
        /// Required designer variable
        /// </summary>
        protected IContainer components;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DialogElement));
            this.GroupBox1 = new GroupBox();
            this._lblStatus = new Label();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // GroupBox1
            //
            this.GroupBox1.BackgroundImageLayout = ImageLayout.None;
            this.GroupBox1.Controls.Add(this._lblStatus);
            this.GroupBox1.Dock = DockStyle.Fill;
            this.GroupBox1.Location = new Point(0, 0);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new Size(492, 45);
            this.GroupBox1.TabIndex = 2;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Click += new EventHandler(this.DialogElement_Click);
            //
            // _lblStatus
            //
            this._lblStatus.Image = ((Image)(resources.GetObject("_lblStatus.Image")));
            this._lblStatus.Location = new Point(12, 20);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new Size(16, 16);
            this._lblStatus.TabIndex = 1;
            this._lblStatus.Click += new EventHandler(this.DialogElement_Click);
            //
            // DialogElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            this.AutoSize = true;
            this.Controls.Add(this.GroupBox1);
            this.Name = "DialogElement";
            this.Size = new Size(492, 45);
            this.Click += new EventHandler(this.DialogElement_Click);
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}