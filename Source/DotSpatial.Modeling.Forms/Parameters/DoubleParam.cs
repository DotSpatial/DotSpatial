// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Double Parameter returned by an ITool allows the tool to specify a range and default value
    /// </summary>
    public class DoubleParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public DoubleParam(string name)
        {
            Name = name;
            ParamType = "DotSpatial Double Param";
            ParamVisible = ShowParamInModel.No;
            DefaultSpecified = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        public DoubleParam(string name, double value)
        {
            Name = name;
            Value = value;
            ParamType = "DotSpatial Double Param";
            ParamVisible = ShowParamInModel.No;
            DefaultSpecified = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public DoubleParam(string name, double value, double min, double max)
        {
            Name = name;
            Max = max;
            Min = min;
            Value = value;
            ParamType = "DotSpatial Double Param";
            ParamVisible = ShowParamInModel.No;
            DefaultSpecified = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum range for the paramater Default: 1.79769313486232e308
        /// </summary>
        public double Max { get; set; } = double.MaxValue;

        /// <summary>
        /// Gets or sets the minimum range for the parameter Default: -1.79769313486232e308
        /// </summary>
        public double Min { get; set; } = double.MinValue;

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input).
        /// </summary>
        public new double Value
        {
            get
            {
                if (DefaultSpecified) return (double)base.Value;
                return 0;
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
            return new DoubleElement(this);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new DoubleElement(this);
        }

        #endregion
    }
}