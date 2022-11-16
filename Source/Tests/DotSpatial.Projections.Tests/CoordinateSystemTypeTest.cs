// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    ///This is a test class for CoordinateSystemTypeTest and is intended
    ///to test the new CoordinateSystemType field for ProjectionInfo.
    ///</summary>
    [TestFixture]
    public class CoordinateSystemTypeTest
    {
        /// <summary>
        /// Proj4 string esri comparison test.
        /// </summary>
        [Test]
        public void Proj4EsriComparisonTest()
        {
            //test projected coordinate systems
            ICoordinateSystemCategoryHolder CoordSysCategoryHolder = (ICoordinateSystemCategoryHolder)KnownCoordinateSystems.Projected;
            TestCategoryHolder(CoordSysCategoryHolder);

            //test geographic coordinate systems
            CoordSysCategoryHolder = KnownCoordinateSystems.Geographic;
            TestCategoryHolder(CoordSysCategoryHolder);
        }

        private void TestCategoryHolder(ICoordinateSystemCategoryHolder CoordSysCategoryHolder)
        {
            System.Diagnostics.Debug.Print("{0}==========================================", CoordSysCategoryHolder.ToString());
            foreach (var majCatName in CoordSysCategoryHolder.Names)
            {
                var coordSysCat = CoordSysCategoryHolder.GetCategory(majCatName);
                if (coordSysCat != null)
                {
                    foreach (var minCatName in coordSysCat.Names)
                    {
                        ProjectionInfo prj = coordSysCat.GetProjection(minCatName);
                        System.Diagnostics.Debug.Print("{0}:{1}\r\n  IsLatLon={2}\r\n  CoordinateSystemType={3}", majCatName, minCatName, prj.IsLatLon, prj.CoordinateSystemType);
                    }
                }
            }
        }
    }
}
