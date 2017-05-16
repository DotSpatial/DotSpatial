// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// DateTimeElement
    /// </summary>
    internal partial class DateTimeElement : DialogElement
    {
        #region Fields

        private DateTimePicker _dateTimePicker2;
        private bool _enableUpdate = true;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeElement"/> class.
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public DateTimeElement(DateTimeParam param)
        {
            // Needed by the designer
            InitializeComponent();
            Param = param;
            if (param.Value > _dateTimePicker2.MinDate)
            {
                _dateTimePicker2.Value = param.Value;
            }

            GroupBox.Text = param.Name;

            // We save the parameters passed in
            SetupDefaultLighting();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new DateTimeParam Param
        {
            get
            {
                return (DateTimeParam)base.Param;
            }

            set
            {
                base.Param = value;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Refresh()
        {
            SetupDefaultLighting();
        }

        private static bool IsDateTime(string theValue)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Convert.ToDateTime(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void DateTimePicker2ValueChanged(object sender, EventArgs e)
        {
            if (_enableUpdate)
            {
                if (IsDateTime(_dateTimePicker2.Text))
                {
                    Param.Value = Convert.ToDateTime(_dateTimePicker2.Text);
                }
            }
        }

        private void SetupDefaultLighting()
        {
            // We load the default parameters
            if (Param.DefaultSpecified)
            {
                if (Param.Value > _dateTimePicker2.MinDate)
                {
                    _dateTimePicker2.Value = Param.Value;
                }

                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
            }
            else
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
            }
        }

        #endregion
    }
}