using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Data;
using NUnit.Framework;
using DotSpatial.Topology;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class RasterBoundTests
    {
        [Test]
        public void TestMethod1()
        {
            double[] affine = new double[6];
            affine[0] = 10;
            affine[1] = 1;
            affine[2] = .2;
            affine[3] = 100;
            affine[4] = .2;
            affine[5] = -1.2;
            RasterBounds rb = new RasterBounds(100, 100, affine);
            int row = 40;
            int col = 30;
            Coordinate c = rb.CellCenter_ToProj(row, col);
            RcIndex rc = rb.ProjToCell(c);
            Assert.AreEqual(rc.Row, row);
            Assert.AreEqual(rc.Column, col);
        }
    }
}
