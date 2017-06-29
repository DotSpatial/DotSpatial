// ********************************************************************************************************
// Product Name: DotSpatial.Tools.IntParam
// Description:  Int Parameters returned by an ITool allows the tool to specify a range and default value
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
    /// Int Parameters returned by an ITool allows the tool to specify a range and default value
    /// </summary>
    public class IntParam : Parameter
    {
        #region variables

        private int _max = int.MaxValue;
        private int _min = int.MinValue;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new integer parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public IntParam(string name)
        {
            Name = name;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Int Param";
            DefaultSpecified = false;
        }

        /// <summary>
        /// Creates a new integer parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        public IntParam(string name, int value)
        {
            Name = name;
            Value = value;
            ParamVisible = ShowParamInModel.No;
            ParamType = "DotSpatial Int Param";
            DefaultSpecified = true;
        }

        /// <summary>
        /// Creates a new integer parameter
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The default value</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
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

        #region properties

        /// <summary>
        /// The minimum range for the parameter Default: -2, 147, 483, 648
        /// </summary>
        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// The maximum range for the paramater Default: 2, 147, 483, 648
        /// </summary>
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Specifies the value to use by default must be between the min and max
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

        /// <summary>
        /// This method returns the dialog component that should be used to visualise INPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement InputDialogElement(List<DataSetArray> dataSets)
        {
            return (new IntElement(this));
        }

        /// <summary>
        /// This method returns the dialog component that should be used to visualise OUTPUT to this parameter
        /// </summary>
        /// <param name="dataSets"></param>
        /// <returns></returns>
        public override DialogElement OutputDialogElement(List<DataSetArray> dataSets)
        {
            return (new IntElement(this));
        }
    }
}