// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2008 12:50:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DrawArgs contains the parameters necessary for 2D drawing.
    /// </summary>
    public class DrawArgs : EventArgs
    {
        #region Fields

        private DrawWindow _drawWindow;
        private Graphics _graphics;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawArgs"/> class.
        /// </summary>
        /// <param name="inGraphics">A System.Windows.Drawing.Graphics object</param>
        /// <param name="inDrawWindow">A DotSpatial.Drawing.DrawWindow to draw to</param>
        public DrawArgs(Graphics inGraphics, DrawWindow inDrawWindow)
            : this(inGraphics, inDrawWindow, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawArgs"/> class.
        /// </summary>
        /// <param name="inGraphics">A System.Windows.Drawing.Graphics object</param>
        /// <param name="inDrawWindow">A DotSpatial.Drawing.DrawWindow to draw to</param>
        /// <param name="inPart">An integer part representing a value from 0 to NumParts being drawn</param>
        public DrawArgs(Graphics inGraphics, DrawWindow inDrawWindow, int inPart)
            : this(inGraphics, inDrawWindow, inPart, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawArgs"/> class.
        /// </summary>
        /// <param name="inGraphics">A System.Windows.Drawing.Graphics object</param>
        /// <param name="inDrawWindow">A DotSpatial.Drawing.DrawWindow to draw to</param>
        /// <param name="inPart">An integer part representing a value from 0 to NumParts being drawn</param>
        /// <param name="inStage">The integer stage representing an object that has several stages, for all of the parts to be passed through</param>
        public DrawArgs(Graphics inGraphics, DrawWindow inDrawWindow, int inPart, int inStage)
        {
            _graphics = inGraphics;
            _drawWindow = inDrawWindow;
            Part = inPart;
            Stage = inStage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic extent for the drawing operation.
        /// </summary>
        public virtual DrawWindow DrawWindow
        {
            get
            {
                return _drawWindow;
            }

            protected set
            {
                _drawWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets the Graphics device to draw to.
        /// </summary>
        public virtual Graphics Graphics
        {
            get
            {
                return _graphics;
            }

            protected set
            {
                _graphics = value;
            }
        }

        /// <summary>
        /// Gets or sets the part index being drawn.
        /// </summary>
        public int Part { get; protected set; }

        /// <summary>
        /// Gets or sets the integer stage index. As an example, if all the borders are drawn first, and then all the fillings are drawn,
        /// but each stage has several parts, the stage gives a way to subdivide a larger object into several drawing passes.
        /// </summary>
        public int Stage { get; protected set; }

        #endregion
    }
}