// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/7/2008 4:50:34 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The main difference here is that tests against the DrawWindow happen before the nested Draw2D methods.
    /// The DrawWindow is editable at this stage, but will become read-only during drawing.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class DrawWindowArgs : EventArgs
    {
        #region Private Variables

        private DrawWindow _drawWindow;
        private Graphics _graphics;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DrawArgs
        /// </summary>
        /// <param name="inGraphics">A System.Windows.Drawing.Graphics object</param>
        /// <param name="inDrawWindow">A DotSpatial.Drawing.DrawWindow to draw to</param>
        public DrawWindowArgs(Graphics inGraphics, DrawWindow inDrawWindow)
        {
            _graphics = inGraphics;
            _drawWindow = inDrawWindow;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Graphics device to draw to
        /// </summary>
        public virtual Graphics Graphics
        {
            get { return _graphics; }
            protected set { _graphics = value; }
        }

        /// <summary>
        /// Gets or sets the geographic extent for the drawing operation
        /// </summary>
        public virtual DrawWindow DrawWindow
        {
            get { return _drawWindow; }
            set { _drawWindow = value; }
        }

        #endregion
    }
}