// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ListParam
// Description:  Double Parameter returned by an ITool allows the tool to specify a range and default value
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// List of strings parameter returned by an ITool allows the tool to specify a list of values and a default
    /// </summary>
    public class ListParam : Parameter
    {
        #region variables

        private List<string> _valueList;

        #endregion

        /// <summary>
        /// Creates a new list parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public ListParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial List Param";
            Value = -1;
            _valueList = new List<string>();
        }

        /// <summary>
        /// Creates a new list parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="valueList">The list of string values to poluate the combo box</param>
        public ListParam(string name, List<string> valueList)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial List Param";
            Value = -1;
            _valueList = valueList;
        }

        /// <summary>
        /// Creates a new list parameter
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
            _valueList = valueList;
            DefaultSpecified = true;
        }

        /// <summary>
        /// Gets or sets the index of the list
        /// </summary>
        public new int Value
        {
            get { return (int)base.Value; }
            set { base.Value = value; }
        }

        /// <summary>
        /// Gets or sets the list of items in the valuelist
        /// </summary>
        public List<string> ValueList
        {
            get { return _valueList; }
            set { _valueList = value; }
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new ListElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new ListElement(this));
        }
    }
}