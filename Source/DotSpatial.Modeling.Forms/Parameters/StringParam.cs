// ********************************************************************************************************
// Product Name: DotSpatial.Tools.StringParam
// Description:  String Parameters returned by an ITool allows the tool to specify a default value
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Modeling.Forms.Elements;

namespace DotSpatial.Modeling.Forms.Parameters
{
    /// <summary>
    /// String Parameters returned by an ITool allows the tool to specify a default value
    /// </summary>
    public class StringParam : Parameter
    {
        #region Constructors

        /// <summary>
        /// Creates a new string parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public StringParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial String Param";
        }

        /// <summary>
        /// Creates a new string parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        public StringParam(string name, string value)
        {
            Name = name;
            Value = value;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial String Param";
            DefaultSpecified = true;
        }

        #endregion

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new string Value
        {
            get { return (string)base.Value; }
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
            return (new StringElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new StringElement(this));
        }
    }
}