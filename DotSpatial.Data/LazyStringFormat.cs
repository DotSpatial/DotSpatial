// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Data.dll version 1.0
//
// The Initial Developer of this Original Code is Kyle Ellison. Created 2012.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name             |   Date    |   Description
// -------------------------------------------------------------------------------------------------
//                  |           |   
// ********************************************************************************************************
using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Creates a formatted string only when the Value property is first accessed.
    /// </summary>
    public class LazyStringFormat
    {
        private readonly object[] _args;
        private readonly string _formatString;
        private string _value;

        /// <summary>
        /// Construct a LazyStringFormat
        /// </summary>
        /// <param name="formatString"></param>
        /// <param name="args"></param>
        public LazyStringFormat(string formatString, params object[] args)
        {
            _formatString = formatString;
            _args = args;
        }

        /// <summary>
        /// Get the value which actually calls String.Format()
        /// </summary>
        public string Value
        {
            get { return _value ?? (_value = String.Format(_formatString, _args)); }
        }
    }
}
