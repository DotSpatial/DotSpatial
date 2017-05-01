// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/21/2009 11:04:37 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This class contains the arguments for the clip event.
    /// </summary>
    public class ClipArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipArgs"/> class.
        /// </summary>
        /// <param name="clipRectangles">Rectangles of this event.</param>
        public ClipArgs(List<Rectangle> clipRectangles)
        {
            ClipRectangles = clipRectangles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipArgs"/> class from a single rectangle instead of a list of rectangles.
        /// </summary>
        /// <param name="clipRectangle">The clip rectangle</param>
        public ClipArgs(Rectangle clipRectangle)
            : this(new List<Rectangle>
                   {
                       clipRectangle
                   })
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ClipRectangle for this event.
        /// </summary>
        public List<Rectangle> ClipRectangles { get; protected set; }

        #endregion
    }
}