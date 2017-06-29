// ********************************************************************************************************
// Product Name: DotSpatial.Tools.BooleanParam
// Description:  Double Parameters returned by an ITool allows the tool to specify a range and default value
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Boolean Parameters returned by an ITool allows the tool to specify default value
    /// </summary>
    public class BooleanParam : Parameter
    {
        private string _checkBoxText = string.Empty;

        /// <summary>
        /// Creates a new Boolean parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="checkBoxText">The text to appear adjacent to the checkBox</param>
        public BooleanParam(string name, string checkBoxText)
        {
            Name = name;
            ParamType = "DotSpatial Boolean Param";
            ParamVisible = ShowParamInModel.No;
            _checkBoxText = checkBoxText;
        }

        /// <summary>
        /// Creates a new Boolean parameter
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
            _checkBoxText = checkBoxText;
        }

        /// <summary>
        /// Specifies the value of the parameter (This is also the default value for input)
        /// </summary>
        public new bool Value
        {
            get { return (bool)base.Value; }
            set
            {
                base.Value = value;
                DefaultSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets the text that appears beside the check box in a Tool Dialog
        /// </summary>
        public string CheckBoxText
        {
            get { return _checkBoxText; }
            set { _checkBoxText = value; }
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new BooleanElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new BooleanElement(this));
        }
    }
}