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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Expressions define the scheme that should be used to build the labels of a LabelCategory. 
    /// </summary>
    public class Expression
    {
        #region Members
        private readonly List<ExpressionPart> _parts = new List<ExpressionPart>();
        private readonly List<Element> _variables = new List<Element>();
        private readonly List<Field> _fields = new List<Field>();
        private readonly List<Operation> _operations = new List<Operation>();
        private readonly List<string> _strings = new List<string>();
        private bool _saveOperations;
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
        public string ErrorMessage { get; private set; }

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
        /// <param name="fid">FID value that is used to replace the FID field.</param>
        /// <returns>The calculated expression.</returns>
        public string CalculateRowValue(DataRow row, int fid)
        {
            if (IsEmpty()) return "";
            ErrorMessage = "";
            if (!_valid && !_expChanged) return ReplaceFieldsOnly(row, fid); //expression is invalid and hasn't changed => simply replace fields

            foreach (Element e in _variables)
            {
                if (e.IsField)
                {
                    if (e.Field.Name.ToLower() == "fid")
                    { e.SetValue(fid); }
                    else if (row.Table.Columns.Contains(e.Field.Name))
                    { e.SetValue(row[e.Field.Name]); }
                }
            }
            ExpressionValue expval = Calculate();
            if (expval == null)
            {
                _valid = false;
                return ReplaceFieldsOnly(row, fid);
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

            ErrorMessage = "";
            if (row == null) //no row -> use exampleValues
            {
                foreach (Element e in _variables)
                {
                    if (e.IsField)
                    {
                        if (e.Value.Type == TkValueType.VtBoolean)
                        { e.SetValue(false); }
                        else if (e.Value.Type == TkValueType.VtDouble)
                        { e.SetValue(2.4); }
                        else if (e.Value.Type == TkValueType.VtObject)
                        { e.SetValue("obj"); }
                        else if (e.Value.Type == TkValueType.VtString)
                        { e.SetValue("str"); }
                    }
                }
            }
            else //use DataRow values
            {
                foreach (Element e in _variables)
                {
                    if (e.IsField && row.Table.Columns.Contains(e.Field.Name))
                    {
                        e.SetValue(row[e.Field.Name]);
                    }
                }
            }

            ExpressionValue expval = Calculate();
            _valid = expval != null;
            if (_valid)
            {
                // ReSharper disable once PossibleNullReferenceException
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
            ErrorMessage = "";
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

            //ExpressionPart bracket = new ExpressionPart();

            _saveOperations = true;
            _variables.Clear();
            _parts.Clear();
            _operations.Clear();
            _strings.Clear();


            foreach (string c in new string[] { "{", "}" })
            {
                int pos = s.IndexOf(c, StringComparison.Ordinal);
                if (pos > -1)
                {
                    ErrorMessage = "Unallowed character " + c + " at " + pos;
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
                ErrorMessage = SymbologyMessageStrings.Expression_UnpairedTextquotes;
                return false;
            }
            if (s.Contains("[]"))
            {
                ErrorMessage = SymbologyMessageStrings.Expression_EmptyField;
                return false;
            }
            if (s.Contains("[") || s.Contains("]"))
            {
                ErrorMessage = SymbologyMessageStrings.Expression_UnpairedBracket;
                return false;
            }

            bool found = true;
            while (found)
            {
                // seeking brackets
                int begin = 0;
                int end;
                found = GetBrackets(s, ref begin, out end);

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
                for (int j = 0; j < part.Elements.Count; j++)
                {
                    if (part.Elements[j].IsField)
                    {
                        _variables.Add(part.Elements[j]);
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
            if (_parts.Count == 0) return null;
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
                part.ActiveCount = part.Elements.Count;
                foreach (var ele in part.Elements)
                {
                    ele.WasCalculated = false;
                    ele.TurnedOff = false;
                }
            }

            do
            {
                ExpressionPart part = _parts[partIndex];

                // if there is more then one element, then definitely some operation must be present
                if (part.Elements.Count > 1)
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

                    part.ActiveCount -= operation.BinaryOperation ? 2 : 1;
                }

                // if there is only one element left, we'll finalize the part
                if (part.ActiveCount == 1)
                {
                    int size = part.Elements.Count;
                    for (int i = 0; i < size; i++)
                    {
                        if (!part.Elements[i].TurnedOff)
                        {
                            part.Value = GetValue(part, i);
                            part.Elements[i].TurnedOff = true;
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

            return success ? _parts[_parts.Count - 1].Value : null;
        }

        /// <summary>
        /// Calculates the given operation for the values of the given part.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private bool CalculateOperation(ExpressionPart part, Operation operation)
        {
            TkOperation oper = part.Elements[operation.Id].Operation;

            ExpressionValue valRight = GetValue(part, operation.Right);
            Element elRight = part.Elements[operation.Right];

            if (oper == TkOperation.OperNot)   //  tis is an unary operator and we read only right operand
            {
                if (valRight.Type != TkValueType.VtBoolean)
                {
                    ErrorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                    return false;
                }
                elRight.CalcValue.Bln = !(valRight.Bln);
                elRight.CalcValue.Type = TkValueType.VtBoolean;
                elRight.WasCalculated = true;
                part.Elements[operation.Id].TurnedOff = true;
            }
            else if (oper == TkOperation.OperChangeSign)  // tis is an unary operator and we read only right operand
            {
                if (valRight.Type != TkValueType.VtDouble)
                {
                    ErrorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                    return false;
                }
                elRight.CalcValue.Dbl = -valRight.Dbl;
                elRight.CalcValue.Type = TkValueType.VtDouble;
                elRight.WasCalculated = true;
                part.Elements[operation.Id].TurnedOff = true;
            }
            else // these are binary operators as we read left and right operands
            {
                ExpressionValue valLeft = GetValue(part, operation.Left);
                Element elLeft = part.Elements[operation.Left];

                if (oper == TkOperation.OperLineBreak)
                {
                    elLeft.CalcValue.Str = valLeft + Environment.NewLine + valRight;
                    elLeft.CalcValue.Type = TkValueType.VtString;
                }
                else if (valLeft.Type == valRight.Type)
                {
                    if (valLeft.Type == TkValueType.VtDouble)
                    {
                        CalculateDoubleOperation(oper, elLeft, elRight, valLeft, valRight);
                    }
                    else if (valLeft.Type == TkValueType.VtString)
                    {
                        CalculateStringOperation(oper, elLeft, elRight, valLeft, valRight);
                    }
                    else if (valLeft.Type == TkValueType.VtBoolean)
                    {
                        CalculateBoolOperation(oper, elLeft, elRight, valLeft, valRight);
                    }
                }
                else if (oper != TkOperation.OperPlus) // plus is the only operation that can have different valuetypes
                {
                    ErrorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                    return false;
                }
                else if (valLeft.Type == TkValueType.VtBoolean || valRight.Type == TkValueType.VtBoolean) // boolean can't be added
                {
                    ErrorMessage = SymbologyMessageStrings.Expression_PlusNotAllowed;
                    return false;
                }
                else //concat strings and doubles
                {
                    elLeft.CalcValue.Str = valLeft + valRight.ToString();
                    elLeft.CalcValue.Type = TkValueType.VtString;
                }
                elLeft.WasCalculated = true;
                part.Elements[operation.Id].TurnedOff = true;
                part.Elements[operation.Right].TurnedOff = true;
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
        private bool CalculateBoolOperation(TkOperation oper, Element elLeft, Element elRight, ExpressionValue valLeft, ExpressionValue valRight)
        {
            switch (oper)
            {
                // logical operators
                case TkOperation.OperOr:
                    elLeft.CalcValue.Bln = valLeft.Bln || valRight.Bln;
                    break;
                case TkOperation.OperAnd:
                    elLeft.CalcValue.Bln = valLeft.Bln && valRight.Bln;
                    break;
                case TkOperation.OperXor:
                    elLeft.CalcValue.Bln = valLeft.Bln ^ valRight.Bln;
                    break;
                case TkOperation.OperEqual:
                    elLeft.CalcValue.Bln = valLeft.Bln == valRight.Bln;
                    break;
                case TkOperation.OperNotEqual:
                    elLeft.CalcValue.Bln = valLeft.Bln != valRight.Bln;
                    break;
                default:
                    {
                        ErrorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
                        return false;
                    }
            }
            elLeft.CalcValue.Type = TkValueType.VtBoolean;
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
        private bool CalculateDoubleOperation(TkOperation oper, Element elLeft, Element elRight, ExpressionValue valLeft, ExpressionValue valRight)
        {
            elLeft.CalcValue.Type = TkValueType.VtDouble;
            switch (oper)
            {
                case TkOperation.OperLess:
                    elLeft.CalcValue.Bln = valLeft.Dbl < valRight.Dbl;
                    elLeft.CalcValue.Type = TkValueType.VtBoolean;
                    break;
                case TkOperation.OperLessEqual:
                    elLeft.CalcValue.Bln = valLeft.Dbl <= valRight.Dbl;
                    elLeft.CalcValue.Type = TkValueType.VtBoolean;
                    break;
                case TkOperation.OperGreater:
                    elLeft.CalcValue.Bln = valLeft.Dbl > valRight.Dbl;
                    elLeft.CalcValue.Type = TkValueType.VtBoolean;
                    break;
                case TkOperation.OperGrEqual:
                    elLeft.CalcValue.Bln = valLeft.Dbl >= valRight.Dbl;
                    elLeft.CalcValue.Type = TkValueType.VtBoolean;
                    break;
                case TkOperation.OperEqual:
                    elLeft.CalcValue.Bln = valLeft.Dbl == valRight.Dbl;
                    elLeft.CalcValue.Type = TkValueType.VtBoolean;
                    break;
                case TkOperation.OperNotEqual:
                    elLeft.CalcValue.Bln = valLeft.Dbl != valRight.Dbl;
                    elLeft.CalcValue.Type = TkValueType.VtBoolean;
                    break;
                case TkOperation.OperMinus:
                    elLeft.CalcValue.Dbl = valLeft.Dbl - valRight.Dbl;
                    elLeft.CalcValue.Type = TkValueType.VtDouble;
                    break;
                case TkOperation.OperMult:
                    elLeft.CalcValue.Dbl = valLeft.Dbl * valRight.Dbl;
                    break;
                case TkOperation.OperExpon:
                    elLeft.CalcValue.Dbl = Math.Pow(valLeft.Dbl, valRight.Dbl);
                    break;
                case TkOperation.OperMod:
                    elLeft.CalcValue.Dbl = (int)valLeft.Dbl % (int)valRight.Dbl;
                    break;
                case TkOperation.OperDiv:
                    if (valRight.Dbl == 0.0)
                    {
                        ErrorMessage = SymbologyMessageStrings.Expression_ZeroDivision;
                        return false;
                    }
                    elLeft.CalcValue.Dbl = valLeft.Dbl / valRight.Dbl;
                    break;
                case TkOperation.OperDivInt:
                    if (valRight.Dbl == 0.0)
                    {
                        ErrorMessage = SymbologyMessageStrings.Expression_ZeroDivision;
                        return false;
                    }
                    elLeft.CalcValue.Dbl = (int)valLeft.Dbl / (int)valRight.Dbl;
                    break;
                case TkOperation.OperPlus:
                    elLeft.CalcValue.Dbl = valLeft.Dbl + valRight.Dbl;
                    break;
                default:
                    {
                        ErrorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
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
        private bool CalculateStringOperation(TkOperation oper, Element elLeft, Element elRight, ExpressionValue valLeft, ExpressionValue valRight)
        {
            int res = string.Compare(valLeft.Str.ToLower(), valRight.Str.ToLower());
            elLeft.CalcValue.Bln = false;
            elLeft.CalcValue.Type = TkValueType.VtBoolean;
            switch (oper)
            {
                case TkOperation.OperPlus:
                    elLeft.CalcValue.Str = valLeft.Str + valRight.Str;
                    elLeft.CalcValue.Type = TkValueType.VtString;
                    break;
                case TkOperation.OperLess:
                    if (res < 0) elLeft.CalcValue.Bln = true;
                    break;
                case TkOperation.OperLessEqual:
                    if (res <= 0) elLeft.CalcValue.Bln = true;
                    break;
                case TkOperation.OperGreater:
                    if (res > 0) elLeft.CalcValue.Bln = true;
                    break;
                case TkOperation.OperGrEqual:
                    if (res >= 0) elLeft.CalcValue.Bln = true;
                    break;
                case TkOperation.OperEqual:
                    if (res == 0) elLeft.CalcValue.Bln = true;
                    break;
                case TkOperation.OperNotEqual:
                    if (res != 0) elLeft.CalcValue.Bln = true;
                    break;
                default:
                    ErrorMessage = SymbologyMessageStrings.Expression_OperationNotSupported;
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

            List<Element> elements = part.Elements;
            int size = elements.Count;
            for (int i = 0; i < size; i++)
            {
                Element element = elements[i];
                if (!element.TurnedOff)
                {
                    if (element.Type == TkElementType.EtOperation)
                    {
                        if (element.Priority < priority)
                        {
                            found = true;
                            priority = element.Priority;
                            operation.Id = i;
                        }
                    }
                }
            }
            if (!found)
            {
                ErrorMessage = SymbologyMessageStrings.Expression_OperationNotFound;
                return false;
            }

            // seeking right operand
            operation.Left = operation.Right = -1;
            for (int i = operation.Id + 1; i < size; i++)
            {
                Element element = elements[i];
                if (!element.TurnedOff)
                {
                    if (element.Type == TkElementType.EtOperation)
                    {
                        if (element.Operation != TkOperation.OperNot && element.Operation != TkOperation.OperChangeSign)
                        {
                            ErrorMessage = SymbologyMessageStrings.Expression_OpereratorInsteadOfValue;
                            return false;
                        }
                    }
                    else
                    {
                        operation.Right = i;
                        break;
                    }
                }
            }
            if (operation.Right == -1)
            {
                ErrorMessage = SymbologyMessageStrings.Expression_RightOperandMissing;
                return false;
            }

            // if the operator is binary, seeking left operand
            if (elements[operation.Id].Operation != TkOperation.OperNot && elements[operation.Id].Operation != TkOperation.OperChangeSign)
            {
                for (int i = operation.Id - 1; i >= 0; i--)
                {
                    if (!elements[i].TurnedOff)
                    {
                        operation.Left = i;
                        break;
                    }
                }
                if (operation.Left == -1)
                {
                    ErrorMessage = SymbologyMessageStrings.Expression_LeftOperandMissing;
                    return false;
                }
                operation.BinaryOperation = true;
            }
            else
            {
                operation.BinaryOperation = false;
            }

            // caching operations
            if (_saveOperations)
            {
                Operation op = new Operation();
                op.Left = operation.Left;
                op.Right = operation.Right;
                op.Id = operation.Id;
                op.BinaryOperation = operation.BinaryOperation;
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
        private bool GetBrackets(string expression, ref int begin, out int end, string openingSymbol = "(", string closingSymbol = ")")
        {
            // closing bracket
            end = expression.IndexOf(closingSymbol, StringComparison.Ordinal);
            if (end == -1) return false;
            //opening bracket
            begin = expression.LastIndexOf(openingSymbol, end, StringComparison.Ordinal);
            return begin != -1;
        }

        /// <summary>Gets the ExpressionValue of the element of the given part.
        /// </summary>
        /// <param name="part">Part that contains the element.</param>
        /// <param name="elementId">Id of the element whose value is returned.</param>
        /// <returns>Value of the element.</returns>
        private ExpressionValue GetValue(ExpressionPart part, int elementId)
        {
            Element element = part.Elements[elementId];

            if (element.WasCalculated)
                return element.CalcValue;
            return element.PartIndex != -1 ? _parts[element.PartIndex].Value : element.Value;
        }

        /// <summary>
        /// Checks whether the given character could belong to a number.
        /// </summary>
        /// <param name="chr">Character that is checked.</param>
        /// <param name="exponential">Remembers whether last character was exponential to check whether +- are allowed.</param>
        /// <returns>False if character can't belong to a number.</returns>
        private bool IsDecimal(char chr, ref bool exponential)
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
                ErrorMessage = SymbologyMessageStrings.Expression_Empty;
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
            ExpressionPart part = new ExpressionPart { Expression = s };

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
                part.Elements.Add(element);

                //in case operation was unary the next element should be value as well
                if (element.Operation != TkOperation.OperNot && element.Operation != TkOperation.OperChangeSign)
                    readVal = !readVal;
            }

            if (part.Elements.Count <= 0) return false;
            _parts.Add(part);
            return true;
        }

        /// <summary> Checks whether there is a value or unary operator at the given position.</summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool ReadValue(string s, ref int position, Element element)
        {
            string sub = ""; // substring
            char chr = s[position];

            switch (chr)
            {
                case '{': //fields, strings or parts
                    {
                        sub = "";
                        position++; //opening bracket
                        chr = s[position]; //s, p or f according to replaced text
                        position++;

                        int closingIndex = s.IndexOf("}", position, StringComparison.Ordinal);
                        if (closingIndex < 0)
                        {
                            ErrorMessage = SymbologyMessageStrings.Expression_ClosingBracket;
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
                            element.Type = TkElementType.EtValue;
                            element.Value.Type = TkValueType.VtString;
                            element.Value.Str = sub;
                        }
                        else if (chr == 'f') //field replacer
                        {
                            int index = Convert.ToInt32(sub);
                            sub = _strings[index].TrimStart('[').TrimEnd(']');

                            if (sub.ToLower() == "fid")
                            {
                                element.Field = new Field("fid", typeof(int));
                            }
                            else
                            {
                                int fieldIndex = _fields.FindIndex(p => p.Name.ToLower() == sub.ToLower());
                                if (fieldIndex < 0)
                                {
                                    ErrorMessage = SymbologyMessageStrings.Expression_FieldNotFound + sub;
                                    return false;
                                }
                                element.Field = _fields[fieldIndex];
                            }
                            element.IsField = true;
                            element.Type = TkElementType.EtValue;
                        }
                        else if (chr == 'p') //part replacer
                        {
                            element.PartIndex = Convert.ToInt32(sub);
                            element.Type = TkElementType.EtPart;
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
                            sub += chr;
                            position++;
                            if (position > s.Length - 1)
                                break;
                            chr = s[position];
                        }
                        position--;

                        double res;
                        if (double.TryParse(sub, out res))
                        {
                            element.Type = TkElementType.EtValue;
                            element.Value.Type = TkValueType.VtDouble;
                            element.Value.Dbl = res;
                        }
                        else
                        {
                            ErrorMessage = SymbologyMessageStrings.Expression_NotANumber + sub;
                            return false;
                        }
                        break;
                    }
                case 'T':
                case 't':
                    if (s.Substring(position, 4).ToLower() == "true")
                    {
                        position += 3;
                        element.Value.Type = TkValueType.VtBoolean;
                        element.Value.Bln = true;
                        element.Type = TkElementType.EtValue;
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
                        element.Value.Type = TkValueType.VtBoolean;
                        element.Value.Bln = false;
                        element.Type = TkElementType.EtValue;
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
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 5;
                        element.Operation = TkOperation.OperNot;
                    }
                    else
                    {
                        SetErrorMessage(position, s, false);
                        return false;
                    }
                    break;
                case '-':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 3;
                    element.Operation = TkOperation.OperChangeSign;
                    element.Type = TkElementType.EtOperation;
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
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 4;
                        element.Operation = TkOperation.OperNotEqual;
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
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 4;
                        element.Operation = TkOperation.OperLessEqual;
                        position++;
                    }
                    else if (s.Substring(position, 2) == "<>")
                    {
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 4;
                        element.Operation = TkOperation.OperNotEqual;
                        position++;
                    }
                    else
                    {
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 4;
                        element.Operation = TkOperation.OperLess;
                    }
                    break;

                case '>': // >, >=
                    if (s.Substring(position, 2) == ">=")
                    {
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 4;
                        element.Operation = TkOperation.OperGrEqual;
                        position++;
                    }
                    else
                    {
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 4;
                        element.Operation = TkOperation.OperGreater;
                    }
                    break;
                case '=':
                    if (s.Substring(position, 2) == "==")
                    {
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 4;
                        element.Operation = TkOperation.OperEqual;
                        position += 1;
                    }
                    else
                    {
                        SetErrorMessage(position, s);
                        return false;
                    }
                    break;
                case '+':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 3;
                    element.Operation = TkOperation.OperPlus;
                    break;
                case '-':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 3;
                    element.Operation = TkOperation.OperMinus;
                    break;
                case '*':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 2;
                    element.Operation = TkOperation.OperMult;
                    break;
                case '/':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 2;
                    element.Operation = TkOperation.OperDiv;
                    break;
                case '\n':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 5;
                    element.Operation = TkOperation.OperLineBreak;
                    break;
                case '\\':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 2;
                    element.Operation = TkOperation.OperDivInt;
                    break;
                case '^':
                    element.Type = TkElementType.EtOperation;
                    element.Priority = 1;
                    element.Operation = TkOperation.OperExpon;
                    break;
                case 'm':
                case 'M':
                    if (s.Substring(position, 3).ToUpper() == "MOD")
                    {
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 3;
                        element.Operation = TkOperation.OperMod;
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
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 5;
                        element.Operation = TkOperation.OperAnd;
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
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 6;
                        element.Operation = TkOperation.OperOr;
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
                        element.Type = TkElementType.EtOperation;
                        element.Priority = 6;
                        element.Operation = TkOperation.OperXor;
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
        /// <param name="row">Datarow that contains the data used to replace the fields.</param>
        /// <param name="fid">FID that is used to replace the FID field.</param>
        /// <returns>ExpressionString with replaced Fields.</returns>
        private string ReplaceFieldsOnly(DataRow row, int fid)
        {
            if (_expressionString == null) return "";
            string s = _expressionString;

            Regex r = new Regex(@"\[(\d+|\w+)\]"); //all fields in [] that contain only word characters or numbers
            var matches = r.Matches(s);

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                string colName = matches[i].Value.Substring(1, matches[i].Length - 2);

                if (colName.ToLower() == "fid")
                {
                    s = s.Replace(matches[i].Value, SaveToString(fid));
                }
                else if (row.Table.Columns.Contains(colName))
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
            if (value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal)
                return Convert.ToDouble(value).ToString(_floatingFormat);
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
            ErrorMessage = string.Format((oper ? SymbologyMessageStrings.Expression_OperatorExprected : SymbologyMessageStrings.Expression_OperandExpected), s[position], position);
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
        private enum TkValueType
        {
            VtDouble = 0,
            VtString = 1,
            VtBoolean = 2,
            VtObject = 3
        };

        private enum TkElementType
        {
            EtValue = 0,
            EtOperation = 1,
            EtPart = 2,
            EtNone = 3
        }

        private enum TkOperation
        {
            OperEqual = 0, // =
            OperNotEqual = 1, // <>
            OperLessEqual = 2, // <=
            OperGrEqual = 3, // >=
            OperGreater = 4, // >
            OperLess = 5, // <

            OperOr = 6, // OR
            OperAnd = 7, // AND
            OperNot = 8, // NOT (unary)
            OperXor = 9, // XOR

            OperPlus = 11, // +
            OperMinus = 12, // -
            OperDiv = 13, // /
            OperMult = 14, // *
            OperMod = 15, // MOD
            OperDivInt = 16, // \
            OperExpon = 17, // ^
            OperChangeSign = 18, // - (unary)

            OperNone = 19,
            OperLineBreak = 20
        }
        #endregion

        #region Classes
        private class ExpressionValue
        {
            public double Dbl;
            public string Str;
            public bool Bln;
            public object Obj;
            public TkValueType Type;
            private readonly string _floatingFormat;

            public ExpressionValue(ref string floatingFormat)
            {
                Dbl = 0.0;
                Bln = false;
                Type = TkValueType.VtDouble;
                _floatingFormat = floatingFormat;
            }

            public override string ToString()
            {
                switch (Type)
                {
                    case TkValueType.VtString:
                        return Str;
                    case TkValueType.VtObject:
                        if (Obj == null || Obj == DBNull.Value) return null;
                        return Obj.ToString();
                    case TkValueType.VtDouble:
                        return Dbl.ToString(_floatingFormat);
                    case TkValueType.VtBoolean:
                        return Bln.ToString();
                    default: return null;
                }
            }
        }

        private class Field
        {
            public readonly string Name;
            public readonly TkValueType ValueType;

            public Field(string name, Type type)
            {
                this.Name = name;

                if (type == typeof(bool))
                {
                    this.ValueType = TkValueType.VtBoolean;
                }
                else if (type == typeof(decimal) || type == typeof(double) || type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
                {
                    this.ValueType = TkValueType.VtDouble;
                }
                else if (type == typeof(string) || type == typeof(char))
                {
                    this.ValueType = TkValueType.VtString;
                }
                else
                {
                    this.ValueType = TkValueType.VtObject;
                }
            }
        }

        private class Element
        {
            public Element(ref string floatingFormat)
            {
                WasCalculated = false;
                Type = TkElementType.EtNone;
                TurnedOff = false;
                Priority = 255;

                Operation = TkOperation.OperNone;
                IsField = false;
                PartIndex = -1;
                Value = new ExpressionValue(ref floatingFormat);
                CalcValue = new ExpressionValue(ref floatingFormat);
            }

            public TkElementType Type; // type of element
            public TkOperation Operation; // type of operation
            public int Priority; // priority of operation, with less absolute values of priority are preformed first
            public readonly ExpressionValue Value; // initial value
            public readonly ExpressionValue CalcValue; // value after calculation (in case of consecutive calculations it doesn't rewrite the initial value)

            public bool IsField; // the element is field from table
            public Field Field;

            // perfoming calculation
            public bool WasCalculated; // the value has been calculated, so calc value should be used henceforth
            public bool TurnedOff; // turned off till the end of calculation
            public int PartIndex; // the element is result of calculations on the bracket with given index

            public bool SetValue(object value)
            {
                if (IsField) Value.Type = Field.ValueType;

                if (Value.Type == TkValueType.VtBoolean)
                {
                    if (value is bool) Value.Bln = (bool)value;
                    else return false;
                }
                else if (Value.Type == TkValueType.VtString)
                {
                    if (value == null || value == DBNull.Value)
                        Value.Str = "";
                    else
                        Value.Str = value.ToString();
                }
                else if (Value.Type == TkValueType.VtDouble)
                {
                    if (value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal)
                        Value.Dbl = Convert.ToDouble(value);
                    else return false;
                }
                else if (Value.Type == TkValueType.VtObject)
                {
                    Value.Obj = value;
                }
                return true;
            }
        }

        // part of expression in brackets
        private class ExpressionPart
        {
            public readonly List<Element> Elements = new List<Element>(); // fields, operators, constants
            public string Expression; // for debugging
            public ExpressionValue Value;
            public int ActiveCount;

            public ExpressionPart()
            {
                ActiveCount = 0;
            }
        }

        private class Operation
        {
            public int Id;
            public int Left;
            public int Right;
            public bool BinaryOperation;
        }
        #endregion
    }
}