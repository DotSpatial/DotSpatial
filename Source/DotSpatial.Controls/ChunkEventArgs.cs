// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 11:46:40 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Controls
{
    /// <summary>
    /// ChunkEventArgs
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class ChunkEventArgs : EventArgs
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ChunkEventArgs
        /// </summary>
        public ChunkEventArgs(int inCurrentChunk, int inNumChunks)
        {
            CurrentChunk = inCurrentChunk;
            NumChunks = inNumChunks;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current chunk for this event
        /// </summary>
        public int CurrentChunk { get; protected set; }

        /// <summary>
        /// Gets the number of chunks
        /// </summary>
        public int NumChunks { get; protected set; }

        #endregion
    }
}