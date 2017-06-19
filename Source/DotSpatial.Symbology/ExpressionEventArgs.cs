// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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