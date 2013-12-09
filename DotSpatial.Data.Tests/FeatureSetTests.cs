using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class FeatureSetTests
    {
        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        [Test]
        public void IndexModeToFeaturesClear()
        {
            var file = Path.Combine(_shapefiles, @"Topology_Test.shp");
            IFeatureSet fs = FeatureSet.Open(file);
            fs.FillAttributes();
            fs.Features.Clear();
            Assert.AreEqual(fs.Features.Count, 0);
            Assert.AreEqual(fs.DataTable.Rows.Count, 0);
        }

        [Test]
        public void UnionFeatureSetTest()
        {
            var file = Path.Combine(_shapefiles, @"Topology_Test.shp");
            IFeatureSet fs = FeatureSet.Open(file);
            FeatureSet fsunion = new FeatureSet();

            // This is needed or else the table won't have the columns for copying attributes.
            fsunion.CopyTableSchema(fs);

            // Create a list of all the original shapes so if we union A->B we don't also union B->A
            var freeFeatures = fs.Features.Select((t, i) => i).ToList();

            while (freeFeatures.Count > 0)
            {
                var fOriginal = fs.Features[freeFeatures[0]];

                // Whether this gets unioned or not, it has been handled and should not be re-done.
                // We also don't want to waste time unioning shapes to themselves.
                freeFeatures.RemoveAt(0);

                // This is the unioned result.  Remember, we may just add the original feature if no 
                // shapes present themselves for unioning.
                IFeature fResult = null;

                // This is the list of any shapes that get unioned with our shape.  
                List<int> mergedList = new List<int>();
                bool shapeChanged;
                do
                {
                    shapeChanged = false; // reset this each time.
                    foreach (int index in freeFeatures)
                    {
                        if (fResult == null)
                        {
                            if (fOriginal.Intersects(fs.Features[index]))
                            {
                                // If FieldJoinType is set to all, and large numbers of shapes are combined,
                                // the attribute table will have a huge number of extra columns, since 
                                // every column will be replicated for each instance.
                                fResult = fOriginal.Union(fs.Features[index], fsunion, FieldJoinType.LocalOnly);

                                // if the shape changed for an index greater than 0, then the newly unioned
                                // shape might now union with an earlier shape that we skipped before.
                                shapeChanged = true;
                            }
                        }
                        else
                        {
                            if (fResult.Intersects(fs.Features[index]))
                            {
                                // snowball unioned features.  Keep adding features to the same unioned shape.
                                fResult = fResult.Union(fs.Features[index], fsunion, FieldJoinType.LocalOnly);
                                shapeChanged = true;
                            }
                        }
                        if (shapeChanged)
                        {

                            // Don't modify the "freefeatures" list during a loop.  Keep track until later.
                            mergedList.Add(index);

                            // Less double-checking happens if we break rather than finishing the loop
                            // and then retest the whole loop because of a change early in the list.
                            break;
                        }

                    }
                    foreach (int index in mergedList)
                    {
                        // We don't want to add the same shape twice.
                        freeFeatures.Remove(index);
                    }
                } while (shapeChanged);

                // Add fResult, unless it is null, in which case add fOriginal.
                fsunion.Features.Add(fResult ?? fOriginal);

                // Union doesn't actually add to the output featureset.  The featureset is only
                // provided to the union method to handle column manipulation if necessary.
                fsunion.Features.Add(fResult);

            }

            // fsunion is in-memory until this is called.  Once this is called, the extension will
            // be parsed to determine that a shapefile is required.  The attributes and features will
            // be moved to variables in an appropriate shapefile class internally, and
            // then that class will save the features to the disk.
            fsunion.SaveAs(_shapefiles + @"Union_Test.shp", true);

            try
            {
                // cleanup
                File.Delete(_shapefiles + @"Union_Test.shp");
                File.Delete(_shapefiles + @"Union_Test.dbf");
                File.Delete(_shapefiles + @"Union_Test.shx");
            }
            catch (IOException)
            {
            }
        }

        /// <summary>
        ///A test for FilePath http://dotspatial.codeplex.com/workitem/232
        ///</summary>
        [Test]
        public void FilePathTestWithSpaces()
        {
            FeatureSet target = new FeatureSet();
            string relPath1 = @"folder";
            string relPath2 = @"name\states.shp";
            string relativeFilePath = relPath1 + " " +  relPath2;
            string expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(), relPath1) +
                                      " " + relPath2;
            string actualFilePath;
            target.FilePath = relativeFilePath;
            actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        /// <summary>
        ///A test for FilePath http://dotspatial.codeplex.com/workitem/232
        ///</summary>
        [Test]
        public void FilePathTest1()
        {
            FeatureSet target = new FeatureSet();
            string relativeFilePath = @"inner\states.shp";
            string expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(),relativeFilePath);

            string actualFilePath;
            target.FilePath = relativeFilePath;
            actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        /// <summary>
        ///A test for FilePath http://dotspatial.codeplex.com/workitem/232
        ///</summary>
        [Test]
        public void FilePathTest2()
        {
            FeatureSet target = new FeatureSet();
            string relativeFilePath = @"..\..\states.shp";
            string expectedFullPath = Path.GetFullPath(relativeFilePath);

            string actualFilePath;
            target.FilePath = relativeFilePath;
            actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }
    }
}
