using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Data.Tests
{


    /// <summary>
    ///This is a test class for FeatureSetTest and is intended
    ///to contain all FeatureSetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FeatureSetTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for FilePath http://dotspatial.codeplex.com/workitem/232
        ///</summary>
        [TestMethod()]
        public void FilePathTestWithSpaces()
        {
            FeatureSet target = new FeatureSet();
            // this path must exist.
            Directory.SetCurrentDirectory("C:\\Windows\\system32");
            // this path does need to actually exist.
            string expectedFullPath = @"C:\Windows\system32\folder name\states.shp";
            string relativeFilePath = @"folder name\states.shp";

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
        [TestMethod()]
        public void FilePathTest1()
        {
            FeatureSet target = new FeatureSet();
            // this path must exist.
            Directory.SetCurrentDirectory("C:\\Windows\\system32");
            // this path does need to actually exist.
            string expectedFullPath = @"C:\Windows\system32\inner\states.shp";
            string relativeFilePath = @"inner\states.shp";

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
        [TestMethod()]
        public void FilePathTest2()
        {
            FeatureSet target = new FeatureSet();
            // this path must exist.
            Directory.SetCurrentDirectory("C:\\Windows\\system32");
            // this path does need to actually exist.
            string expectedFullPath = @"C:\states.shp";
            string relativeFilePath = @"..\..\states.shp";

            string actualFilePath;
            target.FilePath = relativeFilePath;
            actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }
    }
}
