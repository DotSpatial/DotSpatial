// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
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