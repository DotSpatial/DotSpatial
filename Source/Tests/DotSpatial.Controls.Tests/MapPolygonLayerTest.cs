// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Symbology;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// Tests for the MapPolygonLayer.
    /// </summary>
    [TestClass]
    public class MapPolygonLayerTest
    {
        #region Methods

        /// <summary>
        /// Not sure what this is supposed to do.
        /// </summary>
        [TestMethod]
        public void TestSetViewExtents()
        {
            Map mainMap = new()
            {
                Projection = KnownCoordinateSystems.Projected.World.WebMercator
            };

            Extent defaultMapExtent = new(-130, 5, -70, 60);

            string baseMapFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");

            MapGroup baseGroup = new(mainMap.Layers, mainMap.MapFrame, mainMap.ProgressHandler)
            {
                LegendText = "Base Map Data",
                ParentMapFrame = mainMap.MapFrame,
                MapFrame = mainMap.MapFrame,
                IsVisible = true
            };

            // load the 'Countries of the world' layer
            try
            {
                string fileName = Path.Combine(baseMapFolder, "50m_admin_0_countries.shp");
                IFeatureSet fsCountries = FeatureSet.OpenFile(fileName);
                fsCountries.Reproject(mainMap.Projection);
                MapPolygonLayer layCountries = new(fsCountries)
                {
                    LegendText = "Countries"
                };
                PolygonScheme schmCountries = new()
                {
                    EditorSettings =
                        {
                            StartColor = Color.Orange,
                            EndColor = Color.Silver,
                            ClassificationType = ClassificationType.UniqueValues,
                            FieldName = "NAME",
                            UseGradient = true
                        }
                };
                schmCountries.CreateCategories(layCountries.DataSet.DataTable);
                layCountries.Symbology = schmCountries;
                baseGroup.Layers.Add(layCountries);
                layCountries.MapFrame = mainMap.MapFrame;
            }
            catch
            {
            }

            // load U.S. states layer
            try
            {
                string fileName = Path.Combine(baseMapFolder, "50mil_us_states.shp");
                IFeatureSet fsStates = FeatureSet.OpenFile(fileName);
                fsStates.Reproject(mainMap.Projection);
                var layStates = new MapPolygonLayer(fsStates);
                PolygonScheme schmStates = new();
                layStates.IsVisible = true;
                layStates.LegendText = "U.S. States";
                schmStates.EditorSettings.StartColor = Color.LemonChiffon;
                schmStates.EditorSettings.EndColor = Color.LightPink;
                schmStates.EditorSettings.ClassificationType = ClassificationType.UniqueValues;
                schmStates.EditorSettings.FieldName = "NAME";
                schmStates.EditorSettings.UseGradient = true;
                schmStates.CreateCategories(layStates.DataSet.DataTable);
                layStates.Symbology = schmStates;
                baseGroup.Layers.Add(layStates);
                layStates.MapFrame = mainMap.MapFrame;
            }
            catch
            {
            }

            // load Canada Provinces layer
            try
            {
                string fileName = Path.Combine(baseMapFolder, "50mil_canada_provinces.shp");
                IFeatureSet fsProvince = FeatureSet.OpenFile(fileName);
                fsProvince.Reproject(mainMap.Projection);
                MapPolygonLayer layProvince = new(fsProvince);
                PolygonScheme schmProvince = new();
                layProvince.IsVisible = true;
                layProvince.LegendText = "Canada Provinces";
                schmProvince.EditorSettings.StartColor = Color.Green;
                schmProvince.EditorSettings.EndColor = Color.Yellow;
                schmProvince.EditorSettings.ClassificationType = ClassificationType.UniqueValues;
                schmProvince.EditorSettings.FieldName = "NAME";
                schmProvince.EditorSettings.UseGradient = true;
                schmProvince.CreateCategories(layProvince.DataSet.DataTable);
                layProvince.Symbology = schmProvince;
                baseGroup.Layers.Add(layProvince);
                layProvince.MapFrame = mainMap.MapFrame;
            }
            catch
            {
            }

            // theme data group
            // create a new empty 'themes' data group
            try
            {
                MapGroup themeGroup = new(mainMap.Layers, mainMap.MapFrame, mainMap.ProgressHandler)
                {
                    ParentMapFrame = mainMap.MapFrame,
                    MapFrame = mainMap.MapFrame,
                    LegendText = "Themes"
                };
            }
            catch
            {
            }

            double[] xy = new double[4];
            xy[0] = defaultMapExtent.MinX;
            xy[1] = defaultMapExtent.MinY;
            xy[2] = defaultMapExtent.MaxX;
            xy[3] = defaultMapExtent.MaxY;
            double[] z = { 0, 0 };
            string esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            ProjectionInfo wgs84 = ProjectionInfo.FromEsriString(esri);
            Reproject.ReprojectPoints(xy, z, wgs84, mainMap.Projection, 0, 2);

            xy[0] = 1000000000000000;
            xy[1] = 2000000000000000;
            xy[2] = 3000000000000000;
            xy[3] = 4000000000000000;
            Extent ext = new(xy);
            mainMap.ViewExtents = ext;
        }

        #endregion
    }
}