// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// Int Parameters returned by an ITool allows the tool to specify a range and default value.
    /// </summary>
    public class IntParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public IntParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Int Param";
            DefaultSpecified = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The default value.</param>
        public IntParam(string name, int value)
        {
            Name = name;
            Value = value;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Int Param";
            DefaultSpecified = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The default value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public IntParam(string name, int value, int min, int max)
        {
            Name = name;
            Max = max;
            Min = min;
            Value = value;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Int Param";
            DefaultSpecified = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum range for the paramater Default: 2, 147, 483, 648.
        /// </summary>
        public int Max { get; set; } = int.MaxValue;

        /// <summary>
        /// Gets or sets the minimum range for the parameter Default: -2, 147, 483, 648.
        /// </summary>
        public int Min { get; set; } = int.MinValue;

        /// <summary>
        /// Gets or sets the value to use by default must be between the min and max.
        /// </summary>
        public new int Value
        {
            get
            {
                if (DefaultSpecified) return (int)base.Value;
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
            return new IntElement(this);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new IntElement(this);
        }

        #endregion
    }
}