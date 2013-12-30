// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description: The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// This code is based on Felix Obermaier's GDALHelper class from SharpMap http://sharpmap.codeplex.com/
// and the suggestions of Tamas Szekeres
//
// The Initial Developer of this Original Code is tidyup (Ben Tombs). Created 18/11/2010 2:22:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
//        Name       |     Date     |   Description
// -------------------------------------------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// Helper class for GDAL environment variable setting and driver initialisation
    /// </summary>
    public static class GdalHelper
    {
        ///<summary>
        /// True if the GDAL environment has been configured
        ///</summary>
        public static readonly bool IsGdalConfigured;

        static GdalHelper()
        {
            try
            {
                if (!IsGdalConfigured)
                {
                    var driverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (driverPath == null) throw new GdalException("Cannot get the executing assemblies location");
                    Gdal.SetConfigOption("GDAL_DRIVER_PATH", Path.Combine(driverPath, "gdalplugins"));
                    Gdal.SetConfigOption("GDAL_DATA", Path.Combine(driverPath, "gdal-data"));
                    var systemPath = Environment.GetEnvironmentVariable("PATH");
                    if (systemPath == null) throw new GdalException("Cannot get the environment path variable");
                    var paths = systemPath.Split(new[] { Path.PathSeparator });
                    foreach (
                        var pathItem in
                            paths.Where(
                                pathItem =>
                                pathItem != null &&
                                pathItem.Equals(driverPath, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        IsGdalConfigured = true;
                        //TODO can we and do we need to detect other gdal paths and remove them to avoid conflicts? If not just continue.
                        continue;
                    }
                    if (!IsGdalConfigured)
                    {
                        Environment.SetEnvironmentVariable("PATH", driverPath + ";" + systemPath);
                        IsGdalConfigured = true;
                    }
                }
                //The driverPath must be set or already exist to get this far
                Gdal.AllRegister();
            }
            catch (Exception ex)
            {
                throw new GdalException("Error configuring the GDAL data extensions dependencies", ex);
            }
        }

        ///<summary>
        /// set up the gdal dependencies by forcing the default constructor
        ///</summary>
        public static void Configure()
        {
        }
    }
}