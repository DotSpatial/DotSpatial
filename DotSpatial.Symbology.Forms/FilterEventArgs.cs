// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2010 8:36:17 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This class in an EventArgs that also supports a filter expression.
    /// </summary>
    public class FilterEventArgs : EventArgs
    {
        private readonly string _filterExpression;

        /// <summary>
        /// Initializes a new instance of the FilterEventArgs class.
        /// </summary>
        /// <param name="filterExpression">String, the filter expression to add.</param>
        public FilterEventArgs(string filterExpression)
        {
            _filterExpression = filterExpression;
        }

        /// <summary>
        /// Gets the string filter expression.
        /// </summary>
        public string FilterExpression
        {
            get
            {
                return _filterExpression;
            }
        }
    }
}