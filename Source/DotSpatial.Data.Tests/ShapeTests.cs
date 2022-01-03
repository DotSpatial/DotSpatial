// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for Shape.
    /// </summary>
    [TestFixture]
    internal class ShapeTests
    {
        #region Methods

        /// <summary>
        /// Checks whether features can be converted to shape if they have no datarow.
        /// </summary>
        [Test]
        public void CanConvertFeatureToShapeIfDataRowIsMissing()
        {
            var feature = new Feature(FeatureType.Polygon, new[] { new Coordinate(1375930, 6269230), new Coordinate(1376248, 6269230), new Coordinate(1376248, 6268860), new Coordinate(1375930, 6268860), new Coordinate(1375930, 6269230) });
            var shape = feature.ToShape();
            Assert.IsNotNull(shape);
        }

        /// <summary>
        /// Checks whether shapes can be created from coordinates.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        /// <param name="m">The m value.</param>
        [Test]
        [TestCase(1, 1, double.NaN, double.NaN)]
        [TestCase(1, 1, 1, double.NaN)]
        [TestCase(1, 1, 1, 1)]
        public void CanCreateShapeFromCoordinate(double x, double y, double z, double m)
        {
            var c = new CoordinateZM(x, y, z, m);
            var shape = new Shape(c);
            Assert.IsNotNull(shape);
        }

        #endregion
    }
}