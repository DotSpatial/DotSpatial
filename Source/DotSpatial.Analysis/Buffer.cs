// *******************************************************************************************************
// Product: DotSpatial.Analysis.Overlay.cs
// Description: Class for overlay functions. Put other overlay functions here such as intersect, union.

// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Dan Ames           |  2/27/2013         |  Initially written.  
// *******************************************************************************************************

using System;
using DotSpatial.Data;

namespace DotSpatial.Analysis
{

    /// <summary>
    /// A class that makes adding a buffer to a feature set very simpple.
    /// </summary>
    public class Buffer
    {
        /// <summary>
        /// A static function to compute the buffer and return the result to the Execute function.
        /// </summary>
        /// <param name="inputFeatures">The feature set that will be buffered.</param>
        /// <param name="bufferDistance">The distance of the buffer.</param>
        /// <param name="outputFeatures">The resulting feature set that will show the buffer.</param>
        /// <param name="cancelProgressHandler">Optional parameter to report progress and cancel if needed.</param>
        /// <returns></returns>
        public static bool AddBuffer(IFeatureSet inputFeatures, double bufferDistance, IFeatureSet outputFeatures, ICancelProgressHandler cancelProgressHandler = null)
        {
            int numFeatures = inputFeatures.Features.Count;
            for (int i = 0; i < numFeatures; i++)
            {
                inputFeatures.Features[i].Buffer(bufferDistance, outputFeatures);

                // Here we update the progress
                if (cancelProgressHandler != null)
                {
                    if (cancelProgressHandler.Cancel)
                    {
                        return false;
                    }
                    int progress = Convert.ToInt32(i * 100 / numFeatures);
                    cancelProgressHandler.Progress("buffer_tool", progress, "Buffering features.");
                }
            }
            return true;
        }
    }
}
