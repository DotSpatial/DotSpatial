using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.Contourer
{
    public class Snapin : Extension
    {
        public override void Activate()
        {
            AddMenuItems(App.HeaderControl);
            base.Activate();
        }

        private void AddMenuItems(IHeaderControl header)
        {
            SimpleActionItem contourerItem = new SimpleActionItem("Contour...", menu_Click) { Key = "kC" };
            header.Add(contourerItem);
        }

        private void menu_Click(object sender, EventArgs e)
        {
            using (FormContour frm = new FormContour())
            {
                frm.Layers = App.Map.GetRasterLayers();
                if (frm.Layers.GetLength(0) <= 0)
                {
                    MessageBox.Show("No raster layer found!");
                    return;
                }

                if (frm.ShowDialog() != DialogResult.OK) return;
                IMapFeatureLayer fl = App.Map.Layers.Add(frm.Contours);
                fl.LegendText = frm.LayerName + " - Contours";

                int numlevs = frm.Lev.GetLength(0);

                switch (frm.Contourtype)
                {
                    case (Contour.ContourType.Line):
                        {
                            LineScheme ls = new LineScheme();
                            ls.Categories.Clear();

                            for (int i = 0; i < frm.Color.GetLength(0); i++)
                            {
                                LineCategory lc = new LineCategory(frm.Color[i], 2.0)
                                {
                                    FilterExpression = "[Value] = " + frm.Lev[i],
                                    LegendText = frm.Lev[i].ToString(CultureInfo.InvariantCulture)
                                };


                                ls.AddCategory(lc);
                            }

                            fl.Symbology = ls;
                        }
                        break;

                    case (Contour.ContourType.Polygon):
                        {
                            PolygonScheme ps = new PolygonScheme();
                            ps.Categories.Clear();

                            for (int i = 0; i < frm.Color.GetLength(0); i++)
                            {
                                PolygonCategory pc = new PolygonCategory(frm.Color[i], Color.Transparent, 0)
                                {
                                    FilterExpression = "[Lev] = " + i,
                                    LegendText = frm.Lev[i] + " - " + frm.Lev[i + 1]
                                };
                                ps.AddCategory(pc);
                            }

                            fl.Symbology = ps;
                        }
                        break;
                }
            }
        }
    }
}