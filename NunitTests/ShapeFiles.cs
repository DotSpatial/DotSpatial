using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;
using DotSpatial.Topology;
using System.IO;
using System.Diagnostics;
using NUnit.Framework;

namespace ReadWriteTest
{
    [TestFixture]
    public class ReadWriteTest
    {
        [Test]
        public void PolygonShapeFile()
        {
            String filename = "counties.shp";
            String folder = Path.Combine(new String[] { "..", "..", "..", "DotSpatial.Data.Tests", "Data", "Shapefiles" });
            
            String testFile = Path.Combine(new String[] { folder, filename });
            String newFile = Path.Combine(new String[] { folder, "testSaves", filename });

            IFeatureSet original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(testFile);;

            original.Filename = newFile;
            original.Save();

            IFeatureSet newSave = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(newFile);

            Assert.AreEqual(original.Features.Count, newSave.Features.Count);

            for (int j = 0; j < original.Features.Count; j+=100)
            {
                Assert.AreEqual(original.Features.ElementAt(j).Coordinates, newSave.Features.ElementAt(j).Coordinates);
            }
        }

        [Test]
        public void PointShapeFile()
        {
            String filename = "cities.shp";
            String folder = Path.Combine(new String[] { "..", "..", "..", "DotSpatial.Data.Tests", "Data", "Shapefiles" });

            String testFile = Path.Combine(new String[] { folder, filename });
            String newFile = Path.Combine(new String[] { folder, "monoTests", filename });

            IFeatureSet original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(testFile); ;

            original.Filename = newFile;
            original.Save();

            IFeatureSet newSave = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(newFile);

            Assert.AreEqual(original.Features.Count, newSave.Features.Count);

            for (int j = 0; j < original.Features.Count; j+=100)
            {
                Assert.AreEqual(original.Features.ElementAt(j).Coordinates, newSave.Features.ElementAt(j).Coordinates);
            }
        }

        [Test]
        public void LineShapeFile()
        {
            String filename = "rivers.shp";
            String folder = Path.Combine(new String[] { "..", "..", "..", "DotSpatial.Data.Tests", "Data", "Shapefiles" });

            String testFile = Path.Combine(new String[] { folder, filename });
            String newFile = Path.Combine(new String[] { folder, "monoTests", filename });

            IFeatureSet original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(testFile); ;

            original.Filename = newFile;
            original.Save();

            IFeatureSet newSave = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(newFile);

            Assert.AreEqual(original.Features.Count, newSave.Features.Count);

            for (int j = 0; j < original.Features.Count; j+=100)
            {
                Assert.AreEqual(original.Features.ElementAt(j).Coordinates, newSave.Features.ElementAt(j).Coordinates);
            }
        }
    }
}