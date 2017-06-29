using System;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Symbology;
using System.IO;
using System.Drawing;
using DotSpatial.Projections;

namespace DotSpatial.Controls.Tests
{
    [TestClass]
    public class MapPolygonLayerTest
    {
        [TestMethod]
        public void TestSetViewExtents()
        {
            Map mainMap = new Map();
            mainMap.Projection = KnownCoordinateSystems.Projected.World.WebMercator;

            Extent defaultMapExtent = new Extent(-130, 5, -70, 60);

            string baseMapFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
            //SetDefaultMapExtents(mainMap);
            MapPolygonLayer layStates;

            MapGroup baseGroup = new MapGroup(mainMap.Layers, mainMap.MapFrame, mainMap.ProgressHandler);
            baseGroup.LegendText = "Base Map Data";
            baseGroup.ParentMapFrame = mainMap.MapFrame;
            baseGroup.MapFrame = mainMap.MapFrame;
            baseGroup.IsVisible = true;

            //load the 'Countries of the world' layer
            try
            {
                string fileName = Path.Combine(baseMapFolder, "50m_admin_0_countries.shp");
                IFeatureSet fsCountries = FeatureSet.OpenFile(fileName);
                fsCountries.Reproject(mainMap.Projection);
                MapPolygonLayer layCountries = new MapPolygonLayer(fsCountries);
                layCountries.LegendText = "Countries";
                PolygonScheme schmCountries = new PolygonScheme();
                schmCountries.EditorSettings.StartColor = Color.Orange;
                schmCountries.EditorSettings.EndColor = Color.Silver;
                schmCountries.EditorSettings.ClassificationType =
                    ClassificationType.UniqueValues;
                schmCountries.EditorSettings.FieldName = "NAME";
                schmCountries.EditorSettings.UseGradient = true;
                schmCountries.CreateCategories(layCountries.DataSet.DataTable);
                layCountries.Symbology = schmCountries;
                baseGroup.Layers.Add(layCountries);
                layCountries.MapFrame = mainMap.MapFrame;
            }
            catch { }
            //load U.S. states layer
            try
            {
                string fileName = Path.Combine(baseMapFolder, "50mil_us_states.shp");
                IFeatureSet fsStates = FeatureSet.OpenFile(fileName);
                fsStates.Reproject(mainMap.Projection);
                layStates = new MapPolygonLayer(fsStates);
                PolygonScheme schmStates = new PolygonScheme();
                layStates.IsVisible = true;
                layStates.LegendText = "U.S. States";
                schmStates.EditorSettings.StartColor = Color.LemonChiffon;
                schmStates.EditorSettings.EndColor = Color.LightPink;
                schmStates.EditorSettings.ClassificationType =
                    ClassificationType.UniqueValues;
                schmStates.EditorSettings.FieldName = "NAME";
                schmStates.EditorSettings.UseGradient = true;
                schmStates.CreateCategories(layStates.DataSet.DataTable);
                layStates.Symbology = schmStates;
                baseGroup.Layers.Add(layStates);
                layStates.MapFrame = mainMap.MapFrame;
            }
            catch { }
            //load Canada Provinces layer
            try
            {
                string fileName = Path.Combine(baseMapFolder, "50mil_canada_provinces.shp");
                IFeatureSet fsProvince = FeatureSet.OpenFile(fileName);
                fsProvince.Reproject(mainMap.Projection);
                MapPolygonLayer layProvince = new MapPolygonLayer(fsProvince);
                PolygonScheme schmProvince = new PolygonScheme();
                layProvince.IsVisible = true;
                layProvince.LegendText = "Canada Provinces";
                schmProvince.EditorSettings.StartColor = Color.Green;
                schmProvince.EditorSettings.EndColor = Color.Yellow;
                schmProvince.EditorSettings.ClassificationType =
                    ClassificationType.UniqueValues;
                schmProvince.EditorSettings.FieldName = "NAME";
                schmProvince.EditorSettings.UseGradient = true;
                schmProvince.CreateCategories(layProvince.DataSet.DataTable);
                layProvince.Symbology = schmProvince;
                baseGroup.Layers.Add(layProvince);
                layProvince.MapFrame = mainMap.MapFrame;
            }
            catch { }

            

            //theme data group
            //create a new empty 'themes' data group
            try
            {
                MapGroup themeGroup = new MapGroup(mainMap.Layers,
                    mainMap.MapFrame, mainMap.ProgressHandler);
                themeGroup.ParentMapFrame = mainMap.MapFrame;
                themeGroup.MapFrame = mainMap.MapFrame;
                themeGroup.LegendText = "Themes";
            }
            catch { }

            double[] xy = new double[4];
            xy[0] = defaultMapExtent.MinX;
            xy[1] = defaultMapExtent.MinY;
            xy[2] = defaultMapExtent.MaxX;
            xy[3] = defaultMapExtent.MaxY;
            double[] z = new double[] { 0, 0 };
            string esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            ProjectionInfo wgs84 = ProjectionInfo.FromEsriString(esri);
            Reproject.ReprojectPoints(xy, z, wgs84, mainMap.Projection, 0, 2);

            xy[0] = 1000000000000000;
            xy[1] = 2000000000000000;
            xy[2] = 3000000000000000;
            xy[3] = 4000000000000000;
            Extent ext = new Extent(xy);
            mainMap.ViewExtents = ext;
        }
    }
}
