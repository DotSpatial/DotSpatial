﻿// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DateTime
// Description:  String Parameters returned by an ITool allows the tool to specify a default value
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Teva Veluppillai. Created in March, 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        #region Constructors

        /// <summary>
        /// Creates a new string parameter
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
        /// Creates a new string parameter
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

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
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

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new DateTimeElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new DateTimeElement(this));
        }
    }
}