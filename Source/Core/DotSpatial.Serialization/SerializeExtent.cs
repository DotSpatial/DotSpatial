// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// Extent works like an envelope but is faster acting, has a minimum memory profile, only works in 2D and has no events.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SerializeExtent : ICloneable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeExtent"/> class.
        /// This introduces no error checking and assumes
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
        /// Initializes a new instance of the <see cref="SerializeExtent"/> class.
        /// </summary>
        /// <param name="xMin">The x min.</param>
        /// <param name="yMin">The y min.</param>
        /// <param name="xMax">The x max.</param>
        /// <param name="yMax">The y max.</param>
        public SerializeExtent(double xMin, double yMin, double xMax, double yMax)
        {
            XMin = xMin;
            YMin = yMin;
            XMax = xMax;
            YMax = yMax;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeExtent"/> class.
        /// </summary>
        /// <param name="values">Array that contains xmin, ymin, xmax and ymax.</param>
        /// <param name="offset">Offset of Xmin, the others follow directly after.</param>
        public SerializeExtent(double[] values, int offset)
        {
            XMin = values[0 + offset];
            YMin = values[1 + offset];
            XMax = values[2 + offset];
            YMax = values[3 + offset];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeExtent"/> class.
        /// </summary>
        /// <param name="values">Array that contains xmin, ymin, xmax and ymax.</param>
        public SerializeExtent(double[] values)
        {
            XMin = values[0];
            YMin = values[1];
            XMax = values[2];
            YMax = values[3];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Max X.
        /// </summary>
        [Serialize("XMax")]
        public double XMax { get; set; }

        /// <summary>
        /// Gets or sets Min X.
        /// </summary>
        [Serialize("XMin")]
        public double XMin { get; set; }

        /// <summary>
        /// Gets or sets Max Y.
        /// </summary>
        [Serialize("YMax")]
        public double YMax { get; set; }

        /// <summary>
        /// Gets or sets Min Y.
        /// </summary>
        [Serialize("YMin")]
        public double YMin { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Produces a clone, rather than using this same object.
        /// </summary>
        /// <returns>The clone.</returns>
        public object Clone()
        {
            return new SerializeExtent(XMin, YMin, XMax, YMax);
        }

        /// <summary>
        /// If this is undefined, it will have a min that is larger than the max.
        /// </summary>
        /// <returns>Boolean, true if the envelope has not had values set for it yet.</returns>
        public bool IsEmpty()
        {
            if (XMin > XMax || YMin > YMax) return true;

            return false;
        }

        #endregion
    }
}