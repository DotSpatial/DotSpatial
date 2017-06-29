using System;
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Facilitates the creation of a deep copy of an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<T> 
    {
        /// <summary>
        /// Creates a deep copy of the object.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}
