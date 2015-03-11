// ********************************************************************************************************
// Product Name: Expression.cs
// Description:  Class to validate and calculate label expressions.
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow 4.8.8 Expression.cpp
//
// The Initial Developer of this Original Code is Sergei Leschinski. Created 25 june 2010
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// 2015-03-02 - jany_ - Moved to Dotspatial
// ********************************************************************************************************

using System.Collections.Generic;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace DotSpatial.Symbology
{
    public class Expression
    {

        #region Members
        private List<ExpressionPart> _parts = new List<ExpressionPart>();
        private List<Element> _variables = new List<Element>();
        private List<Field> _fields = new List<Field>();
        private List<Operation> _operations = new List<Operation>();
        private List<string> _strings = new List<string>();
        private bool _saveOperations;
        private string _errorMessage; // the description of error
        private string _floatingFormat = "g";

        private bool _valid; //indicates whether the expression-syntax and -operations are valid
        private bool _expChanged; //indicates whether expression was changed after last calculation
        private string _expressionString; //Expression string that is used to calculate the expression for the DataRows
        #endregion

        #region Properties

        /// <summary>
        /// FloatingFormat that is used to convert double values to string.
        /// </summary>
        public string FloatingFormat
        {
            get { return _floatingFormat; }
            set { _floatingFormat = value; }
        }

        /// <summary>
        /// ErrorMessage that was raised by last method.
        /// </summary>
        public string ErrorMessage
        { get { return _errorMessage; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the given DataColumn as Field.
        /// </summary>
        /// <param name="col">DataColumn that should be added.</param>
        /// <returns>False, if DataColumn was null.</returns>
        public bool AddField(DataColumn col)
        {
            if (col == null) return false;
            _fields.Add(new Field(col.ColumnName, col.DataType));
            return true;
        }

        /// <summary>Uses the parsed expression to calculate the expression for the given row. If the expression is invalid only the Fields are replaced (This was the default action for expressions before DS 1.8).
        /// </summary>
        /// <param name="row">Row the expression gets calculated for.</param>
        /// <returns>The calculated expression.</returns>
        public string CalculateRowValue(DataRow row)
        {
            if (IsEmpty()) return "";
            _errorMessage = "";
            if (!_valid && !_expChanged) return ReplaceFieldsOnly(row); //expression is invalid and hasn't changed => simply replace fields

            foreach (Element e in _variables)
            {
                if (e.isField && row.Table.Columns.Contains(e.field.Name))
                {
                    e.setValue(row[e.field.Name]);
                }
            }
            ExpressionValue expval = Calculate();
            if (expval == null)
            {
                _valid = false;
                return ReplaceFieldsOnly(row);
            }
            else
            {
                _valid = true;
                return expval.ToString();
            }
        }

        /// <summary>
        /// Clears the fields.
        /// </summary>
        public void ClearFields()
        {
            _fields.Clear();
        }

        /// <summary>
        /// Checks whether the operations are valid.
        /// </summary>
        /// <param name="retVal">Example result if operation is valid.</param>
        /// <param name="row">Datarow that should be used to show as example result if the operation is valid.</param>
        /// <returns>True, if operations can be calculated.</returns>
        public bool IsValidOperation(ref string retVal, DataRow row = null)
        {
            if (IsEmpty()) return false;

            _errorMessage = "";
            if (row == null) //no row -> use exampleValues
            {
                foreach (Element e in _variables)
                {
                    if (e.isField)
                    {
                        if (e.val.type == tkValueType.vtBoolean)
                        { e.setValue(false); }
                        else if (e.val.type == tkValueType.vtDouble)
                        { e.setValue(2.4); }
                        else if (e.val.type == tkValueType.vtObject)
                        { e.setValue("obj"); }
                        else if (e.val.type == tkValueType.vtString)
                        { e.setValue("str"); }
                    }
                }
            }
            else //use DataRow values
            {
                foreach (Element e in _variables)
                {
                    if (e.isField && row.Table.Columns.Contains(e.field.Name))
                    {
                        e.setValue(row[e.field.Name]);
                    }
                }
            }

            ExpressionValue expval = Calculate();
            _valid = expval != null;
            if (_valid)
            {
                retVal = expval.ToString();
                _expChanged = false;
            }
            return _valid;
        }

        /// <summary>
        /// Parses the given string to Expression.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>True if string can be parsed to expression.</returns>
        public bool ParseExpression(string s)
        {
            _errorMessage = "";
            if (_expressionString != s)
            {
                _expressionString = s;
                _expChanged = true;
                _valid = false;
            }
            else
            {
                _expChanged = false;
                if (_valid) return true;
            }

            if (s.Length == 0) return false;

            ExpressionPart bracket = new ExpressionPart();

            _saveOperations = true;
            _variables.Clear();
            _parts.Clear();
            _operations.Clear();
            _strings.Clear();


            foreach (string c in new string[] { "{", "}" })
            {
                int pos = s.IndexOf(c);
                if (pos > -1)
                {
                    _errorMessage = "Unallowed character " + c + " at " + pos;
                    return false;
                }
            }

            Regex r = new Regex(@"\[(\d+|\w+)\]"); //all fields in [] that contain only word characters and numbers
            var matches = r.Matches(s);
            for (int i = matches.Count - 1; i >= 0; i--)
            {
                _strings.Add(matches[i].Value.Substring(1, matches[i].Length - 2));
                ReplaceSubString(ref s, matches[i].Index, matches[i].Length, "{f" + (_strings.Count - 1) + "}");
            }

            r = new Regex("(\"([^\"\n]*(\"\")*[^\"\n]*)*\")"); //everything inside "" all the same wether it contains no " or paired "
            matches = r.Matches(s);
            for (int i = matches.Count - 1; i >= 0; i--)
            {
                _strings.Add(matches[i].Value.Substring(1, matches[i].Length - 2));
                ReplaceSubString(ref s, matches[i].Index, matches[i].Length, "{s" + (_strings.Count - 1) + "}");
            }

            if (s.Contains("\""))
            {
                _errorMessage = SymbologyMessageStrings.Expression_UnpairedTextquotes;
                return false;
            }
            if (s.Contains("[]"))
            {
                _errorMessage = SymbologyMessageStrings.Expression_EmptyField;
                return false;
            }
            if (s.Contains("[") || s.Contains("]"))
            {
                _errorMessage = SymbologyMessageStrings.Expression_UnpairedBracket;
                return false;
            }

            bool found = true;
            while (found)
            {
                // seeking brackets
                int begin = 0;
                int end = 0;
                found = GetBrackets(s, ref begin, ref end);

                string expression = found ? s.Substring(begin + 1, end - begin - 1) : s;
                if (!ParseExpressionPart(expression))
                    return false;

                if (found)
                    ReplaceSubString(ref s, begin, end - begin + 1, "{p" + (_parts.Count - 1) + "}");
            }

            _strings.Clear();

            // building field list for faster access
            for (int i = 0; i < _parts.Count; i++)
            {
                ExpressionPart part = _parts[i];
                for (int j = 0; j < part.elements.Count; j++)
                {
                    if (part.elements[j].isField)
                    {
                        _variables.Add(part.elements[j]);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Replaces the current fields with the given datacolumns.
        /// </summary>
        /// <param name="columns">Columns that should be added as fields.</param>
        /// <returns>False if columns was null.</returns>
        public bool UpdateFields(DataColumnCollection columns)
        {
            ClearFields();
            if (columns == null) return false;
            foreach (DataColumn col in columns)
            {
                _fields.Add(new Field(col.ColumnName, col.DataType));
            }
            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the whole expression.
        /// </summary>
        /// <returns></returns>
        private ExpressionValue Calculate()
        {
            //int operation, left, right;
            Operation operation = null;
            int partIndex = 0; // we begin from the inner most bracket
            bool success = false;
            // in case we got cached operations
            int operationCount = 0;

            // if the operations should be cached we'll ensure that there is no obsolete data in vector
            if (_saveOperations)
                _operations.Clear();

            foreach (var part in _parts) //reset calculations
            {
                part.activeCount = part.elements.Count;
                foreach (var ele in part.elements)
                {
                    ele.wasCalculated = false;
                    ele.turnedOff = false;
                }
            }

            do
            {
                ExpressionPart part = _parts[partIndex];

                // if there is more then one element, then definitely some operation must be present
                if (part.elements.Count > 1)
                {
                    // reading caching operation
                    bool found = false;
                    if (!_saveOperations)
                    {
                        operation = _operations[operationCount];
                        operationCount++;
                        found = true;
                    }
                    else
                    {
                        if (operation == null)
                            operation = new Operation();
                        found = FindOperation(part, operation);
                    }

                    if (!found || !CalculateOperation(part, operation))
                    {
                        return null;
                    }

                    part.activeCount -= operation.binaryOperation ? 2 : 1;
                }

                // if there is only one element left, we'll finalize the part
                if (part.activeCount == 1)
                {
                    int size = part.elements.Count;
                    for (int i = 0; i < size; i++)
                    {
                        if (!part.elements[i].turnedOff)
                        {
                            part.val = GetValue(part, i);
                            part.elements[i].turnedOff = true;
                            partIndex++;
                            break;
                        }
                    }

                    if (partIndex >= _parts.Count)
                    {
                        // we closed the last part
                        success = true;
                        break;
                    }
                    else
                    {
                        // we are shifting to the next part
                        part = _parts[partIndex];
                    }
                }
            } while (true);

            // operation were saved - no need to cache any more
            if (_saveOperations)
            {
                if (operation != null) operation = null;
                _saveOperations = false;
            }

            return success ? _parts[_parts.Count - 1].val : null;
        }

        /// <summary>
        /// Calculates the given operation for the values of the given part.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private bool CalculateOperation(ExpressionPart part, Operation operation)
        {
            ExpressionValue valLeft = null;
            ExpressionValue valRight = null;
            Element elLeft = null;
            Element elRight = null;

            tkOperation oper = part.elements[operation.id].operation;

            valRight = GetValue(part, operation.right);
            elRight = part.elements[operation.right];

            if (oper == tkOperation.operNOT)   //  tis is an unary operator and we read only right operand
            {
                if (valRight.type != tkValueType.vtBoolean)
                {
                    _errorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                    return false;
                }
                elRight.calcVal.bln = !(valRight.bln);
                elRight.calcVal.type = tkValueType.vtBoolean;
                elRight.wasCalculated = true;
                part.elements[operation.id].turnedOff = true;
            }
            else if (oper == tkOperation.operChangeSign)  // tis is an unary operator and we read only right operand
            {
                if (valRight.type != tkValueType.vtDouble)
                {
                    _errorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                    return false;
                }
                elRight.calcVal.dbl = -valRight.dbl;
                elRight.calcVal.type = tkValueType.vtDouble;
                elRight.wasCalculated = true;
                part.elements[operation.id].turnedOff = true;
            }
            else // these are binary operators as we read left and right operands
            {
                valLeft = GetValue(part, operation.left);
                elLeft = part.elements[operation.left];

                if (oper == tkOperation.operLineBreak)
                {
                    elLeft.calcVal.str = valLeft.ToString() + Environment.NewLine + valRight.ToString();
                    elLeft.calcVal.type = tkValueType.vtString;
                }
                else if (valLeft.type == valRight.type)
                {
                    if (valLeft.type == tkValueType.vtDouble)
                    {
                        CalculateDoubleOperation(oper, elLeft, elRight, valLeft, valRight);
                    }
                    else if (valLeft.type == tkValueType.vtString)
                    {
                        CalculateStringOperation(oper, elLeft, elRight, valLeft, valRight);
                    }
                    else if (valLeft.type == tkValueType.vtBoolean)
                    {
                        CalculateBoolOperation(oper, elLeft, elRight, valLeft, valRight);
                    }
                }
                else if (oper != tkOperation.operPlus) // plus is the only operation that can have different valuetypes
                {
                    _errorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                    return false;
                }
                else if (valLeft.type == tkValueType.vtBoolean || valRight.type == tkValueType.vtBoolean) // boolean can't be added
                {
                    _errorMessage = SymbologyMessageStrings.Expression_PlusNotAllowed;
                    return false;
                }
                else //concat strings and doubles
                {
                    elLeft.calcVal.str = valLeft.ToString() + valRight.ToString();
                    elLeft.calcVal.type = tkValueType.vtString;
                }
                elLeft.wasCalculated = true;
                part.elements[operation.id].turnedOff = true;
                part.elements[operation.right].turnedOff = true;
            }
            return true;
        }

        /// <summary> Calculates the operations that are allowed for two boolean values.
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="elLeft"></param>
        /// <param name="elRight"></param>
        /// <param name="valLeft"></param>
        /// <param name="valRight"></param>
        /// <returns></returns>
        private bool CalculateBoolOperation(tkOperation oper, Element elLeft, Element elRight, ExpressionValue valLeft, ExpressionValue valRight)
        {
            switch (oper)
            {
                // logical operators
                case tkOperation.operOR:
                    elLeft.calcVal.bln = valLeft.bln || valRight.bln;
                    break;
                case tkOperation.operAND:
                    elLeft.calcVal.bln = valLeft.bln && valRight.bln;
                    break;
                case tkOperation.operXOR:
                    elLeft.calcVal.bln = valLeft.bln ^ valRight.bln;
                    break;
                case tkOperation.operEqual:
                    elLeft.calcVal.bln = valLeft.bln == valRight.bln;
                    break;
                case tkOperation.operNotEqual:
                    elLeft.calcVal.bln = valLeft.bln != valRight.bln;
                    break;
                default:
                    {
                        _errorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                        return false;
                    }
            }
            elLeft.calcVal.type = tkValueType.vtBoolean;
            return true;
        }

        /// <summary>Calculates the operations that are allowed for two double values.
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="elLeft"></param>
        /// <param name="elRight"></param>
        /// <param name="valLeft"></param>
        /// <param name="valRight"></param>
        /// <returns></returns>
        private bool CalculateDoubleOperation(tkOperation oper, Element elLeft, Element elRight, ExpressionValue valLeft, ExpressionValue valRight)
        {
            elLeft.calcVal.type = tkValueType.vtDouble;
            switch (oper)
            {
                case tkOperation.operLess:
                    elLeft.calcVal.bln = valLeft.dbl < valRight.dbl;
                    elLeft.calcVal.type = tkValueType.vtBoolean;
                    break;
                case tkOperation.operLessEqual:
                    elLeft.calcVal.bln = valLeft.dbl <= valRight.dbl;
                    elLeft.calcVal.type = tkValueType.vtBoolean;
                    break;
                case tkOperation.operGreater:
                    elLeft.calcVal.bln = valLeft.dbl > valRight.dbl;
                    elLeft.calcVal.type = tkValueType.vtBoolean;
                    break;
                case tkOperation.operGrEqual:
                    elLeft.calcVal.bln = valLeft.dbl >= valRight.dbl;
                    elLeft.calcVal.type = tkValueType.vtBoolean;
                    break;
                case tkOperation.operEqual:
                    elLeft.calcVal.bln = valLeft.dbl == valRight.dbl;
                    elLeft.calcVal.type = tkValueType.vtBoolean;
                    break;
                case tkOperation.operNotEqual:
                    elLeft.calcVal.bln = valLeft.dbl != valRight.dbl;
                    elLeft.calcVal.type = tkValueType.vtBoolean;
                    break;
                case tkOperation.operMinus:
                    elLeft.calcVal.dbl = valLeft.dbl - valRight.dbl;
                    elLeft.calcVal.type = tkValueType.vtDouble;
                    break;
                case tkOperation.operMult:
                    elLeft.calcVal.dbl = valLeft.dbl * valRight.dbl;
                    break;
                case tkOperation.operExpon:
                    elLeft.calcVal.dbl = Math.Pow(valLeft.dbl, valRight.dbl);
                    break;
                case tkOperation.operMOD:
                    elLeft.calcVal.dbl = (double)((int)valLeft.dbl % (int)valRight.dbl);
                    break;
                case tkOperation.operDiv:
                    if (valRight.dbl == 0.0)
                    {
                        _errorMessage = SymbologyMessageStrings.Expression_ZeroDivision;
                        return false;
                    }
                    elLeft.calcVal.dbl = valLeft.dbl / valRight.dbl;
                    break;
                case tkOperation.operDivInt:
                    if (valRight.dbl == 0.0)
                    {
                        _errorMessage = SymbologyMessageStrings.Expression_ZeroDivision;
                        return false;
                    }
                    elLeft.calcVal.dbl = (double)((int)valLeft.dbl / (int)valRight.dbl);
                    break;
                case tkOperation.operPlus:
                    elLeft.calcVal.dbl = valLeft.dbl + valRight.dbl;
                    break;
                default:
                    {
                        _errorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                        return false;
                    }
            }
            return true;
        }

        /// <summary>Calculates the operations that are allowed for two string values.
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="elLeft"></param>
        /// <param name="elRight"></param>
        /// <param name="valLeft"></param>
        /// <param name="valRight"></param>
        /// <returns></returns>
        private bool CalculateStringOperation(tkOperation oper, Element elLeft, Element elRight, ExpressionValue valLeft, ExpressionValue valRight)
        {
            int res = string.Compare(valLeft.str.ToLower(), valRight.str.ToLower());
            elLeft.calcVal.bln = false;
            elLeft.calcVal.type = tkValueType.vtBoolean;
            switch (oper)
            {
                case tkOperation.operPlus:
                    elLeft.calcVal.str = valLeft.str + valRight.str;
                    elLeft.calcVal.type = tkValueType.vtString;
                    break;
                case tkOperation.operLess:
                    if (res < 0) elLeft.calcVal.bln = true;
                    break;
                case tkOperation.operLessEqual:
                    if (res <= 0) elLeft.calcVal.bln = true;
                    break;
                case tkOperation.operGreater:
                    if (res > 0) elLeft.calcVal.bln = true;
                    break;
                case tkOperation.operGrEqual:
                    if (res >= 0) elLeft.calcVal.bln = true;
                    break;
                case tkOperation.operEqual:
                    if (res == 0) elLeft.calcVal.bln = true;
                    break;
                case tkOperation.operNotEqual:
                    if (res != 0) elLeft.calcVal.bln = true;
                    break;
                default:
                    _errorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Seeks operation with the highest priority and operands.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private bool FindOperation(ExpressionPart part, Operation operation)
        {
            // seeking operation
            bool found = false;
            int priority = 255;

            List<Element> elements = part.elements;
            int size = elements.Count;
            for (int i = 0; i < size; i++)
            {
                Element element = elements[i];
                if (!element.turnedOff)
                {
                    if (element.type == tkElementType.etOperation)
                    {
                        if (element.priority < priority)
                        {
                            found = true;
                            priority = element.priority;
                            operation.id = i;
                        }
                    }
                }
            }
            if (!found)
            {
                _errorMessage = SymbologyMessageStrings.Expression_OperationNotFound;
                return false;
            }

            // seeking right operand
            operation.left = operation.right = -1;
            for (int i = operation.id + 1; i < size; i++)
            {
                Element element = elements[i];
                if (!element.turnedOff)
                {
                    if (element.type == tkElementType.etOperation)
                    {
                        if (element.operation != tkOperation.operNOT && element.operation != tkOperation.operChangeSign)
                        {
                            _errorMessage = SymbologyMessageStrings.Expression_OpereratorInsteadOfValue;
                            return false;
                        }
                    }
                    else
                    {
                        operation.right = i;
                        break;
                    }
                }
            }
            if (operation.right == -1)
            {
                _errorMessage = SymbologyMessageStrings.Expression_RightOperandMissing;
                return false;
            }

            // if the operator is binary, seeking left operand
            if (elements[operation.id].operation != tkOperation.operNOT && elements[operation.id].operation != tkOperation.operChangeSign)
            {
                for (int i = operation.id - 1; i >= 0; i--)
                {
                    if (!elements[i].turnedOff)
                    {
                        operation.left = i;
                        break;
                    }
                }
                if (operation.left == -1)
                {
                    _errorMessage = SymbologyMessageStrings.Expression_LeftOperandMissing;
                    return false;
                }
                operation.binaryOperation = true;
            }
            else
            {
                operation.binaryOperation = false;
            }

            // caching operations
            if (_saveOperations)
            {
                Operation op = new Operation();
                op.left = operation.left;
                op.right = operation.right;
                op.id = operation.id;
                op.binaryOperation = operation.binaryOperation;
                _operations.Add(op);
            }
            return true;
        }

        /// <summary>
        /// Checks whether opening- and closingSymbol can be found.
        /// </summary>
        /// <param name="expression">Expression that is checked.</param>
        /// <param name="begin">Returns position of openingSymbol if found.</param>
        /// <param name="end">Returns position of closingSymbol if found.</param>
        /// <param name="openingSymbol">OpeningSymbol that is searched for.</param>
        /// <param name="closingSymbol">ClosingSymbol that is searched for.</param>
        /// <returns>True if opening- and closingSymbol where found.</returns>
        private bool GetBrackets(string expression, ref int begin, ref int end, string openingSymbol = "(", string closingSymbol = ")")
        {
            // closing bracket
            end = expression.IndexOf(closingSymbol);
            if (end == -1) return false;
            //opening bracket
            begin = expression.LastIndexOf(openingSymbol, end);
            return begin != -1;
        }

        /// <summary>Gets the ExpressionValue of the element of the given part.
        /// </summary>
        /// <param name="part">Part that contains the element.</param>
        /// <param name="elementId">Id of the element whose value is returned.</param>
        /// <returns>Value of the element.</returns>
        private ExpressionValue GetValue(ExpressionPart part, int elementId)
        {
            Element element = part.elements[elementId];

            if (element.wasCalculated)
                return element.calcVal;
            else if (element.partIndex != -1)
                return _parts[element.partIndex].val;
            else
                return element.val;
        }

        /// <summary>
        /// Checks whether the given character could belong to a number.
        /// </summary>
        /// <param name="chr">Character that is checked.</param>
        /// <param name="exponential">Remembers whether last character was exponential to check whether +- are allowed.</param>
        /// <returns>False if character can't belong to a number.</returns>
        private bool IsDecimal(Char chr, ref bool exponential)
        {
            if (chr >= '0' && chr <= '9')
                return true;

            if ((chr == '.') || (chr == ',')) // specify decimal separator explicitly
                return true;

            if ((chr == 'e') || (chr == 'E'))
            {
                exponential = true;
                return true;
            }

            if (exponential && (chr == '+' || chr == '-'))
            {
                exponential = false;
                return true;
            }

            exponential = false; // +, - can be in the next position after e only
            return false;
        }

        /// <summary>
        /// Checks whether the ExpressionString is empty.
        /// </summary>
        /// <returns>True, if ExpressionString is empty.</returns>
        private bool IsEmpty()
        {
            if (string.IsNullOrWhiteSpace(_expressionString))
            {
                _errorMessage = SymbologyMessageStrings.Expression_Empty;
                _valid = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates elements of the expression parts and checks.
        /// </summary>
        /// <param name="s">ExpressionString whose bracketed parts are parsed.</param>
        /// <returns>True if partsyntax was correct.</returns>
        private bool ParseExpressionPart(string s)
        {
            bool readVal = true; // true - reading values and unary operations; false - reading binary operations

            // adding a part
            ExpressionPart part = new ExpressionPart();
            part.expression = s;

            for (int i = 0; i < s.Length; i++)
            {
                SkipSpaces(s, ref i);
                if (i >= s.Length)
                    break;

                // reading element
                Element element = new Element(ref _floatingFormat);
                if (readVal)
                {
                    if (!ReadValue(s, ref i, element)) return false;
                }
                else
                {
                    if (!ReadOperation(s, ref i, element)) return false;
                }

                // saving element
                part.elements.Add(element);

                //in case operation was unary the next element should be value as well
                if (element.operation != tkOperation.operNOT && element.operation != tkOperation.operChangeSign)
                    readVal = !readVal;
            }

            if (part.elements.Count > 0)
            {
                _parts.Add(part);
                return true;
            }
            return false;
        }

        /// <summary> Checks whether there is a value or unary operator at the given position.</summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool ReadValue(string s, ref int position, Element element)
        {
            string sub = ""; // substring
            Char chr = s[position];

            switch (chr)
            {
                case '{': //fields, strings or parts
                    {
                        sub = "";
                        position++; //opening bracket
                        chr = s[position]; //s, p or f according to replaced text
                        position++;

                        int closingIndex = s.IndexOf("}", position);
                        if (closingIndex < 0)
                        {
                            _errorMessage = SymbologyMessageStrings.Expression_ClosingBracket;
                            return false;
                        }

                        while (position < closingIndex)
                        {
                            if (char.IsDigit(s[position])) sub += s[position];
                            position++;
                        }

                        if (chr == 's') //string replacer
                        {
                            int index = Convert.ToInt32(sub);
                            sub = _strings[index];
                            element.type = tkElementType.etValue;
                            element.val.type = tkValueType.vtString;
                            element.val.str = sub;
                        }
                        else if (chr == 'f') //field replacer
                        {
                            int index = Convert.ToInt32(sub);
                            sub = _strings[index].TrimStart('[').TrimEnd(']');

                            int fieldIndex = _fields.FindIndex(p => p.Name.ToLower() == sub.ToLower());
                            if (fieldIndex < 0)
                            {
                                _errorMessage = SymbologyMessageStrings.Expression_FieldNotFound + sub;
                                return false;
                            }
                            element.isField = true;
                            element.type = tkElementType.etValue;
                            element.field = _fields[fieldIndex];
                        }
                        else if (chr == 'p') //part replacer
                        {
                            element.partIndex = Convert.ToInt32(sub);
                            element.type = tkElementType.etPart;
                        }
                        break;
                    }

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    { //numbers
                        sub = "";
                        bool exponential = false;

                        while (IsDecimal(chr, ref exponential))
                        {
                            sub += (char)chr;
                            position++;
                            if (position > s.Length - 1)
                                break;
                            chr = s[position];
                        }
                        position--;

                        double res;
                        if (double.TryParse(sub, out res))
                        {
                            element.type = tkElementType.etValue;
                            element.val.type = tkValueType.vtDouble;
                            element.val.dbl = res;
                        }
                        else
                        {
                            _errorMessage = SymbologyMessageStrings.Expression_NotANumber + sub;
                            return false;
                        }
                        break;
                    }
                case 'T':
                case 't':
                    if (s.Substring(position, 4).ToLower() == "true")
                    {
                        position += 3;
                        element.val.type = tkValueType.vtBoolean;
                        element.val.bln = true;
                        element.type = tkElementType.etValue;
                    }
                    else
                    {
                        SetErrorMessage(position, s, false);
                        return false;
                    }
                    break;
                case 'F':
                case 'f':
                    if (s.Substring(position, 5).ToLower() == "false")
                    {
                        position += 4;
                        element.val.type = tkValueType.vtBoolean;
                        element.val.bln = false;
                        element.type = tkElementType.etValue;
                    }
                    else
                    {
                        SetErrorMessage(position, s, false);
                        return false;
                    }
                    break;
                case 'N':
                case 'n':
                    if (s.Substring(position, 3).ToLower() == "not")
                    {
                        position += 2;
                        element.type = tkElementType.etOperation;
                        element.priority = 5;
                        element.operation = tkOperation.operNOT;
                    }
                    else
                    {
                        SetErrorMessage(position, s, false);
                        return false;
                    }
                    break;
                case '-':
                    element.type = tkElementType.etOperation;
                    element.priority = 3;
                    element.operation = tkOperation.operChangeSign;
                    element.type = tkElementType.etOperation;
                    break;
                default:
                    SetErrorMessage(position, s, false);
                    return false;
            }
            return true;
        }

        /// <summary>Checks whether there is an operation at the given position. </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool ReadOperation(string s, ref int position, Element element)
        {
            char chr = s[position];

            switch (chr)
            {
                case '!': // ! !=
                    if (s.Substring(position, 2) == "!=")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 4;
                        element.operation = tkOperation.operNotEqual;
                        position++;
                    }
                    else
                    {
                        SetErrorMessage(position, s);
                        return false;
                    }
                    break;
                case '<': // <, <>, "<="
                    if (s.Substring(position, 2) == "<=")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 4;
                        element.operation = tkOperation.operLessEqual;
                        position++;
                    }
                    else if (s.Substring(position, 2) == "<>")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 4;
                        element.operation = tkOperation.operNotEqual;
                        position++;
                    }
                    else
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 4;
                        element.operation = tkOperation.operLess;
                    }
                    break;

                case '>': // >, >=
                    if (s.Substring(position, 2) == ">=")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 4;
                        element.operation = tkOperation.operGrEqual;
                        position++;
                    }
                    else
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 4;
                        element.operation = tkOperation.operGreater;
                    }
                    break;
                case '=':
                    if (s.Substring(position, 2) == "==")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 4;
                        element.operation = tkOperation.operEqual;
                        position += 1;
                    }
                    else
                    {
                        SetErrorMessage(position, s);
                        return false;
                    }
                    break;
                case '+':
                    element.type = tkElementType.etOperation;
                    element.priority = 3;
                    element.operation = tkOperation.operPlus;
                    break;
                case '-':
                    element.type = tkElementType.etOperation;
                    element.priority = 3;
                    element.operation = tkOperation.operMinus;
                    break;
                case '*':
                    element.type = tkElementType.etOperation;
                    element.priority = 2;
                    element.operation = tkOperation.operMult;
                    break;
                case '/':
                    element.type = tkElementType.etOperation;
                    element.priority = 2;
                    element.operation = tkOperation.operDiv;
                    break;
                case '\n':
                    element.type = tkElementType.etOperation;
                    element.priority = 5;
                    element.operation = tkOperation.operLineBreak;
                    break;
                case '\\':
                    element.type = tkElementType.etOperation;
                    element.priority = 2;
                    element.operation = tkOperation.operDivInt;
                    break;
                case '^':
                    element.type = tkElementType.etOperation;
                    element.priority = 1;
                    element.operation = tkOperation.operExpon;
                    break;
                case 'm':
                case 'M':
                    if (s.Substring(position, 3).ToUpper() == "MOD")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 3;
                        element.operation = tkOperation.operMOD;
                        position += 2;
                    }
                    else
                    {
                        SetErrorMessage(position, s);
                        return false;
                    }
                    break;
                case 'a':
                case 'A':
                    if (s.Substring(position, 3).ToUpper() == "AND")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 5;
                        element.operation = tkOperation.operAND;
                        position += 2;
                    }
                    else
                    {
                        SetErrorMessage(position, s);
                        return false;
                    }
                    break;
                case 'o':
                case 'O':
                    if (s.Substring(position, 2).ToUpper() == "OR")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 6;
                        element.operation = tkOperation.operOR;
                        position++;
                    }
                    else
                    {
                        SetErrorMessage(position, s);
                        return false;
                    }
                    break;
                case 'x':
                case 'X':
                    if (s.Substring(position, 3).ToUpper() == "XOR")
                    {
                        element.type = tkElementType.etOperation;
                        element.priority = 6;
                        element.operation = tkOperation.operXOR;
                        position += 2;
                    }
                    else
                    {
                        SetErrorMessage(position, s);
                        return false;
                    }
                    break;
                default:
                    SetErrorMessage(position, s);
                    return false;
            }
            return true;
        }

        /// <summary>Replaces only the Fields in invalid expressions. Before DS 1.8 this was the only thing that got replaced in expressions.
        /// </summary>
        /// <returns>ExpressionString with replaced Fields.</returns>
        private string ReplaceFieldsOnly(DataRow row)
        {
            if (_expressionString == null) return "";
            string s = _expressionString;

            Regex r = new Regex(@"\[(\d+|\w+)\]"); //all fields in [] that contain only word characters or numbers
            var matches = r.Matches(s);

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                string colName = matches[i].Value.Substring(1, matches[i].Length - 2);
                if (row.Table.Columns.Contains(colName))
                {
                    s = s.Replace(matches[i].Value, SaveToString(row[colName]));
                }
            }
            return s;
        }

        /// <summary>Replace the old substring with the new one.</summary>
        /// <param name="s">String in which the substring gets replaced.</param>
        /// <param name="begin">Startposition of old substring.</param>
        /// <param name="length">Length of old substring.</param>
        /// <param name="replacement">New substring.</param>
        private void ReplaceSubString(ref string s, int begin, int length, string replacement)
        {
            string part1 = "";
            string part2 = "";

            if (begin > 0) part1 = s.Substring(0, begin);
            if ((begin + length) < s.Length) part2 = s.Substring(begin + length);

            s = part1 + replacement + part2;
        }

        /// <summary>
        /// Converts the given value to string. Uses _floatingFormat for double.
        /// </summary>
        /// <param name="value">Value that gets converted.</param>
        /// <returns>value as string</returns>
        private string SaveToString(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            else if (value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal)
                return Convert.ToDouble(value).ToString(_floatingFormat);
            else
                return value.ToString();
        }

        /// <summary>
        /// Sets the ErrorMessage according to the given parameters.
        /// </summary>
        /// <param name="position">Position that caused the error.</param>
        /// <param name="s">String that was searched.</param>
        /// <param name="oper">Indicates whether operator or operand is missing.</param>
        private void SetErrorMessage(int position, string s, bool oper = true)
        {
            _errorMessage = string.Format((oper ? SymbologyMessageStrings.Expression_OperatorExprected : SymbologyMessageStrings.Expression_OperandExpected), s[position], position);
        }

        /// <summary>Finds the next position that is not a space.</summary>
        /// <param name="s">String to check.</param>
        /// <param name="position">Startposition.</param>
        private void SkipSpaces(string s, ref int position)
        {
            while (position < s.Length && s[position] == ' ')
            {
                position++;
            }
        }

        #endregion

        #region Enums
        public enum tkValueType
        {
            vtDouble = 0,
            vtString = 1,
            vtBoolean = 2,
            vtObject = 3
        };

        enum tkElementType
        {
            etValue = 0,
            etOperation = 1,
            etPart = 2,
            etNone = 3
        }

        private enum tkOperation
        {
            operEqual = 0, // =
            operNotEqual = 1, // <>
            operLessEqual = 2, // <=
            operGrEqual = 3, // >=
            operGreater = 4, // >
            operLess = 5, // <

            operOR = 6, // OR
            operAND = 7, // AND
            operNOT = 8, // NOT (unary)
            operXOR = 9, // XOR

            operPlus = 11, // +
            operMinus = 12, // -
            operDiv = 13, // /
            operMult = 14, // *
            operMOD = 15, // MOD
            operDivInt = 16, // \
            operExpon = 17, // ^
            operChangeSign = 18, // - (unary)

            operNone = 19,
            operLineBreak = 20
        }
        #endregion

        #region Classes
        public class ExpressionValue
        {
            public double dbl;
            public string str;
            public bool bln;
            public object obj;
            public tkValueType type;
            private string _floatingFormat;

            public ExpressionValue(ref string floatingFormat)
            {
                dbl = 0.0;
                bln = false;
                type = tkValueType.vtDouble;
                _floatingFormat = floatingFormat;
            }

            public override string ToString()
            {
                switch (type)
                {
                    case tkValueType.vtString:
                        return str;
                    case tkValueType.vtObject:
                        if (obj == null || obj == DBNull.Value) return null;
                        return obj.ToString();
                    case tkValueType.vtDouble:
                        return dbl.ToString(_floatingFormat);
                    case tkValueType.vtBoolean:
                        return bln.ToString();
                    default: return null;
                }
            }
        }

        private class Field
        {
            public string Name;
            public tkValueType valueType;

            public Field(string Name, Type type)
            {
                this.Name = Name;

                if (type == typeof(Boolean))
                {
                    this.valueType = tkValueType.vtBoolean;
                }
                else if (type == typeof(Decimal) || type == typeof(Double) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single) || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64))
                {
                    this.valueType = tkValueType.vtDouble;
                }
                else if (type == typeof(String) || type == typeof(Char))
                {
                    this.valueType = tkValueType.vtString;
                }
                else
                {
                    this.valueType = tkValueType.vtObject;
                }
            }
        }

        private class Element
        {
            public Element(ref string floatingFormat)
            {
                wasCalculated = false;
                type = tkElementType.etNone;
                turnedOff = false;
                priority = 255;

                operation = tkOperation.operNone;
                isField = false;
                partIndex = -1;
                val = new ExpressionValue(ref floatingFormat);
                calcVal = new ExpressionValue(ref floatingFormat);
            }

            public tkElementType type; // type of element
            public tkOperation operation; // type of operation
            public int priority; // priority of operation, with less absolute values of priority are preformed first
            public ExpressionValue val; // initial value
            public ExpressionValue calcVal; // value after calculation (in case of consecutive calculations it doesn't rewrite the initial value)

            public bool isField; // the element is field from table
            public Field field;

            // perfoming calculation
            public bool wasCalculated; // the value has been calculated, so calc value should be used henceforth
            public bool turnedOff; // turned off till the end of calculation
            public int partIndex; // the element is result of calculations on the bracket with given index

            public bool setValue(object value)
            {
                if (isField) val.type = field.valueType;

                if (val.type == tkValueType.vtBoolean)
                {
                    if (value is bool) val.bln = (bool)value;
                    else return false;
                }
                else if (val.type == tkValueType.vtString)
                {
                    if (value == null || value == DBNull.Value)
                        val.str = "";
                    else
                        val.str = value.ToString();
                }
                else if (val.type == tkValueType.vtDouble)
                {
                    if (value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal)
                        val.dbl = Convert.ToDouble(value);
                    else return false;
                }
                else if (val.type == tkValueType.vtObject)
                {
                    val.obj = value;
                }
                return true;
            }
        }

        // part of expression in brackets
        private class ExpressionPart
        {
            public List<Element> elements = new List<Element>(); // fields, operators, constants
            public string expression; // for debugging
            public ExpressionValue val;
            public int activeCount;

            public ExpressionPart()
            {
                activeCount = 0;
            }
        }

        private class Operation
        {
            public int id;
            public int left;
            public int right;
            public bool binaryOperation;
        }
        #endregion

    }
}