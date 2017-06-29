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
    public class DrawVerifyArgs : DrawArgs
    {
        private bool _cancel;

        /// <summary>
        /// Creates a new instance of the CancelPaint Event Arguments.
        /// </summary>
        /// <param name="inGraphics">The device that contains the drawing information for the event to determine whether or not to cancel.</param>
        /// <param name="inDrawWindow">The geographic bounds of the draw event</param>
        /// <param name="inPart">The part being drawn.  This is usually 0, unless NumParts is greater than 1.</param>
        /// <param name="inStage">The 0-based integer index indicating the stage of the drawing.</param>
        /// <param name="inCancel">A boolean specifying the default setting for the draw args.</param>
        public DrawVerifyArgs(Graphics inGraphics, DrawWindow inDrawWindow, int inPart, int inStage, bool inCancel)
            : base(inGraphics, inDrawWindow, inPart, inStage)
        {
            _cancel = inCancel;
        }

        /// <summary>
        /// Constructs a new instance with cancel set to false using an Existing DrawArgs
        /// </summary>
        /// <param name="args"></param>
        public DrawVerifyArgs(DrawArgs args)
            : base(args.Graphics, args.DrawWindow, args.Part, args.Stage)
        {
            _cancel = false;
        }

        /// <summary>
        /// Returns a boolean specifying whether the action that caused the default drawing
        /// for this event should be prevented.
        /// </summary>
        public virtual bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }
    }
}