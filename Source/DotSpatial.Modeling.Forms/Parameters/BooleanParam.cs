// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Boolean Parameters returned by an ITool allows the tool to specify default value
    /// </summary>
    public class BooleanParam : Parameter
    {
        #region Fields

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="checkBoxText">The text to appear adjacent to the checkBox</param>
        public BooleanParam(string name, string checkBoxText)
        {
            Name = name;
            ParamType = "DotSpatial Boolean Param";
            ParamVisible = ShowParamInModel.No;
            CheckBoxText = checkBoxText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="checkBoxText">The text to appear adjacent to the checkBox</param>
        /// <param name="value">The default value</param>
        public BooleanParam(string name, string checkBoxText, bool value)
        {
            Name = name;
            Value = value;
            ParamType = "DotSpatial Boolean Param";
            ParamVisible = ShowParamInModel.No;
            DefaultSpecified = true;
            CheckBoxText = checkBoxText;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text that appears beside the check box in a Tool Dialog.
        /// </summary>
        public string CheckBoxText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is true (This is also the default value for input).
        /// </summary>
        public new bool Value
        {
            get
            {
                return (bool)base.Value;
            }

            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new BooleanElement(this);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new BooleanElement(this);
        }

        #endregion
    }
}