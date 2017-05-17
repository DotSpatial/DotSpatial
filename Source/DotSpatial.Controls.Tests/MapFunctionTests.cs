// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// Tests for MapFunction.
    /// </summary>
    [TestFixture]
    internal class MapFunctionTests
    {
        /// <summary>
        /// Checks that the FunctionActived event is raised inside Activate.
        /// </summary>
        [Test]
        public void FunctionActivatedRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.FunctionActivated += (sender, args) => flag = true;
            target.Activate();
            Assert.AreEqual(true, flag);
        }

        /// <summary>
        /// Checks that the FunctionDeactivated event is raised inside Deactivate.
        /// </summary>
        [Test]
        public void FunctionDeActivatedRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.FunctionDeactivated += (sender, args) => flag = true;
            target.Deactivate();
            Assert.AreEqual(true, flag);
        }

        /// <summary>
        /// Checks that the MouseDown event is raised inside DoMouseDown.
        /// </summary>
        [Test]
        public void MouseDownRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseDown += (sender, args) => flag = true;
            target.DoMouseDown(null);
            Assert.AreEqual(true, flag);
        }

        /// <summary>
        /// Checks that the MouseMove event is raised inside DoMouseMove.
        /// </summary>
        [Test]
        public void MouseMoveRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseMove += (sender, args) => flag = true;
            target.DoMouseMove(null);
            Assert.AreEqual(true, flag);
        }

        /// <summary>
        /// Checks that the MouseUp event is raised inside DoMouseUp.
        /// </summary>
        [Test]
        public void MouseUpRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseUp += (sender, args) => flag = true;
            target.DoMouseUp(null);
            Assert.AreEqual(true, flag);
        }

        /// <summary>
        /// Checks that the MouseWheel event is raised inside DoMouseWheel.
        /// </summary>
        [Test]
        public void MouseWheelRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseWheel += (sender, args) => flag = true;
            target.DoMouseWheel(null);
            Assert.AreEqual(true, flag);
        }

        /// <summary>
        /// Checks that the MouseDoubleClick event is raised inside DoMouseDoubleClick.
        /// </summary>
        [Test]
        public void MouseDoubleClickRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseDoubleClick += (sender, args) => flag = true;
            target.DoMouseDoubleClick(null);
            Assert.AreEqual(true, flag);
        }

        /// <summary>
        /// Checks that the KeyUp event is raised inside DoKeyUp.
        /// </summary>
        [Test]
        public void KeyUpRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.KeyUp += (sender, args) => flag = true;
            target.DoKeyUp(null);
            Assert.AreEqual(true, flag);
        }
    }
}
