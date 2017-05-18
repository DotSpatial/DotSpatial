// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;

namespace DotSpatial.Analysis
{
    /// <summary>
    /// A class that makes adding a buffer to a feature set very simpple.
    /// </summary>
    public class Buffer
    {
        #region Methods

        /// <summary>
        /// A static function to compute the buffer and return the result to the Execute function.
        /// </summary>
        /// <param name="inputFeatures">The feature set that will be buffered.</param>
        /// <param name="bufferDistance">The distance of the buffer.</param>
        /// <param name="outputFeatures">The resulting feature set that will show the buffer.</param>
        /// <param name="cancelProgressHandler">Optional parameter to report progress and cancel if needed.</param>
        /// <returns>False if the Progress was canceled, otherwise true.</returns>
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

        #endregion
    }
}