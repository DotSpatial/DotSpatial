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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/26/2010 10:13:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A generic event argument that also allows sending the strong typed item.
    /// </summary>
    /// <typeparam name="T">The generic item associated with the event.</typeparam>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class ItemEventArgs<T> : EventArgs
    {
        private T _item;

        /// <summary>
        /// Creates a new instance of hte LegendItemEventArgs
        /// </summary>
        /// <param name="item">The item associated with the event.  This is not necessarilly the sender.</param>
        public ItemEventArgs(T item)
        {
        }

        /// <summary>
        /// The item associated with this event.  This is not necessarilly the sender.
        /// </summary>
        public T Item
        {
            get { return _item; }
            protected set { _item = value; }
        }
    }
}