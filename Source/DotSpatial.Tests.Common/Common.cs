// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NUnit.Framework;

namespace DotSpatial.Tests.Common
{
    /// <summary>
    /// This contains common functions for all test classes.
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Gets the absolute path of the given relative path.
        /// </summary>
        /// <param name="relPath">relative path to get the absolute path for.</param>
        /// <returns>The absolute path.</returns>
        public static string AbsolutePath(string relPath)
        {
            return System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, relPath);
        }
    }
}
