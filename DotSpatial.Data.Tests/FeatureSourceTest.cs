using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Data;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class FeatureSourceTest
    {
        //[Test]
        //public void TestMoveShape()
        //{
        //    PolygonShapefileFeatureSource file = new PolygonShapefileFeatureSource("C:\\Data\\Shapefiles\\states.shp");
        //    int index = 0;
        //    IFeatureSet fs = file.Select("STATE_NAME = 'Washington'", null, ref index, 500);
        //    IFeature iFeature = fs.GetFeature(0);
        //    IFeature ifc = iFeature.Copy();
        //    int nNumPoints = ifc.BasicGeometry.NumPoints;
        //    ifc.DataRow["STATE_NAME"] = "Fake_Washington";
        //    for (int j = 0; j < nNumPoints; j++)
        //        ifc.Coordinates[j].X -= 15;
        //    ifc.UpdateEnvelope(); // we change this shape, so update the extent
        //    file.Add(ifc);
        //    file.UpdateExtents(); // we changed the extent of the shapefile so update the extent
        //}
        //[Test]
        //public void UpdateShapes()
        //{
        //    PolygonShapefileFeatureSource file = new PolygonShapefileFeatureSource("C:\\Data\\Shapefiles\\states.shp");
        //    file.UpdateExtents();
        //}
    }
}
