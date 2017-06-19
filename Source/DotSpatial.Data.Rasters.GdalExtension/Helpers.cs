// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// Helper methods for GDAL.
    /// </summary>
    internal static class Helpers
    {
        #region Methods

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">File that gets opened.</param>
        /// <returns>Opened file as data set.</returns>
        public static Dataset Open(string fileName)
        {
            try
            {
                return Gdal.Open(fileName, Access.GA_Update);
            }
            catch
            {
                try
                {
                    return Gdal.Open(fileName, Access.GA_ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new GdalException(ex.ToString());
                }
            }
        }

        #endregion
    }
}