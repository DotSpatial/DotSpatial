// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for Group.
    /// </summary>
    [TestFixture]
    internal class GroupTests
    {
        /// <summary>
        /// Checks that there is no exception when zooming to an empty Group.
        /// </summary>
        [Test(Description = "Checks that there is no exception when zoom to empty Group (https://github.com/DotSpatial/DotSpatial/issues/796)")]
        public void ZoomToGroupDoesntThrowExceptionsOnZoom()
        {
            var target = new Group();

            // Verify context menu item click
            var menuItem = target.ContextMenuItems.First(_ => _.Name == "Zoom to Group");
            Assert.DoesNotThrow(() => menuItem.ClickHandler(menuItem, EventArgs.Empty));

            // Also verify direct call
            Assert.DoesNotThrow(() => target.ZoomToGroup());
        }
    }
}
