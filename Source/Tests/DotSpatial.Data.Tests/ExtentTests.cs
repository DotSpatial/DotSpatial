// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using DotSpatial.NTSExtension;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for the Extent class.
    /// </summary>
    [TestFixture]
    public class ExtentTests
    {
        /// <summary>
        /// This checks whether using the arrray based Extent constructor throws an IndexOutOfRangeException if the array contains less than 4 values.
        /// </summary>
        [Test(Description = "This checks whether using the arrray based Extent constructor throws an IndexOutOfRangeException if the array contains less than 4 values.")]
        public void ExtentToSmallArrayCtorTest()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<IndexOutOfRangeException>(() => { new Extent(new double[] { 9, 3, 4 }); });
        }

        /// <summary>
        /// This checks whether using the array and offset based Extent constructor throws an IndexOutOfRangeException if the array contains less values than offset + 3.
        /// </summary>
        [Test(Description = "This checks whether using the arrray and offset based Extent constructor throws an IndexOutOfRangeException if the array contains less values than offset + 3.")]
        public void ExtentArrayWrongOffsetCtorTest()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<IndexOutOfRangeException>(() => { new Extent(new double[] { 9, 3, 4, 5, 6 }, 2); });
        }

        /// <summary>
        /// This checks whether using the array and offset based Extent constructor assigns the correct values.
        /// </summary>
        [Test(Description = "This checks whether using the arrray and offset based Extent constructor assigns the correct values.")]
        public void ExtentArrayOffsetCtorTest()
        {
            double[] values = { 9, 3, 4, 5, 6, 7, 8 };

            Extent e = new(values, 3);

            Assert.AreEqual(5, e.MinX, "5 = e.MinX");
            Assert.AreEqual(6, e.MinY, "6 = e.MinY");
            Assert.AreEqual(7, e.MaxX, "7 = e.MaxX");
            Assert.AreEqual(8, e.MaxY, "8 = e.MaxY");
        }

        /// <summary>
        /// This checks whether using the Envelope based Extent constructor throws an ArgumentNullException if null is passed in.
        /// </summary>
        [Test(Description = "This checks whether using the Envelope based Extent constructor throws an ArgumentNullException if null is passed in.")]
        public void ExtentNullEnvelopeCtorTest()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new Extent(null as Envelope); });
        }

        /// <summary>
        /// This checks whether using the Envelope based Extent constructor assigns the correct values.
        /// </summary>
        [Test(Description = "This checks whether using the Envelope based Extent constructor assigns the correct values.")]
        public void ExtentEnvelopeCtorTest()
        {
            Envelope env = new(0, 3, 5, 6);

            Extent e = new(env);

            Assert.AreEqual(0, e.MinX, "0 = e.MinX");
            Assert.AreEqual(5, e.MinY, "5 = e.MinY");
            Assert.AreEqual(3, e.MaxX, "3 = e.MaxX");
            Assert.AreEqual(6, e.MaxY, "6 = e.MaxY");
        }

        /// <summary>
        /// This checks whether the properties return the correct values.
        /// </summary>
        [Test(Description = "This checks whether the properties return the correct values.")]
        public void ExtentGetPropertiesTest()
        {
            Extent e = new(0, 3, 10, 15);
            Coordinate center = e.Center;

            Assert.AreEqual(5, center.X, "5 != center.X");
            Assert.AreEqual(9, center.Y, "9 != center.Y");
            Assert.AreEqual(10, e.Width, "10 != e.Width");
            Assert.AreEqual(12, e.Height, "12 != e.Height");
            Assert.AreEqual(0, e.X, "0 != e.X");
            Assert.AreEqual(15, e.Y, "15 != e.Y");
            Assert.AreEqual(false, e.HasM, "false != e.HasM");
            Assert.AreEqual(false, e.HasZ, "false != e.HasZ");
        }

        /// <summary>
        /// This checks whether setting the properties works correctly.
        /// </summary>
        [Test(Description = "This checks whether setting the properties works correctly.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Setting X and Y is needed for testing.")]
        public void ExtentSetPropertiesTest()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            Extent e = new(0, 3, 10, 15);

            e.X = 5;
            Assert.AreEqual(5, e.MinX, "5 != e.MinX");
            Assert.AreEqual(15, e.MaxX, "15 != e.MinX");

            e.Y = 17;
            Assert.AreEqual(5, e.MinY, "5 != e.MinY");
            Assert.AreEqual(17, e.MaxY, "17 != e.MaxY");
        }

        /// <summary>
        /// This checks whether the extent clone is not the same object as the original extent but has the same values.
        /// </summary>
        [Test(Description = "This checks whether the extent clone is not the same object as the original extent but has the same values.")]
        public void ExtentCloneTest()
        {
            Extent e = new(3, 6, 8, 9);
            Assert.AreEqual(e, e.Clone(), "e is not equal to e.Clone()");
            Assert.AreNotSame(e, e.Clone(), "e is the same as e.Clone()");
        }

        /// <summary>
        /// This checks whether the copying from another extent results in both extents having the same values.
        /// </summary>
        [Test(Description = "This checks whether the copying from another extent results in both extents having the same values.")]
        public void ExtentCopyFromTest()
        {
            Extent e = new(3, 6, 8, 9);
            Extent e2 = new();
            e2.CopyFrom(e);

            Assert.AreEqual(e, e2);
        }

        /// <summary>
        /// This checks whether copying from a null extent results in an ArgumentNullException.
        /// </summary>
        [Test(Description = "This checks whether copying from a null extent results in an ArgumentNullException.")]
        public void ExtentCopyFromNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => { new Extent().CopyFrom(null); });
        }

        /// <summary>
        /// This checks whether equals and the equality operator return the expected results.
        /// </summary>
        [Test(Description = "This checks whether equals and the equality operator return the expected results.")]
        public void ExtentEqualsTest()
        {
            Extent e = new(4, 6, 9, 10);

            // ReSharper disable EqualExpressionComparison
            Assert.IsTrue(e.Equals(e), "e.Equals(e)");
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(e == e, "e == e");
#pragma warning restore CS1718 // Comparison made to same variable

            // ReSharper restore EqualExpressionComparison
            object e2 = new Coordinate(10, 23);
            Assert.IsFalse(e.Equals(e2), "e.Equals(e2)");

            Extent e3 = new(4, 6, 9, 10);
            Assert.IsTrue(e.Equals(e3), "e.Equals(e3)");
            Assert.IsTrue(e == e3, "e == e3");

            Extent e4 = new(4, 7, 9, 10);
            Assert.IsFalse(e.Equals(e4), "e.Equals(e4)");
            Assert.IsFalse(e == e4, "e == e4");

            Extent e5 = null;
            Assert.IsFalse(e5 == e4, "e5 == e4");
            Assert.IsTrue(e5 == null, "e5 == null");
        }

        /// <summary>
        /// This checks whether the inequality operator returns the expected results.
        /// </summary>
        [Test(Description = "This checks whether the inequality operator returns the expected results.")]
        public void ExtentInequalityTest()
        {
            Extent e = new(4, 6, 9, 10);

            // ReSharper disable EqualExpressionComparison
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsFalse(e != e, "e != e");
#pragma warning restore CS1718 // Comparison made to same variable

            // ReSharper restore EqualExpressionComparison
            Extent e3 = new(4, 6, 9, 10);
            Assert.IsFalse(e != e3, "e != e3");

            Extent e4 = new(4, 7, 9, 10);
            Assert.IsTrue(e != e4, "e != e4");

            Extent e5 = null;
            Assert.IsTrue(e5 != e4, "e5 != e4");
            Assert.IsFalse(e5 != null, "e5 != null");
        }

        /// <summary>
        /// This checks whether Extent.Parse works as expected.
        /// </summary>
        [Test(Description = "This checks whether Extent.Parse works as expected.")]
        public void ExtentParseTest()
        {
            Extent e = Extent.Parse("X[25|67], Y[34|89], M[3|4], Z[2|7]");
            Assert.AreEqual(new ExtentMz(25, 34, 3, 2, 67, 89, 4, 7), e, "Error while parsing ExtentMz.");

            e = Extent.Parse("X[25|67], Y[34|89], M[3|4]");
            Assert.AreEqual(new ExtentM(25, 34, 3, 67, 89, 4), e, "Error while parsing ExtentM.");

            e = Extent.Parse("X[25|67], Y[34|89]");
            Assert.AreEqual(new Extent(25, 34, 67, 89), e, "Error while parsing Extent.");

            Assert.Throws<ExtentParseException>(() => { Extent.Parse("X[25|67], Y[34|x]"); }, "Should throw ExtentParseException on letter instead of value.");
            Assert.Throws<ExtentParseException>(() => { Extent.Parse("X[25|67], Y[34|]"); }, "Should throw ExtentParseException on empty field instead of value.");
            Assert.Throws<ExtentParseException>(() => { Extent.Parse("X[25|67], Y[3,4]"); }, "Should throw ExtentParseException on , instead of |.");
        }

        /// <summary>
        /// This checks whether Extent.TryParse works as expected.
        /// </summary>
        [Test(Description = "This checks whether Extent.TryParse works as expected.")]
        public void ExtentTryParseTest()
        {
            Assert.IsTrue(Extent.TryParse("X[25|67], Y[34|89], M[3|4], Z[2|7]", out Extent e, out _), "Extent.TryParse('X[25|67], Y[34|89], M[3|4], Z[2|7]', out e, out error)");
            Assert.AreEqual(new ExtentMz(25, 34, 3, 2, 67, 89, 4, 7), e, "Error while trying to parse ExtentMz.");

            Assert.IsTrue(Extent.TryParse("X[25|67], Y[34|89], M[3|4]", out e, out _), "Extent.TryParse('X[25|67], Y[34|89], M[3|4]',out e, out error)");
            Assert.AreEqual(new ExtentM(25, 34, 3, 67, 89, 4), e, "Error while trying to parse ExtentM.");

            Assert.IsTrue(Extent.TryParse("X[25|67], Y[34|89]", out e, out _), "Extent.TryParse('X[25|67], Y[34|89]', out e, out error)");
            Assert.AreEqual(new Extent(25, 34, 67, 89), e, "Error while trying to parse Extent.");

            Assert.IsFalse(Extent.TryParse("X[25|67], Y[34|x]", out _, out string error), "Extent.TryParse('X[25|67], Y[34|x]', out e, out error)");
            Assert.AreEqual(error, "Y", "TryParse letter instead of value");

            Assert.IsFalse(Extent.TryParse("X[|25], Y[34|23]", out _, out error), "Extent.TryParse('X[25|], Y[34|23]', out e, out error)");
            Assert.AreEqual(error, "X", "TryParse empty field instead of value.");

            Assert.IsFalse(Extent.TryParse("X[25|67], Y[3|4], M[2,3]", out _, out error), "Extent.TryParse('X[25|67], Y[3|4], M[2,3]', out e, out error)");
            Assert.AreEqual(error, "M", "TryParse , instead of |");

            Assert.IsFalse(Extent.TryParse("X[25|67], Y[3|4], M[2|3], Z[]", out _, out error), "Extent.TryParse('X[25|67], Y[3|4], M[2|3], Z[]', out e, out error)");
            Assert.AreEqual(error, "Z", "TryParse no values");

            Assert.IsFalse(Extent.TryParse("X[25|67], Y[3|4], M[2.3], Z[3|4]", out _, out error), "Extent.TryParse('X[25|67], Y[3|4], M[2|3], Z[]', out e, out error)");
            Assert.AreEqual(error, "M", ". instead of |");
        }

        /// <summary>
        /// This checks whether setting a null value as Center throws an ArgumentNullException.
        /// </summary>
        [Test(Description = "This checks whether setting a null value as Center throws an ArgumentNullException.")]
        public void ExtentSetCenterNullCoordinateTest()
        {
            Extent e = new();

            Assert.Throws<ArgumentNullException>(() => { e.SetCenter(null); }, "ExtentSetCenter throws on null Coordinate");
            Assert.Throws<ArgumentNullException>(() => { e.SetCenter(null, 3, 4); }, "ExtentSetCenter with height and width throws on null Coordinate");
        }

        /// <summary>
        /// This checks whether setting the Center works as expected.
        /// </summary>
        [Test(Description = "This checks whether setting the Center works as expected.")]
        public void ExtentSetCenterTest()
        {
            Extent e = new(34, 10, 38, 16);

            e.SetCenter(new Coordinate(4, 5));
            Assert.AreEqual(new Extent(2, 2, 6, 8), e, "ExtentSetCenter Coordinate only");

            e.SetCenter(new Coordinate(14, 15), 6, 4);
            Assert.AreEqual(new Extent(11, 13, 17, 17), e, "ExtentSetCenter Coordinate, width and height");

            e.SetCenter(27, 23, 8, 10);
            Assert.AreEqual(new Extent(23, 18, 31, 28), e, "ExtentSetCenter X, Y, width and height");
        }

        /// <summary>
        /// This checks whether expanding the extent by one value results in the correct x,y coordinates.
        /// </summary>
        [Test(Description = "This checks whether expanding the extent by one value results in the correct x,y coordinates.")]
        public void ExtentExpandByOneValueTest()
        {
            Extent e = new(4, 6, 9, 10);
            e.ExpandBy(4);

            Assert.AreEqual(0, e.MinX, "0 != e.MinX");
            Assert.AreEqual(13, e.MaxX, "13 != e.MaxX");
            Assert.AreEqual(2, e.MinY, "2 != e.MinY");
            Assert.AreEqual(14, e.MaxY, "14 != e.MaxY");
        }

        /// <summary>
        /// This checks whether expanding the extent by two values results in the correct x,y coordinates.
        /// </summary>
        [Test(Description = "This checks whether expanding the extent by two values results in the correct x,y coordinates.")]
        public void ExtentExpandByTwoValuesTest()
        {
            Extent e = new(4, 6, 9, 10);
            e.ExpandBy(4, 5);

            Assert.AreEqual(0, e.MinX, "0 != e.MinX");
            Assert.AreEqual(13, e.MaxX, "13 != e.MinY");
            Assert.AreEqual(1, e.MinY, "1 != e.MaxX");
            Assert.AreEqual(15, e.MaxY, "15 != e.MaxY");
        }

        /// <summary>
        /// This checks whether expanding the extent by an extent works as expected.
        /// </summary>
        [Test(Description = "This checks whether expanding the extent by an extent works as expected.")]
        public void ExtentExpandToIncludeExtentTest()
        {
            Extent e = new();

            e.ExpandToInclude(new Extent(4, 5, 6, 7));
            Assert.AreEqual(4, e.MinX, "4 != e.MinX");
            Assert.AreEqual(5, e.MinY, "5 != e.MinY");
            Assert.AreEqual(6, e.MaxX, "6 != e.MaxX");
            Assert.AreEqual(7, e.MaxY, "7 != e.MaxY");

            // make sure only MinX and MaxY change
            e.ExpandToInclude(new Extent(1, 6, 5, 8));
            Assert.AreEqual(1, e.MinX, "1 != e.MinX");
            Assert.AreEqual(5, e.MinY, "5 != e.MinY");
            Assert.AreEqual(6, e.MaxX, "6 != e.MaxX");
            Assert.AreEqual(8, e.MaxY, "8 != e.MaxY");

            // make sure only MinY and MaxX change
            e.ExpandToInclude(new Extent(2, 3, 7, 7));
            Assert.AreEqual(1, e.MinX, "1 != e.MinX");
            Assert.AreEqual(3, e.MinY, "3 != e.MinY");
            Assert.AreEqual(7, e.MaxX, "7 != e.MaxX");
            Assert.AreEqual(8, e.MaxY, "8 != e.MaxY");

            // make sure nothing changes
            e.ExpandToInclude(null);
            Assert.AreEqual(1, e.MinX, "1 != e.MinX");
            Assert.AreEqual(3, e.MinY, "3 != e.MinY");
            Assert.AreEqual(7, e.MaxX, "7 != e.MaxX");
            Assert.AreEqual(8, e.MaxY, "8 != e.MaxY");
        }

        /// <summary>
        /// This checks whether expanding the extent by two values works as expected.
        /// </summary>
        [Test(Description = "This checks whether expanding the extent by two values works as expected.")]
        public void ExtentExpandToIncludeTwoValuesTest()
        {
            Extent e = new();

            // make sure all x and y values are no longer NaN
            e.ExpandToInclude(5, 7);
            Assert.AreEqual(5, e.MinX, "6 != e.MinX");
            Assert.AreEqual(7, e.MinY, "7 != e.MinY");
            Assert.AreEqual(5, e.MaxX, "5 != e.MaxX");
            Assert.AreEqual(7, e.MaxY, "7 != e.MaxY");

            // make sure only MinX and MinY change
            e.ExpandToInclude(4, 6);
            Assert.AreEqual(4, e.MinX, "4 != e.MinX");
            Assert.AreEqual(6, e.MinY, "6 != e.MinY");
            Assert.AreEqual(5, e.MaxX, "5 != e.MaxX");
            Assert.AreEqual(7, e.MaxY, "7 != e.MaxY");

            // make sure only MaxX and MaxY change
            e.ExpandToInclude(13, 14);
            Assert.AreEqual(4, e.MinX, "4 != e.MinX");
            Assert.AreEqual(6, e.MinY, "6 != e.MinY");
            Assert.AreEqual(13, e.MaxX, "13 != e.MaxX");
            Assert.AreEqual(14, e.MaxY, "14 != e.MaxY");
        }

        /// <summary>
        /// This checks whether the extent has the expected hash code.
        /// </summary>
        [Test(Description = "This checks whether the extent has the expected hash code.")]
        public void ExtentGetHashCodeTest()
        {
            Extent e = new(3, 5, 7, 9);
            Assert.AreEqual(2136750625, e.GetHashCode());
        }

        /// <summary>
        /// This checks whether the contained extent is within the container extent. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing extent, the contained extent and the expected return value.</param>
        [Test(Description = "This checks whether the contained extent is within the container extent. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetTestValues))]
        public void ExtentWithinExtentTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            Extent contained = new(data.ContainedValues);

            Assert.AreEqual(data.ExpectedReturnValue, contained.Within(container), "ExtentWithinExtent");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedEnv = contained.ToEnvelope().ToPolygon();

            Assert.AreEqual(data.ExpectedReturnValue, containedEnv.Within(containerEnv), "ExtentGeometryWithinExtentGeometry");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent is contained within a null extent.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent is contained within a null extent.")]
        public void ExtentWithinNullExtentTest()
        {
            Extent container = null;
            Extent contained = new(0, 0, 10, 20);

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { contained.Within(container); });
        }

        /// <summary>
        /// This checks whether the contained extent is within the container envelope. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing envelope, the contained extent and the expected return value.</param>
        [Test(Description = "This checks whether the contained extent is within the container envelope. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetTestValues))]
        public void ExtentWithinEnvelopeTest(ExtentTestData data)
        {
            var vals = data.ContainerValues;
            Envelope container = new(vals[0], vals[2], vals[1], vals[3]);
            Extent contained = new(data.ContainedValues);

            Assert.AreEqual(data.ExpectedReturnValue, contained.Within(container), "ExtentWithinEnvelope");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToPolygon();
            var containedEnv = contained.ToEnvelope().ToPolygon();

            Assert.AreEqual(data.ExpectedReturnValue, containedEnv.Within(containerEnv), "ExtentGeometryWithinEnvelopeGeometry");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent is contained within a null envelope.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent is contained within a null envelope.")]
        public void ExtentWithinNullEnvelopeTest()
        {
            Envelope container = null;
            Extent contained = new(0, 0, 10, 20);

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { contained.Within(container); });
        }

        /// <summary>
        /// This checks whether the container extent contains the contained envelope. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing envelope, the contained extent and the expected return value.</param>
        [Test(Description = "This checks whether the container extent contains the contained envelope. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetTestValues))]
        public void ExtentContainsEnvelopeTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;
            Envelope contained = new(vals[0], vals[2], vals[1], vals[3]);

            Assert.AreEqual(data.ExpectedReturnValue, container.Contains(contained), "ExtentContainsEnvelope");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedEnv = contained.ToPolygon();

            Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Contains(containedEnv), "ExtentGeometryContainsEnvelopeGeometry");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent contains a null envelope.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent contains a null envelope.")]
        public void ExtentContainsNullEnvelopeTest()
        {
            Extent container = new(0, 0, 10, 20);
            Envelope contained = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { container.Contains(contained); });
        }

        /// <summary>
        /// This checks whether the container extent contains the contained extent. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing extent, the contained extent and the expected return value.</param>
        [Test(Description = "This checks whether the container extent contains the contained extent. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetTestValues))]
        public void ExtentContainsExtentTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;
            Extent contained = new(vals[0], vals[1], vals[2], vals[3]);

            Assert.AreEqual(data.ExpectedReturnValue, container.Contains(contained), "ExtentContainsExtent");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedEnv = contained.ToEnvelope().ToPolygon();

            Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Contains(containedEnv), "ExtentGeometryContainsExtentGeometry");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent contains a null extent.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent contains a null extent.")]
        public void ExtentContainsNullExtentTest()
        {
            Extent container = new(0, 0, 10, 20);
            Extent contained = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { container.Contains(contained); });
        }

        /// <summary>
        /// This checks whether the container extent contains the contained coordinate. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing extent, the contained coordinate and the expected return value.</param>
        [Test(Description = "This checks whether the container extent contains the contained coordinate. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetPointIntersectTestValues))]
        public void ExtentContainsCoordinateTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;
            Coordinate contained = new(vals[0], vals[1]);

            Assert.AreEqual(data.ExpectedReturnValue, container.Contains(contained));

            //// this is to make sure that the extent methods work the same way the IGeometry methods do
            // var containerEnv = container.ToEnvelope().ToPolygon();
            // var containedPnt = new Point(contained);
            // Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Contains(containedPnt));
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent contains a null coordinate.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent contains a null coordinate.")]
        public void ExtentContainsNullCoordinateTest()
        {
            Extent container = new(0, 0, 10, 20);
            Coordinate c = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { container.Contains(c); });
        }

        /// <summary>
        /// This checks whether the container extent contains the contained extent. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing extent, the contained extent and the expected return value.</param>
        [Test(Description = "This checks whether the container extent contains the contained extent. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetIntersectTestValues))]
        public void ExtentIntersectsExtentTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;
            Extent contained = new(vals[0], vals[1], vals[2], vals[3]);

            Assert.AreEqual(data.ExpectedReturnValue, container.Intersects(contained), "ExtentIntersectsExtent");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedEnv = contained.ToEnvelope().ToPolygon();

            Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Intersects(containedEnv), "ExtentGeometryIntersectsExtentGeometry");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent intersects a null extent.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent intersects a null extent.")]
        public void ExtentIntersectsNullExtentTest()
        {
            Extent container = new(0, 0, 10, 20);
            Extent contained = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { container.Intersects(contained); });
        }

        /// <summary>
        /// This checks whether the container extent contains the contained envelope. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing envelope, the contained extent and the expected return value.</param>
        [Test(Description = "This checks whether the container extent contains the contained envelope. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetIntersectTestValues))]
        public void ExtentIntersectsEnvelopeTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;
            Envelope contained = new(vals[0], vals[2], vals[1], vals[3]);

            Assert.AreEqual(data.ExpectedReturnValue, container.Intersects(contained), "ExtentIntersectsEnvelope");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedEnv = contained.ToPolygon();

            Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Intersects(containedEnv), "ExtentGeometryIntersectsEnvelopeGeometry");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent intersects a null envelope.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent intersects a null envelope.")]
        public void ExtentIntersectsNullEnvelopeTest()
        {
            Extent container = new(0, 0, 10, 20);
            Envelope contained = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { container.Intersects(contained); });
        }

        /// <summary>
        /// This checks whether the container extent intersects the contained coordinate. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing extent, the contained coordinate and the expected return value.</param>
        [Test(Description = "This checks whether the container extent intersects the contained coordinate. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetPointIntersectTestValues))]
        public void ExtentIntersectsCoordinateTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;
            Coordinate contained = new(vals[0], vals[1]);

            Assert.AreEqual(data.ExpectedReturnValue, container.Intersects(contained), "ExtentIntersectsCoordinate");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedPnt = new Point(contained);

            Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Intersects(containedPnt), "ExtentGeometryIntersectsCoordinateGeometry");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to check whether an Extent intersects a null coordinate.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to check whether an Extent intersects a null coordinate.")]
        public void ExtentIntersectsNullCoordinateTest()
        {
            Extent container = new(0, 0, 10, 20);
            Coordinate c = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => { container.Intersects(c); });
        }

        /// <summary>
        /// This checks whether the container extent intersects the contained point values. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing extent, the contained coordinate and the expected return value.</param>
        [Test(Description = "This checks whether the container extent intersects the contained point values. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetPointIntersectTestValues))]
        public void ExtentIntersectsPointValuesTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;

            Assert.AreEqual(data.ExpectedReturnValue, container.Intersects(vals[0], vals[1]), "ExtentIntersectsPointValues");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedPnt = new Point(vals[0], vals[1]);

            Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Intersects(containedPnt), "ExtentGeometryIntersectsPointValuesGeometry");
        }

        /// <summary>
        /// This checks whether intersecting an Extent with NaN values returns false.
        /// </summary>
        [Test(Description = "This checks whether intersecting an Extent with NaN values returns false.")]
        public void ExtentIntersectsNullPointValuesTest()
        {
            Extent container = new(0, 0, 10, 20);

            Assert.IsFalse(container.Intersects(double.NaN, 0));
            Assert.IsFalse(container.Intersects(0, double.NaN));
        }

        /// <summary>
        /// This checks whether the container extent intersects the contained vertex. This means no points may be outside of the border of the container.
        /// </summary>
        /// <param name="data">The struct that contains the coordinates of the containing extent, the contained vertex and the expected return value.</param>
        [Test(Description = "This checks whether the container extent intersects the contained vertex. This means no points may be outside of the border of the container.")]
        [TestCaseSource(nameof(GetPointIntersectTestValues))]
        public void ExtentIntersectsVertexTest(ExtentTestData data)
        {
            Extent container = new(data.ContainerValues);
            var vals = data.ContainedValues;
            Vertex contained = new(vals[0], vals[1]);

            Assert.AreEqual(data.ExpectedReturnValue, container.Intersects(contained), "ExtentIntersectsVertex");

            // this is to make sure that the extent methods work the same way the IGeometry methods do
            var containerEnv = container.ToEnvelope().ToPolygon();
            var containedPnt = new Point(contained.X, contained.Y);

            Assert.AreEqual(data.ExpectedReturnValue, containerEnv.Intersects(containedPnt), "ExtentGeometryIntersectsVertexGeometry");
        }

        /// <summary>
        /// This checks whether the extent with the given values is empty.
        /// </summary>
        /// <param name="xmin">The xmin value.</param>
        /// <param name="xmax">The xmax value.</param>
        /// <param name="ymin">The ymin value.</param>
        /// <param name="ymax">The ymax value.</param>
        /// <param name="expectedResult">The expected result.</param>
        [Test(Description = "This checks whether the extent with the given values is empty.")]
        [TestCase(0, 10, 0, 5, false)]
        [TestCase(double.NaN, 10, 0, 5, true)]
        [TestCase(0, double.NaN, 0, 5, true)]
        [TestCase(0, 10, double.NaN, 5, true)]
        [TestCase(0, 10, 0, double.NaN, true)]
        [TestCase(double.NaN, double.NaN, double.NaN, double.NaN, true)]
        [TestCase(11, 10, 6, 5, true)]
        public void ExtentIsEmptyTest(double xmin, double xmax, double ymin, double ymax, bool expectedResult)
        {
            var extent = new Extent(xmin, ymin, xmax, ymax);
            Assert.AreEqual(expectedResult, extent.IsEmpty());
        }

        /// <summary>
        /// This checks whether the correct extent intersection is returned.
        /// </summary>
        /// <param name="xmin1">The xmin of the first extent.</param>
        /// <param name="ymin1">The ymin of the first extent.</param>
        /// <param name="xmax1">The xmax of the first extent.</param>
        /// <param name="ymax1">The ymax of the first extent.</param>
        /// <param name="xmin2">The xmin of the second extent.</param>
        /// <param name="ymin2">The ymin of the second extent.</param>
        /// <param name="xmax2">The xmax of the second extent.</param>
        /// <param name="ymax2">The ymax of the second extent.</param>
        /// <param name="xmin3">The xmin of the expected result extent.</param>
        /// <param name="ymin3">The ymin of the expected result extent.</param>
        /// <param name="xmax3">The xmax of the expected result extent.</param>
        /// <param name="ymax3">The ymax of the expected result extent.</param>
        /// <param name="empty">Indicates whether IsEmpty should be true.</param>
        [Test(Description = "This checks whether the correct extent intersection is returned.")]
        [TestCase(0, 0, 10, 10, 5, 5, 15, 15, 5, 5, 10, 10, false)]
        [TestCase(0, 0, 10, 10, 15, 15, 25, 25, 15, 15, 10, 10, true)]
        public void ExtentIntersectionTest(double xmin1, double ymin1, double xmax1, double ymax1, double xmin2, double ymin2, double xmax2, double ymax2, double xmin3, double ymin3, double xmax3, double ymax3, bool empty)
        {
            Extent e1 = new(xmin1, ymin1, xmax1, ymax1);
            Extent e2 = new(xmin2, ymin2, xmax2, ymax2);
            Extent res = new(xmin3, ymin3, xmax3, ymax3);

            Extent intersection = e1.Intersection(e2);

            Assert.AreEqual(res, intersection, "ExtentIntersection");
            Assert.AreEqual(empty, intersection.IsEmpty(), "ExtentIntersection.IsEmpty");
        }

        /// <summary>
        /// This checks whether an ArgumentNullException is thrown if we try to get an Intersection from a null extent.
        /// </summary>
        [Test(Description = "This checks whether an ArgumentNullException is thrown if we try to get an Intersection from a null extent.")]
        public void ExtentIntersectionNullExtentTest()
        {
            Extent e1 = new(0, 0, 10, 10);
            Assert.Throws<ArgumentNullException>(() => { e1.Intersection(null); });
        }

        /// <summary>
        /// This checks whether converting an empty extent to an envelope results in an empty envelope.
        /// </summary>
        [Test(Description = "This checks whether converting an empty extent to an envelope results in an empty envelope.")]
        public void NullExtentToNullEnvelopeTest()
        {
            Extent e = new();
            Assert.IsTrue(e.ToEnvelope().IsNull);
        }

        /// <summary>
        /// This checks whether converting an extent to string results in the correct string.
        /// </summary>
        [Test(Description = "This checks whether converting an extent to string results in the correct string.")]
        public void ExtentToStringTest()
        {
            Extent e = new(0, 3, 12, 23);
            Assert.AreEqual("X[0|12], Y[3|23]", e.ToString());
        }

        private static List<ExtentTestData> GetPointIntersectTestValues()
        {
            var list = new List<ExtentTestData>
                       {
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 0 }, true), // on corner
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 10, 0 }, true), // on corner
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 10, 20 }, true), // on corner
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 20 }, true), // on corner
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1 }, true), // inside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 1 }, true), // on border
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { -1, 5 }, false), // completely left of first
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 11, 5 }, false), // completely right of first
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 5, 21 }, false), // completely above of first
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 5, -1 }, false) // completely below of first
                       };

            return list;
        }

        private static List<ExtentTestData> GetIntersectTestValues()
        {
            var list = new List<ExtentTestData>
                       {
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 0, 10, 20 }, true), // equal
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 9, 19 }, true), // first contains second
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { -1, -1, 11, 21 }, true), // second contains first
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 1, 9, 19 }, true), // one point on border, rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 0, 9, 19 }, true), // one point on border, rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 10, 19 }, true), // one point on border, rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 9, 20 }, true), // one point on border, rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { -1, 1, 9, 19 }, true), // one point outside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, -1, 9, 19 }, true), // one point outside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 11, 19 }, true), // one point outside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 9, 21 }, true), // one point outside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { -10, 0, -1, 20 }, false), // second complete left of first
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 11, 0, 19, 20 }, false), // second complete right of first
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 21, 10, 29 }, false), // second complete above of first
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, -10, 10, -1 }, false) // second complete below of first
                       };

            return list;
        }

        private static List<ExtentTestData> GetTestValues()
        {
            var list = new List<ExtentTestData>
                       {
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 0, 10, 20 }, true), // equal
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 9, 19 }, true), // completely inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 0, 1, 9, 19 }, true), // one point on border rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 0, 9, 19 }, true), // one point on border rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 10, 19 }, true), // one point on border rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 9, 20 }, true), // one point on border rest complete inside interior
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { -1, 1, 9, 19 }, false), // one point outside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, -1, 9, 19 }, false), // one point outside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 11, 19 }, false), // one point outside
                           new ExtentTestData(new double[] { 0, 0, 10, 20 }, new double[] { 1, 1, 9, 21 }, false) // one point outside
                       };

            return list;
        }

        /// <summary>
        /// This structure is used to group the data needed for extent tests.
        /// </summary>
        public struct ExtentTestData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ExtentTestData"/> struct.
            /// </summary>
            /// <param name="containerValues">The values of the container object.</param>
            /// <param name="containedValues">The values of the contained object.</param>
            /// <param name="expectedReturnResult">The value the method that gets run should return.</param>
            public ExtentTestData(double[] containerValues, double[] containedValues, bool expectedReturnResult)
            {
                ContainerValues = containerValues;
                ContainedValues = containedValues;
                ExpectedReturnValue = expectedReturnResult;
            }

            /// <summary>
            /// Gets the values of the container object.
            /// </summary>
            public double[] ContainerValues { get; }

            /// <summary>
            /// Gets the values of the contained object.
            /// </summary>
            public double[] ContainedValues { get; }

            /// <summary>
            /// Gets a value indicating whether the method that gets run should return true.
            /// </summary>
            public bool ExpectedReturnValue { get; }
        }
    }
}
