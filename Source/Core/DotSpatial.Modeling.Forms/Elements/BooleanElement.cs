// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// An element for true/false values.
    /// </summary>
    public partial class BooleanElement : DialogElement
    {
        #region Fields
        private bool _updateBox = true;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanElement"/> class.
        /// </summary>
        /// <param name="param">The parameter this element represents.</param>
        public BooleanElement(BooleanParam param)
        {
            // Needed by the designer
            InitializeComponent();

            Param = param;
            checkBox1.Text = param.CheckBoxText;
            GroupBox.Text = param.Name;

            DoRefresh();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public new BooleanParam Param
        {
            get
            {
                return (BooleanParam)base.Param;
            }

            set
            {
                base.Param = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the status lights.
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the checkbox.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void CheckBox1CheckStateChanged(object sender, EventArgs e)
        {
            if (!_updateBox) return;
            switch (checkBox1.CheckState)
            {
                case CheckState.Checked:
                    Param.Value = true;
                    break;
                case CheckState.Unchecked:
                    Param.Value = false;
                    break;
            }
        }

        /// <summary>
        /// When the check box it clicked this event fires.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void CheckBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void DoRefresh()
        {
            _updateBox = false;

            // This stuff loads the default value
            if (Param.DefaultSpecified == false)
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
                checkBox1.CheckState = CheckState.Indeterminate;
            }
            else if (Param.Value)
            {
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
                checkBox1.CheckState = CheckState.Checked;
            }
            else if (Param.Value == false)
            {
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
                checkBox1.CheckState = CheckState.Unchecked;
            }

            _updateBox = true;
        }

        #endregion
    }
}