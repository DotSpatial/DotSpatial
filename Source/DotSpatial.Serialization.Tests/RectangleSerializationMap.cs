// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Reflection;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// A serialization mapa for rectangles.
    /// </summary>
    public class RectangleSerializationMap : SerializationMap
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleSerializationMap"/> class.
        /// </summary>
        public RectangleSerializationMap()
            : base(typeof(Rectangle))
        {
            Type t = typeof(Rectangle);

            var x = t.GetField("x", BindingFlags.Instance | BindingFlags.NonPublic);
            var y = t.GetField("y", BindingFlags.Instance | BindingFlags.NonPublic);
            var width = t.GetField("width", BindingFlags.Instance | BindingFlags.NonPublic);
            var height = t.GetField("height", BindingFlags.Instance | BindingFlags.NonPublic);

            Serialize(x, "X").AsConstructorArgument(0);
            Serialize(y, "Y").AsConstructorArgument(1);
            Serialize(width, "Width").AsConstructorArgument(2);
            Serialize(height, "Height").AsConstructorArgument(3);
        }

        #endregion
    }
}