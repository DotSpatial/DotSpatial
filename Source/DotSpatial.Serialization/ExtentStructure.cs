// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
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
using System.ComponentModel;
using DotSpatial.Serialization;

namespace DotSpatial
{
    /// <summary>
    /// Extent works like an envelope but is faster acting, has a minimum memory profile, only works in 2D and has no events.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SerializeExtent : ICloneable
    {
        #region Private Variables

        /// <summary>
        /// Max X
        /// </summary>
        [Serialize("XMax")]
        public double XMax;

        /// <summary>
        /// Min X
        /// </summary>
        [Serialize("XMin")]
        public double XMin;

        /// <summary>
        /// Max Y
        /// </summary>
        [Serialize("YMax")]
        public double YMax;

        /// <summary>
        /// Min Y
        /// </summary>
        [Serialize("YMin")]
        public double YMin;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Extent.  This introduces no error checking and assumes
        /// that the user knows what they are doing when working with this.
        /// </summary>
        public SerializeExtent()
        {
            XMin = double.MaxValue;
            XMax = double.MinValue;
            YMin = double.MaxValue;
            YMax = double.MinValue;
        }

        /// <summary>
        /// Creates a new extent from the specified ordinates
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMax"></param>
        public SerializeExtent(double xMin, double yMin, double xMax, double yMax)
        {
            XMin = xMin;
            YMin = yMin;
            XMax = xMax;
            YMax = yMax;
        }

        /// <summary>
        /// Given a long array of doubles, this builds an extent from a small part of that
        /// xmin, ymin, xmax, ymax
        /// </summary>
        /// <param name="values"></param>
        /// <param name="offset"></param>
        public SerializeExtent(double[] values, int offset)
        {
            XMin = values[0 + offset];
            YMin = values[1 + offset];
            XMax = values[2 + offset];
            YMax = values[3 + offset];
        }

        /// <summary>
        /// XMin, YMin, XMax, YMax order
        /// </summary>
        /// <param name="values"></param>
        public SerializeExtent(double[] values)
        {
            XMin = values[0];
            YMin = values[1];
            XMax = values[2];
            YMax = values[3];
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Produces a clone, rather than using this same object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new SerializeExtent(XMin, YMin, XMax, YMax);
        }

        #endregion

        /// <summary>
        /// If this is undefined, it will have a min that is larger than the max.
        /// </summary>
        /// <returns>Boolean, true if the envelope has not had values set for it yet.</returns>
        public bool IsEmpty()
        {
            if (XMin > XMax || YMin > YMax) return true;
            return false;
        }
    }
}