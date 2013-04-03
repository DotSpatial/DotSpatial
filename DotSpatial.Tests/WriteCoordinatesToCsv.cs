using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotSpatial.Projections;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;

namespace DotSpatial.Tests
{
    [TestClass]
    public class WriteCoordinatesToCsv
    {
        [TestMethod]
        public void WriteCoordinatesToCsvFile()
        {
            Random rnd = new Random();
            double[] coords = new double[200];
            double[] z = new double[100];
            for (int i = 0; i < 100; i++)
            {
                coords[i * 2] = rnd.NextDouble() * 6 - 126;
                coords[i * 2 + 1] = rnd.NextDouble() * 90;
            }
            double[] projected = new double[200];
            Array.Copy(coords, projected, 200);
            ProjectionInfo dest = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone10N;
            Reproject.ReprojectPoints(projected, z, KnownCoordinateSystems.Geographic.World.WGS1984, dest, 0, 100);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                sb.Append(coords[2 * i] + ",");
                sb.Append(coords[2 * i + 1] + ",");
                sb.Append(projected[2 * i] + ",");
                sb.AppendLine(projected[2 * i + 1].ToString());
            }
            Directory.CreateDirectory(@"C:\Data\");
            File.WriteAllText(@"C:\Data\projectionTest.csv", sb.ToString());
        }
    }
}