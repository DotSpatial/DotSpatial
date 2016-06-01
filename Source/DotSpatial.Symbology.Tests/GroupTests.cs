using System;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    internal class GroupTests
    {
        [Test(Description = "Checks that there is no exception when zoom to empty Group (https://github.com/DotSpatial/DotSpatial/issues/796)")]
        public void ZoomToGroup_DoesntThrowExceptions_OnZoom()
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
