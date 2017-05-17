// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Reflection;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// A point serialization map.
    /// </summary>
    public class PointSerializationMap : SerializationMap
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSerializationMap"/> class.
        /// </summary>
        public PointSerializationMap()
            : base(typeof(Point))
        {
            var t = typeof(Point);

            var x = t.GetField("x", BindingFlags.Instance | BindingFlags.NonPublic);
            var y = t.GetField("y", BindingFlags.Instance | BindingFlags.NonPublic);

            Serialize(x, "X").AsConstructorArgument(0);
            Serialize(y, "Y").AsConstructorArgument(1);
        }

        #endregion
    }
}