using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    [TestFixture]
    class MapFunctionTests
    {
        [Test]
        public void FunctionActivatedRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.FunctionActivated += (sender, args) => flag = true;
            target.Activate();
            Assert.AreEqual(true, flag);
        }

        [Test]
        public void FunctionDeActivatedRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.FunctionDeactivated += (sender, args) => flag = true;
            target.Deactivate();
            Assert.AreEqual(true, flag);
        }

        [Test]
        public void MouseDownRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseDown += (sender, args) => flag = true;
            target.DoMouseDown(null);
            Assert.AreEqual(true, flag);
        }

        [Test]
        public void MouseMoveRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseMove += (sender, args) => flag = true;
            target.DoMouseMove(null);
            Assert.AreEqual(true, flag);
        }

        [Test]
        public void MouseUpRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseUp += (sender, args) => flag = true;
            target.DoMouseUp(null);
            Assert.AreEqual(true, flag);
        }

        [Test]
        public void MouseWheelRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseWheel += (sender, args) => flag = true;
            target.DoMouseWheel(null);
            Assert.AreEqual(true, flag);
        }

        [Test]
        public void MouseDoubleClickRaised()
        {
            var target = new MapFunction();
            var flag = false;
            target.MouseDoubleClick += (sender, args) => flag = true;
            target.DoMouseDoubleClick(null);
            Assert.AreEqual(true, flag);
        }

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
