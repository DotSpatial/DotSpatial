﻿using DotSpatial.Analysis;
using DotSpatial.Data;

namespace CodeSnippets
{
    public class BufferExamples
    {
        /// <summary>
        /// This code demonstrates how to open an existing shapefile as a new feature set, buffer the features and then save them to a different file.
        /// </summary>
        /// <param name="fileName">Path of your shapefile (e.g. C:\myShapefile.shp).</param>
        public static void BufferFeatures(string fileName)
        {
            // Pass in the file path of the shapefile that will be opened
            IFeatureSet fs = FeatureSet.Open(@"C:\[Your File Path]\Municipalities.shp");
         
            // Buffer the features of the feature set "fs"
            IFeatureSet bs = fs.Buffer(10, true);
            
            // Saves the buffered feature set as a new file
            bs.SaveAs(@"C:\[Your File Path]\Municipalities_Buffer.shp", true);
        }


        /// <summary>
        /// This code demonstrates how to use DotSpatial.Analysis.Buffer.AddBuffer.
        /// </summary>
        /// <param name="fileName">Path of your shapefile (e.g. C:\myShapefile.shp).</param>
        public static void AnalysisBufferAddBuffer(string fileName)
        {
            // Pass in the file path of the shapefile that will be opened
            IFeatureSet fs = FeatureSet.Open(fileName);

            // create an output feature set of the same feature type
            IFeatureSet fs2 = new FeatureSet(fs.FeatureType);

            // buffer the features of the first feature set by 10 and add them to the output feature set
            Buffer.AddBuffer(fs, 10, fs2);
        }

    }
}
