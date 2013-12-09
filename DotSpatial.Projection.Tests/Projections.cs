using DotSpatial.Projections;
using NUnit.Framework;

namespace DotSpatial.Projection.Tests
{
    [TestFixture]
    public class Projections
    {
        [Test]
        public void projectPoint()
        {
            //Sets up a array to contain the x and y coordinates
            double[] first = new double[2];
            first[0] = 0;
            first[1] = 1;

            double[] second = new double[2];
            second[0] = 0;
            second[1] = 1;
            //An array for the z coordinate
            double[] z = new double[1];
            z[0] = 1;
            //Defines the starting coordiante system
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1984;
            //Defines the ending coordiante system
            ProjectionInfo pEnd = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousLambertConformalConic;
            //Calls the reproject function that will transform the input location to the output locaiton
            Reproject.ReprojectPoints(first, z, pStart, pEnd, 0, 1);
            Reproject.ReprojectPoints(second, z, pStart, pEnd, 0, 1);
            
            Assert.AreEqual(first[0], second[0]);
            Assert.AreEqual(first[1], second[1]);

            Assert.AreNotEqual(first[0], 0);
            Assert.AreNotEqual(first[1], 1);
            Assert.AreNotEqual(second[0], 0);
            Assert.AreNotEqual(second[1], 1);

            Assert.AreEqual(first[0], 10723420.030693574);
            Assert.AreEqual(first[1], 1768929.0089786104);

            Assert.AreEqual(second[0], 10723420.030693574);
            Assert.AreEqual(second[1], 1768929.0089786104);
            
        }

    }
}
