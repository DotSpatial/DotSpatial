// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
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
// The Original Code is from DotSpatial.Symbology.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/29/2010 10:34:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An EventArgs specifically tailored to DynamicVisibilityMode.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class DynamicVisibilityEventArgs : EventArgs
    {
        private IDynamicVisibility _item;

        /// <summary>
        /// Initializes a new instance of the DynamicVisibilityModeEventArgs class.
        /// </summary>
        /// <param name="item">The item that supports IDynamicVisibility.</param>
        public DynamicVisibilityEventArgs(IDynamicVisibility item)
        {
            _item = item;
        }

        /// <summary>
        /// Gets the item that supports dynamic visibility.
        /// </summary>
        public IDynamicVisibility Item
        {
            get { return _item; }
            protected set { _item = value; }
        }
    }
}