// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A set of PaintEventArgs that can be used before a drawing function in order to cancel an event.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class DrawCompletedArgs : DrawArgs
    {
        private bool _cancelled; // If this is true, the event was cancelled (not aborted by an exception)
        private Exception _exception; // The exception that aborted the drawing process

        /// <summary>
        /// Creates a new instance of the DrawCompleted Event Arguments
        /// </summary>
        /// <param name="inGraphics">A Graphics surface</param>
        /// <param name="inDrawWindow">An implementation of DotSpatial.Geometries.IEnvelope defining the geographic drawing region</param>
        /// <param name="inCancelled">True if the draw method was cancelled before any rendering took place.  A Cancel is different from when an acception is thrown.</param>
        /// <param name="inException">If an exception occured during rendering, this will not be null.</param>
        public DrawCompletedArgs(Graphics inGraphics, DrawWindow inDrawWindow, bool inCancelled, Exception inException)
            : base(inGraphics, inDrawWindow)
        {
            _cancelled = inCancelled;
            _exception = inException;
        }

        /// <summary>
        /// Creates a default DrawCompleted event for the situation where the drawing was successful, not cancelled and no exception occured
        /// </summary>
        /// <param name="args">The DrawArgs being used during the drawing, specifying the Graphics device and geographic envelope</param>
        /// <param name="inCancelled">A boolean parameter specifying whether or not the drawing was cancelled</param>
        public DrawCompletedArgs(DrawArgs args, bool inCancelled)
            : base(args.Graphics, args.DrawWindow, args.Part, args.Stage)
        {
            _cancelled = inCancelled;
            _exception = null;
        }

        /// <summary>
        /// Creates a new DrawCompleted in the case where an exception was thrown
        /// </summary>
        /// <param name="args">The drawing arguments for drawing, specifying the Graphics device and the geographic envelope </param>
        /// <param name="inException">The Exception being thrown</param>
        public DrawCompletedArgs(DrawArgs args, Exception inException)
            : base(args.Graphics, args.DrawWindow, args.Part, args.Stage)
        {
            _exception = inException;
        }

        /// <summary>
        /// Boolean, true if the draw method was cancelled before rendering took place.
        /// This will not be true if visible was false or an exception was thrown.
        /// </summary>
        public virtual bool Cancelled
        {
            get
            {
                return _cancelled;
            }
            protected set
            {
                _cancelled = value;
            }
        }

        /// <summary>
        /// An Exception object for situations where the drawing threw an exception.
        /// </summary>
        public virtual Exception Exception
        {
            get
            {
                return _exception;
            }
            protected set
            {
                _exception = value;
            }
        }
    }
}