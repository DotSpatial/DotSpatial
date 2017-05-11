// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2009 2:46:54 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// A Copy list is something that is specifically designed to allow internal items
    /// to be cloned.
    /// </summary>
    [Serializable]
    public class CopyList<T> : BaseList<T>, ICloneable where T : class
    {
        #region Methods

        /// <summary>
        /// Returns a duplicate of this entire list, where each item has been cloned
        /// if it implements ICloneable. Otherwise, the values will be a shallow copy.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            CopyList<T> result = MemberwiseClone() as CopyList<T>;
            OnCopy(result);
            return result;
        }

        /// <summary>
        /// This copies any individual members of the list. If the item can be
        /// cloned, then it copies the cloned item. Otherwise it copies the
        /// regular item. This method can be overridden to handle special behavior
        /// in sub-classes.
        /// </summary>
        /// <param name="copy"></param>
        protected virtual void OnCopy(CopyList<T> copy)
        {
            copy.InnerList = new List<T>();
            foreach (T item in InnerList)
            {
                ICloneable c = Global.SafeCastTo<ICloneable>(item);
                if (c != null)
                {
                    T value = Global.SafeCastTo<T>(c.Clone());
                    copy.Add(value);
                }
                else
                {
                    copy.Add(item);
                }
            }
        }

        #endregion
    }
}