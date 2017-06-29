// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
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
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/11/09.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology.Forms
{
    internal class FunClass
    {
        private int _funTok;
        private string _functon;
        private int _idx;
        private string _nextArg;
        private int _noOfArg;
        private int _posInExpr;
        private string _preArg;
        private int _priorityLevel;
        private double _value;

        #region constructor

        /// <summary>
        /// Initializes a new instance of the FunClass class.
        /// </summary>
        public FunClass()
        {
            _idx = 0;
            _functon = null;
            _value = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the FunClass class.
        /// </summary>
        /// <param name="function">The function name.</param>
        /// <param name="index">The index of the class.</param>
        public FunClass(string function, int index)
        {
            _idx = index;
            _functon = function;
            _value = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the FunClass class.
        /// </summary>
        /// <param name="function">The function name.</param>
        /// <param name="index">The index of the class.</param>
        /// <param name="posInExp">The position in the expression.</param>
        public FunClass(string function, int index, int posInExp)
        {
            _idx = index;
            _functon = function;
            _value = 0.0;
            _posInExpr = posInExp;
        }

        /// <summary>
        /// Initializes a new instance of the FunClass class.
        /// </summary>
        /// <param name="function">The function name.</param>
        /// <param name="index">The index of the class.</param>
        /// <param name="posInExp">The position in the expression.</param>
        /// <param name="tokVal">The token value.</param>
        public FunClass(string function, int index, int posInExp, int tokVal)
        {
            _idx = index;
            _functon = function;
            _value = 0.0;
            _posInExpr = posInExp;
            _funTok = tokVal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the function string.
        /// </summary>
        public string FunctionName
        {
            get { return _functon; }
            set { _functon = value; }
        }

        /// <summary>
        /// Gets or sets the integer index.
        /// </summary>
        public int Index
        {
            get { return _idx; }
            set { _idx = value; }
        }

        /// <summary>
        /// Gets or sets the priority level.
        /// </summary>
        public int PriorityLevel
        {
            get { return _priorityLevel; }
            set { _priorityLevel = value; }
        }

        /// <summary>
        /// Gets or sets the previous argument.
        /// </summary>
        public string PreviousArgument
        {
            get { return _preArg; }
            set { _preArg = value; }
        }

        /// <summary>
        /// Gets or sets the next argument.
        /// </summary>
        public string NextArgument
        {
            get { return _nextArg; }
            set { _nextArg = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets position in the expression.
        /// </summary>
        public int PositionInExpression
        {
            get { return _posInExpr; }
            set { _posInExpr = value; }
        }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        public int TokenVal
        {
            get { return _funTok; }
            set { _funTok = value; }
        }

        /// <summary>
        /// set or get the no Of arg fuction type
        /// either one or two or more than two
        /// </summary>
        public int NoOfArg
        {
            get { return _noOfArg; }
            set { _noOfArg = value; }
        }

        #endregion
    }
}