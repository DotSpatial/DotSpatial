// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// List of strings parameter returned by an ITool allows the tool to specify a list of values and a default
    /// </summary>
    public class ListParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public ListParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial List Param";
            Value = -1;
            ValueList = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="valueList">The list of string values to poluate the combo box</param>
        public ListParam(string name, List<string> valueList)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial List Param";
            Value = -1;
            ValueList = valueList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="valueList">The list of string values to poluate the combo box</param>
        /// <param name="value">The default item in the list</param>
        public ListParam(string name, List<string> valueList, int value)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial List Param";
            Value = value;
            ValueList = valueList;
            DefaultSpecified = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index of the list
        /// </summary>
        public new int Value
        {
            get
            {
                return (int)base.Value;
            }

            set
            {
                base.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of items in the valuelist
        /// </summary>
        public List<string> ValueList { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return new ListElement(this);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new ListElement(this);
        }

        #endregion
    }
}