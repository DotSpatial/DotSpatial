// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/11/2009 2:34:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An EventArgs class for passing around an extent.
    /// </summary>
    public class ExtentArgs : EventArgs
    {
        private Extent _extent;

        /// <summary>
        /// Initializes a new instance of the ExtentArgs class.
        /// </summary>
        /// <param name="value">The value for this event.</param>
        public ExtentArgs(Extent value)
        {
            _extent = value;
        }

        /// <summary>
        /// Gets or sets the Extents for this event.
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }
            protected set { _extent = value; }
        }
    }
}