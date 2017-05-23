// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for Size2D.
    /// </summary>
    [TestFixture]
    internal class Size2DTests
    {
        #region Properties

        /// <summary>
        /// Gets the test cases for the Size2D == checks. This is also used for the Size2D != checks.
        /// </summary>
        public IEnumerable<TestCaseData> EqualityOperatorWorksTestCases
        {
            get
            {
                yield return new TestCaseData(null, null, true);
                yield return new TestCaseData(null, new Size2D(0, 0), false);
                yield return new TestCaseData(new Size2D(0, 0), null, false);
                yield return new TestCaseData(new Size2D(0, 0), new Size2D(0, 0), true);
                yield return new TestCaseData(new Size2D(0, 0), new Size2D(4, 5), false);
            }
        }

        /// <summary>
        /// Gets the test cases for the Size2D.Equals(object) checks.
        /// </summary>
        public IEnumerable<TestCaseData> EqualsObjectWorksTestCases
        {
            get
            {
                yield return new TestCaseData(new Size2D(0, 0), null, false);
                yield return new TestCaseData(new Size2D(0, 0), new Size(8, 5), false);
                yield return new TestCaseData(new Size2D(0, 0), new Size2D(0, 0), true);
                yield return new TestCaseData(new Size2D(0, 0), new Size2D(4, 5), false);
            }
        }

        /// <summary>
        /// Gets the test cases for the Size2D.Equals(Size2D) checks.
        /// </summary>
        public IEnumerable<TestCaseData> EqualsSize2DWorksTestCases
        {
            get
            {
                yield return new TestCaseData(new Size2D(0, 0), null, false);
                yield return new TestCaseData(new Size2D(0, 0), new Size2D(0, 0), true);
                yield return new TestCaseData(new Size2D(0, 0), new Size2D(4, 5), false);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether Size2Ds == operator works correctly.
        /// </summary>
        /// <param name="size1">The first Size2D that gets compared.</param>
        /// <param name="size2">The second Size2D that gets compared.</param>
        /// <param name="expectedResult">The result that should be returned from the operator.</param>
        [Test(Description = "Checks whether Size2Ds == operator works correctly.")]
        [TestCaseSource(nameof(EqualityOperatorWorksTestCases))]
        public void EqualityOperatorWorks(Size2D size1, Size2D size2, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, size1 == size2);
        }

        /// <summary>
        /// Checks whether Size2Ds != operator works correctly.
        /// </summary>
        /// <param name="size1">The first Size2D that gets compared.</param>
        /// <param name="size2">The second Size2D that gets compared.</param>
        /// <param name="unExpectedResult">The result that should not be returned from the operator.</param>
        [Test(Description = "Checks whether Size2Ds != operator works correctly.")]
        [TestCaseSource(nameof(EqualityOperatorWorksTestCases))]
        public void UnEqualityOperatorWorks(Size2D size1, Size2D size2, bool unExpectedResult)
        {
            Assert.AreNotEqual(unExpectedResult, size1 != size2); // unequality can use the same cases as equality, but should return not equal
        }

        /// <summary>
        /// Checks whether Size2D.Equals(object) works correctly.
        /// </summary>
        /// <param name="size1">The Size2D that gets compared.</param>
        /// <param name="size2">The object that gets compared.</param>
        /// <param name="expectedResult">The result that should be returned from the operator.</param>
        [Test(Description = "Checks whether Size2D.Equals(object) works correctly.")]
        [TestCaseSource(nameof(EqualsObjectWorksTestCases))]
        public void EqualsObjectWorks(Size2D size1, object size2, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, size1.Equals(size2));
        }

        /// <summary>
        /// Checks whether Size2D.Equals(Size2D) works correctly.
        /// </summary>
        /// <param name="size1">The first Size2D that gets compared.</param>
        /// <param name="size2">The second Size2D that gets compared.</param>
        /// <param name="expectedResult">The result that should be returned from the operator.</param>
        [Test(Description = "Checks whether Size2D.Equals(Size2D) works correctly.")]
        [TestCaseSource(nameof(EqualsSize2DWorksTestCases))]
        public void EqualsSize2DWorks(Size2D size1, Size2D size2, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, size1.Equals(size2));
        }

        /// <summary>
        /// Checks whether creating an empty Size2D results in Height and Width being 0.
        /// </summary>
        [Test(Description = "Checks whether creating an empty Size2D results in Height and Widht being 0.")]
        public void CreateEmptySize2D()
        {
            Size2D size = new Size2D();
            Assert.AreEqual(0, size.Height);
            Assert.AreEqual(0, size.Width);
        }

        /// <summary>
        /// Checks whether creating a Size2D results in Height and Width being the given numbers.
        /// </summary>
        [Test(Description = "Checks whether creating a Size2D results in Height and Width being the given numbers.")]
        public void CreateSize2D()
        {
            Size2D size = new Size2D(4d, 5d);
            Assert.AreEqual(4, size.Width);
            Assert.AreEqual(5, size.Height);
        }

        /// <summary>
        /// Checks whether the property that was set returns the same value that was set.
        /// </summary>
        /// <param name="height">Indicates whether the height or the width property gets set.</param>
        [Test(Description = "Checks whether the property that was set returns the same value that was set.")]
        [TestCase(true)]
        [TestCase(false)]
        public void SettingPropertyWorks(bool height)
        {
            Size2D s = new Size2D();
            if (height)
            {
                s.Height = 5;
                Assert.AreEqual(5, s.Height);
            }
            else
            {
                s.Width = 5;
                Assert.AreEqual(5, s.Width);
            }
        }

        /// <summary>
        /// Checks whether Size2D.ToString returns the expected result.
        /// </summary>
        [Test(Description = "Checks whether Size2D.ToString returns the expected result.")]
        public void ToStringWorks()
        {
            Size2D size = new Size2D(4, 5);
            Assert.AreEqual("4, 5", size.ToString());
        }

        #endregion
    }
}
