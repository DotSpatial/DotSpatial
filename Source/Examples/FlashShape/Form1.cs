// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.


using System.ComponentModel.Composition;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
//using DotSpatial.NTSExtension;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;
//using Point = System.Drawing.Point;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using DotSpatial.Symbology.Forms;

namespace FlashShape
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Is needed to load extensions.
        /// </summary>
        [Export("Shell", typeof(ContainerControl))]
#pragma warning disable IDE0052 // Ungelesene private Member entfernen
        private static ContainerControl Shell;
#pragma warning restore IDE0052 // Ungelesene private Member entfernen

        /// <summary>
        /// Initializes a new Form1.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            if (DesignMode)
            {
                return;
            }

            // These 2 lines are required to load extensions
            Shell = this;
            appManager1.LoadExtensions();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //appManager1.Map = new Map();
            //appManager1.Legend= new Legend();   
            //appManager1.DockManager= new SpatialDockManager();  
            ////var mf = map.MapFrame;

            // open up the lakes shapefile and add it to the map
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileName = Path.Combine(assemblyPath, "Shapefiles", "lakes.shp");
            Debug.Print("filename={0}", fileName);
            IFeatureSet fs = FeatureSet.Open(fileName);

            MapPolygonLayer lyr = new(fs)
            {
                Symbolizer =
                {
                    ScaleMode = ScaleMode.Symbolic,
                    Smoothing = true
                },
                MapFrame = appManager1.Map.MapFrame
            };

            // select 2 shapes
            lyr.SelectByAttribute("[NAME] IN('Lake Erie', 'Lake Michigan')");

            appManager1.Map.Layers.Add(lyr);
        }

        private void btnFlashFirstShape_Click(object sender, EventArgs e)
        {
            // get the first shape of the first layer
            if (appManager1.Map.Layers.Count > 0)
            {
                MapPolygonLayer lyr = (MapPolygonLayer)appManager1.Map.Layers[0];
                if (lyr.FeatureSet.Features.Count > 0)
                {
                    // our checks are done so flash the first shape in the shapefile
                    var firstFeat = lyr.FeatureSet.Features[0];
                    FlashFeature(lyr, firstFeat);
                }
            }
        }

        private void btnFlashLastShape_Click(object sender, EventArgs e)
        {
            // get the first shape of the first layer
            if (appManager1.Map.Layers.Count > 0)
            {
                MapPolygonLayer lyr = (MapPolygonLayer)appManager1.Map.Layers[0];
                if (lyr.FeatureSet.Features.Count > 0)
                {
                    // our checks are done so flash the first shape in the shapefile
                    int lastIdx = lyr.FeatureSet.Features.Count() - 1;
                    var lastFeat = lyr.FeatureSet.Features[lastIdx];
                    FlashFeature(lyr, lastFeat);
                }
            }
        }

        private void btnFlashFirstSelectedShape_Click(object sender, EventArgs e)
        {
            //get the first *selected* shape of the first layer
            //todo: the feature selection doesnt seem to be working
            if (appManager1.Map.Layers.Count > 0)
            {
                //MapPolygonLayer lyr = (MapPolygonLayer)appManager1.Map.Layers[0];
                //Debug.Print("selection count={0}", lyr.Selection.Count);
                //if (lyr.Selection.Count > 0)
                //{
                //    // our checks are done so flash the first *selected* shape in the shapefile
                //    var sel = lyr.Selection.ToFeatureSet();
                //    var firstSelFeat = sel.Features[0];
                //    FlashFeature(lyr, firstSelFeat);
                //}

                //Dim plyrSel As IMapPointLayer = CType(mlyrSel, IMapPointLayer)
                //Dim flyrSel As IMapFeatureLayer = CType(mlyrSel, IMapFeatureLayer)
                //Dim fsSel As IFeatureSet = flyrSel.DataSet

                //IMapLayer mapLyr = appManager1.Map.Layers[0];
                //IMapPolygonLayer pgnLyr = (IMapPolygonLayer)mapLyr;
                //IMapFeatureLayer mapFeatLyr = (IMapFeatureLayer)mapLyr;
                //IFeatureLayer featLyr = (IFeatureLayer)mapLyr;
                //if (featLyr.Selection.Count > 0)
                //{
                //    // our checks are done so flash the first *selected* shape in the shapefile
                //    var selFeats = mapFeatLyr.Selection.ToFeatureSet();
                //    var firstSelFeat = selFeats.Features[0];
                //    FlashFeature(featLyr, firstSelFeat);
                //}
            }
        }

        /// <summary>
        /// Will simulate flash of a polygon shape by changing its outline and fill color for several seconds and will also
        /// display a crosshair across the map centered on the shape. Tested with a polygon shapefile but should work with
        /// lines and points.
        /// </summary>
        /// <param name="featLyr">The feature layer that contains the feature to flash.</param>
        /// <param name="feat">The feature to flash.</param>
        /// <param name="cycleCount">Number of cycles to turn the shape on and off</param>
        /// <remarks>
        /// Needs some work as I couldnt get the shape to flash on and off without refreshing the map. Probably overkill
        /// with the Redraw and the DoEvents.
        /// </remarks>
        public void FlashFeature(IFeatureLayer featLyr, IFeature feat, int cycleCount = 3)
        {
            if (feat == null)
                return;

            if (cycleCount < 0 || cycleCount > 20)
                cycleCount = 3;

            var map = appManager1.Map;
            var mf = appManager1.Map.MapFrame;

            Debug.Print("fid={0}", feat.Fid);

            // *********************************************************************************************************************
            // get the centroid of the shape as a point
            // *********************************************************************************************************************
            var pt = feat.Geometry.Centroid;

            // *********************************************************************************************************************
            // get the extents of the map
            // *********************************************************************************************************************
            var ext = map.Extent;

            // *********************************************************************************************************************
            // now draw two fat lines, one vertical and one horizontal to bring focus the center of the shape.
            // extend the line from each side of the map extents to center of the geometry.
            // *********************************************************************************************************************
            var coord1 = new Coordinate(pt.X, ext.MinY);
            var coord2 = new Coordinate(pt.X, ext.MaxY);
            var coordsH = new List<Coordinate> { coord1, coord2 };// horizontal

            var coord3 = new Coordinate(ext.MinX, pt.Y);
            var coord4 = new Coordinate(ext.MaxX, pt.Y);
            var coordsV = new List<Coordinate> { coord3, coord4 };// vertical

            LineString ls = new LineString(coordsH.ToArray());
            FeatureSet fs = new FeatureSet(FeatureType.Line);
            fs.AddFeature(ls);
            ls = new LineString(coordsV.ToArray());
            fs.AddFeature(ls);
            MapLineLayer grll = new MapLineLayer(fs)
            {
                Symbolizer =
                        {
                            ScaleMode = ScaleMode.Symbolic,
                            Smoothing = true,
                        },
                MapFrame = mf,
            };
            grll.Symbolizer.SetFillColor(Color.Cyan);
            grll.Symbolizer.SetWidth(4);
            mf.DrawingLayers.Add(grll);
            mf.Invalidate();
            map.Invalidate();

            System.Threading.Thread.Sleep(100);
            System.Windows.Forms.Application.DoEvents();

            // *********************************************************************************************************************
            // save the original category of the feature. then add a category to the layer with some default drawing properties. a
            // fill color of green and an outline color of cyan. assign the new category to the feature. this one shape now will take
            // on the properties of our category.
            // *********************************************************************************************************************
            Debug.Print("featLyr type={0}", featLyr.GetType());
            string catKey = "FlashShape_" + Guid.NewGuid();
            IFeatureCategory catOrig;
            IFeatureCategory catNew;
            if (featLyr.GetType() == typeof(MapPolygonLayer))
            {
                var lyr = featLyr as MapPolygonLayer;
                Debug.Print("catCount={0}", lyr.Symbology.Categories.Count);

                catOrig = lyr.GetCategory(feat);

                catNew = new PolygonCategory(Color.Green, Color.Cyan, 2);
                catNew.LegendItemVisible = false;
                catNew.LegendText = catKey;
                lyr.Symbology.Categories.Add(catNew as PolygonCategory);
                Debug.Print("catCount={0}", lyr.Symbology.Categories.Count);

                lyr.SetCategory(feat, catNew);
            }
            else
            {
                throw new NotSupportedException("Unsupported feature type.");
            }

            // *********************************************************************************************************************
            // now we change the drawing options of the category. we color the polygon green, pause, then color the polygon red.
            // And since the category is connected to the shape then it should simulate a flash for a couple cycles, each one being
            // about 300*2 milliseconds in length. i experimented with various ways to display the flashing but in the end it was a 
            // combination of applying the color, redrawing the map, and allowing the application to respond that seemed to work.
            // it may be slow for maps with large amounts of data as it has to redraw in several loops. Overkill on DoEvents?
            // *********************************************************************************************************************
            for (int i = 1; i <= cycleCount; i++)
            {
                // the first pass colors the shape green and leaves it like that for 300 milliseconds
                catNew.SetColor(Color.Green);
                System.Windows.Forms.Application.DoEvents();
                mf.Invalidate();
                map.Invalidate();
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(300);
                System.Windows.Forms.Application.DoEvents();

                // the second pass colors the shape red and leaves it like that for 300 milliseconds
                catNew.SetColor(Color.Red);
                System.Windows.Forms.Application.DoEvents();
                mf.Invalidate();
                map.Invalidate();
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(300);
                System.Windows.Forms.Application.DoEvents();
            }

            // *********************************************************************************************************************
            // now we set the features's category back to the original since we are done flashing. this should set the features's drawing
            // options back to its original. also remove the category we created since we dont need it anymore.
            // *********************************************************************************************************************
            if (featLyr.GetType() == typeof(MapPolygonLayer))
            {
                var lyr = featLyr as MapPolygonLayer;
                Debug.Print("catCount={0}", lyr.Symbology.Categories.Count);
                lyr.SetCategory(feat, catOrig);

                lyr.Symbology.Categories.Remove(catNew as PolygonCategory);
                Debug.Print("catCount={0}", lyr.Symbology.Categories.Count);
            }

            // *********************************************************************************************************************
            // and finally clear the line drawings
            // *********************************************************************************************************************
            System.Threading.Thread.Sleep(100);
            System.Windows.Forms.Application.DoEvents();
            mf.DrawingLayers.Remove(grll);
            map.MapFrame.Invalidate();
            map.Invalidate();
            System.Windows.Forms.Application.DoEvents();
            grll = null;
        }
    }
}