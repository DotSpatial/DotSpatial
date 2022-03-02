// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// Element for Data.
    /// </summary>
    public class DataElement : ModelElement
    {
        #region Fields

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataElement"/> class.
        /// </summary>
        /// <param name="parameter">One of Brian's Parameter classes.</param>
        /// <param name="modelElements">A list of all the elements in the model.</param>
        public DataElement(Parameter parameter, List<ModelElement> modelElements)
            : base(modelElements)
        {
            Parameter = parameter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dataType of the DataElement.
        /// </summary>
        public string DataType => Parameter.ParamType;

        /// <summary>
        /// Gets or sets the Data set that this element represents.
        /// </summary>
        public Parameter Parameter { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// When the user doulbe clicks on a tool call this method.
        /// </summary>
        /// <returns>True.</returns>
        public override bool DoubleClick()
        {
            return true;
        }

        #endregion
    }
}