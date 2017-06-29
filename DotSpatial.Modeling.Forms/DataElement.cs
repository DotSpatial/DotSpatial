// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DataElement
// Description:  This class represents data in the model
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Element for Data
    /// </summary>
    public class DataElement : ModelElement
    {
        #region --------------- class variables

        private Parameter _parameter;

        #endregion

        #region --------------- Constructors

        /// <summary>
        /// Creates an instance of the Data Element
        /// <param name="parameter">One of Brian's Parameter classes</param>
        /// <param name="modelElements">A list of all the elements in the model</param>
        /// </summary>
        public DataElement(Parameter parameter, List<ModelElement> modelElements)
            : base(modelElements)
        {
            _parameter = parameter;
        }

        #endregion

        #region --------------- Properties

        /// <summary>
        /// Gets the dataType of the DataElement
        /// </summary>
        public string DataType
        {
            get { return _parameter.ParamType; }
        }

        /// <summary>
        /// Gets or sets the Data set that this element represents
        /// </summary>
        public Parameter Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        #endregion

        #region --------------- Methods

        /// <summary>
        /// When the user doulbe clicks on a tool call this method
        /// </summary>
        public override bool DoubleClick()
        {
            return true;
        }

        #endregion
    }
}