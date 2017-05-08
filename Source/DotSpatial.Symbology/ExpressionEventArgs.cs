// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/6/2009 12:19:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Expression Arguments for events.
    /// </summary>
    public class ExpressionEventArgs : EventArgs
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEventArgs"/> class.
        /// </summary>
        /// <param name="expression">The string expression for this event args.</param>
        public ExpressionEventArgs(string expression)
        {
            Expression = expression;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string expression for this event.
        /// </summary>
        public string Expression { get; protected set; }

        #endregion
    }
}