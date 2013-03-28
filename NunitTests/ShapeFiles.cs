using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;
using DotSpatial.Topology;
using DotSpatial.Mono;
using System.IO;
using System.Diagnostics;
using NUnit.Framework;

namespace ReadWriteTest
{
    [TestFixture]
    public class ReadWriteTest
    {
        [Test]
        public void ShapefilesTest()
        {
            List<IFeatureSet> originals = new List<IFeatureSet>();
            List<IFeatureSet> monoSaves = new List<IFeatureSet>();
            String testsFolder = Path.Combine(new String[] { "..", "..", "..", "DotSpatial.Data.Tests", "Data", "Shapefiles" });
            String monoFolder = Path.Combine(new String[] { testsFolder, "monoTests" });

            String[] files = Directory.GetFiles(testsFolder, "*.shp");

            foreach (String file in files)
            {
                IDataSet dataset = DataManager.DefaultDataManager.OpenFile(file);
                IFeatureSet fs = (IFeatureSet)dataset;
                if (Mono.IsRunningOnMono())
                {
                    fs.Filename = Path.Combine(new String[] { monoFolder, Path.GetFileName(file) });
                    fs.Save();
                }
                originals.Add(fs);
            }

            String[] monoTests = Directory.GetFiles(monoFolder, "*.shp");

            foreach (String file in monoTests)
            {
                IDataSet dataset = DataManager.DefaultDataManager.OpenFile(file);
                IFeatureSet fs = (IFeatureSet)dataset;
                monoSaves.Add(fs);
            }

            Assert.AreEqual(originals.Count, monoSaves.Count);

            for (int i = 0; i < monoSaves.Count; i+=2)
            {
                IFeatureSet currOriginal = originals.ElementAt(i);
                IFeatureSet currMono = monoSaves.ElementAt(i);

                Assert.AreEqual(currOriginal.Features.Count, currMono.Features.Count);

                for (int j = 0; j < currOriginal.Features.Count; j+=100)
                {
                    Assert.AreEqual(currOriginal.Features.ElementAt(j).Coordinates, currMono.Features.ElementAt(j).Coordinates);
                }
            }
        }
    }
}