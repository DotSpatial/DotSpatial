// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// FunClass.
    /// </summary>
    internal class FunClass
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FunClass"/> class.
        /// </summary>
        public FunClass()
            : this(null, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunClass"/> class.
        /// </summary>
        /// <param name="function">The function name.</param>
        /// <param name="index">The index of the class.</param>
        public FunClass(string function, int index)
        {
            Index = index;
            FunctionName = function;
            Value = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunClass"/> class.
        /// </summary>
        /// <param name="function">The function name.</param>
        /// <param name="index">The index of the class.</param>
        /// <param name="posInExp">The position in the expression.</param>
        public FunClass(string function, int index, int posInExp)
            : this(function, index)
        {
            PositionInExpression = posInExp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunClass"/> class.
        /// </summary>
        /// <param name="function">The function name.</param>
        /// <param name="index">The index of the class.</param>
        /// <param name="posInExp">The position in the expression.</param>
        /// <param name="tokVal">The token value.</param>
        public FunClass(string function, int index, int posInExp, int tokVal)
            : this(function, index, posInExp)
        {
            TokenVal = tokVal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the function string.
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets or sets the integer index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the next argument.
        /// </summary>
        public string NextArgument { get; set; }

        /// <summary>
        /// Gets or sets the no Of arg fuction type
        /// either one or two or more than two.
        /// </summary>
        public int NoOfArg { get; set; }

        /// <summary>
        /// Gets or sets position in the expression.
        /// </summary>
        public int PositionInExpression { get; set; }

        /// <summary>
        /// Gets or sets the previous argument.
        /// </summary>
        public string PreviousArgument { get; set; }

        /// <summary>
        /// Gets or sets the priority level.
        /// </summary>
        public int PriorityLevel { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        public int TokenVal { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value { get; set; }

        #endregion
    }
}