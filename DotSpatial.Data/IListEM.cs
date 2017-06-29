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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/4/2009 4:27:39 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// IListEM
    /// </summary>
    public static class IListEM
    {
        /// <summary>
        /// This extension method helps by simply increasing the index value of the specified item
        /// by one.
        /// </summary>
        /// <typeparam name="T">The generic type of this list</typeparam>
        /// <param name="self">This list</param>
        /// <param name="item">The item to increase the index of</param>
        public static bool IncreaseIndex<T>(this IList<T> self, T item)
        {
            int index = self.IndexOf(item);
            if (index == -1) return false;
            if (index == self.Count - 1) return false;
            self.RemoveAt(index);
            self.Insert(index + 1, item);
            return true;
        }

        /// <summary>
        /// Decreases the index of the specified item by one.
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="self">This list</param>
        /// <param name="item">the item of type T to decrease the index of.</param>
        public static bool DecreaseIndex<T>(this IList<T> self, T item)
        {
            int index = self.IndexOf(item);
            if (index == -1) return false;
            if (index == 0) return false;
            self.RemoveAt(index);
            self.Insert(index - 1, item);
            return true;
        }
    }
}