// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [SetUpFixture]
    public class AssemblyInit
    {
        /// <summary>
        /// 
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            GridShift.InitializeExternalGrids(Common.AbsolutePath("GeogTransformGrids"), false);
        }
    }
}

