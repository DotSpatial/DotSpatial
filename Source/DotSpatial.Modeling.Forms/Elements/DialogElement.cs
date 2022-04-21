// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// A modular component that can be inherited to retrieve parameters for functions.
    /// </summary>
    public partial class DialogElement : UserControl
    {
        #region Fields
        // Status stuff
        private readonly ToolTip _lightTip = new ToolTip();
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogElement"/> class.
        /// </summary>
        public DialogElement()
        {
            // Required by the constructor
            InitializeComponent();

            // Sets up the tooltip
            _lightTip.SetToolTip(_lblStatus, string.Empty);
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires when the inactive areas around the controls are clicked on the element.
        /// </summary>
        public event EventHandler Clicked;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public Parameter Param
        {
            get
            {
                return _param;
            }

            protected set
            {
                _param = value;
                _param.ValueChanged += ParamValueChanged;
            }
        }

        /// <summary>
        /// Gets or sets the current status.
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
        /// Gets or sets the group box that surrounds the element contents.
        /// </summary>
        protected GroupBox GroupBox
        {
            get
            {
                return _groupBox;
            }

            set
            {
                _groupBox = value;
            }
        }

        /// <summary>
        /// Gets or sets the tool tip text to display when the mouse hovers over the light status.
        /// </summary>
        protected string LightTipText
        {
            get
            {
                return _lightTip.GetToolTip(_lblStatus);
            }

            set
            {
                _lightTip.SetToolTip(_lblStatus, value);
            }
        }

        /// <summary>
        /// Gets or sets the status label.
        /// </summary>
        protected Label StatusLabel
        {
            get
            {
                return _lblStatus;
            }

            set
            {
                _lblStatus = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when the dialong element is clicked.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected void DialogElementClick(object sender, EventArgs e)
        {
            OnClick(e);
        }

       /// <summary>
        /// Called to fire the click event for this element.
        /// </summary>
        /// <param name="e">A mouse event args thingy.</param>
        protected new void OnClick(EventArgs e)
       {
           Clicked?.Invoke(this, e);
       }

        /// <summary>
        /// Fires whenever the param value changed.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        protected virtual void ParamValueChanged(Parameter sender)
        {
            Refresh();
        }

        /// <summary>
        /// Sets the given text as the tooltip text of the given control.
        /// </summary>
        /// <param name="control">Control whose tooltip text is set.</param>
        /// <param name="toolTipText">Text that should be shown in tooltip of the control.</param>
        protected void SetToolTipText(Control control, string toolTipText)
        {
            _lightTip.SetToolTip(control, toolTipText);
        }

        #endregion
    }
}