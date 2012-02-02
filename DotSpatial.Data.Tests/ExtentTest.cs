using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Data.Tests
{
    [TestClass]
    public class ExtentTest
    {
        [TestMethod]
        public void ExtentSizeCheck()
        {
            int count = 100000;
            double w = 0;
            // Measure starting point memory use
            long start = GC.GetTotalMemory(true);

            // Allocate a new array of count Extent classes.
            // If methods don't count, should be about 3,200,000 bytes.
            ExtentMZ[] memhog = new ExtentMZ[count];

            for (int i = 0; i < count; i++)
            {
                memhog[i] = new ExtentMZ();
                if (!memhog[i].IsEmpty()) w = memhog[i].Width;
            }

            // Obtain measurements after creating the new byte[]
            long end = GC.GetTotalMemory(true);
            Debug.WriteLine("width: " + w);
            long size = (end - start) / count;

            // Ensure that the Array stays in memory and doesn't get optimized away
            Debug.WriteLine("Memory size of Extent = " + size);

            // Size of Extent is 44.
            // Size of ExtentM is 60
            // Size of ExtentMZ is 76
        }


    }
}
