// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DataElement
// Description:  This class represents data in the model
//
// ********************************************************************************************************
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