// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// DateTime Parameters returned by an ITool allows the tool to specify default value
    /// </summary>
    public class DateTimeParam : Parameter
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public DateTimeParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Date Param";
            DefaultSpecified = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeParam"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        public DateTimeParam(string name, DateTime value)
        {
            Name = name;
            Value = value;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial DateTime Param";
            DefaultSpecified = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of the parameter (This is also the default value for input)
        /// </summary>
        public new DateTime Value
        {
            get
            {
                if (DefaultSpecified) return (DateTime)base.Value;
                return DateTime.Now;
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
            return new DateTimeElement(this);
        }

        /// <inheritdoc />
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return new DateTimeElement(this);
        }

        #endregion
    }
}