// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using NUnit.Framework;

namespace DotSpatial.Tests.Common
{
    /// <summary>
    /// Extension methods for assert.
    /// </summary>
    public static class AssertExt
    {
        /// <summary>
        /// Testing double precision equality is tricky. This uses a tollerance calculated
        /// by multiplying parameter a by 1E-15, and asserting that the difference between
        /// a and b is smaller than that value. This is good for verifying equality
        /// for up to about 15 places.
        /// </summary>
        /// <param name="a">One value. </param>
        /// <param name="b">The value to compare to.</param>
        public static void AreEqual15(double a, double b)
        {
            var tolerance = Math.Abs(a * 1E-14);
            var test = Math.Abs(a - b);
            if (test > tolerance)
            {
                Assert.Fail("Double values " + a + " and " + b + " were not equal.");
            }
        }
    }
}
